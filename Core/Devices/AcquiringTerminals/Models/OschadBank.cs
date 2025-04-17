// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.OschadBank
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
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
using System.Text.RegularExpressions;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  internal class OschadBank : IAcquiringTerminal, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    private object Terminal { get; set; }

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public string Name => nameof (OschadBank);

    public OschadBank(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public void ShowProperties()
    {
      DeviceConnection connection = new DeviceConnection()
      {
        LanPort = this.DevicesConfig.AcquiringTerminal.LanConnection,
        ComPort = this.DevicesConfig.AcquiringTerminal.ComPort,
        ConnectionType = this.DevicesConfig.AcquiringTerminal.ConnectionType
      };
      new FrmConnectionSettings().ShowConfig(connection, ConnectionSettingsViewModel.PortsConfig.ComOrLan);
      this.DevicesConfig.AcquiringTerminal.ConnectionType = connection.ConnectionType;
    }

    public void ShowServiceMenu(out string slip) => throw new NotImplementedException();

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      throw new NotImplementedException();
    }

    public bool DoPayment(
      Decimal sum,
      out string slip,
      out string rrn,
      out string method,
      out string approvalCode,
      out string issuerName,
      out string terminalId,
      out string cardNumber,
      out string paymentSystem)
    {
      method = string.Empty;
      sum *= 100M;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, Decimal, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Purchase", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__0.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__0, this.Terminal, sum, 0, 1);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target1 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p3 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__3;
      System.Type type1 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__2 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target2 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p2 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__1.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__1, this.Terminal);
      object obj2 = target2((CallSite) p2, "Terminal.LastResult ", obj1);
      target1((CallSite) p3, type1, obj2);
      this.WaitResponse(sum);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__6 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target3 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p6 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__6;
      System.Type type2 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__5 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target4 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p5 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__4.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__4, this.Terminal);
      object obj4 = target4((CallSite) p5, "Terminal.LastResult ", obj3);
      target3((CallSite) p6, type2, obj4);
      if (this.GetLastCode("Purchase") == 0)
      {
        LogHelper.Debug("Успешно Purchase");
        ref string local1 = ref rrn;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (OschadBank)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target5 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p8 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__8;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "RRN", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__7.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__7, this.Terminal);
        string str1 = target5((CallSite) p8, obj5);
        local1 = str1;
        ref string local2 = ref approvalCode;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (OschadBank)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target6 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__10.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p10 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__10;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AuthCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__9.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__9, this.Terminal);
        string str2 = target6((CallSite) p10, obj6);
        local2 = str2;
        ref string local3 = ref paymentSystem;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (OschadBank)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target7 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__12.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p12 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__12;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "IssuerName", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__11.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__11, this.Terminal);
        string str3 = target7((CallSite) p12, obj7);
        local3 = str3;
        ref string local4 = ref terminalId;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (OschadBank)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target8 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__14.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p14 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__14;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TerminalID", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__13.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__13, this.Terminal);
        string str4 = target8((CallSite) p14, obj8);
        local4 = str4;
        LogHelper.Debug(rrn);
        LogHelper.Debug(approvalCode);
        LogHelper.Debug(paymentSystem);
        LogHelper.Debug(terminalId);
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__15 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Confirm", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__15.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__15, this.Terminal);
        this.WaitResponse(sum);
        if (this.GetLastCode("Confirm") == 0)
        {
          LogHelper.Debug("Успешно Confirm");
          this.WaitResponse(sum);
          if (this.GetLastCode("Confirm WaitResponse") == 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__16 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__16 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReqCurrReceipt", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__16.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__16, this.Terminal);
            this.WaitResponse(0M);
            if (this.GetLastCode("ReqCurrReceipt") == 0)
            {
              ref string local5 = ref slip;
              // ISSUE: reference to a compiler-generated field
              if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__18 == null)
              {
                // ISSUE: reference to a compiler-generated field
                OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (OschadBank)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, string> target9 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__18.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, string>> p18 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__18;
              // ISSUE: reference to a compiler-generated field
              if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__17 == null)
              {
                // ISSUE: reference to a compiler-generated field
                OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Receipt", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj9 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__17.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__17, this.Terminal);
              string str5 = target9((CallSite) p18, obj9);
              local5 = str5;
              cardNumber = "";
              if (!slip.IsNullOrEmpty())
              {
                cardNumber = new Regex("\\b[0-9X]{16}\\b").Match(slip).Value;
                LogHelper.Debug("cardNumber = " + cardNumber);
                cardNumber = cardNumber.IsNullOrEmpty() ? "XXXXXXXXXXXXXXXX" : cardNumber;
              }
              issuerName = "";
              LogHelper.Debug("rrn = " + rrn);
              LogHelper.Debug("approvalCode = " + approvalCode);
              LogHelper.Debug("terminalId = " + terminalId);
              LogHelper.Debug("cardNumber = " + cardNumber);
              LogHelper.Debug("paymentSystem = " + paymentSystem);
              LogHelper.Debug("Terminal.Receipt = " + slip);
              return true;
            }
          }
        }
      }
      else if (this.GetLastCode("Purchase") == 4)
      {
        this.WaitResponse(sum);
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__21 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target10 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__21.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p21 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__21;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__20 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target11 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__20.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p20 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__20;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__19.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__19, this.Terminal);
        object obj11 = target11((CallSite) p20, obj10, 0);
        if (target10((CallSite) p21, obj11))
        {
          slip = "";
          rrn = "";
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__28 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__28 = CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, System.Type, OschadBank, object, AcquiringException> target12 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__28.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>> p28 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__28;
          System.Type type3 = typeof (AcquiringException);
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__27 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string, object> target13 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__27.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string, object>> p27 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__27;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__26 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target14 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__26.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p26 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__26;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string, object> target15 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__24.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string, object>> p24 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__24;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__23 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, string, object, object> target16 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__23.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, string, object, object>> p23 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__23;
          string терминалуПроизошлаОшибка = Translate.OschadBank_DoPayment_Не_удалось_провести_оплату_по_терминалу__произошла_ошибка_;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj12 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__22.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__22, this.Terminal);
          object obj13 = target16((CallSite) p23, терминалуПроизошлаОшибка, obj12);
          object obj14 = target15((CallSite) p24, obj13, "(");
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__25 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorDescription", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj15 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__25.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__25, this.Terminal);
          object obj16 = target14((CallSite) p26, obj14, obj15);
          object obj17 = target13((CallSite) p27, obj16, ")");
          throw target12((CallSite) p28, type3, this, obj17);
        }
      }
      slip = "";
      rrn = "";
      approvalCode = "";
      issuerName = "";
      terminalId = "";
      cardNumber = "";
      paymentSystem = "";
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__33 = CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, OschadBank, object, AcquiringException> target17 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__33.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>> p33 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__33;
      System.Type type4 = typeof (AcquiringException);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target18 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__32.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p32 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__32;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__30 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__30 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target19 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__30.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p30 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__30;
      string терминалуПроизошлаОшибка1 = Translate.OschadBank_DoPayment_Не_удалось_провести_оплату_по_терминалу__произошла_ошибка_;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj18 = OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__29.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__29, this.Terminal);
      object obj19 = target19((CallSite) p30, терминалуПроизошлаОшибка1, obj18);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__31 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorDescription", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      string str = string.Format(" ({0})", OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__31.Target((CallSite) OschadBank.\u003C\u003Eo__15.\u003C\u003Ep__31, this.Terminal));
      object obj20 = target18((CallSite) p32, obj19, str);
      throw target17((CallSite) p33, type4, this, obj20);
    }

    private void WaitResponse(Decimal sum)
    {
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastStatMsgCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__0.Target((CallSite) OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__0, this.Terminal);
      while (true)
      {
        Func<CallSite, object, bool> target1;
        CallSite<Func<CallSite, object, bool>> p12;
        object obj2;
        do
        {
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target2 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p3 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int, object> target3 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int, object>> p2 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__2;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__1.Target((CallSite) OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__1, this.Terminal);
          object obj4 = target3((CallSite) p2, obj3, 2);
          if (target2((CallSite) p3, obj4))
          {
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target4 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__9.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p9 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__9;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.And, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target5 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__8.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p8 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__8;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target6 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p5 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__5;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastStatMsgCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__4.Target((CallSite) OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__4, this.Terminal);
            object obj6 = target6((CallSite) p5, obj5, 0);
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target7 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__7.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p7 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__7;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastStatMsgCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__6.Target((CallSite) OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__6, this.Terminal);
            object obj8 = obj1;
            object obj9 = target7((CallSite) p7, obj7, obj8);
            object obj10 = target5((CallSite) p8, obj6, obj9);
            if (target4((CallSite) p9, obj10))
            {
              // ISSUE: reference to a compiler-generated field
              if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__10 == null)
              {
                // ISSUE: reference to a compiler-generated field
                OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastStatMsgCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              obj1 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__10.Target((CallSite) OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__10, this.Terminal);
            }
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            target1 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__12.Target;
            // ISSUE: reference to a compiler-generated field
            p12 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__12;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            obj2 = OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__11.Target((CallSite) OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__11, obj1, 11);
          }
          else
            goto label_34;
        }
        while (!target1((CallSite) p12, obj2));
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__13 = CallSite<Action<CallSite, object, Decimal, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CorrectTransaction", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__13.Target((CallSite) OschadBank.\u003C\u003Eo__16.\u003C\u003Ep__13, this.Terminal, sum, 0);
      }
label_34:;
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      sum *= 100M;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, Decimal, int, int, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Refund", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__0.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__0, this.Terminal, sum, 0, 1, rrn);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target1 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p3 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__3;
      System.Type type1 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__2 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target2 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p2 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__1.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__1, this.Terminal);
      object obj2 = target2((CallSite) p2, "Terminal.LastResult ", obj1);
      target1((CallSite) p3, type1, obj2);
      this.WaitResponse(sum);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__6 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target3 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p6 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__6;
      System.Type type2 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__5 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target4 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p5 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__4.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__4, this.Terminal);
      object obj4 = target4((CallSite) p5, "Terminal.LastResult ", obj3);
      target3((CallSite) p6, type2, obj4);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target5 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p9 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target6 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p8 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__7.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__7, this.Terminal);
      object obj6 = target6((CallSite) p8, obj5, 0);
      if (target5((CallSite) p9, obj6))
      {
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__10 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Confirm", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__10.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__10, this.Terminal);
        this.WaitResponse(sum);
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target7 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__13.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p13 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__13;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target8 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__12.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p12 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__12;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__11.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__11, this.Terminal);
        object obj8 = target8((CallSite) p12, obj7, 0);
        if (target7((CallSite) p13, obj8))
        {
          this.WaitResponse(sum);
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target9 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__16.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p16 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__16;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int, object> target10 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__15.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int, object>> p15 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__15;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj9 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__14.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__14, this.Terminal);
          object obj10 = target10((CallSite) p15, obj9, 0);
          if (target9((CallSite) p16, obj10))
          {
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__17 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__17 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReqCurrReceipt", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__17.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__17, this.Terminal);
            this.WaitResponse(0M);
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__20 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target11 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__20.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p20 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__20;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__19 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, int, object> target12 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__19.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, int, object>> p19 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__19;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__18 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj11 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__18.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__18, this.Terminal);
            object obj12 = target12((CallSite) p19, obj11, 0);
            if (target11((CallSite) p20, obj12))
            {
              ref string local = ref slip;
              // ISSUE: reference to a compiler-generated field
              if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__22 == null)
              {
                // ISSUE: reference to a compiler-generated field
                OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (OschadBank)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, string> target13 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__22.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, string>> p22 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__22;
              // ISSUE: reference to a compiler-generated field
              if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__21 == null)
              {
                // ISSUE: reference to a compiler-generated field
                OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Receipt", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj13 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__21.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__21, this.Terminal);
              string str = target13((CallSite) p22, obj13);
              local = str;
              return true;
            }
          }
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__25 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target14 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__25.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p25 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__25;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__24 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target15 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__24.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p24 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__24;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__23 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj14 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__23.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__23, this.Terminal);
        object obj15 = target15((CallSite) p24, obj14, 4);
        if (target14((CallSite) p25, obj15))
        {
          this.WaitResponse(sum);
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__28 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target16 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__28.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p28 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__28;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__27 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int, object> target17 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__27.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int, object>> p27 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__27;
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__26 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj16 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__26.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__26, this.Terminal);
          object obj17 = target17((CallSite) p27, obj16, 0);
          if (target16((CallSite) p28, obj17))
          {
            slip = "";
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__32 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__32 = CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, System.Type, OschadBank, object, AcquiringException> target18 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__32.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>> p32 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__32;
            System.Type type3 = typeof (AcquiringException);
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__31 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__31 = CallSite<Func<CallSite, System.Type, string, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, System.Type, string, object, object, object> target19 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__31.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, System.Type, string, object, object, object>> p31 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__31;
            System.Type type4 = typeof (string);
            string произошлаОшибка01 = Translate.OschadBank_ReturnPayment_Не_удалось_произвести_возврат_по_терминалу__произошла_ошибка____0____1__;
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__29 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj18 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__29.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__29, this.Terminal);
            // ISSUE: reference to a compiler-generated field
            if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__30 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorDescription", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj19 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__30.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__30, this.Terminal);
            object obj20 = target19((CallSite) p31, type4, произошлаОшибка01, obj18, obj19);
            throw target18((CallSite) p32, type3, this, obj20);
          }
        }
      }
      slip = "";
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__36 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__36 = CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, OschadBank, object, AcquiringException> target20 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__36.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>> p36 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__36;
      System.Type type5 = typeof (AcquiringException);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__35 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__35 = CallSite<Func<CallSite, System.Type, string, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Format", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, string, object, object, object> target21 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__35.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, string, object, object, object>> p35 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__35;
      System.Type type6 = typeof (string);
      string произошлаОшибка01_1 = Translate.OschadBank_ReturnPayment_Не_удалось_произвести_возврат_по_терминалу__произошла_ошибка____0____1__;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__33 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__33 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj21 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__33.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__33, this.Terminal);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__34 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorDescription", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj22 = OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__34.Target((CallSite) OschadBank.\u003C\u003Eo__17.\u003C\u003Ep__34, this.Terminal);
      object obj23 = target21((CallSite) p35, type6, произошлаОшибка01_1, obj21, obj22);
      throw target20((CallSite) p36, type5, this, obj23);
    }

    public bool GetReport(out string slip) => throw new NotImplementedException();

    public bool CloseSession(out string slip)
    {
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Settlement", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__0, this.Terminal, 0);
      slip = "";
      this.WaitResponse(0M);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p3 = OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target2 = OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p2 = OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__1.Target((CallSite) OschadBank.\u003C\u003Eo__19.\u003C\u003Ep__1, this.Terminal);
      object obj2 = target2((CallSite) p2, obj1, 0);
      if (!target1((CallSite) p3, obj2))
        return false;
      slip = "";
      return true;
    }

    public bool Connect()
    {
      this.Terminal = Functions.CreateObject("ECRCommX.BPOS1Lib");
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__0, this.Terminal, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.OschadBank_Connect_Не_удалось_создать_объект_драйвера_ОЩАД_банка);
      switch (this.DevicesConfig.AcquiringTerminal.ConnectionType)
      {
        case GlobalDictionaries.Devices.ConnectionTypes.NotSet:
          throw new AcquiringException((IDevice) this, Translate.LeoCas_Не_указан_способ_подключения);
        case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
          DeviceHelper.CheckComPortExists(this.DevicesConfig.AcquiringTerminal.ComPort.PortName, (IDevice) this);
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CommOpen", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__2.Target((CallSite) OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__2, this.Terminal, this.DevicesConfig.AcquiringTerminal.ComPort.PortNumber, this.DevicesConfig.AcquiringTerminal.ComPort.Speed);
          break;
        case GlobalDictionaries.Devices.ConnectionTypes.Lan:
          // ISSUE: reference to a compiler-generated field
          if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string, int?>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CommOpenTCP", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__3.Target((CallSite) OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__3, this.Terminal, this.DevicesConfig.AcquiringTerminal.LanConnection.UrlAddress, this.DevicesConfig.AcquiringTerminal.LanConnection.PortNumber);
          break;
        default:
          throw new KkmException((IDevice) this, Translate.LeoCas_Не_указан_способ_подключения, KkmException.ErrorTypes.NoConnection);
      }
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p5 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__4.Target((CallSite) OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__4, this.Terminal);
      object obj3 = target3((CallSite) p5, obj2, 0);
      if (target2((CallSite) p6, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "SetErrorLang", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__7.Target((CallSite) OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__7, this.Terminal, 2);
        return true;
      }
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__10 = CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, OschadBank, object, AcquiringException> target4 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>> p10 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__10;
      System.Type type = typeof (AcquiringException);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__9 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target5 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p9 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__9;
      string терминалуПроизошлаОшибка = Translate.OschadBank_Connect_Не_удалось_подключиться_к_терминалу__произошла_ошибка_;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__8.Target((CallSite) OschadBank.\u003C\u003Eo__20.\u003C\u003Ep__8, this.Terminal);
      object obj5 = target5((CallSite) p9, терминалуПроизошлаОшибка, obj4);
      throw target4((CallSite) p10, type, this, obj5);
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__0, this.Terminal, (object) null);
      if (target1((CallSite) p1, obj1))
        return true;
      object terminal1 = this.Terminal;
      if (terminal1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CommClose", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__2.Target((CallSite) OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__2, terminal1);
      }
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p4 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__4;
      object terminal2 = this.Terminal;
      object obj2;
      if (terminal2 == null)
      {
        obj2 = (object) null;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        obj2 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__3.Target((CallSite) OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__3, terminal2);
      }
      if (obj2 == null)
        obj2 = (object) 0;
      object obj3 = target3((CallSite) p4, obj2, 0);
      if (target2((CallSite) p5, obj3))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__8 = CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, OschadBank, object, AcquiringException> target4 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, OschadBank, object, AcquiringException>> p8 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__8;
      System.Type type = typeof (AcquiringException);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__7 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, string, object, object> target5 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, string, object, object>> p7 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__7;
      string терминалаПроизошлаОшибка = Translate.OschadBank_Disconnect_Не_удалось_отключиться_от_терминала__произошла_ошибка_;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__6.Target((CallSite) OschadBank.\u003C\u003Eo__21.\u003C\u003Ep__6, this.Terminal);
      object obj5 = target5((CallSite) p7, терминалаПроизошлаОшибка, obj4);
      throw target4((CallSite) p8, type, this, obj5);
    }

    public void EmergencyCancel() => throw new NotImplementedException();

    private int GetLastCode(string log)
    {
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastResult", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__0.Target((CallSite) OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__0, this.Terminal);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__3 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, System.Type, object> target1 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, System.Type, object>> p3 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__3;
      System.Type type1 = typeof (LogHelper);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target2 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> p2 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__1 = CallSite<Func<CallSite, string, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__1.Target((CallSite) OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__1, "LastResult = ", obj1);
      string str1 = " (" + log + ")";
      object obj3 = target2((CallSite) p2, obj2, str1);
      target1((CallSite) p3, type1, obj3);
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__4.Target((CallSite) OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__4, obj1, 0);
      if (target3((CallSite) p5, obj4))
      {
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__9 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Debug", (IEnumerable<System.Type>) null, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, System.Type, object> target4 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, System.Type, object>> p9 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__9;
        System.Type type2 = typeof (LogHelper);
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string, object> target5 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string, object>> p8 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__8;
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorCode", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__6.Target((CallSite) OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__6, this.Terminal);
        // ISSUE: reference to a compiler-generated field
        if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LastErrorDescription", typeof (OschadBank), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        string str2 = string.Format(" ({0})", OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__7.Target((CallSite) OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__7, this.Terminal));
        object obj6 = target5((CallSite) p8, obj5, str2);
        target4((CallSite) p9, type2, obj6);
      }
      // ISSUE: reference to a compiler-generated field
      if (OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (OschadBank)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__10.Target((CallSite) OschadBank.\u003C\u003Eo__23.\u003C\u003Ep__10, obj1);
    }
  }
}
