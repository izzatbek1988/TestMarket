// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.FrmSummaryReport
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
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
namespace Gbs.Forms.Reports.SummaryReport
{
  public class FrmSummaryReport : WindowWithSize, IComponentConnector
  {
    internal Button ButtonShowTotalBalance;
    internal StackPanel PanelKkmSum;
    internal DateFilterControl DateFilterControl;
    internal TextBlock BlockRevenueSum;
    internal TextBlock BlockRevenueChangePercent;
    internal StackPanel PanelProfitSum;
    internal TextBlock BlockProfitSum;
    internal TextBlock BlockProfitChangePercent;
    internal TextBlock BlockProfitPercent;
    internal StackPanel PanelAvgCheckSum;
    internal TextBlock BlockAvgCheckChangePercent;
    internal Button ButtonSales;
    internal Button ButtonCredits;
    internal Button ButtonReturns;
    internal Button ButtonPayments;
    internal Button ButtonCashOut;
    internal Button ButtonCashIn;
    private bool _contentLoaded;

    public void ShowReport()
    {
      (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.ShowSummaryReport);
      if (!access.Result)
        return;
      this.InitializeComponent();
      ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.FrmSummaryReport_ShowReport_Открыт_сводный_отчет, access.User), true);
      this.DataContext = (object) new SummaryReportViewModel(access.User)
      {
        ValueDateTimeStart = DateTime.Now,
        ValueDateTimeEnd = DateTime.Now
      };
      TooltipsSetter.Set(this);
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((UIElement) this)
        }
      };
      this.ShowDialog();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/reports/summaryreport/frmsummaryreport.xaml", UriKind.Relative));
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
          this.ButtonShowTotalBalance = (Button) target;
          break;
        case 2:
          this.PanelKkmSum = (StackPanel) target;
          break;
        case 3:
          this.DateFilterControl = (DateFilterControl) target;
          break;
        case 4:
          this.BlockRevenueSum = (TextBlock) target;
          break;
        case 5:
          this.BlockRevenueChangePercent = (TextBlock) target;
          break;
        case 6:
          this.PanelProfitSum = (StackPanel) target;
          break;
        case 7:
          this.BlockProfitSum = (TextBlock) target;
          break;
        case 8:
          this.BlockProfitChangePercent = (TextBlock) target;
          break;
        case 9:
          this.BlockProfitPercent = (TextBlock) target;
          break;
        case 10:
          this.PanelAvgCheckSum = (StackPanel) target;
          break;
        case 11:
          this.BlockAvgCheckChangePercent = (TextBlock) target;
          break;
        case 12:
          this.ButtonSales = (Button) target;
          break;
        case 13:
          this.ButtonCredits = (Button) target;
          break;
        case 14:
          this.ButtonReturns = (Button) target;
          break;
        case 15:
          this.ButtonPayments = (Button) target;
          break;
        case 16:
          this.ButtonCashOut = (Button) target;
          break;
        case 17:
          this.ButtonCashIn = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
