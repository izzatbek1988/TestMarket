// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.VikiPrint
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models
{
  public class VikiPrint : IFiscalKkm, IDevice
  {
    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData _checkData;

    public static string PrepareMarkCodeForFfd120(string code)
    {
      string oldValue = Convert.ToString(Convert.ToChar(29));
      return (code?.Trim()?.Replace(" ", "$1d") ?? "")?.Trim()?.Replace(oldValue, "$1d") ?? "";
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => nameof (VikiPrint);

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    private string CashierInnAndName(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      string str = string.Empty;
      if (cashier == null)
        return string.Empty;
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (cashier.Inn != null && cashier.Inn.Length >= 10 && devices.CheckPrinter.FiscalKkm.FfdVersion > GlobalDictionaries.Devices.FfdVersions.Ffd100)
        str = cashier.Inn + "&";
      return str + cashier.Name;
    }

    private bool SendDigitalCheck(string address)
    {
      if (address.IsNullOrEmpty())
        return true;
      this.CheckResult(VikiPrint.Driver.SetClientAddress(address));
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.IsNoPrintCheckIfSendDigitalCheck)
        this.CheckResult(VikiPrint.Driver.SetPrintCheck(129));
      return true;
    }

    private bool SetClientNameInn(Gbs.Core.Entities.Clients.Client client)
    {
      if (client != null)
      {
        VikiPrint.Driver.SetClientName(client.Name);
        string str = client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
        if (!str.IsNullOrEmpty())
          VikiPrint.Driver.SetClientInn(str);
      }
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public KkmLastActionResult LasActionResult { get; private set; } = new KkmLastActionResult();

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection.ComPort, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Команда открытия смена на ККМ ВикиПринт игнорируется, т.к. ее нет в драйвере");
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CheckResult(VikiPrint.Driver.SetPrintCheck(1));
      string cashierName = this.CashierInnAndName(cashier);
      switch (reportType)
      {
        case ReportTypes.ZReport:
          this.CheckResult(VikiPrint.Driver.GetZReport(cashierName));
          break;
        case ReportTypes.XReport:
          this.CheckResult(VikiPrint.Driver.GetXReport(cashierName));
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
    }

    private void Ffd120CodeValidation(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      LogHelper.Debug("Начинаю валидацию КМ для ФФД 120, ВикиПринт");
      foreach (CheckGood checkGood in checkData.GoodsList.Where<CheckGood>((Func<CheckGood, bool>) (item => item.MarkedInfo != null && !item.MarkedInfo.FullCode.IsNullOrEmpty())))
      {
        checkGood.MarkedInfo.ValidationResultKkm = (object) 0;
        string fullCode = VikiPrint.PrepareMarkCodeForFfd120(checkGood.MarkedInfo.FullCode);
        int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(checkGood, checkData.CheckType);
        LogHelper.Debug("Валидация кода: " + fullCode);
        int result;
        VikiPrint.Driver.MarkCodeValidation(out result, fullCode, checkGood.Quantity, markingCodeStatus, checkGood.Unit.RuFfdUnitsIndex);
        checkGood.MarkedInfo.ValidationResultKkm = (object) result;
        LogHelper.Debug(string.Format("Validation result code: {0}", checkGood.MarkedInfo.ValidationResultKkm));
        VikiPrint.Driver.AcceptMarkingCode();
      }
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this._checkData = checkData;
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      string userName = checkData.Cashier.Name;
      VikiPrint.Driver.DocTypes docType;
      if (checkData.FiscalType == CheckFiscalTypes.Fiscal)
      {
        VikiPrint.Driver.DocTypes docTypes;
        switch (checkData.CheckType)
        {
          case CheckTypes.Sale:
            docTypes = VikiPrint.Driver.DocTypes.SaleCheck;
            break;
          case CheckTypes.ReturnSale:
            docTypes = VikiPrint.Driver.DocTypes.ReturnCheck;
            break;
          case CheckTypes.Buy:
            throw new NotImplementedException();
          case CheckTypes.ReturnBuy:
            throw new NotImplementedException();
          default:
            throw new ArgumentOutOfRangeException();
        }
        docType = docTypes;
        userName = this.CashierInnAndName(checkData.Cashier);
      }
      else
        docType = VikiPrint.Driver.DocTypes.Service;
      this.CheckResult(VikiPrint.Driver.SetPrintCheck(devices.CheckPrinter.FiscalKkm.IsAlwaysNoPrintCheck ? 129 : 1));
      int resultCode = VikiPrint.Driver.OpenDocument(docType, 1, userName, checkData.RuTaxSystem);
      if (checkData.FiscalType == CheckFiscalTypes.Fiscal)
        this.SendDigitalCheck(checkData.AddressForDigitalCheck);
      this.CheckResult(resultCode);
      if (devices.CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.Ffd120)
        this.Ffd120CodeValidation(checkData);
      return true;
    }

    public bool CloseCheck()
    {
      this.CheckResult(VikiPrint.Driver.CloseDocument());
      return true;
    }

    public void CancelCheck() => this.CheckResult(VikiPrint.Driver.CancelDocument());

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CheckResult(VikiPrint.Driver.OpenDocument(VikiPrint.Driver.DocTypes.CashOutcome, 1, this.CashierInnAndName(cashier)));
      this.CheckResult(VikiPrint.Driver.CashInOut(sum));
      this.CheckResult(VikiPrint.Driver.CloseDocument());
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CheckResult(VikiPrint.Driver.OpenDocument(VikiPrint.Driver.DocTypes.CashIncome, 1, this.CashierInnAndName(cashier)));
      this.CheckResult(VikiPrint.Driver.CashInOut(sum));
      this.CheckResult(VikiPrint.Driver.CloseDocument());
      return true;
    }

    public bool GetCashSum(out Decimal sum)
    {
      this.CheckResult(VikiPrint.Driver.GetCashSum(out sum));
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      switch (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion)
      {
        case GlobalDictionaries.Devices.FfdVersions.OfflineKkm:
        case GlobalDictionaries.Devices.FfdVersions.Ffd100:
          this.CheckResult(VikiPrint.Driver.AddPosition(good.Name, string.Empty, good.Quantity, good.Price, good.TaxRateNumber, good.KkmSectionNumber));
          return true;
        case GlobalDictionaries.Devices.FfdVersions.Ffd105:
        case GlobalDictionaries.Devices.FfdVersions.Ffd110:
          if (VikiPrint.Driver.SetMarkedRequisite(good.MarkedInfo, "") != 0)
            LogHelper.Error(new Exception(), "Реквизит маркируемой продукции не был записан", false);
          this.CheckResult(VikiPrint.Driver.AddPosition(good.Name, string.Empty, good.Quantity, good.Price, good.TaxRateNumber, good.KkmSectionNumber, good.RuFfdPaymentModeCode, good.RuFfdGoodTypeCode));
          return true;
        case GlobalDictionaries.Devices.FfdVersions.Ffd120:
          string str = VikiPrint.PrepareMarkCodeForFfd120(good.MarkedInfo.FullCode);
          if (!str.IsNullOrEmpty())
          {
            int markingCodeStatus = RuOnlineKkmHelper.GetMarkingCodeStatus(good, checkType);
            this.CheckResult(VikiPrint.Driver.AddItemMarkingCode(str, markingCodeStatus, good.Unit.RuFfdUnitsIndex, (int) good.MarkedInfo.ValidationResultKkm));
          }
          this.CheckResult(VikiPrint.Driver.AddPosition(good.Name, string.Empty, good.Quantity, good.Price, good.TaxRateNumber, good.KkmSectionNumber, good.RuFfdPaymentModeCode, good.RuFfdGoodTypeCode, good.Unit.RuFfdUnitsIndex));
          return true;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      switch (payment.Method)
      {
        case GlobalDictionaries.KkmPaymentMethods.Bonus:
        case GlobalDictionaries.KkmPaymentMethods.Certificate:
          return true;
        default:
          return VikiPrint.Driver.AddPayment(devices.CheckPrinter.FiscalKkm.PaymentsMethods.First<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (x => x.Key == payment.Method)).Value, payment.Sum);
      }
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    private void CheckResult(int resultCode)
    {
      if (resultCode != 0)
      {
        string empty = string.Empty;
        string неизвестнаяОшибка = Translate.PalycardApi_Неизвестная_ошибка;
        throw new Exception(string.Format(Translate.VikiPrint_, (object) resultCode) + неизвестнаяОшибка);
      }
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      if (!FileSystemHelper.CheckFileExistWithMsg(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\kkm\\vikiprint\\PiritLib.dll")))
        throw new InvalidOperationException(Translate.VikiPrint_Не_удалось_подключиться_к_ККМ_ВикиПринт__Не_найден_файл_драйвера_);
      if (onlyDriverLoad)
        return;
      if (dc == null)
        dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.CheckResult(VikiPrint.Driver.OpenPort(dc.CheckPrinter.Connection.ComPort.PortName, dc.CheckPrinter.Connection.ComPort.Speed));
      VikiPrint.Driver.KkmState kkmState = new VikiPrint.Driver.KkmState();
      if (!kkmState.Load())
        throw new InvalidOperationException(Translate.VikiPrint_Не_удалось_подключиться_к_ККМ_ВикиПринт);
      if (!kkmState.NeedInitialization)
        return;
      this.CheckResult(VikiPrint.Driver.KkmInitialization());
      this.CheckResult(VikiPrint.Driver.SetPrintCheck(1));
    }

    public bool Disconnect()
    {
      if (!File.Exists("dll\\kkm\\vikiprint\\PiritLib.dll"))
        return true;
      this.CheckResult(VikiPrint.Driver.ClosePort());
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      this.CheckResult(VikiPrint.Driver.SetPrintCheck(1));
      this.CheckResult(VikiPrint.Driver.OpenDocument(VikiPrint.Driver.DocTypes.Service, 1, ""));
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
        this.CheckResult(VikiPrint.Driver.PrintString(nonFiscalString.Text));
      this.CheckResult(VikiPrint.Driver.CloseDocument());
    }

    public bool PrintNonFiscalStrings(NonFiscalString nonFiscalString)
    {
      this.CheckResult(VikiPrint.Driver.SetPrintCheck(1));
      this.CheckResult(VikiPrint.Driver.PrintString(nonFiscalString.Text));
      return true;
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      this.CheckResult(VikiPrint.Driver.PrintBarcode(code, type));
      this.FeedPaper(20);
      return true;
    }

    public bool CutPaper() => true;

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      KkmStatus status = new KkmStatus();
      VikiPrint.Driver.KkmState kkmState = new VikiPrint.Driver.KkmState();
      if (!kkmState.Load())
        throw new Exception(Translate.VikiPrint_Не_удалось_получить_статус_для_ККМ_ВикиПринт);
      status.SessionStatus = !kkmState.IsSessionOpen ? SessionStatuses.Close : (kkmState.IsSessionMore24Hours ? SessionStatuses.OpenMore24Hours : SessionStatuses.Open);
      status.SessionNumber = kkmState.SessionNumber;
      status.CheckStatus = kkmState.IsCheckOpen ? CheckStatuses.Open : CheckStatuses.Close;
      status.CheckNumber = kkmState.CheckNumber;
      status.Model = kkmState.Model;
      status.SessionStarted = new DateTime?(kkmState.SessionStartDateTime);
      status.SoftwareVersion = kkmState.SoftwareVersion;
      status.FactoryNumber = kkmState.FactoryNumber;
      status.FnDateEnd = kkmState.FnEnDateTime;
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\kkm\\vikiprint\\PiritLib.dll"));
      status.DriverVersion = new Version(versionInfo.ProductVersion);
      return status;
    }

    public bool OpenCashDrawer() => true;

    bool IFiscalKkm.SendDigitalCheck(string adress) => throw new NotImplementedException();

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();

    private static class Driver
    {
      public const string PiritlibDllPath = "dll\\kkm\\vikiprint\\PiritLib.dll";

      private static DateTime GetDateTimeFromString(string str)
      {
        str = str.Replace(".", "");
        switch (str.Length)
        {
          case 6:
            int result1;
            int.TryParse(str.Substring(0, 2), out result1);
            int result2;
            int.TryParse(str.Substring(2, 2), out result2);
            int result3;
            int.TryParse(str.Substring(4, 2), out result3);
            return new DateTime(2000 + result3, result2, result1);
          case 12:
            int result4;
            int.TryParse(str.Substring(0, 2), out result4);
            int result5;
            int.TryParse(str.Substring(2, 2), out result5);
            int result6;
            int.TryParse(str.Substring(4, 2), out result6);
            int result7;
            int.TryParse(str.Substring(6, 2), out result7);
            int result8;
            int.TryParse(str.Substring(8, 2), out result8);
            int result9;
            int.TryParse(str.Substring(10, 2), out result9);
            return new DateTime(2000 + result6, result5, result4, result7, result8, result9);
          default:
            return new DateTime();
        }
      }

      private static int GetSessionNumber()
      {
        string answer;
        int result;
        return !VikiPrint.Driver.GetInfo((ushort) 1, VikiPrint.Driver.RequestType.CounterAndRegisters, out answer) || !int.TryParse(answer, out result) ? 1 : result;
      }

      private static int GetCheckNumber()
      {
        string answer;
        int result;
        return !VikiPrint.Driver.GetInfo((ushort) 2, VikiPrint.Driver.RequestType.CounterAndRegisters, out answer) || !int.TryParse(answer, out result) ? 1 : result;
      }

      private static bool GetInfo(
        ushort requestNumber,
        VikiPrint.Driver.RequestType type,
        out string answer)
      {
        VikiPrint.Driver.MData data = new VikiPrint.Driver.MData();
        if (type != VikiPrint.Driver.RequestType.CounterAndRegisters)
        {
          if (type != VikiPrint.Driver.RequestType.KkmInfo)
            throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
          VikiPrint.Driver.lib_getKktInfo(ref data, requestNumber);
        }
        else
          VikiPrint.Driver.lib_getCountersAndRegisters(ref data, requestNumber);
        if (data.errCode == 0)
        {
          answer = VikiPrint.Driver.DataToString(data);
          return true;
        }
        LogHelper.Debug("Ошибка получения данных из ККМ. Код: " + data.errCode.ToString());
        answer = string.Empty;
        return false;
      }

      private static bool GetListOfStatuses(
        out List<char> fatalStatus,
        out List<char> currentStatus,
        out List<char> documentStatus)
      {
        int fatalStatus1;
        int currentFlagStatus;
        int documentStatus1;
        int statusFlags = VikiPrint.Driver.getStatusFlags(out fatalStatus1, out currentFlagStatus, out documentStatus1);
        fatalStatus = Convert.ToString(fatalStatus1, 2).PadLeft(8, '0').ToList<char>();
        currentStatus = Convert.ToString(currentFlagStatus, 2).PadLeft(9, '0').ToList<char>();
        string str = Convert.ToString(documentStatus1, 2).PadLeft(8, '0');
        documentStatus = new List<char>()
        {
          Convert.ToChar(VikiPrint.Driver.ConvertFromBinary(str.Substring(4, 4)).ToString()),
          Convert.ToChar(VikiPrint.Driver.ConvertFromBinary(str.Substring(0, 4)).ToString())
        };
        fatalStatus.Reverse();
        currentStatus.Reverse();
        return statusFlags == 0;
      }

      private static int ConvertFromBinary(string input) => Convert.ToInt32(input, 2);

      public static int OpenPort(string portName, int portSpeed)
      {
        int num = VikiPrint.Driver.LibOpenPort(portName, portSpeed);
        LogHelper.Debug("Код результата открытия порта: " + num.ToString());
        return num;
      }

      public static int ClosePort() => VikiPrint.Driver.LibClosePort();

      public static int SetMarkedRequisite(MarkedInfo info, string tagInfo)
      {
        if (info == null || info.Type == GlobalDictionaries.RuMarkedProductionTypes.None)
          return 0;
        string nomenclatureCode = string.Empty;
        int num = 0;
        if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        {
          string hexStringAttribute = info.GetHexStringAttribute();
          char[] chArray = new char[1]
          {
            " ".ToCharArray()[0]
          };
          foreach (string str in hexStringAttribute.Split(chArray))
            nomenclatureCode = nomenclatureCode + "$" + str.ToUpper();
          LogHelper.Debug("Код маркировки: " + nomenclatureCode);
          num = VikiPrint.Driver.libSetExtraRequisite(nomenclatureCode);
        }
        else if (!tagInfo.IsNullOrEmpty())
          num = VikiPrint.Driver.libSetExtraRequisiteEx(VikiPrint.Driver.C1251To866(DataMatrixHelper.ReplaceSomeCharsToFNC1(info.FullCode)), "", "", "", "", operatorINN: "", idFOIV: "030", establishmentDocDate: "21112023", establishmentDocNum: "1944", sectoralRequisite: tagInfo);
        LogHelper.Debug("Код результата установки реквизита маркируемой продукции: " + num.ToString());
        return num;
      }

      public static int PrintString(string text)
      {
        return VikiPrint.Driver.lib_printString(VikiPrint.Driver.C1251To866(text), 1);
      }

      public static bool AddPayment(int method, Decimal sum)
      {
        return VikiPrint.Driver.lib_addPayment((byte) method, (long) (int) (sum * 100M), VikiPrint.Driver.C1251To866("")) == 0;
      }

      public static int AddPosition(
        string name,
        string barcode,
        Decimal quantity,
        Decimal price,
        int taxRateNumber,
        int sectionNumber)
      {
        int num = VikiPrint.Driver.lib_addPosition(VikiPrint.Driver.C1251To866(name), barcode, (double) quantity, (double) price, (byte) taxRateNumber, numDepart: (byte) sectionNumber);
        LogHelper.Debug("Код результата регистрации позиции std: " + num.ToString());
        return num;
      }

      public static int AddPosition(
        string name,
        string barcode,
        Decimal quantity,
        Decimal price,
        int taxRateNumber,
        int sectionNumber,
        GlobalDictionaries.RuFfdPaymentModes ffdPaymentMode,
        GlobalDictionaries.RuFfdGoodsTypes ffdGoodType,
        int qName)
      {
        string quantityName = qName.ToString("N0");
        int num = VikiPrint.Driver.lib_addPositionLarge(VikiPrint.Driver.C1251To866(name), barcode, (double) quantity, (double) price, (byte) taxRateNumber, numDepart: (byte) sectionNumber, signMethodCalculation: (int) ffdPaymentMode, signCalculationObject: (int) ffdGoodType, quantityName: quantityName);
        LogHelper.Debug("Код результата регистрации позиции ex: " + num.ToString());
        return num;
      }

      public static int AddPosition(
        string name,
        string barcode,
        Decimal quantity,
        Decimal price,
        int taxRateNumber,
        int sectionNumber,
        GlobalDictionaries.RuFfdPaymentModes ffdPaymentMode,
        GlobalDictionaries.RuFfdGoodsTypes ffdGoodType)
      {
        int num = VikiPrint.Driver.lib_addPositionEx(VikiPrint.Driver.C1251To866(name), barcode, (double) quantity, (double) price, (byte) taxRateNumber, numDepart: (byte) sectionNumber, signMethodCalculation: (int) ffdPaymentMode, signCalculationObject: (int) ffdGoodType);
        LogHelper.Debug("Код результата регистрации позиции ex: " + num.ToString());
        return num;
      }

      public static int CashInOut(Decimal sum)
      {
        return VikiPrint.Driver.lib_cashInOut("", (long) (sum * 100M));
      }

      public static int CancelDocument() => VikiPrint.Driver.LibCancelDocument();

      public static int GetCashSum(out Decimal sum)
      {
        VikiPrint.Driver.MData data = new VikiPrint.Driver.MData();
        VikiPrint.Driver.lib_getKktInfo(ref data, (ushort) 7);
        sum = 0M;
        int errCode = data.errCode;
        if (errCode != 0)
          return errCode;
        int result;
        int.TryParse(VikiPrint.Driver.DataToString(data).Replace(".", ""), out result);
        sum = (Decimal) result / 100M;
        return errCode;
      }

      public static int GetZReport(string cashierName)
      {
        return VikiPrint.Driver.lib_zReport(VikiPrint.Driver.C1251To866(cashierName));
      }

      public static int GetXReport(string cashierName)
      {
        return VikiPrint.Driver.lib_xReport(VikiPrint.Driver.C1251To866(cashierName));
      }

      private static string DataToString(VikiPrint.Driver.MData data)
      {
        try
        {
          List<byte> byteList = new List<byte>();
          for (int index = 8; index <= data.dataLength - 5; ++index)
          {
            if (data.data[index] >= (byte) 32)
              byteList.Add(data.data[index]);
            if (data.data[index] == (byte) 28)
              byteList.Add((byte) 46);
          }
          return System.Text.Encoding.GetEncoding(866).GetString(byteList.ToArray());
        }
        catch
        {
          return string.Empty;
        }
      }

      private static string C1251To866(string str1251)
      {
        byte[] bytes1 = System.Text.Encoding.GetEncoding(866).GetBytes(str1251);
        byte[] bytes2 = System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding(866), System.Text.Encoding.GetEncoding(866), bytes1);
        return System.Text.Encoding.GetEncoding(1251).GetString(bytes2);
      }

      public static int OpenDocument(
        VikiPrint.Driver.DocTypes docType,
        int section,
        string userName,
        GlobalDictionaries.RuTaxSystems taxSystem = GlobalDictionaries.RuTaxSystems.None)
      {
        return taxSystem == GlobalDictionaries.RuTaxSystems.None ? VikiPrint.Driver.Lib_openDocument((int) docType, section, VikiPrint.Driver.C1251To866(userName)) : VikiPrint.Driver.lib_openDocumentEx((int) docType, section, VikiPrint.Driver.C1251To866(userName), taxN: (int) taxSystem);
      }

      public static int CloseDocument()
      {
        VikiPrint.Driver.MData md = new VikiPrint.Driver.MData();
        VikiPrint.Driver.lib_closeDocument(ref md, 0);
        return md.errCode;
      }

      public static int MarkCodeValidation(
        out int result,
        string fullCode,
        Decimal q,
        int itemState,
        int unit,
        string qFractional = "")
      {
        VikiPrint.Driver.MData data = new VikiPrint.Driver.MData();
        result = 0;
        string quantity = q.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        VikiPrint.Driver.lib_MarkCodeValidation(ref data, VikiPrint.Driver.C1251To866(fullCode), quantity, itemState, unit);
        string str = VikiPrint.Driver.DataToString(data);
        if (!str.IsNullOrEmpty())
        {
          string[] source = str.Split('.');
          int result1;
          if (((IEnumerable<string>) source).Any<string>() && int.TryParse(source[0], out result1))
            result = result1;
        }
        return data.errCode;
      }

      public static int AcceptMarkingCode()
      {
        VikiPrint.Driver.MData data = new VikiPrint.Driver.MData();
        VikiPrint.Driver.lib_ConfirmMarkCode(ref data);
        return data.errCode;
      }

      public static int AddItemMarkingCode(
        string fullCode,
        int itemState,
        int unit,
        int validResult)
      {
        return VikiPrint.Driver.lib_AddMarkCode(fullCode, itemState, unit, validResult);
      }

      public static int KkmInitialization()
      {
        int num = VikiPrint.Driver.lib_commandStart();
        LogHelper.Debug("Код результата инициализации ККМ: " + num.ToString());
        return num;
      }

      public static int SetClientAddress(string address)
      {
        if (address.IsNullOrEmpty())
        {
          LogHelper.Debug("Адрес клиента пуст");
          return 0;
        }
        int num = VikiPrint.Driver.lib_setClientAddress(address);
        LogHelper.Debug("Код результата установки реквизита адреса клиента: " + num.ToString());
        return num;
      }

      public static bool SetClientInn(string inn)
      {
        if (inn.IsNullOrEmpty())
          return true;
        int num = VikiPrint.Driver.lib_SetBuyerInn(inn);
        LogHelper.Debug("Код результата установки реквизита ИНН клиента: " + num.ToString());
        return num == 0;
      }

      public static int PrintBarcode(string code, BarcodeTypes type)
      {
        if (code.IsNullOrEmpty())
          return 0;
        int typeBarCode;
        switch (type)
        {
          case BarcodeTypes.None:
            typeBarCode = 2;
            break;
          case BarcodeTypes.Ean13:
            typeBarCode = 2;
            break;
          case BarcodeTypes.QrCode:
            typeBarCode = 8;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
        }
        int num = VikiPrint.Driver.libPrintBarCode((byte) 0, (byte) 5, (byte) 30, (byte) typeBarCode, code);
        LogHelper.Debug("Код результата печать QR-кода: " + num.ToString());
        return num;
      }

      public static bool SetClientName(string name)
      {
        if (name.IsNullOrEmpty())
          return true;
        int num = VikiPrint.Driver.lib_SetBuyerInn(name);
        LogHelper.Debug("Код результата установки реквизита ФИО клиента: " + num.ToString());
        return num == 0;
      }

      public static int SetPrintCheck(int i)
      {
        return VikiPrint.Driver.lib_WriteSettingsTable((byte) 1, 7, string.Format("{0}", (object) i));
      }

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "openPort", CallingConvention = CallingConvention.StdCall)]
      private static extern int LibOpenPort(string port, int speed);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "scrollPaper")]
      private static extern int LibScrollPaper();

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "closePort", CallingConvention = CallingConvention.StdCall)]
      private static extern int LibClosePort();

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libSubTotal")]
      private static extern int subTotal();

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libCutDocument")]
      private static extern int lib_cutDocument();

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libCancelDocument")]
      private static extern int LibCancelDocument();

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libPrintString")]
      private static extern int lib_printString(string text, int font);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libPrintXReport")]
      private static extern int lib_xReport(string userName);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libPrintZReport")]
      private static extern int lib_zReport(string userName, int typeOfOrder = 0);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libOpenDocument", CallingConvention = CallingConvention.StdCall)]
      private static extern int Lib_openDocument(
        int typeOfDoc,
        int sectionNum,
        string userName,
        int docNum = 0);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libOpenDocumentEx", CallingConvention = CallingConvention.StdCall)]
      private static extern int lib_openDocumentEx(
        int typeOfDoc,
        int sectionNum,
        string userName,
        int docNum = 0,
        int taxN = 0);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libGetExErrorInfo")]
      private static extern int lib_getExErrorInfo(byte numRequest);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libWriteSettingsTable")]
      private static extern int lib_WriteSettingsTable(byte number, int index, string data);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libCashInOut")]
      private static extern int lib_cashInOut(string info, long sum);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "commandStart")]
      private static extern int lib_commandStart();

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libAddPosition", CallingConvention = CallingConvention.StdCall)]
      private static extern int lib_addPosition(
        string goodName,
        string barcode,
        double count,
        double price,
        byte taxNum,
        int numGoodsPos = 0,
        byte numDepart = 0,
        byte coefType = 0,
        string coefName = "",
        double coefValue = 0.0);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libAddPositionEx", CallingConvention = CallingConvention.StdCall)]
      private static extern int lib_addPositionEx(
        string goodName,
        string barcode,
        double count,
        double price,
        byte taxNum,
        int numGoodsPos = 0,
        byte numDepart = 0,
        int signMethodCalculation = 4,
        int signCalculationObject = 1);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libAddPositionLarge", CallingConvention = CallingConvention.StdCall)]
      private static extern int lib_addPositionLarge(
        string goodName,
        string barcode,
        double count,
        double price,
        byte taxNumber,
        int numGoodsPos = 0,
        byte numDepart = 0,
        byte coefType = 0,
        double coefValue = 0.0,
        int signMethodCalculation = 4,
        int signCalculationObject = 1,
        string quantityName = "");

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libAddPayment")]
      private static extern int lib_addPayment(byte typeOfPayment, long sum, string comment);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libCloseDocument")]
      private static extern int lib_closeDocument(ref VikiPrint.Driver.MData md, int cutPaper = 1);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libGetKKTInfo")]
      private static extern int lib_getKktInfo(ref VikiPrint.Driver.MData data, ushort numRequest);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libGetCountersAndRegisters")]
      private static extern int lib_getCountersAndRegisters(
        ref VikiPrint.Driver.MData data,
        ushort numRequest);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libAddDiscount")]
      private static extern int lib_addDiscount(byte type, string discountName, int sum);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll")]
      private static extern int getStatusFlags(
        out int fatalStatus,
        out int currentFlagStatus,
        out int documentStatus);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libSetBuyerAddress")]
      private static extern int lib_setClientAddress(string buyerAddress);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libSetBuyerName")]
      private static extern int lib_SetBuyerName(string buyerName);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libSetBuyerInn")]
      private static extern int lib_SetBuyerInn(string buyerInn);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libPrintRequsitOFD")]
      private static extern int lib_PrintRequsitOFD(
        int codeReq,
        byte attributeText,
        string reqName,
        string reqSt);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll")]
      private static extern int libSetExtraRequisite(
        string nomenclatureCode,
        string extReq = " ",
        string measureName = " ",
        int agentSign = 0,
        string supplierINN = "000000000000",
        string supplierPhone = "",
        string supplierName = "",
        string operatorAddress = "",
        string operatorINN = "000000000000",
        string operatorName = "",
        string operatorPhone = "",
        string payAgentOperation = "",
        string payAgentPhone = "",
        string recOperatorPhone = "");

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll")]
      private static extern int libSetExtraRequisiteEx(
        string nomenclatureCode,
        string extReq = " ",
        string measureName = " ",
        string agentSign = " ",
        string supplierINN = "000000000000",
        string supplierPhone = "",
        string supplierName = "",
        string operatorAddress = "",
        string operatorINN = "000000000000",
        string operatorName = "",
        string operatorPhone = "",
        string payAgentOperation = "",
        string payAgentPhone = "",
        string recOperatorPhone = "",
        string idFOIV = "",
        string establishmentDocDate = "",
        string establishmentDocNum = "",
        string sectoralRequisite = "");

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libMarkCodeValidation")]
      private static extern int lib_MarkCodeValidation(
        ref VikiPrint.Driver.MData data,
        string markCode,
        string quantity = "",
        int itemState = 0,
        int quantityMode = 0,
        int workMode = 1);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libConfirmMarkCode")]
      private static extern int lib_ConfirmMarkCode(ref VikiPrint.Driver.MData data, int mode = 1);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libAddMarkCode")]
      private static extern int lib_AddMarkCode(
        string markCode = "",
        int itemState = 0,
        int quantityMode = 0,
        int validationResult = 0);

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll", EntryPoint = "libSetExtraRequisite")]
      private static extern int libSetExtraRequisite2(
        string nomenclatureCode,
        string extReq = "",
        string measureName = "",
        int agentSign = 0,
        string supplierINN = "",
        string supplierPhone = "",
        string supplierName = "",
        string operatorAddress = "",
        string operatorINN = "",
        string operatorName = "",
        string operatorPhone = "",
        string payAgentOperation = "",
        string payAgentPhone = "",
        string recOperatorPhone = "");

      [DllImport("dll\\kkm\\vikiprint\\PiritLib.dll")]
      private static extern int libPrintBarCode(
        byte posText,
        byte widthBarCode,
        byte heightBarCode,
        byte typeBarCode,
        string barCode = "");

      public enum DocTypes
      {
        Service = 1,
        SaleCheck = 2,
        ReturnCheck = 3,
        CashIncome = 4,
        CashOutcome = 5,
      }

      public class KkmState
      {
        public bool NeedInitialization { get; private set; }

        public bool IsSessionOpen { get; private set; }

        public bool IsSessionMore24Hours { get; private set; }

        public bool IsCheckOpen { get; private set; }

        public int SessionNumber { get; private set; }

        public int CheckNumber { get; private set; }

        public string FactoryNumber { get; private set; }

        public string SoftwareVersion { get; private set; }

        public string Model { get; private set; }

        public DateTime SessionStartDateTime { get; private set; }

        public DateTime FnEnDateTime { get; private set; }

        public bool Load()
        {
          List<char> currentStatus;
          List<char> documentStatus;
          if (!VikiPrint.Driver.GetListOfStatuses(out List<char> _, out currentStatus, out documentStatus))
          {
            LogHelper.Debug("Не удалось получить статусы из ККМ");
            return false;
          }
          this.NeedInitialization = currentStatus[0] == '1';
          this.IsSessionOpen = currentStatus[2] == '1';
          this.IsSessionMore24Hours = currentStatus[3] == '1';
          this.IsCheckOpen = documentStatus[0] != '0';
          this.SessionNumber = VikiPrint.Driver.GetSessionNumber();
          this.CheckNumber = VikiPrint.Driver.GetCheckNumber();
          string answer1;
          if (VikiPrint.Driver.GetInfo((ushort) 1, VikiPrint.Driver.RequestType.KkmInfo, out answer1))
            this.FactoryNumber = answer1;
          string answer2;
          if (VikiPrint.Driver.GetInfo((ushort) 2, VikiPrint.Driver.RequestType.KkmInfo, out answer2))
            this.SoftwareVersion = answer2;
          string answer3;
          if (VikiPrint.Driver.GetInfo((ushort) 21, VikiPrint.Driver.RequestType.KkmInfo, out answer3))
            this.Model = answer3;
          string answer4;
          if (VikiPrint.Driver.GetInfo((ushort) 17, VikiPrint.Driver.RequestType.KkmInfo, out answer4))
            this.SessionStartDateTime = VikiPrint.Driver.GetDateTimeFromString(answer4);
          string answer5;
          if (VikiPrint.Driver.GetInfo((ushort) 14, VikiPrint.Driver.RequestType.KkmInfo, out answer5))
            this.FnEnDateTime = VikiPrint.Driver.GetDateTimeFromString(answer5);
          return true;
        }
      }

      private enum RequestType
      {
        CounterAndRegisters,
        KkmInfo,
      }

      private struct MData
      {
        public readonly int errCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public readonly byte[] data;
        public readonly int dataLength;
      }
    }
  }
}
