// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.Cas
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Core.Devices.ScalesWIthLabels.Models
{
  public class Cas : IScalesWIthLabels, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.ScaleWithLable;

    public string Name => nameof (Cas);

    private object Driver { get; set; }

    private Gbs.Core.Config.Devices DevicesConfig { get; }

    public Cas(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(this.DevicesConfig.ScaleWithLable.Connection, ConnectionSettingsViewModel.PortsConfig.ComOrLan);
      return true;
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        this.Driver = Functions.CreateObject("CAScentre_DLL_printScale.Scale");
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__0, this.Driver, (object) null);
        if (target1((CallSite) p1, obj1))
          return false;
        if (onlyDriverLoad)
          return true;
        switch (this.DevicesConfig.ScaleWithLable.Connection.ConnectionType)
        {
          case GlobalDictionaries.Devices.ConnectionTypes.NotSet:
            return false;
          case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__12.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "IP", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj2 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__2.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__2, this.Driver, this.DevicesConfig.ScaleWithLable.Connection.ComPort.PortName);
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__12.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Port", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj3 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__3.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__3, this.Driver, this.DevicesConfig.ScaleWithLable.Connection.ComPort.Speed);
            break;
          case GlobalDictionaries.Devices.ConnectionTypes.Lan:
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__12.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "IP", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj4 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__4.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__4, this.Driver, this.DevicesConfig.ScaleWithLable.Connection.LanPort.UrlAddress);
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__12.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Port", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__5.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__5, this.Driver, this.DevicesConfig.ScaleWithLable.Connection.LanPort.PortNumber);
            break;
          default:
            return false;
        }
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__12.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Type", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__6.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__6, this.Driver, (int) this.DevicesConfig.ScaleWithLable.CasType);
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__12.\u003C\u003Ep__7 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Open", (IEnumerable<System.Type>) null, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Cas.\u003C\u003Eo__12.\u003C\u003Ep__7.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__7, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__12.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (Cas)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__10.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p10 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__10;
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__12.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target3 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p9 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__12.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__12.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = Cas.\u003C\u003Eo__12.\u003C\u003Ep__8.Target((CallSite) Cas.\u003C\u003Eo__12.\u003C\u003Ep__8, this.Driver);
        object obj8 = target3((CallSite) p9, obj7, 0);
        return target2((CallSite) p10, obj8);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при установке соединения с весами спечатью CAS", false);
        return false;
      }
    }

    public bool Disconnect()
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__13.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__13.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) Cas.\u003C\u003Eo__13.\u003C\u003Ep__0, this.Driver, (object) null);
        if (target1((CallSite) p1, obj1))
          return true;
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__13.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__13.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Close", (IEnumerable<System.Type>) null, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Cas.\u003C\u003Eo__13.\u003C\u003Ep__2.Target((CallSite) Cas.\u003C\u003Eo__13.\u003C\u003Ep__2, this.Driver);
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__13.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__13.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (Cas)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p5 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__13.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__13.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target3 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p4 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (Cas.\u003C\u003Eo__13.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Cas.\u003C\u003Eo__13.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = Cas.\u003C\u003Eo__13.\u003C\u003Ep__3.Target((CallSite) Cas.\u003C\u003Eo__13.\u003C\u003Ep__3, this.Driver);
        object obj3 = target3((CallSite) p4, obj2, 0);
        return target2((CallSite) p5, obj3);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при закрытии соединения с весами спечатью CAS", false);
        return false;
      }
    }

    public int SendGood(List<GoodForWith> goods)
    {
      string[] source = this.DevicesConfig.BarcodeScanner.Prefixes.WeightGoods.Split(GlobalDictionaries.SplitArr);
      if (((IEnumerable<string>) source).Any<string>())
        Convert.ToInt32(source[0]);
      int num = 0;
      foreach (GoodForWith good in goods)
      {
        if (good.Plu > 4000)
        {
          LogHelper.Debug(string.Format("Товар с кодом [{0}] не передан в весы. Максимальное ПЛУ 4000.", (object) good.Plu));
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluDept", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__0.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__0, this.Driver, 1);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, uint, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluNumber", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__1.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__1, this.Driver, (uint) good.Plu);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluType", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__2.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__2, this.Driver, 1);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluName1", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj4 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__3.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__3, this.Driver, good.Name);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluName2", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__4.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__4, this.Driver, good.Name.Length > 28 ? good.Name.Substring(28) : string.Empty);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluStrLogo", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__5.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__5, this.Driver, "АБВГ");
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluGroupCode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__6.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__6, this.Driver, 1234);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, uint, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluItemCode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__7.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__7, this.Driver, (uint) good.Plu);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluFixedPrice", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj9 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__8.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__8, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, uint, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluPrice", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj10 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__9.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__9, this.Driver, (uint) (good.Price * 100M));
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluWeightTare", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__10.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__10, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluDatePack", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj12 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__11.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__11, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluTimePack", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj13 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__12.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__12, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluDateLife", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj14 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__13.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__13, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluTimeLife", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj15 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__14.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__14, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluNumberMsg", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj16 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__15.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__15, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluTextMessage", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj17 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__16.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__16, this.Driver, "");
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluNumberLabel", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj18 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__17.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__17, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluNumberBarcode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj19 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__18.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__18, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluDateCreate", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj20 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__19.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__19, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluTextNumber", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj21 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__20.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__20, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluSYmbol", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj22 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__21.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__21, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluExtPCS", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj23 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__22.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__22, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluExtOrigin", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj24 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__23.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__23, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluExtBar2", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj25 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__24.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__24, this.Driver, 0);
          // ISSUE: reference to a compiler-generated field
          if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__25 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "pluExtFixedWeight", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj26 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__25.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__25, this.Driver, 0);
          try
          {
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__26 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__14.\u003C\u003Ep__26 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SendPlu", (IEnumerable<System.Type>) null, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Cas.\u003C\u003Eo__14.\u003C\u003Ep__26.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__26, this.Driver);
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__29 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__14.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target1 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__29.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p29 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__29;
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__28 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__14.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target2 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__28.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p28 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__28;
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__27 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__14.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj27 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__27.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__27, this.Driver);
            object obj28 = target2((CallSite) p28, obj27, 0);
            if (target1((CallSite) p29, obj28))
            {
              ++num;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__32 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Cas.\u003C\u003Eo__14.\u003C\u003Ep__32 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Action<CallSite, System.Type, object> target3 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__32.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Action<CallSite, System.Type, object>> p32 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__32;
              System.Type type = typeof (LogHelper);
              // ISSUE: reference to a compiler-generated field
              if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__31 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Cas.\u003C\u003Eo__14.\u003C\u003Ep__31 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, string, object, object> target4 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__31.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, string, object, object>> p31 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__31;
              // ISSUE: reference to a compiler-generated field
              if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__30 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Cas.\u003C\u003Eo__14.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj29 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__30.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__30, this.Driver);
              object obj30 = target4((CallSite) p31, "Ошибка загрузки товара: ", obj29);
              target3((CallSite) p32, type, obj30);
            }
          }
          catch (Exception ex)
          {
            // ISSUE: variable of a boxed type
            __Boxed<int> plu = (ValueType) good.Plu;
            // ISSUE: reference to a compiler-generated field
            if (Cas.\u003C\u003Eo__14.\u003C\u003Ep__33 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Cas.\u003C\u003Eo__14.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (Cas), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj31 = Cas.\u003C\u003Eo__14.\u003C\u003Ep__33.Target((CallSite) Cas.\u003C\u003Eo__14.\u003C\u003Ep__33, this.Driver);
            string message = string.Format("Товар с кодом [{0}] не передан в весы. ResultCode: $[{1}].", (object) plu, obj31);
            LogHelper.Error(ex, message, false);
          }
        }
      }
      return num;
    }
  }
}
