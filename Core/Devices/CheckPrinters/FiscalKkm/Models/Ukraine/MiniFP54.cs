// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.MiniFP54
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine
{
  public class MiniFP54 : IFiscalKkm, IDevice
  {
    private int _pass = 12321;

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Kkm;

    public GlobalDictionaries.Devices.FfdVersions GetFfdVersion()
    {
      throw new NotImplementedException();
    }

    public bool IsCanHoldConnection => true;

    public string Name => nameof (MiniFP54);

    private object Driver { get; set; }

    public KkmLastActionResult LasActionResult { get; } = new KkmLastActionResult()
    {
      ActionResult = ActionsResults.Done
    };

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.Connection, ConnectionSettingsViewModel.PortsConfig.ComOrLan, true);
    }

    public void OpenSession(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      string command1 = "write_table;3;1;" + cashier.Name + ";";
      this.DoCommand(ref command1);
      if (this.GetCodeError(command1) != 0)
        throw new InvalidOperationException(Translate.MiniFP54_Не_удалось_открыть_смену_на_ККМ_MiniPF54);
      string command2 = string.Format("write_table;2;1;{0};", (object) this._pass);
      this.DoCommand(ref command2);
      if (this.GetCodeError(command2) != 0)
        throw new InvalidOperationException(Translate.MiniFP54_Не_удалось_открыть_смену_на_ККМ_MiniPF54);
      string command3 = string.Format("cashier_registration;1;{0};", (object) this._pass);
      this.DoCommand(ref command3);
      if (this.GetCodeError(command3) != 0)
        throw new InvalidOperationException(Translate.MiniFP54_Не_удалось_открыть_смену_на_ККМ_MiniPF54);
    }

    public void GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      string command;
      switch (reportType)
      {
        case ReportTypes.ZReport:
          command = string.Format("execute_Z_report;{0};", (object) this._pass);
          break;
        case ReportTypes.XReport:
          command = string.Format("execute_X_report;{0};", (object) this._pass);
          break;
        case ReportTypes.XReportWithGoods:
          throw new NotSupportedException();
        default:
          throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
      }
      this.DoCommand(ref command);
      if (this.GetCodeError(command) != 0)
        throw new Exception(Translate.MiniFP54_GetReport_Ошибка_при_снятии_отчета__ККМ_MiniFP54__ + this.GetTextError());
    }

    public bool OpenCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData checkData)
    {
      string command = string.Empty;
      switch (checkData.CheckType)
      {
        case CheckTypes.Sale:
          command = "open_receipt;0;";
          break;
        case CheckTypes.ReturnSale:
          command = "open_receipt;1;";
          break;
      }
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
        return true;
      LogHelper.Debug("Ошибка при открытии чека с ККМ MiniFP54: " + this.GetTextError());
      return false;
    }

    public bool CloseCheck() => true;

    public void CancelCheck()
    {
      string command = "cancel_receipt;";
      this.DoCommand(ref command);
      if (this.GetCodeError(command) != 0)
        throw new Exception(Translate.MiniFP54_Не_удалось_отменить_чек_в_ККМ_MiniFP54 + this.GetTextError());
      LogHelper.Debug("Ошибок нет, чек отменен");
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      string command = "in_out;0;0;0;1;" + sum.ToString("0.00;(0.00)", (IFormatProvider) CultureInfo.InvariantCulture) + ";;;";
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
      {
        LogHelper.Debug("Ошибок нет, снятие выполнено");
        return true;
      }
      LogHelper.Debug("Не удалось снять наличные в ККМ MiniFP54" + this.GetTextError());
      return false;
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      string command = "in_out;0;0;0;0;" + sum.ToString("0.00;(0.00)", (IFormatProvider) CultureInfo.InvariantCulture) + ";;;";
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
      {
        LogHelper.Debug("Ошибок нет, внесение выполнено");
        return true;
      }
      LogHelper.Debug("Не удалось внести наличные в ККМ MiniFP54" + this.GetTextError());
      return false;
    }

    public bool WriteOfdAttribute(OfdAttributes ofdAttribute, object value) => true;

    public bool GetCashSum(out Decimal sum)
    {
      string command = "get_cashbox_sum;";
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
      {
        LogHelper.Debug("Ошибок нет, сумма в кассе получена");
        string str = command.Split(';')[1];
        LogHelper.Debug("Сумма: " + str.ToDecimal().ToString());
        sum = str.ToDecimal();
        return true;
      }
      sum = 0M;
      LogHelper.Debug("Не удалось получить сумму в кассе в ККМ MiniFP54" + this.GetTextError());
      return false;
    }

    public bool RegisterGood(CheckGood good, CheckTypes checkType)
    {
      int int32 = Convert.ToInt32(good.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value ?? (object) 1);
      int num1 = good.TaxRateNumber - 1;
      int num2 = (Decimal) (int) good.Quantity != good.Quantity ? 1 : 0;
      string str1 = good.Quantity.ToString("0.000;(0.000)", (IFormatProvider) CultureInfo.InvariantCulture);
      string str2 = "0";
      if (BarcodeHelper.IsEan13Barcode(good.Barcode) || BarcodeHelper.IsEan8Barcode(good.Barcode))
        str2 = good.Barcode;
      string command1 = string.Format("add_plu;{0};{1};{2};0;0;1;{3};{4};{5};{6};{7};", (object) int32, (object) num1, (object) num2, (object) good.KkmSectionNumber, (object) good.Price.ToString("0.00;(0.00)", (IFormatProvider) CultureInfo.InvariantCulture), (object) str2, good.Good.Name.Length > 48 ? (object) good.Good.Name.Remove(48) : (object) good.Good.Name, (object) str1);
      LogHelper.Debug(command1);
      this.DoCommand(ref command1);
      if (this.GetCodeError(command1) != 0)
      {
        LogHelper.Debug("Не удалось зарегестрирвоать товар в в ККМ MiniFP54");
        return false;
      }
      string command2 = string.Format("sale_plu;0;0;0;{0};{1};", (object) str1, (object) int32);
      this.DoCommand(ref command2);
      if (this.GetCodeError(command2) != 0)
      {
        LogHelper.Debug("Не удалось продать товар в в ККМ MiniFP54");
        return false;
      }
      if (!good.Description.IsNullOrEmpty())
      {
        string command3 = "print_attrib_excise_stamp;" + good.Description + ";";
        this.DoCommand(ref command3);
        if (this.GetCodeError(command3) != 0)
        {
          LogHelper.Debug("Не удалось зарег. акциз в ККМ MiniFP54 " + this.GetTextError());
          return false;
        }
      }
      string command4 = "discount_surcharge;0;0;1;" + good.DiscountSum.ToString("0.00;(0.00)", (IFormatProvider) CultureInfo.InvariantCulture) + ";";
      this.DoCommand(ref command4);
      if (this.GetCodeError(command4) == 0)
        return true;
      LogHelper.Debug("Не удалось зарег. скидку в ККМ MiniFP54 " + this.GetTextError());
      return false;
    }

    public bool RegisterPayment(CheckPayment payment)
    {
      string command = string.Format("pay;{0};{1};", (object) new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.PaymentsMethods.First<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>>((Func<KeyValuePair<GlobalDictionaries.KkmPaymentMethods, int>, bool>) (x => x.Key == payment.Method)).Value, (object) payment.Sum.ToString("0.00;(0.00)", (IFormatProvider) CultureInfo.InvariantCulture));
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
      {
        LogHelper.Debug("Ошибок нет, платеж зарегестирован");
        return true;
      }
      LogHelper.Debug("Не удалось зарег. платеж в ККМ MiniFP54" + this.GetTextError());
      return false;
    }

    public bool RegisterCheckDiscount(Decimal sum, string description)
    {
      string command = "discount_surcharge;0;1;1;" + sum.ToString("0.00;(0.00)", (IFormatProvider) CultureInfo.InvariantCulture) + ";";
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
        return true;
      LogHelper.Debug("Не удалось зарег. скидку на чек в ККМ MiniFP54 " + this.GetTextError());
      return false;
    }

    public bool GetCheckRemainder(out Decimal remainder)
    {
      remainder = 0M;
      return true;
    }

    public void Connect(bool onlyDriverLoad = false, Gbs.Core.Config.Devices dc = null)
    {
      LogHelper.Debug("Подключение MiniFP54");
      this.Driver = Functions.CreateObject("ecrmini.t400");
      // ISSUE: reference to a compiler-generated field
      if (MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (MiniFP54), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target = MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p1 = MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (MiniFP54), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__0.Target((CallSite) MiniFP54.\u003C\u003Eo__28.\u003C\u003Ep__0, this.Driver, (object) null);
      if (target((CallSite) p1, obj))
        throw new NullReferenceException(Translate.MiniFP54_Объект_драйвера_ККМ_MiniFP54_не_был_создан);
      if (onlyDriverLoad)
        return;
      if (dc == null)
        dc = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      int result;
      this._pass = int.TryParse(dc.CheckPrinter.Connection.LanPort.Password, out result) ? result : this._pass;
      string command;
      switch (dc.CheckPrinter.Connection.ConnectionType)
      {
        case GlobalDictionaries.Devices.ConnectionTypes.NotSet:
          throw new InvalidDataException(Translate.Maria_Не_указан_тип_соединения);
        case GlobalDictionaries.Devices.ConnectionTypes.ComPort:
          LogHelper.Debug("Подключение по ком-порту");
          command = string.Format("open_port;{0};{1};", (object) dc.CheckPrinter.Connection.ComPort.PortNumber, (object) dc.CheckPrinter.Connection.ComPort.Speed);
          break;
        case GlobalDictionaries.Devices.ConnectionTypes.Lan:
          LogHelper.Debug("Подключение по LAN");
          string str = dc.CheckPrinter.Connection.LanPort.UrlAddress;
          if (str.ToLower().StartsWith("http://"))
            str = str.Replace("http://", "");
          command = string.Format("connect_tcp;{0};{1};{2};", (object) str, (object) dc.CheckPrinter.Connection.LanPort.PortNumber.GetValueOrDefault(), (object) this._pass);
          break;
        default:
          throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_подключиться_к_ККМ);
      }
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
        LogHelper.Debug("Ошибок нет, подключились");
      else
        LogHelper.Debug("Не удалось подключиться к ККМ MiniFP54" + this.GetTextError());
    }

    public bool Disconnect()
    {
      string command = "close_port;";
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
      {
        LogHelper.Debug("Ошибок нет, отключились");
        return true;
      }
      LogHelper.Debug("Не удалось отключиться от ККМ MiniFP54" + this.GetTextError());
      return false;
    }

    public bool IsConnected { get; set; }

    public void PrintNonFiscalStrings(List<NonFiscalString> nonFiscalStrings)
    {
      LogHelper.Debug("Печатаем нефискальные строки МИНИ ФП");
      foreach (NonFiscalString nonFiscalString in nonFiscalStrings)
      {
        string command = "comment;";
        command += nonFiscalString.WideFont ? "1;" : "0;";
        command += "0;0;0;0;0;";
        command = nonFiscalString.Text.Length <= 240 ? command + nonFiscalString.Text + ";" : command + nonFiscalString.Text.Remove(240) + ";";
        this.DoCommand(ref command);
        if (this.GetCodeError(command) != 0)
          throw new KkmException((IDevice) this, string.Format(Translate.MiniFP54_, (object) this.GetTextError()));
      }
    }

    public bool PrintBarcode(string code, BarcodeTypes type) => true;

    public bool CutPaper()
    {
      string command = "cut_paper;";
      this.DoCommand(ref command);
      if (this.GetCodeError(command) == 0)
      {
        LogHelper.Debug("Ошибок нет, отрезали чек");
        return true;
      }
      LogHelper.Debug("Не удалось отрезать чек ККМ MiniFP54" + this.GetTextError());
      return false;
    }

    public KkmStatus GetShortStatus() => this.GetStatus();

    public KkmStatus GetStatus()
    {
      string command = "get_status;1;";
      this.DoCommand(ref command);
      string[] strArray = this.GetCodeError(command) == 0 ? command.Split(';') : throw new Exception(Translate.MiniFP54_Не_удалось_получить_статус__ККМ_MiniFP54 + this.GetTextError());
      KkmStatus status = new KkmStatus()
      {
        SessionStatus = strArray[2] == "0" ? SessionStatuses.Close : (strArray[10] == "0" ? SessionStatuses.Open : SessionStatuses.OpenMore24Hours),
        CheckStatus = strArray[3] == "0" ? CheckStatuses.Close : CheckStatuses.Open,
        SessionNumber = Convert.ToInt32(strArray[14]),
        CheckNumber = Convert.ToInt32(strArray[15]),
        KkmState = KkmStatuses.Ready
      };
      DateTime result;
      if (DateTime.TryParseExact(strArray[11] + " " + strArray[12], "dd.MM.yyyy HH:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        status.SessionStarted = new DateTime?(result);
      LogHelper.Debug("Ошибок нет, статус ККМ MiniFP54 получен");
      return status;
    }

    private void DoCommand(ref string command)
    {
      LogHelper.Debug("Command to KKM: " + command);
      // ISSUE: reference to a compiler-generated field
      if (MiniFP54.\u003C\u003Eo__39.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MiniFP54.\u003C\u003Eo__39.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "t400me", (IEnumerable<System.Type>) null, typeof (MiniFP54), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsRef, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      MiniFP54.\u003C\u003Eo__39.\u003C\u003Ep__0.Target((CallSite) MiniFP54.\u003C\u003Eo__39.\u003C\u003Ep__0, this.Driver, ref command);
    }

    public bool OpenCashDrawer() => true;

    public bool SendDigitalCheck(string adress) => true;

    public void FeedPaper(int lines)
    {
      string command = string.Format("paper_feed;{0};", (object) lines);
      this.DoCommand(ref command);
    }

    public bool EndPrintOldCheck() => throw new NotImplementedException();

    private int GetCodeError(string text)
    {
      LogHelper.Debug("Answer: " + text);
      if (text.IsNullOrEmpty())
        return 0;
      string str;
      if (text == null)
        str = (string) null;
      else
        str = text.Split(';')[0];
      return Convert.ToInt32(str);
    }

    private string GetTextError()
    {
      string empty = string.Empty;
      // ISSUE: reference to a compiler-generated field
      if (MiniFP54.\u003C\u003Eo__45.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MiniFP54.\u003C\u003Eo__45.\u003C\u003Ep__0 = CallSite<\u003C\u003EA\u007B00000040\u007D<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "t400me", (IEnumerable<System.Type>) null, typeof (MiniFP54), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsRef, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      MiniFP54.\u003C\u003Eo__45.\u003C\u003Ep__0.Target((CallSite) MiniFP54.\u003C\u003Eo__45.\u003C\u003Ep__0, this.Driver, ref empty);
      return empty;
    }
  }
}
