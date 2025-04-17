// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Device
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices
{
  public class Device : IDevice
  {
    private readonly IDevice.DeviceTypes _type;
    private readonly string _name;

    public static Dictionary<IDevice.DeviceTypes, string> DictionaryDeviceTypes
    {
      get
      {
        return new Dictionary<IDevice.DeviceTypes, string>()
        {
          {
            IDevice.DeviceTypes.Other,
            Translate.PaymentsActionsViewModel_Не_указан
          },
          {
            IDevice.DeviceTypes.Kkm,
            Translate.ПринтерЧеков
          },
          {
            IDevice.DeviceTypes.BarcodeScanner,
            Translate.PageDevices_СканерШтрихКодов
          },
          {
            IDevice.DeviceTypes.Scale,
            Translate.PageDevices_Весы
          },
          {
            IDevice.DeviceTypes.ScaleWithLable,
            Translate.PageDevices_ВесыСПечатьюЭтикеток
          },
          {
            IDevice.DeviceTypes.AcquiringTerminal,
            Translate.ТерминалЭквайринга
          },
          {
            IDevice.DeviceTypes.DisplayBuyer,
            Translate.PageDevices_ДисплейПокупателя
          },
          {
            IDevice.DeviceTypes.ExtraPrinters,
            Translate.ДопПринтер
          },
          {
            IDevice.DeviceTypes.SecondMonitor,
            Translate.ВторойМонитор
          },
          {
            IDevice.DeviceTypes.LablePrinter,
            Translate.ПринтерЭтикеток
          },
          {
            IDevice.DeviceTypes.Tsd,
            Translate.Device_DictionaryDeviceTypes_Терминал_сбора_данных
          }
        };
      }
    }

    public Device(IDevice.DeviceTypes type, string name)
    {
      this._type = type;
      this._name = name;
    }

    public IDevice.DeviceTypes Type() => this._type;

    public string Name => this._name;
  }
}
