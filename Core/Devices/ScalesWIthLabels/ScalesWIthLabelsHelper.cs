// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.ScalesWIthLabelsHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.ScalesWIthLabels.Models;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.ScalesWIthLabels
{
  public class ScalesWIthLabelsHelper : IDisposable
  {
    private IScalesWIthLabels Scale { get; }

    private bool IsConnected { get; set; }

    public ScalesWIthLabelsHelper(IConfig config)
    {
      try
      {
        if (!(config is Gbs.Core.Config.Devices devicesConfig))
          throw new ArgumentNullException(nameof (config));
        if (devicesConfig.ScaleWithLable.Type == GlobalDictionaries.Devices.ScaleLableTypes.None)
        {
          this.Scale = (IScalesWIthLabels) null;
          LogHelper.Debug("Комманда не может быть выполнена, в настройках не выбрана модель весов");
        }
        else
        {
          LogHelper.Debug("Инициализация весов, тип весов: " + devicesConfig.Scale.Type.ToString());
          switch (devicesConfig.ScaleWithLable.Type)
          {
            case GlobalDictionaries.Devices.ScaleLableTypes.None:
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.ShtrihM:
              this.Scale = (IScalesWIthLabels) new ShtrihM();
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.Cas:
              this.Scale = (IScalesWIthLabels) new Cas(devicesConfig);
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.Rongta:
              this.Scale = (IScalesWIthLabels) new Rongta(devicesConfig);
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.MettlerToledo:
              this.Scale = (IScalesWIthLabels) new MettlerToledo();
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.ScaleManager:
              this.Scale = (IScalesWIthLabels) new ScaleManager();
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.ShtrihM200:
              this.Scale = (IScalesWIthLabels) new ShtrihM200(devicesConfig);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса весов с печатью этикеток");
      }
    }

    public bool ShowProperties() => this.Connect(true) && this.Scale.ShowProperties();

    private bool Connect(bool onlyDriverLoad = false)
    {
      try
      {
        LogHelper.Debug("Подключаюсь к весам с печатью этикеток");
        if (this.Scale == null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.ScalesWIthLabelsHelper_Тип_весов_не_указан);
          return false;
        }
        if (this.IsConnected)
        {
          LogHelper.Debug("Весы уже были подклчены");
          return true;
        }
        int num1 = this.Scale.Connect(onlyDriverLoad) ? 1 : 0;
        if (num1 == 0)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.ScalesWIthLabelsHelper_Не_удалось_подключиться_к_весам_с_печатью_этикеток, icon: MessageBoxImage.Hand);
        }
        if (num1 != 0)
          this.IsConnected = true;
        return num1 != 0;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка подключения к весам с печатью этикеток");
        return false;
      }
    }

    public int SendGood(IEnumerable<Gbs.Core.Entities.Goods.Good> goods)
    {
      if (!this.Connect())
      {
        LogHelper.Debug("Не удалось подключиться к весам для передачи товаров");
        return 0;
      }
      Guid pluUid = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().ScaleWithLable.PluUid;
      if (!(pluUid == Guid.Empty))
        return this.Scale.SendGood(goods.Select<Gbs.Core.Entities.Goods.Good, GoodForWith>((Func<Gbs.Core.Entities.Goods.Good, GoodForWith>) (x => new GoodForWith()
        {
          Name = x.Name,
          Price = x.StocksAndPrices.Any<GoodsStocks.GoodStock>() ? x.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (stock => stock.Price)) : 0M,
          Plu = Convert.ToInt32(x.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == pluUid))?.Value ?? (object) 0)
        })).Where<GoodForWith>((Func<GoodForWith, bool>) (x => x.Plu != 0)).ToList<GoodForWith>());
      MessageBoxHelper.Warning(Translate.ScalesWIthLabelsHelper_В_настройках_программы_не_выбрано_поле_со_значением_PLU__Файл___Настройки___Товары_);
      return 0;
    }

    public void Dispose()
    {
      try
      {
        this.Scale?.Disconnect();
        this.IsConnected = false;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка уничтожения объекта весов с печатью этикеток", false);
      }
    }
  }
}
