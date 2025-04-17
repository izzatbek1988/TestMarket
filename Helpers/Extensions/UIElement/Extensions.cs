// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Extensions.UIElement.Extensions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Windows;

#nullable disable
namespace Gbs.Helpers.Extensions.UIElement
{
  public class Extensions
  {
    public static readonly DependencyProperty GuidProperty = DependencyProperty.RegisterAttached("Guid", typeof (string), typeof (Gbs.Helpers.Extensions.UIElement.Extensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty TagProperty = DependencyProperty.RegisterAttached("Tag", typeof (string), typeof (Gbs.Helpers.Extensions.UIElement.Extensions), new PropertyMetadata((object) null));

    public static void SetGuid(DependencyObject element, string value)
    {
      element.SetValue(Gbs.Helpers.Extensions.UIElement.Extensions.GuidProperty, (object) value);
    }

    public static string GetGuid(DependencyObject element)
    {
      return (string) element.GetValue(Gbs.Helpers.Extensions.UIElement.Extensions.GuidProperty) ?? string.Empty;
    }

    public static void SetTag(DependencyObject element, string value)
    {
      element.SetValue(Gbs.Helpers.Extensions.UIElement.Extensions.TagProperty, (object) value);
    }

    public static string GetTag(DependencyObject element)
    {
      return (string) element.GetValue(Gbs.Helpers.Extensions.UIElement.Extensions.TagProperty) ?? string.Empty;
    }
  }
}
