// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V20
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings.BackEnd;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V20 : ICorrection
  {
    public bool Do()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Gbs.Core.Config.Crpt crpt = new ConfigsRepository<Integrations>().Get().Crpt;
        List<GlobalDictionaries.RuMarkedProductionTypes> list1 = new GoodGroupsRepository(dataBase).GetActiveItems().GroupBy<GoodGroups.Group, GlobalDictionaries.RuMarkedProductionTypes>((Func<GoodGroups.Group, GlobalDictionaries.RuMarkedProductionTypes>) (x => x.RuMarkedProductionType)).Select<IGrouping<GlobalDictionaries.RuMarkedProductionTypes, GoodGroups.Group>, GlobalDictionaries.RuMarkedProductionTypes>((Func<IGrouping<GlobalDictionaries.RuMarkedProductionTypes, GoodGroups.Group>, GlobalDictionaries.RuMarkedProductionTypes>) (x => x.Key)).Where<GlobalDictionaries.RuMarkedProductionTypes>((Func<GlobalDictionaries.RuMarkedProductionTypes, bool>) (x => !x.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol, GlobalDictionaries.RuMarkedProductionTypes.Kz_Shoes))).ToList<GlobalDictionaries.RuMarkedProductionTypes>();
        List<MarkGroupSettings> items = new List<MarkGroupSettings>();
        foreach (KeyValuePair<MarkGroupEnum, bool> markGroup in crpt.MarkGroups)
          items.Add(new MarkGroupSettings()
          {
            Group = markGroup.Key,
            IsAllowedMode = markGroup.Value,
            IsCheckOwner = crpt.IsCheckOwnerForMark,
            IsActive = true
          });
        if (list1.Any<GlobalDictionaries.RuMarkedProductionTypes>((Func<GlobalDictionaries.RuMarkedProductionTypes, bool>) (x => x == GlobalDictionaries.RuMarkedProductionTypes.Other)) || !list1.Any<GlobalDictionaries.RuMarkedProductionTypes>())
        {
          new MarkGroupRepository().Save(items);
          return true;
        }
        List<MarkGroupEnum> list2 = list1.Select<GlobalDictionaries.RuMarkedProductionTypes, List<MarkGroupEnum>>(new Func<GlobalDictionaries.RuMarkedProductionTypes, List<MarkGroupEnum>>(GetCrptMarkGroup)).SelectMany<List<MarkGroupEnum>, MarkGroupEnum>((Func<List<MarkGroupEnum>, IEnumerable<MarkGroupEnum>>) (x => (IEnumerable<MarkGroupEnum>) x)).ToList<MarkGroupEnum>();
        foreach (MarkGroupSettings markGroupSettings in items)
        {
          MarkGroupSettings item = markGroupSettings;
          if (list2.All<MarkGroupEnum>((Func<MarkGroupEnum, bool>) (x => x != item.Group)))
          {
            item.IsActive = false;
            item.IsAllowedMode = false;
            item.IsCheckOwner = false;
          }
        }
        new MarkGroupRepository().Save(items);
        return true;
      }

      static List<MarkGroupEnum> GetCrptMarkGroup(GlobalDictionaries.RuMarkedProductionTypes type)
      {
        switch (type)
        {
          case GlobalDictionaries.RuMarkedProductionTypes.Fur:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Furs
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Drugs:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Pharma
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Tobacco:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Tobacco,
              MarkGroupEnum.Otp,
              MarkGroupEnum.Ncp
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Shoes:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Shoes
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Perfume:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Perfumery
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Tires:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Tires
            };
          case GlobalDictionaries.RuMarkedProductionTypes.LightIndustry:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Lp
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Milk:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Milk
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Water:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Water
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Photo:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Electronics
            };
          case GlobalDictionaries.RuMarkedProductionTypes.Alcohol:
            return new List<MarkGroupEnum>()
            {
              MarkGroupEnum.Beer,
              MarkGroupEnum.NaBeer
            };
          default:
            return new List<MarkGroupEnum>();
        }
      }
    }
  }
}
