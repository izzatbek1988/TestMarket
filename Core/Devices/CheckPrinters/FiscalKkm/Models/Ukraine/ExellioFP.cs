// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.ExellioFP
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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine
{
  public class ExellioFP : IFiscalKkm, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => "ExcellioFP";

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    private string PassKkm => "0000";

    private object Driver { get; set; }

    public KkmLastActionResult LasActionResult
    {
      get
      {
        if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__2 == null)
          ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target1 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__2.Target;
        CallSite<Func<CallSite, object, bool>> p2 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__2;
        if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__1 == null)
          ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        Func<CallSite, object, int, object> target2 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__1.Target;
        CallSite<Func<CallSite, object, int, object>> p1 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__1;
        object driver1 = this.Driver;
        object obj1;
        if (driver1 == null)
        {
          obj1 = (object) null;
        }
        else
        {
          if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
            ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastError", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          obj1 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__0, driver1);
        }
        object obj2 = target2((CallSite) p1, obj1, 0);
        if (target1((CallSite) p2, obj2))
          return new KkmLastActionResult()
          {
            ActionResult = ActionsResults.Done
          };
        if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__8 == null)
          ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__8 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Action<CallSite, System.Type, object> target3 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__8.Target;
        CallSite<Action<CallSite, System.Type, object>> p8 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__8;
        System.Type type1 = typeof (LogHelper);
        if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__4 == null)
          ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__4 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, string, object, object> target4 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__4.Target;
        CallSite<Func<CallSite, string, object, object>> p4 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__4;
        object driver2 = this.Driver;
        object obj3;
        if (driver2 == null)
        {
          obj3 = (object) null;
        }
        else
        {
          if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__3 == null)
            ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorText", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          obj3 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__3.Target((CallSite) ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__3, driver2);
        }
        object obj4 = target4((CallSite) p4, "Ошибка при работе с ККМ ExellioFP: ", obj3);
        if (obj4 == null)
        {
          if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__7 == null)
            ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__7 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          Func<CallSite, string, object, object> target5 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__7.Target;
          CallSite<Func<CallSite, string, object, object>> p7 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__7;
          if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__6 == null)
            ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__6 = CallSite<Func<CallSite, System.Type, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          Func<CallSite, System.Type, string, object, object> target6 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__6.Target;
          CallSite<Func<CallSite, System.Type, string, object, object>> p6 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__6;
          System.Type type2 = typeof (string);
          string lasActionResult0 = Translate.ExellioFP_LasActionResult___0_;
          object driver3 = this.Driver;
          object obj5;
          if (driver3 == null)
          {
            obj5 = (object) null;
          }
          else
          {
            if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__5 == null)
              ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastError", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            obj5 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__5.Target((CallSite) ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__5, driver3);
          }
          if (obj5 == null)
            obj5 = (object) "";
          object obj6 = target6((CallSite) p6, type2, lasActionResult0, obj5);
          obj4 = target5((CallSite) p7, "", obj6);
        }
        target3((CallSite) p8, type1, obj4);
        KkmLastActionResult lasActionResult = new KkmLastActionResult();
        KkmLastActionResult lastActionResult = lasActionResult;
        if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__10 == null)
          ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ExellioFP)));
        Func<CallSite, object, string> target7 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__10.Target;
        CallSite<Func<CallSite, object, string>> p10 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__10;
        object driver4 = this.Driver;
        object obj7;
        if (driver4 == null)
        {
          obj7 = (object) null;
        }
        else
        {
          if (ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__9 == null)
            ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorText", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          obj7 = ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__9.Target((CallSite) ExellioFP.\u003C\u003Eo__13.\u003C\u003Ep__9, driver4);
        }
        if (obj7 == null)
          obj7 = (object) "";
        string str = target7((CallSite) p10, obj7);
        lastActionResult.Message = str;
        return lasActionResult;
      }
    }

    public void CheckResult()
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.ExellioFP_Объект_драйвера_Екселио_не_был_создан);
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p4 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p3 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastError", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__2.Target((CallSite) ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__2, this.Driver);
      object obj3 = target3((CallSite) p3, obj2, 0);
      if (!target2((CallSite) p4, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__8 = CallSite<Func<CallSite, System.Type, object, Exception>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, Exception> target4 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, Exception>> p8 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__8;
        System.Type type1 = typeof (Exception);
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Func<CallSite, System.Type, string, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, string, object, object, object> target5 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, string, object, object, object>> p7 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__7;
        System.Type type2 = typeof (string);
        string работеСКкмExellioFp01 = Translate.ExellioFP_Ошибка_при_работе_с_ККМ_ExellioFP___0___1_;
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorText", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__5.Target((CallSite) ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__5, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastError", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__6.Target((CallSite) ExellioFP.\u003C\u003Eo__14.\u003C\u003Ep__6, this.Driver);
        object obj6 = target5((CallSite) p7, type2, работеСКкмExellioFp01, obj4, obj5);
        throw target4((CallSite) p8, type1, obj6);
      }
    }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection, ConnectionSettingsViewModel.PortsConfig.ComOrLan);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ZReport", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__0, this.Driver, this.PassKkm);
          break;
        case ReportTypes.XReport:
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "XReport", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__1.Target((CallSite) ExellioFP.\u003C\u003Eo__17.\u003C\u003Ep__1, this.Driver, this.PassKkm);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
      }
      this.CheckResult();
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          LogHelper.Debug("Открывю чек продажи на ККМ ExellioFP");
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, string, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenFiscalReceipt", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__0, this.Driver, 1, this.PassKkm, 1);
          LogHelper.Debug("Чек продажи открыт успешно ExellioFP");
          break;
        case CheckTypes.ReturnSale:
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, int, string, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenReturnReceipt", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__1.Target((CallSite) ExellioFP.\u003C\u003Eo__18.\u003C\u003Ep__1, this.Driver, 1, this.PassKkm, 1);
          break;
      }
      this.CheckResult();
      return true;
    }

    public bool CloseCheck()
    {
      LogHelper.Debug("Закрываю чек на ExellioFP");
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CloseFiscalReceipt", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__19.\u003C\u003Ep__0, this.Driver);
      this.CheckResult();
      return true;
    }

    public void CancelCheck()
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CancelReceipt", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__20.\u003C\u003Ep__0, this.Driver);
      this.CheckResult();
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InOut", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__21.\u003C\u003Ep__0, this.Driver, -sum);
      this.CheckResult();
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InOut", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__22.\u003C\u003Ep__0, this.Driver, sum);
      this.CheckResult();
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetSumInCash", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__0, this.Driver);
      ref Decimal local = ref sum;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (ExellioFP)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target1 = ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p3 = ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, object> target2 = ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, object>> p2 = ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__2;
      System.Type type = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "s1", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__1.Target((CallSite) ExellioFP.\u003C\u003Eo__24.\u003C\u003Ep__1, this.Driver);
      object obj2 = target2((CallSite) p2, type, obj1);
      Decimal num = target1((CallSite) p3, obj2);
      local = num;
      this.CheckResult();
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      int int32 = Convert.ToInt32(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value ?? (object) 1);
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__25.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__25.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, string, int, int, Decimal, Decimal, int, Decimal, bool, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Sale", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[11]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__25.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__25.\u003C\u003Ep__0, this.Driver, int32, good.Good.Name, good.TaxRateNumber, 1, good.Price, good.Quantity, 0, -good.DiscountSum, true, this.PassKkm);
      this.CheckResult();
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SubTotal", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__0, this.Driver, 0, 0);
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, string, int, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Total", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__1.Target((CallSite) ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__1, this.Driver, "", 1, payment.Sum);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, string, int, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Total", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__2.Target((CallSite) ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__2, this.Driver, "", 4, payment.Sum);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string, int, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Total", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__3.Target((CallSite) ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__3, this.Driver, "", 3, payment.Sum);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, string, int, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Total", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__4.Target((CallSite) ExellioFP.\u003C\u003Eo__26.\u003C\u003Ep__4, this.Driver, "", 2, payment.Sum);
          break;
      }
      this.CheckResult();
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    public bool GetCheckRemainder(out Decimal remainder)
    {
      remainder = 0M;
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      LogHelper.Debug("Подключение ExellioFP");
      this.Driver = Functions.CreateObject("ExellioFP.FiscalPrinter");
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.ExellioFP_Объект_драйвера_Екселио_не_был_создан);
      if (onlyDriverLoad)
      {
        LogHelper.Debug("onlyDriverLoad true");
      }
      else
      {
        if (dc == null)
          dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        switch (dc.CheckPrinter.Connection.ConnectionType)
        {
          case GlobalDictionaries.Devices.ConnectionTypes.NotSet:
            throw new InvalidDataException(Translate.Maria_Не_указан_тип_соединения);
          case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
            LogHelper.Debug("Подключение по ком-порту");
            // ISSUE: reference to a compiler-generated field
            if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, string, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenPort", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__2.Target((CallSite) ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__2, this.Driver, dc.CheckPrinter.Connection.ComPort.PortName, dc.CheckPrinter.Connection.ComPort.Speed);
            break;
          case GlobalDictionaries.Devices.ConnectionTypes.Lan:
            LogHelper.Debug("Подключение по LAN");
            string str1 = dc.CheckPrinter.Connection.LanPort.UrlAddress;
            if (str1.ToLower().StartsWith("http://"))
              str1 = str1.Replace("http://", "");
            // ISSUE: reference to a compiler-generated field
            if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ConnectLan", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__3.Target((CallSite) ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__3, this.Driver, str1, dc.CheckPrinter.Connection.LanPort.PortNumber.ToString());
            break;
          default:
            LogHelper.Debug("Подключение не распознано");
            throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_подключиться_к_ККМ);
        }
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p6 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__6;
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target3 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p5 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastError", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__4.Target((CallSite) ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__4, this.Driver);
        object obj3 = target3((CallSite) p5, obj2, 0);
        if (target2((CallSite) p6, obj3))
        {
          LogHelper.Debug("Ошибок нет, подключились");
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__11 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, System.Type, object> target4 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__11.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, System.Type, object>> p11 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__11;
          System.Type type = typeof (LogHelper);
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string, object> target5 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__10.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string, object>> p10 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__10;
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__8 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, string, object, object> target6 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__8.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, string, object, object>> p8 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__8;
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorText", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__7.Target((CallSite) ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__7, this.Driver);
          object obj5 = target6((CallSite) p8, "Не удалось подключиться к ККМ ExellioFP", obj4);
          // ISSUE: reference to a compiler-generated field
          if (ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastError", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          string str2 = string.Format(" {0}", ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__9.Target((CallSite) ExellioFP.\u003C\u003Eo__29.\u003C\u003Ep__9, this.Driver));
          object obj6 = target5((CallSite) p10, obj5, str2);
          target4((CallSite) p11, type, obj6);
          throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_подключиться_к_ККМ);
        }
      }
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target((CallSite) p1, obj))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ClosePort", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__2.Target((CallSite) ExellioFP.\u003C\u003Eo__30.\u003C\u003Ep__2, this.Driver);
      this.CheckResult();
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenNonfiscalReceipt", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__0, this.Driver);
      nonFiscalStrings.ForEach((Action<NonFiscalString>) (x =>
      {
        // ISSUE: reference to a compiler-generated field
        if (ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "PrintNonfiscalText", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__1.Target((CallSite) ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__1, this.Driver, x.Text);
      }));
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CloseNonfiscalReceipt", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__2.Target((CallSite) ExellioFP.\u003C\u003Eo__35.\u003C\u003Ep__2, this.Driver);
      this.CheckResult();
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => throw new NotImplementedException();

    public bool CutPaper() => true;

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      return new KkmStatus()
      {
        KkmState = KkmStatuses.Ready
      };
    }

    public bool OpenCashDrawer()
    {
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenDrawer", (IEnumerable<System.Type>) null, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__0.Target((CallSite) ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (ExellioFP)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p2 = ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastError", typeof (ExellioFP), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__1.Target((CallSite) ExellioFP.\u003C\u003Eo__40.\u003C\u003Ep__1, this.Driver);
      object obj2 = target2((CallSite) p2, obj1, 0);
      return target1((CallSite) p3, obj2);
    }

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
