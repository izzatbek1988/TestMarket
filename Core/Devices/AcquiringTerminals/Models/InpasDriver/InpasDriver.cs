// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver.InpasDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Zidium.Api.Dto;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models.InpasDriver
{
  public class InpasDriver
  {
    private readonly LanConnection _lanConnection;
    private bool _isRestartService;

    public InpasDriver(LanConnection lanConnection) => this._lanConnection = lanConnection;

    private void RunCommand(string command)
    {
      string path = Path.Combine(FileSystemHelper.TempFolderPath(), "service.bat");
      System.IO.File.WriteAllText(path, command);
      Process.Start(new ProcessStartInfo()
      {
        FileName = path,
        Verb = "runas",
        UseShellExecute = true
      }).WaitForExit();
    }

    public void RestartService()
    {
      try
      {
        foreach (Process process in Process.GetProcesses())
        {
          if (process.ProcessName.ToLower().Contains("DCService"))
            process.Kill();
        }
        LogHelper.Debug("Перезапуск службы DCService. Останавливаем службу...");
        this.RunCommand("sc stop DCService");
        Thread.Sleep(5000);
        LogHelper.Debug("Перезапуск службы DCService. Запускаем службу...");
        this.RunCommand("sc start DCService");
        Thread.Sleep(3000);
      }
      catch
      {
        LogHelper.Debug("Недостаточно прав для перезапуска службы или возникла другая ошибка во время перезапуска");
      }
    }

    public InpasResponse DoCommand(InpasRequest command)
    {
      try
      {
        string str1 = this._lanConnection.UrlAddress;
        if (!str1.ToLower().StartsWith("http://"))
          str1 = "http://" + str1;
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(string.Format("{0}:{1}", (object) str1, (object) this._lanConnection.PortNumber));
        Other.ConsoleWrite("address: " + httpWebRequest.Address.ToJsonString());
        httpWebRequest.ContentType = "text/xml";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 180000;
        httpWebRequest.ReadWriteTimeout = 180000;
        httpWebRequest.KeepAlive = false;
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
          System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(command.GetType());
          Utf8StringWriter utf8StringWriter1 = new Utf8StringWriter();
          Utf8StringWriter utf8StringWriter2 = utf8StringWriter1;
          InpasRequest o = command;
          xmlSerializer.Serialize((TextWriter) utf8StringWriter2, (object) o);
          string text = utf8StringWriter1.ToString();
          Other.ConsoleWrite(text);
          streamWriter.Write(text);
        }
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(1251)))
        {
          string str2 = streamReader.ReadToEnd().Replace("&#6;", "").Replace("&#0;", "").Replace("&#4;", "");
          LogHelper.Debug(str2);
          object obj = new System.Xml.Serialization.XmlSerializer(typeof (InpasResponse)).Deserialize((TextReader) new StringReader(str2));
          Other.ConsoleWrite("answer:" + ((InpasResponse) obj).ToJsonString());
          return (InpasResponse) obj;
        }
      }
      catch (WebException ex) when (ex.Status == WebExceptionStatus.ProtocolError && !this._isRestartService)
      {
        this.RestartService();
        this._isRestartService = true;
        Thread.Sleep(3000);
        return this.DoCommand(command);
      }
      catch (WebException ex)
      {
        Stream responseStream = ex?.Response?.GetResponseStream();
        if (responseStream == null)
          throw ex;
        string str = new StreamReader(responseStream)?.ReadToEnd();
        string[] strArray = new string[2]{ "<h1>", "</h1>" };
        foreach (string oldValue in strArray)
          str = str.Replace(oldValue, string.Empty);
        throw new Exception(Translate.InpasDriver_DoCommand_Ошибка_при_выполнении_запроса_INPAS + "\r\n\r\n" + str + "\r\n" + ex.Message);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
