// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.CheckPrinter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Gbs.Core.Config
{
  [ExcludeFromCodeCoverage]
  [Serializable]
  public class CheckPrinter
  {
    public GlobalDictionaries.Devices.CheckPrinterTypes Type { get; set; }

    public DeviceConnection Connection { get; set; } = new DeviceConnection();

    public PrinterSetting PrinterSetting { get; set; } = new PrinterSetting();

    public FiscalKkm FiscalKkm { get; set; } = new FiscalKkm();

    public bool PrintCheckOnEverySale { get; set; }

    public bool IsShowPrintConfirmationForm { get; set; }

    public bool IsPrintNoFiscalOtherPrinter { get; set; }

    public bool IsPrintCommentByGood { get; set; }

    public bool IsNewProtocolMercury { get; set; }

    public bool CutPaperAfterDocuments { get; set; } = true;

    public string CheckTemplate { get; set; } = "";

    public string CheckNoFiscalTemplate { get; set; } = "";

    public int PaperSymbolsWidth { get; set; } = 32;
  }
}
