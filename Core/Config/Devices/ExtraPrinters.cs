// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ExtraPrinters
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Documents;
using Gbs.Helpers.FR;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Core.Config
{
  [ExcludeFromCodeCoverage]
  [Serializable]
  public class ExtraPrinters
  {
    private static Document _olDocument;

    public bool IsActive { get; set; }

    public List<ExtraPrinters.ExtraPrinter> Printers { get; set; } = new List<ExtraPrinters.ExtraPrinter>();

    public static void PrepareExtraPrint(Document document)
    {
      ExtraPrinters._olDocument = (Document) null;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        ExtraPrinters._olDocument = new DocumentsRepository(dataBase).GetByUid(document.Uid);
    }

    public static void Print(Document document)
    {
      ExtraPrinters extraPrinters = new ConfigsRepository<Devices>().Get().ExtraPrinters;
      if (!extraPrinters.IsActive)
        return;
      Document printDoc = document.Clone<Document>();
      if (ExtraPrinters._olDocument != null)
      {
        List<Item> collection = printDoc.Items.Clone<List<Item>>();
        foreach (Item obj in printDoc.Items)
        {
          Item item = obj;
          if (ExtraPrinters._olDocument.Items.Any<Item>((Func<Item, bool>) (x => x.Uid == item.Uid)))
          {
            Item oldItem = ExtraPrinters._olDocument.Items.Single<Item>((Func<Item, bool>) (x => x.Uid == item.Uid));
            if (oldItem.Quantity == item.Quantity)
            {
              collection.RemoveAll((Predicate<Item>) (x => x.Uid == oldItem.Uid));
            }
            else
            {
              int index = collection.FindIndex((Predicate<Item>) (x => x.Uid == item.Uid));
              Decimal num;
              string str1;
              if (!(collection[index].Quantity - oldItem.Quantity > 0M))
              {
                num = collection[index].Quantity - oldItem.Quantity;
                str1 = num.ToString("N2");
              }
              else
              {
                num = collection[index].Quantity - oldItem.Quantity;
                str1 = "+" + num.ToString("N2");
              }
              string str2 = str1;
              collection[index].Good.Name = string.Format(Translate.ExtraPrinters_Print__ИЗМЕНЕН___0____, (object) str2) + collection[index].Good.Name;
            }
          }
          else
          {
            int index = collection.FindIndex((Predicate<Item>) (x => x.Uid == item.Uid));
            collection[index].Good.Name = Translate.ExtraPrinters_Print__ДОБАВЛЕН__ + collection[index].Good.Name;
          }
        }
        foreach (Item obj in ExtraPrinters._olDocument.Items.Where<Item>((Func<Item, bool>) (item => printDoc.Items.All<Item>((Func<Item, bool>) (x => x.Uid != item.Uid)))).Select<Item, Item>((Func<Item, Item>) (item => item.Clone<Item>())))
        {
          obj.Good.Name = Translate.ExtraPrinters_Print__УДАЛЕН__ + obj.Good.Name;
          collection.Add(obj);
        }
        printDoc.Items = new List<Item>((IEnumerable<Item>) collection);
      }
      if (!printDoc.Items.Any<Item>())
        return;
      PrintableReportFactory printableReportFactory = new PrintableReportFactory();
      foreach (ExtraPrinters.ExtraPrinter printer1 in extraPrinters.Printers)
      {
        ExtraPrinters.ExtraPrinter printer = printer1;
        if (printer.IsActive)
        {
          Document document1 = printDoc.Clone<Document>();
          document1.Items = document1.Items.Where<Item>((Func<Item, bool>) (x => printer.GoodGroups.Any<Guid>((Func<Guid, bool>) (g => g == x.Good.Group.Uid)))).ToList<Item>();
          if (document1.Items.Any<Item>())
          {
            IPrintableReport forSaleDocument = printableReportFactory.CreateForSaleDocument(document1);
            forSaleDocument.Type = ReportType.ExtraPrinter;
            new FastReportFacade().PrintReport(forSaleDocument, printer.PrinterName, printer.CheckTemplate);
          }
        }
      }
    }

    public class ExtraPrinter
    {
      public Guid Uid { get; set; } = Guid.NewGuid();

      public bool IsActive { get; set; }

      public string Name { get; set; } = string.Empty;

      public string PrinterName { get; set; } = string.Empty;

      public List<Guid> GoodGroups { get; set; } = new List<Guid>();

      public string CheckTemplate { get; set; } = "";

      [JsonIgnore]
      public string CheckTemplateName
      {
        get
        {
          return this.CheckTemplate.IsNullOrEmpty() || !File.Exists(this.CheckTemplate) ? "" : Path.GetFileNameWithoutExtension(new FileInfo(this.CheckTemplate)?.FullName) ?? "";
        }
      }
    }
  }
}
