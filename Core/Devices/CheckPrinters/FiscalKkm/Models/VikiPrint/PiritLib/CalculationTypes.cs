// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.CalculationTypes
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public enum CalculationTypes
  {
    [EnumMember(Value = "PREPAYMENT_FULL")] FullPrepaid,
    [EnumMember(Value = "PREPAYMENT")] Prepaid,
    [EnumMember(Value = "ADVANCE")] Advance,
    [EnumMember(Value = "PAYMENT_FULL")] FullPayment,
    [EnumMember(Value = "CREDIT")] Credit,
    [EnumMember(Value = "CREDIT_FULL")] FullCredit,
    [EnumMember(Value = "CREDIT_PAYMENT")] PaymentCredit,
  }
}
