// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V21
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers.Logging;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V21 : ICorrection
  {
    public bool Do()
    {
      if (Sections.GetSectionsList().Count<Sections.Section>((Func<Sections.Section, bool>) (x => !x.IsDeleted)) > 1)
      {
        LogHelper.Debug("Не выполняем сброс канала обновлений, т.к. больше 1 секции");
        return true;
      }
      int num = new Random().Next(1, 101);
      ConfigsRepository<Settings> configsRepository = new ConfigsRepository<Settings>();
      Settings config = configsRepository.Get();
      if (num > 25 && config.Other.UpdateConfig.VersionUpdate == GlobalDictionaries.VersionUpdate.Beta)
      {
        config.Other.UpdateConfig.VersionUpdate = GlobalDictionaries.VersionUpdate.Release;
        configsRepository.Save(config);
        LogHelper.Debug("Сброшен канал обновления до релиза");
      }
      return true;
    }
  }
}
