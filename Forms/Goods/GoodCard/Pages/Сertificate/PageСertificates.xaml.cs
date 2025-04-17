// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.Pages.Сertificate.CertificateViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard.Pages.Сertificate
{
  public class CertificateViewModel : ViewModelWithForm
  {
    private GlobalDictionaries.CertificateStatus _status = GlobalDictionaries.CertificateStatus.All;
    private static ObservableCollection<CertificateBasicViewModel.CertificateView> _certificates = new ObservableCollection<CertificateBasicViewModel.CertificateView>();
    private ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>> _storageListFilter = new ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>>();

    public Dictionary<GlobalDictionaries.CertificateStatus, string> Statuses
    {
      get => GlobalDictionaries.CertificateStatusDictionary();
    }

    public GlobalDictionaries.CertificateStatus Status
    {
      get => this._status;
      set
      {
        this._status = value;
        this.SearchCertificate();
      }
    }

    private void SearchCertificate()
    {
      List<CertificateBasicViewModel.CertificateView> certificateViewList = new List<CertificateBasicViewModel.CertificateView>((IEnumerable<CertificateBasicViewModel.CertificateView>) CertificateViewModel.CertificatesDb);
      if (this.Status != GlobalDictionaries.CertificateStatus.All)
        certificateViewList = certificateViewList.Where<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => x.Certificate.Status == this.Status)).ToList<CertificateBasicViewModel.CertificateView>();
      if (this.StorageListFilter.Any<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => !x.IsChecked)))
        certificateViewList = certificateViewList.Where<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => this.StorageListFilter.Where<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (s => s.IsChecked)).Any<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (s => s.Item.Uid == x.Certificate.Stock.Storage.Uid)))).ToList<CertificateBasicViewModel.CertificateView>();
      this.CertificatesView = new ObservableCollection<CertificateBasicViewModel.CertificateView>(certificateViewList);
    }

    public static string AddMenuKey => nameof (AddMenuKey);

    public ObservableCollection<CertificateBasicViewModel.CertificateView> CertificatesView
    {
      get => CertificateViewModel._certificates;
      set
      {
        CertificateViewModel._certificates = value;
        this.OnPropertyChanged(nameof (CertificatesView));
      }
    }

    public static List<CertificateBasicViewModel.CertificateView> CertificatesDb { get; set; } = new List<CertificateBasicViewModel.CertificateView>();

    public ICommand AddCertificates
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<GoodsCertificate.Certificate> source = new FrmCardCertificate().GeneratedCertificate(Visibility.Collapsed, Visibility.Visible);
          if (!source.Any<GoodsCertificate.Certificate>())
            return;
          foreach (GoodsCertificate.Certificate certificate1 in source)
          {
            GoodsCertificate.Certificate certificate = certificate1;
            if (this.CertificatesView.Select<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>((Func<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>) (x => x.Certificate.Stock)).Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.GoodUid == certificate.Stock.GoodUid && x.Storage.Uid == certificate.Stock.Storage.Uid)))
              certificate.Stock = this.CertificatesView.Select<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>((Func<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>) (x => x.Certificate.Stock)).First<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.GoodUid == certificate.Stock.GoodUid && x.Storage.Uid == certificate.Stock.Storage.Uid));
            certificate.Stock.Price = CertificateBasicViewModel.Price;
            this.CertificatesView.Add(new CertificateBasicViewModel.CertificateView(certificate));
            CertificateViewModel.CertificatesDb.Add(new CertificateBasicViewModel.CertificateView(certificate));
          }
        }));
      }
    }

    public ICommand AddOneCertificate
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<GoodsCertificate.Certificate> source = new FrmCardCertificate().GeneratedCertificate(Visibility.Visible, Visibility.Collapsed);
          if (!source.Any<GoodsCertificate.Certificate>())
            return;
          foreach (GoodsCertificate.Certificate certificate1 in source)
          {
            GoodsCertificate.Certificate certificate = certificate1;
            if (this.CertificatesView.Select<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>((Func<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>) (x => x.Certificate.Stock)).Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.GoodUid == certificate.Stock.GoodUid && x.Storage.Uid == certificate.Stock.Storage.Uid)))
            {
              certificate.Stock = this.CertificatesView.Select<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>((Func<CertificateBasicViewModel.CertificateView, GoodsStocks.GoodStock>) (x => x.Certificate.Stock)).First<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.GoodUid == certificate.Stock.GoodUid && x.Storage.Uid == certificate.Stock.Storage.Uid));
              certificate.Stock.Stock += 1M;
            }
            certificate.Stock.Price = CertificateBasicViewModel.Price;
            this.CertificatesView.Add(new CertificateBasicViewModel.CertificateView(certificate));
            CertificateViewModel.CertificatesDb.Add(new CertificateBasicViewModel.CertificateView(certificate));
          }
        }));
      }
    }

    public ICommand DeleteCertificates
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<CertificateBasicViewModel.CertificateView> list = ((IEnumerable) obj).Cast<CertificateBasicViewModel.CertificateView>().ToList<CertificateBasicViewModel.CertificateView>();
          if (!list.Any<CertificateBasicViewModel.CertificateView>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.InventoryDoViewModel_Необходимо_выбрать_хотя_бы_одну_запись_);
          }
          else
          {
            if (MessageBoxHelper.Show(string.Format(Translate.CertificateBasicViewModel_Вы_уверены__что_хотите_удалить__0__сертификатов_, (object) list.Count), buttons: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
              return;
            foreach (CertificateBasicViewModel.CertificateView certificateView in list)
            {
              CertificateBasicViewModel.CertificateView c = certificateView;
              CertificateViewModel.CertificatesDb[CertificateViewModel.CertificatesDb.FindIndex((Predicate<CertificateBasicViewModel.CertificateView>) (x => x.Certificate == c.Certificate))].Certificate.IsDeleted = true;
              CertificateViewModel.CertificatesDb.First<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => x.Certificate.Stock.Uid == c.Certificate.Stock.Uid)).Certificate.Stock.Stock -= 1M;
              this.CertificatesView.Remove(c);
            }
          }
        }));
      }
    }

    public ICommand PrintCertificates
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<CertificateBasicViewModel.CertificateView> list = ((IEnumerable) obj).Cast<CertificateBasicViewModel.CertificateView>().ToList<CertificateBasicViewModel.CertificateView>();
          if (!list.Any<CertificateBasicViewModel.CertificateView>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.InventoryDoViewModel_Необходимо_выбрать_хотя_бы_одну_запись_);
          }
          else
          {
            using (DataBase dataBase = Data.GetDataBase())
            {
              Gbs.Core.Entities.Goods.Good good = new GoodRepository(dataBase).GetByUid(list.First<CertificateBasicViewModel.CertificateView>().Certificate.Stock.GoodUid) ?? this.Good;
              new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForCertifications(list, good, CertificateBasicViewModel.Nominal.GetValueOrDefault()), (Users.User) null);
            }
          }
        }));
      }
    }

    private Gbs.Core.Entities.Goods.Good Good { get; set; }

    private List<GoodsStocks.GoodStock> Stock { get; set; }

    public CertificateViewModel()
    {
    }

    public CertificateViewModel(List<GoodsStocks.GoodStock> stock, Gbs.Core.Entities.Goods.Good good)
    {
      this.Stock = stock;
      this.Good = good;
      this.GetCertificateList();
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.AllListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
        this._storageListFilter = new ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>>(this.AllListStorage.Select<Storages.Storage, SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<Storages.Storage, SaleJournalViewModel.ItemSelected<Storages.Storage>>) (x => new SaleJournalViewModel.ItemSelected<Storages.Storage>()
        {
          IsChecked = true,
          Item = x
        })));
      }
    }

    private void GetCertificateList()
    {
      List<CertificateBasicViewModel.CertificateView> list = GoodsCertificate.GetCertificateFilteredList().Where<GoodsCertificate.Certificate>((Func<GoodsCertificate.Certificate, bool>) (x => this.Stock.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s =>
      {
        Guid uid1 = s.Uid;
        Guid? uid2 = x.Stock?.Uid;
        return uid2.HasValue && uid1 == uid2.GetValueOrDefault();
      })) && !x.IsDeleted)).Select<GoodsCertificate.Certificate, CertificateBasicViewModel.CertificateView>((Func<GoodsCertificate.Certificate, CertificateBasicViewModel.CertificateView>) (x => new CertificateBasicViewModel.CertificateView(x, true))).ToList<CertificateBasicViewModel.CertificateView>();
      CertificateViewModel.CertificatesDb = list;
      this.CertificatesView = new ObservableCollection<CertificateBasicViewModel.CertificateView>(list.Where<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => x.Certificate.Barcode != "")));
    }

    private IEnumerable<Storages.Storage> AllListStorage { get; } = (IEnumerable<Storages.Storage>) new List<Storages.Storage>();

    public string ButtonContentStorage
    {
      get
      {
        int num = this._storageListFilter.Count<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => x.IsChecked));
        if (num == this.AllListStorage.Count<Storages.Storage>())
          return Translate.WaybillsViewModel_Все_склады;
        return num != 1 ? Translate.WaybillsViewModel_Складов_ + num.ToString() : this._storageListFilter.Single<SaleJournalViewModel.ItemSelected<Storages.Storage>>((Func<SaleJournalViewModel.ItemSelected<Storages.Storage>, bool>) (x => x.IsChecked)).Item.Name;
      }
    }

    public ObservableCollection<SaleJournalViewModel.ItemSelected<Storages.Storage>> StorageListFilter
    {
      get => this._storageListFilter;
      set
      {
        this._storageListFilter = value;
        this.OnPropertyChanged(nameof (StorageListFilter));
        this.OnPropertyChanged("ButtonContentStorage");
        this.SearchCertificate();
      }
    }
  }
}
