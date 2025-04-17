// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.GoodProp
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class GoodProp
  {
    public bool IsCheckedName { get; set; } = true;

    public bool IsCheckedBarcode { get; set; } = true;

    public bool IsCheckedBarcodes { get; set; } = true;

    public bool IsCheckedDescription { get; set; } = true;

    public bool IsCheckedModificationBarcode { get; set; }

    public List<GoodProp.PropItem> PropList { get; set; } = new List<GoodProp.PropItem>();

    public class PropItem
    {
      public Guid Uid { get; set; }

      public bool IsChecked { get; set; }
    }
  }
}
