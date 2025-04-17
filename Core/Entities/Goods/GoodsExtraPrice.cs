// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GoodsExtraPrice
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GoodsExtraPrice
  {
    public static List<GoodsExtraPrice.GoodExtraPrice> GetGoodExtraPriceList(
      IQueryable<SETTINGS> query = null)
    {
      LogHelper.OnBegin();
      List<GoodsExtraPrice.GoodExtraPrice> goodExtraPriceList1 = new List<GoodsExtraPrice.GoodExtraPrice>();
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<SETTINGS> source = query;
        if (source == null)
          source = dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 10));
        foreach (SETTINGS settings in source.ToList<SETTINGS>().Where<SETTINGS>((Func<SETTINGS, bool>) (x => x.PARAM == "name")))
        {
          List<GoodsExtraPrice.GoodExtraPrice> goodExtraPriceList2 = goodExtraPriceList1;
          GoodsExtraPrice.GoodExtraPrice goodExtraPrice = new GoodsExtraPrice.GoodExtraPrice();
          goodExtraPrice.Uid = settings.ENTITY_UID;
          goodExtraPrice.Name = JsonConvert.DeserializeObject<string>(settings.VAL);
          goodExtraPrice.IsDeleted = settings.IS_DELETED;
          goodExtraPriceList2.Add(goodExtraPrice);
        }
      }
      LogHelper.OnEnd();
      return goodExtraPriceList1;
    }

    public static Dictionary<GoodsExtraPrice.TypeCoeff, string> DictionaryTypeCoeff
    {
      get
      {
        return new Dictionary<GoodsExtraPrice.TypeCoeff, string>()
        {
          {
            GoodsExtraPrice.TypeCoeff.SalePrice,
            Translate.GoodsExtraPrice_Розничная
          },
          {
            GoodsExtraPrice.TypeCoeff.BuyPrice,
            Translate.GoodsExtraPrice_Закупочная
          }
        };
      }
    }

    public enum TypeCoeff
    {
      BuyPrice,
      SalePrice,
    }

    public class GoodExtraPrice : Entity
    {
      public string Name { get; set; } = string.Empty;

      public (string Message, bool Result) DataVerify()
      {
        return this.Name.Length >= 3 ? (string.Empty, true) : (Translate.GoodExtraPrice_Название_доп__цены_должно_быть_не_менее_3_символов_, false);
      }

      public bool Save()
      {
        if (!this.DataVerify().Result)
        {
          MessageBoxHelper.Warning(this.DataVerify().Message);
          return false;
        }
        Setting setting = new Setting();
        setting.Type = Types.ExtraPrices;
        setting.EntityUid = this.Uid;
        setting.Parameter = "name";
        setting.Value = (object) this.Name;
        setting.IsDeleted = this.IsDeleted;
        new SettingsRepository().Save(setting);
        using (DataBase dataBase = Data.GetDataBase())
        {
          ExtraPriceRule extraPriceRule1 = new ExtraPriceRulesRepository().GetByUid(GlobalDictionaries.DefaultExtraRuleUid);
          if (extraPriceRule1 == null)
          {
            ExtraPriceRule extraPriceRule2 = new ExtraPriceRule();
            extraPriceRule2.Groups = new GoodGroupsRepository(dataBase).GetActiveItems();
            extraPriceRule2.Uid = GlobalDictionaries.DefaultExtraRuleUid;
            extraPriceRule2.Name = Translate.GoodExtraPrice_Правило_по_умолчанию;
            extraPriceRule1 = extraPriceRule2;
          }
          ExtraPriceRule extraPriceRule3 = extraPriceRule1;
          if (extraPriceRule3.Items.All<ExtraPriceRule.ItemPricing>((Func<ExtraPriceRule.ItemPricing, bool>) (x => x.Price.Uid != this.Uid)))
            extraPriceRule3.Items.Add(new ExtraPriceRule.ItemPricing()
            {
              Price = this,
              Value = 1M
            });
          if (!extraPriceRule3.Groups.Any<GoodGroups.Group>())
          {
            List<GoodGroups.Group> groups = extraPriceRule3.Groups;
            GoodGroups.Group group = new GoodGroups.Group();
            group.Uid = Guid.Empty;
            groups.Add(group);
          }
          new ExtraPriceRulesRepository().Save(extraPriceRule3);
          return true;
        }
      }
    }
  }
}
