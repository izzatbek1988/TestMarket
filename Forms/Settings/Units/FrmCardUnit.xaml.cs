// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Units.UnitCardModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Units
{
  public partial class UnitCardModelView : ViewModelWithForm
  {
    public Dictionary<int, string> RuFfdUnitsCodes
    {
      get
      {
        return new Dictionary<int, string>()
        {
          {
            0,
            Translate.ШтукиЕдиницы
          },
          {
            10,
            Translate.Грамм
          },
          {
            11,
            Translate.Килограмм
          },
          {
            12,
            Translate.Тонна
          },
          {
            20,
            Translate.Сантиметр
          },
          {
            21,
            Translate.Дециметр
          },
          {
            22,
            Translate.Метр
          },
          {
            30,
            Translate.КвСантиметр
          },
          {
            31,
            Translate.КвДециметр
          },
          {
            32,
            Translate.КвМетр
          },
          {
            40,
            Translate.Миллилитр
          },
          {
            41,
            Translate.Литр
          },
          {
            42,
            Translate.КубическийМетр
          },
          {
            50,
            Translate.КиловаттЧас
          },
          {
            51,
            Translate.Гигакалория
          },
          {
            70,
            Translate.СуткиДень
          },
          {
            71,
            Translate.Час
          },
          {
            72,
            Translate.Минута
          },
          {
            73,
            Translate.Секунда
          },
          {
            80,
            Translate.Килобайт
          },
          {
            81,
            Translate.Мегабайт
          },
          {
            82,
            Translate.Гигабайт
          },
          {
            83,
            Translate.Терабайт
          }
        };
      }
    }

    public Visibility VisibilityRuFfdUnitsCodes
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion < GlobalDictionaries.Devices.FfdVersions.Ffd120 || new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public GoodsUnits.GoodUnit Unit { get; set; }

    public bool SaveResult { get; set; }

    public ICommand SaveCommand { get; set; }

    public UnitCardModelView()
    {
      this.SaveCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    public void Save()
    {
      this.SaveResult = this.Unit.Save();
      if (!this.SaveResult)
        return;
      this.CloseAction();
    }
  }
}
