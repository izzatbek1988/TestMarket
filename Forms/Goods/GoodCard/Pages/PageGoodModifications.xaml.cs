// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.GoodModificationPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
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
namespace Gbs.Forms.Goods.GoodCard
{
  public partial class GoodModificationPageViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodsModifications.GoodModification> _modifications;

    public ICommand AddCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ObservableCollection<GoodsModifications.GoodModification> Modifications
    {
      get => this._modifications;
      set
      {
        this._modifications = value;
        this.OnPropertyChanged(nameof (Modifications));
      }
    }

    public GoodsModifications.GoodModification SelectedModification { get; set; }

    public GoodModificationPageViewModel()
    {
    }

    public GoodModificationPageViewModel(Gbs.Core.Entities.Goods.Good good)
    {
      GoodModificationPageViewModel modificationPageViewModel = this;
      this.Modifications = new ObservableCollection<GoodsModifications.GoodModification>(good.Modifications);
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => modificationPageViewModel.AddModification(good)));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj => modificationPageViewModel.EditModification(good, obj)));
      this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteModification));
    }

    private void DeleteModification(object obj)
    {
      List<GoodsModifications.GoodModification> list = ((IEnumerable) obj).Cast<GoodsModifications.GoodModification>().ToList<GoodsModifications.GoodModification>();
      if (!list.Any<GoodsModifications.GoodModification>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_строку_);
      }
      else if (list.Count > 1)
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
      }
      else
      {
        if (MessageBoxHelper.Show(Translate.GoodModificationPageViewModel_Вы_уверены__что_хотите_удалить_данную_модификацию_, buttons: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
          return;
        this.SelectedModification.IsDeleted = true;
        this.Modifications.Remove(this.SelectedModification);
      }
    }

    private void EditModification(Gbs.Core.Entities.Goods.Good good, object obj)
    {
      List<GoodsModifications.GoodModification> list = ((IEnumerable) obj).Cast<GoodsModifications.GoodModification>().ToList<GoodsModifications.GoodModification>();
      if (!list.Any<GoodsModifications.GoodModification>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_строку_);
      }
      else if (list.Count > 1)
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
      }
      else
      {
        (bool, GoodsModifications.GoodModification) tuple = new FrmGoodModificationCard().ShowCard(this.SelectedModification);
        int num3 = tuple.Item1 ? 1 : 0;
        GoodsModifications.GoodModification modification = tuple.Item2;
        if (num3 == 0)
          return;
        this.Modifications[this.Modifications.ToList<GoodsModifications.GoodModification>().FindIndex((Predicate<GoodsModifications.GoodModification>) (x => x.Uid == modification.Uid))] = modification;
        good.Modifications = (IEnumerable<GoodsModifications.GoodModification>) this.Modifications.ToList<GoodsModifications.GoodModification>();
      }
    }

    private void AddModification(Gbs.Core.Entities.Goods.Good good)
    {
      (bool flag, GoodsModifications.GoodModification goodModification) = new FrmGoodModificationCard().ShowCard((GoodsModifications.GoodModification) null);
      if (!flag)
        return;
      this.Modifications.Add(goodModification);
      good.Modifications = (IEnumerable<GoodsModifications.GoodModification>) this.Modifications.ToList<GoodsModifications.GoodModification>();
    }
  }
}
