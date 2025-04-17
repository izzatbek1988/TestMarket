// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.CBodyRow
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class CBodyRow
  {
    public string CODE { get; set; }

    public string BARCODE { get; set; }

    public string UKTZED { get; set; }

    public string DKPP { get; set; }

    public string NAME { get; set; }

    public string DESCRIPTION { get; set; }

    public int UNITCD { get; set; }

    public string UNITNM { get; set; }

    public Decimal AMOUNT { get; set; }

    public Decimal PRICE { get; set; }

    public string LETTERS { get; set; }

    public Decimal COST { get; set; }

    public string RECIPIENTNM { get; set; }

    public string RECIPIENTIPN { get; set; }

    public string BANKCD { get; set; }

    public string BANKNM { get; set; }

    public string BANKRS { get; set; }

    public string PAYDETAILS { get; set; }

    public string PAYPURPOSE { get; set; }

    public string PAYERNM { get; set; }

    public string PAYERIPN { get; set; }

    public string PAYERPACTNUM { get; set; }

    public string PAYDETAILSP { get; set; }

    public string BASISPAY { get; set; }

    public string PAYOUTTYPE { get; set; }

    public string FUELORDERNUM { get; set; }

    public string FUELUNITNM { get; set; }

    public string FUELTANKNUM { get; set; }

    public string FUELCOLUMNNUM { get; set; }

    public string FUELFAUCETNUM { get; set; }

    public bool FUELSALESIGN { get; set; }

    public int VALCD { get; set; }

    public string VALSYMCD { get; set; }

    public string VALNM { get; set; }

    public int? VALOPERTYPE { get; set; }

    public string VALOUTCD { get; set; }

    public string VALOUTSYMCD { get; set; }

    public string VALOUTNM { get; set; }

    public string VALCOURSE { get; set; }

    public string VALCOURSEDATE { get; set; }

    public Decimal VALFOREIGNSUM { get; set; }

    public Decimal VALNATIONALSUM { get; set; }

    public Decimal VALCOMMISSION { get; set; }

    public int VALOPERCNT { get; set; }

    public bool VALREFUSESELL { get; set; }

    public int? USAGETYPE { get; set; }

    public int? DISCOUNTTYPE { get; set; }

    public Decimal SUBTOTAL { get; set; }

    public int? DISCOUNTNUM { get; set; }

    public string DISCOUNTTAX { get; set; }

    public Decimal DISCOUNTPERCENT { get; set; }

    public Decimal DISCOUNTSUM { get; set; }

    public CExciseLabelsRow[] EXCISELABELS { get; set; }
  }
}
