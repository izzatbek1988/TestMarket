// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.Sberbank
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class Sberbank : IAcquiringTerminal, IDevice
  {
    private const string PathDriver = "dll\\acquiring\\sberbank\\";

    public void EmergencyCancel() => LogHelper.Debug("Аварийная отмена платежа: не реализовано");

    public Sberbank() => Sberbank.FixFpu();

    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern int _fpreset();

    private static void FixFpu()
    {
      try
      {
        Sberbank._fpreset();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "FixFpu error");
      }
    }

    private static bool DoInTask(Func<bool> action)
    {
      try
      {
        bool r = false;
        Exception ex = (Exception) null;
        Task.Run((Action) (() =>
        {
          try
          {
            r = action();
          }
          catch (Exception ex1)
          {
            LogHelper.WriteError(ex1, "Ошибка выполнения операции на терминале");
            ex = ex1;
          }
        })).Wait();
        if (ex != null)
          throw ex;
        return r;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка выполнения операции по терминалу Сбера");
        return false;
      }
    }

    public bool CloseSession(out string slip)
    {
      try
      {
        Sberbank.FixFpu();
        string s = string.Empty;
        int num = Sberbank.DoInTask((Func<bool>) (() =>
        {
          PilotNt.CloseShift(out s);
          return true;
        })) ? 1 : 0;
        slip = s;
        Sberbank.FixFpu();
        return num != 0;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка закрытия смены на терминале сбербанк");
        slip = string.Empty;
        return false;
      }
    }

    public bool Connect()
    {
      try
      {
        List<string> stringList = new List<string>();
        string[] strArray = new string[3]
        {
          "pilot_nt.dll",
          "sb_kernel.dll",
          "gate.dll"
        };
        foreach (string str in strArray)
        {
          if (!FileSystemHelper.CheckFileExistWithMsg(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\acquiring\\sberbank\\" + str), false))
            stringList.Add(str);
        }
        if (stringList.Any<string>())
        {
          MessageBoxHelper.Warning("В папке с драйвером (dll\\acquiring\\sberbank\\) не найдены файлы, необходимые для работы эквайринга Сбербанк:\r\n\r\n" + string.Join("\n", (IEnumerable<string>) stringList) + "\r\n\r\nПроверьте, установлен ли драйвер, и попробуйте снова");
          return false;
        }
        try
        {
          string path = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\acquiring\\sberbank\\" + Guid.NewGuid().ToString() + ".tmp");
          File.WriteAllText(path, "temp");
          File.Delete(path);
        }
        catch (Exception ex)
        {
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Недостаточно прав доступа к папке с драйвером эквайринг-терминала Сбербанк. При выполнении операции могут возникнуть ошибки.", ProgressBarViewModel.Notification.NotificationsTypes.Error));
          LogHelper.WriteError(ex, "Не прошли проверку прав доступа для терминала Сбера");
        }
        LogHelper.Debug("Подключение успешно");
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось подключиться с терминалу Сбербанк");
        return false;
      }
    }

    public bool Disconnect()
    {
      Sberbank.FixFpu();
      return true;
    }

    public void ShowServiceMenu(out string slip)
    {
      slip = string.Empty;
      try
      {
        string path1 = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\acquiring\\sberbank\\");
        string pathToFile = Path.Combine(path1, "LoadParm.exe");
        if (!FileSystemHelper.CheckFileExistWithMsg(pathToFile))
          return;
        Process process = new Process();
        process.StartInfo.FileName = pathToFile;
        process.Start();
        process.WaitForExit();
        string path = Path.Combine(path1, "p");
        if (!File.Exists(path))
          return;
        byte[] bytes = File.ReadAllBytes(path);
        slip = Encoding.GetEncoding(866).GetString(bytes);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка открытия сервисного меню");
      }
    }

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      method = string.Empty;
      try
      {
        string s = string.Empty;
        string rn = string.Empty;
        Sberbank.FixFpu();
        int num = Sberbank.DoInTask((Func<bool>) (() =>
        {
          AuthAnswer13 authAnswer13 = PilotNt.Authorization(sum, out s);
          rn = authAnswer13.RRN;
          LogHelper.Debug("Списание произвели. RRN:" + authAnswer13.RRN + ". CardNumber:" + authAnswer13.CardID);
          PilotNt.CommitTransaction(sum);
          return true;
        })) ? 1 : 0;
        Sberbank.FixFpu();
        slip = s;
        rrn = rn;
        return num != 0;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка проведения платежа на терминале Сбербанк");
        Sberbank.DoInTask((Func<bool>) (() =>
        {
          PilotNt.RollbackTransaction(sum);
          return true;
        }));
        rrn = string.Empty;
        slip = string.Empty;
        return false;
      }
    }

    public bool GetReport(out string slip)
    {
      try
      {
        string s = string.Empty;
        Sberbank.FixFpu();
        int num = Sberbank.DoInTask((Func<bool>) (() =>
        {
          PilotNt.GetStatistics(out s);
          return true;
        })) ? 1 : 0;
        Sberbank.FixFpu();
        slip = s;
        return num != 0;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка снятия отчета на терминале Сбербанк");
        slip = string.Empty;
        return false;
      }
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      try
      {
        string s = string.Empty;
        Sberbank.FixFpu();
        int num = Sberbank.DoInTask((Func<bool>) (() =>
        {
          AuthAnswer13 authAnswer13 = PilotNt.AuthorizationReturn(sum, out s, rrn);
          LogHelper.Debug("Возврат произвели. RRN:" + authAnswer13.RRN + ". CardNumber:" + authAnswer13.CardID);
          PilotNt.CommitTransaction(sum);
          return true;
        })) ? 1 : 0;
        slip = s;
        Sberbank.FixFpu();
        return num != 0;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка проведения возврата на терминале Сбербанк");
        Sberbank.DoInTask((Func<bool>) (() =>
        {
          PilotNt.RollbackTransaction(sum);
          return true;
        }));
        slip = string.Empty;
        return false;
      }
    }

    public void ShowProperties()
    {
      int num = (int) MessageBoxHelper.Show(Translate.Sberbank_Настраивать_параметры_подключения_к_терминалу_Сбербанка_необходимо_в_файле_pinpad_ini);
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => Translate.Sberbank_Сбербанк;
  }
}
