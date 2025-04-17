// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.Pages.Сertificate.CertificateBasicViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard.Pages.Сertificate
{
  public class CertificateBasicViewModel : ViewModelWithForm
  {
    private static Decimal _price;

    public static string AddMenuKey => nameof (AddMenuKey);

    public static List<EntityProperties.PropertyValue> PropertyValue { get; set; }

    public static Decimal? Nominal
    {
      get
      {
        List<EntityProperties.PropertyValue> propertyValue = CertificateBasicViewModel.PropertyValue;
        object obj = propertyValue != null ? propertyValue.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid))?.Value : (object) null;
        return obj != null ? new Decimal?(Convert.ToDecimal(obj)) : new Decimal?();
      }
      set
      {
        CertificateBasicViewModel.PropertyValue.Single<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid)).Value = (object) value;
      }
    }

    public bool IsReusable
    {
      get
      {
        List<EntityProperties.PropertyValue> propertyValue = CertificateBasicViewModel.PropertyValue;
        return Convert.ToInt32(propertyValue != null ? propertyValue.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateReusableUid))?.Value : (object) null) == 1;
      }
      set
      {
        CertificateBasicViewModel.PropertyValue.Single<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateReusableUid)).Value = (object) value;
      }
    }

    public static bool IsEnabledNominal { get; set; }

    public static Decimal Price
    {
      get
      {
        Decimal num;
        if (!CertificateViewModel.CertificatesDb.Any<CertificateBasicViewModel.CertificateView>())
        {
          num = CertificateBasicViewModel._price;
        }
        else
        {
          CertificateBasicViewModel.CertificateView certificateView = CertificateViewModel.CertificatesDb.FirstOrDefault<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => !x.Certificate.IsDeleted));
          num = certificateView != null ? certificateView.Certificate.Stock.Price : CertificateBasicViewModel._price;
        }
        CertificateBasicViewModel._price = num;
        return CertificateBasicViewModel._price;
      }
      set
      {
        CertificateBasicViewModel._price = value;
        if (!CertificateViewModel.CertificatesDb.Any<CertificateBasicViewModel.CertificateView>((Func<CertificateBasicViewModel.CertificateView, bool>) (x => !x.Certificate.IsDeleted)))
          return;
        CertificateViewModel.CertificatesDb.ToList<CertificateBasicViewModel.CertificateView>().ForEach((Action<CertificateBasicViewModel.CertificateView>) (x => x.Certificate.Stock.Price = value));
      }
    }

    public CertificateBasicViewModel()
    {
    }

    public CertificateBasicViewModel(
      Guid goodUid,
      List<EntityProperties.PropertyValue> propertyValue)
    {
      CertificateBasicViewModel.PropertyValue = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) propertyValue);
      CertificateBasicViewModel._price = 0M;
      using (DataBase dataBase = Data.GetDataBase())
        CertificateBasicViewModel.IsEnabledNominal = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Good, dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == goodUid && x.TYPE_UID == GlobalDictionaries.CertificateNominalUid))).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.CertificateNominalUid && x.EntityUid == goodUid)) == null;
    }

    public class CertificateView
    {
      public GoodsCertificate.Certificate Certificate { get; set; }

      public string Status
      {
        get
        {
          return GlobalDictionaries.CertificateStatusDictionary().Single<KeyValuePair<GlobalDictionaries.CertificateStatus, string>>((Func<KeyValuePair<GlobalDictionaries.CertificateStatus, string>, bool>) (x => x.Key == this.Certificate.Status)).Value;
        }
      }

      public bool IsSaveInDb { get; set; }

      public CertificateView(GoodsCertificate.Certificate certificate, bool isSave = false)
      {
        this.Certificate = certificate;
        this.IsSaveInDb = isSave;
      }
    }
  }
}
