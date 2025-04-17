// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.PlanfixApi.GbsRepository.RequestSaleSave
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Helpers.ExternalApi;
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
using System.Linq;

#nullable disable
namespace Gbs.Helpers.PlanfixApi.GbsRepository
{
  public static class RequestSaleSave
  {
    private static List<LinkEntities> ListLinq { get; set; } = new LinkEntitiesRepository().GetAllItems().ToList<LinkEntities>();

    private static PlanfixSetting PlanfixSetting
    {
      get => new ConfigsRepository<Integrations>().Get().Planfix;
    }

    public static void SaveSaleInPf(Document document, bool isVisibilityError = true)
    {
      try
      {
        PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
        ConfigManager.Config = new Planfix.Api.Config(planfix.AccountName, planfix.ApiUrl, planfix.DecryptedKeyApi, planfix.DecryptedToken);
        TaskAdd entity1 = new TaskAdd()
        {
          Title = Translate.FrmMainWindow_Продажа + document.Number,
          Description = document.Comment,
          Client = RequestSaleSave.GetClientPf(document.ContractorUid, document),
          Status = 6,
          BeginDateTime = document.DateTime.ToString("dd-MM-yyyy HH:MM"),
          Project = new Project()
          {
            Id = RequestSaleSave.PlanfixSetting.ProjectId
          },
          Template = RequestSaleSave.PlanfixSetting.TemplateTaskId,
          Workers = new Workers()
          {
            Users = new List<Planfix.Api.Entities.User>()
            {
              RequestSaleSave.GetUserPf(document.UserUid, document)
            }
          }
        };
        Planfix.Api.Answer answer = new TaskRepository().Add(ref entity1);
        Planfix.Api.Entities.Action.Action action = new Planfix.Api.Entities.Action.Action();
        action.Description = Translate.RequestSaleSave_SaveSaleInPf_Аналитика_по_проданным_товарам_и_платежам;
        Planfix.Api.Entities.Task.Task task = new Planfix.Api.Entities.Task.Task();
        task.Id = entity1.Id;
        action.Task = task;
        action.Analitics = new Planfix.Api.Entities.Analitics.Analitics()
        {
          Analitic = new List<Analitic>()
        };
        Planfix.Api.Entities.Action.Action entity2 = action;
        foreach (Gbs.Core.Entities.Documents.Item obj in document.Items)
        {
          LinkEntities linkEntities = new LinkEntitiesRepository().GetByUid(obj.GoodUid);
          Analitic analitic = new Analitic()
          {
            Id = RequestSaleSave.PlanfixSetting.SaleAnalitic.Id,
            AnaliticData = new AnaliticData()
            {
              ItemData = new List<ItemData>()
            }
          };
          analitic.AnaliticData.ItemData.Add(new ItemData(RequestSaleSave.PlanfixSetting.SaleAnalitic.CommentId, (object) obj.Comment));
          analitic.AnaliticData.ItemData.Add(new ItemData(RequestSaleSave.PlanfixSetting.SaleAnalitic.PriceId, (object) obj.SellPrice));
          analitic.AnaliticData.ItemData.Add(new ItemData(RequestSaleSave.PlanfixSetting.SaleAnalitic.QuantityId, (object) obj.Quantity));
          analitic.AnaliticData.ItemData.Add(new ItemData(RequestSaleSave.PlanfixSetting.SaleAnalitic.DiscountId, (object) obj.Discount));
          if (linkEntities == null)
          {
            Planfix.Api.Entities.CustomData planfixGood = PlanfixHelper.GbsGoodToPlanfixGood(obj.Good, RequestSaleSave.PlanfixSetting);
            HandbookRepository handbookRepository = new HandbookRepository();
            Handbook handbook = new Handbook();
            handbook.id = RequestSaleSave.PlanfixSetting.HandbookGood.Id;
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
          analitic.AnaliticData.ItemData.Add(new ItemData(RequestSaleSave.PlanfixSetting.SaleAnalitic.GoodHandbook, (object) linkEntities.Id));
          entity2.Analitics.Analitic.Add(analitic);
        }
        foreach (Gbs.Core.Entities.Payments.Payment payment in document.Payments)
        {
          Analitic analitic = new Analitic()
          {
            Id = RequestSaleSave.PlanfixSetting.PaymentsAnalitic.Id,
            AnaliticData = new AnaliticData()
            {
              ItemData = new List<ItemData>()
            }
          };
          analitic.AnaliticData.ItemData.Add(new ItemData(RequestSaleSave.PlanfixSetting.PaymentsAnalitic.PaymentNameId, (object) (payment.Method?.Name ?? Translate.DocumentViewModel_не_указан)));
          analitic.AnaliticData.ItemData.Add(new ItemData(RequestSaleSave.PlanfixSetting.PaymentsAnalitic.SumId, (object) payment.SumIn));
          entity2.Analitics.Analitic.Add(analitic);
        }
        new ActionRepository().Add(ref entity2);
        if (answer.Result != Planfix.Api.Answer.ResultTypes.Ok)
          return;
        LinkEntities linkEntities1 = new LinkEntities()
        {
          Type = TypeEntity.Document,
          EntityUid = document.Uid,
          Id = entity1.Id
        };
        new LinkEntitiesRepository().Save(linkEntities1);
        RequestSaleSave.ListLinq.Add(linkEntities1);
      }
      catch (Exception ex)
      {
        if (isVisibilityError)
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
          {
            Text = Translate.RequestSaleSave_Возникла_ошибка_отправки_данных_в_Планфикс__Проверьте_настройки
          });
        LogHelper.Error(ex, "Ошибка при сохранении продажи в ПланФикс", false);
      }
    }

    public static ClientPf GetClientPf(Guid clientUid, Document document)
    {
      if (clientUid == Guid.Empty)
        return (ClientPf) null;
      LinkEntities linkEntities1 = RequestSaleSave.ListLinq.FirstOrDefault<LinkEntities>((Func<LinkEntities, bool>) (x => x.EntityUid == clientUid && x.Type == TypeEntity.Client));
      if (linkEntities1 == null)
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          Client byUid = new ClientsRepository(dataBase).GetByUid(clientUid);
          PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
          ConfigManager.Config = new Planfix.Api.Config(planfix.AccountName, planfix.ApiUrl, planfix.DecryptedKeyApi, planfix.DecryptedToken);
          PlanfixSetting setting = planfix;
          Contact planfixContact = PlanfixHelper.GbsContactToPlanfixContact(byUid, setting);
          if (new ContactRepository().Add(ref planfixContact).Result == Planfix.Api.Answer.ResultTypes.Ok)
          {
            LinkEntities linkEntities2 = new LinkEntities()
            {
              Type = TypeEntity.Client,
              EntityUid = document.ContractorUid,
              Id = planfixContact.id
            };
            new LinkEntitiesRepository().Save(linkEntities2);
            RequestSaleSave.ListLinq.Add(linkEntities2);
            return new ClientPf() { Id = planfixContact.id };
          }
        }
      }
      return new ClientPf()
      {
        Id = linkEntities1 != null ? linkEntities1.Id : 0
      };
    }

    public static Planfix.Api.Entities.User GetUserPf(Guid userUid, Document document)
    {
      if (userUid == Guid.Empty)
        return (Planfix.Api.Entities.User) null;
      LinkEntities linkEntities1 = RequestSaleSave.ListLinq.FirstOrDefault<LinkEntities>((Func<LinkEntities, bool>) (x => x.EntityUid == userUid && x.Type == TypeEntity.User));
      if (linkEntities1 != null)
        return new Planfix.Api.Entities.User() { Id = linkEntities1.Id };
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Contact planfixContact = PlanfixHelper.GbsContactToPlanfixContact(new UsersRepository(dataBase).GetByUid(userUid).Client, RequestSaleSave.PlanfixSetting);
        Planfix.Api.Answer answer = new ContactRepository().Add(ref planfixContact);
        if (answer.Result == Planfix.Api.Answer.ResultTypes.Ok)
        {
          LinkEntities linkEntities2 = new LinkEntities()
          {
            Type = TypeEntity.User,
            EntityUid = document.UserUid,
            Id = planfixContact != null ? planfixContact.userid : 0
          };
          RequestSaleSave.ListLinq.Add(linkEntities2);
          new LinkEntitiesRepository().Save(linkEntities2);
        }
        Planfix.Api.Entities.User userPf;
        if (answer.Result != Planfix.Api.Answer.ResultTypes.Ok)
        {
          userPf = new Planfix.Api.Entities.User();
        }
        else
        {
          userPf = new Planfix.Api.Entities.User();
          userPf.Id = planfixContact != null ? planfixContact.userid : 0;
        }
        return userPf;
      }
    }
  }
}
