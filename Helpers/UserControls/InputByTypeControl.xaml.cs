// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.InputByTypeControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Forms._shared;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class InputByTypeControl : UserControl, IComponentConnector
  {
    public static DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (object), typeof (InputByTypeControl));
    public static DependencyProperty TypeOfDataProperty = DependencyProperty.Register(nameof (TypeOfData), typeof (Type), typeof (InputByTypeControl));
    internal Grid grid;
    internal Grid grid2;
    private bool _contentLoaded;

    public object ObjectValue { get; set; }

    public DateTime? DateValue
    {
      get
      {
        DateTime result;
        return DateTime.TryParse(this.Value?.ToString(), out result) ? new DateTime?(result) : new DateTime?();
      }
      set => this.Value = (object) value;
    }

    public int? AutoNumValue
    {
      get
      {
        int result;
        if (!int.TryParse(this.Value?.ToString(), out result))
          return new int?();
        return result > 0 ? new int?(result) : new int?();
      }
      set
      {
        int? nullable = value;
        int num = 0;
        this.Value = (object) (nullable.GetValueOrDefault() <= num & nullable.HasValue ? new int?() : value);
      }
    }

    public string SystemValue
    {
      get => this.Value.ToString();
      set => this.Value = (object) value;
    }

    public string TextValue
    {
      get => this.Value.ToString();
      set => this.Value = (object) value;
    }

    public int? IntegerValue
    {
      get
      {
        int result;
        return int.TryParse(this.Value?.ToString(), out result) ? new int?(result) : new int?();
      }
      set => this.Value = (object) value;
    }

    public Decimal? DecimalValue
    {
      get
      {
        Decimal result;
        return Decimal.TryParse(this.Value?.ToString(), out result) ? new Decimal?(result) : new Decimal?();
      }
      set => this.Value = (object) value;
    }

    public object Value
    {
      get => this.GetValue(InputByTypeControl.ValueProperty);
      set => this.SetValue(InputByTypeControl.ValueProperty, value);
    }

    public GlobalDictionaries.EntityPropertyTypes TypeOfData
    {
      get
      {
        return (GlobalDictionaries.EntityPropertyTypes) this.GetValue(InputByTypeControl.TypeOfDataProperty);
      }
      set => this.SetValue(InputByTypeControl.TypeOfDataProperty, (object) value);
    }

    public Visibility IntegerVisibility
    {
      get
      {
        return this.TypeOfData != GlobalDictionaries.EntityPropertyTypes.Integer ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility DecimalVisibility
    {
      get
      {
        return this.TypeOfData != GlobalDictionaries.EntityPropertyTypes.Decimal ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility TextVisibility
    {
      get
      {
        return this.TypeOfData != GlobalDictionaries.EntityPropertyTypes.Text ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility DateVisibility
    {
      get
      {
        return this.TypeOfData != GlobalDictionaries.EntityPropertyTypes.DateTime ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility AutoNumVisibility
    {
      get
      {
        return this.TypeOfData != GlobalDictionaries.EntityPropertyTypes.AutoNum ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility SystemVisibility
    {
      get
      {
        return this.TypeOfData != GlobalDictionaries.EntityPropertyTypes.System ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public InputByTypeControl() => this.InitializeComponent();

    private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
      FrmKeyboard.ShowKeyboard();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/inputbytypecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.grid = (Grid) target;
          break;
        case 2:
          this.grid2 = (Grid) target;
          break;
        case 3:
          ((UIElement) target).PreviewMouseUp += new MouseButtonEventHandler(this.UIElement_OnMouseUp);
          break;
        case 4:
          ((UIElement) target).PreviewMouseUp += new MouseButtonEventHandler(this.UIElement_OnMouseUp);
          break;
        case 5:
          ((UIElement) target).PreviewMouseUp += new MouseButtonEventHandler(this.UIElement_OnMouseUp);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
