// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsCenterRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

#nullable disable
namespace Gbs.Helpers
{
  public class SmsCenterRepository
  {
    private string SMSC_LOGIN = "";
    private string SMSC_PASSWORD = "";
    private bool SMSC_POST;
    private const bool SMSC_HTTPS = false;
    private const string SMSC_CHARSET = "utf-8";
    public string[][] D2Res;

    public SmsCenterRepository(string login, string password)
    {
      this.SMSC_LOGIN = login;
      this.SMSC_PASSWORD = password;
    }

    [Localizable(false)]
    public SendSmsAnswer send_sms(
      string phones,
      string message,
      string sender = "",
      int translit = 0,
      string time = "",
      int id = 0,
      int format = 0,
      string query = "",
      string[] files = null)
    {
      if (files != null)
        this.SMSC_POST = true;
      string[] strArray = new string[12]
      {
        "flash=1",
        "push=1",
        "hlr=1",
        "bin=1",
        "bin=2",
        "ping=1",
        "mms=1",
        "mail=1",
        "call=1",
        "viber=1",
        "soc=1",
        "whatsapp=1"
      };
      return JsonConvert.DeserializeObject<SendSmsAnswer>(this._smsc_send_cmd("send", "cost=3&phones=" + this._urlencode(phones) + "&mes=" + this._urlencode(message) + "&id=" + id.ToString() + "&translit=" + translit.ToString() + (format > 0 ? "&" + strArray[format - 1] : "") + (sender != "" ? "&sender=" + this._urlencode(sender) : "") + (time != "" ? "&time=" + this._urlencode(time) : "") + (query != "" ? "&" + query : ""), files));
    }

    public BalanceAnswer get_balance()
    {
      return JsonConvert.DeserializeObject<BalanceAnswer>(this._smsc_send_cmd("balance", ""));
    }

    [Localizable(false)]
    private string _smsc_send_cmd(string cmd, string arg, string[] files = null)
    {
      arg = "login=" + this._urlencode(this.SMSC_LOGIN) + "&psw=" + this._urlencode(this.SMSC_PASSWORD) + "&fmt=3&op=1&charset=utf-8&" + arg;
      string requestUriString = "http://" + (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Kazakhstan ? "smsc.ru" : "smsc.kz") + "/sys/" + cmd + ".php" + (this.SMSC_POST ? "" : "?" + arg);
      int num = 0;
      string str1;
      do
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        if (this.SMSC_POST)
        {
          httpWebRequest.Method = "POST";
          string str2 = "----------" + DateTime.Now.Ticks.ToString("x");
          byte[] bytes1 = System.Text.Encoding.ASCII.GetBytes("--" + str2 + "--\r\n");
          StringBuilder stringBuilder = new StringBuilder();
          byte[] farr1 = new byte[0];
          byte[] buffer1;
          if (files == null)
          {
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            buffer1 = System.Text.Encoding.UTF8.GetBytes(arg);
            httpWebRequest.ContentLength = (long) buffer1.Length;
          }
          else
          {
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + str2;
            string[] strArray1 = arg.Split('&');
            int length = files.Length;
            for (int index = 0; index < strArray1.Length + length; ++index)
            {
              stringBuilder.Clear();
              stringBuilder.Append("--");
              stringBuilder.Append(str2);
              stringBuilder.Append("\r\n");
              stringBuilder.Append("Content-Disposition: form-data; name=\"");
              bool flag = index < length;
              string[] strArray2 = new string[0];
              if (flag)
              {
                stringBuilder.Append("File" + (index + 1).ToString());
                stringBuilder.Append("\"; filename=\"");
                stringBuilder.Append(Path.GetFileName(files[index]));
              }
              else
              {
                strArray2 = strArray1[index - length].Split('=');
                stringBuilder.Append(strArray2[0]);
              }
              stringBuilder.Append("\"");
              stringBuilder.Append("\r\n");
              stringBuilder.Append("Content-Type: ");
              stringBuilder.Append(flag ? "application/octet-stream" : "text/plain; charset=\"utf-8\"");
              stringBuilder.Append("\r\n");
              stringBuilder.Append("Content-Transfer-Encoding: binary");
              stringBuilder.Append("\r\n");
              stringBuilder.Append("\r\n");
              byte[] bytes2 = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
              byte[] farr2 = this._concatb(farr1, bytes2);
              if (flag)
              {
                FileStream fileStream = new FileStream(files[index], FileMode.Open, FileAccess.Read);
                byte[] buffer2 = new byte[(int) checked ((uint) Math.Min(4096, (int) fileStream.Length))];
                int newSize;
                while ((newSize = fileStream.Read(buffer2, 0, buffer2.Length)) != 0)
                {
                  byte[] array = buffer2;
                  Array.Resize<byte>(ref array, newSize);
                  farr2 = this._concatb(farr2, array);
                }
              }
              else
              {
                byte[] bytes3 = System.Text.Encoding.UTF8.GetBytes(strArray2[1]);
                farr2 = this._concatb(farr2, bytes3);
              }
              farr1 = this._concatb(farr2, System.Text.Encoding.UTF8.GetBytes("\r\n"));
            }
            buffer1 = this._concatb(farr1, bytes1);
            httpWebRequest.ContentLength = (long) buffer1.Length;
          }
          httpWebRequest.GetRequestStream().Write(buffer1, 0, buffer1.Length);
        }
        try
        {
          str1 = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd();
        }
        catch (WebException ex)
        {
          str1 = "";
        }
      }
      while (str1 == "" && num < 5);
      if (str1 == "")
        str1 = ",";
      if (cmd == "status")
      {
        string str3 = arg;
        char[] chArray = new char[1]{ '&' };
        foreach (string str4 in str3.Split(chArray))
        {
          string[] strArray = str4.Split("=".ToCharArray(), 2);
          if (strArray[0] == "id")
            strArray[1].IndexOf("%2c");
        }
      }
      return str1;
    }

    private string _urlencode(string str) => this.SMSC_POST ? str : HttpUtility.UrlEncode(str);

    private byte[] _concatb(byte[] farr, byte[] sarr)
    {
      int length = farr.Length;
      Array.Resize<byte>(ref farr, farr.Length + sarr.Length);
      Array.Copy((Array) sarr, 0, (Array) farr, length, sarr.Length);
      return farr;
    }
  }
}
