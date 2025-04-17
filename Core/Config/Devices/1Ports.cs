// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.LanConnection
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;

#nullable disable
namespace Gbs.Core.Config
{
  public class LanConnection
  {
    public string UrlAddress { get; set; } = string.Empty;

    public int? PortNumber { get; set; }

    public string UserLogin { get; set; } = string.Empty;

    [JsonProperty]
    private string UserPassword { get; set; } = string.Empty;

    [JsonProperty]
    private CryptoConfig CryptoPassword { get; set; }

    [JsonIgnore]
    public string Password
    {
      get
      {
        if (this.CryptoPassword == null)
        {
          this.CryptoPassword = new CryptoConfig(this.UserPassword);
          this.UserPassword = string.Empty;
        }
        return this.CryptoPassword.DecryptedValue;
      }
      set
      {
        this.UserPassword = string.Empty;
        this.CryptoPassword = new CryptoConfig(value);
      }
    }
  }
}
