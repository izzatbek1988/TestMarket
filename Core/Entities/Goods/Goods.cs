// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Goods.Good
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Gbs.Core.Entities.Goods
{
  public class Good : Entity
  {
    [JsonIgnore]
    private Dictionary<Guid, object> _propertiesDictionary = new Dictionary<Guid, object>();

    public DateTime DateAdd { get; set; } = DateTime.Now;

    public DateTime DateUpdate { get; set; } = new DateTime(2001, 1, 1);

    public int Id { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public GoodGroups.Group Group { get; set; }

    public List<EntityProperties.PropertyValue> Properties { get; set; } = new List<EntityProperties.PropertyValue>();

    [JsonIgnore]
    public Dictionary<Guid, object> PropertiesDictionary
    {
      get
      {
        foreach (EntityProperties.PropertyValue property in this.Properties)
          this._propertiesDictionary[property.Type.Uid] = property.Value;
        return this._propertiesDictionary;
      }
      set => this._propertiesDictionary = value;
    }

    public List<GoodsStocks.GoodStock> StocksAndPrices { get; set; } = new List<GoodsStocks.GoodStock>();

    public IEnumerable<GoodsModifications.GoodModification> Modifications { get; set; } = (IEnumerable<GoodsModifications.GoodModification>) new List<GoodsModifications.GoodModification>();

    public IEnumerable<GoodsSets.Set> SetContent { get; set; } = (IEnumerable<GoodsSets.Set>) new List<GoodsSets.Set>();

    public GlobalDictionaries.GoodsSetStatuses SetStatus { get; set; }

    [StringLength(100)]
    public string Barcode { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public IEnumerable<string> Barcodes { get; set; } = (IEnumerable<string>) new List<string>();

    public object Tag { get; set; }
  }
}
