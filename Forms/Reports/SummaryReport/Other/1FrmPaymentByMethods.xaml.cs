// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.Other.FrmPaymentByMethods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport.Other
{
  public class FrmPaymentByMethods : WindowWithSize, IComponentConnector
  {
    internal DataGrid GridPaymentsSum;
    private bool _contentLoaded;

    public FrmPaymentByMethods() => this.InitializeComponent();

    public void ShowDataPayment(List<Payments.Payment> payments)
    {
      this.DataContext = (object) new PaymentByMethodsViewModel(payments);
      this.ShowDialog();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/reports/summaryreport/other/frmpaymentbymethods.xaml", UriKind.Relative));
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
        this.GridPaymentsSum = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
