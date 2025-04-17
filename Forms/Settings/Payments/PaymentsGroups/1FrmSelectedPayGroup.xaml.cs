// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.PaymentsGroups.FrmSelectedPayGroup
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.UserControls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Payments.PaymentsGroups
{
  public class FrmSelectedPayGroup : WindowWithSize, IComponentConnector
  {
    internal TextBoxWithClearControl SearchTb;
    internal TreeView treeGroup;
    private bool _contentLoaded;

    private GroupsSelectedViewModel ViewModel { get; set; }

    public FrmSelectedPayGroup() => this.InitializeComponent();

    public bool GetSingleSelectedGroupUid(
      out PaymentGroups.PaymentGroup group,
      PaymentGroups.VisiblePaymentGroup enumGroup)
    {
      this.ViewModel = new GroupsSelectedViewModel(new List<GoodGroups.Group>(), enumGroup)
      {
        Close = new Action(((Window) this).Close)
      };
      this.DataContext = (object) this.ViewModel;
      this.ShowDialog();
      group = this.ViewModel.SelectedGroup?.Group;
      return this.ViewModel.Result;
    }

    private void TreeGroup_OnSelectedItemChanged(
      object sender,
      RoutedPropertyChangedEventArgs<object> e)
    {
      this.ViewModel.SelectedGroup = (GroupsSelectedViewModel.GroupWithChilds) this.treeGroup.SelectedValue;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/payments/paymentsgroups/frmselectedpaygroup.xaml", UriKind.Relative));
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
        {
          this.treeGroup = (TreeView) target;
          this.treeGroup.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.TreeGroup_OnSelectedItemChanged);
        }
        else
          this._contentLoaded = true;
      }
      else
        this.SearchTb = (TextBoxWithClearControl) target;
    }
  }
}
