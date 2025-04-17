// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.EditGoodDiscountViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class EditGoodDiscountViewModel : ViewModelWithForm
  {
    private Decimal _discount;
    private Decimal _priceDiscount;

    public Visibility VisibilityAllRow { get; set; }

    public (bool result, Decimal discount) SaveResult { get; set; }

    public string Name { get; set; }

    public Decimal SumToSet { get; set; }

    public Decimal Price { get; set; }

    public Decimal Sum
    {
      get
      {
        return this.SumToSet - Math.Round(this.SumToSet * this.Discount / 100M, 2, MidpointRounding.AwayFromZero);
      }
      set
      {
        this.OnPropertyChanged(nameof (Sum));
        this.SumDiscount = this.SumToSet - value;
      }
    }

    public Decimal PriceDiscount
    {
      get
      {
        return this.IsEnable ? this.Price - Math.Round(this.Price * this.Discount / 100M, 2, MidpointRounding.AwayFromZero) : this._priceDiscount;
      }
      set
      {
        this._priceDiscount = value;
        this.OnPropertyChanged(nameof (PriceDiscount));
        this.Discount = 100M - value / this.Price * 100M;
      }
    }

    public Decimal Discount
    {
      get => this._discount;
      set
      {
        if (value > 100M)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.EditGoodDiscountViewModel_Скидка_не_может_быть_больше_100);
        }
        else if (value < 0M)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.EditGoodDiscountViewModel_Скидка_не_может_быть_меньше_0);
        }
        else
        {
          this._discount = value;
          this.OnPropertyChanged("SumDiscount");
          this.OnPropertyChanged("PriceDiscount");
          this.OnPropertyChanged("Sum");
          this.OnPropertyChanged(nameof (Discount));
        }
      }
    }

    public Decimal SumDiscount
    {
      get => Math.Round(this.SumToSet * this.Discount / 100M, 2, MidpointRounding.AwayFromZero);
      set
      {
        this.OnPropertyChanged(nameof (SumDiscount));
        this.Discount = value / this.SumToSet * 100M;
      }
    }

    public bool IsEnable { get; set; } = true;

    public Action CloseFormAction => new Action(((Window) this.FormToSHow).Close);

    public ICommand SaveFormCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    public (bool result, Decimal discount) ShowCard(
      EditGoodDiscountViewModel.DiscountInfo discountInfo,
      bool isVisibilityAllRow = true)
    {
      this.Name = discountInfo.Name;
      this.Price = discountInfo.Price;
      this.SumToSet = discountInfo.SumWithoutDiscount;
      this.Discount = discountInfo.Discount;
      this.IsEnable = !discountInfo.IsManyGoods;
      this.VisibilityAllRow = isVisibilityAllRow ? Visibility.Visible : Visibility.Collapsed;
      this.FormToSHow = (WindowWithSize) new FrmGoodDiscount();
      this.ShowForm();
      return this.SaveResult;
    }

    private void Save()
    {
      this.SaveResult = (true, this.Discount);
      this.CloseFormAction();
    }

    public class DiscountInfo
    {
      public string Name { get; set; }

      public Decimal Price { get; set; }

      public Decimal SumWithoutDiscount { get; set; }

      public Decimal Discount { get; set; }

      public bool IsManyGoods { get; set; }

      public DiscountInfo(IReadOnlyCollection<BasketItem> basketItems)
      {
        BasketItem firstItem = basketItems.Any<BasketItem>() ? basketItems.First<BasketItem>() : throw new ArgumentOutOfRangeException();
        if (basketItems.Count == 1)
        {
          this.Name = firstItem.DisplayedName;
          this.IsManyGoods = false;
          this.Discount = firstItem.Discount.Value;
          this.Price = firstItem.SalePrice;
          this.SumWithoutDiscount = firstItem.TotalSum + firstItem.DiscountSum;
        }
        else
        {
          this.Name = string.Format(Translate.DiscountInfo_Несколько___0___товаров, (object) basketItems.Count);
          this.IsManyGoods = true;
          this.Discount = basketItems.All<BasketItem>((Func<BasketItem, bool>) (x => x.Discount.Value == firstItem.Discount.Value)) ? firstItem.Discount.Value : 0M;
          this.SumWithoutDiscount = basketItems.Sum<BasketItem>((Func<BasketItem, Decimal>) (x => x.Quantity * x.SalePrice));
        }
      }
    }
  }
}
