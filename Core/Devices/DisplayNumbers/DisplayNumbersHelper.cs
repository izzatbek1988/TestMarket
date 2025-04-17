// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayNumbers.DisplayNumbersHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;

#nullable disable
namespace Gbs.Core.Devices.DisplayNumbers
{
  public class DisplayNumbersHelper : IDisposable
  {
    private bool IsConnected { get; set; }

    private IDisplayNumbers Display { get; }

    public DisplayNumbersHelper(IConfig config)
    {
      try
      {
        if (!(config is Gbs.Core.Config.Devices devicesConfig))
          throw new ArgumentNullException(nameof (config));
        if (devicesConfig.DisplayNumbersPayer.Type == DisplayNumbersTypes.None)
        {
          this.Display = (IDisplayNumbers) null;
          LogHelper.Debug("Комманда не может быть выполнена, в настройках не выбрана модель дисплея покупателя (только цифры)");
        }
        else
        {
          LogHelper.Debug("Инициализация дисплея покупателя (цифры), тип: " + devicesConfig.DisplayNumbersPayer.Type.ToString());
          switch (devicesConfig.DisplayNumbersPayer.Type)
          {
            case DisplayNumbersTypes.None:
              break;
            case DisplayNumbersTypes.EscPos:
              this.Display = (IDisplayNumbers) new EcsPos(devicesConfig);
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

    public bool ShowProperties() => this.Connect(true) && this.Display.ShowProperties();

    public bool Disconnect() => !this.Connect() || this.Display.Disconnect();

    public bool WriteNumber(Decimal numbers) => this.Connect() && this.Display.WriteNumber(numbers);

    public bool Clear() => this.Connect() && this.Display.Clear();

    public bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        LogHelper.Debug("Подключаюсь к дисплею покупателя (цифры)");
        if (this.Display == null)
        {
          MessageBoxHelper.Warning(Translate.DisplayNumbersHelper_Connect_Тип_однострочного_дисплея_покупателя_для_вывода_цифр_не_указан_в_настройках);
          return false;
        }
        if (this.IsConnected)
        {
          LogHelper.Debug("Дисплей покупателя (цифры) уже был подклчен");
          return true;
        }
        int num = this.Display.Connect(onlyDriverLoad) ? 1 : 0;
        if (num == 0)
          MessageBoxHelper.Error(Translate.DisplayNumbersHelper_Connect_Не_удалось_подключиться_к_однострочному_дисплею_покупателя_для_вывода_цифр);
        if (num != 0)
          this.IsConnected = true;
        return num != 0;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка подключения к дисплею покупателя (цифры)");
        return false;
      }
    }

    public void Dispose()
    {
      try
      {
        this.Display?.Disconnect();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка уничтожения объекта дисплея покупателя (цифры)", false);
      }
    }
  }
}
