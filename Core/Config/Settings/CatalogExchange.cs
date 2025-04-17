// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.CatalogExchange
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Newtonsoft.Json;

#nullable disable
namespace Gbs.Core.Config
{
  public class CatalogExchange
  {
    public bool IsCatalogExchangeForAllPoint { get; set; }

    public CatalogExchange.LocalPath Local { get; set; } = new CatalogExchange.LocalPath();

    public CatalogExchange.FtpServer Ftp { get; set; } = new CatalogExchange.FtpServer();

    public class FtpServer
    {
      public bool IsSend { get; set; }

      public string Path { get; set; } = string.Empty;

      public string Time { get; set; } = "9:00, 12:00, 15:00";

      public GlobalDictionaries.Format Format { get; set; }

      public LanConnection Connection { get; set; } = new LanConnection();

      [JsonIgnore]
      public string LoginDecrypt => CryptoHelper.StringCrypter.Decrypt(this.Connection?.UserLogin);

      [JsonIgnore]
      public string PassDecrypt => CryptoHelper.StringCrypter.Decrypt(this.Connection?.Password);
    }

    public class LocalPath
    {
      public bool IsSend { get; set; }

      public string Path { get; set; } = ApplicationInfo.GetInstance().Paths.DataPath;

      public string Time { get; set; } = "9:00, 12:00, 15:00";

      public GlobalDictionaries.Format Format { get; set; }
    }
  }
}
