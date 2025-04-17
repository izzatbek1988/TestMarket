// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities.CloseSessionRequest
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities
{
  public class CloseSessionRequest : Request<ResultAnswerSession>
  {
    public override string Method => "Api.CloseZReport";

    public override object Params { get; set; } = (object) new ParamsSession();
  }
}
