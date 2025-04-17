// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ExchangeDataHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Zidium.Api.Dto;

#nullable disable
namespace Gbs.Helpers
{
  public static class ExchangeDataHelper
  {
    public static void DoExchangeCatalogForAllPoint(bool showMsg = false)
    {
      LogHelper.OnBegin("Выгрузка данных синхронизации каталога");
      ProgressBarHelper.ProgressBar progressBar = (ProgressBarHelper.ProgressBar) null;
      if (showMsg)
        progressBar = new ProgressBarHelper.ProgressBar(Translate.ExchangeDataHelper_DoExchangeCatalogForAllPoint_Выгрузка_данных_синхронизации_каталога);
      string str1 = FileSystemHelper.TempFolderPath();
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          IEnumerable<Gbs.Core.Entities.Goods.Good> source = new GoodRepository(dataBase).GetActiveItems().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.SetStatus != GlobalDictionaries.GoodsSetStatuses.Kit));
          Cloud cloud = new ConfigsRepository<Settings>().Get().RemoteControl.Cloud;
          if (cloud.Path.IsNullOrEmpty())
            throw new FileNotFoundException(Translate.ClientsExchangeHelper_Не_указана_папка_обмена);
          LogHelper.Debug("Папка обмена: " + cloud.Path);
          string str2 = Path.Combine(cloud.Path, "goods");
          if (!FileSystemHelper.ExistsOrCreateFolder(str2, false))
            throw new FileLoadException(string.Format(Translate.ClientsExchangeHelper_Не_удалось_создать_папку__0_, (object) str2));
          Guid entityUid = UidDb.GetUid().EntityUid;
          string str3 = entityUid.ToString();
          string jsonString = source.Select<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>((Func<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>) (x => new ExchangeDataHelper.Good(x))).ToJsonString(true);
          string str4 = Path.Combine(str1, str3 + ".json");
          File.WriteAllText(str4, jsonString);
          HomeOfficeHelper.InfoArchive infoArchive = new HomeOfficeHelper.InfoArchive();
          entityUid = UidDb.GetUid().EntityUid;
          infoArchive.UidDevice = entityUid.ToString();
          infoArchive.NameDataBase = UidDb.GetUid().Value.ToString();
          string str5 = Path.Combine(str1, str3 + ".info");
          string contents = JsonConvert.SerializeObject((object) infoArchive, Formatting.Indented);
          File.WriteAllText(str5, contents);
          string str6 = Path.Combine(str2, str3 + ".zip");
          FileSystemHelper.CreateZip(str6, (IEnumerable<FileInfo>) new List<FileInfo>()
          {
            new FileInfo(str4),
            new FileInfo(str5)
          }, "HMWRnLTMKdGUjw46rSFL");
          if (!File.Exists(str6))
            throw new FileNotFoundException(Translate.ExchangeDataHelper_DoExchangeCatalogForAllPoint_Файл_обмена_каталогом_товара_не_был_создан_);
          LogHelper.OnEnd("Выгрузка каталога товаров для обмена завершена");
        }
      }
      catch (Exception ex)
      {
        int num = showMsg ? 1 : 0;
        LogHelper.Error(ex, "Ошибка при выгузке файла каталоа  для синхронизации", num != 0);
      }
      finally
      {
        Directory.Delete(str1, true);
        progressBar?.Close();
      }
    }

    public static bool DoExchangeCatalog(IEnumerable<Gbs.Core.Entities.Goods.Good> goods, ExchangeData configData)
    {
      List<ExchangeDataHelper.Good> list = goods.Select<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>((Func<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>) (x => new ExchangeDataHelper.Good(x))).ToList<ExchangeDataHelper.Good>();
      return ExchangeDataHelper.DoExchangeCatalogFtp(list, configData, goods) && ExchangeDataHelper.DoExchangeCatalogLocal(list, configData, goods, out string _);
    }

    public static bool DoExchangeCatalogFtp(
      List<ExchangeDataHelper.Good> goods,
      ExchangeData configData,
      IEnumerable<Gbs.Core.Entities.Goods.Good> goodsDb)
    {
      if (!configData.CatalogExchange.Ftp.IsSend)
        return true;
      string str = FileSystemHelper.TempFolderPath();
      string filePath;
      if (ExchangeDataHelper.PrepareFile(goods, configData.CatalogExchange.Ftp.Format, str, out filePath))
      {
        CatalogExchange.FtpServer ftp = configData.CatalogExchange.Ftp;
        FileInfo localFile = new FileInfo(filePath);
        int num = FtpHelper.UploadFile(localFile, configData.CatalogExchange.Ftp.Path + "/" + localFile.Name, ftp.Connection.UrlAddress, ftp.Connection.PortNumber, ftp.LoginDecrypt, ftp.PassDecrypt) ? 1 : 0;
        localFile.Delete();
        Directory.Delete(str, true);
        return num != 0;
      }
      Directory.Delete(str, true);
      return false;
    }

    public static bool DoExchangeCatalogLocal(
      List<ExchangeDataHelper.Good> goods,
      ExchangeData configData,
      IEnumerable<Gbs.Core.Entities.Goods.Good> goodsDb,
      out string filePath)
    {
      if (configData.CatalogExchange.Local.IsSend)
        return ExchangeDataHelper.PrepareFile(goods, configData.CatalogExchange.Local.Format, configData.CatalogExchange.Local.Path, out filePath);
      filePath = "";
      return true;
    }

    public static bool DoOnlyExchangeCatalogLocal(
      out string filePath,
      Settings settings = null,
      bool isOnlyActive = true)
    {
      if (settings == null)
        settings = new ConfigsRepository<Settings>().Get();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> goodList = isOnlyActive ? new GoodRepository(dataBase).GetActiveItems() : new GoodRepository().GetAllItems();
        return ExchangeDataHelper.DoExchangeCatalogLocal(goodList.Select<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>((Func<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>) (x => new ExchangeDataHelper.Good(x))).ToList<ExchangeDataHelper.Good>(), settings.ExchangeData, (IEnumerable<Gbs.Core.Entities.Goods.Good>) goodList, out filePath);
      }
    }

    public static bool DoOnlyExchangeCatalogFtp(Settings settings = null)
    {
      if (settings == null)
        settings = new ConfigsRepository<Settings>().Get();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
        return ExchangeDataHelper.DoExchangeCatalogFtp(activeItems.Select<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>((Func<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>) (x => new ExchangeDataHelper.Good(x))).ToList<ExchangeDataHelper.Good>(), settings.ExchangeData, (IEnumerable<Gbs.Core.Entities.Goods.Good>) activeItems);
      }
    }

    private static bool PrepareFile(
      List<ExchangeDataHelper.Good> goodsEx,
      GlobalDictionaries.Format format,
      string settingPath,
      out string filePath)
    {
      FileSystemHelper.ExistsOrCreateFolder(settingPath);
      string str = "catalog-" + UidDb.GetUid().Value?.ToString();
      LogHelper.Trace("Путь к файлу для выгрузки. Folder:  " + settingPath + "; filename: " + str);
      switch (format)
      {
        case GlobalDictionaries.Format.Excel:
          filePath = Path.Combine(settingPath, str + ".xlsx");
          return Other.ExportInExcel<ExchangeDataHelper.Good>(goodsEx, filePath);
        case GlobalDictionaries.Format.Csv:
          filePath = Path.Combine(settingPath, str + ".csv");
          return Other.ExportInCsv<ExchangeDataHelper.Good>(goodsEx, filePath);
        case GlobalDictionaries.Format.Xml:
          string path2_1 = str + ".xml";
          System.Xml.Serialization.XmlSerializer xmlSerializer1 = new System.Xml.Serialization.XmlSerializer(goodsEx.GetType());
          Utf8StringWriter utf8StringWriter1 = new Utf8StringWriter();
          Utf8StringWriter utf8StringWriter2 = utf8StringWriter1;
          List<ExchangeDataHelper.Good> o = goodsEx;
          xmlSerializer1.Serialize((TextWriter) utf8StringWriter2, (object) o);
          string contents1 = utf8StringWriter1.ToString();
          filePath = Path.Combine(settingPath, path2_1);
          File.WriteAllText(filePath, contents1);
          return true;
        case GlobalDictionaries.Format.Json:
          string path2_2 = str + ".json";
          string contents2 = JsonConvert.SerializeObject((object) goodsEx, Formatting.Indented);
          filePath = Path.Combine(settingPath, path2_2);
          File.WriteAllText(filePath, contents2);
          return true;
        case GlobalDictionaries.Format.Yml:
          string path2_3 = "yml_" + (str + ".xml");
          System.Xml.Serialization.XmlSerializer xmlSerializer2 = new System.Xml.Serialization.XmlSerializer(typeof (YmlCatalog));
          Utf8StringWriter utf8StringWriter3 = new Utf8StringWriter();
          Utf8StringWriter utf8StringWriter4 = utf8StringWriter3;
          YmlCatalog ymlCatalog = YmlHelper.ConvertToYmlCatalog(goodsEx);
          xmlSerializer2.Serialize((TextWriter) utf8StringWriter4, (object) ymlCatalog);
          string contents3 = utf8StringWriter3.ToString();
          filePath = Path.Combine(settingPath, path2_3);
          File.WriteAllText(filePath, contents3);
          return true;
        default:
          filePath = string.Empty;
          return false;
      }
    }

    public class Good
    {
      public int Id { get; set; }

      public Guid Uid { get; set; }

      public string Name { get; set; }

      public string GroupName { get; set; }

      public Guid GroupUid { get; set; }

      public string Barcode { get; set; }

      public Decimal MaxPrice { get; set; }

      public Decimal TotalStock { get; set; }

      public string Description { get; set; }

      public string Barcodes { get; set; }

      public List<ExchangeDataHelper.Good.Properties> PropertiesList { get; set; } = new List<ExchangeDataHelper.Good.Properties>();

      [JsonIgnore]
      [XmlIgnore]
      public Guid UidPoint { get; set; }

      [JsonIgnore]
      [XmlIgnore]
      public string NamePoint { get; set; }

      public Good()
      {
      }

      public Good(Gbs.Core.Entities.Goods.Good good)
      {
        this.Uid = good.Uid;
        this.Name = good.Name;
        this.GroupName = good.Group.Name;
        this.GroupUid = good.Group.Uid;
        this.Barcode = good.Barcode;
        this.Barcodes = string.Join(",", good.Barcodes);
        this.Description = good.Description;
        this.PropertiesList = good.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => !x.Type.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid))).Select<EntityProperties.PropertyValue, ExchangeDataHelper.Good.Properties>((Func<EntityProperties.PropertyValue, ExchangeDataHelper.Good.Properties>) (x => new ExchangeDataHelper.Good.Properties()
        {
          UidType = x.Type.Uid,
          Name = x.Type.Name,
          Value = x.Value
        })).ToList<ExchangeDataHelper.Good.Properties>();
        if (this.PropertiesList.Any<ExchangeDataHelper.Good.Properties>((Func<ExchangeDataHelper.Good.Properties, bool>) (x => x.UidType == GlobalDictionaries.GoodIdUid)))
          this.Id = Convert.ToInt32(this.PropertiesList.First<ExchangeDataHelper.Good.Properties>((Func<ExchangeDataHelper.Good.Properties, bool>) (x => x.UidType == GlobalDictionaries.GoodIdUid)).Value);
        if (!good.StocksAndPrices.Any<GoodsStocks.GoodStock>())
          return;
        if (good.SetStatus.IsEither<GlobalDictionaries.GoodsSetStatuses>(GlobalDictionaries.GoodsSetStatuses.Set, GlobalDictionaries.GoodsSetStatuses.Kit))
        {
          GoodsCatalogModelView.GoodsInfoGrid good1 = new GoodsCatalogModelView.GoodsInfoGrid()
          {
            Good = good
          };
          GoodsSearchModelView.GetPriceForKit(good1);
          this.TotalStock = good1.GoodTotalStock.GetValueOrDefault();
          this.MaxPrice = good1.MaxPrice.GetValueOrDefault();
        }
        else
        {
          this.TotalStock = good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
          this.MaxPrice = SaleHelper.GetSalePriceForGood(good, SalePriceType.Min).GetValueOrDefault();
        }
      }

      public class Properties
      {
        public Guid UidType { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }
      }
    }
  }
}
