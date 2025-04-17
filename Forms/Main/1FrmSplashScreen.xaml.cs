// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.SplashScreenViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Emails;
using Gbs.Core.Entities.Settings;
using Gbs.Core.Models.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Forms.HomeOffice;
using Gbs.Forms.License;
using Gbs.Forms.Users;
using Gbs.Helpers;
using Gbs.Helpers.API.GbsApi.v1;
using Gbs.Helpers.API.Polycard;
using Gbs.Helpers.Cache;
using Gbs.Helpers.DB;
using Gbs.Helpers.FR;
using Gbs.Helpers.Licenses;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.WebOffice;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Gbs.Forms.Main
{
  public sealed class SplashScreenViewModel : ViewModelWithForm
  {
    private string _loadingStatus = Translate.SplashScreenViewModel_Загрузка;
    private int _loadingValue;

    public string Version => ApplicationInfo.GetInstance().AppVersion;

    public int LoadingValue
    {
      get => this._loadingValue;
      set
      {
        this._loadingValue = value;
        this.OnPropertyChanged(nameof (LoadingValue));
      }
    }

    public string LoadingStatus
    {
      get => this._loadingStatus;
      set
      {
        this._loadingStatus = value;
        this.OnPropertyChanged(nameof (LoadingStatus));
      }
    }

    public Visibility OriginNameVisible
    {
      get
      {
        return PartnersHelper.ProgramName() != "GBS.Market" ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility CustomNameVisibility
    {
      get
      {
        return this.OriginNameVisible == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public string ProgramName => PartnersHelper.ProgramName();

    public ImageSource Logo { get; set; }

    public async void Run(Action close)
    {
      SplashScreenViewModel splashScreenViewModel = this;
      LicenseHelper.GetInfo();
      splashScreenViewModel.CloseAction = close;
      try
      {
        // ISSUE: reference to a compiler-generated method
        await Task.Run(new Action(splashScreenViewModel.\u003CRun\u003Eb__20_0));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
    }

    private void SetProgressValue(int value, string text)
    {
      this.LoadingValue = value;
      this.LoadingStatus = text;
    }

    private void CheckUpdate()
    {
      try
      {
        if (DevelopersHelper.IsDebug())
          return;
        ConfigsRepository<Gbs.Core.Config.Settings> configsRepository = new ConfigsRepository<Gbs.Core.Config.Settings>();
        Gbs.Core.Config.Settings config = configsRepository.Get();
        OtherConfig.Update updateConfig = config.Other.UpdateConfig;
        if (updateConfig.UpdateType != OtherConfig.UpdateType.AutoUpdate)
        {
          updateConfig.TryUpdateCount = 0;
          configsRepository.Save(config);
        }
        else if (UpdateHelper.CheckReadyToUpdate() && updateConfig.TryUpdateCount < 2)
        {
          ++updateConfig.TryUpdateCount;
          configsRepository.Save(config);
          this.SetProgressValue(11, Translate.SplashScreenViewModel_Проверка_обновления);
          Thread.Sleep(1000);
          for (int index = 5; index > 0; --index)
          {
            this.SetProgressValue(12, string.Format(Translate.SplashScreenViewModel_Доступна_новая_версия__Программа_будет_перезагружена_________0__, (object) index));
            Thread.Sleep(1000);
          }
          if (Other.RestartApplication())
            return;
          this.SetProgressValue(14, Translate.SplashScreenViewModel_Не_удалось_перезапустить_программу_для_обновления);
        }
        else
        {
          updateConfig.TryUpdateCount = 0;
          configsRepository.Save(config);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка проверки обновления", false);
      }
    }

    private void CheckCorrectExt()
    {
      ConfigsRepository<Gbs.Core.Config.DataBase> configsRepository = new ConfigsRepository<Gbs.Core.Config.DataBase>();
      Gbs.Core.Config.DataBase config = configsRepository.Get();
      if (!config.CorrectExit)
      {
        LogHelper.Debug("Работа восстановлена после аварийного завершения");
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.SplashScreenViewModel_Работа_программы_восстановлена_после_аварийного_завершения, ProgressBarViewModel.Notification.NotificationsTypes.Warning));
      }
      config.CorrectExit = false;
      configsRepository.Save(config);
    }

    private void CheckDBConnection()
    {
      Gbs.Core.Config.DataBase dataBase = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
      if (dataBase.Connection.ServerUrl.ToLower().IsEither<string>("localhost", "127.0.0.1"))
        return;
      bool flag = true;
      try
      {
        DataBaseHelper.CheckDbConnection();
      }
      catch
      {
        flag = false;
      }
      if (flag)
        return;
      MessageBoxHelper.Error(string.Format(Translate.SplashScreenViewModel_CheckDBConnection_неУдалосьУстановитьСоединениесБД, (object) dataBase.Connection.ServerUrl));
      System.Environment.Exit(0);
    }

    [STAThread]
    private void ApplicationLoad()
    {
      try
      {
        LogHelper.Debug("Запуск программы. " + this.Version);
        string str1;
        try
        {
          str1 = HardwareInfo.StableInfo();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка получения данных об оборудовании");
          str1 = "Unknown hardware";
        }
        LogHelper.Debug("HW:" + str1);
        LogHelper.Debug("p427");
        HardwareInfo.GetRamValue();
        LogHelper.Debug(System.Environment.Is64BitOperatingSystem ? "Win x64" : "Win x86");
        this.CheckDBConnection();
        try
        {
          this.CheckCorrectExt();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
        new CpuPerformanceCounter().StartCpuCounting();
        LogHelper.Debug("p437");
        new OnStartWorker().DoWork();
        LogHelper.Debug("p441");
        Gbs.Core.Config.DataBase dbConfig = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
        GlobalDictionaries.Mode modeProgram = dbConfig.ModeProgram;
        LogHelper.Debug("p447");
        this.PrepareDataBase();
        LogHelper.Debug("p451");
        SplashScreenViewModel.WriteHwInfoToLog(dbConfig);
        LogHelper.Debug("p455");
        DataBaseCorrector.CorrectionMethodsCount();
        try
        {
          SplashScreenViewModel.SendStatistic();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
        this.SetProgressValue(10, Translate.SplashScreenViewModel_ApplicationLoad_Удаление_временных_папок);
        if (DevelopersHelper.IsDebug())
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.SplashScreenViewModel_ApplicationLoad_Программа_запущена_в_режиме_отладки));
        try
        {
          SplashScreenViewModel.DeleteTempFolders();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Не удалось удалить временны папки");
        }
        SplashScreenViewModel.CheckLicense();
        this.CheckUpdate();
        this.SetSubscribes();
        this.SetProgressValue(15, Translate.SplashScreenViewModel_Подключение_к_БД___);
        if (modeProgram != GlobalDictionaries.Mode.Home)
        {
          DataBaseHelper.CheckDbConnection();
          LogHelper.Debug(dbConfig.BackUp.ToJsonString(true));
          if (dbConfig.BackUp.CreateBackup && dbConfig.BackUp.IsCreateOnStart)
          {
            this.SetProgressValue(20, Translate.SplashScreenViewModel_Создание_резервной_копии___);
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SplashScreenViewModel_Создание_резервной_копии___);
            BackupHelper.CreateBackup();
            progressBar.Close();
          }
          Gbs.Core.Config.WebOffice webOffice = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().RemoteControl.WebOffice;
          if (webOffice.IsActive)
          {
            if (webOffice.IsCreateOnStart)
            {
              this.SetProgressValue(25, Translate.SplashScreenViewModel_ApplicationLoad_Выгрузка_архива_с_данными_для_веб_офиса);
              WebOfficeHelper.CreateArchive();
            }
            this.SetProgressValue(27, Translate.SplashScreenViewModel_ApplicationLoad_Обновление_статуса_торговой_точки_в_веб_офисе);
            WebOfficeHelper.SetStatusForPoint();
          }
          this.SetProgressValue(40, Translate.SplashScreenViewModel_Создание_таблиц_в_БД___);
          if (!SalePoints.GetSalePointList().Any<SalePoints.SalePoint>())
            Application.Current?.Dispatcher?.Invoke<bool?>((Func<bool?>) (() => new FrmFirstMain().ShowDialog()));
          if (dbConfig.IsCompressDbStart)
          {
            this.SetProgressValue(45, Translate.SplashScreenViewModel_ApplicationLoad_Сжатие_базы_данных);
            DataBaseHelper.CompressDb();
          }
        }
        TaskHelper.TaskRun(new Action(FastReportFacade.DownloadTemplatesFromServer), false);
        this.LoadingStatus = Translate.SplashScreenViewModel_ApplicationLoad_Корректируем_настройки;
        new ConfigsCorrector().Do();
        Gbs.Core.Config.Settings c = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
        LogHelper.Trace(string.Format("Запуск с Windows: {0}", (object) c.BasicConfig.AutoRunProgram));
        this.SetProgressValue(60, Translate.SplashScreenViewModel_Проверка_данных___);
        if (modeProgram != GlobalDictionaries.Mode.Home)
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            try
            {
              new EmailRepository().Delete(new EmailRepository().GetAllItems().Where<Gbs.Core.Entities.Emails.Email>((Func<Gbs.Core.Entities.Emails.Email, bool>) (x => (DateTime.Now - x.Date).TotalDays > 30.0)).ToList<Gbs.Core.Entities.Emails.Email>());
            }
            catch (Exception ex)
            {
              LogHelper.WriteError(ex);
            }
            int countDay = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Users.CountDayActionHistory;
            new ActionHistoryRepository(dataBase).Delete(new ActionHistoryRepository(dataBase).GetAllItems().Where<ActionHistory>((Func<ActionHistory, bool>) (x => (DateTime.Now - x.DateTime).TotalDays > (double) countDay)).ToList<ActionHistory>());
            new UsersRepository(dataBase).DisconnectUsers();
            try
            {
              NikitaConfig.Do();
            }
            catch (Exception ex)
            {
            }
            DataBaseHelper.CreatePropertiesForCountry();
          }
        }
        try
        {
          string path = c.RemoteControl.Cloud.Path;
          if (c.RemoteControl.Cloud.IsActive)
          {
            if (!path.IsNullOrEmpty())
            {
              if (path != ApplicationInfo.GetInstance().Paths.DataPath)
              {
                if (Directory.Exists(path))
                {
                  string[] files = Directory.GetFiles(path);
                  List<string> values = new List<string>()
                  {
                    "Содержимое папки обмена:",
                    "Название | Дата и время создания | Дата и время редактирования"
                  };
                  foreach (string fileName in files)
                  {
                    FileInfo fileInfo = new FileInfo(fileName);
                    List<string> stringList = values;
                    string[] strArray = new string[5]
                    {
                      fileInfo.Name,
                      " | ",
                      null,
                      null,
                      null
                    };
                    DateTime dateTime = fileInfo.CreationTime;
                    strArray[2] = dateTime.ToString();
                    strArray[3] = " | ";
                    dateTime = fileInfo.LastWriteTime;
                    strArray[4] = dateTime.ToString();
                    string str2 = string.Concat(strArray);
                    stringList.Add(str2);
                  }
                  LogHelper.Debug(string.Join("\n", (IEnumerable<string>) values));
                }
              }
            }
          }
        }
        catch
        {
        }
        try
        {
          SplashScreenViewModel.WriteRemoteControlPathContent(c);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка при получении содержимого папки обмена", false);
        }
        this.SetProgressValue(90, Translate.SplashScreenViewModel_Запуск_приложения___);
        PalycardApi.StartListeners();
        Server.StartServer();
        this.PreLoadCache();
        this.LoadingValue = 100;
        if (modeProgram != GlobalDictionaries.Mode.Home)
        {
          this.GetSqlFile();
          try
          {
            NikitaConfig.Do();
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Ошибка м методах для Никиты");
          }
        }
        SplashScreenViewModel.SendLicenseNotifications(c);
        this.ConnectDevices();
        this.Login(dbConfig);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки приложения");
        System.Environment.Exit(0);
      }
    }

    private static void SendLicenseNotifications(Gbs.Core.Config.Settings c)
    {
      LicenseHelper.LicenseInformation info = LicenseHelper.GetInfo();
      DateTime dateTime = DateTime.Now;
      DateTime date1 = dateTime.Date;
      dateTime = info.KeyDateEnd;
      DateTime date2 = dateTime.Date;
      double num = Math.Abs((date1 - date2).TotalDays);
      if (!c.RemoteControl.Telegram.IsSendNotificationLicense || num >= 5.0)
        return;
      string text = string.Format(Translate.SplashScreenViewModel_Срок_лицензии__GBS_ID_6__0___подходит_к_концу__осталось__1__дней_до_окончания_действия, (object) info.GbsId, (object) num);
      if (c.Interface.Country == GlobalDictionaries.Countries.Russia)
        text = text + "\\n\\nВы можете оплатить лицензию по следующей ссылке - https://gbsmarket.ru/cost/?gbsId=6:" + info.GbsId;
      TelegramHelper.SendText(c.RemoteControl.Telegram.UsernameTo, text);
    }

    private static void SendStatistic()
    {
      Gbs.Core.Config.Settings config = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
      if (NetworkHelper.IsWorkInternet() && (!config.System.Info_001.Updated.HasValue || (DateTime.Now - config.System.Info_001.Updated.Value).Days > 30))
      {
        string info = new RegionInfoHelper().GetInfo();
        config.System.Info_001.Data = new CryptoConfig(info);
        config.System.Info_001.Updated = new DateTime?(DateTime.Now);
        new ConfigsRepository<Gbs.Core.Config.Settings>().Save(config);
      }
      TaskHelper.TaskRun((Action) (() => ServerScriptsHelper.SendPing(config.System.Info_001.Data.DecryptedValue)), false);
    }

    [Localizable(false)]
    private static void WriteRemoteControlPathContent(Gbs.Core.Config.Settings c)
    {
      string path = c.RemoteControl.Cloud.Path;
      if (!c.RemoteControl.Cloud.IsActive || path.IsNullOrEmpty() || !(path != ApplicationInfo.GetInstance().Paths.DataPath) || !Directory.Exists(path))
        return;
      List<string> values = new List<string>()
      {
        "Содержимое папки обмена:",
        "Название | Дата и время создания | Дата и время редактирования"
      };
      DateTime dateTime;
      foreach (string file in Directory.GetFiles(path))
      {
        FileInfo fileInfo = new FileInfo(file);
        List<string> stringList = values;
        string[] strArray = new string[5]
        {
          fileInfo.Name,
          " | ",
          null,
          null,
          null
        };
        dateTime = fileInfo.CreationTime;
        strArray[2] = dateTime.ToString();
        strArray[3] = " | ";
        dateTime = fileInfo.LastWriteTime;
        strArray[4] = dateTime.ToString();
        string str = string.Concat(strArray);
        stringList.Add(str);
      }
      foreach (string directory in Directory.GetDirectories(path))
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        List<string> stringList = values;
        string[] strArray = new string[5]
        {
          directoryInfo.Name,
          " | ",
          null,
          null,
          null
        };
        dateTime = directoryInfo.CreationTime;
        strArray[2] = dateTime.ToString();
        strArray[3] = " | ";
        dateTime = directoryInfo.LastWriteTime;
        strArray[4] = dateTime.ToString();
        string str = string.Concat(strArray);
        stringList.Add(str);
      }
      LogHelper.Debug(string.Join("\n", (IEnumerable<string>) values));
    }

    private static void DeleteTempFolders()
    {
      ((IEnumerable<string>) ((IEnumerable<string>) Directory.GetDirectories(Path.GetTempPath(), "GBSMarket6_*")).Concat<string>((IEnumerable<string>) Directory.GetDirectories(Path.GetTempPath(), "b16d85d1-a305-4724-a3cc-ccb7ca3a796a_*")).ToArray<string>()).ToList<string>().ForEach((Action<string>) (x => Directory.Delete(x, true)));
    }

    private void GetAppFolderFiles()
    {
    }

    private void PreLoadCache()
    {
      Task.Run((Action) (() =>
      {
        try
        {
          switch (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram)
          {
            case GlobalDictionaries.Mode.Shop:
              CachesBox.AllGoods();
              CachesBox.AllBuyPrices();
              break;
            case GlobalDictionaries.Mode.Home:
              return;
          }
          Gbs.Core.Entities.Documents.Cache.GetFromCache();
          updateCreditsCache();
          updateDiscountRules();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }));

      static void updateDiscountRules() => BasketDiscounter.ReloadRules();

      static void updateCreditsCache()
      {
        CacheHelper.Get<List<CreditListViewModel.CreditItem>>(CacheHelper.CacheTypes.ClientsCredits, new Func<List<CreditListViewModel.CreditItem>>(CreditListViewModel.UpdateCreditsCache));
      }
    }

    private void SetSubscribes()
    {
      SplashScreenViewModel.SubscribeForCache();
      SplashScreenViewModel.SubscribeForPolyCloud();
    }

    private static void SubscribeForCache()
    {
      Gbs.Core.Config.DataBase c = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
      DocumentsRepository.DocumentSaved += (DocumentsRepository.SaveHandler) (_ => TaskHelper.TaskRun((Action) (() =>
      {
        if (c.ModeProgram == GlobalDictionaries.Mode.Cafe)
          CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.CafeMenu);
        else
          CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllGoods);
      })));
    }

    private static void SubscribeForPolyCloud()
    {
      if (!new ConfigsRepository<Integrations>().Get().PolyCloud.IsActive)
        return;
      DocumentsRepository.DocumentSaved += (DocumentsRepository.SaveHandler) (doc => Task.Run((Action) (() =>
      {
        try
        {
          if (!doc.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.SaleReturn))
            return;
          new Gbs.Helpers.ExternalApi.PolycardCloud.PolyCloud().SendClient(doc.ContractorUid);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      })));
    }

    private static void CheckLicense()
    {
      try
      {
        LicenseHelper.DownloadFromServer();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
      if (!(DateTime.Today > LicenseHelper.GetInfo().KeyDateEnd.AddDays(-7.0)))
        return;
      Application.Current?.Dispatcher?.Invoke((Action) (() => new LicenseNotificationViewModel().Show()));
    }

    [Localizable(false)]
    private static void WriteHwInfoToLog(Gbs.Core.Config.DataBase dbConfig)
    {
      LogHelper.Debug("GBS.ID: " + GbsIdHelperMain.GetGbsId() + "\r\nDate end: " + LicenseHelper.GetInfo().KeyDateEnd.ToString("dd.MM.yyyy"));
      ApplicationInfo instance = ApplicationInfo.GetInstance();
      LogHelper.Debug("p700");
      string str;
      try
      {
        str = new HardWareInfoHelper().GetInfo().ToJsonString(true);
      }
      catch (Exception ex)
      {
        str = "Unknown hardware";
        LogHelper.WriteError(ex, "Hardware error");
      }
      LogHelper.Debug("p710");
      string message = "HW info: " + str + "\r\n\r\nDB host: " + dbConfig.Connection.ServerUrl + "; db path: " + dbConfig.Connection.Path + "\r\napp path: " + instance.Paths.ApplicationPath + "; data path: " + instance.Paths.DataPath + "\r\n" + (dbConfig.ModeProgram != GlobalDictionaries.Mode.Home ? "db version: " + VersionDb.GetVersion().ToString() + "\r\n" : "") + "work mode: " + dbConfig.ModeProgram.ToString();
      LogHelper.Debug("p722");
      DirectoryInfo di = new DirectoryInfo(instance.Paths.ApplicationPath);
      try
      {
        SplashScreenViewModel.WriteAppPathFiles(di);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
      LogHelper.Debug("p735");
      LogHelper.Debug(message);
      LogHelper.Debug("----------------------------\r\n\r\n");
    }

    private static void WriteAppPathFiles(DirectoryInfo di)
    {
      string empty = string.Empty;
      foreach (FileInfo file in di.GetFiles("*.*", SearchOption.AllDirectories))
      {
        string str = string.Empty;
        if (file.Extension.IsEither<string>(".exe", ".dll"))
          str = " ver: " + FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;
        empty += (file.FullName + str + "\r\n").Replace(di.FullName, "");
      }
      LogHelper.Debug("Application files\r\n" + empty);
    }

    private void PrepareDataBase()
    {
      this.LoadingValue = 7;
      this.LoadingStatus = Translate.SplashScreenViewModel_Подготовка_базы_данных;
      DataBaseHelper.PrepareDb();
      if (!FrmSplashScreen.IsUnpackDataZip)
        return;
      Setting uid = UidDb.GetUid();
      uid.EntityUid = Guid.NewGuid();
      UidDb.SetUid(uid);
    }

    private void Login(Gbs.Core.Config.DataBase dbConfig)
    {
      Application.Current?.Dispatcher?.Invoke((Action) (() =>
      {
        bool flag = true;
        Gbs.Core.Entities.Users.User user = new Gbs.Core.Entities.Users.User();
        do
        {
          if (dbConfig.ModeProgram == GlobalDictionaries.Mode.Home)
          {
            flag = new PointSelectViewModel().ShowLogin(true);
            if (!HomeOfficeHelper.IsAuthRequire & flag)
              break;
          }
          flag &= new FrmLoginUser().ShowLogin(out user, true);
        }
        while (!flag && MessageBoxHelper.Show(Translate.SplashScreenViewModel_Вы_уверены__что_хотите_закрыть_программу_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No);
        if (!flag)
          throw new InvalidOperationException(Translate.SplashScreenViewModel_Загрузка_программы_не_удалась);
        ReCalcCashAccountHelper.DoReCalcCashAccount(false, user);
        new MainWindow(this.CloseAction).Show();
      }));
    }

    private void ConnectDevices() => ComPortScanner.Start();

    private void GetSqlFile()
    {
      try
      {
        string pathSql = ApplicationInfo.GetInstance().Paths.DataPath + "gbs.sql";
        if (!File.Exists(pathSql) || MessageBoxHelper.Show(Translate.SplashScreenViewModel_Выполнить_sql_скрипт_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
          return;
        Application.Current?.Dispatcher?.Invoke((Action) (() =>
        {
          if (!new Authorization().GetAccess(Actions.ExecuteScript).Result)
            return;
          DataBaseHelper.DoSql(pathSql);
        }));
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.Error(ex, "Ошибка проверки наличия sql файла");
      }
    }
  }
}
