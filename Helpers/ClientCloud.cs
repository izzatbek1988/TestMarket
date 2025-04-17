// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ClientCloud
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers
{
  [JsonObject]
  public class ClientCloud : Entity
  {
    public DateTime DateAdd { get; set; }

    public Gbs.Core.Entities.Clients.Group Group { get; set; }

    public string Name { get; set; }

    public DateTime? Birthday { get; set; }

    public string Comment { get; set; }

    public string Barcode { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public Decimal SalesSum { get; set; }

    public Decimal CreditSum { get; set; }

    public Decimal Bonuses { get; set; }

    public Dictionary<Guid, object> Properties { get; set; }

    public ClientCloud(ClientAdnSum client)
    {
      this.Uid = client.Client.Uid;
      this.Name = client.Client.Name;
      this.Birthday = client.Client.Birthday;
      this.Comment = client.Client.Comment;
      this.Barcode = client.Client.Barcode;
      this.Address = client.Client.Address;
      this.Email = client.Client.Email;
      this.Phone = client.Client.Phone;
      this.Group = client.Client.Group;
      this.SalesSum = client.CurrentSalesSum;
      this.CreditSum = client.CurrentCreditSum;
      this.Bonuses = client.CurrentBonusSum;
      this.Properties = client.Client.PropertiesDictionary;
      this.DateAdd = client.Client.DateAdd;
    }

    public ClientCloud()
    {
    }
  }
}
