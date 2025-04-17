// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Payments.PaymentsGroups.FrmListPaymentGroup
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
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
namespace Gbs.Forms.Payments.PaymentsGroups
{
  public class FrmListPaymentGroup : WindowWithSize, IComponentConnector
  {
    internal TreeView treeGroup;
    internal Button btnAddGroup;
    internal Button btnEditGroup;
    internal Button btnDelGroup;
    private bool _contentLoaded;

    private PaymentsGroupViewModel ViewModel { get; }

    public FrmListPaymentGroup()
    {
      try
      {
        this.InitializeComponent();
        this.ViewModel = new PaymentsGroupViewModel();
        this.DataContext = (object) this.ViewModel;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.AddItem,
            this.ViewModel.AddCommand
          },
          {
            hotKeys.EditItem,
            this.ViewModel.EditCommand
          },
          {
            hotKeys.DeleteItem,
            this.ViewModel.DeleteCommand
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка групп движения средств");
      }
    }

    private void TreeGroup_OnSelectedItemChanged(
      object sender,
      RoutedPropertyChangedEventArgs<object> e)
    {
      this.ViewModel.SelectedGroup = (PaymentsGroupViewModel.GroupWithChilds) this.treeGroup.SelectedValue;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/payments/paymentsgroups/frmlistpaymentgroup.xaml", UriKind.Relative));
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
          this.treeGroup = (TreeView) target;
          this.treeGroup.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.TreeGroup_OnSelectedItemChanged);
          break;
        case 2:
          this.btnAddGroup = (Button) target;
          break;
        case 3:
          this.btnEditGroup = (Button) target;
          break;
        case 4:
          this.btnDelGroup = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
