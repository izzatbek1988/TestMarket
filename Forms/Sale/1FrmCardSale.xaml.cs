// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Sale.FrmCardSale
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Documents;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Sale
{
  public class FrmCardSale : WindowWithSize, IComponentConnector
  {
    internal Button ButtonPrint;
    internal DataGrid ItemsSale;
    internal DataGrid PaymentsGrid;
    internal DataGrid GridReturnSale;
    private bool _contentLoaded;

    private SaleCardViewModel SaleCard { get; set; }

    public FrmCardSale()
    {
      this.InitializeComponent();
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((UIElement) this)
        }
      };
    }

    public void ShowSaleCard(Document doc)
    {
      try
      {
        this.SaleCard = new SaleCardViewModel(doc.Uid, this.ItemsSale);
        this.DataContext = (object) this.SaleCard;
        this.ShowDialog();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме карточки продажи");
      }
    }

    private void ButtonPrint_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(this.ButtonPrint.FindResource((object) SaleCardViewModel.PrintMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) (sender as Button);
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/sale/frmcardsale.xaml", UriKind.Relative));
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
          this.ButtonPrint = (Button) target;
          this.ButtonPrint.Click += new RoutedEventHandler(this.ButtonPrint_OnClick);
          break;
        case 2:
          this.ItemsSale = (DataGrid) target;
          break;
        case 3:
          this.PaymentsGrid = (DataGrid) target;
          break;
        case 4:
          this.GridReturnSale = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
