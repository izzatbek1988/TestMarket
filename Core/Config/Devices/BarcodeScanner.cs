// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.BarcodeScanner
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

#nullable disable
namespace Gbs.Core.Config
{
  [ExcludeFromCodeCoverage]
  public class BarcodeScanner
  {
    public GlobalDictionaries.Devices.ScannerTypes Type { get; set; }

    public ComPort ComPort { get; set; } = new ComPort();

    public BarcodeScanner.BarcodePrefixes Prefixes { get; set; } = new BarcodeScanner.BarcodePrefixes();

    public bool AllowUseAlphabetBarcodes { get; set; }

    public bool IsEmulateKeyboard { get; set; } = true;

    public HotKeysHelper.Hotkey GsCodeHotKey { get; set; } = new HotKeysHelper.Hotkey(Key.F8);

    public class BarcodePrefixes
    {
      public string DiscountCard { get; set; } = "22";

      public string WeightGoods { get; set; } = "21";

      public string RandomGenerated { get; set; } = "20";

      public string Modifications { get; set; } = "25";

      public string Certificates { get; set; } = "23";

      public string Users { get; set; } = "24";
    }
  }
}
