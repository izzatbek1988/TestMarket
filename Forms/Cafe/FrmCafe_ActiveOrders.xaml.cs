// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.FrmCafeActiveOrders
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
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
namespace Gbs.Forms.Cafe
{
  public partial class FrmCafeActiveOrders : WindowWithSize, IComponentConnector
  {
    internal Expander ExpanderFilters;
    internal System.Windows.Controls.DataGrid ActiveOrderDataGrid;
    internal Button ButtonComment;
    internal Button ButtonPrint;
    internal Button ButtonDelete;
    internal Button ButtonAlsoMenu;
    internal ConfirmPanelControl1 ConfirmPanelControl1;
    private bool _contentLoaded;

    public FrmCafeActiveOrders()
    {
      this.InitializeComponent();
      this.ActiveOrderDataGrid.CreateContextMenu();
      if (!new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().IsTableAndGuest)
      {
        this.ActiveOrderDataGrid.Columns.Remove(this.ActiveOrderDataGrid.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == GlobalDictionaries.CountGuestUidString)));
        this.ActiveOrderDataGrid.Columns.Remove(this.ActiveOrderDataGrid.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == GlobalDictionaries.NumTableUidString)));
      }
      TooltipsSetter.Set(this);
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
        }
      };
    }

    public void ShowPrintMenu()
    {
      if (!(this.FindResource((object) CafeActiveOrdersViewModel.AlsoMenuKey) is ContextMenu resource))
        return;
      resource.PlacementTarget = (System.Windows.UIElement) this.ButtonAlsoMenu;
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/cafe/frmcafe_activeorders.xaml", UriKind.Relative));
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
          this.ExpanderFilters = (Expander) target;
          break;
        case 2:
          this.ActiveOrderDataGrid = (System.Windows.Controls.DataGrid) target;
          break;
        case 3:
          this.ButtonComment = (Button) target;
          break;
        case 4:
          this.ButtonPrint = (Button) target;
          break;
        case 5:
          this.ButtonDelete = (Button) target;
          break;
        case 6:
          this.ButtonAlsoMenu = (Button) target;
          break;
        case 7:
          this.ConfirmPanelControl1 = (ConfirmPanelControl1) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
