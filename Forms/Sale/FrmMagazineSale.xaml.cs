// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.FrmMagazineSale
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
using Gbs.Helpers.XAML.Converters;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms
{
  public partial class FrmMagazineSale : WindowWithSize, IComponentConnector
  {
    internal Menu MainMenu;
    internal MenuItem MenuItemFile;
    internal MenuItem MenuItemFile_saveAs;
    internal MenuItem MenuItemFile_print;
    internal TextBoxWithClearControl SearchField;
    internal CategorySelectionControl CategorySelectionControl;
    internal Button ButtonSaleCard;
    internal Button ButtonPrint;
    internal Button ButtonReturn;
    internal Button ButtonDelete;
    internal Button ButtonMore;
    internal Button ButtonUpdateData;
    internal FrameworkElement FrameWorkElementProxy;
    internal DataGrid ListSaleItems;
    internal DataGridTextColumn NumberCollumn;
    internal ClientSelectionControl ClientSelectionControl;
    internal StackPanel PanelSearchIn;
    internal Button FindButtonData;
    internal Button FindButton;
    internal ComboBox ComboBoxSections;
    internal ComboBox ComboBoxFiscalTypesFilter;
    internal ComboBox ComboBoxPaymentStatusesFilter;
    internal ComboBox ComboBoxUsersFilter;
    internal Button StorageButton;
    internal DateFilterControl DateFilterControl;
    private bool _contentLoaded;

    public FrmMagazineSale()
    {
    }

    public FrmMagazineSale(SaleJournalViewModel viewModel)
    {
      FrmMagazineSale frmMagazineSale = this;
      try
      {
        this.InitializeComponent();
        this.DataContext = (object) viewModel;
        foreach (EntityProperties.PropertyType propertyType in EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
        {
          if (x.IsDeleted)
            return false;
          return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.AlcCodeUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid);
        })))
        {
          DataGridTextColumn dataGridTextColumn1 = new DataGridTextColumn();
          dataGridTextColumn1.Header = (object) propertyType.Name;
          dataGridTextColumn1.Width = new DataGridLength(100.0);
          Binding binding1 = new Binding(string.Format("Item.Good.PropertiesDictionary[{0}]", (object) propertyType.Uid));
          binding1.StringFormat = EntityProperties.GetStringFormat(propertyType);
          dataGridTextColumn1.Binding = (BindingBase) binding1;
          DataGridTextColumn element = dataGridTextColumn1;
          if (propertyType.Type.IsEither<GlobalDictionaries.EntityPropertyTypes>(GlobalDictionaries.EntityPropertyTypes.Decimal, GlobalDictionaries.EntityPropertyTypes.Integer))
          {
            element.CellStyle = (Style) this.FindResource((object) "numberCellStyle");
            DataGridTextColumn dataGridTextColumn2 = element;
            Binding binding2 = new Binding(string.Format("Item.Good.PropertiesDictionary[{0}]", (object) propertyType.Uid));
            binding2.StringFormat = EntityProperties.GetStringFormat(propertyType);
            binding2.Converter = (IValueConverter) new DecimalToStringConverter();
            dataGridTextColumn2.Binding = (BindingBase) binding2;
          }
          Gbs.Helpers.Extensions.UIElement.Extensions.SetGuid((DependencyObject) element, propertyType.Uid.ToString());
          Gbs.Helpers.Extensions.UIElement.Extensions.SetTag((DependencyObject) element, "Item");
          this.ListSaleItems.Columns.Add((DataGridColumn) element);
        }
        ContextMenu resource1 = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
        foreach (SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod> paymentMethod in (Collection<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>) ((SaleJournalViewModel) this.DataContext).PaymentMethodList)
        {
          ItemCollection items = resource1.Items;
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) paymentMethod.Item.Name;
          newItem.IsChecked = paymentMethod.IsChecked;
          newItem.IsCheckable = true;
          newItem.Tag = (object) paymentMethod.Item.Uid;
          items.Add((object) newItem);
        }
        ContextMenu resource2 = (ContextMenu) this.FindButtonData.FindResource((object) "ContextMenuGrid");
        foreach (GoodsSearchModelView.FilterProperty filterProperty in (Collection<GoodsSearchModelView.FilterProperty>) ((SaleJournalViewModel) this.DataContext).FilterProperties)
        {
          ItemCollection items = resource2.Items;
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) filterProperty.Text;
          newItem.IsChecked = filterProperty.IsChecked;
          newItem.IsCheckable = true;
          newItem.Tag = (object) filterProperty.Name;
          items.Add((object) newItem);
        }
        resource1.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
        resource2.Closed += new RoutedEventHandler(this.CmButtonSearchOnClosed);
        ContextMenu resource3 = (ContextMenu) this.StorageButton.FindResource((object) "ContextMenuGrid");
        foreach (SaleJournalViewModel.ItemSelected<Storages.Storage> itemSelected in (Collection<SaleJournalViewModel.ItemSelected<Storages.Storage>>) ((SaleJournalViewModel) this.DataContext).StorageListFilter)
        {
          ItemCollection items = resource3.Items;
          MenuItem newItem = new MenuItem();
          newItem.Header = (object) itemSelected.Item.Name;
          newItem.IsChecked = itemSelected.IsChecked;
          newItem.IsCheckable = true;
          newItem.Tag = (object) itemSelected.Item.Uid;
          items.Add((object) newItem);
        }
        resource3.Closed += new RoutedEventHandler(this.StorageButtonOnClosed);
        SaleJournalViewModel model = (SaleJournalViewModel) this.DataContext;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
          },
          {
            hotKeys.Print,
            (ICommand) new RelayCommand((Action<object>) (obj =>
            {
              model.PrintCommand.Execute((object) frmMagazineSale.ListSaleItems.SelectedItems);
              model.PrintDocument.Execute((object) null);
            }))
          },
          {
            hotKeys.EditItem,
            (ICommand) new RelayCommand((Action<object>) (obj => model.ShowSaleCard.Execute((object) frmMagazineSale.ListSaleItems.SelectedItems)))
          },
          {
            hotKeys.DeleteItem,
            (ICommand) new RelayCommand((Action<object>) (obj => model.DeleteSales.Execute((object) frmMagazineSale.ListSaleItems.SelectedItems)))
          }
        };
        this.Object = (Control) this.ListSaleItems;
        this.SearchTextBox = this.SearchTextBox;
        this.CommandEnter = model.ShowSaleCard;
        TooltipsSetter.Set(this);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме журнала продаж");
      }
    }

    private void StorageButtonOnClosed(object sender, RoutedEventArgs e)
    {
      SaleJournalViewModel model = (SaleJournalViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.StorageListFilter = new ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>>(source.Select<MenuItem, SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<MenuItem, SaleJournalViewModel.ItemSelected<Storages.Storage>>) (x => new SaleJournalViewModel.ItemSelected<Storages.Storage>()
      {
        Item = model.StorageListFilter.Single<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (p => p.Item.Uid == Guid.Parse(x.Tag.ToString()))).Item,
        IsChecked = x.IsChecked
      })));
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      SaleJournalViewModel model = (SaleJournalViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.PaymentMethodList = new ObservableCollection<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>(source.Select<MenuItem, SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<MenuItem, SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>) (x => new SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>()
      {
        Item = model.PaymentMethodList.Single<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>>((Func<SaleJournalViewModel.ItemSelected<PaymentMethods.PaymentMethod>, bool>) (p => p.Item.Uid == Guid.Parse(x.Tag.ToString()))).Item,
        IsChecked = x.IsChecked
      })));
    }

    private void FrmMagazineSale_OnLoaded(object sender, RoutedEventArgs e)
    {
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(((SaleJournalViewModel) this.DataContext).AuthUser, Actions.ViewStock))
          this.ListSaleItems.Columns.Remove(this.ListSaleItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "3D205A8F-AD38-489A-BF93-3C3498CF57BB")));
        List<DataGridColumn> source = new List<DataGridColumn>((IEnumerable<DataGridColumn>) this.ListSaleItems.Columns.ToList<DataGridColumn>());
        string tagColumnDeleted = new ConfigsRepository<Settings>().Get().Interface.ViewSaleJournal == ViewSaleJournal.ListGood ? "Document" : "Item";
        Func<DataGridColumn, bool> predicate = (Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetTag((DependencyObject) x) == tagColumnDeleted);
        foreach (DataGridColumn dataGridColumn in source.Where<DataGridColumn>(predicate))
        {
          DataGridColumn c = dataGridColumn;
          this.ListSaleItems.Columns.Remove(this.ListSaleItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) c))));
        }
        ContextMenu resource = (ContextMenu) this.ListSaleItems.FindResource((object) "ContextMenuGrid");
        foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ListSaleItems.Columns)
        {
          ItemCollection items = resource.Items;
          MenuItem newItem = new MenuItem();
          newItem.Header = column.Header;
          newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) column);
          newItem.IsCheckable = true;
          newItem.IsChecked = column.Visibility == Visibility.Visible;
          items.Add((object) newItem);
        }
        resource.Closed += new RoutedEventHandler(this.CmOnClosed);
      }
    }

    private void CmButtonSearchOnClosed(object sender, RoutedEventArgs e)
    {
      SaleJournalViewModel model = (SaleJournalViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.FilterProperties = new ObservableCollection<GoodsSearchModelView.FilterProperty>(source.Select<MenuItem, GoodsSearchModelView.FilterProperty>((Func<MenuItem, GoodsSearchModelView.FilterProperty>) (x => new GoodsSearchModelView.FilterProperty()
      {
        Name = x.Tag.ToString(),
        IsChecked = x.IsChecked,
        Text = model.FilterProperties.SingleOrDefault<GoodsSearchModelView.FilterProperty>((Func<GoodsSearchModelView.FilterProperty, bool>) (p => p.Name == x.Tag.ToString()))?.Text ?? string.Empty
      })));
      model.TimerSearch.Stop();
      model.TimerSearch.Start();
    }

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Other.IsVisibilityDataGridColumn(this.ListSaleItems, (ContextMenu) sender);
    }

    public void ShowPrintMenu()
    {
      if (!(this.ListSaleItems.FindResource((object) SaleJournalViewModel.PrintMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) this.ButtonPrint;
      resource.IsOpen = true;
    }

    private void FindButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    private void StorageButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    public void ShowMoreMenu()
    {
      if (!(this.FindResource((object) SaleJournalViewModel.MoreMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) this.ButtonMore;
      resource.IsOpen = true;
    }

    private void FindButtonData_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    private void FrmMagazineSale_OnClosed(object sender, EventArgs e)
    {
      SaleJournalViewModel dataContext = (SaleJournalViewModel) this.DataContext;
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      SaleJournalGood saleJournalSearch1 = dataContext.Setting.SaleJournalSearch;
      GoodProp goodProp = new GoodProp();
      MenuItem menuItem1 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Name"));
      goodProp.IsCheckedName = menuItem1 == null || menuItem1.IsChecked;
      MenuItem menuItem2 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Barcodes"));
      goodProp.IsCheckedBarcodes = menuItem2 == null || menuItem2.IsChecked;
      MenuItem menuItem3 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Barcode"));
      goodProp.IsCheckedBarcode = menuItem3 == null || menuItem3.IsChecked;
      saleJournalSearch1.GoodProp = goodProp;
      SaleJournalGood saleJournalSearch2 = dataContext.Setting.SaleJournalSearch;
      MenuItem menuItem4 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Sum"));
      int num1 = menuItem4 != null ? (menuItem4.IsChecked ? 1 : 0) : 1;
      saleJournalSearch2.IsSum = num1 != 0;
      SaleJournalGood saleJournalSearch3 = dataContext.Setting.SaleJournalSearch;
      MenuItem menuItem5 = resource.Items.Cast<MenuItem>().SingleOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Number"));
      int num2 = menuItem5 != null ? (menuItem5.IsChecked ? 1 : 0) : 1;
      saleJournalSearch3.IsNumberCheck = num2 != 0;
      new ConfigsRepository<FilterOptions>().Save(dataContext.Setting);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/sale/frmmagazinesale.xaml", UriKind.Relative));
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
          this.MenuItemFile = (MenuItem) target;
          break;
        case 3:
          this.MenuItemFile_saveAs = (MenuItem) target;
          break;
        case 4:
          this.MenuItemFile_print = (MenuItem) target;
          break;
        case 5:
          this.SearchField = (TextBoxWithClearControl) target;
          break;
        case 6:
          this.CategorySelectionControl = (CategorySelectionControl) target;
          break;
        case 7:
          this.ButtonSaleCard = (Button) target;
          break;
        case 8:
          this.ButtonPrint = (Button) target;
          break;
        case 9:
          this.ButtonReturn = (Button) target;
          break;
        case 10:
          this.ButtonDelete = (Button) target;
          break;
        case 11:
          this.ButtonMore = (Button) target;
          break;
        case 12:
          this.ButtonUpdateData = (Button) target;
          break;
        case 13:
          this.FrameWorkElementProxy = (FrameworkElement) target;
          break;
        case 14:
          this.ListSaleItems = (DataGrid) target;
          break;
        case 15:
          this.NumberCollumn = (DataGridTextColumn) target;
          break;
        case 16:
          this.ClientSelectionControl = (ClientSelectionControl) target;
          break;
        case 17:
          this.PanelSearchIn = (StackPanel) target;
          break;
        case 18:
          this.FindButtonData = (Button) target;
          this.FindButtonData.Click += new RoutedEventHandler(this.FindButtonData_OnClick);
          break;
        case 19:
          this.FindButton = (Button) target;
          this.FindButton.Click += new RoutedEventHandler(this.FindButton_OnClick);
          break;
        case 20:
          this.ComboBoxSections = (ComboBox) target;
          break;
        case 21:
          this.ComboBoxFiscalTypesFilter = (ComboBox) target;
          break;
        case 22:
          this.ComboBoxPaymentStatusesFilter = (ComboBox) target;
          break;
        case 23:
          this.ComboBoxUsersFilter = (ComboBox) target;
          break;
        case 24:
          this.StorageButton = (Button) target;
          this.StorageButton.Click += new RoutedEventHandler(this.StorageButton_OnClick);
          break;
        case 25:
          this.DateFilterControl = (DateFilterControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
