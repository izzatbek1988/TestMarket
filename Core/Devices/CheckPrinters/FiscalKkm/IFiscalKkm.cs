// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.IFiscalKkm
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm
{
  public interface IFiscalKkm : IDevice
  {
    GlobalDictionaries.Devices.FfdVersions GetFfdVersion();

    bool IsCanHoldConnection { get; }

    KkmLastActionResult LasActionResult { get; }

    void ShowProperties();

    void OpenSession(Cashier cashier);

    void GetReport(ReportTypes reportType, Cashier cashier);

    bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData);

    bool CloseCheck();

    void CancelCheck();

    bool CashOut(Decimal sum, Cashier cashier);

    bool CashIn(Decimal sum, Cashier cashier);

    bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value);

    bool GetCashSum(out Decimal sum);

    bool RegisterGood(CheckGood good, CheckTypes checkType);

    bool RegisterPayment(CheckPayment payment);

    bool RegisterCheckDiscount(Decimal sum, string description);

    void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices devicesConfig = null);

    bool Disconnect();

    bool IsConnected { get; set; }

    void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings);

    bool PrintBarcode(string code, BarcodeTypes type);

    bool CutPaper();

    KkmStatus GetStatus();

    KkmStatus GetShortStatus();

    bool OpenCashDrawer();

    bool SendDigitalCheck(string adress);

    void FeedPaper(int lines);

    bool EndPrintOldCheck();
  }
}
