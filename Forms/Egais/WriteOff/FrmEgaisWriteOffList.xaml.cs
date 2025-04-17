// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.EgaisWriteOffListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Egais;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
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
namespace Gbs.Forms.Egais
{
  public partial class EgaisWriteOffListViewModel : ViewModelWithForm
  {
    private EgaisWriteOffActStatus _selectedActStatus;
    public List<EgaisWriteOffListViewModel.EgaisWriteOffItem> CacheDocuments;

    public DateTime ValueDateTimeStart { get; set; }

    public DateTime ValueDateTimeEnd { get; set; }

    public ICommand SearchCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (o => this.Search()));
    }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<EgaisWriteOffListViewModel.EgaisWriteOffItem> list = ((IEnumerable) obj).Cast<EgaisWriteOffListViewModel.EgaisWriteOffItem>().ToList<EgaisWriteOffListViewModel.EgaisWriteOffItem>();
          if (!list.Any<EgaisWriteOffListViewModel.EgaisWriteOffItem>())
          {
            MessageBoxHelper.Error(Translate.PaymentsActionListViewModel_Необходимо_выбрать_хотя_бы_одну_запись_для_удаления);
          }
          else
          {
            if (MessageBoxHelper.Question(string.Format(Translate.EgaisWriteOffListViewModel_DeleteCommand_Вы_уверены__что_хотите_удалить__0__актов_списания_алкогольной_продукции__После_удаления_позиции_будут_доступа_снова_для_отправки_в_ЕГАИС_, (object) list.Count)) == MessageBoxResult.No)
              return;
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.EgaisWriteOffListViewModel_DeleteCommand_Удаление_актов_списания_алкогольной_продукции__ЕГАИС_);
            foreach (EgaisWriteOffListViewModel.EgaisWriteOffItem egaisWriteOffItem in list)
            {
              EgaisWriteOffListViewModel.EgaisWriteOffItem item = egaisWriteOffItem;
              item.Act.IsDeleted = true;
              item.Act.Comment = "";
              new EgaisWriteOffActRepository().Save(item.Act);
              foreach (EgaisWriteOffActsItems writeOffActsItems in item.Act.Items)
              {
                writeOffActsItems.ActUid = Guid.Empty;
                new EgaisWriteOffActsItemRepository().Save(writeOffActsItems);
              }
              this.CacheDocuments.RemoveAll((Predicate<EgaisWriteOffListViewModel.EgaisWriteOffItem>) (x => x.Act.Uid == item.Act.Uid));
            }
            progressBar.Close();
            this.Search();
          }
        }));
      }
    }

    private void Search()
    {
      List<EgaisWriteOffListViewModel.EgaisWriteOffItem> egaisWriteOffItemList = new List<EgaisWriteOffListViewModel.EgaisWriteOffItem>();
      List<EgaisWriteOffListViewModel.EgaisWriteOffItem> source = new List<EgaisWriteOffListViewModel.EgaisWriteOffItem>(this.CacheDocuments.Where<EgaisWriteOffListViewModel.EgaisWriteOffItem>((Func<EgaisWriteOffListViewModel.EgaisWriteOffItem, bool>) (x =>
      {
        DateTime dateTime1 = x.Act.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.ValueDateTimeStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.Act.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.ValueDateTimeEnd;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      })));
      if (this.SelectedActStatus != EgaisWriteOffActStatus.All)
        source = new List<EgaisWriteOffListViewModel.EgaisWriteOffItem>(source.Where<EgaisWriteOffListViewModel.EgaisWriteOffItem>((Func<EgaisWriteOffListViewModel.EgaisWriteOffItem, bool>) (x => x.Act.Status == this.SelectedActStatus)));
      this.Items = new ObservableCollection<EgaisWriteOffListViewModel.EgaisWriteOffItem>((IEnumerable<EgaisWriteOffListViewModel.EgaisWriteOffItem>) source.OrderBy<EgaisWriteOffListViewModel.EgaisWriteOffItem, DateTime>((Func<EgaisWriteOffListViewModel.EgaisWriteOffItem, DateTime>) (x => x.Act.DateTime)));
      this.OnPropertyChanged("Items");
    }

    public EgaisWriteOffActStatus SelectedActStatus
    {
      get => this._selectedActStatus;
      set
      {
        this._selectedActStatus = value;
        this.Search();
      }
    }

    public Dictionary<EgaisWriteOffActStatus, string> EgaisWriteOffActStatusDictionary
    {
      get => GlobalDictionaries.EgaisWriteOffActStatusDictionary;
    }

    public ObservableCollection<EgaisWriteOffListViewModel.EgaisWriteOffItem> Items { get; set; }

    public void Show()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.EgaisWriteOffListViewModel_Show_Загрузка_актов_списания__ЕГАИС_);
      List<EgaisWriteOffAct> activeItems = new EgaisWriteOffActRepository().GetActiveItems();
      if (!activeItems.Any<EgaisWriteOffAct>())
      {
        progressBar.Close();
        MessageBoxHelper.Warning(Translate.EgaisWriteOffListViewModel_Show_Нет_доступных_актов_списания__для_начала_необходимо_продать_товар_и_списать_позиции_);
      }
      else
      {
        using (Data.GetDataBase())
        {
          this.CacheDocuments = new List<EgaisWriteOffListViewModel.EgaisWriteOffItem>(activeItems.Select<EgaisWriteOffAct, EgaisWriteOffListViewModel.EgaisWriteOffItem>((Func<EgaisWriteOffAct, EgaisWriteOffListViewModel.EgaisWriteOffItem>) (x => new EgaisWriteOffListViewModel.EgaisWriteOffItem()
          {
            Act = x
          })));
          foreach (EgaisWriteOffListViewModel.EgaisWriteOffItem cacheDocument in this.CacheDocuments)
          {
            if (!cacheDocument.Act.Status.IsEither<EgaisWriteOffActStatus>(EgaisWriteOffActStatus.Error, EgaisWriteOffActStatus.Done))
              DoWriteOffItemRepository.GetResultTicket(cacheDocument.Act);
          }
          this.Search();
          progressBar.Close();
          this.FormToSHow = (WindowWithSize) new FrmEgaisWriteOffList();
          this.ShowForm();
        }
      }
    }

    public EgaisWriteOffListViewModel()
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      // ISSUE: reference to a compiler-generated field
      this.\u003CValueDateTimeStart\u003Ek__BackingField = dateTime.AddYears(-1);
      // ISSUE: reference to a compiler-generated field
      this.\u003CValueDateTimeEnd\u003Ek__BackingField = DateTime.Now.Date;
      // ISSUE: reference to a compiler-generated field
      this.\u003CItems\u003Ek__BackingField = new ObservableCollection<EgaisWriteOffListViewModel.EgaisWriteOffItem>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public class EgaisWriteOffItem
    {
      public string Type
      {
        get
        {
          return GlobalDictionaries.EgaisTypeWriteOffDictionary.SingleOrDefault<KeyValuePair<TypeWriteOff1, string>>((Func<KeyValuePair<TypeWriteOff1, string>, bool>) (x => x.Key == this.Act.Type)).Value ?? "";
        }
      }

      public string Status
      {
        get
        {
          return GlobalDictionaries.EgaisWriteOffActStatusDictionary.SingleOrDefault<KeyValuePair<EgaisWriteOffActStatus, string>>((Func<KeyValuePair<EgaisWriteOffActStatus, string>, bool>) (x => x.Key == this.Act.Status)).Value ?? "";
        }
      }

      public Decimal TotalGood
      {
        get
        {
          return this.Act.Items.Sum<EgaisWriteOffActsItems>((Func<EgaisWriteOffActsItems, Decimal>) (x => x.Quantity));
        }
      }

      public Decimal TotalSum
      {
        get
        {
          return this.Act.Items.Sum<EgaisWriteOffActsItems>((Func<EgaisWriteOffActsItems, Decimal>) (x => x.Sum));
        }
      }

      public EgaisWriteOffAct Act { get; set; }
    }
  }
}
