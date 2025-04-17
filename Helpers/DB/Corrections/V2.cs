// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V2
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V2 : ICorrection
  {
    private bool FixUncorrectedSales()
    {
      try
      {
        LogHelper.Debug("Корректировка БД: корректировка продаж");
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<Document> list = new DocumentsRepository(dataBase).GetActiveItems().ToList<Document>();
          IEnumerable<Document> documents = list.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale));
          bool flag1 = true;
          int num1 = 0;
          Decimal num2 = 0.0M;
          int num3 = 0;
          Decimal num4 = 0.0M;
          foreach (Document doc in documents)
          {
            Decimal creditSumForDocuments = ClientsRepository.GetCreditSumForDocuments(doc, list);
            if (!(creditSumForDocuments > -1M))
            {
              Decimal num5 = -creditSumForDocuments;
              Payments.Payment payment = new Payments.Payment();
              payment.Comment = "DB correction";
              payment.IsFiscal = false;
              payment.Date = doc.DateTime;
              payment.ParentUid = doc.Uid;
              payment.IsDeleted = false;
              payment.SumOut = num5;
              payment.Type = GlobalDictionaries.PaymentTypes.MoneyDocumentPayment;
              bool flag2 = payment.Save();
              if (doc.ContractorUid != Guid.Empty)
              {
                ++num3;
                num4 += num5;
              }
              Other.ConsoleWrite(string.Format("Корректировка продажи {0} на сумму: {1:N2}; result: {2}", (object) doc.Number, (object) num5, (object) flag2));
              flag1 &= flag2;
              if (flag2)
              {
                ++num1;
                num2 += num5;
              }
            }
          }
          Other.ConsoleWrite(string.Format("Скорректировано продаж: {0}; на сумму: {1:N2};", (object) num1, (object) num2));
          Other.ConsoleWrite(string.Format("Скорректировано с покупателями: {0}; на сумму: {1:N2};", (object) num3, (object) num4));
          LogHelper.Debug("Скорректировано продаж: " + num1.ToString());
          return flag1;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка корректировки продаж", false);
        return false;
      }
    }

    public bool Do() => this.FixUncorrectedSales();
  }
}
