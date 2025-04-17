// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Licenses.IGbsIdHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Helpers.Licenses
{
  public interface IGbsIdHelper
  {
    string GetGbsId();

    string GetGbsIdWithPrefix();

    void DeleteGbsIdFromRegistry();
  }
}
