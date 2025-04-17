// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Scales.ScalesHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.OtherDevices;
using Gbs.Core.Devices.Scales.Models;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

#nullable disable
namespace Gbs.Core.Devices.Scales
{
  public class ScalesHelper : IDisposable
  {
    private System.Timers.Timer _scaleTimer;
    private IConfig _config;
    private bool _isListen;

    public event ScalesHelper.ScaleHandler Notify;

    private IScale Scale { get; set; }

    private bool IsConnected { get; set; }

    public ScalesHelper(IConfig config)
    {
      this._config = config;
      this.Init();
    }

    private void Init()
    {
      try
      {
        if (DevelopersHelper.IsDebug())
        {
          DevelopersHelper.ShowNotification("Работает эмулятор весов!");
          this.Scale = (IScale) new ScaleEmulation();
        }
        else
        {
          if (!(this._config is Gbs.Core.Config.Devices config))
            throw new ArgumentNullException("_config");
          if (config.Scale.Type == GlobalDictionaries.Devices.ScaleTypes.None)
          {
            this.Scale = (IScale) null;
            LogHelper.Debug("Комманда не может быть выполнена, в настройках не выбрана модель весов");
          }
          else
          {
            LogHelper.Debug("Инициализация весов, тип весов: " + config.Scale.Type.ToString());
            IScale scale = (IScale) null;
            switch (config.Scale.Type)
            {
              case GlobalDictionaries.Devices.ScaleTypes.None:
                this.Scale = scale;
                break;
              case GlobalDictionaries.Devices.ScaleTypes.CasComPort:
                scale = (IScale) new CasComPort(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.Wintec:
                scale = (IScale) new Wintec(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.MassaK100:
                scale = (IScale) new MassaK(config, GlobalDictionaries.Devices.ScaleTypes.MassaK100);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.ScalesMassaK:
                scale = (IScale) new MassaK(config, GlobalDictionaries.Devices.ScaleTypes.ScalesMassaK);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.ShtrihM:
                scale = (IScale) new AtolShtrihM();
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.IcsNt:
                scale = (IScale) new IcsNt(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.AtolScaner:
                scale = (IScale) new AtolScaner();
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.DatalogicComPort:
                scale = (IScale) new DatalogicComPort(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.MassaKProtocol2:
                scale = (IScale) new MassaKProtocol2(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.Bta:
                scale = (IScale) new Bta(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.Rongta:
                scale = (IScale) new Rongta(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              case GlobalDictionaries.Devices.ScaleTypes.Pos2Mertech:
                scale = (IScale) new POS2Mertech(config);
                goto case GlobalDictionaries.Devices.ScaleTypes.None;
              default:
                throw new ArgumentOutOfRangeException();
            }
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса весов");
      }
    }

    public bool ShowProperties()
    {
      this.Connect(true);
      return this.Scale.ShowProperties();
    }

    private void _scaleTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (!this._isListen)
      {
        LogHelper.Trace("Сработал таймер весов, но их прослушивание было деактировано. Ничего не делаю");
      }
      else
      {
        try
        {
          this._scaleTimer?.Stop();
          Decimal price = 0M;
          if (this.Scale == null)
          {
            LogHelper.Debug("Объект Scale равен null");
            return;
          }
          LogHelper.Trace("Scale timer elapsed in thread id: " + Thread.CurrentThread.ManagedThreadId.ToString());
          Decimal weight;
          this.Scale.GetWeight(out weight, price);
          LogHelper.Debug("Вес: " + weight.ToString());
          if (new ConfigsRepository<Settings>().Get().Sales.IsUnitsInGrams)
            weight = Decimal.Round(weight * 1000M, 1, MidpointRounding.AwayFromZero);
          if (this._isListen)
          {
            ScalesHelper.ScaleHandler notify = this.Notify;
            if (notify != null)
              notify(weight);
          }
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
        if (!this._isListen)
          return;
        this.CreateTimer();
      }
    }

    private void CreateTimer()
    {
      try
      {
        LogHelper.Trace("Инициализация таймера весов");
        this.DisposeTimer();
        this._scaleTimer = new System.Timers.Timer() { Interval = 300.0 };
        this._scaleTimer.Start();
        this._scaleTimer.AutoReset = false;
        this._scaleTimer.Elapsed += new ElapsedEventHandler(this._scaleTimer_Elapsed);
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошибка инициализации таймера весов");
      }
    }

    public void StartListen()
    {
      if (this._isListen)
      {
        LogHelper.Trace("Весы уже прослушиваются, останавливаю предыдущий процесс, запускаю новый");
        this.StopListen();
      }
      LogHelper.Debug("Начато прослушивание весов");
      this._isListen = true;
      Task.Run((Action) (() =>
      {
        try
        {
          this.Connect();
          this.CreateTimer();
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Ошибка подключения к весам");
          this.StopListen();
          string подключенияКВесам = Translate.ScalesHelper_Ошибка_подключения_к_весам;
          LogHelper.ShowErrorMgs(ex, подключенияКВесам, LogHelper.MsgTypes.Notification);
        }
      }));
    }

    public void StopListen()
    {
      try
      {
        LogHelper.Debug("Завершено прослушивание весов");
        this._isListen = false;
        this.DisposeTimer();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка завершения прослушивания весов");
      }
    }

    private void DisposeTimer()
    {
      LogHelper.Trace("Уничтожение таймера весов");
      if (this._scaleTimer == null)
        return;
      this._scaleTimer.Enabled = false;
      this._scaleTimer.Elapsed -= new ElapsedEventHandler(this._scaleTimer_Elapsed);
      this._scaleTimer.EndInit();
      this._scaleTimer.Stop();
      this._scaleTimer.Dispose();
      this._scaleTimer = (System.Timers.Timer) null;
    }

    private void Connect(bool onlyDriverLoad = false)
    {
      LogHelper.Debug("Подключаюсь к весам");
      if (this.Scale == null)
      {
        LogHelper.Debug("Объект весов не иниализирован, повторяю инициализацию");
        this.Init();
      }
      if (this.IsConnected)
      {
        LogHelper.Debug("Весы уже были подклчены");
      }
      else
      {
        this.Scale.Connect(onlyDriverLoad);
        this.IsConnected = true;
      }
    }

    public void Dispose()
    {
      try
      {
        this.StopListen();
        this.Scale?.Disconnect();
        this.Scale = (IScale) null;
        this.IsConnected = false;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка уничтожения объекта весов", false);
      }
    }

    public void Tara()
    {
      try
      {
        this.Scale?.Tara();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения тары", false);
      }
    }

    public void TaraReset()
    {
      try
      {
        this.Scale?.TaraReset();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка сброса тары с весов", false);
      }
    }

    public void Zero()
    {
      try
      {
        this.Scale?.Zero();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка обнуленния весов", false);
      }
    }

    public delegate void ScaleHandler(Decimal weight);
  }
}
