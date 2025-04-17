// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Barcodes.BarcodeSearcher
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers.Extensions.Linq;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Windows;

#nullable disable
namespace Gbs.Helpers.Barcodes
{
  internal class BarcodeSearcher
  {
    private readonly Gbs.Core.ViewModels.Basket.Basket _basket;
    private readonly List<Gbs.Core.Entities.Goods.Good> _goodList;
    private readonly BarcodeScanner _scannerConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner;

    public BarcodeSearcher(Gbs.Core.ViewModels.Basket.Basket basket, List<Gbs.Core.Entities.Goods.Good> goodList)
    {
      this._basket = basket;
      this._goodList = goodList;
    }

    private bool CheckPrefix(string query, string prefix)
    {
      return ((IEnumerable<string>) prefix.Split(GlobalDictionaries.SplitArr)).Where<string>((Func<string, bool>) (x => x.Trim().Length > 0)).Any<string>(new Func<string, bool>(query.StartsWith));
    }

    private BarcodeSearcher.BarcodeTypes GetBarcodeType(string barcode)
    {
      bool flag = ((IEnumerable<char>) barcode.ToCharArray()).All<char>(new Func<char, bool>(char.IsDigit));
      if (barcode.Length > 8 && (flag || this._scannerConfig.AllowUseAlphabetBarcodes))
      {
        if (this.CheckPrefix(barcode, this._scannerConfig.Prefixes.DiscountCard))
          return BarcodeSearcher.BarcodeTypes.ClientsDiscountCard;
        if (this.CheckPrefix(barcode, this._scannerConfig.Prefixes.Certificates))
          return BarcodeSearcher.BarcodeTypes.GiftCertificate;
        if (this.CheckPrefix(barcode, this._scannerConfig.Prefixes.Users))
          return BarcodeSearcher.BarcodeTypes.UsersBarcode;
        if (BarcodeHelper.IsEan13Barcode(barcode) && this.CheckPrefix(barcode, this._scannerConfig.Prefixes.WeightGoods))
          return BarcodeSearcher.BarcodeTypes.WeightGood;
        if (this.CheckPrefix(barcode, this._scannerConfig.Prefixes.Modifications))
          return BarcodeSearcher.BarcodeTypes.RangeModification;
      }
      if (barcode.Length > 16 && barcode.StartsWith("010"))
        return BarcodeSearcher.BarcodeTypes.RuMarkedCode;
      return barcode.Length.IsEither<int>(21, 25, 29) && (barcode.Length == 29 && BarcodeHelper.IsEan13Barcode(barcode.Remove(14).Remove(0, 1)) || barcode.StartsWith("000000")) ? BarcodeSearcher.BarcodeTypes.RuMarkedCodeTobacco : BarcodeSearcher.BarcodeTypes.Unknown;
    }

    public bool SearchByBarcodeAndAddToBasket(string query, Action scrollToSelected = null)
    {
      switch (this.GetBarcodeType(query))
      {
        case BarcodeSearcher.BarcodeTypes.ClientsDiscountCard:
          this.SearchClient(query);
          return true;
        case BarcodeSearcher.BarcodeTypes.GiftCertificate:
          this.SearchCertificate(query);
          return true;
        case BarcodeSearcher.BarcodeTypes.UsersBarcode:
          int num = (int) MessageBoxHelper.Show(string.Format(Translate.MainWindowViewModel_неудалось_найтитовар, (object) query) + Translate.MainWindowViewModel_Измените_штрих_код_для_товара__);
          return true;
        case BarcodeSearcher.BarcodeTypes.WeightGood:
          if (this.SearchWeightGoods(query, this._goodList))
            return true;
          break;
        case BarcodeSearcher.BarcodeTypes.RangeModification:
          if (this.SearchModifications(query, this._goodList))
          {
            if (scrollToSelected != null)
              scrollToSelected();
            return true;
          }
          break;
        case BarcodeSearcher.BarcodeTypes.RuMarkedCode:
          if (this.SearchGoodsByMarkedCode(query, this._goodList))
          {
            if (scrollToSelected != null)
              scrollToSelected();
            return true;
          }
          break;
        case BarcodeSearcher.BarcodeTypes.RuMarkedCodeTobacco:
          if (this.SearchGoodsByTobaccoMarkedCode(query, this._goodList))
          {
            if (scrollToSelected != null)
              scrollToSelected();
            return true;
          }
          break;
      }
      return false;
    }

    private void SearchClient(string barcode)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ClientsRepository clientsRepository = new ClientsRepository(dataBase);
        Client byBarcode = clientsRepository.GetByBarcode(barcode);
        Guid uid = Guid.Empty;
        if (byBarcode != null)
        {
          uid = byBarcode.Uid;
        }
        else
        {
          ClientsExchangeHelper.GetCashClient();
          ClientCloud clientCloud = ClientsExchangeHelper.Search(barcode);
          if (clientCloud != null)
          {
            Client client = ClientsExchangeHelper.SaveClient(clientCloud);
            if (client != null)
              uid = client.Uid;
          }
        }
        if (uid != Guid.Empty)
        {
          this._basket.IsCheckedClient = true;
          this._basket.Client = clientsRepository.GetClientByUidAndSum(uid);
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.MainWindowViewModel_Покупатель_с_таким_штрих_кодом_не_найден + ": " + barcode);
        }
      }
    }

    private void SearchCertificate(string barcode)
    {
      Performancer performancer = new Performancer("Ищем сертификат запросом при продаже");
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<GoodsCertificate.Certificate> list = GoodsCertificate.GetCertificateFilteredList(dataBase.GetTable<CERTIFICATES>().Where<CERTIFICATES>((Expression<Func<CERTIFICATES, bool>>) (x => x.BARCODE == barcode && !x.IS_DELETED))).ToList<GoodsCertificate.Certificate>();
        if (list.Any<GoodsCertificate.Certificate>())
        {
          foreach (GoodsCertificate.Certificate certificate in list)
          {
            GoodsCertificate.Certificate c = certificate;
            GOODS goods = dataBase.GetTable<GOODS>().SingleOrDefault<GOODS>((Expression<Func<GOODS, bool>>) (x => x.UID == c.Stock.GoodUid));
            Gbs.Core.Entities.Goods.Good good = this._goodList.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == c.Stock.GoodUid));
            if ((goods != null ? (!goods.IS_DELETED ? 1 : 0) : 0) != 0 && good != null)
            {
              Application.Current?.Dispatcher?.Invoke((Action) (() => new BasketHelper(this._basket).ActivatedCertificate(c, good)));
            }
            else
            {
              int num = (int) MessageBoxHelper.Show(Translate.MainWindowViewModel_Сертификат_с_таким_штрих_кодом_не_найден_);
            }
          }
        }
        else
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.MainWindowViewModel_Сертификат_с_таким_штрих_кодом_не_найден_);
        }
        performancer.Stop();
      }
    }

    private bool SearchWeightGoods(string barcode, List<Gbs.Core.Entities.Goods.Good> goods)
    {
      if (!BarcodeHelper.IsEan13Barcode(barcode))
        return false;
      (Gbs.Core.Entities.Goods.Good g, Decimal w) goodsWeight = BarcodeHelper.GetWeightItem(barcode, goods);
      if (goodsWeight.g == null)
        return false;
      Application.Current?.Dispatcher?.Invoke((Action) (() => new BasketHelper(this._basket).GetStock(goodsWeight.g, false, count: new Decimal?(goodsWeight.w), modificationUid: string.Empty, comment: string.Empty, isWeightPlu: true)));
      return true;
    }

    private bool SearchModifications(string barcode, List<Gbs.Core.Entities.Goods.Good> goods)
    {
      Gbs.Core.Entities.Goods.Good singleGood = goods.SingleOrNull<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.SetStatus == GlobalDictionaries.GoodsSetStatuses.Range && x.Modifications.Any<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (m => m.Barcode == barcode))));
      if (singleGood == null)
        return false;
      GoodsModifications.GoodModification singleModification = singleGood.Modifications.SingleOrNull<GoodsModifications.GoodModification>((Func<GoodsModifications.GoodModification, bool>) (x => x.Barcode == barcode));
      if (singleModification == null)
        return false;
      Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        BasketHelper basketHelper = new BasketHelper(this._basket);
        Gbs.Core.Entities.Goods.Good good = singleGood;
        Decimal? count = new Decimal?();
        Guid guid = singleModification.Uid;
        string modificationUid = guid.ToString();
        guid = new Guid();
        Guid tapUid = guid;
        basketHelper.GetStock(good, false, count: count, modificationUid: modificationUid, tapUid: tapUid);
      }));
      return true;
    }

    private bool SearchGoodsByMarkedCode(string query, List<Gbs.Core.Entities.Goods.Good> goods)
    {
      if (query.Length < 16)
        return false;
      LogHelper.Debug("Возможно просканирована КМ, проверяем");
      string barcode = query.Substring(3, 13);
      if (!BarcodeHelper.IsEan13Barcode(barcode))
        return false;
      LogHelper.Debug("КМ разобрана, поиск по ШК " + barcode);
      return this.AddSearchItemByMarkedCode(barcode, query, goods);
    }

    private bool SearchGoodsByTobaccoMarkedCode(string query, List<Gbs.Core.Entities.Goods.Good> goods)
    {
      if (!query.Length.IsEither<int>(21, 25, 29))
        return false;
      LogHelper.Debug("Возможно просканирована КМ сигарет, проверяем");
      string str = "";
      if (query.Length == 29 && query.StartsWith("000000"))
      {
        str = query.Substring(6, 8);
        if (!BarcodeHelper.IsEan8Barcode(str))
          str = "";
      }
      if (str.IsNullOrEmpty())
      {
        str = query.Remove(14).Remove(0, 1);
        if (!BarcodeHelper.IsEan13Barcode(str))
          return false;
      }
      LogHelper.Debug("КМ разобрана, поиск по ШК " + str);
      return this.AddSearchItemByMarkedCode(str, query, goods);
    }

    private bool AddSearchItemByMarkedCode(string barcode, string query, List<Gbs.Core.Entities.Goods.Good> goods)
    {
      Gbs.Core.Entities.Goods.Good goodForMark = goods.SingleOrNull<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcode.ToLower().Trim() == barcode && x.Group.RuMarkedProductionType != 0));
      if (goodForMark == null)
      {
        goodForMark = goods.SingleOrNull<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcodes.Any<string>((Func<string, bool>) (b => b.ToLower().Trim() == barcode)) && x.Group.RuMarkedProductionType != 0));
        if (goodForMark == null)
          return false;
      }
      this._basket.IsNeedComment = false;
      if (Regex.IsMatch(query, "\\p{IsCyrillic}"))
      {
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.BarcodeSearcher_Некорректно_отсканирован_код_маркировки__отсканируйте_код_повторно_в_открытой_форме_));
        this._basket.IsNeedComment = true;
        query = string.Empty;
      }
      Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        new BasketHelper(this._basket).GetStock(goodForMark, false, count: new Decimal?((Decimal) 1), comment: query);
        this._basket.IsNeedComment = true;
        this._basket.ShowCommentNotifications();
      }));
      return true;
    }

    private enum BarcodeTypes
    {
      Unknown,
      ClientsDiscountCard,
      GiftCertificate,
      UsersBarcode,
      WeightGood,
      RangeModification,
      RuMarkedCode,
      RuMarkedCodeTobacco,
    }
  }
}
