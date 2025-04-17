// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.FrmGoodsCatalog
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
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
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods
{
  public class FrmGoodsCatalog : WindowWithSize, IComponentConnector
  {
    internal MenuItem ImportFromExcel;
    internal TextBoxWithClearControl SearchTb;
    internal Button FindButton;
    internal System.Windows.Controls.DataGrid ListGoods;
    internal Button ButtonAddGood;
    internal Button ButtonEditGood;
    internal Button ButtonCopyGood;
    internal Button ButtonJoinGoods;
    internal Button ButtonDeleteGoods;
    private bool _contentLoaded;

    private GoodsCatalogModelView Model { get; set; }

    public static Action<bool> UpdateSelectGoods { get; set; }

    public FrmGoodsCatalog()
    {
      this.InitializeComponent();
      this.IaActiveBlockPress = true;
      this.ListGoods.AddGoodsPropertiesColumns();
      this.SearchTextBox = this.SearchTb;
      this.Object = (Control) this.ListGoods;
      TooltipsSetter.Set(this);
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource1 = (ContextMenu) this.ListGoods.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ListGoods.Columns)
      {
        ItemCollection items = resource1.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = column.Header;
        newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) column);
        newItem.IsCheckable = true;
        newItem.IsChecked = column.Visibility == Visibility.Visible;
        items.Add((object) newItem);
      }
      ContextMenu resource2 = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      foreach (GoodsCatalogModelView.FilterProperty filterProperty in (Collection<GoodsCatalogModelView.FilterProperty>) this.Model.FilterProperties)
      {
        ItemCollection items = resource2.Items;
        MenuItem newItem = new MenuItem();
        newItem.Tag = (object) filterProperty.Name;
        newItem.Header = (object) filterProperty.Text;
        newItem.IsChecked = filterProperty.IsChecked;
        newItem.IsCheckable = true;
        items.Add((object) newItem);
      }
      resource1.Closed += new RoutedEventHandler(this.CmOnClosed);
      resource2.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
      this.LoadImages();
    }

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Gbs.Helpers.Other.IsVisibilityDataGridColumn(this.ListGoods, (ContextMenu) sender);
      this.LoadImages();
    }

    private void LoadImages()
    {
      if (this.ListGoods.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "7FD85029-4950-4788-A4A7-2278A4278D21")).Visibility != Visibility.Visible)
        return;
      ((GoodsCatalogModelView) this.DataContext).LoadingImageForGood();
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      this.Model.FilterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>(((ItemsControl) sender).Items.Cast<MenuItem>().Select<MenuItem, GoodsCatalogModelView.FilterProperty>((Func<MenuItem, GoodsCatalogModelView.FilterProperty>) (x => new GoodsCatalogModelView.FilterProperty()
      {
        Name = x.Tag.ToString(),
        IsChecked = x.IsChecked,
        Text = this.Model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (p => p.Name == x.Tag.ToString())).Text
      })));
      this.Model.SearchGoods?.Execute((object) null);
    }

    public void ShowCatalog()
    {
      try
      {
        if (!Gbs.Helpers.Other.IsActiveAndShowForm<FrmGoodsCatalog>())
        {
          this.IsMainForm = false;
        }
        else
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access1 = new Authorization().GetAccess(Actions.GoodsCatalogShow);
          if (!access1.Result)
            return;
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.FrmGoodsCatalog_ShowCatalog_Открыт_каталог_товаров, access1.User), true);
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            bool access2 = new UsersRepository(dataBase).GetAccess(access1.User, Actions.ViewStock);
            bool access3 = new UsersRepository(dataBase).GetAccess(access1.User, Actions.ShowBuyPrice);
            if (!access2)
              this.ListGoods.Columns.Remove(this.ListGoods.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "3D205A8F-AD38-489A-BF93-3C3498CF57BB")));
            if (!access3)
              this.ListGoods.Columns.Remove(this.ListGoods.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C")));
            GoodsCatalogModelView catalogModelView = new GoodsCatalogModelView(access3);
            catalogModelView.AuthUser = access1.User;
            catalogModelView.VisibilityStock = access2 ? Visibility.Visible : Visibility.Collapsed;
            catalogModelView.FormToSHow = (WindowWithSize) this;
            this.Model = catalogModelView;
            HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
            this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
            {
              {
                F1help.HelpHotKey,
                (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
              },
              {
                hotKeys.AddItem,
                this.Model.AddGoods
              },
              {
                hotKeys.EditItem,
                (ICommand) new RelayCommand((Action<object>) (obj => this.Model.EditGoods.Execute((object) this.ListGoods.SelectedItems)))
              },
              {
                hotKeys.DeleteItem,
                (ICommand) new RelayCommand((Action<object>) (obj => this.Model.DeleteGoods.Execute((object) this.ListGoods.SelectedItems)))
              }
            };
            this.DataContext = (object) this.Model;
            this.SearchTextBox = this.SearchTb;
            this.Object = (Control) this.ListGoods;
            this.CommandEnter = ((GoodsCatalogModelView) this.DataContext).EditGoods;
            this.Show();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в каталоге товаров");
      }
    }

    private void FrmGoodsCatalog_OnClosed(object sender, EventArgs e)
    {
      this.Model?.cts?.Cancel();
      if (this.Model != null)
      {
        this.Model.Setting.GoodsCatalog.GoodProp = new GoodProp()
        {
          IsCheckedName = this.Model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Name")).IsChecked,
          IsCheckedBarcodes = this.Model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Barcodes")).IsChecked,
          IsCheckedBarcode = this.Model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Barcode")).IsChecked,
          IsCheckedDescription = this.Model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "Description")).IsChecked,
          IsCheckedModificationBarcode = this.Model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => x.Name == "ModificationBarcode")).IsChecked,
          PropList = this.Model.FilterProperties.Where<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (x => Guid.TryParse(x.Name, out Guid _))).Select<GoodsCatalogModelView.FilterProperty, GoodProp.PropItem>((Func<GoodsCatalogModelView.FilterProperty, GoodProp.PropItem>) (x => new GoodProp.PropItem()
          {
            Uid = Guid.Parse(x.Name),
            IsChecked = x.IsChecked
          })).ToList<GoodProp.PropItem>()
        };
        new ConfigsRepository<FilterOptions>().Save(this.Model.Setting);
      }
      CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllGoods);
      CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.CafeMenu);
      Action<bool> updateSelectGoods = FrmGoodsCatalog.UpdateSelectGoods;
      if (updateSelectGoods == null)
        return;
      updateSelectGoods(true);
    }

    private void FindButton_OnClickon_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    private void FrmGoodsCatalog_OnActivated(object sender, EventArgs e)
    {
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(((GoodsCatalogModelView) this.DataContext).ComPortScannerOnBarcodeChanged));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/frmgoodscatalog.xaml", UriKind.Relative));
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
          this.ImportFromExcel = (MenuItem) target;
          break;
        case 2:
          this.SearchTb = (TextBoxWithClearControl) target;
          break;
        case 3:
          this.FindButton = (Button) target;
          this.FindButton.Click += new RoutedEventHandler(this.FindButton_OnClickon_Click);
          break;
        case 4:
          this.ListGoods = (System.Windows.Controls.DataGrid) target;
          break;
        case 5:
          this.ButtonAddGood = (Button) target;
          break;
        case 6:
          this.ButtonEditGood = (Button) target;
          break;
        case 7:
          this.ButtonCopyGood = (Button) target;
          break;
        case 8:
          this.ButtonJoinGoods = (Button) target;
          break;
        case 9:
          this.ButtonDeleteGoods = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
