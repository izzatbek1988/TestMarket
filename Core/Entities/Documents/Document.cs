// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Documents.Document
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.HomeOffice.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Documents
{
  public class Document : Gbs.Core.Entities.Entity
  {
    public List<Item> Items { get; set; } = new List<Item>();

    public List<Gbs.Core.Entities.Payments.Payment> Payments { get; set; } = new List<Gbs.Core.Entities.Payments.Payment>();

    public List<EntityProperties.PropertyValue> Properties { get; set; } = new List<EntityProperties.PropertyValue>();

    public Guid ParentUid { get; set; } = Guid.Empty;

    [Required]
    public DateTime DateTime { get; set; } = DateTime.Now;

    [StringLength(20)]
    public string Number { get; set; } = string.Empty;

    [StringLength(200)]
    public string Comment { get; set; } = string.Empty;

    public GlobalDictionaries.DocumentsTypes Type { get; set; }

    public Guid UserUid { get; set; } = Guid.Empty;

    [Required]
    public Storages.Storage Storage { get; set; }

    [Required]
    public Sections.Section Section { get; set; }

    public Guid ContractorUid { get; set; } = Guid.Empty;

    public GlobalDictionaries.DocumentsStatuses Status { get; set; }

    public bool IsFiscal { get; set; }

    public Document()
    {
    }

    public Document(DocumentHome document)
    {
      this.Items = document.Items.Select<Gbs.Helpers.HomeOffice.Entity.Item, Item>((Func<Gbs.Helpers.HomeOffice.Entity.Item, Item>) (x => new Item(x, document.StorageUid))).ToList<Item>();
      this.Payments = document.Payments.Select<Gbs.Helpers.HomeOffice.Entity.Payment, Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Helpers.HomeOffice.Entity.Payment, Gbs.Core.Entities.Payments.Payment>) (x => new Gbs.Core.Entities.Payments.Payment(x))).ToList<Gbs.Core.Entities.Payments.Payment>();
      this.Comment = document.Comment;
      this.ContractorUid = document.ContractorUid;
      this.DateTime = document.DateTime;
      this.IsFiscal = document.IsFiscal;
      this.Number = document.Number;
      this.ParentUid = document.ParentUid;
      Sections.Section section = new Sections.Section();
      section.Uid = document.SectionUid;
      this.Section = section;
      this.Status = document.Status;
      Storages.Storage storage = new Storages.Storage();
      storage.Uid = document.StorageUid;
      this.Storage = storage;
      this.UserUid = document.UserUid;
      this.Type = document.Type;
      this.Uid = document.Uid;
      this.IsDeleted = document.IsDeleted;
    }

    public event Document.SaveHandler Saved;

    public void InvokeSaved()
    {
      Document.SaveHandler saved = this.Saved;
      if (saved == null)
        return;
      saved();
    }

    public delegate void SaveHandler();
  }
}
