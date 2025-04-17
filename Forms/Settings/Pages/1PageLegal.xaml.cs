// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageLegal
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class PageLegal : Page, IComponentConnector
  {
    internal TabControl BillTabControl;
    internal TabItem CrptTabItem;
    internal CheckBox CheckMarkCheckBox;
    internal TabItem EgaisTabItem;
    internal CheckBox CheckBoxEgais;
    internal CheckBox TimeCheckBox;
    private bool _contentLoaded;

    public PageLegal() => this.InitializeComponent();

    public PageLegal(Gbs.Core.Config.Settings settings, Integrations integrations)
    {
      this.InitializeComponent();
      this.DataContext = (object) new SettingBillViewModel(settings, integrations)
      {
        BillTabControl = this.BillTabControl
      };
    }

    public bool Save() => ((SettingBillViewModel) this.DataContext).Save();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pagelegal.xaml", UriKind.Relative));
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
          this.BillTabControl = (TabControl) target;
          break;
        case 2:
          this.CrptTabItem = (TabItem) target;
          break;
        case 3:
          this.CheckMarkCheckBox = (CheckBox) target;
          break;
        case 4:
          this.EgaisTabItem = (TabItem) target;
          break;
        case 5:
          this.CheckBoxEgais = (CheckBox) target;
          break;
        case 6:
          this.TimeCheckBox = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
