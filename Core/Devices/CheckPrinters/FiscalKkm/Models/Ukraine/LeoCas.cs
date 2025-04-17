// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.LeoCas
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine
{
  public class LeoCas : IFiscalKkm, IDevice
  {
    private IntPtr _connection;
    private const string PathToDriver = "\\dll\\kkm\\leocas\\DriverEKKA3.dll";

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "Connect")]
    private static extern IntPtr lib_connect(uint connType, string connPath, uint timeOut);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "Disconnect")]
    private static extern int lib_disconnect(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "InOut")]
    private static extern int lib_inOut(IntPtr conn, uint Flg, int Sum);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "GetStatus")]
    private static extern int lib_getStatus(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "GetStatusEx")]
    private static extern int lib_getStatusEx(IntPtr conn, ref ushort state);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "GetErrorDesc")]
    private static extern IntPtr lib_getErrorDescription(int errorCode);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "RegUser")]
    private static extern int lib_regUser(IntPtr conn, int userId, int pass);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "BegChk")]
    private static extern int lib_begChk(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "EndChk")]
    private static extern int lib_endChk(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "SmenBegin")]
    private static extern int lib_smenBegin(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "PrintRep")]
    private static extern int lib_printRep(IntPtr conn, int repId);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "NProd")]
    private static extern int lib_nProd(
      IntPtr conn,
      ulong codeGood,
      uint q,
      uint price,
      sbyte group,
      sbyte tax,
      string unit,
      string name);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "NProdUktZed")]
    private static extern int lib_nProdUktZed(
      IntPtr conn,
      ulong code,
      ulong uCode,
      sbyte uLen,
      uint q,
      uint price,
      sbyte group,
      sbyte tax,
      string unit,
      string name);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "BegRet")]
    private static extern int lib_begRet(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "VoidChk")]
    private static extern int lib_voidChk(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "Oplata")]
    private static extern int lib_oplata(IntPtr conn, int index, int sum);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "BARCode")]
    private static extern int lib_barcode(
      IntPtr conn,
      uint type,
      uint hight,
      uint isPrintNum,
      string str);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "BegDoc")]
    private static extern int lib_begDoc(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "TextDoc")]
    private static extern int lib_textDoc(IntPtr conn, uint param, string text);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "End_Doc")]
    private static extern int lib_endDoc(IntPtr conn);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "TextChk")]
    private static extern int lib_textChk(IntPtr conn, string text);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "Discount")]
    private static extern int lib_discount(IntPtr conn, sbyte flg, uint val, ulong code);

    [DllImport("\\dll\\kkm\\leocas\\DriverEKKA3.dll", EntryPoint = "AddInfo")]
    private static extern int lib_addInfo(IntPtr conn, sbyte type, string str);

    private void CheckResult(int resultCode)
    {
      LogHelper.Debug("resultCode: " + resultCode.ToString());
      if (resultCode != 0)
      {
        if (resultCode == 55 || resultCode == 56)
          this.CancelCheck();
        if (resultCode == 50)
        {
          LogHelper.Debug("resultCode=50,");
          int num = (int) MessageBoxHelper.Show("На РРО є не роздрукований чек. Помилка принтера: замініть папір та закрийте кришку.");
          LogHelper.Debug("Пытаемся еще раз закрыть чек");
          resultCode = LeoCas.lib_endChk(this._connection);
          if (resultCode == 0)
            return;
        }
        string stringAnsi = Marshal.PtrToStringAnsi(LeoCas.lib_getErrorDescription(resultCode));
        throw new KkmException((IDevice) this, string.Format(Translate.Код___0____Описание___1_, (object) resultCode, (object) stringAnsi), resultCode == 55 ? KkmException.ErrorTypes.NonCorrectData : (resultCode == 56 ? KkmException.ErrorTypes.NeedService : KkmException.ErrorTypes.Unknown));
      }
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public string Name => Translate.LeoCas_Name_ЛеоКАС;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    public KkmLastActionResult LasActionResult => new KkmLastActionResult();

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection, ConnectionSettingsViewModel.PortsConfig.ComOrLan);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CheckResult(LeoCas.lib_smenBegin(this._connection));
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      int resultCode;
      switch (reportType)
      {
        case ReportTypes.ZReport:
          resultCode = LeoCas.lib_printRep(this._connection, 1);
          break;
        case ReportTypes.XReport:
          resultCode = LeoCas.lib_printRep(this._connection, 0);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
      this.CheckResult(resultCode);
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      this.CheckResult(LeoCas.lib_begChk(this._connection));
      if (checkData.CheckType == CheckTypes.ReturnSale)
        this.CheckResult(LeoCas.lib_begRet(this._connection));
      return true;
    }

    public bool CloseCheck()
    {
      this.CheckResult(LeoCas.lib_endChk(this._connection));
      return true;
    }

    public void CancelCheck() => this.CheckResult(LeoCas.lib_voidChk(this._connection));

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CheckResult(LeoCas.lib_begChk(this._connection));
      this.CheckResult(LeoCas.lib_inOut(this._connection, 1U, (int) (sum * 100M)));
      this.CheckResult(LeoCas.lib_endChk(this._connection));
      return true;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      this.CheckResult(LeoCas.lib_begChk(this._connection));
      this.CheckResult(LeoCas.lib_inOut(this._connection, 0U, (int) (sum * 100M)));
      this.CheckResult(LeoCas.lib_endChk(this._connection));
      return true;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      sum = 0M;
      return true;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      Guid uktZedUid = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().GoodsConfig.UktZedUid;
      string str1 = KkmHelper.RemoveSpaceAndEnter(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == uktZedUid))?.Value.ToString() ?? "");
      string str2 = ((IEnumerable<char>) str1.ToCharArray()).All<char>(new Func<char, bool>(char.IsDigit)) ? str1 : "";
      ulong uint64 = Convert.ToUInt64(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value ?? (object) 1);
      sbyte taxRateNumber = (sbyte) good.TaxRateNumber;
      if (good.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol)
      {
        int resultCode = LeoCas.lib_addInfo(this._connection, (sbyte) 2, good.Description);
        LogHelper.Debug(string.Format("Результат записи кода акциза: {0}", (object) resultCode));
        this.CheckResult(resultCode);
      }
      if (str2.IsNullOrEmpty())
      {
        this.CheckResult(LeoCas.lib_nProd(this._connection, uint64, (uint) (good.Quantity * 1000M), (uint) (good.Price * (100M - good.Discount)), (sbyte) 0, taxRateNumber, "   ", good.Name));
      }
      else
      {
        int index = ((IEnumerable<char>) str2.ToCharArray()).ToList<char>().FindIndex((Predicate<char>) (x => x != '0'));
        ulong uCode = ulong.Parse(str2.Remove(0, index));
        LogHelper.Debug("Регистрируем позицию с UKT " + uCode.ToString() + " , первоначальный код: " + str2);
        this.CheckResult(LeoCas.lib_nProdUktZed(this._connection, uint64, uCode, (sbyte) str2.Length, (uint) (good.Quantity * 1000M), (uint) (good.Price * (100M - good.Discount)), (sbyte) 0, taxRateNumber, "   ", good.Name));
      }
      return true;
    }

    public static sbyte SetBit(sbyte val, int num, bool bit)
    {
      if (num > 3 || num < 0)
        throw new ArgumentException();
      byte num1 = (byte) (1U << num);
      val &= (sbyte) ~num1;
      if (bit)
        val |= (sbyte) num1;
      return val;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      int index = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.PaymentsMethods.First<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (x => x.Key == payment.Method)).Value;
      Decimal sum = (Decimal) (int) (Math.Round(payment.Sum, 1, MidpointRounding.AwayFromZero) * 100M);
      int resultCode = LeoCas.lib_oplata(this._connection, index, (int) sum);
      LogHelper.Debug(string.Format("lib_oplata: ind: {0}; sum: {1}; result: {2}", (object) index, (object) sum, (object) resultCode));
      this.CheckResult(resultCode);
      return true;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description) => true;

    [HandleProcessCorruptedStateExceptions]
    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      try
      {
        if (onlyDriverLoad)
          return;
        if (dc == null)
          dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        DeviceConnection connection = dc.CheckPrinter.Connection;
        LogHelper.Debug(connection.ToJsonString(true));
        switch (connection.ConnectionType)
        {
          case GlobalDictionaries.Devices.ConnectionTypes.NotSet:
            throw new KkmException((IDevice) this, Translate.LeoCas_Не_указан_способ_подключения, KkmException.ErrorTypes.NoConnection);
          case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
            DeviceHelper.CheckComPortExists(connection.ComPort.PortName, (IDevice) this);
            this._connection = LeoCas.lib_connect(0U, "\\\\.\\" + connection.ComPort.PortName + " baud=38400 parity=N data=8 stop=1", 3000U);
            LogHelper.Debug("connection result: " + this._connection.ToString());
            break;
          case GlobalDictionaries.Devices.ConnectionTypes.Lan:
            this._connection = LeoCas.lib_connect(1U, connection.LanPort.UrlAddress + ":" + connection.LanPort.PortNumber.ToString(), 3000U);
            LogHelper.Debug("connection result: " + this._connection.ToString());
            break;
          default:
            throw new KkmException((IDevice) this, Translate.LeoCas_Не_указан_способ_подключения, KkmException.ErrorTypes.NoConnection);
        }
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          Gbs.Core.Entities.Users.User byUid = new UsersRepository(dataBase).GetByUid(KkmHelper.UserUid);
          int result1;
          int result2;
          this.CheckResult(byUid == null || !int.TryParse(byUid.LoginForKkm, out result1) || !int.TryParse(byUid.PasswordForKkm, out result2) ? LeoCas.lib_regUser(this._connection, 1, 1) : LeoCas.lib_regUser(this._connection, result1, result2));
        }
      }
      catch (Exception ex)
      {
        throw new KkmException((IDevice) this, Translate.LeoCas_Не_удалось_подключить_к_кассе__проверьте_соединение + "\n\n" + ex.Message, KkmException.ErrorTypes.NoConnection);
      }
    }

    public bool Disconnect()
    {
      if (this._connection == IntPtr.Zero)
        return true;
      KkmHelper.UserUid = Guid.Empty;
      this.CheckResult(LeoCas.lib_disconnect(this._connection));
      return true;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      this.CheckResult(LeoCas.lib_begDoc(this._connection));
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
        this.CheckResult(LeoCas.lib_textDoc(this._connection, 0U, nonFiscalString.Text));
      this.CheckResult(LeoCas.lib_endDoc(this._connection));
    }

    public bool PrintBarcode(string code, BarcodeTypes type)
    {
      uint type1;
      switch (type)
      {
        case BarcodeTypes.None:
          type1 = 1U;
          break;
        case BarcodeTypes.Ean13:
          type1 = 0U;
          break;
        case BarcodeTypes.QrCode:
          throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
        default:
          throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
      }
      this.CheckResult(LeoCas.lib_barcode(this._connection, type1, 10U, 1U, code));
      return true;
    }

    public bool CutPaper() => true;

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      ushort state = 0;
      int statusEx = LeoCas.lib_getStatusEx(this._connection, ref state);
      LogHelper.Debug("StatuEx " + state.ToString() + " and " + statusEx.ToString());
      int status = LeoCas.lib_getStatus(this._connection);
      LogHelper.Debug("Status " + status.ToString());
      KkmStatuses kkmStatuses = KkmStatuses.Ready;
      if (status.GetBit(6))
        kkmStatuses = KkmStatuses.NeedToContinuePrint;
      if (status.GetBit(13))
        kkmStatuses = KkmStatuses.NoPaper;
      if (status.GetBit(14))
        kkmStatuses = KkmStatuses.CoverOpen;
      if (status.GetBit(15) || status.GetBit(0))
        kkmStatuses = KkmStatuses.HardwareError;
      return new KkmStatus()
      {
        SessionStatus = status.GetBit(1) ? SessionStatuses.Open : SessionStatuses.Close,
        CheckStatus = status.GetBit(3) || status.GetBit(4) ? CheckStatuses.Open : CheckStatuses.Close,
        KkmState = kkmStatuses
      };
    }

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
    }

    public bool EndPrintOldCheck()
    {
      this.CheckResult(LeoCas.lib_endChk(this._connection));
      return true;
    }
  }
}
