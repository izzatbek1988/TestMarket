// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HotKeysHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Text;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers
{
  public static class HotKeysHelper
  {
    public static void DoHotKey(KeyEventArgs e, ICommand command, object parameter = null)
    {
      if (command == null)
        return;
      e.Handled = true;
      command.Execute(parameter);
    }

    public class Hotkey
    {
      public Key Key { get; set; }

      public ModifierKeys Modifiers { get; set; }

      public Hotkey()
      {
      }

      public bool IsMatch(KeyEventArgs e)
      {
        return (this.Key != Key.None || this.Modifiers != ModifierKeys.None) && e.Key == this.Key && e.KeyboardDevice.Modifiers == this.Modifiers;
      }

      public Hotkey(Key key, ModifierKeys modifiers = ModifierKeys.None)
      {
        this.Key = key;
        this.Modifiers = modifiers;
      }

      public KeyGesture Gesture() => new KeyGesture(this.Key, this.Modifiers);

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this.Modifiers.HasFlag((Enum) ModifierKeys.Control))
          stringBuilder.Append("Ctrl + ");
        if (this.Modifiers.HasFlag((Enum) ModifierKeys.Shift))
          stringBuilder.Append("Shift + ");
        if (this.Modifiers.HasFlag((Enum) ModifierKeys.Alt))
          stringBuilder.Append("Alt + ");
        if (this.Modifiers.HasFlag((Enum) ModifierKeys.Windows))
          stringBuilder.Append("Win + ");
        if (this.Key != Key.None)
          stringBuilder.Append((object) this.Key);
        return stringBuilder.ToString();
      }
    }
  }
}
