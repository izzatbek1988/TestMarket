// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.CryptoConfig
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Core.Config
{
  public class CryptoConfig
  {
    [JsonProperty]
    private string Value { get; set; }

    [JsonIgnore]
    public string DecryptedValue
    {
      get => !this.Value.IsNullOrEmpty() ? CryptoHelper.StringCrypter.Decrypt(this.Value) : "";
      set => this.Value = CryptoHelper.StringCrypter.Encrypt(value);
    }

    public CryptoConfig()
    {
    }

    public CryptoConfig(string value) => this.DecryptedValue = value;
  }
}
