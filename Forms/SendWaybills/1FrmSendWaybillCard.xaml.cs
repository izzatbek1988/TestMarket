// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.SendWaybills.FrmSendWaybillCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.SendWaybills
{
  public class FrmSendWaybillCard : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid ItemsWriteOffGrid;
    private bool _contentLoaded;

    private SendWaybillCardViewModel Model { get; set; }

    public FrmSendWaybillCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
      this.ItemsWriteOffGrid.AddGoodsPropertiesColumns();
    }

    public bool ShowCard(
      Guid uid,
      out Document document,
      Gbs.Core.Entities.Users.User authUser = null,
      Move basket = null,
      Action updateAction = null)
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
        document = (Document) null;
        return false;
      }
      if (!Gbs.Helpers.Other.IsActiveAndShowForm<FrmSendWaybillCard>(uid.ToString()))
      {
        document = (Document) null;
        return false;
      }
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref authUser, Actions.AddMoveWaybill))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.AddMoveWaybill);
          if (!access.Result)
          {
            document = (Document) null;
            return false;
          }
          authUser = access.User;
        }
        document = uid == Guid.Empty ? new Document() : new DocumentsRepository(dataBase).GetByUid(uid);
        this.Model = new SendWaybillCardViewModel(document, new Action(((Window) this).Close))
        {
          AuthUser = authUser,
          ActionUpdate = updateAction
        };
        if (basket != null)
        {
          basket.Document = document;
          this.Model.SendWaybill = basket;
        }
        if (uid != Guid.Empty)
          this.Model.SendWaybill.Storage = document.Storage;
        this.DataContext = (object) this.Model;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.AddItem,
            this.Model.AddItem
          },
          {
            hotKeys.EditItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.SendWaybill.EditQuantityCommand.Execute((object) this.ItemsWriteOffGrid.SelectedItems)))
          },
          {
            hotKeys.DeleteItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.SendWaybill.DeleteItemCommand.Execute((object) this.ItemsWriteOffGrid.SelectedItems)))
          },
          {
            hotKeys.OkAction,
            this.Model.SaveCommand
          },
          {
            hotKeys.CancelAction,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Close()))
          }
        };
        if (this.Model.IsShow)
          this.Show();
        return this.Model.IsResult;
      }
    }

    private bool CloseCard()
    {
      SendWaybillCardViewModel model = this.Model;
      // ISSUE: explicit non-virtual call
      return (model != null ? (__nonvirtual (model.HasNoSavedChanges()) ? 1 : 0) : 1) != 0 || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    private void FrmWriteOffCard_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.ItemsWriteOffGrid.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ItemsWriteOffGrid.Columns)
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

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Gbs.Helpers.Other.IsVisibilityDataGridColumn(this.ItemsWriteOffGrid, (ContextMenu) sender);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/sendwaybills/frmsendwaybillcard.xaml", UriKind.Relative));
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
      if (connectionId == 1)
        this.ItemsWriteOffGrid = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
