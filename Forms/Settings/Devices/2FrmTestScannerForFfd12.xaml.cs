// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.KeyCharHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public class KeyCharHelper
  {
    [DllImport("user32.dll")]
    public static extern int ToUnicode(
      uint wVirtKey,
      uint wScanCode,
      byte[] lpKeyState,
      [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pwszBuff,
      int cchBuff,
      uint wFlags);

    [DllImport("user32.dll")]
    public static extern bool GetKeyboardState(byte[] lpKeyState);

    [DllImport("user32.dll")]
    public static extern uint MapVirtualKey(uint uCode, KeyCharHelper.MapType uMapType);

    public static char? GetCharFromKey(Key key)
    {
      char? charFromKey = new char?();
      int num = KeyInterop.VirtualKeyFromKey(key);
      byte[] lpKeyState = new byte[256];
      KeyCharHelper.GetKeyboardState(lpKeyState);
      uint wScanCode = KeyCharHelper.MapVirtualKey((uint) num, KeyCharHelper.MapType.MAPVK_VK_TO_VSC);
      StringBuilder pwszBuff = new StringBuilder(2);
      switch (KeyCharHelper.ToUnicode((uint) num, wScanCode, lpKeyState, pwszBuff, pwszBuff.Capacity, 0U))
      {
        case -1:
        case 0:
          return charFromKey;
        case 1:
          charFromKey = new char?(pwszBuff[0]);
          goto case -1;
        default:
          charFromKey = new char?(pwszBuff[0]);
          goto case -1;
      }
    }

    public enum MapType : uint
    {
      MAPVK_VK_TO_VSC,
      MAPVK_VSC_TO_VK,
      MAPVK_VK_TO_CHAR,
      MAPVK_VSC_TO_VK_EX,
    }
  }
}
