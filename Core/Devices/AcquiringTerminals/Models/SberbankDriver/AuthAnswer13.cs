// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.AuthAnswer13
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct AuthAnswer13
  {
    public AuthAnswer ans;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
    public string AuthCode;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 25)]
    public string CardID;
    public int ErrorCode;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
    public string TransDate;
    public int TransNumber;
    public int SberOwnCard;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
    public string Hash;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 104)]
    public string Track3;
    public uint RequestID;
    public uint Department;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
    public string RRN;
    public uint CurrencyCode;
    private byte CardEntryMode;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string CardName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string AID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string FullErrorText;
  }
}
