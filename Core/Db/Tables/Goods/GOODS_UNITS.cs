// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Goods.GOODS_UNITS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Goods
{
  public class GOODS_UNITS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column(Length = 100)]
    [NotNull]
    public string FULL_NAME { get; set; } = string.Empty;

    [Column(Length = 100)]
    [NotNull]
    public string SHORT_NAME { get; set; } = string.Empty;

    [Column(Length = 100)]
    [NotNull]
    public string CODE { get; set; } = string.Empty;

    [Column]
    [NotNull]
    public int RU_FFD_UNITS_INDEX { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }
  }
}
