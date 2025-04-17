// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.ClientHomeList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class ClientHomeList
  {
    public List<ClientHome> ClientList { get; set; }

    public ClientHomeList(List<Client> client)
    {
      this.ClientList = client.Select<Client, ClientHome>((Func<Client, ClientHome>) (x => new ClientHome(x))).ToList<ClientHome>();
    }

    public ClientHomeList()
    {
    }
  }
}
