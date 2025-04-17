// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Licenses.WebAppKeysHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Gbs.Helpers.Licenses
{
  internal class WebAppKeysHelper
  {
    private string GetGbsId() => GbsIdHelperMain.GetGbsId();

    public void DownLoadKeyFile(string tempPath)
    {
      if (!NetworkHelper.IsWorkInternet())
      {
        LogHelper.Debug("Нет подключения к интернету");
      }
      else
      {
        IEnumerable<NetworkHelper.UrlPath> source = NetworkHelper.GetUrlPaths().Where<NetworkHelper.UrlPath>((Func<NetworkHelper.UrlPath, bool>) (x => x.UrlType == NetworkHelper.UrlTypes.Keys));
        if (!source.Any<NetworkHelper.UrlPath>())
        {
          LogHelper.Debug("keys host any = false");
        }
        else
        {
          foreach (NetworkHelper.UrlPath urlPath in source)
          {
            Uri url = urlPath.Url;
            LogHelper.Debug(string.Format("Host for keys: {0}", (object) url));
            if (!urlPath.Ping())
            {
              LogHelper.Debug(string.Format("No ping to {0}", (object) url));
            }
            else
            {
              if (url.Scheme == "https")
              {
                ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
              }
              bool flag = NetworkHelper.DownloadFile(string.Format("{0}api/Public/getKey?gbsid=", (object) url) + this.GetGbsId(), tempPath);
              LogHelper.Debug(string.Format("Key file download result = {0}", (object) flag));
              if (flag)
                break;
            }
          }
          if (!System.IO.File.Exists(tempPath))
          {
            LogHelper.Debug("Not found temp key file");
          }
          else
          {
            string contents = System.IO.File.ReadAllText(tempPath, Encoding.Unicode).Replace("gbskey>", "gbskey_v2>");
            System.IO.File.WriteAllText(tempPath, contents, Encoding.Unicode);
          }
        }
      }
    }

    internal LicenseHelper.LicenseInformation GetInfoFromFile(string path)
    {
      string str1 = System.IO.File.Exists(path) ? System.IO.File.ReadAllText(path, Encoding.Unicode) : throw new FileNotFoundException();
      if (!str1.StartsWith("<gbskey_v2>") || !str1.EndsWith("</gbskey_v2>"))
        throw new FileFormatException();
      string str2 = Encoding.UTF8.GetString(Convert.FromBase64String(str1.Replace("<gbskey_v2>", string.Empty).Replace("</gbskey_v2>", string.Empty)));
      JsonSerializerSettings settings1 = new JsonSerializerSettings()
      {
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        DateParseHandling = DateParseHandling.DateTimeOffset,
        DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind
      };
      JsonSerializerSettings settings2 = settings1;
      LicenseFileNew licenseFileNew = JsonConvert.DeserializeObject<LicenseFileNew>(str2, settings2);
      string publicKey = Encoding.UTF8.GetString(Convert.FromBase64String(licenseFileNew.Public));
      LicenseInfoNew licenseInfoNew = JsonConvert.DeserializeObject<LicenseInfoNew>(licenseFileNew.License, settings1);
      if (!WebAppKeysHelper.VerifyData(licenseFileNew.Sign, licenseFileNew.License, publicKey))
        throw new InvalidOperationException();
      return new LicenseHelper.LicenseInformation()
      {
        GbsId = licenseInfoNew.GbsId,
        KeyDateEnd = licenseInfoNew.Expird
      };
    }

    private static bool VerifyData(string signatureText, string message, string publicKey)
    {
      byte[] bytes = new ASCIIEncoding().GetBytes(message);
      byte[] signature = Convert.FromBase64String(signatureText);
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
      cryptoServiceProvider.FromXmlString(publicKey);
      return cryptoServiceProvider.VerifyData(bytes, (object) new SHA1CryptoServiceProvider(), signature);
    }
  }
}
