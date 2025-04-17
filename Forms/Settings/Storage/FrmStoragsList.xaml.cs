// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Warehouse.StorageListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Storage;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Warehouse
{
  public partial class StorageListViewModel : ViewModelWithForm
  {
    private string _filter = string.Empty;

    public string Filter
    {
      get => this._filter;
      set
      {
        this._filter = value;
        this.StorageList = value.IsNullOrEmpty() ? new ObservableCollection<Storages.Storage>(this.CashStorage) : new ObservableCollection<Storages.Storage>(this.CashStorage.Where<Storages.Storage>((Func<Storages.Storage, bool>) (x => x.Name.ToLower().Contains(value.ToLower()))));
        this.OnPropertyChanged("StorageList");
      }
    }

    private List<Storages.Storage> CashStorage { get; set; } = Storages.GetStorages().Where<Storages.Storage>((Func<Storages.Storage, bool>) (obj => !obj.IsDeleted)).ToList<Storages.Storage>();

    public ObservableCollection<Storages.Storage> StorageList { get; set; } = new ObservableCollection<Storages.Storage>();

    public Storages.Storage SelectedStorage { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public StorageListViewModel()
    {
      try
      {
        this.StorageList = new ObservableCollection<Storages.Storage>(this.CashStorage);
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Storages.Storage storage;
          if (!new FrmCardStorage().ShowCard(Guid.Empty, out storage))
            return;
          this.StorageList.Add(storage);
        }));
        this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedStorage != null)
          {
            if (((IEnumerable) obj).Cast<Storages.Storage>().ToList<Storages.Storage>().Count > 1)
            {
              int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
            }
            else
            {
              Storages.Storage storage;
              if (!new FrmCardStorage().ShowCard(this.SelectedStorage.Uid, out storage))
                return;
              this.StorageList[this.StorageList.ToList<Storages.Storage>().FindIndex((Predicate<Storages.Storage>) (x => x.Uid == storage.Uid))] = storage;
            }
          }
          else
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.StorageListViewModel_Требуется_выбрать_склад, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
        }));
        this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedStorage != null)
          {
            List<Storages.Storage> list = ((IEnumerable) obj).Cast<Storages.Storage>().ToList<Storages.Storage>();
            if (MessageBoxHelper.Show(string.Format(Translate.StorageListViewModel_Вы_уверены__что_хотите_удалить__0__складов_, (object) list.Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            foreach (Storages.Storage storage in list)
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              StorageListViewModel.\u003C\u003Ec__DisplayClass28_1 cDisplayClass281 = new StorageListViewModel.\u003C\u003Ec__DisplayClass28_1();
              // ISSUE: reference to a compiler-generated field
              cDisplayClass281.storage = storage;
              using (DataBase dataBase = Data.GetDataBase())
              {
                ParameterExpression parameterExpression1;
                ParameterExpression parameterExpression2;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: reference to a compiler-generated field
                // ISSUE: method reference
                // ISSUE: method reference
                if (dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => !x.IS_DELETED && new StorageListViewModel.\u003C\u003Ec__DisplayClass28_2()
                {
                  stockList = dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.STORAGE_UID == cDisplayClass281.storage.Uid && !x.IS_DELETED))
                }.stockList.Any<GOODS_STOCK>(System.Linq.Expressions.Expression.Lambda<Func<GOODS_STOCK, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(s.GOOD_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2)))).Any<GOODS>())
                {
                  // ISSUE: reference to a compiler-generated field
                  MessageBoxHelper.Warning(string.Format(Translate.StorageListViewModel_На_складе__0__есть_остатки__невозможно_удалить, (object) cDisplayClass281.storage.Name));
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass281.storage.IsDeleted = true;
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass281.storage.Save();
                  // ISSUE: reference to a compiler-generated field
                  this.StorageList.Remove(cDisplayClass281.storage);
                }
              }
            }
          }
          else
          {
            int num = (int) MessageBoxHelper.Show(Translate.StorageListViewModel_Требуется_выбрать_склад, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
        }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка складов");
      }
    }
  }
}
