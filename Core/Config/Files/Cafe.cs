// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Cafe
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Config
{
  public class Cafe : IConfig
  {
    public bool IsFullScreen { get; set; } = true;

    public bool IsTouchScreen { get; set; }

    public bool IsButtonPrint { get; set; }

    public bool IsPanelClient { get; set; } = true;

    public bool IsTableAndGuest { get; set; }

    public bool IsNeedCommentForDelete { get; set; }

    public Cafe.MenuConfig Menu { get; set; } = new Cafe.MenuConfig();

    public bool IsPercentForService { get; set; }

    public bool IsAddToOrderByBarcode { get; set; }

    public bool IsSpeedCafeOrder { get; set; }

    public string ConfigFileName { get; } = "CafeSetting";

    public class MenuConfig
    {
      public bool IsShowImageGood { get; set; }

      public bool IsShowStockGood { get; set; }

      public bool IsReturnInMain { get; set; }

      public int CardSize { get; set; } = 200;

      public Cafe.MenuConfig.SelectGoodForCafeEnum SelectGoodForCafe { get; set; }

      public enum SelectGoodForCafeEnum
      {
        None,
        OverGroup,
        UnderGroup,
      }
    }
  }
}
