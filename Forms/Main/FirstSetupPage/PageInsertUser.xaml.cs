// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FirstSetupPage.PageUserViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Main.FirstSetupPage
{
  public partial class PageUserViewModel : ViewModelWithForm
  {
    private GlobalDictionaries.Mode _modeProgram;

    public ICommand GenerateTestDataCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!DevelopersHelper.IsDebug())
            return;
          this.Client.Name = "Тестовый пользователь";
          this.Client.Email = "test@gbsmarket.ru";
          this.Organization.Name = "Тестовая точка";
          this.SalePoint.Description.NamePoint = "Тестовая точка";
          this.OnPropertyChanged(isUpdateAllProp: true);
        }));
      }
    }

    public string Password1 { get; set; } = string.Empty;

    public string Password2 { get; set; } = string.Empty;

    public Gbs.Core.Entities.Users.User User { get; set; } = new Gbs.Core.Entities.Users.User();

    public Client Client { get; set; } = new Client();

    public SalePoints.SalePoint SalePoint { get; set; } = new SalePoints.SalePoint();

    public Client Organization { get; set; } = new Client();

    public bool Save()
    {
      Gbs.Core.Config.DataBase config1 = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
      config1.ModeProgram = this.ModeProgram;
      new ConfigsRepository<Gbs.Core.Config.DataBase>().Save(config1);
      if (this.Client.Name.Length < 3)
      {
        MessageBoxHelper.Warning(Translate.ИмяПользователяДолжноБытьНеМенее3Символов);
        return false;
      }
      if (!Other.IsValidateEmail(this.Client.Email))
      {
        MessageBoxHelper.Warning(Translate.АдресЭлектроннойПочтыНекорректен);
        return false;
      }
      if (this.ModeProgram == GlobalDictionaries.Mode.Home)
      {
        this.CheckEmail();
        this.SendRegisteredInfo();
        return true;
      }
      if (this.Password1.Length < 4)
      {
        MessageBoxHelper.Warning(Translate.PageUserViewModel_Слишком_короткий_пароль);
        return false;
      }
      if (this.Password1 != this.Password2)
      {
        MessageBoxHelper.Warning(Translate.PageUserViewModel_Введенные_пароли_не_совпадают);
        return false;
      }
      if (this.Organization.Name.Length < 3)
      {
        MessageBoxHelper.Warning(Translate.НазваниеОрганизацииДолжноБытьНеМенее3Символов);
        return false;
      }
      if (this.SalePoint.Description.NamePoint.Length < 3)
      {
        MessageBoxHelper.Warning(Translate.НазваниеТорговойТочкиДолжноБытьНеМенее3Символов);
        return false;
      }
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Client client = this.Client;
        if (client.Group == null)
          client.Group = new GroupRepository(dataBase).CreateDefaultClientGroup();
        Gbs.Core.Entities.Users.User user = this.User;
        if (user.Group == null)
          user.Group = UserGroups.AddDefaultUserGroup();
        this.Organization.Group = this.Client.Group;
        this.User.Client = this.Client;
        this.User.Password = this.Password1;
        this.SalePoint.Organization = this.Organization;
        ClientsRepository clientsRepository = new ClientsRepository(dataBase);
        PaymentGroups.PaymentGroup paymentGroup1 = new PaymentGroups.PaymentGroup()
        {
          Name = Translate.PageUserViewModel_Внесение,
          VisibleIn = PaymentGroups.VisiblePaymentGroup.Income
        };
        PaymentGroups.PaymentGroup paymentGroup2 = new PaymentGroups.PaymentGroup()
        {
          Name = Translate.PageUserViewModel_Выемка,
          VisibleIn = PaymentGroups.VisiblePaymentGroup.Expense
        };
        int num = new UsersRepository(dataBase).Save(this.User) & clientsRepository.Save(this.Client) & this.SalePoint.Save() & clientsRepository.Save(this.Organization) & paymentGroup1.Save() & paymentGroup2.Save() ? 1 : 0;
        if (num != 0)
        {
          Setting uid = UidDb.GetUid();
          uid.Value = (object) this.SalePoint.Description.NamePoint;
          UidDb.SetUid(uid);
          Gbs.Core.Config.Settings config2 = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
          config2.Interface.IsShowHelpTooltip = !DevelopersHelper.IsDebug();
          Sections.Section s = Sections.GetCurrentSection();
          List<PaymentMethods.PaymentMethod> actionPaymentsList = PaymentMethods.GetActionPaymentsList();
          Gbs.Core.Config.Payments payments = config2.Payments;
          PaymentMethods.PaymentMethod paymentMethod = actionPaymentsList.FirstOrDefault<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.SectionUid == s.Uid));
          // ISSUE: explicit non-virtual call
          Guid? nullable = new Guid?(paymentMethod != null ? __nonvirtual (paymentMethod.Uid) : Guid.Empty);
          payments.DefaultMethodPaymentUid = nullable;
          new ConfigsRepository<Gbs.Core.Config.Settings>().Save(config2);
          this.CheckEmail();
          this.SendRegisteredInfo();
        }
        return num != 0;
      }
    }

    [Localizable(false)]
    private void SendRegisteredInfo()
    {
      if (DevelopersHelper.IsDebug())
        return;
      TaskHelper.TaskRun((Action) (() => SmtpHelper.Send(new Gbs.Core.Entities.Emails.Email()
      {
        AddressTo = "project+33692@gbsmarket.planfix.ru",
        Subject = "Заполнена форма первичной настройки GBS.Market 6",
        Body = (object) ("ФИО: " + this.Client.Name + Other.NewLine() + "E-mail: " + this.Client.Email + Other.NewLine() + string.Format("Режим: {0}", (object) this.ModeProgram) + Other.NewLine() + (this.ModeProgram != GlobalDictionaries.Mode.Home ? "Торговая точка: " + this.SalePoint.Description.NamePoint + Other.NewLine() : "") + (this.ModeProgram != GlobalDictionaries.Mode.Home ? "Организация: " + this.Organization.Name + Other.NewLine() : "") + "GBS ID: " + GbsIdHelperMain.GetGbsId() + Other.NewLine() + string.Format("Version: {0}", (object) ApplicationInfo.GetInstance().GbsVersion))
      })));
      this.AddToHelloEmailSeries();
    }

    private bool CheckEmail() => true;

    private bool IsRussiaAndNoVendor()
    {
      return new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Russia && !(Partner.GetInfo().Uid != Guid.Empty) && Vendor.GetConfig() == null;
    }

    private void AddToHelloEmailSeries()
    {
      if (DevelopersHelper.IsDebug() || !this.IsRussiaAndNoVendor())
        return;
      TaskHelper.TaskRun((Action) (() =>
      {
        string requestUriString = string.Format("https://gbsmarket.ru/scripts/addToHelloEmailSeries.php?email={0}&name={1}&country={2}", (object) this.Client.Email, (object) this.Client.Name, (object) new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country);
        string str = string.Empty;
        using (HttpWebResponse response = (HttpWebResponse) WebRequest.Create(requestUriString).GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            using (StreamReader streamReader = new StreamReader(responseStream))
              str = streamReader.ReadToEnd();
          }
        }
        LogHelper.Trace("Add to Hello result: " + str);
      }));
    }

    private void SendConfirmEmail(string address, string code)
    {
      string str = new VendorConfig()?.ApplicationName ?? "GBS.Market";
      SmtpHelper.Send(new Gbs.Core.Entities.Emails.Email()
      {
        AddressTo = address,
        Subject = Translate.PageUserViewModel_SendConfirmEmail_Подтвердите_адрес_электронной_почты,
        Body = (object) string.Format(Translate.PageUserViewModel_SendConfirmEmail_, (object) str, (object) code)
      });
    }

    public GlobalDictionaries.Mode ModeProgram
    {
      private get => this._modeProgram;
      set
      {
        this._modeProgram = value;
        this.OnPropertyChanged("VisibilityShopData");
        this.OnPropertyChanged("VisibilityHomeData");
      }
    }

    public Visibility VisibilityShopData
    {
      get
      {
        return !this.ModeProgram.IsEither<GlobalDictionaries.Mode>(GlobalDictionaries.Mode.Cafe, GlobalDictionaries.Mode.Shop) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityHomeData
    {
      get
      {
        return !this.ModeProgram.IsEither<GlobalDictionaries.Mode>(GlobalDictionaries.Mode.Home) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Gbs.Core.Config.Settings Config { get; set; } = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();

    public ICommand SelectRemotePathCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
          {
            ShowNewFolderButton = true,
            Description = Translate.RemoteControlViewModel_Выберите_папку_для_обмена_данными
          };
          if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            return;
          this.Config.RemoteControl.Cloud.Path = folderBrowserDialog.SelectedPath;
          new ConfigsRepository<Gbs.Core.Config.Settings>().Save(this.Config);
          this.OnPropertyChanged("Config");
        }));
      }
    }
  }
}
