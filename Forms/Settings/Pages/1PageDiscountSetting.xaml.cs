// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageDiscountSetting
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class PageDiscountSetting : Page, IComponentConnector
  {
    internal CheckBox CheckBoxDiscountForBirthday;
    internal CheckBox CheckBoxBonuses;
    internal Label CountMethodCheckBox;
    internal Button MethodPaymentButton;
    internal CheckBox PeriodBonusesCb;
    private bool _contentLoaded;

    public PageDiscountSetting() => this.InitializeComponent();

    public PageDiscountSetting(Gbs.Core.Config.Settings settings)
    {
      this.InitializeComponent();
      this.DataContext = (object) new PageDiscountSettingViewModel(settings)
      {
        LoadingPaymentMethodsContextMenuGrid = new Action(this.LoadingPaymentMethodsContextMenuGrid)
      };
      this.LoadingPaymentMethodsContextMenuGrid();
    }

    private void LoadingPaymentMethodsContextMenuGrid()
    {
      ContextMenu resource = (ContextMenu) this.MethodPaymentButton.FindResource((object) "ContextMenuGrid");
      resource.Items.Clear();
      foreach (SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView> paymentMethod in (Collection<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>) ((PageDiscountSettingViewModel) this.DataContext).PaymentMethodList)
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) paymentMethod.Item.Name;
        newItem.IsChecked = paymentMethod.IsChecked;
        newItem.IsCheckable = true;
        newItem.Tag = (object) paymentMethod.Item.Method.Uid;
        items.Add((object) newItem);
      }
      resource.MouseLeave += new MouseEventHandler(this.CmButton_MouseLeave);
    }

    private void CmButton_MouseLeave(object sender, MouseEventArgs e)
    {
      PageDiscountSettingViewModel model = (PageDiscountSettingViewModel) this.DataContext;
      IEnumerable<MenuItem> source = ((ItemsControl) sender).Items.Cast<MenuItem>();
      model.PaymentMethodList = new ObservableCollection<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>(source.Select<MenuItem, SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<MenuItem, SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>) (x => new SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>()
      {
        Item = model.PaymentMethodList.Single<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>((Func<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>, bool>) (p => p.Item.Method.Uid == Guid.Parse(x.Tag.ToString()))).Item,
        IsChecked = x.IsChecked
      })).ToList<SaleJournalViewModel.ItemSelected<PageDiscountSettingViewModel.PaymentMethodView>>());
    }

    public bool Save() => ((PageDiscountSettingViewModel) this.DataContext).Save();

    private void MethodPaymentButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) sender;
      resource.IsOpen = true;
    }

    private void PageDiscountSetting_OnLoaded(object sender, RoutedEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pagediscountsetting.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.PageDiscountSetting_OnLoaded);
          break;
        case 2:
          this.CheckBoxDiscountForBirthday = (CheckBox) target;
          break;
        case 3:
          this.CheckBoxBonuses = (CheckBox) target;
          break;
        case 4:
          this.CountMethodCheckBox = (Label) target;
          break;
        case 5:
          this.MethodPaymentButton = (Button) target;
          this.MethodPaymentButton.Click += new RoutedEventHandler(this.MethodPaymentButton_OnClick);
          break;
        case 6:
          this.PeriodBonusesCb = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
