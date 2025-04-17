// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Licenses.FreeLicense
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers.Cache;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Licenses
{
  internal class FreeLicense
  {
    private const int maxUsers = 1;
    private const int maxGoods = 200;
    private const int maxClients = 100;
    private static bool _isAllowFreeWork = false;
    private static DateTime _lastUpdate = DateTime.MinValue;

    public void UpdateInfo()
    {
      FreeLicense._lastUpdate = DateTime.Now;
      Settings settings = new ConfigsRepository<Settings>().Get();
      DataBase dataBase = new ConfigsRepository<DataBase>().Get();
      if (settings.Interface.Country != GlobalDictionaries.Countries.Russia)
        FreeLicense._isAllowFreeWork = false;
      else if (dataBase.ModeProgram == GlobalDictionaries.Mode.Home)
      {
        FreeLicense._isAllowFreeWork = false;
      }
      else
      {
        string serverUrl = dataBase.Connection.ServerUrl;
        if (!serverUrl.Contains("localhost") && !serverUrl.Contains("127.0.0.1"))
          FreeLicense._isAllowFreeWork = false;
        else if (CachesBox.AllUsers().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsKicked && !x.IsDeleted)).Count<Gbs.Core.Entities.Users.User>() > 1)
          FreeLicense._isAllowFreeWork = false;
        else if (CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted)).Count<Gbs.Core.Entities.Goods.Good>() > 200)
        {
          FreeLicense._isAllowFreeWork = false;
        }
        else
        {
          if (CachesBox.AllClients().Where<Client>((Func<Client, bool>) (x => !x.IsDeleted)).Count<Client>() <= 100)
            return;
          FreeLicense._isAllowFreeWork = false;
        }
      }
    }

    public bool IsAllowFreeWork()
    {
      if ((DateTime.Now - FreeLicense._lastUpdate).TotalHours < 12.0)
        return FreeLicense._isAllowFreeWork;
      this.UpdateInfo();
      return FreeLicense._isAllowFreeWork;
    }
  }
}
