// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.FrmSearchClient
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities.Clients;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
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
namespace Gbs.Forms.Clients
{
  public class FrmSearchClient : WindowWithSize, IComponentConnector
  {
    internal TextBoxWithClearControl TextBoxSearch;
    internal Button FindButton;
    internal System.Windows.Controls.DataGrid ClientSearchGrid;
    internal Button ButtonUpdateData;
    private bool _contentLoaded;

    private SearchClientViewModel Model { get; set; }

    public FrmSearchClient()
    {
      this.InitializeComponent();
      this.SearchTextBox = this.TextBoxSearch;
      this.Object = (Control) this.ClientSearchGrid;
      this.IaActiveBlockPress = true;
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand1 = new RelayCommand((Action<object>) (obj => this.Model.SelectClient.Execute((object) this.ClientSearchGrid.SelectedItems)));
        RelayCommand relayCommand2 = new RelayCommand((Action<object>) (o => this.Model.CloseFrm()));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
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
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand2
          },
          {
            hotKeys.AddItem,
            this.Model.AddClient
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public (Client client, bool result) GetClient(bool isSupplier = false, bool isUser = false)
    {
      try
      {
        this.Model = new SearchClientViewModel(isSupplier, isUser)
        {
          CloseFrm = new Action(((Window) this).Close)
        };
        this.CommandEnter = this.Model.SelectClient;
        this.DataContext = (object) this.Model;
        Gbs.Helpers.ControlsHelpers.DataGrid.Other.FocusRow(this.ClientSearchGrid);
        this.SetHotKeys();
        this.ShowDialog();
        return (this.Model.SelectedClient, this.Model.ResultAction);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка поиска клиента");
        return ((Client) null, false);
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

    private void FrmSearchClient_OnLoaded(object sender, RoutedEventArgs e)
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

    private void FrmSearchClient_OnClosed(object sender, EventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.FindButton.FindResource((object) "ContextMenuGrid");
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

    private void FrmSearchClient_OnActivated(object sender, EventArgs e)
    {
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(((SearchClientViewModel) this.DataContext).ComPortScannerOnBarcodeChanged));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/clients/frmsearchclient.xaml", UriKind.Relative));
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
          this.TextBoxSearch = (TextBoxWithClearControl) target;
          break;
        case 2:
          this.FindButton = (Button) target;
          this.FindButton.Click += new RoutedEventHandler(this.FindButton_OnClick);
          break;
        case 3:
          this.ClientSearchGrid = (System.Windows.Controls.DataGrid) target;
          break;
        case 4:
          this.ButtonUpdateData = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
