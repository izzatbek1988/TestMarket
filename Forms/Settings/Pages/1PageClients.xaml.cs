// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageClients
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
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class PageClients : Page, IComponentConnector
  {
    private bool _contentLoaded;

    public PageClients() => this.InitializeComponent();

    public PageClients(Gbs.Core.Config.Settings settings, Integrations integrations)
    {
      this.InitializeComponent();
      this.DataContext = (object) new ClientsPageViewModel(settings, integrations);
    }

    public bool Save() => ((ClientsPageViewModel) this.DataContext).Save();

    private void PageClients_OnMouseEnter(object sender, MouseEventArgs e)
    {
      ((ClientsPageViewModel) this.DataContext).OnReLoad();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pageclients.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((UIElement) target).MouseEnter += new MouseEventHandler(this.PageClients_OnMouseEnter);
      else
        this._contentLoaded = true;
    }
  }
}
