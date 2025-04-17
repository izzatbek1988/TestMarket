// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.ActionsGoodPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.PropertiesEntities;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class ActionsGoodPageViewModel : ViewModel
  {
    private Gbs.Core.Config.Settings _setting = new Gbs.Core.Config.Settings();

    public Visibility VisibilityRuMarkSetting
    {
      get
      {
        Gbs.Core.Config.Settings setting = this.Setting;
        return (setting != null ? (setting.Interface.Country == GlobalDictionaries.Countries.Russia ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ObservableCollection<MultiValueControl.Value> SmokeValues { get; set; } = new ObservableCollection<MultiValueControl.Value>();

    public Gbs.Core.Config.Settings Setting
    {
      get => this._setting;
      set
      {
        this._setting = value;
        this.OnPropertyChanged(nameof (Setting));
      }
    }

    public Dictionary<WaybillConfig.RePriceVariants, string> RepriceRulesDictionary { get; set; } = new Dictionary<WaybillConfig.RePriceVariants, string>()
    {
      {
        WaybillConfig.RePriceVariants.CreateStocksWithNewPrice,
        Translate.ActionsGoodPageViewModel_Не_переоценивать_товары
      },
      {
        WaybillConfig.RePriceVariants.RePriceExitsStocks,
        Translate.ActionsGoodPageViewModel_Переоценивать_имеющиеся_остатки_товаров_по_ценам_из_накладной
      },
      {
        WaybillConfig.RePriceVariants.RequestForEachWaybill,
        Translate.ActionsGoodPageViewModel_RepriceRulesDictionary_Запрашивать_для_каждой_накладной
      }
    };

    public Dictionary<WaybillConfig.SaveDeletedGoodVariants, string> SaveDeletedGoodVariantsDictionary { get; set; } = new Dictionary<WaybillConfig.SaveDeletedGoodVariants, string>()
    {
      {
        WaybillConfig.SaveDeletedGoodVariants.AllRecover,
        Translate.ActionsGoodPageViewModel_SaveDeletedGoodVariantsDictionary_Восстанавливать_все_удаленные_товары
      },
      {
        WaybillConfig.SaveDeletedGoodVariants.NoNullRecover,
        Translate.ActionsGoodPageViewModel_SaveDeletedGoodVariantsDictionary_Восстанавливать_только_ненулевые_удаленные_товары
      },
      {
        WaybillConfig.SaveDeletedGoodVariants.UnRecover,
        Translate.ActionsGoodPageViewModel_SaveDeletedGoodVariantsDictionary_Не_восстанавливать_удаленные_товары
      }
    };

    public ICommand ShowPropDocument
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmPropertyList().ShowProperties(GlobalDictionaries.EntityTypes.Document)));
      }
    }

    public ActionsGoodPageViewModel()
    {
    }

    public ActionsGoodPageViewModel(Gbs.Core.Config.Settings settings)
    {
      this.Setting = settings;
      string smokeBlockValues = settings.Sales.SmokeBlockValues;
      char[] separator = new char[3]{ ' ', ';', ',' };
      foreach (string str in smokeBlockValues.Split(separator, StringSplitOptions.RemoveEmptyEntries))
        this.SmokeValues.Add(new MultiValueControl.Value()
        {
          DisplayedValue = str
        });
    }

    public bool Save()
    {
      this.Setting.Sales.SmokeBlockValues = string.Join("; ", this.SmokeValues.Select<MultiValueControl.Value, string>((Func<MultiValueControl.Value, string>) (x => x.DisplayedValue)));
      return true;
    }
  }
}
