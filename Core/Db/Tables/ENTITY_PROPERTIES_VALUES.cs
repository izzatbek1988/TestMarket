// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.ENTITY_PROPERTIES_VALUES
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db
{
  public class ENTITY_PROPERTIES_VALUES : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column(Length = 1000)]
    [NotNull]
    public string CONTENT { get; set; } = string.Empty;

    [Column("TYPE_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid TYPE_UID { get; set; }

    [Column("ENTITY_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid ENTITY_UID { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }
  }
}
