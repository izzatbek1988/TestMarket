// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.GoodsPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.ExtraPrice;
using Gbs.Forms.Settings.Pages.DevicesSubPages;
using Gbs.Forms.Settings.PricingRules;
using Gbs.Forms.Settings.PropertiesEntities;
using Gbs.Forms.Settings.Units;
using Gbs.Helpers;
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
  public partial class GoodsPageViewModel : ViewModelWithForm
  {
    public Dictionary<SalePriceType, string> SalePriceTypeDictionary
    {
      get
      {
        return new Dictionary<SalePriceType, string>()
        {
          {
            SalePriceType.Max,
            Translate.GoodsPageViewModel_SalePriceTypeDictionary_максимальная
          },
          {
            SalePriceType.Min,
            Translate.GoodsPageViewModel_SalePriceTypeDictionary_минимальная
          },
          {
            SalePriceType.Avg,
            Translate.GoodsPageViewModel_SalePriceTypeDictionary_средняя
          },
          {
            SalePriceType.Last,
            Translate.GoodsPageViewModel_SalePriceTypeDictionary_последняя
          }
        };
      }
    }

    public ICommand ShowExtraPriceList
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new FrmListExtraPrice().ShowDialog()));
      }
    }

    public ICommand ShowExtraPricingRules
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!new ExtraPriceRulesRepository().GetActiveItems().Any<ExtraPriceRule>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.GoodsPageViewModel_ShowExtraPricingRules_Чтобы_добавить_правила_для_расчета__требуется_добавить_хотя_бы_одну_дополнительную_цену_, icon: MessageBoxImage.Exclamation);
          }
          else
            new FrmExtraPriceRuleList().ShowDialog();
        }));
      }
    }

    public Gbs.Core.Config.Settings Settings { get; set; }

    public ICommand ShowPropGood
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          new FrmPropertyList().ShowProperties(GlobalDictionaries.EntityTypes.Good);
          this.OnPropertyChanged("Settings");
          this.PageScale.LoadingPlu();
        }));
      }
    }

    public ScaleSettingViewModel PageScale { get; set; }

    public ICommand ShowUnitsList
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => new FrmGoodUnits().ShowDialog()));
    }

    public ICommand ShowPricingRules
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new PricingRulesList().ShowDialog()));
      }
    }

    public Visibility VisibilityUktZed
    {
      get
      {
        return !new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.HelpMicro, GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas) ? Visibility.Collapsed : Visibility.Visible;
      }
    }
  }
}
