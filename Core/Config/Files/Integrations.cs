// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Integrations
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class Integrations : IConfig
  {
    public PlanfixSetting Planfix { get; set; } = new PlanfixSetting();

    public Polycard Polycard { get; set; } = new Polycard();

    public bool IsActiveBarcodeHarvester { get; set; }

    public PolyCloud PolyCloud { get; set; } = new PolyCloud();

    public JsonApi JsonApi { get; set; } = new JsonApi();

    public EgaisSettings Egais { get; set; } = new EgaisSettings();

    public Crpt Crpt { get; set; } = new Crpt();

    public Sms Sms { get; set; } = new Sms();

    public DaData DaData { get; set; } = new DaData();

    public bool IsBarcodesMiDays { get; set; }
  }
}
