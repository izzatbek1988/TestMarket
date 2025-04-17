// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.AcquiringException
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals
{
  public class AcquiringException : DeviceException
  {
    public AcquiringException(
      IDevice device,
      string deviceMessage,
      AcquiringException.ErrorTypes errorType = AcquiringException.ErrorTypes.Unknown)
    {
      this.Device = device;
      this.ExtMessage = AcquiringException.ErrorsDictionary.SingleOrDefault<KeyValuePair<AcquiringException.ErrorTypes, string>>((Func<KeyValuePair<AcquiringException.ErrorTypes, string>, bool>) (x => x.Key == errorType)).Value + "\r\n" + deviceMessage;
    }

    public static Dictionary<AcquiringException.ErrorTypes, string> ErrorsDictionary
    {
      get
      {
        return new Dictionary<AcquiringException.ErrorTypes, string>()
        {
          {
            AcquiringException.ErrorTypes.Unknown,
            Translate.AcquiringException_Возникла_неизвестная_ошибка_на_устройстве__Обратитесь_в_службу_технической_поддержки_
          },
          {
            AcquiringException.ErrorTypes.OperationCancelByClient,
            Translate.AcquiringException_Клиент_отказался_от_выполнения_операции
          },
          {
            AcquiringException.ErrorTypes.OperationCancelByTimeout,
            Translate.AcquiringException_Истек_период_ожидания_для_выполнения_операции
          },
          {
            AcquiringException.ErrorTypes.DeviceNoFound,
            Translate.AcquiringException_Устройство_не_подключено_или_не_найдено__Проверьте_настройки_подключения
          },
          {
            AcquiringException.ErrorTypes.NotEnoughMoney,
            Translate.AcquiringException_Недостаточно_средств_на_карте
          },
          {
            AcquiringException.ErrorTypes.NeedToTotal,
            Translate.AcquiringException_Для_продолжения_необходимо_выполнить_сверку_итогов
          },
          {
            AcquiringException.ErrorTypes.WrongPinCode,
            Translate.AcquiringException_Введен_некорректный_пин_код
          },
          {
            AcquiringException.ErrorTypes.NoLinkToBank,
            Translate.AcquiringException_Не_удалось_установить_связь_с_банком
          }
        };
      }
    }

    public enum ErrorTypes
    {
      Unknown,
      OperationCancelByClient,
      OperationCancelByTimeout,
      DeviceNoFound,
      NotEnoughMoney,
      NeedToTotal,
      WrongPinCode,
      NoLinkToBank,
    }
  }
}
