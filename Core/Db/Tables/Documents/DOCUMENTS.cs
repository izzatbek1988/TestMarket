// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Db.Documents.DOCUMENTS
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using LinqToDB;
using LinqToDB.Mapping;
using System;

#nullable disable
namespace Gbs.Core.Db.Documents
{
  public class DOCUMENTS : DbTable
  {
    [Column("UID", DataType = DataType.VarChar, Length = 36)]
    [PrimaryKey]
    [NotNull]
    public Guid UID { get; set; }

    [Column("PARENT_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid PARENT_UID { get; set; }

    [Column("STORAGE_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid STORAGE_UID { get; set; }

    [Column("SECTION_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid SECTION_UID { get; set; }

    [Column]
    [NotNull]
    public DateTime DATE_TIME { get; set; }

    [Column(Length = 20)]
    [NotNull]
    public string NUMBER { get; set; } = string.Empty;

    [Column(Length = 200)]
    [NotNull]
    public string COMMENT { get; set; } = string.Empty;

    [Column]
    [NotNull]
    public int TYPE { get; set; }

    [Column]
    [NotNull]
    public int STATUS { get; set; }

    [Column("USER_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid USER_UID { get; set; }

    [Column("CONTRACTOR_UID", DataType = DataType.VarChar, Length = 36)]
    [NotNull]
    public Guid CONTRACTOR_UID { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_FISCAL { get; set; }

    [Column]
    [NotNull]
    [ValueConverter(ConverterType = typeof (BoolToStringConverter))]
    public bool IS_DELETED { get; set; }
  }
}
