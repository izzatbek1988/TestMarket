// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Egais.InfoToTapBeer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Entities.Egais
{
  public class InfoToTapBeer : Entity
  {
    public TapBeer Tap { get; set; }

    public Guid GoodUid { get; set; }

    public Guid ChildGoodUid { get; set; }

    public Guid DocumentUid { get; set; }

    public Decimal? Price { get; set; }

    public bool IsSendToCrpt { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public Decimal? Quantity { get; set; }

    public Decimal SaleQuantity { get; set; }

    public string MarkedInfo { get; set; } = "";

    public DateTime? ConnectingDateTime { get; set; }

    public Guid StorageUid { get; set; }
  }
}
