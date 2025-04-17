// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ExternalApi.PlanfixHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.Goods;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Planfix.Api;
using Planfix.Api.Entities;
using Planfix.Api.Entities.Action;
using Planfix.Api.Entities.Analitics;
using Planfix.Api.Entities.Contacts;
using Planfix.Api.Entities.Handbooks;
using Planfix.Api.Entities.Task;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.ExternalApi
{
  public static class PlanfixHelper
  {
    public static Contact GbsContactToPlanfixContact(Client client, PlanfixSetting setting)
    {
      Contact planfixContact = new Contact()
      {
        email = client.Email,
        address = client.Address,
        description = client.Comment,
        group = new Planfix.Api.Entities.ContactGroups.Group() { id = setting.ContactGroupId }
      };
      planfixContact.phones.Add(new phone()
      {
        number = client.Phone
      });
      string[] strArray = client.Name.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length == 3)
      {
        planfixContact.lastName = strArray[0];
        planfixContact.name = strArray[1];
        planfixContact.midName = strArray[2];
      }
      else
        planfixContact.name = client.Name;
      return planfixContact;
    }

    public static (int count, int countOld) UpdateGoodPf(
      List<Gbs.Core.Entities.Goods.Good> goods,
      PlanfixSetting planfixSetting,
      bool isShowProgressBar = false)
    {
      try
      {
        if (!planfixSetting.IsActive)
          return (0, 0);
        LogHelper.Debug("Передаем товары в ПФ = " + goods.Count.ToString());
        List<LinkEntities> list = new LinkEntitiesRepository().GetAllItems().Where<LinkEntities>((Func<LinkEntities, bool>) (x => x.Type == TypeEntity.Good)).ToList<LinkEntities>();
        if (!goods.Any<Gbs.Core.Entities.Goods.Good>())
          return (0, 0);
        ProgressBarHelper.ProgressBar progressBar = (ProgressBarHelper.ProgressBar) null;
        if (isShowProgressBar)
          progressBar = new ProgressBarHelper.ProgressBar(Translate.GoodsCatalogModelView_Выгрузка_товаров_в_ПланФикс);
        HandbookRepository handbookRepository1 = new HandbookRepository();
        TaskRepository taskRepository = new TaskRepository();
        int num1 = 0;
        int num2 = 0;
        ConfigManager.Config = new Planfix.Api.Config(planfixSetting.AccountName, planfixSetting.ApiUrl, planfixSetting.DecryptedKeyApi, planfixSetting.DecryptedToken);
        foreach (Gbs.Core.Entities.Goods.Good good1 in goods)
        {
          Gbs.Core.Entities.Goods.Good good = good1;
          (Planfix.Api.Answer, int) valueTuple;
          if (planfixSetting.GoodEntityType == PlanfixSetting.GoodsEntityTypes.Handbook)
          {
            int num3 = PlanfixHelper.UpdateGoodGroupPfHandbook(good.Group, planfixSetting);
            Planfix.Api.Entities.CustomData planfixGood = PlanfixHelper.GbsGoodToPlanfixGood(good, planfixSetting);
            if (list.Any<LinkEntities>((Func<LinkEntities, bool>) (x => x.EntityUid == good.Uid)))
            {
              HandbookRepository handbookRepository2 = handbookRepository1;
              Handbook handbook = new Handbook();
              handbook.id = planfixSetting.HandbookGood.Id;
              Planfix.Api.Entities.CustomData data = planfixGood;
              int id = list.First<LinkEntities>((Func<LinkEntities, bool>) (x => x.EntityUid == good.Uid)).Id;
              int idGroup = num3;
              Planfix.Api.Answer answer = handbookRepository2.UpdateItemForHandbook(handbook, data, id, idGroup).answer;
              if (answer.Result == Planfix.Api.Answer.ResultTypes.Ok)
              {
                ++num2;
                continue;
              }
              if (answer.ErrorCode != Planfix.Api.Answer.ErrorCodes.WrongApiKey)
                continue;
              break;
            }
            HandbookRepository handbookRepository3 = handbookRepository1;
            Handbook handbook1 = new Handbook();
            handbook1.id = planfixSetting.HandbookGood.Id;
            Planfix.Api.Entities.CustomData data1 = planfixGood;
            int idGroup1 = num3;
            valueTuple = handbookRepository3.AddItemForHandbook(handbook1, data1, idGroup1);
          }
          else
          {
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
              int num4 = PlanfixHelper.UpdateGoodGroupPfTask(good.Group, groupsRepository.GetByUid(good.Group.ParentGroupUid), planfixSetting);
              Planfix.Api.Entities.CustomData customDataForTask = PlanfixHelper.GetCustomDataForTask(good, num4.ToString(), planfixSetting);
              TaskAdd taskAdd = new TaskAdd();
              TaskGood templateGoodAsTask = planfixSetting.TemplateGoodAsTask;
              taskAdd.Template = templateGoodAsTask != null ? templateGoodAsTask.Id : 0;
              taskAdd.Title = good.Name;
              taskAdd.CustomData = customDataForTask;
              taskAdd.Status = 6;
              taskAdd.Description = good.Description;
              TaskAdd entity = taskAdd;
              if (list.Any<LinkEntities>((Func<LinkEntities, bool>) (x => x.EntityUid == good.Uid)))
              {
                entity.Id = list.First<LinkEntities>((Func<LinkEntities, bool>) (x => x.EntityUid == good.Uid)).Id;
                Planfix.Api.Answer answer = taskRepository.Update(entity);
                if (answer.Result == Planfix.Api.Answer.ResultTypes.Ok)
                {
                  ++num2;
                  continue;
                }
                if (answer.ErrorCode != Planfix.Api.Answer.ErrorCodes.WrongApiKey)
                  continue;
                break;
              }
              valueTuple = (taskRepository.Add(ref entity), entity.Id);
            }
          }
          if (valueTuple.Item1.Result == Planfix.Api.Answer.ResultTypes.Ok)
          {
            ++num1;
            new LinkEntitiesRepository().Save(new LinkEntities()
            {
              EntityUid = good.Uid,
              Type = TypeEntity.Good,
              Id = valueTuple.Item2
            });
          }
          else if (valueTuple.Item1.ErrorCode == Planfix.Api.Answer.ErrorCodes.WrongApiKey)
            break;
        }
        progressBar?.Close();
        return (num1, num2);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при выгрузке товаров в ПФ");
        return (0, 0);
      }
    }

    public static int UpdateGoodGroupPfTask(
      GoodGroups.Group group,
      GoodGroups.Group parentGroup,
      PlanfixSetting planfixSetting)
    {
      try
      {
        if (!planfixSetting.IsActive)
          return 0;
        LinkEntities byUid1 = new LinkEntitiesRepository().GetByUid(group.Uid);
        // ISSUE: explicit non-virtual call
        LinkEntities byUid2 = new LinkEntitiesRepository().GetByUid(parentGroup != null ? __nonvirtual (parentGroup.Uid) : Guid.Empty);
        ConfigManager.Config = new Planfix.Api.Config(planfixSetting.AccountName, planfixSetting.ApiUrl, planfixSetting.DecryptedKeyApi, planfixSetting.DecryptedToken);
        TaskRepository importTask = new TaskRepository();
        int parentId = 0;
        if (parentGroup != null)
          parentId = GetGroupPf(parentGroup, byUid2, importTask);
        return GetGroupPf(group, byUid1, importTask, parentId);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при выгрузке категорий товаров в ПФ");
        return 0;
      }

      int GetGroupPf(
        GoodGroups.Group groupMarket,
        LinkEntities linkEntities,
        TaskRepository importTask,
        int parentId = 0)
      {
        if (linkEntities != null)
        {
          importTask.Update(new TaskAdd()
          {
            Id = linkEntities.Id,
            Title = groupMarket.Name,
            Template = planfixSetting.TemplateGoodsGroupsAsTaskId,
            Status = 6
          });
          return linkEntities.Id;
        }
        TaskAdd entity = new TaskAdd()
        {
          Template = planfixSetting.TemplateGoodsGroupsAsTaskId,
          Title = groupMarket.Name,
          Status = 6
        };
        if (parentId != 0)
          entity.Parent = new Parent() { Id = parentId };
        importTask.Add(ref entity);
        new LinkEntitiesRepository().Save(new LinkEntities()
        {
          EntityUid = groupMarket.Uid,
          Type = TypeEntity.GoodGroups,
          Id = entity.Id
        });
        return entity.Id;
      }
    }

    public static int UpdateGoodGroupPfHandbook(
      GoodGroups.Group group,
      PlanfixSetting planfixSetting)
    {
      try
      {
        if (!planfixSetting.IsActive)
          return 0;
        LinkEntities byUid1 = new LinkEntitiesRepository().GetByUid(group.Uid);
        ConfigManager.Config = new Planfix.Api.Config(planfixSetting.AccountName, planfixSetting.ApiUrl, planfixSetting.DecryptedKeyApi, planfixSetting.DecryptedToken);
        HandbookRepository importHandbook = new HandbookRepository();
        int parentId = 0;
        if (group.ParentGroupUid != Guid.Empty)
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
            LinkEntities byUid2 = new LinkEntitiesRepository().GetByUid(group.ParentGroupUid);
            parentId = byUid2 != null ? byUid2.Id : GetGroupPf(groupsRepository.GetByUid(group.ParentGroupUid), (LinkEntities) null, importHandbook);
          }
        }
        return GetGroupPf(group, byUid1, importHandbook, parentId);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при выгрузке категорий товаров в ПФ");
        return 0;
      }

      int GetGroupPf(
        GoodGroups.Group groupMarket,
        LinkEntities linkEntities,
        HandbookRepository importHandbook,
        int parentId = 0)
      {
        if (groupMarket.ParentGroupUid != Guid.Empty)
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
            LinkEntities byUid = new LinkEntitiesRepository().GetByUid(groupMarket.ParentGroupUid);
            parentId = byUid != null ? byUid.Id : GetGroupPf(groupsRepository.GetByUid(groupMarket.ParentGroupUid), (LinkEntities) null, importHandbook);
          }
        }
        if (linkEntities != null)
        {
          HandbookRepository handbookRepository = importHandbook;
          Handbook handbook = new Handbook();
          handbook.id = planfixSetting.HandbookGood.Id;
          int id = linkEntities.Id;
          int idGroup = parentId;
          string name = groupMarket.Name;
          handbookRepository.UpdateItemForHandbook(handbook, (Planfix.Api.Entities.CustomData) null, id, idGroup, true, name);
          return linkEntities.Id;
        }
        HandbookRepository handbookRepository1 = importHandbook;
        Handbook handbook1 = new Handbook();
        handbook1.id = planfixSetting.HandbookGood.Id;
        int idGroup1 = parentId;
        string name1 = groupMarket.Name;
        (Planfix.Api.Answer answer, int key) tuple = handbookRepository1.AddItemForHandbook(handbook1, (Planfix.Api.Entities.CustomData) null, idGroup1, true, name1);
        new LinkEntitiesRepository().Save(new LinkEntities()
        {
          EntityUid = groupMarket.Uid,
          Type = TypeEntity.GoodGroups,
          Id = tuple.key
        });
        return tuple.key;
      }
    }

    public static Planfix.Api.Entities.CustomData GbsGoodToPlanfixGood(
      GoodsCatalogModelView.GoodsInfoGrid goodInfo,
      PlanfixSetting setting)
    {
      return PlanfixHelper.GetCustomData(goodInfo, setting);
    }

    private static Planfix.Api.Entities.CustomData GetCustomData(
      GoodsCatalogModelView.GoodsInfoGrid goodInfo,
      PlanfixSetting setting)
    {
      Planfix.Api.Entities.CustomData customData = new Planfix.Api.Entities.CustomData();
      List<Planfix.Api.Entities.CustomValue> customValueList = new List<Planfix.Api.Entities.CustomValue>();
      customValueList.Add(new Planfix.Api.Entities.CustomValue()
      {
        Id = setting.HandbookGood.NameId,
        Value = goodInfo.Good.Name
      });
      customValueList.Add(new Planfix.Api.Entities.CustomValue()
      {
        Id = setting.HandbookGood.BarcodeId,
        Value = goodInfo.Good.Barcode
      });
      Planfix.Api.Entities.CustomValue customValue1 = new Planfix.Api.Entities.CustomValue();
      customValue1.Id = setting.HandbookGood.PriceId;
      Decimal? nullable = goodInfo.MaxPrice;
      ref Decimal? local1 = ref nullable;
      Decimal valueOrDefault;
      string str1;
      if (!local1.HasValue)
      {
        str1 = (string) null;
      }
      else
      {
        valueOrDefault = local1.GetValueOrDefault();
        str1 = valueOrDefault.ToString();
      }
      if (str1 == null)
        str1 = "0";
      customValue1.Value = str1;
      customValueList.Add(customValue1);
      Planfix.Api.Entities.CustomValue customValue2 = new Planfix.Api.Entities.CustomValue();
      customValue2.Id = setting.HandbookGood.QuantityId;
      nullable = goodInfo.GoodTotalStock;
      ref Decimal? local2 = ref nullable;
      string str2;
      if (!local2.HasValue)
      {
        str2 = (string) null;
      }
      else
      {
        valueOrDefault = local2.GetValueOrDefault();
        str2 = valueOrDefault.ToString();
      }
      if (str2 == null)
        str2 = "0";
      customValue2.Value = str2;
      customValueList.Add(customValue2);
      customData.CustomValue = customValueList;
      return customData;
    }

    public static Planfix.Api.Entities.CustomData GetCustomDataForTask(
      Gbs.Core.Entities.Goods.Good goodInfo,
      string idGroup,
      PlanfixSetting setting)
    {
      Planfix.Api.Entities.CustomData customDataForTask = new Planfix.Api.Entities.CustomData();
      List<Planfix.Api.Entities.CustomValue> customValueList = new List<Planfix.Api.Entities.CustomValue>();
      customValueList.Add(new Planfix.Api.Entities.CustomValue()
      {
        Id = setting.TemplateGoodAsTask.BarcodeId,
        Value = goodInfo.Barcode
      });
      customValueList.Add(new Planfix.Api.Entities.CustomValue()
      {
        Id = setting.TemplateGoodAsTask.GroupNameId,
        Value = idGroup
      });
      Planfix.Api.Entities.CustomValue customValue1 = new Planfix.Api.Entities.CustomValue();
      customValue1.Id = setting.TemplateGoodAsTask.PriceId;
      Decimal num;
      string str1;
      if (!goodInfo.StocksAndPrices.Any<GoodsStocks.GoodStock>())
      {
        str1 = "0";
      }
      else
      {
        num = goodInfo.StocksAndPrices.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price));
        str1 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture) ?? "0";
      }
      customValue1.Value = str1;
      customValueList.Add(customValue1);
      Planfix.Api.Entities.CustomValue customValue2 = new Planfix.Api.Entities.CustomValue();
      customValue2.Id = setting.TemplateGoodAsTask.QuantityId;
      string str2;
      if (!goodInfo.StocksAndPrices.Any<GoodsStocks.GoodStock>())
      {
        str2 = "0";
      }
      else
      {
        num = goodInfo.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
        str2 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture) ?? "0";
      }
      customValue2.Value = str2;
      customValueList.Add(customValue2);
      Planfix.Api.Entities.CustomValue customValue3 = new Planfix.Api.Entities.CustomValue();
      customValue3.Id = setting.TemplateGoodAsTask.BuyPriceId;
      num = new BuyPriceCounter().GetLastBuyPrice(goodInfo);
      customValue3.Value = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      customValueList.Add(customValue3);
      customDataForTask.CustomValue = customValueList;
      return customDataForTask;
    }

    public static Planfix.Api.Entities.CustomData GbsGoodToPlanfixGood(
      Gbs.Core.Entities.Goods.Good good,
      PlanfixSetting setting)
    {
      SalePriceType salePriceType = new ConfigsRepository<Settings>().Get().GoodsConfig.SalePriceType;
      return PlanfixHelper.GetCustomData(new GoodsCatalogModelView.GoodsInfoGrid()
      {
        Good = good,
        GoodTotalStock = new Decimal?(good.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock))),
        MaxPrice = SaleHelper.GetSalePriceForGood(good, salePriceType)
      }, setting);
    }

    public static Planfix.Api.Entities.CustomData GbsGroupToPlanfixGood(
      GoodGroups.Group group,
      PlanfixSetting setting)
    {
      return new Planfix.Api.Entities.CustomData()
      {
        CustomValue = new List<Planfix.Api.Entities.CustomValue>()
        {
          new Planfix.Api.Entities.CustomValue()
          {
            Id = setting.HandbookGood.NameId,
            Value = group.Name
          }
        }
      };
    }

    public static class AnaliticHelper
    {
      public static void AddReturnAnalitic(Document returnDoc, Document saleDoc)
      {
        LinkEntities byUid = new LinkEntitiesRepository().GetByUid(saleDoc.Uid);
        if (byUid == null)
        {
          new LinkEntitiesRepository().Save(new LinkEntities()
          {
            EntityUid = returnDoc.Uid,
            Type = TypeEntity.ReturnAnalitic
          });
        }
        else
        {
          PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
          if (!planfix.IsSaveSale || !planfix.IsActive)
            return;
          Planfix.Api.Entities.Action.Action action = new Planfix.Api.Entities.Action.Action();
          action.Description = string.Format(Translate.AnaliticHelper_Возврат_продаж__причина___0_, (object) returnDoc.Comment);
          Planfix.Api.Entities.Task.Task task = new Planfix.Api.Entities.Task.Task();
          task.Id = byUid.Id;
          action.Task = task;
          action.Analitics = new Planfix.Api.Entities.Analitics.Analitics()
          {
            Analitic = new List<Analitic>()
          };
          Planfix.Api.Entities.Action.Action entity = action;
          foreach (Gbs.Core.Entities.Payments.Payment payment in returnDoc.Payments)
          {
            Analitic analitic = new Analitic()
            {
              Id = planfix.PaymentsAnalitic.Id,
              AnaliticData = new AnaliticData()
              {
                ItemData = new List<ItemData>()
              }
            };
            analitic.AnaliticData.ItemData.Add(new ItemData(planfix.PaymentsAnalitic.PaymentNameId, (object) (payment.Method?.Name ?? Translate.DocumentViewModel_не_указан)));
            analitic.AnaliticData.ItemData.Add(new ItemData(planfix.PaymentsAnalitic.SumId, (object) -payment.SumOut));
            entity.Analitics.Analitic.Add(analitic);
          }
          foreach (Gbs.Core.Entities.Documents.Item obj in returnDoc.Items)
          {
            LinkEntities linkEntities = new LinkEntitiesRepository().GetByUid(obj.GoodUid);
            Analitic analitic = new Analitic()
            {
              Id = planfix.SaleAnalitic.Id,
              AnaliticData = new AnaliticData()
              {
                ItemData = new List<ItemData>()
              }
            };
            analitic.AnaliticData.ItemData.Add(new ItemData(planfix.SaleAnalitic.CommentId, (object) obj.Comment));
            analitic.AnaliticData.ItemData.Add(new ItemData(planfix.SaleAnalitic.PriceId, (object) obj.SellPrice));
            analitic.AnaliticData.ItemData.Add(new ItemData(planfix.SaleAnalitic.QuantityId, (object) -obj.Quantity));
            analitic.AnaliticData.ItemData.Add(new ItemData(planfix.SaleAnalitic.DiscountId, (object) obj.Discount));
            if (linkEntities == null)
            {
              Planfix.Api.Entities.CustomData planfixGood = PlanfixHelper.GbsGoodToPlanfixGood(obj.Good, planfix);
              HandbookRepository handbookRepository = new HandbookRepository();
              Handbook handbook = new Handbook();
              handbook.id = planfix.HandbookGood.Id;
              Planfix.Api.Entities.CustomData data = planfixGood;
              (Planfix.Api.Answer answer, int key) tuple = handbookRepository.AddItemForHandbook(handbook, data);
              linkEntities = new LinkEntities()
              {
                EntityUid = obj.GoodUid,
                Type = TypeEntity.Good,
                Id = tuple.key
              };
              new LinkEntitiesRepository().Save(linkEntities);
            }
            analitic.AnaliticData.ItemData.Add(new ItemData(planfix.SaleAnalitic.GoodHandbook, (object) linkEntities.Id));
            entity.Analitics.Analitic.Add(analitic);
          }
          if (new ActionRepository().Add(ref entity).Result == Planfix.Api.Answer.ResultTypes.Error)
            new LinkEntitiesRepository().Save(new LinkEntities()
            {
              EntityUid = returnDoc.Uid,
              Type = TypeEntity.ReturnAnalitic
            });
          else
            new LinkEntitiesRepository().Delete(new LinkEntities()
            {
              EntityUid = returnDoc.Uid
            });
        }
      }

      public static void AddPaymentCreditAnalitic(Document doc, List<Gbs.Core.Entities.Payments.Payment> payments)
      {
        LinkEntities byUid = new LinkEntitiesRepository().GetByUid(doc.Uid);
        if (byUid == null)
          return;
        PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
        if (!planfix.IsSaveSale || !planfix.IsActive)
          return;
        Planfix.Api.Entities.Action.Action action = new Planfix.Api.Entities.Action.Action();
        action.Description = Translate.AnaliticHelper_Аналитика_по_внесенным_платежам_за_кредит;
        Planfix.Api.Entities.Task.Task task = new Planfix.Api.Entities.Task.Task();
        task.Id = byUid.Id;
        action.Task = task;
        action.Analitics = new Planfix.Api.Entities.Analitics.Analitics()
        {
          Analitic = new List<Analitic>()
        };
        Planfix.Api.Entities.Action.Action entity = action;
        foreach (Gbs.Core.Entities.Payments.Payment payment in payments)
        {
          Analitic analitic = new Analitic()
          {
            Id = planfix.PaymentsAnalitic.Id,
            AnaliticData = new AnaliticData()
            {
              ItemData = new List<ItemData>()
            }
          };
          analitic.AnaliticData.ItemData.Add(new ItemData(planfix.PaymentsAnalitic.PaymentNameId, (object) (payment.Method?.Name ?? Translate.DocumentViewModel_не_указан)));
          analitic.AnaliticData.ItemData.Add(new ItemData(planfix.PaymentsAnalitic.SumId, (object) payment.SumIn));
          entity.Analitics.Analitic.Add(analitic);
        }
        Planfix.Api.Answer answer = new ActionRepository().Add(ref entity);
        foreach (Gbs.Core.Entities.Payments.Payment payment in payments)
        {
          if (answer.Result == Planfix.Api.Answer.ResultTypes.Error)
            new LinkEntitiesRepository().Save(new LinkEntities()
            {
              EntityUid = payment.Uid,
              Type = TypeEntity.Payment
            });
          else
            new LinkEntitiesRepository().Delete(new LinkEntities()
            {
              EntityUid = payment.Uid
            });
        }
      }
    }
  }
}
