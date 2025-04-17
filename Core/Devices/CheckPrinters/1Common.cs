// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.KkmLastActionResult
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters
{
  public class KkmLastActionResult
  {
    public ActionsResults ActionResult { get; set; }

    public string Message { get; set; } = string.Empty;

    public Exception Exception { get; set; }

    public Actions Action { get; set; }
  }
}
