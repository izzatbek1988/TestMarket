// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.WriteOff.FrmWriteOffJournal
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.WriteOff
{
  public class FrmWriteOffJournal : WindowWithSize, IComponentConnector
  {
    internal DataGrid WriteOffJournalDataGrid;
    private bool _contentLoaded;

    public FrmWriteOffJournal() => this.InitializeComponent();

    public void ShowCard()
    {
      try
      {
        if (!Other.IsActiveAndShowForm<FrmWriteOffJournal>())
        {
          this.IsMainForm = false;
        }
        else
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.ShowJournalWriteOff);
          if (!access.Result)
            return;
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            Visibility visibility = Visibility.Visible;
            if (!new UsersRepository(dataBase).GetAccess(access.User, Actions.ShowBuyPrice))
            {
              this.WriteOffJournalDataGrid.Columns.Remove(this.WriteOffJournalDataGrid.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C")));
              visibility = Visibility.Collapsed;
            }
            WriteOffJournalViewModel model = (WriteOffJournalViewModel) this.DataContext;
            model.AuthUser = access.User;
            model.VisibilityBuySum = visibility;
            HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
            this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
            {
              {
                hotKeys.AddItem,
                model.AddCommand
              },
              {
                hotKeys.Print,
                (ICommand) new RelayCommand((Action<object>) (obj => model.PrintCommand.Execute((object) this.WriteOffJournalDataGrid.SelectedItems)))
              },
              {
                hotKeys.DeleteItem,
                (ICommand) new RelayCommand((Action<object>) (obj => model.DeleteCommand.Execute((object) this.WriteOffJournalDataGrid.SelectedItems)))
              }
            };
            this.Object = (Control) this.WriteOffJournalDataGrid;
            this.SearchTextBox = this.SearchTextBox;
            this.CommandEnter = model.EditCommand;
            this.Show();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в журнале списаний");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/writeoff/frmwriteoffjournal.xaml", UriKind.Relative));
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
        this.WriteOffJournalDataGrid = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
