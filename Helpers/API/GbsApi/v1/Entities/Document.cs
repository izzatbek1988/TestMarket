// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.Document
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class Document : IEntity
  {
    public EntityTypes Type => EntityTypes.Document;

    public Guid Uid { get; set; }

    public Guid ParentUid { get; set; }

    public bool IsDeleted { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    public DocumentTypes DocumentType { get; set; }

    public DateTime DateTime { get; set; }

    public List<DocumentItemSimple> Items { get; set; } = new List<DocumentItemSimple>();

    public List<PaymentItem> Payments { get; set; } = new List<PaymentItem>();

    public Document(Gbs.Core.Entities.Documents.Document doc)
    {
      this.Uid = doc.Uid;
      this.ParentUid = doc.ParentUid;
      this.IsDeleted = doc.IsDeleted;
      this.DateTime = doc.DateTime;
      DocumentTypes documentTypes;
      switch (doc.Type)
      {
        case GlobalDictionaries.DocumentsTypes.Sale:
          documentTypes = DocumentTypes.Sale;
          break;
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
          documentTypes = DocumentTypes.SaleReturn;
          break;
        case GlobalDictionaries.DocumentsTypes.Buy:
          documentTypes = DocumentTypes.Buy;
          break;
        default:
          documentTypes = DocumentTypes.Unknown;
          break;
      }
      this.DocumentType = documentTypes;
      this.Items = doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Select<Gbs.Core.Entities.Documents.Item, DocumentItemSimple>((Func<Gbs.Core.Entities.Documents.Item, DocumentItemSimple>) (x => new DocumentItemSimple(x))).ToList<DocumentItemSimple>();
      this.Payments = doc.Payments.Select<Gbs.Core.Entities.Payments.Payment, PaymentItem>((Func<Gbs.Core.Entities.Payments.Payment, PaymentItem>) (x => new PaymentItem(x))).Where<PaymentItem>((Func<PaymentItem, bool>) (x => !x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment))).ToList<PaymentItem>();
    }
  }
}
