// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.PageImageGood
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard
{
  public class PageImageGood : Page, IComponentConnector
  {
    internal Image ResizableImage;
    internal ScaleTransform ImageScaleTransform;
    internal TranslateTransform ImageTranslateTransform;
    private bool _contentLoaded;

    public PageImageGood(Guid goodUid, bool isVisibilityFunctionButton = true)
    {
      this.InitializeComponent();
      this.DataContext = (object) new ImageGoodViewModel(goodUid, isVisibilityFunctionButton);
    }

    public PageImageGood()
    {
    }

    private void UIElement_OnDrop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;
      ((ImageGoodViewModel) this.DataContext).CopyImagesToGoodFolder(((IEnumerable<string>) (string[]) e.Data.GetData(DataFormats.FileDrop)).ToList<string>());
    }

    private void Border_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/goodcard/pages/images/pageimagegood.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((UIElement) target).Drop += new DragEventHandler(this.UIElement_OnDrop);
          ((UIElement) target).MouseWheel += new MouseWheelEventHandler(this.Border_OnMouseWheel);
          break;
        case 2:
          this.ResizableImage = (Image) target;
          break;
        case 3:
          this.ImageScaleTransform = (ScaleTransform) target;
          break;
        case 4:
          this.ImageTranslateTransform = (TranslateTransform) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
