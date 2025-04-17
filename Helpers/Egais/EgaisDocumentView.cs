// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.EgaisDocumentView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Egais
{
  public class EgaisDocumentView
  {
    public Dictionary<string, string> DictionaryInformBRegId { get; set; } = new Dictionary<string, string>();

    public int Version { get; set; }

    public List<PositionType> Items { get; set; } = new List<PositionType>();

    public string ShipperName { get; set; }

    public string WBRegId { get; set; }

    public DateTime WBDate { get; set; }

    public string WBNUMBER { get; set; }

    public string Identity { get; set; }

    public string INN { get; set; }

    public string KPP { get; set; }

    public EgaisDocumentView()
    {
    }

    public EgaisDocumentView(Documents document)
    {
      if (document.Document.Item.GetType() == typeof (WayBillType))
      {
        WayBillType wayBillType = (WayBillType) document.Document.Item;
        this.Version = 1;
        this.WBDate = wayBillType.Header.Date;
        this.WBNUMBER = wayBillType.Header.NUMBER;
        this.Identity = wayBillType.Identity;
        this.INN = wayBillType.Header.Shipper.INN;
        this.KPP = wayBillType.Header.Shipper.KPP;
        this.ShipperName = wayBillType.Header.Shipper.FullName;
        this.Items = new List<PositionType>((IEnumerable<PositionType>) wayBillType.Content);
      }
      if (document.Document.Item.GetType() == typeof (WayBillType_v2))
      {
        this.Version = 2;
        WayBillType_v2 wayBillTypeV2 = (WayBillType_v2) document.Document.Item;
        this.WBDate = wayBillTypeV2.Header.Date;
        this.WBNUMBER = wayBillTypeV2.Header.NUMBER;
        this.Identity = wayBillTypeV2.Identity;
        if (wayBillTypeV2.Header.Shipper.Item is ULType ulType)
        {
          this.INN = ulType.INN;
          this.KPP = ulType.KPP;
          this.ShipperName = ulType.FullName;
        }
        if (wayBillTypeV2.Header.Shipper.Item is FLType flType)
        {
          this.INN = flType.INN;
          this.ShipperName = flType.FullName;
        }
        this.Items = new List<PositionType>(((IEnumerable<PositionType4>) wayBillTypeV2.Content).Select<PositionType4, PositionType>((Func<PositionType4, PositionType>) (x => new PositionType()
        {
          Identity = x.Identity,
          Price = x.Price,
          Quantity = x.Quantity,
          Pack_ID = x.Pack_ID,
          Party = x.Party,
          Product = new ProductInfo()
          {
            AlcCode = x.Product.AlcCode,
            FullName = x.Product.FullName,
            ShortName = x.Product.ShortName,
            Capacity = x.Product.Capacity,
            AlcVolumeSpecified = x.alcPercentSpecified,
            AlcVolume = x.alcPercent,
            ProductVCode = x.Product.ProductVCode
          }
        })));
      }
      if (document.Document.Item.GetType() == typeof (WayBillType_v3))
      {
        this.Version = 3;
        WayBillType_v3 wayBillTypeV3 = (WayBillType_v3) document.Document.Item;
        this.WBDate = wayBillTypeV3.Header.Date;
        this.WBNUMBER = wayBillTypeV3.Header.NUMBER;
        this.Identity = wayBillTypeV3.Identity;
        if (wayBillTypeV3.Header.Shipper.Item is ULType ulType)
        {
          this.INN = ulType.INN;
          this.KPP = ulType.KPP;
          this.ShipperName = ulType.FullName;
        }
        if (wayBillTypeV3.Header.Shipper.Item is FLType flType)
        {
          this.INN = flType.INN;
          this.ShipperName = flType.FullName;
        }
        this.Items = new List<PositionType>(((IEnumerable<PositionType6>) wayBillTypeV3.Content).Select<PositionType6, PositionType>((Func<PositionType6, PositionType>) (x => new PositionType()
        {
          Identity = x.Identity,
          Price = x.Price,
          Quantity = x.Quantity,
          Pack_ID = x.Pack_ID,
          Party = x.Party,
          Product = new ProductInfo()
          {
            AlcCode = x.Product.AlcCode,
            FullName = x.Product.FullName,
            ShortName = x.Product.ShortName,
            Capacity = x.Product.Capacity,
            AlcVolumeSpecified = x.alcPercentSpecified,
            AlcVolume = x.alcPercent,
            ProductVCode = x.Product.ProductVCode
          }
        })));
      }
      if (document.Document.Item.GetType() == typeof (WayBillType_v4))
      {
        this.Version = 4;
        WayBillType_v4 wayBillTypeV4 = (WayBillType_v4) document.Document.Item;
        this.WBDate = wayBillTypeV4.Header.Date;
        this.WBNUMBER = wayBillTypeV4.Header.NUMBER;
        this.Identity = wayBillTypeV4.Identity;
        if (wayBillTypeV4.Header.Shipper.Item is ULType ulType)
        {
          this.INN = ulType.INN;
          this.KPP = ulType.KPP;
          this.ShipperName = ulType.FullName;
        }
        if (wayBillTypeV4.Header.Shipper.Item is FLType flType)
        {
          this.INN = flType.INN;
          this.ShipperName = flType.FullName;
        }
        this.Items = new List<PositionType>(((IEnumerable<PositionType15>) wayBillTypeV4.Content).Select<PositionType15, PositionType>((Func<PositionType15, PositionType>) (x => new PositionType()
        {
          Identity = x.Identity,
          Price = x.Price,
          Quantity = x.Quantity,
          Pack_ID = x.Pack_ID,
          Party = x.Party,
          Product = new ProductInfo()
          {
            AlcCode = x.Product.AlcCode,
            FullName = x.Product.FullName,
            ShortName = x.Product.ShortName,
            Capacity = x.Product.Capacity,
            AlcVolumeSpecified = x.alcPercentSpecified,
            AlcVolume = x.alcPercent,
            ProductVCode = x.Product.ProductVCode,
            InformF2RegId = x.InformF2.F2RegId
          }
        })));
      }
      if (document.Document.Item.GetType() == typeof (WayBillInformBRegType))
      {
        WayBillInformBRegType billInformBregType = (WayBillInformBRegType) document.Document.Item;
        this.WBDate = billInformBregType.Header.WBDate;
        this.WBNUMBER = billInformBregType.Header.WBNUMBER;
        this.Identity = billInformBregType.Header.Identity;
        this.INN = billInformBregType.Header.Shipper.INN;
        this.KPP = billInformBregType.Header.Shipper.KPP;
        this.WBRegId = billInformBregType.Header.WBRegId;
        this.ShipperName = billInformBregType.Header.Shipper.FullName;
        ((IEnumerable<InformBPositionType>) billInformBregType.Content).ToList<InformBPositionType>().ForEach((Action<InformBPositionType>) (x => this.DictionaryInformBRegId.Add(x.Identity, x.InformBRegId)));
        LogHelper.WriteToEgaisLog(this.DictionaryInformBRegId.ToJsonString(true));
      }
      if (!(document.Document.Item.GetType() == typeof (WayBillInformF2RegType)))
        return;
      WayBillInformF2RegType billInformF2RegType = (WayBillInformF2RegType) document.Document.Item;
      this.WBDate = billInformF2RegType.Header.WBDate;
      this.WBNUMBER = billInformF2RegType.Header.WBNUMBER;
      this.Identity = billInformF2RegType.Header.Identity;
      if (billInformF2RegType.Header.Shipper.Item is ULType ulType1)
      {
        this.INN = ulType1.INN;
        this.KPP = ulType1.KPP;
        this.ShipperName = ulType1.FullName;
      }
      if (billInformF2RegType.Header.Shipper.Item is FLType flType1)
      {
        this.INN = flType1.INN;
        this.ShipperName = flType1.FullName;
      }
      this.WBRegId = billInformF2RegType.Header.WBRegId;
      ((IEnumerable<InformF2PositionType>) billInformF2RegType.Content).ToList<InformF2PositionType>().ForEach((Action<InformF2PositionType>) (x => this.DictionaryInformBRegId.Add(x.Identity, x.InformF2RegId)));
      LogHelper.WriteToEgaisLog(this.DictionaryInformBRegId.ToJsonString(true));
    }
  }
}
