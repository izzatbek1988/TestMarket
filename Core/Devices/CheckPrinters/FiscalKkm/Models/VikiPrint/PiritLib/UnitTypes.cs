// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.UnitTypes
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public enum UnitTypes
  {
    [EnumMember(Value = "шт.")] Pieces = 0,
    [EnumMember(Value = "г")] Gram = 10, // 0x0000000A
    [EnumMember(Value = "кг")] Kilogram = 11, // 0x0000000B
    [EnumMember(Value = "т")] Ton = 12, // 0x0000000C
    [EnumMember(Value = "см")] Centimeter = 20, // 0x00000014
    [EnumMember(Value = "дм")] Decimeter = 21, // 0x00000015
    [EnumMember(Value = "м")] Meter = 22, // 0x00000016
    [EnumMember(Value = "кв. с")] SquareCentimeter = 30, // 0x0000001E
    [EnumMember(Value = "кв. д")] SquareDecimeter = 31, // 0x0000001F
    [EnumMember(Value = "кв. м")] SquareMeter = 32, // 0x00000020
    [EnumMember(Value = "мл")] Milliliter = 40, // 0x00000028
    [EnumMember(Value = "л")] Liter = 41, // 0x00000029
    [EnumMember(Value = "куб.")] CubicMeter = 42, // 0x0000002A
    [EnumMember(Value = "кВт-ч")] KilowattHour = 50, // 0x00000032
    [EnumMember(Value = "Гкал")] Gigacaloria = 51, // 0x00000033
    [EnumMember(Value = "сутки")] Day = 70, // 0x00000046
    [EnumMember(Value = "час")] Hour = 71, // 0x00000047
    [EnumMember(Value = "мин")] Minute = 72, // 0x00000048
    [EnumMember(Value = "с")] Second = 73, // 0x00000049
    [EnumMember(Value = "Кбайт")] Kilobyte = 80, // 0x00000050
    [EnumMember(Value = "Мбайт")] Megabyte = 81, // 0x00000051
    [EnumMember(Value = "Гбайт")] Gigabyte = 82, // 0x00000052
    [EnumMember(Value = "Тбайт")] Terabyte = 83, // 0x00000053
  }
}
