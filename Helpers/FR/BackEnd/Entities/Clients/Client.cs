// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.Clients.Client
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.Clients
{
  public class Client
  {
    public ClientGroups Group { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Barcode { get; set; } = string.Empty;

    public DateTime? Birthday { get; set; }

    public string Comment { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public Decimal SalesSum { get; set; }

    public Decimal CreditSum { get; set; }

    public Decimal Bonuses { get; set; }

    public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
  }
}
