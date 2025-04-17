// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.JsonApiDocumentsRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities
{
  public class JsonApiDocumentsRepository : IRepository<JsonApiDocuments>
  {
    public JsonApiDocuments GetByDocumentUid(Guid documentUid)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        JSON_API_DOCUMENTS jsonApiDocuments = dataBase.GetTable<JSON_API_DOCUMENTS>().FirstOrDefault<JSON_API_DOCUMENTS>((Expression<Func<JSON_API_DOCUMENTS, bool>>) (x => x.UID_DOCUMENT == documentUid));
        if (jsonApiDocuments == null)
          return (JsonApiDocuments) null;
        JsonApiDocuments byDocumentUid = new JsonApiDocuments();
        byDocumentUid.Uid = jsonApiDocuments.UID;
        byDocumentUid.Date = jsonApiDocuments.DATE_EXCHANGE;
        byDocumentUid.DocumentUid = jsonApiDocuments.UID_DOCUMENT;
        byDocumentUid.IsSuccessful = jsonApiDocuments.IS_SUCCESSFUL;
        byDocumentUid.IsDeleted = jsonApiDocuments.IS_DELETED;
        return byDocumentUid;
      }
    }

    public int Delete(List<JsonApiDocuments> itemsList) => throw new NotImplementedException();

    public bool Delete(JsonApiDocuments item) => throw new NotImplementedException();

    public List<JsonApiDocuments> GetActiveItems() => throw new NotImplementedException();

    public List<JsonApiDocuments> GetAllItems() => throw new NotImplementedException();

    public JsonApiDocuments GetByUid(Guid uid) => throw new NotImplementedException();

    public bool Save(JsonApiDocuments item)
    {
      if (this.Validate(item).Result == ActionResult.Results.Error)
        return false;
      using (DataBase dataBase = Data.GetDataBase())
        dataBase.InsertOrReplace<JSON_API_DOCUMENTS>(new JSON_API_DOCUMENTS()
        {
          IS_DELETED = item.IsDeleted,
          UID = item.Uid,
          DATE_EXCHANGE = item.Date,
          IS_SUCCESSFUL = item.IsSuccessful,
          UID_DOCUMENT = item.DocumentUid
        });
      return true;
    }

    public int Save(List<JsonApiDocuments> itemsList)
    {
      return itemsList.Count<JsonApiDocuments>(new Func<JsonApiDocuments, bool>(this.Save));
    }

    public ActionResult Validate(JsonApiDocuments item)
    {
      return new ActionResult(ActionResult.Results.Ok);
    }
  }
}
