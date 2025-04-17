// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.VendorConfig
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class VendorConfig
  {
    [JsonProperty("CodeName")]
    public string CodeName { get; set; } = "gbsmarket";

    [JsonProperty("Country")]
    public GlobalDictionaries.Countries Country { get; set; }

    [Obsolete("Имеет смысл перейти на свойство Languages")]
    [JsonProperty("Language")]
    public GlobalDictionaries.Languages? Language { get; set; }

    [JsonProperty("Languages")]
    public List<GlobalDictionaries.Languages> Languages { get; set; }

    [JsonProperty("ApplicationName")]
    public string ApplicationName { get; set; } = "GBS.Market";

    [JsonProperty("Logo")]
    public VendorConfig.LogoInfo Logo { get; set; }

    [JsonProperty("UpdateBranchPrefix")]
    public string UpdateBranchPrefix { get; set; } = string.Empty;

    public List<VendorConfig.ContactItem> Contacts { get; set; }

    public List<VendorConfig.LinkItem> Links { get; set; }

    public class ContactItem
    {
      public VendorConfig.ContactItem.Types Type { get; set; }

      public string Name { get; set; }

      public string Value { get; set; }

      [JsonConverter(typeof (StringEnumConverter))]
      public enum Types
      {
        Email,
        Phone,
        Other,
      }
    }

    public class LinkItem
    {
      public VendorConfig.LinkItem.Types Type { get; set; }

      public string Value { get; set; }

      [JsonConverter(typeof (StringEnumConverter))]
      public enum Types
      {
        Buy,
        KB,
        Main,
        Support,
      }
    }

    public class LogoInfo
    {
      [JsonProperty("LogoHash")]
      public string LogoHash { get; set; }

      [JsonProperty("ForegroundColor")]
      public string ForegroundColor { get; set; } = "Black";
    }
  }
}
