// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.FilterOptions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class FilterOptions : IConfig
  {
    public SearchGood SearchGood { get; set; } = new SearchGood();

    public GoodsCatalog GoodsCatalog { get; set; } = new GoodsCatalog();

    public ClientSearchOptions ClientSearch { get; set; } = new ClientSearchOptions();

    public SaleJournalGood SaleJournalSearch { get; set; } = new SaleJournalGood();

    public FavoritesGoodsSearchOptions FavoritesGoodsSearch { get; set; } = new FavoritesGoodsSearchOptions();

    public GoodProp RemoteGoodsCatalog { get; set; } = new GoodProp();

    public string ConfigFileName => nameof (FilterOptions);
  }
}
