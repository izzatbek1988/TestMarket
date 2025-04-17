// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.LINK_ENTITIES
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db
{
  internal class LINK_ENTITIES : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column("ENTITY_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid ENTITY_UID { get; set; }

    [Column]
    [NotNull]
    public int ID { get; set; }

    [Column]
    [NotNull]
    public int TYPE { get; set; }
  }
}
