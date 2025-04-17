// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.HomeOffice.PointSelectViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.DB;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Ionic.Zip;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.HomeOffice
{
  public partial class PointSelectViewModel : ViewModelWithForm
  {
    private HomeOfficeHelper.PointInfo _point;
    private readonly Settings _settings = new ConfigsRepository<Settings>().Get();

    public bool ShowLogin(bool isExitApp = false)
    {
      if (!Directory.Exists(this._settings.RemoteControl.Cloud.Path))
      {
        MessageBoxHelper.Warning(Translate.PointSelectViewModel_Папка_обмена__указанная_в_настройках_удаленного_контроля_не_существует__Повторите_выбор_папки_для_синхронизации_);
        this.SelectPath();
      }
      this.IsExitApp = isExitApp;
      this.UpdatePoints();
      this.FormToSHow = (WindowWithSize) new FrmSelectPoint();
      if (this.PointList.Any<HomeOfficeHelper.PointInfo>())
        this.ShowForm();
      else
        this.CloseAction();
      return this.IsSuccessSelectedPoint;
    }

    public List<HomeOfficeHelper.PointInfo> PointList { get; set; }

    public HomeOfficeHelper.PointInfo SelectedPoint
    {
      get => this._point;
      set
      {
        this._point = value;
        this.OnPropertyChanged(nameof (SelectedPoint));
      }
    }

    public bool IsExitApp { get; set; }

    public bool IsSuccessSelectedPoint { get; set; }

    public ICommand DoneCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedPoint == null)
          {
            MessageBoxHelper.Warning(Translate.PointSelectViewModel_Требуется_выбрать_точку_для_входа_);
          }
          else
          {
            Performancer performancer = new Performancer("Загрука данных из торговой точки дом офис");
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.PointSelectViewModel_Загрузка_данных_о_точке);
            string str1 = Path.Combine(ApplicationInfo.GetInstance().Paths.CachePath, this.SelectedPoint.DbUid.ToString());
            DirectoryInfo directory = Directory.CreateDirectory(str1);
            performancer.AddPoint("Проверка папки и создание пути");
            string str2 = Path.Combine(FileSystemHelper.TempFolderPath(), this.SelectedPoint.FileArchive.Name);
            FileSystemHelper.CopyFile(this.SelectedPoint.FileArchive.FullName, str2);
            ZipFile zipWithDb = FileSystemHelper.OpenZip(str2);
            performancer.AddPoint("Открытие архива");
            FileInfo oldMain = ((IEnumerable<FileInfo>) directory.GetFiles()).FirstOrDefault<FileInfo>((Func<FileInfo, bool>) (x => x.Extension == ".fdb"));
            performancer.AddPoint("Полкучение базы данных из кэша");
            if (zipWithDb == null)
            {
              if (this.UsedDataBaseFromCache(oldMain))
              {
                this.IsSuccessSelectedPoint = true;
                this.PrepareHomeOffice(oldMain.FullName);
                this.CloseAction();
              }
              progressBar.Close();
            }
            else
            {
              string str3 = PointSelectViewModel.UpdateDataBase(oldMain, zipWithDb, this.SelectedPoint, str1);
              performancer.Stop();
              if (str3 == null)
                str3 = oldMain?.FullName;
              if (str3.IsNullOrEmpty())
              {
                progressBar.Close();
                MessageBoxHelper.Error(Translate.PointSelectViewModel_Не_удалось_получить_данные_о_торговой_точке);
              }
              else
              {
                this.IsSuccessSelectedPoint = true;
                this.PrepareHomeOffice(str3);
                progressBar.Close();
                this.CloseAction();
              }
            }
          }
        }));
      }
    }

    public static string UpdateDataBase(
      FileInfo oldMain,
      ZipFile zipWithDb,
      HomeOfficeHelper.PointInfo point,
      string cacheFolder)
    {
      DateTime timeEdit = PointSelectViewModel.GetTimeEdit(oldMain?.DirectoryName);
      DateTime lastWriteTime = point.FileArchive.LastWriteTime;
      LogHelper.Debug(string.Format("TimeEdit: {0}; lastWriteTime: {1}; cachePath: {2}", (object) timeEdit, (object) lastWriteTime, (object) cacheFolder));
      if (FileSystemHelper.IsFileLocked(zipWithDb.Name))
      {
        LogHelper.Debug("Файл архива занят другим процессом");
        PointSelectViewModel.ShowNotificationCanUpdateData();
        return (string) null;
      }
      if (timeEdit < lastWriteTime)
      {
        int num1 = zipWithDb.Entries.Count<ZipEntry>((Func<ZipEntry, bool>) (x => x.FileName.Contains(".fbk") || x.FileName.Contains(".fdb")));
        if (num1 == 0)
        {
          LogHelper.Debug("Нет файла БД в архиве (или вообще нет файлов)");
          return (string) null;
        }
        if (num1 > 1)
        {
          LogHelper.Debug("Несколько файлов БД в архиве");
          return (string) null;
        }
        string path2_1 = zipWithDb.Entries.SingleOrDefault<ZipEntry>((Func<ZipEntry, bool>) (x => x.FileName.EndsWith(".fdb", StringComparison.OrdinalIgnoreCase)))?.FileName ?? "main.fdb";
        string path2_2 = zipWithDb.Entries.SingleOrDefault<ZipEntry>((Func<ZipEntry, bool>) (x => x.FileName.EndsWith(".fbk", StringComparison.OrdinalIgnoreCase)))?.FileName ?? "";
        string path2_3 = DateTime.Now.ToString("yyyyMMddHHmmss") + ".fdb";
        DataBaseHelper.CloseConnection();
        Thread.Sleep(500);
        foreach (string file in Directory.GetFiles(cacheFolder))
        {
          if (File.Exists(file) && !FileSystemHelper.IsFileLocked(file))
            File.Delete(file);
        }
        LogHelper.Debug("Обновление БД из архива д/о в обалке");
        string str1 = FileSystemHelper.TempFolderPath();
        FileSystemHelper.ExtractToFile(zipWithDb, point.InfoDataBase.IsSendBackupDb ? path2_2 : path2_1, str1);
        zipWithDb.Dispose();
        string str2 = Path.Combine(str1, path2_1);
        if (point.InfoDataBase.IsSendBackupDb)
          BackupHelper.RestoreDataBase(Path.Combine(str1, path2_2), str2);
        int num2 = FileSystemHelper.MoveFile(str2, Path.Combine(cacheFolder, path2_3)) ? 1 : 0;
        Directory.Delete(str1, true);
        return num2 == 0 ? (string) null : Path.Combine(cacheFolder, path2_3);
      }
      LogHelper.Debug("База данных в кэше новее, чем архив, пропускаем перенос.");
      return (string) null;
    }

    private bool UsedDataBaseFromCache(FileInfo oldMain)
    {
      LogHelper.Debug("Не удалось открыть архив, берем базу из кэша.");
      if (oldMain == null)
      {
        MessageBoxHelper.Error(Translate.PointSelectViewModel_Не_удалось_получить_данные_о_торговой_точке);
        return false;
      }
      DataBaseHelper.PathToDb = oldMain.FullName;
      HomeOfficeHelper.IsAcceptHome = this.SelectedPoint.InfoDataBase.IsAcceptHome;
      bool flag = HomeOfficeHelper.IsAcceptHome;
      LogHelper.Debug("Можно вносить изменения из дом/офис " + flag.ToString());
      HomeOfficeHelper.IsAuthRequire = AuthRequireDb.Get();
      CacheHelper.ClearAll();
      Cache.ClearAndLoadCache();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        new UsersRepository(dataBase).DisconnectUsers();
        flag = true;
        return flag;
      }
    }

    private void PrepareHomeOffice(string pathDb)
    {
      DataBaseHelper.PathToDb = pathDb;
      DataBaseHelper.PrepareDb();
      HomeOfficeHelper.IsAcceptHome = this.SelectedPoint.InfoDataBase.IsAcceptHome;
      LogHelper.Debug("Можно вносить изменения из дом/офис " + HomeOfficeHelper.IsAcceptHome.ToString());
      HomeOfficeHelper.IsAuthRequire = AuthRequireDb.Get();
      CacheHelper.ClearAll();
      Cache.ClearAndLoadCache();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        new UsersRepository(dataBase).DisconnectUsers();
        int num = DataBaseCorrector.CorrectionMethodsCount();
        if (num != 0)
        {
          int version = VersionDb.GetVersion();
          LogHelper.Debug(string.Format("Corrections: {0}; db version: {1}", (object) num, (object) version));
          if (num != version)
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Text = Translate.PointSelectViewModel_Версия_базы_данных_торговой_точки_не_соответствует_версии_программы__Убедитесь__что_версии_программы_в_торговой_точке_и_в_режиме__Дом_офис__совпадают,
              Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
            });
        }
        HomeOfficeHelper.CloudArchiveLastWriteDateTime = this.SelectedPoint.FileArchive.LastWriteTime;
      }
    }

    private static void ShowNotificationCanUpdateData()
    {
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
      {
        Text = Translate.PointSelectViewModel_Не_удалось_обновить_данные_для_выбранной_торговой_точки__Будут_отображены_данные__актуальные_на_момент_последней_загрузки_,
        Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
      });
    }

    public ICommand DeletePoint
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<HomeOfficeHelper.PointInfo> list = ((IEnumerable) obj).Cast<HomeOfficeHelper.PointInfo>().ToList<HomeOfficeHelper.PointInfo>();
          if (!list.Any<HomeOfficeHelper.PointInfo>())
            MessageBoxHelper.Warning(Translate.PointSelectViewModel_Необходимо_выбрать_торговую_точку_для_удаления_из_папки_синхронизации);
          else if (list.Count<HomeOfficeHelper.PointInfo>() > 1)
          {
            MessageBoxHelper.Warning(Translate.PointSelectViewModel_Необходимо_выбрать_только_одну_торговую_точку_для_удаления);
          }
          else
          {
            HomeOfficeHelper.PointInfo pointInfo = list.Single<HomeOfficeHelper.PointInfo>();
            if (pointInfo.FileArchive == null)
            {
              MessageBoxHelper.Error(Translate.PointSelectViewModel_DeletePoint_Не_удалось_удалить_торговую_точку_из_облачного_сервиса__Попробуйте_удалить_данные_о_торговой_точке_вручную_);
            }
            else
            {
              if (MessageBoxHelper.Question(string.Format(Translate.PointSelectViewModel_, (object) pointInfo.InfoDataBase.NameDataBase)) == MessageBoxResult.No)
                return;
              File.Delete(Path.Combine(pointInfo.FileArchive.DirectoryName, pointInfo.DbUid.ToString() + ".info"));
              pointInfo.FileArchive.Delete();
              this.UpdatePoints();
            }
          }
        }));
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    private void UpdatePoints()
    {
      this.PointList = new List<HomeOfficeHelper.PointInfo>((IEnumerable<HomeOfficeHelper.PointInfo>) HomeOfficeHelper.GetPointFromCloud());
      if (this.PointList.Any<HomeOfficeHelper.PointInfo>())
        return;
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
      {
        Text = Translate.PointSelectViewModel_Возможно__что_данный_режим_работы_программы_был_выбран_ошибочно_,
        ActionText = Translate.PointSelectViewModel_Изменить_режим_на_Магазин_склад,
        ActionCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.SetModeProgram()))
      });
      while (!this.PointList.Any<HomeOfficeHelper.PointInfo>())
      {
        if (MessageBoxHelper.Question(string.Format(Translate.PointSelectViewModel_В_указанной_папке_обмена_не_найдено_не_одной_базы_данных__0_Для_просмотра_информации_из_торговой_точки__необходимо_настроить_синхронизацию_между_ними__Укажите_папку_обмена_с_данными_для_дальнейшей_работы_, (object) Other.NewLine(2))) == MessageBoxResult.Yes)
        {
          this.SelectPath();
          this.PointList = new List<HomeOfficeHelper.PointInfo>((IEnumerable<HomeOfficeHelper.PointInfo>) HomeOfficeHelper.GetPointFromCloud());
          this.OnPropertyChanged("PointList");
        }
        else
        {
          Other.SetCorrectExit();
          System.Environment.Exit(0);
        }
      }
    }

    private void SetModeProgram()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.PointSelectViewModel_Изменение_режима_работы_программы);
      ConfigsRepository<Gbs.Core.Config.DataBase> configsRepository = new ConfigsRepository<Gbs.Core.Config.DataBase>();
      Gbs.Core.Config.DataBase config = configsRepository.Get();
      config.ModeProgram = GlobalDictionaries.Mode.Shop;
      configsRepository.Save(config);
      if (Other.RestartApplication())
        return;
      progressBar.Close();
      Other.SetCorrectExit();
      System.Environment.Exit(0);
    }

    private void SelectPath()
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
      {
        Description = Translate.PointSelectViewModel_Выберите_папку_обмена
      };
      this._settings.RemoteControl.Cloud.Path = folderBrowserDialog.ShowDialog() == DialogResult.OK ? folderBrowserDialog.SelectedPath : ApplicationInfo.GetInstance().Paths.DataPath;
      new ConfigsRepository<Settings>().Save(this._settings);
    }

    public static DateTime GetTimeEdit(string folderCash)
    {
      if (folderCash.IsNullOrEmpty())
        return DateTime.MinValue;
      string path = Path.Combine(folderCash, "time.config");
      return !File.Exists(path) ? DateTime.MinValue : JsonConvert.DeserializeObject<DateTime>(File.ReadAllText(path));
    }
  }
}
