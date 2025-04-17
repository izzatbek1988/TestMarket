// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.DoWriteOffItemRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Egais;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Egais
{
  public static class DoWriteOffItemRepository
  {
    public static void GetResultTicket(EgaisWriteOffAct actWriteOff)
    {
      EgaisHelper.GetWaybillOut command = new EgaisHelper.GetWaybillOut();
      new EgaisHelper().GetCommand((EgaisHelper.GetEgiasCommand) command);
      List<Documents> ticketResult;
      SharedRepository.SetWaybillFromTicket(actWriteOff.ReplayUid.ToString(), command.Result, out ticketResult);
      if (!ticketResult.Any<Documents>())
        return;
      IOrderedEnumerable<Documents> source1 = ticketResult.OrderBy<Documents, DateTime?>((Func<Documents, DateTime?>) (x => ((TicketType) x?.Document?.Item)?.TicketDate));
      TicketResultType result = (source1 != null ? (TicketType) source1.LastOrDefault<Documents>((Func<Documents, bool>) (x => ((TicketType) x.Document.Item)?.Result != null))?.Document?.Item : (TicketType) (object) null)?.Result;
      if (result == null)
        return;
      actWriteOff.Status = result.Conclusion == ConclusionType.Accepted ? EgaisWriteOffActStatus.Transfer : EgaisWriteOffActStatus.Error;
      actWriteOff.Comment = result.Comments;
      IOrderedEnumerable<Documents> source2 = ticketResult.OrderBy<Documents, DateTime?>((Func<Documents, DateTime?>) (x => ((TicketType) x?.Document?.Item)?.TicketDate));
      OperationResultType operationResult = (source2 != null ? (TicketType) source2.LastOrDefault<Documents>((Func<Documents, bool>) (x => ((TicketType) x.Document.Item)?.OperationResult != null))?.Document?.Item : (TicketType) (object) null)?.OperationResult;
      if (operationResult == null)
        return;
      actWriteOff.Status = operationResult.OperationResult == ConclusionType.Accepted ? EgaisWriteOffActStatus.Done : actWriteOff.Status;
      actWriteOff.Comment = operationResult.OperationComment;
      new EgaisWriteOffActRepository().Save(actWriteOff);
    }
  }
}
