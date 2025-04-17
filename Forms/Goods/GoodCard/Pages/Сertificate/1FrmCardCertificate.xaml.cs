// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.Pages.Сertificate.FrmCardCertificate
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard.Pages.Сertificate
{
  public class FrmCardCertificate : WindowWithSize, IComponentConnector
  {
    internal IntegerUpDown CountCertificateDown;
    internal TextBox BarcodeTextBox;
    private bool _contentLoaded;

    public FrmCardCertificate() => this.InitializeComponent();

    public List<GoodsCertificate.Certificate> GeneratedCertificate(
      Visibility visibilityBarcode,
      Visibility visibilityCount)
    {
      CardCertificateViewModel certificateViewModel = new CardCertificateViewModel()
      {
        Close = new Action(((Window) this).Close),
        VisibilityCount = visibilityCount,
        VisibilityBarcode = visibilityBarcode
      };
      this.DataContext = (object) certificateViewModel;
      if (visibilityBarcode == Visibility.Visible)
        this.BarcodeTextBox.Focus();
      else
        this.CountCertificateDown.Focus();
      this.ShowDialog();
      return certificateViewModel.Certificates;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/goodcard/pages/%d0%a1ertificate/frmcardcertificate.xaml", UriKind.Relative));
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
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.BarcodeTextBox = (TextBox) target;
        else
          this._contentLoaded = true;
      }
      else
        this.CountCertificateDown = (IntegerUpDown) target;
    }
  }
}
