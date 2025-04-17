// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Goods.GOODS_SETS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Goods
{
  public class GOODS_SETS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column("PARENT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid PARENT_UID { get; set; }

    [Column("MODIFICATION_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid MODIFICATION_UID { get; set; }

    [Column("GOOD_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid GOOD_UID { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }

    [Column(Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal QUANTITY { get; set; }

    [Column(Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal DISCOUNT { get; set; }
  }
}
