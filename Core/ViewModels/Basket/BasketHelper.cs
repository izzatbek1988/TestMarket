// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Basket.BasketHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Models.GoodInList;
using Gbs.Core.ViewModels.Documents.Sales;
using Gbs.Forms._shared;
using Gbs.Forms.Egais.Tap;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

#nullable disable
namespace Gbs.Core.ViewModels.Basket
{
  internal class BasketHelper : ViewModel
  {
    public Gbs.Core.ViewModels.Basket.Basket CurrentBasket { get; set; }

    public BasketHelper(Gbs.Core.ViewModels.Basket.Basket basket) => this.CurrentBasket = basket;

    public void AddItemClientOrder(List<BasketItem> items, Client client, Guid orderUid)
    {
      this.CurrentBasket.Items = new ObservableCollection<BasketItem>(items.Select<BasketItem, BasketItem>((Func<BasketItem, BasketItem>) (x => x.Clone())));
      this.CurrentBasket.IsCheckedClient = true;
      this.CurrentBasket.ClientOrderUid = orderUid;
      this.OnPropertyChanged("PrepaidPaymentsSum");
      this.OnPropertyChanged("VisibilitySumPrepaid");
    }

    public void AddItemCopySale(List<BasketItem> items, Client client)
    {
      this.CurrentBasket.Items = new ObservableCollection<BasketItem>(items.Select<BasketItem, BasketItem>((Func<BasketItem, BasketItem>) (x => x.Clone())));
      this.CurrentBasket.IsCheckedClient = client != null;
    }

    public void AddItemToBasket(
      IEnumerable<Gbs.Core.Entities.Goods.Good> goods,
      bool addAllCount = false,
      bool checkMinus = true,
      Guid tapUid = default (Guid))
    {
      Performancer performancer = new Performancer(string.Format("добавление позиций в корзину. Goods count: {0}", (object) goods.Count<Gbs.Core.Entities.Goods.Good>()));
      bool flag = true;
      foreach (Gbs.Core.Entities.Goods.Good good in goods)
      {
        if (good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None && this.CurrentBasket.Items.Count<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Group.RuMarkedProductionType != GlobalDictionaries.RuMarkedProductionTypes.None && !x.Comment.IsNullOrEmpty())) >= 50 && flag)
        {
          MessageBoxHelper.Warning(Translate.BasketHelper_Максимальное_количество_маркированного_товара_в_одном_чеке___50_позиций__для_передачи_остальных_товаров_создайте_отдельную_продажу_);
          flag = false;
        }
        else
        {
          switch (good.SetStatus)
          {
            case GlobalDictionaries.GoodsSetStatuses.None:
              if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Certificate)
              {
                this.AddCertificateToBasket(good);
                continue;
              }
              goto case GlobalDictionaries.GoodsSetStatuses.Production;
            case GlobalDictionaries.GoodsSetStatuses.Set:
              this.GetStock(good, addAllCount, checkMinus);
              continue;
            case GlobalDictionaries.GoodsSetStatuses.Kit:
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.BasketHelper_Добавление_набора_в_корзину);
              foreach (GoodsSets.Set goodKit in good.SetContent.Where<GoodsSets.Set>((Func<GoodsSets.Set, bool>) (x => !x.IsDeleted)))
                this.GetStock(goodKit);
              progressBar.Close();
              continue;
            case GlobalDictionaries.GoodsSetStatuses.Range:
              this.GetStock(good, addAllCount, checkMinus);
              continue;
            case GlobalDictionaries.GoodsSetStatuses.Production:
              this.GetStock(good, addAllCount, checkMinus, tapUid: tapUid);
              continue;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      performancer.AddPoint("point 100");
      this.OnPropertyChanged("TotalSum");
      performancer.Stop();
    }

    private void AddCertificateToBasket(Gbs.Core.Entities.Goods.Good good)
    {
      (bool result, string output) tuple = MessageBoxHelper.Input("", Translate.BasketHelper_Введите_штрих_код_используемого_сертификата, 1);
      int num1 = tuple.result ? 1 : 0;
      string output = tuple.output;
      if (num1 == 0)
        return;
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<GoodsCertificate.Certificate> list = GoodsCertificate.GetCertificateFilteredList(dataBase.GetTable<CERTIFICATES>().Where<CERTIFICATES>((Expression<Func<CERTIFICATES, bool>>) (x => x.BARCODE == output && !x.IS_DELETED))).ToList<GoodsCertificate.Certificate>();
        if (list.Any<GoodsCertificate.Certificate>() && list.All<GoodsCertificate.Certificate>((Func<GoodsCertificate.Certificate, bool>) (x =>
        {
          Guid? goodUid = x.Stock?.GoodUid;
          Guid uid = good.Uid;
          return goodUid.HasValue && goodUid.GetValueOrDefault() == uid;
        })))
        {
          this.ActivatedCertificate(list.First<GoodsCertificate.Certificate>(), good);
        }
        else
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.BasketHelper_Сертификат_с_данным_штрих_кодом_не_найден_);
        }
      }
    }

    public void GetStock(
      Gbs.Core.Entities.Goods.Good good,
      bool addAllCount,
      bool checkMinus = true,
      Decimal? count = null,
      string modificationUid = "",
      string comment = "",
      bool isWeightPlu = false,
      Guid tapUid = default (Guid))
    {
      Performancer performancer = new Performancer("Get stock");
      List<GoodsStocks.GoodStock> stocks;
      if (!new FrmSelectGoodStock().SelectedStock(good, out stocks, true, modificationUid: modificationUid))
        return;
      performancer.AddPoint("100");
      InfoToTapBeer infoToTapBeer = (InfoToTapBeer) null;
      if (EgaisHelper.IsBeerKega(good) && !stocks.SelectMany<GoodsStocks.GoodStock, EntityProperties.PropertyValue>((Func<GoodsStocks.GoodStock, IEnumerable<EntityProperties.PropertyValue>>) (x => (IEnumerable<EntityProperties.PropertyValue>) x.Properties)).Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.MarkedInfoGood)))
      {
        if (tapUid == new Guid())
        {
          (InfoToTapBeer, bool) tapForGood = new SelectTapViewModel().GetTapForGood(good.SetContent.Single<GoodsSets.Set>().GoodUid);
          if (!tapForGood.Item2)
            return;
          infoToTapBeer = tapForGood.Item1;
        }
        else
          infoToTapBeer = new InfoTapBeerRepository().GetByTapUid(tapUid);
        if (infoToTapBeer == null)
        {
          MessageBoxHelper.Warning("Невозможно добавить " + good.Name + " в чек.\n\nПеред тем как добавлять разливное пиво в чек, его требуется подключить к крану и вскрыть. Пожалуйста, подключите кег к крану и повторите попытку добавления товара в чек.");
          return;
        }
        if (infoToTapBeer.Quantity.GetValueOrDefault() - infoToTapBeer.SaleQuantity <= 0M)
        {
          MessageBoxHelper.Warning("Невозможно добавить " + good.Name + " в чек.\n\nОбъем кега полностью израсходован. Замените кег на кране и повторите попытку добавления товара в чек.");
          return;
        }
      }
      foreach (GoodsStocks.GoodStock goodStock in stocks)
      {
        GoodsStocks.GoodStock stock = goodStock;
        string str = stock.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.MarkedInfoGood))?.Value?.ToString();
        comment = infoToTapBeer == null ? (str.IsNullOrEmpty() ? comment : str) : infoToTapBeer.MarkedInfo.Trim();
        Decimal defaultValue = addAllCount ? stock.Stock : (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight ? 0.001M : 1M);
        Gbs.Core.ViewModels.Basket.Basket currentBasket = this.CurrentBasket;
        BasketItem basketItem = new BasketItem(good, stock.ModificationUid, stock.Price, stock.Storage, count.GetValueOrDefault(defaultValue), comment: comment, goodStockUid: good.StocksAndPrices.First<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Price == stock.Price && x.ModificationUid == stock.ModificationUid && x.Storage.Uid == stock.Storage.Uid)).Uid);
        basketItem.InfoToTapBeer = infoToTapBeer;
        int num1 = checkMinus ? 1 : 0;
        int num2 = isWeightPlu ? 1 : 0;
        ActionResult actionResult = currentBasket.Add(basketItem, num1 != 0, num2 != 0);
        if (actionResult.Result == ActionResult.Results.Error)
        {
          int num3 = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages), icon: MessageBoxImage.Hand);
        }
      }
      performancer.Stop();
    }

    private void GetStockForSet(Gbs.Core.Entities.Goods.Good good)
    {
      this.CurrentBasket.Add(new BasketItem(good, Guid.Empty, 1000M, this.CurrentBasket.Storage));
    }

    private void GetStock(GoodsSets.Set goodKit)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(goodKit.GoodUid);
        List<GoodsStocks.GoodStock> stocks;
        if (!new FrmSelectGoodStock().SelectedStock(byUid, out stocks, true))
          return;
        foreach (GoodsStocks.GoodStock goodStock in stocks)
        {
          BasketItem basketItem = new BasketItem(byUid, goodStock.ModificationUid, goodStock.Price, goodStock.Storage, goodKit.Quantity);
          ActionResult actionResult = this.CurrentBasket.Add(basketItem);
          basketItem.Discount.SetDiscount(goodKit.Discount, SaleDiscount.ReasonEnum.UserEdit, basketItem, this.CurrentBasket);
          if (actionResult.Result == ActionResult.Results.Error)
          {
            int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages), icon: MessageBoxImage.Hand);
          }
        }
        this.CurrentBasket.ReCalcTotals();
      }
    }

    public void ActivatedCertificate(GoodsCertificate.Certificate c, Gbs.Core.Entities.Goods.Good good)
    {
      switch (c.Status)
      {
        case GlobalDictionaries.CertificateStatus.Open:
          if (this.CurrentBasket.Storage == null)
            this.CurrentBasket.Storage = c.Stock.Storage;
          else if (c.Stock.Storage.Uid != this.CurrentBasket.Storage.Uid)
          {
            MessageBoxHelper.Warning(Translate.BasketHelper_В_одной_продаже_могут_быть_товары_только_с_одного_склада_);
            break;
          }
          if (this.CurrentBasket.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.Certificate.Uid == c.Uid)))
          {
            MessageBoxHelper.Warning(Translate.BasketHelper_Данный_сертификат_уже_добавлен_в_эту_продажу);
            break;
          }
          object obj = good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid))?.Value;
          if (obj == null)
          {
            MessageBoxHelper.Warning(Translate.BasketHelper_У_данного_сертификата_не_указан_номинал__добавление_в_продажу_невозможно_);
            break;
          }
          BasketItem basketItem1 = new BasketItem();
          basketItem1.Good = good;
          basketItem1.Storage = c.Stock.Storage;
          basketItem1.Comment = c.Barcode;
          basketItem1.SalePrice = c.Stock.Price;
          basketItem1.Quantity = 1M;
          basketItem1.Certificate = new GoodsListItemsCertificate(c.Uid, Convert.ToDecimal(obj));
          BasketItem basketItem2 = basketItem1;
          this.CurrentBasket.IsNeedComment = false;
          this.CurrentBasket.Add(basketItem2);
          this.CurrentBasket.IsNeedComment = true;
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.BasketHelper_Сертификат_добавлен_в_корзину,
            Text = string.Format(Translate.BasketHelper_Добавлен_сертификат_номиналом__0_N_, (object) basketItem2.Certificate.Nominal)
          });
          break;
        case GlobalDictionaries.CertificateStatus.Saled:
          if (this.CurrentBasket.CertificatesForPay.Any<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate>((Func<Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate, bool>) (x => x.Certificate.Uid == c.Uid)))
          {
            int num = (int) MessageBoxHelper.Show(Translate.BasketHelper_Данный_сертификат_уже_активирован_в_рамках_этой_продажи);
            break;
          }
          Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate certificate = new Gbs.Core.ViewModels.Basket.Basket.BasketPayCertificate(c);
          this.CurrentBasket.AddCertificate(certificate);
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = Translate.BasketHelper_Сертификат_активирован,
            Text = string.Format(Translate.BasketHelper_Активирован_сертификат_номиналом__0_N_, (object) certificate.Nominal)
          });
          this.OnPropertyChanged("CertificatesPaymentSum");
          break;
        case GlobalDictionaries.CertificateStatus.Close:
          int num1 = (int) MessageBoxHelper.Show(Translate.BasketHelper_Данный_сертификат_уже_был_активирован);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
