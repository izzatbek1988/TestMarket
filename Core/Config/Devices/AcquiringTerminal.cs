// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.AcquiringTerminal
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;

#nullable disable
namespace Gbs.Core.Config
{
  public class AcquiringTerminal
  {
    public GlobalDictionaries.Devices.AcquiringTerminalTypes Type { get; set; }

    public ComPort ComPort { get; set; } = new ComPort();

    public LanConnection LanConnection { get; set; } = new LanConnection();

    public GlobalDictionaries.Devices.ConnectionTypes ConnectionType { get; set; }

    public bool PrintSlipFromFile { get; set; }

    public int SlipPrintCounts { get; set; } = 1;

    public string SlipFilePath { get; set; }

    public string Description { get; set; }

    public GlobalDictionaries.Encoding SlipEncoding { get; set; }
  }
}
