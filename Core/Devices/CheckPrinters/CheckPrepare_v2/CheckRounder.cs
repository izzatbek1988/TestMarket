// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2.CheckRounder
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2
{
  public class CheckRounder
  {
    public List<CheckGood> UndiscountCheckItemsForOnlineKkm(List<CheckGood> list)
    {
      List<CheckGood> checkGoodList = new List<CheckGood>();
      foreach (CheckGood checkGood1 in list)
      {
        Decimal num = Math.Round(checkGood1.DiscountSum / checkGood1.Quantity, 2, MidpointRounding.AwayFromZero);
        Decimal price = checkGood1.Price - num;
        CheckGood checkGood2 = new CheckGood(checkGood1.Good, price, 0M, checkGood1.Quantity, checkGood1.Description, checkGood1.Name)
        {
          Uid = checkGood1.Uid,
          CertificateInfo = checkGood1.CertificateInfo
        };
        checkGoodList.Add(checkGood2);
      }
      return checkGoodList;
    }

    public void SplitSumForItems(List<CheckGood> checkItems, Decimal sumToSplit)
    {
      if (sumToSplit == 0M || sumToSplit < -0.1M)
        return;
      LogHelper.OnBegin("Размытие суммы округления по позициям в чеке");
      Decimal num1 = checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
      Decimal num2 = num1 - sumToSplit;
      LogHelper.Trace(string.Format("need: {0}; split: {1}; totalItemsSum: {2}", (object) num2, (object) sumToSplit, (object) num1));
      foreach (CheckGood checkItem in checkItems)
      {
        Decimal num3 = Math.Floor(sumToSplit * (checkItem.Price / num1) * 100M) / 100M;
        checkItem.Price -= num3;
      }
      sumToSplit = checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) - num2;
      LogHelper.Trace(string.Format("После равномерного размытия. Sum to split: {0}; items total sum: {1}", (object) sumToSplit, (object) checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum))));
      if (sumToSplit == 0M)
      {
        LogHelper.Trace("Сумма размытия равна нулю. Завершено");
      }
      else
      {
        while (Math.Abs(sumToSplit) >= 0.01M && checkItems.Any<CheckGood>((Func<CheckGood, bool>) (x => x.Price > 0.01M && x.Quantity == 1M)))
        {
          foreach (CheckGood checkGood in checkItems.Where<CheckGood>((Func<CheckGood, bool>) (x => x.Quantity == 1M)))
          {
            if (sumToSplit > 0M)
            {
              if (checkGood.Price > 0.01M)
              {
                checkGood.Price -= 0.01M;
                sumToSplit -= 0.01M;
              }
            }
            else
            {
              checkGood.Price += 0.01M;
              sumToSplit += 0.01M;
            }
            if (Math.Abs(sumToSplit) < 0.01M)
              break;
          }
        }
        LogHelper.Trace(string.Format("После размытия по штучным. Sum to split: {0}; items total sum: {1}", (object) sumToSplit, (object) checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum))));
        if (sumToSplit == 0M)
        {
          LogHelper.Trace("Сумма размытия равна нулю. Завершено");
        }
        else
        {
          foreach (CheckGood checkItem in checkItems)
          {
            sumToSplit = checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) - num2;
            Other.ConsoleWrite(string.Format("split: {0}; needSum: {1}", (object) sumToSplit, (object) num2));
            int num4 = sumToSplit > 0M ? 1 : -1;
            if (!(0.01M * checkItem.Quantity > Math.Abs(sumToSplit)))
              checkItem.Price -= 0.01M * (Decimal) num4;
          }
          sumToSplit = checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) - num2;
          LogHelper.Trace(string.Format("После размытия по копейке. Sum to split: {0}; items total sum: {1}", (object) sumToSplit, (object) checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum))));
          if (sumToSplit == 0M)
          {
            LogHelper.Trace("Сумма размытия равна нулю. Завершено");
          }
          else
          {
            List<CheckGood> collection = new List<CheckGood>();
            foreach (CheckGood checkItem in checkItems)
            {
              if (checkItem.Quantity > 1M && checkItem.Quantity == Math.Floor(checkItem.Quantity))
              {
                if (!checkItem.Good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Weight, GlobalDictionaries.GoodTypes.Certificate))
                {
                  for (int index = 0; (Decimal) index < checkItem.Quantity; ++index)
                  {
                    Decimal price1 = checkItem.Price;
                    if (sumToSplit > 0M)
                    {
                      Decimal price2 = checkItem.Price - 0.01M;
                      sumToSplit -= 0.01M;
                      CheckGood checkGood = new CheckGood(checkItem.Good, price2, checkItem.Discount, 1M, checkItem.Description, checkItem.Name)
                      {
                        Uid = checkItem.Uid
                      };
                      collection.Add(checkGood);
                    }
                    else
                    {
                      Decimal quantity = checkItem.Quantity - (Decimal) index;
                      CheckGood checkGood = new CheckGood(checkItem.Good, price1, checkItem.Discount, quantity, checkItem.Description, checkItem.Name)
                      {
                        Uid = checkItem.Uid
                      };
                      collection.Add(checkGood);
                      break;
                    }
                  }
                  continue;
                }
              }
              CheckGood checkGood1 = new CheckGood(checkItem.Good, checkItem.Price, checkItem.Discount, checkItem.Quantity, checkItem.Description, checkItem.Name)
              {
                Uid = checkItem.Uid,
                CertificateInfo = checkItem.CertificateInfo
              };
              collection.Add(checkGood1);
            }
            checkItems.Clear();
            checkItems.AddRange((IEnumerable<CheckGood>) collection);
            LogHelper.Trace(string.Format("Размытие завершено. Sum to split: {0}; items total sum: {1}", (object) sumToSplit, (object) checkItems.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum))));
          }
        }
      }
    }
  }
}
