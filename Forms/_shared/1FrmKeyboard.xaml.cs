// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmKeyboard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;
using WindowsInput;
using WindowsInput.Native;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmKeyboard : WindowWithSize, IComponentConnector
  {
    private static bool IsOpened;
    private const int WS_EX_NOACTIVATE = 134217728;
    private const int GWL_EXSTYLE = -20;
    private FrmKeyboardViewModel _model;
    public static bool IsShift;
    private LinkedList<string> InstalledLangs = new LinkedList<string>();
    private bool _contentLoaded;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    protected override void OnSourceInitialized(EventArgs e)
    {
      IntPtr handle = new WindowInteropHelper((Window) this).Handle;
      FrmKeyboard.SetWindowLong(handle, -20, FrmKeyboard.GetWindowLong(handle, -20) | 134217728);
      base.OnSourceInitialized(e);
    }

    public FrmKeyboard()
    {
      try
      {
        this.InitializeComponent();
        this.Closed += new EventHandler(this.OnClosed);
        this.Closing += new CancelEventHandler(this.FrmKeyboard_Closing);
        FrmKeyboardViewModel keyboardViewModel = new FrmKeyboardViewModel();
        keyboardViewModel.FormToSHow = (WindowWithSize) this;
        keyboardViewModel.CloseAction = new Action(((Window) this).Close);
        this._model = keyboardViewModel;
        this.DataContext = (object) this._model;
        this.Loaded += new RoutedEventHandler(this.FrmKeyboard_Loaded);
        FrmKeyboard.CurrentForm = (Window) this;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось инициировать объект экранной клавиатуры");
      }
    }

    private void FrmKeyboard_Closing(object sender, CancelEventArgs e)
    {
      FrmKeyboard.CloseKeyboard();
      e.Cancel = true;
    }

    private static bool CloseMe { get; set; }

    public static void KillKeyboard()
    {
      FrmKeyboard.CurrentForm?.Dispatcher.Invoke((Action) (() =>
      {
        FrmKeyboard.CurrentForm?.Close();
        FrmKeyboard.CurrentForm = (Window) null;
      }));
    }

    public static void CloseKeyboard()
    {
      if (FrmKeyboard.CurrentForm == null)
        return;
      FrmKeyboard.CloseMe = true;
      LogHelper.Trace("Task 364 Start");
      Task.Run((Action) (() =>
      {
        try
        {
          Thread.Sleep(250);
          if (!FrmKeyboard.CloseMe)
            return;
          FrmKeyboard.CurrentForm?.Dispatcher?.InvokeAsync((Action) (() =>
          {
            if (!FrmKeyboard.CloseMe)
              return;
            FrmKeyboard.CurrentForm.Visibility = Visibility.Collapsed;
            FrmKeyboard.IsOpened = false;
          }));
        }
        catch (Exception ex)
        {
          LogHelper.Trace(ex.ToString());
        }
      })).ContinueWith((Action<Task>) (_ => LogHelper.Trace("Task 364 end")));
    }

    public static Window CurrentForm { get; private set; }

    private void FrmKeyboard_Loaded(object sender, RoutedEventArgs e)
    {
      Screen screen = Screen.FromPoint(new System.Drawing.Point((int) this.Left, (int) this.Top));
      LogHelper.Trace("current screen: " + screen.ToJsonString(true));
      LogHelper.Trace(string.Format("window size H: {0}; W: {1}", (object) this.Height, (object) this.Width));
      this._model.Timer = new System.Timers.Timer(250.0);
      this._model.Timer.Elapsed += (ElapsedEventHandler) ((_param1, _param2) => Task.Run((Action) (() =>
      {
        FrmKeyboard.CurrentForm?.Dispatcher?.InvokeAsync((Action) (() =>
        {
          this._model.ShiftBorderVisibility = FrmKeyboard.IsShift || Keyboard.Modifiers == ModifierKeys.Shift ? Visibility.Visible : Visibility.Collapsed;
          this._model.CapsIndicatorVisibility = (Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) == KeyStates.Toggled ? Visibility.Visible : Visibility.Collapsed;
        }));
        this._model.UpdateButtons();
      })).Wait(500));
      this._model.Timer.AutoReset = true;
      this._model.Timer.Start();
      this.InstalledLangs.Clear();
      foreach (InputLanguage installedInputLanguage in (ReadOnlyCollectionBase) InputLanguage.InstalledInputLanguages)
        this.InstalledLangs.AddLast(installedInputLanguage.Culture.Name);
      this.Top = SystemParameters.WorkArea.Bottom - this.Height;
      double num = (double) Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
      this.Left = (double) screen.WorkingArea.Left + ((double) screen.WorkingArea.Width / num - this.Width) / 2.0;
    }

    private void OnClosed(object sender, EventArgs e)
    {
      this._model.Timer?.Stop();
      this._model.Timer?.Dispose();
      FrmKeyboard.IsOpened = false;
      FrmKeyboard.CurrentForm = (Window) null;
    }

    public static void SelectAllInControl(System.Windows.Controls.Control control)
    {
      if (!new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.VirtualKeyboard.IsEnabled)
        return;
      Task.Run((Action) (() =>
      {
        try
        {
          Thread.Sleep(50);
          System.Windows.Application.Current.Dispatcher?.Invoke((Action) (() => { }));
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "");
        }
      }));
    }

    public static void ShowKeyboard(bool hideLetters = false)
    {
      if (!new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.VirtualKeyboard.IsEnabled)
        return;
      LogHelper.Debug("Показ экранной клавиатуры");
      FrmKeyboard.CloseMe = false;
      if (FrmKeyboard.IsOpened)
      {
        LogHelper.Trace("Task 462 start");
        Task.Run((Action) (() =>
        {
          try
          {
            FrmKeyboard.CurrentForm?.Dispatcher?.InvokeAsync((Action) (() => FrmKeyboard.AutoAdaptive(hideLetters)));
          }
          catch (Exception ex)
          {
            LogHelper.Trace(ex.ToString());
          }
        })).ContinueWith((Action<Task>) (_ => LogHelper.Trace("Task 462 end")));
      }
      else
      {
        FrmKeyboard.IsOpened = true;
        LogHelper.Trace("Task 480 start");
        Task.Run((Action) (() =>
        {
          try
          {
            if (FrmKeyboard.CurrentForm != null)
            {
              LogHelper.Trace("Клавиатура скрыта, показываю");
              FrmKeyboard.CurrentForm?.Dispatcher?.InvokeAsync((Action) (() =>
              {
                FrmKeyboard.CurrentForm.Visibility = Visibility.Visible;
                FrmKeyboard.CurrentForm.SizeToContent = SizeToContent.Manual;
                FrmKeyboard.CurrentForm.SizeToContent = SizeToContent.WidthAndHeight;
                FrmKeyboard.AutoAdaptive(hideLetters);
              }));
            }
            else
            {
              LogHelper.Trace("Клавиатура закрыта, запускаю");
              Thread thread = new Thread((ThreadStart) (() =>
              {
                try
                {
                  new FrmKeyboard() { Topmost = true }.Show();
                  Dispatcher.Run();
                  FrmKeyboard.AutoAdaptive(hideLetters);
                }
                catch (Exception ex)
                {
                  LogHelper.Trace(ex.ToString());
                }
              }));
              thread.SetApartmentState(ApartmentState.STA);
              thread.Start();
            }
          }
          catch (Exception ex)
          {
            LogHelper.Trace(ex.ToString());
          }
        })).ContinueWith((Action<Task>) (_ => LogHelper.Trace("Task 480 end")));
      }
    }

    private static void AutoAdaptive(bool hideLetters)
    {
      if (!new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.VirtualKeyboard.AutoAdaptiveForInputType)
        return;
      LogHelper.Trace("Адаптация клавиатуры под поле ввода");
      FrmKeyboardViewModel dataContext = (FrmKeyboardViewModel) FrmKeyboard.CurrentForm?.DataContext;
      if (dataContext == null)
        return;
      dataContext.LettersVisibility = hideLetters ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ChangeLang()
    {
      if (this.InstalledLangs.Count == 1)
        return;
      string name = InputLanguage.CurrentInputLanguage.Culture.Name;
      string newLang = string.Empty;
      LinkedListNode<string> linkedListNode = this.InstalledLangs.Find(name);
      if (linkedListNode == null)
        return;
      newLang = linkedListNode.Next == null ? this.InstalledLangs.First.Value : linkedListNode.Next.Value;
      LogHelper.Trace("Task 569 start");
      Task.Run((Action) (() =>
      {
        try
        {
          FrmKeyboard.CurrentForm?.Dispatcher?.InvokeAsync((Action) (() => InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo(newLang))));
          System.Windows.Application.Current.Dispatcher?.InvokeAsync((Action) (() => InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo(newLang))));
        }
        catch (Exception ex)
        {
          LogHelper.Trace(ex.ToString());
        }
      })).ContinueWith((Action<Task>) (_ => LogHelper.Trace("Task 569 end")));
      this._model.UpdateButtons();
    }

    private void FrmKeyboard_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
      try
      {
        if (!(e.Source is System.Windows.Controls.Button))
          return;
        System.Windows.Controls.Button source = (System.Windows.Controls.Button) e.Source;
        switch (source.Tag?.ToString())
        {
          case "close":
            this.Close();
            break;
          case "lang":
            this.ChangeLang();
            break;
          default:
            if (source.Content == null)
              break;
            source.Content.ToString();
            if (source.Tag == null)
              break;
            if (((int) source.Tag).IsEither<int>(this._model.Button_LSHIFT.Key, this._model.Button_RSHIFT.Key))
            {
              if (!FrmKeyboard.IsShift)
              {
                FrmKeyboard.IsShift = true;
                break;
              }
              FrmKeyboard.IsShift = false;
              break;
            }
            InputSimulator inputSimulator = new InputSimulator();
            VirtualKeyCode tag = (VirtualKeyCode) source.Tag;
            if (FrmKeyboard.IsShift && !source.Content.ToString().All<char>(new Func<char, bool>(char.IsDigit)))
              inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, tag);
            else
              inputSimulator.Keyboard.KeyPress(tag);
            FrmKeyboard.IsShift = false;
            break;
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void FrmKeyboard_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.Source is System.Windows.Controls.Button || e.ChangedButton != System.Windows.Input.MouseButton.Left)
        return;
      this.DragMove();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmkeyboard.xaml", UriKind.Relative));
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
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
