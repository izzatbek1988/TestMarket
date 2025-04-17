// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Crpt
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class Crpt
  {
    public string Token { get; set; }

    public Decimal Timeout { get; set; } = 5M;

    public Decimal CountHoursForBanSaleWithDate { get; set; }

    [Obsolete("Устаревшая опция. Не использовать. Опция перенесена в БД")]
    public bool IsCheckOwnerForMark { get; set; }

    public bool IsBanSaleIfFailConnect { get; set; }

    public bool IsCheckTobaccoPriceForMark { get; set; }

    [Obsolete("Устаревшая опция. Не использовать. Опция перенесена в БД")]
    public Dictionary<MarkGroupEnum, bool> MarkGroups { get; set; } = new Dictionary<MarkGroupEnum, bool>()
    {
      {
        MarkGroupEnum.Lp,
        true
      },
      {
        MarkGroupEnum.Shoes,
        true
      },
      {
        MarkGroupEnum.Tobacco,
        true
      },
      {
        MarkGroupEnum.Perfumery,
        true
      },
      {
        MarkGroupEnum.Tires,
        true
      },
      {
        MarkGroupEnum.Electronics,
        true
      },
      {
        MarkGroupEnum.Pharma,
        true
      },
      {
        MarkGroupEnum.Milk,
        true
      },
      {
        MarkGroupEnum.Bicycle,
        true
      },
      {
        MarkGroupEnum.Wheelchairs,
        true
      },
      {
        MarkGroupEnum.Otp,
        true
      },
      {
        MarkGroupEnum.Water,
        true
      },
      {
        MarkGroupEnum.Furs,
        true
      },
      {
        MarkGroupEnum.Beer,
        true
      },
      {
        MarkGroupEnum.Ncp,
        true
      },
      {
        MarkGroupEnum.Bio,
        true
      },
      {
        MarkGroupEnum.Antiseptic,
        true
      },
      {
        MarkGroupEnum.NaBeer,
        true
      },
      {
        MarkGroupEnum.SoftDrinks,
        true
      },
      {
        MarkGroupEnum.PetFood,
        true
      },
      {
        MarkGroupEnum.SeaFood,
        true
      },
      {
        MarkGroupEnum.VetPharma,
        true
      },
      {
        MarkGroupEnum.Conserve,
        true
      },
      {
        MarkGroupEnum.VegetableOil,
        true
      }
    };
  }
}
