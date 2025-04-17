// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Lable.LablePrintViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Lable
{
  public partial class LablePrintViewModel : ViewModelWithForm
  {
    public bool IsVisibleMessage { get; private set; }

    public Gbs.Core.ViewModels.Lable.Lable Lable { get; set; } = new Gbs.Core.ViewModels.Lable.Lable();

    public LablePrintViewModel.Types Type { get; set; }

    public ICommand PrintItemCommand { get; set; }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public ICommand AddItem { get; set; }

    public void Load()
    {
      this.AddItem = (ICommand) new RelayCommand((Action<object>) (obj => this.AddItems()));
      this.PrintItemCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.PrintItems()));
    }

    private void AddItems()
    {
      (List<Gbs.Core.Entities.Goods.Good> goods, bool allCount) tuple = new FrmSearchGoods().ShowSearch(GlobalDictionaries.DocumentsTypes.LablePrint, isVisNullStock: true, addGood: new Action<IEnumerable<Gbs.Core.Entities.Goods.Good>, bool, bool>(this.AddItemsGood));
      this.AddItemsGood((IEnumerable<Gbs.Core.Entities.Goods.Good>) tuple.goods, tuple.allCount);
    }

    private void AddItemsGood(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, bool isAllCount = false, bool checkMinus = true)
    {
      foreach (Gbs.Core.Entities.Goods.Good good in goods)
      {
        List<GoodsStocks.GoodStock> stocks;
        if (new FrmSelectGoodStock().SelectedStock(good, out stocks, false, true))
        {
          if (stocks == null)
            stocks = new List<GoodsStocks.GoodStock>()
            {
              new GoodsStocks.GoodStock() { Price = 0M, Stock = 1M }
            };
          foreach (GoodsStocks.GoodStock goodStock in stocks)
          {
            Decimal q = !isAllCount ? (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight ? 0.001M : 1M) : (goodStock.Stock > 0M ? goodStock.Stock : (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Weight ? 0.001M : 1M));
            BasketItem basketItem = new BasketItem(good, goodStock != null ? goodStock.ModificationUid : Guid.Empty, goodStock != null ? goodStock.Price : 0M, goodStock?.Storage, q);
            if (this.Type == LablePrintViewModel.Types.Packing)
            {
              (bool result, Decimal? quantity) = new EditGoodQuantityViewModel().ShowQuantityEditCard(new EditGoodQuantityViewModel.QuantityRequest((IReadOnlyCollection<BasketItem>) new List<BasketItem>()
              {
                basketItem
              }, false));
              if (result)
                basketItem.Quantity = quantity.GetValueOrDefault();
              else
                continue;
            }
            this.Lable.Add(basketItem);
            this.Lable.SelectedItem = basketItem;
          }
        }
      }
      ((FrmLablePrint) this.FormToSHow).ListGoodsLable.ScrollToSelectedRow();
      this.OnPropertyChanged("TotalSaleSum");
      this.IsVisibleMessage = true;
    }

    private void PrintItems()
    {
      if (!this.Lable.Items.Any<BasketItem>())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.LablePrintViewModel_В_списке_нет_товаров_);
      }
      else
      {
        Performancer performancer = new Performancer("Печать этикеток/ценников для списка товаров");
        LogHelper.Debug("Передача на печать товаров:\n" + this.Lable.Items.ToJsonString(true));
        PrintableReportFactory printableReportFactory = new PrintableReportFactory();
        performancer.AddPoint("Инициализация классов");
        ObservableCollection<BasketItem> goodsList = this.Lable.Items.Clone<ObservableCollection<BasketItem>>();
        performancer.AddPoint("Клонирование списка items");
        if (this.Type == LablePrintViewModel.Types.Packing)
        {
          Gbs.Core.Config.Devices deviceConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
          foreach (BasketItem basketItem in (Collection<BasketItem>) goodsList)
          {
            object obj = basketItem.Good.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == deviceConfig.ScaleWithLable.PluUid))?.Value ?? (object) 0;
            string[] strArray = deviceConfig.BarcodeScanner.Prefixes.WeightGoods.Split(GlobalDictionaries.SplitArr);
            if (strArray.Length == 0)
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.LablePrintViewModel_PrintItems_В_разделе_Файл___Настройки___Оборудование___Сканер_ШК_требуется_указать_префикс_для_генерации_штрих_кода_весовых_товаров_);
              return;
            }
            basketItem.Good.Barcode = BarcodeHelper.GetWeightBarcode(strArray[0], Convert.ToInt32(obj), basketItem.Quantity);
          }
          performancer.AddPoint("Подготовка для Packing");
        }
        switch (this.Type)
        {
          case LablePrintViewModel.Types.Labels:
          case LablePrintViewModel.Types.Packing:
            IPrintableReport forLabel = printableReportFactory.CreateForLabel((IEnumerable<BasketItem>) goodsList, this.Type == LablePrintViewModel.Types.Labels);
            performancer.AddPoint("Создать объекта для печати этикеток");
            LablePrintViewModel.PrintLable(forLabel);
            break;
          case LablePrintViewModel.Types.PriceTags:
            IPrintableReport forPriceTags = printableReportFactory.CreateForPriceTags((IEnumerable<BasketItem>) goodsList);
            performancer.AddPoint("Создать объекта для печати ценников");
            this.PrintTagPrice(forPriceTags);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        performancer.AddPoint("Отправление коменды на печать");
        performancer.Stop();
        this.IsVisibleMessage = false;
      }
    }

    public static void PrintLable(IPrintableReport report)
    {
      Performancer performancer = new Performancer("Выполнение метода PrintLabel");
      FastReportFacade fastReportFacade = new FastReportFacade();
      LablePrinter lablePrinter = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().LablePrinter;
      if (lablePrinter.PrinterSetting.IsSendInPrinter || lablePrinter.PrinterSetting.IsSelectPrinter)
      {
        string printerName = lablePrinter.PrinterSetting.IsSelectPrinter ? (string) null : lablePrinter.PrinterSetting.PrinterName;
        performancer.AddPoint("Выбор printName");
        if (lablePrinter.TypePrint == GlobalDictionaries.Devices.TypePrintLable.Standard)
        {
          fastReportFacade.PrintReport(report, printerName);
          performancer.AddPoint("Выполнение метода PrintReport");
        }
        else
        {
          fastReportFacade.PrintZplLable(report, lablePrinter.ZplSetting, false, printerName);
          performancer.AddPoint("Выполнение метода PrintZplLable");
        }
      }
      else if (lablePrinter.TypePrint == GlobalDictionaries.Devices.TypePrintLable.Standard)
      {
        fastReportFacade.SelectTemplateAndShowReport(report, (Gbs.Core.Entities.Users.User) null);
        performancer.AddPoint("Выполнение метода SelectTemplateAndShowReport");
      }
      else
      {
        fastReportFacade.PrintZplLable(report, lablePrinter.ZplSetting, true);
        performancer.AddPoint("Выполнение метода PrintZplLable");
      }
      performancer.Stop();
    }

    private void PrintTagPrice(IPrintableReport report)
    {
      new FastReportFacade().SelectTemplateAndShowReport(report, (Gbs.Core.Entities.Users.User) null);
    }

    public enum Types
    {
      Labels,
      PriceTags,
      Packing,
    }
  }
}
