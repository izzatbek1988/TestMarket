// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmSelectGoodStock
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmSelectGoodStock : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid StockGrid;
    private bool _contentLoaded;

    public FrmSelectGoodStock()
    {
      this.InitializeComponent();
      this.Object = (Control) this.StockGrid;
      this.Loaded += (RoutedEventHandler) ((x, y) => Gbs.Helpers.ControlsHelpers.DataGrid.Other.SelectFirstRow((object) this.StockGrid));
    }

    public bool SelectedStock(
      Gbs.Core.Entities.Goods.Good good,
      out List<GoodsStocks.GoodStock> stocks,
      bool isSale,
      bool isAddNullStock = false,
      string modificationUid = "")
    {
      try
      {
        List<GoodsStocks.GoodStock> source = good.StocksAndPrices;
        if (modificationUid != "")
          source = source.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.ModificationUid == Guid.Parse(modificationUid))).ToList<GoodsStocks.GoodStock>();
        if (!source.Any<GoodsStocks.GoodStock>() & isAddNullStock)
        {
          stocks = (List<GoodsStocks.GoodStock>) null;
          return true;
        }
        bool allowSalesToMinus = new ConfigsRepository<Settings>().Get().Sales.AllowSalesToMinus;
        if (((isAddNullStock || allowSalesToMinus ? 0 : (source.All<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Stock <= 0M)) ? 1 : 0)) & (isSale ? 1 : 0)) != 0 && good.Group.GoodsType != GlobalDictionaries.GoodTypes.Service && good.SetStatus != GlobalDictionaries.GoodsSetStatuses.Set)
        {
          int num = (int) MessageBoxHelper.Show(Translate.FrmSelectGoodStock_У_данного_товара_нет_положительных_остатков__выбор_невозможен_);
          stocks = (List<GoodsStocks.GoodStock>) null;
          return false;
        }
        if (!WindowWithSize.IsVisibilityStock || good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)
          this.StockGrid.Columns.Remove(this.StockGrid.Columns.FirstOrDefault<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "3D205A8F-AD38-489A-BF93-3C3498CF57BB")));
        if (!good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
        {
          int num = (int) MessageBoxHelper.Show(Translate.FrmSelectGoodStock_У_данного_товара_не_существует_остатков__действие_невозможно_);
          stocks = (List<GoodsStocks.GoodStock>) null;
          return false;
        }
        StockModelView stockModelView = new StockModelView(good, new Action(((Window) this).Close), isSale: isSale, modificationUid: modificationUid.IsNullOrEmpty() ? Guid.Empty : Guid.Parse(modificationUid));
        this.CommandEnter = stockModelView.SelectStock;
        this.DataContext = (object) stockModelView;
        this.LoadHotKeysDictionary();
        if (!stockModelView.Result)
          this.ShowDialog();
        stocks = stockModelView.ListSelectedStocks.Select<StockModelView.ModificationsAndStock, GoodsStocks.GoodStock>((Func<StockModelView.ModificationsAndStock, GoodsStocks.GoodStock>) (x => x.Stock)).ToList<GoodsStocks.GoodStock>();
        return stockModelView.Result;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка выбора товаров из списка с остатками");
        stocks = (List<GoodsStocks.GoodStock>) null;
        return false;
      }
    }

    public bool SelectedModification(
      Gbs.Core.Entities.Goods.Good good,
      out List<GoodsModifications.GoodModification> modifications,
      bool multipleSelection = false)
    {
      try
      {
        StockModelView stockModelView = new StockModelView(good, new Action(((Window) this).Close), Visibility.Collapsed, true)
        {
          VisibilityStock = Visibility.Collapsed,
          MultipleSelectionModification = multipleSelection
        };
        this.DataContext = (object) stockModelView;
        if (!WindowWithSize.IsVisibilityStock)
          this.StockGrid.Columns.Remove(this.StockGrid.Columns.FirstOrDefault<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "3D205A8F-AD38-489A-BF93-3C3498CF57BB")));
        this.LoadHotKeysDictionary();
        if (!stockModelView.Result)
          this.ShowDialog();
        else
          stockModelView.SelectedStock = stockModelView.ListData.First<StockModelView.ModificationsAndStock>();
        modifications = new List<GoodsModifications.GoodModification>(stockModelView.ListSelectedStocks.Select<StockModelView.ModificationsAndStock, GoodsModifications.GoodModification>((Func<StockModelView.ModificationsAndStock, GoodsModifications.GoodModification>) (x => x.Modification)));
        return stockModelView.Result;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка выбора товаров из списка с модификациями");
        modifications = (List<GoodsModifications.GoodModification>) null;
        return false;
      }
    }

    private void LoadHotKeysDictionary()
    {
      StockModelView model = (StockModelView) this.DataContext;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.OkAction,
          (ICommand) new RelayCommand((Action<object>) (obj => model.SelectStock.Execute((object) this.StockGrid.SelectedItems)))
        },
        {
          hotKeys.CancelAction,
          (ICommand) new RelayCommand((Action<object>) (obj => this.Close()))
        }
      };
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmselectgoodstock.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.StockGrid = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
