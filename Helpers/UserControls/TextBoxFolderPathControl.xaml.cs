// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.TextBoxFolderPathControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class TextBoxFolderPathControl : System.Windows.Controls.UserControl, IComponentConnector
  {
    public static readonly DependencyProperty FolderPathFontSizeProperty = DependencyProperty.Register(nameof (FolderPathFontSize), typeof (Decimal), typeof (TextBoxFolderPathControl), new PropertyMetadata((object) 0M));
    public static readonly DependencyProperty FolderPathProperty = DependencyProperty.Register(nameof (FolderPath), typeof (string), typeof (TextBoxFolderPathControl), new PropertyMetadata((object) null));
    public static readonly DependencyProperty SelectFolderCommandProperty = DependencyProperty.Register(nameof (SelectFolderCommand), typeof (ICommand), typeof (TextBoxFolderPathControl), new PropertyMetadata((PropertyChangedCallback) null));
    internal WatermarkTextBox FolderPathTextBox;
    private bool _contentLoaded;

    public Decimal FolderPathFontSize
    {
      get => (Decimal) this.GetValue(TextBoxFolderPathControl.FolderPathFontSizeProperty);
      set => this.SetValue(TextBoxFolderPathControl.FolderPathFontSizeProperty, (object) value);
    }

    public string FolderPath
    {
      get => (string) this.GetValue(TextBoxFolderPathControl.FolderPathProperty);
      set => this.SetValue(TextBoxFolderPathControl.FolderPathProperty, (object) value);
    }

    public ICommand SelectFolderCommand
    {
      get => (ICommand) this.GetValue(TextBoxFolderPathControl.SelectFolderCommandProperty);
      set => this.SetValue(TextBoxFolderPathControl.SelectFolderCommandProperty, (object) value);
    }

    public TextBoxFolderPathControl()
    {
      this.InitializeComponent();
      if (this.SelectFolderCommand == null)
      {
        ICommand command;
        this.SelectFolderCommand = command = (ICommand) new RelayCommand((Action<object>) (_ => this.SelectFolder()));
      }
      this.GotFocus += new RoutedEventHandler(this.TextBoxWithClearControl_GotFocus);
    }

    private void SelectFolder()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.SelectedPath = this.FolderPath;
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
        return;
      this.FolderPath = folderBrowserDialog.SelectedPath;
    }

    private void TextBoxWithClearControl_GotFocus(object sender, RoutedEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/textboxfolderpathcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.FolderPathTextBox = (WatermarkTextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
