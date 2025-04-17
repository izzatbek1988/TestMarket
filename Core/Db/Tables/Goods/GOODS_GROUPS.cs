// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Goods.GOODS_GROUPS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;
using System.ComponentModel;

#nullable disable
namespace Gbs.Core.Db.Goods
{
  public class GOODS_GROUPS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column(Length = 200)]
    [NotNull]
    public string NAME { get; set; } = string.Empty;

    [Column("PARENT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid PARENT_UID { get; set; }

    [Column]
    [NotNull]
    public int GOODS_TYPE { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DATA_PARENT { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_COMPOSITE_GOOD { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_REQUEST_COUNT { get; set; }

    [Column("UNITS_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid UNITS_UID { get; set; }

    [Column]
    [NotNull]
    public int RU_TAX_SYSTEM { get; set; }

    [Column]
    [NotNull]
    public int RU_MARKED_PRODUCTION_TYPE { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool NEED_COMMENT { get; set; }

    [Column]
    [NotNull]
    [DefaultValue("1")]
    public int KKM_TAX_NUMBER { get; set; }

    [Column]
    [NotNull]
    [DefaultValue("1")]
    public int KKM_SECTION_NUMBER { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_FREE_PRICE { get; set; }

    [Column]
    [NotNull]
    public int RU_FFD_GOODS_TYPE { get; set; }

    [Column]
    [NotNull]
    public int DECIMAL_PLACE { get; set; }
  }
}
