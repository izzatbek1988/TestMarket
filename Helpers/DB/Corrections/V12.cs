// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V12
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  internal class V12 : ICorrection
  {
    public bool Do()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<PAYMENT_METHODS> list = dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => x.KKM_METHOD == 1)).ToList<PAYMENT_METHODS>();
        AcquiringTerminal acquiringTerminal = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().AcquiringTerminal;
        foreach (PAYMENT_METHODS paymentMethods in list)
        {
          paymentMethods.TYPE_METHOD = acquiringTerminal.Type != 0 ? 1 : 0;
          dataBase.InsertOrReplace<PAYMENT_METHODS>(paymentMethods);
        }
        return true;
      }
    }
  }
}
