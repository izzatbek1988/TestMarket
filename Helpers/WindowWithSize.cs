// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WindowWithSize
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers.Cache;
using Gbs.Helpers.ControlsHelpers.Windows.Layout;
using Gbs.Helpers.Logging;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using WindowsInput;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Helpers
{
  public class WindowWithSize : Window
  {
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 524288;
    protected bool IsNonCancelButton;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    public static string SelectionForeground { get; set; }

    public static string DefaultForeground { get; set; }

    public LayoutHelper LayoutHelper { get; set; }

    public static bool IsCancel { get; set; } = true;

    public bool IaActiveBlockPress { get; set; }

    public static IEnumerable<Gbs.Core.Entities.Users.User> ListOnlineUser
    {
      get
      {
        return CachesBox.AllUsers().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.OnlineOnSectionUid == Sections.GetCurrentSection().Uid));
      }
    }

    public static bool IsVisibilityStock
    {
      get
      {
        using (Gbs.Core.Db.DataBase db = Gbs.Core.Data.GetDataBase())
          return WindowWithSize.ListOnlineUser.All<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => new UsersRepository(db).GetAccess(x, Actions.ViewStock)));
      }
    }

    private DateTime TimePress { get; set; } = DateTime.Now;

    public Dictionary<HotKeysHelper.Hotkey, ICommand> HotKeysDictionary { get; set; } = new Dictionary<HotKeysHelper.Hotkey, ICommand>();

    public WindowWithSize()
    {
      this.LayoutHelper = new LayoutHelper((Window) this);
      this.Closed += new EventHandler(this.WindowWithSize_Closed);
      this.Closing += new CancelEventHandler(this.WindowWithSize_Closing);
      this.Initialized += (EventHandler) ((sender, args) => this.LayoutHelper.LoadWindowsSize());
      this.Loaded += (RoutedEventHandler) ((sender, args) =>
      {
        this.LayoutHelper.LoadOption();
        this.LayoutHelper.LoadSort();
      });
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.PreviewKeyDown += new KeyEventHandler(this.OnKeyDown);
      this.PreviewKeyDown += new KeyEventHandler(this.WindowWithSize_PreviewKeyDown);
      this.TextInput += new TextCompositionEventHandler(this.OnTextInput);
      if (!(this is FrmKeyboard))
      {
        WindowWithSize.SelectionForeground = this.TryFindResource((object) nameof (SelectionForeground)).ToString();
        WindowWithSize.DefaultForeground = this.TryFindResource((object) nameof (DefaultForeground)).ToString();
        this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
      }
      this.PreviewGotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.WindowWithSize_PreviewGotKeyboardFocus);
      this.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.WindowWithSize_PreviewMouseLeftButtonUp);
      this.Activated += new EventHandler(this.WindowWithSize_Activated);
    }

    private void WindowWithSize_Activated(object sender, EventArgs e)
    {
      if (this is FrmKeyboard)
        return;
      this.Focus();
    }

    public void WindowWithSize_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      object source = e.Source;
      if (source is System.Windows.Controls.Frame)
        source = e.OriginalSource;
      string str = "type: " + source.GetType().Name;
      if (source is Control control)
        str = str + ", name: " + control.Name;
      LogHelper.Trace("Клик мышью в форме. Title: " + this.Title + "; Type: " + this.GetType().Name + "; source: " + str + "; ");
      this.ShowKeyboard(source);
    }

    private void WindowWithSize_PreviewGotKeyboardFocus(
      object sender,
      KeyboardFocusChangedEventArgs e)
    {
      object source = e.Source;
      if (source is System.Windows.Controls.Frame)
        source = e.OriginalSource;
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.VirtualKeyboard.ActivateOnlyByClick)
        return;
      this.ShowKeyboard(source, false);
    }

    private void ShowKeyboard(object source, bool onClick = true)
    {
      try
      {
        if (!new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.VirtualKeyboard.IsEnabled || this is FrmKeyboard)
          return;
        switch (source)
        {
          case null:
            break;
          case WatermarkTextBox _:
          case TextBox _:
          case TextBoxWithClearControl _:
          case DecimalUpDown _:
          case IntegerUpDown _:
label_7:
            int num;
            switch (source)
            {
              case TextBox textBox when textBox.IsReadOnly:
              case DecimalUpDown decimalUpDown when decimalUpDown.IsReadOnly:
              case IntegerUpDown integerUpDown when integerUpDown.IsReadOnly:
                if (!onClick)
                  return;
                FrmKeyboard.CloseKeyboard();
                return;
              case DecimalUpDown _:
              case IntegerUpDown _:
                num = 1;
                break;
              default:
                num = source is NumberBox ? 1 : 0;
                break;
            }
            FrmKeyboard.ShowKeyboard(num != 0);
            switch (source)
            {
              case DecimalUpDown _:
              case IntegerUpDown _:
              case NumberBox _:
                FrmKeyboard.SelectAllInControl((Control) source);
                return;
              default:
                return;
            }
          default:
            if (!(source.GetType().Name == "TextBoxView") && !(source is NumberBox))
            {
              if (!onClick)
                break;
              FrmKeyboard.CloseKeyboard();
              break;
            }
            goto label_7;
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void WindowWithSize_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Source is DecimalUpDown || e.Source is IntegerUpDown)
      {
        WindowWithSize.NumericBoxCheck(e);
        if (e.Handled)
          return;
      }
      if (e.Key.IsEither<Key>(Key.LeftCtrl, Key.RightCtrl, Key.LeftShift, Key.RightShift, Key.LeftAlt, Key.RightAlt))
        return;
      using (IEnumerator<KeyValuePair<HotKeysHelper.Hotkey, ICommand>> enumerator = this.HotKeysDictionary.Where<KeyValuePair<HotKeysHelper.Hotkey, ICommand>>((Func<KeyValuePair<HotKeysHelper.Hotkey, ICommand>, bool>) (key => key.Key.IsMatch(e))).GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        KeyValuePair<HotKeysHelper.Hotkey, ICommand> current = enumerator.Current;
        HotKeysHelper.DoHotKey(e, current.Value);
      }
    }

    public static void NumericBoxCheck(KeyEventArgs e)
    {
      int num = e.Key.IsEither<Key>(Key.Decimal, Key.OemPeriod, Key.Oem2) ? 1 : 0;
      string decimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      string str;
      switch (e.Source)
      {
        case DecimalUpDown decimalUpDown:
          str = decimalUpDown.Text;
          break;
        case IntegerUpDown integerUpDown:
          str = integerUpDown.Text;
          break;
        default:
          str = string.Empty;
          break;
      }
      string source = str;
      if (num != 0)
      {
        if (source.Contains(decimalSeparator))
        {
          e.Handled = true;
        }
        else
        {
          e.Handled = true;
          new InputSimulator().Keyboard.TextEntry(decimalSeparator);
        }
      }
      else
      {
        bool flag1 = e.Key >= Key.D0 && e.Key <= Key.D9 && !e.KeyboardDevice.IsKeyDown(Key.LeftShift) && !e.KeyboardDevice.IsKeyDown(Key.RightShift);
        bool flag2 = e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9;
        bool flag3 = e.Key.IsEither<Key>(Key.OemMinus, Key.OemPlus, Key.Subtract, Key.Add);
        bool flag4 = e.Key.IsEither<Key>(Key.Delete, Key.Back, Key.Left, Key.Right, Key.Down, Key.Up, Key.Return, Key.Return, Key.Home, Key.End, Key.Tab, Key.LeftCtrl, Key.RightCtrl, Key.V, Key.C, Key.X, Key.Z);
        if (!(flag1 | flag2 | flag3 | flag4))
        {
          e.Handled = true;
        }
        else
        {
          if (!flag3 || !source.Contains<char>('+') && !source.Contains<char>('-'))
            return;
          e.Handled = true;
        }
      }
    }

    private static int GetDecimalDigitsCount(string text)
    {
      if (!Decimal.TryParse(text, out Decimal _))
        return -1;
      string[] strArray = Convert.ToDecimal(text).ToString((IFormatProvider) new NumberFormatInfo()
      {
        NumberDecimalSeparator = "."
      }).Split('.');
      return strArray.Length == 1 || strArray[1].All<char>((Func<char, bool>) (x => x == '0')) || strArray.Length != 2 ? 0 : strArray[1].Length;
    }

    private void WindowWithSize_Closed(object sender, EventArgs e)
    {
    }

    protected TextBoxWithClearControl SearchTextBox { get; set; }

    protected ICommand CommandEnter { get; set; }

    protected object Parameter { get; set; }

    protected Control Object { private get; set; }

    protected Func<bool> QuestionCloseAction { get; set; }

    private void OnTextInput(object sender, TextCompositionEventArgs e)
    {
      try
      {
        if (this.SearchTextBox == null)
          return;
        if (e.Text == "\b")
        {
          int length = this.SearchTextBox.TextString.Length - 1;
          if (length > 0)
            this.SearchTextBox.TextString = this.SearchTextBox.TextString.Substring(0, length);
          else if (length == 0)
            this.SearchTextBox.SearchTextBox.Clear();
        }
        else
        {
          char result;
          if (char.TryParse(e.Text, out result) && (char.IsLetterOrDigit(result) || char.IsPunctuation(result) || char.IsSymbol(result) || result == ' '))
            this.SearchTextBox.TextString += e.Text;
        }
        e.Handled = true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при обработке ввода текста в форме", false);
      }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (this.IaActiveBlockPress)
      {
        DateTime now = DateTime.Now;
        if ((now - this.TimePress).TotalMilliseconds < 200.0 && (e.Key == Key.Return || e.Key == Key.Tab))
        {
          LogHelper.Trace("Блокирую суффикс при нажатии клавиши");
          e.Handled = true;
          return;
        }
        if (!e.Key.IsEither<Key>(Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.LeftShift, Key.RightShift, Key.LWin, Key.RWin, Key.Clear, Key.OemClear, Key.Apps, Key.Return, Key.Return, Key.Tab))
          this.TimePress = now;
      }
      IInputElement focusedElement = Keyboard.FocusedElement;
      if (this.Object == null && this.SearchTextBox == null)
        return;
      TextBoxWithClearControl searchTextBox = this.SearchTextBox;
      int num1;
      if (searchTextBox == null)
      {
        num1 = 0;
      }
      else
      {
        int num2 = searchTextBox.SearchTextBox.IsFocused ? 1 : 0;
        num1 = 1;
      }
      if ((num1 != 0 ? (this.SearchTextBox.SearchTextBox.IsFocused ? 1 : 0) : (object.Equals((object) focusedElement, (object) this) ? 1 : 0)) != 0 || object.Equals((object) focusedElement, (object) this))
      {
        if (!e.Key.IsEither<Key>(Key.Up, Key.Down))
          return;
        e.Handled = true;
        if (this.Object is System.Windows.Controls.DataGrid)
          Gbs.Helpers.ControlsHelpers.DataGrid.Other.FocusRow((System.Windows.Controls.DataGrid) this.Object, e.Key == Key.Up);
        else
          Gbs.Helpers.ControlsHelpers.DataGrid.Other.SelectFirstRow((object) this.Object);
      }
      else
      {
        if (this.SearchTextBox != null && e.Key == Key.Space)
        {
          this.SearchTextBox.TextString += " ";
          e.Handled = true;
        }
        if (e.Key != Key.Return)
          return;
        object parameter = !(this.Object.GetType() == typeof (System.Windows.Controls.DataGrid)) || this.Parameter != null ? this.Parameter : (object) ((MultiSelector) this.Object).SelectedItems;
        if (this.CommandEnter == null)
          return;
        this.CommandEnter.Execute(parameter);
        e.Handled = true;
      }
    }

    protected void OnLoaded(object sender, RoutedEventArgs e)
    {
      LogHelper.Debug("Инициализация формы. Title: " + this.Title + "; " + this.GetType().Name);
      LogHelper.Trace("Thread id: " + Thread.CurrentThread.ManagedThreadId.ToString());
      if (this.IsNonCancelButton)
      {
        IntPtr handle = new WindowInteropHelper((Window) this).Handle;
        WindowWithSize.SetWindowLong(handle, -16, WindowWithSize.GetWindowLong(handle, -16) & -524289);
      }
      if (this is FrmKeyboard)
        return;
      double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
      double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
      double width = this.Width;
      double height = this.Height;
      this.Left = primaryScreenWidth / 2.0 - width / 2.0;
      this.Top = primaryScreenHeight / 2.0 - height / 2.0;
    }

    public bool IsMainForm { get; set; } = true;

    private void WindowWithSize_Closing(object sender, CancelEventArgs e)
    {
      if (this.IsMainForm)
        this.LayoutHelper.UpdateOption();
      if (this.QuestionCloseAction != null && WindowWithSize.IsCancel)
        e.Cancel = !this.QuestionCloseAction();
      LogHelper.Debug("Закрываем форму. Title: " + this.Title + "; " + this.GetType().Name);
      WindowWithSize.IsCancel = true;
    }

    public void UpdateColumnStock(
      System.Windows.Controls.DataGrid grid,
      out Visibility visibilityStock,
      ContextMenu contextMenu = null)
    {
      if (!WindowWithSize.IsVisibilityStock)
      {
        grid.Columns.Remove(grid.Columns.FirstOrDefault<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "3D205A8F-AD38-489A-BF93-3C3498CF57BB".ToString())));
        MenuItem removeItem = contextMenu != null ? contextMenu.Items.Cast<MenuItem>().FirstOrDefault<MenuItem>((Func<MenuItem, bool>) (x => x.Uid == "3D205A8F-AD38-489A-BF93-3C3498CF57BB")) : (MenuItem) null;
        if (removeItem != null)
          contextMenu.Items.Remove((object) removeItem);
        visibilityStock = Visibility.Collapsed;
      }
      else
      {
        if (!grid.Columns.Any<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "3D205A8F-AD38-489A-BF93-3C3498CF57BB")))
        {
          DataGridTextColumn dataGridTextColumn1 = new DataGridTextColumn();
          dataGridTextColumn1.Header = (object) Translate.FrmSelectGoodStock_Остаток;
          dataGridTextColumn1.Width = new DataGridLength(100.0);
          dataGridTextColumn1.CellStyle = (Style) this.FindResource((object) "numberCellStyle");
          Binding binding = new Binding("TotalSumStock");
          binding.StringFormat = "N2";
          dataGridTextColumn1.Binding = (BindingBase) binding;
          DataGridTextColumn dataGridTextColumn2 = dataGridTextColumn1;
          Gbs.Helpers.Extensions.UIElement.Extensions.SetGuid((DependencyObject) dataGridTextColumn2, "3D205A8F-AD38-489A-BF93-3C3498CF57BB");
          grid.Columns.Add((DataGridColumn) dataGridTextColumn2);
          this.LayoutHelper.LoadColumn(grid, dataGridTextColumn2);
          if (contextMenu != null)
          {
            ItemCollection items = contextMenu.Items;
            MenuItem newItem = new MenuItem();
            newItem.Header = dataGridTextColumn2.Header;
            newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) dataGridTextColumn2);
            newItem.IsCheckable = true;
            newItem.IsChecked = dataGridTextColumn2.Visibility == Visibility.Visible;
            items.Add((object) newItem);
          }
        }
        visibilityStock = Visibility.Visible;
      }
    }
  }
}
