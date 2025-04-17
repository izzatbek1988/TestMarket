// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.BarcodesMiDays
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public static class BarcodesMiDays
  {
    private const string miDaysServiceUrl = "https://services.midays.ru/Barcodes/";

    public static List<ItemBarcodesMiDays> GetAll(string barcode)
    {
      try
      {
        LogHelper.Debug("Начинаю поиск товара по ШК в базе MiDays");
        RestHelper restHelper = new RestHelper("https://services.midays.ru/Barcodes/" + barcode + "/all", new int?(), (string) null);
        restHelper.CreateCommand("", TypeRestRequest.Get);
        restHelper.DoCommand();
        if (restHelper.StatusCode == HttpStatusCode.OK)
          return !restHelper.Answer.IsNullOrEmpty() ? JsonConvert.DeserializeObject<List<ItemBarcodesMiDays>>(restHelper.Answer) : new List<ItemBarcodesMiDays>();
        LogHelper.Debug("При добавлении товара в базу MiDays возникла ошибка с кодом " + restHelper.StatusCode.ToString());
        return new List<ItemBarcodesMiDays>();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения товара из базы MiDays", false);
        return new List<ItemBarcodesMiDays>();
      }
    }

    public static void Add(Gbs.Core.Entities.Goods.Good good)
    {
      try
      {
        LogHelper.Debug("Начинаю добавление товара в базу MiDays");
        string url = "https://services.midays.ru/Barcodes/";
        if (string.IsNullOrWhiteSpace(good.Barcode) || string.IsNullOrWhiteSpace(good.Name))
          LogHelper.Debug("Штрихкод или имя не указаны! Отмена добавления товара.");
        else if (good.Barcode.Length < 8)
          LogHelper.Debug("Некорректный штрихкод. Длинна меньше 8 символов.");
        else if (good.Name.Length < 3)
          LogHelper.Debug("Некорректное имя товара. Длинна меньше трёх символов!");
        else if (!long.TryParse(good.Barcode, out long _))
        {
          LogHelper.Debug("Штрихкод не является числовым!");
        }
        else
        {
          ItemBarcodesMiDays newItem = new ItemBarcodesMiDays()
          {
            barcode = good.Barcode,
            name = good.Name.Length > (int) byte.MaxValue ? good.Name.Substring(0, (int) byte.MaxValue) : good.Name
          };
          if (BarcodesMiDays.GetAll(good.Barcode).Where<ItemBarcodesMiDays>((Func<ItemBarcodesMiDays, bool>) (i => i.barcode == newItem.barcode && i.name == newItem.name)).Any<ItemBarcodesMiDays>())
          {
            LogHelper.Debug("В базе уже есть товар с таким ШК и назвванием");
          }
          else
          {
            RestHelper restHelper = new RestHelper(url, new int?(), newItem.ToJsonString(true, true));
            restHelper.CreateCommand("", TypeRestRequest.Post);
            restHelper.DoCommand();
            if (restHelper.StatusCode == HttpStatusCode.OK)
              return;
            LogHelper.Debug("При добавлении товара в базу MiDays возникла ошибка с кодом " + restHelper.StatusCode.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка добавления товара в базу MiDays", false);
      }
    }
  }
}
