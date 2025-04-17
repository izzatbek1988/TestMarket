// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.OtherConfig
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;

#nullable disable
namespace Gbs.Core.Config
{
  public class OtherConfig
  {
    public string EmailForSendDataSupport { get; set; }

    public string Thumbprint { get; set; } = "";

    public OtherConfig.CsvSetting Csv { get; set; } = new OtherConfig.CsvSetting();

    public OtherConfig.LogConfig Logs { get; set; } = new OtherConfig.LogConfig();

    public OtherConfig.Update UpdateConfig { get; set; } = new OtherConfig.Update();

    public class LogConfig
    {
    }

    public class CsvSetting
    {
      public bool IsOnQuote { get; set; }

      public string Separate { get; set; } = ";";
    }

    public class Update
    {
      public GlobalDictionaries.VersionUpdate VersionUpdate { get; set; }

      public OtherConfig.UpdateType UpdateType { get; set; } = OtherConfig.UpdateType.AutoUpdate;

      public int TryUpdateCount { get; set; }
    }

    public enum UpdateType
    {
      NoUpdate,
      UpdateNotification,
      AutoUpdate,
    }
  }
}
