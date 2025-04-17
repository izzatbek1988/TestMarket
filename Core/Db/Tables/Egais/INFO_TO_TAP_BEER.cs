// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Tables.Egais.INFO_TO_TAP_BEER
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Tables.Egais
{
  public class INFO_TO_TAP_BEER : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column("TAP_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid TAP_UID { get; set; }

    [Column("GOOD_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid GOOD_UID { get; set; }

    [Column("CHILD_GOOD_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid CHILD_GOOD_UID { get; set; }

    [Column("STORAGE_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid STORAGE_UID { get; set; }

    [Column("DOCUMENT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid DOCUMENT_UID { get; set; }

    [Column]
    [NotNull]
    public DateTime DATE_TIME { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_SEND_TO_CRPT { get; set; }

    [Column]
    [NotNull]
    public DateTime EXPIRATION_DATE { get; set; }

    [Column(Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal QUANTITY { get; set; }

    [Column(Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal PRICE { get; set; }

    [Column(Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal SALE_QUANTITY { get; set; }

    [Column]
    [NotNull]
    public string MARK_INFO { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }
  }
}
