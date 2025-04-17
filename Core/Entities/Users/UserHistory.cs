// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.UserHistory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Entities
{
  public class UserHistory : Entity
  {
    public Users.User User { get; set; }

    public DateTime DateIn { get; set; } = DateTime.Now;

    public DateTime DateOut { get; set; }

    public Guid SectionUid { get; set; } = Guid.Empty;
  }
}
