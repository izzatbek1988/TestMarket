// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.SbpDefault
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  internal class SbpDefault : ISbp, IDevice
  {
    private SbpQr _qr;
    private const string QrType = "02";
    private readonly string baseUrl;
    private readonly string tokenUrl;
    private readonly string qrUrl;
    private readonly string statusUrl;
    private readonly string legalId;
    private readonly string clientSecrer;
    private readonly string merchantId;
    private Token token;
    private string createdQrId;
    private SpbHelper.EStatusQr statusQr;
    private Decimal _sum;

    public SbpDefault()
    {
      SBP sbp = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SBP;
      this.legalId = sbp.PayQrClientID;
      this.merchantId = sbp.PayQrDeviceID;
      this.clientSecrer = sbp.ClientSecret.DecryptedValue;
      this.baseUrl = sbp.PayQrURL;
      this.tokenUrl = this.baseUrl + "/am/ipslegals/connect/token";
      this.qrUrl = this.baseUrl + "/api/merchant/v1/qrc-data";
      this.statusUrl = this.baseUrl + "/api/merchant/v1/qrc-status";
    }

    public static string Base64Encode(string plainText)
    {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    public bool GetToken(bool isReturn = true)
    {
      try
      {
        string str1 = "read";
        string str2 = SbpDefault.Base64Encode(this.legalId + ":" + this.clientSecrer);
        LogHelper.Debug("Получение токена от " + this.tokenUrl);
        ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.tokenUrl);
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.Headers.Add("Authorization", "Basic " + str2);
        httpWebRequest.Accept = "*/*";
        httpWebRequest.CachePolicy = (RequestCachePolicy) new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
        httpWebRequest.Timeout = -1;
        httpWebRequest.Method = "POST";
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
          {
            string message = "grant_type=client_credentials&scope=" + str1 + "&client_id=" + this.legalId + "&client_secret=" + this.clientSecrer;
            LogHelper.Debug(message);
            streamWriter.Write(message);
          }
        }
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        if (response.StatusCode != HttpStatusCode.OK)
        {
          LogHelper.Debug("Не удалось получить токен от " + this.tokenUrl + ", ответ: " + response.StatusDescription);
          return false;
        }
        Stream responseStream = response.GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
          this.token = JsonConvert.DeserializeObject<Token>(streamReader.ReadToEnd());
        LogHelper.Debug("Получен токен на " + this.tokenUrl);
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Method [SBP.GetToken]");
        return false;
      }
    }

    public bool GetQr(out string payLoad, out string rrn, bool isReturn = true)
    {
      rrn = "";
      try
      {
        LogHelper.Debug("Получение QR на " + this.qrUrl);
        string message = JsonConvert.SerializeObject((object) new SbpCommon()
        {
          MerchantId = this.merchantId,
          QrcType = "02",
          Amount = new long?((long) Convert.ToInt32(this._sum * 100M))
        }, Formatting.Indented);
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.qrUrl);
        httpWebRequest.Timeout = -1;
        httpWebRequest.Method = "POST";
        httpWebRequest.Accept = "application/json";
        httpWebRequest.Headers.Add("Authorization", "Bearer " + this.token.AccessToken);
        httpWebRequest.ContentType = "application/json";
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
          {
            LogHelper.Debug(message);
            streamWriter.Write(message);
          }
        }
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        switch (response.StatusCode)
        {
          case HttpStatusCode.OK:
          case HttpStatusCode.Created:
            this._qr = new SbpQr();
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
              throw new InvalidOperationException();
            using (StreamReader streamReader = new StreamReader(responseStream))
            {
              this._qr = JsonConvert.DeserializeObject<SbpQr>(streamReader.ReadToEnd());
              this.createdQrId = this._qr.QrcId;
              payLoad = this._qr.PayLoad;
              LogHelper.Debug("Получен QR на " + this.qrUrl + "\n" + this._qr.PayLoad);
            }
            return true;
          default:
            LogHelper.Debug("Не удалось получить QR на " + this.qrUrl + ", ответ: " + response.StatusDescription);
            payLoad = string.Empty;
            return false;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Метод [SBP.GetQr]");
        payLoad = string.Empty;
        return false;
      }
    }

    public bool GetStatus(out SpbHelper.EStatusQr statusQr, bool isReturn = true)
    {
      try
      {
        statusQr = SpbHelper.EStatusQr.RJCT;
        DateTime now = DateTime.Now;
        SbpCommon sbpCommon1 = new SbpCommon()
        {
          QrcIds = new List<string>()
        };
        sbpCommon1.QrcIds.Add(this.createdQrId);
        string message = JsonConvert.SerializeObject((object) sbpCommon1, Formatting.Indented);
        LogHelper.Debug("Получение статуса QR-кода от " + this.statusUrl);
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.statusUrl);
        httpWebRequest.Timeout = -1;
        httpWebRequest.Method = "PUT";
        httpWebRequest.Accept = "application/json";
        httpWebRequest.Headers.Add("Authorization", "Bearer " + this.token.AccessToken);
        httpWebRequest.ContentType = "application/json";
        using (Stream requestStream = httpWebRequest.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
          {
            LogHelper.Debug(message);
            streamWriter.Write(message);
          }
        }
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        if (response.StatusCode != HttpStatusCode.OK)
        {
          LogHelper.Debug("Не удалось получить статус QR-кода от " + this.statusUrl + ", ответ: " + response.StatusDescription);
          statusQr = SpbHelper.EStatusQr.RJCT;
          return false;
        }
        Stream responseStream = response.GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
        {
          SbpCommon sbpCommon2 = JsonConvert.DeserializeObject<SbpCommon>(streamReader.ReadToEnd());
          if (sbpCommon2 == null)
          {
            statusQr = SpbHelper.EStatusQr.RJCT;
            return false;
          }
          SbpStatus sbpStatus = sbpCommon2.Statuses.FirstOrDefault<SbpStatus>((Func<SbpStatus, bool>) (s => s.QrcId == this.createdQrId));
          if (sbpStatus == null)
            return false;
          statusQr = this.ParseStatus(sbpStatus.Status);
          LogHelper.Debug("Получен статус QR-кода " + sbpStatus.Status + " от " + this.statusUrl);
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Метод [SBP.GetStatus]");
        statusQr = SpbHelper.EStatusQr.RCVD;
        return false;
      }
    }

    public bool CancelQr() => throw new NotImplementedException();

    private SpbHelper.EStatusQr ParseStatus(string status)
    {
      SpbHelper.EStatusQr result;
      System.Enum.TryParse<SpbHelper.EStatusQr>(status, out result);
      return result;
    }

    public IDevice.DeviceTypes Type() => throw new NotImplementedException();

    public string Name { get; }
  }
}
