// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Clients.CLIENTS_GROUPS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Clients
{
  public class CLIENTS_GROUPS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column(Length = 200)]
    [NotNull]
    public string NAME { get; set; } = string.Empty;

    [Column(Precision = 4, Scale = 2)]
    [NotNull]
    public Decimal DISCOUNT { get; set; }

    [Column(DataType = DataType.Money, Precision = 18, Scale = 4)]
    [NotNull]
    public Decimal MAX_SUM_CREDIT { get; set; }

    [Column]
    [NotNull]
    public Guid PRICE_UID { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_SUPPLIER { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_NON_USE_BONUS { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }
  }
}
