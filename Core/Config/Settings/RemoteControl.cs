// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.RemoteControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Config
{
  public class RemoteControl
  {
    public Email Email { get; set; } = new Email();

    public Telegram Telegram { get; set; } = new Telegram();

    public Cloud Cloud { get; set; } = new Cloud();

    public WebOffice WebOffice { get; set; } = new WebOffice();

    [Obsolete("Не использовать. Перенесено в настройки клиентов")]
    public ClientInfo Client { get; set; }

    public enum CreateItemPeriods
    {
      None,
      AndEvery1Hour,
      AndEvery3Hours,
      AndEvery6Hours,
      AndEvery12Hours,
    }
  }
}
