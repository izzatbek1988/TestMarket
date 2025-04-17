// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Documents.Sales.SaleDiscount
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;

#nullable disable
namespace Gbs.Core.ViewModels.Documents.Sales
{
  public class SaleDiscount : ViewModel
  {
    private Decimal _maxValue = 100M;

    public Decimal Value { get; private set; }

    public Decimal MaxValue
    {
      get => this._maxValue;
      set
      {
        this._maxValue = value;
        if (this.Value > this._maxValue)
          this.Value = this._maxValue;
        this.OnPropertyChanged(isUpdateAllProp: true);
      }
    }

    public SaleDiscount.ReasonEnum Reason { get; private set; }

    private Decimal UserValue { get; set; }

    private Decimal ValueByRules { get; set; }

    public void SetDiscount(
      Decimal value,
      SaleDiscount.ReasonEnum reason,
      BasketItem item,
      Gbs.Core.ViewModels.Basket.Basket basket)
    {
      switch (reason)
      {
        case SaleDiscount.ReasonEnum.UserEdit:
          this.UserValue = value;
          break;
        case SaleDiscount.ReasonEnum.Rules:
          this.ValueByRules = value;
          break;
      }
      if (this.Reason == SaleDiscount.ReasonEnum.UserEdit && reason != SaleDiscount.ReasonEnum.UserEdit)
      {
        value = this.UserValue;
        reason = SaleDiscount.ReasonEnum.UserEdit;
      }
      if (value < this.UserValue)
      {
        value = this.UserValue;
        reason = SaleDiscount.ReasonEnum.UserEdit;
      }
      if (value < this.ValueByRules && reason != SaleDiscount.ReasonEnum.UserEdit)
      {
        value = this.ValueByRules;
        reason = SaleDiscount.ReasonEnum.Rules;
      }
      if (value > this.MaxValue)
      {
        value = this.MaxValue;
        if (item != null && basket != null)
        {
          item.ErrorStrForDisplayedName = Translate.SaleDiscount_SetDiscount_Установлена_максимальная_величина_скидка_в_соответствии_с_настройками_программы__Проверьте_итоговую_цену_товара_для_корректной_продажи;
          basket.SetItemNameColor(item, Gbs.Core.ViewModels.Basket.Basket.CustomColors.Orange);
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.SaleDiscount_SetDiscount_Для_товара__0__скидка_изменена_согласно_настройкам_программы_до_значения__1__, (object) item.DisplayedName, (object) this.MaxValue)));
        }
      }
      this.Reason = reason;
      this.Value = value;
      this.OnPropertyChanged(nameof (SetDiscount));
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    public void CLearRuleValue()
    {
      this.ValueByRules = 0M;
      if (this.Reason != SaleDiscount.ReasonEnum.Rules)
        return;
      this.Reason = SaleDiscount.ReasonEnum.None;
      this.SetDiscount(0M, SaleDiscount.ReasonEnum.None, (BasketItem) null, (Gbs.Core.ViewModels.Basket.Basket) null);
    }

    public enum ReasonEnum
    {
      None,
      UserEdit,
      Rules,
    }
  }
}
