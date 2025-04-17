// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Other.SendDataForSupportViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Other
{
  public partial class SendDataForSupportViewModel : ViewModelWithForm
  {
    public string Email { get; set; }

    public string Question { get; set; } = string.Empty;

    public bool IsSendLogs { get; set; } = true;

    public bool IsSendDb { get; set; }

    public void Show()
    {
      if (!NetworkHelper.IsWorkInternet())
      {
        MessageBoxHelper.Warning(Translate.SendDataForSupportViewModel_Show_Отсутствует_соединение_с_интернетом__проверьте_подключение_к_сети_);
      }
      else
      {
        this.Email = new ConfigsRepository<Settings>().Get().Other.EmailForSendDataSupport;
        this.FormToSHow = (WindowWithSize) new FrmSendDataForSupport();
        this.ShowForm();
      }
    }

    public void Send()
    {
      if (!NetworkHelper.IsWorkInternet())
        MessageBoxHelper.Warning(Translate.SendDataForSupportViewModel_Show_Отсутствует_соединение_с_интернетом__проверьте_подключение_к_сети_);
      else if (this.Email.IsNullOrEmpty())
        MessageBoxHelper.Warning(Translate.SendDataForSupportViewModel_Send_Для_отправки_обращения_необходимо_указать_адрес_электронной_почты);
      else if (!Gbs.Helpers.Other.IsValidateEmail(this.Email))
        MessageBoxHelper.Warning(Translate.АдресЭлектроннойПочтыНекорректен);
      else if (this.Question.IsNullOrEmpty())
        MessageBoxHelper.Warning(Translate.SendDataForSupportViewModel_Send_Для_отправки_обращения_необходимо_описать_возникшую_проблему_);
      else if (!this.IsSendLogs && !this.IsSendDb)
      {
        MessageBoxHelper.Warning(Translate.SendDataForSupportViewModel_Send_Для_отправки_обращения_необходимо_приложить_к_заявке_журнал_работы_программы_или_резервную_копию_);
      }
      else
      {
        Task.Run((Action) (() =>
        {
          Gbs.Core.Entities.Emails.Email email1 = new Gbs.Core.Entities.Emails.Email()
          {
            AddressTo = this.Email,
            Body = (object) this.Question
          };
          string str1 = FileSystemHelper.TempFolderPath();
          if (this.IsSendDb)
          {
            string str2 = Path.Combine(str1, "backup.zip");
            if (BackupHelper.CreateBackup(str1, "backup.zip"))
            {
              email1.Attach = new List<string>() { str2 };
            }
            else
            {
              Gbs.Core.Entities.Emails.Email email2 = email1;
              email2.Body = (object) (email2.Body?.ToString() + "<br/><br/>" + Translate.SendDataForSupportViewModel_Send_ВНИМАНИЕ__Не_удалось_сформировать_резервную_копию_);
            }
          }
          else
          {
            string str3 = Path.Combine(str1, "logs.zip");
            FileSystemHelper.CreateZip(str3, ApplicationInfo.GetInstance().Paths.LogsPath);
            if (System.IO.File.Exists(str3))
            {
              email1.Attach = new List<string>() { str3 };
            }
            else
            {
              Gbs.Core.Entities.Emails.Email email3 = email1;
              email3.Body = (object) (email3.Body?.ToString() + "<br/><br/>" + Translate.SendDataForSupportViewModel_Send_ВНИМАНИЕ__Не_удалось_сформировать_архив_с_логами_программы_);
            }
          }
          Gbs.Core.Entities.Emails.Email email4 = email1;
          email4.Body = (object) (email4.Body?.ToString() + "<br/><br/><b>" + Translate.SendDataForSupportViewModel_Send_Электронная_почта + ":</b> " + this.Email);
          Gbs.Core.Entities.Emails.Email email5 = email1;
          email5.Body = (object) (email5.Body?.ToString() + "<br/><b>" + Translate.PageDataBase_РежимРаботыПрограммы + ":</b> " + new ConfigsRepository<DataBase>().Get().ModeProgram.ToString());
          Gbs.Core.Entities.Emails.Email email6 = email1;
          email6.Body = (object) (email6.Body?.ToString() + "<br/><b>GBS.ID:</b> 6:" + LicenseHelper.GetInfo().GbsId);
          Gbs.Core.Entities.Emails.Email email7 = email1;
          email7.Body = (object) (email7.Body?.ToString() + "<br/><b>" + Translate.SendDataForSupportViewModel_Send_Версия_программы + ":</b> " + ApplicationInfo.GetInstance().GbsVersion?.ToString());
          int? nullable;
          try
          {
            nullable = this.CreateTask(email1);
          }
          catch
          {
            nullable = new int?();
          }
          if (nullable.HasValue)
            ProgressBarHelper.AddNotification(string.Format(Translate.SendDataForSupportViewModel_Send_, (object) nullable));
          else
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.SendDataForSupportViewModel_Send_Не_удалось_отправить_обращение__Обратитесь_в_службу_поддержки_самостоятельно_)
            {
              Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
            });
          try
          {
            Directory.Delete(str1, true);
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Не удалось удалить временную папку после создания обращения");
          }
        }));
        int num = (int) MessageBoxHelper.Show(Translate.SendDataForSupportViewModel_Send_НачатаПодготовкаДанных);
        Settings config = new ConfigsRepository<Settings>().Get();
        config.Other.EmailForSendDataSupport = this.Email;
        new ConfigsRepository<Settings>().Save(config);
        this.CloseAction();
      }
    }

    public ICommand SendCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Send()));
    }

    private int? CreateTask(Gbs.Core.Entities.Emails.Email email)
    {
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      string requestUri = "https://gbsmarket.planfix.ru/webhook/file/cbmk-pgos-djry-tjfw";
      using (HttpClient httpClient = new HttpClient())
      {
        httpClient.Timeout = new TimeSpan(0, 0, 180);
        using (MultipartFormDataContent content1 = new MultipartFormDataContent())
        {
          StringContent content2 = new StringContent(email.Body.ToString(), Encoding.UTF8, "text/plain");
          content1.Add((HttpContent) content2, "message");
          StringContent content3 = new StringContent(email.AddressTo, Encoding.UTF8, "text/plain");
          content1.Add((HttpContent) content3, nameof (email));
          if (email.Attach.Any<string>())
          {
            string path = email.Attach.First<string>();
            StreamContent content4 = new StreamContent((Stream) new FileStream(path, FileMode.Open, FileAccess.Read));
            string fileName = Path.GetFileName(path);
            content4.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
              Name = "\"file\"",
              FileName = fileName
            };
            content1.Add((HttpContent) content4, "file", fileName);
          }
          HttpResponseMessage result1 = httpClient.PostAsync(requestUri, (HttpContent) content1).Result;
          if (result1.IsSuccessStatusCode)
          {
            int num = (int) ((JObject) JsonConvert.DeserializeObject(result1.Content.ReadAsStringAsync().Result))["task"];
            LogHelper.Debug("Обращение в поддержку успешно отправлено, номер задачи - " + num.ToString());
            return new int?(num);
          }
          string result2 = result1.Content.ReadAsStringAsync().Result;
          LogHelper.Debug(string.Format("Ошибка отправки обращения в поддержку. Error: {0}", (object) result1.StatusCode));
          LogHelper.Debug(result2);
          return new int?();
        }
      }
    }
  }
}
