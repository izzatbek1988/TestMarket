// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Performance.HardwarePerformanceHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FirebirdSql.Data.FirebirdClient;
using Gbs.Core.Db;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Helpers.Performance
{
  internal static class HardwarePerformanceHelper
  {
    private static double CpuSpeed(int size = 300, bool multiThread = true)
    {
      try
      {
        Random random = new Random();
        double[,] matrixA = new double[size, size];
        double[,] matrixB = new double[size, size];
        double[,] resultMatrix = new double[size, size];
        for (int index1 = 0; index1 < size; ++index1)
        {
          for (int index2 = 0; index2 < size; ++index2)
          {
            matrixA[index1, index2] = random.NextDouble() * 100.0;
            matrixB[index1, index2] = random.NextDouble() * 100.0;
          }
        }
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        if (multiThread)
        {
          Parallel.For(0, size, (Action<int>) (i =>
          {
            for (int index3 = 0; index3 < size; ++index3)
            {
              double num = 0.0;
              for (int index4 = 0; index4 < size; ++index4)
                num += matrixA[i, index4] * matrixB[index4, index3];
              resultMatrix[i, index3] = num;
            }
          }));
        }
        else
        {
          for (int index5 = 0; index5 < size; ++index5)
          {
            for (int index6 = 0; index6 < size; ++index6)
            {
              double num = 0.0;
              for (int index7 = 0; index7 < size; ++index7)
                num += matrixA[index5, index7] * matrixB[index7, index6];
              resultMatrix[index5, index6] = num;
            }
          }
        }
        stopwatch.Stop();
        return Math.Pow((double) size, 3.0) / stopwatch.Elapsed.TotalSeconds / Math.Pow(10.0, 5.0);
      }
      catch (Exception ex)
      {
        return 0.0;
      }
    }

    private static (double read, double write) HddSpeed()
    {
      string path = FileSystemHelper.TempFolderPath() + "\\testfile.dat";
      int length = 104857600;
      byte[] numArray = new byte[length];
      new Random().NextBytes(numArray);
      try
      {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        File.WriteAllBytes(path, numArray);
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        double totalSeconds1 = elapsed.TotalSeconds;
        double num = (double) (length / 1048576) / totalSeconds1;
        stopwatch.Restart();
        File.ReadAllBytes(path);
        stopwatch.Stop();
        elapsed = stopwatch.Elapsed;
        double totalSeconds2 = elapsed.TotalSeconds;
        return ((double) (length / 1048576) / totalSeconds2, num);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        return (0.0, 0.0);
      }
      finally
      {
        if (File.Exists(path))
          File.Delete(path);
      }
    }

    [Localizable(false)]
    private static (double zip, double unzip) ZipSpeed()
    {
      string path = FileSystemHelper.TempFolderPath();
      try
      {
        string str1 = path + "\\testfile.dat";
        byte[] numArray = new byte[104857600];
        new Random().NextBytes(numArray);
        File.WriteAllBytes(str1, numArray);
        string str2 = path + "\\testfile.txt";
        Random random = new Random();
        string[] strArray = new string[97]
        {
          "экспонат",
          "молоко",
          "клавиатура",
          "алгоритм",
          "дисплей",
          "консоль",
          "программа",
          "блокчейн",
          "экран",
          "кнопка",
          "цикл",
          "функция",
          "класс",
          "метод",
          "объект",
          "протокол",
          "браузер",
          "сервер",
          "процессор",
          "оперативка",
          "память",
          "хранилище",
          "маршрутизатор",
          "интернет",
          "переключатель",
          "модем",
          "устройство",
          "настройка",
          "коммутация",
          "подключение",
          "виртуализация",
          "облачное",
          "программирование",
          "проект",
          "архитектура",
          "система",
          "платформа",
          "интерфейс",
          "версия",
          "документация",
          "релиз",
          "тестирование",
          "база данных",
          "таблица",
          "столбец",
          "строка",
          "индекс",
          "ключ",
          "связь",
          "прототип",
          "образец",
          "пользователь",
          "администратор",
          "протокол",
          "шифрование",
          "безопасность",
          "авторизация",
          "аутентификация",
          "сертификат",
          "подпись",
          "лог",
          "событие",
          "мониторинг",
          "отчет",
          "бюджет",
          "финансирование",
          "инвестор",
          "разработка",
          "поддержка",
          "внедрение",
          "сеть",
          "команда",
          "участник",
          "аналитик",
          "дизайнер",
          "разработчик",
          "тестер",
          "менеджер",
          "архитектор",
          "специалист",
          "эксперт",
          "предприниматель",
          "стратегия",
          "тактика",
          "план",
          "исполнитель",
          "концепция",
          "результат",
          "инновация",
          "идея",
          "стартап",
          "бизнес",
          "управление",
          "контроль",
          "ресурсы",
          "время",
          "эффективность"
        };
        StringBuilder stringBuilder = new StringBuilder();
        for (int index1 = 0; index1 < 5000; ++index1)
        {
          int num = random.Next(100, 1001);
          for (int index2 = 0; index2 < num; ++index2)
          {
            string str3 = strArray[random.Next(strArray.Length)];
            stringBuilder.Append(str3).Append(' ');
          }
          stringBuilder.AppendLine();
        }
        File.WriteAllText(str2, stringBuilder.ToString());
        string str4 = path + "\\archive.zip";
        Stopwatch stopwatch = Stopwatch.StartNew();
        FileSystemHelper.CreateZip(str4, (IEnumerable<FileInfo>) new FileInfo[2]
        {
          new FileInfo(str1),
          new FileInfo(str2)
        });
        double totalSeconds1 = stopwatch.Elapsed.TotalSeconds;
        stopwatch.Restart();
        FileSystemHelper.ExtractAllFile(str4, path + "\\uzip\\");
        double totalSeconds2 = stopwatch.Elapsed.TotalSeconds;
        long length;
        double num1 = (double) (length = new FileInfo(str4).Length) / totalSeconds1 / 100000.0;
        double num2 = (double) length / totalSeconds2 / 100000.0;
        stopwatch.Stop();
        return (num1, num2);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        return (0.0, 0.0);
      }
      finally
      {
        if (Directory.Exists(path))
          Directory.Delete(path, true);
      }
    }

    public static string GetPerformanceInfo()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.HardwarePerformanceHelper_GetPerformanceInfo_Замер_производительности);
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      double num1 = HardwarePerformanceHelper.CpuSpeed(multiThread: false);
      double num2 = HardwarePerformanceHelper.CpuSpeed();
      double totalSeconds1 = stopwatch.Elapsed.TotalSeconds;
      stopwatch.Restart();
      (double read, double write) tuple1 = HardwarePerformanceHelper.HddSpeed();
      double totalSeconds2 = stopwatch.Elapsed.TotalSeconds;
      stopwatch.Restart();
      (double zip, double unzip) tuple2 = HardwarePerformanceHelper.ZipSpeed();
      double totalSeconds3 = stopwatch.Elapsed.TotalSeconds;
      stopwatch.Restart();
      (double read, double write) tuple3 = HardwarePerformanceHelper.DbSpeed();
      double totalSeconds4 = stopwatch.Elapsed.TotalSeconds;
      stopwatch.Stop();
      double num3 = num1 / 2.7;
      double num4 = num2 / 16.8;
      double num5 = tuple1.read / 2.4;
      double num6 = tuple1.write / 2.0;
      double num7 = tuple2.zip / 1.4;
      double num8 = tuple2.unzip / 2.5;
      double num9 = tuple3.read / 3.0;
      double num10 = tuple3.write / 3.0;
      double num11 = ((IEnumerable<double>) new double[8]
      {
        num3,
        num4,
        num5,
        num6,
        num7,
        num8,
        num9,
        num10
      }).Average();
      progressBar.Close();
      return string.Format("CPU single core: {0:N0}; \n", (object) num3) + string.Format("CPU multi core: {0:N0}; \n", (object) num4) + string.Format("CPU test time: {0:N2} s.\n\n", (object) totalSeconds1) + string.Format("HDD write: {0:N0};\n", (object) num6) + string.Format("HDD read: {0:N0}; \n", (object) num5) + string.Format("HDD test time: {0:N2} s.\n\n", (object) totalSeconds2) + string.Format("ZIP zip: {0:N0};\n", (object) num7) + string.Format("ZIP unzip: {0:N0};\n", (object) num8) + string.Format("ZIP test time: {0:N2} s.\n\n", (object) totalSeconds3) + string.Format("DB write: {0:N0};\n", (object) num10) + string.Format("DB read: {0:N0};\n", (object) num9) + string.Format("DB test time: {0:N2} s.\n\n", (object) totalSeconds4) + string.Format("AVG score: {0:N0};\n", (object) num11) + string.Format("Total test time: {0:N2} s.", (object) (totalSeconds1 + totalSeconds2 + totalSeconds4 + totalSeconds3));
    }

    private static void Execute(this DataBase db, string sql)
    {
      using (FbCommand fbCommand = new FbCommand(sql, db.FbConnection))
        fbCommand.ExecuteNonQuery();
    }

    [Localizable(false)]
    private static (double read, double write) DbSpeed()
    {
      try
      {
        string str1 = "\"" + new string(Guid.NewGuid().ToString().Replace("-", "").Take<char>(10).ToArray<char>()) + "\"";
        using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          dataBase.Execute("\r\n                        CREATE TABLE  " + str1 + " (\r\n                            Id INTEGER NOT NULL PRIMARY KEY,\r\n                            Name VARCHAR(50) NOT NULL\r\n                        )");
          Stopwatch stopwatch = new Stopwatch();
          int num1 = 1000;
          stopwatch.Start();
          for (int index = 1; index <= num1; ++index)
          {
            string str2 = Guid.NewGuid().ToString();
            dataBase.Execute(string.Format("INSERT INTO {0} (Id, Name) VALUES ({1}, '{2}')", (object) str1, (object) index, (object) str2));
          }
          stopwatch.Stop();
          double num2 = (double) num1 / ((double) stopwatch.ElapsedMilliseconds / 1000.0);
          stopwatch.Restart();
          DataTable dataTable = new DataTable();
          for (int index = 1; index <= num1; ++index)
          {
            using (FbDataAdapter fbDataAdapter = new FbDataAdapter(string.Format("SELECT * FROM {0} WHERE Id={1} ORDER BY Name", (object) str1, (object) index), dataBase.FbConnection))
              fbDataAdapter.Fill(dataTable);
          }
          stopwatch.Stop();
          double num3 = (double) num1 / ((double) stopwatch.ElapsedMilliseconds / 1000.0);
          dataBase.Execute("DELETE FROM " + str1);
          dataBase.Execute("DROP TABLE " + str1);
          double num4 = num2;
          return (num3, num4);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
        return (0.0, 0.0);
      }
    }
  }
}
