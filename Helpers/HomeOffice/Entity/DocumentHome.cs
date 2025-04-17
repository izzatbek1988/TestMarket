// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.DocumentHome
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class DocumentHome
  {
    public Guid Uid { get; set; }

    public List<Item> Items { get; set; }

    public List<Payment> Payments { get; set; }

    public Guid ParentUid { get; set; }

    public DateTime DateTime { get; set; }

    public string Number { get; set; }

    public string Comment { get; set; }

    public GlobalDictionaries.DocumentsTypes Type { get; set; }

    public Guid UserUid { get; set; }

    public Guid StorageUid { get; set; }

    public Guid SectionUid { get; set; }

    public Guid ContractorUid { get; set; }

    public GlobalDictionaries.DocumentsStatuses Status { get; set; }

    public bool IsFiscal { get; set; }

    public bool IsDeleted { get; set; }

    public DocumentHome(Document doc)
    {
      this.Comment = doc.Comment;
      this.ContractorUid = doc.ContractorUid;
      this.DateTime = doc.DateTime;
      this.IsFiscal = doc.IsFiscal;
      this.Number = doc.Number;
      this.ParentUid = doc.ParentUid;
      this.SectionUid = doc.Section.Uid;
      this.Status = doc.Status;
      this.StorageUid = doc.Storage.Uid;
      this.UserUid = doc.UserUid;
      this.Type = doc.Type;
      this.Uid = doc.Uid;
      this.IsDeleted = doc.IsDeleted;
      this.Items = doc.Items.Select<Gbs.Core.Entities.Documents.Item, Item>((Func<Gbs.Core.Entities.Documents.Item, Item>) (x => new Item(x))).ToList<Item>();
      this.Payments = doc.Payments.Select<Gbs.Core.Entities.Payments.Payment, Payment>((Func<Gbs.Core.Entities.Payments.Payment, Payment>) (x => new Payment(x))).ToList<Payment>();
    }

    public DocumentHome()
    {
    }
  }
}
