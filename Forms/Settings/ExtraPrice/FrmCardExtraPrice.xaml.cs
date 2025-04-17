// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.ExtraPrice.ExtraPriceViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.ExtraPrice
{
  public partial class ExtraPriceViewModel : ViewModelWithForm
  {
    public GoodsExtraPrice.GoodExtraPrice Price { get; set; }

    public bool SaveResult { get; set; }

    public ICommand SaveCommand { get; set; }

    public Action Close { get; set; }

    public ExtraPriceViewModel()
    {
      this.SaveCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    private void Save()
    {
      this.SaveResult = this.Price.Save();
      if (!this.SaveResult)
        return;
      this.Close();
    }
  }
}
