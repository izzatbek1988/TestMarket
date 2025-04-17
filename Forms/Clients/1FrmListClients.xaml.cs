// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.FrmListClients
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.UserControls;
using Gbs.Helpers.XAML.Converters;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Clients
{
  public class FrmListClients : WindowWithSize, IComponentConnector
  {
    internal TextBoxWithClearControl SerachTb;
    internal Button btnAddGood;
    internal Button btnDeleteGood;
    internal Button ButtonUpdateData;
    internal System.Windows.Controls.DataGrid ClientsList;
    internal Button FindButton;
    private bool _contentLoaded;

    public FrmListClients()
    {
      this.InitializeComponent();
      this.IaActiveBlockPress = true;
      this.ClientsList.CreateContextMenu();
      foreach (EntityProperties.PropertyType propertyType in EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted)))
      {
        DataGridTextColumn dataGridTextColumn1 = new DataGridTextColumn();
        dataGridTextColumn1.Header = (object) propertyType.Name;
        dataGridTextColumn1.Width = new DataGridLength(100.0);
        Binding binding1 = new Binding(string.Format("Client.Client.PropertiesDictionary[{0}]", (object) propertyType.Uid));
        binding1.StringFormat = EntityProperties.GetStringFormat(propertyType);
        dataGridTextColumn1.Binding = (BindingBase) binding1;
        DataGridTextColumn element = dataGridTextColumn1;
        if (propertyType.Type.IsEither<GlobalDictionaries.EntityPropertyTypes>(GlobalDictionaries.EntityPropertyTypes.Decimal, GlobalDictionaries.EntityPropertyTypes.Integer))
        {
          element.CellStyle = (Style) this.FindResource((object) "numberCellStyle");
          DataGridTextColumn dataGridTextColumn2 = element;
          Binding binding2 = new Binding(string.Format("Client.Client.PropertiesDictionary[{0}]", (object) propertyType.Uid));
          binding2.StringFormat = EntityProperties.GetStringFormat(propertyType);
          binding2.Converter = (IValueConverter) new DecimalToStringConverter();
          dataGridTextColumn2.Binding = (BindingBase) binding2;
        }
        Gbs.Helpers.Extensions.UIElement.Extensions.SetGuid((DependencyObject) element, propertyType.Uid.ToString());
        this.ClientsList.Columns.Add((DataGridColumn) element);
      }
    }

    private CatalogClientsModelView Model { get; set; }

    public void ShowClientsList()
    {
      try
      {
        if (!Gbs.Helpers.Other.IsActiveAndShowForm<FrmListClients>())
        {
          this.IsMainForm = false;
        }
        else
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.ClientsCatalogShow);
          if (!access.Result)
            return;
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.FrmListClients_ShowClientsList_Открыт_список_покупателей, access.User), true);
          this.Model = new CatalogClientsModelView(true)
          {
            AuthUser = access.User
          };
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              F1help.HelpHotKey,
              (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
            },
            {
              hotKeys.AddItem,
              this.Model.AddCommand
            },
            {
              hotKeys.EditItem,
              (ICommand) new RelayCommand((Action<object>) (obj => this.Model.EditCommand.Execute((object) this.ClientsList.SelectedItems)))
            },
            {
              hotKeys.DeleteItem,
              (ICommand) new RelayCommand((Action<object>) (obj => this.Model.DeleteCommand.Execute((object) this.ClientsList.SelectedItems)))
            }
          };
          this.DataContext = (object) this.Model;
          this.Object = (Control) this.ClientsList;
          this.SearchTextBox = this.SerachTb;
          this.CommandEnter = ((CatalogClientsModelView) this.DataContext).EditCommand;
          this.Show();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в списке покупателей");
      }
    }

    private void FrmListClients_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      foreach (GoodsCatalogModelView.FilterProperty filterProperty in (Collection<GoodsCatalogModelView.FilterProperty>) this.Model.FilterProperties)
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Tag = (object) filterProperty.Name;
        newItem.Header = (object) filterProperty.Text;
        newItem.IsChecked = filterProperty.IsChecked;
        newItem.IsCheckable = true;
        items.Add((object) newItem);
      }
      resource.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
    }

    private void FindButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) sender;
      resource.IsOpen = true;
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      this.Model.FilterProperties = new ObservableCollection<GoodsCatalogModelView.FilterProperty>(((ItemsControl) sender).Items.Cast<MenuItem>().Select<MenuItem, GoodsCatalogModelView.FilterProperty>((Func<MenuItem, GoodsCatalogModelView.FilterProperty>) (x => new GoodsCatalogModelView.FilterProperty()
      {
        Name = x.Tag.ToString(),
        IsChecked = x.IsChecked,
        Text = this.Model.FilterProperties.Single<GoodsCatalogModelView.FilterProperty>((Func<GoodsCatalogModelView.FilterProperty, bool>) (p => p.Name == x.Tag.ToString())).Text
      })));
      this.Model.SearchForFilter();
    }

    private void FrmListClients_OnClosed(object sender, EventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
      if (this.Model == null)
        return;
      this.Model.Setting.ClientSearch = new ClientSearchOptions()
      {
        IsName = resource.Items.Cast<MenuItem>().Single<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Name")).IsChecked,
        IsBarcode = resource.Items.Cast<MenuItem>().Single<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Barcode")).IsChecked,
        IsPhone = resource.Items.Cast<MenuItem>().Single<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Phone")).IsChecked,
        IsEmail = resource.Items.Cast<MenuItem>().Single<MenuItem>((Func<MenuItem, bool>) (x => x.Tag.ToString() == "Email")).IsChecked,
        PropList = resource.Items.Cast<MenuItem>().Where<MenuItem>((Func<MenuItem, bool>) (x => Guid.TryParse(x.Tag.ToString(), out Guid _))).Select<MenuItem, GoodProp.PropItem>((Func<MenuItem, GoodProp.PropItem>) (x => new GoodProp.PropItem()
        {
          Uid = Guid.Parse(x.Tag.ToString()),
          IsChecked = x.IsChecked
        })).ToList<GoodProp.PropItem>()
      };
      new ConfigsRepository<FilterOptions>().Save(this.Model.Setting);
    }

    private void FrmListClients_OnActivated(object sender, EventArgs e)
    {
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(((CatalogClientsModelView) this.DataContext).ComPortScannerOnBarcodeChanged));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/clients/frmlistclients.xaml", UriKind.Relative));
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
          this.SerachTb = (TextBoxWithClearControl) target;
          break;
        case 2:
          this.btnAddGood = (Button) target;
          break;
        case 3:
          this.btnDeleteGood = (Button) target;
          break;
        case 4:
          this.ButtonUpdateData = (Button) target;
          break;
        case 5:
          this.ClientsList = (System.Windows.Controls.DataGrid) target;
          break;
        case 6:
          this.FindButton = (Button) target;
          this.FindButton.Click += new RoutedEventHandler(this.FindButton_OnClick);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
