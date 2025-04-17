// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.NumberBox
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using WindowsInput;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class NumberBox : UserControl, IComponentConnector
  {
    private bool DoSelectAll = true;
    public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (Decimal?), typeof (NumberBox), new PropertyMetadata((object) null, new PropertyChangedCallback(NumberBox.textChangedCallBack)));
    public static DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(nameof (DecimalPlaces), typeof (int?), typeof (NumberBox));
    public static DependencyProperty TextFontSizeProperty = DependencyProperty.Register(nameof (TextFontSize), typeof (Decimal), typeof (NumberBox));
    private string _stringValue;
    internal TextBox Box;
    private bool _contentLoaded;

    private static void textChangedCallBack(
      DependencyObject property,
      DependencyPropertyChangedEventArgs args)
    {
      NumberBox numberBox = (NumberBox) property;
      object newValue = args.NewValue;
      string empty = string.Empty;
      if (newValue != null)
        empty = newValue.ToString();
      numberBox.Box.Text = empty;
    }

    public NumberBox()
    {
      this.InitializeComponent();
      this.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.NumberBox_PreviewMouseLeftButtonUp);
      this.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.NumberBox_LostKeyboardFocus);
      this.GotFocus += (RoutedEventHandler) ((s, e) => this.Box.Focus());
    }

    private void NumberBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      this.DoSelectAll = true;
    }

    private void NumberBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.IsFocused || !this.DoSelectAll)
        return;
      this.DoSelectAll = false;
      this.Box.SelectAll();
    }

    public int? DecimalPlaces
    {
      get => (int?) this.GetValue(NumberBox.DecimalPlacesProperty);
      set => this.SetValue(NumberBox.DecimalPlacesProperty, (object) value);
    }

    public Decimal TextFontSize
    {
      get => (Decimal) this.GetValue(NumberBox.TextFontSizeProperty);
      set => this.SetValue(NumberBox.TextFontSizeProperty, (object) value);
    }

    public Decimal? Value
    {
      get => (Decimal?) this.GetValue(NumberBox.ValueProperty);
      set => this.SetValue(NumberBox.ValueProperty, (object) value);
    }

    public string StringValue
    {
      get
      {
        if (this._stringValue == null)
        {
          if (!this.Value.HasValue)
          {
            this._stringValue = string.Empty;
          }
          else
          {
            Decimal? nullable = this.Value;
            ref Decimal? local = ref nullable;
            this._stringValue = (local.HasValue ? local.GetValueOrDefault().ToString("N" + this.DecimalPlaces.ToString()) : (string) null) ?? "0";
          }
        }
        return this._stringValue;
      }
      set
      {
        this._stringValue = value;
        if (this._stringValue.IsNullOrEmpty())
        {
          this.Value = new Decimal?(0M);
        }
        else
        {
          Decimal result = 0M;
          if (!Decimal.TryParse(value, out result))
            return;
          this.Value = new Decimal?(Math.Round(result, this.DecimalPlaces.GetValueOrDefault(3)));
        }
      }
    }

    private void Box_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
      int num1 = e.Key.IsEither<Key>(Key.Decimal, Key.OemPeriod, Key.Oem2) ? 1 : 0;
      string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      if (num1 != 0)
      {
        if (!this._stringValue.Contains(decimalSeparator))
        {
          int? decimalPlaces = this.DecimalPlaces;
          int num2 = 0;
          if (!(decimalPlaces.GetValueOrDefault() == num2 & decimalPlaces.HasValue))
          {
            e.Handled = true;
            new InputSimulator().Keyboard.TextEntry(decimalSeparator);
            return;
          }
        }
        e.Handled = true;
      }
      else
      {
        bool flag1 = e.Key >= Key.D0 && e.Key <= Key.D9 && !e.KeyboardDevice.IsKeyDown(Key.LeftShift) && !e.KeyboardDevice.IsKeyDown(Key.RightShift);
        bool flag2 = e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9;
        bool flag3 = e.Key.IsEither<Key>(Key.OemMinus, Key.OemPlus, Key.Subtract, Key.Add);
        bool flag4 = e.Key.IsEither<Key>(Key.Delete, Key.Back, Key.Left, Key.Right, Key.Down, Key.Up, Key.Return, Key.Return, Key.Home, Key.End, Key.Tab, Key.LeftCtrl, Key.RightCtrl, Key.C, Key.X, Key.Z, Key.A);
        if (!(flag1 | flag2 | flag3 | flag4))
          e.Handled = true;
        else if (flag3 && (this._stringValue.Contains<char>('+') || this._stringValue.Contains<char>('-')))
        {
          e.Handled = true;
        }
        else
        {
          int caretIndex = this.Box.CaretIndex;
          int num3 = this._stringValue.IndexOf(decimalSeparator);
          int num4 = 0;
          if (num3 != -1)
            num4 = this._stringValue.Substring(num3 + 1).Length;
          if (flag1 | flag2 && num3 != -1)
          {
            int? decimalPlaces = this.DecimalPlaces;
            if (decimalPlaces.HasValue && caretIndex > num3)
            {
              int num5 = num4;
              decimalPlaces = this.DecimalPlaces;
              int valueOrDefault = decimalPlaces.GetValueOrDefault();
              if (num5 >= valueOrDefault & decimalPlaces.HasValue && this.Box.SelectionLength == 0)
              {
                e.Handled = true;
                return;
              }
            }
          }
          Decimal? nullable1;
          if (e.Key == Key.Up)
          {
            Decimal? nullable2;
            if (this.Value.HasValue)
            {
              nullable1 = this.Value;
              Decimal num6 = (Decimal) 1;
              nullable2 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault() + num6) : new Decimal?();
            }
            else
              nullable2 = new Decimal?((Decimal) 1);
            this.Value = nullable2;
          }
          if (e.Key != Key.Down)
            return;
          nullable1 = this.Value;
          Decimal? nullable3;
          if (nullable1.HasValue)
          {
            nullable1 = this.Value;
            Decimal num7 = 0M;
            if (!(nullable1.GetValueOrDefault() == num7 & nullable1.HasValue))
            {
              nullable1 = this.Value;
              Decimal num8 = (Decimal) 1;
              nullable3 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault() - num8) : new Decimal?();
              goto label_25;
            }
          }
          nullable3 = new Decimal?(0M);
label_25:
          this.Value = nullable3;
        }
      }
    }

    private void Box_OnTextChanged(object sender, TextChangedEventArgs e)
    {
    }

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      throw new NotImplementedException();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/numberbox.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.CommandBinding_OnExecuted);
        else
          this._contentLoaded = true;
      }
      else
      {
        this.Box = (TextBox) target;
        this.Box.TextChanged += new TextChangedEventHandler(this.Box_OnTextChanged);
        this.Box.PreviewKeyDown += new KeyEventHandler(this.Box_OnPreviewKeyDown);
      }
    }
  }
}
