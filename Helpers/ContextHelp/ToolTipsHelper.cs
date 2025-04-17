// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ToolTipsHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Gbs.Helpers
{
  public static class ToolTipsHelper
  {
    public static bool IsToolTipsEnable()
    {
      return new ConfigsRepository<Settings>().Get().Interface.Country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Ukraine);
    }

    public static bool IsToolTipsEnableAndActivate()
    {
      return ToolTipsHelper.IsToolTipsEnable() && new ConfigsRepository<Settings>().Get().Interface.IsShowHelpTooltip;
    }

    public static void SetToolTip(
      this FrameworkElement control,
      HelpTip ts,
      bool addIsEnableChangedEvent = true)
    {
      try
      {
        string text = ts.Text;
        control.ToolTip = (object) null;
        if (ts.PlacementMode == PlacementMode.Mouse)
          return;
        if (!string.IsNullOrEmpty(ts.DisabledText))
        {
          if (addIsEnableChangedEvent)
            control.IsEnabledChanged += (DependencyPropertyChangedEventHandler) ((sender, args) => control.SetToolTip(ts, false));
          if (!control.IsEnabled)
            text = ts.DisabledText;
        }
        HelpPopUp helpPopUp = new HelpPopUp();
        string hotkey = string.Empty;
        if (ts.Hotkey != null)
          hotkey = ts.Hotkey.ToString().Replace("Return", "Enter");
        helpPopUp.Show(ts.Header, text, ts.PlacementMode, hotkey);
        ToolTipService.SetInitialShowDelay((DependencyObject) control, 1000);
        ToolTipService.SetShowDuration((DependencyObject) control, 15000);
        control.ToolTip = (object) helpPopUp;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, sendToZidium: false);
      }
    }
  }
}
