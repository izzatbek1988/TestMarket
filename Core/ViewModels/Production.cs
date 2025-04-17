// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Production
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.ViewModels.Basket;
using Gbs.Resources.Localizations;
using System;

#nullable disable
namespace Gbs.Core.ViewModels
{
  public class Production : Gbs.Core.ViewModels.Basket.Basket
  {
    public bool IsBeer { get; set; }

    public override ActionResult Add(BasketItem item, bool checkMinus = true, bool isWeightPlu = false)
    {
      if (item == null)
        throw new ArgumentNullException();
      if (this.Storage != null && this.Storage.Uid != item.Storage.Uid)
        return new ActionResult(ActionResult.Results.Error, Translate.Production_Add_В_одном_документе__производство__могут_быть_рецепты_только_с_одного_склада);
      this.Storage = item.Storage;
      if (!this.IsBeer)
        item.Good.Group.RuMarkedProductionType = GlobalDictionaries.RuMarkedProductionTypes.None;
      item.Good.Group.IsFreePrice = true;
      if (!this.EditQuantity(item))
        return new ActionResult(ActionResult.Results.Warning);
      this.Items.Add(item);
      this.SelectedItem = item;
      this.OnPropertyChanged("Items");
      this.ReCalcTotals();
      return new ActionResult(ActionResult.Results.Ok);
    }
  }
}
