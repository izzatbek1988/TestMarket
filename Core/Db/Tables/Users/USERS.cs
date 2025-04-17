// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Users.USERS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;
using System.ComponentModel;

#nullable disable
namespace Gbs.Core.Db.Users
{
  public class USERS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column("GROUP_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid GROUP_UID { get; set; }

    [Column("CLIENT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid CLIENT_UID { get; set; }

    [Column(Length = 100)]
    [NotNull]
    public string ALIAS { get; set; } = string.Empty;

    [Column(Length = 30)]
    [NotNull]
    public string PASSWORD { get; set; } = string.Empty;

    [Column(Length = 100)]
    [NotNull]
    public string LOGIN_FOR_KKM { get; set; } = string.Empty;

    [Column(Length = 30)]
    [NotNull]
    public string PASSWORD_FOR_KKM { get; set; } = string.Empty;

    [Column(Length = 50)]
    [NotNull]
    public string BARCODE { get; set; } = string.Empty;

    [Column(Precision = 4, Scale = 2)]
    [NotNull]
    [DefaultValue("12")]
    public Decimal FONT_SIZE { get; set; }

    [Column]
    [NotNull]
    public DateTime DATE_IN { get; set; }

    [Column]
    [NotNull]
    public DateTime DATE_OUT { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_KICKED { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }

    [Column("SECTION_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid SECTION_UID { get; set; }
  }
}
