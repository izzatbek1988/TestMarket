// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.RuOnlineKkmHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm
{
  public class RuOnlineKkmHelper
  {
    private Decimal RoundSum(Decimal sum, int decimals = 2, MidpointRounding rounding = MidpointRounding.AwayFromZero)
    {
      return Math.Round(sum, decimals, rounding);
    }

    public static void PrepareCertificatePayments(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      CheckGood[] checkGoodArray = new CheckGood[data.GoodsList.Count];
      data.GoodsList.CopyTo(checkGoodArray);
      foreach (CheckGood source in ((IEnumerable<CheckGood>) checkGoodArray).Where<CheckGood>((Func<CheckGood, bool>) (g => g.CertificateInfo.IsCertificate)))
      {
        if (source.CertificateInfo.Nominal < source.Price)
        {
          data.GoodsList.Remove(source);
          CheckGood checkGood1 = source.Clone<CheckGood>();
          checkGood1.Price = source.CertificateInfo.Nominal;
          checkGood1.RuFfdGoodTypeCode = GlobalDictionaries.RuFfdGoodsTypes.Payment;
          checkGood1.RuFfdPaymentModeCode = GlobalDictionaries.RuFfdPaymentModes.Prepayment;
          CheckGood checkGood2 = source.Clone<CheckGood>();
          checkGood2.Price = source.Price - source.CertificateInfo.Nominal;
          data.GoodsList.Add(checkGood1);
          data.GoodsList.Add(checkGood2);
        }
        else
        {
          source.RuFfdGoodTypeCode = GlobalDictionaries.RuFfdGoodsTypes.Payment;
          source.RuFfdPaymentModeCode = GlobalDictionaries.RuFfdPaymentModes.Prepayment;
        }
      }
    }

    public static string Base64Encode(string plainText)
    {
      return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
    }

    public static int GetMarkingCodeStatus(CheckGood item, CheckTypes type)
    {
      return item.Unit.RuFfdUnitsIndex != 0 ? (type != CheckTypes.Sale ? 4 : 2) : (type != CheckTypes.Sale ? 3 : 1);
    }
  }
}
