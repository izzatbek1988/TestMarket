// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.FrmProductionCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms
{
  public class FrmProductionCard : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid ItemsProductionGrid;
    private bool _contentLoaded;

    public FrmProductionCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
      this.ItemsProductionGrid.AddGoodsPropertiesColumns();
    }

    private bool CloseCard()
    {
      ProductionCardViewModel dataContext = (ProductionCardViewModel) this.DataContext;
      return (dataContext != null ? (dataContext.HasNoSavedChanges() ? 1 : 0) : 1) != 0 || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    private void FrmProductionCard_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.ItemsProductionGrid.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ItemsProductionGrid.Columns)
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = column.Header;
        newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) column);
        newItem.IsCheckable = true;
        newItem.IsChecked = column.Visibility == Visibility.Visible;
        items.Add((object) newItem);
      }
      resource.Closed += new RoutedEventHandler(this.CmOnClosed);
    }

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Gbs.Helpers.Other.IsVisibilityDataGridColumn(this.ItemsProductionGrid, (ContextMenu) sender);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/productions/frmproductioncard.xaml", UriKind.Relative));
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
        this.ItemsProductionGrid = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
