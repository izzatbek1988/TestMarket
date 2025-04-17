// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.SelectGoodsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities
{
  public class SelectGoodsRepository : IRepository<SelectGood>
  {
    public int Delete(List<SelectGood> itemsList) => throw new NotImplementedException();

    public bool Delete(SelectGood item) => throw new NotImplementedException();

    public List<SelectGood> GetActiveItems()
    {
      return this.GetAllItems().Where<SelectGood>((Func<SelectGood, bool>) (x => !x.IsDeleted)).ToList<SelectGood>();
    }

    public List<SelectGood> GetAllItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Setting> settingByQuery = new SettingsRepository().GetSettingByQuery(dataBase.GetTable<SETTINGS>().Where<SETTINGS>((Expression<Func<SETTINGS, bool>>) (x => x.TYPE == 100)));
        IEnumerable<Gbs.Core.Entities.Goods.Good> source1 = CachesBox.AllGoods().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => !x.IsDeleted));
        List<SelectGood> allItems = new List<SelectGood>();
        foreach (IGrouping<\u003C\u003Ef__AnonymousType4<object, bool>, Setting> grouping in settingByQuery.GroupBy(x => new
        {
          Value = x.Value,
          IsDeleted = x.IsDeleted
        }).Where<IGrouping<\u003C\u003Ef__AnonymousType4<object, bool>, Setting>>(x => x.First<Setting>().Parameter == "Uid"))
        {
          IGrouping<\u003C\u003Ef__AnonymousType4<object, bool>, Setting> item = grouping;
          Gbs.Core.Entities.Goods.Good good = source1.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid.ToString() == item.First<Setting>().Value.ToString()));
          IGrouping<\u003C\u003Ef__AnonymousType4<object, bool>, Setting> source2 = item;
          if ((source2 != null ? (source2.First<Setting>().IsDeleted ? 1 : 0) : 1) == 0 && good != null)
            allItems.Add(new SelectGood()
            {
              Uid = item.First<Setting>().EntityUid,
              DisplayName = settingByQuery.FirstOrDefault<Setting>((Func<Setting, bool>) (x => x.Parameter == "DisplayName" && x.EntityUid == item.First<Setting>().EntityUid))?.Value.ToString(),
              Index = Convert.ToInt32(settingByQuery.FirstOrDefault<Setting>((Func<Setting, bool>) (x => x.Parameter == "Index" && x.EntityUid == item.First<Setting>().EntityUid))?.Value),
              Good = good,
              IsDeleted = item.First<Setting>().IsDeleted
            });
        }
        return allItems;
      }
    }

    public SelectGood GetByUid(Guid uid) => throw new NotImplementedException();

    public bool Save(SelectGood item)
    {
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result == ActionResult.Results.Error)
      {
        int num = (int) MessageBoxHelper.Show(string.Join("/n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      Setting setting1 = new Setting();
      setting1.EntityUid = item.Uid;
      setting1.Parameter = "Uid";
      setting1.Value = (object) item.Good.Uid;
      setting1.Type = Types.SelectGood;
      setting1.IsDeleted = item.IsDeleted;
      Setting setting2 = setting1;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "DisplayName";
      setting2.Value = (object) item.DisplayName;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      setting2.Uid = Guid.NewGuid();
      setting2.Parameter = "Index";
      setting2.Value = (object) item.Index;
      setting2.IsDeleted = item.IsDeleted;
      new SettingsRepository().Save(setting2);
      return true;
    }

    public int Save(List<SelectGood> itemsList)
    {
      return itemsList.Count<SelectGood>(new Func<SelectGood, bool>(this.Save));
    }

    public ActionResult Validate(SelectGood item)
    {
      List<string> stringList = new List<string>();
      if (item.DisplayName.IsNullOrEmpty())
        stringList.Add(Translate.ExtraPriceRulesRepository_Validate_Требуется_ввести_название);
      if (item.Good == null)
        stringList.Add(Translate.SelectGoodsRepository_Требуется_указать_товар);
      return new ActionResult(stringList.Any<string>() ? ActionResult.Results.Error : ActionResult.Results.Ok, stringList);
    }
  }
}
