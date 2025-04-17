// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V10
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V10 : ICorrection
  {
    public bool Do()
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (devices.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
      {
        if (devices.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.HiPos, GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas))
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            List<Gbs.Core.Entities.Users.User> allItems = new UsersRepository(dataBase).GetAllItems();
            bool flag = devices.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.HiPos;
            foreach (Gbs.Core.Entities.Users.User user in allItems)
            {
              user.LoginForKkm = flag ? user.Alias : "1";
              user.PasswordForKkm = flag ? user.Password : "1";
            }
            new UsersRepository(dataBase).Save(allItems);
            return true;
          }
        }
      }
      return true;
    }
  }
}
