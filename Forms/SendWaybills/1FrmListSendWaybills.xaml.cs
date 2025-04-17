// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.SendWaybills.FrmListSendWaybills
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.SendWaybills
{
  public class FrmListSendWaybills : WindowWithSize, IComponentConnector
  {
    internal DataGrid SendWaybillJournalDataGrid;
    internal Button PrintButton;
    internal Button MoreButton;
    private bool _contentLoaded;

    public FrmListSendWaybills() => this.InitializeComponent();

    public void ShowMove()
    {
      try
      {
        if (!Other.IsActiveAndShowForm<FrmListSendWaybills>())
        {
          this.IsMainForm = false;
        }
        else
        {
          SendWaybillsJournalViewModel model = new SendWaybillsJournalViewModel(new Action(this.ShowMoreMenu), new Action(this.ShowPrintMenu));
          this.DataContext = (object) model;
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              hotKeys.AddItem,
              model.AddCommand
            },
            {
              hotKeys.Print,
              (ICommand) new RelayCommand((Action<object>) (obj => model.PrintCommand.Execute((object) this.SendWaybillJournalDataGrid.SelectedItems)))
            },
            {
              hotKeys.DeleteItem,
              (ICommand) new RelayCommand((Action<object>) (obj => model.DeleteCommand.Execute((object) this.SendWaybillJournalDataGrid.SelectedItems)))
            }
          };
          this.Object = (Control) this.SendWaybillJournalDataGrid;
          this.CommandEnter = model.PrintCommand;
          this.Show();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в журанле поступлений");
      }
    }

    private void ShowMoreMenu()
    {
      if (!(this.FindResource((object) SendWaybillsJournalViewModel.MoreMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) this.MoreButton;
      resource.IsOpen = true;
    }

    private void ShowPrintMenu()
    {
      if (!(this.FindResource((object) SendWaybillsJournalViewModel.PrintMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) this.PrintButton;
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/sendwaybills/frmlistsendwaybills.xaml", UriKind.Relative));
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
          this.SendWaybillJournalDataGrid = (DataGrid) target;
          break;
        case 2:
          this.PrintButton = (Button) target;
          break;
        case 3:
          this.MoreButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
