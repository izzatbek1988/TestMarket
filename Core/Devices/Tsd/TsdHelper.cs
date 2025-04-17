// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Tsd.TsdHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.Tsd.Models;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Devices.Tsd
{
  public class TsdHelper : IDisposable
  {
    private ITsd Tsd { get; set; }

    public TsdHelper(IConfig config)
    {
      try
      {
        if (!(config is Gbs.Core.Config.Devices devices))
          throw new ArgumentNullException(nameof (config));
        if (devices.Tsd.Type == GlobalDictionaries.Devices.TsdTypes.None)
        {
          this.Tsd = (ITsd) null;
          LogHelper.Debug("Комманда не может быть выполнена, в настройках не выбрана модель ТСД");
        }
        else
        {
          LogHelper.Debug("Инициализация ТСД, тип: " + devices.Tsd.Type.ToString());
          switch (devices.Tsd.Type)
          {
            case GlobalDictionaries.Devices.TsdTypes.Atol:
              break;
            case GlobalDictionaries.Devices.TsdTypes.MobileSmarts:
              this.Tsd = (ITsd) new MobileSmarts((Gbs.Core.Config.Devices) config);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса ТСД");
      }
    }

    public void ShowProperties()
    {
      this.Connect(true);
      this.Tsd.ShowProperties();
    }

    private bool IsConnected { get; set; }

    private bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        LogHelper.Debug("Подключаюсь ТСД");
        if (this.Tsd == null)
        {
          MessageBoxHelper.Warning(Translate.TsdHelper_Connect_Тип_терминала_сбора_данных__ТСД__не_указан_в_настройках);
          return false;
        }
        if (this.IsConnected)
        {
          LogHelper.Debug("ТСД уже был подклчен");
          return true;
        }
        int num = this.Tsd.Connect(onlyDriverLoad) ? 1 : 0;
        if (num == 0)
          MessageBoxHelper.Error(Translate.TsdHelper_Connect_Не_удалось_подключиться_к_терминалу_сбора_данных);
        if (num != 0)
          this.IsConnected = true;
        return num != 0;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка подключения к ТСД");
        return false;
      }
    }

    public bool Disconnect() => !this.Connect() || this.Tsd.Disconnect();

    public List<GoodForTsd> ReadInventory(string idDoc)
    {
      return !this.Connect() ? new List<GoodForTsd>() : this.Tsd.ReadInventory(idDoc);
    }

    public void WriteInventory(List<GoodForTsd> goods, string idDoc)
    {
      if (!this.Connect())
        return;
      this.Tsd.WriteInventory(goods, idDoc);
    }

    public void TestConnect()
    {
      if (!this.Connect())
        return;
      this.Tsd.TestConnect();
    }

    public void Dispose()
    {
      try
      {
        this.Disconnect();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка уничтожения объекта ТСД", false);
      }
    }
  }
}
