// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Excel.FrmWaybillOutExcel
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
namespace Gbs.Forms.Excel
{
  public class FrmWaybillOutExcel : WindowWithSize, IComponentConnector
  {
    internal RadioButton RbSelectedGroup;
    internal RadioButton RbGroup;
    internal RadioButton RbNewGroup;
    internal RadioButton RbEmptyGroup;
    internal RadioButton RbBuyIndex;
    internal RadioButton RbBuySum;
    internal RadioButton RbPrice;
    internal RadioButton RbPriceFormula;
    internal CheckBox RbRound;
    private bool _contentLoaded;

    public FrmWaybillOutExcel() => this.InitializeComponent();

    public (bool Result, List<GoodItem> goods, List<GoodItem> newGoods) ImportOutExcel(
      Users.User user)
    {
      WaybillInExcelViewModel inExcelViewModel1 = new WaybillInExcelViewModel();
      inExcelViewModel1.CloseAction = new Action(((Window) this).Close);
      WaybillInExcelViewModel inExcelViewModel2 = inExcelViewModel1;
      inExcelViewModel2.AuthUser = user;
      this.DataContext = (object) inExcelViewModel2;
      this.ShowDialog();
      return (inExcelViewModel2.Result, inExcelViewModel2.ItemsGood, inExcelViewModel2.ItemsGoodInDb);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/excel/frmwaybilloutexcel.xaml", UriKind.Relative));
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
          this.RbSelectedGroup = (RadioButton) target;
          break;
        case 2:
          this.RbGroup = (RadioButton) target;
          break;
        case 3:
          this.RbNewGroup = (RadioButton) target;
          break;
        case 4:
          this.RbEmptyGroup = (RadioButton) target;
          break;
        case 5:
          this.RbBuyIndex = (RadioButton) target;
          break;
        case 6:
          this.RbBuySum = (RadioButton) target;
          break;
        case 7:
          this.RbPrice = (RadioButton) target;
          break;
        case 8:
          this.RbPriceFormula = (RadioButton) target;
          break;
        case 9:
          this.RbRound = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
