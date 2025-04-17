// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V16
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using System.ComponentModel;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V16 : ICorrection
  {
    [Localizable(false)]
    public bool Do()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        DataBaseHelper.ExecuteSqlRawCommand("ALTER TABLE ENTITY_PROPERTIES_VALUES ALTER CONTENT TYPE varchar(1000);\r\n", dataBase);
        return true;
      }
    }
  }
}
