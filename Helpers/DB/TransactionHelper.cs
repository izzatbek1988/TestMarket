// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.TransactionHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FirebirdSql.Data.FirebirdClient;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Threading;

#nullable disable
namespace Gbs.Helpers.DB
{
  public static class TransactionHelper
  {
    public static void RunActionWithDbTransaction(
      Action action,
      DataContextTransaction transaction,
      int tryCount = 0)
    {
      if (tryCount > 10)
        throw new Exception(Translate.TransactionHelper_RunActionWithDbTransaction_Ошибка_выполнения_задачи_со_связанной_транзакцией);
      try
      {
        action();
        transaction.CommitTransaction();
      }
      catch (FbException ex) when (ex.Message.ToLower().StartsWith("deadlock"))
      {
        LogHelper.WriteError((Exception) ex, "Ошибка коммита транзакции", false);
        ++tryCount;
        transaction.RollbackTransaction();
        Thread.Sleep(new Random().Next(100) * tryCount);
        TransactionHelper.RunActionWithDbTransaction(action, transaction, tryCount);
      }
    }
  }
}
