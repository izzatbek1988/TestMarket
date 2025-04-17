// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.Rtsdrv
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.ScalesWIthLabels.Models
{
  public class Rtsdrv
  {
    public const string PathDriver = "dll\\label_scale\\rongta\\rtscaledrv.dll";

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleConnectEx(
      string RefLFZKFileName,
      string RefCFGFileName,
      string IPAddr);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleDisConnectEx(string IPAddr);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleConnect(
      string RefLFZKFileName,
      string RefCFGFileName,
      int SerialNO,
      string CommName,
      int BaudRate);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleDisConnect(int SerialNO);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleTransferPLUCluster(Rtsdrv.PLU[] plu);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleTransferPLUByJson(string PluJson);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleClearPLUData();

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleTransferMessage(int id, string PMessage, int DataLen);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleGetPluWeight(ref double dWeight);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscaleTransferHotkey(int[] HotkeyTable, int TableIndex);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll")]
    public static extern int rtscalePLUToStr(Rtsdrv.PLU[] LPPLU, StringBuilder ptr);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int rtscaleUploadSaleDataEx(bool AIsClear, IntPtr p);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int rtscaleUploadSaleData(bool AIsClear, IntPtr p);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int rtscaleUploadPluData(IntPtr p);

    [DllImport("dll\\label_scale\\rongta\\rtscaledrv.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int rtscaleUploadMessage(IntPtr p);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PLU
    {
      public string Name;
      public int LFCode;
      public string Code;
      public int BarCode;
      public int UnitPrice;
      public int WeightUnit;
      public int Deptment;
      public double Tare;
      public int ShlefTime;
      public int PackageType;
      public double PackageWeight;
      public int Tolerance;
      public int Message1;
      public byte Reserved;
      public short Reserved1;
      public byte Message2;
      public byte Reserved2;
      public byte MultiLabel;
      public byte Rebate;
      public int Account;
    }
  }
}
