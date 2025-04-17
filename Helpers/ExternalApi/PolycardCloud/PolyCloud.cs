// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ExternalApi.PolycardCloud.PolyCloud
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Helpers.ExternalApi.PolycardCloud.Entities;
using Gbs.Helpers.Logging;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Helpers.ExternalApi.PolycardCloud
{
  public class PolyCloud
  {
    private const string PolyApiUrl = "https://cloud.polycard.club/api/v1/web_service/call";

    private Gbs.Core.Config.PolyCloud CloudConfig
    {
      get => new ConfigsRepository<Integrations>().Get().PolyCloud;
    }

    public void UploadAllClientsToCloud()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        foreach (Request request in new ClientsRepository(dataBase).GetActiveItems().Select<Gbs.Core.Entities.Clients.Client, Request>((Func<Gbs.Core.Entities.Clients.Client, Request>) (c => new Request()
        {
          action = Request.Type.create,
          data = (IPolyCloudEntity) new Gbs.Helpers.ExternalApi.PolycardCloud.Entities.Client()
          {
            name = c.Name,
            surname = string.Empty,
            external_id = c.Uid.ToString(),
            phone = c.Phone,
            email = c.Email
          }
        })))
          this.SendRequest(request);
      }
    }

    public void SendClient(Guid clientUid)
    {
      if (clientUid == Guid.Empty || !this.CloudConfig.IsActive)
        return;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ClientAdnSum clientByUidAndSum = new ClientsRepository(dataBase).GetClientByUidAndSum(clientUid);
        Gbs.Core.Entities.Clients.Client client = clientByUidAndSum.Client;
        string str = UidDb.GetUid().EntityUid.ToString();
        object obj1 = (object) new JObject();
        // ISSUE: reference to a compiler-generated field
        if (PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "bonus_quantity", typeof (PolyCloud), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__0, obj1, clientByUidAndSum.CurrentBonusSum);
        // ISSUE: reference to a compiler-generated field
        if (PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "code", typeof (PolyCloud), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__1.Target((CallSite) PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__1, obj1, client.Barcode);
        // ISSUE: reference to a compiler-generated field
        if (PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "sales_point_ext_id", typeof (PolyCloud), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__2.Target((CallSite) PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__2, obj1, str);
        // ISSUE: reference to a compiler-generated field
        if (PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "sales_sum", typeof (PolyCloud), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__3.Target((CallSite) PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__3, obj1, clientByUidAndSum.CurrentSalesSum);
        // ISSUE: reference to a compiler-generated field
        if (PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "credit_sum", typeof (PolyCloud), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__4.Target((CallSite) PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__4, obj1, clientByUidAndSum.CurrentCreditSum);
        // ISSUE: reference to a compiler-generated field
        if (PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__5 = CallSite<Func<CallSite, PolyCloud, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "SendPostToPolyCloud", (IEnumerable<System.Type>) null, typeof (PolyCloud), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__5.Target((CallSite) PolyCloud.\u003C\u003Eo__3.\u003C\u003Ep__5, this, obj1, "/post_sp_data");
      }
    }

    public Response SendRequest(Request request)
    {
      if (!this.CloudConfig.IsActive)
        return (Response) null;
      string str;
      switch (request.data)
      {
        case Card _:
          str = "/hook_card";
          break;
        case Gbs.Helpers.ExternalApi.PolycardCloud.Entities.Client _:
          str = "/hook_client";
          break;
        case LoyaltyGroup _:
          str = "/hook_loyalty_group";
          break;
        default:
          throw new ArgumentOutOfRangeException("data");
      }
      string urlSuffix = str;
      return JsonConvert.DeserializeObject<Response>(this.SendPostToPolyCloud((object) request, urlSuffix));
    }

    private string SendPostToPolyCloud(object request, string urlSuffix)
    {
      string requestUriString = "https://cloud.polycard.club/api/v1/web_service/call" + urlSuffix;
      string decryptedValue = this.CloudConfig.Token.DecryptedValue;
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Headers.Add("x-token: " + decryptedValue);
      httpWebRequest.Accept = "application/json";
      using (Stream requestStream = httpWebRequest.GetRequestStream())
      {
        using (StreamWriter streamWriter = new StreamWriter(requestStream))
        {
          string str = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings()
          {
            NullValueHandling = NullValueHandling.Ignore
          });
          LogHelper.Debug("url: " + requestUriString + "; data: " + str);
          streamWriter.Write(str);
        }
      }
      using (WebResponse response = httpWebRequest.GetResponse())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
          {
            string end = streamReader.ReadToEnd();
            LogHelper.Debug("Poly cloud api answer: " + Other.NewLine() + end);
            return end;
          }
        }
      }
    }

    public Decimal GetCardBonuses(string cardCode)
    {
      if (!new ConfigsRepository<Integrations>().Get().PolyCloud.IsActive)
        return 0M;
      string str = UidDb.GetUid().EntityUid.ToString();
      string requestUriString = "https://cloud.polycard.club/api/v1/web_service/call/get_sp_data?action=exclude&code=" + cardCode + "&sales_point_ext_id=" + str;
      string decryptedValue = this.CloudConfig.Token.DecryptedValue;
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
      httpWebRequest.Method = "GET";
      httpWebRequest.Headers.Add("x-token: " + decryptedValue);
      using (WebResponse response = httpWebRequest.GetResponse())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
          {
            string end = streamReader.ReadToEnd();
            LogHelper.Debug("Poly cloud api answer: " + Other.NewLine() + end);
          }
        }
      }
      throw new NotImplementedException();
    }

    public Gbs.Core.Entities.Clients.Client GetClientByCard(string card)
    {
      if (!this.CloudConfig.IsActive)
        return (Gbs.Core.Entities.Clients.Client) null;
      string requestUriString = "https://cloud.polycard.club/api/v1/web_service/call/get_client?code=" + card;
      string decryptedValue = this.CloudConfig.Token.DecryptedValue;
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
      httpWebRequest.Method = "GET";
      httpWebRequest.Headers.Add("x-token: " + decryptedValue);
      httpWebRequest.Accept = "application/json";
      LogHelper.Debug(string.Format("GET request to Poly. Uri: {0}", (object) httpWebRequest.RequestUri));
      using (WebResponse response = httpWebRequest.GetResponse())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
            LogHelper.Debug("Poly cloud api answer: \\r\\n" + streamReader.ReadToEnd());
        }
      }
      return (Gbs.Core.Entities.Clients.Client) null;
    }
  }
}
