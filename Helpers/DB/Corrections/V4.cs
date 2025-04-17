// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V4
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V4 : ICorrection
  {
    public bool Do()
    {
      try
      {
        LogHelper.OnBegin();
        using (DataBase dataBase = Data.GetDataBase())
        {
          foreach (ACTIONS_HISTORY actionsHistory in dataBase.GetTable<ACTIONS_HISTORY>().ToList<ACTIONS_HISTORY>())
          {
            ACTIONS_HISTORY action = actionsHistory;
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            dataBase.Connection.GetTable<ACTIONS_HISTORY>().Where<ACTIONS_HISTORY>((Expression<Func<ACTIONS_HISTORY, bool>>) (x => x.UID == action.UID)).Update<ACTIONS_HISTORY>(Expression.Lambda<Func<ACTIONS_HISTORY, ACTIONS_HISTORY>>((Expression) Expression.MemberInit(Expression.New(typeof (ACTIONS_HISTORY)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ACTIONS_HISTORY.set_DATA_BLOB)), )))); // Unable to render the statement
          }
        }
        LogHelper.OnEnd();
        return true;
      }
      catch (Exception ex)
      {
        string ошибкаКорректировкиБд = Translate.V4_Do_Ошибка_корректировки_БД;
        LogHelper.Error(ex, ошибкаКорректировкиБд);
        return false;
      }
    }
  }
}
