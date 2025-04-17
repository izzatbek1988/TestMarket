// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmKeyboardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class FrmKeyboardViewModel : ViewModelWithForm
  {
    public System.Timers.Timer Timer;
    private Visibility _lettersVisibility;
    private static int ButtonBaseSize = 50;

    public int BaseSize => FrmKeyboardViewModel.ButtonBaseSize;

    public int Button050Size => (int) ((double) this.BaseSize * 0.5);

    public int Button150Size => (int) ((double) this.BaseSize * 1.4);

    public int Button170Size
    {
      get => (int) ((double) this.Button150Size * 1.24 + (double) this.ButtonsMargin);
    }

    public int Button200Size => (int) ((double) this.BaseSize * 2.0) + 2 * this.ButtonsMargin;

    public int Button220Size
    {
      get => (int) ((double) this.Button150Size * 1.46 + (double) (4 * this.ButtonsMargin));
    }

    public int Button300Size => (int) ((double) this.BaseSize * 3.0) + 4 * this.ButtonsMargin;

    public int ButtonSpaceSize
    {
      get => (int) ((double) this.BaseSize * 8.5 + (double) (16 * this.ButtonsMargin));
    }

    public int ButtonsMargin => (int) ((double) this.BaseSize / 20.0) + 1;

    public Visibility LettersVisibility
    {
      get => this._lettersVisibility;
      set
      {
        if (this._lettersVisibility == value)
          return;
        this.UpdateFormPosition();
        this._lettersVisibility = value;
        this.NumEnterVisibility = this._lettersVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        this.OnPropertyChanged("NumEnterVisibility");
        this.OnPropertyChanged(nameof (LettersVisibility));
      }
    }

    private void UpdateFormPosition()
    {
      try
      {
        if (FrmKeyboard.CurrentForm == null)
          return;
        Window curF = FrmKeyboard.CurrentForm;
        int coeff = this.LettersVisibility == Visibility.Visible ? 1 : -1;
        curF?.Dispatcher?.InvokeAsync((Action) (() => curF.Left += (double) (8 * this.BaseSize * coeff)));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public Visibility NumEnterVisibility { get; set; } = Visibility.Collapsed;

    public ICommand HideShowLettersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.LettersVisibility == Visibility.Visible)
            this.LettersVisibility = Visibility.Collapsed;
          else
            this.LettersVisibility = Visibility.Visible;
        }));
      }
    }

    public void UpdateButtons()
    {
      FrmKeyboardViewModel.ButtonBaseSize = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.VirtualKeyboard.ButtonSize;
      this.OnPropertyChanged("", true);
    }

    public Visibility ShiftBorderVisibility { get; set; }

    public Visibility CapsIndicatorVisibility { get; set; }

    public FrmKeyboardViewModel.keyButton Button_TAB => new FrmKeyboardViewModel.keyButton(9);

    public FrmKeyboardViewModel.keyButton Q_button => new FrmKeyboardViewModel.keyButton(81);

    public FrmKeyboardViewModel.keyButton W_button => new FrmKeyboardViewModel.keyButton(87);

    public FrmKeyboardViewModel.keyButton E_button => new FrmKeyboardViewModel.keyButton(69);

    public FrmKeyboardViewModel.keyButton R_button => new FrmKeyboardViewModel.keyButton(82);

    public FrmKeyboardViewModel.keyButton T_button => new FrmKeyboardViewModel.keyButton(84);

    public FrmKeyboardViewModel.keyButton Y_button => new FrmKeyboardViewModel.keyButton(89);

    public FrmKeyboardViewModel.keyButton U_button => new FrmKeyboardViewModel.keyButton(85);

    public FrmKeyboardViewModel.keyButton I_button => new FrmKeyboardViewModel.keyButton(73);

    public FrmKeyboardViewModel.keyButton O_button => new FrmKeyboardViewModel.keyButton(79);

    public FrmKeyboardViewModel.keyButton P_button => new FrmKeyboardViewModel.keyButton(80);

    public FrmKeyboardViewModel.keyButton Button_219 => new FrmKeyboardViewModel.keyButton(219);

    public FrmKeyboardViewModel.keyButton Button_220 => new FrmKeyboardViewModel.keyButton(220);

    public FrmKeyboardViewModel.keyButton Button_221 => new FrmKeyboardViewModel.keyButton(221);

    public FrmKeyboardViewModel.keyButton Button_CAPS => new FrmKeyboardViewModel.keyButton(20);

    public FrmKeyboardViewModel.keyButton A_button => new FrmKeyboardViewModel.keyButton(65);

    public FrmKeyboardViewModel.keyButton S_button => new FrmKeyboardViewModel.keyButton(83);

    public FrmKeyboardViewModel.keyButton D_button => new FrmKeyboardViewModel.keyButton(68);

    public FrmKeyboardViewModel.keyButton F_button => new FrmKeyboardViewModel.keyButton(70);

    public FrmKeyboardViewModel.keyButton G_button => new FrmKeyboardViewModel.keyButton(71);

    public FrmKeyboardViewModel.keyButton H_button => new FrmKeyboardViewModel.keyButton(72);

    public FrmKeyboardViewModel.keyButton J_button => new FrmKeyboardViewModel.keyButton(74);

    public FrmKeyboardViewModel.keyButton K_button => new FrmKeyboardViewModel.keyButton(75);

    public FrmKeyboardViewModel.keyButton L_button => new FrmKeyboardViewModel.keyButton(76);

    public FrmKeyboardViewModel.keyButton Button_186 => new FrmKeyboardViewModel.keyButton(186);

    public FrmKeyboardViewModel.keyButton Button_222 => new FrmKeyboardViewModel.keyButton(222);

    public FrmKeyboardViewModel.keyButton Button_ENTER => new FrmKeyboardViewModel.keyButton(13);

    public FrmKeyboardViewModel.keyButton Button_LSHIFT => new FrmKeyboardViewModel.keyButton(160);

    public FrmKeyboardViewModel.keyButton Z_button => new FrmKeyboardViewModel.keyButton(90);

    public FrmKeyboardViewModel.keyButton X_button => new FrmKeyboardViewModel.keyButton(88);

    public FrmKeyboardViewModel.keyButton C_button => new FrmKeyboardViewModel.keyButton(67);

    public FrmKeyboardViewModel.keyButton V_button => new FrmKeyboardViewModel.keyButton(86);

    public FrmKeyboardViewModel.keyButton B_button => new FrmKeyboardViewModel.keyButton(66);

    public FrmKeyboardViewModel.keyButton N_button => new FrmKeyboardViewModel.keyButton(78);

    public FrmKeyboardViewModel.keyButton M_button => new FrmKeyboardViewModel.keyButton(77);

    public FrmKeyboardViewModel.keyButton Button_188 => new FrmKeyboardViewModel.keyButton(188);

    public FrmKeyboardViewModel.keyButton Button_190 => new FrmKeyboardViewModel.keyButton(190);

    public FrmKeyboardViewModel.keyButton Button_191 => new FrmKeyboardViewModel.keyButton(191);

    public FrmKeyboardViewModel.keyButton Button_RSHIFT => new FrmKeyboardViewModel.keyButton(161);

    public FrmKeyboardViewModel.keyButton Button_SPACE => new FrmKeyboardViewModel.keyButton(32);

    public FrmKeyboardViewModel.keyButton Button_D_DOT => new FrmKeyboardViewModel.keyButton(110);

    public FrmKeyboardViewModel.keyButton Button_D0 => new FrmKeyboardViewModel.keyButton(96);

    public FrmKeyboardViewModel.keyButton Button_D1 => new FrmKeyboardViewModel.keyButton(97);

    public FrmKeyboardViewModel.keyButton Button_D2 => new FrmKeyboardViewModel.keyButton(98);

    public FrmKeyboardViewModel.keyButton Button_D3 => new FrmKeyboardViewModel.keyButton(99);

    public FrmKeyboardViewModel.keyButton Button_D4 => new FrmKeyboardViewModel.keyButton(100);

    public FrmKeyboardViewModel.keyButton Button_D5 => new FrmKeyboardViewModel.keyButton(101);

    public FrmKeyboardViewModel.keyButton Button_D6 => new FrmKeyboardViewModel.keyButton(102);

    public FrmKeyboardViewModel.keyButton Button_D7 => new FrmKeyboardViewModel.keyButton(103);

    public FrmKeyboardViewModel.keyButton Button_D8 => new FrmKeyboardViewModel.keyButton(104);

    public FrmKeyboardViewModel.keyButton Button_D9 => new FrmKeyboardViewModel.keyButton(105);

    public FrmKeyboardViewModel.keyButton Button_BackSpace => new FrmKeyboardViewModel.keyButton(8);

    public FrmKeyboardViewModel.keyButton Button_Delete => new FrmKeyboardViewModel.keyButton(46);

    public string CurrnetLang
    {
      get => InputLanguage.CurrentInputLanguage.Culture.ThreeLetterISOLanguageName.ToUpper();
    }

    public class keyButton
    {
      public string Value
      {
        get
        {
          return FrmKeyboard.IsShift ? new FrmKeyboardViewModel.User32Interop().ToAscii(this.Key, Keys.LShiftKey) : new FrmKeyboardViewModel.User32Interop().ToAscii(this.Key);
        }
      }

      public int Key { get; private set; }

      public keyButton(int code) => this.Key = code;
    }

    public class User32Interop
    {
      [DllImport("user32.dll", CharSet = CharSet.Unicode)]
      public static extern int ToUnicode(
        uint virtualKeyCode,
        uint scanCode,
        byte[] keyboardState,
        StringBuilder receivingBuffer,
        int bufferSize,
        uint flags);

      public static string GetCharsFromKeys(Keys keys, bool shift)
      {
        StringBuilder receivingBuffer = new StringBuilder(256);
        byte[] keyboardState = new byte[256];
        if (shift)
          keyboardState[16] = byte.MaxValue;
        FrmKeyboardViewModel.User32Interop.ToUnicode((uint) keys, 0U, keyboardState, receivingBuffer, 256, 0U);
        return receivingBuffer.ToString();
      }

      public string ToAscii(int keyCode, Keys modifiers = Keys.None)
      {
        CultureInfo cur = InputLanguage.CurrentInputLanguage.Culture;
        System.Windows.Application.Current.Dispatcher?.InvokeAsync((Action) (() => cur = InputLanguage.CurrentInputLanguage.Culture));
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(cur);
        int num;
        if (!modifiers.IsEither<Keys>(Keys.Shift, Keys.LShiftKey, Keys.RShiftKey))
          num = Keyboard.Modifiers.IsEither<ModifierKeys>(ModifierKeys.Shift) ? 1 : 0;
        else
          num = 1;
        bool shift = num != 0;
        string ascii = FrmKeyboardViewModel.User32Interop.GetCharsFromKeys((Keys) keyCode, shift);
        if ((Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) == KeyStates.Toggled)
          ascii = !shift ? ascii.ToUpper() : ascii.ToLower();
        return ascii;
      }
    }
  }
}
