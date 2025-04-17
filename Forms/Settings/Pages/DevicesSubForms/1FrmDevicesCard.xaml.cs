// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubForms.FrmDevicesCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices;
using Gbs.Forms.Settings.Pages.DevicesSubPages;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubForms
{
  public class FrmDevicesCard : WindowWithSize, IComponentConnector
  {
    public object DevicesCnf;
    internal Grid Grid;
    internal System.Windows.Controls.Frame FrameDevice;
    private bool _contentLoaded;

    public FrmDevicesCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
    }

    private bool CloseCard()
    {
      return ((DevicesCardViewModel) this.DataContext).HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    public void SetContentEditPage(DevicesCardViewModel dc)
    {
      ItemDevice itemDevice = dc.ItemDevice;
      if (itemDevice == null)
        return;
      Gbs.Core.Config.Settings settings = dc.Settings;
      Page userPage = dc.UserPage;
      Gbs.Core.Config.Devices devicesConfig = new Gbs.Core.Config.Devices();
      this.FrameDevice.Content = (object) null;
      switch (itemDevice.Type)
      {
        case IDevice.DeviceTypes.Other:
          break;
        case IDevice.DeviceTypes.Kkm:
          this.DevicesCnf = ConvertToObject<Gbs.Core.Config.CheckPrinter>(itemDevice.Device) ?? (object) new Gbs.Core.Config.CheckPrinter();
          devicesConfig.CheckPrinter = (Gbs.Core.Config.CheckPrinter) this.DevicesCnf;
          this.FrameDevice.Content = (object) new PageCheckPrinter(devicesConfig, settings, userPage);
          break;
        case IDevice.DeviceTypes.BarcodeScanner:
          this.DevicesCnf = ConvertToObject<BarcodeScanner>(itemDevice.Device) ?? (object) new BarcodeScanner();
          devicesConfig.BarcodeScanner = (BarcodeScanner) this.DevicesCnf;
          this.FrameDevice.Content = (object) new PageBarcodeScanner(devicesConfig, settings);
          break;
        case IDevice.DeviceTypes.Scale:
          break;
        case IDevice.DeviceTypes.AcquiringTerminal:
          break;
        case IDevice.DeviceTypes.DisplayBuyer:
          break;
        case IDevice.DeviceTypes.ExtraPrinters:
          break;
        case IDevice.DeviceTypes.SecondMonitor:
          break;
        case IDevice.DeviceTypes.LablePrinter:
          break;
        case IDevice.DeviceTypes.ScaleWithLable:
          break;
        case IDevice.DeviceTypes.Keyboard:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      static object ConvertToObject<T>(object json)
      {
        try
        {
          return (object) JsonConvert.DeserializeObject<T>(json.ToString());
        }
        catch
        {
          return json;
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/devicessubforms/frmdevicescard.xaml", UriKind.Relative));
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
          this.FrameDevice = (System.Windows.Controls.Frame) target;
        else
          this._contentLoaded = true;
      }
      else
        this.Grid = (Grid) target;
    }
  }
}
