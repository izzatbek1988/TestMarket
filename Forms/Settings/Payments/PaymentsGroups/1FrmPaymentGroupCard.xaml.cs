// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.PaymentsGroups.FrmPaymentGroupCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
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
namespace Gbs.Forms.Settings.Payments.PaymentsGroups
{
  public class FrmPaymentGroupCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TxtName;
    internal Button BtnSelectParent;
    internal ComboBox CmoGoodsType;
    private bool _contentLoaded;

    public FrmPaymentGroupCard() => this.InitializeComponent();

    public bool ShowGroupCard(
      Guid groupUid,
      out PaymentGroups.PaymentGroup group,
      PaymentGroups.PaymentGroup parent)
    {
      try
      {
        group = groupUid == Guid.Empty ? new PaymentGroups.PaymentGroup() : PaymentGroups.GetPaymentGroupByUid(groupUid);
        groupUid = group.Uid;
        if (parent != null && parent.Uid != groupUid)
        {
          group.VisibleIn = parent.VisibleIn;
          group.ParentGroup = parent.Uid == Guid.Empty ? (PaymentGroups.PaymentGroup) null : parent;
        }
        GroupPaymentCardViewModel paymentCardViewModel = new GroupPaymentCardViewModel(group)
        {
          CloseFrm = new Action(((Window) this).Close)
        };
        this.DataContext = (object) paymentCardViewModel;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            paymentCardViewModel.SaveGroup
          },
          {
            hotKeys.CancelAction,
            paymentCardViewModel.CloseCard
          }
        };
        this.ShowDialog();
        return paymentCardViewModel.SaveResult;
      }
      catch (Exception ex)
      {
        group = (PaymentGroups.PaymentGroup) null;
        LogHelper.Error(ex, "Ошибка в форме карточки группы движения средств");
        return false;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/payments/paymentsgroups/frmpaymentgroupcard.xaml", UriKind.Relative));
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
          this.TxtName = (TextBox) target;
          break;
        case 2:
          this.BtnSelectParent = (Button) target;
          break;
        case 3:
          this.CmoGoodsType = (ComboBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
