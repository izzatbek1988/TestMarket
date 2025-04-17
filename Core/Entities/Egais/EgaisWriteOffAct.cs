// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Egais.EgaisWriteOffAct
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Egais;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Entities.Egais
{
  public class EgaisWriteOffAct : IEntity
  {
    public List<EgaisWriteOffActsItems> Items { get; set; } = new List<EgaisWriteOffActsItems>();

    public Guid Uid { get; set; } = Guid.NewGuid();

    public DateTime DateTime { get; set; }

    public Guid ReplayUid { get; set; }

    public string Comment { get; set; } = string.Empty;

    public EgaisWriteOffActStatus Status { get; set; } = EgaisWriteOffActStatus.Unknown;

    public TypeWriteOff1 Type { get; set; }

    public bool IsDeleted { get; set; }
  }
}
