// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmProgressBar
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmProgressBar : WindowWithSize, IComponentConnector
  {
    internal ItemsControl tStack;
    private bool _contentLoaded;

    public ProgressBarViewModel Model { get; set; }

    public FrmProgressBar()
    {
      this.InitializeComponent();
      this.Model = new ProgressBarViewModel();
      this.DataContext = (object) this.Model;
    }

    private void FrmProgressBar_OnLoaded(object sender, RoutedEventArgs e)
    {
      try
      {
        LogHelper.OnBegin("Progress bar on loaded");
        this.Topmost = true;
        this.Focusable = false;
        this.ShowInTaskbar = false;
        this.Visibility = Visibility.Visible;
        Rect workArea = SystemParameters.WorkArea;
        this.Left = workArea.Right - this.Width - 10.0;
        this.Top = 10.0;
        this.Height = workArea.Bottom - 50.0;
        WindowInteropHelper windowInteropHelper = new WindowInteropHelper((Window) this);
        FrmProgressBar.SetWindowLong(windowInteropHelper.Handle, -20, (IntPtr) ((int) FrmProgressBar.GetWindowLong(windowInteropHelper.Handle, -20) | 128));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
    }

    private void FrmProgressBar_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.DragMove();
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

    public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
      try
      {
        FrmProgressBar.SetLastError(0);
        int lastWin32Error;
        IntPtr num1;
        if (IntPtr.Size == 4)
        {
          int num2 = FrmProgressBar.IntSetWindowLong(hWnd, nIndex, FrmProgressBar.IntPtrToInt32(dwNewLong));
          lastWin32Error = Marshal.GetLastWin32Error();
          num1 = new IntPtr(num2);
        }
        else
        {
          num1 = FrmProgressBar.IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
          lastWin32Error = Marshal.GetLastWin32Error();
        }
        if (num1 == IntPtr.Zero)
          ;
        return num1;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка установки значения окна");
        return IntPtr.Zero;
      }
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
    private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
    private static extern int IntSetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    private static int IntPtrToInt32(IntPtr intPtr) => (int) intPtr.ToInt64();

    [DllImport("kernel32.dll")]
    public static extern void SetLastError(int dwErrorCode);

    private void FrmProgressBar_OnActivated(object sender, EventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/progressbar/frmprogressbar.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.tStack = (ItemsControl) target;
      else
        this._contentLoaded = true;
    }

    [Flags]
    public enum ExtendedWindowStyles
    {
      WS_EX_TOOLWINDOW = 128, // 0x00000080
    }

    public enum GetWindowLongFields
    {
      GWL_EXSTYLE = -20, // 0xFFFFFFEC
    }
  }
}
