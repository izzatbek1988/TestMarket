// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Pages.DevicesSubPages;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class DevicesViewModel : ViewModelWithForm
  {
    public readonly Gbs.Core.Config.Devices DevicesConfig;
    public PageBasic PageBasic;

    public Visibility TsdVisibility { get; set; }

    public PageCheckPrinter PageCheckPrinter { get; set; }

    public PageBarcodeScanner PageBarcodeScanner { get; set; }

    public Page PageAcquiringTerminal { get; set; }

    public Page PageScale { get; set; }

    public Page PageTsd { get; set; }

    public Page PageLablePrinter { get; set; }

    public Page PageSecondMonitor { get; set; }

    public Page PageKeyboard { get; set; }

    public Page PageExtraPrinters { get; set; }

    public DevicesViewModel()
    {
    }

    public DevicesViewModel(Gbs.Core.Config.Settings settings, Page userPage, PageBasic pageBasic)
    {
      this.TsdVisibility = settings.Interface.Country == GlobalDictionaries.Countries.Russia ? Visibility.Visible : Visibility.Collapsed;
      Other.ConsoleWrite("Конструктор модели страницы оборудования");
      this.DevicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.PageBasic = pageBasic;
      this.PageCheckPrinter = new PageCheckPrinter(this.DevicesConfig, settings, userPage);
      this.PageBarcodeScanner = new PageBarcodeScanner(this.DevicesConfig, settings);
      this.PageAcquiringTerminal = (Page) new Gbs.Forms.Settings.Pages.DevicesSubPages.PageAcquiringTerminal(this.DevicesConfig, settings);
      this.PageScale = (Page) new Gbs.Forms.Settings.Pages.DevicesSubPages.PageScale(this.DevicesConfig);
      this.PageTsd = (Page) new Gbs.Forms.Settings.Pages.DevicesSubPages.PageTsd(this.DevicesConfig);
      this.PageLablePrinter = (Page) new Gbs.Forms.Settings.Pages.DevicesSubPages.PageLablePrinter(this.DevicesConfig);
      this.PageSecondMonitor = (Page) new Gbs.Forms.Settings.Pages.DevicesSubPages.PageSecondMonitor(this.DevicesConfig);
      this.PageKeyboard = (Page) new Gbs.Forms.Settings.Pages.PageKeyboard(this.DevicesConfig);
      this.PageExtraPrinters = (Page) new Gbs.Forms.Settings.Pages.DevicesSubPages.PageExtraPrinters(this.DevicesConfig);
    }

    public bool Save()
    {
      ConfigsRepository<Gbs.Core.Config.Devices> configsRepository = new ConfigsRepository<Gbs.Core.Config.Devices>();
      ((Gbs.Forms.Settings.Pages.DevicesSubPages.PageExtraPrinters) this.PageExtraPrinters).Save();
      if (this.DevicesConfig.AcquiringTerminal.Type == GlobalDictionaries.Devices.AcquiringTerminalTypes.None && ((BasicPageViewModel) this.PageBasic.DataContext).Mode != GlobalDictionaries.Mode.Home && PaymentMethods.GetActionPaymentsList().Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => !x.IsDeleted && x.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring)) && !((BasicPageViewModel) this.PageBasic.DataContext).IsCloseForHome)
      {
        int num = (int) MessageBoxHelper.Show(Translate.DevicesViewModel_Save_, icon: MessageBoxImage.Exclamation);
      }
      return this.PageCheckPrinter.ValidationConfig() && this.PageBarcodeScanner.ValidationConfig() && configsRepository.Save(this.DevicesConfig);
    }
  }
}
