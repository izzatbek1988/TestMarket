// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DataBaseHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FirebirdSql.Data.FirebirdClient;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Clients;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Db.Payments;
using Gbs.Core.Db.Tables.Egais;
using Gbs.Core.Db.Tables.Users;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Egais;
using Gbs.Forms._shared;
using Gbs.Helpers.DB;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Ionic.Zip;
using LinqToDB;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class DataBaseHelper
  {
    private static string _pathToDb;

    [Localizable(false)]
    private static void CreateIndexes()
    {
      Performancer performancer = new Performancer("Создание индексов БД");
      LogHelper.OnBegin();
      DataBaseHelper.DeleteIndexes_V1();
      performancer.AddPoint("Удаление индексов");
      Dictionary<int, string> dictionary = new Dictionary<int, string>()
      {
        {
          0,
          "\"ENTITY_PROPERTIES_VALUES\" (\"ENTITY_UID\")"
        },
        {
          1,
          "\"ENTITY_PROPERTIES_VALUES\" (\"TYPE_UID\")"
        },
        {
          2,
          "\"ENTITY_PROPERTIES_TYPES\" (\"ENTITY_TYPE\")"
        },
        {
          3,
          "\"DOCUMENTS\" (\"PARENT_UID\")"
        },
        {
          4,
          "\"DOCUMENTS\" (\"TYPE\")"
        },
        {
          5,
          "\"DOCUMENT_ITEMS\" (\"DOCUMENT_UID\")"
        },
        {
          6,
          "\"DOCUMENT_ITEMS\" (\"STOCK_UID\")"
        },
        {
          7,
          "\"DOCUMENT_ITEMS\" (\"GOOD_UID\")"
        },
        {
          8,
          "\"GOODS_STOCK\" (\"GOOD_UID\")"
        },
        {
          9,
          "\"GOODS_STOCK\" (\"IS_DELETED\")"
        },
        {
          10,
          "\"PAYMENTS\" (\"PARENT_UID\")"
        },
        {
          11,
          "\"TEMP_DATA\" (\"SESSION_UID\")"
        },
        {
          12,
          "\"GOODS\" (\"IS_DELETED\")"
        },
        {
          13,
          "\"GOODS\" (\"SET_STATUS\")"
        },
        {
          14,
          "\"SETTINGS\" (\"TYPE\")"
        },
        {
          15,
          "\"SETTINGS\" (\"TYPE\", \"ENTITY_UID\")"
        },
        {
          16,
          "\"USERS\" (\"CLIENT_UID\", \"IS_KICKED\", \"IS_DELETED\")"
        },
        {
          17,
          "\"LINK_ENTITIES\" (\"ENTITY_UID\")"
        },
        {
          18,
          "\"ENTITY_PROPERTIES_VALUES\" (\"TYPE_UID\", \"ENTITY_UID\")"
        },
        {
          19,
          "\"CERTIFICATES\" (\"BARCODE\")"
        },
        {
          20,
          "\"CERTIFICATES\" (\"STOCK_UID\")"
        },
        {
          21,
          "\"DOCUMENTS\" (\"CONTRACTOR_UID\")"
        }
      };
      DataTable dataTable = new DataTable();
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        using (FbDataAdapter fbDataAdapter = new FbDataAdapter("SELECT RDB$INDEX_NAME FROM RDB$INDICES WHERE (RDB$INDEX_NAME LIKE 'gbs6indx_v2_%')", dataBase.FbConnection))
          fbDataAdapter.Fill(dataTable);
        performancer.AddPoint(string.Format("Подготовка списка индексов для создания. Count{0}", (object) dictionary.Count));
        foreach (KeyValuePair<int, string> keyValuePair in dictionary)
        {
          string str = string.Format("gbs6indx_v2_{0}", (object) keyValuePair.Key);
          bool flag = false;
          foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          {
            if (row[0].ToString().Trim() == str)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            string sql = "CREATE INDEX \"" + str + "\" ON " + keyValuePair.Value;
            LogHelper.Trace("Создание индекса запросом: " + sql);
            DataBaseHelper.ExecuteSqlRawCommand(sql, dataBase);
          }
          else
            LogHelper.Trace("Индекс " + str + " уже существует в БД");
        }
        performancer.Stop();
        LogHelper.OnEnd();
      }
    }

    [Localizable(false)]
    private static void DeleteIndexes_V1()
    {
      DataTable dataTable = new DataTable();
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        using (FbDataAdapter fbDataAdapter = new FbDataAdapter("SELECT RDB$INDEX_NAME FROM RDB$INDICES WHERE (RDB$INDEX_NAME LIKE 'gbs6index%')", dataBase.FbConnection))
          fbDataAdapter.Fill(dataTable);
        foreach (string sql in dataTable.Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (row => string.Format("DROP INDEX \"{0}\"", row[0]))))
          DataBaseHelper.ExecuteSqlRawCommand(sql, dataBase);
      }
    }

    public static string PathToDb
    {
      get => DataBaseHelper._pathToDb;
      set
      {
        DataBaseHelper._pathToDb = value;
        DataBaseHelper.CheckDbConnection();
      }
    }

    public static string DbConnectionString => DataBaseHelper.GetConnectionStringFromConfigs();

    private static Gbs.Core.Config.DataBase DbConfig { get; set; } = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();

    public static bool RestoreData(string pathZip, bool restoreDb, bool restoreConfig)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DataBasePageViewModel_Восстановление_данных_из_резервной_копии);
      string str1 = FileSystemHelper.TempFolderPath();
      ZipFile zipFile = FileSystemHelper.OpenZip(pathZip);
      string dataPath = ApplicationInfo.GetInstance().Paths.DataPath;
      try
      {
        FileSystemHelper.ExtractAllFile(pathZip, str1);
        if (restoreDb)
        {
          if (zipFile.Entries.Count<ZipEntry>((System.Func<ZipEntry, bool>) (x => x.FileName.Contains(".fbk"))) != 1)
          {
            progressBar.Close();
            MessageBoxHelper.Warning(Translate.DataBasePageViewModel_В_указанном_архиве_отсутствует_база_данных__восстановление_невозможно_);
            return false;
          }
          string str2 = Path.Combine(str1, zipFile.Entries.FirstOrDefault<ZipEntry>((System.Func<ZipEntry, bool>) (x => x.FileName.Contains(".fbk")))?.FileName ?? "");
          BackupHelper.RestoreDataBase(str2, Path.Combine(str1, "main.fdb"));
          File.Delete(str2);
          DataBaseHelper.CloseConnection();
          string path = Path.Combine(dataPath, "main.fdb");
          if (File.Exists(path))
            File.Delete(path);
          foreach (string file in Directory.GetFiles(str1, "*.*", SearchOption.AllDirectories))
          {
            if (file.ToLower().Contains("main"))
            {
              try
              {
                File.Copy(file, file.Replace(str1, dataPath), true);
                break;
              }
              catch (Exception ex)
              {
                Directory.Delete(str1, true);
                string message = "Не удалось перенести в папку с данными файл " + file;
                LogHelper.Error(ex, message, false);
                throw;
              }
            }
          }
        }
        if (restoreConfig)
        {
          foreach (string directory in Directory.GetDirectories(str1, "*", SearchOption.AllDirectories))
          {
            if (!directory.Contains("Logs"))
            {
              try
              {
                Directory.CreateDirectory(directory.Replace(str1, dataPath));
              }
              catch (Exception ex)
              {
                Directory.Delete(str1, true);
                string message = "Не удалось перенести в папку с данными папку " + directory;
                LogHelper.Error(ex, message, false);
                throw;
              }
            }
          }
          foreach (string file in Directory.GetFiles(str1, "*.*", SearchOption.AllDirectories))
          {
            if (!file.Contains("Logs") && (!restoreDb || !file.ToLower().Contains("main")))
            {
              try
              {
                File.Copy(file, file.Replace(str1, dataPath), true);
              }
              catch (Exception ex)
              {
                Directory.Delete(str1, true);
                string message = "Не удалось перенести в папку с данными файл " + file;
                LogHelper.Error(ex, message, false);
                throw;
              }
            }
          }
        }
        Directory.Delete(str1, true);
        new ConfigsRepository<Settings>().ReloadCache();
        Settings config1 = new ConfigsRepository<Settings>().Get();
        config1.Interface.TemplatesFrPath = Path.Combine(ApplicationInfo.GetInstance().Paths.DataPath, "TemplatesFR");
        new ConfigsRepository<Settings>().Save(config1);
        Gbs.Core.Config.DataBase config2 = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
        if (config2.Connection.ServerUrl.Contains("127.0.0.1") || config2.Connection.ServerUrl.Contains("localhost"))
        {
          config2.Connection.Path = Path.Combine(ApplicationInfo.GetInstance().Paths.DataPath, "MAIN.FDB");
          new ConfigsRepository<Gbs.Core.Config.DataBase>().Save(config2);
        }
        new ConfigsRepository<Settings>().ReloadCache();
        new ConfigsRepository<Gbs.Core.Config.DataBase>().ReloadCache();
        progressBar.Close();
        int num = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Данные_восстановлены_из_резервной_копии__для_продолжения_работы_программа_будет_перезапущена_);
        if (!Other.RestartApplication(false))
        {
          System.Environment.Exit(0);
          return true;
        }
      }
      catch (Exception ex)
      {
        Directory.Delete(str1, true);
        LogHelper.Error(ex, "Ошибка при восстановлении данных из копии.", false);
        progressBar.Close();
        int num = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Не_удалось_восстановить_резервную_копию__обратитесь_в_техническую_поддержку_);
        return false;
      }
      return false;
    }

    public static bool RestoreBackUp(string pathZip)
    {
      return DataBaseHelper.RestoreData(pathZip, true, false);
    }

    public static bool RestoreConfigBackUp(string pathZip)
    {
      return DataBaseHelper.RestoreData(pathZip, false, true);
    }

    public static bool RestoreAllBackUp(string pathZip)
    {
      return DataBaseHelper.RestoreData(pathZip, true, true);
    }

    public static bool CompressDb()
    {
      string str1 = FileSystemHelper.TempFolderPath();
      try
      {
        LogHelper.Debug("Начинаем процесс сжатия БД");
        string str2 = "main.zip";
        int num = BackupHelper.CreateBackup(str1, str2) ? 1 : 0;
        FileSystemHelper.ExtractAllFile(Path.Combine(str1, str2), str1);
        string path2 = ((IEnumerable<FileInfo>) new DirectoryInfo(str1).GetFiles()).FirstOrDefault<FileInfo>((System.Func<FileInfo, bool>) (x => x.Name.Contains(".fbk")))?.Name ?? "";
        string pathFbk = Path.Combine(str1, path2);
        string str3 = Path.Combine(str1, "main.fdb");
        if (num == 0)
        {
          Directory.Delete(str1, true);
          LogHelper.Debug("Не удалось создать РК для сжатия");
          return false;
        }
        BackupHelper.RestoreDataBase(pathFbk, str3);
        DataBaseHelper.CloseConnection();
        string dataPath = ApplicationInfo.GetInstance().Paths.DataPath;
        string path = Path.Combine(dataPath, "main.fdb");
        if (File.Exists(path))
          File.Delete(path);
        File.Copy(str3, Path.Combine(dataPath, "main.fdb"));
        Directory.Delete(str1, true);
        LogHelper.Debug("Закончили сжатие БД");
        return true;
      }
      catch (Exception ex)
      {
        Directory.Delete(str1, true);
        LogHelper.Error(ex, "Ошибка при сжатии базы данных", false);
        return false;
      }
    }

    public static string GetConnectionStringFromConfigs(Gbs.Core.Config.DataBase dbConfig = null)
    {
      Gbs.Core.Config.DataBase dataBase = dbConfig ?? DataBaseHelper.DbConfig;
      return new FbConnectionStringBuilder()
      {
        Charset = "UTF8",
        DataSource = dataBase.Connection.ServerUrl,
        Port = dataBase.Connection.ServerPort,
        UserID = dataBase.Connection.DecryptedLogin,
        Password = DataBaseHelper.DbConfig.Connection.DecryptedPassword,
        Database = (DataBaseHelper.PathToDb ?? dataBase.Connection.Path),
        ServerType = FbServerType.Default,
        ConnectionTimeout = 15,
        ConnectionLifeTime = 15
      }.ConnectionString;
    }

    public static void PrepareDb()
    {
      DataBaseHelper.DbConfig = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
      string serverUrl = DataBaseHelper.DbConfig.Connection.ServerUrl;
      if (serverUrl.ToLower().Contains("localhost") || serverUrl.Contains("127.0.0.1"))
        LogHelper.Debug("Размер БД: " + FileSystemHelper.GetSizeFile(DataBaseHelper.DbConfig.Connection.Path));
      FirebirdHelper.CheckLocalServiceRunning();
      DataBaseHelper.CreateDatabase();
      DataBaseHelper.CreateDbTables();
      if (DataBaseHelper.DbConfig.ModeProgram == GlobalDictionaries.Mode.Home)
        return;
      if (!DataBaseHelper.WriteDefaultData())
        return;
      try
      {
        DataBaseHelper.CreateIndexes();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
      new DataBaseCorrector().DoCorrection();
    }

    private static bool CreatePayments()
    {
      bool payments = true;
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        IQueryable<PAYMENT_METHODS> table = dataBase.GetTable<PAYMENT_METHODS>();
        if (!table.Any<PAYMENT_METHODS>())
          new PaymentMethods.PaymentMethod()
          {
            Name = Translate.DataBaseHelper_CreatePayments_Картой,
            KkmMethod = GlobalDictionaries.KkmPaymentMethods.Card,
            DisplayIndex = 1
          }.Save();
        if (!table.Any<PAYMENT_METHODS>((Expression<System.Func<PAYMENT_METHODS, bool>>) (x => x.UID == GlobalDictionaries.BonusesPaymentUid)))
        {
          int num1 = payments ? 1 : 0;
          PaymentMethods.PaymentMethod paymentMethod = new PaymentMethods.PaymentMethod();
          paymentMethod.Uid = GlobalDictionaries.BonusesPaymentUid;
          paymentMethod.Name = Translate.OnStartWorker_CreatePayments_Баллами;
          paymentMethod.KkmMethod = GlobalDictionaries.KkmPaymentMethods.Bonus;
          int num2 = paymentMethod.Save() ? 1 : 0;
          payments = (num1 & num2) != 0;
        }
        if (!table.Any<PAYMENT_METHODS>((Expression<System.Func<PAYMENT_METHODS, bool>>) (x => x.UID == GlobalDictionaries.CertificateNominalUid)))
        {
          int num3 = payments ? 1 : 0;
          PaymentMethods.PaymentMethod paymentMethod = new PaymentMethods.PaymentMethod();
          paymentMethod.Uid = GlobalDictionaries.CertificatePaymentUid;
          paymentMethod.KkmMethod = GlobalDictionaries.KkmPaymentMethods.Certificate;
          paymentMethod.Name = Translate.DataBaseHelper_Сертификатом;
          int num4 = paymentMethod.Save() ? 1 : 0;
          payments = (num3 & num4) != 0;
        }
        return payments;
      }
    }

    public static bool WriteDefaultData()
    {
      try
      {
        DataBaseHelper.CreateDefaultEntities();
        DataBaseHelper.CreateClientsProperties();
        DataBaseHelper.CreatePaymentAccount();
        return DataBaseHelper.CreateGoodsProperties() && DataBaseHelper.CreatePayments() && DataBaseHelper.CreateDocumentProperties();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка записи значений по умолчанию в БД");
        return false;
      }
    }

    private static void CreatePaymentAccount()
    {
      PaymentsAccounts.PaymentsAccount paymentsAccount = PaymentsAccounts.GetPaymentsAccountsList().FirstOrDefault<PaymentsAccounts.PaymentsAccount>((System.Func<PaymentsAccounts.PaymentsAccount, bool>) (x => x.IsCurrentMoneyBox()));
      // ISSUE: explicit non-virtual call
      if (paymentsAccount == null || !__nonvirtual (paymentsAccount.IsDeleted))
        return;
      paymentsAccount.IsDeleted = false;
      paymentsAccount.Save();
    }

    private static void CreateDefaultEntities()
    {
      if (!Storages.GetStorages().Any<Storages.Storage>())
        new Storages.Storage()
        {
          Name = Translate.SplashScreenViewModel_Основной
        }.Save();
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        GroupRepository groupRepository = new GroupRepository(dataBase);
        if (!groupRepository.GetActiveItems().Any<Gbs.Core.Entities.Clients.Group>((System.Func<Gbs.Core.Entities.Clients.Group, bool>) (x => x.IsSupplier)))
        {
          Gbs.Core.Entities.Clients.Group group = new Gbs.Core.Entities.Clients.Group()
          {
            Name = Translate.DataBaseHelper_Поставщики,
            IsSupplier = true
          };
          groupRepository.Save(group);
        }
        if (dataBase.GetTable<TAP_BEER>().Any<TAP_BEER>())
          return;
        TapBeerRepository tapBeerRepository = new TapBeerRepository();
        for (int index = 5; index > 0; --index)
          tapBeerRepository.Save(new TapBeer()
          {
            Name = "Кран #" + index.ToString()
          });
      }
    }

    private static void CreateClientsProperties()
    {
      using (IEnumerator<EntityProperties.PropertyType> enumerator = new Dictionary<Guid, string>()
      {
        {
          GlobalDictionaries.InnUid,
          Translate.FrmUserCard_ИНН
        },
        {
          GlobalDictionaries.OgrnUid,
          Translate.DataBaseHelper_ОГРН
        },
        {
          GlobalDictionaries.KppUid,
          Translate.Market5ImportHelper_КПП
        },
        {
          GlobalDictionaries.RsUid,
          Translate.Market5ImportHelper_Расчетный_счет
        },
        {
          GlobalDictionaries.KsUid,
          Translate.Market5ImportHelper_Кор__счет
        },
        {
          GlobalDictionaries.BikUid,
          Translate.Market5ImportHelper_БИК
        },
        {
          GlobalDictionaries.BankNameUid,
          Translate.Market5ImportHelper_Банк
        }
      }.Select<KeyValuePair<Guid, string>, EntityProperties.PropertyType>((System.Func<KeyValuePair<Guid, string>, EntityProperties.PropertyType>) (item =>
      {
        return new EntityProperties.PropertyType()
        {
          EntityType = GlobalDictionaries.EntityTypes.Client,
          Type = GlobalDictionaries.EntityPropertyTypes.Text,
          Uid = item.Key,
          Name = item.Value
        };
      })).Where<EntityProperties.PropertyType>((System.Func<EntityProperties.PropertyType, bool>) (prop => !prop.Save())).GetEnumerator())
      {
        if (!enumerator.MoveNext())
          return;
        EntityProperties.PropertyType current = enumerator.Current;
      }
    }

    private static bool CreateDocumentProperties()
    {
      Dictionary<Guid, string> source = new Dictionary<Guid, string>();
      source.Add(GlobalDictionaries.NumTableUid, Translate.DataBaseHelper_Номер_стола);
      source.Add(GlobalDictionaries.CountGuestUid, Translate.DataBaseHelper_Количество_гостей);
      source.Add(GlobalDictionaries.FiscalNumUid, Translate.DataBaseHelper_CreateDocumentProperties_Фискальный_номер_документа);
      source.Add(GlobalDictionaries.TerminalIdPropertyUid, Translate.DataBaseHelper_CreateDocumentProperties_Номер_фискального_терминала);
      source.Add(GlobalDictionaries.ReceiptSeqPropertyUid, Translate.DataBaseHelper_CreateDocumentProperties_SEQ_чека);
      source.Add(GlobalDictionaries.DateTimePropertyUid, Translate.DataBaseHelper_CreateDocumentProperties_Дата_фискального_чека);
      source.Add(GlobalDictionaries.FiscalSignPropertyUid, Translate.DataBaseHelper_CreateDocumentProperties_Подпись_фискального_чека);
      source.Add(GlobalDictionaries.RrnUid, Translate.DataBaseHelper_CreateDocumentProperties_RRN_номер_ссылки_для_эквайринга);
      source.Add(GlobalDictionaries.RrnSbpUid, Translate.DataBaseHelper_CreateDocumentProperties_Номер_ссылки_для_СБП);
      source.Add(GlobalDictionaries.TypeCardMethodUid, Translate.DataBaseHelper_CreateDocumentProperties_Метод_соверешния_операции_картой);
      source.Add(GlobalDictionaries.StatusEgais, Translate.DataBaseHelper_CreateDocumentProperties_Статус_накладной__ЕГАИС_);
      source.Add(GlobalDictionaries.ReplayUidEgais, Translate.DataBaseHelper_CreateDocumentProperties_Идентификатор_ответа__ЕГАИС_);
      source.Add(GlobalDictionaries.TTNEgais, Translate.DataBaseHelper_CreateDocumentProperties_ТТН_номер_накладной__ЕГАИС_);
      source.Add(GlobalDictionaries.OptionalClientOrderUid, Translate.DataBaseHelper_CreateDocumentProperties_Дополнительные_опции_для_заказов_резервов);
      source.Add(GlobalDictionaries.InfoWithTrueApiUid, Translate.DataBaseHelper_CreateDocumentProperties_UID_проверки_КМ_в_Честном_знаке);
      source.Add(GlobalDictionaries.FrNumber, Translate.DataBaseHelper_CreateDocumentProperties_Серийный_номер_ККМ_для_чека);
      source.Add(GlobalDictionaries.TsdDocumentNumberUid, Translate.DataBaseHelper_CreateDocumentProperties_Идентификатор_документа__передаваемый_на_ТСД);
      EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
      propertyType.EntityType = GlobalDictionaries.EntityTypes.Document;
      propertyType.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType.Uid = GlobalDictionaries.DocumentInCreditUid;
      propertyType.Name = "Долг по документу";
      propertyType.Save();
      return source.Select<KeyValuePair<Guid, string>, EntityProperties.PropertyType>((System.Func<KeyValuePair<Guid, string>, EntityProperties.PropertyType>) (item =>
      {
        return new EntityProperties.PropertyType()
        {
          EntityType = GlobalDictionaries.EntityTypes.Document,
          Type = GlobalDictionaries.EntityPropertyTypes.Text,
          Uid = item.Key,
          Name = item.Value
        };
      })).All<EntityProperties.PropertyType>((System.Func<EntityProperties.PropertyType, bool>) (prop => prop.Save()));
    }

    private static bool CreateGoodsProperties()
    {
      EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
      propertyType1.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType1.Type = GlobalDictionaries.EntityPropertyTypes.Decimal;
      propertyType1.Uid = GlobalDictionaries.CertificateNominalUid;
      propertyType1.Name = Translate.DataBaseHelper_CreateGoodsProperties_Номинал_сертификата;
      EntityProperties.PropertyType propertyType2 = propertyType1;
      EntityProperties.PropertyType propertyType3 = new EntityProperties.PropertyType();
      propertyType3.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType3.Type = GlobalDictionaries.EntityPropertyTypes.Integer;
      propertyType3.Uid = GlobalDictionaries.CertificateReusableUid;
      propertyType3.Name = Translate.DataBaseHelper_CreateGoodsProperties_Многоразовый_сертификат;
      EntityProperties.PropertyType propertyType4 = propertyType3;
      EntityProperties.PropertyType propertyType5 = new EntityProperties.PropertyType();
      propertyType5.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType5.Type = GlobalDictionaries.EntityPropertyTypes.AutoNum;
      propertyType5.Uid = GlobalDictionaries.GoodIdUid;
      propertyType5.Name = Translate.FrmGoodsCatalog_КодТовара;
      EntityProperties.PropertyType propertyType6 = new EntityProperties.PropertyType();
      propertyType6.EntityType = GlobalDictionaries.EntityTypes.GoodStock;
      propertyType6.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType6.Uid = GlobalDictionaries.RegIdForGoodStockUidEgais;
      propertyType6.Name = "Номер справки для остатка";
      EntityProperties.PropertyType propertyType7 = propertyType6;
      EntityProperties.PropertyType propertyType8 = new EntityProperties.PropertyType();
      propertyType8.EntityType = GlobalDictionaries.EntityTypes.GoodStock;
      propertyType8.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType8.Uid = GlobalDictionaries.MarkedInfoGood;
      propertyType8.Name = "Маркирвока для кеги пива";
      EntityProperties.PropertyType propertyType9 = propertyType8;
      return propertyType5.Save() && propertyType2.Save() && propertyType4.Save() && propertyType7.Save() && propertyType9.Save();
    }

    public static bool CreatePropertiesForCountry()
    {
      EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
      propertyType1.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType1.Type = GlobalDictionaries.EntityPropertyTypes.Text;
      propertyType1.Uid = GlobalDictionaries.IkpuUid;
      propertyType1.Name = "Код ИКПУ";
      propertyType1.IsDeleted = new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Uzbekistan;
      EntityProperties.PropertyType propertyType2 = propertyType1;
      EntityProperties.PropertyType propertyType3 = new EntityProperties.PropertyType();
      propertyType3.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType3.Type = GlobalDictionaries.EntityPropertyTypes.Text;
      propertyType3.Uid = GlobalDictionaries.UktZedUid;
      propertyType3.Name = "УКТЗЕД";
      EntityProperties.PropertyType propertyType4 = propertyType3;
      int num;
      if (new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Ukraine)
        num = !new ConfigsRepository<Settings>().Get().GoodsConfig.UktZedUid.IsEither<Guid>(Guid.Empty, GlobalDictionaries.UktZedUid) ? 1 : 0;
      else
        num = 1;
      propertyType4.IsDeleted = num != 0;
      EntityProperties.PropertyType propertyType5 = propertyType3;
      EntityProperties.PropertyType propertyType6 = new EntityProperties.PropertyType();
      propertyType6.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType6.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType6.Uid = GlobalDictionaries.AmClassifierIdUid;
      propertyType6.Name = "Классификатор товара (Армения)";
      propertyType6.IsDeleted = new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Armenia;
      EntityProperties.PropertyType propertyType7 = propertyType6;
      EntityProperties.PropertyType propertyType8 = new EntityProperties.PropertyType();
      propertyType8.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType8.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType8.Uid = GlobalDictionaries.AlcCodeUid;
      propertyType8.Name = "Код АП (ЕГАИС)";
      propertyType8.IsDeleted = new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia || !new ConfigsRepository<Integrations>().Get().Egais.IsActive;
      EntityProperties.PropertyType propertyType9 = propertyType8;
      EntityProperties.PropertyType propertyType10 = new EntityProperties.PropertyType();
      propertyType10.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType10.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType10.Uid = GlobalDictionaries.AlcVolumeUid;
      propertyType10.Name = "Крепость (ЕГАИС)";
      propertyType10.IsDeleted = new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia || !new ConfigsRepository<Integrations>().Get().Egais.IsActive;
      EntityProperties.PropertyType propertyType11 = propertyType10;
      EntityProperties.PropertyType propertyType12 = new EntityProperties.PropertyType();
      propertyType12.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType12.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType12.Uid = GlobalDictionaries.CapacityUid;
      propertyType12.Name = "Объем (ЕГАИС)";
      propertyType12.IsDeleted = new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia || !new ConfigsRepository<Integrations>().Get().Egais.IsActive;
      EntityProperties.PropertyType propertyType13 = propertyType12;
      EntityProperties.PropertyType propertyType14 = new EntityProperties.PropertyType();
      propertyType14.EntityType = GlobalDictionaries.EntityTypes.Good;
      propertyType14.Type = GlobalDictionaries.EntityPropertyTypes.System;
      propertyType14.Uid = GlobalDictionaries.ProductCodeUid;
      propertyType14.Name = "Тип продукции";
      propertyType14.IsDeleted = new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia || !new ConfigsRepository<Integrations>().Get().Egais.IsActive;
      EntityProperties.PropertyType propertyType15 = propertyType14;
      EntityProperties.PropertyType propertyType16 = new EntityProperties.PropertyType();
      propertyType16.EntityType = GlobalDictionaries.EntityTypes.Client;
      propertyType16.Type = GlobalDictionaries.EntityPropertyTypes.Text;
      propertyType16.Uid = GlobalDictionaries.FiasUid;
      propertyType16.Name = "ФИАС";
      propertyType16.IsDeleted = new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia;
      EntityProperties.PropertyType propertyType17 = propertyType16;
      if (new ConfigsRepository<Settings>().Get().GoodsConfig.UktZedUid == Guid.Empty)
      {
        Settings config = new ConfigsRepository<Settings>().Get();
        config.GoodsConfig.UktZedUid = GlobalDictionaries.UktZedUid;
        new ConfigsRepository<Settings>().Save(config);
      }
      return propertyType2.Save() && propertyType5.Save() && propertyType7.Save() && propertyType9.Save() && propertyType11.Save() && propertyType15.Save() && propertyType13.Save() && propertyType17.Save();
    }

    private static void CreateDatabase()
    {
      if (DevelopersHelper.IsUnitTest())
        DataBaseHelper.DbConfig = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
      if (File.Exists(DataBaseHelper.DbConfig.Connection.Path))
        return;
      if (DataBaseHelper.DbConfig.Connection.ServerUrl.ToLower().IsEither<string>("localhost", "127.0.0.1"))
      {
        LogHelper.Debug("Создание БД");
        FbConnection.CreateDatabase(DataBaseHelper.DbConnectionString);
        LogHelper.Debug("БД создана");
      }
      DataBaseHelper.CheckDbConnection();
    }

    public static bool ExecuteSqlRawCommand(string sql, Gbs.Core.Db.DataBase db)
    {
      try
      {
        using (FbCommand fbCommand = new FbCommand(sql, db.FbConnection))
        {
          fbCommand.ExecuteNonQuery();
          return true;
        }
      }
      catch (Exception ex)
      {
        string message = "Ошибка при выполнении запроса к БД. Запрос: " + Other.NewLine() + " " + sql;
        LogHelper.Error(ex, message, false);
        return false;
      }
    }

    public static void CheckDbConnection()
    {
      LogHelper.Trace("Проверка подклчюения к БД");
      Other.ConsoleWrite("check connection: " + DataBaseHelper.DbConnectionString.ToJsonString(true));
      using (FbConnection fbConnection = new FbConnection(DataBaseHelper.DbConnectionString))
      {
        fbConnection.Open();
        fbConnection.Close();
        fbConnection.Dispose();
        LogHelper.Trace("Проверка подключения к БД завершена");
      }
    }

    public static bool PingToDb(Gbs.Core.Config.DataBase dataBase)
    {
      try
      {
        using (FbConnection fbConnection = new FbConnection(DataBaseHelper.GetConnectionStringFromConfigs(dataBase)))
        {
          fbConnection.Open();
          fbConnection.Close();
          fbConnection.Dispose();
          return true;
        }
      }
      catch
      {
        return false;
      }
    }

    public static void CloseConnection()
    {
      try
      {
        LogHelper.Debug("DB connection close");
        FbConnection.ClearAllPools();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка закрытия соединения с БД");
      }
    }

    public static void DoSql(string pathSql)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SplashScreenViewModel_Выполнение_SQL_скрипта);
      string[] source = File.ReadAllText(pathSql).Split(new string[1]
      {
        "----"
      }, StringSplitOptions.None);
      using (Gbs.Core.Db.DataBase db = Gbs.Core.Data.GetDataBase())
      {
        int num1 = ((IEnumerable<string>) source).Count<string>((System.Func<string, bool>) (s => DataBaseHelper.ExecuteSqlRawCommand(s, db)));
        progressBar.Close();
        if (num1 == 0)
        {
          int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.SplashScreenViewModelСкриптНеБылВыполненВсеКомандыЗавершилисьСОшибкой, (object) source.Length), icon: MessageBoxImage.Hand);
        }
        else if (num1 != source.Length)
        {
          int num3 = (int) MessageBoxHelper.Show(string.Format(Translate.SplashScreenViewModelВыполненоСОшибкамиУспешноВыполненоИзКоманд, (object) num1, (object) source.Length), icon: MessageBoxImage.Exclamation);
        }
        else
        {
          int num4 = (int) MessageBoxHelper.Show(string.Format(Translate.SplashScreenViewModelСкриптВыполненУспешноВыполненоИзКоманд, (object) num1, (object) source.Length));
        }
        try
        {
          File.Delete(pathSql);
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка при удалении sql-файла", false);
        }
      }
    }

    private static bool TableExist(string tableName)
    {
      try
      {
        string[] restrictionValues = new string[4]
        {
          null,
          null,
          tableName,
          null
        };
        using (FbConnection fbConnection = new FbConnection(DataBaseHelper.DbConnectionString))
        {
          fbConnection.Open();
          DataTable schema = fbConnection.GetSchema("Tables", restrictionValues);
          fbConnection.Close();
          fbConnection.Dispose();
          return schema.Rows.Count != 0;
        }
      }
      catch (Exception ex)
      {
        string message = "Ошибка при проверки существования таблицы в БД. \r\ntableName: " + tableName;
        LogHelper.Error(ex, message);
        return false;
      }
    }

    public static List<Gbs.Core.Db.DbTable> GetAllTablesClasses()
    {
      return new List<Gbs.Core.Db.DbTable>()
      {
        (Gbs.Core.Db.DbTable) new TAP_BEER(),
        (Gbs.Core.Db.DbTable) new CLIENTS(),
        (Gbs.Core.Db.DbTable) new CLIENTS_GROUPS(),
        (Gbs.Core.Db.DbTable) new DOCUMENTS(),
        (Gbs.Core.Db.DbTable) new DOCUMENT_ITEMS(),
        (Gbs.Core.Db.DbTable) new CERTIFICATES(),
        (Gbs.Core.Db.DbTable) new GOODS(),
        (Gbs.Core.Db.DbTable) new GOODS_GROUPS(),
        (Gbs.Core.Db.DbTable) new GOODS_MODIFICATIONS(),
        (Gbs.Core.Db.DbTable) new GOODS_SETS(),
        (Gbs.Core.Db.DbTable) new GOODS_STOCK(),
        (Gbs.Core.Db.DbTable) new GOODS_UNITS(),
        (Gbs.Core.Db.DbTable) new PAYMENT_METHODS(),
        (Gbs.Core.Db.DbTable) new PAYMENTS(),
        (Gbs.Core.Db.DbTable) new PAYMENTS_ACCOUNT(),
        (Gbs.Core.Db.DbTable) new PAYMENTS_GROUP(),
        (Gbs.Core.Db.DbTable) new USER_HISTORY(),
        (Gbs.Core.Db.DbTable) new USERS(),
        (Gbs.Core.Db.DbTable) new USERS_GROUPS(),
        (Gbs.Core.Db.DbTable) new ACTIONS_HISTORY(),
        (Gbs.Core.Db.DbTable) new EMAIL(),
        (Gbs.Core.Db.DbTable) new ENTITY_PROPERTIES_TYPES(),
        (Gbs.Core.Db.DbTable) new ENTITY_PROPERTIES_VALUES(),
        (Gbs.Core.Db.DbTable) new JSON_API_DOCUMENTS(),
        (Gbs.Core.Db.DbTable) new LINK_ENTITIES(),
        (Gbs.Core.Db.DbTable) new SALE_POINTS(),
        (Gbs.Core.Db.DbTable) new SECTIONS(),
        (Gbs.Core.Db.DbTable) new SETTINGS(),
        (Gbs.Core.Db.DbTable) new SHIFTS(),
        (Gbs.Core.Db.DbTable) new STORAGES(),
        (Gbs.Core.Db.DbTable) new TEMP_DATA(),
        (Gbs.Core.Db.DbTable) new TEMP_SESSIONS(),
        (Gbs.Core.Db.DbTable) new EGAIS_WRITEOFFACTS(),
        (Gbs.Core.Db.DbTable) new EGAIS_WRITEOFFACTS_ITEMS(),
        (Gbs.Core.Db.DbTable) new INFO_TO_TAP_BEER()
      };
    }

    private static void CreateDbTables()
    {
      LogHelper.Debug("Создание таблиц в БД");
      DataBaseHelper.CheckDbConnection();
      try
      {
        List<Gbs.Core.Db.DbTable> allTablesClasses = DataBaseHelper.GetAllTablesClasses();
        LogHelper.Debug(string.Format("Таблиц для создания: {0}", (object) allTablesClasses.Count));
        foreach (Gbs.Core.Db.DbTable c in allTablesClasses)
          DataBaseHelper.CreateTables.CreateTableFromClass(c);
      }
      catch (ReflectionTypeLoadException ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Exception loaderException in ex.LoaderExceptions)
        {
          stringBuilder.AppendLine(loaderException.Message);
          if (loaderException is FileNotFoundException notFoundException && !string.IsNullOrEmpty(notFoundException.FusionLog))
          {
            stringBuilder.AppendLine("Fusion Log:");
            stringBuilder.AppendLine(notFoundException.FusionLog);
          }
          stringBuilder.AppendLine();
        }
        string message = stringBuilder.ToString();
        LogHelper.Error((Exception) ex, message);
      }
    }

    private static class CreateTables
    {
      public static bool CreateTableFromClass(Gbs.Core.Db.DbTable c)
      {
        try
        {
          DataBaseHelper.CreateTables.DbTable table = new DataBaseHelper.CreateTables.DbTable()
          {
            Name = c.GetType().Name
          };
          Performancer performancer = new Performancer("Создание таблицы " + table.Name + " в БД");
          PropertyInfo[] properties = c.GetType().GetProperties();
          performancer.AddPoint("Получение свойств класса");
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            performancer.AddPoint("Подклчение к БД");
            foreach (PropertyInfo propertyInfo in properties)
            {
              string name = propertyInfo.Name;
              string str1 = string.Empty;
              bool flag1 = propertyInfo.GetCustomAttributes(typeof (PrimaryKeyAttribute), false).Length != 0;
              bool flag2 = propertyInfo.GetCustomAttributes(typeof (IdentityAttribute), false).Length != 0;
              object[] customAttributes = propertyInfo.GetCustomAttributes(typeof (ColumnAttribute), false);
              object obj = ((IEnumerable<object>) customAttributes).Any<object>() ? ((IEnumerable<object>) customAttributes).Single<object>() : (object) null;
              string str2 = string.Empty;
              if (!flag1)
                str2 = ((DefaultValueAttribute) ((IEnumerable<object>) propertyInfo.GetCustomAttributes(typeof (DefaultValueAttribute), false)).FirstOrDefault<object>())?.Value.ToString();
              switch (propertyInfo.PropertyType.Name)
              {
                case "DateTime":
                  str1 = "TIMESTAMP";
                  if (str2.IsNullOrEmpty())
                  {
                    str2 = "2001-01-01";
                    break;
                  }
                  break;
                case "Boolean":
                  str1 = "BOOLEAN";
                  if (str2.IsNullOrEmpty())
                  {
                    str2 = "FALSE";
                    break;
                  }
                  break;
                case "String":
                  int num1 = flag1 ? 36 : ((ColumnAttribute) ((IEnumerable<object>) propertyInfo.GetCustomAttributes(typeof (ColumnAttribute), false)).First<object>()).Length;
                  if (num1 == 0)
                    num1 = 100;
                  str1 = string.Format("VARCHAR({0})", (object) num1);
                  break;
                case "Guid":
                  str1 = "VARCHAR(36)";
                  str2 = Guid.Empty.ToString();
                  break;
                case "Int32":
                  str1 = "INTEGER";
                  if (str2.IsNullOrEmpty())
                  {
                    str2 = "0";
                    break;
                  }
                  break;
                case "Decimal":
                  int num2 = 18;
                  int num3 = 10;
                  if (obj != null)
                  {
                    ColumnAttribute columnAttribute = (ColumnAttribute) obj;
                    if (columnAttribute.DataType == DataType.Money)
                    {
                      num2 = 18;
                      num3 = 4;
                    }
                    else
                    {
                      num2 = columnAttribute.Precision;
                      num3 = columnAttribute.Scale;
                    }
                  }
                  if (num2 == 0)
                    num2 = 18;
                  if (num3 == 0)
                    num3 = 10;
                  str1 = string.Format("NUMERIC({0},{1})", (object) num2, (object) num3);
                  if (str2.IsNullOrEmpty())
                  {
                    str2 = "0.0";
                    break;
                  }
                  break;
              }
              if (str1 == string.Empty && ((ColumnAttribute) obj).DataType == DataType.Blob)
              {
                str1 = "BLOB";
                str2 = string.Empty;
              }
              table.ColumnsList.Add(new DataBaseHelper.CreateTables.DbTable.Column()
              {
                IsPrimaryKey = flag1,
                Name = name,
                Type = str1,
                DefaultValue = str2,
                IsIdentity = flag2
              });
            }
            performancer.AddPoint("Цикл обработки массива свойств");
            DataBaseHelper.CreateTables.DbTable.Column column = table.ColumnsList.FirstOrDefault<DataBaseHelper.CreateTables.DbTable.Column>((System.Func<DataBaseHelper.CreateTables.DbTable.Column, bool>) (x => x.IsPrimaryKey));
            bool flag = DataBaseHelper.TableExist(table.Name);
            performancer.AddPoint("Проверка существования БД");
            if (!flag)
            {
              LogHelper.Debug("Таблицы " + table.Name + " нет в БД. Создаем...");
              string sql;
              if (column != null)
                sql = "CREATE TABLE " + table.Name + " (" + column.Name + " " + column.Type + " NOT NULL PRIMARY KEY)";
              else
                sql = "CREATE TABLE " + table.Name + " (K INTEGER)";
              Gbs.Core.Db.DataBase db = dataBase;
              flag = DataBaseHelper.ExecuteSqlRawCommand(sql, db);
              performancer.AddPoint("Создание таблицы");
            }
            bool columnList = DataBaseHelper.CreateTables.CreateColumnList(table);
            performancer.AddPoint("Создание столбцов");
            performancer.Stop();
            return flag & columnList;
          }
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка создания таблицы из класса");
          return false;
        }
      }

      private static bool CreateColumnList(DataBaseHelper.CreateTables.DbTable table)
      {
        foreach (DataBaseHelper.CreateTables.DbTable.Column column in table.ColumnsList.Where<DataBaseHelper.CreateTables.DbTable.Column>((System.Func<DataBaseHelper.CreateTables.DbTable.Column, bool>) (x => !x.IsPrimaryKey)))
        {
          if (!DataBaseHelper.CreateTables.CreateColumn(table.Name, column.Name, column.Type, column.DefaultValue, column.IsIdentity))
            return false;
        }
        return true;
      }

      [Localizable(false)]
      private static bool CreateColumn(
        string tableName,
        string columnName,
        string columnType,
        string columnDefaultValue,
        bool isIdentity)
      {
        try
        {
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            string str = isIdentity ? " GENERATED BY DEFAULT AS IDENTITY " : "DEFAULT ''" + columnDefaultValue + "''";
            return DataBaseHelper.ExecuteSqlRawCommand("EXECUTE block as\r\nBEGIN\r\nif (not exists(\r\nselect 1 from RDB$RELATION_FIELDS rf\r\nwhere rf.RDB$RELATION_NAME = '" + tableName + "' and rf.RDB$FIELD_NAME = '" + columnName + "'))\r\nthen\r\nexecute statement 'ALTER TABLE " + tableName + " ADD " + columnName + " " + columnType + " " + str + " NOT NULL';\r\nEND", dataBase);
          }
        }
        catch (Exception ex)
        {
          string message = "Ошибка при попытке создать столбец в БД. \r\ntableName: " + tableName + ", columnName: " + columnName + ", columnType: " + columnType + ", columnDefaultValue: " + columnDefaultValue;
          LogHelper.Error(ex, message);
          return false;
        }
      }

      private class DbTable
      {
        public string Name { get; set; }

        public List<DataBaseHelper.CreateTables.DbTable.Column> ColumnsList { get; } = new List<DataBaseHelper.CreateTables.DbTable.Column>();

        public class Column
        {
          public bool IsPrimaryKey { get; set; }

          public string Name { get; set; }

          public string Type { get; set; }

          public string DefaultValue { get; set; }

          public bool IsIdentity { get; set; }
        }
      }
    }
  }
}
