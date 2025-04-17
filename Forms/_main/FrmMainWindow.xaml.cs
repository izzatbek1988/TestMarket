// Decompiled with JetBrains decompiler
// Type: Gbs.MainWindow
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Cafe;
using Gbs.Forms.Main;
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

#nullable disable
namespace Gbs
{
  public partial class MainWindow : WindowWithSize, IComponentConnector, IStyleConnector
  {
    internal Menu MainMenu;
    internal System.Windows.Controls.MenuItem MainMenuItem_file;
    internal System.Windows.Controls.MenuItem ItemSettings;
    internal System.Windows.Controls.MenuItem ItemUsers;
    internal System.Windows.Controls.MenuItem ItemPoint;
    internal System.Windows.Controls.MenuItem ItemExit;
    internal System.Windows.Controls.MenuItem MenuItem_reports;
    internal System.Windows.Controls.MenuItem MenuItem_reports_summary;
    internal System.Windows.Controls.MenuItem MenuItem_reports_saller;
    internal System.Windows.Controls.MenuItem ItemMagazineSale;
    internal System.Windows.Controls.MenuItem MenuItem_reports_master;
    internal System.Windows.Controls.MenuItem MenuItem_actions;
    internal System.Windows.Controls.MenuItem MenuItem_actions_cashOut;
    internal System.Windows.Controls.MenuItem MenuItem_actions_cashIn;
    internal System.Windows.Controls.MenuItem MenuItem_actions_cashMove;
    internal System.Windows.Controls.MenuItem MenuItem_actions_cashRecalc;
    internal System.Windows.Controls.MenuItem MenuItem_actions_kkmXreport;
    internal System.Windows.Controls.MenuItem MenuItem_actions_kkmZreport;
    internal System.Windows.Controls.MenuItem MenuItem_PKKM_online_mode;
    internal System.Windows.Controls.MenuItem MenuItem_PKKM_offline_mode;
    internal System.Windows.Controls.MenuItem MenuItem_PKKM_account;
    internal System.Windows.Controls.MenuItem MenuItem_actions_acquiringReport;
    internal System.Windows.Controls.MenuItem MenuItem_actions_acquiringShiftClose;
    internal System.Windows.Controls.MenuItem MenuItem_actions_serviceMenu;
    internal System.Windows.Controls.MenuItem MenuItem_goods;
    internal System.Windows.Controls.MenuItem ItemGoodsCatalog;
    internal System.Windows.Controls.MenuItem MenuItem_goods_groups;
    internal System.Windows.Controls.MenuItem MenuItem_goods_priceTags;
    internal System.Windows.Controls.MenuItem MenuItem_goods_lables;
    internal System.Windows.Controls.MenuItem MenuItem_goods_packing;
    internal System.Windows.Controls.MenuItem MenuItem_goods_markedLables;
    internal System.Windows.Controls.MenuItem MenuItem_goods_goodsGroupingEdit;
    internal System.Windows.Controls.MenuItem MenuItem_goods_groupsGroupingEdit;
    internal System.Windows.Controls.MenuItem MenuItem_goods_remoteCatalog;
    internal System.Windows.Controls.MenuItem MenuItem_documents;
    internal System.Windows.Controls.MenuItem MenuItem_documents_newWaybill;
    internal System.Windows.Controls.MenuItem MenuItem_documents_waybillsJournal;
    internal System.Windows.Controls.MenuItem MenuItem_documents_newInvenoty;
    internal System.Windows.Controls.MenuItem MenuItem_documents_inventoriesJournal;
    internal System.Windows.Controls.MenuItem MenuItem_documents_newWriteoff;
    internal System.Windows.Controls.MenuItem MenuItem_documents_writeoffsJournal;
    internal System.Windows.Controls.MenuItem MenuItem_documents_sendToOtherPoint;
    internal System.Windows.Controls.MenuItem MenuItem_documents_otherPointSendsJournal;
    internal System.Windows.Controls.MenuItem MenuItem_documents_moveToOtherStorage;
    internal System.Windows.Controls.MenuItem MenuItem_documents_storageMovesJournal;
    internal System.Windows.Controls.MenuItem MenuItem_documents_newOrder;
    internal System.Windows.Controls.MenuItem MenuItem_documents_clientOrdersJournal;
    internal System.Windows.Controls.MenuItem MenuItem_documents_newProduction;
    internal System.Windows.Controls.MenuItem MenuItem_documents_productionsJournal;
    internal System.Windows.Controls.MenuItem MenuItem_documents_quickProduction;
    internal System.Windows.Controls.MenuItem MenuItem_contacts;
    internal System.Windows.Controls.MenuItem ItemClientAdd;
    internal System.Windows.Controls.MenuItem MenuItem_contacts_groups;
    internal System.Windows.Controls.MenuItem MenuItem_contacts_list;
    internal System.Windows.Controls.MenuItem MenuItem_contacts_credits;
    internal System.Windows.Controls.MenuItem MenuItem_help;
    internal System.Windows.Controls.MenuItem MenuItem_help_about;
    internal System.Windows.Controls.MenuItem MenuItem_help_checkUpdate;
    internal System.Windows.Controls.MenuItem MenuItem_help_licenseInfo;
    internal System.Windows.Controls.MenuItem MenuItem_help_changelog;
    internal System.Windows.Controls.MenuItem MenuItem_help_support;
    internal System.Windows.Controls.MenuItem MenuItem_help_onlineHelp;
    internal System.Windows.Controls.MenuItem MenuItem_help_site;
    internal System.Windows.Controls.MenuItem MenuItem_help_vk;
    internal TextBoxWithClearControl TxtSearch;
    internal Button ButtonSearch;
    internal Button ButtonFavorites;
    internal Button ButtonGoToCafe;
    internal TextBlock TextBlockTime;
    internal RowDefinition RowDefinitionSelectGoods;
    internal System.Windows.Controls.DataGrid BasketGrid;
    internal Button ButtonQty;
    internal Button ButtonDiscount;
    internal Button ButtonAllDiscount;
    internal Button ButtonDelete;
    internal Button ButtonComment;
    internal Button ButtonWeight;
    internal ScrollViewer ScrollViewerSelectItems;
    internal ItemsControl MyItems;
    internal Label LabelTotalSum;
    internal DecimalUpDown InsertDecimalUpDown;
    internal Label LabelChangeSum;
    internal CheckBox CheckBoxPrintCheck;
    internal Button ButtonPrintDocs;
    internal CheckBox CheckBoxClient;
    internal Button ButtonSelectClient;
    internal Button ButtonClientCredit;
    internal Button ButtonTotal;
    internal Button ButtonCancel;
    internal Button ButtonSaleNumber;
    private bool _contentLoaded;

    private MainWindowViewModel Model { get; }

    public MainWindow(Action splashCloseAction)
    {
      try
      {
        this.InitializeComponent();
        this.BasketGrid.AddGoodsPropertiesColumns();
        this.BasketGrid.CreateContextMenu((Action) (() => this.LayoutHelper.UpdateOption()));
        this.UpdateColumnStock(this.BasketGrid, out Visibility _);
        this.Model = new MainWindowViewModel(this.BasketGrid)
        {
          RowDefinitionSelectGood = this.RowDefinitionSelectGoods
        };
        this.DataContext = (object) this.Model;
        this.Model.SearchFocusAction = (Action) (() => KeyboardLayoutHelper.SetSearchFocus((Control) this.TxtSearch, (Window) this));
        this.Activated += new EventHandler(this.MainWindow_Activated);
        this.SearchTextBox = this.TxtSearch;
        this.Object = (Control) this.BasketGrid;
        this.IaActiveBlockPress = true;
        if (splashCloseAction != null)
          splashCloseAction();
        if (new ConfigsRepository<DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Cafe)
        {
          new FrmCafeMain().ShowDialog();
          TimerHelper.InitializeTimers(new Action(this.Model.UpdateTime));
          MainWindowViewModel.MonitorViewModel.UpdateBasket(this.Model.CurrentBasket);
        }
        this.SetHotKeys();
        TooltipsSetter.Set(this);
        this.Model.SetFontSize();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка на главной форме");
      }
    }

    private void SetLesson()
    {
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
            this.Model.SaveSaleCommand
          },
          {
            hotKeys.CancelAction,
            this.Model.CancelDocumentCommand
          },
          {
            hotKeys.NextBasket,
            this.Model.NextBasketCommand
          },
          {
            hotKeys.PrevBasket,
            this.Model.PrevBasketCommand
          },
          {
            hotKeys.SelectClient,
            (ICommand) new RelayCommand((Action<object>) (obj =>
            {
              this.Model.CurrentBasket.IsCheckedClient = !this.Model.CurrentBasket.IsCheckedClient;
              this.Model.SelectClientCommand.Execute((object) null);
            }))
          },
          {
            hotKeys.Print,
            this.Model.PrintDocumentCommand
          },
          {
            hotKeys.EditItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.CurrentBasket.EditQuantityCommand.Execute((object) this.Model.BasketGrid.SelectedItems)))
          },
          {
            hotKeys.DiscountForItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.CurrentBasket.EditDiscountCommand.Execute((object) this.Model.BasketGrid.SelectedItems)))
          },
          {
            hotKeys.DiscountForCheck,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.CurrentBasket.EditDiscountCommand.Execute((object) this.Model.BasketGrid.Items)))
          },
          {
            hotKeys.DeleteItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.CurrentBasket.DeleteItemCommand.Execute((object) this.Model.BasketGrid.SelectedItems)))
          },
          {
            hotKeys.AddItem,
            this.Model.SelectGoodCommand
          },
          {
            hotKeys.FavoritesGoods,
            this.Model.ShowSelectGood
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) new RelayCommand((Action<object>) (o => this.Close()))
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
          },
          {
            hotKeys.FiscalLastSaleAction,
            this.Model.FiscalLastSaleCommand
          },
          {
            hotKeys.InsertPayments,
            (ICommand) new RelayCommand((Action<object>) (obj => KeyboardLayoutHelper.SetSearchFocus((Control) this.InsertDecimalUpDown, (Window) this)))
          }
        };
        if (new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Russia)
          this.HotKeysDictionary.Add(hotKeys.SearchByMarkCode, (ICommand) new RelayCommand((Action<object>) (obj => this.Model.SearchByMarkCode())));
        if (new ConfigsRepository<DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home)
        {
          this.HotKeysDictionary.Add(hotKeys.ShowCafeForm, this.Model.ShowCafeCommand);
        }
        else
        {
          int num;
          this.HotKeysDictionary.Add(hotKeys.ShowCafeForm, (ICommand) new RelayCommand((Action<object>) (msg => num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation))));
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка установки набора горячих клавиш");
      }
    }

    private void MainWindow_Activated(object sender, EventArgs e)
    {
      TooltipsSetter.Set(this);
      this.SetHotKeys();
      this.SetLesson();
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.Model.ComPortScannerOnBarcodeChanged));
    }

    private void MainWindow_OnClosing(object sender, CancelEventArgs e)
    {
      if (this.Model.BasketsList.Any<KeyValuePair<int, Gbs.Core.ViewModels.Basket.Basket>>((Func<KeyValuePair<int, Gbs.Core.ViewModels.Basket.Basket>, bool>) (x => x.Value.Items.Any<BasketItem>())))
      {
        if (!new Authorization().GetAccess(Actions.CancelSale).Result)
        {
          e.Cancel = true;
          return;
        }
        if (MessageBoxHelper.Show(string.Format(Translate.MainWindow__0__При_закрытии_программы_все_не_сохраненные_данные_будут_утеряны__1__Продолжить_закрытие_программы_, this.Model.BasketsList.Count == 1 ? (object) Translate.MainWindow_В_корзине_есть_товары__ : (object) Translate.MainWindow_В_одной_из_корзин_есть_товары_, (object) Gbs.Helpers.Other.NewLine(2)), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
        {
          e.Cancel = true;
          return;
        }
        this.Model.CurrentBasket.Items.Clear();
        this.Model.CurrentBasket.CheckOrder();
        this.Model.CurrentBasket.ReCalcTotals();
      }
      new FrmCloseProgram().GetClosed();
      e.Cancel = true;
    }

    private void WindowWithSize_TextInput(object sender, TextCompositionEventArgs e)
    {
    }

    private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
    {
      if (this.Model.CurrentBasket.IsUpdateReceive)
        return;
      ((UpDownBase<Decimal?>) sender).Value = new Decimal?();
    }

    private void BasketGrid_OnColumnHeaderDragCompleted(object sender, DragCompletedEventArgs e)
    {
      this.LayoutHelper.UpdateOption();
    }

    private void Thumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
    {
      if (this.Model.MenuItems.Any<MainWindowViewModel.SelectGoodView>() && this.RowDefinitionSelectGoods.Height.Value < 50.0)
        this.RowDefinitionSelectGoods.Height = new GridLength(60.0);
      this.LayoutHelper.UpdateOption();
    }

    private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
    {
      Keyboard.ClearFocus();
      if (((UpDownBase<Decimal?>) sender).Value.HasValue)
        return;
      this.Model.CurrentBasket.ReceiveSum = new Decimal?(0M);
      this.Model.CurrentBasket.IsUpdateReceive = false;
      this.Model.CurrentBasket.ReCalcTotals();
    }

    private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
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

    public void BasketCommentCellClick(object sender, MouseButtonEventArgs e)
    {
      MainWindow.CommentClick(sender, e);
    }

    public void BasketPriceCellClick(object sender, MouseButtonEventArgs e)
    {
      MainWindow.PriceClick(sender, e);
    }

    public void BasketNameCellClick(object sender, MouseButtonEventArgs e)
    {
      MainWindow.NameClick(sender, e);
    }

    public static void CommentClick(object sender, MouseButtonEventArgs e)
    {
      if (((FrameworkElement) sender)?.DataContext is BasketItem dataContext)
      {
        if (dataContext.ErrorStr == null)
          return;
        MessageBoxHelper.Warning(string.Format(Translate.MainWindow_CommentClick_ТоварКодМаркировки, (object) dataContext.Good.Name, (object) dataContext.Comment, (object) dataContext.ErrorStr));
      }
      else
        LogHelper.Debug("DataContext в метоже CommentClick имеет некорректный тип или отсутствует.");
    }

    public static void PriceClick(object sender, MouseButtonEventArgs e)
    {
      if (!(((FrameworkElement) sender)?.DataContext is BasketItem dataContext) || dataContext.ErrorStrForPrice == null)
        return;
      MessageBoxHelper.Warning(string.Format(Translate.MainWindow_PriceClick_ТоварПричинаИзмененияЦены, (object) dataContext.Good.Name, (object) dataContext.ErrorStrForPrice));
    }

    public static void NameClick(object sender, MouseButtonEventArgs e)
    {
      if (!(((FrameworkElement) sender)?.DataContext is BasketItem dataContext) || dataContext.ErrorStrForDisplayedName == null)
        return;
      MessageBoxHelper.Warning(string.Format(Translate.MainWindow_NameClick_ТоварУведомление, (object) dataContext.Good.Name, (object) dataContext.ErrorStrForDisplayedName));
    }

    private void BasketGrid_OnPreviewTouchDown(object sender, TouchEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_main/frmmainwindow.xaml", UriKind.Relative));
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
          this.MainMenu = (Menu) target;
          break;
        case 2:
          this.MainMenuItem_file = (System.Windows.Controls.MenuItem) target;
          break;
        case 3:
          this.ItemSettings = (System.Windows.Controls.MenuItem) target;
          break;
        case 4:
          this.ItemUsers = (System.Windows.Controls.MenuItem) target;
          break;
        case 5:
          this.ItemPoint = (System.Windows.Controls.MenuItem) target;
          break;
        case 6:
          this.ItemExit = (System.Windows.Controls.MenuItem) target;
          break;
        case 7:
          this.MenuItem_reports = (System.Windows.Controls.MenuItem) target;
          break;
        case 8:
          this.MenuItem_reports_summary = (System.Windows.Controls.MenuItem) target;
          break;
        case 9:
          this.MenuItem_reports_saller = (System.Windows.Controls.MenuItem) target;
          break;
        case 10:
          this.ItemMagazineSale = (System.Windows.Controls.MenuItem) target;
          break;
        case 11:
          this.MenuItem_reports_master = (System.Windows.Controls.MenuItem) target;
          break;
        case 12:
          this.MenuItem_actions = (System.Windows.Controls.MenuItem) target;
          break;
        case 13:
          this.MenuItem_actions_cashOut = (System.Windows.Controls.MenuItem) target;
          break;
        case 14:
          this.MenuItem_actions_cashIn = (System.Windows.Controls.MenuItem) target;
          break;
        case 15:
          this.MenuItem_actions_cashMove = (System.Windows.Controls.MenuItem) target;
          break;
        case 16:
          this.MenuItem_actions_cashRecalc = (System.Windows.Controls.MenuItem) target;
          break;
        case 17:
          this.MenuItem_actions_kkmXreport = (System.Windows.Controls.MenuItem) target;
          break;
        case 18:
          this.MenuItem_actions_kkmZreport = (System.Windows.Controls.MenuItem) target;
          break;
        case 19:
          this.MenuItem_PKKM_online_mode = (System.Windows.Controls.MenuItem) target;
          break;
        case 20:
          this.MenuItem_PKKM_offline_mode = (System.Windows.Controls.MenuItem) target;
          break;
        case 21:
          this.MenuItem_PKKM_account = (System.Windows.Controls.MenuItem) target;
          break;
        case 22:
          this.MenuItem_actions_acquiringReport = (System.Windows.Controls.MenuItem) target;
          break;
        case 23:
          this.MenuItem_actions_acquiringShiftClose = (System.Windows.Controls.MenuItem) target;
          break;
        case 24:
          this.MenuItem_actions_serviceMenu = (System.Windows.Controls.MenuItem) target;
          break;
        case 25:
          this.MenuItem_goods = (System.Windows.Controls.MenuItem) target;
          break;
        case 26:
          this.ItemGoodsCatalog = (System.Windows.Controls.MenuItem) target;
          break;
        case 27:
          this.MenuItem_goods_groups = (System.Windows.Controls.MenuItem) target;
          break;
        case 28:
          this.MenuItem_goods_priceTags = (System.Windows.Controls.MenuItem) target;
          break;
        case 29:
          this.MenuItem_goods_lables = (System.Windows.Controls.MenuItem) target;
          break;
        case 30:
          this.MenuItem_goods_packing = (System.Windows.Controls.MenuItem) target;
          break;
        case 31:
          this.MenuItem_goods_markedLables = (System.Windows.Controls.MenuItem) target;
          break;
        case 32:
          this.MenuItem_goods_goodsGroupingEdit = (System.Windows.Controls.MenuItem) target;
          break;
        case 33:
          this.MenuItem_goods_groupsGroupingEdit = (System.Windows.Controls.MenuItem) target;
          break;
        case 34:
          this.MenuItem_goods_remoteCatalog = (System.Windows.Controls.MenuItem) target;
          break;
        case 35:
          this.MenuItem_documents = (System.Windows.Controls.MenuItem) target;
          break;
        case 36:
          this.MenuItem_documents_newWaybill = (System.Windows.Controls.MenuItem) target;
          break;
        case 37:
          this.MenuItem_documents_waybillsJournal = (System.Windows.Controls.MenuItem) target;
          break;
        case 38:
          this.MenuItem_documents_newInvenoty = (System.Windows.Controls.MenuItem) target;
          break;
        case 39:
          this.MenuItem_documents_inventoriesJournal = (System.Windows.Controls.MenuItem) target;
          break;
        case 40:
          this.MenuItem_documents_newWriteoff = (System.Windows.Controls.MenuItem) target;
          break;
        case 41:
          this.MenuItem_documents_writeoffsJournal = (System.Windows.Controls.MenuItem) target;
          break;
        case 42:
          this.MenuItem_documents_sendToOtherPoint = (System.Windows.Controls.MenuItem) target;
          break;
        case 43:
          this.MenuItem_documents_otherPointSendsJournal = (System.Windows.Controls.MenuItem) target;
          break;
        case 44:
          this.MenuItem_documents_moveToOtherStorage = (System.Windows.Controls.MenuItem) target;
          break;
        case 45:
          this.MenuItem_documents_storageMovesJournal = (System.Windows.Controls.MenuItem) target;
          break;
        case 46:
          this.MenuItem_documents_newOrder = (System.Windows.Controls.MenuItem) target;
          break;
        case 47:
          this.MenuItem_documents_clientOrdersJournal = (System.Windows.Controls.MenuItem) target;
          break;
        case 48:
          this.MenuItem_documents_newProduction = (System.Windows.Controls.MenuItem) target;
          break;
        case 49:
          this.MenuItem_documents_productionsJournal = (System.Windows.Controls.MenuItem) target;
          break;
        case 50:
          this.MenuItem_documents_quickProduction = (System.Windows.Controls.MenuItem) target;
          break;
        case 51:
          this.MenuItem_contacts = (System.Windows.Controls.MenuItem) target;
          break;
        case 52:
          this.ItemClientAdd = (System.Windows.Controls.MenuItem) target;
          break;
        case 53:
          this.MenuItem_contacts_groups = (System.Windows.Controls.MenuItem) target;
          break;
        case 54:
          this.MenuItem_contacts_list = (System.Windows.Controls.MenuItem) target;
          break;
        case 55:
          this.MenuItem_contacts_credits = (System.Windows.Controls.MenuItem) target;
          break;
        case 56:
          this.MenuItem_help = (System.Windows.Controls.MenuItem) target;
          break;
        case 57:
          this.MenuItem_help_about = (System.Windows.Controls.MenuItem) target;
          break;
        case 58:
          this.MenuItem_help_checkUpdate = (System.Windows.Controls.MenuItem) target;
          break;
        case 59:
          this.MenuItem_help_licenseInfo = (System.Windows.Controls.MenuItem) target;
          break;
        case 60:
          this.MenuItem_help_changelog = (System.Windows.Controls.MenuItem) target;
          break;
        case 61:
          this.MenuItem_help_support = (System.Windows.Controls.MenuItem) target;
          break;
        case 62:
          this.MenuItem_help_onlineHelp = (System.Windows.Controls.MenuItem) target;
          break;
        case 63:
          this.MenuItem_help_site = (System.Windows.Controls.MenuItem) target;
          break;
        case 64:
          this.MenuItem_help_vk = (System.Windows.Controls.MenuItem) target;
          break;
        case 65:
          this.TxtSearch = (TextBoxWithClearControl) target;
          break;
        case 66:
          this.ButtonSearch = (Button) target;
          break;
        case 67:
          this.ButtonFavorites = (Button) target;
          break;
        case 68:
          this.ButtonGoToCafe = (Button) target;
          break;
        case 69:
          this.TextBlockTime = (TextBlock) target;
          this.TextBlockTime.MouseUp += new MouseButtonEventHandler(this.UIElement_OnMouseUp);
          break;
        case 70:
          this.RowDefinitionSelectGoods = (RowDefinition) target;
          break;
        case 71:
          this.BasketGrid = (System.Windows.Controls.DataGrid) target;
          this.BasketGrid.ColumnHeaderDragCompleted += new EventHandler<DragCompletedEventArgs>(this.BasketGrid_OnColumnHeaderDragCompleted);
          this.BasketGrid.PreviewTouchDown += new EventHandler<TouchEventArgs>(this.BasketGrid_OnPreviewTouchDown);
          break;
        case 75:
          this.ButtonQty = (Button) target;
          break;
        case 76:
          this.ButtonDiscount = (Button) target;
          break;
        case 77:
          this.ButtonAllDiscount = (Button) target;
          break;
        case 78:
          this.ButtonDelete = (Button) target;
          break;
        case 79:
          this.ButtonComment = (Button) target;
          break;
        case 80:
          this.ButtonWeight = (Button) target;
          break;
        case 81:
          ((Thumb) target).DragCompleted += new DragCompletedEventHandler(this.Thumb_OnDragCompleted);
          break;
        case 82:
          this.ScrollViewerSelectItems = (ScrollViewer) target;
          break;
        case 83:
          this.MyItems = (ItemsControl) target;
          break;
        case 84:
          this.LabelTotalSum = (Label) target;
          break;
        case 85:
          this.InsertDecimalUpDown = (DecimalUpDown) target;
          this.InsertDecimalUpDown.GotFocus += new RoutedEventHandler(this.UIElement_OnGotFocus);
          this.InsertDecimalUpDown.PreviewLostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.UIElement_OnLostFocus);
          break;
        case 86:
          this.LabelChangeSum = (Label) target;
          break;
        case 87:
          this.CheckBoxPrintCheck = (CheckBox) target;
          break;
        case 88:
          this.ButtonPrintDocs = (Button) target;
          break;
        case 89:
          this.CheckBoxClient = (CheckBox) target;
          break;
        case 90:
          this.ButtonSelectClient = (Button) target;
          break;
        case 91:
          this.ButtonClientCredit = (Button) target;
          break;
        case 92:
          this.ButtonTotal = (Button) target;
          break;
        case 93:
          this.ButtonCancel = (Button) target;
          break;
        case 94:
          this.ButtonSaleNumber = (Button) target;
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
      switch (connectionId)
      {
        case 72:
          ((Style) target).Setters.Add((SetterBase) new EventSetter()
          {
            Event = UIElement.PreviewMouseDownEvent,
            Handler = (Delegate) new MouseButtonEventHandler(this.BasketNameCellClick)
          });
          break;
        case 73:
          ((Style) target).Setters.Add((SetterBase) new EventSetter()
          {
            Event = UIElement.PreviewMouseDownEvent,
            Handler = (Delegate) new MouseButtonEventHandler(this.BasketPriceCellClick)
          });
          break;
        case 74:
          ((Style) target).Setters.Add((SetterBase) new EventSetter()
          {
            Event = UIElement.PreviewMouseDownEvent,
            Handler = (Delegate) new MouseButtonEventHandler(this.BasketCommentCellClick)
          });
          break;
      }
    }
  }
}
