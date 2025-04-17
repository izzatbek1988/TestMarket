// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.PaymentResponse
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class PaymentResponse
  {
    public int ResponseCode { get; internal set; }

    public string ErrorMessage { get; internal set; }

    public bool IsSuccess => this.ResponseCode == 0;

    public string[] Checks { get; internal set; }

    public string AuthCode { get; internal set; }

    public string CardHash { get; internal set; }
  }
}
