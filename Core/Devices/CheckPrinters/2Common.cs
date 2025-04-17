// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.KkmStatus
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters
{
  public class KkmStatus
  {
    public CheckStatuses CheckStatus { get; set; }

    public SessionStatuses SessionStatus { get; set; }

    public KkmStatuses KkmState { get; set; }

    public Version DriverVersion { get; set; } = new Version("1.0.0.0");

    public int CheckNumber { get; set; } = 1;

    public int SessionNumber { get; set; } = 1;

    public DateTime? SessionStarted { get; set; }

    public DateTime? OfdLastSendDateTime { get; set; }

    public int OfdNotSendDocuments { get; set; }

    public string FactoryNumber { get; set; }

    public string SoftwareVersion { get; set; }

    public string Model { get; set; }

    public DateTime FnDateEnd { get; set; }
  }
}
