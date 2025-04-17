// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.XAML.FocusExtension
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Windows;

#nullable disable
namespace Gbs.Helpers.XAML
{
  public static class FocusExtension
  {
    public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof (bool), typeof (FocusExtension), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(FocusExtension.OnIsFocusedPropertyChanged)));

    public static bool GetIsFocused(DependencyObject obj)
    {
      return (bool) obj.GetValue(FocusExtension.IsFocusedProperty);
    }

    public static void SetIsFocused(DependencyObject obj, bool value)
    {
      obj.SetValue(FocusExtension.IsFocusedProperty, (object) value);
    }

    private static void OnIsFocusedPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      UIElement uiElement = (UIElement) d;
      if (!(bool) e.NewValue)
        return;
      uiElement.Focus();
    }
  }
}
