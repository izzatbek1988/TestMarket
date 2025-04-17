// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.ShtrihM
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

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
  public class ShtrihM : IScalesWIthLabels, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public string Name => "ШТРИХ-М";

    private object Driver { get; set; }

    public bool Connect(bool onlyDriverLoad = false)
    {
      this.Driver = Functions.CreateObject("AddIn.DrvLP");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        return false;
      if (onlyDriverLoad)
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (Connect), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__2, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p4 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Connected", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__3, this.Driver);
      if (target2((CallSite) p4, obj2))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p7 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target4 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p6 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__5, this.Driver);
      object obj4 = target4((CallSite) p6, obj3, 0);
      if (!target3((CallSite) p7, obj4))
        return false;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__13 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target5 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p13 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__13;
      System.Type type1 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__12.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p12 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__12;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target7 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p10 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__9 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target8 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p9 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__8.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__8, this.Driver);
      object obj6 = target8((CallSite) p9, "Возникла ошибка подключения к весам с печатью этикеток Штрих-М, код: ", obj5);
      object obj7 = target7((CallSite) p10, obj6, ", описание: ");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__11.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__11, this.Driver);
      object obj9 = target6((CallSite) p12, obj7, obj8);
      target5((CallSite) p13, type1, obj9);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__17 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Show", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target9 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p17 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__17;
      System.Type type2 = typeof (MessageBoxHelper);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__16 = CallSite<Func<CallSite, System.Type, string, string, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, string, string, object, object, object> target10 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, string, string, object, object, object>> p16 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__16;
      System.Type type3 = typeof (string);
      string этикетокШтрихМ012 = Translate.ShtrihM_Возникла_ошибка_подключения_к_весам_с_печатью_этикеток_Штрих_М_0__1____2__;
      string str = Other.NewLine(2);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__14.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__14, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__15.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__15, this.Driver);
      object obj12 = target10((CallSite) p16, type3, этикетокШтрихМ012, str, obj10, obj11);
      target9((CallSite) p17, type2, obj12);
      return false;
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (Disconnect), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p8 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p4 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__3, this.Driver);
      object obj3 = target3((CallSite) p4, obj2, 0);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      object obj4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__7, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target4 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p6 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__6;
        object obj5 = obj3;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Connected", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__5, this.Driver);
        obj4 = target4((CallSite) p6, obj5, obj6);
      }
      else
        obj4 = obj3;
      if (target2((CallSite) p8, obj4))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__14 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target5 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p14 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__14;
      System.Type type = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target6 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p13 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target7 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p11 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__10 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target8 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p10 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__9.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__9, this.Driver);
      object obj8 = target8((CallSite) p10, "Возникла ошибка отключения от  весов с печатью этикеток Штрих-М, код: ", obj7);
      object obj9 = target7((CallSite) p11, obj8, ", описание: ");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__12.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__12, this.Driver);
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
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "PLUNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0, this.Driver, good.Plu);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Price", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1, this.Driver, good.Price);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "NameFirst", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2, this.Driver, good.Name);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ItemCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__3, this.Driver, good.Plu);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__4 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetPLUData", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__4, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__7.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p7 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__7;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target2 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p6 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__6;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__5, this.Driver);
        object obj6 = target2((CallSite) p6, obj5, 0);
        if (target1((CallSite) p7, obj6))
        {
          ++num;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__10 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, System.Type, object> target3 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__10.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, System.Type, object>> p10 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__10;
          System.Type type = typeof (LogHelper);
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__9 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, string, object, object> target4 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__9.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, string, object, object>> p9 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__9;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__8.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__8, this.Driver);
          object obj8 = target4((CallSite) p9, "Ошибка загрузки товара: ", obj7);
          target3((CallSite) p10, type, obj8);
        }
      }
      return num;
    }

    public bool ShowProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (ShowProperties), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p2 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__1, this.Driver);
      object obj2 = target2((CallSite) p2, obj1, 0);
      if (target1((CallSite) p3, obj2))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__9 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target3 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p9 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__9;
      System.Type type = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p8 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target5 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p6 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__5 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target6 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p5 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__4, this.Driver);
      object obj4 = target6((CallSite) p5, "Возникла ошибка при поытке открыть настройки весов с печатью этикеток Штрих-М, код: ", obj3);
      object obj5 = target5((CallSite) p6, obj4, ", описание: ");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__7, this.Driver);
      object obj7 = target4((CallSite) p8, obj5, obj6);
      target3((CallSite) p9, type, obj7);
      return false;
    }
  }
}
