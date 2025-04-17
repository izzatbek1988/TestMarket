// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckData.CheckDataPreparer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckData
{
  public class CheckDataPreparer
  {
    private Gbs.Core.Config.Devices DeviceConfigForConnect { get; }

    [Obsolete("Необходимо заменить на CheckFactory")]
    public CheckDataPreparer()
    {
      this.DeviceConfigForConnect = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
    }

    public static Decimal RoundedSumForCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      RoundTotal roundTotals = new ConfigsRepository<Settings>().Get().Sales.RoundTotals;
      Decimal sum = data.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
      return !roundTotals.IsEnable || roundTotals.Coefficient == 0M ? sum : Math.Round(MathHelper.RoundToCoefficient(sum, roundTotals.Coefficient), 2, MidpointRounding.AwayFromZero);
    }

    public void PrepareCreditPayment(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      Decimal num1 = data.PaymentsList.Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
      Decimal num2 = data.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
      LogHelper.Debug(string.Format("Goods sum: {0}; totalPayments: {1}; check discount: {2}", (object) num2, (object) num1, (object) data.DiscountSum));
      Decimal num3 = num2 - num1 - data.DiscountSum;
      if (num3 <= 0M)
        return;
      data.PaymentsList.Add(new CheckPayment()
      {
        Method = GlobalDictionaries.KkmPaymentMethods.Credit,
        Sum = num3,
        Name = Translate.CheckDataPreparer_PrepareCreditPayment_Кредитом
      });
      GlobalDictionaries.RuFfdPaymentModes ruFfdPaymentModes = data.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (g => g.Sum)) == num3 ? GlobalDictionaries.RuFfdPaymentModes.FullCredit : GlobalDictionaries.RuFfdPaymentModes.PartPaymentAndCredit;
      foreach (CheckGood goods in data.GoodsList)
        goods.RuFfdPaymentModeCode = ruFfdPaymentModes;
    }
  }
}
