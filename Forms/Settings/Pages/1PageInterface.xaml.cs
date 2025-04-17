// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageInterface
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Tooltips;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class PageInterface : Page, IComponentConnector
  {
    internal CheckBox CheckBox_showHelpToolTips;
    private bool _contentLoaded;

    public PageInterface() => this.InitializeComponent();

    public PageInterface(Gbs.Core.Config.Settings settings)
    {
      this.InitializeComponent();
      this.DataContext = (object) new PageVisualModelView(settings);
      this.SetTooltips();
    }

    private void SetTooltips()
    {
      this.CheckBox_showHelpToolTips.SetToolTip(new HelpTip(Translate.ПоказыватьПодсказкиПриНаведении, Translate.PageInterface_SetTooltips_));
    }

    private void ColorPicker_OnSelectedColorChanged(
      object sender,
      RoutedPropertyChangedEventArgs<Color?> e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pageinterface.xaml", UriKind.Relative));
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
          ((ColorPicker) target).SelectedColorChanged += new RoutedPropertyChangedEventHandler<Color?>(this.ColorPicker_OnSelectedColorChanged);
          break;
        case 2:
          ((ColorPicker) target).SelectedColorChanged += new RoutedPropertyChangedEventHandler<Color?>(this.ColorPicker_OnSelectedColorChanged);
          break;
        case 3:
          this.CheckBox_showHelpToolTips = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
