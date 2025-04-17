// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V15
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  internal class V15 : ICorrection
  {
    public bool Do()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<ENTITY_PROPERTIES_VALUES> table = dataBase.GetTable<ENTITY_PROPERTIES_VALUES>();
        Expression<Func<ENTITY_PROPERTIES_VALUES, bool>> predicate = (Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == GlobalDictionaries.CertificateReusableUid);
        foreach (ENTITY_PROPERTIES_VALUES propertiesValues1 in table.Where<ENTITY_PROPERTIES_VALUES>(predicate).ToList<ENTITY_PROPERTIES_VALUES>())
        {
          ENTITY_PROPERTIES_VALUES propertiesValues2 = propertiesValues1;
          string str;
          switch (propertiesValues1.CONTENT.ToString())
          {
            case "\"True\"":
              str = "true";
              break;
            case "\"False\"":
              str = "false";
              break;
            default:
              str = propertiesValues1.CONTENT;
              break;
          }
          propertiesValues2.CONTENT = str;
          dataBase.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(propertiesValues1);
        }
        return true;
      }
    }
  }
}
