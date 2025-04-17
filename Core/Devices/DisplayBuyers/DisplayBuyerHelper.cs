// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayBuyers.DisplayBuyerHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.DisplayBuyers.Models;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.DisplayBuyers
{
  public class DisplayBuyerHelper : IDisposable
  {
    private IDisplayBuyers Display { get; }

    private bool IsConnected { get; set; }

    public DisplayBuyerHelper(IConfig config)
    {
      try
      {
        if (!(config is Gbs.Core.Config.Devices devicesConfig))
          throw new ArgumentNullException(nameof (config));
        if (devicesConfig.DisplayPayer.Type == DisplayBuyerTypes.None)
        {
          this.Display = (IDisplayBuyers) null;
          LogHelper.Debug("Комманда не может быть выполнена, в настройках не выбрана модель дисплея покупателей");
        }
        else
        {
          LogHelper.Debug("Инициализация дисплея покупателя, тип: " + devicesConfig.DisplayPayer.Type.ToString());
          switch (devicesConfig.DisplayPayer.Type)
          {
            case DisplayBuyerTypes.None:
              break;
            case DisplayBuyerTypes.ShtrihM:
              this.Display = (IDisplayBuyers) new ShtrihM();
              break;
            case DisplayBuyerTypes.Atol8:
              this.Display = (IDisplayBuyers) new Atol8();
              break;
            case DisplayBuyerTypes.EscPos:
              this.Display = (IDisplayBuyers) new EscPos(devicesConfig);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса дисплея покупателя", false);
      }
    }

    public bool ShowProperties()
    {
      return this.Connect(true) && this.CheckError(this.Display.ShowProperties());
    }

    public bool WriteText(List<string> lines, bool isTestBySetting = false)
    {
      try
      {
        if (!this.Connect(isShowError: isTestBySetting))
          return false;
        this.Clear();
        bool flag = true;
        int index = 0;
        foreach (string line in lines)
        {
          flag &= this.CheckError(this.Display.WriteText(line, index));
          ++index;
        }
        return flag;
      }
      catch (Exception ex)
      {
        int num = isTestBySetting ? 1 : 0;
        LogHelper.Error(ex, "Ошибка передачи текста на дисплей покупателя", num != 0);
        return false;
      }
    }

    public bool Clear() => this.Connect() && this.CheckError(this.Display.Clear());

    private bool Connect(bool onlyDriverLoad = false, bool isShowError = false)
    {
      try
      {
        LogHelper.Debug("Подключаюсь к дисплею покупателя");
        if (this.Display == null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.DisplayBuyerHelper_Тип_дисплея_покупателя_не_указан);
          return false;
        }
        if (this.IsConnected)
        {
          LogHelper.Debug("Дисплей покупателя уже был подклчен");
          return true;
        }
        int num1 = this.Display.Connect(onlyDriverLoad) ? 1 : 0;
        if (num1 == 0 & isShowError)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.DisplayBuyerHelper_Не_удалось_подключиться_к_дисплею_покупателя + Other.NewLine() + this.Display.LastResultCodeDescriptor, icon: MessageBoxImage.Hand);
        }
        if (num1 != 0)
          this.IsConnected = true;
        return num1 != 0;
      }
      catch (Exception ex)
      {
        int num = isShowError ? 1 : 0;
        LogHelper.Error(ex, "Ошибка подключения к дисплею покупателя", num != 0);
        return false;
      }
    }

    private bool CheckError(bool r)
    {
      if (r)
        return true;
      LogHelper.Debug("Ошибка при попытке работы с  дисплея покупателя");
      LogHelper.Debug(this.Display.LastResultCodeDescriptor);
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
        LogHelper.Error(ex, "Ошибка уничтожения объекта дисплея покупателя", false);
      }
    }

    public static string GetTextAndSum(string text, Decimal sum)
    {
      DisplayPayer displayPayer = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().DisplayPayer;
      string str = string.Format("={0:F2}", (object) sum);
      int num = displayPayer.CountCharInRow - text.Length - str.Length;
      int count = num < 0 ? 0 : num;
      return text + string.Join("", Enumerable.Repeat<string>(" ", count)) + str;
    }
  }
}
