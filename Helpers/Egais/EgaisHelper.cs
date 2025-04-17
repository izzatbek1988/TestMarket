// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.EgaisHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  public class EgaisHelper
  {
    private readonly string _pathDebugEgais = Path.Combine(ApplicationInfo.GetInstance().Paths.DataPath, "EgaisDebug");

    public static List<int> ListCodeForBeer
    {
      get
      {
        return new List<int>()
        {
          261,
          2611,
          2612,
          2613,
          2614,
          262,
          263,
          510,
          520,
          530,
          531,
          532,
          533,
          534,
          535,
          536,
          537,
          500
        };
      }
    }

    public static EgaisHelper.AlcoholTypeGorEgais GetAlcoholType(Gbs.Core.Entities.Goods.Good good)
    {
      object obj1;
      if (good == null)
      {
        obj1 = (object) null;
      }
      else
      {
        List<EntityProperties.PropertyValue> properties = good.Properties;
        obj1 = properties != null ? properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.ProductCodeUid))?.Value : (object) null;
      }
      object obj2 = obj1;
      if (obj2 == null)
        return EgaisHelper.AlcoholTypeGorEgais.NoAlcohol;
      LogHelper.WriteToEgaisLog(good.Name + ": code " + obj2?.ToString());
      int result;
      if (!int.TryParse(obj2.ToString(), out result))
        return EgaisHelper.AlcoholTypeGorEgais.NoAlcohol;
      return !result.IsEither<int>((IEnumerable<int>) EgaisHelper.ListCodeForBeer) ? EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol : EgaisHelper.AlcoholTypeGorEgais.Beer;
    }

    public static bool IsBeerKega(Gbs.Core.Entities.Goods.Good good)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if ((good.SetStatus != GlobalDictionaries.GoodsSetStatuses.Production ? 0 : (good.SetContent.Count<GoodsSets.Set>() == 1 ? 1 : 0)) == 0)
          return false;
        Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(good.SetContent.Single<GoodsSets.Set>().GoodUid);
        return EgaisHelper.GetAlcoholType(byUid) == EgaisHelper.AlcoholTypeGorEgais.Beer || byUid.Group.IsCompositeGood && byUid.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Alcohol;
      }
    }

    public void GetCommand(EgaisHelper.GetEgiasCommand command, bool isFullCommand = true)
    {
      EgaisSettings egais = new ConfigsRepository<Integrations>().Get().Egais;
      if (DevelopersHelper.IsDebug())
        egais.PathUtm = "https://gbsmarket.ru/egais-test";
      string url = isFullCommand ? egais.PathUtm + command.Method : command.Method;
      LogHelper.WriteToEgaisLog("Делаем GET запрос в УТМ: " + url);
      RestHelper restHelper = new RestHelper(url, new int?(), (string) null);
      restHelper.CreateCommand("", TypeRestRequest.Get);
      restHelper.DoCommand();
      if (restHelper.StatusCode != HttpStatusCode.OK)
      {
        LogHelper.WriteToEgaisLog("code = " + restHelper.StatusCode.ToString());
        LogHelper.WriteToEgaisLog("Answer = " + restHelper.Answer);
      }
      else
      {
        command.AnswerString = restHelper.Answer;
        LogHelper.WriteToEgaisLog("Egais answer: " + command.AnswerString);
      }
    }

    public void PostCommand(EgaisHelper.PostEgiasCommand command, bool isFullCommand = true)
    {
      EgaisSettings egais = new ConfigsRepository<Integrations>().Get().Egais;
      string requestUri = isFullCommand ? egais.PathUtm + command.Method : command.Method;
      LogHelper.WriteToEgaisLog("Делаем POST запрос в УТМ: " + requestUri);
      XmlSerializer xmlSerializer = new XmlSerializer(command.Type);
      string str = Path.Combine(FileSystemHelper.TempFolderPath(), "WaybillAct_v4.xml");
      using (FileStream fileStream = new FileStream(str, FileMode.Create))
      {
        LogHelper.WriteToEgaisLog("Serialize: " + command.ToJsonString(true));
        xmlSerializer.Serialize((Stream) fileStream, command.Item);
      }
      string empty1 = string.Empty;
      try
      {
        string empty2 = string.Empty;
        XmlDocument xmlDocument = new XmlDocument();
        string xml = System.IO.File.ReadAllText(str);
        xmlDocument.LoadXml(xml);
        string outerXml = xmlDocument.OuterXml;
        if (DevelopersHelper.IsDebug())
        {
          if (!Directory.Exists(this._pathDebugEgais))
            Directory.CreateDirectory(this._pathDebugEgais);
          FileSystemHelper.MoveFile(str, Path.Combine(this._pathDebugEgais, string.Format("{0}-{1}", (object) Guid.NewGuid(), (object) command.FileName)));
          command.AnswerString = "<A><ver>2</ver></A>";
        }
        else
        {
          LogHelper.WriteToEgaisLog("XML запрос:\n" + xml);
          using (HttpClient httpClient = new HttpClient())
          {
            using (MultipartFormDataContent content1 = new MultipartFormDataContent())
            {
              ByteArrayContent content2 = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(outerXml));
              content2.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
              content1.Add((HttpContent) content2, "xml_file", command.FileName);
              Task<HttpResponseMessage> task1 = httpClient.PostAsync(requestUri, (HttpContent) content1);
              task1.Wait();
              Task<string> task2 = task1.Result.Content.ReadAsStringAsync();
              task2.Wait();
              command.AnswerString = task2.Result;
              LogHelper.WriteToEgaisLog("Egais answer: " + command.AnswerString);
            }
          }
        }
      }
      catch (AggregateException ex)
      {
        foreach (Exception innerException in ex.InnerExceptions)
          LogHelper.WriteToEgaisLog("Egais Exception answer: " + innerException.Message, innerException);
        command.AnswerString = ex.Message;
      }
      catch (HttpRequestException ex)
      {
        string message = ex.Message;
        command.AnswerString = message;
        LogHelper.WriteToEgaisLog("Egais HttpRequestException answer: " + command.AnswerString, (Exception) ex);
      }
      catch (Exception ex)
      {
        LogHelper.WriteToEgaisLog("Egais Exception answer: " + ex.Message, ex);
      }
    }

    public enum StatusEgaisTTN
    {
      New,
      Cancel,
      Accept,
      SendedAct,
      All,
    }

    public enum TypeEgaisTTN
    {
      None,
      Waybill,
      Act,
      Tiket,
      WriteOffAct,
      Form2,
    }

    public enum DirectionEgaisTTN
    {
      In,
      Out,
    }

    public enum AlcoholTypeGorEgais
    {
      NoAlcohol,
      StrongAlcohol,
      Beer,
    }

    public abstract class GetEgiasCommand
    {
      [XmlIgnore]
      public virtual string Method { get; }

      [XmlIgnore]
      public string AnswerString { get; set; }

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

    public abstract class PostEgiasCommand
    {
      [XmlIgnore]
      public virtual object Item => (object) this.Documents;

      [XmlIgnore]
      public virtual string FileName => "";

      [XmlIgnore]
      public virtual Type Type => typeof (Documents);

      public virtual Documents Documents { get; set; }

      public virtual string Method { get; }

      public string AnswerString { get; set; }

      [JsonIgnore]
      public XmlReader AnswerXml
      {
        get
        {
          XmlReader answerXml = !string.IsNullOrEmpty(this.AnswerString) ? XmlReader.Create((TextReader) new StringReader(this.AnswerString), new XmlReaderSettings()
          {
            ConformanceLevel = ConformanceLevel.Fragment,
            IgnoreWhitespace = true,
            IgnoreComments = true
          }) : throw new InvalidOperationException("Ответный XML не может быть пустым или null.");
          answerXml.Read();
          return answerXml;
        }
      }

      [XmlIgnore]
      public EgaisHelper.A Result
      {
        get
        {
          return (EgaisHelper.A) new XmlSerializer(typeof (EgaisHelper.A)).Deserialize(this.AnswerXml);
        }
      }
    }

    public class GetWaybillOut : EgaisHelper.GetEgiasCommand
    {
      public override string Method => "/opt/out";

      public EgaisHelper.A Result
      {
        get
        {
          return (EgaisHelper.A) new XmlSerializer(typeof (EgaisHelper.A)).Deserialize(this.AnswerXml);
        }
      }
    }

    public class GetForm2 : EgaisHelper.GetEgiasCommand
    {
      public override string Method => this.Path;

      public string Path { get; set; }

      public Documents Result
      {
        get => (Documents) new XmlSerializer(typeof (Documents)).Deserialize(this.AnswerXml);
      }
    }

    public class SendWayBillAct4 : EgaisHelper.PostEgiasCommand
    {
      public override string FileName => "WaybillAct_v4.xml";

      public override string Method => "/opt/in/WayBillAct_v4";
    }

    public class PostOldWaybill : EgaisHelper.PostEgiasCommand
    {
      public override string FileName => "QueryNATTN.xml";

      public override string Method => "/opt/in/QueryNATTN";
    }

    public class PostSingleWaybill : EgaisHelper.PostEgiasCommand
    {
      public override string FileName => "QueryResendDoc.xml";

      public override string Method => "/opt/in/QueryResendDoc";
    }

    public class PostOldStockForShop : EgaisHelper.PostEgiasCommand
    {
      public override string FileName => "QueryRestsShop.xml";

      public override string Method => "/opt/in/QueryRestsShop_v2";
    }

    public class PostOldStockForOneRegister : EgaisHelper.PostEgiasCommand
    {
      public override string FileName => "QueryParameters.xml";

      public override string Method => "/opt/in/QueryRests";
    }

    public class DoSaleStrongCommand : EgaisHelper.PostEgiasCommand
    {
      public override string FileName => "Cheque.xml";

      [XmlIgnore]
      public override Type Type => typeof (Cheque);

      public override object Item => (object) this.Cheque;

      public override string Method => "/xml";

      [XmlIgnore]
      public override Documents Documents { get; set; }

      public Cheque Cheque { get; set; }
    }

    public class SendActWriteOffBeer : EgaisHelper.PostEgiasCommand
    {
      public override string FileName => "WaybillAct_v3.xml";

      public override string Method => "/opt/in/ActWriteOff_v3";
    }

    public class GetTicket : EgaisHelper.GetEgiasCommand
    {
      public override string Method => this.Path;

      public string Path { get; set; }

      public Documents Result
      {
        get => (Documents) new XmlSerializer(typeof (Documents)).Deserialize(this.AnswerXml);
      }
    }

    public class GetWaybillTicket : EgaisHelper.GetEgiasCommand
    {
      public override string Method => this.Path;

      public string Path { get; set; }

      public Documents Result
      {
        get => (Documents) new XmlSerializer(typeof (Documents)).Deserialize(this.AnswerXml);
      }
    }

    public class GetWaybillOutByReplyId : EgaisHelper.GetEgiasCommand
    {
      public override string Method => "/opt/out?replyId=" + this.ReplyId;

      public string ReplyId { get; set; }
    }

    public class GetWaybillOutByReplyPartner : EgaisHelper.GetEgiasCommand
    {
      public override string Method => "/opt/out/replypartner";
    }

    [XmlRoot(ElementName = "url")]
    public class Url
    {
      [XmlAttribute(AttributeName = "replyId")]
      public string ReplyId { get; set; }

      [XmlText]
      public string Text { get; set; }
    }

    [XmlRoot(ElementName = "A")]
    public class A
    {
      [XmlElement(ElementName = "error")]
      public string Error { get; set; }

      [XmlElement(ElementName = "url")]
      public List<EgaisHelper.Url> Url { get; set; }

      [XmlElement(ElementName = "ver")]
      public int Ver { get; set; }
    }
  }
}
