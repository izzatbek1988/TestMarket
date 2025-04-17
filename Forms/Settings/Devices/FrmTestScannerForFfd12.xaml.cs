// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.TestScannerForFfd12ViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public partial class TestScannerForFfd12ViewModel : ViewModelWithForm
  {
    private Gbs.Core.Config.Devices _devConfig;
    private Random _random = new Random(System.Environment.TickCount);
    private string _keysInfoText;
    private Key _currentKey;
    private string _textScanner = string.Empty;
    private Visibility _setGsHotKeyVisibility;

    public string DataMatrix { get; set; }

    public string TextScanner
    {
      get => this._textScanner;
      set
      {
        this._textScanner = value;
        this.OnPropertyChanged(nameof (TextScanner));
      }
    }

    public BitmapImage DataMatrixImage => this.DataMatrix.GetDataMatrixCode();

    public TestScannerForFfd12ViewModel()
    {
      char ch = Convert.ToChar(29);
      this.DataMatrix = this.RandomString(10) + ch.ToString() + this.RandomString(10);
    }

    public ICommand CheckTextCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          char newChar = Convert.ToChar(29);
          LogHelper.Debug("Со сканера пришла строка " + this.TextScanner + " Код для проверки: " + this.DataMatrix);
          string str = this.TextScanner.Trim('\r', '\n', ' ').Replace(' ', newChar);
          LogHelper.Debug("Преобразовали строку для сравнения " + str);
          if (this.TextScanner.Equals(this.DataMatrix) || str.Equals(this.DataMatrix))
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.TestScannerForFfd12ViewModel_CheckTextCommand_Сканер_настроен_корректно_);
          }
          else
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.TestScannerForFfd12ViewModel_CheckTextCommand_, icon: MessageBoxImage.Hand);
          }
        }));
      }
    }

    public string KeysInfoText
    {
      get => this._keysInfoText;
      set
      {
        this._keysInfoText = value;
        this.OnPropertyChanged(nameof (KeysInfoText));
      }
    }

    public Key CurrentKey
    {
      get => this._currentKey;
      set
      {
        this._currentKey = value;
        if (KeyCharHelper.GetCharFromKey(this._currentKey).HasValue && Keyboard.Modifiers != ModifierKeys.Control)
          return;
        if (this._currentKey.IsEither<Key>(Key.LeftShift, Key.RightShift, Key.LeftCtrl, Key.RightCtrl, Key.Delete, Key.Back))
          return;
        string textScanner = this.TextScanner;
        if (textScanner.Length == 0)
          return;
        string str1 = this.DataMatrix.Replace(textScanner, string.Empty);
        ModifierKeys modifiers = Keyboard.Modifiers;
        string str2 = string.Empty;
        if (modifiers != ModifierKeys.None)
          str2 = string.Format("{0} + ", (object) modifiers);
        string str3 = this._currentKey.ToString();
        string str4 = Convert.ToChar(29).ToString();
        if (!str1.StartsWith(str4))
          return;
        this.KeysInfoText = string.Format(Translate.TestScannerForFfd12ViewModel_, (object) str2, (object) str3);
      }
    }

    public Visibility SetGsHotKeyVisibility
    {
      get => Visibility.Hidden;
      set
      {
        this._setGsHotKeyVisibility = value;
        this.OnPropertyChanged(nameof (SetGsHotKeyVisibility));
      }
    }

    public ICommand SetGsHotKeyCommand => (ICommand) new RelayCommand((Action<object>) (o => { }));

    public void Test(Gbs.Core.Config.Devices devicesConfig)
    {
      CultureInfo currentInputLanguage = InputLanguageManager.Current.CurrentInputLanguage;
      IEnumerable availableInputLanguages = InputLanguageManager.Current.AvailableInputLanguages;
      CultureInfo cultureInfo = availableInputLanguages != null ? availableInputLanguages.OfType<CultureInfo>().FirstOrDefault<CultureInfo>((Func<CultureInfo, bool>) (l => l.Name.StartsWith("en"))) : (CultureInfo) null;
      if (cultureInfo != null)
      {
        LogHelper.Debug("Устанавливаю EN раскладку для ввода кода маркировки");
        InputLanguageManager.Current.CurrentInputLanguage = cultureInfo;
      }
      this._devConfig = devicesConfig;
      this.FormToSHow = (WindowWithSize) new FrmTestScannerForFfd12(this._devConfig.BarcodeScanner.GsCodeHotKey, this);
      this.ShowForm();
      if (currentInputLanguage == null)
        return;
      InputLanguageManager.Current.CurrentInputLanguage = currentInputLanguage;
    }

    public string RandomString(int length)
    {
      string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&**()_+=-";
      StringBuilder stringBuilder = new StringBuilder(length);
      for (int index = 0; index < length; ++index)
        stringBuilder.Append(str[this._random.Next(str.Length)]);
      return stringBuilder.ToString();
    }

    internal void ComPortScannerOnBarcodeChanged(string barcode) => this.TextScanner = barcode;
  }
}
