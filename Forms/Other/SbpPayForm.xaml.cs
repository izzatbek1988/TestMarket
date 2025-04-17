// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Other.SbpPayViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.Sbp;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.Other
{
  public partial class SbpPayViewModel : ViewModelWithForm
  {
    private readonly System.Timers.Timer _timer = new System.Timers.Timer(5000.0);
    private Decimal _sum;
    public bool NoUserClose;
    private Action _stopSbp;
    private ISbp _sbpHelper;

    private void TimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      this.IsEnableBtnCheckStatus = true;
      this.OnPropertyChanged("IsEnableBtnCheckStatus");
      this._timer.Stop();
    }

    public bool IsEnableBtnCheckStatus { get; set; } = true;

    public Visibility VisibilityBtnCheckStatus { get; set; }

    public ICommand CheckStatusPayCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.IsEnableBtnCheckStatus = false;
          this.OnPropertyChanged("IsEnableBtnCheckStatus");
          NewPayDriver.GetStatusOperationCommand.Answer statusOperation = ((NewPay) this._sbpHelper).GetStatusOperation();
          if (statusOperation.Code == 0)
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.SbpPayViewModel_CheckStatusPayCommand_, (object) statusOperation.Error)));
          else if (statusOperation.Status == 1 && statusOperation.Result)
            ((NewPay) this._sbpHelper)._wsAnswer = statusOperation.ToJsonString();
          else if (statusOperation.Status == 1 && !statusOperation.Result)
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.SbpPayViewModel_CheckStatusPayCommand_При_оплате_произошла_ошибка__повторите_попытку_еще_раз_));
            this.NoUserClose = true;
            this._stopSbp();
          }
          else
          {
            if (statusOperation.Status == 0)
              ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.SbpPayViewModel_CheckStatusPayCommand_Операция_еще_не_оплачена_покупателем__пожалуйста__ожидайте_));
            this._timer.Start();
          }
        }));
      }
    }

    public string SumForForm
    {
      get => string.Format(Translate.SbpPayViewModel_SumForForm_Сумма___0_N2_, (object) this._sum);
    }

    public BitmapImage QrImage { get; set; }

    public void DonePay(Decimal sum)
    {
      System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.SbpPayViewModel_DonePay_Оплата_по_СБП_на_сумму__0__рублей_успешно_совершена_, (object) sum)));
        this.NoUserClose = true;
        this.CloseAction();
      }));
    }

    public void ErrorPay(Decimal sum, string errorMsg)
    {
      System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        int num = (int) MessageBoxHelper.Show(string.Format(Translate.SbpPayViewModel_ErrorPay_, (object) sum, (object) errorMsg), icon: MessageBoxImage.Hand);
        this.NoUserClose = true;
        this.CloseAction();
      }));
    }

    public void ShowQr(BitmapImage qr, Action stopSbp, Decimal sum, object helper)
    {
      this.QrImage = qr;
      this._sum = sum;
      this._stopSbp = stopSbp;
      this._timer.Elapsed += new ElapsedEventHandler(this.TimerOnElapsed);
      this.VisibilityBtnCheckStatus = helper is NewPay ? Visibility.Visible : Visibility.Collapsed;
      this._sbpHelper = (ISbp) helper;
      this.FormToSHow = (WindowWithSize) new SbpPayForm();
      this.FormToSHow.Closed += new EventHandler(this.FormToSHowOnClosed);
      this.FormToSHow.Loaded += new RoutedEventHandler(this.FormToSHowOnLoaded);
      this.ShowForm();
    }

    private void FormToSHowOnLoaded(object sender, RoutedEventArgs e)
    {
      SecondMonitor config = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SecondMonitor;
      Screen screen = ((IEnumerable<Screen>) Screen.AllScreens).FirstOrDefault<Screen>((Func<Screen, bool>) (x => x.DeviceName == config.MonitorName));
      if (screen == null)
        return;
      this.FormToSHow.WindowStartupLocation = WindowStartupLocation.Manual;
      WindowWithSize formToShow1 = this.FormToSHow;
      int left = screen.Bounds.Left;
      Rectangle bounds = screen.Bounds;
      int num1 = bounds.Width / 2;
      double num2 = (double) (left + num1) - this.FormToSHow.Width / 2.0;
      formToShow1.Left = num2;
      WindowWithSize formToShow2 = this.FormToSHow;
      bounds = screen.Bounds;
      int top = bounds.Top;
      bounds = screen.Bounds;
      int num3 = bounds.Height / 2;
      double num4 = (double) (top + num3) - this.FormToSHow.Height / 2.0;
      formToShow2.Top = num4;
      this.FormToSHow.WindowStartupLocation = WindowStartupLocation.Manual;
    }

    private void FormToSHowOnClosed(object sender, EventArgs e)
    {
      if (this.NoUserClose)
        return;
      this._stopSbp();
    }
  }
}
