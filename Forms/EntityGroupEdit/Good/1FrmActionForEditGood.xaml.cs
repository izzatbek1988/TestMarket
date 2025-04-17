// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroupEdit.FrmActionForEditGood
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
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
namespace Gbs.Forms.GoodGroupEdit
{
  public class FrmActionForEditGood : WindowWithSize, IComponentConnector
  {
    internal CheckBox IsEditGroupCb;
    internal CheckBox CheckBoxEditPrice;
    internal RadioButton RbPriceEqual;
    internal RadioButton RbPricing;
    internal CheckBox CheckBoxPrint;
    private bool _contentLoaded;

    public FrmActionForEditGood()
    {
      this.InitializeComponent();
      ActionGoodEditViewModel dataContext = (ActionGoodEditViewModel) this.DataContext;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((UIElement) this)
        },
        {
          hotKeys.OkAction,
          dataContext.DoActionCommand
        },
        {
          hotKeys.CancelAction,
          dataContext.CloseCommand
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/entitygroupedit/good/frmactionforeditgood.xaml", UriKind.Relative));
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
          this.IsEditGroupCb = (CheckBox) target;
          break;
        case 2:
          this.CheckBoxEditPrice = (CheckBox) target;
          break;
        case 3:
          this.RbPriceEqual = (RadioButton) target;
          break;
        case 4:
          this.RbPricing = (RadioButton) target;
          break;
        case 5:
          this.CheckBoxPrint = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
