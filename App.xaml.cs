// Decompiled with JetBrains decompiler
// Type: Gbs.App
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Gbs
{
  public partial class App : Application
  {
    private bool _contentLoaded;

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      this.ChangeSkin(new ConfigsRepository<Settings>().Get().Interface.Theme);
    }

    public void ChangeSkin(GlobalDictionaries.Skin newSkin)
    {
      this.Resources.Clear();
      this.Resources.MergedDictionaries.Clear();
      this.LoadTheme(newSkin.ToString());
      this.LoadSharedSources();
      this.UpdateColors();
    }

    public void UpdateColors()
    {
      try
      {
        this.Resources.Clear();
        this.LoadSharedSources();
        this.UpdateBackground();
        this.UpdateSelectionColor();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось обновить цвета");
      }
    }

    private void UpdateSelectionColor()
    {
      string selectionColor = new ConfigsRepository<Settings>().Get().Interface.SelectionColor;
      if (selectionColor.IsNullOrEmpty())
        return;
      System.Drawing.Color color1 = ColorTranslator.FromHtml(selectionColor);
      if (color1 == System.Drawing.Color.Transparent || color1.A == (byte) 0)
        return;
      System.Windows.Media.Color color2 = System.Windows.Media.Color.FromArgb(color1.A, color1.R, color1.G, color1.B);
      Application.Current.Resources[(object) "SelectionBackground"] = (object) new SolidColorBrush(color2);
      Application.Current.Resources[(object) "SelectionColor"] = (object) color2;
    }

    private void UpdateBackground()
    {
      string backgroundColor = new ConfigsRepository<Settings>().Get().Interface.BackgroundColor;
      if (backgroundColor.IsNullOrEmpty())
        return;
      System.Drawing.Color color1 = ColorTranslator.FromHtml(backgroundColor);
      if (color1 == System.Drawing.Color.Transparent || color1.A == (byte) 0)
        return;
      System.Windows.Media.Color color2 = ((SolidColorBrush) Application.Current.Resources[(object) "DefaultForeground"]).Color;
      if (Math.Abs(System.Drawing.Color.FromArgb((int) color2.A, (int) color2.R, (int) color2.G, (int) color2.B).RGB2HSL().L - color1.RGB2HSL().L) < 0.2)
        color1 = color1.InvertBrightness();
      Application.Current.Resources[(object) "WindowsBackground"] = (object) new SolidColorBrush(System.Windows.Media.Color.FromArgb(color1.A, color1.R, color1.G, color1.B));
    }

    private void LoadTheme(string themeName)
    {
      this.Resources.MergedDictionaries.Add(Application.LoadComponent(new Uri("/Market;component/Resources/Styles/Themes/" + themeName + ".xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary);
    }

    private void LoadSharedSources()
    {
      foreach (string uriString in new List<string>()
      {
        "/Market;component/Resources/Styles/Controls/AllControls.xaml"
      })
        this.Resources.MergedDictionaries.Add(Application.LoadComponent(new Uri(uriString, UriKind.RelativeOrAbsolute)) as ResourceDictionary);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      this.StartupUri = new Uri("Forms\\Main\\FrmSplashScreen.xaml", UriKind.Relative);
      Application.LoadComponent((object) this, new Uri("/Market;component/app.xaml", UriKind.Relative));
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
