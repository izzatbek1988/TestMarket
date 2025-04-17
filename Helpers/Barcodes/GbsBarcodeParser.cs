// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Barcodes.GbsBarcodeParser
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.Barcodes
{
  public static class GbsBarcodeParser
  {
    private static SortedDictionary<string, GbsBarcodeParser.AII> aiiDict = new SortedDictionary<string, GbsBarcodeParser.AII>();
    private static string[] aiis;
    private static int minLengthOfAI = 1;
    private static int maxLengthOfAI = 4;
    private static char groutSeperator = '\u001D';
    private static string ean128StartCode = "]C1";
    private static bool hasCheckSum = true;

    public static bool HasCheckSum
    {
      get => GbsBarcodeParser.hasCheckSum;
      set => GbsBarcodeParser.hasCheckSum = value;
    }

    public static char GroutSeperator
    {
      get => GbsBarcodeParser.groutSeperator;
      set => GbsBarcodeParser.groutSeperator = value;
    }

    public static string EAN128StartCode
    {
      get => GbsBarcodeParser.ean128StartCode;
      set => GbsBarcodeParser.ean128StartCode = value;
    }

    [Localizable(false)]
    static GbsBarcodeParser()
    {
      GbsBarcodeParser.Add("00", "SerialShippingContainerCode", 2, GbsBarcodeParser.DataType.Numeric, 18, false);
      GbsBarcodeParser.Add("01", "EAN-NumberOfTradingUnit", 2, GbsBarcodeParser.DataType.Numeric, 14, false);
      GbsBarcodeParser.Add("02", "EAN-NumberOfTheWaresInTheShippingUnit", 2, GbsBarcodeParser.DataType.Numeric, 14, false);
      GbsBarcodeParser.Add("10", "Charge_Number", 2, GbsBarcodeParser.DataType.Alphanumeric, 20, true);
      GbsBarcodeParser.Add("11", "ProducerDate_JJMMDD", 2, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("12", "DueDate_JJMMDD", 2, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("13", "PackingDate_JJMMDD", 2, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("15", "MinimumDurabilityDate_JJMMDD", 2, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("17", "ExpiryDate_JJMMDD", 2, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("20", "ProductModel", 2, GbsBarcodeParser.DataType.Numeric, 2, false);
      GbsBarcodeParser.Add("21", "SerialNumber", 2, GbsBarcodeParser.DataType.Alphanumeric, 20, true);
      GbsBarcodeParser.Add("22", "HIBCCNumber", 2, GbsBarcodeParser.DataType.Alphanumeric, 29, false);
      GbsBarcodeParser.Add("240", "PruductIdentificationOfProducer", 3, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("241", "CustomerPartsNumber", 3, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("250", "SerialNumberOfAIntegratedModule", 3, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("251", "ReferenceToTheBasisUnit", 3, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("252", "GlobalIdentifierSerialisedForTrade", 3, GbsBarcodeParser.DataType.Numeric, 2, false);
      GbsBarcodeParser.Add("30", "AmountInParts", 2, GbsBarcodeParser.DataType.Numeric, 8, true);
      GbsBarcodeParser.Add("310d", "NetWeight_Kilogram", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("311d", "Length_Meter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("312d", "Width_Meter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("313d", "Heigth_Meter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("314d", "Surface_SquareMeter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("315d", "NetVolume_Liters", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("316d", "NetVolume_CubicMeters", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("320d", "NetWeight_Pounds", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("321d", "Length_Inches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("322d", "Length_Feet", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("323d", "Length_Yards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("324d", "Width_Inches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("325d", "Width_Feed", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("326d", "Width_Yards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("327d", "Heigth_Inches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("328d", "Heigth_Feed", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("329d", "Heigth_Yards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("330d", "GrossWeight_Kilogram", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("331d", "Length_Meter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("332d", "Width_Meter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("333d", "Heigth_Meter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("334d", "Surface_SquareMeter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("335d", "GrossVolume_Liters", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("336d", "GrossVolume_CubicMeters", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("337d", "KilogramPerSquareMeter", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("340d", "GrossWeight_Pounds", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("341d", "Length_Inches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("342d", "Length_Feet", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("343d", "Length_Yards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("344d", "Width_Inches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("345d", "Width_Feed", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("346d", "Width_Yards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("347d", "Heigth_Inches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("348d", "Heigth_Feed", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("349d", "Heigth_Yards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("350d", "Surface_SquareInches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("351d", "Surface_SquareFeet", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("352d", "Surface_SquareYards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("353d", "Surface_SquareInches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("354d", "Surface_SquareFeed", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("355d", "Surface_SquareYards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("356d", "NetWeight_TroyOunces", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("357d", "NetVolume_Ounces", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("360d", "NetVolume_Quarts", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("361d", "NetVolume_Gallonen", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("362d", "GrossVolume_Quarts", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("363d", "GrossVolume_Gallonen", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("364d", "NetVolume_CubicInches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("365d", "NetVolume_CubicFeet", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("366d", "NetVolume_CubicYards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("367d", "GrossVolume_CubicInches", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("368d", "GrossVolume_CubicFeet", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("369d", "GrossVolume_CubicYards", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("37", "QuantityInParts", 2, GbsBarcodeParser.DataType.Numeric, 8, true);
      GbsBarcodeParser.Add("390d", "AmountDue_DefinedValutaBand", 4, GbsBarcodeParser.DataType.Numeric, 15, true);
      GbsBarcodeParser.Add("391d", "AmountDue_WithISOValutaCode", 4, GbsBarcodeParser.DataType.Numeric, 18, true);
      GbsBarcodeParser.Add("392d", "BePayingAmount_DefinedValutaBand", 4, GbsBarcodeParser.DataType.Numeric, 15, true);
      GbsBarcodeParser.Add("393d", "BePayingAmount_WithISOValutaCode", 4, GbsBarcodeParser.DataType.Numeric, 18, true);
      GbsBarcodeParser.Add("400", "JobNumberOfGoodsRecipient", 3, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("401", "ShippingNumber", 3, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("402", "DeliveryNumber", 3, GbsBarcodeParser.DataType.Numeric, 17, false);
      GbsBarcodeParser.Add("403", "RoutingCode", 3, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("410", "EAN_UCC_GlobalLocationNumber(GLN)_GoodsRecipient", 3, GbsBarcodeParser.DataType.Numeric, 13, false);
      GbsBarcodeParser.Add("411", "EAN_UCC_GlobalLocationNumber(GLN)_InvoiceRecipient", 3, GbsBarcodeParser.DataType.Numeric, 13, false);
      GbsBarcodeParser.Add("412", "EAN_UCC_GlobalLocationNumber(GLN)_Distributor", 3, GbsBarcodeParser.DataType.Numeric, 13, false);
      GbsBarcodeParser.Add("413", "EAN_UCC_GlobalLocationNumber(GLN)_FinalRecipient", 3, GbsBarcodeParser.DataType.Numeric, 13, false);
      GbsBarcodeParser.Add("414", "EAN_UCC_GlobalLocationNumber(GLN)_PhysicalLocation", 3, GbsBarcodeParser.DataType.Numeric, 13, false);
      GbsBarcodeParser.Add("415", "EAN_UCC_GlobalLocationNumber(GLN)_ToBilligParticipant", 3, GbsBarcodeParser.DataType.Numeric, 13, false);
      GbsBarcodeParser.Add("420", "ZipCodeOfRecipient_withoutCountryCode", 3, GbsBarcodeParser.DataType.Alphanumeric, 20, true);
      GbsBarcodeParser.Add("421", "ZipCodeOfRecipient_withCountryCode", 3, GbsBarcodeParser.DataType.Alphanumeric, 12, true);
      GbsBarcodeParser.Add("422", "BasisCountryOfTheWares_ISO3166Format", 3, GbsBarcodeParser.DataType.Numeric, 3, false);
      GbsBarcodeParser.Add("7001", "Nato Stock Number", 4, GbsBarcodeParser.DataType.Numeric, 13, false);
      GbsBarcodeParser.Add("8001", "RolesProducts", 4, GbsBarcodeParser.DataType.Numeric, 14, false);
      GbsBarcodeParser.Add("8002", "SerialNumberForMobilePhones", 4, GbsBarcodeParser.DataType.Alphanumeric, 20, true);
      GbsBarcodeParser.Add("8003", "GlobalReturnableAssetIdentifier", 4, GbsBarcodeParser.DataType.Alphanumeric, 34, true);
      GbsBarcodeParser.Add("8004", "GlobalIndividualAssetIdentifier", 4, GbsBarcodeParser.DataType.Numeric, 30, true);
      GbsBarcodeParser.Add("8005", "SalesPricePerUnit", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("8006", "IdentifikationOfAProductComponent", 4, GbsBarcodeParser.DataType.Numeric, 18, false);
      GbsBarcodeParser.Add("8007", "IBAN", 4, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("8008", "DataAndTimeOfManufacturing", 4, GbsBarcodeParser.DataType.Numeric, 12, true);
      GbsBarcodeParser.Add("8018", "GlobalServiceRelationNumber", 4, GbsBarcodeParser.DataType.Numeric, 18, false);
      GbsBarcodeParser.Add("8020", "NumberBillCoverNumber", 4, GbsBarcodeParser.DataType.Alphanumeric, 25, false);
      GbsBarcodeParser.Add("8100", "CouponExtendedCode_NSC_offerCcode", 4, GbsBarcodeParser.DataType.Numeric, 10, false);
      GbsBarcodeParser.Add("8101", "CouponExtendedCode_NSC_offerCcode_EndOfOfferCode", 4, GbsBarcodeParser.DataType.Numeric, 14, false);
      GbsBarcodeParser.Add("8102", "CouponExtendedCode_NSC", 4, GbsBarcodeParser.DataType.Numeric, 6, false);
      GbsBarcodeParser.Add("90", "InformationForBilateralCoordinatedApplications", 2, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.Add("93", "Company specific", 2, GbsBarcodeParser.DataType.Alphanumeric, 30, true);
      GbsBarcodeParser.aiis = GbsBarcodeParser.aiiDict.Keys.ToArray<string>();
      GbsBarcodeParser.minLengthOfAI = GbsBarcodeParser.aiiDict.Values.Min<GbsBarcodeParser.AII>((Func<GbsBarcodeParser.AII, int>) (el => el.LengthOfAI));
      GbsBarcodeParser.maxLengthOfAI = GbsBarcodeParser.aiiDict.Values.Max<GbsBarcodeParser.AII>((Func<GbsBarcodeParser.AII, int>) (el => el.LengthOfAI));
    }

    public static void Add(
      string AI,
      string Description,
      int LengthOfAI,
      GbsBarcodeParser.DataType DataDescription,
      int LengthOfData,
      bool FNC1)
    {
      GbsBarcodeParser.aiiDict[AI] = new GbsBarcodeParser.AII(AI, Description, LengthOfAI, DataDescription, LengthOfData, FNC1);
    }

    public static Dictionary<GbsBarcodeParser.AII, string> Parse(string data, bool throwException = false)
    {
      if (data.StartsWith(GbsBarcodeParser.EAN128StartCode))
        data = data.Substring(GbsBarcodeParser.EAN128StartCode.Length);
      if (GbsBarcodeParser.HasCheckSum)
        data = data.Substring(0, data.Length - 2);
      Dictionary<GbsBarcodeParser.AII, string> dictionary = new Dictionary<GbsBarcodeParser.AII, string>();
      int index = 0;
      while (index < data.Length)
      {
        GbsBarcodeParser.AII ai = GbsBarcodeParser.GetAI(data, ref index);
        if (ai == null)
        {
          if (throwException)
            throw new InvalidOperationException("AI not found");
          return dictionary;
        }
        string code = GbsBarcodeParser.GetCode(data, ai, ref index);
        dictionary[ai] = code;
      }
      return dictionary;
    }

    private static GbsBarcodeParser.AII GetAI(string data, ref int index, bool usePlaceHolder = false)
    {
      GbsBarcodeParser.AII ai = (GbsBarcodeParser.AII) null;
      for (int minLengthOfAi = GbsBarcodeParser.minLengthOfAI; minLengthOfAi <= GbsBarcodeParser.maxLengthOfAI; ++minLengthOfAi)
      {
        string key = data.Substring(index, minLengthOfAi);
        if (usePlaceHolder)
          key = key.Remove(key.Length - 1) + "d";
        if (GbsBarcodeParser.aiiDict.TryGetValue(key, out ai))
        {
          index += minLengthOfAi;
          return ai;
        }
      }
      if (!usePlaceHolder)
        ai = GbsBarcodeParser.GetAI(data, ref index, true);
      return ai;
    }

    private static string GetCode(string data, GbsBarcodeParser.AII ai, ref int index)
    {
      int length = Math.Min(ai.LengthOfData, data.Length - index);
      string code = data.Substring(index, length);
      if (ai.FNC1)
      {
        int num = code.IndexOf(GbsBarcodeParser.GroutSeperator);
        if (num >= 0)
          length = num + 1;
        code = data.Substring(index, length);
      }
      index += length;
      return code;
    }

    public enum DataType
    {
      Numeric,
      Alphanumeric,
    }

    public class AII
    {
      public string AI { get; set; }

      public string Description { get; set; }

      public int LengthOfAI { get; set; }

      public GbsBarcodeParser.DataType DataDescription { get; set; }

      public int LengthOfData { get; set; }

      public bool FNC1 { get; set; }

      public AII(
        string AI,
        string Description,
        int LengthOfAI,
        GbsBarcodeParser.DataType DataDescription,
        int LengthOfData,
        bool FNC1)
      {
        this.AI = AI;
        this.Description = Description;
        this.LengthOfAI = LengthOfAI;
        this.DataDescription = DataDescription;
        this.LengthOfData = LengthOfData;
        this.FNC1 = FNC1;
      }

      public override string ToString()
      {
        return string.Format("{0} [{1}]", (object) this.AI, (object) this.Description);
      }
    }
  }
}
