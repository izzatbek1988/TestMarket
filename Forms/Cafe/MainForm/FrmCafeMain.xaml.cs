// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.FrmCafeMain
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms.Goods;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.Cafe
{
  public partial class FrmCafeMain : WindowWithSize, IComponentConnector, IStyleConnector
  {
    private FrmCafeViewModel Model;
    internal ColumnDefinition CafeGridSplitter;
    internal Button ButtonHome;
    internal TextBoxWithClearControl TxtSearch;
    internal Button FindButton;
    internal Button ButtonAlsoCommand;
    internal Button ButtonCloseCafe;
    internal Button ButtonExit;
    internal Button ButtonTableInfo;
    internal Button ButtonActiveOrders;
    internal ScrollViewer MenuViewer;
    internal ItemsControl MyItems;
    internal Button ButtonSelectClient;
    internal Button ButtonClientName;
    internal ToolBarPanel PanelClientInfo;
    internal GridSplitter Splitter;
    internal System.Windows.Controls.DataGrid CafeBasketGrid;
    internal Button ButtonQty;
    internal Button ButtonSelectedDiscount;
    internal Button ButtonAllDiscount;
    internal Button ButtonDelete;
    internal Button ButtonComment;
    internal Button ButtonPrint;
    internal Button ButtonScale;
    internal Button ButtonSave;
    internal Button ButtonCancel;
    internal Button ButtonUpdateCache;
    private bool _contentLoaded;

    public FrmCafeMain()
    {
      this.InitializeComponent();
      this.CafeBasketGrid.AddGoodsPropertiesColumns();
      this.CafeBasketGrid.CreateContextMenu((Action) (() => this.LayoutHelper.UpdateOption()));
      this.UpdateColumnStock(this.CafeBasketGrid, out Visibility _);
      FrmCafeViewModel.IconFolder = new BitmapImage(new Uri(this.TryFindResource((object) "IconFolder").ToString()));
      FrmCafeViewModel frmCafeViewModel = new FrmCafeViewModel(this.CafeBasketGrid);
      frmCafeViewModel.CloseAction = (Action) (() =>
      {
        this.Close();
        this.Model.ClearImagesList();
        this.Model = (FrmCafeViewModel) null;
        this.DataContext = (object) null;
        GC.Collect();
      });
      this.Model = frmCafeViewModel;
      this.Model.SearchFocusAction = new Action(this.SetSearchFocus);
      this.DataContext = (object) this.Model;
      this.SearchTextBox = this.TxtSearch;
      this.Object = (Control) this.CafeBasketGrid;
      this.CommandEnter = (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Basket.EditQuantityCommand.Execute(obj)));
      this.IaActiveBlockPress = true;
      this.SetSearchFocus();
      this.SetHotKeys();
      TooltipsSetter.Set(this);
      if (new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().IsFullScreen)
        return;
      this.AllowsTransparency = false;
      this.WindowStyle = WindowStyle.SingleBorderWindow;
      this.ResizeMode = ResizeMode.CanResize;
    }

    private void ShowPrintMenu()
    {
      if (!(this.FindResource((object) CafeActiveOrdersViewModel.AlsoMenuKey) is ContextMenu resource))
        return;
      resource.PlacementTarget = (UIElement) this.ButtonAlsoCommand;
      resource.IsOpen = true;
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
          },
          {
            hotKeys.OkAction,
            this.Model.SaveOrder
          },
          {
            hotKeys.CancelAction,
            this.Model.CancelCommand
          },
          {
            hotKeys.SelectClient,
            (ICommand) new RelayCommand((Action<object>) (obj =>
            {
              this.Model.Basket.IsCheckedClient = !this.Model.Basket.IsCheckedClient;
              this.Model.GetClientCommand.Execute((object) null);
            }))
          },
          {
            hotKeys.EditItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Basket.EditQuantityCommand.Execute((object) this.CafeBasketGrid.SelectedItems)))
          },
          {
            hotKeys.DiscountForItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Basket.EditDiscountCommand.Execute((object) this.CafeBasketGrid.SelectedItems)))
          },
          {
            hotKeys.DiscountForCheck,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Basket.EditDiscountCommand.Execute((object) this.CafeBasketGrid.Items)))
          },
          {
            hotKeys.DeleteItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Basket.DeleteItemCommand.Execute((object) this.CafeBasketGrid.SelectedItems)))
          },
          {
            hotKeys.KkmGetXReport,
            this.Model.GetKkmXReport
          },
          {
            hotKeys.KkmGetZReport,
            this.Model.GetKkmZReport
          },
          {
            hotKeys.CashIn,
            this.Model.DepositСash
          },
          {
            hotKeys.CashOut,
            this.Model.RemoveCash
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка установки набора горячих клавиш");
      }
    }

    private void FrmCafeMain_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      foreach (GoodsSearchModelView.FilterProperty filterProperty in (Collection<GoodsSearchModelView.FilterProperty>) this.Model.FilterProperties)
      {
        ItemCollection items = resource.Items;
        System.Windows.Controls.MenuItem newItem = new System.Windows.Controls.MenuItem();
        newItem.Tag = (object) filterProperty.Name;
        newItem.Header = (object) filterProperty.Text;
        newItem.IsChecked = filterProperty.IsChecked;
        newItem.IsCheckable = true;
        items.Add((object) newItem);
      }
      resource.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      this.Model.FilterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>(((ItemsControl) sender).Items.Cast<System.Windows.Controls.MenuItem>().Select<System.Windows.Controls.MenuItem, GoodsSearchModelView.FilterProperty>((Func<System.Windows.Controls.MenuItem, GoodsSearchModelView.FilterProperty>) (x => new GoodsSearchModelView.FilterProperty()
      {
        Name = x.Tag.ToString(),
        IsChecked = x.IsChecked,
        Text = this.Model.FilterProperties.Single<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (p => p.Name == x.Tag.ToString())).Text
      })));
    }

    private void BasketGrid_OnColumnHeaderDragCompleted(object sender, DragCompletedEventArgs e)
    {
      this.LayoutHelper.UpdateOption();
    }

    private void ButtonAlsoCommand_OnClick(object sender, RoutedEventArgs e)
    {
      this.ShowPrintMenu();
    }

    private void Thumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
    {
      this.LayoutHelper.UpdateOption();
    }

    private void SetSearchFocus()
    {
      try
      {
        bool isFormEnabled = false;
        Task.Run((Action) (() =>
        {
          Application.Current.Dispatcher?.Invoke((Action) (() => isFormEnabled = this.IsEnabled));
          while (!isFormEnabled)
          {
            Thread.Sleep(50);
            Application.Current.Dispatcher?.Invoke((Action) (() => isFormEnabled = this.IsEnabled));
          }
        })).ContinueWith((Action<Task>) (_ => Application.Current.Dispatcher?.Invoke((Action) (() =>
        {
          Keyboard.Focus((IInputElement) this.TxtSearch);
          this.TxtSearch.Focus();
        }))));
      }
      catch (Exception ex)
      {
        Gbs.Helpers.Other.ConsoleWrite(ex?.ToString() ?? "");
      }
    }

    private void FindButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) sender;
      resource.IsOpen = true;
    }

    private void FrmCafeMain_OnClosed(object sender, EventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      this.Model.Setting.SearchGood.GoodProp = new GoodProp()
      {
        IsCheckedName = resource.Items.Cast<System.Windows.Controls.MenuItem>().Single<System.Windows.Controls.MenuItem>((Func<System.Windows.Controls.MenuItem, bool>) (x => x.Tag.ToString() == "Name")).IsChecked,
        IsCheckedBarcodes = resource.Items.Cast<System.Windows.Controls.MenuItem>().Single<System.Windows.Controls.MenuItem>((Func<System.Windows.Controls.MenuItem, bool>) (x => x.Tag.ToString() == "Barcodes")).IsChecked,
        IsCheckedBarcode = resource.Items.Cast<System.Windows.Controls.MenuItem>().Single<System.Windows.Controls.MenuItem>((Func<System.Windows.Controls.MenuItem, bool>) (x => x.Tag.ToString() == "Barcode")).IsChecked,
        IsCheckedDescription = resource.Items.Cast<System.Windows.Controls.MenuItem>().Single<System.Windows.Controls.MenuItem>((Func<System.Windows.Controls.MenuItem, bool>) (x => x.Tag.ToString() == "Description")).IsChecked,
        IsCheckedModificationBarcode = resource.Items.Cast<System.Windows.Controls.MenuItem>().Single<System.Windows.Controls.MenuItem>((Func<System.Windows.Controls.MenuItem, bool>) (x => x.Tag.ToString() == "ModificationBarcode")).IsChecked,
        PropList = resource.Items.Cast<System.Windows.Controls.MenuItem>().Where<System.Windows.Controls.MenuItem>((Func<System.Windows.Controls.MenuItem, bool>) (x => Guid.TryParse(x.Tag.ToString(), out Guid _))).Select<System.Windows.Controls.MenuItem, GoodProp.PropItem>((Func<System.Windows.Controls.MenuItem, GoodProp.PropItem>) (x => new GoodProp.PropItem()
        {
          Uid = Guid.Parse(x.Tag.ToString()),
          IsChecked = x.IsChecked
        })).ToList<GoodProp.PropItem>()
      };
    }

    private void FrmCafeMain_OnClosing(object sender, CancelEventArgs e)
    {
      if (!this.Model.Basket.Items.Any<BasketItem>())
        return;
      int num = (int) MessageBoxHelper.Show(Translate.FrmCafeViewModel_Нельзя_закрыть__т_к__в_списке_есть_товары, icon: MessageBoxImage.Exclamation);
      e.Cancel = true;
    }

    private void FrmCafeMain_OnPreviewKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return || !this.SearchTextBox.IsKeyboardFocusWithin)
        return;
      this.Model.StartSearch();
      e.Handled = true;
    }

    private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
      if (!new ConfigsRepository<Settings>().Get().Interface.IsSwitchThemeForClickToTime)
        return;
      FrmCafeViewModel.ChangeSkinCommand.Execute((object) null);
    }

    private void BasketCommentCellClick(object sender, MouseButtonEventArgs e)
    {
      MainWindow.CommentClick(sender, e);
    }

    public void BasketPriceCellClick(object sender, MouseButtonEventArgs e)
    {
      MainWindow.PriceClick(sender, e);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/cafe/mainform/frmcafemain.xaml", UriKind.Relative));
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
          this.CafeGridSplitter = (ColumnDefinition) target;
          break;
        case 2:
          this.ButtonHome = (Button) target;
          break;
        case 3:
          this.TxtSearch = (TextBoxWithClearControl) target;
          break;
        case 4:
          this.FindButton = (Button) target;
          this.FindButton.Click += new RoutedEventHandler(this.FindButton_OnClick);
          break;
        case 5:
          this.ButtonAlsoCommand = (Button) target;
          this.ButtonAlsoCommand.Click += new RoutedEventHandler(this.ButtonAlsoCommand_OnClick);
          break;
        case 6:
          this.ButtonCloseCafe = (Button) target;
          break;
        case 7:
          this.ButtonExit = (Button) target;
          break;
        case 8:
          ((UIElement) target).MouseUp += new MouseButtonEventHandler(this.UIElement_OnMouseUp);
          break;
        case 9:
          this.ButtonTableInfo = (Button) target;
          break;
        case 10:
          this.ButtonActiveOrders = (Button) target;
          break;
        case 11:
          this.MenuViewer = (ScrollViewer) target;
          break;
        case 12:
          this.MyItems = (ItemsControl) target;
          break;
        case 13:
          this.ButtonSelectClient = (Button) target;
          break;
        case 14:
          this.ButtonClientName = (Button) target;
          break;
        case 15:
          this.PanelClientInfo = (ToolBarPanel) target;
          break;
        case 16:
          this.Splitter = (GridSplitter) target;
          this.Splitter.DragCompleted += new DragCompletedEventHandler(this.Thumb_OnDragCompleted);
          break;
        case 17:
          this.CafeBasketGrid = (System.Windows.Controls.DataGrid) target;
          this.CafeBasketGrid.ColumnHeaderDragCompleted += new EventHandler<DragCompletedEventArgs>(this.BasketGrid_OnColumnHeaderDragCompleted);
          break;
        case 20:
          this.ButtonQty = (Button) target;
          break;
        case 21:
          this.ButtonSelectedDiscount = (Button) target;
          break;
        case 22:
          this.ButtonAllDiscount = (Button) target;
          break;
        case 23:
          this.ButtonDelete = (Button) target;
          break;
        case 24:
          this.ButtonComment = (Button) target;
          break;
        case 25:
          this.ButtonPrint = (Button) target;
          break;
        case 26:
          this.ButtonScale = (Button) target;
          break;
        case 27:
          this.ButtonSave = (Button) target;
          break;
        case 28:
          this.ButtonCancel = (Button) target;
          break;
        case 29:
          this.ButtonUpdateCache = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 18)
      {
        if (connectionId != 19)
          return;
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseDownEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.BasketCommentCellClick)
        });
      }
      else
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseDownEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.BasketPriceCellClick)
        });
    }
  }
}
