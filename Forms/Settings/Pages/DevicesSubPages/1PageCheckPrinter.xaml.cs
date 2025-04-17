// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.PageCheckPrinterViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public class PageCheckPrinterViewModel : ViewModelWithForm
  {
    private Gbs.Core.Config.Devices _devicesConfig;
    private Gbs.Core.Config.Settings _settings;
    private IEnumerable _printersList;

    public Page UserPage { get; set; }

    private void UpdateUserPage()
    {
      if (this.UserPage == null)
        return;
      ((UsersPageViewModel) this.UserPage.DataContext).UpdateEnableRequestAuthorizationOnSale(this._devicesConfig);
    }

    public Visibility VisibilityDefaultRuTaxSystem
    {
      get
      {
        return this.Settings.Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityFfd
    {
      get
      {
        return this.Settings.Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityNoFiscalReport
    {
      get
      {
        return !this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.WebKassa, GlobalDictionaries.Devices.FiscalKkmTypes.ReKassa, GlobalDictionaries.Devices.FiscalKkmTypes.HiPos, GlobalDictionaries.Devices.FiscalKkmTypes.Hdm, GlobalDictionaries.Devices.FiscalKkmTypes.UzPos) || this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityButtonShowDriver
    {
      get
      {
        return !this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.VikiPrint, GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas) || this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand ShowFolderDriver
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          switch (this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType)
          {
            case GlobalDictionaries.Devices.FiscalKkmTypes.None:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.Atol8:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.Atol10:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.Shtrih:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.VikiPrint:
              FileSystemHelper.ShowFolderDriver(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\kkm\\vikiprint"));
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.Mercury:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.HelpMicro:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.ExellioFP:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.MiniFP54:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.UzFiscalModule:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.PortFPGKZ:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.FsPRRO:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas:
              FileSystemHelper.ShowFolderDriver(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\kkm\\leocas"));
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart:
              break;
            case GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests:
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }));
      }
    }

    public Visibility VisibilityUktZed
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas, GlobalDictionaries.Devices.FiscalKkmTypes.HelpMicro, GlobalDictionaries.Devices.FiscalKkmTypes.HiPos) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityIkpu
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.UzPos) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityFreeKkmPort
    {
      get
      {
        return this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Mercury, GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer, GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests, GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer, GlobalDictionaries.Devices.FiscalKkmTypes.HelpMicro, GlobalDictionaries.Devices.FiscalKkmTypes.UzFiscalModule, GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart, GlobalDictionaries.Devices.FiscalKkmTypes.FsPRRO, GlobalDictionaries.Devices.FiscalKkmTypes.HiPos, GlobalDictionaries.Devices.FiscalKkmTypes.SmartOne, GlobalDictionaries.Devices.FiscalKkmTypes.VikiDriver, GlobalDictionaries.Devices.FiscalKkmTypes.Hdm) || this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityMercuryModel
    {
      get
      {
        return this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType != GlobalDictionaries.Devices.FiscalKkmTypes.Mercury || this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityAzSmart
    {
      get
      {
        return this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType != GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart || this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityModelKkm
    {
      get
      {
        return !this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.ReKassa, GlobalDictionaries.Devices.FiscalKkmTypes.WebKassa, GlobalDictionaries.Devices.FiscalKkmTypes.SmartOne, GlobalDictionaries.Devices.FiscalKkmTypes.VikiDriver, GlobalDictionaries.Devices.FiscalKkmTypes.Hdm) || this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public List<EntityProperties.PropertyType> ListUkt { get; set; }

    public Dictionary<string, string> MercuryModel
    {
      get
      {
        return new Dictionary<string, string>()
        {
          {
            "119F",
            Translate.PageCheckPrinterViewModel__119Ф
          },
          {
            "185F",
            Translate.PageCheckPrinterViewModel_Другие__115Ф__130Ф__180Ф__185Ф_
          }
        };
      }
    }

    public bool PrintCheckOnEverySale
    {
      get => this._devicesConfig.CheckPrinter.PrintCheckOnEverySale;
      set
      {
        this._devicesConfig.CheckPrinter.PrintCheckOnEverySale = value;
        this.OnPropertyChanged("VisibilityAllowSalesWithoutCheck");
      }
    }

    public bool IsShowPrintConfirmationForm
    {
      get => this._devicesConfig.CheckPrinter.IsShowPrintConfirmationForm;
      set
      {
        this._devicesConfig.CheckPrinter.IsShowPrintConfirmationForm = value;
        this.OnPropertyChanged("VisibilityPrintCheckOnEverySale");
        this.OnPropertyChanged("VisibilityAllowSalesWithoutCheck");
      }
    }

    public bool IsPrintNoFiscalOtherPrinter
    {
      get => this._devicesConfig.CheckPrinter.IsPrintNoFiscalOtherPrinter;
      set
      {
        this._devicesConfig.CheckPrinter.IsPrintNoFiscalOtherPrinter = value;
        this.OnPropertyChanged("PrinterByNameSelectorVisibility");
      }
    }

    public bool IsNoSendDigitalCheck
    {
      get => this._devicesConfig.CheckPrinter.FiscalKkm.IsNoSendDigitalCheck;
      set
      {
        this._devicesConfig.CheckPrinter.FiscalKkm.IsNoSendDigitalCheck = value;
        this.OnPropertyChanged("VisibilityNoPrintCheckIfSendDigitalCheck");
        this.OnPropertyChanged("VisibilityOptionDigitalCheck");
      }
    }

    public bool IsAlwaysNoPrintCheck
    {
      get => this._devicesConfig.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck;
      set
      {
        this._devicesConfig.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck = value;
        this.OnPropertyChanged("VisibilityNoPrintCheckIfSendDigitalCheck");
        this.OnPropertyChanged("VisibilityOptionDigitalCheck");
      }
    }

    public GlobalDictionaries.Devices.FfdVersions FfdVersion
    {
      get => this._devicesConfig.CheckPrinter.FiscalKkm.FfdVersion;
      set
      {
        this._devicesConfig.CheckPrinter.FiscalKkm.FfdVersion = value;
        this.OnPropertyChanged("VisibilityOptionOnlineKkm");
        this.OnPropertyChanged("VisibilitySendOnlineCheck");
        this.OnPropertyChanged("VisibilityOptionCheckMarkInfo");
      }
    }

    public Visibility VisibilityPrintCheckOnEverySale
    {
      get
      {
        return !this.DevicesConfig.CheckPrinter.IsShowPrintConfirmationForm ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityPrintCommentByGood
    {
      get
      {
        if (this.DevicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
        {
          if (this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Atol10, GlobalDictionaries.Devices.FiscalKkmTypes.Atol8, GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer, GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests, GlobalDictionaries.Devices.FiscalKkmTypes.Shtrih, GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer))
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }

    public Visibility VisibilityAllowSalesWithoutCheck
    {
      get
      {
        return this.DevicesConfig.CheckPrinter.IsShowPrintConfirmationForm && this.DevicesConfig.CheckPrinter.Type.IsEither<GlobalDictionaries.Devices.CheckPrinterTypes>(new GlobalDictionaries.Devices.CheckPrinterTypes[1]) && this.VisibilityPrintCheckOnEverySale == Visibility.Collapsed || (!this.DevicesConfig.CheckPrinter.PrintCheckOnEverySale || this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm) && this.VisibilityPrintCheckOnEverySale != Visibility.Collapsed ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Gbs.Core.Config.Devices DevicesConfig
    {
      get => this._devicesConfig;
      set
      {
        this._devicesConfig = value;
        this.OnPropertyChanged(nameof (DevicesConfig));
      }
    }

    public Gbs.Core.Config.Settings Settings
    {
      get => this._settings;
      set
      {
        this._settings = value;
        this.OnPropertyChanged(nameof (Settings));
      }
    }

    public Dictionary<GlobalDictionaries.Devices.FfdVersions, string> FfdVersionsDictionary { get; set; }

    public Dictionary<GlobalDictionaries.Devices.CheckPrinterTypes, string> CheckPrinterDictionary { get; set; }

    public Dictionary<GlobalDictionaries.RuTaxSystems, string> RuTaxSystemsDictionary { get; set; }

    public Dictionary<int, Gbs.Core.Config.FiscalKkm.TaxRate> TaxRatesDictionary { get; set; }

    public IEnumerable PrintersList
    {
      get => this._printersList != null ? this._printersList : (IEnumerable) new List<string>();
    }

    public Dictionary<GlobalDictionaries.Devices.FiscalKkmTypes, string> FiscalKkmTypesDictionary
    {
      get
      {
        return GlobalDictionaries.Devices.FiscalKkmTypesDictionary().Where<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>, bool>) (x => x.Country.Any<GlobalDictionaries.Countries>((Func<GlobalDictionaries.Countries, bool>) (c => c.IsEither<GlobalDictionaries.Countries>(this.Settings.Interface.Country, GlobalDictionaries.Countries.NotSet))))).ToDictionary<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>, GlobalDictionaries.Devices.FiscalKkmTypes, string>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>, GlobalDictionaries.Devices.FiscalKkmTypes>) (x => x.Type), (Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>, string>) (x => x.TypeName));
      }
    }

    public List<FileInfo> CheckTemplatesList { get; set; }

    public List<FileInfo> CheckNoFiscalTemplatesList { get; set; }

    public Visibility KkmTypeSelectorVisibility
    {
      get
      {
        return this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility PrinterByNameSelectorVisibility
    {
      get
      {
        return this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter && (!this.IsPrintNoFiscalOtherPrinter || this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || this.FiscalKkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility ConnectionConfigGroupVisibility
    {
      get
      {
        return this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.EscPos && (this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || this.FiscalKkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility OnlineKkmSettingsVisibility
    {
      get
      {
        if (this.CheckPrinterType == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
        {
          if (this.FiscalKkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Atol10, GlobalDictionaries.Devices.FiscalKkmTypes.Atol8, GlobalDictionaries.Devices.FiscalKkmTypes.Shtrih, GlobalDictionaries.Devices.FiscalKkmTypes.VikiPrint, GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer, GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer, GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests, GlobalDictionaries.Devices.FiscalKkmTypes.Mercury, GlobalDictionaries.Devices.FiscalKkmTypes.Neva, GlobalDictionaries.Devices.FiscalKkmTypes.VikiDriver))
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }

    public Visibility KkmSettingsVisibility
    {
      get
      {
        return this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || this.FiscalKkmType == GlobalDictionaries.Devices.FiscalKkmTypes.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility PrintSettingsVisibility
    {
      get
      {
        return this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.None && (this.FiscalKkmType != GlobalDictionaries.Devices.FiscalKkmTypes.None || this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm) ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility KkmAndEscPosSettingsVisibility
    {
      get
      {
        return !this.CheckPrinterType.IsEither<GlobalDictionaries.Devices.CheckPrinterTypes>(GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm, GlobalDictionaries.Devices.CheckPrinterTypes.EscPos) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SettingsVisibility
    {
      get
      {
        return this.CheckPrinterType != GlobalDictionaries.Devices.CheckPrinterTypes.None ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public ICommand ShowPrinterPropertiesCommand { get; set; }

    public ICommand PrinterConnectionTestCommand { get; set; }

    public ICommand KkmStatusCommand { get; set; }

    public ICommand SetWauPay { get; set; }

    public ICommand ShowTaxConfigsCommand { get; set; }

    public GlobalDictionaries.Devices.CheckPrinterTypes CheckPrinterType
    {
      get => this.DevicesConfig.CheckPrinter.Type;
      set
      {
        this.DevicesConfig.CheckPrinter.Type = value;
        this.OnPropertyChanged(nameof (CheckPrinterType));
        this.OnPropertyChanged("KkmTypeSelectorVisibility");
        this.OnPropertyChanged("PrinterByNameSelectorVisibility");
        this.OnPropertyChanged("ConnectionConfigGroupVisibility");
        this.OnPropertyChanged("VisibilityButtonShowDriver");
        this.OnPropertyChanged("VisibilityMercuryModel");
        this.OnPropertyChanged("VisibilityNoFiscalReport");
        this.OnPropertyChanged("OnlineKkmSettingsVisibility");
        this.OnPropertyChanged("KkmSettingsVisibility");
        this.OnPropertyChanged("KkmAndEscPosSettingsVisibility");
        this.OnPropertyChanged("VisibilityAllowSalesWithoutCheck");
        this.OnPropertyChanged("SettingsVisibility");
        this.OnPropertyChanged("VisibilityPrintCommentByGood");
        this.OnPropertyChanged("PrintSettingsVisibility");
        this.OnPropertyChanged("VisibilityUktZed");
        this.OnPropertyChanged("VisibilityIkpu");
        this.OnPropertyChanged("VisibilitySendOnlineCheck");
        this.OnPropertyChanged("VisibilityNoPrintCheckIfSendDigitalCheck");
        this.OnPropertyChanged("VisibilityOptionDigitalCheck");
        this.OnPropertyChanged("VisibilityModelKkm");
        this.UpdateUserPage();
      }
    }

    public bool IsEnabledPrintNoFiscalOtherPrinter
    {
      get
      {
        return !this.FiscalKkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.WebKassa, GlobalDictionaries.Devices.FiscalKkmTypes.ReKassa, GlobalDictionaries.Devices.FiscalKkmTypes.HiPos, GlobalDictionaries.Devices.FiscalKkmTypes.UzPos, GlobalDictionaries.Devices.FiscalKkmTypes.Hdm);
      }
    }

    public GlobalDictionaries.Devices.FiscalKkmTypes FiscalKkmType
    {
      get => this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType;
      set
      {
        this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType = value;
        if (value.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.WebKassa, GlobalDictionaries.Devices.FiscalKkmTypes.ReKassa, GlobalDictionaries.Devices.FiscalKkmTypes.HiPos, GlobalDictionaries.Devices.FiscalKkmTypes.UzPos, GlobalDictionaries.Devices.FiscalKkmTypes.Hdm))
          this.IsPrintNoFiscalOtherPrinter = true;
        this.OnPropertyChanged("IsEnabledPrintNoFiscalOtherPrinter");
        this.OnPropertyChanged("IsPrintNoFiscalOtherPrinter");
        this.OnPropertyChanged(nameof (FiscalKkmType));
        this.OnPropertyChanged("ConnectionConfigGroupVisibility");
        this.OnPropertyChanged("OnlineKkmSettingsVisibility");
        this.OnPropertyChanged("VisibilityPrintCommentByGood");
        this.OnPropertyChanged("VisibilityMercuryModel");
        this.OnPropertyChanged("VisibilityButtonShowDriver");
        this.OnPropertyChanged("VisibilityFreeKkmPort");
        this.OnPropertyChanged("VisibilityNoFiscalReport");
        this.OnPropertyChanged("VisibilityModelKkm");
        this.OnPropertyChanged("VisibilityAzSmart");
        this.OnPropertyChanged("PrinterByNameSelectorVisibility");
        this.OnPropertyChanged("PrintSettingsVisibility");
        this.OnPropertyChanged("SettingsVisibility");
        this.OnPropertyChanged("KkmSettingsVisibility");
        this.OnPropertyChanged("VisibilityUktZed");
        this.OnPropertyChanged("VisibilitySendOnlineCheck");
        this.OnPropertyChanged("VisibilityIkpu");
        this.OnPropertyChanged("VisibilityNoPrintCheckIfSendDigitalCheck");
        this.OnPropertyChanged("VisibilityOptionDigitalCheck");
        this.UpdateUserPage();
      }
    }

    public Visibility VisibilityOptionOnlineKkm
    {
      get
      {
        return this._devicesConfig.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm || this._settings.Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityOptionCheckMarkInfo
    {
      get
      {
        return this._devicesConfig.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilitySendOnlineCheck
    {
      get
      {
        return this._devicesConfig.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm && this._settings.Interface.Country == GlobalDictionaries.Countries.Russia || this._devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.HiPos ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility VisibilityNoPrintCheckIfSendDigitalCheck
    {
      get
      {
        if (!this._devicesConfig.CheckPrinter.FiscalKkm.IsNoSendDigitalCheck && !this._devicesConfig.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck)
        {
          if (this._devicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Atol10, GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer, GlobalDictionaries.Devices.FiscalKkmTypes.Shtrih, GlobalDictionaries.Devices.FiscalKkmTypes.VikiPrint, GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer, GlobalDictionaries.Devices.FiscalKkmTypes.VikiDriver, GlobalDictionaries.Devices.FiscalKkmTypes.Atol8, GlobalDictionaries.Devices.FiscalKkmTypes.Mercury, GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests))
            return Visibility.Visible;
        }
        return Visibility.Collapsed;
      }
    }

    public Visibility VisibilityOptionDigitalCheck
    {
      get
      {
        return this._devicesConfig.CheckPrinter.FiscalKkm.IsNoSendDigitalCheck ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public PageCheckPrinterViewModel(Gbs.Core.Config.Devices devicesConfig, Gbs.Core.Config.Settings settings, Page userPage)
    {
      List<EntityProperties.PropertyType> first = new List<EntityProperties.PropertyType>();
      EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
      propertyType.Name = Translate.GlobalDictionaries_Не_указано;
      propertyType.Uid = Guid.Empty;
      first.Add(propertyType);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListUkt\u003Ek__BackingField = first.Concat<EntityProperties.PropertyType>(EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good, false).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
      {
        if (x.Type != GlobalDictionaries.EntityPropertyTypes.Text)
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid);
      }))).ToList<EntityProperties.PropertyType>();
      this._devicesConfig = new Gbs.Core.Config.Devices();
      this._settings = new Gbs.Core.Config.Settings();
      // ISSUE: reference to a compiler-generated field
      this.\u003CFfdVersionsDictionary\u003Ek__BackingField = GlobalDictionaries.Devices.FfdVersionsDictionary();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCheckPrinterDictionary\u003Ek__BackingField = GlobalDictionaries.Devices.CheckPrinterTypesDictionary();
      // ISSUE: reference to a compiler-generated field
      this.\u003CRuTaxSystemsDictionary\u003Ek__BackingField = GlobalDictionaries.RuTaxSystemsDictionary();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCheckTemplatesList\u003Ek__BackingField = new List<FileInfo>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCheckNoFiscalTemplatesList\u003Ek__BackingField = new List<FileInfo>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      PageCheckPrinterViewModel printerViewModel = this;
      try
      {
        this._printersList = (IEnumerable) PrinterSettings.InstalledPrinters;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения списка принтеров");
        this._printersList = (IEnumerable) new List<string>();
      }
      this.UserPage = userPage;
      this.DevicesConfig = devicesConfig;
      this.Settings = settings;
      this.UpdateUserPage();
      this.TaxRatesDictionary = devicesConfig.CheckPrinter.FiscalKkm.TaxRates;
      if (ReportType.CashMemo.Directory.Exists)
      {
        FileInfo[] files = ReportType.CashMemo.Directory.GetFiles("*.frx");
        this.CheckTemplatesList = new List<FileInfo>();
        foreach (FileInfo fileInfo in files)
          this.CheckTemplatesList.Add(fileInfo);
      }
      if (ReportType.NonFiscalPrint.Directory.Exists)
      {
        FileInfo[] files = ReportType.NonFiscalPrint.Directory.GetFiles("*.frx");
        this.CheckNoFiscalTemplatesList = new List<FileInfo>();
        foreach (FileInfo fileInfo in files)
          this.CheckNoFiscalTemplatesList.Add(fileInfo);
      }
      this.SetWauPay = (ICommand) new RelayCommand((Action<object>) (obj => new FrmPaymentMethodKkm().ShowConfig(devicesConfig.CheckPrinter.FiscalKkm)));
      this.ShowTaxConfigsCommand = (ICommand) new RelayCommand((Action<object>) (obj => new FrmTaxKkmSettings().ShowConfig(devicesConfig.CheckPrinter.FiscalKkm)));
      this.ShowPrinterPropertiesCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        switch (printerViewModel.CheckPrinterType)
        {
          case GlobalDictionaries.Devices.CheckPrinterTypes.EscPos:
            int num = (int) MessageBoxHelper.Show(Translate.PageCheckPrinterViewModel_Возможность_не_реализована);
            break;
          case GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm:
            using (KkmHelper kkmHelper = new KkmHelper(printerViewModel.DevicesConfig))
            {
              kkmHelper.ShowProperties();
              break;
            }
        }
      }));
      this.PrinterConnectionTestCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.PageCheckPrinterViewModel_Проверка_связи_с_принтером_чеков);
        switch (printerViewModel.CheckPrinterType)
        {
          case GlobalDictionaries.Devices.CheckPrinterTypes.EscPos:
            int num = (int) MessageBoxHelper.Show(Translate.PageCheckPrinterViewModel_Возможность_не_реализована);
            break;
          case GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm:
            using (KkmHelper kkmHelper = new KkmHelper(devicesConfig))
            {
              kkmHelper.PrintCheck(new Gbs.Core.Devices.CheckPrinters.CheckData.CheckData(new List<CheckGood>()
              {
                new CheckGood(new Gbs.Core.Entities.Goods.Good()
                {
                  Name = PartnersHelper.ProgramName(),
                  Group = new GoodGroups.Group()
                }, 3.57M, 0M, 1.2M, "", "")
              }, new List<CheckPayment>()
              {
                new CheckPayment()
                {
                  Method = GlobalDictionaries.KkmPaymentMethods.Cash,
                  Sum = 50.0M
                }
              }, CheckFiscalTypes.NonFiscal, new Cashier()
              {
                Name = Translate.CheckData_Кассир
              }));
              break;
            }
        }
        progressBar.Close();
      }));
      this.KkmStatusCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        using (KkmHelper kkmHelper = new KkmHelper(devicesConfig))
          kkmHelper.ShowStatus();
      }));
    }

    public PageCheckPrinterViewModel()
    {
      List<EntityProperties.PropertyType> first = new List<EntityProperties.PropertyType>();
      EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
      propertyType.Name = Translate.GlobalDictionaries_Не_указано;
      propertyType.Uid = Guid.Empty;
      first.Add(propertyType);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListUkt\u003Ek__BackingField = first.Concat<EntityProperties.PropertyType>(EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good, false).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
      {
        if (x.Type != GlobalDictionaries.EntityPropertyTypes.Text)
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid);
      }))).ToList<EntityProperties.PropertyType>();
      this._devicesConfig = new Gbs.Core.Config.Devices();
      this._settings = new Gbs.Core.Config.Settings();
      // ISSUE: reference to a compiler-generated field
      this.\u003CFfdVersionsDictionary\u003Ek__BackingField = GlobalDictionaries.Devices.FfdVersionsDictionary();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCheckPrinterDictionary\u003Ek__BackingField = GlobalDictionaries.Devices.CheckPrinterTypesDictionary();
      // ISSUE: reference to a compiler-generated field
      this.\u003CRuTaxSystemsDictionary\u003Ek__BackingField = GlobalDictionaries.RuTaxSystemsDictionary();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCheckTemplatesList\u003Ek__BackingField = new List<FileInfo>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCheckNoFiscalTemplatesList\u003Ek__BackingField = new List<FileInfo>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool ValidationConfig()
    {
      if (this.DevicesConfig.CheckPrinter.Type.IsEither<GlobalDictionaries.Devices.CheckPrinterTypes>(GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter) && this.DevicesConfig.CheckPrinter.CheckTemplate.IsNullOrEmpty())
      {
        MessageBoxHelper.Warning(Translate.PageCheckPrinterViewModel_Необходимо_выбрать_шаблон_для_печати_чека);
        return false;
      }
      if (this.DevicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter && this.DevicesConfig.CheckPrinter.PrinterSetting.IsSendInPrinter && this.DevicesConfig.CheckPrinter.PrinterSetting.PrinterName.IsNullOrEmpty())
      {
        MessageBoxHelper.Warning(Translate.PageCheckPrinterViewModel_Необходимо_выбрать_принтер_для_печати_чека);
        return false;
      }
      if (this.DevicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.Mercury && this.DevicesConfig.CheckPrinter.FiscalKkm.Model.IsNullOrEmpty())
      {
        MessageBoxHelper.Warning(Translate.PageCheckPrinterViewModel_Необходимо_выбрать_модель_ККТ_Меркурий);
        return false;
      }
      if (this.DevicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm || this.DevicesConfig.CheckPrinter.FiscalKkm.KkmType != GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart || !this.DevicesConfig.CheckPrinter.FiscalKkm.AzSmartMerchantId.IsNullOrEmpty())
        return true;
      MessageBoxHelper.Warning(Translate.PageCheckPrinterViewModel_Необходимо_указать_MerchantId_для_ККТ_AZ_SMART);
      return false;
    }
  }
}
