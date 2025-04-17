// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.OtherDevices.KkmServerDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

#nullable enable
namespace Gbs.Core.Devices.OtherDevices
{
  public class KkmServerDriver
  {
    private 
    #nullable disable
    LanConnection _lanConnection;

    public KkmServerDriver(LanConnection lanConnection) => this._lanConnection = lanConnection;

    public bool SendCommand(KkmServerDriver.KkmServerCommand command)
    {
      LogHelper.Debug("Команда на ККМ-Сервер:" + command.ToJsonString(true));
      command.Timeout = 1;
      if (!this.DoCommand(command))
        return false;
      if (command.DeviceAnswer.Status.IsEither<int>(1, 4))
      {
        bool result;
        int status;
        int[] numArray;
        do
        {
          Thread.Sleep(1000);
          result = this.GetResult(command);
          status = command.DeviceAnswer.Status;
          numArray = new int[2]{ 1, 4 };
        }
        while (status.IsEither<int>(numArray) & result);
      }
      LogHelper.Debug("Ответ ККМ-сервера:\r\n" + command.Answer);
      if (command.DeviceAnswer.Status != 0)
        throw new Exception(Translate.KkmServerDriver_Ошибка_выполнения_команды_на_ККМ_Сервер__ + command.DeviceAnswer.Error);
      return true;
    }

    private bool GetResult(KkmServerDriver.KkmServerCommand command)
    {
      KkmServerDriver.GetResultCommand getResultCommand = new KkmServerDriver.GetResultCommand();
      getResultCommand.IdCommand = command.IdCommand;
      getResultCommand.Timeout = 10;
      KkmServerDriver.GetResultCommand command1 = getResultCommand;
      int num = this.DoCommand((KkmServerDriver.KkmServerCommand) command1) ? 1 : 0;
      JToken jtoken = JsonConvert.DeserializeObject<JObject>(command1.Answer)["Rezult"];
      command.Answer = jtoken?.ToString();
      command.DeviceAnswer.Status = JsonConvert.DeserializeObject<KkmServerDriver.DeviceAnswer>(command.Answer).Status;
      command.DeviceAnswer.Error = JsonConvert.DeserializeObject<KkmServerDriver.DeviceAnswer>(command.Answer).Error;
      return num != 0;
    }

    private bool DoCommand(KkmServerDriver.KkmServerCommand command)
    {
      try
      {
        string str1 = this._lanConnection.UrlAddress;
        if (!str1.ToLower().StartsWith("http://"))
          str1 = "http://" + str1;
        string str2 = str1 + ":" + this._lanConnection.PortNumber.ToString() + "/Execute";
        CredentialCache credentialCache = new CredentialCache()
        {
          {
            new Uri(str2),
            "Basic",
            new NetworkCredential(this._lanConnection.UserLogin, this._lanConnection.Password)
          }
        };
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(str2);
        httpWebRequest.Credentials = (ICredentials) credentialCache;
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.Timeout = 120000;
        httpWebRequest.ReadWriteTimeout = 120000;
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Headers.Add(HttpRequestHeader.CacheControl, "must-revalidate");
        string str3 = JsonConvert.SerializeObject((object) command, Formatting.Indented, new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Ignore
        });
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          streamWriter.Write(str3);
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        response.Headers.Add(HttpResponseHeader.CacheControl, "must-revalidate");
        Stream responseStream = response.GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream))
          command.Answer = streamReader.ReadToEnd();
        LogHelper.Debug("KKM SERER ANSWER: " + Other.NewLine() + command.Answer);
        command.DeviceAnswer = JsonConvert.DeserializeObject<KkmServerDriver.DeviceAnswer>(command.Answer);
        return true;
      }
      catch (WebException ex)
      {
        switch (ex.Status)
        {
          case WebExceptionStatus.ConnectFailure:
            LogHelper.Error((Exception) ex, string.Format(Translate.KkmServerDriver_DoCommand_Не_удалось_подключиться_к_ККМ_серверу_по_адресу___0___1___Убедитесь__что_адрес_указан_корректно_и_сервер_запущен_, (object) this._lanConnection.UrlAddress, (object) this._lanConnection.PortNumber), false);
            throw new WebException(string.Format(Translate.KkmServerDriver_DoCommand_Не_удалось_подключиться_к_ККМ_серверу_по_адресу___0___1___Убедитесь__что_адрес_указан_корректно_и_сервер_запущен_, (object) this._lanConnection.UrlAddress, (object) this._lanConnection.PortNumber));
          case WebExceptionStatus.ProtocolError:
            if ((ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.Unauthorized)
            {
              LogHelper.Error((Exception) ex, Translate.KkmServerDriver_DoCommand_Не_удалось_подключиться_к_ККМ_Серверу__неверный_логин_или_пароль_, false);
              throw new WebException(Translate.KkmServerDriver_DoCommand_Не_удалось_подключиться_к_ККМ_Серверу__неверный_логин_или_пароль_);
            }
            break;
        }
        LogHelper.Error((Exception) ex, Translate.KkmServerDriver_DoCommand_Ошибка_отправки_команды_на_ККМ_СЕРВЕР, false);
        throw new WebException(Translate.KkmServerDriver_DoCommand_Ошибка_отправки_команды_на_ККМ_СЕРВЕР);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка отправки команды на ККМ-СЕРВЕР", false);
        throw new WebException(Translate.KkmServerDriver_DoCommand_Ошибка_отправки_команды_на_ККМ_СЕРВЕР);
      }
    }

    private interface IKkmSeverCommand
    {
      string Command { get; }
    }

    public abstract class KkmServerCommand
    {
      [JsonIgnore]
      public string Answer { get; set; }

      [JsonIgnore]
      public KkmServerDriver.DeviceAnswer DeviceAnswer { get; set; }

      public Guid IdCommand { get; set; } = Guid.NewGuid();

      public int Timeout { get; set; } = 60000;
    }

    public class KkmOpenDrawer : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      public string Command { get; } = "OpenCashDrawer";
    }

    public class KkmGetInfo : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      [JsonIgnore]
      public KkmServerDriver.KkmGetInfo.KkmInfo Data
      {
        get => JsonConvert.DeserializeObject<KkmServerDriver.KkmGetInfo.KkmInfo>(this.Answer);
      }

      public string Command { get; } = "GetDataKKT";

      public class KkmInfo
      {
        public int CheckNumber { get; set; }

        public int SessionNumber { get; set; }

        public KkmServerDriver.KkmGetInfo.KkmInfo.Information Info { get; set; }

        public class Information
        {
          public int SessionState { get; set; }

          public Decimal BalanceCash { get; set; }

          public string Firmware_Version { get; set; }

          public DateTime FN_DateEnd { get; set; }
        }
      }
    }

    private class GetResultCommand : 
      KkmServerDriver.KkmServerCommand,
      KkmServerDriver.IKkmSeverCommand
    {
      public string Command { get; } = "GetRezult";
    }

    public class KkmCashOut : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      public string CashierName { get; set; }

      public string CashierVATIN { get; set; }

      public Decimal Amount { get; set; }

      public string Command { get; } = "PaymentCash";
    }

    public class KkmCashIn : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      public string CashierName { get; set; }

      public string CashierVATIN { get; set; }

      public Decimal Amount { get; set; }

      public string Command { get; } = "DepositingCash";
    }

    public class KkmCloseShift : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      public string CashierName { get; set; }

      public string CashierVATIN { get; set; }

      public string Command { get; } = "CloseShift";
    }

    public class KkmOpenSession : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      public string CashierName { get; set; }

      public string CashierVATIN { get; set; }

      public string Command { get; } = "OpenShift";
    }

    public class KkmGetXReport : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      public string Command { get; } = "XReport";
    }

    public class KkmPrintCheck : KkmServerDriver.KkmServerCommand, KkmServerDriver.IKkmSeverCommand
    {
      public bool IsFiscalCheck { get; set; }

      public int TypeCheck { get; set; }

      public bool NotPrint { get; set; }

      public string CashierName { get; set; }

      public string CashierVATIN { get; set; }

      public string ClientAddress { get; set; }

      public string ClientInfo { get; set; }

      public string ClientINN { get; set; }

      public int? TaxVariant { get; set; }

      public Decimal Cash { get; set; }

      public Decimal ElectronicPayment { get; set; }

      public Decimal AdvancePayment { get; set; }

      public Decimal Credit { get; set; }

      public List<KkmServerDriver.KkmPrintCheck.CheckString> CheckStrings { get; set; } = new List<KkmServerDriver.KkmPrintCheck.CheckString>();

      public string Command { get; } = "RegisterCheck";

      public class CheckString
      {
        public KkmServerDriver.KkmPrintCheck.PrintText PrintText { get; set; }

        public KkmServerDriver.KkmPrintCheck.PrintImage PrintImage { get; set; }

        public KkmServerDriver.KkmPrintCheck.Register Register { get; set; }

        public KkmServerDriver.KkmPrintCheck.BarCode BarCode { get; set; }
      }

      public class Register
      {
        public string Name { get; set; }

        public Decimal Quantity { get; set; }

        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }

        public int Department { get; set; }

        public int Tax { get; set; }

        public int SignMethodCalculation { get; set; }

        public int SignCalculationObject { get; set; }

        public int? MeasureOfQuantity { get; set; }

        public KkmServerDriver.KkmPrintCheck.Register.GoodCode GoodCodeData { get; set; }

        public class GoodCode
        {
          public string StampType { get; set; }

          public string GTIN { get; set; }

          public string SerialNumber { get; set; }

          public string BarCode { get; set; }

          public bool? ContainsSerialNumber { get; set; }

          public bool? AcceptOnBad { get; set; }

          public 
          #nullable enable
          string? IndustryProps { get; set; }
        }
      }

      public class PrintText
      {
        public 
        #nullable disable
        string Text { get; set; }

        public int Font { get; set; }

        public int Intensity { get; set; }
      }

      public class PrintImage
      {
        public string Image { get; set; }
      }

      public class BarCode
      {
        public string BarcodeType { get; set; }

        public string Barcode { get; set; }
      }
    }

    public class TerminalDoPayment : 
      KkmServerDriver.KkmServerCommand,
      KkmServerDriver.IKkmSeverCommand
    {
      public Decimal Amount { get; set; }

      public string ReceiptNumber { get; set; }

      [JsonIgnore]
      public KkmServerDriver.TerminalDoPayment.Information Data
      {
        get
        {
          return JsonConvert.DeserializeObject<KkmServerDriver.TerminalDoPayment.Information>(this.Answer);
        }
      }

      public string Command { get; } = "PayByPaymentCard";

      public class Information
      {
        public string Slip { get; set; }

        public string RRNCode { get; set; }

        public string UniversalID { get; set; }

        public string AuthorizationCode { get; set; }
      }
    }

    public class TerminalReturnPayment : 
      KkmServerDriver.KkmServerCommand,
      KkmServerDriver.IKkmSeverCommand
    {
      public Decimal Amount { get; set; }

      public string ReceiptNumber { get; set; }

      public string UniversalID { get; set; }

      [JsonIgnore]
      public KkmServerDriver.TerminalReturnPayment.Information Data
      {
        get
        {
          return JsonConvert.DeserializeObject<KkmServerDriver.TerminalReturnPayment.Information>(this.Answer);
        }
      }

      public string Command { get; } = "ReturnPaymentByPaymentCard";

      public class Information
      {
        public string Slip { get; set; }

        public string RRNCode { get; set; }

        public string AuthorizationCode { get; set; }
      }
    }

    public class TerminalCloseSession : 
      KkmServerDriver.KkmServerCommand,
      KkmServerDriver.IKkmSeverCommand
    {
      [JsonIgnore]
      public KkmServerDriver.TerminalCloseSession.Information Data
      {
        get
        {
          return JsonConvert.DeserializeObject<KkmServerDriver.TerminalCloseSession.Information>(this.Answer);
        }
      }

      public string Command { get; } = "Settlement";

      public class Information
      {
        public string Slip { get; set; }
      }
    }

    public class TerminalGetReport : 
      KkmServerDriver.KkmServerCommand,
      KkmServerDriver.IKkmSeverCommand
    {
      public bool Detailed { get; set; } = true;

      [JsonIgnore]
      public KkmServerDriver.TerminalGetReport.Information Data
      {
        get
        {
          return JsonConvert.DeserializeObject<KkmServerDriver.TerminalGetReport.Information>(this.Answer);
        }
      }

      public string Command { get; } = "TerminalReport";

      public class Information
      {
        public string Slip { get; set; }
      }
    }

    public class DeviceAnswer
    {
      public int Status { get; set; }

      public string Error { get; set; }

      public Guid? IdCommand { get; set; }
    }
  }
}
