// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.Pages.Сertificate.PageСertificate
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
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
namespace Gbs.Forms.Goods.GoodCard.Pages.Сertificate
{
  public class PageСertificate : Page, IComponentConnector
  {
    internal Button StorageButton;
    internal DataGrid CertificateGrid;
    internal Button btnDeleteGood;
    private bool _contentLoaded;

    public PageСertificate() => this.InitializeComponent();

    public PageСertificate(List<GoodsStocks.GoodStock> stock, Good good)
    {
      this.InitializeComponent();
      this.DataContext = (object) new CertificateViewModel(stock, good);
      ContextMenu resource = (ContextMenu) this.StorageButton.FindResource((object) "ContextMenuGrid");
      foreach (SaleJournalViewModel.ItemSelected<Storages.Storage> itemSelected in (Collection<SaleJournalViewModel.ItemSelected<Storages.Storage>>) ((CertificateViewModel) this.DataContext).StorageListFilter)
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) itemSelected.Item.Name;
        newItem.IsChecked = itemSelected.IsChecked;
        newItem.IsCheckable = true;
        newItem.Tag = (object) itemSelected.Item.Uid;
        items.Add((object) newItem);
      }
      resource.Closed += new RoutedEventHandler(this.StorageButtonOnClosed);
    }

    private void StorageButtonOnClosed(object sender, RoutedEventArgs e)
    {
      CertificateViewModel model = (CertificateViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.StorageListFilter = new ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>>(source.Select<MenuItem, SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<MenuItem, SaleJournalViewModel.ItemSelected<Storages.Storage>>) (x => new SaleJournalViewModel.ItemSelected<Storages.Storage>()
      {
        Item = model.StorageListFilter.Single<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (p => p.Item.Uid == Guid.Parse(x.Tag.ToString()))).Item,
        IsChecked = x.IsChecked
      })));
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(this.FindResource((object) CertificateBasicViewModel.AddMenuKey) is ContextMenu resource))
        return;
      resource.PlacementTarget = (UIElement) (sender as Button);
      resource.IsOpen = true;
    }

    private void StorageButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) sender;
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/goodcard/pages/%d0%a1ertificate/page%d0%a1ertificates.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.StorageButton = (Button) target;
          this.StorageButton.Click += new RoutedEventHandler(this.StorageButton_OnClick);
          break;
        case 2:
          this.CertificateGrid = (DataGrid) target;
          break;
        case 3:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClick);
          break;
        case 4:
          this.btnDeleteGood = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
