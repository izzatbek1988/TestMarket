// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Inventory.FrmInventoryJournal
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Inventory
{
  public partial class FrmInventoryJournal : WindowWithSize, IComponentConnector
  {
    internal DataGrid InventoryJournalDataGrid;
    internal Button btnDelete;
    private bool _contentLoaded;

    public FrmInventoryJournal() => this.InitializeComponent();

    public void ShowInventoryJournal()
    {
      try
      {
        if (!Other.IsActiveAndShowForm<FrmInventoryJournal>())
        {
          this.IsMainForm = false;
        }
        else
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.ShowJournalInventory);
          if (!access.Result)
            return;
          InventoryJournalViewModel invm = new InventoryJournalViewModel()
          {
            AuthUser = access.User
          };
          invm.InitForm();
          this.DataContext = (object) invm;
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              hotKeys.AddItem,
              invm.AddInventoryCommand
            },
            {
              hotKeys.Print,
              (ICommand) new RelayCommand((Action<object>) (obj => invm.PrintInventoryCommand.Execute((object) this.InventoryJournalDataGrid.SelectedItems)))
            },
            {
              hotKeys.EditItem,
              (ICommand) new RelayCommand((Action<object>) (obj => invm.EditInventoryCommand.Execute((object) this.InventoryJournalDataGrid.SelectedItems)))
            },
            {
              hotKeys.DeleteItem,
              (ICommand) new RelayCommand((Action<object>) (obj => invm.DeleteInventoryCommand.Execute((object) this.InventoryJournalDataGrid.SelectedItems)))
            }
          };
          this.Object = (Control) this.InventoryJournalDataGrid;
          this.CommandEnter = invm.EditInventoryCommand;
          this.Show();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в журнале инвентаризаций");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/inventory/frminventoryjournal.xaml", UriKind.Relative));
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
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.btnDelete = (Button) target;
        else
          this._contentLoaded = true;
      }
      else
        this.InventoryJournalDataGrid = (DataGrid) target;
    }
  }
}
