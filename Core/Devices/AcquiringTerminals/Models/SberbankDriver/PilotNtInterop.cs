// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.PilotNtInterop
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  internal static class PilotNtInterop
  {
    private const string PilotNtPath = "dll/acquiring/sberbank/pilot_nt.dll";

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_card_authorize13", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int CardAuthorize(string track2, ref AuthAnswer13 authAnswer);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_SuspendTrx", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int SuspendTransaction(int amount, string track2);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_CommitTrx", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int CommitTransaction(int amount, string track2);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_RollbackTrx", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int RollBackTransaction(int amount, string track2);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_close_day", CallingConvention = CallingConvention.Cdecl)]
    public static extern int CloseDay(ref AuthAnswer ans);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_get_statistics", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetStatistics(ref AuthAnswer ans);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_ServiceMenu", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ServiceMenu(ref AuthAnswer ans);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_GetTerminalID", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetTerminalID(byte[] pTerminalId);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_SetConfigData", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SetConfigData(byte[] pConfData);

    [DllImport("dll/acquiring/sberbank/pilot_nt.dll", EntryPoint = "_Done", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Done();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GlobalFree(IntPtr handle);
  }
}
