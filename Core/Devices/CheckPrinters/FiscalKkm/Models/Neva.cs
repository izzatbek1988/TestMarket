// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Neva
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
  public class Neva : IOnlineKkm, IFiscalKkm, IDevice
  {
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _checkData;
    private byte[] _industryInfo;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.ККТНЕВА03Ф;

    private object Driver { get; set; }

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public Neva(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    private void CheckResult()
    {
      Thread.Sleep(300);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target1 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__12.\u003C\u003Ep__0, this.Driver);
      int num = target1((CallSite) p1, obj1);
      switch (num)
      {
        case 0:
          return;
        case 177:
          LogHelper.Debug("Ошибка 177. Пытаемся допечатать документ");
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "continuePrint", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__12.\u003C\u003Ep__2, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target2 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p4 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__4;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__12.\u003C\u003Ep__3, this.Driver);
          num = target2((CallSite) p4, obj2);
          LogHelper.Debug("Результат допечатования: " + num.ToString());
          if (num == 0)
            return;
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__12.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__12.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target3 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p6 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorDescription", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__12.\u003C\u003Ep__5.Target((CallSite) Neva.\u003C\u003Eo__12.\u003C\u003Ep__5, this.Driver);
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
      if (Neva.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__13.\u003C\u003Ep__0, this.Driver, ofdAttributeNumber, value);
      this.CheckResult();
    }

    private KkmStatuses GetKkmState()
    {
      KkmStatuses kkmState = KkmStatuses.Ready;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__2;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_SHORT_STATUS", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__1, this.Driver);
      target1((CallSite) p2, driver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__14.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__3, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p7 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p6 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamBool", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__5;
      object driver2 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_COVER_OPENED", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__4, this.Driver);
      object obj4 = target4((CallSite) p5, driver2, obj3);
      object obj5 = target3((CallSite) p6, obj4, 1);
      if (target2((CallSite) p7, obj5))
        kkmState = KkmStatuses.CoverOpen;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target5 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p11 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target6 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p10 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamBool", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target7 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p9 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__9;
      object driver3 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_PAPER_PRESENT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__8.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__8, this.Driver);
      object obj7 = target7((CallSite) p9, driver3, obj6);
      object obj8 = target6((CallSite) p10, obj7, 1);
      if (target5((CallSite) p11, obj8))
        kkmState = KkmStatuses.NoPaper;
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, object> target8 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__14.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, object>> p14 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__14;
        object driver4 = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__12.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__12, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_OFD_EXCHANGE_STATUS", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__13.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__13, this.Driver);
        target8((CallSite) p14, driver4, obj9, obj10);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__15 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "fnQueryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__14.\u003C\u003Ep__15.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__15, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target9 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__17.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p17 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__17;
        object driver5 = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DOCUMENTS_COUNT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj11 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__16.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__16, this.Driver);
        object obj12 = target9((CallSite) p17, driver5, obj11);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target10 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__19.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p19 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__19;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__18 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj13 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__18.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__18, obj12, 0);
        if (target10((CallSite) p19, obj13))
        {
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__25 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__14.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target11 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__25.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p25 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__25;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__14.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int, object> target12 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__24.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int, object>> p24 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__24;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__14.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TotalDays", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target13 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__23.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p23 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__23;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__14.\u003C\u003Ep__22 = CallSite<Func<CallSite, DateTime, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Subtract, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, DateTime, object, object> target14 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__22.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, DateTime, object, object>> p22 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__22;
          DateTime now = DateTime.Now;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__14.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target15 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__21.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p21 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__21;
          object driver6 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__14.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj14 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__20.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__20, this.Driver);
          object obj15 = target15((CallSite) p21, driver6, obj14);
          object obj16 = target14((CallSite) p22, now, obj15);
          object obj17 = target13((CallSite) p23, obj16);
          object obj18 = target12((CallSite) p24, obj17, 25);
          if (target11((CallSite) p25, obj18))
            kkmState = KkmStatuses.OfdDocumentsToMany;
        }
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__28 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__28 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, System.Type, object> target16 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__28.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, System.Type, object>> p28 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__28;
        System.Type type = typeof (LogHelper);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__27 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string, object> target17 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__27.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string, object>> p27 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__27;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__14.\u003C\u003Ep__26 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__14.\u003C\u003Ep__26 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj19 = Neva.\u003C\u003Eo__14.\u003C\u003Ep__26.Target((CallSite) Neva.\u003C\u003Eo__14.\u003C\u003Ep__26, "В ОФД неотправлено ", obj12);
        object obj20 = target17((CallSite) p27, obj19, " документов");
        target16((CallSite) p28, type, obj20);
      }
      return kkmState;
    }

    public bool SendDigitalCheck(string address)
    {
      bool flag = !string.IsNullOrEmpty(address) && this.WriteOfdAttribute(OfdAttributes.ClientEmailPhone, (object) address);
      if (flag && this.DevicesConfig.CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target = Neva.\u003C\u003Eo__15.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p1 = Neva.\u003C\u003Eo__15.\u003C\u003Ep__1;
        object driver = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_ELECTRONICALLY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Neva.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__15.\u003C\u003Ep__0, this.Driver);
        target((CallSite) p1, driver, obj, true);
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
      LogHelper.Debug("Открываем настройки ККМ Нева");
      int num1;
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, Version>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, Version> target1 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, Version>> p2 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__2;
        System.Type type = typeof (Version);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target2 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p1 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "version", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__20.\u003C\u003Ep__0, this.Driver);
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
          if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__20.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Neva)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target3 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p5 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__5;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__20.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object, IntPtr, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "showProperties", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, IntPtr, object> target4 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, IntPtr, object>> p4 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__4;
          object driver = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_GUI_PARENT_NATIVE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__20.\u003C\u003Ep__3, this.Driver);
          IntPtr num3 = num2;
          object obj4 = target4((CallSite) p4, driver, obj3, num3);
          num1 = target3((CallSite) p5, obj4);
        }
        else
        {
          LogHelper.Debug("открываем без обхекта окна");
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__20.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Neva)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target5 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p7 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__7;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__20.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "showProperties", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__6.Target((CallSite) Neva.\u003C\u003Eo__20.\u003C\u003Ep__6, this.Driver, (object) null, (object) null);
          num1 = target5((CallSite) p7, obj5);
        }
      }
      catch
      {
        LogHelper.Debug("открываем без объекта, была ошибка");
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__20.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Neva)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = Neva.\u003C\u003Eo__20.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p9 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__20.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "showProperties", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = Neva.\u003C\u003Eo__20.\u003C\u003Ep__8.Target((CallSite) Neva.\u003C\u003Eo__20.\u003C\u003Ep__8, this.Driver, (object) null, (object) null);
        num1 = target((CallSite) p9, obj);
      }
      Gbs.Core.Config.FiscalKkm fiscalKkm = this.DevicesConfig.CheckPrinter.FiscalKkm;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__20.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target6 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p11 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__11;
      if (num1 == -1)
        throw new ErrorHelper.GbsException(Translate.Atol10_Не_удалось_открыть_окно_настроек_драйвера);
      object obj6;
      if (num1 == 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__20.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__20.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getSettings", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        obj6 = Neva.\u003C\u003Eo__20.\u003C\u003Ep__10.Target((CallSite) Neva.\u003C\u003Eo__20.\u003C\u003Ep__10, this.Driver);
      }
      else
        obj6 = (object) this.DevicesConfig.CheckPrinter.FiscalKkm.Atol10ConnectionConfig;
      fiscalKkm.Atol10ConnectionConfig = target6((CallSite) p11, obj6);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.OperatorLogin(cashier);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "openShift", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__21.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__21.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__21.\u003C\u003Ep__1, this.Driver);
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
          if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__22.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__2;
          object driver1 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_REPORT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__22.\u003C\u003Ep__0, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__22.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_CLOSE_SHIFT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__22.\u003C\u003Ep__1, this.Driver);
          target1((CallSite) p2, driver1, obj1, obj2);
          break;
        case ReportTypes.XReport:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__22.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__5;
          object driver2 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__22.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_REPORT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__22.\u003C\u003Ep__3, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__22.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_X", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Neva.\u003C\u003Eo__22.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__22.\u003C\u003Ep__4, this.Driver);
          target2((CallSite) p5, driver2, obj3, obj4);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__22.\u003C\u003Ep__6 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "report", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__22.\u003C\u003Ep__6.Target((CallSite) Neva.\u003C\u003Eo__22.\u003C\u003Ep__6, this.Driver);
      this.CheckResult();
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__22.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__22.\u003C\u003Ep__7 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__22.\u003C\u003Ep__7.Target((CallSite) Neva.\u003C\u003Eo__22.\u003C\u003Ep__7, this.Driver);
      this.CheckResult();
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._checkData = checkData;
      LogHelper.Debug("Открытие чека Нева");
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
            if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "utilFormTlv", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__0, this.Driver);
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__1, this.Driver, 1227, checkData.Client.Client.Name);
            string str = checkData.Client.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
            if (!str.IsNullOrEmpty())
            {
              // ISSUE: reference to a compiler-generated field
              if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__2 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Neva.\u003C\u003Eo__24.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__2, this.Driver, 1228, str);
            }
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "utilFormTlv", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__3, this.Driver);
            Thread.Sleep(1000);
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, byte[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (byte[]), typeof (Neva)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, byte[]> target1 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__6.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, byte[]>> p6 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__6;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamByteArray", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__5;
            object driver = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAG_VALUE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj1 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__4, this.Driver);
            object obj2 = target2((CallSite) p5, driver, obj1);
            byte[] numArray = target1((CallSite) p6, obj2);
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__24.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, int, byte[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__7.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__7, this.Driver, 1256, numArray);
          }
        }
        this.SendDigitalCheck(checkData.AddressForDigitalCheck);
      }
      object obj3;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_SELL", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__8.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__8, this.Driver);
          break;
        case CheckTypes.ReturnSale:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_SELL_RETURN", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__9.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__9, this.Driver);
          break;
        case CheckTypes.Buy:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_BUY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__10.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__10, this.Driver);
          break;
        case CheckTypes.ReturnBuy:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__24.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_BUY_RETURN", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj3 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__11.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__11, this.Driver);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      object obj4 = obj3;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__24.\u003C\u003Ep__13 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target3 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p13 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__13;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__24.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__12.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__12, this.Driver);
      object obj6 = obj4;
      target3((CallSite) p13, driver1, obj5, obj6);
      if (this.DevicesConfig.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck)
      {
        LogHelper.Debug("Выключаем печать бумажного чека Нева");
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__24.\u003C\u003Ep__15 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target4 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__15.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p15 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__15;
        object driver2 = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__24.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_ELECTRONICALLY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = Neva.\u003C\u003Eo__24.\u003C\u003Ep__14.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__14, this.Driver);
        target4((CallSite) p15, driver2, obj7, true);
      }
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__24.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__24.\u003C\u003Ep__16 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "openReceipt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__24.\u003C\u003Ep__16.Target((CallSite) Neva.\u003C\u003Eo__24.\u003C\u003Ep__16, this.Driver);
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
      if (Neva.\u003C\u003Eo__25.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__25.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "operatorLogin", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__25.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__25.\u003C\u003Ep__0, this.Driver);
      this.CheckResult();
    }

    [HandleProcessCorruptedStateExceptions]
    private void Ffd120CodeValidation(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      LogHelper.Debug("Начинаю валидацию КМ для ФФД 120, Нева 10");
      try
      {
        if (checkData.GoodsList.All<CheckGood>((Func<CheckGood, bool>) (x =>
        {
          MarkedInfo markedInfo = x.MarkedInfo;
          return (markedInfo != null ? (int) markedInfo.Type : 0) == 0;
        })))
          return;
        foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (item => item.MarkedInfo != null && !item.MarkedInfo.FullCode.IsNullOrEmpty())))
        {
          checkGood.MarkedInfo.ValidationResultKkm = (object) null;
          string fnC1 = DataMatrixHelper.ReplaceSomeCharsToFNC1(checkGood.MarkedInfo.FullCode);
          int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(checkGood, checkData.CheckType);
          LogHelper.Debug("Валидация кода: " + fnC1);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__2;
          object driver1 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__0, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_MCT12_AUTO", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__1, this.Driver);
          target1((CallSite) p2, driver1, obj1, obj2);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, string> target2 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, string>> p4 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__4;
          object driver2 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__3, this.Driver);
          string str = fnC1;
          target2((CallSite) p4, driver2, obj3, str);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__6 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, int> target3 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__6.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, int>> p6 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__6;
          object driver3 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_STATUS", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__5.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__5, this.Driver);
          int num = markingCodeStatus;
          target3((CallSite) p6, driver3, obj4, num);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, int> target4 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__8.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, int>> p8 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__8;
          object driver4 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_PROCESSING_MODE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__7.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__7, this.Driver);
          target4((CallSite) p8, driver4, obj5, 0);
          if (markingCodeStatus.IsEither<int>(2, 4))
          {
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, double> target5 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__10.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, double>> p10 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__10;
            object driver5 = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_QUANTITY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj6 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__9.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__9, this.Driver);
            double quantity = (double) checkGood.Quantity;
            target5((CallSite) p10, driver5, obj6, quantity);
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__12 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target6 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__12.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p12 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__12;
            object driver6 = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MEASUREMENT_UNIT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__11.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__11, this.Driver);
            int ruFfdUnitsIndex1 = checkGood.Unit.RuFfdUnitsIndex;
            target6((CallSite) p12, driver6, obj7, ruFfdUnitsIndex1);
            int ruFfdUnitsIndex2 = checkGood.Unit.RuFfdUnitsIndex;
          }
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__13 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "beginMarkingCodeValidation", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__26.\u003C\u003Ep__13.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__13, this.Driver);
          Func<CallSite, object, bool> target7;
          CallSite<Func<CallSite, object, bool>> p18;
          object obj8;
          do
          {
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__14 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "getMarkingCodeValidationStatus", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__14.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__14, this.Driver);
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__18 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            target7 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__18.Target;
            // ISSUE: reference to a compiler-generated field
            p18 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__18;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__17 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target8 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__17.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p17 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__17;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__16 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamBool", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target9 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__16.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p16 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__16;
            object driver7 = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__15 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__26.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_VALIDATION_READY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj9 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__15.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__15, this.Driver);
            object obj10 = target9((CallSite) p16, driver7, obj9);
            obj8 = target8((CallSite) p17, obj10, 1);
          }
          while (!target7((CallSite) p18, obj8));
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target10 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__20.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p20 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__20;
          object driver8 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_ONLINE_VALIDATION_RESULT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = Neva.\u003C\u003Eo__26.\u003C\u003Ep__19.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__19, this.Driver);
          object obj12 = target10((CallSite) p20, driver8, obj11);
          LogHelper.Debug(string.Format("Validation ready: {0}; result code: {1}", obj12, checkGood.MarkedInfo.ValidationResultKkm));
          MarkedInfo markedInfo = checkGood.MarkedInfo;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: variable of a boxed type
          __Boxed<int> local = (ValueType) Neva.\u003C\u003Eo__26.\u003C\u003Ep__21.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__21, obj12);
          markedInfo.ValidationResultKkm = (object) local;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__26.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__26.\u003C\u003Ep__22 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "acceptMarkingCode", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__26.\u003C\u003Ep__22.Target((CallSite) Neva.\u003C\u003Eo__26.\u003C\u003Ep__22, this.Driver);
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
      if (Neva.\u003C\u003Eo__27.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__27.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "closeReceipt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__27.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__27.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__27.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__27.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__27.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__27.\u003C\u003Ep__1, this.Driver);
      this.CheckResult();
      return true;
    }

    public void CancelCheck()
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__28.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__28.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cancelReceipt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__28.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__28.\u003C\u003Ep__0, this.Driver);
      this.CheckResult();
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__29.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__29.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target = Neva.\u003C\u003Eo__29.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p1 = Neva.\u003C\u003Eo__29.\u003C\u003Ep__1;
      object driver = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__29.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__29.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SUM", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Neva.\u003C\u003Eo__29.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__29.\u003C\u003Ep__0, this.Driver);
      double num = (double) sum;
      target((CallSite) p1, driver, obj, num);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__29.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__29.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cashOutcome", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__29.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__29.\u003C\u003Ep__2, this.Driver);
      this.CheckResult();
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__30.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__30.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target = Neva.\u003C\u003Eo__30.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p1 = Neva.\u003C\u003Eo__30.\u003C\u003Ep__1;
      object driver = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__30.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__30.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SUM", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Neva.\u003C\u003Eo__30.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__30.\u003C\u003Ep__0, this.Driver);
      double num = (double) sum;
      target((CallSite) p1, driver, obj, num);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__30.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__30.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cashIncome", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__30.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__30.\u003C\u003Ep__2, this.Driver);
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
      if (Neva.\u003C\u003Eo__32.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__32.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__2;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__32.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__32.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__32.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__32.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__32.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_CASH_SUM", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__32.\u003C\u003Ep__1, this.Driver);
      target1((CallSite) p2, driver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__32.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__32.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__32.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__32.\u003C\u003Ep__3, this.Driver);
      this.CheckResult();
      ref Decimal local = ref sum;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__32.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__32.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Decimal), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target2 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p6 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__32.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__32.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDouble", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target3 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__5;
      object driver2 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__32.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__32.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SUM", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__32.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__32.\u003C\u003Ep__4, this.Driver);
      object obj4 = target3((CallSite) p5, driver2, obj3);
      Decimal num = target2((CallSite) p6, obj4);
      local = num;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      LogHelper.Debug("Нева 10: 100");
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__2;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAX_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_NO", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__1, this.Driver);
      target1((CallSite) p2, driver1, obj1, obj2);
      Decimal num1 = Math.Round(good.Sum / good.Quantity, 2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, string> target2 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, string>> p4 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__4;
      object driver2 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_COMMODITY_NAME", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__3, this.Driver);
      string name = good.Name;
      target2((CallSite) p4, driver2, obj3, name);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__6 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target3 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p6 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__6;
      object driver3 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PRICE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__5.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__5, this.Driver);
      double num2 = (double) num1;
      target3((CallSite) p6, driver3, obj4, num2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target4 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__8;
      object driver4 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_QUANTITY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__7.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__7, this.Driver);
      double quantity = (double) good.Quantity;
      target4((CallSite) p8, driver4, obj5, quantity);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target5 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p10 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__10;
      object driver5 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DEPARTMENT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__9.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__9, this.Driver);
      int kkmSectionNumber = good.KkmSectionNumber;
      target5((CallSite) p10, driver5, obj6, kkmSectionNumber);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__12 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target6 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__12.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p12 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__12;
      object driver6 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_INFO_DISCOUNT_SUM", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__11.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__11, this.Driver);
      double discountSum = (double) good.DiscountSum;
      target6((CallSite) p12, driver6, obj7, discountSum);
      object obj8;
      switch (good.TaxRateNumber)
      {
        case 1:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_NO", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__13.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__13, this.Driver);
          break;
        case 2:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT0", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__14.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__14, this.Driver);
          break;
        case 3:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT10", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__15.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__15, this.Driver);
          break;
        case 4:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT20", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__16.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__16, this.Driver);
          break;
        case 5:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT110", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__17.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__17, this.Driver);
          break;
        case 6:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT120", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__18.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__18, this.Driver);
          break;
        case 7:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT5", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__19.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__19, this.Driver);
          break;
        case 8:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT7", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__20.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__20, this.Driver);
          break;
        case 9:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT105", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__21.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__21, this.Driver);
          break;
        case 10:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_VAT107", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__22.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__22, this.Driver);
          break;
        default:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_TAX_NO", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj8 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__23.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__23, this.Driver);
          break;
      }
      object obj9 = obj8;
      LogHelper.Debug("Нева 10: 110");
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__25 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target7 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__25.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p25 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__25;
      object driver7 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAX_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = Neva.\u003C\u003Eo__33.\u003C\u003Ep__24.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__24, this.Driver);
      object obj11 = obj9;
      target7((CallSite) p25, driver7, obj10, obj11);
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      GlobalDictionaries.RuFfdGoodsTypes ruFfdGoodsTypes = good.RuFfdGoodTypeCode == GlobalDictionaries.RuFfdGoodsTypes.None ? GlobalDictionaries.RuFfdGoodsTypes.SimpleGood : good.RuFfdGoodTypeCode;
      LogHelper.Debug("Нева 10: 120");
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__26 = CallSite<Action<CallSite, object, int, GlobalDictionaries.RuFfdGoodsTypes>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__33.\u003C\u003Ep__26.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__26, this.Driver, 1212, ruFfdGoodsTypes);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__33.\u003C\u003Ep__27 = CallSite<Action<CallSite, object, int, GlobalDictionaries.RuFfdPaymentModes>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__33.\u003C\u003Ep__27.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__27, this.Driver, 1214, good.RuFfdPaymentModeCode);
      switch (devices.CheckPrinter.FiscalKkm.FfdVersion)
      {
        case GlobalDictionaries.Devices.FfdVersions.OfflineKkm:
        case GlobalDictionaries.Devices.FfdVersions.Ffd100:
          LogHelper.Debug("Нева 10: 130");
          try
          {
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__33.\u003C\u003Ep__28 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__33.\u003C\u003Ep__28 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "registration", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__33.\u003C\u003Ep__28.Target((CallSite) Neva.\u003C\u003Eo__33.\u003C\u003Ep__28, this.Driver);
          }
          catch (Exception ex)
          {
            LogHelper.Error(ex, "ОШИБКА РЕГИСТРАЦИИ Нева", false);
            throw new KkmException((IDevice) this, "ОШИБКА РЕГИСТРАЦИИ Нева");
          }
          LogHelper.Debug("Нева 10: 140");
          this.CheckResult();
          this.PrintNonFiscalStrings(good.CommentForFiscalCheck.Select<string, NonFiscalString>((Func<string, NonFiscalString>) (x => new NonFiscalString(x))).ToList<NonFiscalString>());
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
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target1 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p1 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__1;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MEASUREMENT_UNIT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__0, this.Driver);
      int ruFfdUnitsIndex = good.Unit.RuFfdUnitsIndex;
      target1((CallSite) p1, driver1, obj1, ruFfdUnitsIndex);
      string str1 = this.PrepareMarkCodeForFfd120(good.MarkedInfo?.FullCode ?? string.Empty);
      if (good.MarkedInfo == null || str1.IsNullOrEmpty())
        return;
      if (this._industryInfo != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__34.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, byte[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__2, this.Driver, 1260, this._industryInfo);
      }
      LogHelper.Debug(string.Format("Запись тега 2106: {0}", good.MarkedInfo.ValidationResultKkm));
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, string> target2 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, string>> p4 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__4;
      object driver2 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__3, this.Driver);
      string str2 = str1;
      target2((CallSite) p4, driver2, obj2, str2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__6 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target3 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p6 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__6;
      object driver3 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_STATUS", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__5.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__5, this.Driver);
      int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(good, this._checkData.CheckType);
      target3((CallSite) p6, driver3, obj3, markingCodeStatus);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, int> target4 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, int>> p8 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__8;
      object driver4 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_PROCESSING_MODE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__7.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__7, this.Driver);
      target4((CallSite) p8, driver4, obj4, 0);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__11 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target5 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p11 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__11;
      object driver5 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__9.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__9, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_MCT12_AUTO", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__10.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__10, this.Driver);
      target5((CallSite) p11, driver5, obj5, obj6);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__13 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target6 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p13 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__13;
      object driver6 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__34.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__34.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MARKING_CODE_ONLINE_VALIDATION_RESULT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = Neva.\u003C\u003Eo__34.\u003C\u003Ep__12.Target((CallSite) Neva.\u003C\u003Eo__34.\u003C\u003Ep__12, this.Driver);
      object obj8 = good.MarkedInfo.ValidationResultKkm ?? (object) 0;
      target6((CallSite) p13, driver6, obj7, obj8);
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
      if (Neva.\u003C\u003Eo__35.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__35.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParamStrHex", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__35.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__35.\u003C\u003Ep__0, this.Driver, 1162, hexStringAttribute);
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__2;
          object driver1 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__0, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_CASH", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__1, this.Driver);
          target1((CallSite) p2, driver1, obj1, obj2);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__5;
          object driver2 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__3, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_ELECTRONICALLY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__4, this.Driver);
          target2((CallSite) p5, driver2, obj3, obj4);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__8 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target3 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__8.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p8 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__8;
          object driver3 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__6.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__6, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_6", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__7.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__7, this.Driver);
          target3((CallSite) p8, driver3, obj5, obj6);
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__11 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target4 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__11.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p11 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__11;
          object driver4 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__9.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__9, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_CREDIT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__10.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__10, this.Driver);
          target4((CallSite) p11, driver4, obj7, obj8);
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target5 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__14.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p14 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__14;
          object driver5 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj9 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__12.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__12, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__36.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PT_PREPAID", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj10 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__13.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__13, this.Driver);
          target5((CallSite) p14, driver5, obj9, obj10);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__36.\u003C\u003Ep__16 = CallSite<Action<CallSite, object, object, double>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, double> target6 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, double>> p16 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__16;
      object driver6 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__36.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_PAYMENT_SUM", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = Neva.\u003C\u003Eo__36.\u003C\u003Ep__15.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__15, this.Driver);
      double sum = (double) payment.Sum;
      target6((CallSite) p16, driver6, obj11, sum);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__36.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__36.\u003C\u003Ep__17 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (payment), (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__36.\u003C\u003Ep__17.Target((CallSite) Neva.\u003C\u003Eo__36.\u003C\u003Ep__17, this.Driver);
      this.CheckResult();
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      this.PrintNonFiscalStrings(new List<NonFiscalString>()
      {
        new NonFiscalString(string.Format("{0}: {1:N2}", (object) description, (object) sum), TextAlignment.Right)
      });
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      LogHelper.Debug("Подключение к ККМ НЕВА");
      this.Driver = Functions.CreateObject("AddIn.FiscalCore1");
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__38.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__38.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Neva.\u003C\u003Eo__38.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Neva.\u003C\u003Eo__38.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__38.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__38.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Neva.\u003C\u003Eo__38.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__38.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target((CallSite) p1, obj))
        throw new NullReferenceException("Объект драйвера НЕВА-03-Ф не был создан.");
      string connectionConfig = this.DevicesConfig.CheckPrinter.FiscalKkm.Atol10ConnectionConfig;
      if (!connectionConfig.IsNullOrEmpty())
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__38.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__38.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setSettings", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__38.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__38.\u003C\u003Ep__2, this.Driver, connectionConfig);
      }
      if (onlyDriverLoad)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__38.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__38.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "open", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__38.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__38.\u003C\u003Ep__3, this.Driver);
      this.CheckResult();
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__39.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__39.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Neva.\u003C\u003Eo__39.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Neva.\u003C\u003Eo__39.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__39.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__39.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Neva.\u003C\u003Eo__39.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__39.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target((CallSite) p1, obj))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__39.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__39.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "close", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__39.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__39.\u003C\u003Ep__2, this.Driver);
      this.CheckResult();
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__39.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__39.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReleaseComObject", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__39.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__39.\u003C\u003Ep__3, typeof (Marshal), this.Driver);
      this.Driver = (object) null;
      return true;
    }

    public bool IsConnected
    {
      get
      {
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__1 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target1 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__1.Target;
        CallSite<Func<CallSite, object, bool>> p1 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__1;
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__0 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        object obj1 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__41.\u003C\u003Ep__0, this.Driver, (object) null);
        if (target1((CallSite) p1, obj1))
          return false;
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__4 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__4 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Action<CallSite, System.Type, object> target2 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__4.Target;
        CallSite<Action<CallSite, System.Type, object>> p4 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__4;
        System.Type type = typeof (LogHelper);
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__3 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__3 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, string, object, object> target3 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__3.Target;
        CallSite<Func<CallSite, string, object, object>> p3 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__3;
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__2 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "isOpened", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj2 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__41.\u003C\u003Ep__2, this.Driver);
        object obj3 = target3((CallSite) p3, "Driver.isOpened() = ", obj2);
        target2((CallSite) p4, type, obj3);
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__7 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (Neva)));
        Func<CallSite, object, bool> target4 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__7.Target;
        CallSite<Func<CallSite, object, bool>> p7 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__7;
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__6 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        Func<CallSite, object, int, object> target5 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__6.Target;
        CallSite<Func<CallSite, object, int, object>> p6 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__6;
        if (Neva.\u003C\u003Eo__41.\u003C\u003Ep__5 == null)
          Neva.\u003C\u003Eo__41.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "isOpened", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        object obj4 = Neva.\u003C\u003Eo__41.\u003C\u003Ep__5.Target((CallSite) Neva.\u003C\u003Eo__41.\u003C\u003Ep__5, this.Driver);
        object obj5 = target5((CallSite) p6, obj4, 1);
        return target4((CallSite) p7, obj5);
      }
      set
      {
      }
    }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, string> target1 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, string>> p1 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__1;
        object driver1 = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TEXT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__0, this.Driver);
        string text = nonFiscalString.Text;
        target1((CallSite) p1, driver1, obj1, text);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target2 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p3 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__3;
        object driver2 = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FONT_DOUBLE_WIDTH", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__2, this.Driver);
        int num1 = nonFiscalString.WideFont ? 1 : 0;
        target2((CallSite) p3, driver2, obj2, num1 != 0);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, object, bool>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, bool> target3 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, bool>> p5 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__5;
        object driver3 = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FONT_DOUBLE_HEIGHT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__4, this.Driver);
        int num2 = nonFiscalString.WideFont ? 1 : 0;
        target3((CallSite) p5, driver3, obj3, num2 != 0);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, object, int> target4 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, object, int>> p7 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__7;
        object driver4 = this.Driver;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TEXT_WRAP", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__6.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__6, this.Driver);
        target4((CallSite) p7, driver4, obj4, 1);
        switch (nonFiscalString.Alignment)
        {
          case TextAlignment.Left:
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__9 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target5 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__9.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p9 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__9;
            object driver5 = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__8.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__8, this.Driver);
            target5((CallSite) p9, driver5, obj5, 0);
            break;
          case TextAlignment.Right:
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__11 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target6 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__11.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p11 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__11;
            object driver6 = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj6 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__10.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__10, this.Driver);
            target6((CallSite) p11, driver6, obj6, 2);
            break;
          case TextAlignment.Center:
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__13 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__13 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target7 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__13.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p13 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__13;
            object driver7 = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__12.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__12, this.Driver);
            target7((CallSite) p13, driver7, obj7, 1);
            break;
          default:
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__15 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__15 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, object, object, int> target8 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__15.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, object, object, int>> p15 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__15;
            object driver8 = this.Driver;
            // ISSUE: reference to a compiler-generated field
            if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Neva.\u003C\u003Eo__43.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj8 = Neva.\u003C\u003Eo__43.\u003C\u003Ep__14.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__14, this.Driver);
            target8((CallSite) p15, driver8, obj8, 0);
            break;
        }
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__43.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__43.\u003C\u003Ep__16 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "printText", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__43.\u003C\u003Ep__16.Target((CallSite) Neva.\u003C\u003Eo__43.\u003C\u003Ep__16, this.Driver);
        this.CheckResult();
      }
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__44.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, string> target1 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, string>> p1 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__1;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__44.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_BARCODE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__0, this.Driver);
      string str = code;
      target1((CallSite) p1, driver1, obj1, str);
      switch (type)
      {
        case BarcodeTypes.None:
          return true;
        case BarcodeTypes.Ean13:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p4 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__4;
          object driver2 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_BARCODE_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__2, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_BT_EAN_13", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__3, this.Driver);
          target2((CallSite) p4, driver2, obj2, obj3);
          break;
        case BarcodeTypes.QrCode:
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target3 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p7 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__7;
          object driver3 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_BARCODE_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__5.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__5, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_BT_QR", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__6.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__6, this.Driver);
          target3((CallSite) p7, driver3, obj4, obj5);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, object> target4 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__10.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, object>> p10 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__10;
          object driver4 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_ALIGNMENT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__8.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__8, this.Driver);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_ALIGNMENT_CENTER", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__9.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__9, this.Driver);
          target4((CallSite) p10, driver4, obj6, obj7);
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__12 = CallSite<Action<CallSite, object, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, object, int> target5 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__12.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, object, int>> p12 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__12;
          object driver5 = this.Driver;
          // ISSUE: reference to a compiler-generated field
          if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Neva.\u003C\u003Eo__44.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SCALE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = Neva.\u003C\u003Eo__44.\u003C\u003Ep__11.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__11, this.Driver);
          target5((CallSite) p12, driver5, obj8, 7);
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__44.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__44.\u003C\u003Ep__13 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "printBarcode", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__44.\u003C\u003Ep__13.Target((CallSite) Neva.\u003C\u003Eo__44.\u003C\u003Ep__13, this.Driver);
      this.CheckResult();
      return true;
    }

    public bool CutPaper()
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__45.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__45.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "cut", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__45.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__45.\u003C\u003Ep__0, this.Driver);
      return true;
    }

    public KkmStatus GetStatus()
    {
      KkmStatus status = new KkmStatus()
      {
        KkmState = this.GetKkmState()
      };
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__2;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_SHIFT_STATE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__1, this.Driver);
      target1((CallSite) p2, driver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__46.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__3, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__5;
      object driver2 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SHIFT_STATE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__4, this.Driver);
      if (target2((CallSite) p5, driver2, obj3) is int num1)
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
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target3 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p8 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p7 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__7;
      object driver3 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SHIFT_NUMBER", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__6.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__6, this.Driver);
      object obj5 = target4((CallSite) p7, driver3, obj4);
      int num2 = target3((CallSite) p8, obj5);
      kkmStatus1.SessionNumber = num2;
      KkmStatus kkmStatus2 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (DateTime), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target5 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p11 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p10 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__10;
      object driver4 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__9.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__9, this.Driver);
      object obj7 = target6((CallSite) p10, driver4, obj6);
      DateTime? nullable1 = new DateTime?(target5((CallSite) p11, obj7).AddHours(-24.0));
      kkmStatus2.SessionStarted = nullable1;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target7 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p14 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__14;
      object driver5 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__12.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__12, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_RECEIPT_STATE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__13.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__13, this.Driver);
      target7((CallSite) p14, driver5, obj8, obj9);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__15 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__46.\u003C\u003Ep__15.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__15, this.Driver);
      KkmStatus kkmStatus3 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target8 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__21.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p21 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__21;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target9 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__20.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p20 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__20;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target10 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p17 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__17;
      object driver6 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__16.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__16, this.Driver);
      object obj11 = target10((CallSite) p17, driver6, obj10);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target11 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p19 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__19;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_CLOSED", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__18.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__18, this.Driver);
      int num3 = target11((CallSite) p19, obj12);
      object obj13 = target9((CallSite) p20, obj11, num3);
      int num4 = target8((CallSite) p21, obj13) ? 2 : 1;
      kkmStatus3.CheckStatus = (CheckStatuses) num4;
      KkmStatus kkmStatus4 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target12 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__24.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p24 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__24;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target13 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__23.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p23 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__23;
      object driver7 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_NUMBER", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__22.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__22, this.Driver);
      object obj15 = target13((CallSite) p23, driver7, obj14);
      int num5 = target12((CallSite) p24, obj15) + 1;
      kkmStatus4.CheckNumber = num5;
      KkmStatus kkmStatus5 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__27 = CallSite<Func<CallSite, System.Type, object, Version>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, Version> target14 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__27.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, Version>> p27 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__27;
      System.Type type = typeof (Version);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target15 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__26.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p26 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__26;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "version", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj16 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__25.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__25, this.Driver);
      object obj17 = target15((CallSite) p26, obj16);
      Version version = target14((CallSite) p27, type, obj17);
      kkmStatus5.DriverVersion = version;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__30 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target16 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__30.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p30 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__30;
      object driver8 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj18 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__28.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__28, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_STATUS", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj19 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__29.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__29, this.Driver);
      target16((CallSite) p30, driver8, obj18, obj19);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__31 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__46.\u003C\u003Ep__31.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__31, this.Driver);
      KkmStatus kkmStatus6 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target17 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__34.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p34 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__34;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamString", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target18 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__33.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p33 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__33;
      object driver9 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SERIAL_NUMBER", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj20 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__32.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__32, this.Driver);
      object obj21 = target18((CallSite) p33, driver9, obj20);
      string str1 = target17((CallSite) p34, obj21);
      kkmStatus6.FactoryNumber = str1;
      KkmStatus kkmStatus7 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__37 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__37 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target19 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__37.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p37 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__37;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamString", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target20 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__36.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p36 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__36;
      object driver10 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_MODEL_NAME", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj22 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__35.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__35, this.Driver);
      object obj23 = target20((CallSite) p36, driver10, obj22);
      string str2 = target19((CallSite) p37, obj23);
      kkmStatus7.Model = str2;
      KkmStatus kkmStatus8 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__40 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__40 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target21 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__40.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p40 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__40;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__39 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__39 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamString", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target22 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__39.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p39 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__39;
      object driver11 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__38 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_UNIT_VERSION", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj24 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__38.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__38, this.Driver);
      object obj25 = target22((CallSite) p39, driver11, obj24);
      string str3 = target21((CallSite) p40, obj25);
      kkmStatus8.SoftwareVersion = str3;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__43 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__43 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target23 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__43.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p43 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__43;
      object driver12 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__41 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__41 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj26 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__41.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__41, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__42 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__42 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_OFD_EXCHANGE_STATUS", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj27 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__42.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__42, this.Driver);
      target23((CallSite) p43, driver12, obj26, obj27);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__44 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__44 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "fnQueryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__46.\u003C\u003Ep__44.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__44, this.Driver);
      KkmStatus kkmStatus9 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__47 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__47 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target24 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__47.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p47 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__47;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__46 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__46 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target25 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__46.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p46 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__46;
      object driver13 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__45 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__45 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DOCUMENTS_COUNT", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj28 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__45.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__45, this.Driver);
      object obj29 = target25((CallSite) p46, driver13, obj28);
      int num6 = target24((CallSite) p47, obj29);
      kkmStatus9.OfdNotSendDocuments = num6;
      KkmStatus kkmStatus10 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__50 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__50 = CallSite<Func<CallSite, object, DateTime?>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime?), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime?> target26 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__50.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime?>> p50 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__50;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__49 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__49 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target27 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__49.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p49 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__49;
      object driver14 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__48 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__48 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj30 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__48.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__48, this.Driver);
      object obj31 = target27((CallSite) p49, driver14, obj30);
      DateTime? nullable2 = target26((CallSite) p50, obj31);
      kkmStatus10.OfdLastSendDateTime = nullable2;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__53 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__53 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target28 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__53.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p53 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__53;
      object driver15 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__51 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__51 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj32 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__51.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__51, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__52 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__52 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_VALIDITY", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj33 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__52.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__52, this.Driver);
      target28((CallSite) p53, driver15, obj32, obj33);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__54 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__54 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "fnQueryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__46.\u003C\u003Ep__54.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__54, this.Driver);
      KkmStatus kkmStatus11 = status;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__57 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__57 = CallSite<Func<CallSite, object, DateTime>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, DateTime> target29 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__57.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, DateTime>> p57 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__57;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__56 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__56 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamDateTime", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target30 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__56.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p56 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__56;
      object driver16 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__55 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__55 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATE_TIME", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj34 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__55.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__55, this.Driver);
      object obj35 = target30((CallSite) p56, driver16, obj34);
      DateTime dateTime = target29((CallSite) p57, obj35);
      kkmStatus11.FnDateEnd = dateTime;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__59 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__59 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target31 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__59.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p59 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__59;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__58 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__58 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj36 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__58.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__58, this.Driver);
      if (target31((CallSite) p59, obj36) == 82)
      {
        LogHelper.Debug("Ошибка 82. Пытаемся закрыть прошлый документ");
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__60 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__46.\u003C\u003Ep__60 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "closeReceipt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__60.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__60, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__61 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__46.\u003C\u003Ep__61 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__46.\u003C\u003Ep__61.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__61, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__63 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__46.\u003C\u003Ep__63 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target32 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__63.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p63 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__63;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__46.\u003C\u003Ep__62 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__46.\u003C\u003Ep__62 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj37 = Neva.\u003C\u003Eo__46.\u003C\u003Ep__62.Target((CallSite) Neva.\u003C\u003Eo__46.\u003C\u003Ep__62, this.Driver);
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
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__2;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_DT_SHIFT_STATE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__1, this.Driver);
      target1((CallSite) p2, driver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__47.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__3, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__5;
      object driver2 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_SHIFT_STATE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__4, this.Driver);
      if (target2((CallSite) p5, driver2, obj3) is int num1)
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
      KkmStatus kkmStatus = shortStatus;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p11 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target4 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p10 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target5 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p7 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__7;
      object driver3 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_RECEIPT_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__6.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__6, this.Driver);
      object obj5 = target5((CallSite) p7, driver3, obj4);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target6 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p9 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_RT_CLOSED", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__8.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__8, this.Driver);
      int num2 = target6((CallSite) p9, obj6);
      object obj7 = target4((CallSite) p10, obj5, num2);
      int num3 = target3((CallSite) p11, obj7) ? 2 : 1;
      kkmStatus.CheckStatus = (CheckStatuses) num3;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target7 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p13 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__12.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__12, this.Driver);
      if (target7((CallSite) p13, obj8) == 82)
      {
        LogHelper.Debug("Ошибка 82. Пытаемся закрыть прошлый документ");
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__47.\u003C\u003Ep__14 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "closeReceipt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__14.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__14, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__47.\u003C\u003Ep__15 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "checkDocumentClosed", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__47.\u003C\u003Ep__15.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__15, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__17 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__47.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Neva)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target8 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__17.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p17 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__17;
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__47.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__47.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "errorCode", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = Neva.\u003C\u003Eo__47.\u003C\u003Ep__16.Target((CallSite) Neva.\u003C\u003Eo__47.\u003C\u003Ep__16, this.Driver);
        LogHelper.Debug("Результат закрытия: " + target8((CallSite) p17, obj9).ToString());
      }
      return shortStatus;
    }

    public bool OpenCashDrawer()
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__48.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__48.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "openDrawer", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__48.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__48.\u003C\u003Ep__0, this.Driver);
      return true;
    }

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__49.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__49.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, object, object> target1 = Neva.\u003C\u003Eo__49.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, object, object>> p2 = Neva.\u003C\u003Eo__49.\u003C\u003Ep__2;
      object driver1 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__49.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__49.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FN_DATA_TYPE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__49.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__49.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__49.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__49.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_FNDT_FFD_VERSIONS", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Neva.\u003C\u003Eo__49.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__49.\u003C\u003Ep__1, this.Driver);
      target1((CallSite) p2, driver1, obj1, obj2);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__49.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__49.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "queryData", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__49.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__49.\u003C\u003Ep__3, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__49.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__49.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamInt", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__49.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = Neva.\u003C\u003Eo__49.\u003C\u003Ep__5;
      object driver2 = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__49.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__49.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_FFD_VERSION", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Neva.\u003C\u003Eo__49.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__49.\u003C\u003Ep__4, this.Driver);
      if (target2((CallSite) p5, driver2, obj3) is int num)
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
      for (int index = 0; index < lines; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (Neva.\u003C\u003Eo__50.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Neva.\u003C\u003Eo__50.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "lineFeed", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__50.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__50.\u003C\u003Ep__0, this.Driver);
      }
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
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__53.\u003C\u003Ep__0.Target((CallSite) Neva.\u003C\u003Eo__53.\u003C\u003Ep__0, this.Driver, 1262, "030");
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__53.\u003C\u003Ep__1.Target((CallSite) Neva.\u003C\u003Eo__53.\u003C\u003Ep__1, this.Driver, 1263, "21.11.2023");
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__53.\u003C\u003Ep__2.Target((CallSite) Neva.\u003C\u003Eo__53.\u003C\u003Ep__2, this.Driver, 1264, "1944");
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "setParam", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__53.\u003C\u003Ep__3.Target((CallSite) Neva.\u003C\u003Eo__53.\u003C\u003Ep__3, this.Driver, 1265, info);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__4 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "utilFormTlv", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Neva.\u003C\u003Eo__53.\u003C\u003Ep__4.Target((CallSite) Neva.\u003C\u003Eo__53.\u003C\u003Ep__4, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, byte[]>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (byte[]), typeof (Neva)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, byte[]> target1 = Neva.\u003C\u003Eo__53.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, byte[]>> p7 = Neva.\u003C\u003Eo__53.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "getParamByteArray", (IEnumerable<System.Type>) null, typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = Neva.\u003C\u003Eo__53.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p6 = Neva.\u003C\u003Eo__53.\u003C\u003Ep__6;
      object driver = this.Driver;
      // ISSUE: reference to a compiler-generated field
      if (Neva.\u003C\u003Eo__53.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Neva.\u003C\u003Eo__53.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LIBFPTR_PARAM_TAG_VALUE", typeof (Neva), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Neva.\u003C\u003Eo__53.\u003C\u003Ep__5.Target((CallSite) Neva.\u003C\u003Eo__53.\u003C\u003Ep__5, this.Driver);
      object obj2 = target2((CallSite) p6, driver, obj1);
      this._industryInfo = target1((CallSite) p7, obj2);
    }
  }
}
