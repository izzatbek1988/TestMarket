// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.CategorySelectionControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms.GoodGroups;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class CategorySelectionControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(nameof (ButtonContent), typeof (string), typeof (CategorySelectionControl), new PropertyMetadata((object) Translate.GoodsCatalogModelView_Все_категории));
    public static readonly DependencyProperty GroupsListFilterProperty = DependencyProperty.Register(nameof (GroupsListFilter), typeof (ObservableCollection<Gbs.Core.Entities.GoodGroups.Group>), typeof (CategorySelectionControl), new PropertyMetadata((object) new ObservableCollection<Gbs.Core.Entities.GoodGroups.Group>()));
    public static readonly DependencyProperty FontSizeContentProperty = DependencyProperty.Register(nameof (FontSizeContent), typeof (int), typeof (CategorySelectionControl), new PropertyMetadata((object) 24));
    public static readonly DependencyProperty IsAllEquallyNullProperty = DependencyProperty.Register(nameof (IsAllEquallyNull), typeof (bool), typeof (CategorySelectionControl), new PropertyMetadata((object) false));
    private IEnumerable<Gbs.Core.Entities.GoodGroups.Group> _allListGroupGood;
    internal Border ClearBtn;
    private bool _contentLoaded;

    public void UpdateTextButton(ObservableCollection<Gbs.Core.Entities.GoodGroups.Group> groups)
    {
      int count = groups.Count;
      if (count != 0 && count != this._allListGroupGood.Count<Gbs.Core.Entities.GoodGroups.Group>())
        this.ButtonContent = count == 1 ? groups.First<Gbs.Core.Entities.GoodGroups.Group>().Name : Translate.GoodsCatalogModelView_Категорий__ + count.ToString();
      else if (count == 1)
        this.ButtonContent = groups.First<Gbs.Core.Entities.GoodGroups.Group>().Name;
      else if (count == 0 && !this.IsAllEquallyNull)
        this.ButtonContent = Translate.FormSelectGroup_ВыберитеКатегорию;
      else
        this.ButtonContent = Translate.GoodsCatalogModelView_Все_категории;
    }

    public string ButtonContent
    {
      get => (string) this.GetValue(CategorySelectionControl.ButtonContentProperty);
      set => this.SetValue(CategorySelectionControl.ButtonContentProperty, (object) value);
    }

    public int FontSizeContent
    {
      get => (int) this.GetValue(CategorySelectionControl.FontSizeContentProperty);
      set => this.SetValue(CategorySelectionControl.FontSizeContentProperty, (object) value);
    }

    public bool IsAllEquallyNull
    {
      get => (bool) this.GetValue(CategorySelectionControl.IsAllEquallyNullProperty);
      set => this.SetValue(CategorySelectionControl.IsAllEquallyNullProperty, (object) value);
    }

    public ObservableCollection<Gbs.Core.Entities.GoodGroups.Group> GroupsListFilter
    {
      get
      {
        return (ObservableCollection<Gbs.Core.Entities.GoodGroups.Group>) this.GetValue(CategorySelectionControl.GroupsListFilterProperty);
      }
      set
      {
        this.SetValue(CategorySelectionControl.GroupsListFilterProperty, (object) value);
        this.UpdateTextButton(value);
        this.ClearBtn.Visibility = !value.Any<Gbs.Core.Entities.GoodGroups.Group>() || value.Count == this._allListGroupGood.Count<Gbs.Core.Entities.GoodGroups.Group>() && this.IsAllEquallyNull ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand GetGroupsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          FormSelectGroup formSelectGroup = new FormSelectGroup();
          List<Gbs.Core.Entities.GoodGroups.Group> list = new List<Gbs.Core.Entities.GoodGroups.Group>((IEnumerable<Gbs.Core.Entities.GoodGroups.Group>) this.GroupsListFilter);
          ref List<Gbs.Core.Entities.GoodGroups.Group> local = ref list;
          if (!formSelectGroup.GetSelectedGroupUid((Users.User) null, ref local))
            return;
          using (DataBase dataBase = Data.GetDataBase())
          {
            this._allListGroupGood = (IEnumerable<Gbs.Core.Entities.GoodGroups.Group>) new GoodGroupsRepository(dataBase).GetActiveItems();
            this.GroupsListFilter = new ObservableCollection<Gbs.Core.Entities.GoodGroups.Group>(list);
          }
        }));
      }
    }

    public ICommand ClearCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => this.GroupsListFilter = new ObservableCollection<Gbs.Core.Entities.GoodGroups.Group>()));
      }
    }

    public CategorySelectionControl()
    {
      this.InitializeComponent();
      using (DataBase dataBase = Data.GetDataBase())
        this._allListGroupGood = (IEnumerable<Gbs.Core.Entities.GoodGroups.Group>) new GoodGroupsRepository(dataBase).GetActiveItems();
    }

    private void CategorySelectionControl_OnLoaded(object sender, RoutedEventArgs e)
    {
      if (this.IsAllEquallyNull)
        this.ClearBtn.Visibility = Visibility.Collapsed;
      else
        this.ClearBtn.Visibility = Visibility.Visible;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/categoryselectioncontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.ClearBtn = (Border) target;
        else
          this._contentLoaded = true;
      }
      else
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.CategorySelectionControl_OnLoaded);
    }
  }
}
