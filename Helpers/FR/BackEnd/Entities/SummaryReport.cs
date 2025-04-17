// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.SummaryReport
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities
{
  public class SummaryReport
  {
    public DateTime DateStart { get; set; }

    public DateTime DateFinish { get; set; }

    public Decimal TotalSalesCount { get; set; }

    public Decimal TotalGoods { get; set; }

    public Decimal TotalSaleSum { get; set; }

    public Decimal TotalReturnCount { get; set; }

    public Decimal TotalReturnsSum { get; set; }

    public Decimal SumCash { get; set; }

    public Decimal IncomeSum { get; set; }

    public Decimal DiscountsSum { get; set; }

    public Decimal MoneyOutcomeSum { get; set; }

    public Decimal MoneyIncomeSum { get; set; }

    public List<CheckPayment> Payments { get; set; } = new List<CheckPayment>();

    public Decimal TotalCreditSum { get; set; }

    public Decimal CreditPaymentsSum { get; set; }
  }
}
