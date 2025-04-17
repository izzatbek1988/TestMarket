// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DevelopersHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Gbs.Helpers
{
  public static class DevelopersHelper
  {
    private static bool? _isDebug;

    public static bool IsDebug()
    {
      if (new ConfigsRepository<Settings>().Get().DevelopedMode == "frezzer")
        DevelopersHelper._isDebug = new bool?(true);
      if (DevelopersHelper._isDebug.HasValue)
        return DevelopersHelper._isDebug.Value;
      bool isAttached = Debugger.IsAttached;
      DevelopersHelper._isDebug = new bool?(isAttached);
      return isAttached;
    }

    public static void LogTrace(string message)
    {
      if (!DevelopersHelper.IsDebug())
        return;
      LogHelper.Trace(message);
    }

    public static void RandomException(int max = 10)
    {
      if (DevelopersHelper.IsDebug() && new Random().Next(1, max) == 1)
        throw new Exception("Developer test exception");
    }

    private static bool IsTesting() => false;

    public static bool IsUnitTest() => DevelopersHelper.IsTesting();

    [Localizable(false)]
    public static void ShowNotification(string text)
    {
      if (!DevelopersHelper.IsDebug())
        return;
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(text)
      {
        Title = "Developer notification"
      });
    }
  }
}
