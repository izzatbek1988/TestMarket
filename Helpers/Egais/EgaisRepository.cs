// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.EgaisRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace Gbs.Helpers.Egais
{
  internal class EgaisRepository
  {
    private readonly EgaisHelper _helper = new EgaisHelper();

    public List<EgaisDocument> GetListDocuments()
    {
      LogHelper.WriteToEgaisLog("Делаем запрос в УТМ для получения списка накладных");
      EgaisHelper.GetWaybillOut getWaybillOut = new EgaisHelper.GetWaybillOut();
      this._helper.GetCommand((EgaisHelper.GetEgiasCommand) getWaybillOut);
      LogHelper.WriteToEgaisLog("Начинаю разбор адресов документов");
      IEnumerable<EgaisHelper.Url> urls1 = getWaybillOut.Result.Url.Where<EgaisHelper.Url>((Func<EgaisHelper.Url, bool>) (x => x.Text.ToLower().Contains("/opt/out/waybill")));
      IEnumerable<EgaisHelper.Url> urls2 = getWaybillOut.Result.Url.Where<EgaisHelper.Url>((Func<EgaisHelper.Url, bool>) (x => x.Text.ToLower().Contains("/opt/out/form2reginfo/") || x.Text.ToLower().Contains("/opt/out/formbreginfo/")));
      LogHelper.WriteToEgaisLog("Список накладных из УТМ:\n" + urls1.ToJsonString(true));
      LogHelper.WriteToEgaisLog("Список справок из УТМ:\n" + urls2.ToJsonString(true));
      LogHelper.WriteToEgaisLog("Начинаем разбор справок формы 2");
      List<Gbs.Helpers.Egais.Documents> source = new List<Gbs.Helpers.Egais.Documents>();
      foreach (EgaisHelper.Url url in urls2)
      {
        EgaisHelper.GetForm2 command = new EgaisHelper.GetForm2()
        {
          Path = url.Text
        };
        this._helper.GetCommand((EgaisHelper.GetEgiasCommand) command, false);
        source.Add(command.Result);
      }
      LogHelper.WriteToEgaisLog("Разбор списка накладных");
      List<Gbs.Helpers.Egais.Documents> waybills = new List<Gbs.Helpers.Egais.Documents>();
      foreach (EgaisHelper.Url url in urls1)
      {
        EgaisHelper.GetWaybillTicket command = new EgaisHelper.GetWaybillTicket()
        {
          Path = url.Text
        };
        this._helper.GetCommand((EgaisHelper.GetEgiasCommand) command, false);
        waybills.Add(command.Result);
      }
      List<EgaisDocument> list = GetListDocumentsRepository.SetWaybillsForm2(waybills, source.Select<Gbs.Helpers.Egais.Documents, EgaisDocumentView>((Func<Gbs.Helpers.Egais.Documents, EgaisDocumentView>) (x => new EgaisDocumentView(x))).ToList<EgaisDocumentView>()).ToList<EgaisDocument>();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Document> itemsWithFilter = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Buy, true);
        GetListDocumentsRepository.SetWaybillsStatus((IEnumerable<EgaisDocument>) list, getWaybillOut, (IReadOnlyCollection<Document>) itemsWithFilter);
        return list;
      }
    }

    public void DoWriteOffItem(List<EgaisWriteOffActsItems> items, TypeWriteOff1 typeWriteOff)
    {
      string str;
      switch (typeWriteOff)
      {
        case TypeWriteOff1.Реализация:
          str = "Списание по факту розничной продажи";
          break;
        case TypeWriteOff1.Пересортица:
        case TypeWriteOff1.Недостача:
        case TypeWriteOff1.Уценка:
        case TypeWriteOff1.Порча:
        case TypeWriteOff1.Потери:
        case TypeWriteOff1.Проверки:
        case TypeWriteOff1.Арест:
        case TypeWriteOff1.Иныецели:
        case TypeWriteOff1.Производственныепотери:
          str = "Служебное списание";
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (typeWriteOff), (object) typeWriteOff, (string) null);
      }
      IEnumerable<EgaisWriteOffActsItems> source = items.GroupBy(x => new
      {
        FbNumber = x.FbNumber,
        MarkInfo = x.MarkInfo,
        StockUid = x.StockUid
      }).Select<IGrouping<\u003C\u003Ef__AnonymousType15<string, string, Guid>, EgaisWriteOffActsItems>, EgaisWriteOffActsItems>(x => new EgaisWriteOffActsItems()
      {
        FbNumber = x.Key.FbNumber,
        MarkInfo = x.Key.MarkInfo,
        Quantity = x.Sum<EgaisWriteOffActsItems>((Func<EgaisWriteOffActsItems, Decimal>) (s => s.Quantity)),
        StockUid = x.Key.StockUid,
        Sum = x.Sum<EgaisWriteOffActsItems>((Func<EgaisWriteOffActsItems, Decimal>) (s => s.Sum))
      });
      int identity = 1;
      EgaisHelper.SendActWriteOffBeer sendActWriteOffBeer = new EgaisHelper.SendActWriteOffBeer();
      sendActWriteOffBeer.Documents = new Gbs.Helpers.Egais.Documents()
      {
        Owner = new SenderInfo()
        {
          FSRAR_ID = new ConfigsRepository<Integrations>().Get().Egais.RarId.Trim()
        },
        Document = new DocBody()
        {
          Item = (object) new ActWriteOffType_v3()
          {
            Content = source.Select<EgaisWriteOffActsItems, ActWriteOffPositionType2>((Func<EgaisWriteOffActsItems, ActWriteOffPositionType2>) (x =>
            {
              ActWriteOffPositionType2 offPositionType2_1 = new ActWriteOffPositionType2();
              offPositionType2_1.Quantity = x.Quantity;
              offPositionType2_1.InformF1F2 = new InformF1F21()
              {
                Item = (object) new InformF2TypeItem()
                {
                  F2RegId = x.FbNumber
                }
              };
              ActWriteOffPositionType2 offPositionType2_2 = offPositionType2_1;
              string[] strArray;
              if (!x.MarkInfo.IsNullOrEmpty())
                strArray = new string[1]{ x.MarkInfo };
              else
                strArray = (string[]) null;
              offPositionType2_2.MarkCodeInfo = strArray;
              offPositionType2_1.SumSale = x.Sum;
              offPositionType2_1.Identity = identity++.ToString();
              offPositionType2_1.SumSaleSpecified = true;
              return offPositionType2_1;
            })).ToArray<ActWriteOffPositionType2>(),
            Header = new ActWriteOffType_v3Header()
            {
              Note = str,
              ActDate = DateTime.Now,
              ActNumber = Guid.NewGuid().ToString(),
              TypeWriteOff = typeWriteOff
            }
          },
          ItemElementName = ItemChoiceType.ActWriteOff_v3
        }
      };
      EgaisHelper.SendActWriteOffBeer command = sendActWriteOffBeer;
      new EgaisHelper().PostCommand((EgaisHelper.PostEgiasCommand) command);
      EgaisWriteOffAct egaisWriteOffAct = new EgaisWriteOffAct();
      egaisWriteOffAct.DateTime = DateTime.Now;
      EgaisHelper.A result = command.Result;
      string input;
      if (result == null)
      {
        input = (string) null;
      }
      else
      {
        List<EgaisHelper.Url> url = result.Url;
        input = url != null ? url.First<EgaisHelper.Url>()?.Text : (string) null;
      }
      if (input == null)
        input = Guid.Empty.ToString();
      egaisWriteOffAct.ReplayUid = Guid.Parse(input);
      egaisWriteOffAct.Status = EgaisWriteOffActStatus.Send;
      egaisWriteOffAct.Type = typeWriteOff;
      EgaisWriteOffAct actWriteOff = egaisWriteOffAct;
      new EgaisWriteOffActRepository().Save(actWriteOff);
      items.ForEach((Action<EgaisWriteOffActsItems>) (x =>
      {
        x.ActUid = actWriteOff.Uid;
        x.ActType = typeWriteOff;
      }));
      new EgaisWriteOffActsItemRepository().Save(items);
      DoWriteOffItemRepository.GetResultTicket(actWriteOff);
    }

    public void DoWaybillItem(
      EgaisDocument egaisDocument,
      ref Document documentDb,
      AcceptType3 acceptType,
      string comment)
    {
      Gbs.Helpers.Egais.Documents documents = new Gbs.Helpers.Egais.Documents()
      {
        Owner = new SenderInfo()
        {
          FSRAR_ID = new ConfigsRepository<Integrations>().Get().Egais.RarId.Trim()
        },
        Document = new DocBody()
        {
          ItemElementName = ItemChoiceType.WayBillAct_v4
        }
      };
      WayBillActType_v4 wayBillActTypeV4 = new WayBillActType_v4()
      {
        Header = new WayBillActType_v4Header()
        {
          ActDate = DateTime.Now,
          WBRegId = egaisDocument.Form2.WBRegId,
          ACTNUMBER = DateTime.Now.ToString("yyyyMMddHHmmss"),
          IsAccept = acceptType,
          Note = comment
        }
      };
      List<PositionType14> positionType14List = new List<PositionType14>();
      if (acceptType == AcceptType3.Differences)
      {
        Document doc = documentDb;
        positionType14List = egaisDocument.Waybill.Items.Select<PositionType, PositionType14>((Func<PositionType, PositionType14>) (x => new PositionType14()
        {
          Identity = x.Identity,
          RealQuantity = doc.Items.Single<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == x.UidGoodInDb && i.Comment == x.Identity)).Quantity,
          InformF2RegId = egaisDocument.Form2.DictionaryInformBRegId.SingleOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (f => f.Key == x.Identity)).Value ?? ""
        })).ToList<PositionType14>();
      }
      wayBillActTypeV4.Content = positionType14List.ToArray();
      documents.Document.Item = (object) wayBillActTypeV4;
      EgaisHelper.SendWayBillAct4 sendWayBillAct4 = new EgaisHelper.SendWayBillAct4();
      sendWayBillAct4.Documents = documents;
      EgaisHelper.SendWayBillAct4 command = sendWayBillAct4;
      new EgaisHelper().PostCommand((EgaisHelper.PostEgiasCommand) command);
      this.CheckError(command.Result);
      List<EntityProperties.PropertyValue> properties1 = documentDb.Properties;
      EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
      propertyValue1.EntityUid = documentDb.Uid;
      EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
      propertyType1.Uid = GlobalDictionaries.TTNEgais;
      propertyValue1.Type = propertyType1;
      propertyValue1.Value = (object) egaisDocument.Form2.WBRegId;
      properties1.Add(propertyValue1);
      List<EntityProperties.PropertyValue> properties2 = documentDb.Properties;
      EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue();
      propertyValue2.EntityUid = documentDb.Uid;
      EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType();
      propertyType2.Uid = GlobalDictionaries.ReplayUidEgais;
      propertyValue2.Type = propertyType2;
      EgaisHelper.A result = command.Result;
      propertyValue2.Value = result != null ? (object) result.Url.FirstOrDefault<EgaisHelper.Url>()?.Text : (object) (string) null;
      properties2.Add(propertyValue2);
      List<EntityProperties.PropertyValue> properties3 = documentDb.Properties;
      EntityProperties.PropertyValue propertyValue3 = new EntityProperties.PropertyValue();
      propertyValue3.EntityUid = documentDb.Uid;
      EntityProperties.PropertyValue propertyValue4 = propertyValue3;
      EntityProperties.PropertyType propertyType3 = new EntityProperties.PropertyType();
      propertyType3.Uid = GlobalDictionaries.StatusEgais;
      propertyValue4.Type = propertyType3;
      EntityProperties.PropertyValue propertyValue5 = propertyValue3;
      EgaisHelper.StatusEgaisTTN statusEgaisTtn;
      switch (wayBillActTypeV4.Header.IsAccept)
      {
        case AcceptType3.Accepted:
          statusEgaisTtn = EgaisHelper.StatusEgaisTTN.Accept;
          break;
        case AcceptType3.Rejected:
          statusEgaisTtn = EgaisHelper.StatusEgaisTTN.Cancel;
          break;
        case AcceptType3.Differences:
          statusEgaisTtn = EgaisHelper.StatusEgaisTTN.SendedAct;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      propertyValue5.Value = (object) statusEgaisTtn;
      properties3.Add(propertyValue3);
    }

    public void DoSaleStrongItem(List<BasketItem> items, Document saleDocument)
    {
      List<BasketItem> list = items.Where<BasketItem>((Func<BasketItem, bool>) (x => EgaisHelper.GetAlcoholType(x.Good) == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)).ToList<BasketItem>();
      if (!list.Any<BasketItem>())
        return;
      EgaisHelper.DoSaleStrongCommand command = new EgaisHelper.DoSaleStrongCommand();
      try
      {
        string str = "1";
        int num = 1;
        try
        {
          using (KkmHelper kkmHelper = new KkmHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
          {
            KkmStatus kkmStatus = kkmHelper.GetKkmStatus();
            str = kkmStatus.FactoryNumber;
            num = kkmStatus.SessionNumber;
          }
        }
        catch (Exception ex)
        {
          LogHelper.WriteToEgaisLog("Не удалось получить из ККМ данные о кассе и номере смены", ex);
        }
        SalePoints.SalePoint salePoint = SalePoints.GetSalePointList().First<SalePoints.SalePoint>();
        Cheque cheque = new Cheque()
        {
          Name = salePoint.Organization.Name,
          Inn = salePoint.Organization.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? "0",
          Kpp = salePoint.Organization.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.KppUid))?.Value.ToString() ?? "",
          Kassa = str,
          Number = saleDocument.Number,
          Address = salePoint.Organization.Address,
          Shift = num,
          Datetime = saleDocument.DateTime.ToString("ddMMyyHHmm"),
          Bottle = new List<Cheque.BottleItem>(list.Select<BasketItem, Cheque.BottleItem>((Func<BasketItem, Cheque.BottleItem>) (x => new Cheque.BottleItem()
          {
            Price = x.SalePrice.ToString("F2").Replace(',', '.'),
            Barcode = x.Comment,
            Ean = x.Good.Barcode,
            Volume = Convert.ToDecimal(x.Good.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.AlcVolumeUid)).Value).ToString("F4").Replace(',', '.')
          })))
        };
        command.Cheque = cheque;
        new EgaisHelper().PostCommand((EgaisHelper.PostEgiasCommand) command);
      }
      catch (Exception ex)
      {
        LogHelper.WriteToEgaisLog("Не удалось сформировать или отправить чек для списания крепкого алкоголя.\n" + ex.Message, ex);
        throw new Exception("Не удалось отправить данные в УТМ для продажи крепкого алкоголя, проверьте запущен ли УТМ или обратитесь в техническую поддержку.");
      }
      this.CheckError(command.Result);
    }

    public void GetOldWaybill()
    {
      Gbs.Helpers.Egais.Documents documents = new Gbs.Helpers.Egais.Documents()
      {
        Owner = new SenderInfo()
        {
          FSRAR_ID = new ConfigsRepository<Integrations>().Get().Egais.RarId.Trim()
        },
        Document = new DocBody()
        {
          ItemElementName = ItemChoiceType.QueryNATTN,
          Item = (object) new QueryParameters()
          {
            Parameters = new List<Parameter>()
            {
              new Parameter()
              {
                Name = "КОД",
                Value = new ConfigsRepository<Integrations>().Get().Egais.RarId.Trim()
              }
            }.ToArray()
          }
        }
      };
      EgaisHelper.PostOldWaybill postOldWaybill = new EgaisHelper.PostOldWaybill();
      postOldWaybill.Documents = documents;
      EgaisHelper.PostOldWaybill command = postOldWaybill;
      new EgaisHelper().PostCommand((EgaisHelper.PostEgiasCommand) command);
      this.CheckError(command.Result);
      this.DoWaitForResponse((Action) (() =>
      {
        string error;
        int resultTicket = (int) SharedRepository.GetResultTicket(command.Result.Url.First<EgaisHelper.Url>().Text, out error, out Gbs.Helpers.Egais.Documents _, ItemChoiceType.ReplyNoAnswerTTN);
        ProgressBarViewModel.Notification n = new ProgressBarViewModel.Notification()
        {
          Title = "Запрос необработанных накладных",
          Text = error
        };
        n.Type = resultTicket != 0 ? ProgressBarViewModel.Notification.NotificationsTypes.Error : ProgressBarViewModel.Notification.NotificationsTypes.Info;
        ProgressBarHelper.AddNotification(n);
      }), command.Result.Url.First<EgaisHelper.Url>().Text, ItemChoiceType.ReplyNoAnswerTTN);
    }

    public void GetSingleWaybill(string number)
    {
      Gbs.Helpers.Egais.Documents documents = new Gbs.Helpers.Egais.Documents()
      {
        Owner = new SenderInfo()
        {
          FSRAR_ID = new ConfigsRepository<Integrations>().Get().Egais.RarId.Trim()
        },
        Document = new DocBody()
        {
          ItemElementName = ItemChoiceType.QueryResendDoc,
          Item = (object) new QueryParameters()
          {
            Parameters = new List<Parameter>()
            {
              new Parameter() { Name = "WBREGID", Value = number }
            }.ToArray()
          }
        }
      };
      EgaisHelper.PostSingleWaybill postSingleWaybill = new EgaisHelper.PostSingleWaybill();
      postSingleWaybill.Documents = documents;
      EgaisHelper.PostSingleWaybill command = postSingleWaybill;
      new EgaisHelper().PostCommand((EgaisHelper.PostEgiasCommand) command);
      this.CheckError(command.Result);
      this.DoWaitForResponse((Action) (() =>
      {
        string error;
        int resultTicket = (int) SharedRepository.GetResultTicket(command.Result.Url.First<EgaisHelper.Url>().Text, out error, out Gbs.Helpers.Egais.Documents _);
        ProgressBarViewModel.Notification n = new ProgressBarViewModel.Notification()
        {
          Title = "Повторный запрос накладной " + number,
          Text = error
        };
        n.Type = resultTicket != 0 ? ProgressBarViewModel.Notification.NotificationsTypes.Error : ProgressBarViewModel.Notification.NotificationsTypes.Info;
        ProgressBarHelper.AddNotification(n);
      }), command.Result.Url.First<EgaisHelper.Url>().Text, ItemChoiceType.ActWriteOff_v3);
    }

    public void GetOldStockForTwoRegister(bool updateRegId = false)
    {
      Gbs.Helpers.Egais.Documents documents = new Gbs.Helpers.Egais.Documents()
      {
        Owner = new SenderInfo()
        {
          FSRAR_ID = new ConfigsRepository<Integrations>().Get().Egais.RarId.Trim()
        },
        Document = new DocBody()
        {
          ItemElementName = ItemChoiceType.QueryRestsShop_v2,
          Item = (object) new QueryParameters()
        }
      };
      EgaisHelper.PostOldStockForShop postOldStockForShop = new EgaisHelper.PostOldStockForShop();
      postOldStockForShop.Documents = documents;
      EgaisHelper.PostOldStockForShop command = postOldStockForShop;
      new EgaisHelper().PostCommand((EgaisHelper.PostEgiasCommand) command);
      this.CheckError(command.Result);
      this.DoWaitForResponse((Action) (() =>
      {
        string error;
        Gbs.Helpers.Egais.Documents documentsAnswer;
        if (SharedRepository.GetResultTicket(command.Result.Url.First<EgaisHelper.Url>().Text, out error, out documentsAnswer, ItemChoiceType.ReplyRestsShop_v2) == SharedRepository.AnswerConclusion.Accepted)
        {
          ReplyRestsShop_v2 item = (ReplyRestsShop_v2) documentsAnswer.Document.Item;
          if (!((IEnumerable<ShopPositionType>) item.Products).Any<ShopPositionType>((Func<ShopPositionType, bool>) (x => x.Quantity != 0M)) && !updateRegId)
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Запрос на получение остатков со второго регистра успешно обработан.\n\nНа данный момент нет нереализованных остатков."));
          }
          else
          {
            if (MessageBoxHelper.Question("Получена информация об остатках не реализованных товаров из ЕГАИС со второго регистра.\n\nПродолжить " + (updateRegId ? "обновление номеров справок" : "сохранение остатков") + " в программе?.") == MessageBoxResult.No)
              return;
            int i = 0;
            Application.Current?.Dispatcher?.Invoke((Action) (() =>
            {
              List<StockPositionType> list = ((IEnumerable<ShopPositionType>) item.Products).Select<ShopPositionType, StockPositionType>((Func<ShopPositionType, StockPositionType>) (x => new StockPositionType()
              {
                Quantity = x.Quantity,
                Product = new ProductInfo()
                {
                  AlcCode = x.Product.AlcCode,
                  AlcVolume = x.Product.AlcVolume,
                  Capacity = x.Product.Capacity,
                  FullName = x.Product.FullName,
                  Identity = i++.ToString("N0"),
                  ProductVCode = x.Product.ProductVCode
                }
              })).ToList<StockPositionType>();
              if ((updateRegId ? (SharedRepository.UpdateRegIdStockForEgais(list) ? 1 : 0) : (SharedRepository.SaveOldStockForEgais(list, (Gbs.Core.Entities.Users.User) null) ? 1 : 0)) != 0)
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Остатки алкогольной продукции со второго регистра успешно " + (updateRegId ? "обновлены" : "сохранены") + "." + (!updateRegId ? "\n\nДанные остатки будут доступны в качестве новой накладной в разделе Документы - Журнал поступлений." : "")));
              else
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Не удалось сохранить полученные остатки со второго регистра, обратитесь в техническую поддержку для уточнения", ProgressBarViewModel.Notification.NotificationsTypes.Error));
            }));
          }
        }
        else
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = "Запрос остатков товара со второго регистра (ЕГАИС)",
            Text = error
          });
      }), command.Result.Url.First<EgaisHelper.Url>().Text, ItemChoiceType.ReplyRestsShop_v2);
    }

    public void GetOldStockForOneRegister(bool updateRegId = false)
    {
      Gbs.Helpers.Egais.Documents documents = new Gbs.Helpers.Egais.Documents()
      {
        Owner = new SenderInfo()
        {
          FSRAR_ID = new ConfigsRepository<Integrations>().Get().Egais.RarId.Trim()
        },
        Document = new DocBody()
        {
          ItemElementName = ItemChoiceType.QueryRests,
          Item = (object) new QueryParameters()
        }
      };
      EgaisHelper.PostOldStockForOneRegister stockForOneRegister = new EgaisHelper.PostOldStockForOneRegister();
      stockForOneRegister.Documents = documents;
      EgaisHelper.PostOldStockForOneRegister command = stockForOneRegister;
      new EgaisHelper().PostCommand((EgaisHelper.PostEgiasCommand) command);
      this.CheckError(command.Result);
      this.DoWaitForResponse((Action) (() =>
      {
        string error;
        Gbs.Helpers.Egais.Documents documentsAnswer;
        if (SharedRepository.GetResultTicket(command.Result.Url.First<EgaisHelper.Url>().Text, out error, out documentsAnswer, ItemChoiceType.ReplyRests) == SharedRepository.AnswerConclusion.Accepted)
        {
          ReplyRests item = (ReplyRests) documentsAnswer.Document.Item;
          if (!((IEnumerable<StockPositionType>) item.Products).Any<StockPositionType>((Func<StockPositionType, bool>) (x => x.Quantity != 0M)) && !updateRegId)
          {
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Запрос на получение остатков со второго регистра успешно обработан.\n\nНа данный момент нет нереализованных остатков."));
          }
          else
          {
            if (MessageBoxHelper.Question("Получена информация об остатках не реализованных товаров из ЕГАИС с первого регистра.\n\nПродолжить " + (updateRegId ? "обновление номеров справок" : "сохранение остатков") + " в программе?.") == MessageBoxResult.No)
              return;
            Application.Current?.Dispatcher?.Invoke((Action) (() =>
            {
              if ((updateRegId ? (SharedRepository.UpdateRegIdStockForEgais(((IEnumerable<StockPositionType>) item.Products).ToList<StockPositionType>()) ? 1 : 0) : (SharedRepository.SaveOldStockForEgais(((IEnumerable<StockPositionType>) item.Products).ToList<StockPositionType>(), (Gbs.Core.Entities.Users.User) null) ? 1 : 0)) != 0)
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Остатки алкогольной продукции с первого регистра успешно " + (updateRegId ? "обновлены" : "сохранены") + "." + (!updateRegId ? "\n\nДанные остатки будут доступны в качестве новой накладной в разделе Документы - Журнал поступлений." : "")));
              else
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Не удалось сохранить полученные остатки со первого регистра, обратитесь в техническую поддержку для уточнения.", ProgressBarViewModel.Notification.NotificationsTypes.Error));
            }));
          }
        }
        else
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Title = (updateRegId ? "Запрос на обновление номеров справок товаров" : "Запрос остатков товара") + "со второго регистра (ЕГАИС)",
            Text = error
          });
      }), command.Result.Url.First<EgaisHelper.Url>().Text, ItemChoiceType.ReplyRests);
    }

    private void CheckError(EgaisHelper.A result)
    {
      if (result == null)
        return;
      string error = result.Error;
      bool? nullable = error != null ? new bool?(error.IsNullOrEmpty()) : new bool?();
      bool flag = false;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        throw new Exception("Не удалось отправить данные в УТМ, проверьте запущен ли УТМ или обратитесь в техническую поддержку.\n\n" + result.Error);
    }

    private void DoWaitForResponse(Action action, string replayUid, ItemChoiceType choiceType)
    {
      Task.Run((Action) (() =>
      {
        int num = 0;
        while (SharedRepository.GetResultTicket(replayUid, out string _, out Gbs.Helpers.Egais.Documents _, choiceType) == SharedRepository.AnswerConclusion.InProgress)
        {
          ++num;
          Thread.Sleep(60000);
          if (num == 5)
            return;
        }
        action();
      }));
    }
  }
}
