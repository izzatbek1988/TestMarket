// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.GoodCalculationTypes
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public enum GoodCalculationTypes
  {
    [EnumMember(Value = "GOODS")] Goods,
    [EnumMember(Value = "EXCISABLE")] Excisable,
    [EnumMember(Value = "JOB")] Job,
    [EnumMember(Value = "SERVICE")] Service,
    [EnumMember(Value = "GAMBLING_RATE")] GamblingRate,
    [EnumMember(Value = "GAMBLING_WINNINGS")] GamblingWinnings,
    [EnumMember(Value = "LOTTERY_TICKET")] LotteryTicket,
    [EnumMember(Value = "LOTTERY_WINNINGS")] LotteryWinnings,
    [EnumMember(Value = "RID")] Rid,
    [EnumMember(Value = "PAYMENT")] Payment,
    [EnumMember(Value = "AGENCY_REWARDS")] AgencyRewards,
    [EnumMember(Value = "ANOTHER")] Another,
    [EnumMember(Value = "PROPERTY_LAW")] PropertyLaw,
    [EnumMember(Value = "UNREAL_INCOME")] UnrealIncome,
    [EnumMember(Value = "INSURANCE_CONTR")] InsuranceContr,
    [EnumMember(Value = "TRADE_FEE")] TradeFee,
    [EnumMember(Value = "RESORT_FEE")] ResortFee,
    [EnumMember(Value = "DEPOSIT")] Deposit,
    [EnumMember(Value = "AMOUNT_OF_EXPENSES")] AmountOfExpenses,
    [EnumMember(Value = "PENSION_INSURANCE_NO_INDIVIDUAL")] PensionInsuranceNoIndividual,
    [EnumMember(Value = "PENSION_INSURANCE_INDIVIDUAL")] PensionInsuranceIndividual,
    [EnumMember(Value = "HEALTH_INSURANCE_NO_INDIVIDUAL")] HealthInsuranceNoIndividual,
    [EnumMember(Value = "HEALTH_INSURANCE_INDIVIDUAL")] HealthInsuranceIndividual,
    [EnumMember(Value = "SOCIAL_INSURANCE")] SocialInsurance,
    [EnumMember(Value = "CASINO")] Casino,
    [EnumMember(Value = "BANK_PAYING_AGENT")] BankPayingAgent,
    [EnumMember(Value = "EXCISABLE_NO_CODE_MARK")] ExcisableNoCodeMark,
    [EnumMember(Value = "EXCISABLE_CODE_MARK")] ExcisableCodeMark,
    [EnumMember(Value = "GOODS_NO_CODE_MARK")] GoodsNoCodeMark,
    [EnumMember(Value = "GOODS_CODE_MARK")] GoodsCodeMark,
  }
}
