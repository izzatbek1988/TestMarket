// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Interface
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

#nullable disable
namespace Gbs.Core.Config
{
  public class Interface
  {
    private GlobalDictionaries.Languages _language;

    public ViewSaleJournal ViewSaleJournal { get; set; } = ViewSaleJournal.ListSale;

    [JsonConverter(typeof (StringEnumConverter))]
    public GlobalDictionaries.Skin Theme { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    public GlobalDictionaries.Languages Language
    {
      get => this._language;
      set => this._language = value;
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public GlobalDictionaries.Countries Country { get; set; } = GlobalDictionaries.Countries.Russia;

    public bool IsVisibilityDesign { get; set; }

    public bool IsVisibilitySaleSumForBasket { get; set; }

    public bool IsHideExtraZeros { get; set; }

    public bool IsHideExtraZerosPrice { get; set; }

    public int CountSelectGood { get; set; }

    public bool IsGroupSelectGood { get; set; }

    public int BasketsCountInMainForm { get; set; } = 1;

    public string TemplatesFrPath { get; set; } = ApplicationInfo.GetInstance().Paths.DataPath + "TemplatesFR\\";

    public string BackgroundColor { get; set; }

    public string SelectionColor { get; set; }

    public bool IsVisibilityAllDiscountBtn { get; set; }

    public bool IsVisibilityExtraPercent { get; set; }

    public bool IsPlaySoundsForEvents { get; set; }

    public bool IsColorSalePriceMoreBuy { get; set; }

    public bool IsColorEditSalePrice { get; set; }

    public bool IsSwitchThemeForClickToTime { get; set; }

    public bool IsShowHelpTooltip { get; set; }
  }
}
