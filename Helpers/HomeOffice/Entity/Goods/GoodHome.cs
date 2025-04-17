// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOffice.Entity.GoodHome
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.HomeOffice.Entity
{
  public class GoodHome
  {
    public Guid Uid { get; set; }

    public DateTime DateAdd { get; set; } = DateTime.Now;

    public string Name { get; set; } = string.Empty;

    public Guid GroupUid { get; set; }

    public List<EntityProperties.PropertyValue> Properties { get; set; }

    public List<GoodsStocks.GoodStock> StocksAndPrices { get; set; }

    public IEnumerable<GoodsModifications.GoodModification> Modifications { get; set; }

    public IEnumerable<GoodsSets.Set> SetContent { get; set; }

    public GlobalDictionaries.GoodsSetStatuses SetStatus { get; set; }

    public string Barcode { get; set; }

    public string Description { get; set; }

    public IEnumerable<string> Barcodes { get; set; }

    public bool IsDeleted { get; set; }

    public GoodHome(Gbs.Core.Entities.Goods.Good good)
    {
      this.Uid = good.Uid;
      this.Barcode = good.Barcode;
      this.Barcodes = good.Barcodes;
      this.DateAdd = good.DateAdd;
      this.Description = good.Description;
      this.Name = good.Name;
      this.GroupUid = good.Group.Uid;
      this.IsDeleted = good.IsDeleted;
      this.SetStatus = good.SetStatus;
      this.Properties = good.Properties;
      this.Modifications = good.Modifications;
      this.SetContent = good.SetContent;
      this.StocksAndPrices = good.StocksAndPrices;
    }

    public GoodHome()
    {
    }
  }
}
