// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.Sbrf
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class Sbrf : IAcquiringTerminal, IDevice
  {
    private object Driver;

    private static List<string> CheckSeparator { get; set; } = new List<string>()
    {
      "~S",
      "\u0001"
    };

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => "Сбербанк (SBRF)";

    public void ShowProperties()
    {
      int num = (int) MessageBoxHelper.Show(Translate.Sberbank_Настраивать_параметры_подключения_к_терминалу_Сбербанка_необходимо_в_файле_pinpad_ini);
    }

    public void ShowServiceMenu(out string slip)
    {
      if (!Directory.Exists("C:\\sc552"))
      {
        MessageBoxHelper.Warning("Не удалось найти папку C:\\sc552 с драйвером для терминала Сбера.");
        slip = "";
      }
      else
      {
        string pathToFile = Path.Combine("C:\\sc552", "LoadParm.exe");
        if (!FileSystemHelper.CheckFileExistWithMsg(pathToFile))
        {
          slip = "";
        }
        else
        {
          try
          {
            Process process = new Process();
            process.StartInfo.FileName = pathToFile;
            process.Start();
            process.WaitForExit();
            string path = Path.Combine("C:\\sc552", "p");
            if (!File.Exists(path))
            {
              slip = "";
            }
            else
            {
              byte[] bytes = File.ReadAllBytes(path);
              slip = Encoding.GetEncoding(866).GetString(bytes);
            }
          }
          catch (Exception ex)
          {
            slip = "";
            LogHelper.Error(ex, "Ошибка открытия сервисного меню");
          }
        }
      }
    }

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      rrn = "";
      method = "";
      bool flag = this.DoOperation(sum, out slip, "", Sbrf.EFunc.Purchase);
      ref string local = ref rrn;
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Sbrf)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target = Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p1 = Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GParamString", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__10.\u003C\u003Ep__0, this.Driver, "RRN");
      string str = target((CallSite) p1, obj);
      local = str;
      LogHelper.Debug("После оплаты получили RRN = " + rrn);
      return flag;
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      return this.DoOperation(sum, out slip, rrn, Sbrf.EFunc.Refund);
    }

    private bool DoOperation(Decimal sum, out string slip, string rrn, Sbrf.EFunc typeOperation)
    {
      try
      {
        int int32 = Convert.ToInt32(sum * 100M);
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Clear", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__0, this.Driver);
        this.Amount(int32);
        if (!rrn.IsNullOrEmpty())
        {
          // ISSUE: reference to a compiler-generated field
          if (Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, string, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SParam", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__1.Target((CallSite) Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__1, this.Driver, "RRN", rrn);
        }
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Sbrf)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target1 = Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p3 = Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "NFun", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__2, this.Driver, (int) typeOperation);
        this.CheckError(target1((CallSite) p3, obj1));
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Sbrf)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target2 = Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p5 = Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "NFun", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__4.Target((CallSite) Sbrf.\u003C\u003Eo__12.\u003C\u003Ep__4, this.Driver, 6003);
        int resultCode = target2((CallSite) p5, obj2);
        LogHelper.Debug("Перевод транзакции в не подтвержденнное состояние.");
        this.CheckError(resultCode);
        slip = this.GetSlip();
        if (resultCode == 4141 && slip.ToLower().Contains("одобрено"))
        {
          LogHelper.Debug("Проводим операцию сразу без транзакции");
          return true;
        }
        LogHelper.Debug(slip);
        this.CheckTransaction();
        return true;
      }
      catch (Exception ex)
      {
        slip = "";
        LogHelper.Error(ex, "Ошибка проведения операции по терминалу. Обратитесь в службу поддержки.");
        return false;
      }
    }

    public bool GetReport(out string slip)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Clear", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__0, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Sbrf)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p2 = Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "NFun", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__1.Target((CallSite) Sbrf.\u003C\u003Eo__13.\u003C\u003Ep__1, this.Driver, 7004);
        this.CheckError(target((CallSite) p2, obj));
        slip = this.GetSlip();
        return true;
      }
      catch (Exception ex)
      {
        slip = "";
        LogHelper.Error(ex, "Ошибка снятия краткого отчёта по терминалу. Обратитесь в техническую поддержку.");
        return false;
      }
    }

    public bool CloseSession(out string slip)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Clear", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__0, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Sbrf)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p2 = Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "NFun", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) Sbrf.\u003C\u003Eo__14.\u003C\u003Ep__1, this.Driver, 6000);
        this.CheckError(target((CallSite) p2, obj));
        slip = this.GetSlip();
        return true;
      }
      catch (Exception ex)
      {
        slip = "";
        LogHelper.Error(ex, "Ошибка сверки итогов по терминалу. Обратитесь в техническую поддержку.");
        return false;
      }
    }

    public bool Connect()
    {
      try
      {
        this.Driver = Functions.CreateObject("SBRFSRV.Server");
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target = Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__15.\u003C\u003Ep__0, this.Driver, (object) null);
        if (target((CallSite) p1, obj))
          throw new NullReferenceException("Не удалось создать бъект драйвера SBRFSRV, возможно библиотека не зарегестрирована в системе");
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Сбербанк. Возникла не опознанная ошибка. Продолжить работу с устройством не возможно.");
        return false;
      }
    }

    public bool Disconnect() => true;

    public void EmergencyCancel() => throw new NotImplementedException();

    private void CheckError(int resultCode)
    {
      switch ((Sbrf.EFunc) resultCode)
      {
        case Sbrf.EFunc.NotError:
          break;
        case Sbrf.EFunc.NotConn:
          throw new Exception(string.Format("Отсутствует соединение [{0}] с терминалом. Обратитесь в техническую поддержку.", (object) resultCode));
        case Sbrf.EFunc.CancelPay:
          throw new Exception(string.Format("Отмена [{0}] проведения операции по терминалу.", (object) resultCode));
        case Sbrf.EFunc.NotUseTransaction:
          LogHelper.Debug("Сообщение с кодом 4141 от терминала проигнорировано");
          break;
        case Sbrf.EFunc.NotCorrectDepartment:
          throw new Exception("Номер отдела превышает количество настроенных отделов в терминале");
        default:
          throw new Exception(string.Format("Ошибка [{0}] выполнения действия на терминале.  Обратитесь в техническую поддержку.", (object) resultCode));
      }
    }

    public bool CheckTransaction(bool isSuccessfully = true)
    {
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target((CallSite) p1, obj1))
        return false;
      int num = isSuccessfully ? 6001 : 6004;
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "NFun", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__2.Target((CallSite) Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__2, this.Driver, num);
      LogHelper.Debug(isSuccessfully ? "Подтверждение транзакции." : "Отклонение транзакции.");
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__3 = CallSite<Action<CallSite, Sbrf, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckError", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__3.Target((CallSite) Sbrf.\u003C\u003Eo__19.\u003C\u003Ep__3, this, obj2);
      return true;
    }

    public string GetSlip()
    {
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GParamString", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__0, this.Driver, "Cheque");
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__1.Target((CallSite) Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__1, obj1, "~S");
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target2 = Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p4 = Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string[], StringSplitOptions, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Split", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__3.Target((CallSite) Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__3, obj1, Sbrf.CheckSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        obj1 = target2((CallSite) p4, obj3, 0);
      }
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Sbrf)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__5.Target((CallSite) Sbrf.\u003C\u003Eo__20.\u003C\u003Ep__5, obj1);
    }

    private void Amount(int total)
    {
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SParam", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Sbrf.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__21.\u003C\u003Ep__0, this.Driver, nameof (Amount), total);
    }

    private void SetDepartment(int DepartmentPOS)
    {
      // ISSUE: reference to a compiler-generated field
      if (Sbrf.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Sbrf.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SParam", (IEnumerable<System.Type>) null, typeof (Sbrf), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Sbrf.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) Sbrf.\u003C\u003Eo__22.\u003C\u003Ep__0, this.Driver, "Department", DepartmentPOS);
    }

    private enum EFunc
    {
      NotError = 0,
      NotConn = 99, // 0x00000063
      CancelPay = 2000, // 0x000007D0
      Purchase = 4000, // 0x00000FA0
      Refund = 4002, // 0x00000FA2
      RefundWithCancel = 4003, // 0x00000FA3
      NotUseTransaction = 4141, // 0x0000102D
      NotCorrectDepartment = 4191, // 0x0000105F
      ConfirmTransaction = 6001, // 0x00001771
      WaitTransaction = 6003, // 0x00001773
      CancelTransaction = 6004, // 0x00001774
    }
  }
}
