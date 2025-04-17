// Decompiled with JetBrains decompiler
// Type: DotNetKit.Windows.Media.VisualTreeModule
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace DotNetKit.Windows.Media
{
  internal static class VisualTreeModule
  {
    public static FrameworkElement FindChild(DependencyObject obj, string childName)
    {
      if (obj == null)
        return (FrameworkElement) null;
      Queue<DependencyObject> dependencyObjectQueue = new Queue<DependencyObject>();
      dependencyObjectQueue.Enqueue(obj);
      while (dependencyObjectQueue.Count > 0)
      {
        obj = dependencyObjectQueue.Dequeue();
        int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
        for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
        {
          DependencyObject child1 = VisualTreeHelper.GetChild(obj, childIndex);
          if (child1 is FrameworkElement child2 && child2.Name == childName)
            return child2;
          dependencyObjectQueue.Enqueue(child1);
        }
      }
      return (FrameworkElement) null;
    }
  }
}
