// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ClientOrder.ClientOrderCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.ClientOrder
{
  public class ClientOrderCard : WindowWithSize, IComponentConnector
  {
    internal CheckBox ActualityCb;
    internal System.Windows.Controls.DataGrid GridClientOrder;
    internal System.Windows.Controls.DataGrid GridWaybillPayments;
    private bool _contentLoaded;

    public ClientOrderCard()
    {
      this.InitializeComponent();
      this.GridClientOrder.CreateContextMenu();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
      this.GridClientOrder.AddGoodsPropertiesColumns();
    }

    private bool CloseCard()
    {
      return ((ClientOrderViewModel) this.DataContext).HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/clientorder/frmclientordercard.xaml", UriKind.Relative));
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
          this.ActualityCb = (CheckBox) target;
          break;
        case 2:
          this.GridClientOrder = (System.Windows.Controls.DataGrid) target;
          break;
        case 3:
          this.GridWaybillPayments = (System.Windows.Controls.DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
