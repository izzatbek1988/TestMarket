// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.Payment
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.FR.BackEnd.Entities.Clients;
using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities
{
  public class Payment
  {
    public Decimal SumIn { get; set; }

    public Decimal SumOut { get; set; }

    public GlobalDictionaries.PaymentTypes Type { get; set; }

    public Guid ParentUid { get; set; } = Guid.Empty;

    public DateTime Date { get; set; } = DateTime.Now;

    public PaymentsAccount AccountOut { get; set; }

    public PaymentsAccount AccountIn { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string GroupInfo { get; set; }

    public Client Client { get; set; }

    public PaymentMethod Method { get; set; }

    public User User { get; set; }

    public Section Section { get; set; }

    public bool IsFiscal { get; set; }
  }
}
