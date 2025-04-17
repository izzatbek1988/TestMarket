// Decompiled with JetBrains decompiler
// Type: DotNetKit.Windows.Controls.AutoCompleteComboBox
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using DotNetKit.Misc.Disposables;
using DotNetKit.Windows.Media;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace DotNetKit.Windows.Controls
{
  public partial class AutoCompleteComboBox : ComboBox, IComponentConnector
  {
    private readonly SerialDisposable disposable = new SerialDisposable();
    private TextBox editableTextBoxCache;
    private Predicate<object> defaultItemsFilter;
    public new static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (IEnumerable), typeof (AutoCompleteComboBox), new PropertyMetadata((object) null, new PropertyChangedCallback(AutoCompleteComboBox.ItemsSourcePropertyChanged)));
    private static readonly DependencyProperty settingProperty = DependencyProperty.Register(nameof (Setting), typeof (AutoCompleteComboBoxSetting), typeof (AutoCompleteComboBox));
    private long revisionId;
    private string previousText;
    private bool _contentLoaded;

    public TextBox EditableTextBox
    {
      get
      {
        if (this.editableTextBoxCache == null)
          this.editableTextBoxCache = (TextBox) VisualTreeModule.FindChild((DependencyObject) this, "PART_EditableTextBox");
        return this.editableTextBoxCache;
      }
    }

    private string TextFromItem(object item)
    {
      if (item == null)
        return string.Empty;
      DependencyVariable<string> dependencyVariable = new DependencyVariable<string>();
      dependencyVariable.SetBinding(item, TextSearch.GetTextPath((DependencyObject) this));
      return dependencyVariable.Value ?? string.Empty;
    }

    public new IEnumerable ItemsSource
    {
      get => (IEnumerable) this.GetValue(AutoCompleteComboBox.ItemsSourceProperty);
      set => this.SetValue(AutoCompleteComboBox.ItemsSourceProperty, (object) value);
    }

    private static void ItemsSourcePropertyChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dpcea)
    {
      ComboBox comboBox = (ComboBox) dependencyObject;
      object selectedItem = comboBox.SelectedItem;
      if (dpcea.NewValue is ICollectionView newValue1)
      {
        ((AutoCompleteComboBox) dependencyObject).defaultItemsFilter = newValue1.Filter;
        comboBox.ItemsSource = (IEnumerable) newValue1;
      }
      else
      {
        ((AutoCompleteComboBox) dependencyObject).defaultItemsFilter = (Predicate<object>) null;
        IEnumerable newValue = dpcea.NewValue as IEnumerable;
        CollectionViewSource collectionViewSource = new CollectionViewSource()
        {
          Source = (object) newValue
        };
        comboBox.ItemsSource = (IEnumerable) collectionViewSource.View;
      }
      comboBox.SelectedItem = selectedItem;
      if (comboBox.SelectedItem == selectedItem)
        return;
      comboBox.SelectedItem = (object) null;
    }

    public static DependencyProperty SettingProperty => AutoCompleteComboBox.settingProperty;

    public AutoCompleteComboBoxSetting Setting
    {
      get => (AutoCompleteComboBoxSetting) this.GetValue(AutoCompleteComboBox.SettingProperty);
      set => this.SetValue(AutoCompleteComboBox.SettingProperty, (object) value);
    }

    private AutoCompleteComboBoxSetting SettingOrDefault
    {
      get => this.Setting ?? AutoCompleteComboBoxSetting.Default;
    }

    private static int CountWithMax<T>(IEnumerable<T> xs, Predicate<T> predicate, int maxCount)
    {
      int num = 0;
      foreach (T obj in xs)
      {
        if (predicate(obj))
        {
          ++num;
          if (num > maxCount)
            return num;
        }
      }
      return num;
    }

    private void Unselect()
    {
      TextBox editableTextBox = this.EditableTextBox;
      editableTextBox.Select(editableTextBox.SelectionStart + editableTextBox.SelectionLength, 0);
    }

    private void UpdateFilter(Predicate<object> filter)
    {
      using (new AutoCompleteComboBox.TextBoxStatePreserver(this.EditableTextBox))
      {
        using (this.Items.DeferRefresh())
          this.Items.Filter = filter;
      }
    }

    private void OpenDropDown(Predicate<object> filter)
    {
      this.UpdateFilter(filter);
      this.IsDropDownOpen = true;
      this.Unselect();
    }

    private void OpenDropDown() => this.OpenDropDown(this.GetFilter());

    private void UpdateSuggestionList()
    {
      string text = this.Text;
      if (text == this.previousText)
        return;
      this.previousText = text;
      if (string.IsNullOrEmpty(text))
      {
        this.IsDropDownOpen = false;
        this.SelectedItem = (object) null;
        using (this.Items.DeferRefresh())
          this.Items.Filter = this.defaultItemsFilter;
      }
      else
      {
        if (this.SelectedItem != null && this.TextFromItem(this.SelectedItem) == text)
          return;
        using (new AutoCompleteComboBox.TextBoxStatePreserver(this.EditableTextBox))
          this.SelectedItem = (object) null;
        Predicate<object> filter = this.GetFilter();
        int maxSuggestionCount = this.SettingOrDefault.MaxSuggestionCount;
        IEnumerable itemsSource = this.ItemsSource;
        int num = AutoCompleteComboBox.CountWithMax<object>((itemsSource != null ? itemsSource.Cast<object>() : (IEnumerable<object>) null) ?? Enumerable.Empty<object>(), filter, maxSuggestionCount);
        if (0 < num && num <= maxSuggestionCount)
          this.OpenDropDown(filter);
        if (num != 0)
          return;
        this.IsDropDownOpen = false;
        this.SelectedItem = (object) null;
        using (this.Items.DeferRefresh())
          this.Items.Filter = this.defaultItemsFilter;
      }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
      long id = ++this.revisionId;
      AutoCompleteComboBoxSetting settingOrDefault = this.SettingOrDefault;
      if (settingOrDefault.Delay <= TimeSpan.Zero)
        this.UpdateSuggestionList();
      else
        this.disposable.Content = (IDisposable) new Timer((TimerCallback) (state => this.Dispatcher.InvokeAsync((Action) (() =>
        {
          if (this.revisionId != id)
            return;
          this.UpdateSuggestionList();
        }))), (object) null, settingOrDefault.Delay, Timeout.InfiniteTimeSpan);
    }

    private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (!Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Control) || e.Key != Key.Space)
        return;
      this.OpenDropDown();
      e.Handled = true;
    }

    private Predicate<object> GetFilter()
    {
      Predicate<object> filter = this.SettingOrDefault.GetFilter(this.Text, new Func<object, string>(this.TextFromItem));
      return this.defaultItemsFilter == null ? filter : (Predicate<object>) (i => this.defaultItemsFilter(i) && filter(i));
    }

    public AutoCompleteComboBox()
    {
      this.InitializeComponent();
      this.AddHandler(TextBoxBase.TextChangedEvent, (Delegate) new TextChangedEventHandler(this.OnTextChanged));
    }

    private void AutoCompleteComboBox_OnDropDownOpened(object sender, EventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/autocompletecombobox/autocompletecombobox.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        ((UIElement) target).PreviewKeyDown += new KeyEventHandler(this.ComboBox_PreviewKeyDown);
        ((ComboBox) target).DropDownOpened += new EventHandler(this.AutoCompleteComboBox_OnDropDownOpened);
      }
      else
        this._contentLoaded = true;
    }

    private struct TextBoxStatePreserver : IDisposable
    {
      private readonly TextBox textBox;
      private readonly int selectionStart;
      private readonly int selectionLength;
      private readonly string text;

      public void Dispose()
      {
        this.textBox.Text = this.text;
        this.textBox.Select(this.selectionStart, this.selectionLength);
      }

      public TextBoxStatePreserver(TextBox textBox)
      {
        this.textBox = textBox;
        this.selectionStart = textBox.SelectionStart;
        this.selectionLength = textBox.SelectionLength;
        this.text = textBox.Text;
      }
    }
  }
}
