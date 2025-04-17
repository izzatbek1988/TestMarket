// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Telegram
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Config
{
  public class Telegram
  {
    public bool IsActive { get; set; }

    public string UsernameTo { get; set; } = string.Empty;

    public bool IsSendForTime { get; set; }

    public string TimesSend { get; set; } = "9:00, 12:00, 15:00";

    public bool IsSendForSum { get; set; }

    public Decimal SumSend { get; set; }

    public bool IsSendOnOff { get; set; }

    public bool IsSendBackUp { get; set; }

    public bool IsSendNotificationLicense { get; set; }
  }
}
