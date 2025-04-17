// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.User
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities
{
  public class User
  {
    public Guid Uid { get; set; }

    public string Alias { get; set; }

    public string ClientName { get; set; }

    public User(Users.User user)
    {
      if (user == null)
        return;
      this.Uid = user.Uid;
      this.Alias = user.Alias;
      this.ClientName = user.Client.Name;
    }
  }
}
