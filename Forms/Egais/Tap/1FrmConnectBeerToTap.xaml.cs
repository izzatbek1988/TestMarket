// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.FrmConnectBeerToTap
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Egais
{
  public class FrmConnectBeerToTap : WindowWithSize, IComponentConnector
  {
    internal DecimalUpDown QuantityDecimalUpDown;
    internal DecimalUpDown SalePriceDecimalUpDown;
    private bool _contentLoaded;

    public FrmConnectBeerToTap()
    {
      this.InitializeComponent();
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      ConnectBeerToTapViewModel dataContext = (ConnectBeerToTapViewModel) this.DataContext;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.CancelAction,
          (ICommand) new RelayCommand((Action<object>) (obj => this.Close()))
        },
        {
          hotKeys.OkAction,
          dataContext.SaveCommand
        }
      };
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
    }

    private void FrmConnectBeerToTap_OnActivated(object sender, EventArgs e)
    {
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(((ConnectBeerToTapViewModel) this.DataContext).ComPortScannerOnBarcodeChanged));
    }

    private bool CloseCard()
    {
      return ((ConnectBeerToTapViewModel) this.DataContext).HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/egais/tap/frmconnectbeertotap.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.SalePriceDecimalUpDown = (DecimalUpDown) target;
        else
          this._contentLoaded = true;
      }
      else
        this.QuantityDecimalUpDown = (DecimalUpDown) target;
    }
  }
}
