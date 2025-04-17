// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.MettlerToledoDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.ScalesWIthLabels.Models
{
  public class MettlerToledoDriver
  {
    public const string PathDriver = "dll\\label_scale\\mettler\\TransferEth.dll";

    [DllImport("dll\\label_scale\\mettler\\TransferEth.dll", CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr Transfer_Ethernet(string cTransScale);
  }
}
