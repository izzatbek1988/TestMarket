// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ClientOrder
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.FR.BackEnd.Entities.Clients;
using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities
{
  [Serializable]
  public class ClientOrder
  {
    public Guid Uid { get; set; }

    public string Number { get; set; }

    public DateTime DateCreate { get; set; }

    public DateTime DateClose { get; set; }

    public string Status { get; set; }

    public Client Client { get; set; }

    public string Comment { get; set; }

    public Decimal SumPayment { get; set; }
  }
}
