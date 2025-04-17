// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.ISbp
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  internal interface ISbp : IDevice
  {
    bool GetToken(bool isReturn = true);

    bool GetQr(out string payLoad, out string rrn, bool isReturn = true);

    bool GetStatus(out SpbHelper.EStatusQr status, bool isReturn = true);

    bool CancelQr();
  }
}
