// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Devices
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class Devices : IConfig
  {
    public List<ItemDevice> ListDevices { get; set; } = new List<ItemDevice>();

    public BarcodeScanner BarcodeScanner { get; set; } = new BarcodeScanner();

    public CheckPrinter CheckPrinter { get; set; } = new CheckPrinter();

    public AcquiringTerminal AcquiringTerminal { get; set; } = new AcquiringTerminal();

    public SBP SBP { get; set; } = new SBP();

    public DisplayPayer DisplayPayer { get; set; } = new DisplayPayer();

    public Tsd Tsd { get; set; } = new Tsd();

    public Scale Scale { get; set; } = new Scale();

    public ScaleWithLable ScaleWithLable { get; set; } = new ScaleWithLable();

    public LablePrinter LablePrinter { get; set; } = new LablePrinter();

    public SecondMonitor SecondMonitor { get; set; } = new SecondMonitor();

    public DisplayQr DisplayQr { get; set; } = new DisplayQr();

    public DisplayNumbersPayer DisplayNumbersPayer { get; set; } = new DisplayNumbersPayer();

    public ExtraPrinters ExtraPrinters { get; set; } = new ExtraPrinters();

    public KeyboardDevice Keyboard { get; set; } = new KeyboardDevice();
  }
}
