// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Discounts.DiscountForWeekday.FrmDiscountWeekdayList
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
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
namespace Gbs.Forms.Settings.Discounts.DiscountForWeekday
{
  public class FrmDiscountWeekdayList : WindowWithSize, IComponentConnector
  {
    internal DataGrid GridDiscountForDayList;
    private bool _contentLoaded;

    public FrmDiscountWeekdayList()
    {
      FrmDiscountWeekdayList discountWeekdayList = this;
      this.InitializeComponent();
      DiscountForWeekDayListViewModel model = (DiscountForWeekDayListViewModel) this.DataContext;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.AddItem,
          model.AddCommand
        },
        {
          hotKeys.EditItem,
          (ICommand) new RelayCommand((Action<object>) (obj => model.EditCommand.Execute((object) discountWeekdayList.GridDiscountForDayList.SelectedItems)))
        },
        {
          hotKeys.DeleteItem,
          (ICommand) new RelayCommand((Action<object>) (obj => model.DeleteCommand.Execute((object) discountWeekdayList.GridDiscountForDayList.SelectedItems)))
        }
      };
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/discounts/discountforweekday/frmdiscountweekdaylist.xaml", UriKind.Relative));
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
        this.GridDiscountForDayList = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
