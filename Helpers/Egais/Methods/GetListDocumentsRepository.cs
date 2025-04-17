// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.GetListDocumentsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Egais
{
  public static class GetListDocumentsRepository
  {
    public static IEnumerable<EgaisDocument> SetWaybillsForm2(
      List<Gbs.Helpers.Egais.Documents> waybills,
      List<EgaisDocumentView> forms2)
    {
      LogHelper.WriteToEgaisLog("Ввыборка F2 для накладных");
      foreach (Gbs.Helpers.Egais.Documents waybill in waybills)
      {
        EgaisDocumentView item = new EgaisDocumentView(waybill);
        List<EgaisDocumentView> list = forms2.Where<EgaisDocumentView>((Func<EgaisDocumentView, bool>) (f => f.WBDate == item.WBDate && f.WBNUMBER == item.WBNUMBER && f.Identity == item.Identity && f.INN == item.INN && f.KPP == item.KPP)).ToList<EgaisDocumentView>();
        string[] strArray = new string[6]
        {
          "Накладная. Num: ",
          item.WBNUMBER,
          "; identity: ",
          item.Identity,
          ". F2 count ",
          null
        };
        int num = list.Count<EgaisDocumentView>();
        strArray[5] = num.ToString();
        LogHelper.WriteToEgaisLog(string.Concat(strArray));
        EgaisDocument egaisDocument = new EgaisDocument()
        {
          Waybill = new EgaisDocumentView(waybill)
        };
        num = list.Count<EgaisDocumentView>();
        switch (num)
        {
          case 0:
            egaisDocument.Form2 = new EgaisDocumentView()
            {
              WBRegId = Translate.EgaisDocument_DictionaryStatusesForm2_Не_найдена_справка_формы_2
            };
            egaisDocument.StatusForm2 = EgaisDocument.StatusesForm2.NotFoundForm2;
            break;
          case 1:
            egaisDocument.Form2 = list.Single<EgaisDocumentView>();
            egaisDocument.StatusForm2 = EgaisDocument.StatusesForm2.FoundForm2;
            break;
          case 2:
            if (list.First<EgaisDocumentView>().WBRegId == list.Last<EgaisDocumentView>().WBRegId)
            {
              egaisDocument.Form2 = list.First<EgaisDocumentView>();
              egaisDocument.StatusForm2 = EgaisDocument.StatusesForm2.FoundForm2;
              break;
            }
            egaisDocument.Form2 = new EgaisDocumentView()
            {
              WBRegId = Translate.EgaisRepository_SetWaybillsForm2_Найдено_2_справки_формы_2
            };
            egaisDocument.StatusForm2 = EgaisDocument.StatusesForm2.FoundMoreOneForm2;
            break;
          default:
            egaisDocument.Form2 = new EgaisDocumentView()
            {
              WBRegId = Translate.EgaisRepository_SetWaybillsForm2_Найдено_большей_одной_справки_формы_2
            };
            egaisDocument.StatusForm2 = EgaisDocument.StatusesForm2.FoundMoreOneForm2;
            break;
        }
        yield return egaisDocument;
      }
    }

    public static void SetWaybillsStatus(
      IEnumerable<EgaisDocument> documentsUtm,
      EgaisHelper.GetWaybillOut tickets,
      IReadOnlyCollection<Document> documentWaybills)
    {
      foreach (EgaisDocument egaisDocument in documentsUtm)
      {
        EgaisDocument d = egaisDocument;
        LogHelper.WriteToEgaisLog("Проверяю, нет ли записи о данной накладной в базе. Накладная num:" + d.Form2.WBNUMBER + "; id: " + d.Form2.Identity + "; from2.regId: " + d.Form2.WBRegId);
        List<Document> list = documentWaybills.Where<Document>((Func<Document, bool>) (x => (x.Properties.LastOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == GlobalDictionaries.TTNEgais))?.Value.ToString() ?? string.Empty) == d.Form2.WBRegId)).ToList<Document>();
        if (!list.Any<Document>((Func<Document, bool>) (x => !x.IsDeleted)))
        {
          LogHelper.WriteToEgaisLog("Накладной в БД нет, значит считаем статус = NEW");
          d.StatusWaybill = EgaisDocument.WaybillStatuses.New;
        }
        else if (list.Count<Document>() > 1)
        {
          LogHelper.WriteToEgaisLog("Найдено более одной записи о накладной в базе с regId " + d.Form2.WBRegId);
          d.StatusWaybill = EgaisDocument.WaybillStatuses.MoreOneDocumentInDb;
        }
        else
        {
          Document document = list.Single<Document>();
          d.WaybillDocument = document;
          string str = document.Properties.LastOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.StatusEgais))?.Value.ToString();
          EgaisHelper.StatusEgaisTTN result;
          if (!Enum.TryParse<EgaisHelper.StatusEgaisTTN>(str, out result))
          {
            LogHelper.WriteToEgaisLog("Не удалось опредеелить статус накладной, пропускаем. Статус в БД = " + str);
            continue;
          }
          d.StatusTtn = result;
          d.StatusWaybill = EgaisDocument.WaybillStatuses.Old;
          d.ReplayId = document.Properties.LastOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.ReplayUidEgais))?.Value.ToString() ?? "";
          LogHelper.WriteToEgaisLog("Найдена одна запись о накладной с replyId:" + d.ReplayId);
        }
        if (d.StatusWaybill == EgaisDocument.WaybillStatuses.Old)
          GetListDocumentsRepository.SetWaybillFromTicket(d, tickets.Result);
      }
    }

    public static void SetWaybillFromTicket(EgaisDocument document, EgaisHelper.A ticketsPathList)
    {
      List<Gbs.Helpers.Egais.Documents> ticketResult;
      SharedRepository.SetWaybillFromTicket(document.ReplayId, ticketsPathList, out ticketResult);
      if (!ticketResult.Any<Gbs.Helpers.Egais.Documents>())
      {
        document.TicketText = Translate.EgaisRepository_SetWaybillFromTicket_Акт_отправлен__ответа_нет;
      }
      else
      {
        document.TicketText = string.Format(Translate.EgaisRepository_SetWaybillFromTicket_Найдено_тикетов___0_, (object) ticketResult.Count<Gbs.Helpers.Egais.Documents>());
        foreach (TicketType ticketType in ticketResult.Select<Gbs.Helpers.Egais.Documents, TicketType>((Func<Gbs.Helpers.Egais.Documents, TicketType>) (t => (TicketType) t.Document.Item)))
        {
          string str1 = "\n-----------------\n" + string.Format(Translate.EgaisRepository_SetWaybillFromTicket_0, (object) ticketType.TicketDate, (object) ticketType.DocType);
          string str2 = ticketType.Result == null ? str1 + string.Format(Translate.EgaisRepository_SetWaybillFromTicket_Операция___0___результат___1___комментарий___2_, (object) ticketType.OperationResult.OperationName, (object) ticketType.OperationResult.OperationResult, (object) ticketType.OperationResult.OperationComment) : str1 + string.Format(Translate.EgaisRepository_SetWaybillFromTicket_Результат___0___комментарий___1_, (object) ticketType.Result.Conclusion, (object) ticketType.Result.Comments);
          document.TicketText += str2;
        }
      }
    }
  }
}
