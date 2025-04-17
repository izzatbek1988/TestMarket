// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ContextHelp.F1help
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms;
using Gbs.Forms.Cafe;
using Gbs.Forms.Clients;
using Gbs.Forms.GoodGroupEdit;
using Gbs.Forms.GoodGroups;
using Gbs.Forms.Goods;
using Gbs.Forms.Goods.GoodGroupEdit;
using Gbs.Forms.Inventory;
using Gbs.Forms.Reports;
using Gbs.Forms.Reports.MasterReport;
using Gbs.Forms.Reports.SummaryReport;
using Gbs.Forms.Sale;
using Gbs.Forms.Settings;
using Gbs.Forms.Settings.ActionsHistory;
using Gbs.Forms.Settings.Organization;
using Gbs.Forms.Settings.Sections;
using Gbs.Forms.Users;
using Gbs.Forms.Users.UsersGroup;
using Gbs.Forms.Waybills;
using Gbs.Helpers.MVVM;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers.ContextHelp
{
  public static class F1help
  {
    public static HotKeysHelper.Hotkey HelpHotKey = new HotKeysHelper.Hotkey(Key.F1);

    public static RelayCommand OpenPage(UIElement frm)
    {
      if (new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia)
        return new RelayCommand((Action<object>) null);
      int num;
      switch (frm)
      {
        case MainWindow _:
          num = 16561;
          break;
        case FrmCafeMain _:
          num = 23204;
          break;
        case FrmCafeActiveOrders _:
          num = 24336;
          break;
        case FrmSearchGoods _:
          num = 16749;
          break;
        case FrmSearchClient _:
          num = 29875;
          break;
        case FrmLoginUser _:
          num = 16663;
          break;
        case FrmSummaryReport _:
          num = 16452;
          break;
        case SellerReport _:
          num = 16646;
          break;
        case FrmMagazineSale _:
          num = 16473;
          break;
        case FrmCardSale _:
          num = 21441;
          break;
        case FrmMasterReport _:
          num = 24283;
          break;
        case FrmGoodsCatalog _:
          num = 17049;
          break;
        case FrmGoodCard _:
          num = 18214;
          break;
        case FormGroup _:
          num = 21582;
          break;
        case FrmGoodGroupCard _:
          num = 16688;
          break;
        case FrmGoodsForGroupEdit _:
          num = 18285;
          break;
        case FrmActionForEditGood _:
          num = 18285;
          break;
        case FrmWaybillCard _:
          num = 21491;
          break;
        case FrmWaybillsList _:
          num = 21419;
          break;
        case FrmInventoryCard_v2 _:
          num = 25909;
          break;
        case FrmListClients _:
          num = 20169;
          break;
        case FrmClientCard _:
          num = 20199;
          break;
        case FrmClientGroup _:
          num = 21055;
          break;
        case FrmClientGroupsCard _:
          num = 18157;
          break;
        case FrmCreditClients _:
          num = 20998;
          break;
        case FrmSettings _:
          num = 13001;
          break;
        case FrmActionsHistoryList _:
          num = 21464;
          break;
        case FrmListUserGroups _:
          num = 25870;
          break;
        case FrmCardUserGroup _:
          num = 25809;
          break;
        case FrmUserList _:
          num = 28866;
          break;
        case FrmUserCard _:
          num = 28849;
          break;
        case FrmSectionsList _:
          num = 25887;
          break;
        case FrmOrganizationInfo _:
          num = 32679;
          break;
        default:
          num = -1;
          break;
      }
      int postId = num;
      return postId == -1 ? new RelayCommand((Action<object>) null) : new RelayCommand((Action<object>) (_ => FileSystemHelper.OpenSite("https://gbsmarket.ru/?p=" + postId.ToString() + "&utm_source=desktop_app&utm_medium=f1help")));
    }
  }
}
