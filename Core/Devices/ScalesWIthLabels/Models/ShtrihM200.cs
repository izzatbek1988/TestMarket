// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.ShtrihM200
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
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
namespace Gbs.Core.Devices.ScalesWIthLabels.Models
{
  public class ShtrihM200 : IScalesWIthLabels, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    private Gbs.Core.Config.Devices DevicesConfig { get; }

    public ShtrihM200(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public string Name => "ШТРИХ-М (РС-200)";

    private object Driver { get; set; }

    public bool Connect(bool onlyDriverLoad = false)
    {
      this.Driver = Functions.CreateObject("AddIn.PC200CE");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        return false;
      if (onlyDriverLoad)
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ConnectDevice", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__2.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__2, this.Driver, this.DevicesConfig.ScaleWithLable.Connection.LanPort.UrlAddress, this.DevicesConfig.ScaleWithLable.Connection.LanPort.PortNumber.GetValueOrDefault());
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p7 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__3.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__3, obj2, 0);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      object obj4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__6.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__6, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target3 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p5 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__5;
        object obj5 = obj3;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Connected", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__4.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__4, this.Driver);
        obj4 = target3((CallSite) p5, obj5, obj6);
      }
      else
        obj4 = obj3;
      if (target2((CallSite) p7, obj4))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p10 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target5 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p9 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__8.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__8, this.Driver);
      object obj8 = target5((CallSite) p9, obj7, 0);
      if (!target4((CallSite) p10, obj8))
        return false;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__16 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target6 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p16 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__16;
      System.Type type1 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target7 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__15.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p15 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__15;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target8 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p13 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__12 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target9 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__12.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p12 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__12;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__11.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__11, this.Driver);
      object obj10 = target9((CallSite) p12, "Возникла ошибка подключения к весам с печатью этикеток Штрих-М (РС-200), код: ", obj9);
      object obj11 = target8((CallSite) p13, obj10, ", описание: ");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__14.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__14, this.Driver);
      object obj13 = target7((CallSite) p15, obj11, obj12);
      target6((CallSite) p16, type1, obj13);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__20 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Show", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target10 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__20.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p20 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__20;
      System.Type type2 = typeof (MessageBoxHelper);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__19 = CallSite<Func<CallSite, System.Type, string, string, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, string, string, object, object, object> target11 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, string, string, object, object, object>> p19 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__19;
      System.Type type3 = typeof (string);
      string этикетокШтрихМ012 = Translate.ShtrihM_Возникла_ошибка_подключения_к_весам_с_печатью_этикеток_Штрих_М_0__1____2__;
      string str = Other.NewLine(2);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__17.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__17, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj15 = ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__18.Target((CallSite) ShtrihM200.\u003C\u003Eo__11.\u003C\u003Ep__18, this.Driver);
      object obj16 = target11((CallSite) p19, type3, этикетокШтрихМ012, str, obj14, obj15);
      target10((CallSite) p20, type2, obj16);
      return false;
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "DisconnectDevice", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__2, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p8 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p4 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__3, this.Driver);
      object obj3 = target3((CallSite) p4, obj2, 0);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      object obj4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__7.Target((CallSite) ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__7, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target4 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p6 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__6;
        object obj5 = obj3;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Connected", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__5.Target((CallSite) ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__5, this.Driver);
        obj4 = target4((CallSite) p6, obj5, obj6);
      }
      else
        obj4 = obj3;
      if (target2((CallSite) p8, obj4))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__14 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target5 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p14 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__14;
      System.Type type = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p13 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target7 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p11 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__10 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target8 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p10 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__9.Target((CallSite) ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__9, this.Driver);
      object obj8 = target8((CallSite) p10, "Возникла ошибка отключения от  весов с печатью этикеток Штрих-М (РС-200), код: ", obj7);
      object obj9 = target7((CallSite) p11, obj8, ", описание: ");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__12.Target((CallSite) ShtrihM200.\u003C\u003Eo__12.\u003C\u003Ep__12, this.Driver);
      object obj11 = target6((CallSite) p13, obj9, obj10);
      target5((CallSite) p14, type, obj11);
      return false;
    }

    public int SendGood(List<GoodForWith> goods)
    {
      int num = 0;
      foreach (GoodForWith good in goods)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ClearBlock", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__0, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cName", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__1.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__1, this.Driver, good.Name.Length > (int) byte.MaxValue ? good.Name.Remove(254) : good.Name);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__2.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__2, this.Driver, good.Plu);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, double, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cPrice", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__3.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__3, this.Driver, (double) good.Price);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "cMessageCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__4.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__4, this.Driver, good.Plu);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__5 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddPLUToBlock", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__5.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__5, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__6 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetPLUBlockData", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__6.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__6, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p9 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target2 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p8 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__8;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__7.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__7, this.Driver);
        object obj6 = target2((CallSite) p8, obj5, 0);
        if (target1((CallSite) p9, obj6))
        {
          ++num;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__12 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, System.Type, object> target3 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__12.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, System.Type, object>> p12 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__12;
          System.Type type = typeof (LogHelper);
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__11 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, string, object, object> target4 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__11.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, string, object, object>> p11 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__11;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM200), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__10.Target((CallSite) ShtrihM200.\u003C\u003Eo__13.\u003C\u003Ep__10, this.Driver);
          object obj8 = target4((CallSite) p11, "Ошибка загрузки товара: ", obj7);
          target3((CallSite) p12, type, obj8);
        }
      }
      return num;
    }

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(this.DevicesConfig.ScaleWithLable.Connection.LanPort, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
      {
        NeedAuth = false
      });
      return true;
    }
  }
}
