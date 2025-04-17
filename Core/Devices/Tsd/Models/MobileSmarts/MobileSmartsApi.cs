// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Tsd.Models.MobileSmartsApi
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace Gbs.Core.Devices.Tsd.Models
{
  public class MobileSmartsApi
  {
    private readonly LanConnection _lanConnection;

    public MobileSmartsApi(LanConnection lanConnection) => this._lanConnection = lanConnection;

    public void DoCommand(MobileSmartsApi.MobileSmartsCommand command)
    {
      RestHelper restHelper = new RestHelper(this._lanConnection.UrlAddress, this._lanConnection.PortNumber, command.ToJsonString());
      restHelper.CreateCommand(command.Path, command.Type);
      restHelper.SetAccept(command.Accept);
      restHelper.DoCommand();
      command.AnswerString = restHelper.Answer;
      if (restHelper.StatusCode != HttpStatusCode.OK)
      {
        Exception exception;
        try
        {
          exception = JsonConvert.DeserializeObject<Exception>(restHelper.Answer);
        }
        catch
        {
          throw new Exception(Translate.MobileSmartsApi_DoCommand_ошибкаПередачиДанныхВТСД);
        }
        throw new DeviceException(exception?.InnerException?.Message ?? Translate.MobileSmartsApi_DoCommand_Возникла_ошибка_при_передаче_данных_на_ТСД_);
      }
    }

    public class MobileSmartsCommand : RestHelper.RestCommand
    {
      [JsonIgnore]
      public virtual string Accept { get; }
    }

    public class BeginOverwriteCommand : MobileSmartsApi.MobileSmartsCommand
    {
      public override string Accept => "text/html";

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/MobileSMARTS/api/v1/Products/BeginOverwrite";
    }

    public class EndOverwriteCommand : MobileSmartsApi.MobileSmartsCommand
    {
      public override string Accept => "text/html";

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/MobileSMARTS/api/v1/Products/EndOverwrite";
    }

    public class EndUpdateDocumentCommand : MobileSmartsApi.MobileSmartsCommand
    {
      [JsonIgnore]
      public string DocumentId { get; set; }

      public override string Accept => "text/html";

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path
      {
        get => "/MobileSMARTS/api/v1/Docs('" + this.DocumentId + "')/EndUpdate";
      }
    }

    public class AddProductsCommand : MobileSmartsApi.MobileSmartsCommand
    {
      public override string Accept => "application/json";

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/MobileSMARTS/api/v1/Products";

      [JsonProperty("id")]
      public string Uid { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("barcode")]
      public string Barcode { get; set; }

      [JsonProperty("basePackingId")]
      public string BasePackingId => "0";

      [JsonProperty("packings")]
      public List<MobileSmartsApi.AddProductsCommand.Packing> Packings { get; set; } = new List<MobileSmartsApi.AddProductsCommand.Packing>();

      [JsonProperty("qty")]
      public Decimal Count { get; set; }

      [JsonProperty("price")]
      public Decimal Price { get; set; }

      public class Packing
      {
        [JsonProperty("id")]
        public string Id => "0";

        [JsonProperty("name")]
        public string UnitName { get; set; }

        [JsonProperty("barcode")]
        public string UnitBarcode { get; set; }
      }
    }

    public class CreatedInventoryCommand : MobileSmartsApi.MobileSmartsCommand
    {
      public override string Accept => "application/json";

      public override TypeRestRequest Type => TypeRestRequest.Post;

      public override string Path => "/MobileSMARTS/api/v1/Docs/Inventarizaciya";

      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("documentTypeName")]
      public string DocumentType { get; set; } = "Инвентаризация";

      [JsonProperty("warehouseId")]
      public string WarehouseId { get; set; } = "1";

      [JsonProperty("declaredItems")]
      public List<MobileSmartsApi.CreatedInventoryCommand.DocumentItem> Items { get; set; } = new List<MobileSmartsApi.CreatedInventoryCommand.DocumentItem>();

      public class DocumentItem
      {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("productBarcode")]
        public string Barcode { get; set; }

        [JsonProperty("osnShK")]
        public string MainBarcode { get; set; }

        [JsonProperty("shK")]
        public string OtherBarcode { get; set; }

        [JsonProperty("packingId")]
        public string PackingId => "0";

        [JsonProperty("declaredQuantity")]
        public Decimal DeclaredQuantity { get; set; }

        [JsonProperty("currentQuantity")]
        public Decimal CurrentQuantity { get; set; }
      }
    }

    public class GetInventoryCommand : MobileSmartsApi.MobileSmartsCommand
    {
      public string DocumentId { get; set; }

      public override string Accept => "application/json";

      public override TypeRestRequest Type => TypeRestRequest.Get;

      public override string Path
      {
        get => "/MobileSMARTS/api/v1/Docs/Inventarizaciya('" + this.DocumentId + "')/declaredItems";
      }

      [JsonIgnore]
      public MobileSmartsApi.GetInventoryCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<MobileSmartsApi.GetInventoryCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("value")]
        public List<MobileSmartsApi.CreatedInventoryCommand.DocumentItem> Items { get; set; }
      }
    }

    public class BaseInfoCommand : MobileSmartsApi.MobileSmartsCommand
    {
      public override string Accept => "application/json";

      public override TypeRestRequest Type => TypeRestRequest.Get;

      public override string Path => "/MobileSMARTS/api/v1/BaseInfo";

      [JsonIgnore]
      public MobileSmartsApi.BaseInfoCommand.Answer Result
      {
        get
        {
          return JsonConvert.DeserializeObject<MobileSmartsApi.BaseInfoCommand.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("serverVersion")]
        public string ServerVersion { get; set; }
      }
    }
  }
}
