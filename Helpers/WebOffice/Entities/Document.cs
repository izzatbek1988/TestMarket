// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WebOffice.Document
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
namespace Gbs.Helpers.WebOffice
{
  public class Document : IEntity
  {
    public string Number { get; set; } = "";

    public Guid Uid { get; set; }

    public Guid ParentUid { get; set; }

    public bool IsDeleted { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    public DocumentTypes Type { get; set; }

    public DateTime DateTime { get; set; }

    public List<PaymentItem> Payments { get; set; }

    public List<DocumentItemSimple> Items { get; set; }

    public ClientSimple Client { get; set; }

    public UserSimple User { get; set; }

    public Section Section { get; set; }

    public Document(Gbs.Core.Entities.Documents.Document doc)
    {
      this.Uid = doc.Uid;
      this.ParentUid = doc.ParentUid;
      this.IsDeleted = doc.IsDeleted;
      this.DateTime = doc.DateTime;
      this.Number = doc.Number;
      this.Client = new ClientSimple()
      {
        Uid = doc.ContractorUid
      };
      this.User = new UserSimple() { Uid = doc.UserUid };
      this.Section = new Section()
      {
        Uid = doc.Section.Uid,
        Name = doc.Section.Name,
        IsDeleted = doc.Section.IsDeleted
      };
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
      this.Type = documentTypes;
      this.Items = doc.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => !x.IsDeleted)).Select<Gbs.Core.Entities.Documents.Item, DocumentItemSimple>((Func<Gbs.Core.Entities.Documents.Item, DocumentItemSimple>) (x => new DocumentItemSimple(x))).ToList<DocumentItemSimple>();
      this.Payments = doc.Payments.Select<Gbs.Core.Entities.Payments.Payment, PaymentItem>((Func<Gbs.Core.Entities.Payments.Payment, PaymentItem>) (x => new PaymentItem(x))).Where<PaymentItem>((Func<PaymentItem, bool>) (x => !x.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment))).ToList<PaymentItem>();
    }
  }
}
