// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Other.MessBoxViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Gbs.Forms.Other
{
  public partial class MessBoxViewModel : ViewModelWithForm
  {
    private Visibility _cancelVisibility = Visibility.Collapsed;
    private SolidColorBrush _color = Brushes.DeepSkyBlue;
    private Visibility _iconErrorVisibility = Visibility.Collapsed;
    private Visibility _iconInfoVisibility = Visibility.Collapsed;
    private Visibility _iconQuestionVisibility = Visibility.Collapsed;
    private Visibility _iconWarningVisibility = Visibility.Collapsed;
    private Visibility _inputVisibility = Visibility.Collapsed;
    private Visibility _noVisibility = Visibility.Collapsed;
    private Visibility _okVisibility = Visibility.Collapsed;
    private Visibility _yesVisibility = Visibility.Collapsed;
    public string _pushedKeys = string.Empty;
    private string _inputString;

    public Visibility IconQuestionVisibility
    {
      get => this._iconQuestionVisibility;
      set
      {
        this._iconQuestionVisibility = value;
        this.OnPropertyChanged(nameof (IconQuestionVisibility));
      }
    }

    public Visibility IconWarningVisibility
    {
      get => this._iconWarningVisibility;
      set
      {
        this._iconWarningVisibility = value;
        this.OnPropertyChanged(nameof (IconWarningVisibility));
      }
    }

    public Visibility IconErrorVisibility
    {
      get => this._iconErrorVisibility;
      set
      {
        this._iconErrorVisibility = value;
        this.OnPropertyChanged(nameof (IconErrorVisibility));
      }
    }

    public Visibility IconInfoVisibility
    {
      get => this._iconInfoVisibility;
      set
      {
        this._iconInfoVisibility = value;
        this.OnPropertyChanged(nameof (IconInfoVisibility));
      }
    }

    public SolidColorBrush Color
    {
      get => this._color;
      set
      {
        this._color = value;
        this.OnPropertyChanged(nameof (Color));
      }
    }

    public string InputString
    {
      get => this._inputString;
      set
      {
        this._inputString = value;
        this.OnPropertyChanged(nameof (InputString));
      }
    }

    public Visibility InputVisibility
    {
      get => this._inputVisibility;
      set
      {
        this._inputVisibility = value;
        this.OnPropertyChanged(nameof (InputVisibility));
      }
    }

    public int InputMinLength { get; set; }

    public MessageBoxResult Result { get; set; }

    public ICommand OkCommand { get; set; }

    public string AppVersion
    {
      get => ApplicationInfo.GetInstance().AppVersion;
      set => throw new NotImplementedException();
    }

    public ICommand CancelCommand { get; set; }

    public ICommand YesCommand { get; set; }

    public ICommand NoCommand { get; set; }

    public ObservableCollection<MessBoxViewModel.CheckboxItem> CheckboxesList { get; set; }

    public ObservableCollection<MessBoxViewModel.CommandItem> CommandsList { get; set; }

    public Action SetInputFocus { get; set; }

    public string Title { get; set; } = PartnersHelper.ProgramName();

    public string Text { get; set; }

    public object Image { get; set; }

    public Visibility OkVisibility
    {
      get => this._okVisibility;
      set
      {
        this._okVisibility = value;
        this.OnPropertyChanged(nameof (OkVisibility));
      }
    }

    public Visibility CancelVisibility
    {
      get => this._cancelVisibility;
      set
      {
        this._cancelVisibility = value;
        this.OnPropertyChanged(nameof (CancelVisibility));
      }
    }

    public Visibility YesVisibility
    {
      get => this._yesVisibility;
      set
      {
        this._yesVisibility = value;
        this.OnPropertyChanged(nameof (YesVisibility));
      }
    }

    public Visibility NoVisibility
    {
      get => this._noVisibility;
      set
      {
        this._noVisibility = value;
        this.OnPropertyChanged(nameof (NoVisibility));
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public ICommand CopyMessageComand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => Clipboard.SetText(this.Text + "\n" + this.AppVersion)));
      }
    }

    public Visibility CopyButtonVisibility
    {
      get => this.InputVisibility != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
    }

    public Visibility BottomBorderVisibility { get; set; }

    public MessBoxViewModel()
    {
      this.OkCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (obj != null)
          this.InputString = obj.ToString();
        if (this.InputString == null)
        {
          string empty;
          this.InputString = empty = string.Empty;
        }
        if (this.InputString.Length < this.InputMinLength)
        {
          int num = (int) MessageBoxHelper.Show(string.Format(Translate.MessBoxViewModel_Вы_не_ввели_необходимую_информацию__0_Строка_должна_длинной_не_менее__1__символов, (object) Gbs.Helpers.Other.NewLine(), (object) this.InputMinLength), icon: MessageBoxImage.Exclamation);
          this.SetInputFocus();
        }
        else
        {
          LogHelper.Trace("В msg были нажаты клавиши: " + this._pushedKeys);
          this.InputString = this.InputString.Trim();
          this.Result = MessageBoxResult.OK;
          this.CloseAction();
        }
      }));
      this.CancelCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = MessageBoxResult.Cancel;
        this.CloseAction();
      }));
      this.YesCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = MessageBoxResult.Yes;
        this.CloseAction();
      }));
      this.NoCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = MessageBoxResult.No;
        this.CloseAction();
      }));
    }

    public void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Gbs.Helpers.Other.IsActiveForm<MessageBox>())
        return;
      this.InputString = barcode.Replace("\n", "");
      if (!this.InputString.EndsWith("\r"))
        return;
      this.InputString = this.InputString.Replace("\r", "");
      Thread.Sleep(250);
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        if (this.InputString.Length <= 0)
          return;
        this.OkCommand.Execute((object) null);
      }));
    }

    public class CheckboxItem
    {
      public int Id { get; set; }

      public string Text { get; set; }

      public bool IsChecked { get; set; }

      public CheckboxItem(int id, string text, bool isChecked)
      {
        this.Id = id;
        this.Text = text;
        this.IsChecked = isChecked;
      }
    }

    public class CommandItem
    {
      public string Text { get; set; }

      public ICommand Command { get; set; }

      public CommandItem(string text, ICommand command)
      {
        this.Text = text;
        this.Command = command;
      }
    }
  }
}
