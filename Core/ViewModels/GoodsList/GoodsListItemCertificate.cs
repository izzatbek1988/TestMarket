// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Models.GoodInList.GoodsListItemsCertificate
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Core.Models.GoodInList
{
  public class GoodsListItemsCertificate
  {
    public Guid Uid { get; set; } = Guid.Empty;

    public Decimal Nominal { get; set; }

    public bool IsCertificate => this.Uid != Guid.Empty;

    public GoodsListItemsCertificate()
    {
    }

    public GoodsListItemsCertificate(Guid uid, Decimal nominal)
    {
      this.Uid = uid;
      this.Nominal = nominal;
    }
  }
}
