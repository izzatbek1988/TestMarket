// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.HomeOfficeHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers.HomeOffice.Entity;
using Gbs.Helpers.HomeOffice.Entity.Clients;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public class HomeOfficeHelper
  {
    private int _writeIndex;

    public static DateTime CloudArchiveLastWriteDateTime { get; set; }

    public static bool IsAcceptHome { get; set; }

    public static bool IsAuthRequire { get; set; }

    private static Gbs.Core.Config.DataBase SettingDataBase
    {
      get => new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
    }

    private static Gbs.Core.Config.Settings Setting => new ConfigsRepository<Gbs.Core.Config.Settings>().Get();

    private static Gbs.Core.Entities.Settings.Setting InfoDb => UidDb.GetUid();

    private List<HomeOfficeHelper.EditFileHome> EditFilesList { get; set; }

    public static void CreateArchive(bool isShowMsg = false)
    {
      try
      {
        Performancer performancer = new Performancer("Подготовка архива для синхронизации");
        if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
          return;
        Cloud cloud = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.Cloud;
        Gbs.Core.Entities.Settings.Setting uid = UidDb.GetUid();
        string str1 = Path.Combine(cloud.Path, uid.EntityUid.ToString());
        string str2 = FileSystemHelper.TempFolderPath();
        string str3 = Path.Combine(str2, uid.EntityUid.ToString());
        List<FileInfo> sourceFilePaths = new List<FileInfo>()
        {
          new FileInfo(ApplicationInfo.GetInstance().Paths.DataPath + "main.fdb")
        };
        if (!FileSystemHelper.ExistsOrCreateFolder(cloud.Path, false))
        {
          Directory.Delete(str2, true);
        }
        else
        {
          string contents = JsonConvert.SerializeObject((object) new HomeOfficeHelper.InfoArchive()
          {
            UidDevice = GbsIdHelperMain.GetGbsId(),
            IsAcceptHome = cloud.IsAcceptHome,
            NameDataBase = uid.Value.ToString(),
            IsSendBackupDb = cloud.IsSendBackupDb
          }, Formatting.Indented);
          File.WriteAllText(str1 + ".info", contents);
          if (cloud.IsSendBackupDb)
          {
            BackupHelper.CreateBackup(str2, uid.EntityUid.ToString() + ".7z");
            performancer.AddPoint("Подготовка архива (с сжатием БД)");
          }
          else
          {
            FileSystemHelper.CreateZip(str3 + ".7z", (IEnumerable<FileInfo>) sourceFilePaths);
            performancer.AddPoint("Подготовка архива (без сжатия БД)");
          }
          FileSystemHelper.MoveFile(str3 + ".7z", str1 + ".7z");
          performancer.AddPoint("Копирование файлов");
          Directory.Delete(str2, true);
          if (isShowMsg)
          {
            int num = (int) MessageBoxHelper.Show(Translate.АрхивСДаннымиУспешноВыгруженВПапкуОбмена);
          }
          performancer.Stop();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка создания архива для дом/офис", false);
      }
    }

    public void CreateEditFile<T>(T command, HomeOfficeHelper.EntityEditHome type)
    {
      if (DevelopersHelper.IsUnitTest() || HomeOfficeHelper.SettingDataBase.ModeProgram != GlobalDictionaries.Mode.Home)
        return;
      if (!HomeOfficeHelper.IsAcceptHome && type != HomeOfficeHelper.EntityEditHome.Sections)
        throw new Exception(Translate.HomeOfficeHelper_CreateJsonAndSend_Прием_изменений_из_дом_офис_отключен_в_торговой_точке__);
      LogHelper.Trace(string.Format("Отправка документа из д/о; entityType: {0}", (object) type));
      object obj;
      switch (type)
      {
        case HomeOfficeHelper.EntityEditHome.Good:
          obj = (object) new GoodHome((object) command as Gbs.Core.Entities.Goods.Good);
          break;
        case HomeOfficeHelper.EntityEditHome.GoodList:
          obj = (object) new GoodHomeList((object) command as List<Gbs.Core.Entities.Goods.Good>);
          break;
        case HomeOfficeHelper.EntityEditHome.GoodGroup:
          obj = (object) new GoodGroupHome((object) command as GoodGroups.Group);
          break;
        case HomeOfficeHelper.EntityEditHome.GoodGroupList:
          obj = (object) new GoodGroupHomeList((object) command as List<GoodGroups.Group>);
          break;
        case HomeOfficeHelper.EntityEditHome.GoodStockList:
          obj = (object) new GoodStockHomeList((object) command as List<GoodsStocks.GoodStock>);
          break;
        case HomeOfficeHelper.EntityEditHome.Client:
          obj = (object) new ClientHome((object) command as Client);
          break;
        case HomeOfficeHelper.EntityEditHome.ClientList:
          obj = (object) new ClientHomeList((object) command as List<Client>);
          break;
        case HomeOfficeHelper.EntityEditHome.ClientGroup:
          obj = (object) new ClientGroupHome((object) command as Gbs.Core.Entities.Clients.Group);
          break;
        case HomeOfficeHelper.EntityEditHome.ClientGroupList:
          obj = (object) new ClientGroupHomeList((object) command as List<Gbs.Core.Entities.Clients.Group>);
          break;
        case HomeOfficeHelper.EntityEditHome.Document:
          obj = (object) new DocumentHome((object) command as Document);
          break;
        case HomeOfficeHelper.EntityEditHome.EgaisWriteOffActsItemsList:
          obj = (object) ((object) command as List<EgaisWriteOffActsItems>);
          break;
        case HomeOfficeHelper.EntityEditHome.Sections:
          obj = (object) new SectionHome((object) command as Sections.Section);
          break;
        default:
          int num = (int) MessageBoxHelper.Show(Translate.HomeOfficeHelper_Внести_данные_изменения_из_дом_офис_невозможно_);
          return;
      }
      Guid guid = Guid.NewGuid();
      this.EditFilesList.Add(new HomeOfficeHelper.EditFileHome()
      {
        Uid = guid,
        Object = obj,
        Type = type
      });
    }

    public void Prepare() => this.EditFilesList = new List<HomeOfficeHelper.EditFileHome>();

    public void Send()
    {
      if (HomeOfficeHelper.SettingDataBase.ModeProgram != GlobalDictionaries.Mode.Home)
        return;
      string str1 = Path.Combine(HomeOfficeHelper.Setting.RemoteControl.Cloud.Path, "Delta_v2", HomeOfficeHelper.InfoDb.EntityUid.ToString());
      FileSystemHelper.ExistsOrCreateFolder(str1);
      Guid guid = Guid.NewGuid();
      string str2 = Path.Combine(str1, guid.ToString());
      string str3 = Path.Combine(FileSystemHelper.TempFolderPath(), guid.ToString());
      if (!FileSystemHelper.ExistsOrCreateFolder(str3, false))
        throw new FileNotFoundException(Translate.HomeOfficeHelper_Send_Не_удается_сохранить_измеения_для_дом_офис__Пожалуйста__обратитесь_в_техническую_поддержку_);
      foreach (HomeOfficeHelper.EditFileHome editFiles in this.EditFilesList)
      {
        string jsonString = editFiles.ToJsonString(true);
        File.WriteAllText(Path.Combine(str3, editFiles.Uid.ToString() + ".json"), jsonString);
      }
      FileSystemHelper.CreateZip(str3 + ".7z", str3);
      FileSystemHelper.MoveFile(str3 + ".7z", str2 + ".7z");
      Directory.Delete(str3, true);
      string str4 = Path.Combine(ApplicationInfo.GetInstance().Paths.CachePath, HomeOfficeHelper.InfoDb.EntityUid.ToString());
      if (!FileSystemHelper.ExistsOrCreateFolder(str4))
        return;
      File.WriteAllText(Path.Combine(str4, "time.config"), JsonConvert.SerializeObject((object) DateTime.Now));
    }

    public void PrepareAndSend<T>(T command, HomeOfficeHelper.EntityEditHome type)
    {
      this.Prepare();
      this.CreateEditFile<T>(command, type);
      this.Send();
    }

    private static void AddDocumentInFolder(string path)
    {
      try
      {
        string str1 = Path.Combine(ApplicationInfo.GetInstance().Paths.ArchivesFromHomePath);
        if (!Directory.Exists(str1))
          Directory.CreateDirectory(str1);
        string str2 = Path.Combine(str1, DateTime.Now.ToString("yyyy-MM-dd"));
        if (!Directory.Exists(str2))
          Directory.CreateDirectory(str2);
        FileInfo fileInfo = new FileInfo(path);
        FileSystemHelper.CopyFile(path, Path.Combine(str2, fileInfo.Name));
      }
      catch (Exception ex)
      {
        string message = "Не удается скоприровать файл из папки обмена в папку " + path;
        LogHelper.Error(ex, message);
      }
    }

    public static void FindFilesFromHomeOffice()
    {
      if (HomeOfficeHelper.SettingDataBase.ModeProgram == GlobalDictionaries.Mode.Home)
        return;
      if (!File.Exists(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().Connection.Path))
      {
        LogHelper.Debug("Не найдена база данных, изменения из дом/офис не вносим");
      }
      else
      {
        List<HomeOfficeHelper.EditFileHome> editFileHomeList = new List<HomeOfficeHelper.EditFileHome>();
        string nameDeltaFolder1 = "Delta_v1";
        editFileHomeList.AddRange((IEnumerable<HomeOfficeHelper.EditFileHome>) HomeOfficeHelper.GetFilesInCloud(nameDeltaFolder1));
        string nameDeltaFolder2 = "Delta_v2";
        editFileHomeList.AddRange((IEnumerable<HomeOfficeHelper.EditFileHome>) HomeOfficeHelper.GetFilesInCloud(nameDeltaFolder2));
        if (!editFileHomeList.Any<HomeOfficeHelper.EditFileHome>())
          return;
        HomeOfficeHelper.ExecuteFiles(editFileHomeList);
      }
    }

    private static List<HomeOfficeHelper.EditFileHome> GetFilesInCloud(string nameDeltaFolder)
    {
      string path = Path.Combine(HomeOfficeHelper.Setting.RemoteControl.Cloud.Path, nameDeltaFolder, HomeOfficeHelper.InfoDb.EntityUid.ToString());
      if (!Directory.Exists(path))
        return new List<HomeOfficeHelper.EditFileHome>();
      string[] files = Directory.GetFiles(path);
      if (!((IEnumerable<string>) files).Any<string>())
        return new List<HomeOfficeHelper.EditFileHome>();
      List<HomeOfficeHelper.EditFileHome> filesInCloud = new List<HomeOfficeHelper.EditFileHome>();
      List<FileInfo> source1 = new List<FileInfo>();
      if (nameDeltaFolder == "Delta_v2")
      {
        List<FileInfo> list = ((IEnumerable<string>) files).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).Where<FileInfo>((Func<FileInfo, bool>) (x => x.Extension == ".7z")).ToList<FileInfo>();
        string str = FileSystemHelper.TempFolderPath();
        foreach (FileInfo fileInfo in list)
        {
          try
          {
            using (ZipArchive zipArchive = ZipFile.OpenRead(fileInfo.FullName))
            {
              foreach (ZipArchiveEntry source2 in zipArchive.Entries.Where<ZipArchiveEntry>((Func<ZipArchiveEntry, bool>) (x => x.FullName.EndsWith(".json"))))
                source2.ExtractToFile(Path.Combine(str, source2.Name));
            }
            File.Delete(fileInfo.FullName);
          }
          catch (InvalidDataException ex)
          {
            LogHelper.WriteError((Exception) ex, "Ошибка при обработке архива " + fileInfo.FullName + ": " + ex.Message + ". Файл будет удалён.");
            File.Delete(fileInfo.FullName);
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Непредвиденная ошибка при обработке архива " + fileInfo.FullName + ": " + ex.Message);
          }
        }
        source1.AddRange(((IEnumerable<string>) Directory.GetFiles(str)).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))));
      }
      else
        source1 = ((IEnumerable<string>) files).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).Where<FileInfo>((Func<FileInfo, bool>) (x => x.Extension == ".json")).ToList<FileInfo>();
      foreach (FileInfo fileInfo in source1)
      {
        HomeOfficeHelper.AddDocumentInFolder(fileInfo.FullName);
        try
        {
          HomeOfficeHelper.EditFileHome editFileHome = JsonConvert.DeserializeObject<HomeOfficeHelper.EditFileHome>(File.ReadAllText(fileInfo.FullName));
          editFileHome.File = fileInfo;
          filesInCloud.Add(editFileHome);
        }
        catch
        {
          LogHelper.Debug("Не удалось разобрать файл из папки обмена");
          File.Delete(fileInfo.FullName);
        }
      }
      source1.Where<FileInfo>((Func<FileInfo, bool>) (x => x.Exists)).ToList<FileInfo>().ForEach((Action<FileInfo>) (x => x.Delete()));
      return filesInCloud;
    }

    private static void ExecuteFiles(List<HomeOfficeHelper.EditFileHome> listFile)
    {
      GlobalData.IsMarket5ImportAcitve = true;
      foreach (HomeOfficeHelper.EditFileHome editFileHome in (IEnumerable<HomeOfficeHelper.EditFileHome>) listFile.OrderBy<HomeOfficeHelper.EditFileHome, DateTime>((Func<HomeOfficeHelper.EditFileHome, DateTime>) (x => x.Date)))
      {
        if (!HomeOfficeHelper.Setting.RemoteControl.Cloud.IsAcceptHome && editFileHome.Type != HomeOfficeHelper.EntityEditHome.Sections)
        {
          GlobalData.IsMarket5ImportAcitve = false;
          return;
        }
        if (new JsonApiDocumentsRepository().GetByDocumentUid(editFileHome.Uid) != null)
        {
          File.Delete(editFileHome.File.FullName);
          LogHelper.Debug("Документ из дом офис с таким UID уже принимался, пропускаем.");
        }
        else if (editFileHome.Object == null)
        {
          File.Delete(editFileHome.File.FullName);
        }
        else
        {
          bool result = true;
          string str = editFileHome.Object.ToString();
          using (Gbs.Core.Db.DataBase db = Data.GetDataBase())
          {
            List<Gbs.Core.Entities.Goods.Good> goodsDb = new GoodRepository(db).GetAllItems();
            GoodGroupsRepository repGr = new GoodGroupsRepository(db);
            LogHelper.Debug(string.Format("Получили из дом/офис файл с сущностью {0}. Вносим изменения.", (object) editFileHome.Type) + string.Format("UID документа из облака - {0}", (object) editFileHome.Uid));
            try
            {
              switch (editFileHome.Type)
              {
                case HomeOfficeHelper.EntityEditHome.Good:
                  Gbs.Core.Entities.Goods.Good g1 = GoodsFactory.Create(JsonConvert.DeserializeObject<GoodHome>(str));
                  foreach (GoodsStocks.GoodStock stocksAndPrice in g1.StocksAndPrices)
                  {
                    GoodsStocks.GoodStock stock = stocksAndPrice;
                    GoodsStocks.GoodStock goodStock = stock;
                    Gbs.Core.Entities.Goods.Good good = goodsDb.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (gDb => gDb.Uid == g1.Uid));
                    Decimal num = (good != null ? good.StocksAndPrices.SingleOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (st => st.Uid == stock.Uid))?.Stock : new Decimal?()) ?? stock.Stock;
                    goodStock.Stock = num;
                  }
                  result = new GoodRepository(db).Save(g1);
                  break;
                case HomeOfficeHelper.EntityEditHome.GoodList:
                  JsonConvert.DeserializeObject<GoodHomeList>(str).GoodsList.ForEach((Action<GoodHome>) (x =>
                  {
                    Gbs.Core.Entities.Goods.Good g2 = GoodsFactory.Create(x);
                    foreach (GoodsStocks.GoodStock stocksAndPrice in g2.StocksAndPrices)
                    {
                      GoodsStocks.GoodStock stock = stocksAndPrice;
                      GoodsStocks.GoodStock goodStock = stock;
                      Gbs.Core.Entities.Goods.Good good = goodsDb.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (gDb => gDb.Uid == g2.Uid));
                      Decimal num = (good != null ? good.StocksAndPrices.SingleOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (st => st.Uid == stock.Uid))?.Stock : new Decimal?()) ?? stock.Stock;
                      goodStock.Stock = num;
                    }
                    result &= new GoodRepository(db).Save(g2);
                  }));
                  break;
                case HomeOfficeHelper.EntityEditHome.GoodGroup:
                  GoodGroupHome group = JsonConvert.DeserializeObject<GoodGroupHome>(str);
                  result = repGr.Save(new GoodGroups.Group(group));
                  break;
                case HomeOfficeHelper.EntityEditHome.GoodGroupList:
                  JsonConvert.DeserializeObject<GoodGroupHomeList>(str).Groups.ForEach((Action<GoodGroupHome>) (x => result &= repGr.Save(new GoodGroups.Group(x))));
                  break;
                case HomeOfficeHelper.EntityEditHome.GoodStockList:
                  JsonConvert.DeserializeObject<GoodStockHomeList>(str).GoodsStockList.ForEach((Action<GoodStockHome>) (x => result &= new GoodsStocks.GoodStock(x).UpdateProperties(db)));
                  break;
                case HomeOfficeHelper.EntityEditHome.Client:
                  ClientHome clientHome = JsonConvert.DeserializeObject<ClientHome>(str);
                  result = new ClientsRepository(db).Save(new Client(clientHome));
                  break;
                case HomeOfficeHelper.EntityEditHome.ClientList:
                  JsonConvert.DeserializeObject<ClientHomeList>(str).ClientList.ForEach((Action<ClientHome>) (x => result &= new ClientsRepository(db).Save(new Client(x))));
                  break;
                case HomeOfficeHelper.EntityEditHome.ClientGroup:
                  ClientGroupHome groupHome = JsonConvert.DeserializeObject<ClientGroupHome>(str);
                  result = new GroupRepository(db).Save(new Gbs.Core.Entities.Clients.Group(groupHome));
                  break;
                case HomeOfficeHelper.EntityEditHome.ClientGroupList:
                  JsonConvert.DeserializeObject<ClientGroupHomeList>(str).ClientGroupList.ForEach((Action<ClientGroupHome>) (x => result &= new GroupRepository(db).Save(new Gbs.Core.Entities.Clients.Group(x))));
                  break;
                case HomeOfficeHelper.EntityEditHome.Document:
                  Document document = new Document(JsonConvert.DeserializeObject<DocumentHome>(str));
                  document.Items.ForEach((Action<Gbs.Core.Entities.Documents.Item>) (x => x.Good = goodsDb.SingleOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g => g.Uid == x.GoodUid))));
                  result = new DocumentsRepository(db).Save(document);
                  break;
                case HomeOfficeHelper.EntityEditHome.EgaisWriteOffActsItemsList:
                  JsonConvert.DeserializeObject<List<EgaisWriteOffActsItems>>(str).ForEach((Action<EgaisWriteOffActsItems>) (x => result &= new EgaisWriteOffActsItemRepository().Save(x)));
                  break;
                case HomeOfficeHelper.EntityEditHome.Sections:
                  new Sections.Section(JsonConvert.DeserializeObject<SectionHome>(str)).Save();
                  result = true;
                  break;
                default:
                  throw new ArgumentOutOfRangeException();
              }
            }
            catch (Exception ex)
            {
              GlobalData.IsMarket5ImportAcitve = false;
              LogHelper.Error(ex, "Во время десериализации файла произошла ошибка, удаляем файл из облака", false);
              File.Delete(editFileHome.File.FullName);
              continue;
            }
            new JsonApiDocumentsRepository().Save(new JsonApiDocuments()
            {
              Date = editFileHome.Date,
              DocumentUid = editFileHome.Uid,
              IsSuccessful = result
            });
            File.Delete(editFileHome.File.FullName);
          }
        }
      }
      GlobalData.IsMarket5ImportAcitve = false;
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
      {
        Text = Translate.HomeOfficeHelper_Внесены_изменения_из_Дом_офис,
        Title = Translate.HomeOfficeHelper_Дом_офис
      });
      HomeOfficeHelper.CreateArchive();
    }

    public static List<HomeOfficeHelper.PointInfo> GetPointFromCloud()
    {
      List<FileInfo> list = ((IEnumerable<string>) Directory.GetFiles(HomeOfficeHelper.Setting.RemoteControl.Cloud.Path)).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).ToList<FileInfo>();
      List<HomeOfficeHelper.PointInfo> pointFromCloud = new List<HomeOfficeHelper.PointInfo>();
      foreach (FileInfo fileInfo in list.Where<FileInfo>((Func<FileInfo, bool>) (x => x.Extension == ".info")))
      {
        Guid result;
        if (Guid.TryParse(fileInfo.Name.Replace(".info", ""), out result))
        {
          try
          {
            HomeOfficeHelper.InfoArchive infoArchive = JsonConvert.DeserializeObject<HomeOfficeHelper.InfoArchive>(File.ReadAllText(fileInfo.FullName));
            pointFromCloud.Add(new HomeOfficeHelper.PointInfo()
            {
              DbUid = result,
              InfoDataBase = infoArchive
            });
          }
          catch (Exception ex)
          {
            bool flag = true;
            if (ex is JsonReaderException)
              flag = false;
            int num = flag ? 1 : 0;
            LogHelper.Error(ex, "Ошибка при получения информации о торговой точки", false, num != 0);
          }
        }
      }
      return pointFromCloud;
    }

    public enum EntityEditHome
    {
      Good = 10, // 0x0000000A
      GoodList = 15, // 0x0000000F
      GoodGroup = 16, // 0x00000010
      GoodGroupList = 17, // 0x00000011
      GoodStockList = 18, // 0x00000012
      Client = 20, // 0x00000014
      ClientList = 22, // 0x00000016
      ClientGroup = 25, // 0x00000019
      ClientGroupList = 26, // 0x0000001A
      Document = 30, // 0x0000001E
      EgaisWriteOffActsItemsList = 50, // 0x00000032
      Sections = 90, // 0x0000005A
    }

    public class PointInfo
    {
      public Guid DbUid { get; set; }

      public HomeOfficeHelper.InfoArchive InfoDataBase { get; set; }

      public FileInfo FileArchive
      {
        get
        {
          return new FileInfo(Path.Combine(HomeOfficeHelper.Setting.RemoteControl.Cloud.Path, this.DbUid.ToString() + ".7z"));
        }
      }
    }

    public class InfoArchive
    {
      public string NameDataBase { get; set; }

      public bool IsSendBackupDb { get; set; }

      public bool IsAcceptHome { get; set; }

      public string UidDevice { get; set; }
    }

    private class EditFileHome
    {
      [JsonIgnore]
      public FileInfo File { get; set; }

      public HomeOfficeHelper.EntityEditHome Type { get; set; }

      public Guid Uid { get; set; } = Guid.NewGuid();

      public object Object { get; set; }

      public DateTime Date { get; set; } = DateTime.Now;
    }
  }
}
