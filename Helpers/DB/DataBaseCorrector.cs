// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.DataBaseCorrector
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Forms._shared;
using Gbs.Helpers.DB.Corrections;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers.DB
{
  public class DataBaseCorrector
  {
    public bool DoCorrection()
    {
      try
      {
        if (new ConfigsRepository<DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
        {
          LogHelper.Debug("В режиме дом/офис корректировка БД не выполняется");
          return true;
        }
        if (!LockDb.LockedDb())
          return false;
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DataBaseCorrector_Корректировка_базы_данных);
        bool flag = this.CorrectionLoop();
        progressBar.Close();
        LockDb.UnLockedDb();
        return flag;
      }
      catch
      {
        LockDb.UnLockedDb();
        ProgressBarHelper.Close();
        return false;
      }
    }

    public static int CorrectionMethodsCount()
    {
      try
      {
        Type type = typeof (ICorrection);
        int num = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (s => (IEnumerable<Type>) s.GetTypes())).Where<Type>((Func<Type, bool>) (p => type.IsAssignableFrom(p))).Count<Type>();
        LogHelper.Debug(string.Format("ICorrection class count: {0}", (object) num));
        return num;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        return 0;
      }
    }

    private bool CorrectionLoop()
    {
      int version1 = VersionDb.GetVersion();
      int num = version1;
      LogHelper.Trace(string.Format("Начинаем корректировку БД. Текущая версия БД: {0}, новая версия БД: {1}", (object) version1, (object) (num + 1)));
      bool flag1;
      switch (version1)
      {
        case 1:
          flag1 = new V1().Do();
          break;
        case 2:
          flag1 = new V2().Do();
          break;
        case 3:
          flag1 = new V3().Do();
          break;
        case 4:
          flag1 = new V4().Do();
          break;
        case 5:
          flag1 = new V5().Do();
          break;
        case 6:
          flag1 = new V6().Do();
          break;
        case 7:
          flag1 = new V7().Do();
          break;
        case 8:
          flag1 = new V8().Do();
          break;
        case 9:
          flag1 = new V9().Do();
          break;
        case 10:
          flag1 = new V10().Do();
          break;
        case 11:
          flag1 = new V11().Do();
          break;
        case 12:
          flag1 = new V12().Do();
          break;
        case 13:
          flag1 = new V13().Do();
          break;
        case 14:
          flag1 = new V14().Do();
          break;
        case 15:
          flag1 = new V15().Do();
          break;
        case 16:
          flag1 = new V16().Do();
          break;
        case 17:
          flag1 = new V17().Do();
          break;
        case 18:
          flag1 = new V18().Do();
          break;
        case 19:
          flag1 = new V19().Do();
          break;
        case 20:
          flag1 = new V20().Do();
          break;
        case 21:
          flag1 = new V21().Do();
          break;
        default:
          flag1 = false;
          break;
      }
      bool flag2 = flag1;
      if (flag2)
      {
        int version2 = num + 1;
        VersionDb.SetVersion(version2);
        if (version2 > version1)
          flag2 &= this.CorrectionLoop();
      }
      return flag2;
    }
  }
}
