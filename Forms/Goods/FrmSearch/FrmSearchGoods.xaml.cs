// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.FrmSearchGoods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.Tooltips;
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
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods
{
  public partial class FrmSearchGoods : WindowWithSize, IComponentConnector
  {
    internal TextBoxWithClearControl SearchTb;
    internal CategorySelectionControl CategorySelectionControl;
    internal StackPanel PanelSearchIn;
    internal Button FindButton;
    internal StackPanel PanelPriceFilter;
    internal StackPanel PanelStockFilter;
    internal ComboBox ComboBoxSearchType;
    internal ComboBox ComboBoxStorage;
    internal System.Windows.Controls.DataGrid ListGoodsSearch;
    internal Button ButtonNewGood;
    internal Button ButtonEditGood;
    internal Button ButtonUpdateData;
    internal Button ButtonOk;
    internal Button ButtonCancel;
    internal CheckBox CheckBoxCloseAfterAdd;
    internal CheckBox CheckBoxAddAllStocks;
    private bool _contentLoaded;

    private GoodsSearchModelView Model { get; set; }

    public FrmSearchGoods()
    {
      this.InitializeComponent();
      this.IaActiveBlockPress = true;
      this.ListGoodsSearch.AddGoodsPropertiesColumns();
      this.ListGoodsSearch.CreateContextMenu(new Action(this.LoadImages));
      this.SearchTextBox = this.SearchTb;
      this.Object = (Control) this.ListGoodsSearch;
      TooltipsSetter.Set(this);
    }

    public (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) ShowSearch(
      GlobalDictionaries.DocumentsTypes type,
      string query = null,
      bool isVisNullStock = false,
      Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool> addGood = null,
      Gbs.Core.Entities.Users.User user = null,
      List<Gbs.Core.Entities.Goods.Good> selectedGood = null)
    {
      try
      {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Visibility visibilityStock;
        this.UpdateColumnStock(this.ListGoodsSearch, out visibilityStock);
        GoodsSearchModelView goodsSearchModelView = new GoodsSearchModelView(type, query, isVisNullStock, addGood);
        goodsSearchModelView.VisibilityStock = visibilityStock;
        goodsSearchModelView.AuthUser = user;
        goodsSearchModelView.FormToSHow = (WindowWithSize) this;
        goodsSearchModelView.CloseAction = new Action(((Window) this).Close);
        this.Model = goodsSearchModelView;
        this.DataContext = (object) this.Model;
        this.SetHotKeys();
        stopwatch.Stop();
        if (!this.Model.IsNonShow)
          this.ShowDialog();
        return (this.Model.SelectedGoodList, this.Model.OptionAllCount && this.Model.OptionAllCountVisibility == Visibility.Visible);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при поиске товаров");
        return (new List<Gbs.Core.Entities.Goods.Good>(), false);
      }
    }

    private void FrmSearchGoods_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      foreach (GoodsSearchModelView.FilterProperty filterProperty in (Collection<GoodsSearchModelView.FilterProperty>) this.Model.FilterProperties)
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Tag = (object) filterProperty.Name;
        newItem.Header = (object) filterProperty.Text;
        newItem.IsChecked = filterProperty.IsChecked;
        newItem.IsCheckable = true;
        items.Add((object) newItem);
      }
      this.SearchTb.SearchTextBox.Focus();
      this.SearchTb.SearchTextBox.SelectionStart = this.SearchTb.SearchTextBox.Text.Length;
      if (this.ListGoodsSearch.Items.Count > 0)
        this.ListGoodsSearch.SelectedItem = this.ListGoodsSearch.Items[0];
      resource.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      this.Model.FilterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>(((ItemsControl) sender).Items.Cast<MenuItem>().Select<MenuItem, GoodsSearchModelView.FilterProperty>((Func<MenuItem, GoodsSearchModelView.FilterProperty>) (x => new GoodsSearchModelView.FilterProperty()
      {
        Name = x.Tag.ToString(),
        IsChecked = x.IsChecked,
        Text = this.Model.FilterProperties.SingleOrDefault<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (p => p.Name == x.Tag.ToString()))?.Text ?? string.Empty
      })));
      this.Model.TimerSearch.Stop();
      this.Model.TimerSearch.Start();
    }

    private void LoadImages()
    {
      if (this.ListGoodsSearch.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "7FD85029-4950-4788-A4A7-2278A4278D21")).Visibility != Visibility.Visible)
        return;
      ((GoodsSearchModelView) this.DataContext).LoadingImageForGoodInList();
    }

    private void WindowWithSize_TextInput(object sender, TextCompositionEventArgs e)
    {
      try
      {
        foreach (char ch in e.Text)
        {
          if (ch == '\b')
          {
            int length = this.SearchTb.TextString.Length - 1;
            if (length > 0)
              this.Model.Filter = this.Model.Filter.Substring(0, length);
            else if (length == 0)
              this.Model.Filter = string.Empty;
          }
          else if (char.TryParse(e.Text, out char _) && (char.IsLetterOrDigit(char.Parse(e.Text)) || char.IsPunctuation(char.Parse(e.Text)) || char.IsSymbol(char.Parse(e.Text))))
            this.Model.Filter += ch.ToString();
        }
        e.Handled = true;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void FrmSearchGoods_OnClosed(object sender, EventArgs e)
    {
      if (this.Model == null)
        return;
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      SearchGood searchGood = this.Model.Setting.SearchGood;
      GoodProp goodProp = new GoodProp();
      MenuItem menuItem1 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Name"));
      goodProp.IsCheckedName = menuItem1 == null || menuItem1.IsChecked;
      MenuItem menuItem2 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Barcodes"));
      goodProp.IsCheckedBarcodes = menuItem2 == null || menuItem2.IsChecked;
      MenuItem menuItem3 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Barcode"));
      goodProp.IsCheckedBarcode = menuItem3 == null || menuItem3.IsChecked;
      MenuItem menuItem4 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Description"));
      goodProp.IsCheckedDescription = menuItem4 == null || menuItem4.IsChecked;
      MenuItem menuItem5 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "ModificationBarcode"));
      goodProp.IsCheckedModificationBarcode = menuItem5 == null || menuItem5.IsChecked;
      goodProp.PropList = resource.Items.Cast<MenuItem>().Where<MenuItem>((Func<MenuItem, bool>) (x => Guid.TryParse(x.Tag.ToString(), out Guid _))).Select<MenuItem, GoodProp.PropItem>((Func<MenuItem, GoodProp.PropItem>) (x => new GoodProp.PropItem()
      {
        Uid = Guid.Parse(x.Tag.ToString()),
        IsChecked = x.IsChecked
      })).ToList<GoodProp.PropItem>();
      searchGood.GoodProp = goodProp;
      new ConfigsRepository<FilterOptions>().Save(this.Model.Setting);
      if (!this.Model.IsEditGoodInFrm)
        return;
      CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllGoods);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand1 = new RelayCommand((Action<object>) (obj => this.Model.SelectGoodsCommand.Execute((object) this.ListGoodsSearch.SelectedItems)));
        RelayCommand relayCommand2 = new RelayCommand((Action<object>) (o => this.Model.CloseAction()));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
          },
          {
            hotKeys.OkAction,
            (ICommand) relayCommand1
          },
          {
            new HotKeysHelper.Hotkey(Key.Return),
            (ICommand) relayCommand1
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand2
          },
          {
            hotKeys.AddItem,
            this.Model.AddGoodsCommand
          },
          {
            hotKeys.EditItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.EditGoodsCommand.Execute((object) this.ListGoodsSearch.SelectedItems)))
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand2
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void FrmSearchGoods_OnActivated(object sender, EventArgs e)
    {
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(((GoodsSearchModelView) this.DataContext).ComPortScannerOnBarcodeChanged));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/frmsearch/frmsearchgoods.xaml", UriKind.Relative));
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
          this.CategorySelectionControl = (CategorySelectionControl) target;
          break;
        case 3:
          this.PanelSearchIn = (StackPanel) target;
          break;
        case 4:
          this.FindButton = (Button) target;
          this.FindButton.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 5:
          this.PanelPriceFilter = (StackPanel) target;
          break;
        case 6:
          this.PanelStockFilter = (StackPanel) target;
          break;
        case 7:
          this.ComboBoxSearchType = (ComboBox) target;
          break;
        case 8:
          this.ComboBoxStorage = (ComboBox) target;
          break;
        case 9:
          this.ListGoodsSearch = (System.Windows.Controls.DataGrid) target;
          this.ListGoodsSearch.TextInput += new TextCompositionEventHandler(this.WindowWithSize_TextInput);
          break;
        case 10:
          this.ButtonNewGood = (Button) target;
          break;
        case 11:
          this.ButtonEditGood = (Button) target;
          break;
        case 12:
          this.ButtonUpdateData = (Button) target;
          break;
        case 13:
          this.ButtonOk = (Button) target;
          break;
        case 14:
          this.ButtonCancel = (Button) target;
          break;
        case 15:
          this.CheckBoxCloseAfterAdd = (CheckBox) target;
          break;
        case 16:
          this.CheckBoxAddAllStocks = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
