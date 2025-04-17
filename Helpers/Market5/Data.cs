// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Market5.Market5Data
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.Market5
{
  public class Market5Data
  {
    public List<Market5Data.GoodsCategory> GoodsCategories { get; set; } = new List<Market5Data.GoodsCategory>();

    public List<Market5Data.Good> goods { get; set; } = new List<Market5Data.Good>();

    public List<Market5Data.GoodSet> Good_sets { get; set; } = new List<Market5Data.GoodSet>();

    public List<Market5Data.GoodsModificationsIn5> goods_modifications { get; set; } = new List<Market5Data.GoodsModificationsIn5>();

    public List<Market5Data.ClientsGroups> clients_groups { get; set; } = new List<Market5Data.ClientsGroups>();

    public List<Market5Data.Client> Clients { get; set; } = new List<Market5Data.Client>();

    public List<Market5Data.Supplier> Suppliers { get; set; } = new List<Market5Data.Supplier>();

    public List<Market5Data.User5> Users { get; set; } = new List<Market5Data.User5>();

    public List<Market5Data.Sale> sales { get; set; } = new List<Market5Data.Sale>();

    public List<Market5Data.SaleItem> sale_items { get; set; } = new List<Market5Data.SaleItem>();

    public List<Market5Data.SalePayment> sales_payments { get; set; } = new List<Market5Data.SalePayment>();

    public List<Market5Data.SaleReturn> sale_returns { get; set; } = new List<Market5Data.SaleReturn>();

    public List<Market5Data.SaleReturnItem> sale_return_items { get; set; } = new List<Market5Data.SaleReturnItem>();

    public List<Market5Data.Bonus> deposit_journal { get; set; } = new List<Market5Data.Bonus>();

    public List<Market5Data.Waybill> waybills { get; set; } = new List<Market5Data.Waybill>();

    public List<Market5Data.WaybillItem> waybill_items { get; set; } = new List<Market5Data.WaybillItem>();

    public class Waybill
    {
      public int id { get; set; }

      public DateTime date_time { get; set; }

      public bool in_way { get; set; }

      public int supplier_id { get; set; }

      public int user_id { get; set; }

      public string number { get; set; }

      public string comment { get; set; }

      public int with_payments { get; set; }

      public bool is_deleted { get; set; }
    }

    public class WaybillItem
    {
      public int id { get; set; }

      public int waybill_id { get; set; }

      public int good_id { get; set; }

      public Decimal income { get; set; }

      public Decimal in_price { get; set; }

      public int mod_id { get; set; }

      public bool is_deleted { get; set; }
    }

    public class Bonus
    {
      public int id { get; set; }

      public int sale_item_id { get; set; }

      public Decimal points { get; set; }

      public string comment { get; set; }

      public bool is_deleted { get; set; }
    }

    public class Sale
    {
      public int id { get; set; }

      public DateTime date_time { get; set; }

      public string comment { get; set; }

      public int client_id { get; set; }

      public Decimal discount { get; set; }

      public int user_id { get; set; }

      public int section_id { get; set; }

      public bool is_fiscal { get; set; }

      public bool is_deleted { get; set; }
    }

    public class SaleItem
    {
      public int id { get; set; }

      public int sale_id { get; set; }

      public int good_id { get; set; }

      public Decimal count { get; set; }

      public Decimal discount { get; set; }

      public Decimal price { get; set; }

      public string comment { get; set; }

      public int mod_id { get; set; }

      public Decimal in_price1 { get; set; }

      public bool is_deleted { get; set; }
    }

    public class SalePayment
    {
      public int id { get; set; }

      public int sale_id { get; set; }

      public DateTime date_time { get; set; }

      public Decimal summ { get; set; }

      public int method { get; set; }

      public string comment { get; set; }

      public int user_id { get; set; }

      public bool is_deleted { get; set; }

      public int section_id { get; set; }
    }

    public class SaleReturn
    {
      public int id { get; set; }

      public DateTime date_time { get; set; }

      public Decimal summ { get; set; }

      public int user_id { get; set; }

      public string comment { get; set; }

      public bool is_deleted { get; set; }

      public int section_id { get; set; }
    }

    public class SaleReturnItem
    {
      public int id { get; set; }

      public int return_id { get; set; }

      public int sale_item_id { get; set; }

      public Decimal count { get; set; }

      public bool is_deleted { get; set; }
    }

    public class GoodsCategory
    {
      public int Id { get; set; }

      public string Name { get; set; }

      public int id_parent { get; set; }

      public int goods_type { get; set; }

      public string units { get; set; }

      public int digits { get; set; }

      public Decimal max_discount { get; set; }

      public bool needComment { get; set; }

      public Decimal salaryPercent { get; set; }

      public int taxType { get; set; }

      public bool nonFiscal { get; set; }

      public int sectionNumber { get; set; }

      public bool is_freePrice { get; set; }

      public Decimal nds { get; set; }

      public int taxSystem { get; set; }

      public int ffdGoodType { get; set; }

      public bool is_deleted { get; set; }

      [JsonIgnore]
      public Guid uid { get; } = Guid.NewGuid();
    }

    public class Good
    {
      public int Id { get; set; }

      public string name { get; set; }

      public int group_id { get; set; }

      public string barcode { get; set; }

      public Decimal price { get; set; }

      public Decimal price1 { get; set; }

      public Decimal price2 { get; set; }

      public Decimal price3 { get; set; }

      public Decimal price4 { get; set; }

      public Decimal price5 { get; set; }

      public Decimal priceC { get; set; }

      public Decimal stock { get; set; }

      public Decimal minimum_stock { get; set; }

      public string ext1 { get; set; }

      public string ext2 { get; set; }

      public string ext3 { get; set; }

      public string ext4 { get; set; }

      public string ext5 { get; set; }

      public string comment { get; set; }

      public int set_status { get; set; }

      public bool is_deleted { get; set; }

      [JsonIgnore]
      public Guid uid { get; set; } = Guid.NewGuid();
    }

    public class GoodSet
    {
      public int Id { get; set; }

      public int parent_id { get; set; }

      public int good_id { get; set; }

      public Decimal count { get; set; }

      public Decimal discount { get; set; }

      public bool is_deleted { get; set; }
    }

    public class GoodsModificationsIn5
    {
      public int Id { get; set; }

      public int good_id { get; set; }

      public string name { get; set; }

      public Decimal stock { get; set; }

      public bool is_deleted { get; set; }

      [JsonIgnore]
      public Guid Uid { get; } = Guid.NewGuid();
    }

    public class ClientsGroups
    {
      public int Id { get; set; }

      [JsonIgnore]
      public Guid uid { get; } = Guid.NewGuid();

      public string Name { get; set; }

      public Decimal maxCreditSum { get; set; }

      public int priceColumn { get; set; }

      public Decimal discount { get; set; }

      public bool nonUseBonus { get; set; }

      public bool is_deleted { get; set; }
    }

    public class Client
    {
      public int Id { get; set; }

      public string name { get; set; }

      public string phone { get; set; }

      public string email { get; set; }

      public Decimal discount { get; set; }

      public string date_in { get; set; }

      public string comment { get; set; }

      public int extPriceId { get; set; }

      public string card_barcode { get; set; } = string.Empty;

      public Decimal startSaleSumm { get; set; }

      public string address { get; set; }

      public string ext1 { get; set; }

      public string ext2 { get; set; }

      public string ext3 { get; set; }

      public string inn { get; set; }

      public string kpp { get; set; }

      public string rs { get; set; }

      public string ks { get; set; }

      public string bik { get; set; }

      public string bank { get; set; }

      public bool nonUseBonus { get; set; }

      public bool is_deleted { get; set; }

      public DateTime birthday { get; set; }

      public int group_id { get; set; }

      [JsonIgnore]
      public Guid uid { get; } = Guid.NewGuid();
    }

    public class Supplier
    {
      public int Id { get; set; }

      public string short_name { get; set; }

      public string full_name { get; set; }

      public string information { get; set; }

      public bool is_deleted { get; set; }
    }

    public class User5
    {
      public int Id { get; set; }

      public int Client_id { get; set; }

      public DateTime date_in { get; set; }

      public DateTime date_out { get; set; }

      public string pass { get; set; }

      public int rules { get; set; }

      public bool status { get; set; }

      public bool is_kicked { get; set; }

      public bool is_deleted { get; set; }

      public string gbs_id { get; set; }

      public string alias { get; set; }
    }
  }
}
