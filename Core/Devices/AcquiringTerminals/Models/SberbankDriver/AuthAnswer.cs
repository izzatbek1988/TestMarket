// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.AuthAnswer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct AuthAnswer
  {
    public TransactionType TransactionType;
    public int Amount;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
    public string ResultCode;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string Message;
    public int CardType;
    public IntPtr Cheque;
  }
}
