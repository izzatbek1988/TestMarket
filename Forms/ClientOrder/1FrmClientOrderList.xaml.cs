// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ClientOrder.ClientOrderList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
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
namespace Gbs.Forms.ClientOrder
{
  public class ClientOrderList : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid ListClientOrder;
    private bool _contentLoaded;

    public ClientOrderList()
    {
      this.InitializeComponent();
      this.ListClientOrder.CreateContextMenu();
    }

    public void ShowList(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      if (!Gbs.Helpers.Other.IsActiveAndShowForm<ClientOrderList>())
      {
        this.IsMainForm = false;
      }
      else
      {
        ClientOrderListViewModel orderListViewModel = new ClientOrderListViewModel(basket);
        orderListViewModel.CloseAction = new Action(((Window) this).Close);
        this.DataContext = (object) orderListViewModel;
        this.Object = (Control) this.ListClientOrder;
        this.SetHotKeys();
        this.Show();
      }
    }

    private void SetHotKeys()
    {
      try
      {
        ClientOrderListViewModel model = (ClientOrderListViewModel) this.DataContext;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand1 = new RelayCommand((Action<object>) (obj => model.EditCommand.Execute((object) this.ListClientOrder.SelectedItems)));
        RelayCommand relayCommand2 = new RelayCommand((Action<object>) (obj => model.DeleteCommand.Execute((object) this.ListClientOrder.SelectedItems)));
        RelayCommand relayCommand3 = new RelayCommand((Action<object>) (o => model.CloseAction()));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.AddItem,
            model.AddOrderCommand
          },
          {
            hotKeys.EditItem,
            (ICommand) relayCommand1
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand3
          },
          {
            hotKeys.DeleteItem,
            (ICommand) relayCommand2
          },
          {
            hotKeys.Print,
            model.PrintOrdersCommand
          },
          {
            new HotKeysHelper.Hotkey(Key.Return),
            (ICommand) relayCommand1
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand3
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/clientorder/frmclientorderlist.xaml", UriKind.Relative));
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
        this.ListClientOrder = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
