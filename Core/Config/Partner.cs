// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Partner
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

#nullable disable
namespace Gbs.Core.Config
{
  internal class Partner
  {
    private static Partner.Info _info;

    public static Partner.Info GetInfo()
    {
      if (Partner._info != null)
        return Partner._info;
      string path = ApplicationInfo.GetInstance().Paths.ApplicationPath + "\\\\partner.json";
      if (!File.Exists(path))
      {
        Partner._info = new Partner.Info();
        return Partner._info;
      }
      Partner._info = JsonConvert.DeserializeObject<Partner.Info>(File.ReadAllText(path));
      LogHelper.Debug("load partner uid: " + Partner._info.Uid.ToString());
      return Partner._info;
    }

    public class Info
    {
      public Guid Uid { get; set; } = Guid.Empty;

      public bool IsBlockRegion { get; set; }
    }
  }
}
