// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayBuyers.Models.ShtrihM
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
namespace Gbs.Core.Devices.DisplayBuyers.Models
{
  public class ShtrihM : IDisplayBuyers, IDevice
  {
    private object ShtrihDriver { get; set; }

    private bool CheckError()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (ShtrihM)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p1 = ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__4.\u003C\u003Ep__0, this.ShtrihDriver);
      object obj2 = target2((CallSite) p1, obj1, 0);
      return target1((CallSite) p2, obj2);
    }

    public string LastResultCodeDescriptor
    {
      get
      {
        if (ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
          ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ShtrihM)));
        Func<CallSite, object, string> target = ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__1.Target;
        CallSite<Func<CallSite, object, string>> p1 = ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__1;
        object shtrihDriver = this.ShtrihDriver;
        object obj;
        if (shtrihDriver == null)
        {
          obj = (object) null;
        }
        else
        {
          if (ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
            ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescriptor", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          obj = ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__6.\u003C\u003Ep__0, shtrihDriver);
        }
        if (obj == null)
          obj = (object) Translate.ShtrihM_Табло_Штрих_М_не_инициализировано;
        return target((CallSite) p1, obj);
      }
    }

    public bool Clear()
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ClearDispl", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__7.\u003C\u003Ep__0, this.ShtrihDriver);
        return this.CheckError();
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка  очищенния табло покупателя Штрих-М");
        return false;
      }
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        this.ShtrihDriver = Functions.CreateObject("DrvDspl.v1_2");
        if (onlyDriverLoad)
          return true;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Enable", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__0, this.ShtrihDriver, 1);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InitialDispl", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__1, this.ShtrihDriver);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ClearDispl", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__8.\u003C\u003Ep__2, this.ShtrihDriver);
        return this.CheckError();
      }
      catch (Exception ex)
      {
        int num = onlyDriverLoad ? 1 : 0;
        LogHelper.Error(ex, "Ошибка при подключении к табло покупателя Штрих-М", num != 0);
        return false;
      }
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__0, this.ShtrihDriver, (object) null);
      if (target((CallSite) p1, obj1))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Enable", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__9.\u003C\u003Ep__2, this.ShtrihDriver, 0);
      this.CheckError();
      this.ShtrihDriver = (object) null;
      return true;
    }

    public bool ShowProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ShowVisual", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__10.\u003C\u003Ep__0, this.ShtrihDriver);
      return this.CheckError();
    }

    public bool WriteText(string line, int index)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "EnterStr", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__11.\u003C\u003Ep__0, this.ShtrihDriver, index, line);
        return this.CheckError();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка  ввода текста на табло покупателя Штрих-М", false);
        return false;
      }
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.DisplayBuyer;

    public string Name { get; }
  }
}
