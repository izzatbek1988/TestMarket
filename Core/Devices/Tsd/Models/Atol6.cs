// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Tsd.Models.Atol6
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
namespace Gbs.Core.Devices.Tsd.Models
{
  public class Atol6 : ITsd, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Tsd;

    public string Name => Translate.ТСДАТОЛV6;

    private object AtolDriver { get; set; }

    private void CheckResult()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (Atol6)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target1 = Atol6.\u003C\u003Eo__7.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p1 = Atol6.\u003C\u003Eo__7.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol6.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) Atol6.\u003C\u003Eo__7.\u003C\u003Ep__0, this.AtolDriver);
      int num = target1((CallSite) p1, obj1);
      if (num != 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__7.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__7.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (Atol6)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target2 = Atol6.\u003C\u003Eo__7.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p3 = Atol6.\u003C\u003Eo__7.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__7.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__7.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultDescription", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Atol6.\u003C\u003Eo__7.\u003C\u003Ep__2.Target((CallSite) Atol6.\u003C\u003Eo__7.\u003C\u003Ep__2, this.AtolDriver);
        string str = target2((CallSite) p3, obj2);
        string message = string.Format(Translate.Код___0____Описание___1_, (object) num, (object) str);
        LogHelper.Debug("Error Atol6 ТСД " + message);
        throw new DeviceException(message, (IDevice) this);
      }
    }

    public void ShowProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (ShowProperties), (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol6.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) Atol6.\u003C\u003Eo__8.\u003C\u003Ep__0, this.AtolDriver);
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      this.AtolDriver = Functions.CreateObject("AddIn.PDX45");
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__9.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__9.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = Atol6.\u003C\u003Eo__9.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = Atol6.\u003C\u003Eo__9.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = Atol6.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) Atol6.\u003C\u003Eo__9.\u003C\u003Ep__0, this.AtolDriver, (object) null);
      if (target((CallSite) p1, obj))
        throw new NullReferenceException(Translate.Atol6_Connect_Объект_драйвера_АТОЛ_6_для_ТСД_не_был_создан_);
      int num = onlyDriverLoad ? 1 : 0;
      return true;
    }

    public bool Disconnect() => true;

    public List<GoodForTsd> ReadInventory(string idDoc)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FormNumber", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__0, this.AtolDriver, 0);
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "BeginReport", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol6.\u003C\u003Eo__11.\u003C\u003Ep__1.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__1, this.AtolDriver);
      this.CheckResult();
      List<GoodForTsd> goodForTsdList = new List<GoodForTsd>();
      int num1 = 1;
      while (true)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__11.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p4 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__11.\u003C\u003Ep__3 = CallSite<Func<CallSite, int, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThanOrEqual, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, int, object, object> target2 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, int, object, object>> p3 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__3;
        int num2 = num1;
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__11.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ReportRecordCount", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__2.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__2, this.AtolDriver);
        object obj3 = target2((CallSite) p3, num2, obj2);
        if (target1((CallSite) p4, obj3))
        {
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__5 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetRecord", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__11.\u003C\u003Ep__5.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__5, this.AtolDriver);
          this.CheckResult();
          GoodForTsd goodForTsd1 = new GoodForTsd();
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__6.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__6, this.AtolDriver, 0);
          GoodForTsd goodForTsd2 = goodForTsd1;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, Guid>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Guid), typeof (Atol6)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, Guid> target3 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__9.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, Guid>> p9 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__9;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__8 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, System.Type, object, object> target4 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__8.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, System.Type, object, object>> p8 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__8;
          System.Type type1 = typeof (Guid);
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__7.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__7, this.AtolDriver);
          object obj6 = target4((CallSite) p8, type1, obj5);
          Guid guid = target3((CallSite) p9, obj6);
          goodForTsd2.Uid = guid;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__10.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__10, this.AtolDriver, 1);
          GoodForTsd goodForTsd3 = goodForTsd1;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Atol6)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string> target5 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__12.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string>> p12 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__12;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__11.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__11, this.AtolDriver);
          string str1 = target5((CallSite) p12, obj8);
          goodForTsd3.Barcode = str1;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj9 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__13.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__13, this.AtolDriver, 2);
          GoodForTsd goodForTsd4 = goodForTsd1;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (Atol6)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string> target6 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__15.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string>> p15 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__15;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj10 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__14.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__14, this.AtolDriver);
          string str2 = target6((CallSite) p15, obj10);
          goodForTsd4.Name = str2;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__16.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__16, this.AtolDriver, 3);
          GoodForTsd goodForTsd5 = goodForTsd1;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (Atol6)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, Decimal> target7 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__19.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, Decimal>> p19 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__19;
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__18 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, System.Type, object, object> target8 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__18.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, System.Type, object, object>> p18 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__18;
          System.Type type2 = typeof (Convert);
          // ISSUE: reference to a compiler-generated field
          if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Atol6.\u003C\u003Eo__11.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj12 = Atol6.\u003C\u003Eo__11.\u003C\u003Ep__17.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__17, this.AtolDriver);
          object obj13 = target8((CallSite) p18, type2, obj12);
          Decimal num3 = target7((CallSite) p19, obj13);
          goodForTsd5.Price = num3;
          goodForTsdList.Add(goodForTsd1);
          ++num1;
        }
        else
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__11.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__11.\u003C\u003Ep__20 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "EndReport", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol6.\u003C\u003Eo__11.\u003C\u003Ep__20.Target((CallSite) Atol6.\u003C\u003Eo__11.\u003C\u003Ep__20, this.AtolDriver);
      this.CheckResult();
      return goodForTsdList;
    }

    public void WriteInventory(List<GoodForTsd> goods, string idDoc)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FormNumber", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__0, this.AtolDriver, 0);
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ShowProgress", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__1.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__1, this.AtolDriver, true);
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "BeginAdd", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol6.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__2, this.AtolDriver);
      this.CheckResult();
      foreach (GoodForTsd good in goods)
      {
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__3, this.AtolDriver, 0);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, Guid, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__4.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__4, this.AtolDriver, good.Uid);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__5.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__5, this.AtolDriver, 1);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__6.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__6, this.AtolDriver, good.Barcode);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__7.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__7, this.AtolDriver, 2);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__8.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__8, this.AtolDriver, good.Name);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportFieldIndex", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__9.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__9, this.AtolDriver, 3);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ReportField", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__10.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__10, this.AtolDriver, good.Price);
        // ISSUE: reference to a compiler-generated field
        if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Atol6.\u003C\u003Eo__12.\u003C\u003Ep__11 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetRecord", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__12.\u003C\u003Ep__11.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__11, this.AtolDriver);
        this.CheckResult();
      }
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__12.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ShowProgress", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__12.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__12, this.AtolDriver, true);
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__12.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "AddMode", typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = Atol6.\u003C\u003Eo__12.\u003C\u003Ep__13.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__13, this.AtolDriver, 1);
      // ISSUE: reference to a compiler-generated field
      if (Atol6.\u003C\u003Eo__12.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol6.\u003C\u003Eo__12.\u003C\u003Ep__14 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "EndAdd", (IEnumerable<System.Type>) null, typeof (Atol6), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol6.\u003C\u003Eo__12.\u003C\u003Ep__14.Target((CallSite) Atol6.\u003C\u003Eo__12.\u003C\u003Ep__14, this.AtolDriver);
      this.CheckResult();
    }

    public void TestConnect() => throw new NotImplementedException();
  }
}
