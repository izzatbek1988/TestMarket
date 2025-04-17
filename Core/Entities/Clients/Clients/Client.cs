// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Clients.Client
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db.Clients;
using Gbs.Helpers;
using Gbs.Helpers.HomeOffice.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities.Clients
{
  [Localizable(false)]
  public class Client : Gbs.Core.Entities.Entity
  {
    [JsonIgnore]
    private Dictionary<Guid, object> _propertiesDictionary = new Dictionary<Guid, object>();

    public DateTime DateAdd { get; set; } = DateTime.Now;

    [Required]
    [ValidationHelper.ValidateObject]
    public Group Group { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateTime? Birthday { get; set; }

    [StringLength(300)]
    public string Comment { get; set; } = string.Empty;

    [StringLength(100)]
    public string Barcode { get; set; } = string.Empty;

    [StringLength(300)]
    public string Address { get; set; } = string.Empty;

    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    public string Phone { get; set; } = string.Empty;

    public List<EntityProperties.PropertyValue> Properties { get; set; } = new List<EntityProperties.PropertyValue>();

    [JsonIgnore]
    public Dictionary<Guid, object> PropertiesDictionary
    {
      get
      {
        foreach (EntityProperties.PropertyValue property in this.Properties)
          this._propertiesDictionary[property.Type.Uid] = property.Value;
        return this._propertiesDictionary;
      }
      set => this._propertiesDictionary = value;
    }

    public Client()
    {
    }

    public Client(CLIENTS dbItem)
    {
      this.Uid = dbItem.UID;
      this.IsDeleted = dbItem.IS_DELETED;
      this.Name = dbItem.NAME;
      this.Comment = dbItem.COMMENT;
      this.Address = dbItem.ADDRESS;
      this.Barcode = dbItem.BARCODE;
      this.Email = dbItem.EMAIL;
      this.Phone = dbItem.PHONE;
      this.DateAdd = dbItem.DATE_ADD;
      if (!(dbItem.BIRTHDAY != new DateTime(1, 1, 1)))
        return;
      this.Birthday = new DateTime?(dbItem.BIRTHDAY);
    }

    public Client(ClientHome clientHome)
    {
      this.Uid = clientHome.Uid;
      this.IsDeleted = clientHome.IsDeleted;
      this.Name = clientHome.Name;
      this.Comment = clientHome.Comment;
      this.Address = clientHome.Address;
      this.Barcode = clientHome.Barcode;
      this.Email = clientHome.Email;
      this.Phone = clientHome.Phone;
      this.Properties = clientHome.Properties;
      Group group = new Group();
      group.Uid = clientHome.GroupUid;
      group.Name = "заполняется для прохождения валидации";
      this.Group = group;
      this.DateAdd = clientHome.DateAdd;
      DateTime? birthday = clientHome.Birthday;
      DateTime dateTime = new DateTime(1, 1, 1);
      if ((birthday.HasValue ? (birthday.GetValueOrDefault() != dateTime ? 1 : 0) : 1) == 0)
        return;
      this.Birthday = clientHome.Birthday;
    }

    public string GetInn()
    {
      return this.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
    }
  }
}
