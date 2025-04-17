// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.FrmGoodRemoteCatalog
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.UserControls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods
{
  public class FrmGoodRemoteCatalog : WindowWithSize, IComponentConnector
  {
    internal TextBoxWithClearControl SearchTb;
    internal Button FindButton;
    internal DataGrid ListGoods;
    private bool _contentLoaded;

    public FrmGoodRemoteCatalog() => this.InitializeComponent();

    private void FindButton_OnClickon_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    private void FrmGoodRemoteCatalog_OnClosed(object sender, EventArgs e)
    {
      GoodRemoteCatalogViewModel dataContext = (GoodRemoteCatalogViewModel) this.DataContext;
      dataContext.Setting.RemoteGoodsCatalog = new GoodProp()
      {
        IsCheckedName = dataContext.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Name")).IsChecked,
        IsCheckedBarcodes = dataContext.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Barcodes")).IsChecked,
        IsCheckedBarcode = dataContext.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Barcode")).IsChecked,
        IsCheckedDescription = dataContext.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Description")).IsChecked
      };
      new ConfigsRepository<FilterOptions>().Save(dataContext.Setting);
    }

    private void FrmGoodRemoteCatalog_OnLoaded(object sender, RoutedEventArgs e)
    {
      GoodRemoteCatalogViewModel dataContext = (GoodRemoteCatalogViewModel) this.DataContext;
      ContextMenu resource1 = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      foreach (GoodsCatalogModelView.FilterProperty filterProperty in (Collection<GoodsCatalogModelView.FilterProperty>) dataContext.FilterProperties)
      {
        ItemCollection items = resource1.Items;
        MenuItem newItem = new MenuItem();
        newItem.Tag = (object) filterProperty.Name;
        newItem.Header = (object) filterProperty.Text;
        newItem.IsChecked = filterProperty.IsChecked;
        newItem.IsCheckable = true;
        items.Add((object) newItem);
      }
      resource1.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
      ContextMenu resource2 = (ContextMenu) this.ListGoods.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ListGoods.Columns)
      {
        ItemCollection items = resource2.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = column.Header;
        newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) column);
        newItem.IsCheckable = true;
        newItem.IsChecked = column.Visibility == Visibility.Visible;
        items.Add((object) newItem);
      }
      resource2.Closed += new RoutedEventHandler(this.CmOnClosed);
    }

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Other.IsVisibilityDataGridColumn(this.ListGoods, (ContextMenu) sender);
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      GoodRemoteCatalogViewModel model = (GoodRemoteCatalogViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.FilterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>(source.Select<MenuItem, GoodsCatalogModelView.FilterProperty>((Func<MenuItem, GoodsCatalogModelView.FilterProperty>) (x => new GoodsCatalogModelView.FilterProperty()
      {
        Name = x.Tag.ToString(),
        IsChecked = x.IsChecked,
        Text = model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (p => p.Name == x.Tag.ToString())).Text
      })));
      model.SearchForFilter();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/frmgoodremotecatalog.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.SearchTb = (TextBoxWithClearControl) target;
          break;
        case 2:
          this.FindButton = (Button) target;
          this.FindButton.Click += new RoutedEventHandler(this.FindButton_OnClickon_Click);
          break;
        case 3:
          this.ListGoods = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
