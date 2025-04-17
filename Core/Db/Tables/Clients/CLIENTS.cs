// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Clients.CLIENTS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Clients
{
  public class CLIENTS : DbTable
  {
    [Column]
    [NotNull]
    public DateTime DATE_ADD { get; set; }

    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column(Length = 200)]
    [NotNull]
    public string NAME { get; set; } = string.Empty;

    [Column("GROUP_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid GROUP_UID { get; set; }

    [Column]
    [NotNull]
    public DateTime BIRTHDAY { get; set; }

    [Column(Length = 300)]
    [NotNull]
    public string COMMENT { get; set; } = string.Empty;

    [Column(Length = 100)]
    [NotNull]
    public string BARCODE { get; set; } = string.Empty;

    [Column(Length = 300)]
    [NotNull]
    public string ADDRESS { get; set; } = string.Empty;

    [Column(Length = 100)]
    [NotNull]
    public string PHONE { get; set; } = string.Empty;

    [Column(Length = 100)]
    [NotNull]
    public string EMAIL { get; set; } = string.Empty;

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }
  }
}
