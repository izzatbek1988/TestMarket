// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Atol10
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class Atol10 : IOnlineKkm, IFiscalKkm, IDevice
  {
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _checkData;
    private byte[] _industryInfo;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.Atol10_Name_АТОЛ_v_10;

    private object AtolDriver { get; set; }

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public Atol10(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    private void CheckResult()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target1 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__12.\u003C\u003Ep__0, this.AtolDriver);
      int num = target1((CallSite) p1, obj1);
      switch (num)
      {
        case 0:
          return;
        case 177:
          LogHelper.Debug("Ошибка 177. Пытаемся допечатать документ");
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "continuePrint", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__12.\u003C\u003Ep__2, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target2 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p4 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__4;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__12.\u003C\u003Ep__3, this.AtolDriver);
          num = target2((CallSite) p4, obj2);
          LogHelper.Debug("Результат допечатования: " + num.ToString());
          if (num == 0)
            return;
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__12.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__12.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target3 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p6 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorDescription", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__12.\u003C\u003Ep__5.Target((CallSite) Atol10.\u003C\u003Eo__12.\u003C\u003Ep__5, this.AtolDriver);
      string str1 = target3((CallSite) p6, obj3);
      string str2 = string.Format(Translate.Код___0____Описание___1_, (object) num, (object) str1);
      KkmException.ErrorTypes errorTypes;
      switch (num)
      {
        case 1:
          errorTypes = KkmException.ErrorTypes.NoConnection;
          break;
        case 2:
          errorTypes = KkmException.ErrorTypes.NoConnection;
          break;
        case 3:
          errorTypes = KkmException.ErrorTypes.PortBusy;
          break;
        case 4:
          errorTypes = KkmException.ErrorTypes.NoConnection;
          break;
        case 5:
          errorTypes = KkmException.ErrorTypes.NonCorrectData;
          break;
        case 35:
          errorTypes = KkmException.ErrorTypes.UnCorrectDateTime;
          break;
        case 44:
          errorTypes = KkmException.ErrorTypes.NoPaper;
          break;
        case 45:
          errorTypes = KkmException.ErrorTypes.NoPaper;
          break;
        case 47:
          errorTypes = KkmException.ErrorTypes.NeedService;
          break;
        case 68:
          errorTypes = KkmException.ErrorTypes.SessionMore24Hour;
          break;
        case 137:
          errorTypes = KkmException.ErrorTypes.TooManyOfflineDocuments;
          break;
        default:
          errorTypes = KkmException.ErrorTypes.Unknown;
          break;
      }
      KkmException.ErrorTypes key = errorTypes;
      throw new ErrorHelper.GbsException(KkmException.ErrorsDictionary[key] + "\n" + str2)
      {
        Direction = key == KkmException.ErrorTypes.Unknown ? ErrorHelper.ErrorDirections.Unknown : ErrorHelper.ErrorDirections.Outer
      };
    }

    private void WriteOfdAttribute(int ofdAttributeNumber, object value)
    {
      if (value == null || value.ToString().IsNullOrEmpty())
        return;
      LogHelper.Debug(string.Format("Запись атрибута: number: {0}; value: {1}", (object) ofdAttributeNumber, value));
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__13.\u003C\u003Ep__0, this.AtolDriver, ofdAttributeNumber, value);
      this.CheckResult();
    }

    private KkmStatuses GetKkmState()
    {
      KkmStatuses kkmState = KkmStatuses.Ready;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__2;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_SHORT_STATUS", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, atolDriver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__14.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__3, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamBool", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target3 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__5;
      object atolDriver2 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_COVER_OPENED", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__4, this.AtolDriver);
      object obj4 = target3((CallSite) p5, atolDriver2, obj3);
      if (target2((CallSite) p6, obj4))
        kkmState = KkmStatuses.CoverOpen;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p10 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool, object> target5 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool, object>> p9 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamBool", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p8 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__8;
      object atolDriver3 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_PAPER_PRESENT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__7, this.AtolDriver);
      object obj6 = target6((CallSite) p8, atolDriver3, obj5);
      object obj7 = target5((CallSite) p9, obj6, false);
      if (target4((CallSite) p10, obj7))
        kkmState = KkmStatuses.NoPaper;
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__13 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, object> target7 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__13.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, object>> p13 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__13;
        object atolDriver4 = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__11.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__11, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_OFD_EXCHANGE_STATUS", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__12.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__12, this.AtolDriver);
        target7((CallSite) p13, atolDriver4, obj8, obj9);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__14 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "fnQueryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__14.\u003C\u003Ep__14.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__14, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target8 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__16.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p16 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__16;
        object atolDriver5 = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DOCUMENTS_COUNT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__15.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__15, this.AtolDriver);
        object obj11 = target8((CallSite) p16, atolDriver5, obj10);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target9 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__18.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p18 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__18;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj12 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__17.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__17, obj11, 0);
        if (target9((CallSite) p18, obj12))
        {
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__14.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target10 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__24.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p24 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__24;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__14.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int, object> target11 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__23.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int, object>> p23 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__23;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__14.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TotalDays", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target12 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__22.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p22 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__22;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__14.\u003C\u003Ep__21 = CallSite<Func<CallSite, DateTime, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Subtract, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, DateTime, object, object> target13 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__21.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, DateTime, object, object>> p21 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__21;
          DateTime now = DateTime.Now;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__14.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target14 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__20.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p20 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__20;
          object atolDriver6 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__14.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj13 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__19.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__19, this.AtolDriver);
          object obj14 = target14((CallSite) p20, atolDriver6, obj13);
          object obj15 = target13((CallSite) p21, now, obj14);
          object obj16 = target12((CallSite) p22, obj15);
          object obj17 = target11((CallSite) p23, obj16, 25);
          if (target10((CallSite) p24, obj17))
            kkmState = KkmStatuses.OfdDocumentsToMany;
        }
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__27 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__27 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, System.Type, object> target15 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__27.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, System.Type, object>> p27 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__27;
        System.Type type = typeof (LogHelper);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__26 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string, object> target16 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__26.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string, object>> p26 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__26;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__14.\u003C\u003Ep__25 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__14.\u003C\u003Ep__25 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj18 = Atol10.\u003C\u003Eo__14.\u003C\u003Ep__25.Target((CallSite) Atol10.\u003C\u003Eo__14.\u003C\u003Ep__25, "В ОФД неотправлено ", obj11);
        object obj19 = target16((CallSite) p26, obj18, " документов");
        target15((CallSite) p27, type, obj19);
      }
      return kkmState;
    }

    public bool SendDigitalCheck(string address)
    {
      bool flag = !string.IsNullOrEmpty(address) && this.WriteOfdAttribute(OfdAttributes.ClientEmailPhone, (object) address);
      if (flag && this.DevicesConfig.CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target = Atol10.\u003C\u003Eo__15.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p1 = Atol10.\u003C\u003Eo__15.\u003C\u003Ep__1;
        object atolDriver = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_ELECTRONICALLY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Atol10.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__15.\u003C\u003Ep__0, this.AtolDriver);
        target((CallSite) p1, atolDriver, obj, true);
      }
      return flag;
    }

    public KkmLastActionResult LasActionResult
    {
      get
      {
        return new KkmLastActionResult()
        {
          ActionResult = ActionsResults.Done
        };
      }
    }

    public bool IsCanHoldConnection => true;

    public void ShowProperties()
    {
      LogHelper.Debug("Открываем настройки ККМ Атол");
      int num1;
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, Version>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, Version> target1 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, Version>> p2 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__2;
        System.Type type = typeof (Version);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target2 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p1 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "version", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__20.\u003C\u003Ep__0, this.AtolDriver);
        object obj2 = target2((CallSite) p1, obj1);
        if (target1((CallSite) p2, type, obj2) > new Version(10, 7))
        {
          IntPtr num2 = IntPtr.Zero;
          using (IEnumerator<FrmSettings> enumerator = Application.Current.Windows.OfType<FrmSettings>().GetEnumerator())
          {
            if (enumerator.MoveNext())
              num2 = new WindowInteropHelper((Window) enumerator.Current).Handle;
          }
          LogHelper.Debug("открываем с объектом окна");
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__20.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Atol10)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target3 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p5 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__5;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__20.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object, IntPtr, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "showProperties", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, IntPtr, object> target4 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, IntPtr, object>> p4 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__4;
          object atolDriver = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_GUI_PARENT_NATIVE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__20.\u003C\u003Ep__3, this.AtolDriver);
          IntPtr num3 = num2;
          object obj4 = target4((CallSite) p4, atolDriver, obj3, num3);
          num1 = target3((CallSite) p5, obj4);
        }
        else
        {
          LogHelper.Debug("открываем без обхекта окна");
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__20.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Atol10)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target5 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p7 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__7;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__20.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "showProperties", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__6.Target((CallSite) Atol10.\u003C\u003Eo__20.\u003C\u003Ep__6, this.AtolDriver, (object) null, (object) null);
          num1 = target5((CallSite) p7, obj5);
        }
      }
      catch
      {
        LogHelper.Debug("открываем без объекта, была ошибка");
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__20.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Atol10)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p9 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__20.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "showProperties", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__8.Target((CallSite) Atol10.\u003C\u003Eo__20.\u003C\u003Ep__8, this.AtolDriver, (object) null, (object) null);
        num1 = target((CallSite) p9, obj);
      }
      Gbs.Core.Config.FiscalKkm fiscalKkm = this.DevicesConfig.CheckPrinter.FiscalKkm;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__20.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target6 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p11 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__11;
      if (num1 == -1)
        throw new ErrorHelper.GbsException(Translate.Atol10_Не_удалось_открыть_окно_настроек_драйвера);
      object obj6;
      if (num1 == 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__20.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__20.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getSettings", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        obj6 = Atol10.\u003C\u003Eo__20.\u003C\u003Ep__10.Target((CallSite) Atol10.\u003C\u003Eo__20.\u003C\u003Ep__10, this.AtolDriver);
      }
      else
        obj6 = (object) this.DevicesConfig.CheckPrinter.FiscalKkm.Atol10ConnectionConfig;
      fiscalKkm.Atol10ConnectionConfig = target6((CallSite) p11, obj6);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.OperatorLogin(cashier);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "openShift", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__21.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__21.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__21.\u003C\u003Ep__1, this.AtolDriver);
      this.CheckResult();
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      switch (reportType)
      {
        case ReportTypes.ZReport:
          if (devices.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
            this.OperatorLogin(cashier);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__22.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__2;
          object atolDriver1 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_REPORT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__22.\u003C\u003Ep__0, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__22.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_CLOSE_SHIFT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__22.\u003C\u003Ep__1, this.AtolDriver);
          target1((CallSite) p2, atolDriver1, obj1, obj2);
          break;
        case ReportTypes.XReport:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__22.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__5;
          object atolDriver2 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__22.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_REPORT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__22.\u003C\u003Ep__3, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__22.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_X", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Atol10.\u003C\u003Eo__22.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__22.\u003C\u003Ep__4, this.AtolDriver);
          target2((CallSite) p5, atolDriver2, obj3, obj4);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__22.\u003C\u003Ep__6 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "report", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__22.\u003C\u003Ep__6.Target((CallSite) Atol10.\u003C\u003Eo__22.\u003C\u003Ep__6, this.AtolDriver);
      this.CheckResult();
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__22.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__22.\u003C\u003Ep__7 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__22.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__22.\u003C\u003Ep__7, this.AtolDriver);
      this.CheckResult();
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._checkData = checkData;
      LogHelper.Debug("Открытие чека АТОЛ 10");
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (devices.CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
        this.Ffd120CodeValidation(checkData);
      if (devices.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        this.OperatorLogin(checkData.Cashier);
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
        if (checkData.Client != null)
        {
          if (devices.CheckPrinter.FiscalKkm.FfdVersion.IsEither<GlobalDictionaries.Devices.FfdVersions>(GlobalDictionaries.Devices.FfdVersions.Ffd105, GlobalDictionaries.Devices.FfdVersions.Ffd110))
          {
            this.WriteOfdAttribute(OfdAttributes.ClientName, (object) checkData.Client.Client.Name);
            string str = checkData.Client.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
            if (!str.IsNullOrEmpty())
              this.WriteOfdAttribute(OfdAttributes.ClientInn, (object) str);
          }
          if (devices.CheckPrinter.FiscalKkm.FfdVersion.IsEither<GlobalDictionaries.Devices.FfdVersions>(GlobalDictionaries.Devices.FfdVersions.Ffd120))
          {
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "utilFormTlv", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__0, this.AtolDriver);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__1, this.AtolDriver, 1227, checkData.Client.Client.Name);
            string str = checkData.Client.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
            if (!str.IsNullOrEmpty())
            {
              // ISSUE: reference to a compiler-generated field
              if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__2 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Atol10.\u003C\u003Eo__24.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__2, this.AtolDriver, 1228, str);
            }
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "utilFormTlv", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__3, this.AtolDriver);
            Thread.Sleep(1000);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, byte[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (byte[]), typeof (Atol10)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, byte[]> target1 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__6.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, byte[]>> p6 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__6;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamByteArray", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__5;
            object atolDriver = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAG_VALUE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj1 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__4, this.AtolDriver);
            object obj2 = target2((CallSite) p5, atolDriver, obj1);
            byte[] numArray = target1((CallSite) p6, obj2);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__24.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, int, byte[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__7, this.AtolDriver, 1256, numArray);
          }
        }
        this.SendDigitalCheck(checkData.AddressForDigitalCheck);
      }
      object obj3;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_SELL", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__8.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__8, this.AtolDriver);
          break;
        case CheckTypes.ReturnSale:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_SELL_RETURN", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__9, this.AtolDriver);
          break;
        case CheckTypes.Buy:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_BUY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__10.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__10, this.AtolDriver);
          break;
        case CheckTypes.ReturnBuy:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__24.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_BUY_RETURN", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__11.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__11, this.AtolDriver);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      object obj4 = obj3;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__24.\u003C\u003Ep__13 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target3 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p13 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__13;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__24.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__12.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__12, this.AtolDriver);
      object obj6 = obj4;
      target3((CallSite) p13, atolDriver1, obj5, obj6);
      if (this.DevicesConfig.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck)
      {
        LogHelper.Debug("Выключаем печать бумажного чека АТОЛ");
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__24.\u003C\u003Ep__15 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target4 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__15.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p15 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__15;
        object atolDriver2 = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__24.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_ELECTRONICALLY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = Atol10.\u003C\u003Eo__24.\u003C\u003Ep__14.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__14, this.AtolDriver);
        target4((CallSite) p15, atolDriver2, obj7, true);
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__24.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__24.\u003C\u003Ep__16 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "openReceipt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__24.\u003C\u003Ep__16.Target((CallSite) Atol10.\u003C\u003Eo__24.\u003C\u003Ep__16, this.AtolDriver);
      this.CheckResult();
      this.GetSalesNotice(checkData.TrueApiInfoForKkm);
      return true;
    }

    private void OperatorLogin(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.WriteOfdAttribute(OfdAttributes.CashierName, (object) cashier.Name);
      if (devices.CheckPrinter.FiscalKkm.FfdVersion > GlobalDictionaries.Devices.FfdVersions.Ffd100)
        this.WriteOfdAttribute(OfdAttributes.CashierInn, (object) cashier.Inn);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__25.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__25.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "operatorLogin", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__25.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__25.\u003C\u003Ep__0, this.AtolDriver);
      this.CheckResult();
    }

    [HandleProcessCorruptedStateExceptions]
    private void Ffd120CodeValidation(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      LogHelper.Debug("Начинаю валидацию КМ для ФФД 120, АТОЛ 10");
      try
      {
        if (checkData.GoodsList.All<CheckGood>((Func<CheckGood, bool>) (x =>
        {
          MarkedInfo markedInfo = x.MarkedInfo;
          return (markedInfo != null ? (int) markedInfo.Type : 0) == 0;
        })))
          return;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__26.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cancelMarkingCodeValidation", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__26.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__0, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__26.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "clearMarkingCodeValidationResult", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__26.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__1, this.AtolDriver);
        foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (item => item.MarkedInfo != null && !item.MarkedInfo.FullCode.IsNullOrEmpty())))
        {
          checkGood.MarkedInfo.ValidationResultKkm = (object) null;
          string fnC1 = DataMatrixHelper.ReplaceSomeCharsToFNC1(checkGood.MarkedInfo.FullCode);
          int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(checkGood, checkData.CheckType);
          LogHelper.Debug("Валидация кода: " + fnC1);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p4 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__4;
          object atolDriver1 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__2, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_MCT12_AUTO", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__3, this.AtolDriver);
          target1((CallSite) p4, atolDriver1, obj1, obj2);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__6 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, string> target2 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__6.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, string>> p6 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__6;
          object atolDriver2 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__5.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__5, this.AtolDriver);
          string str = fnC1;
          target2((CallSite) p6, atolDriver2, obj3, str);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, int> target3 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__8.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, int>> p8 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__8;
          object atolDriver3 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_STATUS", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__7, this.AtolDriver);
          int num = markingCodeStatus;
          target3((CallSite) p8, atolDriver3, obj4, num);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, bool> target4 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__10.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, bool>> p10 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__10;
          object atolDriver4 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_WAIT_FOR_VALIDATION_RESULT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__9, this.AtolDriver);
          target4((CallSite) p10, atolDriver4, obj5, true);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__12 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, int> target5 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__12.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, int>> p12 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__12;
          object atolDriver5 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_PROCESSING_MODE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__11.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__11, this.AtolDriver);
          target5((CallSite) p12, atolDriver5, obj6, 0);
          if (markingCodeStatus.IsEither<int>(2, 4))
          {
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, double> target6 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__14.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, double>> p14 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__14;
            object atolDriver6 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__13 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_QUANTITY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__13.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__13, this.AtolDriver);
            double quantity = (double) checkGood.Quantity;
            target6((CallSite) p14, atolDriver6, obj7, quantity);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__16 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__16 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target7 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__16.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p16 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__16;
            object atolDriver7 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__15 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MEASUREMENT_UNIT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj8 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__15.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__15, this.AtolDriver);
            int ruFfdUnitsIndex1 = checkGood.Unit.RuFfdUnitsIndex;
            target7((CallSite) p16, atolDriver7, obj8, ruFfdUnitsIndex1);
            int ruFfdUnitsIndex2 = checkGood.Unit.RuFfdUnitsIndex;
          }
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__17 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "beginMarkingCodeValidation", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__26.\u003C\u003Ep__17.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__17, this.AtolDriver);
          bool flag = false;
          for (int index = 0; index < 30; ++index)
          {
            LogHelper.Debug("Валидация кода продолэается, круг " + index.ToString());
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__18 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__18 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "getMarkingCodeValidationStatus", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__18.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__18, this.AtolDriver);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__21 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target8 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__21.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p21 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__21;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__20 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamBool", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target9 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__20.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p20 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__20;
            object atolDriver8 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__19 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_VALIDATION_READY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj9 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__19.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__19, this.AtolDriver);
            object obj10 = target9((CallSite) p20, atolDriver8, obj9);
            if (target8((CallSite) p21, obj10))
            {
              flag = true;
              break;
            }
            Thread.Sleep(1000);
          }
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__23 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target10 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__23.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p23 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__23;
            object atolDriver9 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__22 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_ONLINE_VALIDATION_RESULT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj11 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__22.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__22, this.AtolDriver);
            object obj12 = target10((CallSite) p23, atolDriver9, obj11);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__25 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target11 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__25.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p25 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__25;
            object atolDriver10 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__24 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_ONLINE_VALIDATION_ERROR", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj13 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__24.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__24, this.AtolDriver);
            object obj14 = target11((CallSite) p25, atolDriver10, obj13);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__27 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamString", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target12 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__27.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p27 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__27;
            object atolDriver11 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__26 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_ONLINE_VALIDATION_ERROR_DESCRIPTION", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj15 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__26.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__26, this.AtolDriver);
            object obj16 = target12((CallSite) p27, atolDriver11, obj15);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__29 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target13 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__29.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p29 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__29;
            object atolDriver12 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__28 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_OFFLINE_VALIDATION_ERROR", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj17 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__28.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__28, this.AtolDriver);
            object obj18 = target13((CallSite) p29, atolDriver12, obj17);
            LogHelper.Debug("Проверка кода маркировки закончилась.");
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__33 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__33 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, System.Type, object> target14 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__33.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, System.Type, object>> p33 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__33;
            System.Type type1 = typeof (LogHelper);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__32 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target15 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__32.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p32 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__32;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__31 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string, object> target16 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__31.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string, object>> p31 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__31;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__30 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__30 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj19 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__30.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__30, "ErrorOnlineResult: ", obj14);
            object obj20 = target16((CallSite) p31, obj19, ", ");
            object obj21 = obj16;
            object obj22 = target15((CallSite) p32, obj20, obj21);
            target14((CallSite) p33, type1, obj22);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__35 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__35 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, System.Type, object> target17 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__35.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, System.Type, object>> p35 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__35;
            System.Type type2 = typeof (LogHelper);
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__34 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__34 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj23 = Atol10.\u003C\u003Eo__26.\u003C\u003Ep__34.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__34, "ErrorOfflineResult: ", obj18);
            target17((CallSite) p35, type2, obj23);
            MarkedInfo markedInfo = checkGood.MarkedInfo;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__36 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: variable of a boxed type
            __Boxed<int> local = (ValueType) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__36.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__36, obj12);
            markedInfo.ValidationResultKkm = (object) local;
            LogHelper.Debug(string.Format("Validation ready: {0}; result code: {1}", (object) flag, checkGood.MarkedInfo.ValidationResultKkm));
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__38 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__38 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "acceptMarkingCode", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__38.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__38, this.AtolDriver);
          }
          else
          {
            checkGood.MarkedInfo.ValidationResultKkm = (object) 0;
            LogHelper.Debug("Проверка кода не завершена, таймаут проверки, отменяем проверку, но проводим КМ в чек");
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__26.\u003C\u003Ep__37 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__26.\u003C\u003Ep__37 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "acceptMarkingCode", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__26.\u003C\u003Ep__37.Target((CallSite) Atol10.\u003C\u003Eo__26.\u003C\u003Ep__37, this.AtolDriver);
            break;
          }
        }
      }
      catch (Exception ex)
      {
        string кодаМаркировкиВКкм = Translate.Atol10_Ffd120CodeValidation_Ошибка_проверки_кода_маркировки_в_ККМ;
        string logMessage = кодаМаркировкиВКкм;
        LogHelper.WriteError(ex, logMessage);
        string message = кодаМаркировкиВКкм;
        LogHelper.ShowErrorMgs(ex, message, LogHelper.MsgTypes.Notification);
      }
    }

    public bool CloseCheck()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__27.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__27.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "closeReceipt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__27.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__27.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__27.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__27.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__27.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__27.\u003C\u003Ep__1, this.AtolDriver);
      this.CheckResult();
      return true;
    }

    public void CancelCheck()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__28.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__28.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cancelReceipt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__28.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__28.\u003C\u003Ep__0, this.AtolDriver);
      this.CheckResult();
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__29.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__29.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target = Atol10.\u003C\u003Eo__29.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p1 = Atol10.\u003C\u003Eo__29.\u003C\u003Ep__1;
      object atolDriver = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__29.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__29.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SUM", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol10.\u003C\u003Eo__29.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__29.\u003C\u003Ep__0, this.AtolDriver);
      double num = (double) sum;
      target((CallSite) p1, atolDriver, obj, num);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__29.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__29.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cashOutcome", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__29.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__29.\u003C\u003Ep__2, this.AtolDriver);
      this.CheckResult();
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__30.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__30.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target = Atol10.\u003C\u003Eo__30.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p1 = Atol10.\u003C\u003Eo__30.\u003C\u003Ep__1;
      object atolDriver = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__30.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__30.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SUM", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol10.\u003C\u003Eo__30.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__30.\u003C\u003Ep__0, this.AtolDriver);
      double num = (double) sum;
      target((CallSite) p1, atolDriver, obj, num);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__30.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__30.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cashIncome", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__30.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__30.\u003C\u003Ep__2, this.AtolDriver);
      this.CheckResult();
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
      if (Atol10.\u003C\u003Eo__32.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__32.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__2;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__32.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__32.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__32.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__32.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__32.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_CASH_SUM", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__32.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, atolDriver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__32.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__32.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__32.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__32.\u003C\u003Ep__3, this.AtolDriver);
      this.CheckResult();
      ref Decimal local = ref sum;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__32.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__32.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Decimal), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target2 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p6 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__32.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__32.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDouble", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target3 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__5;
      object atolDriver2 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__32.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__32.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SUM", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__32.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__32.\u003C\u003Ep__4, this.AtolDriver);
      object obj4 = target3((CallSite) p5, atolDriver2, obj3);
      Decimal num = target2((CallSite) p6, obj4);
      local = num;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      LogHelper.Debug("АТОЛ 10: 100");
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__2;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAX_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_NO", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, atolDriver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, string> target2 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, string>> p4 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__4;
      object atolDriver2 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_COMMODITY_NAME", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__3, this.AtolDriver);
      string name = good.Name;
      target2((CallSite) p4, atolDriver2, obj3, name);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__6 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target3 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p6 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__6;
      object atolDriver3 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PRICE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__5.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__5, this.AtolDriver);
      double price = (double) good.Price;
      target3((CallSite) p6, atolDriver3, obj4, price);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target4 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__8;
      object atolDriver4 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_QUANTITY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__7, this.AtolDriver);
      double quantity = (double) good.Quantity;
      target4((CallSite) p8, atolDriver4, obj5, quantity);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target5 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p10 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__10;
      object atolDriver5 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DEPARTMENT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__9, this.AtolDriver);
      int kkmSectionNumber = good.KkmSectionNumber;
      target5((CallSite) p10, atolDriver5, obj6, kkmSectionNumber);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__12 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target6 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__12.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p12 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__12;
      object atolDriver6 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_INFO_DISCOUNT_SUM", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__11.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__11, this.AtolDriver);
      double discountSum = (double) good.DiscountSum;
      target6((CallSite) p12, atolDriver6, obj7, discountSum);
      object obj8;
      switch (good.TaxRateNumber)
      {
        case 1:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_NO", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__13.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__13, this.AtolDriver);
          break;
        case 2:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT0", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__14.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__14, this.AtolDriver);
          break;
        case 3:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT10", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__15.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__15, this.AtolDriver);
          break;
        case 4:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT20", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__16.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__16, this.AtolDriver);
          break;
        case 5:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT110", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__17.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__17, this.AtolDriver);
          break;
        case 6:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT120", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__18.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__18, this.AtolDriver);
          break;
        case 7:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT5", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__19.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__19, this.AtolDriver);
          break;
        case 8:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT7", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__20.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__20, this.AtolDriver);
          break;
        case 9:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT105", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__21.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__21, this.AtolDriver);
          break;
        case 10:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT107", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__22.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__22, this.AtolDriver);
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_NO", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__23.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__23, this.AtolDriver);
          break;
      }
      object obj9 = obj8;
      LogHelper.Debug("АТОЛ 10: 110");
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__25 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target7 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__25.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p25 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__25;
      object atolDriver7 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAX_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = Atol10.\u003C\u003Eo__33.\u003C\u003Ep__24.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__24, this.AtolDriver);
      object obj11 = obj9;
      target7((CallSite) p25, atolDriver7, obj10, obj11);
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      GlobalDictionaries.RuFfdGoodsTypes ruFfdGoodsTypes = good.RuFfdGoodTypeCode == GlobalDictionaries.RuFfdGoodsTypes.None ? GlobalDictionaries.RuFfdGoodsTypes.SimpleGood : good.RuFfdGoodTypeCode;
      LogHelper.Debug("АТОЛ 10: 120");
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__26 = CallSite<Action<CallSite, object, int, GlobalDictionaries.RuFfdGoodsTypes>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__33.\u003C\u003Ep__26.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__26, this.AtolDriver, 1212, ruFfdGoodsTypes);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__33.\u003C\u003Ep__27 = CallSite<Action<CallSite, object, int, GlobalDictionaries.RuFfdPaymentModes>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__33.\u003C\u003Ep__27.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__27, this.AtolDriver, 1214, good.RuFfdPaymentModeCode);
      switch (devices.CheckPrinter.FiscalKkm.FfdVersion)
      {
        case GlobalDictionaries.Devices.FfdVersions.OfflineKkm:
        case GlobalDictionaries.Devices.FfdVersions.Ffd100:
          LogHelper.Debug("АТОЛ 10: 130");
          try
          {
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__33.\u003C\u003Ep__28 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__33.\u003C\u003Ep__28 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "registration", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__33.\u003C\u003Ep__28.Target((CallSite) Atol10.\u003C\u003Eo__33.\u003C\u003Ep__28, this.AtolDriver);
          }
          catch (Exception ex)
          {
            LogHelper.Error(ex, "ОШИБКА РЕГИСТРАЦИИ АТОЛ", false);
            throw new KkmException((IDevice) this, "ОШИБКА РЕГИСТРАЦИИ АТОЛ");
          }
          LogHelper.Debug("АТОЛ 10: 140");
          this.CheckResult();
          this.PrintNonFiscalStrings(good.CommentForFiscalCheck.Select<string, NonFiscalString>((Func<string, NonFiscalString>) (x => new NonFiscalString(x))).ToList<NonFiscalString>(), false);
          return true;
        case GlobalDictionaries.Devices.FfdVersions.Ffd105:
        case GlobalDictionaries.Devices.FfdVersions.Ffd110:
          this.SetInfo_ffd105_ffd110(good);
          goto case GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
        case GlobalDictionaries.Devices.FfdVersions.Ffd120:
          this.SetInfo_ffd120(good);
          goto case GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void SetInfo_ffd120(CheckGood good)
    {
      LogHelper.Debug(string.Format("Запись тега 2108: {0}", (object) good.Unit.RuFfdUnitsIndex));
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target1 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p1 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__1;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MEASUREMENT_UNIT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__0, this.AtolDriver);
      int ruFfdUnitsIndex = good.Unit.RuFfdUnitsIndex;
      target1((CallSite) p1, atolDriver1, obj1, ruFfdUnitsIndex);
      string str1 = this.PrepareMarkCodeForFfd120(good.MarkedInfo?.FullCode ?? string.Empty);
      if (good.MarkedInfo == null || str1.IsNullOrEmpty())
        return;
      if (this._industryInfo != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__34.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, byte[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__2, this.AtolDriver, 1260, this._industryInfo);
      }
      LogHelper.Debug(string.Format("Запись тега 2106: {0}", good.MarkedInfo.ValidationResultKkm));
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, string> target2 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, string>> p4 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__4;
      object atolDriver2 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__3, this.AtolDriver);
      string str2 = str1;
      target2((CallSite) p4, atolDriver2, obj2, str2);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__6 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target3 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p6 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__6;
      object atolDriver3 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_STATUS", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__5.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__5, this.AtolDriver);
      int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(good, this._checkData.CheckType);
      target3((CallSite) p6, atolDriver3, obj3, markingCodeStatus);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target4 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p8 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__8;
      object atolDriver4 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_PROCESSING_MODE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__7, this.AtolDriver);
      target4((CallSite) p8, atolDriver4, obj4, 0);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__11 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target5 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p11 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__11;
      object atolDriver5 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__9, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_MCT12_AUTO", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__10.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__10, this.AtolDriver);
      target5((CallSite) p11, atolDriver5, obj5, obj6);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__13 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target6 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p13 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__13;
      object atolDriver6 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__34.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__34.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_ONLINE_VALIDATION_RESULT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = Atol10.\u003C\u003Eo__34.\u003C\u003Ep__12.Target((CallSite) Atol10.\u003C\u003Eo__34.\u003C\u003Ep__12, this.AtolDriver);
      object obj8 = good.MarkedInfo.ValidationResultKkm ?? (object) 0;
      target6((CallSite) p13, atolDriver6, obj7, obj8);
    }

    private void SetInfo_ffd105_ffd110(CheckGood good)
    {
      if (good.MarkedInfo == null || good.MarkedInfo.Type == GlobalDictionaries.RuMarkedProductionTypes.None || !good.MarkedInfo.IsValidCode())
        return;
      LogHelper.Debug("Маркировка " + good.MarkedInfo.ToJsonString());
      string hexStringAttribute = good.MarkedInfo.GetHexStringAttribute();
      LogHelper.Debug("Маркировка " + hexStringAttribute);
      if (hexStringAttribute.IsNullOrEmpty())
        return;
      LogHelper.Debug("Запись тега 1162: " + hexStringAttribute);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__35.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__35.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParamStrHex", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__35.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__35.\u003C\u003Ep__0, this.AtolDriver, 1162, hexStringAttribute);
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__2;
          object atolDriver1 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__0, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_CASH", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__1, this.AtolDriver);
          target1((CallSite) p2, atolDriver1, obj1, obj2);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__5;
          object atolDriver2 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__3, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_ELECTRONICALLY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__4, this.AtolDriver);
          target2((CallSite) p5, atolDriver2, obj3, obj4);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target3 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__8.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p8 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__8;
          object atolDriver3 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__6.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__6, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_6", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__7, this.AtolDriver);
          target3((CallSite) p8, atolDriver3, obj5, obj6);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__11 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target4 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__11.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p11 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__11;
          object atolDriver4 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__9, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_CREDIT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__10.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__10, this.AtolDriver);
          target4((CallSite) p11, atolDriver4, obj7, obj8);
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target5 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__14.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p14 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__14;
          object atolDriver5 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj9 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__12.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__12, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__36.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_PREPAID", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj10 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__13.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__13, this.AtolDriver);
          target5((CallSite) p14, atolDriver5, obj9, obj10);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__36.\u003C\u003Ep__16 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target6 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p16 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__16;
      object atolDriver6 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__36.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_SUM", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = Atol10.\u003C\u003Eo__36.\u003C\u003Ep__15.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__15, this.AtolDriver);
      double sum = (double) payment.Sum;
      target6((CallSite) p16, atolDriver6, obj11, sum);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__36.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__36.\u003C\u003Ep__17 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (payment), (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__36.\u003C\u003Ep__17.Target((CallSite) Atol10.\u003C\u003Eo__36.\u003C\u003Ep__17, this.AtolDriver);
      this.CheckResult();
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      this.PrintNonFiscalStrings(new List<NonFiscalString>()
      {
        new NonFiscalString(string.Format("{0}: {1:N2}", (object) description, (object) sum), TextAlignment.Right)
      }, false);
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      LogHelper.Debug("Подключение к ККМ АТОЛ 10");
      this.AtolDriver = Functions.CreateObject("AddIn.Fptr10");
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__38.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__38.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Atol10.\u003C\u003Eo__38.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Atol10.\u003C\u003Eo__38.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__38.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__38.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol10.\u003C\u003Eo__38.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__38.\u003C\u003Ep__0, this.AtolDriver, (object) null);
      if (target((CallSite) p1, obj))
        throw new NullReferenceException(Translate.Atol10_Объект_драйвера_АТОЛ_10_не_был_создан);
      string connectionConfig = this.DevicesConfig.CheckPrinter.FiscalKkm.Atol10ConnectionConfig;
      if (!connectionConfig.IsNullOrEmpty())
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__38.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__38.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setSettings", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__38.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__38.\u003C\u003Ep__2, this.AtolDriver, connectionConfig);
      }
      if (onlyDriverLoad)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__38.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__38.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "open", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__38.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__38.\u003C\u003Ep__3, this.AtolDriver);
      this.CheckResult();
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__39.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__39.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Atol10.\u003C\u003Eo__39.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Atol10.\u003C\u003Eo__39.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__39.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__39.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol10.\u003C\u003Eo__39.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__39.\u003C\u003Ep__0, this.AtolDriver, (object) null);
      if (target((CallSite) p1, obj))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__39.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__39.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "close", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__39.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__39.\u003C\u003Ep__2, this.AtolDriver);
      this.CheckResult();
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__39.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__39.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReleaseComObject", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__39.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__39.\u003C\u003Ep__3, typeof (Marshal), this.AtolDriver);
      this.AtolDriver = (object) null;
      return true;
    }

    public bool IsConnected
    {
      get
      {
        if (Atol10.\u003C\u003Eo__41.\u003C\u003Ep__1 == null)
          Atol10.\u003C\u003Eo__41.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target1 = Atol10.\u003C\u003Eo__41.\u003C\u003Ep__1.Target;
        CallSite<Func<CallSite, object, bool>> p1 = Atol10.\u003C\u003Eo__41.\u003C\u003Ep__1;
        if (Atol10.\u003C\u003Eo__41.\u003C\u003Ep__0 == null)
          Atol10.\u003C\u003Eo__41.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        object obj1 = Atol10.\u003C\u003Eo__41.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__41.\u003C\u003Ep__0, this.AtolDriver, (object) null);
        if (target1((CallSite) p1, obj1))
          return false;
        if (Atol10.\u003C\u003Eo__41.\u003C\u003Ep__3 == null)
          Atol10.\u003C\u003Eo__41.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (Atol10)));
        Func<CallSite, object, bool> target2 = Atol10.\u003C\u003Eo__41.\u003C\u003Ep__3.Target;
        CallSite<Func<CallSite, object, bool>> p3 = Atol10.\u003C\u003Eo__41.\u003C\u003Ep__3;
        if (Atol10.\u003C\u003Eo__41.\u003C\u003Ep__2 == null)
          Atol10.\u003C\u003Eo__41.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "isOpened", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj2 = Atol10.\u003C\u003Eo__41.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__41.\u003C\u003Ep__2, this.AtolDriver);
        return target2((CallSite) p3, obj2);
      }
      set
      {
      }
    }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings, bool isOpenCheck)
    {
      if (isOpenCheck)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "beginNonfiscalDocument", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__43.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__0, this.AtolDriver);
      }
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, string> target1 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, string>> p2 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__2;
        object atolDriver1 = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TEXT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__1, this.AtolDriver);
        string text = nonFiscalString.Text;
        target1((CallSite) p2, atolDriver1, obj1, text);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target2 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p4 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__4;
        object atolDriver2 = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FONT_DOUBLE_WIDTH", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__3, this.AtolDriver);
        int num1 = nonFiscalString.WideFont ? 1 : 0;
        target2((CallSite) p4, atolDriver2, obj2, num1 != 0);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__6 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target3 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p6 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__6;
        object atolDriver3 = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FONT_DOUBLE_HEIGHT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__5.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__5, this.AtolDriver);
        int num2 = nonFiscalString.WideFont ? 1 : 0;
        target3((CallSite) p6, atolDriver3, obj3, num2 != 0);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, int> target4 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, int>> p8 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__8;
        object atolDriver4 = this.AtolDriver;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TEXT_WRAP", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__7, this.AtolDriver);
        target4((CallSite) p8, atolDriver4, obj4, 1);
        switch (nonFiscalString.Alignment)
        {
          case TextAlignment.Left:
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target5 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__10.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p10 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__10;
            object atolDriver5 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__9, this.AtolDriver);
            target5((CallSite) p10, atolDriver5, obj5, 0);
            break;
          case TextAlignment.Right:
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__12 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target6 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__12.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p12 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__12;
            object atolDriver6 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj6 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__11.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__11, this.AtolDriver);
            target6((CallSite) p12, atolDriver6, obj6, 2);
            break;
          case TextAlignment.Center:
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target7 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__14.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p14 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__14;
            object atolDriver7 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__13 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__13.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__13, this.AtolDriver);
            target7((CallSite) p14, atolDriver7, obj7, 1);
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__16 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__16 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target8 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__16.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p16 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__16;
            object atolDriver8 = this.AtolDriver;
            // ISSUE: reference to a compiler-generated field
            if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__15 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Atol10.\u003C\u003Eo__43.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj8 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__15.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__15, this.AtolDriver);
            target8((CallSite) p16, atolDriver8, obj8, 0);
            break;
        }
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__43.\u003C\u003Ep__17 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "printText", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__43.\u003C\u003Ep__17.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__17, this.AtolDriver);
        this.CheckResult();
      }
      if (!isOpenCheck)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__43.\u003C\u003Ep__19 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, bool> target = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, bool>> p19 = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__19;
      object atolDriver = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__43.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PRINT_FOOTER", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol10.\u003C\u003Eo__43.\u003C\u003Ep__18.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__18, this.AtolDriver);
      target((CallSite) p19, atolDriver, obj, false);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__43.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__43.\u003C\u003Ep__20 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "endNonfiscalDocument", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__43.\u003C\u003Ep__20.Target((CallSite) Atol10.\u003C\u003Eo__43.\u003C\u003Ep__20, this.AtolDriver);
    }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      this.PrintNonFiscalStrings(nonFiscalStrings, true);
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__45.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, string> target1 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, string>> p1 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__1;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__45.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_BARCODE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__0, this.AtolDriver);
      string str = code;
      target1((CallSite) p1, atolDriver1, obj1, str);
      switch (type)
      {
        case BarcodeTypes.None:
          return true;
        case BarcodeTypes.Ean13:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p4 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__4;
          object atolDriver2 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_BARCODE_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__2, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_BT_EAN_13", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__3, this.AtolDriver);
          target2((CallSite) p4, atolDriver2, obj2, obj3);
          break;
        case BarcodeTypes.QrCode:
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target3 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p7 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__7;
          object atolDriver3 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_BARCODE_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__5.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__5, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_BT_QR", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__6.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__6, this.AtolDriver);
          target3((CallSite) p7, atolDriver3, obj4, obj5);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target4 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__10.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p10 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__10;
          object atolDriver4 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__8.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__8, this.AtolDriver);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_ALIGNMENT_CENTER", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__9, this.AtolDriver);
          target4((CallSite) p10, atolDriver4, obj6, obj7);
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__12 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, int> target5 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__12.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, int>> p12 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__12;
          object atolDriver5 = this.AtolDriver;
          // ISSUE: reference to a compiler-generated field
          if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol10.\u003C\u003Eo__45.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SCALE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = Atol10.\u003C\u003Eo__45.\u003C\u003Ep__11.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__11, this.AtolDriver);
          target5((CallSite) p12, atolDriver5, obj8, 7);
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__45.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__45.\u003C\u003Ep__13 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "printBarcode", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__45.\u003C\u003Ep__13.Target((CallSite) Atol10.\u003C\u003Eo__45.\u003C\u003Ep__13, this.AtolDriver);
      this.CheckResult();
      return true;
    }

    public bool CutPaper()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__46.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__46.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cut", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__46.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__46.\u003C\u003Ep__0, this.AtolDriver);
      return true;
    }

    public KkmStatus GetStatus()
    {
      KkmStatus status = new KkmStatus()
      {
        KkmState = this.GetKkmState()
      };
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__2;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_SHIFT_STATE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, atolDriver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__47.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__3, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__5;
      object atolDriver2 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SHIFT_STATE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__4, this.AtolDriver);
      if (target2((CallSite) p5, atolDriver2, obj3) is int num1)
      {
        switch (num1)
        {
          case 0:
            status.SessionStatus = SessionStatuses.Close;
            break;
          case 1:
            status.SessionStatus = SessionStatuses.Open;
            break;
          case 2:
            status.SessionStatus = SessionStatuses.OpenMore24Hours;
            break;
        }
      }
      KkmStatus kkmStatus1 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target3 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p8 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p7 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__7;
      object atolDriver3 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SHIFT_NUMBER", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__6.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__6, this.AtolDriver);
      object obj5 = target4((CallSite) p7, atolDriver3, obj4);
      int num2 = target3((CallSite) p8, obj5);
      kkmStatus1.SessionNumber = num2;
      KkmStatus kkmStatus2 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (DateTime), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target5 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p11 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p10 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__10;
      object atolDriver4 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__9, this.AtolDriver);
      object obj7 = target6((CallSite) p10, atolDriver4, obj6);
      DateTime? nullable1 = new DateTime?(target5((CallSite) p11, obj7).AddHours(-24.0));
      kkmStatus2.SessionStarted = nullable1;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target7 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p14 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__14;
      object atolDriver5 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__12.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__12, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_RECEIPT_STATE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__13.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__13, this.AtolDriver);
      target7((CallSite) p14, atolDriver5, obj8, obj9);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__15 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__47.\u003C\u003Ep__15.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__15, this.AtolDriver);
      KkmStatus kkmStatus3 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target8 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__21.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p21 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__21;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target9 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__20.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p20 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__20;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target10 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p17 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__17;
      object atolDriver6 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__16.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__16, this.AtolDriver);
      object obj11 = target10((CallSite) p17, atolDriver6, obj10);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target11 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p19 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__19;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_CLOSED", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__18.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__18, this.AtolDriver);
      int num3 = target11((CallSite) p19, obj12);
      object obj13 = target9((CallSite) p20, obj11, num3);
      int num4 = target8((CallSite) p21, obj13) ? 2 : 1;
      kkmStatus3.CheckStatus = (CheckStatuses) num4;
      KkmStatus kkmStatus4 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target12 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__24.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p24 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__24;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target13 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__23.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p23 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__23;
      object atolDriver7 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_NUMBER", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__22.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__22, this.AtolDriver);
      object obj15 = target13((CallSite) p23, atolDriver7, obj14);
      int num5 = target12((CallSite) p24, obj15) + 1;
      kkmStatus4.CheckNumber = num5;
      KkmStatus kkmStatus5 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__27 = CallSite<Func<CallSite, System.Type, object, Version>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, Version> target14 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__27.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, Version>> p27 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__27;
      System.Type type = typeof (Version);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target15 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__26.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p26 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__26;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "version", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj16 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__25.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__25, this.AtolDriver);
      object obj17 = target15((CallSite) p26, obj16);
      Version version = target14((CallSite) p27, type, obj17);
      kkmStatus5.DriverVersion = version;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__30 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target16 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__30.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p30 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__30;
      object atolDriver8 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj18 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__28.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__28, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_STATUS", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj19 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__29.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__29, this.AtolDriver);
      target16((CallSite) p30, atolDriver8, obj18, obj19);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__31 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__47.\u003C\u003Ep__31.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__31, this.AtolDriver);
      KkmStatus kkmStatus6 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target17 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__34.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p34 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__34;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamString", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target18 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__33.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p33 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__33;
      object atolDriver9 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SERIAL_NUMBER", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj20 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__32.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__32, this.AtolDriver);
      object obj21 = target18((CallSite) p33, atolDriver9, obj20);
      string str1 = target17((CallSite) p34, obj21);
      kkmStatus6.FactoryNumber = str1;
      KkmStatus kkmStatus7 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__37 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__37 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target19 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__37.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p37 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__37;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamString", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target20 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__36.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p36 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__36;
      object atolDriver10 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MODEL_NAME", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj22 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__35.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__35, this.AtolDriver);
      object obj23 = target20((CallSite) p36, atolDriver10, obj22);
      string str2 = target19((CallSite) p37, obj23);
      kkmStatus7.Model = str2;
      KkmStatus kkmStatus8 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__40 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__40 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target21 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__40.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p40 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__40;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__39 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__39 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamString", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target22 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__39.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p39 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__39;
      object atolDriver11 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__38 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_UNIT_VERSION", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj24 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__38.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__38, this.AtolDriver);
      object obj25 = target22((CallSite) p39, atolDriver11, obj24);
      string str3 = target21((CallSite) p40, obj25);
      kkmStatus8.SoftwareVersion = str3;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__43 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__43 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target23 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__43.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p43 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__43;
      object atolDriver12 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__41 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj26 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__41.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__41, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__42 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_OFD_EXCHANGE_STATUS", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj27 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__42.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__42, this.AtolDriver);
      target23((CallSite) p43, atolDriver12, obj26, obj27);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__44 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__44 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "fnQueryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__47.\u003C\u003Ep__44.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__44, this.AtolDriver);
      KkmStatus kkmStatus9 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__47 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__47 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target24 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__47.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p47 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__47;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__46 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__46 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target25 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__46.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p46 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__46;
      object atolDriver13 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__45 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DOCUMENTS_COUNT", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj28 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__45.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__45, this.AtolDriver);
      object obj29 = target25((CallSite) p46, atolDriver13, obj28);
      int num6 = target24((CallSite) p47, obj29);
      kkmStatus9.OfdNotSendDocuments = num6;
      KkmStatus kkmStatus10 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__50 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__50 = CallSite<Func<CallSite, object, DateTime?>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime?), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime?> target26 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__50.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime?>> p50 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__50;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__49 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__49 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target27 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__49.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p49 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__49;
      object atolDriver14 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__48 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj30 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__48.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__48, this.AtolDriver);
      object obj31 = target27((CallSite) p49, atolDriver14, obj30);
      DateTime? nullable2 = target26((CallSite) p50, obj31);
      kkmStatus10.OfdLastSendDateTime = nullable2;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__53 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__53 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target28 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__53.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p53 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__53;
      object atolDriver15 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__51 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj32 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__51.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__51, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__52 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_VALIDITY", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj33 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__52.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__52, this.AtolDriver);
      target28((CallSite) p53, atolDriver15, obj32, obj33);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__54 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__54 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "fnQueryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__47.\u003C\u003Ep__54.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__54, this.AtolDriver);
      KkmStatus kkmStatus11 = status;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__57 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__57 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target29 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__57.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p57 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__57;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__56 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__56 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target30 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__56.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p56 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__56;
      object atolDriver16 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__55 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj34 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__55.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__55, this.AtolDriver);
      object obj35 = target30((CallSite) p56, atolDriver16, obj34);
      DateTime dateTime = target29((CallSite) p57, obj35);
      kkmStatus11.FnDateEnd = dateTime;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__59 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__59 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target31 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__59.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p59 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__59;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__58 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__58 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj36 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__58.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__58, this.AtolDriver);
      if (target31((CallSite) p59, obj36) == 82)
      {
        LogHelper.Debug("Ошибка 82. Пытаемся закрыть прошлый документ");
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__60 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__47.\u003C\u003Ep__60 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "closeReceipt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__60.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__60, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__61 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__47.\u003C\u003Ep__61 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__47.\u003C\u003Ep__61.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__61, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__63 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__47.\u003C\u003Ep__63 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target32 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__63.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p63 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__63;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__47.\u003C\u003Ep__62 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__47.\u003C\u003Ep__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj37 = Atol10.\u003C\u003Eo__47.\u003C\u003Ep__62.Target((CallSite) Atol10.\u003C\u003Eo__47.\u003C\u003Ep__62, this.AtolDriver);
        LogHelper.Debug("Результат закрытия: " + target32((CallSite) p63, obj37).ToString());
      }
      return status;
    }

    public KkmStatus GetShortStatus()
    {
      KkmStatus shortStatus = new KkmStatus()
      {
        KkmState = this.GetKkmState()
      };
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__2;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_SHIFT_STATE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, atolDriver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__48.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__3, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__5;
      object atolDriver2 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SHIFT_STATE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__4, this.AtolDriver);
      if (target2((CallSite) p5, atolDriver2, obj3) is int num1)
      {
        switch (num1)
        {
          case 0:
            shortStatus.SessionStatus = SessionStatuses.Close;
            break;
          case 1:
            shortStatus.SessionStatus = SessionStatuses.Open;
            break;
          case 2:
            shortStatus.SessionStatus = SessionStatuses.OpenMore24Hours;
            break;
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target3 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p8 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__8;
      object atolDriver3 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__6.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__6, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_RECEIPT_STATE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__7.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__7, this.AtolDriver);
      target3((CallSite) p8, atolDriver3, obj4, obj5);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__9 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__48.\u003C\u003Ep__9.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__9, this.AtolDriver);
      KkmStatus kkmStatus = shortStatus;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__15.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p15 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__15;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target5 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p14 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__14;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p11 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__11;
      object atolDriver4 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__10.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__10, this.AtolDriver);
      object obj7 = target6((CallSite) p11, atolDriver4, obj6);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target7 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p13 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_CLOSED", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__12.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__12, this.AtolDriver);
      int num2 = target7((CallSite) p13, obj8);
      object obj9 = target5((CallSite) p14, obj7, num2);
      int num3 = target4((CallSite) p15, obj9) ? 2 : 1;
      kkmStatus.CheckStatus = (CheckStatuses) num3;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target8 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p17 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__17;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__16.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__16, this.AtolDriver);
      if (target8((CallSite) p17, obj10) == 82)
      {
        LogHelper.Debug("Ошибка 82. Пытаемся закрыть прошлый документ");
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__48.\u003C\u003Ep__18 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "closeReceipt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__18.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__18, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__48.\u003C\u003Ep__19 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__48.\u003C\u003Ep__19.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__19, this.AtolDriver);
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__21 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__48.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol10)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target9 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__21.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p21 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__21;
        // ISSUE: reference to a compiler-generated field
        if (Atol10.\u003C\u003Eo__48.\u003C\u003Ep__20 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol10.\u003C\u003Eo__48.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj11 = Atol10.\u003C\u003Eo__48.\u003C\u003Ep__20.Target((CallSite) Atol10.\u003C\u003Eo__48.\u003C\u003Ep__20, this.AtolDriver);
        LogHelper.Debug("Результат закрытия: " + target9((CallSite) p21, obj11).ToString());
      }
      return shortStatus;
    }

    public bool OpenCashDrawer()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__49.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__49.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "openDrawer", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__49.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__49.\u003C\u003Ep__0, this.AtolDriver);
      return true;
    }

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__50.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__50.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Atol10.\u003C\u003Eo__50.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Atol10.\u003C\u003Eo__50.\u003C\u003Ep__2;
      object atolDriver1 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__50.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__50.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__50.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__50.\u003C\u003Ep__0, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__50.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__50.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_FFD_VERSIONS", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol10.\u003C\u003Eo__50.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__50.\u003C\u003Ep__1, this.AtolDriver);
      target1((CallSite) p2, atolDriver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__50.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__50.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__50.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__50.\u003C\u003Ep__3, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__50.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__50.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__50.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Atol10.\u003C\u003Eo__50.\u003C\u003Ep__5;
      object atolDriver2 = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__50.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__50.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FFD_VERSION", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol10.\u003C\u003Eo__50.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__50.\u003C\u003Ep__4, this.AtolDriver);
      if (target2((CallSite) p5, atolDriver2, obj3) is int num)
      {
        switch (num)
        {
          case 0:
            return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
          case 1:
            return GlobalDictionaries.Devices.FfdVersions.Ffd105;
          case 2:
            return GlobalDictionaries.Devices.FfdVersions.Ffd110;
          case 3:
            return GlobalDictionaries.Devices.FfdVersions.Ffd120;
        }
      }
      return GlobalDictionaries.Devices.FfdVersions.OfflineKkm;
    }

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();

    public string PrepareMarkCodeForFfd120(string code)
    {
      return DataMatrixHelper.ReplaceSomeCharsToFNC1(code);
    }

    private void GetSalesNotice(string info)
    {
      if (info.IsNullOrEmpty() || new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__54.\u003C\u003Ep__0.Target((CallSite) Atol10.\u003C\u003Eo__54.\u003C\u003Ep__0, this.AtolDriver, 1262, "030");
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__54.\u003C\u003Ep__1.Target((CallSite) Atol10.\u003C\u003Eo__54.\u003C\u003Ep__1, this.AtolDriver, 1263, "21.11.2023");
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__54.\u003C\u003Ep__2.Target((CallSite) Atol10.\u003C\u003Eo__54.\u003C\u003Ep__2, this.AtolDriver, 1264, "1944");
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__54.\u003C\u003Ep__3.Target((CallSite) Atol10.\u003C\u003Eo__54.\u003C\u003Ep__3, this.AtolDriver, 1265, info);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__4 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "utilFormTlv", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol10.\u003C\u003Eo__54.\u003C\u003Ep__4.Target((CallSite) Atol10.\u003C\u003Eo__54.\u003C\u003Ep__4, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, byte[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (byte[]), typeof (Atol10)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, byte[]> target1 = Atol10.\u003C\u003Eo__54.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, byte[]>> p7 = Atol10.\u003C\u003Eo__54.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamByteArray", (IEnumerable<System.Type>) null, typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Atol10.\u003C\u003Eo__54.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p6 = Atol10.\u003C\u003Eo__54.\u003C\u003Ep__6;
      object atolDriver = this.AtolDriver;
      // ISSUE: reference to a compiler-generated field
      if (Atol10.\u003C\u003Eo__54.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol10.\u003C\u003Eo__54.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAG_VALUE", typeof (Atol10), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol10.\u003C\u003Eo__54.\u003C\u003Ep__5.Target((CallSite) Atol10.\u003C\u003Eo__54.\u003C\u003Ep__5, this.AtolDriver);
      object obj2 = target2((CallSite) p6, atolDriver, obj1);
      this._industryInfo = target1((CallSite) p7, obj2);
    }
  }
}
