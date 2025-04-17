// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.Cache
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Timers;

#nullable disable
namespace Gbs.Core.Entities.Documents
{
  public static class Cache
  {
    private static List<Document> _cache = (List<Document>) null;
    private static readonly object _lock = new object();
    private static Timer _timer;
    private static bool _isUpdating = false;
    private const int CacheDays = 370;
    private static readonly TimeSpan CacheUpdateInterval = TimeSpan.FromMinutes(20.0);

    public static DateTime CacheStart { get; private set; }

    static Cache()
    {
      Cache._timer = new Timer();
      Cache._timer.Interval = Cache.CacheUpdateInterval.TotalMilliseconds;
      Cache._timer.Elapsed += new ElapsedEventHandler(Cache.TimerElapsed);
      Cache._timer.AutoReset = true;
      Cache._timer.Enabled = true;
    }

    private static void TimerElapsed(object sender, ElapsedEventArgs e)
    {
      if (Cache._isUpdating)
        return;
      TaskHelper.TaskRun(new Action(Cache.ReloadCache));
    }

    private static void ReloadCache()
    {
      Cache._isUpdating = true;
      List<Document> documentList = Cache.LoadDocumentsFromDb();
      lock (Cache._lock)
      {
        Cache.ClearCache();
        Cache._cache = documentList;
      }
      Cache._isUpdating = false;
    }

    private static List<Document> LoadDocumentsFromDb()
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.AddDays(-370.0);
      Cache.CacheStart = dateTime.Date;
      using (DataBase dataBase = Data.GetDataBase())
        return new DocumentsRepository(dataBase).GetByQuery(dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME >= Cache.CacheStart)));
    }

    private static void ClearCache()
    {
      lock (Cache._lock)
      {
        if (Cache._cache == null)
          return;
        Cache._cache.Clear();
        Cache._cache = (List<Document>) null;
        GC.Collect();
      }
    }

    public static List<Document> GetFromCache()
    {
      lock (Cache._lock)
      {
        if (Cache._cache == null)
          Cache.ReloadCache();
        List<Document> cache = Cache._cache;
        return cache != null ? cache.ToList<Document>() : (List<Document>) null;
      }
    }

    public static void UpdateInCache(Document document)
    {
      TaskHelper.TaskRun((Action) (() =>
      {
        lock (Cache._lock)
        {
          Cache.GetFromCache();
          if (Cache._cache.Any<Document>((Func<Document, bool>) (x => x.Uid == document.Uid)))
            Cache._cache.RemoveAll((Predicate<Document>) (x => x.Uid == document.Uid));
          Cache._cache.Add(document);
        }
      }), false);
    }

    public static void ClearAndLoadCache()
    {
      Cache.ClearCache();
      Cache.GetFromCache();
    }
  }
}
