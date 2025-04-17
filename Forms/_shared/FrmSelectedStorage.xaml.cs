// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.StorageSelectedViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class StorageSelectedViewModel : ViewModelWithForm
  {
    public bool Result { get; set; }

    public List<Storages.Storage> SelectedStorages { get; private set; }

    public ICommand SelectedStorage { get; set; }

    public Action Close { get; set; }

    public ObservableCollection<StorageSelectedViewModel.StorageView> StoragesList { get; set; } = new ObservableCollection<StorageSelectedViewModel.StorageView>();

    public StorageSelectedViewModel()
    {
    }

    public StorageSelectedViewModel(List<Storages.Storage> selectedList)
    {
      this.SelectedStorages = selectedList;
      using (DataBase dataBase = Data.GetDataBase())
        this.StoragesList = new ObservableCollection<StorageSelectedViewModel.StorageView>(Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false))).Select<Storages.Storage, StorageSelectedViewModel.StorageView>((Func<Storages.Storage, StorageSelectedViewModel.StorageView>) (storage => new StorageSelectedViewModel.StorageView()
        {
          Storage = storage,
          IsChecked = this.SelectedStorages.Any<Storages.Storage>((Func<Storages.Storage, bool>) (x => x.Uid == storage.Uid))
        })));
      this.SelectedStorage = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = true;
        this.SelectedStorages = this.StoragesList.Where<StorageSelectedViewModel.StorageView>((Func<StorageSelectedViewModel.StorageView, bool>) (x => x.IsChecked)).Select<StorageSelectedViewModel.StorageView, Storages.Storage>((Func<StorageSelectedViewModel.StorageView, Storages.Storage>) (x => x.Storage)).ToList<Storages.Storage>();
        this.Close();
      }));
    }

    public ICommand AllSelectedCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.StoragesList = new ObservableCollection<StorageSelectedViewModel.StorageView>(this.StoragesList.Select<StorageSelectedViewModel.StorageView, StorageSelectedViewModel.StorageView>((Func<StorageSelectedViewModel.StorageView, StorageSelectedViewModel.StorageView>) (x => new StorageSelectedViewModel.StorageView()
          {
            Storage = x.Storage,
            IsChecked = true
          })));
          this.OnPropertyChanged("StoragesList");
        }));
      }
    }

    public ICommand AllUnSelectedCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.StoragesList = new ObservableCollection<StorageSelectedViewModel.StorageView>(this.StoragesList.Select<StorageSelectedViewModel.StorageView, StorageSelectedViewModel.StorageView>((Func<StorageSelectedViewModel.StorageView, StorageSelectedViewModel.StorageView>) (x => new StorageSelectedViewModel.StorageView()
          {
            Storage = x.Storage,
            IsChecked = false
          })));
          this.OnPropertyChanged("StoragesList");
        }));
      }
    }

    public class StorageView
    {
      public bool IsChecked { get; set; }

      public Storages.Storage Storage { get; set; }
    }
  }
}
