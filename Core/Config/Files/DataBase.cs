// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.DataBase
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Core.Config
{
  [Serializable]
  public class DataBase : IConfig
  {
    public DataBase.DbConnection Connection { get; set; } = new DataBase.DbConnection();

    public DataBase.DbBackUp BackUp { get; set; } = new DataBase.DbBackUp();

    public string GoodsImagesPath { get; set; } = ApplicationInfo.GetInstance().Paths.DataPath + "Images";

    public bool IsCompressDbStart { get; set; }

    public GlobalDictionaries.Mode ModeProgram { get; set; }

    public bool CorrectExit { get; set; } = true;

    public class DbConnection
    {
      private static string _password = CryptoHelper.StringCrypter.Encrypt("masterkey");
      private static string _login = CryptoHelper.StringCrypter.Encrypt("SYSDBA");
      private string _decryptedLogin = CryptoHelper.StringCrypter.Decrypt(DataBase.DbConnection._login);
      private string _decryptedPassword = CryptoHelper.StringCrypter.Decrypt(DataBase.DbConnection._password);

      public string Path { get; set; } = ApplicationInfo.GetInstance().Paths.DataPath + "main.fdb";

      public DataBase.DbConnection.ConnectionTypes ConnectionType { get; set; } = DataBase.DbConnection.ConnectionTypes.Local;

      public string ServerUrl { get; set; } = "localhost";

      public int ServerPort { get; set; } = 3060;

      [JsonIgnore]
      public string DecryptedLogin
      {
        get => this._decryptedLogin;
        set => DataBase.DbConnection._login = CryptoHelper.StringCrypter.Encrypt(value);
      }

      [JsonIgnore]
      public string DecryptedPassword
      {
        get => this._decryptedPassword;
        set => DataBase.DbConnection._password = CryptoHelper.StringCrypter.Encrypt(value);
      }

      [JsonProperty]
      private string Login
      {
        get => DataBase.DbConnection._login;
        set
        {
          DataBase.DbConnection._login = value;
          this._decryptedLogin = CryptoHelper.StringCrypter.Decrypt(DataBase.DbConnection._login);
        }
      }

      [JsonProperty]
      private string Password
      {
        get => DataBase.DbConnection._password;
        set
        {
          DataBase.DbConnection._password = value;
          this._decryptedPassword = CryptoHelper.StringCrypter.Decrypt(DataBase.DbConnection._password);
        }
      }

      public enum ConnectionTypes
      {
        NotSet,
        Local,
        NetWork,
      }
    }

    public class DbBackUp
    {
      public bool CreateDbInBackup { get; set; } = true;

      public bool CreateBackup { get; set; } = true;

      public string Path { get; set; } = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\" + (Vendor.GetConfig()?.ApplicationName ?? "GBS.Market6") + "_backups\\";

      public int StorageLifeDays { get; set; } = 30;

      public DataBase.DbBackUp.CreateBackupPeriods CreatePeriod { get; set; }

      public bool IsCreateOnStart { get; set; } = true;

      public bool IsCreateOnExit { get; set; } = true;

      public enum CreateBackupPeriods
      {
        None,
        AndEvery1Hour,
        AndEvery3Hours,
        AndEvery6Hours,
        AndEvery12Hours,
      }
    }
  }
}
