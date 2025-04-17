// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.ClientHome
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class ClientHome
  {
    public Guid Uid { get; set; }

    public Guid GroupUid { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateAdd { get; set; }

    public DateTime? Birthday { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string Barcode { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public List<EntityProperties.PropertyValue> Properties { get; set; } = new List<EntityProperties.PropertyValue>();

    public ClientHome()
    {
    }

    public ClientHome(Client client)
    {
      this.Name = client.Name;
      this.Birthday = client.Birthday;
      this.Comment = client.Comment;
      this.Barcode = client.Barcode;
      this.Address = client.Address;
      this.Email = client.Email;
      this.Phone = client.Phone;
      Gbs.Core.Entities.Clients.Group group = client.Group;
      // ISSUE: explicit non-virtual call
      this.GroupUid = group != null ? __nonvirtual (group.Uid) : Guid.Empty;
      this.Uid = client.Uid;
      this.IsDeleted = client.IsDeleted;
      this.Properties = client.Properties.ToList<EntityProperties.PropertyValue>();
      this.DateAdd = client.DateAdd;
    }
  }
}
