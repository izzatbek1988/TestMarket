// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Settings.SellerReportSetting
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Entities.Settings
{
  public class SellerReportSetting : Entity
  {
    public bool IsVisibilityReturn { get; set; }

    public bool IsVisibilityRemove { get; set; }

    public bool IsVisibilityInsert { get; set; }

    public bool IsVisibilitySumCash { get; set; }

    public bool IsVisibilitySumKkm { get; set; }

    public bool IsVisibilitySumPayment { get; set; }

    public bool IsVisibilityTablePayment { get; set; }

    public bool IsVisibilityDate { get; set; }
  }
}
