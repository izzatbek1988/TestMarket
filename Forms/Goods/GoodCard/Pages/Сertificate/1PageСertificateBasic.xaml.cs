// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.Pages.Сertificate.PageСertificateBasic
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard.Pages.Сertificate
{
  public class PageСertificateBasic : Page, IComponentConnector
  {
    private bool _contentLoaded;

    public PageСertificateBasic() => this.InitializeComponent();

    public PageСertificateBasic(
      List<EntityProperties.PropertyValue> propertyValues,
      Guid uid)
    {
      this.InitializeComponent();
      this.DataContext = (object) new CertificateBasicViewModel(uid, propertyValues);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/goodcard/pages/%d0%a1ertificate/page%d0%a1ertificatebasic.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
