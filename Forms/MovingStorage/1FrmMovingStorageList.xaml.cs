// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.MovingStorage.FrmMovingStorageList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
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
namespace Gbs.Forms.MovingStorage
{
  public class FrmMovingStorageList : WindowWithSize, IComponentConnector
  {
    internal DataGrid SendWaybillJournalDataGrid;
    private bool _contentLoaded;

    public FrmMovingStorageList()
    {
    }

    public FrmMovingStorageList(SendStorageJournalViewModel viewModel)
    {
      FrmMovingStorageList movingStorageList = this;
      this.InitializeComponent();
      this.DataContext = (object) viewModel;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.AddItem,
          viewModel.AddCommand
        },
        {
          hotKeys.Print,
          (ICommand) new RelayCommand((Action<object>) (obj => viewModel.PrintCommand.Execute((object) movingStorageList.SendWaybillJournalDataGrid.SelectedItems)))
        },
        {
          hotKeys.DeleteItem,
          (ICommand) new RelayCommand((Action<object>) (obj => viewModel.DeleteCommand.Execute((object) movingStorageList.SendWaybillJournalDataGrid.SelectedItems)))
        }
      };
      this.Object = (Control) this.SendWaybillJournalDataGrid;
      this.CommandEnter = viewModel.PrintCommand;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/movingstorage/frmmovingstoragelist.xaml", UriKind.Relative));
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
        this.SendWaybillJournalDataGrid = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
