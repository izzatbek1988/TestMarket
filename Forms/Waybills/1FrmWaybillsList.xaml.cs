// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Waybills.FrmWaybillsList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
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
namespace Gbs.Forms.Waybills
{
  public class FrmWaybillsList : WindowWithSize, IComponentConnector
  {
    private WaybillsViewModel _model;
    internal TextBoxWithClearControl SearchText;
    internal Button StorageButton;
    internal Button ButtonPrint;
    internal Button ButtonGr;
    internal DataGrid ListWaybills;
    internal Button ButtonUpdateData;
    private bool _contentLoaded;

    public FrmWaybillsList() => this.InitializeComponent();

    private void ShowMenuPrint()
    {
      if (!(this.FindResource((object) WaybillsViewModel.PrintMenuKey) is ContextMenu resource))
        return;
      resource.PlacementTarget = (System.Windows.UIElement) this.ButtonPrint;
      resource.IsOpen = true;
    }

    public void ShowWaybills()
    {
      try
      {
        if (!Other.IsActiveAndShowForm<FrmWaybillsList>())
        {
          this.IsMainForm = false;
        }
        else
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.WaybillListShow);
          if (!access.Result)
            return;
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.FrmWaybillsList_ShowWaybills_Открыт_журнал_поступлений, access.User), false);
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            Visibility visibility = Visibility.Visible;
            if (!new UsersRepository(dataBase).GetAccess(access.User, Actions.ShowBuyPrice))
            {
              this.ListWaybills.Columns.Remove(this.ListWaybills.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C")));
              visibility = Visibility.Collapsed;
            }
            WaybillsViewModel m = new WaybillsViewModel(false)
            {
              AuthUser = access.User,
              ShowMenuPrint = new Action(this.ShowMenuPrint),
              VisibilityBuySum = visibility
            };
            this.DataContext = (object) m;
            ((WaybillsViewModel) this.DataContext).ShowMenuAction = new Action(this.ShowPrintMenu);
            this._model = (WaybillsViewModel) this.DataContext;
            this.InitializeComponent();
            HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
            this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
            {
              {
                F1help.HelpHotKey,
                (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
              },
              {
                hotKeys.AddItem,
                m.AddNewWaybill
              },
              {
                hotKeys.Print,
                (ICommand) new RelayCommand((Action<object>) (obj =>
                {
                  m.SelectedList = this.ListWaybills.SelectedItems.Cast<WaybillsViewModel.WaybillItemsInfoGrid>().ToList<WaybillsViewModel.WaybillItemsInfoGrid>();
                  m.PrintWaybills.Execute((object) null);
                }))
              },
              {
                hotKeys.EditItem,
                (ICommand) new RelayCommand((Action<object>) (obj => m.EditCardWaybill.Execute((object) this.ListWaybills.SelectedItems)))
              },
              {
                hotKeys.DeleteItem,
                (ICommand) new RelayCommand((Action<object>) (obj => m.DeleteWaybill.Execute((object) this.ListWaybills.SelectedItems)))
              }
            };
            this.Object = (Control) this.ListWaybills;
            this.SearchTextBox = this.SearchText;
            this.CommandEnter = this._model.EditCardWaybill;
            this.Show();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в журанле поступлений");
      }
    }

    public void ShowReturnWaybills()
    {
      try
      {
        if (!Other.IsActiveAndShowForm<FrmWaybillsList>())
        {
          this.IsMainForm = false;
        }
        else
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.WaybillListShow);
          if (!access.Result)
            return;
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.FrmWaybillsList_ShowWaybills_Открыт_журнал_поступлений, access.User), false);
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            Visibility visibility = Visibility.Visible;
            if (!new UsersRepository(dataBase).GetAccess(access.User, Actions.ShowBuyPrice))
            {
              this.ListWaybills.Columns.Remove(this.ListWaybills.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C")));
              visibility = Visibility.Collapsed;
            }
            this.DataContext = (object) new WaybillsViewModel(true)
            {
              AuthUser = access.User,
              ShowMenuPrint = new Action(this.ShowMenuPrint),
              VisibilityBuySum = visibility
            };
            ((WaybillsViewModel) this.DataContext).ShowMenuAction = new Action(this.ShowPrintMenu);
            this._model = (WaybillsViewModel) this.DataContext;
            HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
            this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
            {
              {
                hotKeys.AddItem,
                this._model.AddNewWaybill
              },
              {
                hotKeys.Print,
                (ICommand) new RelayCommand((Action<object>) (obj =>
                {
                  this._model.SelectedList = this.ListWaybills.SelectedItems.Cast<WaybillsViewModel.WaybillItemsInfoGrid>().ToList<WaybillsViewModel.WaybillItemsInfoGrid>();
                  this._model.PrintWaybills.Execute((object) null);
                }))
              },
              {
                hotKeys.EditItem,
                (ICommand) new RelayCommand((Action<object>) (obj => this._model.EditCardWaybill.Execute((object) this.ListWaybills.SelectedItems)))
              },
              {
                hotKeys.DeleteItem,
                (ICommand) new RelayCommand((Action<object>) (obj => this._model.DeleteWaybill.Execute((object) this.ListWaybills.SelectedItems)))
              }
            };
            this.Object = (Control) this.ListWaybills;
            this.SearchTextBox = this.SearchText;
            this.CommandEnter = this._model.EditCardWaybill;
            this.Show();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в журанле поступлений");
      }
    }

    private void FrmWaybillsList_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource1 = (ContextMenu) this.ListWaybills.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ListWaybills.Columns)
      {
        ItemCollection items = resource1.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = column.Header;
        newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) column);
        newItem.IsCheckable = true;
        newItem.IsChecked = column.Visibility == Visibility.Visible;
        items.Add((object) newItem);
      }
      this.SetVisibilityCredit();
      resource1.Closed += new RoutedEventHandler(this.CmOnClosed);
      ContextMenu resource2 = (ContextMenu) this.StorageButton.FindResource((object) "ContextMenuGrid");
      foreach (SaleJournalViewModel.ItemSelected<Storages.Storage> itemSelected in (Collection<SaleJournalViewModel.ItemSelected<Storages.Storage>>) ((WaybillsViewModel) this.DataContext).StorageListFilter)
      {
        ItemCollection items = resource2.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) itemSelected.Item.Name;
        newItem.IsChecked = itemSelected.IsChecked;
        newItem.IsCheckable = true;
        newItem.Tag = (object) itemSelected.Item.Uid;
        items.Add((object) newItem);
      }
      resource2.Closed += new RoutedEventHandler(this.StorageButtonOnClosed);
    }

    private void StorageButtonOnClosed(object sender, RoutedEventArgs e)
    {
      WaybillsViewModel model = (WaybillsViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.StorageListFilter = new ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>>(source.Select<MenuItem, SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<MenuItem, SaleJournalViewModel.ItemSelected<Storages.Storage>>) (x => new SaleJournalViewModel.ItemSelected<Storages.Storage>()
      {
        Item = model.StorageListFilter.Single<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (p => p.Item.Uid == Guid.Parse(x.Tag.ToString()))).Item,
        IsChecked = x.IsChecked
      })));
    }

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Other.IsVisibilityDataGridColumn(this.ListWaybills, (ContextMenu) sender);
      this.SetVisibilityCredit();
    }

    private void SetVisibilityCredit()
    {
      this._model.VisibilityCreditSum = this.ListWaybills.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == WaybillsViewModel.CreditColumnUid)).Visibility;
      this._model.VisibilityPaymentsSum = this.ListWaybills.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == WaybillsViewModel.PaymentsColumnUid)).Visibility;
    }

    private void ShowPrintMenu()
    {
      if (!(this.FindResource((object) WaybillsViewModel.AlsoMenuKey) is ContextMenu resource))
        return;
      resource.PlacementTarget = (System.Windows.UIElement) this.ButtonGr;
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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/waybills/frmwaybillslist.xaml", UriKind.Relative));
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
          this.SearchText = (TextBoxWithClearControl) target;
          break;
        case 2:
          this.StorageButton = (Button) target;
          this.StorageButton.Click += new RoutedEventHandler(this.StorageButton_OnClick);
          break;
        case 3:
          this.ButtonPrint = (Button) target;
          break;
        case 4:
          this.ButtonGr = (Button) target;
          break;
        case 5:
          this.ListWaybills = (DataGrid) target;
          break;
        case 6:
          this.ButtonUpdateData = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
