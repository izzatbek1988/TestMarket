// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.Pages.Сertificate.CardCertificateViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard.Pages.Сertificate
{
  public class CardCertificateViewModel
  {
    public ICommand SaveCertificate { get; set; }

    public Action Close { private get; set; }

    public IEnumerable<Storages.Storage> ListStorage { get; set; }

    public Storages.Storage Storage { get; set; }

    public int Count { get; set; }

    public string Barcode { get; set; } = string.Empty;

    public Visibility VisibilityBarcode { get; set; }

    public Visibility VisibilityCount { get; set; }

    public List<GoodsCertificate.Certificate> Certificates { get; set; } = new List<GoodsCertificate.Certificate>();

    public CardCertificateViewModel()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        this.ListStorage = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.IS_DELETED == false)));
      List<Storages.Storage> list = this.ListStorage.ToList<Storages.Storage>();
      if (list.Count == 1)
        this.Storage = list.First<Storages.Storage>();
      this.SaveCertificate = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.Count > 1000)
        {
          if (MessageBoxHelper.Show(Translate.CardCertificateViewModel_Возможно_создать_только_1000_сертификатов_за_один_раз__уменьшить_кол_во_и_продолжить_, buttons: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          this.Count = 1000;
        }
        string[] strArray = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.Certificates.Split(GlobalDictionaries.SplitArr);
        string prefix = "";
        if (strArray.Length != 0)
          prefix = strArray[0];
        if (this.Storage == null)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.CardCertificateViewModel_Невозможно_создать_без_склада);
        }
        else
        {
          GoodsStocks.GoodStock goodStock = new GoodsStocks.GoodStock()
          {
            Storage = this.Storage,
            Stock = (Decimal) (this.VisibilityCount == Visibility.Visible ? this.Count : 1)
          };
          if (this.VisibilityCount == Visibility.Visible)
          {
            for (int index = 1; index <= this.Count; ++index)
              this.Certificates.Add(new GoodsCertificate.Certificate()
              {
                Barcode = BarcodeHelper.RandomBarcode(prefix),
                Status = GlobalDictionaries.CertificateStatus.Open,
                Stock = goodStock
              });
          }
          else
          {
            if (this.Barcode.Length < 4)
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.CardCertificateViewModel_Длина_штрих_кода_должна_быть_не_менее_4_символов);
              return;
            }
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              // ISSUE: object of a compiler-generated type is created
              // ISSUE: variable of a compiler-generated type
              CardCertificateViewModel.\u003C\u003Ec__DisplayClass36_0 cDisplayClass360 = new CardCertificateViewModel.\u003C\u003Ec__DisplayClass36_0();
              // ISSUE: reference to a compiler-generated field
              cDisplayClass360.cDb = dataBase.GetTable<CERTIFICATES>().Where<CERTIFICATES>((Expression<Func<CERTIFICATES, bool>>) (x => !x.IS_DELETED && x.BARCODE == this.Barcode));
              // ISSUE: reference to a compiler-generated field
              if (cDisplayClass360.cDb.Any<CERTIFICATES>())
              {
                ParameterExpression parameterExpression1;
                ParameterExpression parameterExpression2;
                ParameterExpression parameterExpression3;
                ParameterExpression parameterExpression4;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: method reference
                // ISSUE: method reference
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: reference to a compiler-generated field
                // ISSUE: method reference
                // ISSUE: method reference
                if (dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => !x.IS_DELETED && new CardCertificateViewModel.\u003C\u003Ec__DisplayClass36_1()
                {
                  stocks = dataBase.GetTable<GOODS_STOCK>().Where<GOODS_STOCK>((Expression<Func<GOODS_STOCK, bool>>) (x => cDisplayClass360.cDb.Any<CERTIFICATES>(System.Linq.Expressions.Expression.Lambda<Func<CERTIFICATES, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(c.STOCK_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS_STOCK.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2))))
                }.stocks.Any<GOODS_STOCK>(System.Linq.Expressions.Expression.Lambda<Func<GOODS_STOCK, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(s.GOOD_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression4)))).ToList<GOODS>().Any<GOODS>())
                {
                  int num3 = (int) MessageBoxHelper.Show(Translate.CardCertificateViewModel_Сертификат_с_таким_штрих_кодом_уже_существует__измените_штрих_код);
                  return;
                }
              }
            }
            if (this.Barcode.Remove(prefix.Length) != prefix)
            {
              int num4 = (int) MessageBoxHelper.Show(string.Format(Translate.CardCertificateViewModel_Префикс_введенного_штрих_кода_не_соответствует_префиксу_в_настройках___0__, (object) prefix));
              return;
            }
            this.Certificates.Add(new GoodsCertificate.Certificate()
            {
              Barcode = this.Barcode,
              Status = GlobalDictionaries.CertificateStatus.Open,
              Stock = goodStock
            });
          }
          this.Close();
        }
      }));
    }
  }
}
