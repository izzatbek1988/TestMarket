// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.FrmOrderCombine
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using System;
using System.CodeDom.Compiler;
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
  public class FrmOrderCombine : WindowWithSize, IComponentConnector, IStyleConnector
  {
    internal System.Windows.Controls.DataGrid DataGridOrdersCombine;
    private bool _contentLoaded;

    public FrmOrderCombine()
    {
      this.InitializeComponent();
      if (new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().IsTableAndGuest)
        return;
      this.DataGridOrdersCombine.Columns.Remove(this.DataGridOrdersCombine.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == GlobalDictionaries.CountGuestUidString)));
      this.DataGridOrdersCombine.Columns.Remove(this.DataGridOrdersCombine.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == GlobalDictionaries.NumTableUidString)));
    }

    private void MouseEnterHandler(object sender, MouseEventArgs e)
    {
      if (!(e.OriginalSource is DataGridRow originalSource) || e.LeftButton != MouseButtonState.Pressed)
        return;
      originalSource.IsSelected = !originalSource.IsSelected;
      e.Handled = true;
    }

    private void PreviewMouseDownHandler(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton != MouseButtonState.Pressed)
        return;
      if (Gbs.Helpers.ControlsHelpers.DataGrid.Other.GetVisualParentByType((DependencyObject) e.OriginalSource, typeof (DataGridRow)) is DataGridRow visualParentByType)
        visualParentByType.IsSelected = !visualParentByType.IsSelected;
      e.Handled = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/cafe/actionorder/frmordercombine.xaml", UriKind.Relative));
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
        this.DataGridOrdersCombine = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 2)
        return;
      ((Style) target).Setters.Add((SetterBase) new EventSetter()
      {
        Event = System.Windows.UIElement.MouseEnterEvent,
        Handler = (Delegate) new MouseEventHandler(this.MouseEnterHandler)
      });
      ((Style) target).Setters.Add((SetterBase) new EventSetter()
      {
        Event = System.Windows.UIElement.PreviewMouseDownEvent,
        Handler = (Delegate) new MouseButtonEventHandler(this.PreviewMouseDownHandler)
      });
    }
  }
}
