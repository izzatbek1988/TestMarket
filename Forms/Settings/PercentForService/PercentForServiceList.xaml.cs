// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PercentForService.PercentForServiceListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Settings.BackEnd;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.PercentForService
{
  public partial class PercentForServiceListViewModel : ViewModelWithForm
  {
    public ObservableCollection<PercentForServiceSetting> ListPercentForService { get; set; }

    public Discount Discount { get; set; } = new Discount();

    public PercentForServiceSetting SelectedPercentForService { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public PercentForServiceListViewModel()
    {
      this.Discount.Load();
      this.ListPercentForService = new ObservableCollection<PercentForServiceSetting>(this.Discount.PercentForServiceRuleRepository.GetActiveItems());
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (bool result2, PercentForServiceSetting discount2) = new PercentForServiceCardViewModel().ShowCard(Guid.Empty, this.Discount);
        if (!result2)
          return;
        this.ListPercentForService.Add(discount2);
      }));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (((ICollection) obj).Count > 1)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
        else if (this.SelectedPercentForService != null)
        {
          (bool result, PercentForServiceSetting discount) tuple = new PercentForServiceCardViewModel().ShowCard(this.SelectedPercentForService.Uid, this.Discount);
          int num2 = tuple.result ? 1 : 0;
          PercentForServiceSetting percent = tuple.discount;
          if (num2 == 0)
            return;
          this.ListPercentForService[this.ListPercentForService.ToList<PercentForServiceSetting>().FindIndex((Predicate<PercentForServiceSetting>) (x => x.Uid == percent.Uid))] = percent;
        }
        else
        {
          int num3 = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedPercentForService != null)
        {
          List<PercentForServiceSetting> list = ((IEnumerable) obj).Cast<PercentForServiceSetting>().ToList<PercentForServiceSetting>();
          if (MessageBoxHelper.Show(Translate.PaymentAccountListViewModel_Вы_уверены_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          foreach (PercentForServiceSetting forServiceSetting in list)
          {
            forServiceSetting.IsDeleted = true;
            this.Discount.PercentForServiceRuleRepository.Save(forServiceSetting);
            this.ListPercentForService.Remove(forServiceSetting);
          }
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.SaleJournalViewModel_Требуется_выбрать_запись, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
    }
  }
}
