// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WebOffice.PaymentItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#nullable disable
namespace Gbs.Helpers.WebOffice
{
  public class PaymentItem
  {
    public Guid Uid { get; set; }

    public DateTime DateTime { get; set; }

    public Decimal SumIn { get; set; }

    public Decimal SumOut { get; set; }

    public PaymentItem.PaymentMethod Method { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    public GlobalDictionaries.PaymentTypes Type { get; set; }

    public PaymentItem(Payments.Payment payment)
    {
      this.Uid = payment.Uid;
      this.DateTime = payment.Date;
      this.SumIn = payment.SumIn;
      this.SumOut = payment.SumOut;
      this.Method = new PaymentItem.PaymentMethod(payment.Method);
      this.Type = payment.Type;
    }

    public class PaymentMethod
    {
      public Guid Uid { get; set; }

      public string Name { get; set; }

      public PaymentMethod(PaymentMethods.PaymentMethod method)
      {
        // ISSUE: explicit non-virtual call
        this.Uid = method != null ? __nonvirtual (method.Uid) : Guid.Empty;
        this.Name = method?.Name;
      }
    }
  }
}
