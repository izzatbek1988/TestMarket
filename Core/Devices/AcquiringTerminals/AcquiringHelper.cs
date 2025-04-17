// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.AcquiringHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.AcquiringTerminals.Models;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals
{
  public class AcquiringHelper : IDisposable
  {
    private readonly Gbs.Core.Config.Devices _devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();

    private IAcquiringTerminal Terminal { get; set; }

    public AcquiringHelper(Gbs.Core.Config.Devices devicesConfig)
    {
      this._devicesConfig = devicesConfig;
      this.LoadTerminal();
    }

    public AcquiringHelper() => this.LoadTerminal();

    private void LoadTerminal()
    {
      try
      {
        LogHelper.Debug("Загрузка терминала, тип: " + this._devicesConfig.AcquiringTerminal.Type.ToString());
        switch (this._devicesConfig.AcquiringTerminal.Type)
        {
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.None:
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.KkmServer:
            this.Terminal = (IAcquiringTerminal) new KkmServer(this._devicesConfig.AcquiringTerminal.LanConnection);
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.Sberbank:
            this.Terminal = (IAcquiringTerminal) new Sberbank();
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.Inpas:
            this.Terminal = (IAcquiringTerminal) new Inpas(this._devicesConfig);
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.PrivatBank:
            this.Terminal = (IAcquiringTerminal) new PrivatBank(this._devicesConfig.AcquiringTerminal.LanConnection);
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.Acrus2:
            this.Terminal = (IAcquiringTerminal) new Arcus2();
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.OschadBank:
            this.Terminal = (IAcquiringTerminal) new OschadBank(this._devicesConfig);
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.SmartPos:
            this.Terminal = (IAcquiringTerminal) new SmartPOS(this._devicesConfig);
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.BPOS:
            this.Terminal = (IAcquiringTerminal) new OschadBank(this._devicesConfig);
            break;
          case GlobalDictionaries.Devices.AcquiringTerminalTypes.SBRF:
            this.Terminal = (IAcquiringTerminal) new Sbrf();
            break;
          default:
            throw new ArgumentOutOfRangeException("Type", Translate.AcquiringHelper_Некорректный_тип_эквайринг_терминала__проверьте_настройки);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса эквайринг-терминала");
      }
    }

    public void GetReport()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.AcquiringHelper_GetReport_Печать_отчета_на_терминале);
      string slip = string.Empty;
      if ((!this.Terminal.Connect() ? 0 : (this.Terminal.GetReport(out slip) ? 1 : 0)) == 0)
      {
        int num = (int) MessageBoxHelper.Show(Translate.AcquiringHelper_GetReport_Не_удалось_снять_отчет_на_терминале, string.Empty, icon: MessageBoxImage.Hand);
      }
      else
        this.PrintSlip(slip);
      progressBar.Close();
    }

    public void CloseSession()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.AcquiringHelper_CloseSession_Закрытие_смены_на_терминале);
      string slip = string.Empty;
      if ((!this.Terminal.Connect() ? 0 : (this.Terminal.CloseSession(out slip) ? 1 : 0)) == 0)
      {
        int num = (int) MessageBoxHelper.Show(Translate.AcquiringHelper_CloseSession_Не_удалось_закрыть_смену_на_терминале, string.Empty, icon: MessageBoxImage.Hand);
      }
      else
        this.PrintSlip(slip);
      progressBar.Close();
    }

    private void PrintSlip(string slip)
    {
      try
      {
        if (this._devicesConfig.AcquiringTerminal.PrintSlipFromFile)
          slip = this.GetSlipFromFile(this._devicesConfig.AcquiringTerminal.SlipFilePath);
        if (slip == null)
          slip = "";
        slip = slip.Replace("\r\n\r\n", "\r\n");
        LogHelper.Debug("СЛИП ТЕРМИНАЛА:" + Other.NewLine() + slip);
        if (this._devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
        {
          using (KkmHelper kkmHelper = new KkmHelper(this._devicesConfig))
          {
            List<string> list = ((IEnumerable<string>) slip.Split('\n')).ToList<string>();
            kkmHelper.PrintNonFiscalDocument(list);
          }
        }
        if (this._devicesConfig.CheckPrinter.Type == GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter)
          new CheckPrinterHelper(this._devicesConfig).PrintText(slip);
        if (this._devicesConfig.CheckPrinter.CutPaperAfterDocuments)
          return;
        Thread.Sleep(3000);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось распечатать слип-чек для оплаты картой");
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.AcquiringHelper_PrintSlip_Не_удается_распечатать_слип_чек_оплаты_картой__проверьте_настройки_принтера_чека_));
      }
    }

    private string GetSlipFromFile(string filePath)
    {
      System.Text.Encoding encoding1;
      switch (this._devicesConfig.AcquiringTerminal.SlipEncoding)
      {
        case GlobalDictionaries.Encoding.CP866:
          encoding1 = System.Text.Encoding.GetEncoding(866);
          break;
        case GlobalDictionaries.Encoding.W1251:
          encoding1 = System.Text.Encoding.GetEncoding(1251);
          break;
        case GlobalDictionaries.Encoding.Utf8:
          encoding1 = System.Text.Encoding.UTF8;
          break;
        case GlobalDictionaries.Encoding.KOI8R:
          encoding1 = System.Text.Encoding.GetEncoding("KOI8-R");
          break;
        default:
          encoding1 = System.Text.Encoding.GetEncoding(866);
          break;
      }
      System.Text.Encoding encoding2 = encoding1;
      return !File.Exists(filePath) ? string.Empty : File.ReadAllText(filePath, encoding2);
    }

    public void ShowProperties() => this.Terminal.ShowProperties();

    public void ShowServiceMenu()
    {
      string slip;
      this.Terminal.ShowServiceMenu(out slip);
      if (slip.IsNullOrEmpty())
        return;
      this.PrintSlip(slip);
    }

    public void EmergencyCancel()
    {
      if (this._devicesConfig.AcquiringTerminal.Type == GlobalDictionaries.Devices.AcquiringTerminalTypes.None)
        LogHelper.Debug("Тип терминала не указан");
      else
        LogHelper.Debug("Аварийна отмена последнего платежа");
    }

    public bool DoPayment(
      Decimal sum,
      out string rrn,
      out string method,
      out string approvalCode,
      out string issuerName,
      out string terminalId,
      out string cardNumber,
      out string paymentSystem)
    {
      string slip = string.Empty;
      rrn = string.Empty;
      method = string.Empty;
      approvalCode = string.Empty;
      issuerName = string.Empty;
      terminalId = string.Empty;
      cardNumber = string.Empty;
      paymentSystem = string.Empty;
      if (this._devicesConfig.AcquiringTerminal.Type == GlobalDictionaries.Devices.AcquiringTerminalTypes.None)
      {
        LogHelper.Debug("Тип терминала не указан");
        return true;
      }
      if (sum < 0M)
        throw new ArgumentOutOfRangeException();
      if (sum == 0M)
      {
        LogHelper.Debug("Сумма оплаты по терминалу равна нулю");
        return true;
      }
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.AcquiringHelper_DoPayment_Ожидание_оплаты);
      LogHelper.Debug("Начинаем проведение платежа");
      try
      {
        if (!(!(this.Terminal is PrivatBank terminal1) ? (!(this.Terminal is OschadBank terminal2) ? this.Terminal.Connect() && this.Terminal.DoPayment(sum, out slip, out rrn, out method) : this.Terminal.Connect() && terminal2.DoPayment(sum, out slip, out rrn, out method, out approvalCode, out issuerName, out terminalId, out cardNumber, out paymentSystem)) : this.Terminal.Connect() && terminal1.DoPayment(sum, out slip, out rrn, out method, out approvalCode, out issuerName, out terminalId, out cardNumber, out paymentSystem)))
        {
          progressBar.Close();
          int num = (int) MessageBoxHelper.Show(Translate.AcquiringHelper_DoPayment_Не_удалось_выполнить_платеж_картой_через_терминал, icon: MessageBoxImage.Hand);
          rrn = string.Empty;
          return false;
        }
        for (int index = 0; index < this._devicesConfig.AcquiringTerminal.SlipPrintCounts; ++index)
          this.PrintSlip(slip);
        progressBar.Close();
      }
      catch (Exception ex)
      {
        progressBar.Close();
        throw ex;
      }
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
      {
        Title = Translate.AcquiringHelper_DoPayment_Оплата_прошла_успешно,
        Text = string.Format(Translate.AcquiringHelper_DoPayment_Оплата_картой_на_сумму__0_N2__успешно_выполнена, (object) sum)
      });
      return true;
    }

    public bool ReturnPayment(Decimal sum, string rrn = "", string method = "")
    {
      string slip = string.Empty;
      bool flag = this.Terminal.Connect() && this.Terminal.ReturnPayment(sum, out slip, rrn, method);
      for (int index = 0; index < this._devicesConfig.AcquiringTerminal.SlipPrintCounts; ++index)
        this.PrintSlip(slip);
      return flag;
    }

    public void Dispose() => this.Terminal?.Disconnect();
  }
}
