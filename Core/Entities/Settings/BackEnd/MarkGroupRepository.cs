// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.BackEnd.MarkGroupRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities.Settings.BackEnd
{
  public class MarkGroupRepository
  {
    public List<MarkGroupSettings> GetGroups()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<SETTINGS> source = dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 150));
        if (!source.Any<SETTINGS>())
          return new List<MarkGroupSettings>();
        ParameterExpression parameterExpression;
        // ISSUE: method reference
        // ISSUE: method reference
        return source.Select<SETTINGS, MarkGroupSettings>(Expression.Lambda<Func<SETTINGS, MarkGroupSettings>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (JsonConvert.DeserializeObject)), (Expression) Expression.Call(JsonConvert.DeserializeObject(x.VAL), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (object.ToString)), Array.Empty<Expression>())), parameterExpression)).ToList<MarkGroupSettings>();
      }
    }

    public bool Save(List<MarkGroupSettings> items)
    {
      foreach (MarkGroupSettings markGroupSettings in items)
        new SettingsRepository().Save(new Setting()
        {
          Parameter = "MarkGroupSettings",
          Value = (object) markGroupSettings.ToJsonString(),
          Type = Types.MarkGroupSetting,
          EntityUid = markGroupSettings.Uid
        });
      return true;
    }
  }
}
