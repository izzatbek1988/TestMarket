// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.MessageBoxHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Forms.Other;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class MessageBoxHelper
  {
    public static MessageBoxResult Show(
      string message,
      string caption = "",
      MessageBoxButton buttons = MessageBoxButton.OK,
      MessageBoxImage icon = MessageBoxImage.Asterisk)
    {
      LogHelper.Debug("Msg: " + message);
      if (caption.Length == 0)
        caption = PartnersHelper.ProgramName();
      if (DevelopersHelper.IsUnitTest())
      {
        Gbs.Helpers.Other.ConsoleWrite("MSG: " + message);
        return MessageBoxResult.OK;
      }
      try
      {
        MessageBoxResult result = MessageBoxResult.None;
        Gbs.Helpers.Other.ConsoleWrite("MessageBox.Helper: 30");
        Application.Current.Dispatcher?.Invoke((Action) (() =>
        {
          Gbs.Forms.Other.MessageBox messageBox = new Gbs.Forms.Other.MessageBox();
          Gbs.Helpers.Other.ConsoleWrite("MessageBox.Helper: 38");
          result = messageBox.Show(message, caption, buttons, icon).Result;
        }));
        return result;
      }
      catch
      {
        return System.Windows.MessageBox.Show(message, caption, buttons, icon);
      }
    }

    public static Gbs.Forms.Other.MessageBox.MsgBoxResult ShowWithCheckboxes(
      string message,
      string caption = "",
      MessageBoxImage icon = MessageBoxImage.Asterisk,
      List<MessBoxViewModel.CheckboxItem> checkboxes = null)
    {
      LogHelper.Debug("Msg: " + message);
      try
      {
        Gbs.Forms.Other.MessageBox.MsgBoxResult result = new Gbs.Forms.Other.MessageBox.MsgBoxResult();
        Application.Current.Dispatcher?.Invoke((Action) (() => result = new Gbs.Forms.Other.MessageBox().Show(message, caption, image: icon, checkboxes: checkboxes)));
        return result;
      }
      catch
      {
        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(message, caption, MessageBoxButton.OK, icon);
        return new Gbs.Forms.Other.MessageBox.MsgBoxResult()
        {
          Result = messageBoxResult,
          SelectedIndex = -1
        };
      }
    }

    public static Gbs.Forms.Other.MessageBox.MsgBoxResult ShowWithCommands(
      string message,
      string caption = "",
      MessageBoxImage icon = MessageBoxImage.Asterisk,
      Dictionary<int, string> commands = null)
    {
      LogHelper.Debug("Msg: " + message);
      if (caption.Length == 0)
        caption = PartnersHelper.ProgramName();
      if (DevelopersHelper.IsUnitTest())
      {
        Gbs.Helpers.Other.ConsoleWrite("MSG: " + message);
        return new Gbs.Forms.Other.MessageBox.MsgBoxResult()
        {
          Result = MessageBoxResult.Cancel,
          SelectedIndex = -1
        };
      }
      try
      {
        Gbs.Forms.Other.MessageBox.MsgBoxResult result = new Gbs.Forms.Other.MessageBox.MsgBoxResult();
        Gbs.Helpers.Other.ConsoleWrite("MessageBox.Helper: 30");
        Application.Current.Dispatcher?.Invoke((Action) (() =>
        {
          Gbs.Forms.Other.MessageBox messageBox = new Gbs.Forms.Other.MessageBox();
          Gbs.Helpers.Other.ConsoleWrite("MessageBox.Helper: 38");
          result = messageBox.Show(message, caption, image: icon, buttons: commands);
        }));
        return result;
      }
      catch
      {
        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(message, caption, MessageBoxButton.OK, icon);
        return new Gbs.Forms.Other.MessageBox.MsgBoxResult()
        {
          Result = messageBoxResult,
          SelectedIndex = -1
        };
      }
    }

    public static void Error(string message, string caption = "")
    {
      int num = (int) MessageBoxHelper.Show(message, caption, icon: MessageBoxImage.Hand);
    }

    public static void Warning(string message, string caption = "")
    {
      int num = (int) MessageBoxHelper.Show(message, caption, icon: MessageBoxImage.Exclamation);
    }

    public static MessageBoxResult Question(string message, bool withCancel = false)
    {
      MessageBoxButton buttons = withCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo;
      return MessageBoxHelper.Show(message, buttons: buttons, icon: MessageBoxImage.Question);
    }

    public static (bool result, string output) Input(
      string input,
      string message,
      int minLength = 0,
      string caption = "",
      MessageBoxButton buttons = MessageBoxButton.OK,
      MessageBoxImage icon = MessageBoxImage.Asterisk)
    {
      if (DevelopersHelper.IsUnitTest())
        return (true, Guid.NewGuid().ToString());
      if (caption.Length == 0)
        caption = PartnersHelper.ProgramName();
      try
      {
        (MessageBoxResult, string) result = (MessageBoxResult.None, string.Empty);
        Application.Current.Dispatcher?.Invoke((Action) (() => result = new Gbs.Forms.Other.MessageBox().ShowInput(input, message, minLength, caption)));
        return (result.Item1 == MessageBoxResult.OK, result.Item2);
      }
      catch
      {
        return (false, string.Empty);
      }
    }
  }
}
