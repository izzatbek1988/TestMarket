// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.Models.IcsNt
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
namespace Gbs.Core.Devices.Scales.Models
{
  public class IcsNt : IScale, IDevice
  {
    private object Driver { get; set; }

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public IcsNt(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.Scale.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public void Connect(bool onlyDriverLoad = false)
    {
      this.Driver = Functions.CreateObject("AddIn.Ics_nt");
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.Atol10_Объект_драйвера_не_был_создан);
      if (onlyDriverLoad)
        return;
      ComPort comPort = this.DevicesConfig.Scale.ComPort;
      DeviceHelper.CheckComPortExists(comPort.PortName, (IDevice) this);
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (IcsNt)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target2 = IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p3 = IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "init", (IEnumerable<System.Type>) null, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__2.Target((CallSite) IcsNt.\u003C\u003Eo__10.\u003C\u003Ep__2, this.Driver, comPort.PortNumber);
      if (target2((CallSite) p3, obj2) == 1)
        throw new DeviceException(Translate.IcsNt_Connect_Не_удалось_установить_соединение_с_весами_ICS_NT, (IDevice) this);
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target1((CallSite) p1, obj1))
      {
        LogHelper.Debug("Экземпляр драйвера не ссылается на объект");
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (IcsNt)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target2 = IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p3 = IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "uninit", (IEnumerable<System.Type>) null, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__2.Target((CallSite) IcsNt.\u003C\u003Eo__11.\u003C\u003Ep__2, this.Driver);
      if (target2((CallSite) p3, obj2) != 1)
        return true;
      LogHelper.Debug("Не удалось отключиться от весов ICS NT");
      return false;
    }

    public void GetWeight(out Decimal weight, Decimal price)
    {
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToDecimal", (IEnumerable<System.Type>) null, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, object> target1 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, object>> p1 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__1;
      System.Type type1 = typeof (Convert);
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "get_weight", (IEnumerable<System.Type>) null, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__0, this.Driver);
      object obj2 = target1((CallSite) p1, type1, obj1);
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target2 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p3 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__3;
      System.Type type2 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__2, "Данные с весов: ", obj2);
      target2((CallSite) p3, type2, obj3);
      ref Decimal local = ref weight;
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Decimal), typeof (IcsNt)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target3 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p5 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Divide, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__4.Target((CallSite) IcsNt.\u003C\u003Eo__12.\u003C\u003Ep__4, obj2, 1000M);
      Decimal num = target3((CallSite) p5, obj4);
      local = num;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Scale;

    public void Tara()
    {
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "tare", (IEnumerable<System.Type>) null, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__0, this.Driver);
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (IcsNt), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) IcsNt.\u003C\u003Eo__14.\u003C\u003Ep__1, obj1, 0);
      if (target((CallSite) p2, obj2))
        throw new InvalidOperationException(Translate.IcsNt_Tara_Не_удалось_тарировать_вес_);
    }

    public void Zero() => throw new NotImplementedException();

    public void TaraReset() => this.Tara();

    public string Name => "ICS NT";
  }
}
