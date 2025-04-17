// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Waybill.Waybill
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.ViewModels.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers.Egais;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Core.ViewModels.Waybill
{
  public class Waybill : DocumentViewModel<WaybillItem>
  {
    public bool IsEgaisThis { get; set; }

    public EgaisDocument EgaisDocument { get; set; }

    public bool IsShowBuyPrice { get; set; }

    public Decimal TotalBuySum
    {
      get => this.Items.Sum<WaybillItem>((Func<WaybillItem, Decimal>) (x => x.BuySum));
    }

    public Decimal TotalSaleSum
    {
      get => this.Items.Sum<WaybillItem>((Func<WaybillItem, Decimal>) (x => x.SaleSum));
    }

    public Decimal TotalPercent
    {
      get
      {
        return !(this.TotalBuySum == 0M) ? Math.Round((this.TotalSaleSum - this.TotalBuySum) / this.TotalBuySum * 100M, MidpointRounding.AwayFromZero) : 0M;
      }
    }

    public Decimal TotalPayment
    {
      get
      {
        ObservableCollection<Gbs.Core.Entities.Payments.Payment> payments = this.Payments;
        return payments == null ? 0M : payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut - x.SumIn));
      }
    }

    public Decimal TotalCredit => this.TotalBuySum - this.TotalPayment;

    public ObservableCollection<Gbs.Core.Entities.Payments.Payment> Payments { get; set; } = new ObservableCollection<Gbs.Core.Entities.Payments.Payment>();

    public override ICommand EditQuantityCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.EditQuantity(obj)));
    }

    public bool EditQuantity(object obj, bool isReadOnly = true)
    {
      List<WaybillItem> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return false;
      castedList.ForEach((Action<WaybillItem>) (x => x.IsReadOnly = isReadOnly));
      (bool result, Decimal? quantity1, Decimal? salePrice, Decimal? buyPrice) = new EditGoodQuantityViewModel().ShowQuantityEditForWaybill(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<WaybillItem>) castedList), this.IsShowBuyPrice);
      if (!result)
        return false;
      foreach (WaybillItem waybillItem1 in castedList)
      {
        WaybillItem item = waybillItem1;
        EgaisDocument egaisDocument = this.EgaisDocument;
        PositionType positionType = egaisDocument != null ? egaisDocument.Waybill.Items.SingleOrDefault<PositionType>((Func<PositionType, bool>) (x => x.UidGoodInDb == item.Good.Uid && item.Identity == x.Identity)) : (PositionType) null;
        Decimal? nullable;
        if (this.IsEgaisThis && positionType != null)
        {
          Decimal quantity2 = positionType.Quantity;
          nullable = quantity1;
          Decimal valueOrDefault = nullable.GetValueOrDefault();
          if (quantity2 < valueOrDefault & nullable.HasValue)
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.Waybill_EditQuantity_Указать_количество_больше__чем_есть_в_накладной_ЕГАИС_невозможно__Для_позиции__0__установлено_максимальное_количество_по_накладной, (object) item.DisplayedName)));
            item.Quantity = positionType.Quantity;
            goto label_10;
          }
        }
        WaybillItem waybillItem2 = item;
        nullable = quantity1;
        Decimal num1 = nullable ?? item.Quantity;
        waybillItem2.Quantity = num1;
label_10:
        WaybillItem waybillItem3 = item;
        nullable = buyPrice;
        Decimal num2 = nullable ?? item.BuyPrice;
        waybillItem3.BuyPrice = num2;
        WaybillItem waybillItem4 = item;
        nullable = salePrice;
        Decimal num3 = nullable ?? item.SalePrice;
        waybillItem4.SalePrice = num3;
      }
      this.OnPropertyChanged("Items");
      this.ReCalcTotals();
      return true;
    }

    public override ActionResult Save() => throw new NotImplementedException();

    public override void ReCalcTotals()
    {
      this.OnPropertyChanged("TotalQuantity");
      this.OnPropertyChanged("TotalSaleSum");
      this.OnPropertyChanged("TotalBuySum");
      this.OnPropertyChanged("TotalPercent");
      this.OnPropertyChanged("TotalPayment");
      this.OnPropertyChanged("TotalCredit");
    }

    public ActionResult AddGood(WaybillItem item)
    {
      this.Items.Add(item);
      this.SelectedItem = item;
      this.ReCalcTotals();
      return new ActionResult(ActionResult.Results.Ok);
    }
  }
}
