// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.ReportType
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Resources.Localizations;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Gbs.Helpers.FR
{
  public class ReportType : Enumeration
  {
    public static ReportType ClientsOrder = new ReportType(7, Translate.PageVisualModelView_Заказы_клиентов, "ClientOrders");
    public static ReportType SendWaybill = new ReportType(15, Translate.ReportType_SendWaybill_Перемещения_между_точками, nameof (SendWaybill));
    public static ReportType SendWaybillStorage = new ReportType(16, Translate.ReportType_SendWaybillStorage_Перемещения_между_складами, nameof (SendWaybillStorage));
    public static ReportType GoodCard = new ReportType(24, Translate.PageVisualModelView_Сводный_отчет, nameof (GoodCard));
    public static ReportType SummaryReport = new ReportType(25, Translate.PageVisualModelView_Сводный_отчет, nameof (SummaryReport));
    public static ReportType SellerReport = new ReportType(26, Translate.FrmMainWindow_ОтчетПродавца, nameof (SellerReport));

    public DirectoryInfo Directory { get; }

    public static ReportType SaleDocs
    {
      get => new ReportType(0, Translate.PageVisualModelView_Доп__документы, nameof (SaleDocs));
    }

    public static ReportType Label
    {
      get => new ReportType(1, Translate.PageVisualModelView_Этикетки, nameof (Label));
    }

    public static ReportType PriceTags
    {
      get => new ReportType(2, Translate.PageVisualModelView_Ценники, nameof (PriceTags));
    }

    public static ReportType Certificates
    {
      get
      {
        return new ReportType(3, Translate.PageBarcodeScanner_ПодарочныеСертификаты, nameof (Certificates));
      }
    }

    public static ReportType CashMemo
    {
      get => new ReportType(6, Translate.PageVisualModelView_Чеки_при_продаже, nameof (CashMemo));
    }

    public static ReportType ClientsCatalog
    {
      get
      {
        return new ReportType(8, Translate.PageVisualModelView_Список_контактов, nameof (ClientsCatalog));
      }
    }

    public static ReportType EmailOrders
    {
      get
      {
        return new ReportType(10, Translate.PageVisualModelView_Отчеты_на_E_mail, nameof (EmailOrders));
      }
    }

    public static ReportType GoodsCatalog
    {
      get
      {
        return new ReportType(11, Translate.PageVisualModelView_Каталог_товаров, nameof (GoodsCatalog));
      }
    }

    public static ReportType GoodGroups
    {
      get => new ReportType(12, Translate.FormGroup_КатегорииТоваров, nameof (GoodGroups));
    }

    public static ReportType Inventory
    {
      get => new ReportType(13, Translate.PageVisualModelView_Инвентаризация, nameof (Inventory));
    }

    public static ReportType SaleOrder
    {
      get => new ReportType(14, Translate.PageVisualModelView_Журнал_продаж, nameof (SaleOrder));
    }

    public static ReportType Waybill
    {
      get => new ReportType(17, Translate.PageVisualModelView_Поступления, nameof (Waybill));
    }

    public static ReportType WriteOff
    {
      get => new ReportType(18, Translate.PageVisualModelView_Списания, nameof (WriteOff));
    }

    public static ReportType ReportOldGoods
    {
      get
      {
        return new ReportType(19, Translate.ReportType_Отчет__залежавшиеся_товары, Path.Combine("MasterReport", "OldGoodsReport"));
      }
    }

    public static ReportType ReportSale
    {
      get
      {
        return new ReportType(19, Translate.ReportType_Отчет_по_продажам, Path.Combine("MasterReport", nameof (ReportSale)));
      }
    }

    public static ReportType ReportOrderGood
    {
      get
      {
        return new ReportType(19, Translate.ReportType_Отчет__товары_для_дозаказа, Path.Combine("MasterReport", nameof (ReportOrderGood)));
      }
    }

    public static ReportType ReportHistoryGoods
    {
      get
      {
        return new ReportType(19, Translate.ReportType_ReportHistoryGoods_Отчет__история_товаров, Path.Combine("MasterReport", nameof (ReportHistoryGoods)));
      }
    }

    public static ReportType ReportPaymentsForSupplier
    {
      get
      {
        return new ReportType(19, Translate.ReportType_ReportPaymentsForSupplier_Отчет__взаиморасчеты_с_поставщиками, Path.Combine("MasterReport", nameof (ReportPaymentsForSupplier)));
      }
    }

    public static ReportType ReportPaymentsMove
    {
      get
      {
        return new ReportType(19, Translate.ReportType_ReportPaymentsMove_Отчет__движение_денежных_средств, Path.Combine("MasterReport", nameof (ReportPaymentsMove)));
      }
    }

    public static ReportType ReportMarkedLable
    {
      get
      {
        return new ReportType(20, Translate.ReportType_ReportMarkedLable_Этикетки_для_маркируемой_продукции, "MarkedLable");
      }
    }

    public static ReportType ExtraPrinter
    {
      get
      {
        return new ReportType(21, Translate.ReportType_ExtraPrinter_Дополнительный_принтер, nameof (ExtraPrinter));
      }
    }

    public static ReportType NonFiscalPrint
    {
      get
      {
        return new ReportType(22, Translate.ReportType_NonFiscalPrint_Нефискальная_печать, nameof (NonFiscalPrint));
      }
    }

    public static ReportType Production
    {
      get
      {
        return new ReportType(23, Translate.JournalGoodViewModel_JournalGoodViewModel_Производство, nameof (Production));
      }
    }

    private static string _templatesRootDirectory
    {
      get => ApplicationInfo.GetInstance().Paths.TemplatesFrPath;
    }

    private ReportType(int id, string name, string directoryName)
      : base(id, name)
    {
      this.Directory = new DirectoryInfo(Path.Combine(ReportType._templatesRootDirectory, directoryName));
    }

    public static List<ReportType> GetAll()
    {
      return new List<ReportType>()
      {
        ReportType.Certificates,
        ReportType.CashMemo,
        ReportType.ClientsCatalog,
        ReportType.EmailOrders,
        ReportType.GoodsCatalog,
        ReportType.GoodGroups,
        ReportType.Inventory,
        ReportType.Label,
        ReportType.PriceTags,
        ReportType.SaleDocs,
        ReportType.SaleOrder,
        ReportType.Waybill,
        ReportType.WriteOff,
        ReportType.ReportOldGoods,
        ReportType.ReportSale,
        ReportType.ReportOrderGood,
        ReportType.ClientsOrder,
        ReportType.SendWaybill,
        ReportType.SendWaybillStorage,
        ReportType.SummaryReport,
        ReportType.SellerReport,
        ReportType.ReportMarkedLable,
        ReportType.ReportHistoryGoods,
        ReportType.ReportPaymentsForSupplier,
        ReportType.ExtraPrinter,
        ReportType.ReportPaymentsMove,
        ReportType.NonFiscalPrint,
        ReportType.Production,
        ReportType.GoodCard
      };
    }
  }
}
