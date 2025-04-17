// Decompiled with JetBrains decompiler
// Type: BeerTapCardModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Egais;
using Gbs.Forms.Settings.Egais;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Windows.Input;

#nullable disable
public partial class BeerTapCardModelView : ViewModelWithForm
{
  private bool _isResult;

  public TapBeer Tap { get; set; }

  public bool Show(Guid uidTap, out TapBeer tapBeer)
  {
    tapBeer = uidTap == Guid.Empty ? new TapBeer() : new TapBeerRepository().GetByUid(uidTap);
    this.Tap = tapBeer;
    this.FormToSHow = (WindowWithSize) new FrmBeerTapCard();
    this.ShowForm();
    return this._isResult;
  }

  public ICommand SaveCommand
  {
    get
    {
      return (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this._isResult = new TapBeerRepository().Save(this.Tap);
        this.CloseAction();
      }));
    }
  }
}
