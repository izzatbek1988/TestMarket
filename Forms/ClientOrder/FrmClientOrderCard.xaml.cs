// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ClientOrder.ClientOrderViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Forms.Goods;
using Gbs.Forms.Sale.Return;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.ClientOrder
{
  public partial class ClientOrderViewModel : ViewModelWithForm, ICheckChangesViewModel
  {
    private Document _documentClone;
    private ClientOrderViewModel.OptionalClientOrder _lastOptionalOrder;
    private CheckFiscalTypes _type;

    public ICommand CopyPhoneCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Clipboard.SetText(this.Basket.Client?.Client?.Phone ?? "");
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.ClientOrderViewModel_CopyPhoneCommand_Номер_телефона_клиента_скопирован_в_буфер_обмена));
        }));
      }
    }

    public bool IsEnabledOptionSavePriceValue { get; set; } = true;

    public ClientOrderViewModel.OptionalClientOrder OptionalOrder { get; set; } = new ClientOrderViewModel.OptionalClientOrder();

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            this.CurrentBasket = ((MainWindowViewModel) (Application.Current.Windows.OfType<MainWindow>().SingleOrDefault<MainWindow>() ?? throw new NullReferenceException(Translate.ClientOrderViewModel_SaveCommand_Не_удалось_найти_экземпляр_главного_окна)).DataContext).CurrentBasket;
            if (this._documentClone.Status == GlobalDictionaries.DocumentsStatuses.Close)
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.ClientOrderViewModel_Вносить_изменения_в_уже_закрытый_заказа_невозможно_, icon: MessageBoxImage.Exclamation);
            }
            else if (this.Basket.Client == null)
              MessageBoxHelper.Warning(Translate.ClientOrderViewModel_Для_сохранения_заказа_необходимо_выбрать_покупателя_);
            else if (this.Basket.Document.Status == GlobalDictionaries.DocumentsStatuses.Close && this.CurrentBasket.Items.Any<BasketItem>())
            {
              int num3 = (int) MessageBoxHelper.Show(Translate.ClientOrderViewModel_В_корзине_уже_есть_товары__сначала_требуется_очистить_товары__Сохраните_заказ_с_другим_статусом, icon: MessageBoxImage.Exclamation);
            }
            else
            {
              this.CreateDocOrder(this.Basket.Document);
              this.UpdateDocument();
              if (this.Basket.Document.Status == GlobalDictionaries.DocumentsStatuses.Close && SaleHelper.GetSumDocument(this.Basket.Document) < this.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)))
              {
                int num4 = (int) MessageBoxHelper.Show(Translate.DocumentsRepository_Сумма_платежей_не_может_быть_больше__чем_сумма_товаров, icon: MessageBoxImage.Exclamation);
              }
              else
              {
                if (this.Basket.Document.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.OptionalClientOrderUid)))
                {
                  this.Basket.Document.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.OptionalClientOrderUid)).Value = (object) JsonConvert.SerializeObject((object) this.OptionalOrder);
                }
                else
                {
                  List<EntityProperties.PropertyValue> properties = this.Basket.Document.Properties;
                  properties.Add(new EntityProperties.PropertyValue()
                  {
                    EntityUid = this.Basket.Document.Uid,
                    Type = new EntityProperties.PropertyType()
                    {
                      Uid = GlobalDictionaries.OptionalClientOrderUid
                    },
                    Value = (object) JsonConvert.SerializeObject((object) this.OptionalOrder)
                  });
                }
                this.Basket.Document.Payments.ForEach((Action<Gbs.Core.Entities.Payments.Payment>) (x => x.Comment = string.Format(Translate.ClientOrderViewModel_SaveCommand_Предоплата_по_заказу___0_, (object) this.Basket.Document.Number)));
                using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
                {
                  if (!new DocumentsRepository(dataBase).Save(this.Basket.Document, false))
                    return;
                  List<Gbs.Core.Entities.Payments.Payment> list1 = this.Basket.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => ((Document) this.EntityClone).Payments.All<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (i => i.Uid != x.Uid)))).ToList<Gbs.Core.Entities.Payments.Payment>();
                  string str = this.Basket.Items.FirstOrDefault<BasketItem>()?.DisplayedName ?? string.Empty;
                  Document document1 = new Document()
                  {
                    Type = GlobalDictionaries.DocumentsTypes.Sale,
                    Items = new List<Gbs.Core.Entities.Documents.Item>()
                    {
                      new Gbs.Core.Entities.Documents.Item()
                      {
                        Quantity = 1M,
                        SellPrice = list1.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)),
                        Good = new Gbs.Core.Entities.Goods.Good()
                        {
                          Name = !new ConfigsRepository<Settings>().Get().Sales.IsSendNameForPrepaidCheck || str.IsNullOrEmpty() ? Translate.ClientOrderViewModel_SaveCommand_ + this.Basket.Document.Number : str,
                          Group = new GoodGroups.Group()
                        }
                      }
                    },
                    Payments = list1.Clone<List<Gbs.Core.Entities.Payments.Payment>>()
                  };
                  if (list1.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
                  {
                    PaymentMethods.PaymentMethod method = x.Method;
                    return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring;
                  })))
                    this.Basket.PayAcquiring(list1.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
                    {
                      PaymentMethods.PaymentMethod method = x.Method;
                      return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring;
                    })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)), (Document) null);
                  List<Gbs.Core.Entities.Payments.Payment> list2 = ((Document) this.EntityClone).Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted && this.Basket.Document.Payments.Single<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (p => p.Uid == x.Uid)).IsDeleted)).ToList<Gbs.Core.Entities.Payments.Payment>();
                  Document document2 = new Document()
                  {
                    Type = GlobalDictionaries.DocumentsTypes.SaleReturn,
                    Items = new List<Gbs.Core.Entities.Documents.Item>()
                    {
                      new Gbs.Core.Entities.Documents.Item()
                      {
                        Quantity = 1M,
                        SellPrice = list2.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)),
                        Good = new Gbs.Core.Entities.Goods.Good()
                        {
                          Name = Translate.ClientOrderViewModel_SaveCommand_вовзрат_предоплаты_по_заказу + this.Basket.Document.Number,
                          Group = new GoodGroups.Group()
                        }
                      }
                    },
                    Payments = list2.Clone<List<Gbs.Core.Entities.Payments.Payment>>()
                  };
                  if (list2.Any<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
                  {
                    PaymentMethods.PaymentMethod method = x.Method;
                    return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring;
                  })))
                    ReturnItemsViewModel.AcquiringReturn(list2.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x =>
                    {
                      PaymentMethods.PaymentMethod method = x.Method;
                      return method != null && method.PaymentMethodsType == GlobalDictionaries.PaymentMethodsType.Acquiring;
                    })).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn)), "", "");
                  ActionPrintViewModel.TypePrint typePrint1 = ActionPrintViewModel.TypePrint.Fiscal;
                  if ((!new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.IsShowPrintConfirmationForm ? 0 : (document1.Payments.Any<Gbs.Core.Entities.Payments.Payment>() ? 1 : (document2.Payments.Any<Gbs.Core.Entities.Payments.Payment>() ? 1 : 0))) != 0)
                  {
                    (ActionPrintViewModel.TypePrint type, bool isPrint) typePrint2 = new ActionPrintViewModel().GetTypePrint(this.Basket);
                    typePrint1 = typePrint2.type;
                    if (!typePrint2.isPrint)
                      return;
                  }
                  if (typePrint1.IsEither<ActionPrintViewModel.TypePrint>(ActionPrintViewModel.TypePrint.Fiscal, ActionPrintViewModel.TypePrint.NonFiscal))
                  {
                    this._type = typePrint1 == ActionPrintViewModel.TypePrint.Fiscal ? CheckFiscalTypes.Fiscal : CheckFiscalTypes.NonFiscal;
                    if (document2.Payments.Any<Gbs.Core.Entities.Payments.Payment>())
                      this.PrintCheck(document2);
                    if (document1.Payments.Any<Gbs.Core.Entities.Payments.Payment>())
                      this.PrintCheck(document1);
                  }
                  if (this.Basket.Document.Status == GlobalDictionaries.DocumentsStatuses.Close)
                  {
                    this.DeleteReserveGood();
                    BasketHelper basketHelper = new BasketHelper(this.CurrentBasket);
                    List<Gbs.Core.Entities.Payments.Payment> list3 = this.Payments.ToList<Gbs.Core.Entities.Payments.Payment>();
                    list3.ForEach((Action<Gbs.Core.Entities.Payments.Payment>) (x => x.Method.PaymentMethodsType = GlobalDictionaries.PaymentMethodsType.NoBroker));
                    this.CurrentBasket.PaymentsPrepaid = new List<Gbs.Core.Entities.Payments.Payment>((IEnumerable<Gbs.Core.Entities.Payments.Payment>) list3);
                    this.CurrentBasket.UpdateOrderList = this.UpdateOrderList;
                    this.CurrentBasket.Storage = this.Basket.Storage;
                    this.CurrentBasket.Client = new ClientsRepository(dataBase).GetClientByUidAndSum(this.Basket.Client.Client.Uid);
                    List<BasketItem> list4 = this.Basket.Items.ToList<BasketItem>();
                    Client client = this.Basket.Client.Client;
                    Guid uid = this.Basket.Document.Uid;
                    basketHelper.AddItemClientOrder(list4, client, uid);
                    this.CurrentBasket.ReCalcTotals();
                    Other.IsActiveAndShowForm<MainWindow>();
                  }
                  else
                    ClientOrderViewModel.ReserveGood(this.Basket.Document, this.OptionalOrder.IsReserveGood);
                  this.ResultSave = true;
                  WindowWithSize.IsCancel = false;
                  this.CloseAction();
                }
              }
            }
          }
        }));
      }
    }

    private void PrintCheck(Document document)
    {
      while (!Gbs.Core.ViewModels.Basket.Basket.PrintPreCheck(document, this.UserAuth, this._type))
      {
        switch (MessageBoxHelper.Show(Translate.Basket_Чек_не_удалось_распечатать__Попробовать_еще_раз_, buttons: MessageBoxButton.YesNoCancel, icon: MessageBoxImage.Hand))
        {
          case MessageBoxResult.Cancel:
            return;
          case MessageBoxResult.Yes:
            continue;
          case MessageBoxResult.No:
            return;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public Gbs.Core.ViewModels.Basket.Basket Basket { get; set; } = new Gbs.Core.ViewModels.Basket.Basket();

    public ICommand ClientSelectedCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (Client client, bool result) client1 = new FrmSearchClient().GetClient();
          Client client2 = client1.client;
          if (!client1.result)
            return;
          this.Basket.Client = new ClientAdnSum()
          {
            Client = client2
          };
          this.Basket.ReCalcTotals();
        }));
      }
    }

    public ICommand AddGoodCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.ClientOrder, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemInBasket), user: this.UserAuth);
          this.AddItemInBasket((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
        }));
      }
    }

    public ICommand AddPaymentCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (bool Result2, List<SelectPaymentMethods.PaymentGrid> ListPayment2, Decimal _) = new FrmInsertPaymentMethods().GetValuePayment(this.Basket.TotalSum, 0M, false, isPaymentPrepaid: true);
          if (!Result2)
            return;
          foreach (SelectPaymentMethods.PaymentGrid paymentGrid in ListPayment2)
            this.Payments.Add(new Gbs.Core.Entities.Payments.Payment()
            {
              Method = paymentGrid.Method,
              SumIn = paymentGrid.Sum.GetValueOrDefault(),
              AccountIn = PaymentsAccounts.GetPaymentsAccountByUid(paymentGrid.Method.AccountUid),
              Type = GlobalDictionaries.PaymentTypes.MoneyDocumentPayment,
              User = this.UserAuth
            });
          this.OnPropertyChanged("Payments");
          this.OnPropertyChanged("TotalPayment");
          this.OnPropertyChanged("TotalCredit");
        }));
      }
    }

    public ICommand DeletePayment
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<Gbs.Core.Entities.Payments.Payment> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Payments.Payment>().ToList<Gbs.Core.Entities.Payments.Payment>();
          if (!list.Any<Gbs.Core.Entities.Payments.Payment>())
          {
            MessageBoxHelper.Warning(Translate.WaybillCardViewModel_Требуется_выбрать_платежи_для_удаления);
          }
          else
          {
            if (MessageBoxHelper.Question(Translate.WaybillCardViewModelВыУвереныЧтоХотитеУдалитьВсеВыделенныеПлатежиWaybillCardViewModelВыУвереныЧтоХотитеУдалитьВсеВыделенныеПлатежи) == MessageBoxResult.No)
              return;
            foreach (Gbs.Core.Entities.Payments.Payment payment in list)
              this.Payments.Remove(payment);
            this.OnPropertyChanged("Payments");
            this.OnPropertyChanged("TotalPayment");
            this.OnPropertyChanged("TotalCredit");
          }
        }));
      }
    }

    public Decimal TotalPayment
    {
      get
      {
        ObservableCollection<Gbs.Core.Entities.Payments.Payment> payments = this.Payments;
        return payments == null ? 0M : payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn));
      }
    }

    public Decimal TotalCredit
    {
      get
      {
        Gbs.Core.ViewModels.Basket.Basket basket = this.Basket;
        return (basket != null ? basket.TotalSum : 0M) - this.TotalPayment;
      }
    }

    public Dictionary<GlobalDictionaries.DocumentsStatuses, string> DictionaryStatusOrder { get; } = GlobalDictionaries.ClientOrderStatusDictionary;

    private Gbs.Core.ViewModels.Basket.Basket CurrentBasket { get; set; }

    private bool ResultSave { get; set; }

    public Gbs.Core.Entities.Users.User UserAuth { get; set; }

    public bool IsReadOnlyIfClose
    {
      get
      {
        Document document = this.Basket.Document;
        return (document != null ? (int) document.Status : 2) != 3;
      }
    }

    public bool ShowOrder(Guid docUid, out Document document, Gbs.Core.Entities.Users.User authUser = null, Action action = null)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref authUser, docUid == Guid.Empty ? Gbs.Core.Entities.Actions.CreateClientOrder : Gbs.Core.Entities.Actions.EditClientOrder))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(docUid == Guid.Empty ? Gbs.Core.Entities.Actions.CreateClientOrder : Gbs.Core.Entities.Actions.EditClientOrder);
          if (!access.Result)
          {
            document = (Document) null;
            return false;
          }
          authUser = access.User;
        }
        this.UpdateOrderList = action;
        this.UserAuth = authUser;
        ref Document local = ref document;
        Document document1;
        if (!(docUid == Guid.Empty))
        {
          document1 = new DocumentsRepository(dataBase).GetByUid(docUid);
        }
        else
        {
          document1 = new Document();
          document1.Status = GlobalDictionaries.DocumentsStatuses.Open;
        }
        local = document1;
        this._documentClone = document.Clone<Document>();
        this.EntityClone = (IEntity) document.Clone<Document>();
        this.Basket.Document = document;
        this.Basket.Storage = document.Storage;
        this.Basket.DocumentsType = GlobalDictionaries.DocumentsTypes.ClientOrder;
        this.Payments = new ObservableCollection<Gbs.Core.Entities.Payments.Payment>(document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted)));
        if (document.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.OptionalClientOrderUid)))
        {
          this.OptionalOrder = JsonConvert.DeserializeObject<ClientOrderViewModel.OptionalClientOrder>(document.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.OptionalClientOrderUid)).Value.ToString());
          this._lastOptionalOrder = this.OptionalOrder.Clone<ClientOrderViewModel.OptionalClientOrder>();
        }
        if (document.ContractorUid != Guid.Empty)
          this.Basket.Client = new ClientAdnSum()
          {
            Client = new ClientsRepository(dataBase).GetByUid(document.ContractorUid)
          };
        this.IsEnabledOptionSavePriceValue = !document.Items.Any<Gbs.Core.Entities.Documents.Item>() || document.Items.Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodStock != null && x.GoodStock.Uid != Guid.Empty));
        this.OnPropertyChanged("IsEnabledOptionSavePriceValue");
        List<string> values = new List<string>()
        {
          Translate.ClientOrderViewModel_ShowOrder_Для_следующих_товаров_изменилась_розничная_цена_в_заказе_
        };
        foreach (Gbs.Core.Entities.Documents.Item obj in document.Items)
        {
          Gbs.Core.Entities.Documents.Item good = obj;
          Gbs.Core.Entities.Goods.Good good1 = good.Good;
          Guid modificationUid = good.ModificationUid;
          Decimal sellPrice = good.SellPrice;
          Storages.Storage storage = document.Storage;
          Decimal quantity = good.Quantity;
          Decimal discount = good.Discount;
          Guid uid = good.Uid;
          string comment = good.Comment;
          GoodsStocks.GoodStock goodStock1 = good.GoodStock;
          // ISSUE: explicit non-virtual call
          Guid goodStockUid = goodStock1 != null ? __nonvirtual (goodStock1.Uid) : Guid.Empty;
          BasketItem basketItem = new BasketItem(good1, modificationUid, sellPrice, storage, quantity, discount, uid, comment, goodStockUid);
          ClientOrderViewModel.OptionalClientOrder optionalOrder = this.OptionalOrder;
          if ((optionalOrder != null ? (!optionalOrder.IsSavePriceValue ? 1 : 0) : 0) != 0)
          {
            GoodsStocks.GoodStock goodStock2 = good.Good.StocksAndPrices.FirstOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Uid == good.GoodStock.Uid));
            Decimal num = goodStock2 != null ? goodStock2.Price : basketItem.SalePrice;
            if (num != basketItem.SalePrice)
              values.Add(string.Format(Translate.ClientOrderViewModel_ShowOrder_, (object) basketItem.DisplayedName, (object) basketItem.SalePrice, (object) num));
            basketItem.SalePrice = num;
          }
          this.Basket.Items.Add(basketItem);
        }
        if (values.Count > 1)
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Join("\n", (IEnumerable<string>) values)));
        this.FormToSHow = (WindowWithSize) new ClientOrderCard();
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        this.SetHotKeys();
        this.ShowForm();
        if (this.ResultSave)
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory(this.EntityClone, (IEntity) this.Basket.Document, docUid == Guid.Empty ? ActionType.Add : ActionType.Edit, GlobalDictionaries.EntityTypes.Document, authUser), false);
        return this.ResultSave;
      }
    }

    private void SetHotKeys()
    {
      try
      {
        ClientOrderCard frm = (ClientOrderCard) this.FormToSHow;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand1 = new RelayCommand((Action<object>) (obj => this.EditCommand.Execute((object) frm.GridClientOrder.SelectedItems)));
        RelayCommand relayCommand2 = new RelayCommand((Action<object>) (obj => this.DeleteCommand.Execute((object) frm.GridClientOrder.SelectedItems)));
        RelayCommand relayCommand3 = new RelayCommand((Action<object>) (obj => this.EditDiscountCommand.Execute((object) frm.GridClientOrder.SelectedItems)));
        RelayCommand relayCommand4 = new RelayCommand((Action<object>) (o => this.CloseAction()));
        this.FormToSHow.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.AddItem,
            this.AddGoodCommand
          },
          {
            hotKeys.EditItem,
            (ICommand) relayCommand1
          },
          {
            hotKeys.DiscountForItem,
            (ICommand) relayCommand3
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand4
          },
          {
            hotKeys.DeleteItem,
            (ICommand) relayCommand2
          },
          {
            new HotKeysHelper.Hotkey(Key.Return),
            (ICommand) relayCommand1
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand4
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public ICommand EditCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Basket.EditQuantityCommand.Execute(obj);
          this.OnPropertyChanged("Payments");
          this.OnPropertyChanged("TotalPayment");
          this.OnPropertyChanged("TotalCredit");
        }));
      }
    }

    public ICommand EditDiscountCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Basket.EditDiscountCommand.Execute(obj);
          this.OnPropertyChanged("Payments");
          this.OnPropertyChanged("TotalPayment");
          this.OnPropertyChanged("TotalCredit");
        }));
      }
    }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Basket.DeleteItemCommand.Execute(obj);
          this.OnPropertyChanged("Payments");
          this.OnPropertyChanged("TotalPayment");
          this.OnPropertyChanged("TotalCredit");
        }));
      }
    }

    private Action UpdateOrderList { get; set; }

    private void CreateDocOrder(Document document)
    {
      document.Type = GlobalDictionaries.DocumentsTypes.ClientOrder;
      document.Storage = this.Basket.Storage;
      Document document1 = document;
      if (document1.Section == null)
        document1.Section = Sections.GetCurrentSection();
      Document document2 = document;
      Guid guid1;
      if (!(this.Basket.Document.UserUid == Guid.Empty))
      {
        guid1 = this.Basket.Document.UserUid;
      }
      else
      {
        Gbs.Core.Entities.Users.User userAuth = this.UserAuth;
        // ISSUE: explicit non-virtual call
        guid1 = userAuth != null ? __nonvirtual (userAuth.Uid) : Guid.Empty;
      }
      document2.UserUid = guid1;
      document.Number = this.Basket.Document.Number.IsNullOrEmpty() ? Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.ClientOrder) : this.Basket.Document.Number;
      Document document3 = document;
      DateTime dateTime1 = this.Basket.Document.DateTime;
      DateTime dateTime2;
      if (!(dateTime1.Date == DateTime.Today))
      {
        dateTime1 = this.Basket.Document.DateTime;
        dateTime2 = dateTime1.Date;
      }
      else
        dateTime2 = DateTime.Now;
      document3.DateTime = dateTime2;
      Document document4 = document;
      ClientAdnSum client = this.Basket.Client;
      Guid guid2 = client != null ? client.Client.Uid : Guid.Empty;
      document4.ContractorUid = guid2;
    }

    private void AddItemInBasket(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool addAllCount = false, bool checkMinus = true)
    {
      new BasketHelper(this.Basket).AddItemToBasket(goods, addAllCount, checkMinus);
    }

    private void UpdateDocument()
    {
      List<Gbs.Core.Entities.Documents.Item> objList1 = new List<Gbs.Core.Entities.Documents.Item>();
      foreach (BasketItem basketItem in (Collection<BasketItem>) this.Basket.Items)
      {
        List<Gbs.Core.Entities.Documents.Item> objList2 = objList1;
        Gbs.Core.Entities.Documents.Item obj = new Gbs.Core.Entities.Documents.Item(basketItem, this.Basket.Document.Uid);
        GoodsStocks.GoodStock goodStock = new GoodsStocks.GoodStock();
        goodStock.Uid = basketItem.GoodStockUid;
        obj.GoodStock = goodStock;
        objList2.Add(obj);
      }
      foreach (Gbs.Core.Entities.Documents.Item obj in this.Basket.Document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => this.Basket.Items.All<BasketItem>((Func<BasketItem, bool>) (g => g.Uid != x.Uid)))))
      {
        obj.IsDeleted = true;
        objList1.Add(obj);
      }
      this.Basket.Document.Items = objList1;
      List<Gbs.Core.Entities.Payments.Payment> paymentList = new List<Gbs.Core.Entities.Payments.Payment>();
      foreach (Gbs.Core.Entities.Payments.Payment payment in (Collection<Gbs.Core.Entities.Payments.Payment>) this.Payments)
        paymentList.Add(payment);
      foreach (Gbs.Core.Entities.Payments.Payment payment in this.Basket.Document.Payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => this.Payments.All<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (g => g.Uid != x.Uid)))))
      {
        payment.IsDeleted = true;
        paymentList.Add(payment);
      }
      this.Basket.Document.Payments = paymentList;
    }

    public ObservableCollection<Gbs.Core.Entities.Payments.Payment> Payments { get; set; } = new ObservableCollection<Gbs.Core.Entities.Payments.Payment>();

    public IEntity EntityClone { get; set; }

    public Visibility PaymentsTabVisibility => Visibility.Visible;

    public bool HasNoSavedChanges()
    {
      this.UpdateDocument();
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Basket.Document);
    }

    public static void ReserveGood(Document document, bool isReserveGood)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Document document1 = new DocumentsRepository(dataBase).GetByParentUid(document.Uid).SingleOrDefault<Document>();
        if (document1 != null)
          new DocumentsRepository(dataBase).Delete(document1);
        if (!isReserveGood)
          return;
        Document document2 = document.Clone<Document>();
        document2.Payments.Clear();
        // ISSUE: explicit non-virtual call
        document2.Uid = document1 != null ? __nonvirtual (document1.Uid) : Guid.NewGuid();
        document2.ParentUid = document.Uid;
        document2.Type = GlobalDictionaries.DocumentsTypes.ClientOrderReserve;
        document2.Status = GlobalDictionaries.DocumentsStatuses.Close;
        new DocumentsRepository(dataBase).Save(document2);
      }
    }

    private void DeleteReserveGood()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Document> byParentUid = new DocumentsRepository(dataBase).GetByParentUid(this.Basket.Document.Uid);
        if (byParentUid == null)
          return;
        new DocumentsRepository(dataBase).Delete(byParentUid);
        List<Gbs.Core.Entities.Goods.Good> goodList = CachesBox.AllGoods();
        foreach (Gbs.Core.Entities.Documents.Item obj in byParentUid.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items)))
        {
          Gbs.Core.Entities.Goods.Good good = new GoodRepository(dataBase).GetByUid(obj.GoodUid);
          if (goodList.Any<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == good.Uid)))
            goodList[goodList.FindIndex((Predicate<Gbs.Core.Entities.Goods.Good>) (x => x.Uid == good.Uid))] = good;
        }
        CacheHelper.UpdateCache<List<Gbs.Core.Entities.Goods.Good>>(CacheHelper.CacheTypes.AllGoods, goodList);
      }
    }

    public class OptionalClientOrder
    {
      public bool IsSavePriceValue { get; set; } = true;

      public DateTime? ActualityOrderDate { get; set; }

      public bool IsActualityOrder { get; set; }

      public bool IsReserveGood { get; set; }
    }
  }
}
