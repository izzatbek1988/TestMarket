// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Other.MessageBox
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using WindowsInput;
using WindowsInput.Native;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Other
{
  public class MessageBox : WindowWithSize, IComponentConnector
  {
    private int _commandIndex = -1;
    internal Grid ColorGrid;
    internal WatermarkTextBox TextBox;
    internal ItemsControl tStack;
    internal StackPanel StackPanelButton;
    internal Button ButtonOk;
    internal Button ButtonYes;
    internal Button ButtonNo;
    internal Button ButtonCancel;
    private bool _contentLoaded;

    private MessBoxViewModel Model { get; set; }

    public MessageBox() => this.InitializeComponent();

    public (MessageBoxResult, string) ShowInput(
      string inputSting,
      string text,
      int inputMinLength,
      string title = "")
    {
      if (title.Length == 0)
        title = PartnersHelper.ProgramName();
      MessBoxViewModel messBoxViewModel = new MessBoxViewModel();
      messBoxViewModel.Text = text;
      messBoxViewModel.Title = title;
      messBoxViewModel.CloseAction = new Action(((Window) this).Close);
      messBoxViewModel.OkVisibility = Visibility.Visible;
      messBoxViewModel.CancelVisibility = Visibility.Visible;
      messBoxViewModel.InputVisibility = Visibility.Visible;
      messBoxViewModel.InputString = inputSting;
      messBoxViewModel.InputMinLength = inputMinLength;
      messBoxViewModel.SetInputFocus = new Action(this.SetInputFocus);
      this.Model = messBoxViewModel;
      this.TextBox.Focus();
      new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.SetGsHotKey();
      new MessageBox.Sounder().Play(MessageBox.Sounder.SoundType.Question);
      this.PreviewKeyDown += new KeyEventHandler(this.MessageBox_PreviewKeyDown);
      this.DataContext = (object) this.Model;
      this.Loaded += new RoutedEventHandler(this.MessageBox_Loaded);
      this.ShowDialog();
      return (this.Model.Result, this.Model.InputString);
    }

    private void SetGsHotKey()
    {
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.GsCodeHotKey,
          (ICommand) new RelayCommand((Action<object>) (o => new InputSimulator().Keyboard.KeyPress(VirtualKeyCode.SPACE)))
        }
      };
    }

    private void SetInputFocus()
    {
      this.TextBox.Focus();
      this.TextBox.SelectionStart = this.TextBox.Text.Length;
    }

    private void MessageBox_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.TextBox.Visibility != Visibility.Visible)
        return;
      this.SetInputFocus();
    }

    private void MessageBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (this.TextBox.IsFocused)
      {
        MessBoxViewModel model = this.Model;
        model._pushedKeys = model._pushedKeys + " " + e.Key.ToString();
      }
      if (!this.TextBox.IsFocused || e.Key != Key.Return || this.TextBox.Visibility != Visibility.Visible || Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        return;
      this.Model.OkCommand.Execute((object) this.TextBox.Text);
      e.Handled = true;
    }

    public MessageBox.MsgBoxResult Show(
      string text,
      string title = "",
      MessageBoxButton button = MessageBoxButton.OK,
      MessageBoxImage image = MessageBoxImage.Asterisk,
      Dictionary<int, string> buttons = null,
      List<MessBoxViewModel.CheckboxItem> checkboxes = null)
    {
      Gbs.Helpers.Other.ConsoleWrite("MessageBox.Show");
      if (title.Length == 0)
        title = PartnersHelper.ProgramName();
      MessBoxViewModel messBoxViewModel = new MessBoxViewModel();
      messBoxViewModel.Text = text;
      messBoxViewModel.Title = title;
      messBoxViewModel.CloseAction = new Action(((Window) this).Close);
      messBoxViewModel.BottomBorderVisibility = Visibility.Visible;
      this.Model = messBoxViewModel;
      if (buttons != null)
      {
        this.Model.CommandsList = new ObservableCollection<MessBoxViewModel.CommandItem>();
        foreach (KeyValuePair<int, string> button1 in buttons)
        {
          KeyValuePair<int, string> b = button1;
          this.Model.CommandsList.Add(new MessBoxViewModel.CommandItem(b.Value, (ICommand) new RelayCommand((Action<object>) (_ =>
          {
            this._commandIndex = b.Key;
            this.Model.Result = MessageBoxResult.OK;
            this.Close();
          }))));
        }
        this.Model.CancelVisibility = Visibility.Visible;
      }
      else if (checkboxes != null)
      {
        this.Model.CheckboxesList = new ObservableCollection<MessBoxViewModel.CheckboxItem>(checkboxes);
        this.Model.OkVisibility = Visibility.Visible;
        this.Model.CancelVisibility = Visibility.Visible;
      }
      else
      {
        switch (button)
        {
          case MessageBoxButton.OK:
            this.Model.OkVisibility = Visibility.Visible;
            this.ButtonOk.Focus();
            break;
          case MessageBoxButton.OKCancel:
            this.Model.OkVisibility = Visibility.Visible;
            this.Model.CancelVisibility = Visibility.Visible;
            this.ButtonOk.Focus();
            break;
          case MessageBoxButton.YesNoCancel:
            this.Model.NoVisibility = Visibility.Visible;
            this.Model.YesVisibility = Visibility.Visible;
            this.Model.CancelVisibility = Visibility.Visible;
            this.ButtonYes.Focus();
            break;
          case MessageBoxButton.YesNo:
            this.Model.NoVisibility = Visibility.Visible;
            this.Model.YesVisibility = Visibility.Visible;
            this.ButtonYes.Focus();
            break;
          default:
            this.Model.OkVisibility = this.Visibility;
            this.ButtonOk.Focus();
            break;
        }
      }
      System.Drawing.Color color = ColorTranslator.FromHtml(this.TryFindResource((object) "DefaultForegroundColor").ToString());
      this.Model.Color = new SolidColorBrush(new System.Windows.Media.Color()
      {
        A = color.A,
        R = color.R,
        G = color.G,
        B = color.B
      });
      MessageBox.Sounder sounder = new MessageBox.Sounder();
      switch (image)
      {
        case MessageBoxImage.Hand:
          this.Model.Color = System.Windows.Media.Brushes.Red;
          this.Model.IconErrorVisibility = Visibility.Visible;
          sounder.Play(MessageBox.Sounder.SoundType.Error);
          break;
        case MessageBoxImage.Question:
          this.Model.Color = System.Windows.Media.Brushes.DeepSkyBlue;
          this.Model.IconQuestionVisibility = Visibility.Visible;
          sounder.Play(MessageBox.Sounder.SoundType.Question);
          break;
        case MessageBoxImage.Exclamation:
          this.Model.Color = System.Windows.Media.Brushes.DarkOrange;
          this.Model.IconWarningVisibility = Visibility.Visible;
          sounder.Play(MessageBox.Sounder.SoundType.Warning);
          break;
        case MessageBoxImage.Asterisk:
          this.Model.IconInfoVisibility = Visibility.Visible;
          sounder.Play(MessageBox.Sounder.SoundType.Info);
          break;
      }
      this.DataContext = (object) this.Model;
      this.Topmost = true;
      this.ShowDialog();
      MessageBox.MsgBoxResult msgBoxResult = new MessageBox.MsgBoxResult();
      msgBoxResult.Result = this.Model.Result;
      msgBoxResult.SelectedIndex = this._commandIndex;
      ObservableCollection<MessBoxViewModel.CheckboxItem> checkboxesList = this.Model.CheckboxesList;
      msgBoxResult.Checkboxes = checkboxesList != null ? checkboxesList.ToList<MessBoxViewModel.CheckboxItem>() : (List<MessBoxViewModel.CheckboxItem>) null;
      return msgBoxResult;
    }

    private void MessageBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Left)
        return;
      int key = (int) e.Key;
    }

    private void MessageBox_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != System.Windows.Input.MouseButton.Left)
        return;
      this.DragMove();
    }

    private void TextBox_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      FrmKeyboard.ShowKeyboard();
    }

    private void MessageBox_OnActivated(object sender, EventArgs e)
    {
      if (this.TextBox.Visibility != Visibility.Visible)
        return;
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.Model.ComPortScannerOnBarcodeChanged));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/other/messagebox.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.ColorGrid = (Grid) target;
          break;
        case 2:
          this.TextBox = (WatermarkTextBox) target;
          this.TextBox.PreviewMouseUp += new MouseButtonEventHandler(this.TextBox_OnPreviewMouseUp);
          break;
        case 3:
          this.tStack = (ItemsControl) target;
          break;
        case 4:
          this.StackPanelButton = (StackPanel) target;
          break;
        case 5:
          this.ButtonOk = (Button) target;
          break;
        case 6:
          this.ButtonYes = (Button) target;
          break;
        case 7:
          this.ButtonNo = (Button) target;
          break;
        case 8:
          this.ButtonCancel = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    public class MsgBoxResult
    {
      public MessageBoxResult Result { get; set; } = MessageBoxResult.Cancel;

      public int SelectedIndex { get; set; } = -1;

      public List<MessBoxViewModel.CheckboxItem> Checkboxes { get; set; }
    }

    private class Sounder
    {
      public void PlayFromResource(MessageBox.Sounder.SoundType type)
      {
        try
        {
          Stream stream = (Stream) null;
          switch (type)
          {
            case MessageBox.Sounder.SoundType.Info:
              stream = (Stream) Gbs.Properties.Resources.info;
              break;
            case MessageBox.Sounder.SoundType.Question:
              stream = (Stream) Gbs.Properties.Resources.question;
              break;
            case MessageBox.Sounder.SoundType.Warning:
              stream = (Stream) Gbs.Properties.Resources.warning;
              break;
            case MessageBox.Sounder.SoundType.Error:
              stream = (Stream) Gbs.Properties.Resources.error;
              break;
          }
          if (stream == null)
            return;
          SoundPlayerHelper.PlaySound(stream);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка воспроизведения звука");
        }
      }

      public void Play(MessageBox.Sounder.SoundType type)
      {
        if (!new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.IsPlaySoundsForEvents)
          return;
        TaskHelper.TaskRun((Action) (() => this.PlayFromResource(type)));
      }

      public enum SoundType
      {
        Info,
        Question,
        Warning,
        Error,
      }
    }
  }
}
