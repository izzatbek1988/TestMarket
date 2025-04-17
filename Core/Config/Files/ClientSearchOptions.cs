// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ClientSearchOptions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class ClientSearchOptions
  {
    public bool IsName { get; set; } = true;

    public bool IsBarcode { get; set; } = true;

    public bool IsPhone { get; set; } = true;

    public bool IsEmail { get; set; } = true;

    public List<GoodProp.PropItem> PropList { get; set; } = new List<GoodProp.PropItem>();
  }
}
