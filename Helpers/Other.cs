// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Other
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Emails;
using Gbs.Forms._shared;
using Gbs.Forms.Main;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Excel;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Updates;
using Gbs.Helpers.WebOffice;
using Gbs.Resources.Localizations;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers
{
  public static class Other
  {
    private static int? _maxGoodId;

    public static Decimal ConvertToDecimal(string s)
    {
      LogHelper.Debug("Пришло: " + s);
      if (s == null)
        return 0M;
      if (s.Contains<char>(','))
        s = s.Replace(',', '.');
      LogHelper.Debug("Получилось: " + s);
      return Convert.ToDecimal(s, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTimeStamp).ToLocalTime();
    }

    public static bool SetAllowUnsafeHeaderParsing()
    {
      try
      {
        Assembly assembly = Assembly.GetAssembly(typeof (SettingsSection));
        if (assembly == (Assembly) null)
          return false;
        Type type = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
        if (type == (Type) null)
          return false;
        object obj = type.InvokeMember("Section", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty, (Binder) null, (object) null, new object[0]);
        if (obj == null)
          return false;
        FieldInfo field = type.GetField("useUnsafeHeaderParsing", BindingFlags.Instance | BindingFlags.NonPublic);
        if (field == (FieldInfo) null)
          return false;
        field.SetValue(obj, (object) true);
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка установки конфигурации");
        return false;
      }
    }

    public static bool IsValidateEmail(string email)
    {
      try
      {
        MailAddress mailAddress = new MailAddress(email);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool IsActiveAndShowForm<T>(string uid = "") where T : Window
    {
      if (System.Windows.Application.Current.Windows.OfType<T>().Count<T>((System.Func<T, bool>) (x => x.IsVisible && x.Uid == uid)) < 1)
        return true;
      T obj = System.Windows.Application.Current.Windows.OfType<T>().First<T>((System.Func<T, bool>) (x => x.IsVisible && x.Uid == uid));
      obj.Activate();
      obj.WindowState = WindowState.Normal;
      return false;
    }

    public static bool IsActiveForm<T>() where T : Window
    {
      try
      {
        bool r = false;
        System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() => r = System.Windows.Application.Current.Windows.OfType<T>().Any<T>((System.Func<T, bool>) (x => x.IsActive))));
        return r;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка получения статуса активности окна");
        return false;
      }
    }

    public static bool IsAnyForm()
    {
      bool r = false;
      System.Windows.Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        System.Windows.Application.Current.Windows.OfType<Window>();
        r = System.Windows.Application.Current.Windows.OfType<WindowWithSize>().Where<WindowWithSize>((System.Func<WindowWithSize, bool>) (x => !x.GetType().IsEither<Type>(typeof (WindowWithSize), typeof (MainWindow), typeof (Authorization)))).Any<WindowWithSize>();
      }));
      Other.ConsoleWrite(string.Format("Is any windows opened: {0}", (object) r));
      return r;
    }

    public static bool CloseAllForm()
    {
      try
      {
        LogHelper.Debug("Закрываем все формы");
        System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
        {
          System.Windows.Application current = System.Windows.Application.Current;
          if (current == null)
            return;
          current.Windows.OfType<Window>().Where<Window>((System.Func<Window, bool>) (x => !x.GetType().IsEither<Type>(typeof (MainWindow), typeof (FrmSplashScreen)))).ToList<Window>().ForEach((Action<Window>) (x => x.Close()));
        }));
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка при закрытии всех окон программы");
        return false;
      }
    }

    [Localizable(false)]
    [Obsolete("Лучше не использовать, чтобы для локализации создавались нормальные фразы")]
    public static string NewLine(int lines = 1, bool onlyR = false)
    {
      string empty = string.Empty;
      for (int index = 0; index < lines; ++index)
        empty += onlyR ? "\r" : "\r\n";
      return empty;
    }

    public static void ConsoleWrite(object o) => Other.ConsoleWrite(o.ToString());

    [Localizable(false)]
    public static void ConsoleWrite(string text)
    {
      try
      {
        if (DevelopersHelper.IsUnitTest())
          return;
        Console.WriteLine(text);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Не удалось вывести в консоль сообщение. Исключение: " + ex?.ToString());
      }
    }

    public static void SetCorrectExit()
    {
      ConfigsRepository<Gbs.Core.Config.DataBase> configsRepository = new ConfigsRepository<Gbs.Core.Config.DataBase>();
      Gbs.Core.Config.DataBase config = configsRepository.Get();
      config.CorrectExit = true;
      configsRepository.Save(config);
    }

    public static bool RestartApplication(bool isPrepareToClose = true)
    {
      try
      {
        LogHelper.Debug("Restart application");
        string applicationPath = ApplicationInfo.GetInstance().Paths.ApplicationPath;
        List<string> stringList = new List<string>();
        stringList.Add("App.Restarter.exe");
        stringList.Add("GBS.Restarter.exe");
        string str1 = string.Empty;
        foreach (string str2 in stringList)
        {
          string path = applicationPath + "\\Restarter\\" + str2;
          if (File.Exists(path))
          {
            str1 = path;
            break;
          }
        }
        if (string.IsNullOrEmpty(str1))
        {
          int num = (int) MessageBoxHelper.Show(Translate.НеУдалосьПерезапуститьПриложениеТКНеНайденФайлGBSRestarterExe + Other.NewLine(2) + Translate.ПерезапуститеПриложениеВручную);
          return false;
        }
        if (isPrepareToClose)
          Other.PrepareToCloseApplication();
        if (UpdateHelper_from65to66.IsReadyToUpdate())
          UpdateHelper_from65to66.DoUpdate();
        bool flag1 = UpdateHelper.CheckUpdateFromInfoFile(out Version _);
        GlobalDictionaries.VersionUpdate versionUpdate = new ConfigsRepository<Settings>().Get().Other.UpdateConfig.VersionUpdate;
        string md5Hash = CryptoHelper.GetMd5Hash(ApplicationInfo.GetInstance().Paths.ApplicationPath);
        try
        {
          FileSystemHelper.CopyFolder(ApplicationInfo.GetInstance().Paths.UpdatesPath, System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\AppUpdater_9b6e7ae5\\" + md5Hash);
        }
        catch (Exception ex)
        {
        }
        string str3 = string.Empty;
        if (flag1)
          str3 = !str1.EndsWith("App.Restarter.exe") ? " /" + versionUpdate.ToString().ToLower() : " /" + md5Hash;
        Process process = new Process();
        process.StartInfo.FileName = str1;
        process.StartInfo.Arguments = str3;
        process.StartInfo.WorkingDirectory = applicationPath + "\\Restarter\\";
        process.StartInfo.UseShellExecute = false;
        LogHelper.Debug("filename: " + str1 + ", args: " + str3);
        bool flag2 = process.Start();
        LogHelper.Debug(string.Format("Restarter.exe start result: {0}", (object) flag2));
        if (!flag2)
          return false;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка перезапуска приложения");
        return false;
      }
      try
      {
        ProgressBarHelper.Close();
        System.Environment.Exit(0);
      }
      catch (Exception ex)
      {
      }
      return true;
    }

    private static void PrepareToCloseApplication()
    {
      LogHelper.Debug("Подготовка к завершению работы");
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.Other_PrepareToCloseApplication_Подготовка_к_завершению_работы_программы);
      Task task1 = Task.Run((Action) (() =>
      {
        Gbs.Core.Config.DataBase.DbBackUp backUp = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().BackUp;
        LogHelper.Trace("Запуск задачи создания бэкапа");
        if (backUp.CreateBackup && backUp.IsCreateOnExit)
        {
          BackupHelper.CreateBackup();
          LogHelper.Trace("Задача создания бэкапа завершена");
        }
        else
          LogHelper.Trace("Не создаем бэкап  при закрытии программы согласно правилам из настроек");
      }));
      Task task2 = Task.Run((Action) (() =>
      {
        LogHelper.Trace("Запуск задачи отправки отчета на Telegram");
        Telegram telegram = new ConfigsRepository<Settings>().Get().RemoteControl.Telegram;
        if (telegram.IsSendOnOff && telegram.IsActive)
        {
          TelegramHelper.SendReport(DateTime.Now, telegram.UsernameTo, false);
          LogHelper.Trace("Задача отправки отчета  в телеграмм завершена");
        }
        else
          LogHelper.Trace("Задача отправки отчета отменена из-за настроек программы");
      }));
      Task task3 = Task.Run((Action) (() =>
      {
        LogHelper.Trace("Запуск задачи отправки отчета на email");
        Gbs.Core.Config.Email email = new ConfigsRepository<Settings>().Get().RemoteControl.Email;
        if (email.IsSendOnOff && email.IsActive)
        {
          new EmailRepository().Send(DateTime.Now, email.EmailTo, false);
          LogHelper.Trace("Задача отправки отчета завершена");
        }
        else
          LogHelper.Trace("Задача отправки отчета отменена из-за настроек программы");
      }));
      Task task4 = Task.Run((Action) (() => { }));
      Task task5 = Task.Run((Action) (() =>
      {
        Gbs.Core.Config.WebOffice webOffice = new ConfigsRepository<Settings>().Get().RemoteControl.WebOffice;
        LogHelper.Trace("Запуск задачи создания архива для вебофиса");
        if (webOffice.IsActive)
        {
          WebOfficeHelper.SetStatusForPoint();
          LogHelper.Trace("Статус ТТ в веб-офисе обновлен");
          if (!webOffice.IsCreateOnExit)
            return;
          WebOfficeHelper.CreateArchive();
          LogHelper.Trace("Задача создания архива для ВО завершена");
        }
        else
          LogHelper.Trace("Не создаем архив для ВО  при закрытии программы согласно правилам из настроек");
      }));
      while (!task1.IsCompleted || !task3.IsCompleted || !task4.IsCompleted || !task2.IsCompleted || !task5.IsCompleted)
        Thread.Sleep(50);
      LogHelper.Trace("Запуск задачи выхода пользователей");
      using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        new UsersRepository(dataBase).DisconnectUsers();
        LogHelper.Trace("Задача выхода пользователей заверешена");
        Other.SetCorrectExit();
        Other.CloseAllForm();
        progressBar.Close();
      }
    }

    public static void CloseApplication(bool pcShutdown = false)
    {
      Gbs.Core.Config.Email email = new ConfigsRepository<Settings>().Get().RemoteControl.Email;
      Other.PrepareToCloseApplication();
      if (pcShutdown)
        Process.Start(new ProcessStartInfo("shutdown", "/s /t 10")
        {
          CreateNoWindow = true,
          UseShellExecute = false
        });
      try
      {
        System.Environment.Exit(0);
      }
      catch
      {
      }
    }

    public static string GetNumberDocument(GlobalDictionaries.DocumentsTypes type)
    {
      SalePoints.SalePoint salePoint = SalePoints.GetSalePointList().First<SalePoints.SalePoint>();
      int numDocument;
      switch (type)
      {
        case GlobalDictionaries.DocumentsTypes.Sale:
        case GlobalDictionaries.DocumentsTypes.CafeOrder:
          numDocument = salePoint.Number.SaleNumber;
          ++salePoint.Number.SaleNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.SaleReturn:
          numDocument = salePoint.Number.SaleReturnNumber;
          ++salePoint.Number.SaleReturnNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.Buy:
          using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            List<string> list = dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<System.Func<DOCUMENTS, bool>>) (x => x.TYPE == 3)).Select<DOCUMENTS, string>((Expression<System.Func<DOCUMENTS, string>>) (x => x.NUMBER)).ToList<string>();
            numDocument = salePoint.Number.WaybillNumber;
            while (list.Any<string>((System.Func<string, bool>) (x => x == numDocument.ToString())))
            {
              ++salePoint.Number.WaybillNumber;
              numDocument = salePoint.Number.WaybillNumber;
            }
            ++salePoint.Number.WaybillNumber;
            break;
          }
        case GlobalDictionaries.DocumentsTypes.BuyReturn:
          numDocument = salePoint.Number.WaybillReturnNumber;
          ++salePoint.Number.WaybillReturnNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.Move:
          numDocument = salePoint.Number.MoveNumber;
          ++salePoint.Number.MoveNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.MoveReturn:
          numDocument = salePoint.Number.MoveReturnNumber;
          ++salePoint.Number.MoveReturnNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.WriteOff:
          numDocument = salePoint.Number.WriteOffNumber;
          ++salePoint.Number.WriteOffNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.Inventory:
          numDocument = salePoint.Number.InventoryNumber;
          ++salePoint.Number.InventoryNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.ClientOrder:
          numDocument = salePoint.Number.ClientOrderNumber;
          ++salePoint.Number.ClientOrderNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.MoveStorage:
          numDocument = salePoint.Number.MoveStorageNumber;
          ++salePoint.Number.MoveStorageNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.ProductionList:
          numDocument = salePoint.Number.ProductionListNumber;
          ++salePoint.Number.ProductionListNumber;
          break;
        case GlobalDictionaries.DocumentsTypes.BeerProductionList:
          numDocument = salePoint.Number.BeerProductionListNumber;
          ++salePoint.Number.BeerProductionListNumber;
          break;
        default:
          return string.Empty;
      }
      salePoint.Save();
      return numDocument.ToString();
    }

    public static (bool Result, Gbs.Core.Entities.Users.User User) GetUserForDocument(
      Actions actions)
    {
      try
      {
        Gbs.Core.Config.Users users = new ConfigsRepository<Settings>().Get().Users;
        Gbs.Core.Entities.Users.User user1 = new Gbs.Core.Entities.Users.User();
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Users.User> list = CachesBox.AllUsers().Where<Gbs.Core.Entities.Users.User>((System.Func<Gbs.Core.Entities.Users.User, bool>) (x => x.OnlineOnSectionUid == Sections.GetCurrentSection().Uid)).ToList<Gbs.Core.Entities.Users.User>();
          if (users.NotRequestAuthorizationForSingleOnlineUser && list.Count == 1)
          {
            Gbs.Core.Entities.Users.User user2 = list.Single<Gbs.Core.Entities.Users.User>();
            if (new UsersRepository(dataBase).GetAccess(user2, actions))
              return (true, user2);
            if (!new Authorization().LoginUser(ref user2))
              return (false, (Gbs.Core.Entities.Users.User) null);
            if (new UsersRepository(dataBase).GetAccess(user2, actions))
              return (true, user2);
            int num = (int) MessageBoxHelper.Show(Translate.Other_Недостаточно_прав_для_выполнения_данного_действия_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Hand);
            return (false, (Gbs.Core.Entities.Users.User) null);
          }
          if (!new Authorization().LoginUser(ref user1))
            return (false, (Gbs.Core.Entities.Users.User) null);
          if (new UsersRepository(dataBase).GetAccess(user1, actions))
            return (true, user1);
          int num1 = (int) MessageBoxHelper.Show(Translate.Other_Недостаточно_прав_для_выполнения_данного_действия_, PartnersHelper.ProgramName(), icon: MessageBoxImage.Hand);
          return (false, (Gbs.Core.Entities.Users.User) null);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения пользователя для документа");
        return (false, (Gbs.Core.Entities.Users.User) null);
      }
    }

    public static void IsVisibilityDataGridColumn(System.Windows.Controls.DataGrid dg, System.Windows.Controls.ContextMenu cm)
    {
      if (dg.Columns.Count == cm.Items.Cast<System.Windows.Controls.MenuItem>().Count<System.Windows.Controls.MenuItem>((System.Func<System.Windows.Controls.MenuItem, bool>) (x => !x.IsChecked)))
      {
        int num = (int) MessageBoxHelper.Show(Translate.Other_Невозможно_скрыть_все_столбцы_);
        foreach (System.Windows.Controls.MenuItem menuItem in (IEnumerable) cm.Items)
        {
          System.Windows.Controls.MenuItem rb = menuItem;
          rb.IsChecked = dg.Columns.Single<DataGridColumn>((System.Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == rb.Uid)).Visibility == Visibility.Visible;
        }
      }
      else
      {
        foreach (System.Windows.Controls.MenuItem menuItem in (IEnumerable) cm.Items)
        {
          System.Windows.Controls.MenuItem rb = menuItem;
          DataGridColumn dataGridColumn = dg.Columns.FirstOrDefault<DataGridColumn>((System.Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == rb.Uid));
          if (dataGridColumn != null)
            dataGridColumn.Visibility = rb.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
      }
    }

    private static DataTable DataGridToDataTable(System.Windows.Controls.DataGrid dg)
    {
      try
      {
        DataTable dataTable = new DataTable();
        string livePreviewText = string.Empty;
        System.Windows.Application.Current?.Dispatcher?.Invoke((Action) (() =>
        {
          for (int index = 0; index < 10; ++index)
          {
            try
            {
              dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
              System.Windows.Clipboard.Clear();
              Thread.Sleep(100);
              dg.SelectAllCells();
              ApplicationCommands.Copy.Execute((object) null, (IInputElement) dg);
              Thread.Sleep(100);
              dg.UnselectAllCells();
              livePreviewText = (string) System.Windows.Clipboard.GetData(System.Windows.DataFormats.UnicodeText);
              break;
            }
            catch
            {
            }
            Thread.Sleep(100);
          }
        }));
        string[] source1 = livePreviewText.Split('\n');
        int num1 = 0;
        foreach (string[] strArray in ((IEnumerable<string>) source1).Where<string>((System.Func<string, bool>) (x => x.Length > 0)).Select<string, string[]>((System.Func<string, string[]>) (line => line.TrimEnd('\n').TrimEnd('\r').Split('\t'))))
        {
          if (num1 == 0)
          {
            int num2 = 1;
            List<string> source2 = new List<string>();
            foreach (string str1 in strArray)
            {
              string str2 = str1;
              if (source2.Contains(str1))
              {
                str2 += string.Format(" ({0})", (object) num2);
                ++num2;
              }
              source2.Add(str2);
            }
            dataTable.Columns.AddRange(source2.Select<string, DataColumn>((System.Func<string, DataColumn>) (x => new DataColumn(x))).ToArray<DataColumn>());
          }
          DataRow row = dataTable.NewRow();
          int columnIndex = 0;
          foreach (string str in strArray)
          {
            row[columnIndex] = (object) str;
            ++columnIndex;
          }
          dataTable.Rows.Add(row);
          ++num1;
        }
        return dataTable;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка преобразования DataGrid в DataTable");
        return (DataTable) null;
      }
    }

    private static string DataTableToCsv(DataTable dt)
    {
      try
      {
        string csv1 = string.Empty;
        OtherConfig.CsvSetting csv2 = new ConfigsRepository<Settings>().Get().Other.Csv;
        string str1 = csv2.IsOnQuote ? "\"" : string.Empty;
        string separate = csv2.Separate;
        foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
        {
          foreach (string str2 in ((IEnumerable<object>) row.ItemArray).Where<object>((System.Func<object, bool>) (x => !(x is DBNull))))
          {
            string str3 = str2.Replace('\r', ' ').Replace('\n', ' ');
            csv1 = csv1 + str1 + str3 + str1 + separate;
          }
          csv1 += Other.NewLine();
        }
        return csv1;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось преобразовать DataTable в Csv");
        return (string) null;
      }
    }

    public static async void ExportInFileDataGrid(System.Windows.Controls.DataGrid dg)
    {
      try
      {
        if (dg == null)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.Other_Нет_строк_для_сохранения_в_файл, icon: MessageBoxImage.Exclamation);
        }
        else if (dg.Items.Count == 0)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.Other_Нет_строк_для_сохранения_в_файл, icon: MessageBoxImage.Exclamation);
        }
        else
        {
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Filter = Translate.Other_Книга_Excel + " | *.xlsx|" + Translate.Other_Файл_CSV + " | *.csv";
          saveFileDialog.AddExtension = true;
          using (SaveFileDialog saveDialog = saveFileDialog)
          {
            if (saveDialog.ShowDialog() != DialogResult.OK)
              return;
            string path = saveDialog.FileName;
            if (path.IsNullOrEmpty())
              return;
            int filterIndex = saveDialog.FilterIndex;
            ProgressBarHelper.ProgressBar p = new ProgressBarHelper.ProgressBar(Translate.Other_Сохранение_данных_в_файл___);
            await Task.Run((Action) (() =>
            {
              Other.SaveToFile(dg, filterIndex, path);
              p.Close();
            }));
          }
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.Error(ex, "Ошибка при экспорте данных в файл");
      }
    }

    private static void SaveToFile(System.Windows.Controls.DataGrid dg, int filterIndex, string path)
    {
      bool flag = false;
      DataTable dataTable = Other.DataGridToDataTable(dg);
      if (dataTable != null)
      {
        switch (filterIndex)
        {
          case 1:
            XSSFWorkbook xssfWorkbook = ExcelFactory.Create(dataTable);
            if (xssfWorkbook != null)
            {
              ExcelFile.Write((IWorkbook) xssfWorkbook, path);
              flag = true;
              break;
            }
            break;
          case 2:
            string csv = Other.DataTableToCsv(dataTable);
            if (csv != null)
            {
              File.WriteAllText(path, csv, System.Text.Encoding.UTF8);
              flag = true;
              break;
            }
            break;
        }
      }
      ProgressBarViewModel.Notification n;
      if (!flag)
      {
        n = new ProgressBarViewModel.Notification()
        {
          Title = Translate.Other_SaveToFile_Экспорт_данных_в_файл,
          Text = Translate.Other_SaveToFile_При_экспорте_данных_в_файл_произошла_ошибка,
          Type = ProgressBarViewModel.Notification.NotificationsTypes.Error
        };
      }
      else
      {
        n = new ProgressBarViewModel.Notification();
        n.Title = Translate.Other_SaveToFile_Экспорт_данных_в_файл;
        n.Text = Translate.Other_SaveToFile_Экспорт_данных_в_файл_успешно_завершен;
      }
      ProgressBarHelper.AddNotification(n);
    }

    public static bool ExportInExcel<T>(List<T> data, string path)
    {
      ExcelFile.Write(!(data is List<ExchangeDataHelper.Good> goods) ? (IWorkbook) ExcelFactory.Create<T>(data) : (IWorkbook) ExcelFactory.Create(goods), path);
      return true;
    }

    public static bool ExportInCsv<T>(List<T> data, string path, bool isSave = true)
    {
      try
      {
        OtherConfig.CsvSetting csv = new ConfigsRepository<Settings>().Get().Other.Csv;
        string separate = csv.Separate;
        bool useQuotes = csv.IsOnQuote;
        List<string> headers = new List<string>();
        List<List<string>> contentList = new List<List<string>>();
        if (data is List<ExchangeDataHelper.Good> goods)
        {
          getTextGood(goods, ref headers, ref contentList, useQuotes);
        }
        else
        {
          headers = ((IEnumerable<PropertyInfo>) typeof (T).GetProperties()).Select<PropertyInfo, string>((System.Func<PropertyInfo, string>) (x => x.Name)).Select<string, string>((System.Func<string, string>) (column => !useQuotes ? column : "\"" + column + "\"")).ToList<string>();
          contentList = data.Select<T, List<string>>((System.Func<T, List<string>>) (r => ((IEnumerable<PropertyInfo>) r.GetType().GetProperties()).Select<PropertyInfo, string>((System.Func<PropertyInfo, string>) (prop => prop.GetValue((object) r)?.ToString() ?? "")).Select<string, string>((System.Func<string, string>) (text => !useQuotes ? text : "\"" + text + "\"")).ToList<string>())).ToList<List<string>>();
        }
        IEnumerable<IEnumerable<string>> strings = (IEnumerable<IEnumerable<string>>) contentList;
        if (FileSystemHelper.CheckIfFileIsBeingUsed(path))
        {
          LogHelper.Debug("Данные не сохранены, т.к. нет доступа для записи в файл " + path + Other.NewLine(2) + " Возможно, файл открыт в другой программе.");
          return false;
        }
        if (isSave)
        {
          using (StreamWriter streamWriter = new StreamWriter(path, false, System.Text.Encoding.Unicode))
          {
            streamWriter.WriteLine(string.Join(separate, (IEnumerable<string>) headers));
            foreach (IEnumerable<string> values in strings)
              streamWriter.WriteLine(string.Join(separate, values));
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при сохранении в файл CSV", false);
        return false;
      }

      static void getTextGood(
        List<ExchangeDataHelper.Good> goods,
        ref List<string> headers,
        ref List<List<string>> contentList,
        bool useQuotes)
      {
        headers = new List<string>();
        foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) typeof (ExchangeDataHelper.Good).GetProperties()).Where<PropertyInfo>((System.Func<PropertyInfo, bool>) (x => !x.Name.IsEither<string>("PropertiesList", "NamePoint", "UidPoint"))))
          headers.Add(useQuotes ? "\"" + propertyInfo.Name + "\"" : propertyInfo.Name);
        Dictionary<Guid, int> source1 = new Dictionary<Guid, int>();
        IEnumerable<EntityProperties.PropertyType> propertyTypes = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((System.Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted));
        int count = headers.Count;
        foreach (EntityProperties.PropertyType propertyType in propertyTypes)
        {
          headers.Add(useQuotes ? "\"" + propertyType.Name + "\"" : propertyType.Name);
          source1.Add(propertyType.Uid, count);
          ++count;
        }
        foreach (ExchangeDataHelper.Good good in goods)
        {
          int index1 = 0;
          string[] source2 = new string[headers.Count];
          foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) good.GetType().GetProperties()).Where<PropertyInfo>((System.Func<PropertyInfo, bool>) (x => !x.Name.IsEither<string>("PropertiesList", "NamePoint", "UidPoint"))))
          {
            string str = propertyInfo.GetValue((object) good)?.ToString() ?? "";
            source2[index1] = useQuotes ? "\"" + str + "\"" : str;
            ++index1;
          }
          foreach (ExchangeDataHelper.Good.Properties properties in good.PropertiesList)
          {
            ExchangeDataHelper.Good.Properties prop = properties;
            int index2 = source1.FirstOrDefault<KeyValuePair<Guid, int>>((System.Func<KeyValuePair<Guid, int>, bool>) (x => x.Key == prop.UidType)).Value;
            source2[index2] = useQuotes ? string.Format("\"{0}\"", prop.Value) : prop.Value.ToString();
            ++index1;
          }
          contentList.Add(((IEnumerable<string>) source2).ToList<string>());
        }
      }
    }
  }
}
