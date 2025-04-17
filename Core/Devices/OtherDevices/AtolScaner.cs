// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.OtherDevices.AtolScaner
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.Scales;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Core.Devices.OtherDevices
{
  public class AtolScaner : IScale, IDevice
  {
    private object Driver { get; set; }

    public bool ShowProperties()
    {
      this.Connect(false);
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (ShowProperties), (IEnumerable<System.Type>) null, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (AtolScaner)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p2 = AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__1.Target((CallSite) AtolScaner.\u003C\u003Eo__4.\u003C\u003Ep__1, this.Driver);
      object obj2 = target2((CallSite) p2, obj1, 0);
      return target1((CallSite) p3, obj2);
    }

    public void Connect(bool onlyDriverLoad = false)
    {
      this.Driver = Functions.CreateObject("AddIn.Scaner8");
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.Atol10_Объект_драйвера_не_был_создан);
      if (onlyDriverLoad)
        return;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p4 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target3 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p3 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DeviceEnabled", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__2.Target((CallSite) AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__2, this.Driver);
      object obj3 = target3((CallSite) p3, obj2);
      if (target2((CallSite) p4, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DeviceEnabled", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__5.Target((CallSite) AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__5, this.Driver, true);
      }
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p8 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target5 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p7 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__6.Target((CallSite) AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__6, this.Driver);
      object obj6 = target5((CallSite) p7, obj5, 0);
      if (!target4((CallSite) p8, obj6))
      {
        // ISSUE: reference to a compiler-generated field
        if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__12 = CallSite<Func<CallSite, System.Type, object, AtolScaner, DeviceException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, AtolScaner, DeviceException> target6 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__12.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, AtolScaner, DeviceException>> p12 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__12;
        System.Type type1 = typeof (DeviceException);
        // ISSUE: reference to a compiler-generated field
        if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__11 = CallSite<Func<CallSite, System.Type, string, int, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, string, int, object, object, object> target7 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__11.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, string, int, object, object, object>> p11 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__11;
        System.Type type2 = typeof (string);
        string устройствуАтол012 = Translate.AtolScaner_Возникла_ошибка_подключения_к_считывающему_устройству_АТОЛ_0__1____2__;
        // ISSUE: reference to a compiler-generated field
        if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultDescription", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__9.Target((CallSite) AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__9, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__10.Target((CallSite) AtolScaner.\u003C\u003Eo__5.\u003C\u003Ep__10, this.Driver);
        object obj9 = target7((CallSite) p11, type2, устройствуАтол012, 46, obj7, obj8);
        throw target6((CallSite) p12, type1, obj9, this);
      }
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DeviceEnabled", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__2.Target((CallSite) AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__2, this.Driver, false);
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p4 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__3.Target((CallSite) AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__3, this.Driver);
      object obj4 = target3((CallSite) p4, obj3, 0);
      if (target2((CallSite) p5, obj4))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__11 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target4 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p11 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__11;
      System.Type type = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target5 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p10 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target6 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p8 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__7 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target7 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p7 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__6.Target((CallSite) AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__6, this.Driver);
      object obj6 = target7((CallSite) p7, "Возникла ошибка отключения от считывающего у-ва АТОЛ, код: ", obj5);
      object obj7 = target6((CallSite) p8, obj6, ", описание: ");
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultDescription", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__9.Target((CallSite) AtolScaner.\u003C\u003Eo__6.\u003C\u003Ep__9, this.Driver);
      object obj9 = target5((CallSite) p10, obj7, obj8);
      target4((CallSite) p11, type, obj9);
      return false;
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Decimal), typeof (AtolScaner)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target1 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p1 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReadWeight", (IEnumerable<System.Type>) null, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__0, this.Driver);
      Decimal num = target1((CallSite) p1, obj1);
      Other.ConsoleWrite("w: " + num.ToString());
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p3 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__2.Target((CallSite) AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__2, this.Driver);
      object obj3 = target2((CallSite) p3, obj2, 0);
      // ISSUE: reference to a compiler-generated field
      if (AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__6.Target((CallSite) AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__6, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target3 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p5 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof (AtolScaner), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__4.Target((CallSite) AtolScaner.\u003C\u003Eo__7.\u003C\u003Ep__4, obj3, num == -1M);
        if (!target3((CallSite) p5, obj4))
        {
          weight = num;
          return;
        }
      }
      throw new InvalidOperationException();
    }

    public void Tara() => throw new NotImplementedException();

    public void Zero() => throw new NotImplementedException();

    public void TaraReset() => throw new NotImplementedException();

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public string Name => "АТОЛ (Scanner8)";
  }
}
