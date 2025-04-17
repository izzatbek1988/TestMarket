// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.TimerHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Emails;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.HomeOffice;
using Gbs.Helpers.BackgroundTasks;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.Licenses;
using Gbs.Helpers.Logging;
using Gbs.Helpers.PlanfixApi.GbsRepository;
using Gbs.Helpers.WebOffice;
using Gbs.Resources.Localizations;
using Ionic.Zip;
using Newtonsoft.Json;
using Planfix.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class TimerHelper
  {
    private static Action _updateTime;
    private static Timer _timeTimer = new Timer();
    public static string CurrentTime;
    public static string CurrentDate;
    private static Timer _backUpTimer = new Timer();
    private static Timer _updateCheckTimer = new Timer();
    private static Timer _emailOldTimer = new Timer();
    private static Timer _emailTimer = new Timer();
    private static int _countErrorSenMail;
    private static Timer _telegramOldTimer = new Timer();
    private static Timer _telegramTimer = new Timer();
    private static int _countErrorSendTelegram;
    private static Timer _planfixTimer = new Timer();
    private static Timer _exchangeCatalogTimer = new Timer();
    private static Timer _homeOfficeTimer = new Timer();
    private static Timer _homeCheckJsonTimer = new Timer();
    private static Timer _homeUpdateDataBaseTimer = new Timer();
    private static Timer _clientUploadTimer = new Timer();
    private static Timer _catalogUploadTimer = new Timer();
    private static Timer _updateUserInputTimer;
    private static Timer _moveCheckTimer = new Timer();
    private static Timer _ramTimer = new Timer();
    private static Timer _infoLogTimer = new Timer();
    private static Timer _licenseTimer = new Timer();
    private static Timer _checkRegistryTimer = new Timer();
    private static Timer _statusWoTimer = new Timer();
    private static Timer _woTimer = new Timer();
    private static Timer _statusGrafanaTimer = new Timer();

    private static Settings Settings { get; set; } = new ConfigsRepository<Settings>().Get();

    private static void InitializeTime(Action updateTime)
    {
      TimerHelper._updateTime = updateTime;
      TimerHelper.StartTimeTimer();
    }

    private static void StartTimeTimer()
    {
      TimerHelper._timeTimer.Dispose();
      TimerHelper._timeTimer = new Timer()
      {
        Interval = 1000.0
      };
      TimerHelper._timeTimer.Elapsed += new ElapsedEventHandler(TimerHelper.TimeTimerElapsed);
      TimerHelper._timeTimer.Start();
    }

    private static void TimeTimerElapsed(object sender, ElapsedEventArgs e)
    {
      try
      {
        TimerHelper.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
        TimerHelper.CurrentDate = DateTime.Now.ToString("dd.MM.yyyy, ddd");
        TimerHelper._updateTime();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в таймере часов");
        TimerHelper.StartTimeTimer();
      }
    }

    private static void InitializeBackUpTimer()
    {
      TimerHelper._backUpTimer.Stop();
      Gbs.Core.Config.DataBase.DbBackUp backUp = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().BackUp;
      if (!backUp.CreateBackup)
        return;
      int hours;
      switch (backUp.CreatePeriod)
      {
        case Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery1Hour:
          hours = 1;
          break;
        case Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery3Hours:
          hours = 3;
          break;
        case Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery6Hours:
          hours = 6;
          break;
        case Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery12Hours:
          hours = 12;
          break;
        default:
          return;
      }
      TimerHelper._backUpTimer.Dispose();
      TimerHelper._backUpTimer = new Timer()
      {
        Interval = new TimeSpan(hours, 0, 0).TotalMilliseconds
      };
      TimerHelper._backUpTimer.Elapsed += new ElapsedEventHandler(TimerHelper._backUpTimer_Elapsed);
      TimerHelper._backUpTimer.Start();
    }

    private static void _backUpTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер резервного копирования");
      BackupHelper.CreateBackup();
    }

    private static void InitializeUpdateTimer()
    {
      if (TimerHelper.Settings.Other.UpdateConfig.UpdateType == OtherConfig.UpdateType.NoUpdate)
        return;
      TimerHelper._updateCheckTimer.Dispose();
      TimerHelper._updateCheckTimer = new Timer()
      {
        Interval = 1800000.0
      };
      TimerHelper._updateCheckTimer.Elapsed += new ElapsedEventHandler(TimerHelper.UpdateCheckTimerElapsed);
      TimerHelper._updateCheckTimer.Start();
    }

    private static void UpdateCheckTimerElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер проверки обновлений");
      BackgroundTasksHelper.AddTaskToQueue(new Action(UpdateHelper.CheckUpdate), BackgroundTaskTypes.DataBaseSyncDataPreparing);
    }

    private static void InitializeEmailOldTimer()
    {
      TimerHelper._emailOldTimer.Dispose();
      TimerHelper._emailOldTimer = new Timer()
      {
        Interval = 300000.0
      };
      TimerHelper._emailOldTimer.Elapsed += new ElapsedEventHandler(TimerHelper._emailOldTimer_Elapsed);
      TimerHelper._emailOldTimer.Start();
    }

    private static void InitializeEmailTimer()
    {
      TimerHelper._emailTimer.Stop();
      if (!TimerHelper.Settings.RemoteControl.Email.IsActive)
        return;
      TimerHelper._emailTimer.Dispose();
      TimerHelper._emailTimer = new Timer()
      {
        Interval = 60000.0
      };
      TimerHelper._emailTimer.Elapsed += new ElapsedEventHandler(TimerHelper.EmailSend);
      TimerHelper._countErrorSenMail = 0;
      TimerHelper._emailTimer.Start();
    }

    private static void _emailOldTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер отправки старых писем");
      if (TimerHelper._countErrorSenMail >= 10)
        TimerHelper._emailOldTimer.Stop();
      else
        Task.Run((Action) (() =>
        {
          try
          {
            List<Gbs.Core.Entities.Emails.Email> list = new EmailRepository().GetActiveItems().Where<Gbs.Core.Entities.Emails.Email>((Func<Gbs.Core.Entities.Emails.Email, bool>) (x => !x.IsSend && (DateTime.Now - x.Date).TotalDays <= 3.0)).ToList<Gbs.Core.Entities.Emails.Email>();
            if (!list.Any<Gbs.Core.Entities.Emails.Email>())
              return;
            Gbs.Core.Entities.Emails.Email mail = list.OrderBy<Gbs.Core.Entities.Emails.Email, DateTime>((Func<Gbs.Core.Entities.Emails.Email, DateTime>) (x => x.Date)).First<Gbs.Core.Entities.Emails.Email>();
            LogHelper.Debug("Отправка отчета на почту (старые отчеты) - " + mail.AddressTo);
            TimerHelper._countErrorSenMail = !new EmailRepository().SendOldMail(mail) ? 1 : 0;
            if (TimerHelper._countErrorSenMail < 10)
              return;
            TimerHelper._emailOldTimer.Stop();
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Ошибка таймера отправки email");
          }
        }));
    }

    private static void EmailSend(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер отправки отчетов");
      if (TimerHelper._countErrorSenMail >= 10)
      {
        LogHelper.Debug("Превышено количество попыток отправки Email");
        TimerHelper._emailTimer.Stop();
      }
      else
      {
        TimerHelper._emailTimer.Stop();
        Task.Run((Action) (() =>
        {
          try
          {
            if (!TimerHelper.Settings.RemoteControl.Email.IsActive || !TimerHelper.Settings.RemoteControl.Email.IsSendForTime)
              return;
            string timesSend = TimerHelper.Settings.RemoteControl.Email.TimesSend;
            char[] separator = new char[3]{ ';', ' ', ',' };
            foreach (string s in timesSend.Split(separator, StringSplitOptions.RemoveEmptyEntries))
            {
              DateTime result;
              if (DateTime.TryParse(s, out result))
              {
                TimeSpan timeOfDay1 = DateTime.Now.TimeOfDay;
                int hours1 = timeOfDay1.Hours;
                TimeSpan timeOfDay2 = result.TimeOfDay;
                int hours2 = timeOfDay2.Hours;
                if (hours1 == hours2)
                {
                  int minutes1 = timeOfDay1.Minutes;
                  timeOfDay2 = result.TimeOfDay;
                  int minutes2 = timeOfDay2.Minutes;
                  if (minutes1 == minutes2)
                  {
                    LogHelper.Debug("Отправка отчета на почту по расписанию - " + TimerHelper.Settings.RemoteControl.Email.EmailTo);
                    bool flag = new EmailRepository().Send(DateTime.Now, TimerHelper.Settings.RemoteControl.Email.EmailTo);
                    TimerHelper._countErrorSenMail += !flag ? 1 : 0;
                    if (TimerHelper._countErrorSenMail >= 10)
                    {
                      TimerHelper._emailTimer.Stop();
                      return;
                    }
                    TimerHelper._emailTimer.Start();
                    return;
                  }
                }
              }
            }
            TimerHelper._emailTimer.Start();
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Email timer error");
          }
        }));
      }
    }

    private static void InitializeTelegramOldTimer()
    {
      TimerHelper._telegramOldTimer.Dispose();
      TimerHelper._telegramOldTimer = new Timer()
      {
        Interval = 300000.0
      };
      TimerHelper._telegramOldTimer.Elapsed += new ElapsedEventHandler(TimerHelper.TelegramOldTimerOnElapsed);
      TimerHelper._telegramOldTimer.Start();
    }

    private static void InitializeTelegramTimer()
    {
      TimerHelper._telegramTimer.Stop();
      if (!TimerHelper.Settings.RemoteControl.Telegram.IsActive)
        return;
      TimerHelper._telegramTimer.Dispose();
      TimerHelper._telegramTimer = new Timer()
      {
        Interval = 60000.0
      };
      TimerHelper._telegramTimer.Elapsed += new ElapsedEventHandler(TimerHelper.TelegramTimerOnElapsed);
      TimerHelper._countErrorSendTelegram = 0;
      TimerHelper._telegramTimer.Start();
    }

    private static void TelegramOldTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер отправки старых сообщений ТГ");
      if (TimerHelper._countErrorSendTelegram >= 10)
        TimerHelper._telegramOldTimer.Stop();
      else
        Task.Run((Action) (() => { }));
    }

    private static void TelegramTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер отправки сообщений ТГ");
      if (TimerHelper._countErrorSendTelegram >= 10)
      {
        LogHelper.Debug("Превышено количество попыток отправки сообщений ТГ");
        TimerHelper._telegramTimer.Stop();
      }
      else
      {
        TimerHelper._telegramTimer.Stop();
        Task.Run((Action) (() =>
        {
          try
          {
            if (!TimerHelper.Settings.RemoteControl.Telegram.IsActive || !TimerHelper.Settings.RemoteControl.Telegram.IsSendForTime)
              return;
            string[] strArray = TimerHelper.Settings.RemoteControl.Telegram.TimesSend.Split(new char[3]
            {
              ';',
              ' ',
              ','
            }, StringSplitOptions.RemoveEmptyEntries);
            TimeSpan timeOfDay1 = DateTime.Now.TimeOfDay;
            foreach (string s in strArray)
            {
              DateTime result;
              if (DateTime.TryParse(s, out result))
              {
                int hours1 = timeOfDay1.Hours;
                TimeSpan timeOfDay2 = result.TimeOfDay;
                int hours2 = timeOfDay2.Hours;
                if (hours1 == hours2)
                {
                  int minutes1 = timeOfDay1.Minutes;
                  timeOfDay2 = result.TimeOfDay;
                  int minutes2 = timeOfDay2.Minutes;
                  if (minutes1 == minutes2)
                  {
                    LogHelper.Debug("Отправка отчета в Телеграмм по расписанию - " + TimerHelper.Settings.RemoteControl.Telegram.UsernameTo);
                    bool flag = TelegramHelper.SendReport(DateTime.Now, TimerHelper.Settings.RemoteControl.Telegram.UsernameTo);
                    TimerHelper._countErrorSendTelegram += !flag ? 1 : 0;
                    if (TimerHelper._countErrorSendTelegram >= 10)
                    {
                      TimerHelper._telegramTimer.Stop();
                      return;
                    }
                    TimerHelper._telegramTimer.Start();
                    return;
                  }
                }
              }
            }
            TimerHelper._telegramTimer.Start();
          }
          catch (Exception ex)
          {
            LogHelper.WriteError(ex, "Telegram timer error");
          }
        }));
      }
    }

    private static void InitializePlanfixTimer()
    {
      TimerHelper._planfixTimer.Stop();
      PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
      if (!planfix.IsActive || !planfix.IsSaveSale)
        return;
      ConfigManager.Config = new Planfix.Api.Config(planfix.AccountName, planfix.ApiUrl, planfix.DecryptedKeyApi, planfix.DecryptedToken);
      TimerHelper._planfixTimer.Dispose();
      TimerHelper._planfixTimer = new Timer()
      {
        Interval = (double) (planfix.IntervalAutoSave * 60000)
      };
      TimerHelper._planfixTimer.Elapsed += new ElapsedEventHandler(TimerHelper.UpdateDataInPlanFix);
      TimerHelper._planfixTimer.Start();
    }

    private static void UpdateDataInPlanFix(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер выгрузки в ПланФикс");
      TimerHelper._planfixTimer.Stop();
      Task.Run((Action) (() =>
      {
        Performancer performancer = new Performancer("Выгрзука данных в ПланФикс");
        try
        {
          PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
            List<Gbs.Core.Entities.Documents.Document> itemsNoLinqPf = documentsRepository.GetItemsNoLinqPf(planfix.DateStart);
            List<Gbs.Core.Entities.Documents.Document> itemReturnNoLinqPf = documentsRepository.GetItemReturnNoLinqPf(planfix.DateStart);
            IEnumerable<List<Gbs.Core.Entities.Payments.Payment>> paymentLists = Gbs.Core.Entities.Payments.GetItemPaymentNoLinqPf(planfix.DateStart).GroupBy<Gbs.Core.Entities.Payments.Payment, Guid>((Func<Gbs.Core.Entities.Payments.Payment, Guid>) (x => x.ParentUid)).Select<IGrouping<Guid, Gbs.Core.Entities.Payments.Payment>, List<Gbs.Core.Entities.Payments.Payment>>((Func<IGrouping<Guid, Gbs.Core.Entities.Payments.Payment>, List<Gbs.Core.Entities.Payments.Payment>>) (x => x.ToList<Gbs.Core.Entities.Payments.Payment>()));
            performancer.AddPoint("Получение данных");
            foreach (Gbs.Core.Entities.Documents.Document document in itemsNoLinqPf)
              RequestSaleSave.SaveSaleInPf(document);
            performancer.AddPoint("Сохранение продаж");
            foreach (Gbs.Core.Entities.Documents.Document document in itemReturnNoLinqPf)
            {
              Gbs.Core.Entities.Documents.Document returnDoc = document;
              PlanfixHelper.AnaliticHelper.AddReturnAnalitic(returnDoc, new Gbs.Core.Entities.Documents.Document()
              {
                Uid = document.ParentUid
              });
            }
            performancer.AddPoint("Добавление аналитик возврата");
            foreach (List<Gbs.Core.Entities.Payments.Payment> paymentList in paymentLists)
              PlanfixHelper.AnaliticHelper.AddPaymentCreditAnalitic(new Gbs.Core.Entities.Documents.Document()
              {
                Uid = paymentList.First<Gbs.Core.Entities.Payments.Payment>().ParentUid
              }, paymentList);
            performancer.AddPoint("Добавление аналитик по платежам");
          }
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка при выгрузке продаж в ПланФикс по расписанию");
        }
        finally
        {
          performancer.Stop();
        }
        TimerHelper._planfixTimer.Start();
      }));
    }

    private static void InitializeCatalogTimer()
    {
      TimerHelper._exchangeCatalogTimer.Stop();
      if (!TimerHelper.Settings.ExchangeData.CatalogExchange.Ftp.IsSend && !TimerHelper.Settings.ExchangeData.CatalogExchange.Local.IsSend)
        return;
      TimerHelper._exchangeCatalogTimer.Dispose();
      TimerHelper._exchangeCatalogTimer = new Timer()
      {
        Interval = 60000.0
      };
      TimerHelper._exchangeCatalogTimer.Elapsed += new ElapsedEventHandler(TimerHelper.ExchangeCatalogTimerOnElapsed);
      TimerHelper._exchangeCatalogTimer.Start();
    }

    private static void ExchangeCatalogTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер выгрузки каталога товаров");
      TimerHelper._exchangeCatalogTimer.Stop();
      Task.Run((Action) (() =>
      {
        try
        {
          string[] times1 = TimerHelper.Settings.ExchangeData.CatalogExchange.Local.Time.Split(new char[3]
          {
            ';',
            ' ',
            ','
          }, StringSplitOptions.RemoveEmptyEntries);
          string[] times2 = TimerHelper.Settings.ExchangeData.CatalogExchange.Ftp.Time.Split(new char[3]
          {
            ';',
            ' ',
            ','
          }, StringSplitOptions.RemoveEmptyEntries);
          if (TimerHelper.Settings.ExchangeData.CatalogExchange.Local.IsSend)
            CheckTime(times1, true);
          if (TimerHelper.Settings.ExchangeData.CatalogExchange.Ftp.IsSend)
            CheckTime(times2, false);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка в таймере выгрузки каталога");
        }
        TimerHelper._exchangeCatalogTimer.Start();
      }));

      static void CheckTime(string[] times, bool isLocal)
      {
        foreach (string time in times)
        {
          DateTime result;
          if (DateTime.TryParse(time, out result))
          {
            TimeSpan timeOfDay1 = DateTime.Now.TimeOfDay;
            int hours1 = timeOfDay1.Hours;
            TimeSpan timeOfDay2 = result.TimeOfDay;
            int hours2 = timeOfDay2.Hours;
            if (hours1 == hours2)
            {
              int minutes1 = timeOfDay1.Minutes;
              timeOfDay2 = result.TimeOfDay;
              int minutes2 = timeOfDay2.Minutes;
              if (minutes1 == minutes2)
              {
                LogHelper.Debug(string.Format("Выгрузка каталога по расписанию - {0} в папку {1} ", (object) time, (object) TimerHelper.Settings.ExchangeData.CatalogExchange.Local) + "(FTP: " + TimerHelper.Settings.ExchangeData.CatalogExchange.Ftp.Path);
                using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
                {
                  List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
                  List<ExchangeDataHelper.Good> list = activeItems.Select<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>((Func<Gbs.Core.Entities.Goods.Good, ExchangeDataHelper.Good>) (x => new ExchangeDataHelper.Good(x))).ToList<ExchangeDataHelper.Good>();
                  if (isLocal)
                    ExchangeDataHelper.DoExchangeCatalogLocal(list, TimerHelper.Settings.ExchangeData, (IEnumerable<Gbs.Core.Entities.Goods.Good>) activeItems, out string _);
                  else
                    ExchangeDataHelper.DoExchangeCatalogFtp(list, TimerHelper.Settings.ExchangeData, (IEnumerable<Gbs.Core.Entities.Goods.Good>) activeItems);
                  TimerHelper._exchangeCatalogTimer.Start();
                  break;
                }
              }
            }
          }
        }
      }
    }

    private static void InitializeHomeOfficeTimer()
    {
      TimerHelper._homeOfficeTimer.Stop();
      TimerHelper._homeCheckJsonTimer.Stop();
      TimerHelper._homeUpdateDataBaseTimer.Stop();
      if (!TimerHelper.Settings.RemoteControl.Cloud.IsActive)
        return;
      if (TimerHelper.Settings.RemoteControl.Cloud.IsSendForTime)
      {
        TimerHelper._homeOfficeTimer.Dispose();
        TimerHelper._homeOfficeTimer = new Timer()
        {
          Interval = 60000.0
        };
        TimerHelper._homeOfficeTimer.Elapsed += new ElapsedEventHandler(TimerHelper.HomeOfficeTimerOnElapsed);
        TimerHelper._homeOfficeTimer.Start();
      }
      TimerHelper._homeCheckJsonTimer.Dispose();
      TimerHelper._homeCheckJsonTimer = new Timer()
      {
        Interval = 60000.0
      };
      TimerHelper._homeCheckJsonTimer.Elapsed += new ElapsedEventHandler(TimerHelper.HomeCheckJsonTimerOnElapsed);
      TimerHelper._homeCheckJsonTimer.Start();
    }

    public static void InitializeUpdateHomeOfficeTimer()
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home)
        return;
      TimerHelper._homeUpdateDataBaseTimer.Dispose();
      TimerHelper._homeUpdateDataBaseTimer = new Timer()
      {
        Interval = 300000.0
      };
      TimerHelper._homeUpdateDataBaseTimer.Elapsed += new ElapsedEventHandler(TimerHelper.HomeUpdateDataBaseTimerOnElapsed);
      TimerHelper._homeUpdateDataBaseTimer.Start();
    }

    private static void HomeUpdateDataBaseTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер обновления данных в режиме дом\\офис");
      TimerHelper._homeUpdateDataBaseTimer.Stop();
      Task.Run((Action) (() =>
      {
        ZipFile zipWithDb = (ZipFile) null;
        try
        {
          DataBaseHelper.CheckDbConnection();
          string uidDb = UidDb.GetUid().EntityUid.ToString();
          List<FileInfo> list = ((IEnumerable<string>) Directory.GetFiles(TimerHelper.Settings.RemoteControl.Cloud.Path)).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).Where<FileInfo>((Func<FileInfo, bool>) (x => x.Name.Contains(uidDb))).ToList<FileInfo>();
          if (!list.Any<FileInfo>())
            return;
          FileInfo fileInfo1 = list.FirstOrDefault<FileInfo>((Func<FileInfo, bool>) (x => x.Extension == ".info"));
          Guid result;
          if (fileInfo1 == null || !Guid.TryParse(fileInfo1.Name.Replace(".info", ""), out result))
            return;
          HomeOfficeHelper.InfoArchive infoArchive = JsonConvert.DeserializeObject<HomeOfficeHelper.InfoArchive>(File.ReadAllText(fileInfo1.FullName));
          HomeOfficeHelper.PointInfo point = new HomeOfficeHelper.PointInfo()
          {
            InfoDataBase = infoArchive,
            DbUid = result
          };
          List<Gbs.Core.Entities.Users.User> onlineUsers = new List<Gbs.Core.Entities.Users.User>();
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            onlineUsers = new UsersRepository(dataBase).GetOnlineUsersList();
          FileInfo fileInfo2 = new FileInfo(Path.Combine(TimerHelper.Settings.RemoteControl.Cloud.Path, uidDb + ".7z"));
          if (HomeOfficeHelper.CloudArchiveLastWriteDateTime >= fileInfo2.LastWriteTime)
          {
            GlobalData.IsMarket5ImportAcitve = false;
            LogHelper.Debug("Архив в облаке не изменился");
            TimerHelper._homeUpdateDataBaseTimer.Start();
          }
          else if (Other.IsAnyForm() && !DevelopersHelper.IsDebug())
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.TimerHelper_HomeUpdateDataBaseTimerOnElapsed_Доступны_для_отображения_новые_данные_из_торговой_точки__Для_обновления_откройте_Файл___Сменить_базу_данных_));
            LogHelper.Debug("Открыты доп. окна в программе, не обновляем архив");
          }
          else
          {
            string str1 = Path.Combine(FileSystemHelper.TempFolderPath(), fileInfo2.Name);
            FileSystemHelper.CopyFile(fileInfo2.FullName, str1);
            zipWithDb = FileSystemHelper.OpenZip(str1);
            string str2 = Path.Combine(ApplicationInfo.GetInstance().Paths.CachePath, uidDb);
            FileInfo oldMain = ((IEnumerable<FileInfo>) Directory.CreateDirectory(str2).GetFiles()).FirstOrDefault<FileInfo>((Func<FileInfo, bool>) (x => x.Extension == ".fdb"));
            Other.ConsoleWrite(string.Format("timeEdit: {0}; lastWriteTime: {1}", (object) PointSelectViewModel.GetTimeEdit(str2), (object) fileInfo2.LastWriteTime));
            GlobalData.IsMarket5ImportAcitve = true;
            string str3 = PointSelectViewModel.UpdateDataBase(oldMain, zipWithDb, point, str2) ?? oldMain?.FullName;
            if (!str3.IsNullOrEmpty())
            {
              LogHelper.Debug("Обновление БД успешно прошло, path = " + str3);
              DataBaseHelper.PathToDb = str3;
              CacheHelper.ClearAll();
              Cache.ClearAndLoadCache();
              using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
              {
                foreach (Gbs.Core.Entities.Users.User user in new UsersRepository(dataBase).GetAllItems().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (u => onlineUsers.Any<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Uid == u.Uid)))))
                {
                  Gbs.Core.Entities.Users.User u = user;
                  u.OnlineOnSectionUid = onlineUsers.First<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Uid == u.Uid)).OnlineOnSectionUid;
                  new UsersRepository(dataBase).Save(u);
                }
                Application.Current?.Dispatcher?.Invoke((Action) (() =>
                {
                  MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault<MainWindow>();
                  if (mainWindow == null)
                    return;
                  MainWindowViewModel dataContext = (MainWindowViewModel) mainWindow.DataContext;
                  dataContext.CurrentBasket.SaleNumber = SalePoints.GetSalePointList().First<SalePoints.SalePoint>()?.Number.SaleNumber.ToString() ?? "";
                  dataContext.UpdateInfo();
                }));
              }
              zipWithDb.Dispose();
              GlobalData.IsMarket5ImportAcitve = false;
              HomeOfficeHelper.CloudArchiveLastWriteDateTime = fileInfo2.LastWriteTime;
              ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
              {
                Text = string.Format(Translate.TimerHelper_Обновлены_данные_из_торговой_точки__0_, UidDb.GetUid().Value)
              });
            }
            else
              zipWithDb.Dispose();
          }
        }
        catch (Exception ex)
        {
          GlobalData.IsMarket5ImportAcitve = false;
          LogHelper.WriteError(ex, "Ошибка в таймере дом-офис");
          zipWithDb?.Dispose();
        }
        finally
        {
          GlobalData.IsMarket5ImportAcitve = false;
          TimerHelper._homeUpdateDataBaseTimer.Start();
        }
      }));
    }

    private static void HomeOfficeTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер выгрузки данных для дом\\офис");
      Task.Run((Action) (() =>
      {
        try
        {
          if (!TimerHelper.Settings.RemoteControl.Cloud.IsSendForTime)
            return;
          string timesSend = TimerHelper.Settings.RemoteControl.Cloud.TimesSend;
          char[] separator = new char[3]{ ';', ' ', ',' };
          foreach (string s in timesSend.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            DateTime result;
            if (DateTime.TryParse(s, out result))
            {
              TimeSpan timeOfDay1 = DateTime.Now.TimeOfDay;
              int hours1 = timeOfDay1.Hours;
              TimeSpan timeOfDay2 = result.TimeOfDay;
              int hours2 = timeOfDay2.Hours;
              if (hours1 == hours2)
              {
                int minutes1 = timeOfDay1.Minutes;
                timeOfDay2 = result.TimeOfDay;
                int minutes2 = timeOfDay2.Minutes;
                if (minutes1 == minutes2)
                {
                  LogHelper.Debug("Выгрузка архива для дом/офис по расписанию");
                  BackgroundTasksHelper.AddTaskToQueue((Action) (() => HomeOfficeHelper.CreateArchive()), BackgroundTaskTypes.DataBaseSyncDataPreparing);
                  break;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Home office timer error");
        }
      }));
    }

    private static void HomeCheckJsonTimerOnElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер проверки изменений сделанных в дом\\офис");
      TimerHelper._homeCheckJsonTimer.Stop();
      Task.Run((Action) (() =>
      {
        try
        {
          HomeOfficeHelper.FindFilesFromHomeOffice();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка разбора изменений из д/о");
        }
        finally
        {
          TimerHelper._homeCheckJsonTimer.Start();
        }
      }));
    }

    private static void InitializeClientUploadTimer()
    {
      TimerHelper._clientUploadTimer.Stop();
      if (new ConfigsRepository<Settings>().Get().Clients.SyncMode == GlobalDictionaries.ClientSyncModes.None)
        return;
      TimerHelper._clientUploadTimer.Dispose();
      TimerHelper._clientUploadTimer = new Timer()
      {
        Interval = 60000.0
      };
      TimerHelper._clientUploadTimer.Elapsed += new ElapsedEventHandler(TimerHelper._clientUploadTimer_Elapsed);
      TimerHelper._clientUploadTimer.Start();
    }

    private static void _clientUploadTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      DateTime now = DateTime.Now;
      if (now.Minute != 5 && now.Minute != 35)
        return;
      LogHelper.Trace("Сработал таймер выгрузки контактов");
      ClientsExchangeHelper.UploadFileClients();
    }

    private static void InitializeCatalogUploadTimer()
    {
      TimerHelper._catalogUploadTimer.Stop();
      if (!new ConfigsRepository<Settings>().Get().ExchangeData.CatalogExchange.IsCatalogExchangeForAllPoint)
        return;
      TimerHelper._catalogUploadTimer.Dispose();
      TimerHelper._catalogUploadTimer = new Timer()
      {
        Interval = 900000.0
      };
      TimerHelper._catalogUploadTimer.Elapsed += new ElapsedEventHandler(TimerHelper._catalogUploadTimer_Elapsed);
      TimerHelper._catalogUploadTimer.Start();
    }

    private static void _catalogUploadTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      DateTime now = DateTime.Now;
      TimerHelper._catalogUploadTimer.Stop();
      LogHelper.Trace("Сработал таймер выгрузки каталога товаров");
      ExchangeDataHelper.DoExchangeCatalogForAllPoint();
      TimerHelper._catalogUploadTimer.Start();
    }

    private static void InitializeUpdateUserInputTimer()
    {
      if (TimerHelper._updateUserInputTimer != null)
        return;
      TimerHelper._updateUserInputTimer?.Dispose();
      TimerHelper._updateUserInputTimer = new Timer()
      {
        Interval = 60000.0
      };
      TimerHelper._updateUserInputTimer.Elapsed += new ElapsedEventHandler(TimerHelper.UpdateUserInputTimerElapsed);
      TimerHelper._updateUserInputTimer.Start();
    }

    private static void UpdateUserInputTimerElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер обновления статуса пользователей");
      TimerHelper._updateUserInputTimer.Stop();
      Task.Run((Action) (() =>
      {
        try
        {
          Other.ConsoleWrite("Обнолвение статуса пользователей в истории");
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            UserHistoryRepository historyRepository = new UserHistoryRepository(dataBase);
            Guid currentSectionUid = Sections.GetCurrentSection().Uid;
            List<UserHistory> list = historyRepository.GetActiveItems().Where<UserHistory>((Func<UserHistory, bool>) (x => x.SectionUid == currentSectionUid && x.User != null)).GroupBy<UserHistory, Guid>((Func<UserHistory, Guid>) (x => x.User.Uid)).Select<IGrouping<Guid, UserHistory>, UserHistory>((Func<IGrouping<Guid, UserHistory>, UserHistory>) (x => x.OrderBy<UserHistory, DateTime>((Func<UserHistory, DateTime>) (v => v.DateIn)).Last<UserHistory>())).Where<UserHistory>((Func<UserHistory, bool>) (x => x.User.OnlineOnSectionUid == currentSectionUid)).ToList<UserHistory>();
            foreach (UserHistory userHistory in list)
              userHistory.DateOut = DateTime.Now;
            historyRepository.Save(list);
          }
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка в таймере обнолвения статуса пользователя");
        }
        TimerHelper._updateUserInputTimer.Start();
      }));
    }

    private static void InitializeMoveTimer()
    {
      TimerHelper._moveCheckTimer.Dispose();
      TimerHelper._moveCheckTimer = new Timer()
      {
        Interval = 1800000.0
      };
      TimerHelper._moveCheckTimer.Elapsed += new ElapsedEventHandler(TimerHelper.MoveCheckTimerElapsed);
      TimerHelper._moveCheckTimer.Start();
    }

    private static void MoveCheckTimerElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер проверки отправлений из другой ТТ");
      TimerHelper._moveCheckTimer.Stop();
      Task.Run((Action) (() =>
      {
        try
        {
          if (MoveHelper.IsNewDocument())
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.TimerHelper_Перемещения_товаров,
              Text = Translate.TimerHelper_Найдено_входящее_перемещение__для_того_чтобы_принять_нажмите_Товары___Новое_поступление___Из_другой_точки_
            });
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка таймера проверки перемещений");
        }
        TimerHelper._moveCheckTimer.Start();
      }));
    }

    private static void InitializeRamTimer()
    {
      TimerHelper._ramTimer.Dispose();
      TimerHelper._ramTimer = new Timer()
      {
        Interval = 300000.0
      };
      TimerHelper._ramTimer.Elapsed += new ElapsedEventHandler(TimerHelper.RamTimerElapsed);
      TimerHelper._ramTimer.Start();
    }

    private static void RamTimerElapsed(object sender, ElapsedEventArgs e)
    {
      HardwareInfo.GetRamValue();
    }

    private static void InitializeInfoLogTimer()
    {
      TimerHelper._infoLogTimer.Dispose();
      TimerHelper._infoLogTimer = new Timer()
      {
        Interval = 21600000.0
      };
      TimerHelper._infoLogTimer.Elapsed += new ElapsedEventHandler(TimerHelper.InfoLogTimerElapsed);
      TimerHelper._infoLogTimer.Start();
    }

    private static void InfoLogTimerElapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Debug("Пишем по таймеру инфо для логов:");
      LogHelper.Debug("GBS ID: " + LicenseHelper.GetInfo().GbsId);
      LogHelper.Debug("Версия: " + ApplicationInfo.GetInstance().GbsVersion?.ToString());
    }

    private static void InitializeLicenseTimer()
    {
      TimerHelper._licenseTimer.Dispose();
      TimerHelper._licenseTimer = new Timer()
      {
        Interval = 300000.0
      };
      TimerHelper._licenseTimer.Elapsed += new ElapsedEventHandler(TimerHelper.LicenseTimerElapsed);
      TimerHelper._licenseTimer.Start();
    }

    private static void LicenseTimerElapsed(object sender, ElapsedEventArgs e)
    {
    }

    private static void InitCheckRegistryTimer()
    {
      TimerHelper._checkRegistryTimer.Dispose();
      TimerHelper._checkRegistryTimer = new Timer()
      {
        Interval = 3600000.0
      };
      TimerHelper._checkRegistryTimer.Elapsed += (ElapsedEventHandler) ((sender, args) =>
      {
        try
        {
          GbsIdHelperV02.RegData regData = GbsIdHelperV02.ReadDataFromRegistry();
          if (regData == null)
            LogHelper.Trace("registry data is null");
          else
            LogHelper.Trace(string.Format("registry data: {0}", (object) regData));
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      });
      TimerHelper._checkRegistryTimer.Start();
    }

    private static void InitializeSetStatusWebOfficeTimer()
    {
      TimerHelper._statusWoTimer.Stop();
      TimerHelper._statusWoTimer.Dispose();
      if (!new ConfigsRepository<Settings>().Get().RemoteControl.WebOffice.IsActive)
        return;
      TimerHelper._statusWoTimer = new Timer()
      {
        Interval = new TimeSpan(0, 15, 0).TotalMilliseconds
      };
      TimerHelper._statusWoTimer.Elapsed += new ElapsedEventHandler(TimerHelper._statusWoTimer_Elapsed);
      TimerHelper._statusWoTimer.Start();
    }

    private static void _statusWoTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер обновления статуса ТТ для WO");
      WebOfficeHelper.SetStatusForPoint();
    }

    private static void InitializeWebOfficeTimer()
    {
      TimerHelper._woTimer.Stop();
      Gbs.Core.Config.WebOffice webOffice = new ConfigsRepository<Settings>().Get().RemoteControl.WebOffice;
      if (!webOffice.IsActive)
        return;
      int hours;
      switch (webOffice.CreatePeriod)
      {
        case Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery1Hour:
          hours = 1;
          break;
        case Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery3Hours:
          hours = 3;
          break;
        case Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery6Hours:
          hours = 6;
          break;
        case Gbs.Core.Config.RemoteControl.CreateItemPeriods.AndEvery12Hours:
          hours = 12;
          break;
        default:
          return;
      }
      TimerHelper._woTimer.Dispose();
      TimerHelper._woTimer = new Timer()
      {
        Interval = new TimeSpan(hours, 0, 0).TotalMilliseconds
      };
      TimerHelper._woTimer.Elapsed += new ElapsedEventHandler(TimerHelper._woTimer_Elapsed);
      TimerHelper._woTimer.Start();
    }

    private static void _woTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер выгрузки в ВО");
      WebOfficeHelper.CreateArchive();
    }

    private static void InitializeSetStatusGrafanaTimer()
    {
      TimerHelper._statusGrafanaTimer.Stop();
      TimerHelper._statusGrafanaTimer.Dispose();
      TimerHelper._statusGrafanaTimer = new Timer()
      {
        Interval = new TimeSpan(6, 0, 0).TotalMilliseconds
      };
      TimerHelper._statusGrafanaTimer.Elapsed += new ElapsedEventHandler(TimerHelper._statusGrafanaTimer_Elapsed);
      TimerHelper._statusGrafanaTimer.Start();
    }

    private static void _statusGrafanaTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      LogHelper.Trace("Сработал таймер обновления статуса в графане (статистика)");
      TaskHelper.TaskRun((Action) (() => ServerScriptsHelper.SendPing(new ConfigsRepository<Settings>().Get().System.Info_001.Data.DecryptedValue)), false);
    }

    public static void InitializeTimers(Action updateTime)
    {
      LogHelper.Debug("Инициализация таймеров");
      TimerHelper.Settings = new ConfigsRepository<Settings>().Get();
      TimerHelper.InitializeBackUpTimer();
      TimerHelper.InitializeUpdateTimer();
      TimerHelper.InitializeTime(updateTime);
      TimerHelper.InitializeUpdateHomeOfficeTimer();
      TimerHelper.InitializeRamTimer();
      TimerHelper.InitCheckRegistryTimer();
      TimerHelper.InitializeSetStatusGrafanaTimer();
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
        return;
      TimerHelper.InitializeUpdateUserInputTimer();
      TimerHelper.InitializeEmailOldTimer();
      TimerHelper.InitializePlanfixTimer();
      TimerHelper.InitializeCatalogTimer();
      TimerHelper.InitializeEmailTimer();
      TimerHelper.InitializeHomeOfficeTimer();
      TimerHelper.InitializeClientUploadTimer();
      TimerHelper.InitializeCatalogUploadTimer();
      TimerHelper.InitializeMoveTimer();
      TimerHelper.InitializeTelegramOldTimer();
      TimerHelper.InitializeTelegramTimer();
      TimerHelper.InitializeWebOfficeTimer();
      TimerHelper.InitializeSetStatusWebOfficeTimer();
    }
  }
}
