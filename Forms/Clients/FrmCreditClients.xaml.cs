// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.FrmCreditClients
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
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
namespace Gbs.Forms.Clients
{
  public partial class FrmCreditClients : WindowWithSize, IComponentConnector
  {
    internal DateFilterControl DateFilterControl;
    internal ClientSelectionControl ClientSelectionControl;
    internal System.Windows.Controls.DataGrid CreditGrid;
    internal Button ButtonUpdateData;
    internal Button ButtonSaleInfo;
    internal Button ButtonDoPayment;
    private bool _contentLoaded;

    public FrmCreditClients()
    {
      this.InitializeComponent();
      this.CreditGrid.CreateContextMenu();
      TooltipsSetter.Set(this);
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((UIElement) this)
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/clients/frmcreditclients.xaml", UriKind.Relative));
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
          this.DateFilterControl = (DateFilterControl) target;
          break;
        case 2:
          this.ClientSelectionControl = (ClientSelectionControl) target;
          break;
        case 3:
          this.CreditGrid = (System.Windows.Controls.DataGrid) target;
          break;
        case 4:
          this.ButtonUpdateData = (Button) target;
          break;
        case 5:
          this.ButtonSaleInfo = (Button) target;
          break;
        case 6:
          this.ButtonDoPayment = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
