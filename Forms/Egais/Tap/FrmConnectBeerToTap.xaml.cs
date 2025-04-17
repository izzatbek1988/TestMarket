// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.ConnectBeerToTapViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.GoodGroups;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using LinqToDB.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Egais
{
  public partial class ConnectBeerToTapViewModel : ViewModelWithForm
  {
    private CultureInfo _originalLang;
    private bool _isDownCaps;
    private bool _isResult;
    private Gbs.Core.Entities.Users.User _authUser;
    private Gbs.Core.Entities.Goods.Good _oldBeerItem;

    private void SetLangOnEn()
    {
      this._originalLang = InputLanguageManager.Current.CurrentInputLanguage;
      if ((Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) == KeyStates.Toggled)
      {
        this._isDownCaps = true;
        KeyboardLayoutHelper.IsDownCapsLock();
      }
      IEnumerable availableInputLanguages = InputLanguageManager.Current.AvailableInputLanguages;
      CultureInfo cultureInfo = availableInputLanguages != null ? availableInputLanguages.OfType<CultureInfo>().FirstOrDefault<CultureInfo>((Func<CultureInfo, bool>) (l => l.Name.StartsWith("en"))) : (CultureInfo) null;
      if (cultureInfo == null)
        return;
      LogHelper.Debug("Устанавливаю EN раскладку для ввода кода маркировки");
      InputLanguageManager.Current.CurrentInputLanguage = cultureInfo;
    }

    private void SetLangOnDefault()
    {
      if (this._originalLang != null)
        InputLanguageManager.Current.CurrentInputLanguage = this._originalLang;
      if (!this._isDownCaps || (Keyboard.GetKeyStates(Key.Capital) & KeyStates.Toggled) == KeyStates.Toggled)
        return;
      KeyboardLayoutHelper.IsDownCapsLock();
    }

    public bool Show(InfoToTapBeer info, Gbs.Core.Entities.Users.User authUser)
    {
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref authUser, Actions.ActionsToBeerTap))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.ActionsToBeerTap);
          if (!access.Result)
            return false;
          authUser = access.User;
        }
        this._authUser = authUser;
        this.Info = info;
        this.EntityClone = (IEntity) info.Clone<InfoToTapBeer>();
        this.Storages = new List<Gbs.Core.Entities.Storages.Storage>(Gbs.Core.Entities.Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => !x.IS_DELETED))));
        if (this.Storages.Count == 1)
          this.Storage = this.Storages.Single<Gbs.Core.Entities.Storages.Storage>();
        if (info.Uid != Guid.Empty)
        {
          this.VisibilityForCrpt = Visibility.Visible;
          this.IsEnabledInfo = false;
          this.IsEnabledMarkedInfo = !info.IsSendToCrpt;
          this.Good = new GoodRepository(dataBase).GetByUid(this.Info.GoodUid);
          this.Storage = this.Storages.SingleOrDefault<Gbs.Core.Entities.Storages.Storage>((Func<Gbs.Core.Entities.Storages.Storage, bool>) (x => x.Uid == info.StorageUid));
        }
        else
          this.Info.Uid = Guid.NewGuid();
        this.OnPropertyChanged("VisibilityStorage");
        this.FormToSHow = (WindowWithSize) new FrmConnectBeerToTap();
        this.SetLangOnEn();
        this.ShowForm();
        this.SetLangOnDefault();
        return this._isResult;
      }
    }

    public Gbs.Core.Entities.Storages.Storage Storage { get; set; }

    public InfoToTapBeer Info { get; set; }

    public Gbs.Core.Entities.Goods.Good Good { get; set; }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          InfoToTapBeer info = this.Info;
          Gbs.Core.Entities.Goods.Good good = this.Good;
          Guid guid = good != null ? __nonvirtual (good.Uid) : Guid.Empty;
          info.GoodUid = guid;
          if (!this.ValidateInfo() || !this.DoOpeningBeer())
            return;
          this.Info.ConnectingDateTime = new DateTime?(DateTime.Now);
          this.Info.StorageUid = this.Storage.Uid;
          this.Info.ChildGoodUid = this.Info.ChildGoodUid == Guid.Empty ? this._oldBeerItem.Uid : this.Info.ChildGoodUid;
          this._isResult = new InfoTapBeerRepository().Save(this.Info);
          WindowWithSize.IsCancel = false;
          this.CloseAction();
        }));
      }
    }

    private bool ValidateInfo()
    {
      if (this.Good != null && this.IsEnabledInfo)
      {
        List<InfoToTapBeer> byGoodUid = new InfoTapBeerRepository().GetByGoodUid(this.Good.Uid);
        if ((byGoodUid != null ? (byGoodUid.Any<InfoToTapBeer>() ? 1 : 0) : 0) != 0 && MessageBoxHelper.Question("Выбранная продукция уже вскрыта и установлена на другом кране.\n\nВы уверены, что хотите вскрыть еще один кег такого же вида пива?") == MessageBoxResult.No)
          return false;
      }
      if (this.Good != null && this.IsEnabledInfo && new ConfigsRepository<Integrations>().Get().Egais.IsBanOpenNegativeBeerKega)
      {
        Decimal num1 = this.Good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Stock > 0M)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
        if (num1 == 0M)
        {
          MessageBoxHelper.Warning("Выбранный кег отсутсвует на остатке, вскрытие запрещено настройками программы.\n\nИзмените настройки вскрытия или выберите другой товар (кег), а после повторите попытку вскрытия.");
          return false;
        }
        Decimal num2 = num1 * 10M;
        Decimal? quantity = this.Info.Quantity;
        Decimal valueOrDefault = quantity.GetValueOrDefault();
        if (num2 < valueOrDefault & quantity.HasValue)
        {
          MessageBoxHelper.Warning("Остаток выбраннного кега меньше заявленного объема, вскрытие запрещено настройками программы.\n\nИзмените настройки вскрытия или выберите другой товар (кег), а после повторите попытку вскрытия.");
          return false;
        }
      }
      if (this.Storage == null)
      {
        MessageBoxHelper.Warning("Необходимо указать склад для вскрытия кеги, и после этого повторите попытку вскрытия.");
        return false;
      }
      ActionResult actionResult = new InfoTapBeerRepository().Validate(this.Info);
      if (actionResult.Result != ActionResult.Results.Ok)
      {
        MessageBoxHelper.Warning("Допущены ошибки при вскрытии кеги, для сохранения необходимо:\n\n" + string.Join("\n", (IEnumerable<string>) actionResult.Messages));
        return false;
      }
      DateTime? expirationDate;
      if (this.IsEnabledInfo)
      {
        expirationDate = this.Info.ExpirationDate;
        if (((expirationDate ?? DateTime.Now) - DateTime.Now).Days > 90)
        {
          MessageBoxHelper.Warning("Указан некорректный срок годности кеги, скорректируйте значение и повторите попытку вскрытия.");
          return false;
        }
      }
      if (this.IsEnabledInfo)
      {
        expirationDate = this.Info.ExpirationDate;
        if ((expirationDate ?? DateTime.Now).Date < DateTime.Now.Date)
        {
          MessageBoxHelper.Warning("Указан некорректный срок годности кеги (в прошлом), скорректируйте значение и повторите попытку вскрытия.");
          return false;
        }
      }
      if (this.IsEnabledInfo)
      {
        Decimal? quantity = this.Info.Quantity;
        Decimal num = (Decimal) 100;
        if (quantity.GetValueOrDefault() > num & quantity.HasValue && MessageBoxHelper.Question(string.Format("Указан слишком большой объем кеги, продолжить вскрытие с объемом {0:N2} литров?", (object) this.Info.Quantity)) == MessageBoxResult.No)
          return false;
      }
      this.Info.MarkedInfo = this.Info.MarkedInfo?.Trim() ?? string.Empty;
      return true;
    }

    public bool IsEnabledInfo { get; set; } = true;

    public bool IsEnabledMarkedInfo { get; set; } = true;

    public ICommand GetGoodCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.BeerProductionList, isVisNullStock: true, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.GetGoodForTapBeer));
          this.GetGoodForTapBeer((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
        }));
      }
    }

    private void GetGoodForTapBeer(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool allCount = false, bool checkMinus = true)
    {
      List<Gbs.Core.Entities.Goods.Good> list = goods.ToList<Gbs.Core.Entities.Goods.Good>();
      if (!list.Any<Gbs.Core.Entities.Goods.Good>())
        return;
      if (list.Count<Gbs.Core.Entities.Goods.Good>() != 1)
      {
        MessageBoxHelper.Warning("Необходимо выбрать только одну пивную кегу для вскрытия.");
      }
      else
      {
        this.Good = list.Single<Gbs.Core.Entities.Goods.Good>().Clone<Gbs.Core.Entities.Goods.Good>();
        this.GetOldBeerItem();
        this.OnPropertyChanged(isUpdateAllProp: true);
      }
    }

    private void GetOldBeerItem()
    {
      if (!this.GoodsInDb.Any<Gbs.Core.Entities.Goods.Good>())
      {
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          this.GoodsInDb = new GoodRepository(dataBase).GetByQuery(dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.SET_STATUS == 4 || x.SET_STATUS == 0)));
      }
      this._oldBeerItem = this.GoodsInDb.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.SetContent.Any<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (s => s.GoodUid == this.Good.Uid)) && !x.IsDeleted));
      if (this._oldBeerItem == null)
        return;
      this.Info.Price = new Decimal?(this._oldBeerItem.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? this._oldBeerItem.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price)) : 0M);
    }

    private List<Gbs.Core.Entities.Goods.Good> GoodsInDb { get; set; } = new List<Gbs.Core.Entities.Goods.Good>();

    public string TextForOkButton => !this.IsEnabledInfo ? "СОХРАНИТЬ" : "ВСКРЫТЬ";

    public string TextForCrpt
    {
      get
      {
        InfoToTapBeer info = this.Info;
        return (info != null ? (info.IsSendToCrpt ? 1 : 0) : 0) == 0 ? "Не отправлено в Честный знак" : "Отправлено в Честный знак";
      }
    }

    public string ColorForCrpt
    {
      get
      {
        InfoToTapBeer info = this.Info;
        return (info != null ? (info.IsSendToCrpt ? 1 : 0) : 0) == 0 ? "DarkRed" : "Green";
      }
    }

    public Visibility VisibilityForCrpt { get; set; } = Visibility.Collapsed;

    public bool DoOpeningBeer()
    {
      if (this.IsEnabledInfo)
      {
        bool flag = false;
        BasketItem basketItem1 = new BasketItem(this.Good.Clone<Gbs.Core.Entities.Goods.Good>(), Guid.Empty, this.Info.Price.GetValueOrDefault(), this.Storage, this.Info.Quantity.GetValueOrDefault(), guid: Guid.NewGuid(), comment: this.Info.MarkedInfo);
        BasketItem basketItem2 = basketItem1.Clone();
        Gbs.Core.Entities.GoodGroups.Group group1 = (Gbs.Core.Entities.GoodGroups.Group) null;
        this.GetOldBeerItem();
        if (this._oldBeerItem == null)
        {
          while (group1 == null)
          {
            Gbs.Core.Entities.GoodGroups.Group group2;
            if (!new FormSelectGroup().GetSingleSelectedGroupUid(this._authUser, out group2))
              return false;
            if (group2.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service))
              MessageBoxHelper.Warning("Для позиций из ЕГАИС можно выбрать категорию только с типом товаров: обычные или весовые.");
            else
              group1 = group2;
          }
          Guid guid = Guid.NewGuid();
          Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good();
          good.Uid = guid;
          good.Name = this.Good.Name + " [1 литр]";
          good.SetContent = (IEnumerable<GoodsSets.Set>) new GoodsSets.Set[1]
          {
            new GoodsSets.Set()
            {
              GoodUid = this.Good.Uid,
              Quantity = 0.1M,
              ParentUid = guid
            }
          };
          good.SetStatus = GlobalDictionaries.GoodsSetStatuses.Production;
          good.Group = group1;
          this._oldBeerItem = good;
          flag = true;
          this.GoodsInDb.Add(this._oldBeerItem);
        }
        basketItem1.Good = this._oldBeerItem;
        LogHelper.Debug("Вскрытие пивной кеги " + basketItem1.DisplayedName);
        (Document parent, List<Document> documentList) = this.PrepareDocuments(basketItem1);
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
          if (!new DocumentsRepository(dataBase).Save(parent))
          {
            connectionTransaction.Rollback();
            return false;
          }
          if (new DocumentsRepository(dataBase).Save(documentList) != documentList.Count)
          {
            connectionTransaction.Rollback();
            return false;
          }
          LogHelper.Debug("Успешно сохранены дочерние документы вскрытия кеги");
          if (flag && !new GoodRepository(dataBase).Save(this._oldBeerItem))
          {
            connectionTransaction.Rollback();
            return false;
          }
          connectionTransaction.Commit();
          ProductionCardViewModel.DoWriteOffBeerKega(basketItem2, documentList.Single<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.BeerProductionSet)));
          this.Info.DocumentUid = parent.Uid;
        }
      }
      if (!this.Info.IsSendToCrpt && MessageBoxHelper.Question("Отправить информацию о вскрытии кеги в Честный знак?") == MessageBoxResult.Yes)
        this.Info.IsSendToCrpt = this.ConnectTapToTrueApi();
      return true;
    }

    private (Document parent, List<Document> childDocs) PrepareDocuments(BasketItem item)
    {
      Guid guid = Guid.NewGuid();
      Document source = new Document();
      source.Storage = this.Storage;
      Gbs.Core.Entities.Users.User authUser = this._authUser;
      // ISSUE: explicit non-virtual call
      source.UserUid = authUser != null ? __nonvirtual (authUser.Uid) : Guid.Empty;
      source.Status = GlobalDictionaries.DocumentsStatuses.Close;
      Document document1 = source.Clone<Document>();
      document1.Uid = Guid.NewGuid();
      document1.ParentUid = guid;
      document1.Type = GlobalDictionaries.DocumentsTypes.BeerProductionItem;
      document1.Items = new List<Gbs.Core.Entities.Documents.Item>()
      {
        new Gbs.Core.Entities.Documents.Item()
        {
          SellPrice = item.SalePrice,
          Quantity = item.Quantity,
          Good = item.Good
        }
      };
      LogHelper.Debug("Создаем документ для добавления остатков произведенного пива" + document1.ToJsonString(true));
      Document document2 = source.Clone<Document>();
      document1.Uid = Guid.NewGuid();
      document2.ParentUid = document1.Uid;
      document2.Type = GlobalDictionaries.DocumentsTypes.BeerProductionSet;
      document2.Items = item.Good.SetContent.Select<GoodsSets.Set, Gbs.Core.Entities.Documents.Item>((Func<GoodsSets.Set, Gbs.Core.Entities.Documents.Item>) (x => new Gbs.Core.Entities.Documents.Item()
      {
        Good = this.GoodsInDb.Single<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (s => s.Uid == x.GoodUid)),
        Quantity = item.Quantity / 10M,
        SellPrice = item.SalePrice
      })).ToList<Gbs.Core.Entities.Documents.Item>();
      LogHelper.Debug("Создаем документ для списания остатков из кеги пива" + document2.ToJsonString(true));
      Document document3 = source.Clone<Document>();
      document3.Uid = guid;
      document3.Type = GlobalDictionaries.DocumentsTypes.BeerProductionList;
      document3.Number = Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.BeerProductionList);
      return (document3, new List<Document>()
      {
        document1,
        document2
      });
    }

    private bool ConnectTapToTrueApi()
    {
      LogHelper.Debug("Начинаем отправлять данные ЧЗ: " + this.Info.ToJsonString(true));
      string fullCode = this.Info.MarkedInfo;
      int startIndex = this.Info.MarkedInfo.LastIndexOf("93", StringComparison.InvariantCultureIgnoreCase);
      if (startIndex > 0)
        fullCode = this.Info.MarkedInfo.Remove(startIndex);
      MarkedInfo markedInfo = new MarkedInfo(fullCode, GlobalDictionaries.RuMarkedProductionTypes.Alcohol);
      List<TrueApiHelper.ConnectTapDocument.CodeBeer> codes = new List<TrueApiHelper.ConnectTapDocument.CodeBeer>()
      {
        new TrueApiHelper.ConnectTapDocument.CodeBeer()
        {
          Cis = markedInfo.FullCode,
          ConnectDate = DateTime.Now.ToString("yyyy-MM-dd")
        }
      };
      List<string> listError;
      while (this.CheckCode(this.Info.MarkedInfo, out bool _, out listError))
      {
        if (DevelopersHelper.IsDebug() || TrueApiRepository.ConnectTapBeerKega(codes))
          return true;
        if (MessageBoxHelper.Show("Не удалось отправить информацию о вскрытии кеги в Честный знак. Необходимо выполнить вскрытие самостоятельно через личный кабинет Честного знака. Попробовать еще раз?", buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Hand) == MessageBoxResult.No)
          return false;
      }
      MessageBoxHelper.Warning("Указанный код не прошел проверку в системе Честный знак. Убедитесь в корректности указанного кода маркировки и повторите проверку.\n\nПричины:\n" + string.Join("\n", (IEnumerable<string>) listError));
      return false;
    }

    public List<Gbs.Core.Entities.Storages.Storage> Storages { get; set; } = new List<Gbs.Core.Entities.Storages.Storage>();

    public Visibility VisibilityStorage
    {
      get => this.Storages.Count != 1 ? Visibility.Visible : Visibility.Collapsed;
    }

    public ICommand CheckMarkInfoInCrpt
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Info.MarkedInfo.IsNullOrEmpty())
          {
            MessageBoxHelper.Warning("Перед проверкой кода маркировки в Честном знаке, требуется указать код в данном окне.");
          }
          else
          {
            bool isExceptionConnect;
            List<string> listError;
            bool flag = this.CheckCode(this.Info.MarkedInfo, out isExceptionConnect, out listError);
            if (isExceptionConnect)
              MessageBoxHelper.Warning("Не удалось выполнить проверку кода маркировки в Честном знаке. Повторите попытку позже или обратитесь в техническую поддержку.");
            else if (listError.Any<string>() || !flag)
            {
              MessageBoxHelper.Warning("Указанный код не прошел проверку в системе Честный знак. Убедитесь в корректности указанного кода маркировки и повторите проверку.\n\nПричины:\n" + string.Join("\n", (IEnumerable<string>) listError));
            }
            else
            {
              int num = (int) MessageBoxHelper.Show("Указанный код успешно прошел проверку в системе Честный знак.");
            }
          }
        }));
      }
    }

    private bool CheckCode(string code, out bool isExceptionConnect, out List<string> listError)
    {
      return TrueApiRepository.CheckCode(new MarkedInfo(code, GlobalDictionaries.RuMarkedProductionTypes.Alcohol).FullCode, out listError, GlobalDictionaries.DocumentsTypes.BeerProductionList, out isExceptionConnect, out bool _, out Decimal _, false);
    }

    public void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Other.IsActiveForm<FrmConnectBeerToTap>())
        return;
      this.Info.MarkedInfo = barcode;
      this.OnPropertyChanged(isUpdateAllProp: true);
    }

    public ICommand SetCountByBtn
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Info.Quantity = new Decimal?(Convert.ToDecimal(obj.ToString()));
          this.OnPropertyChanged(isUpdateAllProp: true);
        }));
      }
    }

    public ICommand SetDateByBtn
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Info.ExpirationDate = new DateTime?((this.Info.ExpirationDate ?? DateTime.Now).AddDays((double) Convert.ToInt32(obj.ToString())));
          this.OnPropertyChanged(isUpdateAllProp: true);
        }));
      }
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Info);
    }
  }
}
