// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.ClientCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Payments;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Clients
{
  public partial class ClientCardViewModel : ViewModelWithForm, ICheckChangesViewModel
  {
    public bool IsEdit;
    private Decimal _totalBonusesOld;
    private ClientCardViewModel.ClientAction _selectedTypes;
    private List<ClientCardViewModel.JournalItem> _journalCache = new List<ClientCardViewModel.JournalItem>();
    public bool CancelTask;

    public Action FocusSumBonuses { get; set; }

    public Visibility VisibilityTextBonuses { get; set; }

    public Visibility VisibilityUpDownBonuses { get; set; } = Visibility.Collapsed;

    public Decimal BonusesSum { get; set; }

    public ICommand EditBonusesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          {
            int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            if (this.VisibilityTextBonuses == Visibility.Visible)
            {
              using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
              {
                if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.ClientsBonusesEdit) && !new Authorization().GetAccess(Actions.ClientsBonusesEdit).Result)
                  return;
                this.VisibilityTextBonuses = Visibility.Collapsed;
                this.VisibilityUpDownBonuses = Visibility.Visible;
              }
            }
            else
            {
              this.Client.CurrentBonusSum += this.BonusesSum - this.Client.TotalBonusSum;
              this.VisibilityTextBonuses = Visibility.Visible;
              this.VisibilityUpDownBonuses = Visibility.Collapsed;
            }
            this.OnPropertyChanged("Client");
            this.OnPropertyChanged("VisibilityTextBonuses");
            this.OnPropertyChanged("VisibilityUpDownBonuses");
            if (this.VisibilityUpDownBonuses != Visibility.Visible)
              return;
            this.FocusSumBonuses();
          }
        }));
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public static IEnumerable<Gbs.Core.Entities.Clients.Group> ListGroups { get; set; }

    public bool SaveResult { get; set; }

    public ICommand SaveClient { get; set; }

    public ICommand GeneratedBarcode { get; set; }

    public ICommand CloseCommand { get; set; }

    public Action CloseForm { get; set; }

    public ClientAdnSum Client { get; set; }

    public List<GoodCardModelView.ValuesProperty> ListValuesProperties { get; set; }

    public ObservableCollection<GoodCardModelView.ValuesProperty> ListValuesRequisites { get; set; }

    public ICommand ShowCreditListForClient
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new CreditListViewModel().ShowCreditList(this.Client.Client, this.AuthUser)));
      }
    }

    public ICommand ShowSaleListForClient
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new SaleJournalViewModel().ShowCard(new DateTime(2018, 1, 1), DateTime.Now, client: this.Client.Client, user: this.AuthUser, uidForm: this.Client.Client.Uid)));
      }
    }

    private Gbs.Core.Config.Devices DevicesConfig { get; set; } = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();

    public ClientCardViewModel()
    {
    }

    private void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Other.IsActiveForm<FrmClientCard>())
        return;
      this.Client.Client.Barcode = barcode;
      this.OnPropertyChanged("Client");
    }

    public ClientCardViewModel(ClientAdnSum client)
    {
      ClientCardViewModel clientCardViewModel = this;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ClientCardViewModel.ListGroups = (IEnumerable<Gbs.Core.Entities.Clients.Group>) new GroupRepository(dataBase).GetActiveItems().OrderBy<Gbs.Core.Entities.Clients.Group, string>((Func<Gbs.Core.Entities.Clients.Group, string>) (x => x.Name)).ToList<Gbs.Core.Entities.Clients.Group>();
        this.Client = client;
        this.BonusesSum = client.TotalBonusSum;
        this._totalBonusesOld = client.TotalBonusSum;
        ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.ComPortScannerOnBarcodeChanged));
        if (ClientCardViewModel.ListGroups.Count<Gbs.Core.Entities.Clients.Group>() == 1)
          this.Client.Client.Group = ClientCardViewModel.ListGroups.First<Gbs.Core.Entities.Clients.Group>();
        this.EntityClone = (IEntity) client.Client.Clone<Gbs.Core.Entities.Clients.Client>();
        this.ShowProperty();
        this.SaveClient = (ICommand) new RelayCommand((Action<object>) (obj => clientCardViewModel.Save()));
        this.GeneratedBarcode = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string[] strArray = clientCardViewModel.DevicesConfig.BarcodeScanner.Prefixes.DiscountCard.Split(GlobalDictionaries.SplitArr);
          if (strArray.Length == 0)
          {
            MessageBoxHelper.Warning(Translate.ClientCardViewModel_ClientCardViewModel_В_разделе_Файл___Настройки___Оборудование___Сканер_ШК_требуется_указать_префикс_для_генерации_штрих_кода_дисконтной_карты_);
          }
          else
          {
            clientCardViewModel.Client.Client.Barcode = BarcodeHelper.RandomBarcode(strArray[0]);
            clientCardViewModel.OnPropertyChanged(nameof (Client));
          }
        }));
        this.CloseCommand = (ICommand) new RelayCommand((Action<object>) (obj => clientCardViewModel.CloseForm()));
        Task.Run((Action) (() => ClientCardViewModel.GetCloudBonuses(client)));
        Task.Run(new Action(this.LoadingJournal));
      }
    }

    private static void GetCloudBonuses(ClientAdnSum client)
    {
      try
      {
        new Gbs.Helpers.ExternalApi.PolycardCloud.PolyCloud().GetCardBonuses(client.Client.Barcode);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    private void ShowProperty()
    {
      this.ListValuesProperties = new List<GoodCardModelView.ValuesProperty>();
      this.ListValuesRequisites = new ObservableCollection<GoodCardModelView.ValuesProperty>();
      foreach (EntityProperties.PropertyType types in EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client, false))
      {
        EntityProperties.PropertyType propertyType = types;
        if (!(propertyType.Uid == GlobalDictionaries.FiasUid) || new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Russia)
        {
          EntityProperties.PropertyValue propertyValue1;
          if (!this.Client.Client.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == propertyType.Uid)))
            propertyValue1 = new EntityProperties.PropertyValue()
            {
              Type = propertyType,
              EntityUid = this.Client.Client.Uid
            };
          else
            propertyValue1 = this.Client.Client.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == propertyType.Uid));
          EntityProperties.PropertyValue propertyValue2 = propertyValue1;
          if (GlobalDictionaries.RequisitesUidList.Any<Guid>((Func<Guid, bool>) (x => x == propertyType.Uid)))
            this.ListValuesRequisites.Add(new GoodCardModelView.ValuesProperty()
            {
              Value = propertyValue2,
              Type = propertyType
            });
          else
            this.ListValuesProperties.Add(new GoodCardModelView.ValuesProperty()
            {
              Value = propertyValue2,
              Type = propertyType
            });
        }
      }
      this.ListValuesProperties = this.ListValuesProperties.OrderBy<GoodCardModelView.ValuesProperty, string>((Func<GoodCardModelView.ValuesProperty, string>) (x => x.Type.Name)).ToList<GoodCardModelView.ValuesProperty>();
      this.ListValuesRequisites = new ObservableCollection<GoodCardModelView.ValuesProperty>(this.ListValuesRequisites.OrderBy<GoodCardModelView.ValuesProperty, string>((Func<GoodCardModelView.ValuesProperty, string>) (x => x.Type.Name)).ToList<GoodCardModelView.ValuesProperty>());
    }

    private void Save()
    {
      this.Client.Client.Properties.Clear();
      this.ListValuesProperties.Where<GoodCardModelView.ValuesProperty>((Func<GoodCardModelView.ValuesProperty, bool>) (x => x.Value.Value != null)).ToList<GoodCardModelView.ValuesProperty>().ForEach((Action<GoodCardModelView.ValuesProperty>) (x => this.Client.Client.Properties.Add(x.Value)));
      this.ListValuesRequisites.Where<GoodCardModelView.ValuesProperty>((Func<GoodCardModelView.ValuesProperty, bool>) (x => x.Value.Value != null)).ToList<GoodCardModelView.ValuesProperty>().ForEach((Action<GoodCardModelView.ValuesProperty>) (x => this.Client.Client.Properties.Add(x.Value)));
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        bool flag = new ClientsRepository(dataBase).Save(this.Client.Client);
        this.SaveResult = flag;
        if (!flag)
          return;
        if (this.VisibilityUpDownBonuses == Visibility.Visible)
        {
          if (MessageBoxHelper.Show(Translate.ClientCardViewModel_Save_Вы_не_сохранили_изменение_количества_баллов__продолжить_сохранение_карточки_контакта_без_изменения_суммы_баллов_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
            return;
        }
        else
        {
          Decimal d = this.BonusesSum - this._totalBonusesOld;
          if (d != 0M)
            new Gbs.Core.Entities.Payments.Payment()
            {
              SumOut = Math.Round(d, 4),
              Type = GlobalDictionaries.PaymentTypes.BonusesCorrection,
              Client = this.Client.Client,
              Date = DateTime.Now
            }.Save();
        }
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory(this.EntityClone, (IEntity) this.Client.Client, this.IsEdit ? ActionType.Edit : ActionType.Add, GlobalDictionaries.EntityTypes.Client, this.AuthUser), true);
        WindowWithSize.IsCancel = false;
        this.CloseForm();
      }
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Client.Client);
    }

    public List<ClientCardViewModel.JournalItem> Journal { get; set; } = new List<ClientCardViewModel.JournalItem>();

    public Dictionary<ClientCardViewModel.ClientAction, string> ActionList
    {
      get
      {
        return new Dictionary<ClientCardViewModel.ClientAction, string>()
        {
          {
            ClientCardViewModel.ClientAction.All,
            Translate.JournalGoodViewModel_Все_действия
          },
          {
            ClientCardViewModel.ClientAction.Sale,
            Translate.ClientCardViewModel_ActionList_Покупка
          },
          {
            ClientCardViewModel.ClientAction.Waybill,
            Translate.ClientCardViewModel_ActionList_Поставка_товара
          },
          {
            ClientCardViewModel.ClientAction.CafeOrder,
            Translate.ClientCardViewModel_ActionList_Заказ_в_кафе
          },
          {
            ClientCardViewModel.ClientAction.ClientOrder,
            Translate.ClientCardViewModel_ActionList_Заказ_резерв
          },
          {
            ClientCardViewModel.ClientAction.Payment,
            Translate.GlobalDictionaries_Платеж_по_документу
          }
        };
      }
    }

    public ClientCardViewModel.ClientAction SelectedTypes
    {
      get => this._selectedTypes;
      set
      {
        this._selectedTypes = value;
        this.Journal = this.SelectedTypes == ClientCardViewModel.ClientAction.All ? new List<ClientCardViewModel.JournalItem>((IEnumerable<ClientCardViewModel.JournalItem>) this._journalCache.OrderByDescending<ClientCardViewModel.JournalItem, DateTime?>((Func<ClientCardViewModel.JournalItem, DateTime?>) (item => item?.Date))) : new List<ClientCardViewModel.JournalItem>((IEnumerable<ClientCardViewModel.JournalItem>) this._journalCache.Where<ClientCardViewModel.JournalItem>((Func<ClientCardViewModel.JournalItem, bool>) (x => x.Type == value)).OrderByDescending<ClientCardViewModel.JournalItem, DateTime>((Func<ClientCardViewModel.JournalItem, DateTime>) (item => item.Date)));
        this.OnPropertyChanged("Journal");
      }
    }

    public string TitleTabJournal { get; set; } = Translate.ClientCardViewModel_TitleTabJournal_Загрузка___;

    public bool IsEnableJournal { get; set; }

    private void LoadingJournal()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Document> list = new DocumentsRepository(dataBase).GetItemsForContractor(this.Client.Client.Uid).Where<Document>((Func<Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.SaleReturn, GlobalDictionaries.DocumentsTypes.Buy, GlobalDictionaries.DocumentsTypes.CafeOrder, GlobalDictionaries.DocumentsTypes.ClientOrder))).ToList<Document>();
        List<Gbs.Core.Entities.Payments.Payment> paymentsList = Gbs.Core.Entities.Payments.GetPaymentsList(dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.PAYER_UID == this.Client.Client.Uid)));
        foreach (Document document in list)
        {
          if (this.CancelTask)
            return;
          string.Format("от {0:dd.MM.yyyy HH:mm}", (object) document?.DateTime);
          List<ClientCardViewModel.JournalItem> journalCache = this._journalCache;
          ClientCardViewModel.JournalItem journalItem1 = new ClientCardViewModel.JournalItem();
          ClientCardViewModel.JournalItem journalItem2 = journalItem1;
          ClientCardViewModel.ClientAction clientAction;
          switch (document.Type)
          {
            case GlobalDictionaries.DocumentsTypes.Sale:
              clientAction = ClientCardViewModel.ClientAction.Sale;
              break;
            case GlobalDictionaries.DocumentsTypes.SaleReturn:
              clientAction = ClientCardViewModel.ClientAction.Return;
              break;
            case GlobalDictionaries.DocumentsTypes.Buy:
              clientAction = ClientCardViewModel.ClientAction.Waybill;
              break;
            case GlobalDictionaries.DocumentsTypes.CafeOrder:
              clientAction = ClientCardViewModel.ClientAction.CafeOrder;
              break;
            case GlobalDictionaries.DocumentsTypes.ClientOrder:
              clientAction = ClientCardViewModel.ClientAction.ClientOrder;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          journalItem2.Type = clientAction;
          journalItem1.Date = document.DateTime;
          ClientCardViewModel.JournalItem journalItem3 = journalItem1;
          string str;
          switch (document.Type)
          {
            case GlobalDictionaries.DocumentsTypes.Sale:
              str = string.Format(Translate.ClientCardViewModel_LoadingJournal_покупкаТоваровНаСумму, (object) document.Number, (object) document.Items.Count, (object) SaleHelper.GetSumDocument(document)) + string.Format(Translate.ClientCardViewModel_LoadingJournal_Payed, (object) document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type != GlobalDictionaries.PaymentTypes.CheckDiscount)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut)));
              break;
            case GlobalDictionaries.DocumentsTypes.SaleReturn:
              str = string.Format(Translate.ClientCardViewModel_LoadingJournal_ВозвратТоваровНаСумму, (object) document.Number, (object) document.Items.Count, (object) SaleHelper.GetSumDocument(document));
              break;
            case GlobalDictionaries.DocumentsTypes.Buy:
              str = string.Format(Translate.ClientCardViewModel_LoadingJournal_ПоставкаТоваровНаСумму, (object) document.Number, (object) document.Items.Count, (object) SaleHelper.GetSumDocument(document));
              break;
            case GlobalDictionaries.DocumentsTypes.CafeOrder:
              str = string.Format(Translate.ClientCardViewModel_LoadingJournal_ЗаказТоваровНаСумму, (object) document.Number, (object) document.Items.Count, (object) SaleHelper.GetSumDocument(document));
              break;
            case GlobalDictionaries.DocumentsTypes.ClientOrder:
              str = string.Format(Translate.ClientCardViewModel_LoadingJournal_ЗаказРезервнТоваровНаСумму, (object) document.Number, (object) document.Items.Count, (object) SaleHelper.GetSumDocument(document));
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          journalItem3.Action = str;
          journalCache.Add(journalItem1);
        }
        foreach (Gbs.Core.Entities.Payments.Payment payment1 in paymentsList)
        {
          Gbs.Core.Entities.Payments.Payment payment = payment1;
          if (this.CancelTask)
            return;
          if (!payment.Type.IsEither<GlobalDictionaries.PaymentTypes>(GlobalDictionaries.PaymentTypes.BonusesCorrection, GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment, GlobalDictionaries.PaymentTypes.BonusesDocumentPayment))
          {
            Document document = list.SingleOrDefault<Document>((Func<Document, bool>) (x =>
            {
              Guid uid = x.Uid;
              Gbs.Core.Entities.Payments.Payment payment2 = payment;
              Guid guid = payment2 != null ? payment2.ParentUid : Guid.Empty;
              return uid == guid;
            }));
            List<ClientCardViewModel.JournalItem> journalCache = this._journalCache;
            ClientCardViewModel.JournalItem journalItem4 = new ClientCardViewModel.JournalItem();
            journalItem4.Date = payment.Date;
            ClientCardViewModel.JournalItem journalItem5 = journalItem4;
            ClientCardViewModel.ClientAction clientAction;
            switch (payment.Type)
            {
              case GlobalDictionaries.PaymentTypes.MoneyDocumentPayment:
                clientAction = ClientCardViewModel.ClientAction.Payment;
                break;
              case GlobalDictionaries.PaymentTypes.BonusesDocumentPayment:
                clientAction = ClientCardViewModel.ClientAction.Bonuses;
                break;
              case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment:
                clientAction = ClientCardViewModel.ClientAction.Bonuses;
                break;
              case GlobalDictionaries.PaymentTypes.BonusesCorrection:
                clientAction = ClientCardViewModel.ClientAction.Bonuses;
                break;
              case GlobalDictionaries.PaymentTypes.Prepaid:
                clientAction = ClientCardViewModel.ClientAction.Payment;
                break;
              default:
                clientAction = ClientCardViewModel.ClientAction.Payment;
                break;
            }
            journalItem5.Type = clientAction;
            ClientCardViewModel.JournalItem journalItem6 = journalItem4;
            string str;
            switch (payment.Type)
            {
              case GlobalDictionaries.PaymentTypes.MoneyDocumentPayment:
                str = string.Format(Translate.ClientCardViewModel_LoadingJournal_Платеж_по_документу___0_ПлатежПоДокументу, (object) document?.Number) + string.Format(Translate.ClientCardViewModel_LoadingJournal_ОтСумма, (object) document?.DateTime, (object) (payment.SumIn - payment.SumOut), (object) payment.Method.Name);
                break;
              case GlobalDictionaries.PaymentTypes.BonusesDocumentPayment:
                str = string.Format(Translate.ClientCardViewModel_LoadingJournal_Списание_баллов_на_покупку___0_, (object) document?.Number) + string.Format(Translate.ClientCardViewModel_LoadingJournal_отДДММГГСумма, (object) document?.DateTime, (object) (payment.SumIn - payment.SumOut));
                break;
              case GlobalDictionaries.PaymentTypes.BonusesDocumentItemPayment:
                str = !(payment.SumIn < payment.SumOut) ? string.Format(Translate.ClientCardViewModel_LoadingJournal_Списание_баллов_при_возврате___0_, (object) document?.Number) + string.Format(" от {0:dd.MM.yyyy HH:mm}\nСумма: {1:N2}", (object) document?.DateTime, (object) (payment.SumIn - payment.SumOut)) : string.Format(Translate.ClientCardViewModel_LoadingJournal_Начисление_баллов_за_покупку___0_, (object) document?.Number) + string.Format(Translate.ClientCardViewModel_LoadingJournal_отДДММГГ_Сумма, (object) document.DateTime, (object) (payment.SumOut - payment.SumIn));
                break;
              case GlobalDictionaries.PaymentTypes.BonusesCorrection:
                str = string.Format(Translate.ClientCardViewModel_LoadingJournal_КорректировкаБалансаБаллов, (object) (payment.SumIn - payment.SumOut));
                break;
              case GlobalDictionaries.PaymentTypes.Prepaid:
                str = string.Format(Translate.ClientCardViewModel_LoadingJournal_ПредоплатаПоЗаказуРезерву, (object) document?.Number, (object) (payment.SumIn - payment.SumOut), (object) payment.Method.Name);
                break;
              default:
                str = (string) null;
                break;
            }
            journalItem6.Action = str;
            journalCache.Add(journalItem4);
          }
        }
        this._journalCache = this._journalCache.Where<ClientCardViewModel.JournalItem>((Func<ClientCardViewModel.JournalItem, bool>) (x => x.Action != null)).ToList<ClientCardViewModel.JournalItem>();
        this.Journal = new List<ClientCardViewModel.JournalItem>(this._journalCache.OrderBy<ClientCardViewModel.JournalItem, DateTime?>((Func<ClientCardViewModel.JournalItem, DateTime?>) (item => item?.Date)).Reverse<ClientCardViewModel.JournalItem>());
        this.TitleTabJournal = Translate.FrmGoodCard_Журнал;
        this.IsEnableJournal = true;
        this.OnPropertyChanged("TitleTabJournal");
        this.OnPropertyChanged("IsEnableJournal");
        this.OnPropertyChanged("Journal");
      }
    }

    public enum ClientAction
    {
      All,
      Sale,
      Waybill,
      Return,
      ClientOrder,
      CafeOrder,
      Payment,
      Bonuses,
    }

    public class JournalItem
    {
      public ClientCardViewModel.ClientAction Type { get; set; }

      public DateTime Date { get; set; }

      public string Action { get; set; }
    }
  }
}
