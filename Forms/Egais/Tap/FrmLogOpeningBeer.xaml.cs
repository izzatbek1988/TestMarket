// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.LogOpeningBeerViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using LinqToDB.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Egais
{
  public partial class LogOpeningBeerViewModel : ViewModelWithForm
  {
    private List<ManagementForTapBeerViewModel.InfoTapBeerItem> _cacheItemsBeers;

    public Decimal TotalQuantity
    {
      get
      {
        ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem> itemsBeers = this.ItemsBeers;
        return itemsBeers == null ? 0M : itemsBeers.Sum<ManagementForTapBeerViewModel.InfoTapBeerItem>((Func<ManagementForTapBeerViewModel.InfoTapBeerItem, Decimal>) (x => ((Decimal?) x.Info?.Quantity).GetValueOrDefault()));
      }
    }

    public Decimal TotalSum
    {
      get
      {
        ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem> itemsBeers = this.ItemsBeers;
        return itemsBeers == null ? 0M : itemsBeers.Sum<ManagementForTapBeerViewModel.InfoTapBeerItem>((Func<ManagementForTapBeerViewModel.InfoTapBeerItem, Decimal>) (x =>
        {
          Decimal? nullable = x.Info.Quantity;
          Decimal valueOrDefault1 = nullable.GetValueOrDefault();
          nullable = x.Info.Price;
          Decimal valueOrDefault2 = nullable.GetValueOrDefault();
          return valueOrDefault1 * valueOrDefault2;
        }));
      }
    }

    public ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem> ItemsBeers { get; set; }

    public DateTime ValueDateTimeStart { get; set; }

    public DateTime ValueDateTimeEnd { get; set; }

    public void Search()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar("Обновление журнала вскрытия пивных кег");
      this.ItemsBeers = new ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem>((IEnumerable<ManagementForTapBeerViewModel.InfoTapBeerItem>) new List<ManagementForTapBeerViewModel.InfoTapBeerItem>(this._cacheItemsBeers.Where<ManagementForTapBeerViewModel.InfoTapBeerItem>((Func<ManagementForTapBeerViewModel.InfoTapBeerItem, bool>) (x =>
      {
        DateTime valueDateTimeStart = x.Info.ConnectingDateTime.Value;
        DateTime date1 = valueDateTimeStart.Date;
        valueDateTimeStart = this.ValueDateTimeStart;
        DateTime date2 = valueDateTimeStart.Date;
        if (!(date1 >= date2))
          return false;
        DateTime valueDateTimeEnd = x.Info.ConnectingDateTime.Value;
        DateTime date3 = valueDateTimeEnd.Date;
        valueDateTimeEnd = this.ValueDateTimeEnd;
        DateTime date4 = valueDateTimeEnd.Date;
        return date3 <= date4;
      }))).OrderByDescending<ManagementForTapBeerViewModel.InfoTapBeerItem, DateTime?>((Func<ManagementForTapBeerViewModel.InfoTapBeerItem, DateTime?>) (x => x.Info.ConnectingDateTime)));
      this.OnPropertyChanged("ItemsBeers");
      progressBar.Close();
    }

    public ICommand SearchCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.Search()));
    }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ManagementForTapBeerViewModel.InfoTapBeerItem> list = ((IEnumerable) obj).Cast<ManagementForTapBeerViewModel.InfoTapBeerItem>().ToList<ManagementForTapBeerViewModel.InfoTapBeerItem>();
          if (list.Count != 1)
          {
            MessageBoxHelper.Warning("Требуется выбрать одну запись вскрытия кеги для удаления.");
          }
          else
          {
            Decimal stock = list.Single<ManagementForTapBeerViewModel.InfoTapBeerItem>().Stock;
            Decimal? quantity = list.Single<ManagementForTapBeerViewModel.InfoTapBeerItem>().Info.Quantity;
            Decimal valueOrDefault = quantity.GetValueOrDefault();
            if (!(stock == valueOrDefault & quantity.HasValue))
            {
              MessageBoxHelper.Warning("Удалить выбранное вскрытие нельзя, так как часть товара уже было продано.");
            }
            else
            {
              if (MessageBoxHelper.Question("Вы уверены, что хотите удалить выбранное вскрытие кеги?\n\nОтмена вскрытия НЕ отобразится в Честном знаке и ЕГАИС, из-за чего могут возникнуть расхождения остатков.") == MessageBoxResult.No)
                return;
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar("Удаление документа вскрытия кеги");
              using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
              {
                DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
                DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
                Document byUid = documentsRepository.GetByUid(list.Single<ManagementForTapBeerViewModel.InfoTapBeerItem>().Info.DocumentUid);
                byUid.IsDeleted = true;
                List<Document> byParentUid = documentsRepository.GetByParentUid(byUid.Uid);
                byParentUid.ForEach((Action<Document>) (x => x.IsDeleted = true));
                if (!documentsRepository.Save(byUid) || documentsRepository.Save(byParentUid) != byParentUid.Count)
                {
                  connectionTransaction.Rollback();
                  progressBar.Close();
                }
                else
                {
                  InfoToTapBeer info = list.Single<ManagementForTapBeerViewModel.InfoTapBeerItem>().Info;
                  info.DocumentUid = Guid.Empty;
                  info.IsDeleted = true;
                  new InfoTapBeerRepository().Save(info);
                  connectionTransaction.Commit();
                  progressBar.Close();
                  this._cacheItemsBeers = this._cacheItemsBeers.Where<ManagementForTapBeerViewModel.InfoTapBeerItem>((Func<ManagementForTapBeerViewModel.InfoTapBeerItem, bool>) (x => x.Info.Uid != info.Uid)).ToList<ManagementForTapBeerViewModel.InfoTapBeerItem>();
                  this.Search();
                }
              }
            }
          }
        }));
      }
    }

    public void ShowLog()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar("Загрузка журнала вскрытия пивных кег");
      using (DataBase db = Gbs.Core.Data.GetDataBase())
      {
        this._cacheItemsBeers = new List<ManagementForTapBeerViewModel.InfoTapBeerItem>(new InfoTapBeerRepository().GetAllItems().Where<InfoToTapBeer>((Func<InfoToTapBeer, bool>) (x => x.DocumentUid != Guid.Empty)).Select<InfoToTapBeer, ManagementForTapBeerViewModel.InfoTapBeerItem>((Func<InfoToTapBeer, ManagementForTapBeerViewModel.InfoTapBeerItem>) (x => new ManagementForTapBeerViewModel.InfoTapBeerItem(x, db))));
        this.Search();
        progressBar.Close();
        this.FormToSHow = (WindowWithSize) new FrmLogOpeningBeer();
        this.ShowForm();
      }
    }

    public LogOpeningBeerViewModel()
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      // ISSUE: reference to a compiler-generated field
      this.\u003CValueDateTimeStart\u003Ek__BackingField = dateTime.AddYears(-1);
      // ISSUE: reference to a compiler-generated field
      this.\u003CValueDateTimeEnd\u003Ek__BackingField = DateTime.Now.Date;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
