// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.LicenseHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Helpers.Licenses;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Gbs.Helpers
{
  public class LicenseHelper
  {
    public const string keyFileName = "key.id";

    public LicenseHelper.LicenseTypes GetLicenseType() => throw new NotImplementedException();

    private static string GetGbsId() => GbsIdHelperMain.GetGbsId();

    private static string PublicKey { get; set; } = "<RSAKeyValue><Modulus>nIx8YHmsILeI4ktSLWFR/RkcVo61hz3yERzNWeY0T2bcqlPJ4yXz8ze0kkWCJEBTzxndAC7grtwFOWyKa+8w6O1z63vunjOoF9d7VbR5HNEpS/V1PWz14JYTraacPA1D+HrfhM571dX4Ge3fOl2QzdOxPT/aL1NsCKCFVRR58Cs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

    public static bool DownloadFromServer()
    {
      LicenseHelper.GetGbsId();
      string str = Path.Combine(ApplicationInfo.GetInstance().Paths.DataPath, "temp.gbs6");
      new WebAppKeysHelper().DownLoadKeyFile(str);
      LicenseHelper.LicenseInformation infoFromFile = LicenseHelper.GetInfoFromFile(str);
      DevelopersHelper.LogTrace("key file: " + infoFromFile.ToJsonString(true));
      if (infoFromFile.GbsId != LicenseHelper.GetGbsId())
        throw new InvalidOperationException();
      string destinationFile = Path.Combine(ApplicationInfo.GetInstance().Paths.DataPath, "key.id");
      if (!FileSystemHelper.MoveFile(str, destinationFile))
        throw new IOException("");
      MainWindowViewModel.CachedLicenseInformation = LicenseHelper.GetInfo();
      LogHelper.Debug("Файл ключа успешно загружен");
      return true;
    }

    private static LicenseHelper.LicenseInformation GetInfoFromFile(string path)
    {
      if (!File.Exists(path))
        throw new FileNotFoundException();
      if (File.ReadAllText(path).StartsWith("<gbskey_v2>"))
      {
        LogHelper.Trace("Файл лицензии v.2");
        return new WebAppKeysHelper().GetInfoFromFile(path);
      }
      string str = File.ReadAllText(path);
      LicenseFile licenseFile = str.StartsWith("<gbskey>") && str.EndsWith("</gbskey>") ? JsonConvert.DeserializeObject<LicenseFile>(CryptoHelper.StringCrypter.Decrypt(str.Replace("<gbskey>", string.Empty).Replace("</gbskey>", string.Empty)), new JsonSerializerSettings()
      {
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        DateParseHandling = DateParseHandling.DateTimeOffset,
        DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
      }) : throw new FileFormatException();
      if (CryptoHelper.GetMd5Hash((licenseFile.License.GbsId + licenseFile.License.TerminationDate.ToString("dd.MM.yyyy")).ToLower()) != LicenseHelper.DecryptSign(licenseFile.Sign))
        throw new InvalidOperationException();
      return new LicenseHelper.LicenseInformation()
      {
        GbsId = licenseFile.License.GbsId,
        KeyDateEnd = licenseFile.License.TerminationDate
      };
    }

    private static void RenameOldKeyFile()
    {
      string dataPath = ApplicationInfo.GetInstance().Paths.DataPath;
      string str = Path.Combine(dataPath, "key.gbs6");
      string destinationFile = Path.Combine(dataPath, "key.id");
      if (!File.Exists(str))
        return;
      FileSystemHelper.MoveFile(str, destinationFile);
    }

    public static LicenseHelper.LicenseInformation GetInfo()
    {
      LicenseHelper.LicenseInformation info = new LicenseHelper.LicenseInformation()
      {
        GbsId = LicenseHelper.GetGbsId()
      };
      try
      {
        string dataPath = ApplicationInfo.GetInstance().Paths.DataPath;
        LicenseHelper.RenameOldKeyFile();
        LicenseHelper.LicenseInformation infoFromFile = LicenseHelper.GetInfoFromFile(Path.Combine(dataPath, "key.id"));
        if (infoFromFile == null)
          throw new InvalidOperationException("li is null");
        if (LicenseHelper.GetGbsId() != infoFromFile.GbsId)
          throw new InvalidOperationException("ids is not equal");
        DevelopersHelper.LogTrace("license info: " + infoFromFile.ToJsonString());
        return infoFromFile;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка получения информации о лицензии");
        return info;
      }
    }

    private static byte[] HexToBytes(string hexStr)
    {
      int length = hexStr.Length;
      byte[] bytes = new byte[length / 2];
      for (int startIndex = 0; startIndex < length; startIndex += 2)
        bytes[startIndex / 2] = Convert.ToByte(hexStr.Substring(startIndex, 2), 16);
      return bytes;
    }

    private static string DecryptSign(string signText)
    {
      RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
      rsa.FromXmlString(LicenseHelper.PublicKey);
      return Encoding.UTF8.GetString(rsa.PublicDecryption(LicenseHelper.HexToBytes(signText)));
    }

    public static void RestartWmi()
    {
      try
      {
        FileSystemHelper.RunBat(LicenseHelper.GetBatFileContent(), true);
        int num = (int) MessageBoxHelper.Show("Команда успешно выполнена, перезагрузите устройство и проверьте работу программы еще раз.");
        System.Environment.Exit(0);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось выполнить BAT-файл для восстановления WMI");
        MessageBoxHelper.Warning("Не удалось автоматически восстановить службу WMI для получения ID программы. Обратитесь в техническую поддержку.");
      }
    }

    [Localizable(false)]
    private static string GetBatFileContent()
    {
      return "@echo off\nwinmgmt /verifyrepository\nnet stop Winmgmt\nnet start Winmgmt\nwinmgmt /resetrepository\nwinmgmt /salvagerepository\necho Done.\npause\n";
    }

    public class LicenseInformation
    {
      private DateTime _keyDateEnd = DateTime.Today.AddDays(-3.0);
      public int MaxDocumentsForDemo = 2000;
      public int MaxSectionsForDemo = 3;
      private static LicenseHelper.LicenseInformation.DocsQtyCache _docsQtyCache;

      public string GbsId { get; set; }

      public DateTime KeyDateEnd
      {
        get => !(this._keyDateEnd > this.DemoDateEnd) ? this.DemoDateEnd : this._keyDateEnd.Date;
        set => this._keyDateEnd = value;
      }

      public string KeyDateInfo
      {
        get
        {
          if (this.KeyDateEnd.Year == 2500)
            return Translate.LicenseInfoViewModel_БЕССРОЧНО;
          return this.KeyDateEnd >= DateTime.Today ? this.KeyDateEnd.ToShortDateString() + string.Format(Translate.LicenseInformation____0__дней_, (object) (this.KeyDateEnd - DateTime.Today).TotalDays) : Translate.СРОКДЕЙСТВИЯИСТЕК;
        }
      }

      public bool IsOverLimitDocument { get; }

      public bool IsActive
      {
        get
        {
          return this._keyDateEnd >= DateTime.Today & this.GbsId == LicenseHelper.GetGbsId() || this.IsDemoActive;
        }
      }

      private DateTime DemoDateEnd
      {
        get
        {
          DateTime dateTime = new DirectoryInfo(ApplicationInfo.GetInstance().Paths.DataPath).CreationTime;
          dateTime = dateTime.Date;
          DateTime demoDateEnd = dateTime.AddDays(30.0);
          LogHelper.Trace(string.Format("Demo date :{0:yyyy-MM-dd}", (object) demoDateEnd));
          return demoDateEnd;
        }
      }

      private bool IsDemoActive => !this.IsOverLimitDocument && this.DemoDateEnd >= DateTime.Today;

      public LicenseInformation()
      {
        int num = 0;
        if (LicenseHelper.LicenseInformation._docsQtyCache == null || DateTime.Now.AddHours(-6.0) > LicenseHelper.LicenseInformation._docsQtyCache.UpdatedTime)
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            try
            {
              DataBaseHelper.CheckDbConnection();
              LicenseHelper.LicenseInformation._docsQtyCache = new LicenseHelper.LicenseInformation.DocsQtyCache(dataBase.GetTable<DOCUMENTS>().Count<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.DATE_TIME.Year > 2018)));
              num = dataBase.GetTable<SECTIONS>().Count<SECTIONS>();
            }
            catch (Exception ex)
            {
              LogHelper.WriteError(ex);
            }
          }
        }
        LicenseHelper.LicenseInformation.DocsQtyCache docsQtyCache = LicenseHelper.LicenseInformation._docsQtyCache;
        int qty = docsQtyCache != null ? docsQtyCache.Qty : 0;
        LogHelper.Trace(string.Format("Кол-во документов в БД: {0}", (object) qty));
        this.IsOverLimitDocument = qty > this.MaxDocumentsForDemo || num > this.MaxSectionsForDemo;
      }

      private class DocsQtyCache
      {
        public int Qty { get; set; }

        public DateTime UpdatedTime { get; }

        public DocsQtyCache(int qty)
        {
          this.UpdatedTime = DateTime.Now;
          this.Qty = qty;
        }
      }
    }

    public enum LicenseTypes
    {
      Unpayment,
      Lite,
      Demo,
      Full,
    }
  }
}
