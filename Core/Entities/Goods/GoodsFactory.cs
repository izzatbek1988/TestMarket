// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Goods.GoodsFactory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Factories;
using Gbs.Helpers.HomeOffice.Entity;

#nullable disable
namespace Gbs.Core.Entities.Goods
{
  public class GoodsFactory : IFactory<Good>
  {
    public static Good Create(GoodHome homeGood)
    {
      if (homeGood == null)
        return new Good();
      Good good = new Good();
      good.Uid = homeGood.Uid;
      good.Barcode = homeGood.Barcode;
      good.Barcodes = homeGood.Barcodes;
      good.DateAdd = homeGood.DateAdd;
      good.Description = homeGood.Description;
      good.Name = homeGood.Name;
      GoodGroups.Group group = new GoodGroups.Group();
      group.Uid = homeGood.GroupUid;
      good.Group = group;
      good.IsDeleted = homeGood.IsDeleted;
      good.SetStatus = homeGood.SetStatus;
      good.Properties = homeGood.Properties;
      good.Modifications = homeGood.Modifications;
      good.SetContent = homeGood.SetContent;
      good.StocksAndPrices = homeGood.StocksAndPrices;
      return good;
    }
  }
}
