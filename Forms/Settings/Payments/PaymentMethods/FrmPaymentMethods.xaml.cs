// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.PaymentMethods.FrmPaymentMethods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
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
using System.Windows.Media;

#nullable disable
namespace Gbs.Forms.Settings.Payments.PaymentMethods
{
  public partial class FrmPaymentMethods : WindowWithSize, IComponentConnector
  {
    private int _prevRowIndex = -1;
    internal DataGrid GridPaymentMethodsList;
    internal Button btnAddEntity;
    internal Button btnEditEntity;
    internal Button btnDelEntity;
    private bool _contentLoaded;

    public FrmPaymentMethods()
    {
      FrmPaymentMethods frmPaymentMethods = this;
      this.InitializeComponent();
      this.GridPaymentMethodsList.Drop += new DragEventHandler(this.dgEmployee_Drop);
      this.GridPaymentMethodsList.MouseLeftButtonDown += new MouseButtonEventHandler(this.dgEmployee_PreviewMouseLeftButtonDown);
      PaymentMethodsViewModel model = (PaymentMethodsViewModel) this.DataContext;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.AddItem,
          model.AddCommand
        },
        {
          hotKeys.EditItem,
          (ICommand) new RelayCommand((Action<object>) (obj => model.EditCommand.Execute((object) frmPaymentMethods.GridPaymentMethodsList.SelectedItems)))
        },
        {
          hotKeys.DeleteItem,
          (ICommand) new RelayCommand((Action<object>) (obj => model.DeleteCommand.Execute((object) frmPaymentMethods.GridPaymentMethodsList.SelectedItems)))
        }
      };
    }

    private void dgEmployee_Drop(object sender, DragEventArgs e)
    {
      if (this._prevRowIndex < 0)
        return;
      int itemCurrentRowIndex = this.GetDataGridItemCurrentRowIndex(new FrmPaymentMethods.GetDragDropPosition(e.GetPosition));
      if (itemCurrentRowIndex < 0 || itemCurrentRowIndex == this._prevRowIndex)
        return;
      ObservableCollection<PaymentMethodsViewModel.PaymentMethodView> paymentMethods = ((PaymentMethodsViewModel) this.DataContext).PaymentMethods;
      PaymentMethodsViewModel.PaymentMethodView paymentMethodView = paymentMethods[this._prevRowIndex];
      paymentMethods.RemoveAt(this._prevRowIndex);
      paymentMethods.Insert(itemCurrentRowIndex, paymentMethodView);
    }

    private void dgEmployee_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this._prevRowIndex = this.GetDataGridItemCurrentRowIndex(new FrmPaymentMethods.GetDragDropPosition(((MouseEventArgs) e).GetPosition));
      if (this._prevRowIndex < 0)
        return;
      this.GridPaymentMethodsList.SelectedIndex = this._prevRowIndex;
      if (!(this.GridPaymentMethodsList.Items[this._prevRowIndex] is PaymentMethodsViewModel.PaymentMethodView data))
        return;
      DragDropEffects allowedEffects = DragDropEffects.Move;
      if (DragDrop.DoDragDrop((DependencyObject) this.GridPaymentMethodsList, (object) data, allowedEffects) == DragDropEffects.None)
        return;
      this.GridPaymentMethodsList.SelectedItem = (object) data;
    }

    private bool IsTheMouseOnTargetRow(Visual theTarget, FrmPaymentMethods.GetDragDropPosition pos)
    {
      return theTarget != null && VisualTreeHelper.GetDescendantBounds(theTarget).Contains(pos((IInputElement) theTarget));
    }

    private DataGridRow GetDataGridRowItem(int index)
    {
      return this.GridPaymentMethodsList.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated ? (DataGridRow) null : this.GridPaymentMethodsList.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
    }

    private int GetDataGridItemCurrentRowIndex(FrmPaymentMethods.GetDragDropPosition pos)
    {
      int itemCurrentRowIndex = -1;
      for (int index = 0; index < this.GridPaymentMethodsList.Items.Count; ++index)
      {
        if (this.IsTheMouseOnTargetRow((Visual) this.GetDataGridRowItem(index), pos))
        {
          itemCurrentRowIndex = index;
          break;
        }
      }
      return itemCurrentRowIndex;
    }

    private void FrmPaymentMethods_OnClosing(object sender, CancelEventArgs e)
    {
      ObservableCollection<PaymentMethodsViewModel.PaymentMethodView> paymentMethods = ((PaymentMethodsViewModel) this.DataContext).PaymentMethods;
      foreach (PaymentMethodsViewModel.PaymentMethodView paymentMethodView in (Collection<PaymentMethodsViewModel.PaymentMethodView>) paymentMethods)
      {
        PaymentMethodsViewModel.PaymentMethodView item = paymentMethodView;
        item.Method.DisplayIndex = paymentMethods.ToList<PaymentMethodsViewModel.PaymentMethodView>().FindIndex((Predicate<PaymentMethodsViewModel.PaymentMethodView>) (x => x.Method.Uid == item.Method.Uid));
        item.Method.Save();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/payments/paymentmethods/frmpaymentmethods.xaml", UriKind.Relative));
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
          this.GridPaymentMethodsList = (DataGrid) target;
          break;
        case 2:
          this.btnAddEntity = (Button) target;
          break;
        case 3:
          this.btnEditEntity = (Button) target;
          break;
        case 4:
          this.btnDelEntity = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private delegate Point GetDragDropPosition(IInputElement theElement);
  }
}
