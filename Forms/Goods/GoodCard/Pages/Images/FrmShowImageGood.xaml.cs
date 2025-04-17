// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.ShowImageGoodViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Windows.Controls;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard
{
  public partial class ShowImageGoodViewModel : ViewModelWithForm
  {
    public Page PageImage { get; set; }

    public void ShowImage(Guid goodUid)
    {
      this.PageImage = (Page) new PageImageGood(goodUid, false);
      this.FormToSHow = (WindowWithSize) new FrmShowImageGood();
      this.ShowForm();
    }
  }
}
