// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Lable.FrmLablePrint
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Lable
{
  public class FrmLablePrint : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid ListGoodsLable;
    private bool _contentLoaded;

    private LablePrintViewModel Model { get; set; }

    public FrmLablePrint()
    {
      this.InitializeComponent();
      this.Closing += new CancelEventHandler(this.OnClosing);
      this.ListGoodsLable.AddGoodsPropertiesColumns();
    }

    private void OnClosing(object sender, CancelEventArgs e)
    {
      if (!this.Model.IsVisibleMessage || MessageBoxHelper.Show(Translate.FrmGoodsForGroupEdit_Вы_уверены__что_хотите_закрыть_, buttons: MessageBoxButton.YesNo) != MessageBoxResult.No)
        return;
      e.Cancel = true;
    }

    public void Print(LablePrintViewModel.Types type, List<BasketItem> items = null)
    {
      LablePrintViewModel lablePrintViewModel = new LablePrintViewModel();
      lablePrintViewModel.CloseAction = new Action(((Window) this).Close);
      lablePrintViewModel.Type = type;
      this.Model = lablePrintViewModel;
      this.Model.Load();
      if (items != null)
        this.Model.Lable.Items = new ObservableCollection<BasketItem>(items);
      this.DataContext = (object) this.Model;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.AddItem,
          this.Model.AddItem
        },
        {
          hotKeys.DeleteItem,
          (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Lable.DeleteItemCommand.Execute((object) this.ListGoodsLable.SelectedItems)))
        },
        {
          hotKeys.EditItem,
          (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Lable.EditQuantityCommand.Execute((object) this.ListGoodsLable.SelectedItems)))
        },
        {
          hotKeys.OkAction,
          this.Model.PrintItemCommand
        },
        {
          hotKeys.CancelAction,
          this.Model.CloseCommand
        }
      };
      this.Model.FormToSHow = (WindowWithSize) this;
      this.Show();
    }

    private void FrmLablePrint_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.ListGoodsLable.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ListGoodsLable.Columns)
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
      Gbs.Helpers.Other.IsVisibilityDataGridColumn(this.ListGoodsLable, (ContextMenu) sender);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/lable/frmlableprint.xaml", UriKind.Relative));
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
        this.ListGoodsLable = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
