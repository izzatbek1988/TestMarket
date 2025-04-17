// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.MoveHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers.HomeOffice.Entity;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public static class MoveHelper
  {
    private static string Path => MoveHelper.PreparePath(UidDb.GetUid().EntityUid);

    public static string PreparePath(Guid uidDb)
    {
      return System.IO.Path.Combine(new ConfigsRepository<Settings>().Get().RemoteControl.Cloud.Path, "send", uidDb.ToString("N"));
    }

    public static bool IsNewDocument()
    {
      if (!Directory.Exists(MoveHelper.Path))
        return false;
      foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<string>) Directory.GetFiles(MoveHelper.Path)).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))))
      {
        if (Guid.TryParse(fileSystemInfo.Name.Replace(".zip", ""), out Guid _))
          return true;
      }
      return false;
    }

    private static void AddDocumentInFolder(string path)
    {
      string str1 = System.IO.Path.Combine(ApplicationInfo.GetInstance().Paths.ArchivesFromPointsMovePath);
      if (!Directory.Exists(str1))
        Directory.CreateDirectory(str1);
      string str2 = System.IO.Path.Combine(str1, DateTime.Now.ToString("yyyy-MM-dd"));
      if (!Directory.Exists(str2))
        Directory.CreateDirectory(str2);
      FileInfo fileInfo = new FileInfo(path);
      FileSystemHelper.CopyFile(path, System.IO.Path.Combine(str2, fileInfo.Name));
    }

    public static List<MoveHelper.DocumentSend> GetDocuments()
    {
      IEnumerable<FileInfo> source1 = ((IEnumerable<string>) Directory.GetFiles(MoveHelper.Path)).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).Where<FileInfo>((Func<FileInfo, bool>) (x => x.Name.Contains(".zip") && Guid.TryParse(x.Name.Replace(".zip", ""), out Guid _)));
      LogHelper.Debug("Содержимое папки обмена: " + source1.Select<FileInfo, string>((Func<FileInfo, string>) (x => x.Name)).ToJsonString());
      List<MoveHelper.DocumentSend> documents = new List<MoveHelper.DocumentSend>();
      List<Gbs.Core.Entities.Goods.Good> source2 = new List<Gbs.Core.Entities.Goods.Good>();
      if (source1.Any<FileInfo>())
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          source2 = new GoodRepository(dataBase).GetActiveItems();
      }
      foreach (FileInfo fileInfo in source1)
      {
        MoveHelper.AddDocumentInFolder(fileInfo.FullName);
        string str1 = FileSystemHelper.TempFolderPath();
        FileSystemHelper.ExtractAllFile(fileInfo.FullName, str1);
        LogHelper.Trace("Разбор файла перемещения из другой ТТ: " + fileInfo.FullName);
        if (((IEnumerable<string>) Directory.GetFiles(str1, "document.json")).Any<string>())
        {
          string path1 = ((IEnumerable<string>) Directory.GetFiles(str1, "document.json")).First<string>();
          MoveHelper.DocumentSend documentSend;
          try
          {
            documentSend = JsonConvert.DeserializeObject<MoveHelper.DocumentSend>(File.ReadAllText(path1));
            documentSend.PathFile = fileInfo.FullName;
            documentSend.DocumentEntity = new Document(documentSend.Document);
            documentSend.DocumentEntity.Storage = (Storages.Storage) null;
            documentSend.DocumentEntity.Items = documentSend.DocumentEntity.Items.GroupBy(x => new
            {
              GoodUid = x.GoodUid,
              BuyPrice = x.BuyPrice,
              SellPrice = x.SellPrice,
              Comment = x.Comment,
              ModificationUid = x.ModificationUid,
              FbNumberForEgais = x.FbNumberForEgais
            }).Select<IGrouping<\u003C\u003Ef__AnonymousType32<Guid, Decimal, Decimal, string, Guid, string>, Gbs.Core.Entities.Documents.Item>, Gbs.Core.Entities.Documents.Item>(x =>
            {
              return new Gbs.Core.Entities.Documents.Item()
              {
                Good = x.First<Gbs.Core.Entities.Documents.Item>().Good,
                Uid = x.First<Gbs.Core.Entities.Documents.Item>().Uid,
                BaseSalePrice = x.First<Gbs.Core.Entities.Documents.Item>().BaseSalePrice,
                BuyPrice = x.First<Gbs.Core.Entities.Documents.Item>().BuyPrice,
                SellPrice = x.First<Gbs.Core.Entities.Documents.Item>().SellPrice,
                Comment = x.First<Gbs.Core.Entities.Documents.Item>().Comment,
                DocumentUid = x.First<Gbs.Core.Entities.Documents.Item>().DocumentUid,
                Quantity = x.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => i.Quantity)),
                ModificationUid = x.First<Gbs.Core.Entities.Documents.Item>().ModificationUid,
                IsDeleted = x.First<Gbs.Core.Entities.Documents.Item>().IsDeleted,
                Discount = x.First<Gbs.Core.Entities.Documents.Item>().Discount,
                GoodStock = new GoodsStocks.GoodStock(),
                FbNumberForEgais = x.First<Gbs.Core.Entities.Documents.Item>().FbNumberForEgais
              };
            }).ToList<Gbs.Core.Entities.Documents.Item>();
          }
          catch
          {
            Directory.Delete(str1, true);
            continue;
          }
          string path2 = ((IEnumerable<string>) Directory.GetFiles(str1, "goods.json")).First<string>();
          try
          {
            GoodHomeList goodHomeList = JsonConvert.DeserializeObject<GoodHomeList>(File.ReadAllText(path2));
            List<Gbs.Core.Entities.Goods.Good> goodList = new List<Gbs.Core.Entities.Goods.Good>();
            foreach (GoodHome goods in goodHomeList.GoodsList)
            {
              GoodHome item = goods;
              Gbs.Core.Entities.Goods.Good good1 = source2.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == item.Uid));
              if (good1 != null)
              {
                foreach (Gbs.Core.Entities.Documents.Item obj in documentSend.DocumentEntity.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == item.Uid)))
                  obj.Good = good1;
              }
              else
              {
                List<Gbs.Core.Entities.Goods.Good> list = source2.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => string.Equals(x.Name, item.Name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(x.Barcode, item.Barcode, StringComparison.CurrentCultureIgnoreCase) && string.Equals(x.Description, item.Description, StringComparison.CurrentCultureIgnoreCase) && x.SetStatus == item.SetStatus)).ToList<Gbs.Core.Entities.Goods.Good>();
                if (item.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.AlcCodeUid)) && list.Any<Gbs.Core.Entities.Goods.Good>())
                  list = list.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.AlcCodeUid))?.Value == item.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.AlcCodeUid))?.Value)).ToList<Gbs.Core.Entities.Goods.Good>();
                Gbs.Core.Entities.Goods.Good good2 = list.FirstOrDefault<Gbs.Core.Entities.Goods.Good>();
                if (good2 != null)
                {
                  foreach (Gbs.Core.Entities.Documents.Item obj in documentSend.DocumentEntity.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == item.Uid)))
                    obj.Good = good2;
                }
                else
                {
                  Gbs.Core.Entities.Goods.Good good3 = GoodsFactory.Create(item);
                  good3.Properties.RemoveAll((Predicate<EntityProperties.PropertyValue>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid));
                  good3.DateAdd = DateTime.Now;
                  foreach (Gbs.Core.Entities.Documents.Item obj in documentSend.DocumentEntity.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == item.Uid)))
                    obj.Good = good3;
                  goodList.Add(good3);
                }
              }
            }
            documentSend.GoodList = goodList;
          }
          catch
          {
            Directory.Delete(str1, true);
            continue;
          }
          string str2 = ((IEnumerable<string>) Directory.GetFiles(str1, "goodGroups.json")).First<string>();
          List<GoodGroups.Group> source3 = new List<GoodGroups.Group>();
          if (str2.Any<char>())
          {
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
              source3 = new GoodGroupsRepository(dataBase).GetActiveItems();
          }
          try
          {
            GoodGroupHomeList goodGroupHomeList = JsonConvert.DeserializeObject<GoodGroupHomeList>(File.ReadAllText(str2));
            foreach (Gbs.Core.Entities.Goods.Good good in documentSend.GoodList)
            {
              Gbs.Core.Entities.Goods.Good item = good;
              item.Group = new GoodGroups.Group(goodGroupHomeList.Groups.First<GoodGroupHome>((Func<GoodGroupHome, bool>) (x => x.Uid == item.Group.Uid)));
              GoodGroups.Group group1 = source3.FirstOrDefault<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.Uid == item.Group.Uid));
              if (group1 != null)
              {
                foreach (Gbs.Core.Entities.Documents.Item obj in documentSend.DocumentEntity.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == item.Uid)))
                  obj.Good.Group = group1;
              }
              else
              {
                GoodGroups.Group group2 = source3.FirstOrDefault<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => string.Equals(x.Name, item.Group.Name, StringComparison.CurrentCultureIgnoreCase) && x.GoodsType == item.Group.GoodsType));
                if (group2 != null)
                {
                  foreach (Gbs.Core.Entities.Documents.Item obj in documentSend.DocumentEntity.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == item.Uid)))
                    obj.Good.Group = group2;
                }
                else
                {
                  GoodGroups.Group group3 = new GoodGroups.Group(goodGroupHomeList.Groups.FirstOrDefault<GoodGroupHome>((Func<GoodGroupHome, bool>) (g => g.Uid == item.Group.Uid)));
                  foreach (Gbs.Core.Entities.Documents.Item obj in documentSend.DocumentEntity.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == item.Uid)))
                    obj.Good.Group = group3;
                  documentSend.DocumentEntity.Items[documentSend.DocumentEntity.Items.FindIndex((Predicate<Gbs.Core.Entities.Documents.Item>) (x => x.GoodUid == item.Uid))].Good.Group = group3;
                }
              }
            }
          }
          catch
          {
            Directory.Delete(str1, true);
            continue;
          }
          documents.Add(documentSend);
          Directory.Delete(str1, true);
        }
      }
      return documents;
    }

    public static void DeleteDocumentCloud(Guid uid)
    {
      string path = System.IO.Path.Combine(MoveHelper.Path, uid.ToString() + ".zip");
      if (!File.Exists(path))
        return;
      try
      {
        File.Delete(path);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось удалить архив с перемещением");
      }
    }

    public static bool IsExistsDocumentCloud(Guid uid)
    {
      return File.Exists(System.IO.Path.Combine(MoveHelper.Path, uid.ToString() + ".zip"));
    }

    public static bool CreateMoveFile(Document sendWaybill, Guid pointMoveUid)
    {
      LogHelper.OnBegin();
      Guid uid = sendWaybill.Uid;
      string str1 = MoveHelper.PreparePath(pointMoveUid);
      sendWaybill.ParentUid = sendWaybill.Uid;
      ProfitHelper profitHelper = new ProfitHelper(new BuyPriceCounter(true));
      foreach (Gbs.Core.Entities.Documents.Item currentItem in sendWaybill.Items)
      {
        currentItem.BuyPrice = profitHelper.GetBuyPriceForItem(sendWaybill, currentItem);
        if (currentItem.Good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Production)
          currentItem.Good.SetStatus = GlobalDictionaries.GoodsSetStatuses.None;
      }
      string jsonString1 = new MoveHelper.DocumentSend()
      {
        Uid = uid,
        Document = new DocumentHome(sendWaybill),
        RecipientPointUid = pointMoveUid,
        SenderPointName = UidDb.GetUid().Value.ToString(),
        SenderPointUid = UidDb.GetUid().EntityUid,
        GbsId = GbsIdHelperMain.GetGbsId()
      }.ToJsonString(true);
      if (!FileSystemHelper.ExistsOrCreateFolder(str1))
        return false;
      string str2 = FileSystemHelper.TempFolderPath();
      File.WriteAllText(System.IO.Path.Combine(str2, "document.json"), jsonString1);
      string path = System.IO.Path.Combine(str2, "goods.json");
      List<Gbs.Core.Entities.Goods.Good> list = sendWaybill.Items.GroupBy<Gbs.Core.Entities.Documents.Item, Guid>((Func<Gbs.Core.Entities.Documents.Item, Guid>) (x => x.GoodUid)).Select<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, Gbs.Core.Entities.Goods.Good>((Func<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, Gbs.Core.Entities.Goods.Good>) (x => x.First<Gbs.Core.Entities.Documents.Item>().Good)).ToList<Gbs.Core.Entities.Goods.Good>();
      list.ForEach((Action<Gbs.Core.Entities.Goods.Good>) (x => x.StocksAndPrices = new List<GoodsStocks.GoodStock>()));
      string jsonString2 = new GoodHomeList(list).ToJsonString(true);
      File.WriteAllText(path, jsonString2);
      File.WriteAllText(System.IO.Path.Combine(str2, "goodGroups.json"), new GoodGroupHomeList(sendWaybill.Items.GroupBy<Gbs.Core.Entities.Documents.Item, Guid>((Func<Gbs.Core.Entities.Documents.Item, Guid>) (x => x.Good.Group.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, GoodGroups.Group>((Func<IGrouping<Guid, Gbs.Core.Entities.Documents.Item>, GoodGroups.Group>) (x => x.First<Gbs.Core.Entities.Documents.Item>().Good.Group)).ToList<GoodGroups.Group>()).ToJsonString(true));
      FileSystemHelper.CreateZip(System.IO.Path.Combine(str1, uid.ToString() + ".zip"), str2);
      Directory.Delete(str2, true);
      LogHelper.OnEnd();
      return true;
    }

    public static List<MoveHelper.DisplayDocumentSend> GetDisplayDocument()
    {
      return MoveHelper.GetDocuments().Select<MoveHelper.DocumentSend, MoveHelper.DisplayDocumentSend>((Func<MoveHelper.DocumentSend, MoveHelper.DisplayDocumentSend>) (x => new MoveHelper.DisplayDocumentSend()
      {
        Document = x
      })).ToList<MoveHelper.DisplayDocumentSend>();
    }

    public class DocumentSend
    {
      [JsonIgnore]
      public string PathFile { get; set; }

      public Guid Uid { get; set; }

      public Guid RecipientPointUid { get; set; }

      public Guid SenderPointUid { get; set; }

      public string SenderPointName { get; set; }

      public string GbsId { get; set; }

      public DocumentHome Document { get; set; }

      [JsonIgnore]
      public Document DocumentEntity { get; set; }

      [JsonIgnore]
      public List<Gbs.Core.Entities.Goods.Good> GoodList { get; set; }
    }

    public class DisplayDocumentSend
    {
      public MoveHelper.DocumentSend Document { get; set; }

      public Decimal TotalCount
      {
        get => this.Document.Document.Items.Sum<Gbs.Helpers.HomeOffice.Entity.Item>((Func<Gbs.Helpers.HomeOffice.Entity.Item, Decimal>) (x => x.Quantity));
      }

      public Decimal TotalBuySum
      {
        get
        {
          return this.Document.Document.Items.Sum<Gbs.Helpers.HomeOffice.Entity.Item>((Func<Gbs.Helpers.HomeOffice.Entity.Item, Decimal>) (x => x.BuyPrice * x.Quantity));
        }
      }

      public Decimal TotalSaleSum
      {
        get
        {
          return this.Document.Document.Items.Sum<Gbs.Helpers.HomeOffice.Entity.Item>((Func<Gbs.Helpers.HomeOffice.Entity.Item, Decimal>) (x => x.SellPrice * x.Quantity));
        }
      }
    }
  }
}
