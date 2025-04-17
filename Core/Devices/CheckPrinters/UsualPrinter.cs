// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.UsualPrinter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters
{
  public class UsualPrinter : ICheckPrinterDevice, IDisposable
  {
    private readonly Gbs.Core.Config.Devices _devicesConfig;

    public UsualPrinter(Gbs.Core.Config.Devices devices) => this._devicesConfig = devices;

    public void PrintPrepareCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data, IPrintableReport prepareReport = null)
    {
      if (this._devicesConfig.CheckPrinter.CheckTemplate.IsNullOrEmpty())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.UsualPrinter_Укажите_шаблон_в_настройках_программы);
      }
      else if (!File.Exists(this._devicesConfig.CheckPrinter.CheckTemplate))
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.UsualPrinter_Указанный_шаблон_в_настройках_не_существует_);
      }
      else if (this._devicesConfig.CheckPrinter.PrinterSetting.PrinterName.IsNullOrEmpty() && this._devicesConfig.CheckPrinter.PrinterSetting.IsSendInPrinter)
      {
        int num3 = (int) MessageBoxHelper.Show(Translate.UsualPrinter_Укажите_принтер_для_печати_в_настройках_программы);
      }
      else
      {
        PrintableReportFactory printableReportFactory = new PrintableReportFactory();
        if (prepareReport == null)
        {
          if (this._devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.HiPos && this._devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && data.FiscalType == CheckFiscalTypes.NonFiscal)
          {
            HiPosDriver.GetCheckCommand checkData = (HiPosDriver.GetCheckCommand) null;
            string str = "";
            try
            {
              if (!data.FiscalNum.IsNullOrEmpty())
              {
                KkmHelper.UserUid = data.Cashier.UserUid;
                HiPos hiPos = new HiPos();
                hiPos.Connect(false, this._devicesConfig);
                str = hiPos.GetStatus().FactoryNumber;
                checkData = hiPos.GetCheck(data.FiscalNum);
              }
            }
            catch (Exception ex)
            {
              checkData = (HiPosDriver.GetCheckCommand) null;
              LogHelper.WriteError(ex, "Не удалось получить данные для печати нефискального чека ПРРО");
            }
            finally
            {
              if (checkData == null || checkData.Result == null)
              {
                HiPosDriver.GetCheckCommand.CheckAnswer checkAnswer = new HiPosDriver.GetCheckCommand.CheckAnswer()
                {
                  Items = data.GoodsList.Select<CheckGood, HiPosDriver.GetCheckCommand.Item>((Func<CheckGood, HiPosDriver.GetCheckCommand.Item>) (x => new HiPosDriver.GetCheckCommand.Item()
                  {
                    Name = x.Name,
                    Amount = x.Quantity,
                    Barcode = x.Barcode,
                    Cost = x.Sum,
                    Discount = x.Discount,
                    Uktzed = x.Good.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.UktZedUid))?.Value.ToString() ?? "",
                    Price = x.Price,
                    ExciseLables = new List<string>()
                    {
                      x.Description ?? string.Empty
                    }
                  })).ToList<HiPosDriver.GetCheckCommand.Item>(),
                  Pays = data.PaymentsList.Select<CheckPayment, HiPosDriver.GetCheckCommand.Pay>((Func<CheckPayment, HiPosDriver.GetCheckCommand.Pay>) (x => new HiPosDriver.GetCheckCommand.Pay()
                  {
                    Sum = x.Sum,
                    PaymentName = x.Name
                  })).ToList<HiPosDriver.GetCheckCommand.Pay>(),
                  Excises = new List<HiPosDriver.GetCheckCommand.Excise>(),
                  Taxes = new List<HiPosDriver.GetCheckCommand.Taxis>(),
                  OrderDate = data.DateTime,
                  Cashier = data.Cashier.Name,
                  DocType = (int) data.CheckType,
                  TotalSum = new Decimal?(data.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)))
                };
                HiPosDriver.GetCheckCommand getCheckCommand = new HiPosDriver.GetCheckCommand();
                getCheckCommand.AnswerString = checkAnswer.ToJsonString();
                checkData = getCheckCommand;
              }
              data.CustomData.Add("PrroFiscalNumber", (object) str);
              data.CustomData.Add("CheckType", (object) (int) data.CheckType);
              prepareReport = printableReportFactory.CreateForHiPosFiscalCheck(checkData, data.CustomData);
            }
          }
          if (this._devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.ReKassa && this._devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && data.FiscalType == CheckFiscalTypes.NonFiscal)
          {
            data.CustomData.Add("QRCodeUrl", (object) string.Empty);
            data.CustomData.Add("CheckNumber", (object) string.Empty);
            data.CustomData.Add("ShiftNumber", (object) 1);
            data.CustomData.Add("SerialNumber", (object) this._devicesConfig.CheckPrinter.FiscalKkm.Model);
            data.CustomData.Add("RegistrationNumber", (object) string.Empty);
            data.CustomData.Add("KkmName", (object) "");
            data.CustomData.Add("OfdName", (object) string.Empty);
            data.CustomData.Add("OfdHost", (object) string.Empty);
          }
          if (this._devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.UzPos && this._devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && data.FiscalType == CheckFiscalTypes.NonFiscal)
          {
            data.CustomData.Add("QRCodeUrl", (object) string.Empty);
            data.CustomData.Add("FiscalSign", (object) string.Empty);
            data.CustomData.Add("ReceiptSeq", (object) string.Empty);
            data.CustomData.Add("TerminalId", (object) string.Empty);
            data.CustomData.Add("ReceivedCash", (object) string.Empty);
            data.CustomData.Add("ReceivedCard", (object) string.Empty);
            data.CustomData.Add("ChekTuri", (object) "Xarid");
          }
          if (this._devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.WebKassa && this._devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && data.FiscalType == CheckFiscalTypes.NonFiscal)
          {
            data.CustomData.Add("QRCodeUrl", (object) string.Empty);
            data.CustomData.Add("UniqueNumber", (object) string.Empty);
            data.CustomData.Add("RegistrationNumber", (object) string.Empty);
            data.CustomData.Add("CheckNumber", (object) string.Empty);
            data.CustomData.Add("OfdName", (object) string.Empty);
            data.CustomData.Add("OfdHost", (object) string.Empty);
            data.CustomData.Add("OfflineMode", (object) true);
          }
          if (this._devicesConfig.CheckPrinter.FiscalKkm.KkmType == GlobalDictionaries.Devices.FiscalKkmTypes.Hdm && this._devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm && data.FiscalType == CheckFiscalTypes.NonFiscal)
          {
            data.CustomData.Add("Tin", (object) "");
            data.CustomData.Add("Address", (object) "");
            data.CustomData.Add("CheckNumber", (object) data.Number);
            data.CustomData.Add("Crn", (object) "");
            data.CustomData.Add("FiscalNumber", (object) "");
            data.CustomData.Add("SerialNumber", (object) "");
            data.CustomData.Add("Time", (object) data.DateTime);
            data.CustomData.Add("CheckType", (object) (int) data.CheckType);
          }
          if (prepareReport == null)
            prepareReport = printableReportFactory.CreateForUsualCheck(data);
        }
        FastReportFacade fastReportFacade = new FastReportFacade();
        if (this._devicesConfig.CheckPrinter.PrinterSetting.IsSendInPrinter || this._devicesConfig.CheckPrinter.PrinterSetting.IsSelectPrinter)
        {
          string printerName = this._devicesConfig.CheckPrinter.PrinterSetting.IsSelectPrinter ? (string) null : this._devicesConfig.CheckPrinter.PrinterSetting.PrinterName;
          Other.ConsoleWrite(" ptinte name: " + printerName);
          fastReportFacade.PrintReport(prepareReport, printerName, this._devicesConfig.CheckPrinter.CheckTemplate);
        }
        else
          fastReportFacade.SelectTemplateAndShowReport(prepareReport, (Gbs.Core.Entities.Users.User) null, this._devicesConfig.CheckPrinter.CheckTemplate);
      }
    }

    public void PrintNonFiscalReport(List<string> texts)
    {
      if (this._devicesConfig.CheckPrinter.CheckNoFiscalTemplate.IsNullOrEmpty())
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.UsualPrinter_Укажите_шаблон_в_настройках_программы);
      }
      else if (!File.Exists(this._devicesConfig.CheckPrinter.CheckNoFiscalTemplate))
      {
        int num2 = (int) MessageBoxHelper.Show(Translate.UsualPrinter_Указанный_шаблон_в_настройках_не_существует_);
      }
      else if (this._devicesConfig.CheckPrinter.PrinterSetting.PrinterName.IsNullOrEmpty() && this._devicesConfig.CheckPrinter.PrinterSetting.IsSendInPrinter)
      {
        int num3 = (int) MessageBoxHelper.Show(Translate.UsualPrinter_Укажите_принтер_для_печати_в_настройках_программы);
      }
      else
      {
        IPrintableReport forNonFiscalReport = new PrintableReportFactory().CreateForNonFiscalReport(texts);
        FastReportFacade fastReportFacade = new FastReportFacade();
        if (this._devicesConfig.CheckPrinter.PrinterSetting.IsSendInPrinter || this._devicesConfig.CheckPrinter.PrinterSetting.IsSelectPrinter)
        {
          string printerName = this._devicesConfig.CheckPrinter.PrinterSetting.IsSelectPrinter ? (string) null : this._devicesConfig.CheckPrinter.PrinterSetting.PrinterName;
          Other.ConsoleWrite(" ptinte name: " + printerName);
          fastReportFacade.PrintReport(forNonFiscalReport, printerName, this._devicesConfig.CheckPrinter.CheckNoFiscalTemplate);
        }
        else
          fastReportFacade.SelectTemplateAndShowReport(forNonFiscalReport, (Gbs.Core.Entities.Users.User) null, this._devicesConfig.CheckPrinter.CheckNoFiscalTemplate);
      }
    }

    public void Dispose()
    {
    }

    public void PrintCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      this.PrintPrepareCheck(data);
    }

    public void PrintQrCode(List<string> line, string qr) => throw new NotImplementedException();
  }
}
