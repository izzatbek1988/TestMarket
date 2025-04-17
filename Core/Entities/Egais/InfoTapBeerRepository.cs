// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Egais.InfoTapBeerRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Tables.Egais;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities.Egais
{
  public class InfoTapBeerRepository : IEntityRepository<InfoToTapBeer, INFO_TO_TAP_BEER>
  {
    public int Delete(List<InfoToTapBeer> itemsList) => throw new NotImplementedException();

    public bool Delete(InfoToTapBeer item) => throw new NotImplementedException();

    public List<InfoToTapBeer> GetActiveItems()
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<INFO_TO_TAP_BEER>().Where<INFO_TO_TAP_BEER>((Expression<Func<INFO_TO_TAP_BEER, bool>>) (x => x.IS_DELETED == false)));
    }

    public List<InfoToTapBeer> GetAllItems()
    {
      return this.GetByQuery((IQueryable<INFO_TO_TAP_BEER>) null);
    }

    public List<InfoToTapBeer> GetByQuery(IQueryable<INFO_TO_TAP_BEER> query)
    {
      List<TapBeer> taps = new TapBeerRepository().GetAllItems();
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<INFO_TO_TAP_BEER>();
        return query.ToList<INFO_TO_TAP_BEER>().Select<INFO_TO_TAP_BEER, InfoToTapBeer>((Func<INFO_TO_TAP_BEER, InfoToTapBeer>) (s =>
        {
          return new InfoToTapBeer()
          {
            Uid = s.UID,
            IsDeleted = s.IS_DELETED,
            Quantity = new Decimal?(s.QUANTITY),
            ExpirationDate = new DateTime?(s.EXPIRATION_DATE),
            IsSendToCrpt = s.IS_SEND_TO_CRPT,
            MarkedInfo = s.MARK_INFO,
            Tap = taps.SingleOrDefault<TapBeer>((Func<TapBeer, bool>) (x => x.Uid == s.TAP_UID)),
            GoodUid = s.GOOD_UID,
            ConnectingDateTime = new DateTime?(s.DATE_TIME),
            SaleQuantity = s.SALE_QUANTITY,
            Price = new Decimal?(s.PRICE),
            StorageUid = s.STORAGE_UID,
            ChildGoodUid = s.CHILD_GOOD_UID,
            DocumentUid = s.DOCUMENT_UID
          };
        })).ToList<InfoToTapBeer>();
      }
    }

    public InfoToTapBeer GetByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<INFO_TO_TAP_BEER>().Where<INFO_TO_TAP_BEER>((Expression<Func<INFO_TO_TAP_BEER, bool>>) (x => x.UID == uid))).SingleOrDefault<InfoToTapBeer>();
    }

    public InfoToTapBeer GetByTapUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<INFO_TO_TAP_BEER>().Where<INFO_TO_TAP_BEER>((Expression<Func<INFO_TO_TAP_BEER, bool>>) (x => x.TAP_UID == uid && x.IS_DELETED == false))).FirstOrDefault<InfoToTapBeer>();
    }

    public List<InfoToTapBeer> GetByGoodUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return this.GetByQuery(dataBase.GetTable<INFO_TO_TAP_BEER>().Where<INFO_TO_TAP_BEER>((Expression<Func<INFO_TO_TAP_BEER, bool>>) (x => x.GOOD_UID == uid && x.IS_DELETED == false)));
    }

    public bool Save(InfoToTapBeer item)
    {
      DataContext dataContext = Data.GetDataContext();
      INFO_TO_TAP_BEER infoToTapBeer = new INFO_TO_TAP_BEER();
      infoToTapBeer.UID = item.Uid;
      infoToTapBeer.IS_DELETED = item.IsDeleted;
      infoToTapBeer.GOOD_UID = item.GoodUid;
      DateTime? nullable1 = item.ExpirationDate;
      infoToTapBeer.EXPIRATION_DATE = nullable1 ?? DateTime.Now;
      infoToTapBeer.IS_SEND_TO_CRPT = item.IsSendToCrpt;
      infoToTapBeer.MARK_INFO = item.MarkedInfo;
      Decimal? nullable2 = item.Quantity;
      infoToTapBeer.QUANTITY = nullable2.GetValueOrDefault();
      infoToTapBeer.TAP_UID = item.Tap.Uid;
      nullable1 = item.ConnectingDateTime;
      infoToTapBeer.DATE_TIME = nullable1 ?? DateTime.Now;
      infoToTapBeer.SALE_QUANTITY = item.SaleQuantity;
      nullable2 = item.Price;
      infoToTapBeer.PRICE = nullable2.GetValueOrDefault();
      infoToTapBeer.STORAGE_UID = item.StorageUid;
      infoToTapBeer.CHILD_GOOD_UID = item.ChildGoodUid;
      infoToTapBeer.DOCUMENT_UID = item.DocumentUid;
      dataContext.InsertOrReplace<INFO_TO_TAP_BEER>(infoToTapBeer);
      return true;
    }

    public int Save(List<InfoToTapBeer> itemsList)
    {
      return itemsList.Count<InfoToTapBeer>(new Func<InfoToTapBeer, bool>(this.Save));
    }

    public ActionResult Validate(InfoToTapBeer item)
    {
      ActionResult actionResult = item.DataValidation();
      if (item.MarkedInfo.IsNullOrEmpty())
        actionResult.Messages.Add("Указать код маркировки");
      if (item.GoodUid == Guid.Empty)
        actionResult.Messages.Add("Выбрать пивной кег из каталога");
      if (!item.ExpirationDate.HasValue)
        actionResult.Messages.Add("Указать срок годности продукции");
      Decimal? nullable = item.Quantity;
      if (nullable.HasValue)
      {
        nullable = item.Quantity;
        Decimal num = 0M;
        if (!(nullable.GetValueOrDefault() <= num & nullable.HasValue))
          goto label_9;
      }
      actionResult.Messages.Add("Указать корректный объем кеги в литрах");
label_9:
      nullable = item.Price;
      if (nullable.HasValue)
      {
        nullable = item.Price;
        Decimal num1 = 0M;
        if (!(nullable.GetValueOrDefault() <= num1 & nullable.HasValue))
        {
          nullable = item.Price;
          Decimal num2 = (Decimal) 100000;
          if (!(nullable.GetValueOrDefault() > num2 & nullable.HasValue))
            goto label_13;
        }
      }
      actionResult.Messages.Add("Указать корректную цену за литр");
label_13:
      if (actionResult.Messages.Any<string>())
        actionResult.Result = ActionResult.Results.Error;
      return actionResult;
    }
  }
}
