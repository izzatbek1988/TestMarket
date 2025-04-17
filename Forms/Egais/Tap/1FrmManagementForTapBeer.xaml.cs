// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.FrmManagementForTapBeer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Egais;
using Gbs.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Gbs.Forms.Egais
{
  public class FrmManagementForTapBeer : WindowWithSize, IComponentConnector
  {
    private int _prevRowIndex = -1;
    internal DataGrid TapDataGrid;
    internal Button ButtonMore;
    private bool _contentLoaded;

    public FrmManagementForTapBeer()
    {
      this.InitializeComponent();
      this.TapDataGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.dgEmployee_PreviewMouseLeftButtonDown);
      this.TapDataGrid.Drop += new DragEventHandler(this.dgEmployee_Drop);
      this.Object = (Control) this.TapDataGrid;
    }

    private void dgEmployee_Drop(object sender, DragEventArgs e)
    {
      if (this._prevRowIndex < 0)
        return;
      int itemCurrentRowIndex = this.GetDataGridItemCurrentRowIndex(new FrmManagementForTapBeer.GetDragDropPosition(e.GetPosition));
      if (itemCurrentRowIndex < 0 || itemCurrentRowIndex == this._prevRowIndex)
        return;
      ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem> items = ((ManagementForTapBeerViewModel) this.DataContext).Items;
      ManagementForTapBeerViewModel.InfoTapBeerItem infoTapBeerItem = items[this._prevRowIndex];
      items.RemoveAt(this._prevRowIndex);
      items.Insert(itemCurrentRowIndex, infoTapBeerItem);
      ((ManagementForTapBeerViewModel) this.DataContext).IsEditing = true;
    }

    private void dgEmployee_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
      {
        ((ManagementForTapBeerViewModel) this.DataContext).EditTapCommand.Execute((object) this.TapDataGrid.SelectedItems);
      }
      else
      {
        this._prevRowIndex = this.GetDataGridItemCurrentRowIndex(new FrmManagementForTapBeer.GetDragDropPosition(((MouseEventArgs) e).GetPosition));
        if (this._prevRowIndex < 0)
          return;
        this.TapDataGrid.SelectedIndex = this._prevRowIndex;
        if (!(this.TapDataGrid.Items[this._prevRowIndex] is ManagementForTapBeerViewModel.InfoTapBeerItem data))
          return;
        DragDropEffects allowedEffects = DragDropEffects.Move;
        if (DragDrop.DoDragDrop((DependencyObject) this.TapDataGrid, (object) data, allowedEffects) == DragDropEffects.None)
          return;
        this.TapDataGrid.SelectedItem = (object) data;
      }
    }

    private bool IsTheMouseOnTargetRow(
      Visual theTarget,
      FrmManagementForTapBeer.GetDragDropPosition pos)
    {
      return theTarget != null && VisualTreeHelper.GetDescendantBounds(theTarget).Contains(pos((IInputElement) theTarget));
    }

    private DataGridRow GetDataGridRowItem(int index)
    {
      return this.TapDataGrid.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated ? (DataGridRow) null : this.TapDataGrid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
    }

    private int GetDataGridItemCurrentRowIndex(FrmManagementForTapBeer.GetDragDropPosition pos)
    {
      int itemCurrentRowIndex = -1;
      for (int index = 0; index < this.TapDataGrid.Items.Count; ++index)
      {
        if (this.IsTheMouseOnTargetRow((Visual) this.GetDataGridRowItem(index), pos))
        {
          itemCurrentRowIndex = index;
          break;
        }
      }
      return itemCurrentRowIndex;
    }

    private void FrmManagementForTapBeer_OnClosing(object sender, CancelEventArgs e)
    {
      if (!((ManagementForTapBeerViewModel) this.DataContext).IsEditing)
        return;
      ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem> items = ((ManagementForTapBeerViewModel) this.DataContext).Items;
      foreach (ManagementForTapBeerViewModel.InfoTapBeerItem infoTapBeerItem in (Collection<ManagementForTapBeerViewModel.InfoTapBeerItem>) items)
      {
        ManagementForTapBeerViewModel.InfoTapBeerItem item = infoTapBeerItem;
        item.Info.Tap.Index = items.ToList<ManagementForTapBeerViewModel.InfoTapBeerItem>().FindIndex((Predicate<ManagementForTapBeerViewModel.InfoTapBeerItem>) (x => x.Info.Tap.Uid == item.Info.Tap.Uid));
        new TapBeerRepository().Save(item.Info.Tap);
      }
    }

    public void ShowMoreMenu()
    {
      if (!(this.FindResource((object) ManagementForTapBeerViewModel.MoreMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) this.ButtonMore;
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/egais/tap/frmmanagementfortapbeer.xaml", UriKind.Relative));
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
          this.ButtonMore = (Button) target;
        else
          this._contentLoaded = true;
      }
      else
        this.TapDataGrid = (DataGrid) target;
    }

    private delegate Point GetDragDropPosition(IInputElement theElement);
  }
}
