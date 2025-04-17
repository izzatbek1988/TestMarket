// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.CafeViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms.Settings.PercentForService;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class CafeViewModel : ViewModelWithForm
  {
    public Good PercentForServiceGood;

    public Visibility VisibilitySettingForCafe
    {
      get
      {
        DataBase dataBaseConfig = this.DataBaseConfig;
        return (dataBaseConfig != null ? (dataBaseConfig.ModeProgram == GlobalDictionaries.Mode.Cafe ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityNotification
    {
      get
      {
        DataBase dataBaseConfig = this.DataBaseConfig;
        return (dataBaseConfig != null ? (dataBaseConfig.ModeProgram == GlobalDictionaries.Mode.Cafe ? 1 : 0) : 0) == 0 ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public ICommand EditModeProgramCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.DataBaseConfig.ModeProgram = GlobalDictionaries.Mode.Cafe;
          this.SetModeActionProgram(GlobalDictionaries.Mode.Cafe);
          this.OnPropertyChanged("VisibilitySettingForCafe");
          this.OnPropertyChanged("VisibilityNotification");
        }));
      }
    }

    public Dictionary<Cafe.MenuConfig.SelectGoodForCafeEnum, string> DictionarySelectGoodForCafe
    {
      get
      {
        return new Dictionary<Cafe.MenuConfig.SelectGoodForCafeEnum, string>()
        {
          {
            Cafe.MenuConfig.SelectGoodForCafeEnum.None,
            Translate.CafeViewModel_DictionarySelectGoodForCafe_Не_отображать
          },
          {
            Cafe.MenuConfig.SelectGoodForCafeEnum.OverGroup,
            Translate.CafeViewModel_DictionarySelectGoodForCafe_Показывать_НАД_категориями_товаров
          },
          {
            Cafe.MenuConfig.SelectGoodForCafeEnum.UnderGroup,
            Translate.CafeViewModel_DictionarySelectGoodForCafe_Показывать_ПОД_категориями_товаров
          }
        };
      }
    }

    public bool IsPercentForService
    {
      get
      {
        Cafe cafeConfig = this.CafeConfig;
        return cafeConfig != null && cafeConfig.IsPercentForService;
      }
      set
      {
        this.CafeConfig.IsPercentForService = value;
        this.PercentForServiceGood.IsDeleted = !value;
      }
    }

    public Cafe CafeConfig { get; set; }

    private DataBase DataBaseConfig { get; set; }

    public CafeViewModel(Cafe cafeConfig, DataBase dataBaseConfig)
    {
      Good good = new Good();
      good.Uid = GlobalDictionaries.PercentForServiceGoodUid;
      good.Name = Translate.ПроцентЗаОбслуживание;
      GoodGroups.Group group = new GoodGroups.Group();
      group.Uid = GlobalDictionaries.PercentForServiceGroupUid;
      good.Group = group;
      good.StocksAndPrices = new List<GoodsStocks.GoodStock>()
      {
        new GoodsStocks.GoodStock()
        {
          Storage = Storages.GetStorages().First<Storages.Storage>()
        }
      };
      good.IsDeleted = true;
      this.PercentForServiceGood = good;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CafeConfig = cafeConfig;
      this.DataBaseConfig = dataBaseConfig;
    }

    public CafeViewModel()
    {
      Good good = new Good();
      good.Uid = GlobalDictionaries.PercentForServiceGoodUid;
      good.Name = Translate.ПроцентЗаОбслуживание;
      GoodGroups.Group group = new GoodGroups.Group();
      group.Uid = GlobalDictionaries.PercentForServiceGroupUid;
      good.Group = group;
      good.StocksAndPrices = new List<GoodsStocks.GoodStock>()
      {
        new GoodsStocks.GoodStock()
        {
          Storage = Storages.GetStorages().First<Storages.Storage>()
        }
      };
      good.IsDeleted = true;
      this.PercentForServiceGood = good;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool Save() => new ConfigsRepository<Cafe>().Save(this.CafeConfig);

    public ICommand ShowPercentForServiceRuleCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new PercentForServiceList().ShowDialog()));
      }
    }

    public Action<GlobalDictionaries.Mode> SetModeActionProgram { get; set; }
  }
}
