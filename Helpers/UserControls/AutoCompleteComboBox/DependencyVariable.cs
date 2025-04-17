// Decompiled with JetBrains decompiler
// Type: DotNetKit.Windows.DependencyVariable`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Windows;
using System.Windows.Data;

#nullable disable
namespace DotNetKit.Windows
{
  internal sealed class DependencyVariable<T> : DependencyObject
  {
    private static readonly DependencyProperty valueProperty = DependencyProperty.Register(nameof (Value), typeof (T), typeof (DependencyVariable<T>));

    public static DependencyProperty ValueProperty => DependencyVariable<T>.valueProperty;

    public T Value
    {
      get => (T) this.GetValue(DependencyVariable<T>.ValueProperty);
      set => this.SetValue(DependencyVariable<T>.ValueProperty, (object) value);
    }

    public void SetBinding(Binding binding)
    {
      BindingOperations.SetBinding((DependencyObject) this, DependencyVariable<T>.ValueProperty, (BindingBase) binding);
    }

    public void SetBinding(object dataContext, string propertyPath)
    {
      this.SetBinding(new Binding(propertyPath)
      {
        Source = dataContext
      });
    }
  }
}
