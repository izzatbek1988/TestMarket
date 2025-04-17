// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Inventory.FrmInventoryCard_v2
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit.Primitives;

#nullable disable
namespace Gbs.Forms.Inventory
{
  public partial class FrmInventoryCard_v2 : WindowWithSize, IComponentConnector, IStyleConnector
  {
    internal System.Windows.Controls.DataGrid GoodsList;
    internal Button btnEditQuantity;
    internal Button btnDelete;
    private bool _contentLoaded;

    public FrmInventoryCard_v2()
    {
      this.InitializeComponent();
      this.GoodsList.AddGoodsPropertiesColumns();
      this.GoodsList.CreateContextMenu();
    }

    public void ShowCard(Document doc, Func<bool> loadJournal, Gbs.Core.Entities.Users.User authUser = null)
    {
      if (doc != null && !Gbs.Helpers.Other.IsActiveAndShowForm<FrmInventoryCard_v2>(doc.Uid.ToString()))
        this.IsMainForm = false;
      else if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
      }
      else
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(ref authUser, doc == null ? Actions.CreateInventory : Actions.EditInventory))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(doc == null ? Actions.CreateInventory : Actions.EditInventory);
            if (!access.Result)
              return;
            authUser = access.User;
          }
          InventoryCardViewModel_v2 inventoryCardViewModelV2 = new InventoryCardViewModel_v2(doc, authUser);
          inventoryCardViewModelV2.CloseAction = new Action(((Window) this).Close);
          inventoryCardViewModelV2.ReloadJournal = loadJournal;
          InventoryCardViewModel_v2 model = inventoryCardViewModelV2;
          if (doc != null)
            this.Uid = doc.Uid.ToString();
          this.QuestionCloseAction = new Func<bool>(this.CloseCard);
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              F1help.HelpHotKey,
              (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
            },
            {
              hotKeys.OkAction,
              model.NextPageCommand
            },
            {
              hotKeys.CancelAction,
              model.PreviousPageCommand
            },
            {
              hotKeys.EditItem,
              (ICommand) new RelayCommand((Action<object>) (_ => model.EditQuantityCommand.Execute((object) this.GoodsList.SelectedItems)))
            },
            {
              hotKeys.DeleteItem,
              (ICommand) new RelayCommand((Action<object>) (_ => model.ItemDeleteCommand.Execute((object) this.GoodsList.SelectedItems)))
            }
          };
          bool access1 = new UsersRepository(dataBase).GetAccess(authUser, Actions.ViewStock);
          if (!access1)
          {
            this.GoodsList.Columns.Remove(this.GoodsList.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "3D205A8F-AD38-489A-BF93-3C3498CF57BB")));
            this.GoodsList.Columns.Remove(this.GoodsList.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "23C00E9D-068D-422E-A131-42FD43915EEF")));
          }
          model.DbQuantityVisibility = access1 ? Visibility.Visible : Visibility.Collapsed;
          model.CloseAction = new Action(((Window) this).Close);
          this.DataContext = (object) model;
          this.Show();
        }
      }
    }

    private bool CloseCard()
    {
      return ((InventoryCardViewModel_v2) this.DataContext).HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    private void UpDownBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      ((InventoryCardViewModel_v2) this.DataContext).ReCalcTotals();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/inventory/card/frminventorycard_v2.xaml", UriKind.Relative));
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
          this.GoodsList = (System.Windows.Controls.DataGrid) target;
          break;
        case 3:
          this.btnEditQuantity = (Button) target;
          break;
        case 4:
          this.btnDelete = (Button) target;
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
      if (connectionId != 2)
        return;
      ((UpDownBase<Decimal?>) target).ValueChanged += new RoutedPropertyChangedEventHandler<object>(this.UpDownBase_OnValueChanged);
    }
  }
}
