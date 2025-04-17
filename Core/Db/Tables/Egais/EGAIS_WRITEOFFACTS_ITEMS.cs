// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Tables.Egais.EGAIS_WRITEOFFACTS_ITEMS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Tables.Egais
{
  public class EGAIS_WRITEOFFACTS_ITEMS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column("ACT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid ACT_UID { get; set; }

    [Column("STOCK_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid STOCK_UID { get; set; }

    [Column(Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal QUANTITY { get; set; }

    [Column]
    [NotNull]
    public string FB_NUMBER { get; set; }

    [Column]
    [NotNull]
    public string MARK_INFO { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }

    [Column(DataType = DataType.Money, Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal SUM_ITEM { get; set; }

    [Column]
    [NotNull]
    public int TYPE { get; set; }
  }
}
