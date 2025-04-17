// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.SelectGood
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Goods;
using System;

#nullable disable
namespace Gbs.Core.Entities
{
  public class SelectGood : IEntity
  {
    public Guid ParentUid { get; set; } = Guid.Empty;

    public int Index { get; set; }

    public string DisplayName { get; set; }

    public Good Good { get; set; }

    public Guid Uid { get; set; } = Guid.NewGuid();

    public bool IsDeleted { get; set; }
  }
}
