// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.PartnersHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers.Licenses.GbsIdHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class PartnersHelper
  {
    public const string OriginalName = "GBS.Market";
    private static string _appName;

    private static Guid PartnerUid => Partner.GetInfo().Uid;

    private static bool IsBlockRegion
    {
      get
      {
        Partner.Info info = Partner.GetInfo();
        VendorConfig config = Vendor.GetConfig();
        return info.IsBlockRegion || config != null;
      }
    }

    public static bool IsPartner() => PartnersHelper.PartnerUid != Guid.Empty;

    public static Visibility GetVisibilityRegionSetting()
    {
      if (PartnersHelper.IsBlockRegion)
        return Visibility.Collapsed;
      VendorConfig config = Vendor.GetConfig();
      return config == null || config.Country == GlobalDictionaries.Countries.NotSet ? Visibility.Visible : Visibility.Collapsed;
    }

    public static Visibility GetLanguageSelectorVisibility()
    {
      List<GlobalDictionaries.Languages> allowedLanguagesList = PartnersHelper.GetAllowedLanguagesList();
      return allowedLanguagesList != null && allowedLanguagesList.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
    }

    public static List<GlobalDictionaries.Languages> GetAllowedLanguagesList()
    {
      Partner.Info info = Partner.GetInfo();
      VendorConfig config = Vendor.GetConfig();
      if (info.IsBlockRegion)
        return new List<GlobalDictionaries.Languages>();
      if (config == null)
        return GlobalDictionaries.LanguagesList().Select<GlobalDictionaries.Language, GlobalDictionaries.Languages>((Func<GlobalDictionaries.Language, GlobalDictionaries.Languages>) (x => x.Value)).ToList<GlobalDictionaries.Languages>();
      if (config.Languages != null && config.Languages.Any<GlobalDictionaries.Languages>())
        return config.Languages;
      return new List<GlobalDictionaries.Languages>()
      {
        config.Language.Value
      };
    }

    private static Guid LeoUid => Guid.Parse("8f5f791d-ccb2-44a0-ab9e-b21960d7b1ab");

    private static Guid HiPosUid => Guid.Parse("ef9f8cdb-6132-4292-b54a-41463062f331");

    public static string ProgramName()
    {
      if (PartnersHelper._appName != null)
        return PartnersHelper._appName;
      VendorConfig config = Vendor.GetConfig();
      if (config != null)
      {
        PartnersHelper._appName = config.ApplicationName;
        return PartnersHelper._appName;
      }
      if (PartnersHelper.PartnerUid == PartnersHelper.LeoUid)
      {
        PartnersHelper._appName = "LEO.Market";
        return PartnersHelper._appName;
      }
      if (PartnersHelper.PartnerUid == PartnersHelper.HiPosUid)
      {
        PartnersHelper._appName = "HiPos";
        return PartnersHelper._appName;
      }
      PartnersHelper._appName = "GBS.Market";
      return PartnersHelper._appName;
    }

    public static void OpenPage(PartnersHelper.PageTypes type)
    {
      Guid partnerUid = PartnersHelper.PartnerUid;
      string gbsIdWithPrefix = GbsIdHelperMain.GetGbsIdWithPrefix();
      Version gbsVersion = ApplicationInfo.GetInstance().GbsVersion;
      Settings settings = new ConfigsRepository<Settings>().Get();
      string str1;
      if (!PartnersHelper.IsPartner())
      {
        string str2 = "https://gbsmarket.ru";
        string str3;
        switch (type)
        {
          case PartnersHelper.PageTypes.Buy:
            str3 = "cost";
            break;
          case PartnersHelper.PageTypes.KnowlageBase:
            str3 = "kb6";
            break;
          case PartnersHelper.PageTypes.MainPage:
            str3 = "";
            break;
          case PartnersHelper.PageTypes.Support:
            str3 = "contacts";
            break;
          default:
            str3 = "";
            break;
        }
        string str4 = str3;
        str1 = str2 + "/" + str4 + "/?";
      }
      else
      {
        string str5 = "https://redirect.com.de/redirect.php";
        string str6;
        switch (type)
        {
          case PartnersHelper.PageTypes.Buy:
            str6 = "buy";
            break;
          case PartnersHelper.PageTypes.KnowlageBase:
            str6 = "kb";
            break;
          case PartnersHelper.PageTypes.MainPage:
            str6 = "";
            break;
          case PartnersHelper.PageTypes.Support:
            str6 = "support";
            break;
          default:
            str6 = "";
            break;
        }
        string str7 = str6;
        str1 = str5 + string.Format("?uid={0}&p={1}&", (object) partnerUid, (object) str7);
      }
      if (settings.Interface.Country == GlobalDictionaries.Countries.Ukraine)
      {
        string str8;
        switch (type)
        {
          case PartnersHelper.PageTypes.Buy:
            str8 = "https://gbsmarket.com.ua/contacts/?";
            break;
          case PartnersHelper.PageTypes.KnowlageBase:
            str8 = "https://www.youtube.com/playlist?list=PLCcdKxZwfWdXC13wcJSjmCL05SCBMUVwc&";
            break;
          case PartnersHelper.PageTypes.MainPage:
            str8 = "https://gbsmarket.com.ua/?";
            break;
          case PartnersHelper.PageTypes.Support:
            str8 = "https://gbsmarket.com.ua/uk/support-2/?";
            break;
          default:
            str8 = "";
            break;
        }
        str1 = str8;
      }
      NetworkHelper.OpenUrl(str1 + string.Format("gbsId={0}&utm_campaign={1}&utm_source=desktop_app&utm_medium=interface", (object) gbsIdWithPrefix, (object) gbsVersion));
    }

    public enum PageTypes
    {
      Buy,
      KnowlageBase,
      MainPage,
      Support,
    }
  }
}
