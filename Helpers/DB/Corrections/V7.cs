// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V7
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V7 : ICorrection
  {
    public bool Do()
    {
      using (DataBase dataBase1 = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Payments.Payment> paymentsList = Gbs.Core.Entities.Payments.GetPaymentsList();
        List<Document> allItems = new DocumentsRepository(dataBase1).GetAllItems();
        Other.ConsoleWrite(string.Format("total payments: {0}", (object) paymentsList.Count));
        foreach (Gbs.Core.Entities.Payments.Payment payment1 in paymentsList)
        {
          Gbs.Core.Entities.Payments.Payment payment = payment1;
          DataBase dataBase2 = dataBase1;
          PAYMENTS payments = new PAYMENTS();
          payments.UID = payment.Uid;
          PaymentMethods.PaymentMethod method = payment.Method;
          // ISSUE: explicit non-virtual call
          payments.METHOD_UID = method != null ? __nonvirtual (method.Uid) : Guid.Empty;
          PaymentsAccounts.PaymentsAccount accountIn = payment.AccountIn;
          // ISSUE: explicit non-virtual call
          payments.ACCOUNT_IN_UID = accountIn != null ? __nonvirtual (accountIn.Uid) : Guid.Empty;
          PaymentsAccounts.PaymentsAccount accountOut = payment.AccountOut;
          // ISSUE: explicit non-virtual call
          payments.ACCOUNT_OUT_UID = accountOut != null ? __nonvirtual (accountOut.Uid) : Guid.Empty;
          payments.DATE_TIME = payment.Date;
          payments.PARENT_UID = payment.ParentUid;
          payments.TYPE = (int) payment.Type;
          payments.SECTION_UID = allItems.FirstOrDefault<Document>((Func<Document, bool>) (x => x.Uid == payment.ParentUid))?.Section?.Uid ?? Guid.Empty;
          payments.SUM_IN = Math.Round(payment.SumIn, 4, MidpointRounding.AwayFromZero);
          payments.SUM_OUT = Math.Round(payment.SumOut, 4, MidpointRounding.AwayFromZero);
          Users.User user = payment.User;
          // ISSUE: explicit non-virtual call
          payments.USER_UID = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
          payments.COMMENT = payment.Comment;
          Client client = payment.Client;
          // ISSUE: explicit non-virtual call
          payments.PAYER_UID = client != null ? __nonvirtual (client.Uid) : Guid.Empty;
          payments.IS_DELETED = payment.IsDeleted;
          dataBase2.InsertOrReplace<PAYMENTS>(payments);
        }
        return true;
      }
    }
  }
}
