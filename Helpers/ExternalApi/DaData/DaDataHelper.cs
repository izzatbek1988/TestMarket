// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DaDataHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public class DaDataHelper
  {
    private readonly string _url = "http://suggestions.dadata.ru/suggestions";

    public bool DoCommand(DaDataHelper.DaDataCommand command)
    {
      if (!NetworkHelper.PingHost("suggestions.dadata.ru"))
        return false;
      RestHelper restHelper = new RestHelper(this._url, new int?(), command.ToJsonString(true));
      restHelper.CreateCommand(command.Path, TypeRestRequest.Post);
      restHelper.AddHeader("Authorization", "Token " + command.Token);
      restHelper.DoCommand();
      command.AnswerString = restHelper.Answer;
      return restHelper.StatusCode == HttpStatusCode.OK;
    }

    public class DaDataCommand
    {
      [JsonIgnore]
      public virtual string Path { get; }

      [JsonIgnore]
      public string Token { get; set; } = "f111d19412baa1bfefc15d84e82163ce88f1aa7e";

      [JsonIgnore]
      public string AnswerString { get; set; }
    }

    public class FindOrganizationByInnOrOgrn : DaDataHelper.DaDataCommand
    {
      public override string Path => "/api/4_1/rs/suggest/party";

      [JsonProperty("query")]
      public string Inn { get; set; }

      [JsonIgnore]
      public DaDataHelper.FindOrganizationByInnOrOgrn.Answer Info
      {
        get
        {
          return JsonConvert.DeserializeObject<DaDataHelper.FindOrganizationByInnOrOgrn.Answer>(this.AnswerString);
        }
      }

      public class Answer
      {
        [JsonProperty("suggestions")]
        public List<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.MainData>> Suggestions { get; set; }
      }

      public class Address : 
        DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.AddressData>
      {
        [JsonProperty("invalidity")]
        public object Invalidity { get; set; }
      }

      public class AddressData
      {
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_iso_code")]
        public string CountryIsoCode { get; set; }

        [JsonProperty("federal_district")]
        public string FederalDistrict { get; set; }

        [JsonProperty("region_fias_id")]
        public string RegionFiasId { get; set; }

        [JsonProperty("region_kladr_id")]
        public string RegionKladrId { get; set; }

        [JsonProperty("region_iso_code")]
        public string RegionIsoCode { get; set; }

        [JsonProperty("region_with_type")]
        public string RegionWithType { get; set; }

        [JsonProperty("region_type")]
        public string RegionType { get; set; }

        [JsonProperty("region_type_full")]
        public string RegionTypeFull { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("area_fias_id")]
        public string AreaFiasId { get; set; }

        [JsonProperty("area_kladr_id")]
        public string AreaKladrId { get; set; }

        [JsonProperty("area_with_type")]
        public string AreaWithType { get; set; }

        [JsonProperty("area_type")]
        public string AreaType { get; set; }

        [JsonProperty("area_type_full")]
        public string AreaTypeFull { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("city_fias_id")]
        public string CityFiasId { get; set; }

        [JsonProperty("city_kladr_id")]
        public string CityKladrId { get; set; }

        [JsonProperty("city_with_type")]
        public string CityWithType { get; set; }

        [JsonProperty("city_type")]
        public string CityType { get; set; }

        [JsonProperty("city_type_full")]
        public string CityTypeFull { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("city_area")]
        public object CityArea { get; set; }

        [JsonProperty("city_district_fias_id")]
        public object CityDistrictFiasId { get; set; }

        [JsonProperty("city_district_kladr_id")]
        public object CityDistrictKladrId { get; set; }

        [JsonProperty("city_district_with_type")]
        public object CityDistrictWithType { get; set; }

        [JsonProperty("city_district_type")]
        public object CityDistrictType { get; set; }

        [JsonProperty("city_district_type_full")]
        public object CityDistrictTypeFull { get; set; }

        [JsonProperty("city_district")]
        public object CityDistrict { get; set; }

        [JsonProperty("settlement_fias_id")]
        public object SettlementFiasId { get; set; }

        [JsonProperty("settlement_kladr_id")]
        public object SettlementKladrId { get; set; }

        [JsonProperty("settlement_with_type")]
        public object SettlementWithType { get; set; }

        [JsonProperty("settlement_type")]
        public object SettlementType { get; set; }

        [JsonProperty("settlement_type_full")]
        public object SettlementTypeFull { get; set; }

        [JsonProperty("settlement")]
        public object Settlement { get; set; }

        [JsonProperty("street_fias_id")]
        public object StreetFiasId { get; set; }

        [JsonProperty("street_kladr_id")]
        public object StreetKladrId { get; set; }

        [JsonProperty("street_with_type")]
        public object StreetWithType { get; set; }

        [JsonProperty("street_type")]
        public object StreetType { get; set; }

        [JsonProperty("street_type_full")]
        public object StreetTypeFull { get; set; }

        [JsonProperty("street")]
        public object Street { get; set; }

        [JsonProperty("stead_fias_id")]
        public object SteadFiasId { get; set; }

        [JsonProperty("stead_cadnum")]
        public object SteadCadnum { get; set; }

        [JsonProperty("stead_type")]
        public object SteadType { get; set; }

        [JsonProperty("stead_type_full")]
        public object SteadTypeFull { get; set; }

        [JsonProperty("stead")]
        public object Stead { get; set; }

        [JsonProperty("house_fias_id")]
        public object HouseFiasId { get; set; }

        [JsonProperty("house_kladr_id")]
        public object HouseKladrId { get; set; }

        [JsonProperty("house_cadnum")]
        public object HouseCadnum { get; set; }

        [JsonProperty("house_type")]
        public object HouseType { get; set; }

        [JsonProperty("house_type_full")]
        public object HouseTypeFull { get; set; }

        [JsonProperty("house")]
        public object House { get; set; }

        [JsonProperty("block_type")]
        public object BlockType { get; set; }

        [JsonProperty("block_type_full")]
        public object BlockTypeFull { get; set; }

        [JsonProperty("block")]
        public object Block { get; set; }

        [JsonProperty("entrance")]
        public object Entrance { get; set; }

        [JsonProperty("floor")]
        public object Floor { get; set; }

        [JsonProperty("flat_fias_id")]
        public object FlatFiasId { get; set; }

        [JsonProperty("flat_cadnum")]
        public object FlatCadnum { get; set; }

        [JsonProperty("flat_type")]
        public object FlatType { get; set; }

        [JsonProperty("flat_type_full")]
        public object FlatTypeFull { get; set; }

        [JsonProperty("flat")]
        public object Flat { get; set; }

        [JsonProperty("flat_area")]
        public object FlatArea { get; set; }

        [JsonProperty("square_meter_price")]
        public object SquareMeterPrice { get; set; }

        [JsonProperty("flat_price")]
        public object FlatPrice { get; set; }

        [JsonProperty("room_fias_id")]
        public object RoomFiasId { get; set; }

        [JsonProperty("room_cadnum")]
        public object RoomCadnum { get; set; }

        [JsonProperty("room_type")]
        public object RoomType { get; set; }

        [JsonProperty("room_type_full")]
        public object RoomTypeFull { get; set; }

        [JsonProperty("room")]
        public object Room { get; set; }

        [JsonProperty("postal_box")]
        public object PostalBox { get; set; }

        [JsonProperty("fias_id")]
        public string FiasId { get; set; }

        [JsonProperty("fias_code")]
        public string FiasCode { get; set; }

        [JsonProperty("fias_level")]
        public string FiasLevel { get; set; }

        [JsonProperty("fias_actuality_state")]
        public string FiasActualityState { get; set; }

        [JsonProperty("kladr_id")]
        public string KladrId { get; set; }

        [JsonProperty("geoname_id")]
        public string GeonameId { get; set; }

        [JsonProperty("capital_marker")]
        public string CapitalMarker { get; set; }

        [JsonProperty("okato")]
        public string Okato { get; set; }

        [JsonProperty("oktmo")]
        public string Oktmo { get; set; }

        [JsonProperty("tax_office")]
        public string TaxOffice { get; set; }

        [JsonProperty("tax_office_legal")]
        public string TaxOfficeLegal { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("geo_lat")]
        public string GeoLat { get; set; }

        [JsonProperty("geo_lon")]
        public string GeoLon { get; set; }

        [JsonProperty("beltway_hit")]
        public string BeltwayHit { get; set; }

        [JsonProperty("beltway_distance")]
        public string BeltwayDistance { get; set; }

        [JsonProperty("metro")]
        public object Metro { get; set; }

        [JsonProperty("divisions")]
        public object Divisions { get; set; }

        [JsonProperty("qc_geo")]
        public string QcGeo { get; set; }

        [JsonProperty("qc_complete")]
        public object QcComplete { get; set; }

        [JsonProperty("qc_house")]
        public object QcHouse { get; set; }

        [JsonProperty("history_values")]
        public object HistoryValues { get; set; }

        [JsonProperty("unparsed_parts")]
        public object UnparsedParts { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("qc")]
        public string Qc { get; set; }
      }

      public class MainData
      {
        [JsonProperty("fio")]
        public DaDataHelper.FindOrganizationByInnOrOgrn.Fio MainFullName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("state")]
        public DaDataHelper.FindOrganizationByInnOrOgrn.State State { get; set; }

        [JsonProperty("opf")]
        public DaDataHelper.FindOrganizationByInnOrOgrn.Opf Opf { get; set; }

        [JsonProperty("name")]
        public DaDataHelper.FindOrganizationByInnOrOgrn.Name OrgFullName { get; set; }

        [JsonProperty("inn")]
        public string Inn { get; set; }

        [JsonProperty("kpp")]
        public string Kpp { get; set; }

        [JsonProperty("ogrn")]
        public string Ogrn { get; set; }

        [JsonProperty("okpo")]
        public string Okpo { get; set; }

        [JsonProperty("okato")]
        public string Okato { get; set; }

        [JsonProperty("oktmo")]
        public string Oktmo { get; set; }

        [JsonProperty("okogu")]
        public string Okogu { get; set; }

        [JsonProperty("okfs")]
        public string Okfs { get; set; }

        [JsonProperty("okved")]
        public string Okved { get; set; }

        [JsonProperty("address")]
        public DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.AddressData> Address { get; set; }

        [JsonProperty("phones")]
        public List<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.PhoneData>> Phones { get; set; }

        [JsonProperty("emails")]
        public List<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.EmailData>> Emails { get; set; }
      }

      public class Fio
      {
        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("patronymic")]
        public string Patronymic { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("qc")]
        public object Qc { get; set; }
      }

      public class Name
      {
        [JsonProperty("full_with_opf")]
        public string FullWithOpf { get; set; }

        [JsonProperty("short_with_opf")]
        public string ShortWithOpf { get; set; }

        [JsonProperty("latin")]
        public object Latin { get; set; }

        [JsonProperty("full")]
        public string Full { get; set; }

        [JsonProperty("short")]
        public string Short { get; set; }
      }

      public class Opf
      {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("full")]
        public string Full { get; set; }

        [JsonProperty("short")]
        public string Short { get; set; }
      }

      public class PhoneData
      {
        [JsonProperty("contact")]
        public object Contact { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("qc")]
        public object Qc { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("extension")]
        public object Extension { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("country")]
        public object Country { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("city")]
        public object City { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("city_code")]
        public string CityCode { get; set; }

        [JsonProperty("qc_conflict")]
        public object QcConflict { get; set; }
      }

      public class EmailData
      {
        [JsonProperty("local")]
        public string Local { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("type")]
        public object Type { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("qc")]
        public object Qc { get; set; }
      }

      public class State
      {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public object Code { get; set; }

        [JsonProperty("actuality_date")]
        public object ActualityDate { get; set; }

        [JsonProperty("registration_date")]
        public object RegistrationDate { get; set; }

        [JsonProperty("liquidation_date")]
        public object LiquidationDate { get; set; }
      }

      public class Item<T>
      {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("unrestricted_value")]
        public string UnrestrictedValue { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
      }
    }
  }
}
