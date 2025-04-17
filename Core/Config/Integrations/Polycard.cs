// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Polycard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Core.Config
{
  public class Polycard
  {
    public bool IsActive { get; set; }

    public int Port { get; set; } = 2020;

    public Guid GroupContactUid { get; set; }

    private string _decryptedLogin
    {
      get
      {
        return !this.Login.IsNullOrEmpty() ? CryptoHelper.StringCrypter.Decrypt(this.Login) : "polycard";
      }
    }

    private string _decryptedPassword
    {
      get
      {
        return !this.Password.IsNullOrEmpty() ? CryptoHelper.StringCrypter.Decrypt(this.Password) : "market";
      }
    }

    [JsonProperty]
    private string Login { get; set; }

    [JsonIgnore]
    public string DecryptedLogin
    {
      get => this._decryptedLogin;
      set => this.Login = CryptoHelper.StringCrypter.Encrypt(value);
    }

    [JsonProperty]
    private string Password { get; set; }

    [JsonIgnore]
    public string DecryptedPassword
    {
      get => this._decryptedPassword;
      set => this.Password = CryptoHelper.StringCrypter.Encrypt(value);
    }
  }
}
