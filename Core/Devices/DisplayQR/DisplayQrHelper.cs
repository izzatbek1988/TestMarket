// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayQR.DisplayQrHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.DisplayQR.Models;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.DisplayQR
{
  public class DisplayQrHelper : IDisposable
  {
    private IDisplayQr Display { get; }

    private bool IsConnected { get; set; }

    public DisplayQrHelper(IConfig config)
    {
      try
      {
        if (!(config is Gbs.Core.Config.Devices devicesConfig))
          throw new ArgumentNullException(nameof (config));
        if (devicesConfig.DisplayQr.Type == DisplayQrTypes.None)
        {
          this.Display = (IDisplayQr) null;
          LogHelper.Debug("Комманда не может быть выполнена, в настройках не выбрана модель дисплея QR");
        }
        else
        {
          LogHelper.Debug("Инициализация дисплея QR, тип: " + devicesConfig.DisplayQr.Type.ToString());
          switch (devicesConfig.DisplayQr.Type)
          {
            case DisplayQrTypes.None:
              break;
            case DisplayQrTypes.Mertech:
              this.Display = (IDisplayQr) new Mertech(devicesConfig);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса дисплея QR");
      }
    }

    public bool ShowProperties()
    {
      return this.Connect(true) && this.CheckError(this.Display.ShowProperties());
    }

    public bool WriteQr(string qr) => this.Connect() && this.CheckError(this.Display.WriteQr(qr));

    public bool Done() => this.Connect() && this.CheckError(this.Display.Done());

    public bool Error() => this.Connect() && this.CheckError(this.Display.Error());

    public bool Clear() => this.Connect() && this.CheckError(this.Display.Clear());

    private bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        LogHelper.Debug("Подключаюсь к дисплею QR");
        if (this.Display == null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.DisplayQrHelper_Connect_Тип_дисплея_для_вывода_QR_кода_не_указан_в_настройках);
          return false;
        }
        if (this.IsConnected)
        {
          LogHelper.Debug("Дисплей QR уже был подклчен");
          return true;
        }
        int num1 = this.Display.Connect(onlyDriverLoad) ? 1 : 0;
        if (num1 == 0)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.DisplayQrHelper_Connect_Не_удалось_подключиться_к_QR_дисплею, icon: MessageBoxImage.Hand);
        }
        if (num1 != 0)
          this.IsConnected = true;
        return num1 != 0;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка подключения к дисплею QR");
        return false;
      }
    }

    private bool CheckError(bool r)
    {
      if (r)
        return true;
      LogHelper.Debug("Ошибка при попытке работы с  дисплея QR");
      return false;
    }

    public void Dispose()
    {
      try
      {
        this.Display?.Disconnect();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка уничтожения объекта дисплея QR", false);
      }
    }
  }
}
