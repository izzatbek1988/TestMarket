// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Fifo.FifoItem
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System;

#nullable disable
namespace Gbs.Helpers.Fifo
{
  public class FifoItem
  {
    public Gbs.Core.Entities.Goods.Good Good { get; set; }

    public Decimal Quantity { get; set; }

    public Decimal Price { get; set; }

    public Guid ModificationUid { get; set; }

    public FifoItem(Gbs.Core.Entities.Goods.Good good, Decimal quantity, Decimal price)
    {
      this.Good = good;
      this.Quantity = quantity;
      this.Price = price;
    }

    public FifoItem(Gbs.Core.Entities.Goods.Good good, Decimal quantity, Decimal price, Guid modificationUid)
    {
      this.Good = good;
      this.Quantity = quantity;
      this.Price = price;
      this.ModificationUid = modificationUid;
    }

    public void Validate()
    {
      if (this.Quantity <= 0M)
        throw new ArgumentException(Translate.FifoItem_Validate_Значение_кол_ва_для_должно_быть_больше_нуля);
    }
  }
}
