// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.SharedRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.GoodGroups;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Egais
{
  public static class SharedRepository
  {
    public static string GetFbNumberForGoodStock(GoodsStocks.GoodStock item)
    {
      string str;
      if (item == null)
      {
        str = (string) null;
      }
      else
      {
        List<EntityProperties.PropertyValue> properties = item.Properties;
        str = properties != null ? properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x =>
        {
          Guid? uid = x.Type?.Uid;
          Guid goodStockUidEgais = GlobalDictionaries.RegIdForGoodStockUidEgais;
          return uid.HasValue && uid.GetValueOrDefault() == goodStockUidEgais;
        }))?.Value.ToString() : (string) null;
      }
      return str ?? string.Empty;
    }

    public static void SetWaybillFromTicket(
      string replayId,
      EgaisHelper.A ticketsPathList,
      out List<Gbs.Helpers.Egais.Documents> ticketResult)
    {
      replayId = replayId.Trim().ToLower();
      EgaisHelper egaisHelper = new EgaisHelper();
      LogHelper.WriteToEgaisLog(ticketsPathList.ToJsonString(true));
      List<EgaisHelper.Url> list = ticketsPathList.Url.Where<EgaisHelper.Url>((Func<EgaisHelper.Url, bool>) (x => x.ReplyId?.Trim().ToLower() == replayId)).ToList<EgaisHelper.Url>();
      ticketResult = new List<Gbs.Helpers.Egais.Documents>();
      if (!list.Any<EgaisHelper.Url>())
      {
        LogHelper.WriteToEgaisLog("Тикетов для документа нет, replayId=" + replayId);
      }
      else
      {
        LogHelper.WriteToEgaisLog("Для документа обнаружено " + list.Count<EgaisHelper.Url>().ToString() + " тикетов");
        foreach (EgaisHelper.GetTicket command in list.Select<EgaisHelper.Url, EgaisHelper.GetTicket>((Func<EgaisHelper.Url, EgaisHelper.GetTicket>) (url => new EgaisHelper.GetTicket()
        {
          Path = url.Text
        })))
        {
          try
          {
            egaisHelper.GetCommand((EgaisHelper.GetEgiasCommand) command, false);
          }
          catch (Exception ex)
          {
            LogHelper.Error(ex, "Не удалось получить данные из тикета");
            continue;
          }
          ticketResult.Add(command.Result);
        }
      }
    }

    public static SharedRepository.AnswerConclusion GetResultTicket(
      string replayUid,
      out string error,
      out Gbs.Helpers.Egais.Documents documentsAnswer,
      ItemChoiceType choiceType = ItemChoiceType.ActWriteOff_v3)
    {
      documentsAnswer = (Gbs.Helpers.Egais.Documents) null;
      EgaisHelper.GetWaybillOut command = new EgaisHelper.GetWaybillOut();
      new EgaisHelper().GetCommand((EgaisHelper.GetEgiasCommand) command);
      List<Gbs.Helpers.Egais.Documents> ticketResult;
      SharedRepository.SetWaybillFromTicket(replayUid, command.Result, out ticketResult);
      if (!ticketResult.Any<Gbs.Helpers.Egais.Documents>())
      {
        error = "";
        return SharedRepository.AnswerConclusion.InProgress;
      }
      if (ticketResult.Count == 1 && choiceType == ticketResult.Single<Gbs.Helpers.Egais.Documents>().Document.ItemElementName)
      {
        documentsAnswer = ticketResult.Single<Gbs.Helpers.Egais.Documents>();
        error = "";
        return SharedRepository.AnswerConclusion.Accepted;
      }
      IOrderedEnumerable<Gbs.Helpers.Egais.Documents> source = ticketResult.OrderBy<Gbs.Helpers.Egais.Documents, DateTime?>((Func<Gbs.Helpers.Egais.Documents, DateTime?>) (x => ((TicketType) x?.Document?.Item)?.TicketDate));
      TicketResultType result = (source != null ? (TicketType) source.LastOrDefault<Gbs.Helpers.Egais.Documents>((Func<Gbs.Helpers.Egais.Documents, bool>) (x => ((TicketType) x.Document.Item)?.Result != null))?.Document?.Item : (TicketType) (object) null)?.Result;
      if (result == null)
      {
        error = "";
        return SharedRepository.AnswerConclusion.Accepted;
      }
      error = result.Comments;
      return result.Conclusion != ConclusionType.Rejected ? SharedRepository.AnswerConclusion.Accepted : SharedRepository.AnswerConclusion.Rejected;
    }

    public static bool SaveOldStockForEgais(List<StockPositionType> itemsStock, Users.User authUser)
    {
      Gbs.Core.Entities.GoodGroups.Group group;
      while (new FormSelectGroup().GetSingleSelectedGroupUid(authUser, out group))
      {
        if (group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service))
        {
          MessageBoxHelper.Warning("Для позиций из ЕГАИС можно выбрать категорию только с типом товаров: обычные или весовые.");
        }
        else
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar("Создание накладной с остатками товара (ЕГАИС)");
          try
          {
            using (DataBase dataBase = Data.GetDataBase())
            {
              List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
              List<Gbs.Core.Entities.Documents.Item> objList = new List<Gbs.Core.Entities.Documents.Item>();
              List<Gbs.Core.Entities.Goods.Good> itemsList = new List<Gbs.Core.Entities.Goods.Good>();
              int num = 0;
              Document document = new Document()
              {
                Comment = "Запрос остатков продукции из ЕГАИС",
                Status = GlobalDictionaries.DocumentsStatuses.Draft,
                Type = GlobalDictionaries.DocumentsTypes.Buy
              };
              foreach (StockPositionType stockPositionType in itemsStock)
              {
                stockPositionType.Product.Identity = num++.ToString("N0");
                bool isSaveGood;
                Gbs.Core.Entities.Goods.Good goodGorEgais = SharedRepository.GetGoodGorEgais(activeItems, stockPositionType.Product.AlcCode, stockPositionType.Product.AlcVolume, stockPositionType.Product.Capacity, stockPositionType.Product.ProductVCode, stockPositionType.Product.FullName, group, out isSaveGood);
                objList.Add(new Gbs.Core.Entities.Documents.Item()
                {
                  Good = goodGorEgais,
                  Quantity = stockPositionType.Quantity,
                  Comment = stockPositionType.Product.Identity,
                  BuyPrice = 0M,
                  DocumentUid = document.Uid
                });
                if (isSaveGood)
                  itemsList.Add(goodGorEgais);
              }
              document.Items = objList;
              new DocumentsRepository(dataBase).Save(document);
              new GoodRepository(dataBase).Save(itemsList);
              foreach (Gbs.Core.Entities.Documents.Item obj in document.Items)
              {
                Gbs.Core.Entities.Documents.Item documentItem = obj;
                string str = itemsStock.SingleOrDefault<StockPositionType>((Func<StockPositionType, bool>) (x => x.Product.Identity == documentItem.Comment))?.InformBRegId ?? "";
                if (!str.IsNullOrEmpty())
                {
                  GoodsStocks.GoodStock stock = documentItem.GoodStock.Clone();
                  SharedRepository.AddRegIdForStock(ref stock, str);
                  stock.Save(dataBase);
                }
              }
              progressBar.Close();
              return true;
            }
          }
          catch (Exception ex)
          {
            progressBar.Close();
            LogHelper.Error(ex, "Ошибка в форме карточки накладной");
            return false;
          }
        }
      }
      ProgressBarHelper.Close();
      return false;
    }

    public static void AddRegIdForStock(ref GoodsStocks.GoodStock stock, string informBRegId)
    {
      stock = GoodsStocks.GetStocksByUid(stock.Uid);
      LogHelper.WriteToEgaisLog("Проверяем остаток " + stock.ToJsonString(true));
      if (!SharedRepository.GetFbNumberForGoodStock(stock).IsNullOrEmpty())
      {
        LogHelper.WriteToEgaisLog("Изменяем старое доп. поле.Было informBRegId=" + stock.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.RegIdForGoodStockUidEgais)).Value?.ToString() + "Стало informBRegId=" + informBRegId);
        stock.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.RegIdForGoodStockUidEgais)).Value = (object) informBRegId;
      }
      else
      {
        LogHelper.WriteToEgaisLog("Добавляем новое доп. поле со значением informBRegId=" + informBRegId);
        GoodsStocks.GoodStock goodStock = stock;
        if (goodStock.Properties == null)
        {
          List<EntityProperties.PropertyValue> propertyValueList;
          goodStock.Properties = propertyValueList = new List<EntityProperties.PropertyValue>();
        }
        List<EntityProperties.PropertyValue> properties = stock.Properties;
        EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
        propertyValue.EntityUid = stock.Uid;
        EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
        propertyType.Uid = GlobalDictionaries.RegIdForGoodStockUidEgais;
        propertyType.EntityType = GlobalDictionaries.EntityTypes.GoodStock;
        propertyValue.Type = propertyType;
        propertyValue.Value = (object) informBRegId;
        properties.Add(propertyValue);
      }
    }

    public static bool UpdateRegIdStockForEgais(List<StockPositionType> itemsStock)
    {
      LogHelper.WriteToEgaisLog("Обновляем номера справок для ЕГАИС.\nИнформация, полученная из УТМ: " + itemsStock.ToJsonString(true));
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar("Обновление номеров справок для товара (ЕГАИС)");
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
          foreach (StockPositionType stockPositionType in itemsStock)
          {
            LogHelper.WriteToEgaisLog("Товар: " + stockPositionType.ToJsonString(true));
            bool isSaveGood;
            Gbs.Core.Entities.Goods.Good goodGorEgais = SharedRepository.GetGoodGorEgais(activeItems, stockPositionType.Product.AlcCode, stockPositionType.Product.AlcVolume, stockPositionType.Product.Capacity, stockPositionType.Product.ProductVCode, stockPositionType.Product.FullName, (Gbs.Core.Entities.GoodGroups.Group) null, out isSaveGood);
            LogHelper.WriteToEgaisLog(isSaveGood ? "Не нашли такой товар в БД" : "Нашли такой товар в БД:\n" + goodGorEgais.ToJsonString(true));
            if (!isSaveGood)
            {
              foreach (GoodsStocks.GoodStock goodStock in goodGorEgais.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Stock > 0M)))
              {
                GoodsStocks.GoodStock stock = goodStock.Clone();
                SharedRepository.AddRegIdForStock(ref stock, stockPositionType.InformBRegId);
                LogHelper.WriteToEgaisLog("Остаток перед сохранением: " + goodStock.ToJsonString(true));
                stock.Save(dataBase);
              }
            }
          }
          progressBar.Close();
          return true;
        }
      }
      catch (Exception ex)
      {
        progressBar.Close();
        LogHelper.Error(ex, "Ошибка в форме карточки накладной");
        return false;
      }
    }

    public static Gbs.Core.Entities.Goods.Good GetGoodGorEgais(
      List<Gbs.Core.Entities.Goods.Good> allGoods,
      string alcCode,
      Decimal alcVolume,
      Decimal capacity,
      string productCode,
      string name,
      Gbs.Core.Entities.GoodGroups.Group group,
      out bool isSaveGood)
    {
      LogHelper.WriteToEgaisLog("Ищем товар: " + alcCode);
      Gbs.Core.Entities.Goods.Good goodGorEgais = allGoods.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => (x.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.AlcCodeUid))?.Value.ToString().ToLower().Trim() ?? "") == alcCode.ToLower().Trim()));
      isSaveGood = goodGorEgais == null;
      if (goodGorEgais == null)
      {
        Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good();
        good.Name = name;
        good.Group = group;
        List<EntityProperties.PropertyValue> propertyValueList = new List<EntityProperties.PropertyValue>();
        EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
        propertyType1.Uid = GlobalDictionaries.AlcCodeUid;
        propertyValue1.Type = propertyType1;
        propertyValue1.Value = (object) alcCode;
        propertyValueList.Add(propertyValue1);
        EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType();
        propertyType2.Uid = GlobalDictionaries.AlcVolumeUid;
        propertyValue2.Type = propertyType2;
        propertyValue2.Value = (object) alcVolume;
        propertyValueList.Add(propertyValue2);
        EntityProperties.PropertyValue propertyValue3 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType3 = new EntityProperties.PropertyType();
        propertyType3.Uid = GlobalDictionaries.CapacityUid;
        propertyValue3.Type = propertyType3;
        propertyValue3.Value = (object) capacity;
        propertyValueList.Add(propertyValue3);
        EntityProperties.PropertyValue propertyValue4 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType4 = new EntityProperties.PropertyType();
        propertyType4.Uid = GlobalDictionaries.ProductCodeUid;
        propertyValue4.Type = propertyType4;
        propertyValue4.Value = (object) productCode;
        propertyValueList.Add(propertyValue4);
        good.Properties = propertyValueList;
        goodGorEgais = good;
      }
      return goodGorEgais;
    }

    public enum AnswerConclusion
    {
      Accepted,
      Rejected,
      InProgress,
    }
  }
}
