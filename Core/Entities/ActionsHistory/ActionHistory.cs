// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.ActionHistory
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Entities
{
  public class ActionHistory : Entity
  {
    public DateTime DateTime { get; set; } = DateTime.Now;

    public GlobalDictionaries.EntityTypes EntityType { get; set; }

    public Guid EntityUid { get; set; }

    public ActionType ActionType { get; set; }

    public List<string> Data { get; set; } = new List<string>();

    public string DataLine => string.Join("", (IEnumerable<string>) this.Data);

    public Users.User User { get; set; }

    public Sections.Section Section { get; set; }
  }
}
