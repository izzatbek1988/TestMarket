// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountSumSale.FrmDiscountSumSaleCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.UserControls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Discounts.DiscountSumSale
{
  public class FrmDiscountSumSaleCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    internal CategorySelectionControl CategorySelectionControl;
    internal DataGrid ItemsGrid;
    private bool _contentLoaded;

    public FrmDiscountSumSaleCard() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/discounts/discountsumsale/frmdiscountsumsalecard.xaml", UriKind.Relative));
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
          this.TextBoxName = (TextBox) target;
          break;
        case 2:
          this.CategorySelectionControl = (CategorySelectionControl) target;
          break;
        case 3:
          this.ItemsGrid = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
