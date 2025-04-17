// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Settings
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Core.Config
{
  public class Settings : IConfig
  {
    public string DevelopedMode { get; set; }

    public Sales Sales { get; set; } = new Sales();

    public Settings.ClientOrderConfig ClientOrder { get; set; } = new Settings.ClientOrderConfig();

    public Users Users { get; set; } = new Users();

    public Interface Interface { get; set; } = new Interface();

    [Obsolete("НЕ ИСПОЛЬЗОВАТЬ! Перенесено в БД.")]
    public Discounts Discounts { get; set; }

    public Payments Payments { get; set; } = new Payments();

    public RemoteControl RemoteControl { get; set; } = new RemoteControl();

    public GoodsConfig GoodsConfig { get; set; } = new GoodsConfig();

    public OtherConfig Other { get; set; } = new OtherConfig();

    public WaybillConfig Waybill { get; set; } = new WaybillConfig();

    public Production Production { get; set; } = new Production();

    public ExchangeData ExchangeData { get; set; } = new ExchangeData();

    public BasicConfig BasicConfig { get; set; } = new BasicConfig();

    public Settings.GoodsSearchConfig GoodsSearch { get; set; } = new Settings.GoodsSearchConfig();

    public Settings.ClientsConfig Clients { get; set; } = new Settings.ClientsConfig();

    public Gbs.Core.Config.System System { get; set; } = new Gbs.Core.Config.System();

    public class ClientsConfig
    {
      public GlobalDictionaries.ClientSyncModes SyncMode { get; set; }

      public string FileSyncPath { get; set; }

      public GlobalDictionaries.ActionAuthType BonusesAuthType { get; set; }

      public GlobalDictionaries.ActionAuthType CreditAuthType { get; set; }
    }

    public class GoodsSearchConfig
    {
      public bool ClearQueryAfterAdd { get; set; }
    }

    public class ClientOrderConfig
    {
      public bool IsUnitePositions { get; set; } = true;
    }
  }
}
