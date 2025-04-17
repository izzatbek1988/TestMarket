// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Entity
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Gbs.Core.Entities
{
  [Serializable]
  public abstract class Entity : IEntity
  {
    public ActionResult DataValidation() => ValidationHelper.DataValidation(this);

    [Required]
    public Guid Uid { get; set; } = Guid.NewGuid();

    public bool IsDeleted { get; set; }
  }
}
