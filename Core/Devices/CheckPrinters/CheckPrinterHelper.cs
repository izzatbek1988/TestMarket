// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckPrinterHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters
{
  public class CheckPrinterHelper
  {
    private readonly Gbs.Core.Config.Devices _config;

    public CheckPrinterHelper(Gbs.Core.Config.Devices config) => this._config = config;

    public bool PrintCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      try
      {
        if (data.FiscalType == CheckFiscalTypes.Fiscal)
          data.VerifyData();
        LogHelper.Debug("Начинаю печатать чек. Устройство для печати: " + this._config.CheckPrinter.Type.ToString());
        return this.PrintByPrinterType(data);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка печати чека");
        return false;
      }
    }

    private bool PrintByPrinterType(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      ICheckPrinterDevice checkPrinterDevice;
      if (this._config.CheckPrinter.IsPrintNoFiscalOtherPrinter && data.FiscalType == CheckFiscalTypes.NonFiscal)
      {
        checkPrinterDevice = (ICheckPrinterDevice) new UsualPrinter(this._config);
      }
      else
      {
        switch (this._config.CheckPrinter.Type)
        {
          case GlobalDictionaries.Devices.CheckPrinterTypes.None:
            return true;
          case GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter:
            checkPrinterDevice = (ICheckPrinterDevice) new UsualPrinter(this._config);
            break;
          case GlobalDictionaries.Devices.CheckPrinterTypes.EscPos:
            throw new NotImplementedException();
          case GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm:
            checkPrinterDevice = (ICheckPrinterDevice) new KkmHelper(this._config);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      try
      {
        checkPrinterDevice.PrintCheck(data);
      }
      finally
      {
        checkPrinterDevice.Dispose();
      }
      return true;
    }

    public void PrintText(string text, string qr = "")
    {
      new FastReportFacade().PrintText(text, this._config.CheckPrinter.PrinterSetting.IsSendInPrinter ? this._config.CheckPrinter.PrinterSetting.PrinterName : (string) null, qr);
    }

    public bool PrintQrCode(List<string> line, string qr)
    {
      ICheckPrinterDevice checkPrinterDevice;
      if (this._config.CheckPrinter.IsPrintNoFiscalOtherPrinter)
      {
        checkPrinterDevice = (ICheckPrinterDevice) new UsualPrinter(this._config);
      }
      else
      {
        switch (this._config.CheckPrinter.Type)
        {
          case GlobalDictionaries.Devices.CheckPrinterTypes.None:
            return true;
          case GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter:
            this.PrintText(string.Join("\n", (IEnumerable<string>) line), qr);
            return true;
          case GlobalDictionaries.Devices.CheckPrinterTypes.EscPos:
            throw new NotImplementedException();
          case GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm:
            checkPrinterDevice = (ICheckPrinterDevice) new KkmHelper(this._config);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      try
      {
        checkPrinterDevice.PrintQrCode(line, qr);
      }
      finally
      {
        checkPrinterDevice.Dispose();
      }
      return true;
    }
  }
}
