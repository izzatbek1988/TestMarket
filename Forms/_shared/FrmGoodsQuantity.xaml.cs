// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.EditGoodQuantityViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.Scales;
using Gbs.Core.Entities;
using Gbs.Core.ViewModels.Basket;
using Gbs.Core.ViewModels.Documents;
using Gbs.Core.ViewModels.Inventory;
using Gbs.Core.ViewModels.Waybill;
using Gbs.Forms.Excel;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class EditGoodQuantityViewModel : ViewModelWithForm
  {
    private string _textQuantity = Translate.FrmGoodsQuantity_Количество;
    private readonly Gbs.Core.Config.Devices _deviceConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
    private Decimal? _buyPrice;
    private Decimal? _quantity;
    private Decimal? _percent;
    private Decimal? _salePrice;
    private readonly int _countRound = new ConfigsRepository<Settings>().Get().Waybill.IsMoreDecimalPlaceBuyPrice ? 4 : 2;
    private ScalesHelper _scale;

    public string TextQuantity
    {
      get => this._textQuantity;
      set
      {
        this._textQuantity = value;
        this.OnPropertyChanged(nameof (TextQuantity));
      }
    }

    public Visibility VisibilityPrice { get; set; }

    public Visibility VisibilitySumPrice { get; set; } = Visibility.Collapsed;

    public bool IsEnableCount { get; set; } = true;

    public Visibility VisibilityBtnTara
    {
      get
      {
        return !this._deviceConfig.Scale.IsShowBtnTara || this._deviceConfig.Scale.Type == GlobalDictionaries.Devices.ScaleTypes.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public string TextTaraBtn
    {
      get
      {
        return !this.IsTaraReset || this._deviceConfig.Scale.Type == GlobalDictionaries.Devices.ScaleTypes.Pos2Mertech ? Translate.EditGoodQuantityViewModel_TextTaraBtn_Тара : Translate.EditGoodQuantityViewModel_TextTaraBtn_Сброс_тары;
      }
    }

    public Decimal TaraQuantity { get; set; }

    public Visibility VisibilityTaraQuantity
    {
      get
      {
        return !this._deviceConfig.Scale.IsShowBtnTara || this._deviceConfig.Scale.Type == GlobalDictionaries.Devices.ScaleTypes.None || this._deviceConfig.Scale.Type == GlobalDictionaries.Devices.ScaleTypes.Pos2Mertech ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public bool IsTaraReset { get; set; }

    public ICommand TaraCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.IsTaraReset)
          {
            this._scale?.TaraReset();
            this.IsTaraReset = false;
            this.TaraQuantity = 0M;
          }
          else
          {
            Decimal? quantity = this.Quantity;
            if (!quantity.HasValue)
            {
              int num = (int) MessageBoxHelper.Show(Translate.EditGoodQuantityViewModel_TaraCommand_Не_указано_количество_);
              return;
            }
            this._scale?.Tara();
            quantity = this.Quantity;
            this.TaraQuantity = quantity.GetValueOrDefault();
            this.IsTaraReset = true;
          }
          this.OnPropertyChanged("TextTaraBtn");
          this.OnPropertyChanged("TaraQuantity");
        }));
      }
    }

    public ICommand ZeroCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this._scale?.Zero()));
    }

    public ICommand PricingCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Rule == null)
            return;
          List<PricingRules.ItemPricing> list = this.Rule.Items.Where<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, bool>) (x =>
          {
            Decimal minSum = x.MinSum;
            Decimal? buyPrice = this.BuyPrice;
            Decimal valueOrDefault = buyPrice.GetValueOrDefault();
            return minSum <= valueOrDefault & buyPrice.HasValue;
          })).ToList<PricingRules.ItemPricing>();
          Decimal max = list.Any<PricingRules.ItemPricing>() ? list.Max<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, Decimal>) (m => m.MinSum)) : 0M;
          PricingRules.ItemPricing itemPricing = list.FirstOrDefault<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, bool>) (x => x.MinSum == max));
          if (itemPricing == null)
            return;
          Decimal? buyPrice1 = this.BuyPrice;
          this._salePrice = new Decimal?(HelpClassExcel.RoundSum(buyPrice1.GetValueOrDefault() * (1M + (itemPricing != null ? itemPricing.Margin : 0M) / 100M), itemPricing != null ? itemPricing.RoundingValue : 0M));
          this.OnPropertyChanged("SalePrice");
          Decimal valueOrDefault1 = this._salePrice.GetValueOrDefault();
          buyPrice1 = this.BuyPrice;
          Decimal valueOrDefault2 = buyPrice1.GetValueOrDefault();
          Decimal num = (valueOrDefault1 - valueOrDefault2) * 100M;
          buyPrice1 = this.BuyPrice;
          Decimal valueOrDefault3 = buyPrice1.GetValueOrDefault();
          this._percent = new Decimal?(Math.Round(num / valueOrDefault3, 2, MidpointRounding.AwayFromZero));
          this.OnPropertyChanged("Percent");
        }));
      }
    }

    public bool IsReadOnlySalePrice { get; set; }

    public Visibility VisibilityBuyPrice { get; set; }

    public Visibility VisibilityExtraPercent
    {
      get
      {
        return this.VisibilityBuyPrice != Visibility.Visible || !new ConfigsRepository<Settings>().Get().Interface.IsVisibilityExtraPercent ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public bool IsReadOnlyBuySum
    {
      get
      {
        if (!this.Quantity.HasValue)
          return true;
        Decimal? quantity = this.Quantity;
        Decimal num = 0M;
        return quantity.GetValueOrDefault() == num & quantity.HasValue;
      }
    }

    public string Name { get; set; }

    public Decimal BuySum
    {
      get
      {
        Decimal? nullable = this.BuyPrice;
        Decimal valueOrDefault1 = nullable.GetValueOrDefault();
        nullable = this.Quantity;
        Decimal valueOrDefault2 = nullable.GetValueOrDefault();
        return valueOrDefault1 * valueOrDefault2;
      }
      set
      {
        Decimal? quantity1 = this.Quantity;
        Decimal num1 = 0M;
        if (quantity1.GetValueOrDefault() == num1 & quantity1.HasValue || !this.Quantity.HasValue)
          return;
        Decimal num2 = value;
        Decimal? quantity2 = this.Quantity;
        this.BuyPrice = new Decimal?(Math.Round((quantity2.HasValue ? new Decimal?(num2 / quantity2.GetValueOrDefault()) : new Decimal?()).GetValueOrDefault(), this._countRound, MidpointRounding.AwayFromZero));
        this.OnPropertyChanged("BuyPrice");
        this.OnPropertyChanged("SalePrice");
        this.OnPropertyChanged(nameof (BuySum));
      }
    }

    public Decimal? SaleSum
    {
      get
      {
        Decimal? nullable = this.SalePrice;
        Decimal valueOrDefault1 = nullable.GetValueOrDefault();
        nullable = this.Quantity;
        Decimal valueOrDefault2 = nullable.GetValueOrDefault();
        return new Decimal?(valueOrDefault1 * valueOrDefault2);
      }
      set
      {
        Decimal? salePrice1 = this.SalePrice;
        Decimal num = 0M;
        if (salePrice1.GetValueOrDefault() == num & salePrice1.HasValue || !this.SalePrice.HasValue)
          return;
        Decimal? nullable = value;
        Decimal? salePrice2 = this.SalePrice;
        this.Quantity = new Decimal?(Math.Round((nullable.HasValue & salePrice2.HasValue ? new Decimal?(nullable.GetValueOrDefault() / salePrice2.GetValueOrDefault()) : new Decimal?()).GetValueOrDefault(), this._countRound, MidpointRounding.AwayFromZero));
        this.OnPropertyChanged("SalePrice");
        this.OnPropertyChanged(nameof (SaleSum));
      }
    }

    public Decimal? Percent
    {
      get => this._percent;
      set
      {
        this._percent = value;
        this.SalePrice = new Decimal?(Math.Round(this.BuyPrice.GetValueOrDefault() * (1M + value.GetValueOrDefault() / 100M), 2, MidpointRounding.AwayFromZero));
        this.OnPropertyChanged("SalePrice");
        this.OnPropertyChanged(nameof (Percent));
      }
    }

    public Decimal? Quantity
    {
      get => this._quantity;
      set
      {
        Decimal? nullable = value;
        Decimal num = (Decimal) 10000000;
        this._quantity = !(nullable.GetValueOrDefault() > num & nullable.HasValue) ? value : new Decimal?((Decimal) 10000000);
        this.OnPropertyChanged(nameof (Quantity));
        this.OnPropertyChanged("BuySum");
        this.OnPropertyChanged("SaleSum");
        this.OnPropertyChanged("IsReadOnlyBuySum");
      }
    }

    public ICommand OldSalePriceCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SalePrice = this.OldSalePrice;
          this.OnPropertyChanged("SalePrice");
        }));
      }
    }

    public Decimal? OldSalePrice { get; set; }

    public string OldSaleStringValue
    {
      get
      {
        Decimal? oldSalePrice = this.OldSalePrice;
        ref Decimal? local = ref oldSalePrice;
        return (local.HasValue ? local.GetValueOrDefault().ToString("N2") : (string) null) ?? "null";
      }
    }

    public Visibility VisibilityOldSalePrice { get; set; } = Visibility.Collapsed;

    public Decimal? SalePrice
    {
      get
      {
        if (this.Rule != null && !this.IsReadOnly)
        {
          List<PricingRules.ItemPricing> list = this.Rule.Items.Where<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, bool>) (x =>
          {
            Decimal minSum = x.MinSum;
            Decimal? buyPrice = this.BuyPrice;
            Decimal valueOrDefault = buyPrice.GetValueOrDefault();
            return minSum <= valueOrDefault & buyPrice.HasValue;
          })).ToList<PricingRules.ItemPricing>();
          Decimal max = list.Any<PricingRules.ItemPricing>() ? list.Max<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, Decimal>) (m => m.MinSum)) : 0M;
          PricingRules.ItemPricing itemPricing = list.FirstOrDefault<PricingRules.ItemPricing>((Func<PricingRules.ItemPricing, bool>) (x => x.MinSum == max));
          this._salePrice = new Decimal?(HelpClassExcel.RoundSum(this.BuyPrice.GetValueOrDefault() * (1M + (itemPricing != null ? itemPricing.Margin : 0M) / 100M), itemPricing != null ? itemPricing.RoundingValue : 0M));
          Decimal? buyPrice1 = this.BuyPrice;
          Decimal num1 = 0M;
          if (buyPrice1.GetValueOrDefault() > num1 & buyPrice1.HasValue)
          {
            Decimal valueOrDefault1 = this._salePrice.GetValueOrDefault();
            buyPrice1 = this.BuyPrice;
            Decimal valueOrDefault2 = buyPrice1.GetValueOrDefault();
            Decimal num2 = (valueOrDefault1 - valueOrDefault2) * 100M;
            buyPrice1 = this.BuyPrice;
            Decimal valueOrDefault3 = buyPrice1.GetValueOrDefault();
            this._percent = new Decimal?(Math.Round(num2 / valueOrDefault3, 2, MidpointRounding.AwayFromZero));
            this.OnPropertyChanged("Percent");
          }
        }
        return this._salePrice;
      }
      set
      {
        this._salePrice = value;
        this.IsReadOnly = true;
        if (this.BuyPrice.HasValue)
        {
          Decimal? buyPrice = this.BuyPrice;
          Decimal num1 = 0M;
          if (!(buyPrice.GetValueOrDefault() == num1 & buyPrice.HasValue))
          {
            Decimal valueOrDefault1 = value.GetValueOrDefault();
            buyPrice = this.BuyPrice;
            Decimal valueOrDefault2 = buyPrice.GetValueOrDefault();
            Decimal num2 = (valueOrDefault1 - valueOrDefault2) * 100M;
            buyPrice = this.BuyPrice;
            Decimal valueOrDefault3 = buyPrice.GetValueOrDefault();
            this._percent = new Decimal?(Math.Round(num2 / valueOrDefault3, 2, MidpointRounding.AwayFromZero));
            this.OnPropertyChanged("Percent");
          }
        }
        this.OnPropertyChanged(nameof (SalePrice));
        this.OnPropertyChanged("SaleSum");
      }
    }

    public Decimal? BuyPrice
    {
      get => this._buyPrice;
      set
      {
        this._buyPrice = value;
        this.OnPropertyChanged(nameof (BuyPrice));
        this.OnPropertyChanged("BuySum");
        this.OnPropertyChanged("SalePrice");
        if (this.BuyPrice.HasValue)
        {
          Decimal? buyPrice = this.BuyPrice;
          Decimal num = 0M;
          if (!(buyPrice.GetValueOrDefault() == num & buyPrice.HasValue))
          {
            this._percent = new Decimal?(Math.Round((this.SalePrice.GetValueOrDefault() - value.GetValueOrDefault()) * 100M / value.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero));
            this.OnPropertyChanged("Percent");
          }
        }
        this.OnPropertyChanged(nameof (BuyPrice));
      }
    }

    public int? DecimalPlace { get; set; }

    public ICommand PlusCommand { get; set; }

    public ICommand MinusCommand { get; set; }

    public ICommand SaveQuantityCommand { get; set; }

    private bool IsReadOnly { get; set; } = true;

    private bool SaveResult { get; set; }

    private PricingRules Rule { get; set; }

    private List<IDocumentItemViewModel> GoodsForEdit { get; set; }

    public EditGoodQuantityViewModel()
    {
      this.SaveQuantityCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      this.PlusCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.PlusQty()));
      this.MinusCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.MinusQty()));
    }

    private void MinusQty()
    {
      this.StopScale();
      Decimal? quantity1 = this.Quantity;
      Decimal num1 = (Decimal) 1;
      Decimal? nullable;
      if (!(quantity1.GetValueOrDefault() > num1 & quantity1.HasValue))
      {
        nullable = new Decimal?(0M);
      }
      else
      {
        Decimal? quantity2 = this.Quantity;
        Decimal num2 = (Decimal) 1;
        nullable = quantity2.HasValue ? new Decimal?(quantity2.GetValueOrDefault() - num2) : new Decimal?();
      }
      this.Quantity = nullable;
    }

    private void PlusQty()
    {
      this.StopScale();
      Decimal? quantity = this.Quantity;
      Decimal num = (Decimal) 1;
      this.Quantity = quantity.HasValue ? new Decimal?(quantity.GetValueOrDefault() + num) : new Decimal?();
    }

    private void GetW(Decimal w)
    {
      LogHelper.Debug("scale event; w: " + w.ToString());
      if (w == 0M)
        LogHelper.Debug("С весов получено значение = 0,  не меняем вес.");
      else
        this.Quantity = new Decimal?(w);
      TaskHelper.TaskRun((Action) (() =>
      {
        Thread.Sleep(50);
        Application.Current.Dispatcher.Invoke((Action) (() => ((FrmGoodsQuantity) this.FormToSHow).BoxQuantity.Box.SelectAll()));
      }), false);
    }

    public void StopScale()
    {
      if (this._scale == null)
        return;
      this._scale.Notify -= new ScalesHelper.ScaleHandler(this.GetW);
      this._scale.StopListen();
    }

    private void StartGetWeight(bool isOneWightGood)
    {
      LogHelper.OnBegin();
      if (!isOneWightGood || this._deviceConfig.Scale.Type == GlobalDictionaries.Devices.ScaleTypes.None)
        return;
      this._scale = ScalesFactory.Create((IConfig) this._deviceConfig);
      this._scale.StartListen();
      this._scale.Notify += new ScalesHelper.ScaleHandler(this.GetW);
    }

    private void Save()
    {
      if (this.GoodsForEdit == null)
      {
        this.SaveResult = true;
        this.CloseAction();
      }
      else
      {
        foreach (IDocumentItemViewModel documentItemViewModel1 in this.GoodsForEdit)
        {
          Decimal? nullable;
          if (documentItemViewModel1 is WaybillItem waybillItem1)
          {
            WaybillItem waybillItem = waybillItem1;
            Decimal num;
            if (this.VisibilityBuyPrice == Visibility.Visible)
            {
              nullable = this.BuyPrice;
              if (nullable.HasValue)
              {
                nullable = this.BuyPrice;
                num = nullable.GetValueOrDefault();
                goto label_9;
              }
            }
            num = waybillItem1.BuyPrice;
label_9:
            waybillItem.BuyPrice = num;
          }
          IDocumentItemViewModel documentItemViewModel2 = documentItemViewModel1;
          nullable = this.Quantity;
          Decimal num1;
          if (!nullable.HasValue)
          {
            num1 = documentItemViewModel1.Quantity;
          }
          else
          {
            nullable = this.Quantity;
            num1 = nullable.GetValueOrDefault();
          }
          documentItemViewModel2.Quantity = num1;
          if (documentItemViewModel1 is BasketItem basketItem1)
          {
            BasketItem basketItem = basketItem1;
            Decimal num2;
            if (this.IsEnableCount)
            {
              nullable = this.SalePrice;
              if (nullable.HasValue)
              {
                nullable = this.SalePrice;
                num2 = nullable.GetValueOrDefault();
                goto label_18;
              }
            }
            num2 = basketItem1.SalePrice;
label_18:
            basketItem.SalePrice = num2;
          }
        }
        this.SaveResult = true;
        this.CloseAction();
      }
    }

    public (bool result, Decimal? quantity, Decimal? salePrice, Decimal? buyPrice) ShowQuantityEditForWaybill(
      EditGoodQuantityViewModel.QuantityRequest q,
      bool isShowBuyPrice)
    {
      this.Name = q.Name;
      this.DecimalPlace = new ConfigsRepository<Settings>().Get().Sales.IsLimitedDecimalPlace ? new int?(q.DecimalPlace) : new int?();
      this.Quantity = q.Quantity;
      this.IsEnableCount = q.IsEnableCount;
      this.BuyPrice = q.BuyPrice;
      this.SalePrice = q.SalePrice;
      this.VisibilityBuyPrice = isShowBuyPrice ? Visibility.Visible : Visibility.Collapsed;
      this.IsReadOnlySalePrice = false;
      this.Rule = q.Rule;
      this.IsReadOnly = q.IsReadOnly;
      if (new ConfigsRepository<Settings>().Get().Waybill.IsOfferPreviousSalePrice)
      {
        Decimal? oldSalePrice = q.OldSalePrice;
        if (oldSalePrice.HasValue)
        {
          oldSalePrice = q.OldSalePrice;
          Decimal num = 0M;
          if (!(oldSalePrice.GetValueOrDefault() == num & oldSalePrice.HasValue))
          {
            this.OldSalePrice = q.OldSalePrice;
            this.VisibilityOldSalePrice = Visibility.Visible;
          }
        }
      }
      this.StartGetWeight(q.IsOneWightGood);
      this.FormToSHow = (WindowWithSize) new FrmGoodsQuantity(this);
      this.ShowForm();
      this.StopScale();
      return (this.SaveResult, this.Quantity, this.SalePrice, this.BuyPrice);
    }

    public (bool result, Decimal? quantity, Decimal? salePrice) ShowQuantityWithSalePriceEdit(
      EditGoodQuantityViewModel.QuantityRequest info,
      bool isOfferPreviousSalePrice = false)
    {
      this.Name = info.Name;
      this.DecimalPlace = new ConfigsRepository<Settings>().Get().Sales.IsLimitedDecimalPlace ? new int?(info.DecimalPlace) : new int?();
      this.Quantity = info.Quantity;
      this.IsEnableCount = info.IsEnableCount;
      this.VisibilityPrice = info.VisibilityPrice;
      this.VisibilityBuyPrice = Visibility.Collapsed;
      this.VisibilitySumPrice = info.IsVisibilitySaleSumForBasket ? Visibility.Visible : Visibility.Collapsed;
      this.IsReadOnlySalePrice = !info.ShowSalePrice;
      this.StartGetWeight(info.IsOneWightGood);
      this.SalePrice = info.SalePrice;
      Decimal? nullable;
      if (isOfferPreviousSalePrice)
      {
        nullable = info.OldSalePrice;
        if (nullable.HasValue)
        {
          nullable = info.OldSalePrice;
          Decimal num = 0M;
          if (!(nullable.GetValueOrDefault() == num & nullable.HasValue))
          {
            this.OldSalePrice = info.OldSalePrice;
            this.VisibilityOldSalePrice = Visibility.Visible;
          }
        }
      }
      if (info.IsTextQuantityForBeer)
      {
        this.TextQuantity = "Объем, (л)";
        this.OnPropertyChanged("TextQuantity");
      }
      if (!DevelopersHelper.IsUnitTest())
      {
        this.FormToSHow = (WindowWithSize) new FrmGoodsQuantity(this);
        if (!this.IsEnableCount)
          KeyboardLayoutHelper.SetSearchFocus((Control) ((FrmGoodsQuantity) this.FormToSHow).SalePriceDecimalUpDown, (Window) this.FormToSHow);
        this.ShowForm();
      }
      this.StopScale();
      if (!info.ShowSalePrice)
      {
        nullable = new Decimal?();
        this.SalePrice = nullable;
      }
      return (this.SaveResult, this.Quantity, this.SalePrice);
    }

    public (bool result, Decimal? quantity) ShowQuantityEditCard(
      EditGoodQuantityViewModel.QuantityRequest info)
    {
      this.Name = info.Name;
      this.IsEnableCount = info.IsEnableCount;
      this.DecimalPlace = new ConfigsRepository<Settings>().Get().Sales.IsLimitedDecimalPlace ? new int?(info.DecimalPlace) : new int?();
      this.Quantity = info.Quantity;
      this.VisibilityBuyPrice = Visibility.Collapsed;
      this.IsReadOnlySalePrice = !info.ShowSalePrice;
      this.StartGetWeight(info.IsOneWightGood);
      if (info.VisibilityPrice == Visibility.Visible)
        this.SalePrice = info.SalePrice;
      this.FormToSHow = (WindowWithSize) new FrmGoodsQuantity(this);
      if (!this.IsEnableCount)
        KeyboardLayoutHelper.SetSearchFocus((Control) ((FrmGoodsQuantity) this.FormToSHow).SalePriceDecimalUpDown, (Window) this.FormToSHow);
      this.ShowForm();
      this.StopScale();
      return (this.SaveResult, this.Quantity);
    }

    public class QuantityRequest
    {
      public bool IsVisibilitySaleSumForBasket { get; set; }

      public bool IsTextQuantityForBeer { get; set; }

      public bool IsOneWightGood { get; set; }

      public int DecimalPlace { get; set; }

      public string Name { get; set; }

      public Decimal? Quantity { get; set; }

      public Decimal? SalePrice { get; set; }

      public Decimal? OldSalePrice { get; set; }

      public Decimal? BuyPrice { get; set; }

      public PricingRules Rule { get; set; }

      public bool IsReadOnly { get; set; } = true;

      public bool IsEnableCount { get; set; } = true;

      public bool ShowSalePrice { get; set; }

      public Visibility VisibilityPrice { get; set; }

      public QuantityRequest(IReadOnlyCollection<WaybillItem> waybillItems)
      {
        WaybillItem firstItem = waybillItems.Any<WaybillItem>() ? waybillItems.First<WaybillItem>() : throw new ArgumentOutOfRangeException();
        if (waybillItems.Count == 1)
        {
          this.Name = firstItem.DisplayedName;
          this.Quantity = new Decimal?(firstItem.Quantity);
          this.BuyPrice = new Decimal?(firstItem.BuyPrice);
          this.SalePrice = new Decimal?(firstItem.SalePrice);
          this.OldSalePrice = firstItem.Good.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? new Decimal?(firstItem.Good.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price))) : new Decimal?();
          this.IsReadOnly = firstItem.IsReadOnly;
          this.DecimalPlace = firstItem.Good.Group.DecimalPlace;
          this.Rule = new PricingRulesRepository().GetActiveItems().ToList<PricingRules>().FirstOrDefault<PricingRules>((Func<PricingRules, bool>) (x => x.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (g => g.Uid == firstItem.Good.Group.Uid))));
        }
        else
        {
          this.Name = string.Format(Translate.DiscountInfo_Несколько___0___товаров, (object) waybillItems.Count);
          this.Quantity = waybillItems.All<WaybillItem>((Func<WaybillItem, bool>) (x => x.Quantity == firstItem.Quantity)) ? new Decimal?(firstItem.Quantity) : new Decimal?();
          this.SalePrice = waybillItems.All<WaybillItem>((Func<WaybillItem, bool>) (x => x.SalePrice == firstItem.SalePrice)) ? new Decimal?(firstItem.SalePrice) : new Decimal?();
          this.DecimalPlace = waybillItems.Min<WaybillItem>((Func<WaybillItem, int>) (x => x.Good.Group.DecimalPlace));
          this.BuyPrice = waybillItems.All<WaybillItem>((Func<WaybillItem, bool>) (x => x.BuyPrice == firstItem.BuyPrice)) ? new Decimal?(firstItem.BuyPrice) : new Decimal?();
        }
        this.IsOneWightGood = waybillItems.Count == 1 & waybillItems.All<WaybillItem>((Func<WaybillItem, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight));
      }

      public QuantityRequest(IReadOnlyCollection<DocumentItemViewModel> item)
      {
        this.LoadForDocumentItem(item);
      }

      public QuantityRequest(IReadOnlyCollection<InventoryItem> item, bool canChangeSalePrice)
      {
        this.LoadForDocumentItem((IReadOnlyCollection<DocumentItemViewModel>) item);
        bool flag = item.All<InventoryItem>((Func<InventoryItem, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service || x.Good.Group.IsFreePrice));
        InventoryItem firstItem = item.First<InventoryItem>();
        if (item.Count > 1)
          this.VisibilityPrice = Visibility.Collapsed;
        this.SalePrice = new Decimal?(item.All<InventoryItem>((Func<InventoryItem, bool>) (x => x.SalePrice == firstItem.SalePrice)) ? firstItem.SalePrice : 0M);
        if (!(canChangeSalePrice & flag))
          return;
        this.ShowSalePrice = true;
      }

      public QuantityRequest(IReadOnlyCollection<BasketItem> item, bool canChangeSalePrice)
      {
        this.LoadForDocumentItem((IReadOnlyCollection<DocumentItemViewModel>) item);
        bool flag = item.All<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service || x.Good.Group.IsFreePrice));
        BasketItem firstItem = item.First<BasketItem>();
        if (item.Count > 1)
          this.VisibilityPrice = Visibility.Collapsed;
        this.SalePrice = new Decimal?(item.All<BasketItem>((Func<BasketItem, bool>) (x => x.SalePrice == firstItem.SalePrice)) ? firstItem.SalePrice : 0M);
        if (canChangeSalePrice & flag)
          this.ShowSalePrice = true;
        if (item.Count != 1)
          return;
        this.OldSalePrice = item.Single<BasketItem>().Good.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? new Decimal?(firstItem.Good.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price))) : new Decimal?();
      }

      private void LoadForDocumentItem(IReadOnlyCollection<DocumentItemViewModel> item)
      {
        DocumentItemViewModel firstItem = item.Any<DocumentItemViewModel>() ? item.First<DocumentItemViewModel>() : throw new ArgumentOutOfRangeException();
        if (item.Count == 1)
        {
          this.Name = firstItem.DisplayedName;
          this.DecimalPlace = firstItem.Good.Group.DecimalPlace;
          this.Quantity = new Decimal?(firstItem.Quantity);
        }
        else
        {
          this.Name = string.Format(Translate.DiscountInfo_Несколько___0___товаров, (object) item.Count);
          this.DecimalPlace = item.Min<DocumentItemViewModel>((Func<DocumentItemViewModel, int>) (x => x.Good.Group.DecimalPlace));
          this.Quantity = !item.All<DocumentItemViewModel>((Func<DocumentItemViewModel, bool>) (x => x.Quantity == firstItem.Quantity)) ? new Decimal?() : new Decimal?(firstItem.Quantity);
        }
        this.IsOneWightGood = item.Count == 1 & item.All<DocumentItemViewModel>((Func<DocumentItemViewModel, bool>) (x => x.Good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight));
      }
    }
  }
}
