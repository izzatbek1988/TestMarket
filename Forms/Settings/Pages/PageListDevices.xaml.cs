// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.ListDevicesViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices;
using Gbs.Forms.Settings.Pages.DevicesSubForms;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class ListDevicesViewModel : ViewModel
  {
    private Gbs.Core.Config.Settings _settings;
    private Gbs.Core.Config.Devices _devicesConfig;

    public ObservableCollection<ItemDevice> Devices { get; set; } = new ObservableCollection<ItemDevice>();

    public ICommand GetDevicesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.Kkm,
            Device = (object) this._devicesConfig.CheckPrinter,
            Name = Translate.PageDevices_ПечатьЧеков
          });
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.AcquiringTerminal,
            Device = (object) this._devicesConfig.AcquiringTerminal,
            Name = Translate.PageDevices_Эквайринг
          });
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.BarcodeScanner,
            Device = (object) this._devicesConfig.BarcodeScanner,
            Name = Translate.СканерШК
          });
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.Scale,
            Device = (object) this._devicesConfig.Scale,
            Name = Translate.PageDevices_Весы
          });
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.DisplayBuyer,
            Device = (object) this._devicesConfig.DisplayPayer,
            Name = Translate.PageDevices_ДисплейПокупателя
          });
          this._devicesConfig.ExtraPrinters.Printers.ForEach((Action<ExtraPrinters.ExtraPrinter>) (x => this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.ExtraPrinters,
            Device = (object) x,
            Name = x.Name
          })));
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.ScaleWithLable,
            Device = (object) this._devicesConfig.ScaleWithLable,
            Name = Translate.PageDevices_ВесыСПечатьюЭтикеток
          });
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.SecondMonitor,
            Device = (object) this._devicesConfig.SecondMonitor,
            Name = Translate.ВторойМонитор
          });
          this.Devices.Add(new ItemDevice()
          {
            Type = IDevice.DeviceTypes.LablePrinter,
            Device = (object) this._devicesConfig.LablePrinter,
            Name = Translate.ПринтерЭтикеток
          });
          this.OnPropertyChanged("Devices");
        }));
      }
    }

    public ICommand EditCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ItemDevice> list = ((IEnumerable) obj).Cast<ItemDevice>().ToList<ItemDevice>();
          if (!list.Any<ItemDevice>() || list.Count > 1)
          {
            int num = (int) MessageBoxHelper.Show(Translate.ListDevicesViewModel_EditCommand_Необходимо_выбрать_одно_устройство_для_изменения_параметров_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            ItemDevice refValue = list.Single<ItemDevice>();
            if (!new DevicesCardViewModel().ShowCard(false, this._settings, ref refValue))
              return;
            this.Devices[this.Devices.ToList<ItemDevice>().FindIndex((Predicate<ItemDevice>) (x => x.Uid == refValue.Uid))] = refValue;
          }
        }));
      }
    }

    public ICommand AddCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ItemDevice itemDevice = new ItemDevice();
          if (!new DevicesCardViewModel().ShowCard(true, this._settings, ref itemDevice))
            return;
          this.Devices.Add(itemDevice);
          this.OnPropertyChanged("Devices");
        }));
      }
    }

    public ListDevicesViewModel()
    {
    }

    public ListDevicesViewModel(Gbs.Core.Config.Devices devicesConfig, Gbs.Core.Config.Settings settings)
    {
      this._devicesConfig = devicesConfig;
      this._settings = settings;
      this.Devices = new ObservableCollection<ItemDevice>(devicesConfig.ListDevices);
    }

    public bool Save()
    {
      this._devicesConfig.ListDevices = new List<ItemDevice>((IEnumerable<ItemDevice>) this.Devices);
      return true;
    }
  }
}
