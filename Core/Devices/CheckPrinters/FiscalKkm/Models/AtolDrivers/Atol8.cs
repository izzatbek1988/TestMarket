// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Atol8
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  internal class Atol8 : IFiscalKkm, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.Atol8_Name_АТОЛ_v_8;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    private object AtolDriver { get; set; }

    private void CheckResult(int? resultCode)
    {
      resultCode.GetValueOrDefault();
      if (!resultCode.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = Atol8.\u003C\u003Eo__10.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p1 = Atol8.\u003C\u003Eo__10.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Atol8.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__10.\u003C\u003Ep__0, this.AtolDriver);
        resultCode = new int?(target((CallSite) p1, obj));
      }
      int? nullable = resultCode;
      int num = 0;
      if (!(nullable.GetValueOrDefault() == num & nullable.HasValue))
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (Atol8)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = Atol8.\u003C\u003Eo__10.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p3 = Atol8.\u003C\u003Eo__10.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultDescription", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Atol8.\u003C\u003Eo__10.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__10.\u003C\u003Ep__2, this.AtolDriver);
        string str = target((CallSite) p3, obj);
        throw new KkmException((IDevice) this, string.Format(Translate.Код___0____Описание___1_, (object) resultCode, (object) str), !resultCode.HasValue || resultCode.GetValueOrDefault() != -3807 ? KkmException.ErrorTypes.Unknown : KkmException.ErrorTypes.NoPaper);
      }
    }

    private KkmLastActionResult GetLastActionResult()
    {
      KkmLastActionResult lastActionResult = new KkmLastActionResult();
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target1 = Atol8.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = Atol8.\u003C\u003Eo__11.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__11.\u003C\u003Ep__0, this.AtolDriver);
      int num = target1((CallSite) p1, obj1);
      if (num == 0)
      {
        lastActionResult.ActionResult = ActionsResults.Done;
        return lastActionResult;
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__11.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__11.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target2 = Atol8.\u003C\u003Eo__11.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p3 = Atol8.\u003C\u003Eo__11.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__11.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__11.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultDescription", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__11.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__11.\u003C\u003Ep__2, this.AtolDriver);
      string str = target2((CallSite) p3, obj2);
      switch (num)
      {
        case -4019:
        case -3933:
        case -3909:
          lastActionResult.ActionResult = ActionsResults.HardwareError;
          goto case -3894;
        case -3894:
          lastActionResult.Message = string.Format(Translate.Atol10_Код_ошибки___0____Описание_ошибки___1_, (object) num, (object) str);
          LogHelper.Error(new Exception("Ошибка в ККМ АТОЛ"), lastActionResult.Message, false);
          return lastActionResult;
        case -3865:
        case -3802:
          lastActionResult.ActionResult = ActionsResults.CheckOpen;
          goto case -3894;
        case -3828:
          lastActionResult.ActionResult = ActionsResults.SessionClosed;
          goto case -3894;
        case -3822:
          lastActionResult.ActionResult = ActionsResults.SessionMore24Hours;
          goto case -3894;
        case -3807:
          lastActionResult.ActionResult = ActionsResults.NoPaper;
          goto case -3894;
        case -16:
          lastActionResult.ActionResult = ActionsResults.ModeNotSupported;
          goto case -3894;
        case -14:
          lastActionResult.ActionResult = ActionsResults.PortBusy;
          goto case -3894;
        case -13:
          lastActionResult.ActionResult = ActionsResults.NeedReinstallDriver;
          goto case -3894;
        case -12:
          lastActionResult.ActionResult = ActionsResults.CommandNotSupported;
          goto case -3894;
        case -11:
          lastActionResult.ActionResult = ActionsResults.DeviceOffline;
          goto case -3894;
        case -3:
        case -1:
          lastActionResult.ActionResult = ActionsResults.NotConnected;
          goto case -3894;
        default:
          lastActionResult.ActionResult = ActionsResults.Unknown;
          goto case -3894;
      }
    }

    private void WriteOfdAttribute(int ofdAttributeNumber, object value)
    {
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
        return;
      LogHelper.Debug(string.Format("Запись отрибута ОФД. Number: {0}; value: {1}", (object) ofdAttributeNumber, value));
      if (value == null || value.ToString().IsNullOrEmpty())
        return;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "AttrNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__12.\u003C\u003Ep__0, this.AtolDriver, ofdAttributeNumber);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "AttrValue", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__12.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__12.\u003C\u003Ep__1, this.AtolDriver, value);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "WriteAttribute", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol8.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__12.\u003C\u003Ep__2, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__12.\u003C\u003Ep__3, this, obj3);
    }

    private KkmStatuses GetKkmState()
    {
      KkmStatuses kkmState = KkmStatuses.Ready;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetStatus", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CoverOpened", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__1, this.AtolDriver);
      if (target1((CallSite) p2, obj1))
        kkmState = KkmStatuses.CoverOpen;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool, object> target3 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool, object>> p4 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CheckPaperPresent", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__3, this.AtolDriver);
      object obj3 = target3((CallSite) p4, obj2, false);
      if (target2((CallSite) p5, obj3))
        kkmState = KkmStatuses.NoPaper;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p7 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PrinterMechanismError", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__6.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__6, this.AtolDriver);
      if (target4((CallSite) p7, obj4))
        kkmState = KkmStatuses.HardwareError;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target5 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p9 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PrinterCutMechanismError", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__8.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__8, this.AtolDriver);
      if (target5((CallSite) p9, obj5))
        kkmState = KkmStatuses.HardwareError;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target6 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p11 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "PrinterOverheatError", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__10.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__10, this.AtolDriver);
      if (target6((CallSite) p11, obj6))
        kkmState = KkmStatuses.HardwareError;
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__13.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "RegisterNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__12.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__12, this.AtolDriver, 44);
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__13.\u003C\u003Ep__13 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetRegister", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__13.\u003C\u003Ep__13.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__13, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__13.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target7 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__16.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p16 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__16;
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__13.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target8 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__15.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p15 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__15;
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__13.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__14.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__14, this.AtolDriver);
        object obj9 = target8((CallSite) p15, obj8, 0);
        if (target7((CallSite) p16, obj9))
        {
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "RegisterNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj10 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__17.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__17, this.AtolDriver, 45);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__18 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetRegister", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__13.\u003C\u003Ep__18.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__18, this.AtolDriver);
          DateTime dateTime;
          ref DateTime local = ref dateTime;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target9 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__20.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p20 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__20;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Year", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__19.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__19, this.AtolDriver);
          int year = target9((CallSite) p20, obj11);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target10 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__22.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p22 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__22;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Month", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj12 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__21.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__21, this.AtolDriver);
          int month = target10((CallSite) p22, obj12);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target11 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__24.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p24 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__24;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Day", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj13 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__23.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__23, this.AtolDriver);
          int day = target11((CallSite) p24, obj13);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__26 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target12 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__26.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p26 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__26;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__25 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Hour", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj14 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__25.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__25, this.AtolDriver);
          int hour = target12((CallSite) p26, obj14);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__28 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target13 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__28.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p28 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__28;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__27 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Minute", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj15 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__27.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__27, this.AtolDriver);
          int minute = target13((CallSite) p28, obj15);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__30 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target14 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__30.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p30 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__30;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__13.\u003C\u003Ep__29 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__13.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Second", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj16 = Atol8.\u003C\u003Eo__13.\u003C\u003Ep__29.Target((CallSite) Atol8.\u003C\u003Eo__13.\u003C\u003Ep__29, this.AtolDriver);
          int second = target14((CallSite) p30, obj16);
          local = new DateTime(year, month, day, hour, minute, second);
          if ((DateTime.Now - dateTime).TotalDays > 25.0)
            kkmState = KkmStatuses.OfdDocumentsToMany;
        }
      }
      return kkmState;
    }

    private bool SessionMore24Hours()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "RegisterNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__0, this.AtolDriver, 17);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetRegister", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__1, this.AtolDriver);
      DateTime dateTime1;
      ref DateTime local1 = ref dateTime1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target1 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p3 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Year", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__2, this.AtolDriver);
      int year1 = target1((CallSite) p3, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target2 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p5 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Month", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__4, this.AtolDriver);
      int month1 = target2((CallSite) p5, obj3);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target3 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p7 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Day", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__6.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__6, this.AtolDriver);
      int day1 = target3((CallSite) p7, obj4);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target4 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p9 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Hour", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__8.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__8, this.AtolDriver);
      int hour1 = target4((CallSite) p9, obj5);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target5 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p11 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Minute", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__10.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__10, this.AtolDriver);
      int minute1 = target5((CallSite) p11, obj6);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target6 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p13 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Second", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__12.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__12, this.AtolDriver);
      int second1 = target6((CallSite) p13, obj7);
      local1 = new DateTime(year1, month1, day1, hour1, minute1, second1);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "RegisterNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__14.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__14, this.AtolDriver, 18);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__15 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetRegister", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__14.\u003C\u003Ep__15.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__15, this.AtolDriver);
      DateTime dateTime2;
      ref DateTime local2 = ref dateTime2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target7 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p17 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__17;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Year", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__16.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__16, this.AtolDriver);
      int year2 = target7((CallSite) p17, obj9);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target8 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p19 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__19;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Month", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__18.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__18, this.AtolDriver);
      int month2 = target8((CallSite) p19, obj10);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target9 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__21.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p21 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__21;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Day", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__20.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__20, this.AtolDriver);
      int day2 = target9((CallSite) p21, obj11);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target10 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__23.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p23 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__23;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Hour", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__22.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__22, this.AtolDriver);
      int hour2 = target10((CallSite) p23, obj12);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target11 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__25.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p25 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__25;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Minute", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj13 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__24.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__24, this.AtolDriver);
      int minute2 = target11((CallSite) p25, obj13);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target12 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__27.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p27 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__27;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__14.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__14.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Second", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = Atol8.\u003C\u003Eo__14.\u003C\u003Ep__26.Target((CallSite) Atol8.\u003C\u003Eo__14.\u003C\u003Ep__26, this.AtolDriver);
      int second2 = target12((CallSite) p27, obj14);
      local2 = new DateTime(year2, month2, day2, hour2, minute2, second2);
      return dateTime1.AddMinutes(5.0) > dateTime2;
    }

    public bool SendDigitalCheck(string address)
    {
      bool flag = !string.IsNullOrEmpty(address) && this.WriteOfdAttribute(OfdAttributes.ClientEmailPhone, (object) address);
      bool sendDigitalCheck = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CheckMode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol8.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__15.\u003C\u003Ep__0, this.AtolDriver, !sendDigitalCheck ? 1 : 0);
      return flag;
    }

    public KkmLastActionResult LasActionResult => this.GetLastActionResult();

    public void ShowProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__18.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__18.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (ShowProperties), (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__18.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__18.\u003C\u003Ep__0, this.AtolDriver);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Mode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__19.\u003C\u003Ep__0, this.AtolDriver, 1);
      this.WriteOfdAttribute(OfdAttributes.CashierName, (object) cashier.Name);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__19.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__19.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Atol8.\u003C\u003Eo__19.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = Atol8.\u003C\u003Eo__19.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__19.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__19.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = Atol8.\u003C\u003Eo__19.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p2 = Atol8.\u003C\u003Eo__19.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__19.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__19.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SetMode", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__19.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__19.\u003C\u003Ep__1, this.AtolDriver);
      object obj3 = target2((CallSite) p2, obj2, 0);
      if (target1((CallSite) p3, obj3))
        throw new InvalidOperationException(Translate.Atol8_Не_удалось_установить_режим_работы_ККМ);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__19.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__19.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (OpenSession), (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol8.\u003C\u003Eo__19.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__19.\u003C\u003Ep__4, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__19.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__19.\u003C\u003Ep__5 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__19.\u003C\u003Ep__5.Target((CallSite) Atol8.\u003C\u003Eo__19.\u003C\u003Ep__5, this, obj4);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      int num1;
      int num2;
      switch (reportType)
      {
        case ReportTypes.ZReport:
          num1 = 3;
          num2 = 1;
          break;
        case ReportTypes.XReport:
          num1 = 2;
          num2 = 2;
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Mode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__20.\u003C\u003Ep__0, this.AtolDriver, num1);
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
        this.WriteOfdAttribute(OfdAttributes.CashierName, (object) cashier.Name);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target1 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p2 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SetMode", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__20.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, this, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportType", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__20.\u003C\u003Ep__3, this.AtolDriver, num2);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__20.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__20.\u003C\u003Ep__5 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target2 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p5 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__20.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__20.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Report", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol8.\u003C\u003Eo__20.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__20.\u003C\u003Ep__4, this.AtolDriver);
      target2((CallSite) p5, this, obj4);
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Mode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__21.\u003C\u003Ep__0, this.AtolDriver, 1);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__21.\u003C\u003Ep__2 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target1 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p2 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SetMode", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__21.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, this, obj2);
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__21.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CheckType", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__21.\u003C\u003Ep__3, this.AtolDriver, 1);
          goto case CheckTypes.Buy;
        case CheckTypes.ReturnSale:
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__21.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CheckType", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__21.\u003C\u003Ep__4, this.AtolDriver, 2);
          goto case CheckTypes.Buy;
        case CheckTypes.Buy:
        case CheckTypes.ReturnBuy:
          Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
          if (devices.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck)
          {
            // ISSUE: reference to a compiler-generated field
            if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__21.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CheckMode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__5.Target((CallSite) Atol8.\u003C\u003Eo__21.\u003C\u003Ep__5, this.AtolDriver, 0);
          }
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__21.\u003C\u003Ep__7 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, Atol8, object> target2 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, Atol8, object>> p7 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__7;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__21.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__21.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (OpenCheck), (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Atol8.\u003C\u003Eo__21.\u003C\u003Ep__6.Target((CallSite) Atol8.\u003C\u003Eo__21.\u003C\u003Ep__6, this.AtolDriver);
          target2((CallSite) p7, this, obj6);
          if (devices.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
          {
            int num1;
            switch (checkData.RuTaxSystem)
            {
              case GlobalDictionaries.RuTaxSystems.None:
                num1 = 0;
                break;
              case GlobalDictionaries.RuTaxSystems.Osn:
                num1 = 1;
                break;
              case GlobalDictionaries.RuTaxSystems.UsnDohod:
                num1 = 2;
                break;
              case GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod:
                num1 = 4;
                break;
              case GlobalDictionaries.RuTaxSystems.Envd:
                num1 = 8;
                break;
              case GlobalDictionaries.RuTaxSystems.Esn:
                num1 = 16;
                break;
              case GlobalDictionaries.RuTaxSystems.Psn:
                num1 = 32;
                break;
              default:
                throw new ArgumentOutOfRangeException();
            }
            int num2 = num1;
            if (num2 > 0)
              this.WriteOfdAttribute(OfdAttributes.TaxSystem, (object) num2);
          }
          if (devices.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
          {
            this.WriteOfdAttribute(OfdAttributes.CashierName, (object) checkData.Cashier.Name);
            if (devices.CheckPrinter.FiscalKkm.FfdVersion > GlobalDictionaries.Devices.FfdVersions.Ffd100)
            {
              this.WriteOfdAttribute(OfdAttributes.CashierInn, (object) checkData.Cashier.Inn);
              if (checkData.Client != null)
              {
                this.WriteOfdAttribute(OfdAttributes.ClientName, (object) checkData.Client.Client.Name);
                string str = checkData.Client.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
                if (!str.IsNullOrEmpty())
                  this.WriteOfdAttribute(OfdAttributes.ClientInn, (object) str);
              }
            }
            this.SendDigitalCheck(checkData.AddressForDigitalCheck);
          }
          return true;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool CloseCheck()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__22.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__22.\u003C\u003Ep__1 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target = Atol8.\u003C\u003Eo__22.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p1 = Atol8.\u003C\u003Eo__22.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (CloseCheck), (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol8.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__22.\u003C\u003Ep__0, this.AtolDriver);
      target((CallSite) p1, this, obj);
      return true;
    }

    public void CancelCheck()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__23.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__23.\u003C\u003Ep__1 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target = Atol8.\u003C\u003Eo__23.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p1 = Atol8.\u003C\u003Eo__23.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__23.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__23.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (CancelCheck), (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol8.\u003C\u003Eo__23.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__23.\u003C\u003Ep__0, this.AtolDriver);
      target((CallSite) p1, this, obj);
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Mode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__24.\u003C\u003Ep__0, this.AtolDriver, 1);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__24.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__24.\u003C\u003Ep__2 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target1 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p2 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__24.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__24.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SetMode", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__24.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, this, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__24.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__24.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__24.\u003C\u003Ep__3, this.AtolDriver, sum);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__24.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__24.\u003C\u003Ep__5 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target2 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p5 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__24.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__24.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CashOutcome", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol8.\u003C\u003Eo__24.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__24.\u003C\u003Ep__4, this.AtolDriver);
      target2((CallSite) p5, this, obj4);
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__25.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__25.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Mode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__25.\u003C\u003Ep__0, this.AtolDriver, 1);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__25.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__25.\u003C\u003Ep__2 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target1 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p2 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__25.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__25.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SetMode", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__25.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, this, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__25.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__25.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__25.\u003C\u003Ep__3, this.AtolDriver, sum);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__25.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__25.\u003C\u003Ep__5 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target2 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p5 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__25.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__25.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CashIncome", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol8.\u003C\u003Eo__25.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__25.\u003C\u003Ep__4, this.AtolDriver);
      target2((CallSite) p5, this, obj4);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value)
    {
      this.WriteOfdAttribute((int) ofdAttribute, value);
      return true;
    }

    public bool GetCashSum(out Decimal sum)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__27.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__27.\u003C\u003Ep__1 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, Atol8, object> target1 = Atol8.\u003C\u003Eo__27.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, Atol8, object>> p1 = Atol8.\u003C\u003Eo__27.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__27.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__27.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetSumm", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__27.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__27.\u003C\u003Ep__0, this.AtolDriver);
      target1((CallSite) p1, this, obj1);
      ref Decimal local = ref sum;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__27.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__27.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Decimal), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target2 = Atol8.\u003C\u003Eo__27.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p3 = Atol8.\u003C\u003Eo__27.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__27.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__27.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Summ", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__27.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__27.\u003C\u003Ep__2, this.AtolDriver);
      Decimal num = target2((CallSite) p3, obj2);
      local = num;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      switch (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion)
      {
        case GlobalDictionaries.Devices.FfdVersions.OfflineKkm:
        case GlobalDictionaries.Devices.FfdVersions.Ffd100:
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Name", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__0, this.AtolDriver, good.Name.Length > 128 ? good.Name.Remove(128) : good.Name);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Price", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__1, this.AtolDriver, good.Price);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Quantity", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__2, this.AtolDriver, good.Quantity);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Department", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__3, this.AtolDriver, good.KkmSectionNumber);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TaxTypeNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__4, this.AtolDriver, good.TaxRateNumber);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DiscountType", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__5.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__5, this.AtolDriver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DiscountValue", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__6.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__6, this.AtolDriver, good.DiscountSum);
          switch (checkType)
          {
            case CheckTypes.Sale:
              // ISSUE: reference to a compiler-generated field
              if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__7 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Atol8.\u003C\u003Eo__28.\u003C\u003Ep__7 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Registration", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__28.\u003C\u003Ep__7.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__7, this.AtolDriver);
              break;
            case CheckTypes.ReturnSale:
              // ISSUE: reference to a compiler-generated field
              if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__8 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Atol8.\u003C\u003Eo__28.\u003C\u003Ep__8 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Return", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__28.\u003C\u003Ep__8.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__8, this.AtolDriver);
              break;
            case CheckTypes.Buy:
            case CheckTypes.ReturnBuy:
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof (checkType), (object) checkType, (string) null);
          }
          break;
        case GlobalDictionaries.Devices.FfdVersions.Ffd105:
        case GlobalDictionaries.Devices.FfdVersions.Ffd110:
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__9 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "BeginItem", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__28.\u003C\u003Ep__9.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__9, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Name", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__10.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__10, this.AtolDriver, good.Name.Length > 128 ? good.Name.Remove(128) : good.Name);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Price", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj9 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__11.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__11, this.AtolDriver, good.Price);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Quantity", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj10 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__12.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__12, this.AtolDriver, good.Quantity);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Department", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__13.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__13, this.AtolDriver, good.KkmSectionNumber);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TaxTypeNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj12 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__14.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__14, this.AtolDriver, good.TaxRateNumber);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj13 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__15.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__15, this.AtolDriver, Math.Round(good.Price * good.Quantity, 2, MidpointRounding.AwayFromZero));
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, GlobalDictionaries.RuFfdGoodsTypes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ItemType", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj14 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__16.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__16, this.AtolDriver, good.RuFfdGoodTypeCode);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, GlobalDictionaries.RuFfdPaymentModes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "PaymentMode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj15 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__17.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__17, this.AtolDriver, good.RuFfdPaymentModeCode);
          if (good.MarkedInfo != null && good.MarkedInfo.Type != GlobalDictionaries.RuMarkedProductionTypes.None && good.MarkedInfo.IsValidCode())
          {
            // ISSUE: reference to a compiler-generated field
            if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__18 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__28.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StreamFormat", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj16 = Atol8.\u003C\u003Eo__28.\u003C\u003Ep__18.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__18, this.AtolDriver, 5);
            if (!good.MarkedInfo.GetHexStringAttribute().IsNullOrEmpty())
              this.WriteOfdAttribute(1162, (object) good.MarkedInfo.GetHexStringAttribute());
          }
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__28.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__28.\u003C\u003Ep__19 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "EndItem", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__28.\u003C\u003Ep__19.Target((CallSite) Atol8.\u003C\u003Eo__28.\u003C\u003Ep__19, this.AtolDriver);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.PrintNonFiscalStrings(good.CommentForFiscalCheck.Select<string, NonFiscalString>((Func<string, NonFiscalString>) (x => new NonFiscalString(x))).ToList<NonFiscalString>());
      return this.LasActionResult.ActionResult == ActionsResults.Done;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        default:
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__29.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__29.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TypeClose", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Atol8.\u003C\u003Eo__29.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__29.\u003C\u003Ep__0, this.AtolDriver, devices.CheckPrinter.FiscalKkm.PaymentsMethods.First<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (x => x.Key == payment.Method)).Value);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__29.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__29.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Atol8.\u003C\u003Eo__29.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__29.\u003C\u003Ep__1, this.AtolDriver, payment.Sum);
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__29.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__29.\u003C\u003Ep__3 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, Atol8, object> target = Atol8.\u003C\u003Eo__29.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, Atol8, object>> p3 = Atol8.\u003C\u003Eo__29.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (Atol8.\u003C\u003Eo__29.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol8.\u003C\u003Eo__29.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Payment", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol8.\u003C\u003Eo__29.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__29.\u003C\u003Ep__2, this.AtolDriver);
          target((CallSite) p3, this, obj3);
          return true;
      }
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__30.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__30.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Destination", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Atol8.\u003C\u003Eo__30.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__30.\u003C\u003Ep__0, this.AtolDriver, 0);
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__30.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__30.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Atol8.\u003C\u003Eo__30.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__30.\u003C\u003Ep__1, this.AtolDriver, sum);
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__30.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__30.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SummDiscount", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__30.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__30.\u003C\u003Ep__2, this.AtolDriver);
      }
      this.PrintNonFiscalStrings(new List<NonFiscalString>()
      {
        new NonFiscalString(string.Format("{0}: {1:N2}", (object) description, (object) sum), TextAlignment.Right)
      });
      return true;
    }

    public bool GetCheckRemainder(out Decimal remainder) => throw new NotImplementedException();

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      this.AtolDriver = Functions.CreateObject("AddIn.FprnM45");
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__32.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__32.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Atol8.\u003C\u003Eo__32.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Atol8.\u003C\u003Eo__32.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__32.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__32.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__32.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__32.\u003C\u003Ep__0, this.AtolDriver, (object) null);
      if (target((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.Atol8_Объект_драйвера_АТОЛ_8_не_был_создан);
      if (onlyDriverLoad)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__32.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__32.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DeviceEnabled", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__32.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__32.\u003C\u003Ep__2, this.AtolDriver, true);
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__33.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__33.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Atol8.\u003C\u003Eo__33.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Atol8.\u003C\u003Eo__33.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__33.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__33.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__33.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__33.\u003C\u003Ep__0, this.AtolDriver, (object) null);
      if (target((CallSite) p1, obj1))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__33.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__33.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DeviceEnabled", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__33.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__33.\u003C\u003Ep__2, this.AtolDriver, false);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__33.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__33.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReleaseComObject", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__33.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__33.\u003C\u003Ep__3, typeof (Marshal), this.AtolDriver);
      this.AtolDriver = (object) null;
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__38.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FontDblHeight", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__0, this.AtolDriver, nonFiscalString.WideFont);
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__38.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FontDblWidth", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__1, this.AtolDriver, nonFiscalString.WideFont);
        switch (nonFiscalString.Alignment)
        {
          case TextAlignment.Left:
            // ISSUE: reference to a compiler-generated field
            if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__38.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Alignment", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj3 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__2, this.AtolDriver, 0);
            break;
          case TextAlignment.Right:
            // ISSUE: reference to a compiler-generated field
            if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__38.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Alignment", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj4 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__3, this.AtolDriver, 2);
            break;
          case TextAlignment.Center:
          case TextAlignment.Justify:
            // ISSUE: reference to a compiler-generated field
            if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__38.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Alignment", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__4, this.AtolDriver, 1);
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol8.\u003C\u003Eo__38.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Alignment", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj6 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__5.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__5, this.AtolDriver, 0);
            break;
        }
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__38.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Caption", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__6.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__6, this.AtolDriver, nonFiscalString.Text);
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__38.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TextWrap", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__7.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__7, this.AtolDriver, 1);
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__38.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p9 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (Atol8.\u003C\u003Eo__38.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol8.\u003C\u003Eo__38.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PrintField", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = Atol8.\u003C\u003Eo__38.\u003C\u003Ep__8.Target((CallSite) Atol8.\u003C\u003Eo__38.\u003C\u003Ep__8, this.AtolDriver);
        if (target((CallSite) p9, obj9) != 0)
          throw new KkmException((IDevice) this, Translate.Atol8_Ошибка_при_печати_нефискальных_строк);
      }
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => throw new NotImplementedException();

    public bool CutPaper()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__40.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__40.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Atol8.\u003C\u003Eo__40.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = Atol8.\u003C\u003Eo__40.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__40.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__40.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = Atol8.\u003C\u003Eo__40.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p1 = Atol8.\u003C\u003Eo__40.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__40.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__40.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FullCut", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__40.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__40.\u003C\u003Ep__0, this.AtolDriver);
      object obj2 = target2((CallSite) p1, obj1, 0);
      return target1((CallSite) p2, obj2);
    }

    public KkmStatus GetStatus()
    {
      KkmStatus status = new KkmStatus()
      {
        KkmState = this.GetKkmState()
      };
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (GetStatus), (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__41.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__1 = CallSite<Action<CallSite, Atol8, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__41.\u003C\u003Ep__1.Target((CallSite) Atol8.\u003C\u003Eo__41.\u003C\u003Ep__1, this, obj1);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (bool), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SessionOpened", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__2.Target((CallSite) Atol8.\u003C\u003Eo__41.\u003C\u003Ep__2, this.AtolDriver);
      status.SessionStatus = !target1((CallSite) p3, obj2) ? SessionStatuses.Close : (this.SessionMore24Hours() ? SessionStatuses.OpenMore24Hours : SessionStatuses.Open);
      KkmStatus kkmStatus1 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p5 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CheckState", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__41.\u003C\u003Ep__4, this.AtolDriver);
      object obj4 = target3((CallSite) p5, obj3, 0);
      int num1 = target2((CallSite) p6, obj4) ? 2 : 1;
      kkmStatus1.CheckStatus = (CheckStatuses) num1;
      KkmStatus kkmStatus2 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__9 = CallSite<Func<CallSite, System.Type, object, Version>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, Version> target4 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, Version>> p9 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__9;
      System.Type type = typeof (Version);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target5 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p8 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Version", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__7.Target((CallSite) Atol8.\u003C\u003Eo__41.\u003C\u003Ep__7, this.AtolDriver);
      object obj6 = target5((CallSite) p8, obj5);
      Version version = target4((CallSite) p9, type, obj6);
      kkmStatus2.DriverVersion = version;
      KkmStatus kkmStatus3 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target6 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p11 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "CheckNumber", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__10.Target((CallSite) Atol8.\u003C\u003Eo__41.\u003C\u003Ep__10, this.AtolDriver);
      int num2 = target6((CallSite) p11, obj7) + 1;
      kkmStatus3.CheckNumber = num2;
      KkmStatus kkmStatus4 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol8)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target7 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p13 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__41.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__41.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Session", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = Atol8.\u003C\u003Eo__41.\u003C\u003Ep__12.Target((CallSite) Atol8.\u003C\u003Eo__41.\u003C\u003Ep__12, this.AtolDriver);
      int num3 = target7((CallSite) p13, obj8) + 1;
      kkmStatus4.SessionNumber = num3;
      return status;
    }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public bool OpenCashDrawer()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__43.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__43.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenDrawer", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__43.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__43.\u003C\u003Ep__0, this.AtolDriver);
      return true;
    }

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
