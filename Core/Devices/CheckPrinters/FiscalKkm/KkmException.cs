// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.KkmException
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm
{
  public class KkmException : DeviceException
  {
    public KkmException(IDevice device, string deviceMessage, KkmException.ErrorTypes errorType = KkmException.ErrorTypes.Unknown)
    {
      this.Device = device;
      this.ExtMessage = KkmException.ErrorsDictionary.SingleOrDefault<KeyValuePair<KkmException.ErrorTypes, string>>((Func<KeyValuePair<KkmException.ErrorTypes, string>, bool>) (x => x.Key == errorType)).Value + "\r\n" + deviceMessage;
    }

    public static Dictionary<KkmException.ErrorTypes, string> ErrorsDictionary
    {
      get
      {
        return new Dictionary<KkmException.ErrorTypes, string>()
        {
          {
            KkmException.ErrorTypes.Unknown,
            Translate.KkmException_Возникла_неизвестная_ошибка_в_ККМ__Обратитесь_в_службу_технической_поддержки
          },
          {
            KkmException.ErrorTypes.NeedService,
            Translate.KkmException_Возникла_ошибка_устройства_ККМ__Обратитесь_в_сервисный_центр
          },
          {
            KkmException.ErrorTypes.NoPaper,
            Translate.KkmException_В_устройстве_нет_бумаги__Установите_бумагу__кассовую_ленту_
          },
          {
            KkmException.ErrorTypes.SessionMore24Hour,
            Translate.KkmException_Смена_превышает_24_часа__Необходимо_закрыть_смену_
          },
          {
            KkmException.ErrorTypes.UnCorrectPaymentIndex,
            Translate.KkmException_Недопустимый_индекс_способа_оплаты__Необходимо_проверить_настройки_
          },
          {
            KkmException.ErrorTypes.NoConnection,
            Translate.KkmException_Нет_связи_с_устройством__Проверьте_соединение_и_настройки_подключения__
          },
          {
            KkmException.ErrorTypes.NonCorrectData,
            Translate.KkmException_В_ККМ_переданы_некорректные_данные_
          },
          {
            KkmException.ErrorTypes.PortBusy,
            Translate.KkmException_Порт_устройства_занят__Попробуйте_перезапустить_программу__выключите_включите_ККМ_
          },
          {
            KkmException.ErrorTypes.CoverOpen,
            Translate.KkmException_Открыта_крышка_ККМ__Закройте_и_повторите_попытку_печати
          },
          {
            KkmException.ErrorTypes.TooManyOfflineDocuments,
            Translate.KkmException_Превышен_срок_хранения_оффлайн_документов__Необходимо_проверить_связь_с_ОФД_
          },
          {
            KkmException.ErrorTypes.None,
            string.Empty
          },
          {
            KkmException.ErrorTypes.UnCorrectDateTime,
            Translate.KkmException_ErrorsDictionary_Некорректное_значение_даты_или_времени_в_ККМ
          },
          {
            KkmException.ErrorTypes.ConnectionError,
            Translate.KkmException_ErrorsDictionary_Воникла_ошибка_подключения_к_ККМ_или_связь_с_ККМ_была_разорвана__Попробуйте_перезагрузить_компьютер_и_выключить_включить_ККМ_
          }
        };
      }
    }

    public enum ErrorTypes
    {
      Unknown,
      NeedService,
      NoPaper,
      SessionMore24Hour,
      UnCorrectPaymentIndex,
      NoConnection,
      NonCorrectData,
      PortBusy,
      CoverOpen,
      TooManyOfflineDocuments,
      None,
      UnCorrectDateTime,
      ConnectionError,
    }
  }
}
