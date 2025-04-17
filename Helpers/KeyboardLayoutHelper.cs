// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.KeyboardLayoutHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers
{
  public class KeyboardLayoutHelper
  {
    private const string EnLayout = "00000409";
    private const string RuLayout = "00000419";
    private const int VK_CAPITAL = 20;
    private const int KEYEVENTF_EXTENDEDKEY = 1;
    private const int KEYEVENTF_KEYUP = 2;

    [DllImport("user32.dll")]
    private static extern void keybd_event(
      byte bVk,
      byte bScan,
      uint dwFlags,
      UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

    [DllImport("user32")]
    private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int ToUnicodeEx(
      uint virtualKeyCode,
      uint scanCode,
      byte[] keyboardState,
      [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder receivingBuffer,
      int bufferSize,
      uint flags,
      IntPtr dwhkl);

    private static string ConvertStringByKeyboardLayout(
      string srcStr,
      IntPtr LayoutFrom,
      IntPtr LayoutTo)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in srcStr)
      {
        short num1 = KeyboardLayoutHelper.VkKeyScanEx(ch, LayoutFrom);
        if (num1 == (short) -1)
        {
          stringBuilder.Append(ch);
        }
        else
        {
          int num2 = (int) BitConverter.GetBytes(num1)[1];
          byte[] keyboardState = new byte[256];
          if (Convert.ToBoolean(num2 & 1))
            keyboardState[16] = byte.MaxValue;
          if (Convert.ToBoolean(num2 & 2))
            keyboardState[17] = byte.MaxValue;
          if (Convert.ToBoolean(num2 & 4))
            keyboardState[18] = byte.MaxValue;
          StringBuilder receivingBuffer = new StringBuilder();
          KeyboardLayoutHelper.ToUnicodeEx(Convert.ToUInt32(num1), 0U, keyboardState, receivingBuffer, 5, 0U, LayoutTo);
          stringBuilder.Append((object) receivingBuffer);
        }
      }
      return stringBuilder.ToString();
    }

    private static string ConvertStringByKeyboardLayoutName(
      string srcStr,
      string strLayoutFrom,
      string strLayoutTo)
    {
      IntPtr LayoutFrom = KeyboardLayoutHelper.LoadKeyboardLayout(strLayoutFrom, 0U);
      IntPtr LayoutTo = KeyboardLayoutHelper.LoadKeyboardLayout(strLayoutTo, 0U);
      return KeyboardLayoutHelper.ConvertStringByKeyboardLayout(srcStr, LayoutFrom, LayoutTo);
    }

    public static string ConvertEnToRuByKbl(string srcStr)
    {
      return KeyboardLayoutHelper.ConvertStringByKeyboardLayoutName(srcStr, "00000409", "00000419");
    }

    public static void IsDownCapsLock()
    {
      KeyboardLayoutHelper.keybd_event((byte) 20, (byte) 69, 1U, (UIntPtr) 0UL);
      KeyboardLayoutHelper.keybd_event((byte) 20, (byte) 69, 3U, (UIntPtr) 0UL);
    }

    public static string ConvertRuToEnByKbl(string srcStr)
    {
      try
      {
        return KeyboardLayoutHelper.ConvertStringByKeyboardLayoutName(srcStr, "00000419", "00000409");
      }
      catch
      {
        return srcStr;
      }
    }

    public static void SetSearchFocus(Control control, Window window)
    {
      try
      {
        bool isFormEnabled = false;
        Task.Run((Action) (() =>
        {
          Application.Current.Dispatcher?.Invoke((Action) (() => isFormEnabled = window.IsEnabled));
          while (!isFormEnabled)
          {
            Thread.Sleep(50);
            Application.Current.Dispatcher?.Invoke((Action) (() => isFormEnabled = window.IsEnabled));
          }
        })).ContinueWith((Action<Task>) (_ => Application.Current.Dispatcher?.Invoke((Action) (() =>
        {
          Keyboard.Focus((IInputElement) control);
          control.Focus();
        }))));
      }
      catch (Exception ex)
      {
        Other.ConsoleWrite(ex?.ToString() ?? "");
      }
    }
  }
}
