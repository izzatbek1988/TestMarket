// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.ShtrihM
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  internal class ShtrihM : IOnlineKkm, IFiscalKkm, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.Devices_Штрих_М;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    private Dictionary<int, Decimal> Payments { get; set; }

    private object ShtrihDriver { get; set; }

    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData Data { get; set; }

    public KkmLastActionResult LasActionResult
    {
      get
      {
        return new KkmLastActionResult()
        {
          ActionResult = ActionsResults.Done
        };
      }
    }

    private void OffPrintDoc()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Password", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__0, this.ShtrihDriver, 30);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TableNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__1, this.ShtrihDriver, 17);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetTableStruct", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__2, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "FieldNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__3, this.ShtrihDriver, 7);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__4 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetFieldStruct", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__4, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TableNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__5, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FieldName", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__6.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__6, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "FieldType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__7, this.ShtrihDriver);
      LogHelper.Debug(string.Format("tab {0}, FieldName {1}, FieldType {2}", obj4, obj5, obj6));
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "RowNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__8.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__8, this.ShtrihDriver, 1);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ValueOfFieldInteger", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__9.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__9, this.ShtrihDriver, 1);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__10 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetFieldStruct", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__10.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__10, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__11 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "WriteTable", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__11.Target((CallSite) ShtrihM.\u003C\u003Eo__20.\u003C\u003Ep__11, this.ShtrihDriver);
    }

    public bool SendDigitalCheck(string address)
    {
      if (address.IsNullOrEmpty())
      {
        LogHelper.Debug("Адрес для отправки чека = пустая строка");
        return true;
      }
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CustomerEmail", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__0, this.ShtrihDriver, address);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNSendCustomerEmail", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__21.\u003C\u003Ep__1, this.ShtrihDriver);
      target((CallSite) p2, this, obj2);
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
        this.OffPrintDoc();
      return true;
    }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
      {
        string str = nonFiscalString.Text;
        if (nonFiscalString.Text.Length > 248)
          str = nonFiscalString.Text.Remove(247);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StringForPrinting", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__0, this.ShtrihDriver, str);
        int? resultCode;
        if (!nonFiscalString.WideFont)
        {
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int?>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int?), typeof (ShtrihM)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int?> target = ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int?>> p4 = ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__4;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PrintString", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__3, this.ShtrihDriver);
          resultCode = target((CallSite) p4, obj2);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int?>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int?), typeof (ShtrihM)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int?> target = ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int?>> p2 = ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__2;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PrintWideString", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__22.\u003C\u003Ep__1, this.ShtrihDriver);
          resultCode = target((CallSite) p2, obj3);
        }
        this.CheckResult(resultCode);
      }
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      if (type == BarcodeTypes.None || code.IsNullOrEmpty())
        return true;
      if (type != BarcodeTypes.Ean13)
      {
        if (type != BarcodeTypes.QrCode)
          throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Barcode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__3, this.ShtrihDriver, code);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarcodeType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__4, this.ShtrihDriver, 3);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarcodeParameter1", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__5, this.ShtrihDriver, 0);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarcodeParameter2", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__6.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__6, this.ShtrihDriver, 0);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarcodeParameter3", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__7, this.ShtrihDriver, 6);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarcodeParameter4", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__8.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__8, this.ShtrihDriver, 0);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarcodeParameter5", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__9.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__9, this.ShtrihDriver, 1);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarcodeAlignment", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj8 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__10.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__10, this.ShtrihDriver, 1);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__11 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "LoadAndPrint2DBarcode", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__11.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__11, this.ShtrihDriver);
        this.FeedPaper(5);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__0, this.ShtrihDriver, code);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PrintBarCode", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj10 = ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__23.\u003C\u003Ep__1, this.ShtrihDriver);
        target((CallSite) p2, this, obj10);
      }
      return true;
    }

    public bool GetCashSum(out Decimal sum)
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "RegisterNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__0, this.ShtrihDriver, 241);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target1 = ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetCashReg", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__1, this.ShtrihDriver);
      target1((CallSite) p2, this, obj2);
      ref Decimal local = ref sum;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, Decimal>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Decimal), typeof (ShtrihM)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Decimal> target2 = ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Decimal>> p4 = ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContentsOfCashRegister", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__24.\u003C\u003Ep__3, this.ShtrihDriver);
      Decimal num = target2((CallSite) p4, obj3);
      local = num;
      return true;
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (OpenSession), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__0, this.ShtrihDriver);
        target((CallSite) p1, this, obj);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__3 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target1 = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p3 = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNBeginOpenSession", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__2, this.ShtrihDriver);
        target1((CallSite) p3, this, obj1);
        this.WriteOfdAttribute(OfdAttributes.CashierName, (object) cashier.Name);
        this.WriteOfdAttribute(OfdAttributes.CashierInn, (object) cashier.Inn);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__5 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target2 = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p5 = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNOpenSession", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__25.\u003C\u003Ep__4, this.ShtrihDriver);
        target2((CallSite) p5, this, obj2);
      }
      Thread.Sleep(2000);
    }

    private void CloseSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PrintReportWithCleaning", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__0, this.ShtrihDriver);
        target((CallSite) p1, this, obj);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__3 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target1 = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p3 = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNBeginCloseSession", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__2, this.ShtrihDriver);
        target1((CallSite) p3, this, obj1);
        this.WriteOfdAttribute(OfdAttributes.CashierName, (object) cashier.Name);
        this.WriteOfdAttribute(OfdAttributes.CashierInn, (object) cashier.Inn);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__5 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target2 = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p5 = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNCloseSession", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__26.\u003C\u003Ep__4, this.ShtrihDriver);
        target2((CallSite) p5, this, obj2);
      }
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      switch (reportType)
      {
        case ReportTypes.ZReport:
          this.CloseSession(cashier);
          break;
        case ReportTypes.XReport:
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "PrintReportWithoutCleaning", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj = ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__27.\u003C\u003Ep__0, this.ShtrihDriver);
          target((CallSite) p1, this, obj);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    public void CancelCheck()
    {
      this.Data = (Gbs.Core.Devices.CheckPrinters.CheckData.CheckData) null;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (CancelCheck), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__28.\u003C\u003Ep__0, this.ShtrihDriver);
      target((CallSite) p1, this, obj);
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ1", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__0, this.ShtrihDriver, sum);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CashIncome", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__29.\u003C\u003Ep__1, this.ShtrihDriver);
      target((CallSite) p2, this, obj2);
      return true;
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ1", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__0, this.ShtrihDriver, sum);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CashOutcome", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__30.\u003C\u003Ep__1, this.ShtrihDriver);
      target((CallSite) p2, this, obj2);
      return true;
    }

    public bool Disconnect()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__0, this.ShtrihDriver, (object) null);
      if (target1((CallSite) p1, obj1))
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__3 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target2 = ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p3 = ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (Disconnect), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__2, this.ShtrihDriver);
      target2((CallSite) p3, this, obj2);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__4 = CallSite<Action<CallSite, System.Type, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReleaseComObject", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__31.\u003C\u003Ep__4, typeof (Marshal), this.ShtrihDriver);
      this.ShtrihDriver = (object) null;
      return true;
    }

    public bool IsConnected { get; set; }

    public bool CutPaper()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CutType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__0, this.ShtrihDriver, true);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CutCheck", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__1, this.ShtrihDriver);
      while (true)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target = ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p3 = ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__2, obj2, 80);
        if (target((CallSite) p3, obj3))
        {
          Thread.Sleep(300);
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CutCheck", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          obj2 = ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__4, this.ShtrihDriver);
        }
        else
          break;
      }
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__5 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__36.\u003C\u003Ep__5, this, obj2);
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value)
    {
      if (value == null || value.ToString().IsNullOrEmpty())
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__0, this.ShtrihDriver, (int) ofdAttribute);
      if (ofdAttribute <= OfdAttributes.CashierName)
      {
        if (ofdAttribute != OfdAttributes.ClientEmailPhone && ofdAttribute != OfdAttributes.CashierName)
          goto label_20;
      }
      else if (ofdAttribute != OfdAttributes.CashierInn)
      {
        if (ofdAttribute == OfdAttributes.UnitCode)
        {
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__3, this.ShtrihDriver, 2);
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueInt", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__4, this.ShtrihDriver, value);
          goto label_21;
        }
        else
          goto label_20;
      }
      if (value.ToString().IsNullOrEmpty())
        return true;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__1, this.ShtrihDriver, 7);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueStr", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__2, this.ShtrihDriver, value.ToString());
      goto label_21;
label_20:
      throw new ArgumentOutOfRangeException(nameof (ofdAttribute), (object) ofdAttribute, (string) null);
label_21:
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__6 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p6 = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNSendTag", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__37.\u003C\u003Ep__5, this.ShtrihDriver);
      target((CallSite) p6, this, obj6);
      return true;
    }

    public void ShowProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (ShowProperties), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__0, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__38.\u003C\u003Ep__1, this.ShtrihDriver);
      target((CallSite) p2, this, obj);
    }

    private void RegisterGood_OfflineFFD10(CheckGood good, CheckTypes checkType, Gbs.Core.Config.Devices dc)
    {
      if (checkType != CheckTypes.Sale)
      {
        if (checkType != CheckTypes.ReturnSale)
          throw new NotImplementedException();
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__3 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p3 = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReturnSale", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__2, this.ShtrihDriver);
        target((CallSite) p3, this, obj);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Sale", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__0, this.ShtrihDriver);
        target((CallSite) p1, this, obj);
      }
      if (dc.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm || !(good.DiscountSum > 0M))
        return;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StringForPrinting", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__4, this.ShtrihDriver, " ");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ1", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__5, this.ShtrihDriver, good.DiscountSum);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__7 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target1 = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p7 = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Discount", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__6.Target((CallSite) ShtrihM.\u003C\u003Eo__39.\u003C\u003Ep__6, this.ShtrihDriver);
      target1((CallSite) p7, this, obj3);
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      string str = good.Name;
      if (str.Length > 100)
        str = str.Substring(0, 100);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StringForPrinting", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__0, this.ShtrihDriver, str);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Price", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__1, this.ShtrihDriver, good.Price);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Quantity", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__2, this.ShtrihDriver, good.Quantity);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Department", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__3, this.ShtrihDriver, good.KkmSectionNumber);
      Gbs.Core.Config.Devices dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      switch (dc.CheckPrinter.FiscalKkm.FfdVersion)
      {
        case GlobalDictionaries.Devices.FfdVersions.OfflineKkm:
        case GlobalDictionaries.Devices.FfdVersions.Ffd100:
          this.RegisterGood_OfflineFFD10(good, checkType, dc);
          break;
        case GlobalDictionaries.Devices.FfdVersions.Ffd105:
        case GlobalDictionaries.Devices.FfdVersions.Ffd110:
        case GlobalDictionaries.Devices.FfdVersions.Ffd120:
          this.RegisterGood_onlineKkm(good, checkType);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.PrintNonFiscalStrings(good.CommentForFiscalCheck.Select<string, NonFiscalString>((Func<string, NonFiscalString>) (x => new NonFiscalString(x))).ToList<NonFiscalString>());
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StringForPrinting", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__40.\u003C\u003Ep__4, this.ShtrihDriver, "");
      return true;
    }

    private void RegisterGood_onlineKkm(CheckGood good, CheckTypes checkType)
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CheckType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target1 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__0.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p0 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__0;
      object shtrihDriver1 = this.ShtrihDriver;
      int num;
      if (checkType != CheckTypes.Sale)
      {
        if (checkType != CheckTypes.ReturnSale)
          throw new NotImplementedException();
        num = 2;
      }
      else
        num = 1;
      object obj1 = target1((CallSite) p0, shtrihDriver1, num);
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "MeasureUnit", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int, object> target2 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int, object>> p1 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__1;
        object shtrihDriver2 = this.ShtrihDriver;
        GoodsUnits.GoodUnit unit = good.Unit;
        int ruFfdUnitsIndex = unit != null ? unit.RuFfdUnitsIndex : 0;
        object obj2 = target2((CallSite) p1, shtrihDriver2, ruFfdUnitsIndex);
      }
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Tax1", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__2, this.ShtrihDriver, good.TaxRateNumber);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, GlobalDictionaries.RuFfdGoodsTypes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "PaymentItemSign", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__3, this.ShtrihDriver, good.RuFfdGoodTypeCode);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, GlobalDictionaries.RuFfdPaymentModes, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "PaymentTypeSign", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__4, this.ShtrihDriver, good.RuFfdPaymentModeCode);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ1Enabled", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__5, this.ShtrihDriver, false);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__7 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target3 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p7 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNOperation", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__6.Target((CallSite) ShtrihM.\u003C\u003Eo__41.\u003C\u003Ep__6, this.ShtrihDriver);
      target3((CallSite) p7, this, obj7);
      if (good.MarkedInfo == null)
        return;
      MarkedInfo markedInfo = good.MarkedInfo;
      if ((markedInfo != null ? (markedInfo.Type != 0 ? 1 : 0) : 1) == 0 || good.MarkedInfo.FullCode.IsNullOrEmpty())
        return;
      if (this.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
      {
        this.SetSalesNotice(this.Data.TrueApiInfoForKkm);
        this.SendMarkedInfo_ffd120(good);
      }
      else
        this.SendMarkedInfo_Ffd100_ffd110(good);
    }

    private void SendMarkedInfo_ffd120(CheckGood good)
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__0, this.ShtrihDriver, DataMatrixHelper.ReplaceSomeCharsToFNC1(good.MarkedInfo.FullCode));
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNSendItemBarcode", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__42.\u003C\u003Ep__1, this.ShtrihDriver);
      target((CallSite) p2, this, obj2);
    }

    private void SendMarkedInfo_Ffd100_ffd110(CheckGood good)
    {
      string gtin = good.MarkedInfo.Gtin;
      if ((gtin != null ? gtin.Length : 0) <= 0)
        return;
      string serial = good.MarkedInfo.Serial;
      if ((serial != null ? serial.Length : 0) <= 0)
        return;
      int num1;
      switch (good.MarkedInfo.Type)
      {
        case GlobalDictionaries.RuMarkedProductionTypes.None:
          num1 = 0;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Fur:
          throw new NotSupportedException();
        case GlobalDictionaries.RuMarkedProductionTypes.Drugs:
          num1 = 17485;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Tobacco:
          num1 = 5;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Shoes:
          num1 = 5408;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Perfume:
          num1 = 17485;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Tires:
          num1 = 17485;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.LightIndustry:
          num1 = 17485;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      int num2 = num1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "MarkingType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__0, this.ShtrihDriver, num2);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "GTIN", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__1, this.ShtrihDriver, good.MarkedInfo.Gtin);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "SerialNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__2, this.ShtrihDriver, good.MarkedInfo.Serial);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__4 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p4 = ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNSendItemCodeData", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__43.\u003C\u003Ep__3, this.ShtrihDriver);
      target((CallSite) p4, this, obj4);
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this.Data = checkData;
      this.Payments = new Dictionary<int, Decimal>();
      this.CheckDiscountsList = new List<ShtrihM.CheckDiscount>();
      for (int key = 1; key < 17; ++key)
        this.Payments.Add(key, 0M);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CheckType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target1 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__0.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p0 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__0;
      object shtrihDriver = this.ShtrihDriver;
      int num;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          num = 0;
          break;
        case CheckTypes.ReturnSale:
          num = 2;
          break;
        case CheckTypes.Buy:
          num = 1;
          break;
        case CheckTypes.ReturnBuy:
          num = 3;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      object obj1 = target1((CallSite) p0, shtrihDriver, num);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target2 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (OpenCheck), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__1, this.ShtrihDriver);
      target2((CallSite) p2, this, obj2);
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck)
        this.OffPrintDoc();
      if (this.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        this.WriteOfdAttribute(OfdAttributes.CashierName, (object) this.Data.Cashier.Name);
        this.WriteOfdAttribute(OfdAttributes.CashierInn, (object) this.Data.Cashier.Inn);
      }
      if (this.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
      {
        LogHelper.Debug("Начинаем проверку КМ для ФФД 1.2");
        foreach (CheckGood goods in checkData.GoodsList)
        {
          if (goods.MarkedInfo != null && !goods.MarkedInfo.FullCode.IsNullOrEmpty())
          {
            int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(goods, checkData.CheckType);
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "BarCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj3 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__3, this.ShtrihDriver, DataMatrixHelper.ReplaceSomeCharsToFNC1(goods.MarkedInfo.FullCode));
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ItemStatus", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj4 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__4, this.ShtrihDriver, markingCodeStatus);
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "CheckItemMode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj5 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__5, this.ShtrihDriver, 0);
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "MarkingType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj6 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__6.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__6, this.ShtrihDriver, 17485);
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DivisionalQuantity", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__7, this.ShtrihDriver, false);
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TLVDataHex", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj8 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__8.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__8, this.ShtrihDriver, "");
            if (markingCodeStatus.IsEither<int>(2, 4))
            {
              LogHelper.Debug("Записываем для дробного количества данные");
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__9 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj9 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__9.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__9, this.ShtrihDriver, 2108);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__10 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj10 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__10.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__10, this.ShtrihDriver, 0);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__11 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueInt", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj11 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__11.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__11, this.ShtrihDriver, goods.Unit.RuFfdUnitsIndex);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__12 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__12 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetTagAsTLV", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__12.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__12, this.ShtrihDriver);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__13 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "TLVDataHex", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj12 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__13.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__13, this.ShtrihDriver);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__14 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj13 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__14.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__14, this.ShtrihDriver, 1023);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__15 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj14 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__15.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__15, this.ShtrihDriver, 4);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__16 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueFVLN", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj15 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__16.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__16, this.ShtrihDriver, goods.Quantity);
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__17 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__17 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetTagAsTLV", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__17.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__17, this.ShtrihDriver);
              int ruFfdUnitsIndex = goods.Unit.RuFfdUnitsIndex;
              // ISSUE: reference to a compiler-generated field
              if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__18 == null)
              {
                // ISSUE: reference to a compiler-generated field
                ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TLVDataHex", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj16 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__18.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__18, this.ShtrihDriver, obj12);
            }
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__20 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__20 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, ShtrihM, object> target3 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__20.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, ShtrihM, object>> p20 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__20;
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__19 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNCheckItemBarcode", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj17 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__19.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__19, this.ShtrihDriver);
            target3((CallSite) p20, this, obj17);
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__22 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__22 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Action<CallSite, ShtrihM, object> target4 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__22.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Action<CallSite, ShtrihM, object>> p22 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__22;
            // ISSUE: reference to a compiler-generated field
            if (ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__21 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNAcceptMarkingCode", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj18 = ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__21.Target((CallSite) ShtrihM.\u003C\u003Eo__44.\u003C\u003Ep__21, this.ShtrihDriver);
            target4((CallSite) p22, this, obj18);
            LogHelper.Debug("Код КМ успешно проверен");
          }
        }
      }
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      this.ShtrihDriver = Functions.CreateObject("AddIn.DRvFR");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__0, this.ShtrihDriver, (object) null);
      if (target1((CallSite) p1, obj1))
        throw new NullReferenceException(Translate.ShtrihM_Объект_драйвера_ККМ_Штрих_М_не_был_создан);
      if (onlyDriverLoad)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__3 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target2 = ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p3 = ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (Connect), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__45.\u003C\u003Ep__2, this.ShtrihDriver);
      target2((CallSite) p3, this, obj2);
      this.WaitPrint();
    }

    private void CheckResult(int? resultCode)
    {
      resultCode.GetValueOrDefault();
      if (!resultCode.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (ShtrihM)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p1 = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__0, this.ShtrihDriver);
        resultCode = new int?(target((CallSite) p1, obj));
      }
      int? nullable = resultCode;
      int num1 = 0;
      if (!(nullable.GetValueOrDefault() == num1 & nullable.HasValue))
      {
        if (resultCode.GetValueOrDefault() == 88)
        {
          LogHelper.Debug("Ошибка 88. Пытаемся допечатать документ");
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__2 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ContinuePrint", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__2, this.ShtrihDriver);
          ref int? local = ref resultCode;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (ShtrihM)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int> target = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int>> p4 = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__4;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__3, this.ShtrihDriver);
          int num2 = target((CallSite) p4, obj);
          local = new int?(num2);
          nullable = resultCode;
          LogHelper.Debug("Результат допечатования: " + nullable.ToString());
          nullable = resultCode;
          int num3 = 0;
          if (nullable.GetValueOrDefault() == num3 & nullable.HasValue)
            return;
        }
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (string), typeof (ShtrihM)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target1 = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p6 = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__6;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCodeDescription", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__46.\u003C\u003Ep__5, this.ShtrihDriver);
        string str1 = target1((CallSite) p6, obj1);
        string str2 = string.Format(Translate.Код___0____Описание___1_, (object) resultCode, (object) str1);
        KkmException.ErrorTypes errorTypes;
        if (resultCode.HasValue)
        {
          switch (resultCode.GetValueOrDefault())
          {
            case -1:
              errorTypes = KkmException.ErrorTypes.NoConnection;
              goto label_26;
            case 107:
              errorTypes = KkmException.ErrorTypes.NoPaper;
              goto label_26;
          }
        }
        errorTypes = KkmException.ErrorTypes.Unknown;
label_26:
        KkmException.ErrorTypes key = errorTypes;
        throw new ErrorHelper.GbsException(KkmException.ErrorsDictionary[key] + "\n" + str2)
        {
          Direction = key == KkmException.ErrorTypes.Unknown ? ErrorHelper.ErrorDirections.Unknown : ErrorHelper.ErrorDirections.Outer
        };
      }
    }

    private KkmStatuses GetKkmState()
    {
      KkmStatuses kkmState = KkmStatuses.Ready;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target1 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetECRStatus", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__0, this.ShtrihDriver);
      target1((CallSite) p1, this, obj1);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p4 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool, object> target3 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool, object>> p3 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ReceiptRibbonIsPresent", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__2, this.ShtrihDriver);
      object obj3 = target3((CallSite) p3, obj2, false);
      if (target2((CallSite) p4, obj3))
        kkmState = KkmStatuses.NoPaper;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "LidPositionSensor", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__47.\u003C\u003Ep__5, this.ShtrihDriver);
      if (target4((CallSite) p6, obj4))
        kkmState = KkmStatuses.CoverOpen;
      return kkmState;
    }

    public KkmStatus GetShortStatus()
    {
      KkmStatus shortStatus = new KkmStatus()
      {
        KkmState = this.GetKkmState()
      };
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target1 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetECRStatus", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__0, this.ShtrihDriver);
      target1((CallSite) p1, this, obj1);
      KkmStatus kkmStatus1 = shortStatus;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ECRMode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      SessionStatuses sessionStatuses;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__2, this.ShtrihDriver) is int num1)
      {
        switch (num1)
        {
          case 2:
            sessionStatuses = SessionStatuses.Open;
            goto label_13;
          case 3:
            sessionStatuses = SessionStatuses.OpenMore24Hours;
            goto label_13;
          case 4:
            sessionStatuses = SessionStatuses.Close;
            goto label_13;
          case 8:
            sessionStatuses = SessionStatuses.Open;
            goto label_13;
        }
      }
      sessionStatuses = SessionStatuses.Unknown;
label_13:
      kkmStatus1.SessionStatus = sessionStatuses;
      KkmStatus kkmStatus2 = shortStatus;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p5 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target3 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p4 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ECRMode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__48.\u003C\u003Ep__3, this.ShtrihDriver);
      object obj3 = target3((CallSite) p4, obj2, 8);
      int num2 = target2((CallSite) p5, obj3) ? 1 : 2;
      kkmStatus2.CheckStatus = (CheckStatuses) num2;
      this.WaitPrint();
      return shortStatus;
    }

    public KkmStatus GetStatus()
    {
      KkmStatus status = new KkmStatus()
      {
        KkmState = this.GetKkmState()
      };
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__1 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target1 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p1 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetECRStatus", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__0, this.ShtrihDriver);
      target1((CallSite) p1, this, obj1);
      KkmStatus kkmStatus1 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ECRMode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      SessionStatuses sessionStatuses;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__2, this.ShtrihDriver) is int num1)
      {
        switch (num1)
        {
          case 2:
            sessionStatuses = SessionStatuses.Open;
            goto label_13;
          case 3:
            sessionStatuses = SessionStatuses.OpenMore24Hours;
            goto label_13;
          case 4:
            sessionStatuses = SessionStatuses.Close;
            goto label_13;
          case 8:
            sessionStatuses = SessionStatuses.Open;
            goto label_13;
        }
      }
      sessionStatuses = SessionStatuses.Unknown;
label_13:
      kkmStatus1.SessionStatus = sessionStatuses;
      KkmStatus kkmStatus2 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ShtrihM)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target2 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p4 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SerialNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__3, this.ShtrihDriver);
      string str1 = target2((CallSite) p4, obj2);
      kkmStatus2.FactoryNumber = str1;
      KkmStatus kkmStatus3 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ShtrihM)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target3 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p6 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ECRSoftVersion", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__5, this.ShtrihDriver);
      string str2 = target3((CallSite) p6, obj3);
      kkmStatus3.SoftwareVersion = str2;
      KkmStatus kkmStatus4 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target4 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p9 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__9;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target5 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p8 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ECRMode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__7, this.ShtrihDriver);
      object obj5 = target5((CallSite) p8, obj4, 8);
      int num2 = target4((CallSite) p9, obj5) ? 1 : 2;
      kkmStatus4.CheckStatus = (CheckStatuses) num2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "RegisterNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__10.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__10, this.ShtrihDriver, 148);
      KkmStatus kkmStatus5 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target6 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p13 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target7 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__12.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p12 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__12;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "GetOperationReg", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__11.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__11, this.ShtrihDriver);
      object obj8 = target7((CallSite) p12, obj7, 0);
      int num3;
      if (!target6((CallSite) p13, obj8))
      {
        num3 = 1;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__16 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (ShtrihM)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target8 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__16.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p16 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__16;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__15 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__15 = CallSite<Func<CallSite, System.Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToInt32", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, System.Type, object, object> target9 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__15.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, System.Type, object, object>> p15 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__15;
        System.Type type = typeof (Convert);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ContentsOfOperationRegister", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj9 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__14.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__14, this.ShtrihDriver);
        object obj10 = target9((CallSite) p15, type, obj9);
        num3 = target8((CallSite) p16, obj10);
      }
      kkmStatus5.CheckNumber = num3;
      KkmStatus kkmStatus6 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__19 = CallSite<Func<CallSite, System.Type, object, Version>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, System.Type, object, Version> target10 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__19.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, System.Type, object, Version>> p19 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__19;
      System.Type type1 = typeof (Version);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target11 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__18.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p18 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__18;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "DriverVersion", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__17.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__17, this.ShtrihDriver);
      object obj12 = target11((CallSite) p18, obj11);
      Version version = target10((CallSite) p19, type1, obj12);
      kkmStatus6.DriverVersion = version;
      KkmStatus kkmStatus7 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__22 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (ShtrihM)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int> target12 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__22.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int>> p22 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__22;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__21 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Add, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, int, object> target13 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__21.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, int, object>> p21 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__21;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__20 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "SessionNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj13 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__20.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__20, this.ShtrihDriver);
      object obj14 = target13((CallSite) p21, obj13, 1);
      int num4 = target12((CallSite) p22, obj14);
      kkmStatus7.SessionNumber = num4;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__23 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__23 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetDeviceMetrics", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__23.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__23, this.ShtrihDriver);
      KkmStatus kkmStatus8 = status;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (ShtrihM)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target14 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__25.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p25 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__25;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__24 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "UDescription", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj15 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__24.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__24, this.ShtrihDriver);
      string str3 = target14((CallSite) p25, obj15);
      kkmStatus8.Model = str3;
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__27 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__27 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target15 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__27.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p27 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__27;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__26 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNGetInfoExchangeStatus", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj16 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__26.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__26, this.ShtrihDriver);
        target15((CallSite) p27, this, obj16);
        KkmStatus kkmStatus9 = status;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__29 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, DateTime?>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (DateTime?), typeof (ShtrihM)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, DateTime?> target16 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__29.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, DateTime?>> p29 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__29;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__28 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__28 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Date", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj17 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__28.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__28, this.ShtrihDriver);
        DateTime? nullable = target16((CallSite) p29, obj17);
        kkmStatus9.OfdLastSendDateTime = nullable;
        KkmStatus kkmStatus10 = status;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__31 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (ShtrihM)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target17 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__31.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p31 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__31;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__30 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "MessageCount", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj18 = ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__30.Target((CallSite) ShtrihM.\u003C\u003Eo__49.\u003C\u003Ep__30, this.ShtrihDriver);
        int num5 = target17((CallSite) p31, obj18);
        kkmStatus10.OfdNotSendDocuments = num5;
      }
      this.WaitPrint();
      return status;
    }

    private void WaitPrint()
    {
      for (int index = 0; index < 10; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__0 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "GetECRStatus", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__0, this.ShtrihDriver);
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (int), typeof (ShtrihM)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, int> target = ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, int>> p2 = ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ECRAdvancedMode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__50.\u003C\u003Ep__1, this.ShtrihDriver);
        int num = target((CallSite) p2, obj);
        LogHelper.Debug("ShtrihDriver.ECRAdvancedMode " + num.ToString());
        if (!num.IsEither<int>(4, 5))
          break;
        LogHelper.Debug("Не закончена печать предыдущей команды, ждем 1 секунду");
        Thread.Sleep(1000);
      }
    }

    private void RegisterAllPayments()
    {
      LogHelper.Debug("Список платежей ШТРИХ М\n" + this.Payments.ToJsonString(true));
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ1", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__0, this.ShtrihDriver, this.Payments[1]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ2", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__1, this.ShtrihDriver, this.Payments[2]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ3", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__2, this.ShtrihDriver, this.Payments[3]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ4", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__3, this.ShtrihDriver, this.Payments[4]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ5", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__4, this.ShtrihDriver, this.Payments[5]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ6", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__5, this.ShtrihDriver, this.Payments[6]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ7", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__6.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__6, this.ShtrihDriver, this.Payments[7]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ8", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__7, this.ShtrihDriver, this.Payments[8]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ9", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__8.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__8, this.ShtrihDriver, this.Payments[9]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ10", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__9.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__9, this.ShtrihDriver, this.Payments[10]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ11", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__10.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__10, this.ShtrihDriver, this.Payments[11]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ12", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__11.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__11, this.ShtrihDriver, this.Payments[12]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ13", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj13 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__12.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__12, this.ShtrihDriver, this.Payments[13]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ14", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj14 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__13.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__13, this.ShtrihDriver, this.Payments[14]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ15", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj15 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__14.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__14, this.ShtrihDriver, this.Payments[15]);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ16", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj16 = ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__15.Target((CallSite) ShtrihM.\u003C\u003Eo__51.\u003C\u003Ep__15, this.ShtrihDriver, this.Payments[16]);
    }

    private void RegisterAllCheckDiscount()
    {
      foreach (ShtrihM.CheckDiscount checkDiscount in this.CheckDiscountsList.Where<ShtrihM.CheckDiscount>((Func<ShtrihM.CheckDiscount, bool>) (x => x.Sum > 0M)))
      {
        if (this.FfdVersion == GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
        {
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StringForPrinting", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__0, this.ShtrihDriver, checkDiscount.Description);
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Decimal, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Summ1", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__1, this.ShtrihDriver, checkDiscount.Sum);
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__3 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, ShtrihM, object>> p3 = ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Discount", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__52.\u003C\u003Ep__2, this.ShtrihDriver);
          target((CallSite) p3, this, obj3);
        }
        else
          this.PrintNonFiscalStrings(new List<NonFiscalString>()
          {
            new NonFiscalString(string.Format("{0}: {1:N2}", (object) checkDiscount.Description, (object) checkDiscount.Sum), TextAlignment.Right)
          });
      }
    }

    public bool CloseCheck()
    {
      if (this.Data == null)
        throw new ArgumentNullException("Data");
      this.RegisterAllCheckDiscount();
      this.RegisterAllPayments();
      if (this.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        int num1;
        switch (this.Data.RuTaxSystem)
        {
          case GlobalDictionaries.RuTaxSystems.None:
            num1 = 0;
            break;
          case GlobalDictionaries.RuTaxSystems.Osn:
            num1 = 1;
            break;
          case GlobalDictionaries.RuTaxSystems.UsnDohod:
            num1 = 2;
            break;
          case GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod:
            num1 = 4;
            break;
          case GlobalDictionaries.RuTaxSystems.Envd:
            num1 = 8;
            break;
          case GlobalDictionaries.RuTaxSystems.Esn:
            num1 = 16;
            break;
          case GlobalDictionaries.RuTaxSystems.Psn:
            num1 = 32;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        int num2 = num1;
        if (num2 > 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TaxType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj = ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__0, this.ShtrihDriver, num2);
        }
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "FNCloseCheckEx", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__1, this.ShtrihDriver);
        target((CallSite) p2, this, obj1);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__4 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, ShtrihM, object>> p4 = ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, nameof (CloseCheck), (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__53.\u003C\u003Ep__3, this.ShtrihDriver);
        target((CallSite) p4, this, obj);
      }
      return true;
    }

    private GlobalDictionaries.Devices.FfdVersions FfdVersion
    {
      get => new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion;
    }

    public bool OpenCashDrawer()
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DrawerNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__0, this.ShtrihDriver, 0);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__2 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p2 = ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "OpenDrawer", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__56.\u003C\u003Ep__1, this.ShtrihDriver);
      target((CallSite) p2, this, obj2);
      return true;
    }

    private List<ShtrihM.CheckDiscount> CheckDiscountsList { get; set; }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      this.CheckDiscountsList.Add(new ShtrihM.CheckDiscount()
      {
        Sum = sum,
        Description = description
      });
      return true;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      Dictionary<GlobalDictionaries.KkmPaymentMethods, int> paymentsMethods = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.PaymentsMethods;
      if (payment.Method.IsEither<GlobalDictionaries.KkmPaymentMethods>(GlobalDictionaries.KkmPaymentMethods.Bonus))
        return true;
      int key = paymentsMethods[payment.Method];
      if (!this.Payments.ContainsKey(key))
        throw new KkmException((IDevice) this, this.Name, KkmException.ErrorTypes.UnCorrectPaymentIndex);
      this.Payments[key] += payment.Sum;
      return true;
    }

    public void FeedPaper(int lines)
    {
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "StringQuantity", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__0, this.ShtrihDriver, lines);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FeedDocument", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__64.\u003C\u003Ep__1, this.ShtrihDriver);
    }

    public string PrepareMarkCodeForFfd120(string code)
    {
      return DataMatrixHelper.ReplaceSomeCharsToFNC1(code);
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();

    private void SetSalesNotice(string info)
    {
      if (info.IsNullOrEmpty() || new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        return;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__0.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__0, this.ShtrihDriver, 1262);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__1.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__1, this.ShtrihDriver, 7);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueStr", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__2.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__2, this.ShtrihDriver, "030");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FNSendTagOperation", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__3.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__3, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__4.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__4, this.ShtrihDriver, 1263);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__5.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__5, this.ShtrihDriver, 7);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueStr", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__6.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__6, this.ShtrihDriver, "21.11.2023");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__7 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FNSendTagOperation", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__7.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__7, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__8.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__8, this.ShtrihDriver, 1264);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj8 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__9.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__9, this.ShtrihDriver, 7);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueStr", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj9 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__10.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__10, this.ShtrihDriver, "1944");
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__11 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FNSendTagOperation", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__11.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__11, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagNumber", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj10 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__12.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__12, this.ShtrihDriver, 1265);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagType", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj11 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__13.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__13, this.ShtrihDriver, 7);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "TagValueStr", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj12 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__14.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__14, this.ShtrihDriver, info);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__15 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__15 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FNSendTagOperation", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__15.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__15, this.ShtrihDriver);
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__17 = CallSite<Action<CallSite, ShtrihM, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "CheckResult", (IEnumerable<System.Type>) null, typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, ShtrihM, object> target = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__17.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, ShtrihM, object>> p17 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__17;
      // ISSUE: reference to a compiler-generated field
      if (ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ResultCode", typeof (ShtrihM), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj13 = ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__16.Target((CallSite) ShtrihM.\u003C\u003Eo__67.\u003C\u003Ep__16, this.ShtrihDriver);
      target((CallSite) p17, this, obj13);
    }

    private class CheckDiscount
    {
      public Decimal Sum { get; set; }

      public string Description { get; set; }
    }
  }
}
