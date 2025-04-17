// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmGoodsQuantity
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.UserControls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmGoodsQuantity : WindowWithSize, IComponentConnector
  {
    internal NumberBox BoxQuantity;
    internal DecimalUpDown SalePriceDecimalUpDown;
    private bool _contentLoaded;

    private EditGoodQuantityViewModel Model { get; set; }

    public FrmGoodsQuantity()
    {
      this.InitializeComponent();
      this.SetQtyFocus();
    }

    public FrmGoodsQuantity(EditGoodQuantityViewModel model)
    {
      this.InitializeComponent();
      this.Model = model;
      this.SetQtyFocus();
    }

    public void SetQtyFocus()
    {
      TaskHelper.TaskRun((Action) (() =>
      {
        Thread.Sleep(30);
        Application.Current.Dispatcher.Invoke((Action) (() =>
        {
          this.BoxQuantity.Focus();
          this.BoxQuantity.Box.Focus();
          this.BoxQuantity.Box.SelectAll();
        }));
      }), false);
    }

    private void FrmGoodsQuantity_OnClosed(object sender, EventArgs e) => this.Model.StopScale();

    private void UIElement_OnKeyDown(object sender, KeyEventArgs e) => this.Model.StopScale();

    private void UpDownBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (!Keyboard.IsKeyDown(Key.Back) && !Keyboard.IsKeyDown(Key.Delete) && !Keyboard.IsKeyDown(Key.Up) && !Keyboard.IsKeyDown(Key.Down))
        return;
      this.Model.StopScale();
    }

    private void BoxQuantity_OnPreviewKeyUp(object sender, KeyEventArgs e)
    {
      bool flag1 = e.Key >= Key.D0 && e.Key <= Key.D9 && !e.KeyboardDevice.IsKeyDown(Key.LeftShift) && !e.KeyboardDevice.IsKeyDown(Key.RightShift);
      bool flag2 = e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9;
      if (!(e.Key.IsEither<Key>(Key.Back, Key.Delete, Key.Up, Key.Down) | flag2 | flag1))
        return;
      this.Model.StopScale();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmgoodsquantity.xaml", UriKind.Relative));
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
          this.SalePriceDecimalUpDown = (DecimalUpDown) target;
        else
          this._contentLoaded = true;
      }
      else
        this.BoxQuantity = (NumberBox) target;
    }
  }
}
