// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Armenia;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Kazakhstan;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Ukraine.RRO;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms._shared;
using Gbs.Forms.Other;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Principal;
using System.Windows;
using WebSocketSharp;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm
{
  public class KkmHelper : ICheckPrinterDevice, IDisposable
  {
    public static Guid UserUid;

    private void PrintFiscalCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      LogHelper.Debug("Начинаю печатать фискальный чек. Тип чека " + data.CheckType.ToString());
      LogHelper.Trace("Содержимое чека: " + data.ToJsonString(true));
      this.CheckKkmReady(data.Cashier);
      this.CheckKkmReadyToReturn(data);
      this.SetClientForCheck(data);
      if (!TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.OpenCheck(data))))
      {
        LogHelper.Debug("Не удалось открыть чек");
        this.LasActionResult();
        KkmHelper.DisconnectAndDisposeKkm();
        throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_открыть_фискальный_чек);
      }
      this.RegisterGoods(data);
      this.RegisterDiscountForCheck(data);
      KkmHelper.RegisterCertificateAndBonuses(data);
      this.RegisterPayments(data);
      if (!this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.IsNoSendDigitalCheck && !StringExtensions.IsNullOrEmpty(data.AddressForDigitalCheck) && !TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.SendDigitalCheck(data.AddressForDigitalCheck))))
        LogHelper.Debug("Не удалось отправить электронный  чек");
      this.CloseCheck(data);
      this.CutPaper();
    }

    private void PrintNonFiscalCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      LogHelper.Debug("Начинаю печатать нефискальный чек");
      KkmHelper.UserUid = data.Cashier.UserUid;
      string text1 = ".";
      int width = this.DeviceConfigForConnect.CheckPrinter.PaperSymbolsWidth;
      string text2 = new string('_', width);
      List<NonFiscalString> pList = new List<NonFiscalString>()
      {
        new NonFiscalString(text1),
        new NonFiscalString(Translate.KkmHelper_ДОБРО_ПОЖАЛОВАТЬ__.PadCenter(width)),
        new NonFiscalString(text1),
        new NonFiscalString(Translate.KkmHelper_Дата__время__ + data.DateTime.ToString("dd.MM.yyyy HH:mm:ss")),
        new NonFiscalString(text2)
        {
          Alignment = TextAlignment.Center
        },
        new NonFiscalString(text1)
      };
      int i = 1;
      data.GoodsList.ForEach((Action<CheckGood>) (g =>
      {
        pList.Add(new NonFiscalString(i.ToString() + ". " + g.Name, TextAlignment.Left));
        Decimal num = g.Price * g.Quantity - g.DiscountSum;
        pList.Add(new NonFiscalString(string.Format("{0:N2} X {1:0.###} = {2:N2}", (object) g.Price, (object) g.Quantity, (object) num).PadLeft(width))
        {
          Alignment = TextAlignment.Right
        });
        if (g.DiscountSum > 0M)
          pList.Add(new NonFiscalString(string.Format(Translate.KkmHelper_СКИДКА___0____1_N2___, (object) g.DiscountSum, (object) g.Discount).PadLeft(width), TextAlignment.Right));
        if (!Ext.IsNullOrEmpty(g.Description))
        {
          MarkedInfo markedInfo = g.MarkedInfo;
          if ((markedInfo != null ? (int) markedInfo.Type : 0) == 0 && this.DeviceConfigForConnect.CheckPrinter.IsPrintCommentByGood)
          {
            int length = width > g.Description.Length + 13 ? g.Description.Length + 13 : width;
            pList.Add(new NonFiscalString(string.Format(Translate.KkmHelper_PrintNonFiscalCheck_Комментарий___0_, (object) g.Description).Substring(0, length)));
          }
        }
        ++i;
      }));
      pList.Add(new NonFiscalString(text2, TextAlignment.Center));
      pList.Add(new NonFiscalString(text1));
      pList.Add(new NonFiscalString((Translate.KkmHelper_ИТОГ + (data.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (g => g.Quantity * g.Price - g.DiscountSum)) - data.DiscountSum).ToString("N2").PadLeft(10)).PadLeft(width), TextAlignment.Right));
      Decimal num1 = data.PaymentsList.Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
      pList.Add(new NonFiscalString((Translate.KkmHelper_ПОЛУЧЕНО + num1.ToString("N2").PadLeft(10)).PadLeft(width), TextAlignment.Right));
      pList.Add(new NonFiscalString(text2, TextAlignment.Center));
      pList.Add(new NonFiscalString(text1));
      pList.Add(new NonFiscalString(text1));
      pList.Add(new NonFiscalString(text1));
      pList.Add(new NonFiscalString(Translate.KkmHelper_КАССИР_ + " ____________________", TextAlignment.Left));
      pList.Add(new NonFiscalString(data.Cashier.Name.PadLeft(20)));
      pList.Add(new NonFiscalString(text1));
      pList.Add(new NonFiscalString(Translate.KkmHelper_СПАСИБО_ЗА_ПОКУПКУ_.PadCenter(width)));
      pList.Add(new NonFiscalString(text1));
      this.PrintNonFiscalDocument(pList, true);
    }

    private Gbs.Core.Config.Devices DeviceConfigForConnect { get; }

    private static IFiscalKkm Kkm { get; set; }

    private static Gbs.Core.Config.Devices DevicesConfigStatic { get; set; }

    public KkmHelper(Gbs.Core.Config.Devices devicesConfig)
    {
      try
      {
        this.DeviceConfigForConnect = devicesConfig;
        if (this.DeviceConfigForConnect.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm)
        {
          KkmHelper.DisconnectAndDisposeKkm();
          throw new DeviceException(Translate.KkmHelper_Команда_не_может_быть_выполнена__т_к__в_настройках_используется_тип_устройства__отличного_от_ККМ);
        }
        if (KkmHelper.DevicesConfigStatic != null)
        {
          Gbs.Core.Config.CheckPrinter checkPrinter = KkmHelper.DevicesConfigStatic.CheckPrinter;
          if (checkPrinter.FiscalKkm.KkmType == this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.KkmType)
          {
            int? portNumber1 = checkPrinter.Connection.ComPort?.PortNumber;
            int? portNumber2 = this.DeviceConfigForConnect.CheckPrinter.Connection.ComPort?.PortNumber;
            if (portNumber1.GetValueOrDefault() == portNumber2.GetValueOrDefault() & portNumber1.HasValue == portNumber2.HasValue && checkPrinter.Connection.LanPort.UrlAddress == this.DeviceConfigForConnect.CheckPrinter.Connection.LanPort.UrlAddress)
            {
              int? portNumber3 = checkPrinter.Connection.LanPort.PortNumber;
              portNumber1 = this.DeviceConfigForConnect.CheckPrinter.Connection.LanPort.PortNumber;
              if (portNumber3.GetValueOrDefault() == portNumber1.GetValueOrDefault() & portNumber3.HasValue == portNumber1.HasValue && KkmHelper.Kkm != null && !checkPrinter.FiscalKkm.IsFreeKkmPort)
              {
                KkmHelper.Kkm.IsConnected = true;
                LogHelper.Debug("ККМ уже проинициализированна, тип ККМ: " + this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.KkmType.ToString());
                return;
              }
            }
          }
          KkmHelper.RunAction((Action) (() => KkmHelper.Kkm?.Disconnect()));
        }
        LogHelper.Debug("Инициализация ККМ, тип ККМ: " + this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.KkmType.ToString());
        IFiscalKkm fiscalKkm;
        switch (this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.KkmType)
        {
          case GlobalDictionaries.Devices.FiscalKkmTypes.None:
            fiscalKkm = (IFiscalKkm) null;
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.Atol8:
            fiscalKkm = (IFiscalKkm) new Atol8();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.Atol10:
            fiscalKkm = (IFiscalKkm) new Atol10(devicesConfig);
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.Shtrih:
            fiscalKkm = (IFiscalKkm) new ShtrihM();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.VikiPrint:
            fiscalKkm = (IFiscalKkm) new VikiPrint();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer:
            fiscalKkm = (IFiscalKkm) new KkmServer();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.Mercury:
            fiscalKkm = (IFiscalKkm) new Mercury();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer:
            fiscalKkm = (IFiscalKkm) new AtolKkmServer();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.HelpMicro:
            fiscalKkm = (IFiscalKkm) new HelpMicroJson();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.ExellioFP:
            fiscalKkm = (IFiscalKkm) new ExellioFP();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.MiniFP54:
            fiscalKkm = (IFiscalKkm) new MiniFP54();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.UzFiscalModule:
            fiscalKkm = (IFiscalKkm) new GbsUzOnlineKkm();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.PortFPGKZ:
            fiscalKkm = (IFiscalKkm) new PortFPGKZ();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.FsPRRO:
            fiscalKkm = (IFiscalKkm) new FsPRRO();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas:
            fiscalKkm = (IFiscalKkm) new LeoCas();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart:
            fiscalKkm = (IFiscalKkm) new Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart.AzSmart(devicesConfig);
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.WebKassa:
            fiscalKkm = (IFiscalKkm) new WebKassa();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.ReKassa:
            fiscalKkm = (IFiscalKkm) new ReKassa();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.HiPos:
            fiscalKkm = (IFiscalKkm) new HiPos();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.UzPos:
            fiscalKkm = (IFiscalKkm) new UzPos();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.DevicesConnector:
            fiscalKkm = (IFiscalKkm) new DevicesConnector();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.MobilKassa:
            fiscalKkm = (IFiscalKkm) new MobilKassa();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.Neva:
            fiscalKkm = (IFiscalKkm) new Neva(devicesConfig);
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests:
            fiscalKkm = (IFiscalKkm) new AtolWebRequest();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.SmartOne:
            fiscalKkm = (IFiscalKkm) new SmartOne();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.VikiDriver:
            fiscalKkm = (IFiscalKkm) new VikiPrintWeb();
            break;
          case GlobalDictionaries.Devices.FiscalKkmTypes.Hdm:
            fiscalKkm = (IFiscalKkm) new Hdm();
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        KkmHelper.Kkm = fiscalKkm;
        KkmHelper.DevicesConfigStatic = devicesConfig.Clone<Gbs.Core.Config.Devices>();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса ККМ", false);
        KkmHelper.DisconnectAndDisposeKkm();
      }
    }

    private void CheckFfdVersion()
    {
      if (this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.FfdVersion == GlobalDictionaries.Devices.FfdVersions.OfflineKkm & this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.VikiPrint, GlobalDictionaries.Devices.FiscalKkmTypes.Mercury, GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer))
        MessageBoxHelper.Warning(Translate.KkmHelper_CheckFfdVersion_);
      this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Atol10, GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer, GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests, GlobalDictionaries.Devices.FiscalKkmTypes.Mercury);
    }

    private KkmLastActionResult LasActionResult()
    {
      try
      {
        KkmLastActionResult lasActionResult = KkmHelper.Kkm?.LasActionResult;
        if (lasActionResult == null)
          return new KkmLastActionResult()
          {
            ActionResult = ActionsResults.Done
          };
        KkmLastActionResult lastActionResult = lasActionResult;
        lastActionResult.Message = lastActionResult.Message + Gbs.Helpers.Other.NewLine(2) + "Action: " + lasActionResult.Action.ToString() + ", Result: " + lasActionResult.ActionResult.ToString();
        LogHelper.Debug("Результат последнего действия на ККМ: " + lasActionResult.Message);
        int actionResult = (int) lasActionResult.ActionResult;
        return lasActionResult;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения результата последней операции на ККМ", false);
        throw new DeviceException("Ошибка получения результата последней операции на ККМ", ex);
      }
    }

    public void PrintCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      LogHelper.OnBegin();
      KkmHelper.UserUid = data.Cashier.UserUid;
      LogHelper.Debug("===========ЧЕК=========== " + Gbs.Helpers.Other.NewLine() + data.ToJsonString(true));
      switch (data.FiscalType)
      {
        case CheckFiscalTypes.Fiscal:
          this.PrintFiscalCheck(data);
          break;
        case CheckFiscalTypes.NonFiscal:
          this.PrintNonFiscalCheck(data);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      LogHelper.OnEnd();
    }

    public void CheckKkmReady(Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      LogHelper.Debug("Проверка готовности ККМ");
      KkmHelper.UserUid = cashier.UserUid;
      this.Connect();
      this.CheckFfdVersion();
      KkmStatus shortStatus = this.GetShortStatus();
      if (shortStatus.SessionStatus != SessionStatuses.Close)
      {
        if (shortStatus.CheckStatus == CheckStatuses.Open)
          KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.CancelCheck()));
        if (shortStatus.SessionStatus == SessionStatuses.OpenMore24Hours && !this.GetReport(ReportTypes.ZReport, cashier))
        {
          this.LasActionResult();
          KkmHelper.DisconnectAndDisposeKkm();
          throw new InvalidOperationException(Translate.KkmHelper_Смена_более_24_часов_и_ее_не_удалось_закрыть);
        }
      }
      if (shortStatus.SessionStatus == SessionStatuses.Close)
      {
        try
        {
          LogHelper.Debug("Смена закрыта, открываю");
          KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.OpenSession(cashier)));
        }
        catch
        {
          this.LasActionResult();
          KkmHelper.DisconnectAndDisposeKkm();
          throw;
        }
      }
      if (shortStatus.KkmState != KkmStatuses.NeedToContinuePrint)
        return;
      try
      {
        LogHelper.Debug("Необходимо допечатать документы, пытаемся");
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.EndPrintOldCheck()));
      }
      catch
      {
        this.LasActionResult();
        KkmHelper.DisconnectAndDisposeKkm();
        throw;
      }
    }

    private void SetClientForCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      string address;
      if (this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm && new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Russia && (data.Client != null || this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.IsAlwaysSendDigitalCheck) && !this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.IsNoSendDigitalCheck && new FrmSendDigitalCheck().ShowForm(data.Client?.Client, out address))
        data.AddressForDigitalCheck = address;
      if (data.Client == null)
        return;
      string str = data.Client.Client.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value?.ToString() ?? "";
      if (this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.SendBuyerInfoToCheck && !string.IsNullOrEmpty(str))
        return;
      LogHelper.Debug("Передача данных о клиенте в чек отключена");
      data.Client = (ClientAdnSum) null;
    }

    private void CheckKkmReadyToReturn(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      if (data.CheckType != CheckTypes.ReturnSale)
        return;
      LogHelper.Debug("Это чек возврата, проверяем сумму наличности в ККМ");
      Decimal num1 = data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method == GlobalDictionaries.KkmPaymentMethods.Cash)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      Decimal sum;
      this.CashSum(out sum);
      Decimal num2 = sum;
      if (!(num1 > num2))
        return;
      if (!KkmHelper.Kkm.GetType().IsEither<Type>(typeof (Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart.AzSmart), typeof (LeoCas), typeof (FsPRRO), typeof (Mercury), typeof (ReKassa), typeof (HiPos), typeof (UzPos), typeof (SmartOne), typeof (Hdm)))
        throw new Exception(Translate.KkmHelper_В_ККМ_недостаточно_наличности_для_проведения_чека_возврата);
    }

    private void CloseCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      IFiscalKkm kkm = KkmHelper.Kkm;
      PortFPGKZ portFpgkz = kkm as PortFPGKZ;
      bool flag;
      if (portFpgkz == null)
      {
        Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart.AzSmart azSmart = kkm as Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart.AzSmart;
        flag = azSmart != null ? TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => azSmart.CloseCheck(data))) : TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.CloseCheck()));
      }
      else
        flag = TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => portFpgkz.CloseCheck(data)));
      if (!flag)
      {
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.CancelCheck()));
        LogHelper.Debug("Не удалось закрыть чек");
        this.LasActionResult();
        KkmHelper.DisconnectAndDisposeKkm();
        throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_закрыть_фискальный_чек);
      }
      if (!new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.IsShowNotificationForCheck)
        return;
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.KkmHelper_CloseCheck_Фискальный_чек_успешно_передан_в_ККМ_));
    }

    private static void RegisterCertificateAndBonuses(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      Decimal certificatesPaymentSum = data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method == GlobalDictionaries.KkmPaymentMethods.Certificate)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      if (certificatesPaymentSum != 0M)
      {
        LogHelper.Debug(string.Format("Регистрирую скидку на чек (оплата сертификатом): {0}", (object) certificatesPaymentSum));
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.RegisterCheckDiscount(certificatesPaymentSum, Translate.KkmHelper_ОПЛАЧЕНО_СЕРТИФИКАТОМ)));
      }
      Decimal bonusesPaymentsSum = data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method == GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      if (!(bonusesPaymentsSum != 0M))
        return;
      LogHelper.Debug(string.Format("Регистрирую скидку на чек (оплата баллами): {0}", (object) bonusesPaymentsSum));
      KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.RegisterCheckDiscount(bonusesPaymentsSum, Translate.KkmHelper_СПИСАНО_БАЛЛОВ)));
    }

    private void RegisterDiscountForCheck(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      if (!(data.DiscountSum != 0M))
        return;
      LogHelper.Debug(string.Format("Регистрирую скидку на чек: {0}", (object) data.DiscountSum));
      if (!TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.RegisterCheckDiscount(data.DiscountSum, ""))))
      {
        this.LasActionResult();
        KkmHelper.DisconnectAndDisposeKkm();
        throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_зарегистрировать_скидку_в_фискальном_чеке);
      }
    }

    private void RegisterGoods(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      LogHelper.Debug("Начинаю регистрацию товаров");
      foreach (CheckGood goods in data.GoodsList)
      {
        CheckGood good = goods;
        if (!TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.RegisterGood(good, data.CheckType))))
        {
          this.LasActionResult();
          KkmHelper.DisconnectAndDisposeKkm();
          throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_зарегистрировать_товарную_позицию_в_фискальном_чеке);
        }
      }
    }

    private void RegisterPayments(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data)
    {
      LogHelper.Debug("Начинаю регистрацию платежей");
      List<CheckPayment> list = data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method != 0)).ToList<CheckPayment>();
      list.AddRange(data.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Cash)));
      foreach (CheckPayment checkPayment in list.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Sum != 0M && x.Method != GlobalDictionaries.KkmPaymentMethods.Bonus)))
      {
        CheckPayment p = checkPayment;
        if (!TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.RegisterPayment(p))))
        {
          LogHelper.Debug("Не удалось зарегистрировать платеж");
          this.LasActionResult();
          KkmHelper.DisconnectAndDisposeKkm();
          throw new InvalidOperationException(Translate.KkmHelper_Не_удалось_зарегистрировать_платеж_в_фискальном_чеке);
        }
      }
    }

    public void PrintNonFiscalDocument(List<string> stings)
    {
      List<NonFiscalString> stings1 = new List<NonFiscalString>();
      foreach (string sting in stings)
        stings1.Add(new NonFiscalString(sting));
      this.PrintNonFiscalDocument(stings1, true);
    }

    private void PrintNonFiscalDocument(List<NonFiscalString> stings, bool isEndDoc)
    {
      this.Connect();
      List<NonFiscalString> newL = new List<NonFiscalString>();
      int paperSymbolsWidth = this.DeviceConfigForConnect.CheckPrinter.PaperSymbolsWidth;
      using (List<NonFiscalString>.Enumerator enumerator = stings.GetEnumerator())
      {
label_6:
        if (enumerator.MoveNext())
        {
          NonFiscalString current = enumerator.Current;
          string str = current.Text;
          int length = str.Length;
          do
          {
            int num = paperSymbolsWidth > str.Length ? str.Length : paperSymbolsWidth;
            NonFiscalString nonFiscalString = new NonFiscalString(str.Substring(0, num), current.Alignment)
            {
              WideFont = current.WideFont
            };
            if (nonFiscalString.Text.Trim() != string.Empty)
              newL.Add(nonFiscalString);
            str = str.Substring(num);
            length -= paperSymbolsWidth;
          }
          while (length > 0);
          goto label_6;
        }
      }
      LogHelper.Trace("Массив нефискальных строк для печати:" + newL.ToJsonString());
      if (KkmHelper.Kkm is MiniFP54)
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.FeedPaper(5)));
      KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.PrintNonFiscalStrings(newL)));
      this.CutPaper();
      if (!isEndDoc)
        return;
      try
      {
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.FeedPaper(5)));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Не удалось  продвинуть чековую ленту");
      }
    }

    public void ShowStatus()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.KkmHelper_Проверка_статуса_ККМ);
      try
      {
        this.Connect();
        KkmStatus status = this.GetStatus();
        Decimal sum;
        this.GetCashSum(out sum);
        progressBar.Close();
        new FrmStatusKkm().ShowStatusKkm(status, sum);
      }
      catch (Exception ex)
      {
        progressBar.Close();
        string str = (ex is KkmException kkmException ? "\n" + kkmException.ExtMessage : "\n" + ex.Message) + "\nККМ: " + KkmHelper.Kkm.Name;
        LogHelper.Error(ex.InnerException, "Ошибка отображения статуса ККМ", false);
        throw new DeviceException(Translate.KkmHelper_Ошибка_отображения_статуса_ККМ + str, ex.InnerException);
      }
    }

    public KkmStatus GetKkmStatus()
    {
      try
      {
        this.Connect();
        return this.GetStatus();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex.InnerException, "Ошибка получения статуса ККМ", false);
        throw new DeviceException(Translate.KkmHelper_Ошибка_отображения_статуса_ККМ, ex.InnerException);
      }
    }

    public bool CreditPay(
      List<SelectPaymentMethods.PaymentGrid> paymentsList,
      Gbs.Core.Entities.Users.User user,
      string docId)
    {
      try
      {
        this.Connect();
        return ((SmartOne) KkmHelper.Kkm).CreditPay(paymentsList, user, docId);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex.InnerException, "Ошибка внесения долга по продаже", false);
        throw new DeviceException(Translate.KkmHelper_CreditPay_Ошибка_внесения_долга_по_продаже, ex.InnerException);
      }
    }

    public void ShowProperties()
    {
      LogHelper.Debug("Открываю настройки подключения к ККМ");
      if (KkmHelper.Kkm is ShtrihM && !new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator) && MessageBoxHelper.Show(Translate.KkmHelper_НастройкиШтрихМДолжныБытьСохраненыОтИмениАдминистратора, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
        return;
      this.Connect(true);
      KkmHelper.Kkm.ShowProperties();
    }

    private void CutPaper()
    {
      if (!this.DeviceConfigForConnect.CheckPrinter.CutPaperAfterDocuments)
        return;
      LogHelper.Debug("Отправляю команду на отрезку чека");
      try
      {
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.CutPaper()));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка отрезки бумаги в ККМ", false);
      }
    }

    private KkmStatus GetStatus()
    {
      KkmStatus status = TaskHelper.TaskRunAndWait<KkmStatus>((Func<KkmStatus>) (() => KkmHelper.Kkm.GetStatus()));
      LogHelper.Debug("Статус ККМ: " + status.ToJsonString(true));
      return status;
    }

    private KkmStatus GetShortStatus()
    {
      KkmStatus shortStatus = TaskHelper.TaskRunAndWait<KkmStatus>((Func<KkmStatus>) (() => KkmHelper.Kkm.GetShortStatus()));
      LogHelper.Debug("Короткий Статус ККМ: " + shortStatus.ToJsonString(true));
      return shortStatus;
    }

    public bool GetReport(ReportTypes reportType, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      try
      {
        KkmHelper.UserUid = cashier.UserUid;
        this.Connect();
        switch (reportType)
        {
          case ReportTypes.ZReport:
            KkmStatus shortStatus = this.GetShortStatus();
            if (shortStatus.SessionStatus == SessionStatuses.Close)
              throw new ErrorHelper.GbsException(Translate.KkmHelper_Нельзя_снять_Z_отчет__т_к__смена_уже_закрыта, (Exception) null, LogHelper.MsgTypes.MessageBox, ErrorHelper.ErrorDirections.Outer, false, MsgException.LevelTypes.Warm);
            if (shortStatus.CheckStatus == CheckStatuses.Open)
            {
              LogHelper.Debug("Закрываю чек для снятия Z-отчета");
              KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.CancelCheck()));
              return false;
            }
            if (MessageBoxHelper.Show(Translate.KkmHelper_Снять_Z_отчет_и_закрыть_смену_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
              return false;
            goto case ReportTypes.XReportWithGoods;
          case ReportTypes.XReport:
            if (KkmHelper.Kkm.GetType().IsEither<Type>(typeof (LeoCas), typeof (HiPos)))
            {
              LogHelper.Debug("Проверяем состояние ККТ для снятия отчета на " + KkmHelper.Kkm.GetType()?.ToString());
              this.CheckKkmReady(cashier);
              goto case ReportTypes.XReportWithGoods;
            }
            else
              goto case ReportTypes.XReportWithGoods;
          case ReportTypes.XReportWithGoods:
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.KkmHelper_Печать_отчета_на_онлайн_кассе);
            LogHelper.Debug(string.Format("Печатаю отчет на ККМ. Тип отчета: {0}; кассир: {1}", (object) reportType, (object) cashier.ToJsonString()));
            KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.GetReport(reportType, cashier)));
            this.CutPaper();
            progressBar.Close();
            return true;
          default:
            throw new ArgumentOutOfRangeException(nameof (reportType), (object) reportType, (string) null);
        }
      }
      catch (DeviceException ex)
      {
        ProgressBarHelper.Close();
        LogHelper.Error((Exception) ex, "Ошибка снятия отчета в ККМ", false);
        throw new DeviceException(Translate.KkmHelper_Ошибка_снятия_отчета_в_ККМ, (Exception) ex);
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.Error(ex, "Ошибка снятия отчета в ККМ", false);
        throw new DeviceException(Translate.KkmHelper_Ошибка_снятия_отчета_в_ККМ, ex);
      }
    }

    public bool CashOut(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      try
      {
        LogHelper.Debug(string.Format("Снятие наличности на сумму {0}", (object) sum));
        this.CheckKkmReady(cashier);
        Decimal sum1;
        this.GetCashSum(out sum1);
        if (sum > sum1)
        {
          if (!KkmHelper.Kkm.GetType().IsEither<Type>(typeof (Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Azerbaijan.AzSmart.AzSmart), typeof (LeoCas), typeof (FsPRRO), typeof (Mercury), typeof (HiPos), typeof (UzPos), typeof (SmartOne), typeof (Hdm)))
            throw new ErrorHelper.GbsException(string.Format(Translate.KkmHelper_CashOut_Сумма_наличности_в_ККМ_меньше__чем_сумма_для_выемки__В_ККМ____0_N2_, (object) sum1), (Exception) null, LogHelper.MsgTypes.MessageBox, ErrorHelper.ErrorDirections.Outer, false, MsgException.LevelTypes.Warm);
        }
        LogHelper.Debug("Выполняю изъятие наличности через ККМ");
        KkmHelper.UserUid = cashier.UserUid;
        this.Connect();
        return TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.CashOut(sum, cashier)));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка изъятия наличности из ККМ", false);
        throw new DeviceException(Translate.KkmHelper_Ошибка_изъятия_наличности_из_ККМ, ex);
      }
    }

    public bool CashIn(Decimal sum, Gbs.Core.Devices.CheckPrinters.Cashier cashier)
    {
      try
      {
        this.CheckKkmReady(cashier);
        LogHelper.Debug("Выполняю внесение наличности через ККМ");
        KkmHelper.UserUid = cashier.UserUid;
        this.Connect();
        return TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.CashIn(sum, cashier)));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка внесения наличности в ККМ", false);
        throw new DeviceException("Ошибка внесения наличности в ККМ", ex);
      }
    }

    public void GetCashSum(out Decimal sum)
    {
      LogHelper.Debug("Запрашиваю сумму наличности в ККМ");
      sum = 0M;
      this.Connect();
      Decimal sumOut = 0M;
      TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => this.CashSum(out sumOut)));
      sum = sumOut;
      LogHelper.Debug(sum.ToString("N2"));
    }

    private bool CashSum(out Decimal sum)
    {
      if (KkmHelper.Kkm == null)
      {
        sum = 0M;
        return false;
      }
      Decimal sumOut = 0M;
      int num = TaskHelper.TaskRunAndWait<bool>((Func<bool>) (() => KkmHelper.Kkm.GetCashSum(out sumOut))) ? 1 : 0;
      sum = sumOut;
      if (num == 0)
        this.LasActionResult();
      sum = sum == -1M ? 0M : sum;
      return num != 0;
    }

    private void Connect(bool onlyDriverLoad = false)
    {
      LogHelper.Debug("Подключаюсь к ККМ");
      if (KkmHelper.Kkm == null)
        throw new DeviceException(Translate.KkmHelper_Возможно__тип_ККМ_не_указан);
      if (KkmHelper.Kkm.IsConnected)
      {
        LogHelper.Debug("ККМ уже была подклчена");
      }
      else
      {
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.Connect(onlyDriverLoad)));
        KkmHelper.Kkm.IsConnected = true;
      }
    }

    public void PrintBarcode(string code, BarcodeTypes type)
    {
      LogHelper.OnBegin();
      KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.PrintBarcode(code, type)));
      LogHelper.OnEnd();
    }

    public void PrintQrCode(List<string> line, string qr)
    {
      this.PrintNonFiscalDocument(line.Select<string, NonFiscalString>((Func<string, NonFiscalString>) (x => new NonFiscalString(x, TextAlignment.Center)
      {
        WideFont = true
      })).ToList<NonFiscalString>(), false);
      this.PrintBarcode(qr, BarcodeTypes.QrCode);
      KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.FeedPaper(5)));
      KkmHelper.RunAction((Action) (() => KkmHelper.Kkm.CutPaper()));
    }

    public void Dispose()
    {
      if (!this.DeviceConfigForConnect.CheckPrinter.FiscalKkm.IsFreeKkmPort)
      {
        IFiscalKkm kkm = KkmHelper.Kkm;
        if ((kkm != null ? (kkm.IsCanHoldConnection ? 1 : 0) : 0) != 0)
        {
          LogHelper.Debug("Не отключаемся от ККМ из-за настройки");
          return;
        }
      }
      LogHelper.Debug("Отключение от ККМ");
      KkmHelper.DisconnectAndDisposeKkm();
    }

    private static void DisconnectAndDisposeKkm()
    {
      try
      {
        KkmHelper.RunAction((Action) (() => KkmHelper.Kkm?.Disconnect()));
        KkmHelper.Kkm = (IFiscalKkm) null;
      }
      catch (Exception ex)
      {
        KkmHelper.Kkm = (IFiscalKkm) null;
        LogHelper.Error(ex, "Ошибка уничтожения объекта ККМ", false);
      }
    }

    [HandleProcessCorruptedStateExceptions]
    private static void RunAction(Action action)
    {
      if (KkmHelper.Kkm?.GetType() == typeof (Atol10) || KkmHelper.Kkm?.GetType() == typeof (Neva))
        TaskHelper.TaskRunAndWait(action);
      else
        action();
    }

    public static string RemoveSpaceAndEnter(string str)
    {
      return str.Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "");
    }
  }
}
