// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.GoodsStockViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard
{
  public partial class GoodsStockViewModel : ViewModelWithForm
  {
    public Visibility VisibilityStock { get; set; }

    public bool IsEnabled { get; set; } = true;

    public bool IsEnabledStock { get; set; } = true;

    public bool IsEnabledPrice { get; set; } = true;

    public bool SaveResult { get; set; }

    public ICommand SaveStock { get; set; }

    public Action Close { get; set; }

    public GoodsStocks.GoodStock Stock { get; set; } = new GoodsStocks.GoodStock();

    public IEnumerable<Storages.Storage> ListStorage { get; set; }

    public GoodsStockViewModel()
    {
    }

    public GoodsStockViewModel(GoodsStocks.GoodStock stock, Gbs.Core.Entities.Goods.Good good)
    {
      this.Stock = stock;
      if (good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set || good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
        this.VisibilityStock = Visibility.Collapsed;
      using (DataBase dataBase = Data.GetDataBase())
        this.ListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
      List<Storages.Storage> list = this.ListStorage.ToList<Storages.Storage>();
      if (list.Count == 1)
        this.Stock.Storage = list.First<Storages.Storage>();
      this.SaveStock = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.SaveResult = this.Stock.Storage != null;
        if (this.SaveResult)
        {
          this.Close();
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.GoodsStockViewModel_Невозможно_сохранить_остаток_без_склада);
        }
      }));
    }
  }
}
