// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.License.LicenseNotificationViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Helpers;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.License
{
  public class LicenseNotificationViewModel : ViewModelWithForm
  {
    public string AppVersion
    {
      get => ApplicationInfo.GetInstance().AppVersion;
      set => throw new NotImplementedException();
    }

    public void Show()
    {
      try
      {
        if (NetworkHelper.IsWorkInternet())
        {
          if (LicenseHelper.DownloadFromServer())
          {
            LicenseHelper.LicenseInformation info = LicenseHelper.GetInfo();
            if (info.IsActive)
            {
              if ((info.KeyDateEnd - DateTime.Now).TotalDays > 14.0)
                return;
            }
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
      if (LicenseNotificationViewModel.LastNotificationDateTime.HasValue)
      {
        DateTime now = DateTime.Now;
        DateTime? notificationDateTime = LicenseNotificationViewModel.LastNotificationDateTime;
        double totalMinutes = (notificationDateTime.HasValue ? new TimeSpan?(now - notificationDateTime.GetValueOrDefault()) : new TimeSpan?()).Value.TotalMinutes;
      }
      if (!this.LicenseInfo.IsActive)
      {
        this.HeaderText = Translate.СрокДействияЛицензии;
        this.DaysText = Translate.ИСТЕК;
        this.Timer = new Timer(1000.0);
        this.ContinueButtonEnable = false;
        this.SecondsToContinue = 10 + 3 * (int) (DateTime.Today - this.LicenseInfo.KeyDateEnd).TotalDays;
        if (this.SecondsToContinue < 0)
        {
          int num;
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            num = dataBase.GetTable<DOCUMENTS>().Count<DOCUMENTS>();
          if (num > this.LicenseInfo.MaxDocumentsForDemo)
            this.SecondsToContinue = 10 + (num - this.LicenseInfo.MaxDocumentsForDemo) / 100;
        }
        if (this.SecondsToContinue < 0)
          this.SecondsToContinue = 30;
        if (this.SecondsToContinue > 120)
          this.SecondsToContinue = 120;
        this.ContinueButtonText = Translate.FrmReturnSales_ПРОДОЛЖИТЬ + " (" + this.SecondsToContinue.ToString() + ")";
        this.Timer.Elapsed += new ElapsedEventHandler(this.Timer_Elapsed);
        this.Timer.Start();
      }
      else
      {
        this.HeaderText = Translate.СрокДействияЛицензииИстекает;
        this.DaysText = string.Format(Translate.LicenseNotificationViewModel_ЧЕРЕЗ__0__ДНЕЙ, (object) (this.LicenseInfo.KeyDateEnd - DateTime.Today).TotalDays);
        this.ContinueButtonEnable = true;
        this.ContinueButtonText = Translate.FrmReturnSales_ПРОДОЛЖИТЬ;
      }
      this.OnPropertyChanged("ContinueButtonText");
      this.FormToSHow = (WindowWithSize) new FrmLicenseNotification();
      this.ShowForm();
    }

    public static DateTime? LastNotificationDateTime { get; set; }

    public ICommand BuyCommand { get; set; }

    public ICommand ContinueCommand { get; set; }

    public ICommand CopyIdCommand { get; set; }

    public string ContinueButtonText { get; set; }

    public bool ContinueButtonEnable { get; set; }

    public string HeaderText { get; set; }

    public string DaysText { get; set; }

    public string GbsId { get; set; }

    public BitmapImage QrImage => GbsIdHelperMain.GetGbsIdWithPrefix().GetQrCode();

    private LicenseHelper.LicenseInformation LicenseInfo { get; }

    public Visibility DocumentLimitVisibility { get; set; }

    private Timer Timer { get; set; }

    private int SecondsToContinue { get; set; }

    public LicenseNotificationViewModel()
    {
      this.LicenseInfo = LicenseHelper.GetInfo();
      this.GbsId = GbsIdHelperMain.GetGbsIdWithPrefix();
      this.DocumentLimitVisibility = !this.LicenseInfo.IsOverLimitDocument || this.LicenseInfo.IsActive ? Visibility.Collapsed : Visibility.Visible;
      this.BuyCommand = (ICommand) new RelayCommand((Action<object>) (obj => LicenseNotificationViewModel.DoBuyAction()));
      this.ContinueCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
      this.CopyIdCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        try
        {
          Clipboard.SetText(Translate.FrmLicenseInfo_GBSID + ": [" + this.GbsId + "]");
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
          string gbsIdВБуферОбмена = Translate.LicenseNotificationViewModel_Ошибка_копирования_GBS_ID_в_буфер_обмена;
          LogHelper.ShowErrorMgs(ex, gbsIdВБуферОбмена, LogHelper.MsgTypes.Notification);
        }
      }));
    }

    public static void DoBuyAction()
    {
      VendorConfig config = Vendor.GetConfig();
      if (config == null)
      {
        PartnersHelper.OpenPage(PartnersHelper.PageTypes.Buy);
      }
      else
      {
        List<VendorConfig.LinkItem> links = config.Links;
        VendorConfig.LinkItem linkItem = links != null ? links.FirstOrDefault<VendorConfig.LinkItem>((Func<VendorConfig.LinkItem, bool>) (x => x.Type == VendorConfig.LinkItem.Types.Buy)) : (VendorConfig.LinkItem) null;
        if (linkItem != null)
        {
          NetworkHelper.OpenUrl(linkItem.Value);
        }
        else
        {
          List<VendorConfig.ContactItem> contacts = config.Contacts;
          if (contacts != null && contacts.Any<VendorConfig.ContactItem>())
          {
            string str = string.Empty;
            foreach (VendorConfig.ContactItem contactItem in contacts)
              str = str + contactItem.Name + ": " + contactItem.Value + "\n";
            int num = (int) MessageBoxHelper.Show(string.Format(Translate.LicenseNotificationViewModel_DoBuyAction_, (object) str));
          }
          else
            PartnersHelper.OpenPage(PartnersHelper.PageTypes.Buy);
        }
      }
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      --this.SecondsToContinue;
      this.ContinueButtonText = Translate.FrmReturnSales_ПРОДОЛЖИТЬ + " (" + this.SecondsToContinue.ToString() + ")";
      if (this.SecondsToContinue == 0)
      {
        this.ContinueButtonEnable = true;
        this.ContinueButtonText = Translate.FrmReturnSales_ПРОДОЛЖИТЬ;
        LicenseNotificationViewModel.LastNotificationDateTime = new DateTime?(DateTime.Now);
        this.Timer.Stop();
      }
      this.OnPropertyChanged("ContinueButtonEnable");
      this.OnPropertyChanged("ContinueButtonText");
      this.OnPropertyChanged("SecondsToContinue");
    }
  }
}
