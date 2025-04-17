// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.MovingStorage.FrmSendStorageCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
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
namespace Gbs.Forms.MovingStorage
{
  public class FrmSendStorageCard : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid ItemsMoveGrid;
    private bool _contentLoaded;

    public FrmSendStorageCard()
    {
    }

    public FrmSendStorageCard(SendStorageCardViewModel model)
    {
      this.InitializeComponent();
      this.DataContext = (object) model;
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
      this.ItemsMoveGrid.AddGoodsPropertiesColumns();
      SendStorageCardViewModel viewModel = (SendStorageCardViewModel) this.DataContext;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.AddItem,
          viewModel.AddItem
        },
        {
          hotKeys.EditItem,
          (ICommand) new RelayCommand((Action<object>) (obj => viewModel.SendStorage.EditQuantityCommand.Execute((object) this.ItemsMoveGrid.SelectedItems)))
        },
        {
          hotKeys.DeleteItem,
          (ICommand) new RelayCommand((Action<object>) (obj => viewModel.SendStorage.DeleteItemCommand.Execute((object) this.ItemsMoveGrid.SelectedItems)))
        },
        {
          hotKeys.OkAction,
          viewModel.SaveCommand
        },
        {
          hotKeys.CancelAction,
          (ICommand) new RelayCommand((Action<object>) (obj => this.Close()))
        }
      };
    }

    private bool CloseCard()
    {
      SendStorageCardViewModel dataContext = (SendStorageCardViewModel) this.DataContext;
      return (dataContext != null ? (dataContext.HasNoSavedChanges() ? 1 : 0) : 1) != 0 || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    private void FrmSendStorageCard_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.ItemsMoveGrid.FindResource((object) "ContextMenuGrid");
      foreach (DataGridColumn column in (Collection<DataGridColumn>) this.ItemsMoveGrid.Columns)
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
      Gbs.Helpers.Other.IsVisibilityDataGridColumn(this.ItemsMoveGrid, (ContextMenu) sender);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/movingstorage/frmsendstoragecard.xaml", UriKind.Relative));
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
        this.ItemsMoveGrid = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
