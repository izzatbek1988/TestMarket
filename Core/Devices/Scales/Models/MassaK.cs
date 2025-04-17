// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.MassaK
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
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
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.Scales.Models
{
  public class MassaK : IScale, IDevice
  {
    private GlobalDictionaries.Devices.ScaleTypes _type { get; set; }

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    private object Driver { get; set; }

    public MassaK(Gbs.Core.Config.Devices devicesConfig, GlobalDictionaries.Devices.ScaleTypes type)
    {
      this.DevicesConfig = devicesConfig;
      this._type = type;
    }

    public void Connect(bool onlyDriverLoad = false)
    {
      LogHelper.Debug("Начинаю подключение к весам Масса-К");
      this.Driver = Functions.CreateObject(this._type == GlobalDictionaries.Devices.ScaleTypes.MassaK100 ? "MassaKDriver100.Scales" : "ScalesMassaK.Scale");
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) MassaK.\u003C\u003Eo__13.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.Atol10_Объект_драйвера_не_был_создан);
      if (onlyDriverLoad)
        return;
      ComPort comPort = this.DevicesConfig.Scale.ComPort;
      DeviceHelper.CheckComPortExists(comPort.PortName, (IDevice) this);
      LogHelper.Debug("Параметры com-порта: " + comPort.ToJsonString());
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Connection", typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__2.Target((CallSite) MassaK.\u003C\u003Eo__13.\u003C\u003Ep__2, this.Driver, comPort.PortName);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "OpenConnection", typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__3.Target((CallSite) MassaK.\u003C\u003Eo__13.\u003C\u003Ep__3, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__4.Target((CallSite) MassaK.\u003C\u003Eo__13.\u003C\u003Ep__4, obj3, 0);
      if (target2((CallSite) p5, obj4))
      {
        LogHelper.Debug("Подклчение к весам Масса-К прошло успешно");
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__13.\u003C\u003Ep__6 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__6.Target((CallSite) MassaK.\u003C\u003Eo__13.\u003C\u003Ep__6, Translate.MassaK_Connect_Возникла_ошибка_подключения_к_весам_Масса_К__код__, obj3);
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__13.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__13.\u003C\u003Ep__7 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__7.Target((CallSite) MassaK.\u003C\u003Eo__13.\u003C\u003Ep__7, typeof (LogHelper), obj5);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__13.\u003C\u003Ep__8 = MassaK.\u003C\u003Eo__13.\u003C\u003Ep__8 == null ? CallSite<Func<CallSite, System.Type, object, MassaK, DeviceException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        })) : throw MassaK.\u003C\u003Eo__13.\u003C\u003Ep__8.Target((CallSite) MassaK.\u003C\u003Eo__13.\u003C\u003Ep__8, typeof (DeviceException), obj5, this);
      }
    }

    public bool Disconnect()
    {
      LogHelper.Debug("Отключение от весов Масса-К...");
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) MassaK.\u003C\u003Eo__14.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
      {
        LogHelper.Debug("Объект драйвера весов Масса К = null");
        return true;
      }
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CloseConnection", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__2.Target((CallSite) MassaK.\u003C\u003Eo__14.\u003C\u003Ep__2, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReleaseComObject", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      MassaK.\u003C\u003Eo__14.\u003C\u003Ep__3.Target((CallSite) MassaK.\u003C\u003Eo__14.\u003C\u003Ep__3, typeof (Marshal), this.Driver);
      this.Driver = (object) null;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__4.Target((CallSite) MassaK.\u003C\u003Eo__14.\u003C\u003Ep__4, obj2, 0);
      if (target2((CallSite) p5, obj3))
      {
        LogHelper.Debug("Отключение от весов Масса-К прошло успешно");
        return true;
      }
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target3 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p7 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__7;
      System.Type type = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = MassaK.\u003C\u003Eo__14.\u003C\u003Ep__6.Target((CallSite) MassaK.\u003C\u003Eo__14.\u003C\u003Ep__6, "Возникла ошибка отключения от весов Масса-К, код: ", obj2);
      target3((CallSite) p7, type, obj4);
      return false;
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadWeight", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) MassaK.\u003C\u003Eo__15.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__1.Target((CallSite) MassaK.\u003C\u003Eo__15.\u003C\u003Ep__1, obj1, 0);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__15.\u003C\u003Ep__4 = CallSite<Func<CallSite, System.Type, object, InvalidOperationException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, InvalidOperationException> target2 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, InvalidOperationException>> p4 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__4;
        System.Type type = typeof (InvalidOperationException);
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__15.\u003C\u003Ep__3 = CallSite<Func<CallSite, System.Type, string, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__3.Target((CallSite) MassaK.\u003C\u003Eo__15.\u003C\u003Ep__3, typeof (string), Translate.MassaK_Не_удалось_получить_вес_с_Масса_К__Код_ошибки__0_, obj1);
        throw target2((CallSite) p4, type, obj3);
      }
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p8 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target4 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p7 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__6 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToInt32", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, object> target5 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, object>> p6 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__6;
      System.Type type1 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Stable", typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__5.Target((CallSite) MassaK.\u003C\u003Eo__15.\u003C\u003Ep__5, this.Driver);
      object obj5 = target5((CallSite) p6, type1, obj4);
      object obj6 = target4((CallSite) p7, obj5, 1);
      if (target3((CallSite) p8, obj6))
        throw new ArgumentException(Translate.AtolShtrihM_Вес_не_стабилен);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (MassaK)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target6 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p11 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__10 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, object> target7 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, object>> p10 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__10;
      System.Type type2 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Weight", typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = MassaK.\u003C\u003Eo__15.\u003C\u003Ep__9.Target((CallSite) MassaK.\u003C\u003Eo__15.\u003C\u003Ep__9, this.Driver);
      object obj8 = target7((CallSite) p10, type2, obj7);
      Decimal num1 = target6((CallSite) p11, obj8);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__15.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Division", typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      Decimal num2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__15.\u003C\u003Ep__12.Target((CallSite) MassaK.\u003C\u003Eo__15.\u003C\u003Ep__12, this.Driver) is int num3)
      {
        switch (num3)
        {
          case 0:
            num2 = num1 / 1000000M;
            goto label_36;
          case 1:
            num2 = num1 / 1000M;
            goto label_36;
          case 2:
            num2 = num1;
            goto label_36;
        }
      }
      num2 = 0M;
label_36:
      weight = num2;
    }

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.Scale.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public void Tara()
    {
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__18.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__18.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SetTare", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__0.Target((CallSite) MassaK.\u003C\u003Eo__18.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__18.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__18.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__18.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__18.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__1.Target((CallSite) MassaK.\u003C\u003Eo__18.\u003C\u003Ep__1, obj1, 0);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__18.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__18.\u003C\u003Ep__5 = CallSite<Func<CallSite, System.Type, object, InvalidOperationException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, InvalidOperationException> target2 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, InvalidOperationException>> p5 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__5;
        System.Type type1 = typeof (InvalidOperationException);
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__18.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__18.\u003C\u003Ep__4 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, object> target3 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, object>> p4 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__4;
        System.Type type2 = typeof (string);
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__18.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__18.\u003C\u003Ep__3 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = MassaK.\u003C\u003Eo__18.\u003C\u003Ep__3.Target((CallSite) MassaK.\u003C\u003Eo__18.\u003C\u003Ep__3, Translate.MassaK_Tara_Не_удалось_тарировать_вес__код_ошибки__, obj1);
        object obj4 = target3((CallSite) p4, type2, obj3);
        throw target2((CallSite) p5, type1, obj4);
      }
    }

    public void Zero()
    {
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SetZero", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) MassaK.\u003C\u003Eo__19.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__19.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__19.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (MassaK.\u003C\u003Eo__19.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MassaK.\u003C\u003Eo__19.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__1.Target((CallSite) MassaK.\u003C\u003Eo__19.\u003C\u003Ep__1, obj1, 0);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__19.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__19.\u003C\u003Ep__5 = CallSite<Func<CallSite, System.Type, object, InvalidOperationException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, InvalidOperationException> target2 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, InvalidOperationException>> p5 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__5;
        System.Type type1 = typeof (InvalidOperationException);
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__19.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__19.\u003C\u003Ep__4 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, object> target3 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, object>> p4 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__4;
        System.Type type2 = typeof (string);
        // ISSUE: reference to a compiler-generated field
        if (MassaK.\u003C\u003Eo__19.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MassaK.\u003C\u003Eo__19.\u003C\u003Ep__3 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (MassaK), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = MassaK.\u003C\u003Eo__19.\u003C\u003Ep__3.Target((CallSite) MassaK.\u003C\u003Eo__19.\u003C\u003Ep__3, Translate.MassaK_Zero_Не_удалось_выполнить_команду_Zero__код_ошибки__, obj1);
        object obj4 = target3((CallSite) p4, type2, obj3);
        throw target2((CallSite) p5, type1, obj4);
      }
    }

    public void TaraReset() => this.Tara();

    public string Name
    {
      get
      {
        return "Масса К (" + (this._type == GlobalDictionaries.Devices.ScaleTypes.MassaK100 ? "Driver100" : "DriverMassa") + ")";
      }
    }
  }
}
