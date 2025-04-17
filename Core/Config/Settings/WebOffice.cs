// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.WebOffice
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class WebOffice
  {
    public RemoteControl.CreateItemPeriods CreatePeriod { get; set; }

    public bool IsActive => true;

    public string Token { get; set; } = "";

    public bool IsCreateOnStart { get; set; }

    public bool IsCreateOnExit { get; set; }
  }
}
