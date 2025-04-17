// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Payments.PAYMENTS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Payments
{
  public class PAYMENTS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column]
    [NotNull]
    public DateTime DATE_TIME { get; set; }

    [Column]
    [NotNull]
    public int TYPE { get; set; }

    [Column(DataType = DataType.Money, Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal SUM_IN { get; set; }

    [Column(DataType = DataType.Money, Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal SUM_OUT { get; set; }

    [Column("ACCOUNT_IN_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid ACCOUNT_IN_UID { get; set; }

    [Column("SECTION_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid SECTION_UID { get; set; }

    [Column("ACCOUNT_OUT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid ACCOUNT_OUT_UID { get; set; }

    [Column("PARENT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid PARENT_UID { get; set; }

    [Column("METHOD_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid METHOD_UID { get; set; }

    [Column("PAYER_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid PAYER_UID { get; set; }

    [Column("USER_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid USER_UID { get; set; }

    [Column]
    [NotNull]
    public string COMMENT { get; set; } = string.Empty;

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }
  }
}
