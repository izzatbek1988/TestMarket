// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.ConfirmPanelControl1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class ConfirmPanelControl1 : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty CancelTextProperty = DependencyProperty.Register(nameof (CancelButtonText), typeof (string), typeof (ConfirmPanelControl1), new PropertyMetadata((object) Translate.FrmClientCard_ОТМЕНА));
    public static readonly DependencyProperty OkButtonTextProperty = DependencyProperty.Register(nameof (OkButtonText), typeof (string), typeof (ConfirmPanelControl1), new PropertyMetadata((object) Translate.FrmClientCard_СОХРАНИТЬ));
    public static readonly DependencyProperty CancelButtonCommandProperty = DependencyProperty.Register(nameof (CancelButtonCommand), typeof (ICommand), typeof (ConfirmPanelControl1), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OkButtonCommandProperty = DependencyProperty.Register(nameof (OkButtonCommand), typeof (ICommand), typeof (ConfirmPanelControl1), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OkButtonParameterProperty = DependencyProperty.Register(nameof (OkButtonParameter), typeof (object), typeof (ConfirmPanelControl1), new PropertyMetadata((object) null));
    public static readonly DependencyProperty IsEnabledOkButtonProperty = DependencyProperty.Register(nameof (IsEnabledOkButton), typeof (bool), typeof (ConfirmPanelControl1), new PropertyMetadata((object) true));
    public static readonly DependencyProperty IsEnabledCancelButtonProperty = DependencyProperty.Register(nameof (IsEnabledCancelButton), typeof (bool), typeof (ConfirmPanelControl1), new PropertyMetadata((object) true));
    public static readonly DependencyProperty VisibilityOkButtonProperty = DependencyProperty.Register(nameof (VisibilityOkButton), typeof (Visibility), typeof (ConfirmPanelControl1), new PropertyMetadata((object) Visibility.Visible));
    public static readonly DependencyProperty VisibilityCancelButtonProperty = DependencyProperty.Register(nameof (VisibilityCancelButton), typeof (Visibility), typeof (ConfirmPanelControl1), new PropertyMetadata((object) Visibility.Visible));
    internal Button ButtonCancel;
    internal Button ButtonOk;
    private bool _contentLoaded;

    public string CancelButtonText
    {
      get => (string) this.GetValue(ConfirmPanelControl1.CancelTextProperty);
      set => this.SetValue(ConfirmPanelControl1.CancelTextProperty, (object) value);
    }

    public string OkButtonText
    {
      get => (string) this.GetValue(ConfirmPanelControl1.OkButtonTextProperty);
      set => this.SetValue(ConfirmPanelControl1.OkButtonTextProperty, (object) value);
    }

    public ICommand CancelButtonCommand
    {
      get => (ICommand) this.GetValue(ConfirmPanelControl1.CancelButtonCommandProperty);
      set => this.SetValue(ConfirmPanelControl1.CancelButtonCommandProperty, (object) value);
    }

    public ICommand OkButtonCommand
    {
      get => (ICommand) this.GetValue(ConfirmPanelControl1.OkButtonCommandProperty);
      set => this.SetValue(ConfirmPanelControl1.OkButtonCommandProperty, (object) value);
    }

    public object OkButtonParameter
    {
      get => this.GetValue(ConfirmPanelControl1.OkButtonParameterProperty);
      set => this.SetValue(ConfirmPanelControl1.OkButtonParameterProperty, value);
    }

    public bool IsEnabledOkButton
    {
      get => (bool) this.GetValue(ConfirmPanelControl1.IsEnabledOkButtonProperty);
      set => this.SetValue(ConfirmPanelControl1.IsEnabledOkButtonProperty, (object) value);
    }

    public bool IsEnabledCancelButton
    {
      get => (bool) this.GetValue(ConfirmPanelControl1.IsEnabledCancelButtonProperty);
      set => this.SetValue(ConfirmPanelControl1.IsEnabledCancelButtonProperty, (object) value);
    }

    public Visibility VisibilityCancelButton
    {
      get => (Visibility) this.GetValue(ConfirmPanelControl1.VisibilityCancelButtonProperty);
      set => this.SetValue(ConfirmPanelControl1.VisibilityCancelButtonProperty, (object) value);
    }

    public Visibility VisibilityOkButton
    {
      get => (Visibility) this.GetValue(ConfirmPanelControl1.VisibilityOkButtonProperty);
      set => this.SetValue(ConfirmPanelControl1.VisibilityOkButtonProperty, (object) value);
    }

    public ConfirmPanelControl1() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/confirmpanelcontrol1.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.ButtonOk = (Button) target;
        else
          this._contentLoaded = true;
      }
      else
        this.ButtonCancel = (Button) target;
    }
  }
}
