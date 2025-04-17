// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GoodsCertificate
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities.Documents;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GoodsCertificate
  {
    public static IEnumerable<GoodsCertificate.Certificate> GetCertificateFilteredList(
      IQueryable<CERTIFICATES> query = null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GoodsCertificate.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = new GoodsCertificate.\u003C\u003Ec__DisplayClass0_0();
      Performancer performancer = new Performancer("Загрузка сертификатов с запросом");
      // ISSUE: reference to a compiler-generated field
      cDisplayClass00.db = Data.GetDataBase();
      try
      {
        // ISSUE: reference to a compiler-generated field
        IQueryable<CERTIFICATES> source = query ?? cDisplayClass00.db.GetTable<CERTIFICATES>();
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        List<GoodsStocks.GoodStock> goodStockList = GoodsStocks.GetGoodStockList(source.SelectMany(Expression.Lambda<Func<CERTIFICATES, IEnumerable<GOODS_STOCK>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
        {
          (Expression) Expression.Call(cDisplayClass00.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<Expression>()),
          (Expression) Expression.Quote((Expression) Expression.Lambda<Func<GOODS_STOCK, bool>>((Expression) Expression.Equal(x.UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (CERTIFICATES.get_STOCK_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))
        }), parameterExpression1), (g, gs) => new
        {
          g = g,
          gs = gs
        }).Where(data => data.gs.IS_DELETED == false).Select(data => data.gs));
        List<GoodsCertificate.Certificate> list = source.ToList<CERTIFICATES>().Join<CERTIFICATES, GoodsStocks.GoodStock, Guid, GoodsCertificate.Certificate>((IEnumerable<GoodsStocks.GoodStock>) goodStockList, (Func<CERTIFICATES, Guid>) (c => c.STOCK_UID), (Func<GoodsStocks.GoodStock, Guid>) (st => st.Uid), (Func<CERTIFICATES, GoodsStocks.GoodStock, GoodsCertificate.Certificate>) ((c, st) =>
        {
          return new GoodsCertificate.Certificate()
          {
            Uid = c.UID,
            Stock = st,
            Status = (GlobalDictionaries.CertificateStatus) c.STATUS,
            Barcode = c.BARCODE,
            IsDeleted = c.IS_DELETED
          };
        })).AsParallel<GoodsCertificate.Certificate>().ToList<GoodsCertificate.Certificate>();
        performancer.Stop();
        return (IEnumerable<GoodsCertificate.Certificate>) list;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass00.db != null)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass00.db.Dispose();
        }
      }
    }

    public static GoodsCertificate.Certificate GetCertificateByUd(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return GoodsCertificate.GetCertificateFilteredList(dataBase.GetTable<CERTIFICATES>().Where<CERTIFICATES>((Expression<Func<CERTIFICATES, bool>>) (x => x.UID == uid))).FirstOrDefault<GoodsCertificate.Certificate>();
    }

    public static GoodsCertificate.Certificate GetCertificateByBarcode(string barcode)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return GoodsCertificate.GetCertificateFilteredList(dataBase.GetTable<CERTIFICATES>().Where<CERTIFICATES>((Expression<Func<CERTIFICATES, bool>>) (x => x.BARCODE == barcode))).FirstOrDefault<GoodsCertificate.Certificate>();
    }

    public class Certificate : Entity
    {
      public Certificate()
      {
      }

      public Certificate(CERTIFICATES c)
      {
        this.Uid = c.UID;
        this.Stock = GoodsStocks.GetStocksByUid(c.STOCK_UID);
        this.Status = (GlobalDictionaries.CertificateStatus) c.STATUS;
        this.Barcode = c.BARCODE;
        this.IsDeleted = c.IS_DELETED;
      }

      [Required]
      public GoodsStocks.GoodStock Stock { get; set; }

      public GlobalDictionaries.CertificateStatus Status { get; set; }

      public string Barcode { get; set; } = string.Empty;

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool Save()
      {
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        using (DataBase dataBase = Data.GetDataBase())
          dataBase.InsertOrReplace<CERTIFICATES>(new CERTIFICATES()
          {
            UID = this.Uid,
            STOCK_UID = this.Stock.Uid,
            STATUS = (int) this.Status,
            BARCODE = this.Barcode,
            IS_DELETED = this.IsDeleted
          });
        return true;
      }

      public bool SaveDocumentActivatedCertificate(GoodsStocks.GoodStock stock, Document docSale)
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          GOODS_STOCK goodsStock1 = dataBase.GetTable<GOODS_STOCK>().FirstOrDefault<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => x.UID == stock.Uid));
          if (goodsStock1 == null)
            goodsStock1 = new GOODS_STOCK()
            {
              STOCK = 0M,
              UID = Guid.Empty
            };
          GOODS_STOCK goodsStock2 = goodsStock1;
          if (goodsStock2.UID == Guid.Empty && stock.IsDeleted)
            return true;
          Document document = new Document()
          {
            Type = GlobalDictionaries.DocumentsTypes.UserStockEdit,
            ParentUid = stock.GoodUid,
            Storage = stock.Storage,
            Section = Sections.GetCurrentSection()
          };
          Gbs.Core.Entities.Documents.Item obj1 = new Gbs.Core.Entities.Documents.Item();
          Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good();
          good.Uid = stock.GoodUid;
          obj1.Good = good;
          obj1.ModificationUid = stock.ModificationUid;
          obj1.DocumentUid = document.Uid;
          obj1.GoodStock = stock;
          obj1.Quantity = stock.Stock - goodsStock2.STOCK;
          Gbs.Core.Entities.Documents.Item obj2 = obj1;
          document.Items.Add(obj2);
          string str = GlobalDictionaries.CertificateStatusDictionary().Single<KeyValuePair<GlobalDictionaries.CertificateStatus, string>>((Func<KeyValuePair<GlobalDictionaries.CertificateStatus, string>, bool>) (x => x.Key == this.Status)).Value;
          document.Comment = string.Format(Translate.Certificate_СертификатИспользованВПродажеСтатус, (object) this.Barcode, (object) docSale.Number, (object) str);
          return new DocumentsRepository(dataBase).Save(document);
        }
      }
    }
  }
}
