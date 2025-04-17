// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.PayMasterDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class PayMasterDriver
  {
    public static string SecretKey;

    public PayMasterDriver(string secretKey) => PayMasterDriver.SecretKey = secretKey;

    public void DoCommand(PayMasterDriver.PayMasterCommand command)
    {
      try
      {
        XmlSerializer xmlSerializer = new XmlSerializer(command.GetType());
        string path = Path.Combine(FileSystemHelper.TempFolderPath(), "sbp.xml");
        using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
          xmlSerializer.Serialize((Stream) fileStream, (object) command);
        string str = System.IO.File.ReadAllText(path);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        LogHelper.Debug("Начинаю выполнять комманду PayMaster: " + Other.NewLine() + str);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://paymaster.ru/qpay/ApiService.svc/" + command.Url);
        httpWebRequest.ContentType = "application/xml";
        httpWebRequest.Method = "POST";
        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
          streamWriter.Write(str);
        Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
        if (responseStream == null)
          throw new InvalidOperationException();
        using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(1251)))
          command.AnswerString = streamReader.ReadToEnd();
        LogHelper.Debug("Ответ PayMaster:\r\n" + command.AnswerString);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошиба отправки команды на PayMaster");
      }
    }

    private static string ConvertSha256(string randomString)
    {
      SHA256Managed shA256Managed = new SHA256Managed();
      StringBuilder stringBuilder = new StringBuilder();
      byte[] bytes = Encoding.UTF8.GetBytes(randomString);
      foreach (byte num in shA256Managed.ComputeHash(bytes))
        stringBuilder.Append(num.ToString("x2"));
      return stringBuilder.ToString();
    }

    public abstract class PayMasterCommand
    {
      [XmlIgnore]
      public string AnswerString { get; set; }

      [XmlIgnore]
      public virtual string Url { get; }

      [XmlElement(ElementName = "sign")]
      public virtual string Sign { get; set; }

      [XmlElement(ElementName = "reqn")]
      public long Reqn { get; set; }

      [XmlIgnore]
      public XmlReader AnswerXml
      {
        get
        {
          XmlReader answerXml = XmlReader.Create((TextReader) new StringReader(this.AnswerString), new XmlReaderSettings()
          {
            ConformanceLevel = ConformanceLevel.Fragment,
            IgnoreWhitespace = true,
            IgnoreComments = true
          });
          answerXml.Read();
          return answerXml;
        }
      }
    }

    public abstract class PayMasterAnswer
    {
      public int retval { get; set; }

      public string retdesc { get; set; }
    }

    [XmlRoot("w3s.request")]
    public class GenerateQrCodeCommand : PayMasterDriver.PayMasterCommand
    {
      public PayMasterDriver.GenerateQrCodeCommand.GenerateQrCode generate_qr_code = new PayMasterDriver.GenerateQrCodeCommand.GenerateQrCode();

      public override string Url => "generate_qr_code";

      public override string Sign
      {
        get
        {
          return PayMasterDriver.ConvertSha256(this.generate_qr_code.posid + this.generate_qr_code.orderid.ToString() + this.generate_qr_code.ptype + this.Reqn.ToString() + PayMasterDriver.SecretKey);
        }
      }

      public PayMasterDriver.GenerateQrCodeCommand.GenerateQrCodeAnswer Result
      {
        get
        {
          return (PayMasterDriver.GenerateQrCodeCommand.GenerateQrCodeAnswer) new XmlSerializer(typeof (PayMasterDriver.GenerateQrCodeCommand.GenerateQrCodeAnswer)).Deserialize(this.AnswerXml);
        }
      }

      public class GenerateQrCode
      {
        public string posid { get; set; }

        public Decimal amount { get; set; }

        public long orderid { get; set; }

        public string ptype { get; set; } = "sbp";
      }

      [XmlRoot("w3s.response")]
      public class GenerateQrCodeAnswer : PayMasterDriver.PayMasterAnswer
      {
        public PayMasterDriver.GenerateQrCodeCommand.GenerateQrCodeAnswer.GenerateQrCode generate_qr_code { get; set; } = new PayMasterDriver.GenerateQrCodeCommand.GenerateQrCodeAnswer.GenerateQrCode();

        public class GenerateQrCode
        {
          public long orderid { get; set; }

          public string qr { get; set; }
        }
      }
    }

    [XmlRoot("w3s.request")]
    public class GenerateQrCodeReturnCommand : PayMasterDriver.PayMasterCommand
    {
      public PayMasterDriver.GenerateQrCodeReturnCommand.GenerateQrCodeReturn refundorder = new PayMasterDriver.GenerateQrCodeReturnCommand.GenerateQrCodeReturn();

      public override string Url => "refund_order";

      public override string Sign
      {
        get
        {
          string posid = this.refundorder.posid;
          long num = this.refundorder.orderid;
          string str1 = num.ToString();
          num = this.Reqn;
          string str2 = num.ToString();
          string secretKey = PayMasterDriver.SecretKey;
          return PayMasterDriver.ConvertSha256(posid + str1 + str2 + secretKey);
        }
      }

      public PayMasterDriver.GenerateQrCodeReturnCommand.GenerateQrCodeReturnAnswer Result
      {
        get
        {
          return (PayMasterDriver.GenerateQrCodeReturnCommand.GenerateQrCodeReturnAnswer) new XmlSerializer(typeof (PayMasterDriver.GenerateQrCodeReturnCommand.GenerateQrCodeReturnAnswer)).Deserialize(this.AnswerXml);
        }
      }

      public class GenerateQrCodeReturn
      {
        public string posid { get; set; }

        public Decimal amount { get; set; }

        public long orderid { get; set; }

        public long sequenceid { get; set; }
      }

      [XmlRoot("w3s.response")]
      public class GenerateQrCodeReturnAnswer : PayMasterDriver.PayMasterAnswer
      {
      }
    }

    [XmlRoot(ElementName = "w3s.request")]
    public class GetStatusPayCommand : PayMasterDriver.PayMasterCommand
    {
      public PayMasterDriver.GetStatusPayCommand.GetStatusPayAnswer Result
      {
        get
        {
          return (PayMasterDriver.GetStatusPayCommand.GetStatusPayAnswer) new XmlSerializer(typeof (PayMasterDriver.GetStatusPayCommand.GetStatusPayAnswer)).Deserialize(this.AnswerXml);
        }
      }

      public override string Url => "get_outinvoices";

      public override string Sign
      {
        get
        {
          return PayMasterDriver.ConvertSha256(this.Posid + this.Reqn.ToString() + PayMasterDriver.SecretKey);
        }
      }

      [XmlElement(ElementName = "posid")]
      public string Posid { get; set; }

      [XmlElement(ElementName = "outinvoices")]
      public PayMasterDriver.GetStatusPayCommand.OutinvoicesList Outinvoices { get; set; }

      [XmlRoot(ElementName = "order")]
      public class Order
      {
        [XmlElement(ElementName = "orderid")]
        public long Orderid { get; set; }
      }

      [XmlRoot(ElementName = "outinvoices")]
      public class OutinvoicesList
      {
        [XmlElement(ElementName = "order")]
        public List<PayMasterDriver.GetStatusPayCommand.Order> Order { get; set; }
      }

      [XmlRoot(ElementName = "w3s.response")]
      public class GetStatusPayAnswer : PayMasterDriver.PayMasterAnswer
      {
        [XmlElement(ElementName = "invoices")]
        public List<PayMasterDriver.GetStatusPayCommand.GetStatusPayAnswer.InvoicesItem> Invoices { get; set; }

        [XmlRoot(ElementName = "invoices")]
        public class InvoicesItem
        {
          [XmlElement(ElementName = "orderid")]
          public long Orderid { get; set; }

          [XmlElement(ElementName = "invoiceid")]
          public int Invoiceid { get; set; }

          [XmlElement(ElementName = "amount")]
          public double Amount { get; set; }

          [XmlElement(ElementName = "ptype")]
          public string Ptype { get; set; }

          [XmlElement(ElementName = "state")]
          public int State { get; set; }
        }
      }
    }

    [XmlRoot(ElementName = "w3s.request")]
    public class GetStatusReturnCommand : PayMasterDriver.PayMasterCommand
    {
      public PayMasterDriver.GetStatusReturnCommand.GetStatusReturnAnswer Result
      {
        get
        {
          return (PayMasterDriver.GetStatusReturnCommand.GetStatusReturnAnswer) new XmlSerializer(typeof (PayMasterDriver.GetStatusReturnCommand.GetStatusReturnAnswer)).Deserialize(this.AnswerXml);
        }
      }

      public override string Url => "get_refunds";

      public override string Sign
      {
        get
        {
          string posid = this.Posid;
          long num = this.GetRefunds.Orderid;
          string str1 = num.ToString();
          num = this.Reqn;
          string str2 = num.ToString();
          string secretKey = PayMasterDriver.SecretKey;
          return PayMasterDriver.ConvertSha256(posid + str1 + str2 + secretKey);
        }
      }

      [XmlElement(ElementName = "posid")]
      public string Posid { get; set; }

      [XmlElement(ElementName = "getrefunds")]
      public PayMasterDriver.GetStatusReturnCommand.GetRefundsItem GetRefunds { get; set; }

      [XmlRoot(ElementName = "outinvoices")]
      public class GetRefundsItem
      {
        [XmlElement(ElementName = "posid")]
        public string Posid { get; set; }

        [XmlElement(ElementName = "orderid")]
        public long Orderid { get; set; }
      }

      [XmlRoot(ElementName = "w3s.response")]
      public class GetStatusReturnAnswer : PayMasterDriver.PayMasterAnswer
      {
        [XmlElement(ElementName = "refunds")]
        public List<PayMasterDriver.GetStatusReturnCommand.GetStatusReturnAnswer.InvoicesItem> Refunds { get; set; }

        [XmlRoot(ElementName = "refunds")]
        public class InvoicesItem
        {
          [XmlElement(ElementName = "orderid")]
          public long Orderid { get; set; }

          [XmlElement(ElementName = "refundid")]
          public int Refundid { get; set; }

          [XmlElement(ElementName = "amount")]
          public double Amount { get; set; }

          [XmlElement(ElementName = "state")]
          public int State { get; set; }
        }
      }
    }

    [XmlRoot(ElementName = "w3s.request")]
    public class CancelOrderCommand : PayMasterDriver.PayMasterCommand
    {
      public PayMasterDriver.CancelOrderCommand.CancelOrderAnswer Result
      {
        get
        {
          return (PayMasterDriver.CancelOrderCommand.CancelOrderAnswer) new XmlSerializer(typeof (PayMasterDriver.CancelOrderCommand.CancelOrderAnswer)).Deserialize(this.AnswerXml);
        }
      }

      public override string Url => "cancel_order";

      public override string Sign
      {
        get
        {
          string posid = this.CancelOrder.Posid;
          long num = this.CancelOrder.Orderid;
          string str1 = num.ToString();
          num = this.Reqn;
          string str2 = num.ToString();
          string secretKey = PayMasterDriver.SecretKey;
          return PayMasterDriver.ConvertSha256(posid + str1 + str2 + secretKey);
        }
      }

      [XmlElement(ElementName = "posid")]
      public string Posid { get; set; }

      [XmlElement(ElementName = "cancelorder")]
      public PayMasterDriver.CancelOrderCommand.CancelOrderItem CancelOrder { get; set; }

      [XmlRoot(ElementName = "cancelorder")]
      public class CancelOrderItem
      {
        [XmlElement(ElementName = "posid")]
        public string Posid { get; set; }

        [XmlElement(ElementName = "orderid")]
        public long Orderid { get; set; }
      }

      [XmlRoot(ElementName = "w3s.response")]
      public class CancelOrderAnswer : PayMasterDriver.PayMasterAnswer
      {
      }
    }
  }
}
