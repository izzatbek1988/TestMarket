// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Egais.EgaisWriteOffActsItems
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Egais;
using System;

#nullable disable
namespace Gbs.Core.Entities.Egais
{
  public class EgaisWriteOffActsItems : IEntity
  {
    public Guid Uid { get; set; } = Guid.NewGuid();

    public bool IsDeleted { get; set; }

    public Guid ActUid { get; set; } = Guid.Empty;

    public Guid StockUid { get; set; } = Guid.Empty;

    public Decimal Quantity { get; set; }

    public Decimal Sum { get; set; }

    public string FbNumber { get; set; }

    public string MarkInfo { get; set; }

    public TypeWriteOff1 ActType { get; set; }
  }
}
