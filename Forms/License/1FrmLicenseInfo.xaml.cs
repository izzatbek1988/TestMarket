// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.License.LicenseInfoViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.License
{
  public class LicenseInfoViewModel : ViewModelWithForm
  {
    public ICommand CopyGbsIdCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            Clipboard.SetText(Translate.FrmLicenseInfo_GBSID + ": [" + this.GbsId + "]");
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Ошибка копировария ИД в буфер обмена", false);
            MessageBoxHelper.Error(Translate.LicenseInfoViewModel_Не_удалось_скопировать_GBS_ID_в_буфер_обмена);
          }
        }));
      }
    }

    public ICommand BuyCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => LicenseNotificationViewModel.DoBuyAction()));
      }
    }

    public ICommand ResetId
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          GbsIdHelperMain.DeleteGbsIdFromRegistry();
          int num = (int) MessageBoxHelper.Show(Translate.LicenseInfoViewModel_Перезапустите_программу_для_сброса_GBS_ID);
        }));
      }
    }

    public ICommand LoadKey
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (LicenseHelper.DownloadFromServer())
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.LicenseInfoViewModel_LoadKey_Данные_о_лицензии_успешно_загружены_с_сервера));
          this.InitData();
        }));
      }
    }

    public string GbsId { get; set; }

    public BitmapImage QrImage => GbsIdHelperMain.GetGbsIdWithPrefix().GetQrCode();

    public Visibility IsContinueVisible { get; set; }

    public Visibility DocumentLimitVisibility { get; set; }

    public LicenseHelper.LicenseInformation LicenseInformation { get; set; }

    public LicenseInfoViewModel() => this.InitData();

    public void InitData()
    {
      this.GbsId = GbsIdHelperMain.GetGbsIdWithPrefix();
      this.LicenseInformation = LicenseHelper.GetInfo();
      this.IsContinueVisible = this.LicenseInformation.KeyDateEnd.Year != 2500 ? Visibility.Visible : Visibility.Collapsed;
      this.DocumentLimitVisibility = !this.LicenseInformation.IsOverLimitDocument || this.LicenseInformation.IsActive ? Visibility.Collapsed : Visibility.Visible;
      this.OnPropertyChanged(isUpdateAllProp: true);
    }
  }
}
