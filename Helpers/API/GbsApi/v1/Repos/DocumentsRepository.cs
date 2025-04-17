// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.GbsApi.v1.Repos.DocumentsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers.API.GbsApi.v1.Entities;
using Gbs.Helpers.API.GbsApi.v1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.API.GbsApi.v1.Repos
{
  public class DocumentsRepository : IRepository
  {
    public IAnswer GetData(Uri uri)
    {
      Dictionary<string, string> dictionary = uri.DecodeQueryParameters();
      Gbs.Core.Entities.Documents.DocumentsRepository.CommonFilter commonFilter = new Gbs.Core.Entities.Documents.DocumentsRepository.CommonFilter();
      string s1;
      if (dictionary.TryGetValue("date_start", out s1))
      {
        DateTime dateTime = DateTime.Parse(s1);
        commonFilter.DateStart = dateTime;
      }
      string s2;
      if (dictionary.TryGetValue("date_end", out s2))
      {
        DateTime dateTime = DateTime.Parse(s2);
        commonFilter.DateEnd = dateTime;
      }
      string str;
      if (dictionary.TryGetValue("type", out str))
      {
        DocumentTypes result;
        Enum.TryParse<DocumentTypes>(str, out result);
        GlobalDictionaries.DocumentsTypes documentsTypes1;
        switch (result)
        {
          case DocumentTypes.Sale:
            documentsTypes1 = GlobalDictionaries.DocumentsTypes.Sale;
            break;
          case DocumentTypes.SaleReturn:
            documentsTypes1 = GlobalDictionaries.DocumentsTypes.SaleReturn;
            break;
          case DocumentTypes.Buy:
            documentsTypes1 = GlobalDictionaries.DocumentsTypes.Buy;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        GlobalDictionaries.DocumentsTypes documentsTypes2 = documentsTypes1;
        commonFilter.Types = new GlobalDictionaries.DocumentsTypes[1]
        {
          documentsTypes2
        };
      }
      using (DataBase dataBase = Data.GetDataBase())
        return (IAnswer) new ListOfObjectsAnswer(PagingHelper.GetPage((IEnumerable<Gbs.Helpers.API.GbsApi.v1.Entities.IEntity>) new Gbs.Core.Entities.Documents.DocumentsRepository(dataBase).GetItemsWithFilter((Gbs.Core.Entities.Documents.DocumentsRepository.IFilter) commonFilter).Select<Gbs.Core.Entities.Documents.Document, Gbs.Helpers.API.GbsApi.v1.Entities.Document>((Func<Gbs.Core.Entities.Documents.Document, Gbs.Helpers.API.GbsApi.v1.Entities.Document>) (x => new Gbs.Helpers.API.GbsApi.v1.Entities.Document(x))).ToList<Gbs.Helpers.API.GbsApi.v1.Entities.Document>().Cast<Gbs.Helpers.API.GbsApi.v1.Entities.IEntity>().ToList<Gbs.Helpers.API.GbsApi.v1.Entities.IEntity>(), uri));
    }
  }
}
