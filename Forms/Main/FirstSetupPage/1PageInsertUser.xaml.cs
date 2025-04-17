// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FirstSetupPage.PageInsertUser
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Main.FirstSetupPage
{
  public class PageInsertUser : Page, IComponentConnector
  {
    internal TextBox ClientNameTb;
    internal WatermarkPasswordBox PassBox1;
    internal WatermarkPasswordBox PassBox2;
    private bool _contentLoaded;

    public PageInsertUser() => this.InitializeComponent();

    public void UpdateMode(GlobalDictionaries.Mode modeProgram)
    {
      ((PageUserViewModel) this.DataContext).ModeProgram = modeProgram;
    }

    public bool Save()
    {
      PageUserViewModel dataContext = (PageUserViewModel) this.DataContext;
      dataContext.Password1 = this.PassBox1.Password;
      dataContext.Password2 = this.PassBox2.Password;
      return dataContext.Save();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/main/firstsetuppage/pageinsertuser.xaml", UriKind.Relative));
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
          this.ClientNameTb = (TextBox) target;
          break;
        case 2:
          this.PassBox1 = (WatermarkPasswordBox) target;
          break;
        case 3:
          this.PassBox2 = (WatermarkPasswordBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
