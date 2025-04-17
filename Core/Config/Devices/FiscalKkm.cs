// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.FiscalKkm
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class FiscalKkm
  {
    public bool IsShowNotificationForCheck { get; set; } = true;

    public bool IsAlwaysSendDigitalCheck { get; set; }

    public bool IsAlwaysNoPrintCheck { get; set; }

    public bool IsSaveInfoClient { get; set; }

    public bool IsNoPrintCheckIfSendDigitalCheck { get; set; }

    public string AzSmartMerchantId { get; set; }

    public string Atol10ConnectionConfig { get; set; }

    public string Model { get; set; }

    public GlobalDictionaries.Devices.FiscalKkmTypes KkmType { get; set; }

    public GlobalDictionaries.RuTaxSystems DefaultRuTaxSystem { get; set; } = GlobalDictionaries.RuTaxSystems.None;

    public int DefaultTaxRate { get; set; } = 1;

    public GlobalDictionaries.Devices.FfdVersions FfdVersion { get; set; } = GlobalDictionaries.Devices.FfdVersions.Ffd110;

    public bool IsLetNonFiscal { get; set; }

    public bool IsFreeKkmPort { get; set; } = true;

    public bool AllowSalesWithoutCheck { get; set; }

    public bool SendBuyerInfoToCheck { get; set; }

    public bool IsNoSendDigitalCheck { get; set; } = true;

    public Dictionary<int, FiscalKkm.TaxRate> TaxRates { get; set; } = new Dictionary<int, FiscalKkm.TaxRate>()
    {
      {
        1,
        new FiscalKkm.TaxRate(-1M, Translate.FiscalKkm_БЕЗ_НДС, 1)
      },
      {
        2,
        new FiscalKkm.TaxRate(0M, Translate.НДС0, 2)
      },
      {
        3,
        new FiscalKkm.TaxRate(10M, Translate.НДС10, 3)
      },
      {
        4,
        new FiscalKkm.TaxRate(20M, Translate.НДС20, 4)
      },
      {
        5,
        new FiscalKkm.TaxRate(10M, Translate.НДС10110, 5)
      },
      {
        6,
        new FiscalKkm.TaxRate(20M, Translate.НДС20120, 6)
      },
      {
        7,
        new FiscalKkm.TaxRate(5M, Translate.FiscalKkm_TaxRates_НДС_5_, 7)
      },
      {
        8,
        new FiscalKkm.TaxRate(7M, Translate.FiscalKkm_TaxRates_НДС_7_, 8)
      },
      {
        9,
        new FiscalKkm.TaxRate(5M, Translate.FiscalKkm_TaxRates_НДС_5_105, 9)
      },
      {
        10,
        new FiscalKkm.TaxRate(7M, Translate.FiscalKkm_TaxRates_НДС_7_107, 10)
      }
    };

    public Dictionary<GlobalDictionaries.KkmPaymentMethods, int> PaymentsMethods { get; set; } = new Dictionary<GlobalDictionaries.KkmPaymentMethods, int>()
    {
      {
        GlobalDictionaries.KkmPaymentMethods.Cash,
        0
      },
      {
        GlobalDictionaries.KkmPaymentMethods.Card,
        1
      },
      {
        GlobalDictionaries.KkmPaymentMethods.Bank,
        2
      },
      {
        GlobalDictionaries.KkmPaymentMethods.Credit,
        3
      },
      {
        GlobalDictionaries.KkmPaymentMethods.PrePayment,
        4
      }
    };

    public class TaxRate
    {
      public Decimal TaxValue { get; set; }

      public string Name { get; set; }

      public int KkmIndex { get; set; }

      public TaxRate(Decimal value, string name, int index)
      {
        this.TaxValue = value;
        this.Name = name;
        this.KkmIndex = index;
      }
    }
  }
}
