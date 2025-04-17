// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Sales
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class Sales
  {
    public bool IsOverStockForInventoryAddLastWaybill { get; set; }

    public bool AllowSalesToMinus { get; set; }

    public bool AllowSalesMissingItems { get; set; } = true;

    public bool IsUnitsInGrams { get; set; }

    public bool IsReturnEmptyComment { get; set; }

    public bool IsTabooSaleLessBuyPrice { get; set; }

    public bool IsSearchAllBarcode { get; set; }

    public bool IsCommentSale { get; set; }

    public bool IsMinusForReturnInSale { get; set; }

    public bool UseLastIncomeStockForMinusActions { get; set; }

    public RoundTotal RoundTotals { get; set; } = new RoundTotal();

    public string SmokeBlockValues { get; set; } = "10";

    public bool IsTabooSaleNoLabel { get; set; } = true;

    public bool IsTabooSaleNoСorrected { get; set; } = true;

    public bool IsCheckMarkInfoTrueApi { get; set; } = true;

    public bool IsSendNameForPrepaidCheck { get; set; }

    public bool IsLimitedDecimalPlace { get; set; }
  }
}
