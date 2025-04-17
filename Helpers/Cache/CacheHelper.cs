// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.CacheHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.BackgroundTasks;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Timers;

#nullable disable
namespace Gbs.Helpers
{
  public static class CacheHelper
  {
    private static MemoryCache Cache = new MemoryCache(Guid.NewGuid().ToString());
    private static readonly List<CacheHelper.ICacheManager> ManagersList = new List<CacheHelper.ICacheManager>();
    private static readonly Dictionary<CacheHelper.CacheTypes, Mutex> MutexDic = new Dictionary<CacheHelper.CacheTypes, Mutex>();

    private static void UpdateCaches()
    {
      CacheHelper.ManagersList.Where<CacheHelper.ICacheManager>((Func<CacheHelper.ICacheManager, bool>) (x => !x.IsOnUpdate && x.NextUpdateDateTime < DateTime.Now)).OrderBy<CacheHelper.ICacheManager, DateTime>((Func<CacheHelper.ICacheManager, DateTime>) (x => x.NextUpdateDateTime)).FirstOrDefault<CacheHelper.ICacheManager>()?.UpdateCache();
    }

    static CacheHelper()
    {
      foreach (CacheHelper.CacheTypes key in Enum.GetValues(typeof (CacheHelper.CacheTypes)))
        CacheHelper.MutexDic.Add(key, new Mutex());
      System.Timers.Timer timer = new System.Timers.Timer(30000.0);
      timer.AutoReset = true;
      timer.Elapsed += (ElapsedEventHandler) ((s, e) => CacheHelper.CheckCacheManagers());
      timer.Start();
    }

    private static void CheckCacheManagers()
    {
      if (GlobalData.IsMarket5ImportAcitve)
      {
        Other.ConsoleWrite("Кэш не обновляем при импорте");
      }
      else
      {
        if (!CacheHelper.ManagersList.Any<CacheHelper.ICacheManager>((Func<CacheHelper.ICacheManager, bool>) (x => !x.IsOnUpdate && x.NextUpdateDateTime < DateTime.Now)))
          return;
        BackgroundTasksHelper.AddTaskToQueue(new Action(CacheHelper.UpdateCaches), BackgroundTaskTypes.CheckUpdate);
      }
    }

    public static T Get<T>(CacheHelper.CacheTypes type, Func<T> loadFunc, T preloadedData = null) where T : class
    {
      Mutex mutex = (Mutex) null;
      try
      {
        string key = type.ToString();
        T dataFromCache1 = CacheHelper.GetDataFromCache<T>(key);
        if ((object) dataFromCache1 != null)
          return dataFromCache1;
        mutex = CacheHelper.MutexDic[type];
        mutex.WaitOne();
        T dataFromCache2 = CacheHelper.GetDataFromCache<T>(key);
        if ((object) dataFromCache2 != null)
          return dataFromCache2;
        CacheHelper.SetDataIntoCache<T>(type, loadFunc, preloadedData);
        return CacheHelper.GetDataFromCache<T>(key);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка получения данных из кэша");
        return loadFunc();
      }
      finally
      {
        mutex?.ReleaseMutex();
      }
    }

    public static void ClearAll()
    {
      LogHelper.Debug("Полная очистка кэша");
      foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) CacheHelper.Cache)
        CacheHelper.Cache.Remove(keyValuePair.Key, (string) null);
      CacheHelper.Cache.Trim(100);
      CacheHelper.Cache.Dispose();
      GC.Collect();
      CacheHelper.Cache = new MemoryCache(Guid.NewGuid().ToString());
      GC.WaitForPendingFinalizers();
      GC.Collect();
    }

    public static void Clear(CacheHelper.CacheTypes type)
    {
      string key = type.ToString();
      if (CacheHelper.Cache == null || !CacheHelper.Cache.Contains(key, (string) null))
        return;
      CacheHelper.Cache.Remove(key, (string) null);
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    public static void UpdateCacheAsync(CacheHelper.CacheTypes type, int pause = 15)
    {
      CacheHelper.ICacheManager cacheManager = CacheHelper.ManagersList.FirstOrDefault<CacheHelper.ICacheManager>((Func<CacheHelper.ICacheManager, bool>) (x => x.Type == type));
      if (cacheManager == null)
        return;
      cacheManager.NextUpdateDateTime = DateTime.Now.AddSeconds((double) pause);
    }

    public static void UpdateCache<T>(CacheHelper.CacheTypes type, T preloadedData)
    {
      CacheItemPolicy policy = new CacheItemPolicy()
      {
        SlidingExpiration = TimeSpan.FromHours(1.0)
      };
      CacheHelper.Cache.Add(type.ToString(), (object) preloadedData, policy);
    }

    private static void SetDataIntoCache<T>(
      CacheHelper.CacheTypes type,
      Func<T> loadFunc,
      T preloadedData = null)
      where T : class
    {
      Performancer performancer = new Performancer(string.Format("Загрузка данных в кэш, тип {0}", (object) type));
      CacheHelper.ICacheManager cacheManager = CacheHelper.ManagersList.FirstOrDefault<CacheHelper.ICacheManager>((Func<CacheHelper.ICacheManager, bool>) (x => x.Type == type));
      if (cacheManager == null)
      {
        cacheManager = (CacheHelper.ICacheManager) new CacheHelper.CacheManager<T>(type, loadFunc);
        if (type == CacheHelper.CacheTypes.ClientsCredits)
          cacheManager.UpdateInterval = TimeSpan.FromMinutes(20.0);
        CacheHelper.ManagersList.Add(cacheManager);
      }
      cacheManager.IsOnUpdate = true;
      string key = type.ToString();
      T obj = preloadedData ?? loadFunc();
      performancer.AddPoint("clear cache");
      CacheHelper.Clear(type);
      CacheItemPolicy policy = new CacheItemPolicy()
      {
        SlidingExpiration = TimeSpan.FromHours(1.0)
      };
      performancer.AddPoint("add cache");
      CacheHelper.Cache.Add(key, (object) obj, policy);
      cacheManager.LastUpdateTime = DateTime.Now;
      cacheManager.NextUpdateDateTime = DateTime.Now.Add(cacheManager.UpdateInterval);
      cacheManager.IsOnUpdate = false;
      performancer.Stop();
    }

    private static T GetDataFromCache<T>(string key) where T : class
    {
      return CacheHelper.Cache.Get(key, (string) null) as T;
    }

    public enum CacheTypes
    {
      CafeMenu,
      ClientForPolyCard,
      AllUsers,
      AllGoods,
      Clients,
      AllClients,
      AllUnits,
      ClientsCredits,
      DiscountRules,
      ExtraPriceRules,
      BuyPrices,
    }

    private interface ICacheManager
    {
      void UpdateCache();

      CacheHelper.CacheTypes Type { get; set; }

      bool IsOnUpdate { set; get; }

      TimeSpan UpdateInterval { set; get; }

      DateTime NextUpdateDateTime { get; set; }

      DateTime LastUpdateTime { get; set; }
    }

    private class CacheManager<T> : CacheHelper.ICacheManager where T : class
    {
      private readonly Func<T> _loadFunc;

      public CacheHelper.CacheTypes Type { get; set; }

      public bool IsOnUpdate { get; set; }

      public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromMinutes(5.0);

      public DateTime NextUpdateDateTime { get; set; } = DateTime.Now;

      public DateTime LastUpdateTime { get; set; }

      public CacheManager(CacheHelper.CacheTypes type, Func<T> loadFunc)
      {
        this.Type = type;
        this._loadFunc = loadFunc;
      }

      public void UpdateCache()
      {
        if (GlobalData.IsMarket5ImportAcitve)
          return;
        Mutex mutex = CacheHelper.MutexDic[this.Type];
        mutex.WaitOne();
        if (this.IsOnUpdate)
        {
          mutex.ReleaseMutex();
        }
        else
        {
          this.IsOnUpdate = true;
          try
          {
            Performancer performancer = new Performancer(string.Format("Обновление кэша с типом {0}", (object) this.Type));
            T preloadedData = this._loadFunc();
            CacheHelper.Clear(this.Type);
            CacheHelper.Get<T>(this.Type, this._loadFunc, preloadedData);
            performancer.Stop();
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Ошибка таймера обновления кэша. Кэш будет очищен");
            CacheHelper.Clear(this.Type);
          }
          finally
          {
            this.LastUpdateTime = DateTime.Now;
            this.NextUpdateDateTime = DateTime.Now.Add(this.UpdateInterval);
            this.IsOnUpdate = false;
            mutex.ReleaseMutex();
          }
        }
      }
    }
  }
}
