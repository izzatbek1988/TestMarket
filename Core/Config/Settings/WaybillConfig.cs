// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.WaybillConfig
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class WaybillConfig
  {
    public bool IsOfferPreviousBuyPrice { get; set; }

    public bool IsOfferPreviousSalePrice { get; set; }

    public WaybillConfig.RePriceVariants RePriceRule { get; set; }

    public WaybillConfig.SaveDeletedGoodVariants SaveDeletedGoodVariant { get; set; }

    public bool IsMoreDecimalPlaceBuyPrice { get; set; }

    public enum RePriceVariants
    {
      CreateStocksWithNewPrice,
      RePriceExitsStocks,
      RequestForEachWaybill,
    }

    public enum SaveDeletedGoodVariants
    {
      AllRecover,
      NoNullRecover,
      UnRecover,
    }
  }
}
