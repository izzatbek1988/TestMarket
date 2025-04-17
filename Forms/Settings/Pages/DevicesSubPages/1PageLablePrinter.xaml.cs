// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.LablePrinterPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public class LablePrinterPageViewModel : ViewModelWithForm
  {
    public Gbs.Core.Config.Devices DeviceConfig { get; set; } = new Gbs.Core.Config.Devices();

    public GlobalDictionaries.Devices.TypePrintLable TypePrint
    {
      get => this.DeviceConfig.LablePrinter.TypePrint;
      set
      {
        this.DeviceConfig.LablePrinter.TypePrint = value;
        this.OnPropertyChanged(nameof (TypePrint));
        this.OnPropertyChanged("DeviceConfig");
        this.OnPropertyChanged("ZplSettingsVisibility");
      }
    }

    public Dictionary<GlobalDictionaries.Devices.TypePrintLable, string> DictionaryTypePrint { get; set; } = GlobalDictionaries.Devices.TypePrintLableDictionary;

    public Visibility ZplSettingsVisibility
    {
      get
      {
        return this.TypePrint != GlobalDictionaries.Devices.TypePrintLable.Zpl ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public IEnumerable PrinterList { get; set; } = (IEnumerable) new List<string>();

    public LablePrinterPageViewModel()
    {
    }

    public LablePrinterPageViewModel(Gbs.Core.Config.Devices devicesConfig)
    {
      this.DeviceConfig = devicesConfig;
      try
      {
        this.PrinterList = (IEnumerable) PrinterSettings.InstalledPrinters;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения списка принтеров для печати этикеток");
      }
    }
  }
}
