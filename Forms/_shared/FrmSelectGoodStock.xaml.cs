// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.StockModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class StockModelView : ViewModelWithForm
  {
    public bool Result;
    private Visibility _visibilityStock;

    public bool IsSale { get; set; }

    public ICommand SelectStock { get; set; }

    public string Name { get; set; }

    public Visibility VisibilityModification { get; set; }

    public Visibility VisibilityMarkedInfo { get; set; } = Visibility.Collapsed;

    public Visibility VisibilityInfo { get; set; }

    public Visibility VisibilityStock
    {
      get
      {
        return this.VisibilityInfo == Visibility.Collapsed ? Visibility.Collapsed : this._visibilityStock;
      }
      set
      {
        this._visibilityStock = value;
        this.OnPropertyChanged(nameof (VisibilityStock));
      }
    }

    public List<StockModelView.ModificationsAndStock> ListData { get; set; } = new List<StockModelView.ModificationsAndStock>();

    public List<StockModelView.ModificationsAndStock> ListSelectedStocks { get; set; } = new List<StockModelView.ModificationsAndStock>();

    public StockModelView.ModificationsAndStock SelectedStock { get; set; }

    private bool IsAllowSalesToMinus { get; set; }

    private Action Close { get; set; }

    public StockModelView()
    {
    }

    public bool MultipleSelectionModification { get; set; }

    private Gbs.Core.Entities.Goods.Good Good { get; set; }

    public StockModelView(
      Gbs.Core.Entities.Goods.Good good,
      Action close,
      Visibility visibilityInfo = Visibility.Visible,
      bool isSale = false,
      Guid modificationUid = default (Guid))
    {
      this.ModificationUid = modificationUid;
      this.Name = good.Name;
      this.Good = good;
      this.IsSale = isSale;
      this.VisibilityInfo = visibilityInfo;
      this.Close = close;
      this.IsAllowSalesToMinus = new ConfigsRepository<Settings>().Get().Sales.AllowSalesToMinus;
      this.SelectStock = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (((ICollection) obj).Count > 1 && this.VisibilityInfo == Visibility.Collapsed && !this.MultipleSelectionModification)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.StockModelView_Требуется_выбрать_только_один_остаток_);
        }
        else if (this.SelectedStock == null)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.StockModelView_Требуется_выбрать_остаток);
        }
        else
        {
          this.ListSelectedStocks = new List<StockModelView.ModificationsAndStock>(((IEnumerable) obj).Cast<StockModelView.ModificationsAndStock>());
          this.Result = true;
          this.Close();
        }
      }));
      this.LoadData(good);
      this.ListData = new List<StockModelView.ModificationsAndStock>((IEnumerable<StockModelView.ModificationsAndStock>) this.ListData.OrderBy<StockModelView.ModificationsAndStock, string>((Func<StockModelView.ModificationsAndStock, string>) (x => x.Stock.Storage?.Name)).ThenByDescending<StockModelView.ModificationsAndStock, Decimal>((Func<StockModelView.ModificationsAndStock, Decimal>) (x => x.Stock.Price)).ThenBy<StockModelView.ModificationsAndStock, string>((Func<StockModelView.ModificationsAndStock, string>) (x => x.Modification?.Name)).ThenByDescending<StockModelView.ModificationsAndStock, Decimal>((Func<StockModelView.ModificationsAndStock, Decimal>) (x => x.Stock.Stock)));
      if (this.ListData.Count == 1)
      {
        this.ListSelectedStocks = new List<StockModelView.ModificationsAndStock>((IEnumerable<StockModelView.ModificationsAndStock>) this.ListData);
        this.Result = true;
      }
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    private List<GoodsStocks.GoodStock> StocksOptimization(Gbs.Core.Entities.Goods.Good good)
    {
      List<GoodsStocks.GoodStock> source1 = new List<GoodsStocks.GoodStock>();
      List<GoodsStocks.GoodStock> source2 = good.StocksAndPrices;
      if (this.ModificationUid != Guid.Empty)
        source2 = source2.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.ModificationUid == this.ModificationUid)).ToList<GoodsStocks.GoodStock>();
      foreach (GoodsStocks.GoodStock goodStock1 in source2)
      {
        GoodsStocks.GoodStock s = goodStock1;
        if (source1.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.ModificationUid == s.ModificationUid && x.Price == s.Price && x.Storage.Uid == s.Storage.Uid && x.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.MarkedInfoGood))?.Value.ToString() == s.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.MarkedInfoGood))?.Value.ToString())))
        {
          source1.First<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.ModificationUid == s.ModificationUid && x.Price == s.Price && x.Storage.Uid == s.Storage.Uid)).Stock += s.Stock;
        }
        else
        {
          List<GoodsStocks.GoodStock> goodStockList = source1;
          GoodsStocks.GoodStock goodStock2 = new GoodsStocks.GoodStock();
          goodStock2.GoodUid = s.GoodUid;
          goodStock2.ModificationUid = s.ModificationUid;
          goodStock2.IsDeleted = s.IsDeleted;
          goodStock2.Price = s.Price;
          goodStock2.Stock = s.Stock;
          goodStock2.Storage = s.Storage;
          goodStock2.Properties = s.Properties;
          goodStockList.Add(goodStock2);
        }
      }
      return source1;
    }

    private void LoadData(Gbs.Core.Entities.Goods.Good good)
    {
      List<GoodsStocks.GoodStock> goodStockList = this.StocksOptimization(good);
      if (this.VisibilityInfo == Visibility.Collapsed && good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Range)
      {
        good.Modifications.Distinct<GoodsModifications.GoodModification>().ToList<GoodsModifications.GoodModification>().ForEach((Action<GoodsModifications.GoodModification>) (x => this.ListData.Add(new StockModelView.ModificationsAndStock(new GoodsStocks.GoodStock(), x))));
      }
      else
      {
        if (good.Modifications.All<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.IsDeleted)))
        {
          this.VisibilityModification = Visibility.Collapsed;
          foreach (GoodsStocks.GoodStock stock in goodStockList)
            this.ListData.Add(new StockModelView.ModificationsAndStock(stock));
        }
        else
        {
          this.VisibilityModification = Visibility.Visible;
          foreach (GoodsStocks.GoodStock goodStock in goodStockList)
          {
            GoodsStocks.GoodStock stock = goodStock;
            this.ListData.Add(new StockModelView.ModificationsAndStock(stock, good.Modifications.FirstOrDefault<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Uid == stock.ModificationUid))));
          }
        }
        if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service)
        {
          this.VisibilityStock = Visibility.Collapsed;
          this.OnPropertyChanged("VisibilityInfo");
        }
        if (!this.IsAllowSalesToMinus && this.IsSale)
        {
          if (good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))
          {
            if (good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.None, GlobalDictionaries.GoodsSetStatuses.Range, GlobalDictionaries.GoodsSetStatuses.Production))
              this.ListData = this.ListData.Where<StockModelView.ModificationsAndStock>((Func<StockModelView.ModificationsAndStock, bool>) (x => x.Stock.Stock > 0M)).ToList<StockModelView.ModificationsAndStock>();
          }
        }
        if (!this.ListData.Any<StockModelView.ModificationsAndStock>((Func<StockModelView.ModificationsAndStock, bool>) (x => !x.MarkedInfo.IsNullOrEmpty())))
          return;
        this.VisibilityMarkedInfo = Visibility.Visible;
        this.OnPropertyChanged("VisibilityMarkedInfo");
      }
    }

    private Guid ModificationUid { get; set; }

    public class ModificationsAndStock
    {
      public GoodsModifications.GoodModification Modification { get; set; }

      public GoodsStocks.GoodStock Stock { get; set; }

      public string MarkedInfo { get; set; }

      public ModificationsAndStock(
        GoodsStocks.GoodStock stock,
        GoodsModifications.GoodModification mod = null)
      {
        this.Modification = mod;
        this.Stock = stock;
        this.MarkedInfo = stock.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.MarkedInfoGood))?.Value?.ToString() ?? "";
      }

      public ModificationsAndStock()
      {
      }
    }
  }
}
