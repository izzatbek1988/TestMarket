// Decompiled with JetBrains decompiler
// Type: DotNetKit.Windows.Controls.AutoCompleteComboBoxSetting
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace DotNetKit.Windows.Controls
{
  public class AutoCompleteComboBoxSetting
  {
    private static AutoCompleteComboBoxSetting @default = new AutoCompleteComboBoxSetting();

    public virtual Predicate<object> GetFilter(string query, Func<object, string> stringFromItem)
    {
      return (Predicate<object>) (item => stringFromItem(item).IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0);
    }

    public virtual int MaxSuggestionCount => 100;

    public virtual TimeSpan Delay => TimeSpan.FromMilliseconds(500.0);

    public static AutoCompleteComboBoxSetting Default
    {
      get => AutoCompleteComboBoxSetting.@default;
      set
      {
        AutoCompleteComboBoxSetting.@default = value != null ? value : throw new ArgumentNullException(nameof (value));
      }
    }
  }
}
