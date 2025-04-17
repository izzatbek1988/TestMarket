// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayBuyers.Models.Atol8
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Core.Devices.DisplayBuyers.Models
{
  public class Atol8 : IDisplayBuyers, IDevice
  {
    private object AtolDriver;

    private bool CheckError()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__1.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__1.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = Atol8.\u003C\u003Eo__1.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = Atol8.\u003C\u003Eo__1.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__1.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__1.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = Atol8.\u003C\u003Eo__1.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p1 = Atol8.\u003C\u003Eo__1.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = Atol8.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__1.\u003C\u003Ep__0, this.AtolDriver);
      object obj2 = target2((CallSite) p1, obj1, 0);
      if (target1((CallSite) p2, obj2))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__1.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__1.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultDescription", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = Atol8.\u003C\u003Eo__1.\u003C\u003Ep__3.Target((CallSite) Atol8.\u003C\u003Eo__1.\u003C\u003Ep__3, this.AtolDriver);
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__1.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__1.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = Atol8.\u003C\u003Eo__1.\u003C\u003Ep__4.Target((CallSite) Atol8.\u003C\u003Eo__1.\u003C\u003Ep__4, this.AtolDriver);
      LogHelper.Debug(string.Format("Ошибка при работе с дисплеем покупателя АТОЛ: {0} ({1})", obj3, obj4));
      return false;
    }

    public string LastResultCodeDescriptor => "";

    public bool Clear()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ClearText", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__4.\u003C\u003Ep__0, this.AtolDriver);
      return this.CheckError();
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        this.AtolDriver = Functions.CreateObject("AddIn.Line45");
        return this.CheckError();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при создании объекта драйвера АТОЛ ДИсплей покупателя.");
        return false;
      }
    }

    public bool Disconnect() => true;

    public bool ShowProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (ShowProperties), (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__7.\u003C\u003Ep__0, this.AtolDriver);
      return true;
    }

    public bool WriteText(string line, int index)
    {
      // ISSUE: reference to a compiler-generated field
      if (Atol8.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        Atol8.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, int, string, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "DisplayText", (IEnumerable<System.Type>) null, typeof (Atol8), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Atol8.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) Atol8.\u003C\u003Eo__8.\u003C\u003Ep__0, this.AtolDriver, index, 0, line, 0);
      return this.CheckError();
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.DisplayBuyer;

    public string Name { get; }
  }
}
