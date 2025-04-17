// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ActionsHistoryHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Helpers
{
  public static class ActionsHistoryHelper
  {
    public static void AddActionListGoodThread(List<Gbs.Core.Entities.Goods.Good> goods, Users.User authUser)
    {
      Task.Run((Action) (() =>
      {
        using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          foreach (ActionHistory actionHistory in goods.Select<Gbs.Core.Entities.Goods.Good, ActionHistory>((Func<Gbs.Core.Entities.Goods.Good, ActionHistory>) (good => ActionsHistoryHelper.CreateHistory((IEntity) good, (IEntity) good, ActionType.Add, GlobalDictionaries.EntityTypes.Good, authUser))))
          {
            try
            {
              if (actionHistory.Data.IsNullOrEmpty())
                break;
              new ActionHistoryRepository(dataBase).Save(actionHistory);
            }
            catch (Exception ex)
            {
              LogHelper.WriteError(ex);
            }
          }
        }
      }));
    }

    public static void AddActionThread(ActionHistory actionHistory, bool isCheckNull)
    {
      Task.Run((Action) (() =>
      {
        try
        {
          if (actionHistory.Data.IsNullOrEmpty() & isCheckNull)
            return;
          using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
            new ActionHistoryRepository(dataBase).Save(actionHistory);
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }));
    }

    public static ActionHistory CreateHistory(
      IEntity oldItem,
      IEntity newItem,
      ActionType type,
      GlobalDictionaries.EntityTypes typeEntity,
      Users.User user)
    {
      ActionHistory action = new ActionHistory()
      {
        ActionType = type,
        EntityType = typeEntity,
        Section = Sections.GetCurrentSection(),
        User = user,
        EntityUid = newItem.Uid
      };
      switch (typeEntity)
      {
        case GlobalDictionaries.EntityTypes.Good:
          ActionsHistoryHelper.CreateGood((Gbs.Core.Entities.Goods.Good) oldItem, (Gbs.Core.Entities.Goods.Good) newItem, ref action);
          goto case GlobalDictionaries.EntityTypes.GroupEditGood;
        case GlobalDictionaries.EntityTypes.Client:
          ActionsHistoryHelper.CreateClient((Client) oldItem, (Client) newItem, ref action);
          goto case GlobalDictionaries.EntityTypes.GroupEditGood;
        case GlobalDictionaries.EntityTypes.Document:
          ActionsHistoryHelper.CreateDocument((Document) oldItem, (Document) newItem, ref action);
          goto case GlobalDictionaries.EntityTypes.GroupEditGood;
        case GlobalDictionaries.EntityTypes.GoodGroup:
          ActionsHistoryHelper.CreateGoodGroup((GoodGroups.Group) oldItem, (GoodGroups.Group) newItem, ref action);
          goto case GlobalDictionaries.EntityTypes.GroupEditGood;
        case GlobalDictionaries.EntityTypes.ClientGroup:
          ActionsHistoryHelper.CreateClientGroup((Gbs.Core.Entities.Clients.Group) oldItem, (Gbs.Core.Entities.Clients.Group) newItem, ref action);
          goto case GlobalDictionaries.EntityTypes.GroupEditGood;
        case GlobalDictionaries.EntityTypes.Payment:
          ActionsHistoryHelper.CreatePayment((Payments.Payment) oldItem, (Payments.Payment) newItem, ref action);
          goto case GlobalDictionaries.EntityTypes.GroupEditGood;
        case GlobalDictionaries.EntityTypes.ItemList:
          ActionsHistoryHelper.CreateItemList((BasketItem) oldItem, (BasketItem) newItem, ref action);
          goto case GlobalDictionaries.EntityTypes.GroupEditGood;
        case GlobalDictionaries.EntityTypes.GroupEditGood:
        case GlobalDictionaries.EntityTypes.GroupEditGoodGroup:
          return action;
        default:
          throw new ArgumentOutOfRangeException(nameof (typeEntity), (object) typeEntity, (string) null);
      }
    }

    public static ActionHistory CreateTextHistory(
      string data,
      GlobalDictionaries.EntityTypes typeEntity,
      Users.User user)
    {
      return new ActionHistory()
      {
        ActionType = ActionType.Edit,
        EntityType = typeEntity,
        Section = Sections.GetCurrentSection(),
        User = user,
        EntityUid = Guid.Empty,
        Data = new List<string>() { data }
      };
    }

    public static ActionHistory CreateShowHistory(string data, Users.User user)
    {
      return new ActionHistory()
      {
        ActionType = ActionType.Show,
        EntityType = GlobalDictionaries.EntityTypes.Window,
        Section = Sections.GetCurrentSection(),
        User = user,
        EntityUid = Guid.Empty,
        Data = new List<string>() { data }
      };
    }

    private static void CreateDocument(
      Document oldItem,
      Document newItem,
      ref ActionHistory action)
    {
      if (newItem.IsDeleted && !oldItem.IsDeleted && action.ActionType == ActionType.Delete)
      {
        switch (newItem.Type)
        {
          case GlobalDictionaries.DocumentsTypes.Sale:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_Удалена_продажа__0_, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.Buy:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.Move:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_удаленоОтправлниеМеждуТочками, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.WriteOff:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_удаленоСписание, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.Inventory:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_удаленаИнвентаризация, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.CafeOrder:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_удален_заказ, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.ClientOrder:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_удаленЗаказРезерв, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.MoveStorage:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_удаленоПеремещениеМеждуСкладами, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.ProductionList:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_удаленДокументПроизводства, (object) newItem.Number));
            break;
        }
      }
      else if (action.ActionType == ActionType.Add)
      {
        switch (newItem.Type)
        {
          case GlobalDictionaries.DocumentsTypes.Buy:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_ДобавленаНакладная, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.Move:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_CreatePointMovement, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.WriteOff:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_CreateWriteoffAct, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.Inventory:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_CreateInventoryAct, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.ClientOrder:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_CreateClientReserve, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.MoveStorage:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_CreateStorageMovement, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.ProductionList:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_CreateProduction, (object) newItem.Number));
            break;
        }
      }
      else
      {
        if (oldItem.Comment != newItem.Comment)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Комментарий, (object) oldItem.Comment, (object) newItem.Comment));
        if (oldItem.Number != newItem.Number)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Номер, (object) oldItem.Number, (object) newItem.Number));
        if (oldItem.ContractorUid != newItem.ContractorUid)
          action.Data.Add(Translate.ActionsHistoryHelper_Изменен_контрагент);
        if (oldItem.DateTime != newItem.DateTime)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Дата, (object) oldItem.DateTime, (object) newItem.DateTime));
        if (oldItem.ParentUid != newItem.ParentUid)
          action.Data.Add(Translate.ActionsHistoryHelper_Изменен_родитель_документа);
        if (oldItem.UserUid != newItem.UserUid)
          action.Data.Add(Translate.ActionsHistoryHelper_Изменен_пользователь);
        if (oldItem.Status != newItem.Status)
        {
          Dictionary<GlobalDictionaries.DocumentsStatuses, string> statusesDictionary = GlobalDictionaries.DocumentStatusesDictionary;
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Статус, (object) (statusesDictionary.SingleOrDefault<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, bool>) (x => x.Key == oldItem.Status)).Value ?? ""), (object) (statusesDictionary.SingleOrDefault<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>>((Func<KeyValuePair<GlobalDictionaries.DocumentsStatuses, string>, bool>) (x => x.Key == newItem.Status)).Value ?? "")));
        }
        Guid? uid1 = oldItem.Storage?.Uid;
        Guid? uid2 = newItem.Storage?.Uid;
        if ((uid1.HasValue == uid2.HasValue ? (uid1.HasValue ? (uid1.GetValueOrDefault() != uid2.GetValueOrDefault() ? 1 : 0) : 0) : 1) != 0)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Склад, (object) oldItem.Storage?.Name, (object) newItem.Storage?.Name));
        if (!newItem.Items.SequenceEqual<Gbs.Core.Entities.Documents.Item>((IEnumerable<Gbs.Core.Entities.Documents.Item>) oldItem.Items))
        {
          List<string> data;
          ActionsHistoryHelper.CheckList<Gbs.Core.Entities.Documents.Item>((IEnumerable<Gbs.Core.Entities.Documents.Item>) oldItem.Items, (IEnumerable<Gbs.Core.Entities.Documents.Item>) newItem.Items, out data);
          data.RemoveAll((Predicate<string>) (s => s == string.Empty));
          if (data.Any<string>())
          {
            action.Data.Add("\n");
            action.Data.Add(Translate.ActionsHistoryHelper_Изменен_список_товаров);
            action.Data.AddRange((IEnumerable<string>) data);
          }
        }
        if (!newItem.Payments.SequenceEqual<Payments.Payment>((IEnumerable<Payments.Payment>) oldItem.Payments))
        {
          List<string> data;
          ActionsHistoryHelper.CheckList<Payments.Payment>((IEnumerable<Payments.Payment>) oldItem.Payments, (IEnumerable<Payments.Payment>) newItem.Payments, out data);
          data.RemoveAll((Predicate<string>) (s => s == string.Empty));
          if (data.Any<string>())
          {
            action.Data.Add("\n");
            action.Data.Add(Translate.ActionsHistoryHelper_Изменен_список_платежей);
            action.Data.AddRange((IEnumerable<string>) data);
          }
        }
        if (!action.Data.Any<string>())
          return;
        switch (newItem.Type)
        {
          case GlobalDictionaries.DocumentsTypes.Sale:
            break;
          case GlobalDictionaries.DocumentsTypes.Buy:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_ИзмененаНакладная, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.BuyReturn:
            break;
          case GlobalDictionaries.DocumentsTypes.Move:
            break;
          case GlobalDictionaries.DocumentsTypes.MoveReturn:
            break;
          case GlobalDictionaries.DocumentsTypes.WriteOff:
            break;
          case GlobalDictionaries.DocumentsTypes.UserStockEdit:
            break;
          case GlobalDictionaries.DocumentsTypes.Inventory:
            action.Data.Clear();
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_ChangeInventoryAct, (object) newItem.Number));
            break;
          case GlobalDictionaries.DocumentsTypes.InventoryAct:
            break;
          case GlobalDictionaries.DocumentsTypes.CafeOrder:
            break;
          case GlobalDictionaries.DocumentsTypes.ClientOrder:
            action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_CreateDocument_Изменен_заказ_резерв___0_, (object) newItem.Number));
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private static void CreateGood(Gbs.Core.Entities.Goods.Good oldItem, Gbs.Core.Entities.Goods.Good newItem, ref ActionHistory action)
    {
      if (action.ActionType.IsEither<ActionType>(ActionType.Add, ActionType.Delete))
        action.Data.Add(string.Format(Translate.ActionsHistoryHelper_CreateHistory__0__, (object) newItem.Name) + string.Format(Translate.ActionsHistoryHelper_CreateHistory__ID__0__, newItem.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value));
      else if (action.ActionType == ActionType.JoinGood)
      {
        action.Data.Add(string.Format(Translate.ActionsHistoryHelper_CreateGood__0___ID__1__, (object) oldItem.Name, oldItem.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value) + string.Format(Translate.ActionsHistoryHelper_CreateGood__заменили_на__0___ID__1__, (object) newItem.Name, newItem.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value));
      }
      else
      {
        if (oldItem.Name != newItem.Name)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Наименование, (object) oldItem.Name, (object) newItem.Name));
        if (oldItem.Barcode != newItem.Barcode)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ШтрихКод, (object) oldItem.Barcode, (object) newItem.Barcode));
        if (!new HashSet<string>(oldItem.Barcodes).SetEquals(newItem.Barcodes))
          action.Data.Add(Translate.ActionsHistoryHelper_Изменены_доп__штрих_коды);
        if (oldItem.Description != newItem.Description)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Описание, (object) oldItem.Description, (object) newItem.Description));
        if (oldItem.Group.Uid != newItem.Group.Uid)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Категория, (object) oldItem.Group.Name, (object) newItem.Group.Name));
        if (oldItem.SetStatus != newItem.SetStatus)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ТипСоставногоТовара, (object) oldItem.SetStatus, (object) newItem.SetStatus));
        if (!new HashSet<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) oldItem.Properties).SetEquals((IEnumerable<EntityProperties.PropertyValue>) newItem.Properties))
        {
          List<string> data;
          ActionsHistoryHelper.CheckList<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) oldItem.Properties, (IEnumerable<EntityProperties.PropertyValue>) newItem.Properties, out data);
          data.RemoveAll((Predicate<string>) (s => s == string.Empty));
          if (data.Any<string>((Func<string, bool>) (x => !x.IsNullOrEmpty())))
          {
            action.Data.Add("\n");
            action.Data.Add(Translate.ActionsHistoryHelper_Изменены_доп__поля);
            action.Data.AddRange((IEnumerable<string>) data);
          }
        }
        if (!new HashSet<GoodsStocks.GoodStock>((IEnumerable<GoodsStocks.GoodStock>) oldItem.StocksAndPrices).SetEquals((IEnumerable<GoodsStocks.GoodStock>) newItem.StocksAndPrices))
        {
          List<string> data;
          ActionsHistoryHelper.CheckList<GoodsStocks.GoodStock>((IEnumerable<GoodsStocks.GoodStock>) oldItem.StocksAndPrices, (IEnumerable<GoodsStocks.GoodStock>) newItem.StocksAndPrices, out data);
          data.RemoveAll((Predicate<string>) (s => s == string.Empty));
          if (data.Any<string>())
          {
            Decimal num = newItem.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)) - oldItem.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
            action.Data.Add("\n");
            action.Data.Add(Translate.ActionsHistoryHelper_Изменены_остатки + " " + (num > 0M ? "+" + num.ToString("N2") : num.ToString("N2")));
            action.Data.AddRange((IEnumerable<string>) data);
          }
        }
        if (!action.Data.Any<string>())
          return;
        action.Data.Insert(0, Translate.ActionsHistoryHelper_Изменен_товар__ + string.Format(Translate.ActionsHistoryHelper_CreateHistory__0__, (object) newItem.Name) + string.Format(Translate.ActionsHistoryHelper_CreateHistory__ID__0__, newItem.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.GoodIdUid))?.Value));
      }
    }

    private static void CreateGoodGroup(
      GoodGroups.Group oldItem,
      GoodGroups.Group newItem,
      ref ActionHistory action)
    {
      if (action.ActionType.IsEither<ActionType>(ActionType.Add, ActionType.Delete))
      {
        action.Data.Add(newItem.Name);
      }
      else
      {
        if (oldItem.Name != newItem.Name)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Название, (object) oldItem.Name, (object) newItem.Name));
        if (oldItem.DecimalPlace != newItem.DecimalPlace)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_КоличествоЗнаковПослеЗапятой, (object) oldItem.DecimalPlace, (object) newItem.DecimalPlace));
        if (oldItem.GoodsType != newItem.GoodsType)
        {
          List<string> data = action.Data;
          string helperТипТоваров0 = Translate.ActionsHistoryHelper_Тип_товаров___0_;
          KeyValuePair<GlobalDictionaries.GoodTypes, string> keyValuePair = GlobalDictionaries.GoodTypesDictionary().Single<KeyValuePair<GlobalDictionaries.GoodTypes, string>>((Func<KeyValuePair<GlobalDictionaries.GoodTypes, string>, bool>) (x => x.Key == oldItem.GoodsType));
          string str1 = keyValuePair.Value;
          string str2 = string.Format(helperТипТоваров0, (object) str1);
          keyValuePair = GlobalDictionaries.GoodTypesDictionary().Single<KeyValuePair<GlobalDictionaries.GoodTypes, string>>((Func<KeyValuePair<GlobalDictionaries.GoodTypes, string>, bool>) (x => x.Key == newItem.GoodsType));
          string str3 = keyValuePair.Value;
          string str4 = str2 + " => " + str3;
          data.Add(str4);
        }
        if (oldItem.IsDataParent != newItem.IsDataParent)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ИспользоватьСвойствоРодителя, (object) oldItem.IsDataParent, (object) newItem.IsDataParent));
        if (oldItem.IsFreePrice != newItem.IsFreePrice)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_СвободнаяЦена, (object) oldItem.IsFreePrice, (object) newItem.IsFreePrice));
        if (oldItem.IsRequestCount != newItem.IsRequestCount)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ЗапрашиватьКоличество, (object) oldItem.IsRequestCount, (object) newItem.IsRequestCount));
        if (oldItem.NeedComment != newItem.NeedComment)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ЗапрашиватьКОмментарий, (object) oldItem.NeedComment, (object) newItem.NeedComment));
        if (oldItem.KkmSectionNumber != newItem.KkmSectionNumber)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_НомерСекции, (object) oldItem.KkmSectionNumber, (object) newItem.KkmSectionNumber));
        if (oldItem.TaxRateNumber != newItem.TaxRateNumber)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_НДС, (object) oldItem.TaxRateNumber, (object) newItem.TaxRateNumber));
        if (oldItem.ParentGroupUid != newItem.ParentGroupUid)
        {
          using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
          {
            GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
            string str5 = groupsRepository.GetByUid(oldItem.ParentGroupUid)?.Name ?? "";
            string str6 = groupsRepository.GetByUid(newItem.ParentGroupUid)?.Name ?? "";
            action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Родитель, (object) str5, (object) str6));
          }
        }
        if (oldItem.UnitsUid != newItem.UnitsUid)
        {
          string str7 = GoodsUnits.GetByUid(oldItem.UnitsUid)?.FullName ?? "";
          string str8 = GoodsUnits.GetByUid(newItem.UnitsUid)?.FullName ?? "";
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ЕдиницыИзмерения, (object) str7, (object) str8));
        }
        if (oldItem.RuFfdGoodsType != newItem.RuFfdGoodsType)
        {
          string str9 = GlobalDictionaries.RuFfdGoodsTypesDictionary().Single<KeyValuePair<GlobalDictionaries.RuFfdGoodsTypes, string>>((Func<KeyValuePair<GlobalDictionaries.RuFfdGoodsTypes, string>, bool>) (x => x.Key == oldItem.RuFfdGoodsType)).Value;
          string str10 = GlobalDictionaries.RuFfdGoodsTypesDictionary().Single<KeyValuePair<GlobalDictionaries.RuFfdGoodsTypes, string>>((Func<KeyValuePair<GlobalDictionaries.RuFfdGoodsTypes, string>, bool>) (x => x.Key == newItem.RuFfdGoodsType)).Value;
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ТипТовараФфд, (object) str9, (object) str10));
        }
        if (oldItem.RuMarkedProductionType != newItem.RuMarkedProductionType)
        {
          string typeName1 = GlobalDictionaries.MarkedProductionTypesList.Single<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, bool>) (x => x.Type == oldItem.RuMarkedProductionType)).TypeName;
          string typeName2 = GlobalDictionaries.MarkedProductionTypesList.Single<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, bool>) (x => x.Type == newItem.RuMarkedProductionType)).TypeName;
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Маркировка, (object) typeName1, (object) typeName2));
        }
        if (oldItem.RuTaxSystem != newItem.RuTaxSystem)
        {
          string str11 = GlobalDictionaries.RuTaxSystemsDictionary().Single<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>>((Func<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>, bool>) (x => x.Key == oldItem.RuTaxSystem)).Value;
          string str12 = GlobalDictionaries.RuTaxSystemsDictionary().Single<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>>((Func<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>, bool>) (x => x.Key == newItem.RuTaxSystem)).Value;
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_СНО, (object) str11, (object) str12));
        }
        if (!action.Data.Any<string>())
          return;
        action.Data.Insert(0, string.Format(Translate.ActionsHistoryHelper_Изменена_категория_товаров___0_, (object) string.Format(Translate.ActionsHistoryHelper_CreateHistory__0__, (object) newItem.Name)));
      }
    }

    private static void CreateClient(Client oldItem, Client newItem, ref ActionHistory action)
    {
      if (action.ActionType.IsEither<ActionType>(ActionType.Add, ActionType.Delete))
        action.Data.Add(string.Format(Translate.ActionsHistoryHelper_CreateHistory__0__, (object) newItem.Name));
      else if (action.ActionType == ActionType.JoinGood)
      {
        action.Data.Add(string.Format(Translate.ActionsHistoryHelper_CreateClient_Контакт___0___заменили_на___1__, (object) oldItem.Name, (object) newItem.Name));
      }
      else
      {
        if (oldItem.Name != newItem.Name)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ФИО, (object) oldItem.Name, (object) newItem.Name));
        if (oldItem.Barcode != newItem.Barcode)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ШтрихКод, (object) oldItem.Barcode, (object) newItem.Barcode));
        if (oldItem.Address != newItem.Address)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Адрес, (object) oldItem.Address, (object) newItem.Address));
        Guid? uid1 = oldItem.Group?.Uid;
        Guid? uid2 = newItem.Group?.Uid;
        if ((uid1.HasValue == uid2.HasValue ? (uid1.HasValue ? (uid1.GetValueOrDefault() != uid2.GetValueOrDefault() ? 1 : 0) : 0) : 1) != 0)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Группа, (object) oldItem.Group.Name, (object) newItem.Group.Name));
        if (oldItem.Comment != newItem.Comment)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Комментарий, (object) oldItem.Comment, (object) newItem.Comment));
        if (oldItem.Email != newItem.Email)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Емайл, (object) oldItem.Email, (object) newItem.Email));
        if (oldItem.Phone != newItem.Phone)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Телефон, (object) oldItem.Phone, (object) newItem.Phone));
        DateTime? birthday1 = oldItem.Birthday;
        DateTime? birthday2 = newItem.Birthday;
        if ((birthday1.HasValue == birthday2.HasValue ? (birthday1.HasValue ? (birthday1.GetValueOrDefault() != birthday2.GetValueOrDefault() ? 1 : 0) : 0) : 1) != 0)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ДеньРождения, (object) oldItem.Birthday, (object) newItem.Birthday));
        if (!newItem.Properties.SequenceEqual<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) oldItem.Properties))
        {
          List<string> data;
          ActionsHistoryHelper.CheckList<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) oldItem.Properties, (IEnumerable<EntityProperties.PropertyValue>) newItem.Properties, out data);
          data.RemoveAll((Predicate<string>) (s => s == string.Empty));
          if (data.Any<string>())
          {
            action.Data.Add("\n");
            action.Data.Add(Translate.ActionsHistoryHelper_Изменены_доп__поля);
            action.Data.AddRange((IEnumerable<string>) data);
          }
        }
        if (!action.Data.Any<string>())
          return;
        action.Data.Insert(0, Translate.ActionsHistoryHelper_Изменен_контакт__ + string.Format(Translate.ActionsHistoryHelper_CreateHistory__0__, (object) newItem.Name));
      }
    }

    private static void CreateClientGroup(Gbs.Core.Entities.Clients.Group oldItem, Gbs.Core.Entities.Clients.Group newItem, ref ActionHistory action)
    {
      if (action.ActionType.IsEither<ActionType>(ActionType.Add, ActionType.Delete))
      {
        action.Data.Add(string.Format(Translate.ActionsHistoryHelper_CreateHistory__0__, (object) newItem.Name));
      }
      else
      {
        if (oldItem.Name != newItem.Name)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Название, (object) oldItem.Name, (object) newItem.Name));
        if (oldItem.Discount != newItem.Discount)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Скидка, (object) oldItem.Discount, (object) newItem.Discount));
        if (oldItem.IsNonUseBonus != newItem.IsNonUseBonus)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ЗапретНаИспользованиеБонусов, (object) oldItem.IsNonUseBonus, (object) newItem.IsNonUseBonus));
        if (oldItem.IsSupplier != newItem.IsSupplier)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ГруппаПоставщик, (object) oldItem.IsSupplier, (object) newItem.IsSupplier));
        Decimal? maxSumCredit1 = oldItem.MaxSumCredit;
        Decimal? maxSumCredit2 = newItem.MaxSumCredit;
        if (!(maxSumCredit1.GetValueOrDefault() == maxSumCredit2.GetValueOrDefault() & maxSumCredit1.HasValue == maxSumCredit2.HasValue))
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_МаксСуммаДолга, (object) oldItem.MaxSumCredit, (object) newItem.MaxSumCredit));
        if (oldItem.Price.Uid != newItem.Price.Uid)
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_ДопЦЕна, (object) oldItem.Price.Name, (object) newItem.Price.Name));
        if (!action.Data.Any<string>())
          return;
        action.Data.Insert(0, Translate.ActionsHistoryHelper_Изменена_группа_контактов__ + string.Format(Translate.ActionsHistoryHelper_CreateHistory__0__, (object) newItem.Name));
      }
    }

    private static void CreatePayment(
      Payments.Payment oldItem,
      Payments.Payment newItem,
      ref ActionHistory action)
    {
      if (!action.ActionType.IsEither<ActionType>(ActionType.Delete))
        return;
      if (newItem.Type == GlobalDictionaries.PaymentTypes.MoneyDocumentPayment)
      {
        using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          Document byUid = new DocumentsRepository(dataBase).GetByUid(newItem.ParentUid);
          if (byUid == null)
            return;
          action.Data.Add(string.Format(Translate.ActionsHistoryHelper_УдаленПЛатежКДокументу, (object) byUid.Number, (object) newItem.SumIn));
        }
      }
      else
      {
        DateTime date1;
        if (newItem.SumIn != 0M && newItem.SumOut == 0M)
        {
          List<string> data = action.Data;
          string сумму0N2От1DdMmYyyy = Translate.ActionsHistoryHelper_Удалено_внесение_на_сумму__0_N2__от__1_dd_MM_yyyy_;
          // ISSUE: variable of a boxed type
          __Boxed<Decimal> sumIn = (ValueType) newItem.SumIn;
          date1 = newItem.Date;
          // ISSUE: variable of a boxed type
          __Boxed<DateTime> date2 = (ValueType) date1.Date;
          string str = string.Format(сумму0N2От1DdMmYyyy, (object) sumIn, (object) date2);
          data.Add(str);
        }
        if (newItem.SumIn == 0M && newItem.SumOut != 0M)
        {
          List<string> data = action.Data;
          string сумму0N2От1DdMmYyyy = Translate.ActionsHistoryHelper_Удалено_снятие_на_сумму__0_N2__от__1_dd_MM_yyyy_;
          // ISSUE: variable of a boxed type
          __Boxed<Decimal> sumOut = (ValueType) newItem.SumOut;
          date1 = newItem.Date;
          // ISSUE: variable of a boxed type
          __Boxed<DateTime> date3 = (ValueType) date1.Date;
          string str = string.Format(сумму0N2От1DdMmYyyy, (object) sumOut, (object) date3);
          data.Add(str);
        }
        if (!(newItem.SumIn != 0M) || !(newItem.SumOut != 0M) || newItem.Type != GlobalDictionaries.PaymentTypes.MoneyMovement)
          return;
        List<string> data1 = action.Data;
        string сумму0N2От1DdMmYyyy1 = Translate.ActionsHistoryHelper_Удалено_перемещение_на_сумму__0_N2__от__1_dd_MM_yyyy_;
        // ISSUE: variable of a boxed type
        __Boxed<Decimal> sumOut1 = (ValueType) newItem.SumOut;
        date1 = newItem.Date;
        // ISSUE: variable of a boxed type
        __Boxed<DateTime> date4 = (ValueType) date1.Date;
        string str1 = string.Format(сумму0N2От1DdMmYyyy1, (object) sumOut1, (object) date4);
        data1.Add(str1);
      }
    }

    private static void CreateItemList(
      BasketItem oldItem,
      BasketItem newItem,
      ref ActionHistory action)
    {
      if (!action.ActionType.IsEither<ActionType>(ActionType.Delete))
        return;
      action.Data.Add(string.Format(Translate.ActionsHistoryHelper_Из_корзины_удален_товар___0_, (object) newItem.DisplayedName));
      action.Data.Add(Translate.ActionsHistoryHelper_CreateItemList_Количество__ + newItem.Quantity.ToString("N2"));
      action.Data.Add(Translate.ActionsHistoryHelper_CreateItemList_Скидка__ + newItem.Discount.Value.ToString("N") + "%");
      if (newItem.Comment.IsNullOrEmpty())
        return;
      action.Data.Add(string.Format(Translate.JournalItem_Причина___0_, (object) newItem.Comment));
    }

    private static void CheckList<T>(
      IEnumerable<T> oldItems,
      IEnumerable<T> newItems,
      out List<string> data)
    {
      data = new List<string>();
      List<Entity> list = oldItems.Cast<Entity>().ToList<Entity>();
      List<Entity> newItemsEntity = newItems.Cast<Entity>().ToList<Entity>();
      foreach (Entity entity1 in newItemsEntity)
      {
        Entity item = entity1;
        Entity entity2 = list.FirstOrDefault<Entity>((Func<Entity, bool>) (x => x.Uid == item.Uid));
        if (item is GoodsStocks.GoodStock newStock)
          data.Add(GoodStockEdit(newStock, (GoodsStocks.GoodStock) entity2));
        else
          data.Add(ObjectEdit(item, entity2));
      }

      string GoodStockEdit(GoodsStocks.GoodStock newStock, GoodsStocks.GoodStock oldStock)
      {
        int num = newItemsEntity.FindIndex((Predicate<Entity>) (x => x.Uid == newStock.Uid)) + 1;
        if (oldStock == null)
          return string.Format(Translate.ActionsHistoryHelper_CheckList_Добавлен_остаток__количество__0___цена__1___склад__2_, (object) newStock.Stock.ToString("N2"), (object) newStock.Price.ToString("N2"), (object) newStock.Storage.Name, (object) num);
        if (Functions.IsObjectEqual<GoodsStocks.GoodStock>(newStock, oldStock))
          return "";
        if (newStock.IsDeleted && !oldStock.IsDeleted)
          return string.Format(Translate.ActionsHistoryHelper_CheckList_Удален_остаток__количество__0___цена__1___склад__2_, (object) newStock.Stock.ToString("N2"), (object) newStock.Price.ToString("N2"), (object) newStock.Storage.Name, (object) num);
        string str = string.Format(Translate.ActionsHistoryHelper_Изменен_остаток___0___, (object) num);
        if (oldStock.Stock != newStock.Stock)
          str += string.Format(Translate.ActionsHistoryHelper_количество__0_N2______1_N2__, (object) oldStock.Stock, (object) newStock.Stock);
        if (oldStock.Price != newStock.Price)
          str += string.Format(Translate.ActionsHistoryHelper_цена__0_N2______1_N2__, (object) oldStock.Price, (object) newStock.Price);
        if (oldStock.Storage.Uid != newStock.Storage.Uid)
          str += string.Format(Translate.ActionsHistoryHelper___склад__0______1_, (object) oldStock.Storage.Name, (object) newStock.Storage.Name);
        return str;
      }

      string ObjectEdit(Entity item, Entity old)
      {
        int num = newItemsEntity.FindIndex((Predicate<Entity>) (x => x.Uid == item.Uid)) + 1;
        if (old == null)
          return string.Format(Translate.ActionsHistoryHelper_ДобавленаСтрока, (object) num);
        if (Functions.IsObjectEqual<Entity>(item, old))
          return "";
        return item.IsDeleted && !old.IsDeleted ? string.Format(Translate.ActionsHistoryHelper_УдаленаСтрока, (object) num) : string.Format(Translate.ActionsHistoryHelper_ИзмененаСтрока, (object) num);
      }
    }
  }
}
