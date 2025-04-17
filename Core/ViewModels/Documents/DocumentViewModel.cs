// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Documents.DocumentViewModel`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
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
namespace Gbs.Core.ViewModels.Documents
{
  public abstract class DocumentViewModel<T> : ViewModel, IDocumentViewModel<T> where T : class, IDocumentItemViewModel
  {
    public ObservableCollection<T> _items = new ObservableCollection<T>();
    private T _selectedItem;
    private Storages.Storage _storage;

    public string StorageName => this.Storage?.Name ?? Translate.DocumentViewModel_не_указан;

    public Storages.Storage Storage
    {
      get => this._storage;
      set
      {
        this._storage = value;
        this.OnPropertyChanged(nameof (Storage));
        this.OnPropertyChanged("StorageName");
      }
    }

    public Users.User User { get; set; }

    public virtual ICommand EditQuantityCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.EditQuantity));
    }

    public virtual ICommand DeleteItemCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.DeleteItems));
    }

    public Decimal TotalQuantity => this.Items.Sum<T>((Func<T, Decimal>) (i => i.Quantity));

    public T SelectedItem
    {
      get => this._selectedItem;
      set
      {
        this._selectedItem = value;
        this.OnPropertyChanged(nameof (SelectedItem));
      }
    }

    private void EditQuantity(object obj)
    {
      List<T> castedList;
      if (!this.CheckSelectedItems(obj, out castedList))
        return;
      (bool result, Decimal? quantity) = new EditGoodQuantityViewModel().ShowQuantityEditCard(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<DocumentItemViewModel>) castedList.Cast<DocumentItemViewModel>().ToList<DocumentItemViewModel>()));
      if (!result)
        return;
      foreach (T obj1 in castedList)
        obj1.Quantity = quantity ?? obj1.Quantity;
      this.ReCalcTotals();
    }

    private void DeleteItems(object obj)
    {
      List<T> castedList;
      if (!this.CheckSelectedItems(obj, out castedList) || MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) castedList.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
        return;
      foreach (T obj1 in castedList)
        this.Items.Remove(obj1);
      if (!this.Items.Any<T>())
        this.Storage = (Storages.Storage) null;
      this.ReCalcTotals();
      this.OnPropertyChanged("Items");
    }

    protected bool CheckSelectedItems(object obj, out List<T> castedList)
    {
      IList source = (IList) obj;
      castedList = new List<T>();
      if (source == null || source.Count == 0)
      {
        int num = (int) MessageBoxHelper.Show(Translate.DocumentViewModel_Необходимо_выбрать_хотя_бы_одну_позицию_в_списке, icon: MessageBoxImage.Exclamation);
        return false;
      }
      castedList = source.Cast<T>().ToList<T>();
      return true;
    }

    public virtual void ReCalcTotals() => this.OnPropertyChanged("TotalQuantity");

    public virtual ActionResult Add(T item)
    {
      this.Items.Add(item);
      this.OnPropertyChanged(isUpdateAllProp: true);
      this.ReCalcTotals();
      return new ActionResult(ActionResult.Results.Ok);
    }

    public Document Document { get; set; }

    public ObservableCollection<T> Items
    {
      get => this._items;
      set
      {
        this._items = value;
        this.OnPropertyChanged(nameof (Items));
      }
    }

    public abstract ActionResult Save();
  }
}
