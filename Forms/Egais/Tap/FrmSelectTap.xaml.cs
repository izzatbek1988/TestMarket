// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.Tap.SelectTapViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Egais.Tap
{
  public partial class SelectTapViewModel : ViewModelWithForm
  {
    private bool _result;

    public string Name { get; set; }

    public List<InfoToTapBeer> Items { get; set; }

    public InfoToTapBeer SelectedTap { get; set; }

    public (InfoToTapBeer, bool) GetTapForGood(Guid uid)
    {
      List<InfoToTapBeer> list = new InfoTapBeerRepository().GetByGoodUid(uid).Where<InfoToTapBeer>((Func<InfoToTapBeer, bool>) (x =>
      {
        Decimal saleQuantity = x.SaleQuantity;
        Decimal? quantity = x.Quantity;
        Decimal valueOrDefault = quantity.GetValueOrDefault();
        return saleQuantity < valueOrDefault & quantity.HasValue;
      })).ToList<InfoToTapBeer>();
      if (!list.Any<InfoToTapBeer>())
        return ((InfoToTapBeer) null, true);
      if (list.Count<InfoToTapBeer>() == 1)
        return (list.Single<InfoToTapBeer>(), true);
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.Name = new GoodRepository(dataBase).GetByUid(uid).Name;
        this.Items = new List<InfoToTapBeer>((IEnumerable<InfoToTapBeer>) list);
        this.FormToSHow = (WindowWithSize) new FrmSelectTap();
        this.ShowForm();
        return (this.SelectedTap, this._result);
      }
    }

    public ICommand SelectTap
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedTap == null)
          {
            MessageBoxHelper.Warning("Необходимо выбрать кран, который будет использоваться для данной алкогольной продукции.");
          }
          else
          {
            this._result = true;
            this.CloseAction();
          }
        }));
      }
    }

    public ICommand CancelSelectTap
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SelectedTap = (InfoToTapBeer) null;
          this.CloseAction();
        }));
      }
    }
  }
}
