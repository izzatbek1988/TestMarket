// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.EgaisDocument
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Documents;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Egais
{
  public class EgaisDocument
  {
    public Document WaybillDocument { get; set; }

    public string StatusText
    {
      get
      {
        return EgaisDocument.DictionaryWaybillStatuses.Single<KeyValuePair<EgaisDocument.WaybillStatuses, string>>((Func<KeyValuePair<EgaisDocument.WaybillStatuses, string>, bool>) (x => x.Key == this.StatusWaybill)).Value + "\n" + EgaisDocument.DictionaryStatusesForm2.Single<KeyValuePair<EgaisDocument.StatusesForm2, string>>((Func<KeyValuePair<EgaisDocument.StatusesForm2, string>, bool>) (x => x.Key == this.StatusForm2)).Value + "\n" + EgaisDocument.DictionaryStatusEgaisTTN.Single<KeyValuePair<EgaisHelper.StatusEgaisTTN, string>>((Func<KeyValuePair<EgaisHelper.StatusEgaisTTN, string>, bool>) (x => x.Key == this.StatusTtn)).Value + "\n" + this.TicketText;
      }
    }

    public Decimal TotalCount
    {
      get => this.Waybill.Items.Sum<PositionType>((Func<PositionType, Decimal>) (x => x.Quantity));
    }

    public string ReplayId { get; set; }

    public EgaisDocumentView Waybill { get; set; }

    public EgaisDocumentView Form2 { get; set; }

    public EgaisDocument.WaybillStatuses StatusWaybill { get; set; } = EgaisDocument.WaybillStatuses.New;

    public EgaisDocument.StatusesForm2 StatusForm2 { get; set; } = EgaisDocument.StatusesForm2.NotFoundForm2;

    public EgaisHelper.StatusEgaisTTN StatusTtn { get; set; }

    public string TicketText { get; set; }

    public static Dictionary<EgaisHelper.StatusEgaisTTN, string> DictionaryStatusEgaisTTN
    {
      get
      {
        return new Dictionary<EgaisHelper.StatusEgaisTTN, string>()
        {
          {
            EgaisHelper.StatusEgaisTTN.All,
            Translate.EgaisDocument_DictionaryStatusEgaisTTN_Все_статусы_накладных
          },
          {
            EgaisHelper.StatusEgaisTTN.Accept,
            Translate.EgaisDocument_DictionaryStatusEgaisTTN_Накладная_принята
          },
          {
            EgaisHelper.StatusEgaisTTN.Cancel,
            Translate.EgaisDocument_DictionaryStatusEgaisTTN_Отказ_от_накладной
          },
          {
            EgaisHelper.StatusEgaisTTN.New,
            Translate.EgaisDocument_DictionaryStatusEgaisTTN_Ответ_на_накладную_не_отправлен
          },
          {
            EgaisHelper.StatusEgaisTTN.SendedAct,
            Translate.EgaisDocument_DictionaryStatusEgaisTTN_Отправлен_акт_разногласий
          }
        };
      }
    }

    public static Dictionary<EgaisDocument.WaybillStatuses, string> DictionaryWaybillStatuses
    {
      get
      {
        return new Dictionary<EgaisDocument.WaybillStatuses, string>()
        {
          {
            EgaisDocument.WaybillStatuses.All,
            Translate.EgaisDocument_DictionaryWaybillStatuses_Все_накладные
          },
          {
            EgaisDocument.WaybillStatuses.New,
            Translate.EgaisDocument_DictionaryWaybillStatuses_Новая_накладная
          },
          {
            EgaisDocument.WaybillStatuses.Old,
            Translate.EgaisDocument_DictionaryWaybillStatuses_Обработанная_накладная
          },
          {
            EgaisDocument.WaybillStatuses.MoreOneDocumentInDb,
            Translate.EgaisDocument_DictionaryWaybillStatuses_Некорректно_обработанная_накладная
          }
        };
      }
    }

    public static Dictionary<EgaisDocument.StatusesForm2, string> DictionaryStatusesForm2
    {
      get
      {
        return new Dictionary<EgaisDocument.StatusesForm2, string>()
        {
          {
            EgaisDocument.StatusesForm2.FoundForm2,
            Translate.EgaisDocument_DictionaryStatusesForm2_Справка_формы_2_найдена
          },
          {
            EgaisDocument.StatusesForm2.FoundMoreOneForm2,
            Translate.EgaisDocument_DictionaryStatusesForm2_Найдено_больше_одной_справки_формы_2
          },
          {
            EgaisDocument.StatusesForm2.NotFoundForm2,
            Translate.EgaisDocument_DictionaryStatusesForm2_Не_найдена_справка_формы_2
          }
        };
      }
    }

    public enum WaybillStatuses
    {
      All,
      New,
      Old,
      MoreOneDocumentInDb,
    }

    public enum StatusesForm2
    {
      FoundForm2,
      NotFoundForm2,
      FoundMoreOneForm2,
    }
  }
}
