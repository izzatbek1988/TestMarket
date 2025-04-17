// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.CHead
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public class CHead
  {
    public CheckDocumentType DOCTYPE { get; set; }

    public CheckDocumentSubType DOCSUBTYPE { get; set; }

    public string UID { get; set; }

    public string TIN { get; set; }

    public string IPN { get; set; }

    public string ORGNM { get; set; }

    public string POINTNM { get; set; }

    public string POINTADDR { get; set; }

    public string ORDERDATE { get; set; }

    public string ORDERTIME { get; set; }

    public string ORDERNUM { get; set; }

    public string CASHDESKNUM { get; set; }

    public string CASHREGISTERNUM { get; set; }

    public string ORDERRETNUM { get; set; }

    public string ORDERSTORNUM { get; set; }

    public string OPERTYPENM { get; set; }

    public string VEHICLERN { get; set; }

    public bool REVOKELASTONLINEDOC { get; set; }

    public string CASHIER { get; set; }

    public string LOGOURL { get; set; }

    public string VER { get; set; }

    public string ORDERTAXNUM { get; set; }

    public bool REVOKED { get; set; }

    public bool STORNED { get; set; }

    public bool OFFLINE { get; set; }

    public string PREVDOCHASH { get; set; }
  }
}
