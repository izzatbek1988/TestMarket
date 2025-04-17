// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan.PortFPGKZ
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
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
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan
{
  internal class PortFPGKZ : IFiscalKkm, IDevice
  {
    private void CheckResult(int? resultCode)
    {
      resultCode.GetValueOrDefault();
      if (!resultCode.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        if (PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (PortFPGKZ)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p1 = PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__0, this.Driver);
        resultCode = new int?(target((CallSite) p1, obj));
      }
      int? nullable = resultCode;
      int num = 0;
      if (!(nullable.GetValueOrDefault() == num & nullable.HasValue))
      {
        // ISSUE: reference to a compiler-generated field
        if (PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (PortFPGKZ)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p3 = PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__2.Target((CallSite) PortFPGKZ.\u003C\u003Eo__0.\u003C\u003Ep__2, this.Driver);
        string str = target((CallSite) p3, obj);
        throw new KkmException((IDevice) this, string.Format(Translate.Код___0____Описание___1_, (object) resultCode, (object) str), !resultCode.HasValue || resultCode.GetValueOrDefault() != -1 ? KkmException.ErrorTypes.Unknown : KkmException.ErrorTypes.NoConnection);
      }
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => nameof (PortFPGKZ);

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    private object Driver { get; set; }

    public KkmLastActionResult LasActionResult { get; }

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection, ConnectionSettingsViewModel.PortsConfig.OnlyCom);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      char ch;
      switch (reportType)
      {
        case ReportTypes.ZReport:
          ch = 'Z';
          break;
        case ReportTypes.XReport:
          ch = 'X';
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__16.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__16.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B09249200\u007D<CallSite, object, char, int, Decimal, Decimal, Decimal, Decimal, Decimal, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "PrintReport", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[9]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      int num1;
      Decimal num2;
      Decimal num3;
      Decimal num4;
      Decimal num5;
      Decimal num6;
      Decimal num7;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__16.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__16.\u003C\u003Ep__0, this.Driver, ch, ref num1, ref num2, ref num3, ref num4, ref num5, ref num6, ref num7);
      this.CheckResult(new int?());
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      char ch1;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          ch1 = '0';
          break;
        case CheckTypes.ReturnSale:
          ch1 = '1';
          break;
        case CheckTypes.Buy:
          ch1 = '2';
          break;
        case CheckTypes.ReturnBuy:
          ch1 = '3';
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      char ch2 = ch1;
      switch (checkData.FiscalType)
      {
        case CheckFiscalTypes.Fiscal:
          // ISSUE: reference to a compiler-generated field
          if (PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00040000\u007D<CallSite, object, int, string, int, char, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenFiscalInv", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[6]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          int num1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__0, this.Driver, 1, "11", 1, ch2, ref num1);
          break;
        case CheckFiscalTypes.NonFiscal:
          // ISSUE: reference to a compiler-generated field
          if (PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__1 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenServiceInv", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
            }));
          }
          int num2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__1.Target((CallSite) PortFPGKZ.\u003C\u003Eo__17.\u003C\u003Ep__1, this.Driver, ref num2);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.CheckResult(new int?());
      return true;
    }

    public bool CloseCheck() => true;

    public bool CloseCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      if (data.FiscalType == CheckFiscalTypes.Fiscal)
      {
        // ISSUE: reference to a compiler-generated field
        if (PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CloseFiscalInv", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__0, this.Driver, ref num);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__1 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CloseServiceInv", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__1.Target((CallSite) PortFPGKZ.\u003C\u003Eo__19.\u003C\u003Ep__1, this.Driver, ref num);
      }
      this.CheckResult(new int?());
      return true;
    }

    public void CancelCheck()
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CancelFiscalInv", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__20.\u003C\u003Ep__0, this.Driver, ref num);
      this.CheckResult(new int?());
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00049000\u007D<CallSite, object, char, Decimal, Decimal, Decimal, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ParishOrConsumption", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[6]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      Decimal num1;
      Decimal num2;
      Decimal num3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__21.\u003C\u003Ep__0, this.Driver, '1', sum, ref num1, ref num2, ref num3);
      this.CheckResult(new int?());
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00049000\u007D<CallSite, object, char, Decimal, Decimal, Decimal, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ParishOrConsumption", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[6]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      Decimal num1;
      Decimal num2;
      Decimal num3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__22.\u003C\u003Ep__0, this.Driver, '0', sum, ref num1, ref num2, ref num3);
      this.CheckResult(new int?());
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00049000\u007D<CallSite, object, char, int, Decimal, Decimal, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ParishOrConsumption", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[6]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      Decimal num1;
      Decimal num2;
      Decimal num3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__24.\u003C\u003Ep__0, this.Driver, '0', 0, ref num1, ref num2, ref num3);
      this.CheckResult(new int?());
      sum = num1;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__25.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__25.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B40000000\u007D<CallSite, object, string, char, Decimal, Decimal, int, char, Decimal, Decimal, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RegisterSale", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[10]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__25.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__25.\u003C\u003Ep__0, this.Driver, good.Name, '1', good.Price, good.Quantity, good.KkmSectionNumber, good.Discount == 0M ? '0' : '2', good.Discount, good.Sum, ref num);
      this.CheckResult(new int?());
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      char ch1;
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Cash:
          ch1 = '0';
          break;
        case GlobalDictionaries.KkmPaymentMethods.Card:
          ch1 = '1';
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bank:
          ch1 = '1';
          break;
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.Credit:
          ch1 = '2';
          break;
        case GlobalDictionaries.KkmPaymentMethods.PrePayment:
          return true;
        case GlobalDictionaries.KkmPaymentMethods.EMoney:
          return true;
        default:
          throw new ArgumentOutOfRangeException();
      }
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__26.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__26.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00049000\u007D<CallSite, object, char, Decimal, char, Decimal, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Total", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[6]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      char ch2;
      Decimal num1;
      int num2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__26.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__26.\u003C\u003Ep__0, this.Driver, ch1, payment.Sum, ref ch2, ref num1, ref num2);
      this.CheckResult(new int?());
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      if (sum == 0M)
        return true;
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__27.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__27.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B49240000\u007D<CallSite, object, char, char, char, Decimal, int, Decimal, Decimal, Decimal, Decimal>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Subtotal", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[10]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      int num1;
      Decimal num2;
      Decimal num3;
      Decimal num4;
      Decimal num5;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__27.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__27.\u003C\u003Ep__0, this.Driver, '0', '0', '4', sum, ref num1, ref num2, ref num3, ref num4, ref num5);
      this.CheckResult(new int?());
      return true;
    }

    public bool GetCheckRemainder(out Decimal remainder)
    {
      remainder = 0M;
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null)
    {
      this.Driver = Functions.CreateObject("AddIn.NewtonPortFPGKZ");
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target((CallSite) p1, obj))
        throw new NullReferenceException(Translate.PortFPGKZ_Объект_драйвера_PortFPGKZ_не_был_создан);
      if (onlyDriverLoad)
      {
        LogHelper.Debug("onlyDriverLoad true");
      }
      else
      {
        if (devicesConfig == null)
          devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        // ISSUE: reference to a compiler-generated field
        if (PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__2 = CallSite<Action<CallSite, object, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (Connect), (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__2.Target((CallSite) PortFPGKZ.\u003C\u003Eo__29.\u003C\u003Ep__2, this.Driver, devicesConfig.CheckPrinter.Connection.ComPort.PortNumber, devicesConfig.CheckPrinter.Connection.ComPort.Speed);
        this.CheckResult(new int?());
      }
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__30.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__30.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (Disconnect), (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__30.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__30.\u003C\u003Ep__0, this.Driver);
      this.CheckResult(new int?());
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "OpenServiceInv", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      int num1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__0, this.Driver, ref num1);
      this.CheckResult(new int?());
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
      {
        char ch = '0';
        switch (nonFiscalString.Alignment)
        {
          case TextAlignment.Left:
            ch = '1';
            goto case TextAlignment.Justify;
          case TextAlignment.Right:
            ch = '3';
            goto case TextAlignment.Justify;
          case TextAlignment.Center:
            ch = '2';
            goto case TextAlignment.Justify;
          case TextAlignment.Justify:
            // ISSUE: reference to a compiler-generated field
            if (PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, string, char, char, char, char, char, char>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "PrintFreeTextInServiceInvV2", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[8]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__1.Target((CallSite) PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__1, this.Driver, nonFiscalString.Text, nonFiscalString.WideFont ? '1' : '0', '0', '0', '0', '0', ch);
            this.CheckResult(new int?());
            continue;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__2 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "CloseServiceInv", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      int num2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__2.Target((CallSite) PortFPGKZ.\u003C\u003Eo__35.\u003C\u003Ep__2, this.Driver, ref num2);
      this.CheckResult(new int?());
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper()
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__37.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__37.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (CutPaper), (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__37.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__37.\u003C\u003Ep__0, this.Driver);
      this.CheckResult(new int?());
      return true;
    }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__39.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__39.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00249240\u007D<CallSite, object, char, int, Decimal, Decimal, int, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FiscalTransactionsStatusV2", (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[7]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
        }));
      }
      char ch;
      int num1;
      Decimal num2;
      Decimal num3;
      int num4;
      int num5;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__39.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__39.\u003C\u003Ep__0, this.Driver, ref ch, ref num1, ref num2, ref num3, ref num4, ref num5);
      KkmStatus status = new KkmStatus();
      status.CheckNumber = num5;
      status.CheckStatus = ch == '0' ? CheckStatuses.Close : CheckStatuses.Open;
      status.KkmState = KkmStatuses.Ready;
      this.CheckResult(new int?());
      return status;
    }

    public bool OpenCashDrawer()
    {
      // ISSUE: reference to a compiler-generated field
      if (PortFPGKZ.\u003C\u003Eo__40.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        PortFPGKZ.\u003C\u003Eo__40.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, int>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (OpenCashDrawer), (IEnumerable<System.Type>) null, typeof (PortFPGKZ), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      PortFPGKZ.\u003C\u003Eo__40.\u003C\u003Ep__0.Target((CallSite) PortFPGKZ.\u003C\u003Eo__40.\u003C\u003Ep__0, this.Driver, 5000);
      this.CheckResult(new int?());
      return true;
    }

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();
  }
}
