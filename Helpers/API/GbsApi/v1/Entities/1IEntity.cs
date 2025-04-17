// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Entities.ListOfObjectsAnswer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.API.GbsApi.v1.Helpers;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Entities
{
  public class ListOfObjectsAnswer : IAnswer
  {
    public AnswerStatuses Status { get; set; }

    public long ResponseTime { get; set; }

    public int TotalPages { get; set; }

    public int TotalItems { get; set; }

    public List<IEntity> Data { get; set; }

    public ListOfObjectsAnswer(PagingHelper.PagerInfo p)
    {
      this.TotalPages = p.TotalPages;
      this.TotalItems = p.TotalItems;
      this.Data = p.FilteredItems;
    }

    public ListOfObjectsAnswer()
    {
    }
  }
}
