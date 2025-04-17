// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.Maria
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine
{
  public class Maria : IFiscalKkm, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    public string Name => Translate.Maria;

    private object OleDriver { get; set; }

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection, ConnectionSettingsViewModel.PortsConfig.ComOrLan);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Maria.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Maria.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target1 = Maria.\u003C\u003Eo__14.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p1 = Maria.\u003C\u003Eo__14.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Maria.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Maria.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "NullCheck", (IEnumerable<System.Type>) null, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Maria.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) Maria.\u003C\u003Eo__14.\u003C\u003Ep__0, this.OleDriver);
      object obj2 = target1((CallSite) p1, obj1, 1);
      // ISSUE: reference to a compiler-generated field
      if (Maria.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Maria.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = Maria.\u003C\u003Eo__14.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = Maria.\u003C\u003Eo__14.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (Maria.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Maria.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Maria.\u003C\u003Eo__14.\u003C\u003Ep__2.Target((CallSite) Maria.\u003C\u003Eo__14.\u003C\u003Ep__2, obj2, false);
      if (target2((CallSite) p3, obj3))
        throw new InvalidOperationException(Translate.Maria_Не_удалось_открыть_смену_на_ККМ_Maria);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      int num;
      switch (reportType)
      {
        case ReportTypes.ZReport:
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Maria)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target1 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p1 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ZReport", (IEnumerable<System.Type>) null, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) Maria.\u003C\u003Eo__15.\u003C\u003Ep__0, this.OleDriver);
          num = target1((CallSite) p1, obj1);
          break;
        case ReportTypes.XReport:
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__15.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Maria)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target2 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p3 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__15.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "XReport", (IEnumerable<System.Type>) null, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__2.Target((CallSite) Maria.\u003C\u003Eo__15.\u003C\u003Ep__2, this.OleDriver);
          num = target2((CallSite) p3, obj2);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          num = 0;
          break;
      }
      if (num != 1)
      {
        // ISSUE: reference to a compiler-generated field
        if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Maria.\u003C\u003Eo__15.\u003C\u003Ep__9 = CallSite<Func<CallSite, System.Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, Exception> target3 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, Exception>> p9 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__9;
        System.Type type = typeof (Exception);
        // ISSUE: reference to a compiler-generated field
        if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Maria.\u003C\u003Eo__15.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target4 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p8 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__8;
        // ISSUE: reference to a compiler-generated field
        if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Maria.\u003C\u003Eo__15.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string, object> target5 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string, object>> p6 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__6;
        // ISSUE: reference to a compiler-generated field
        if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Maria.\u003C\u003Eo__15.\u003C\u003Ep__5 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, string, object, object> target6 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, string, object, object>> p5 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__5;
        string снятииОтчетаСКкмМария = Translate.Maria_Ошибка_при_снятии_отчета_с_ККМ_Мария__;
        // ISSUE: reference to a compiler-generated field
        if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Maria.\u003C\u003Eo__15.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__4.Target((CallSite) Maria.\u003C\u003Eo__15.\u003C\u003Ep__4, this.OleDriver);
        object obj4 = target6((CallSite) p5, снятииОтчетаСКкмМария, obj3);
        object obj5 = target5((CallSite) p6, obj4, " ");
        // ISSUE: reference to a compiler-generated field
        if (Maria.\u003C\u003Eo__15.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Maria.\u003C\u003Eo__15.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorMessage", typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = Maria.\u003C\u003Eo__15.\u003C\u003Ep__7.Target((CallSite) Maria.\u003C\u003Eo__15.\u003C\u003Ep__7, this.OleDriver);
        object obj7 = target4((CallSite) p8, obj5, obj6);
        throw target3((CallSite) p9, type, obj7);
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      throw new NotImplementedException();
    }

    public bool CloseCheck() => throw new NotImplementedException();

    public void CancelCheck() => throw new NotImplementedException();

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      throw new NotImplementedException();
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      throw new NotImplementedException();
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value)
    {
      throw new NotImplementedException();
    }

    public bool GetCashSum(out Decimal sum) => throw new NotImplementedException();

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      throw new NotImplementedException();
    }

    public bool RegisterPayment(CheckPayment payment) => throw new NotImplementedException();

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      throw new NotImplementedException();
    }

    public bool GetCheckRemainder(out Decimal remainder) => throw new NotImplementedException();

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      this.OleDriver = Functions.CreateObject("M304Manager.Application");
      // ISSUE: reference to a compiler-generated field
      if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Maria.\u003C\u003Eo__27.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Maria.\u003C\u003Eo__27.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__0.Target((CallSite) Maria.\u003C\u003Eo__27.\u003C\u003Ep__0, this.OleDriver, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.Maria_Объект_драйвера_ККМ_Maria_не_был_загружен);
      if (onlyDriverLoad)
      {
        LogHelper.Debug("onlyDriverLoad true");
      }
      else
      {
        if (dc == null)
          dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        int num;
        switch (dc.CheckPrinter.Connection.ConnectionType)
        {
          case GlobalDictionaries.Devices.ConnectionTypes.NotSet:
            throw new InvalidOperationException(Translate.Maria_Не_указан_тип_соединения);
          case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
            LogHelper.Debug("Подключение по ком-порту");
            // ISSUE: reference to a compiler-generated field
            if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Maria.\u003C\u003Eo__27.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Maria)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int> target2 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__3.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int>> p3 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__3;
            // ISSUE: reference to a compiler-generated field
            if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Maria.\u003C\u003Eo__27.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Init", (IEnumerable<System.Type>) null, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj2 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__2.Target((CallSite) Maria.\u003C\u003Eo__27.\u003C\u003Ep__2, this.OleDriver, dc.CheckPrinter.Connection.ComPort.PortName);
            num = target2((CallSite) p3, obj2);
            break;
          case GlobalDictionaries.Devices.ConnectionTypes.Lan:
            LogHelper.Debug("Подключение по LAN");
            string str1 = dc.CheckPrinter.Connection.LanPort.UrlAddress;
            if (str1.ToLower().StartsWith("http://"))
              str1 = str1.Replace("http://", "");
            string str2 = str1 + string.Format(":{0}", (object) dc.CheckPrinter.Connection.LanPort.PortNumber.GetValueOrDefault());
            // ISSUE: reference to a compiler-generated field
            if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Maria.\u003C\u003Eo__27.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Maria)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int> target3 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int>> p5 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__5;
            // ISSUE: reference to a compiler-generated field
            if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Maria.\u003C\u003Eo__27.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Init", (IEnumerable<System.Type>) null, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj3 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__4.Target((CallSite) Maria.\u003C\u003Eo__27.\u003C\u003Ep__4, this.OleDriver, str2);
            num = target3((CallSite) p5, obj3);
            break;
          default:
            LogHelper.Debug("Подключение не распознано");
            throw new InvalidOperationException(Translate.Maria_Подключение_не_распознано);
        }
        if (num == 1)
        {
          LogHelper.Debug("Ошибок нет, подключились");
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__27.\u003C\u003Ep__10 = CallSite<Func<CallSite, System.Type, object, InvalidOperationException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, System.Type, object, InvalidOperationException> target4 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__10.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, System.Type, object, InvalidOperationException>> p10 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__10;
          System.Type type = typeof (InvalidOperationException);
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__27.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string, object> target5 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__9.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string, object>> p9 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__9;
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__27.\u003C\u003Ep__7 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, string, object, object> target6 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, string, object, object>> p7 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__7;
          string подключитьсяККкмMaria = Translate.Maria_Не_удалось_подключиться_к_ККМ_Maria;
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__27.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Maria.\u003C\u003Eo__27.\u003C\u003Ep__6.Target((CallSite) Maria.\u003C\u003Eo__27.\u003C\u003Ep__6, this.OleDriver);
          object obj5 = target6((CallSite) p7, подключитьсяККкмMaria, obj4);
          // ISSUE: reference to a compiler-generated field
          if (Maria.\u003C\u003Eo__27.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Maria.\u003C\u003Eo__27.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorMessage", typeof (Maria), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          string str3 = string.Format(" {0}", Maria.\u003C\u003Eo__27.\u003C\u003Ep__8.Target((CallSite) Maria.\u003C\u003Eo__27.\u003C\u003Ep__8, this.OleDriver));
          object obj6 = target5((CallSite) p9, obj5, str3);
          throw target4((CallSite) p10, type, obj6);
        }
      }
    }

    public bool Disconnect() => throw new NotImplementedException();

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      throw new NotImplementedException();
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => throw new NotImplementedException();

    public bool CutPaper() => throw new NotImplementedException();

    public KkmStatus GetStatus() => throw new NotImplementedException();

    public KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer() => throw new NotImplementedException();

    public bool SendDigitalCheck(string adress) => throw new NotImplementedException();

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
