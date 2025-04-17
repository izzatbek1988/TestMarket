// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.BarcodeScanners.ComPortScanner
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using WindowsInput;

#nullable disable
namespace Gbs.Core.Devices.BarcodeScanners
{
  public class ComPortScanner : IDevice
  {
    private static SerialPort _port = new SerialPort();
    private static Timer _timer = new Timer();
    private static InputSimulator _keyboardSimulator = new InputSimulator();

    private static event ComPortScanner.BarcodeChangeHandler BarcodeChanged;

    public static void Start(ComPort comPort = null)
    {
      bool flag = false;
      try
      {
        BarcodeScanner barcodeScanner = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner;
        if (comPort == null && barcodeScanner.Type != GlobalDictionaries.Devices.ScannerTypes.ComPort)
        {
          LogHelper.Debug(string.Format("Запуск сканера ШК отменен, т.к. его тип {0}", (object) barcodeScanner.Type));
          return;
        }
        DeviceHelper.CheckComPortExists(barcodeScanner.ComPort.PortName, (IDevice) new ComPortScanner());
        ComPort comPort1 = comPort ?? barcodeScanner.ComPort;
        LogHelper.Debug(string.Format("Подключаемся к сканеру ШК. Port: {0}, speed: {1}", (object) comPort1.PortName, (object) comPort1.Speed));
        ComPortScanner._port.PortName = comPort1.PortName;
        ComPortScanner._port.BaudRate = comPort1.Speed;
        ComPortScanner._port.Open();
        flag = ComPortScanner._port.IsOpen;
        if (flag)
          Task.Run((Action) (() =>
          {
            try
            {
              ComPortScanner.StartTimer();
            }
            catch (Exception ex)
            {
              LogHelper.WriteError(ex);
            }
          }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка подключения к сканеру ШК", false);
      }
      if (!flag)
        MessageBoxHelper.Error(Translate.ComPortScanner_Не_удалось_подключиться_к_сканеру_штрих_кодов);
      LogHelper.Debug(string.Format("Результат подключения к сканеру: {0}", (object) flag));
    }

    private static void StartTimer()
    {
      ComPortScanner._timer?.Dispose();
      ComPortScanner._timer = new Timer()
      {
        Interval = 100.0,
        AutoReset = true
      };
      ComPortScanner._timer.Elapsed += new ElapsedEventHandler(ComPortScanner._timer_Elapsed);
      ComPortScanner._timer.Start();
    }

    public static void RaiseEvent(string barcode)
    {
      ComPortScanner.BarcodeChangeHandler barcodeChanged = ComPortScanner.BarcodeChanged;
      if (barcodeChanged == null)
        return;
      barcodeChanged(barcode);
    }

    public static void SetDelegat(ComPortScanner.BarcodeChangeHandler action)
    {
      ComPortScanner.BarcodeChanged = (ComPortScanner.BarcodeChangeHandler) null;
      ComPortScanner.BarcodeChanged += action;
      LogHelper.Trace("Привязка события BarcodeChanged к " + action.Method.DeclaringType.FullName + "." + action.Method.Name);
    }

    private static void _timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      try
      {
        if (!ComPortScanner._port.IsOpen)
        {
          LogHelper.Debug("ком порт не открыт");
          LogHelper.Debug("Пробуем переоткрыть");
          ComPortScanner.Stop();
          ComPortScanner.Start();
          if (!ComPortScanner._port.IsOpen)
            return;
        }
        string str = ComPortScanner._port.ReadExisting();
        if (str.Any<char>())
          LogHelper.Trace("Сканер вернул: " + str);
        if (str.Length == 0)
          return;
        if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.IsEmulateKeyboard)
        {
          ComPortScanner._keyboardSimulator.Keyboard.TextEntry(str);
        }
        else
        {
          ComPortScanner.BarcodeChangeHandler barcodeChanged = ComPortScanner.BarcodeChanged;
          if (barcodeChanged == null)
            return;
          barcodeChanged(str);
        }
      }
      catch
      {
      }
    }

    public static void Stop()
    {
      LogHelper.OnBegin();
      ComPortScanner._timer?.Stop();
      if (ComPortScanner._port == null || !ComPortScanner._port.IsOpen)
        return;
      ComPortScanner._port.Close();
      LogHelper.OnEnd(string.Format("Result: {0}", (object) !ComPortScanner._port.IsOpen));
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.BarcodeScanner;

    public string Name => Translate.ComPortScanner_Сканер_ШК__com_port_;

    public delegate void BarcodeChangeHandler(string barcode);
  }
}
