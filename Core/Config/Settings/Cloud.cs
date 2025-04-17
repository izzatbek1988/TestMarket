// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Cloud
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class Cloud
  {
    public bool IsSendBackupDb { get; set; }

    public bool IsActive { get; set; }

    public string Path { get; set; } = ApplicationInfo.GetInstance().Paths.DataPath;

    public bool IsAutoSend { get; set; }

    public bool IsSendForTime { get; set; } = true;

    public string TimesSend { get; set; } = "9:00, 12:00, 15:00";

    public bool IsAcceptHome { get; set; }
  }
}
