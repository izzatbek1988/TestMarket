// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.LicenseFile
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Helpers
{
  public class LicenseFile
  {
    public LicenseFile.LicenseInfo License { get; set; }

    public string Sign { get; set; }

    public class LicenseInfo
    {
      public string GbsId { get; set; }

      [JsonConverter(typeof (DateFormatConverter), new object[] {"yyyy-MM-dd"})]
      public DateTime TerminationDate { get; set; }

      public LicenseFile.LicenseInfo.ClientInfo Client { get; set; }

      public bool IsCancel { get; set; }

      public Guid Uid { get; set; }

      public class ClientInfo
      {
        public string Name { get; set; }

        public string Email { get; set; }

        public Guid Uid { get; set; }
      }
    }
  }
}
