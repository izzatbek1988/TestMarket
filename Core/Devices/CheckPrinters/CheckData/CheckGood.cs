// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckData.CheckGood
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Models.GoodInList;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckData
{
  public class CheckGood
  {
    private Decimal _quantity;
    private Decimal _discount;
    private Decimal _price;
    private static List<GoodsUnits.GoodUnit> units = CachesBox.AllGoodsUnits();

    public Guid Uid { get; set; } = Guid.NewGuid();

    public Guid GoodUid
    {
      get
      {
        Gbs.Core.Entities.Goods.Good good = this.Good;
        return good == null ? Guid.Empty : __nonvirtual (good.Uid);
      }
    }

    public GoodsListItemsCertificate CertificateInfo { get; set; } = new GoodsListItemsCertificate();

    [JsonIgnore]
    public Gbs.Core.Entities.Goods.Good Good { get; }

    public string Name { get; }

    public string Barcode { get; }

    public Decimal Price
    {
      get => Math.Round(this._price, 2, MidpointRounding.AwayFromZero);
      set => this._price = value;
    }

    public Decimal Discount
    {
      get => Math.Round(this._discount, 2, MidpointRounding.AwayFromZero);
      set => this._discount = value;
    }

    public Decimal DiscountSum
    {
      get
      {
        return Math.Round(this._discount / 100M * this._price * this._quantity, 2, MidpointRounding.AwayFromZero);
      }
    }

    public Decimal Sum
    {
      get
      {
        return Math.Round(this._price * this._quantity, 2, MidpointRounding.AwayFromZero) - this.DiscountSum;
      }
    }

    public Decimal Quantity
    {
      get => Math.Round(this._quantity, 3, MidpointRounding.AwayFromZero);
      set => this._quantity = value;
    }

    public int KkmSectionNumber { get; }

    public int TaxRateNumber { get; set; }

    public string Description { get; set; }

    public List<string> CommentForFiscalCheck { get; set; } = new List<string>();

    public MarkedInfo MarkedInfo { get; set; }

    public GoodsUnits.GoodUnit Unit { get; set; }

    public GlobalDictionaries.RuFfdGoodsTypes RuFfdGoodTypeCode { get; set; }

    public GlobalDictionaries.RuFfdPaymentModes RuFfdPaymentModeCode { get; set; }

    public CheckGood()
    {
    }

    public CheckGood(
      Gbs.Core.Entities.Goods.Good good,
      Decimal price,
      Decimal discount,
      Decimal quantity,
      string description,
      string displayName)
    {
      Performancer performancer = new Performancer("Конструктор товара в чек", false);
      this.Good = good;
      this.Discount = discount;
      this.Name = displayName.IsNullOrEmpty() ? good.Name : displayName;
      this.Barcode = good.Barcode;
      this.Price = price;
      this._quantity = quantity;
      performancer.AddPoint("10");
      this.Unit = CachesBox.AllGoodsUnits().SingleOrDefault<GoodsUnits.GoodUnit>((Func<GoodsUnits.GoodUnit, bool>) (u => u.Uid == (good?.Group?.UnitsUid ?? Guid.Empty))) ?? new GoodsUnits.GoodUnit();
      performancer.AddPoint("20");
      this.KkmSectionNumber = good.Group.KkmSectionNumber;
      int taxRateId = good.Group.TaxRateNumber;
      Dictionary<int, FiscalKkm.TaxRate> taxRates = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.TaxRates;
      if (taxRateId == 0)
      {
        int defaultTaxRateId = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.DefaultTaxRate;
        taxRateId = taxRates.FirstOrDefault<KeyValuePair<int, FiscalKkm.TaxRate>>((Func<KeyValuePair<int, FiscalKkm.TaxRate>, bool>) (x => x.Key == defaultTaxRateId)).Key;
      }
      performancer.AddPoint("30");
      int num = 0;
      List<KeyValuePair<int, FiscalKkm.TaxRate>> list = taxRates.Where<KeyValuePair<int, FiscalKkm.TaxRate>>((Func<KeyValuePair<int, FiscalKkm.TaxRate>, bool>) (x => x.Key == taxRateId)).ToList<KeyValuePair<int, FiscalKkm.TaxRate>>();
      if (list.Count == 1)
        num = list.Single<KeyValuePair<int, FiscalKkm.TaxRate>>().Value.KkmIndex;
      this.TaxRateNumber = num;
      this.Description = description;
      this.RuFfdGoodTypeCode = good.Group.RuFfdGoodsType;
      performancer.AddPoint("40");
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion >= GlobalDictionaries.Devices.FfdVersions.Ffd110 && this.RuFfdGoodTypeCode == GlobalDictionaries.RuFfdGoodsTypes.None && new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Russia)
        this.RuFfdGoodTypeCode = GlobalDictionaries.RuFfdGoodsTypes.SimpleGood;
      this.RuFfdPaymentModeCode = GlobalDictionaries.RuFfdPaymentModes.FullPayment;
      performancer.Stop();
    }
  }
}
