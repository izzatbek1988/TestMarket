// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Sbp.SpbHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.DisplayQR;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Forms.Other;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Core.Devices.Sbp
{
  public class SpbHelper : IDisposable
  {
    private bool _stopPay;
    private string _payLoad;
    private string _rrn;
    private SbpPayViewModel _payForm;

    private static ISbp Sbp { get; set; }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public SpbHelper(Decimal sum, long numOrder, string returnId = "0")
    {
      try
      {
        SBP sbp1 = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SBP;
        if (sbp1.Type == GlobalDictionaries.Devices.SbpTypes.None)
          throw new DeviceException(Translate.SpbHelper_SpbHelper_Не_настроена_оплата_по_СБП_в_настройках_программы__проведите_настройку_и_повторите_оплату_еще_раз_);
        LogHelper.Debug("Инициализация СБП, тип СБП: " + sbp1.Type.ToString());
        ISbp sbp2;
        switch (sbp1.Type)
        {
          case GlobalDictionaries.Devices.SbpTypes.OpenBank:
            sbp2 = (ISbp) new SbpDefault();
            break;
          case GlobalDictionaries.Devices.SbpTypes.PayMaster:
            sbp2 = (ISbp) new PayMaster(sum, numOrder, Convert.ToInt64(returnId));
            break;
          case GlobalDictionaries.Devices.SbpTypes.NewPay:
            sbp2 = (ISbp) new NewPay(sum, returnId);
            break;
          case GlobalDictionaries.Devices.SbpTypes.AtolPay:
            sbp2 = (ISbp) new AtolPay(sum, returnId);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        SpbHelper.Sbp = sbp2;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса СБП", false);
        throw ex;
      }
    }

    private BitmapImage GenerateQR(string url) => url.GetQrCode();

    public void Dispose()
    {
    }

    public string Name { get; }

    public bool Pay(Decimal sum, out string rrn)
    {
      Gbs.Core.Config.Devices devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (!this.GetToken())
      {
        rrn = this._rrn;
        return false;
      }
      if (!this.GetQr())
      {
        string проведитеОперациюЕщеРаз = Translate.SpbHelper_Pay_Вы_отказались_от_оплаты_по_СБП__при_необходимости_проведите_операцию_еще_раз_;
        LogHelper.Debug(проведитеОперациюЕщеРаз);
        int num = (int) MessageBoxHelper.Show(string.Format(Translate.SbpPayViewModel_ErrorPay_, (object) sum, (object) проведитеОперациюЕщеРаз), icon: MessageBoxImage.Hand);
      }
      bool r = false;
      Task task = Task.Run((Action) (() =>
      {
        while (!this._stopPay && !this.GetStatus(sum, out r))
          Thread.Sleep(5000);
        try
        {
          if (devicesConfig.DisplayQr.Type == DisplayQrTypes.None)
            return;
          TaskHelper.TaskRun((Action) (() =>
          {
            DisplayQrHelper displayQrHelper = new DisplayQrHelper((IConfig) devicesConfig);
            if (r)
              displayQrHelper.Done();
            else
              displayQrHelper.Error();
            Thread.Sleep(1000);
            displayQrHelper.Clear();
          }), false);
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка передачи итога для СБП", false);
        }
      }));
      BitmapImage qr = this.GenerateQR(this._payLoad);
      try
      {
        if (devicesConfig.DisplayQr.Type != DisplayQrTypes.None)
        {
          using (DisplayQrHelper displayQrHelper = new DisplayQrHelper((IConfig) devicesConfig))
            displayQrHelper.WriteQr(this._payLoad);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка передачи QR для СБП", false);
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.SpbHelper_Pay_Не_удалось_отобразить_QR_код_для_оплаты_СБП_на_дисплее__Проверьте_настройки_подключения_к_оборудованию_));
      }
      try
      {
        if (devicesConfig.SBP.IsPrintQrForCheck)
        {
          CheckPrinterHelper checkPrinterHelper = new CheckPrinterHelper(devicesConfig);
          List<string> line = new List<string>();
          line.Add(Translate.SpbHelper_Pay_ОПЛАТА_ПО_СБП);
          line.Add(string.Format(Translate.SpbHelper_Pay_СУММА___0_N2_, (object) sum));
          string payLoad = this._payLoad;
          checkPrinterHelper.PrintQrCode(line, payLoad);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка передачи QR для СБП на печать", false);
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Не удалось напечатать QR-код для оплаты СБП на принтере. Проверьте настройки подключения к оборудованию."));
      }
      this._payForm = new SbpPayViewModel();
      this._payForm.ShowQr(qr, new Action(this.StopPay), sum, (object) SpbHelper.Sbp);
      task.Wait();
      if (this._stopPay)
      {
        if (this.GetStatus(sum, out r))
        {
          rrn = this._rrn;
          return r;
        }
        SpbHelper.Sbp.CancelQr();
        string проведитеОперациюЕщеРаз = Translate.SpbHelper_Pay_Вы_отказались_от_оплаты_по_СБП__при_необходимости_проведите_операцию_еще_раз_;
        LogHelper.Debug(проведитеОперациюЕщеРаз);
        int num = (int) MessageBoxHelper.Show(string.Format(Translate.SbpPayViewModel_ErrorPay_, (object) sum, (object) проведитеОперациюЕщеРаз), icon: MessageBoxImage.Hand);
        r = false;
      }
      rrn = this._rrn;
      return r;
    }

    public bool GetStatus(Decimal sum, out bool result)
    {
      SpbHelper.EStatusQr statusQr;
      if (this.GetStatus(out statusQr))
      {
        if (statusQr == SpbHelper.EStatusQr.ACWP)
        {
          this._payForm.DonePay(sum);
          LogHelper.Debug("Получен подтверждение оплаты QR-кода");
          result = true;
          return true;
        }
        if (statusQr == SpbHelper.EStatusQr.RJCT)
        {
          this._payForm.ErrorPay(sum, Translate.SpbHelper_GetStatus_Получен_отказ_оплаты_QR_кода);
          result = false;
          return true;
        }
      }
      result = false;
      return false;
    }

    public bool Return(Decimal sum)
    {
      if (!this.GetToken(true))
        return false;
      if (!this.GetQr(true))
      {
        MessageBoxHelper.Error(Translate.SpbHelper_Return_Не_удалось_совершить_возврат_по_СБП__попробуйте_еще_раз_или_обратитесь_в_службу_поддержки_);
        return false;
      }
      bool r = false;
      Task.Run((Action) (() =>
      {
        int num = 0;
        while (!this._stopPay)
        {
          if (num == 90)
          {
            string таймауту60Секунд = Translate.SpbHelper_Pay_Получение_статуса_от_СБП_прервано_по_таймауту__60_секунд__;
            LogHelper.Debug(таймауту60Секунд);
            MessageBoxHelper.Error(таймауту60Секунд);
            r = false;
            break;
          }
          SpbHelper.EStatusQr statusQr;
          if (this.GetStatus(out statusQr, true))
          {
            if (statusQr == SpbHelper.EStatusQr.ACWP)
            {
              LogHelper.Debug("Получен подтверждение возврата QR-кода");
              r = true;
              break;
            }
            if (statusQr == SpbHelper.EStatusQr.RJCT)
            {
              r = false;
              break;
            }
          }
          Thread.Sleep(1000);
          ++num;
        }
      })).Wait();
      return r;
    }

    public bool GetToken(bool isReturn = false) => SpbHelper.Sbp.GetToken(isReturn);

    public bool GetQr(bool isReturn = false)
    {
      string payLoad;
      string rrn;
      if (!SpbHelper.Sbp.GetQr(out payLoad, out rrn, isReturn))
        return false;
      this._payLoad = payLoad;
      this._rrn = rrn;
      return true;
    }

    public bool GetStatus(out SpbHelper.EStatusQr statusQr, bool isReturn = false)
    {
      return SpbHelper.Sbp.GetStatus(out statusQr, isReturn);
    }

    private void StopPay() => this._stopPay = true;

    public enum EStatusQr
    {
      NTST,
      RCVD,
      ACWP,
      RJCT,
    }
  }
}
