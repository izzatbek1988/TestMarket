// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.Payment
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class Payment : Gbs.Core.Entities.Entity
  {
    public Decimal SumIn { get; set; }

    public Decimal SumOut { get; set; }

    [Required]
    public GlobalDictionaries.PaymentTypes Type { get; set; }

    public Guid ParentUid { get; set; } = Guid.Empty;

    [Required]
    public DateTime Date { get; set; } = DateTime.Now;

    public Guid AccountOutUid { get; set; }

    public Guid AccountInUid { get; set; }

    public string Comment { get; set; } = string.Empty;

    public Guid ClientUid { get; set; }

    public Guid MethodUid { get; set; }

    public Guid UserUid { get; set; }

    public bool IsFiscal { get; set; }

    public Payment(Payments.Payment item)
    {
      this.Uid = item.Uid;
      this.IsDeleted = item.IsDeleted;
      this.SumIn = item.SumIn;
      this.SumOut = item.SumOut;
      this.Type = item.Type;
      this.ParentUid = item.ParentUid;
      this.Date = item.Date;
      PaymentsAccounts.PaymentsAccount accountOut = item.AccountOut;
      // ISSUE: explicit non-virtual call
      this.AccountOutUid = accountOut != null ? __nonvirtual (accountOut.Uid) : Guid.Empty;
      this.Comment = item.Comment;
      PaymentsAccounts.PaymentsAccount accountIn = item.AccountIn;
      // ISSUE: explicit non-virtual call
      this.AccountInUid = accountIn != null ? __nonvirtual (accountIn.Uid) : Guid.Empty;
      Client client = item.Client;
      // ISSUE: explicit non-virtual call
      this.ClientUid = client != null ? __nonvirtual (client.Uid) : Guid.Empty;
      PaymentMethods.PaymentMethod method = item.Method;
      // ISSUE: explicit non-virtual call
      this.MethodUid = method != null ? __nonvirtual (method.Uid) : Guid.Empty;
      Users.User user = item.User;
      // ISSUE: explicit non-virtual call
      this.UserUid = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
      this.IsFiscal = item.IsFiscal;
    }

    public Payment()
    {
    }
  }
}
