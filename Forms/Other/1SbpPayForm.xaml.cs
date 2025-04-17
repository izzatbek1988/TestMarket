// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Other.SbpPayForm
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Other
{
  public class SbpPayForm : WindowWithSize, IComponentConnector
  {
    private bool _contentLoaded;

    public SbpPayForm() => this.InitializeComponent();

    private void SbpPayForm_OnClosing(object sender, CancelEventArgs e)
    {
      if (((SbpPayViewModel) this.DataContext).NoUserClose || MessageBoxHelper.Question(Translate.SbpPayForm_SbpPayForm_OnClosing_Вы_уверены__что_хотите_отказаться_от_оплаты_по_СБП__Убедитесь__что_покупатель_не_оплатил_заказ_) != MessageBoxResult.No)
        return;
      e.Cancel = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/other/sbppayform.xaml", UriKind.Relative));
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
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
