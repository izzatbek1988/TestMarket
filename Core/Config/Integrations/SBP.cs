// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.SBP
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;

#nullable disable
namespace Gbs.Core.Config
{
  public class SBP
  {
    public GlobalDictionaries.Devices.SbpTypes Type { get; set; }

    public GlobalDictionaries.Devices.SbpVisualizationTypes VisualizationType { get; set; }

    public string PayQrClientID { get; set; } = "";

    public string PayQrDeviceID { get; set; } = "";

    public string PayQrURL { get; set; }

    public bool IsPrintQrForCheck { get; set; }

    public CryptoConfig ClientSecret { get; set; } = new CryptoConfig();
  }
}
