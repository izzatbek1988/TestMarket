// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.CloseViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Main
{
  public partial class CloseViewModel : ViewModelWithForm
  {
    private bool _cancelEnabled = true;
    private bool _isEnabled = true;
    private int _timeToClose = 3;

    public string ContentButtonClose { get; set; } = Translate.FrmInsertPaymentMethods_ОТМЕНА;

    public bool IsEnabled
    {
      get => this._isEnabled;
      set
      {
        this._isEnabled = value;
        this.OnPropertyChanged(nameof (IsEnabled));
      }
    }

    public bool CancelEnabled
    {
      get => this._cancelEnabled;
      set
      {
        this._cancelEnabled = value;
        this.OnPropertyChanged(nameof (CancelEnabled));
      }
    }

    public Visibility VisibilityZPrint { get; set; } = Visibility.Collapsed;

    public ICommand CloseProgram { get; set; }

    public ICommand CloseAndPrintZ { get; set; }

    public ICommand RestartProgram { get; set; }

    public ICommand CancelOff { get; set; }

    public bool OffWindows { get; set; }

    public Timer TimerOff { get; } = new Timer(1000.0);

    private CloseViewModel.CloseActionVariants CloseActionVariant { get; set; } = CloseViewModel.CloseActionVariants.Close;

    public CloseViewModel()
    {
      try
      {
        Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        if (devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
          this.VisibilityZPrint = Visibility.Visible;
        this.TimerOff.Elapsed += new ElapsedEventHandler(this.TimerOff_Elapsed);
        this.TimerOff.AutoReset = true;
        this.CloseProgram = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.IsEnabled = false;
          this.CloseActionVariant = CloseViewModel.CloseActionVariants.Close;
          this._timeToClose = this.OffWindows ? 5 : 3;
          this.TimerOff.Start();
        }));
        this.CloseAndPrintZ = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.IsEnabled = false;
          (bool Result, Gbs.Core.Entities.Users.User User) userForDocument = Other.GetUserForDocument(Gbs.Core.Entities.Actions.PrintKkmReport);
          int num = userForDocument.Result ? 1 : 0;
          Gbs.Core.Entities.Users.User user = userForDocument.User;
          if (num == 0)
            return;
          using (KkmHelper kkmHelper = new KkmHelper(devicesConfig))
          {
            if (!kkmHelper.GetReport(ReportTypes.ZReport, new Cashier()
            {
              Name = user.Client.Name,
              Inn = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.EntityUid == user.Client.Uid && x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? ""
            }))
            {
              this.IsEnabled = true;
              return;
            }
          }
          this.TimerOff.Start();
        }));
        this.RestartProgram = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.IsEnabled = false;
          this.CloseActionVariant = CloseViewModel.CloseActionVariants.Restart;
          this._timeToClose = 3;
          this.TimerOff.Start();
        }));
        this.CancelOff = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.TimerOff.Stop();
          this.CloseAction();
        }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме завершения работы");
      }
    }

    private void TimerOff_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.ContentButtonClose = string.Format("{0} ({1})", (object) Translate.FrmInsertPaymentMethods_ОТМЕНА, (object) this._timeToClose);
      this.OnPropertyChanged("ContentButtonClose");
      if (this._timeToClose == 0)
      {
        this.CancelEnabled = false;
        this.TimerOff.Stop();
        switch (this.CloseActionVariant)
        {
          case CloseViewModel.CloseActionVariants.Restart:
            if (Other.RestartApplication())
              break;
            this.CancelEnabled = true;
            this.ContentButtonClose = Translate.FrmInsertPaymentMethods_ОТМЕНА;
            this.OnPropertyChanged("ContentButtonClose");
            this.IsEnabled = true;
            this.OnPropertyChanged("IsEnabled");
            break;
          case CloseViewModel.CloseActionVariants.Close:
            Other.CloseApplication(this.OffWindows);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      else
        --this._timeToClose;
    }

    private enum CloseActionVariants
    {
      Restart,
      Close,
    }
  }
}
