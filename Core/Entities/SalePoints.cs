// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.SalePoints
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class SalePoints
  {
    public static List<SalePoints.SalePoint> GetSalePointList(IQueryable<SALE_POINTS> query = null)
    {
      using (DataBase db = Data.GetDataBase())
      {
        if (query == null)
          query = db.GetTable<SALE_POINTS>();
        return query.ToList<SALE_POINTS>().Select<SALE_POINTS, SalePoints.SalePoint>((Func<SALE_POINTS, SalePoints.SalePoint>) (s =>
        {
          return new SalePoints.SalePoint()
          {
            Uid = s.UID,
            IsDeleted = s.IS_DELETED,
            Organization = new ClientsRepository(db).GetByUid(s.ORGANIZATION_UID),
            Description = JsonConvert.DeserializeObject<SalePoints.Descript>(s.DESCRIPTION),
            Number = JsonConvert.DeserializeObject<SalePoints.NumberDocuments>(s.NUMBER_DOCUMENTS) ?? new SalePoints.NumberDocuments(),
            EntitiesId = JsonConvert.DeserializeObject<SalePoints.EntitiesId>(s.ENTITIES_ID) ?? new SalePoints.EntitiesId()
          };
        })).ToList<SalePoints.SalePoint>();
      }
    }

    public class SalePoint : Entity
    {
      [Required]
      public Client Organization { get; set; }

      public SalePoints.Descript Description { get; set; } = new SalePoints.Descript();

      public SalePoints.NumberDocuments Number { get; set; } = new SalePoints.NumberDocuments();

      public SalePoints.EntitiesId EntitiesId { get; set; } = new SalePoints.EntitiesId();

      public bool Save()
      {
        LogHelper.OnBegin();
        using (DataBase dataBase1 = Data.GetDataBase())
        {
          if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
            return false;
          DataBase dataBase2 = dataBase1;
          SALE_POINTS salePoints = new SALE_POINTS();
          salePoints.UID = this.Uid;
          salePoints.IS_DELETED = this.IsDeleted;
          Client organization = this.Organization;
          // ISSUE: explicit non-virtual call
          salePoints.ORGANIZATION_UID = organization != null ? __nonvirtual (organization.Uid) : Guid.Empty;
          salePoints.DESCRIPTION = JsonConvert.SerializeObject((object) this.Description);
          salePoints.NUMBER_DOCUMENTS = JsonConvert.SerializeObject((object) this.Number);
          salePoints.ENTITIES_ID = JsonConvert.SerializeObject((object) this.EntitiesId);
          dataBase2.InsertOrReplace<SALE_POINTS>(salePoints);
          LogHelper.OnEnd();
          return true;
        }
      }

      public ActionResult VerifyBeforeSave() => this.DataValidation();
    }

    public class Descript
    {
      public string NamePoint { get; set; }

      public string Adress { get; set; }

      public string Phone { get; set; }

      public string ExtraInfo { get; set; }
    }

    public class NumberDocuments
    {
      public int SaleNumber { get; set; } = 1;

      public int WaybillNumber { get; set; } = 1;

      public int WaybillReturnNumber { get; set; } = 1;

      public int MoveNumber { get; set; } = 1;

      public int MoveReturnNumber { get; set; } = 1;

      public int SaleReturnNumber { get; set; } = 1;

      public int WriteOffNumber { get; set; } = 1;

      public int ClientOrderNumber { get; set; } = 1;

      public int InventoryNumber { get; set; } = 1;

      public int MoveStorageNumber { get; set; } = 1;

      public int ProductionListNumber { get; set; } = 1;

      public int BeerProductionListNumber { get; set; } = 1;
    }

    public class EntitiesId
    {
      public int GoodIg { get; set; } = 1;
    }
  }
}
