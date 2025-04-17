// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.NotificationsBeginningTurnoverTypeHeader
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  [GeneratedCode("xsd", "4.8.3928.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(AnonymousType = true, Namespace = "http://fsrar.ru/WEGAIS/NotificationsBeginningTurnover")]
  [Serializable]
  public class NotificationsBeginningTurnoverTypeHeader
  {
    private string nUMBERField;
    private DateTime dateField;
    private string licenseRegNumberField;
    private string notifierField;
    private string producerField;
    private string fullNameField;
    private string fullNameManufacturerLanguageField;
    private Decimal alcVolumeMinField;
    private bool alcVolumeMinFieldSpecified;
    private Decimal alcVolumeMaxField;
    private bool alcVolumeMaxFieldSpecified;
    private Decimal alcVolumeField;
    private bool alcVolumeFieldSpecified;
    private WbUnitType1 unitTypeField;
    private string packageTypeField;
    private Decimal capacityField;
    private bool capacityFieldSpecified;
    private string distinctiveCharacteristicsField;
    private string shelfLifeField;
    private Decimal storageTemperatureMinField;
    private Decimal storageTemperatureMaxField;
    private Decimal storageHumidityMinField;
    private Decimal storageHumidityMaxField;
    private string otherStorageCharacteristicsField;
    private string codAP231Field;
    private string codOKPD2Field;
    private string codTNVEDTSField;
    private string vidAP171FZField;
    private DateTime dateFirstDeliveryField;
    private string trademarkDetailsField;
    private string noteField;
    private string termsTransportationField;
    private string termsSaleField;
    private string termsDisposalField;
    private byte[] labelFotoField;

    public string NUMBER
    {
      get => this.nUMBERField;
      set => this.nUMBERField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime Date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    public string LicenseRegNumber
    {
      get => this.licenseRegNumberField;
      set => this.licenseRegNumberField = value;
    }

    public string Notifier
    {
      get => this.notifierField;
      set => this.notifierField = value;
    }

    public string Producer
    {
      get => this.producerField;
      set => this.producerField = value;
    }

    public string FullName
    {
      get => this.fullNameField;
      set => this.fullNameField = value;
    }

    public string FullNameManufacturerLanguage
    {
      get => this.fullNameManufacturerLanguageField;
      set => this.fullNameManufacturerLanguageField = value;
    }

    public Decimal AlcVolumeMin
    {
      get => this.alcVolumeMinField;
      set => this.alcVolumeMinField = value;
    }

    [XmlIgnore]
    public bool AlcVolumeMinSpecified
    {
      get => this.alcVolumeMinFieldSpecified;
      set => this.alcVolumeMinFieldSpecified = value;
    }

    public Decimal AlcVolumeMax
    {
      get => this.alcVolumeMaxField;
      set => this.alcVolumeMaxField = value;
    }

    [XmlIgnore]
    public bool AlcVolumeMaxSpecified
    {
      get => this.alcVolumeMaxFieldSpecified;
      set => this.alcVolumeMaxFieldSpecified = value;
    }

    public Decimal AlcVolume
    {
      get => this.alcVolumeField;
      set => this.alcVolumeField = value;
    }

    [XmlIgnore]
    public bool AlcVolumeSpecified
    {
      get => this.alcVolumeFieldSpecified;
      set => this.alcVolumeFieldSpecified = value;
    }

    public WbUnitType1 UnitType
    {
      get => this.unitTypeField;
      set => this.unitTypeField = value;
    }

    public string PackageType
    {
      get => this.packageTypeField;
      set => this.packageTypeField = value;
    }

    public Decimal Capacity
    {
      get => this.capacityField;
      set => this.capacityField = value;
    }

    [XmlIgnore]
    public bool CapacitySpecified
    {
      get => this.capacityFieldSpecified;
      set => this.capacityFieldSpecified = value;
    }

    public string DistinctiveCharacteristics
    {
      get => this.distinctiveCharacteristicsField;
      set => this.distinctiveCharacteristicsField = value;
    }

    [XmlElement(DataType = "integer")]
    public string ShelfLife
    {
      get => this.shelfLifeField;
      set => this.shelfLifeField = value;
    }

    public Decimal StorageTemperatureMin
    {
      get => this.storageTemperatureMinField;
      set => this.storageTemperatureMinField = value;
    }

    public Decimal StorageTemperatureMax
    {
      get => this.storageTemperatureMaxField;
      set => this.storageTemperatureMaxField = value;
    }

    public Decimal StorageHumidityMin
    {
      get => this.storageHumidityMinField;
      set => this.storageHumidityMinField = value;
    }

    public Decimal StorageHumidityMax
    {
      get => this.storageHumidityMaxField;
      set => this.storageHumidityMaxField = value;
    }

    public string OtherStorageCharacteristics
    {
      get => this.otherStorageCharacteristicsField;
      set => this.otherStorageCharacteristicsField = value;
    }

    public string CodAP231
    {
      get => this.codAP231Field;
      set => this.codAP231Field = value;
    }

    public string CodOKPD2
    {
      get => this.codOKPD2Field;
      set => this.codOKPD2Field = value;
    }

    public string CodTNVEDTS
    {
      get => this.codTNVEDTSField;
      set => this.codTNVEDTSField = value;
    }

    public string VidAP171FZ
    {
      get => this.vidAP171FZField;
      set => this.vidAP171FZField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime DateFirstDelivery
    {
      get => this.dateFirstDeliveryField;
      set => this.dateFirstDeliveryField = value;
    }

    public string TrademarkDetails
    {
      get => this.trademarkDetailsField;
      set => this.trademarkDetailsField = value;
    }

    public string Note
    {
      get => this.noteField;
      set => this.noteField = value;
    }

    public string TermsTransportation
    {
      get => this.termsTransportationField;
      set => this.termsTransportationField = value;
    }

    public string TermsSale
    {
      get => this.termsSaleField;
      set => this.termsSaleField = value;
    }

    public string TermsDisposal
    {
      get => this.termsDisposalField;
      set => this.termsDisposalField = value;
    }

    [XmlElement(DataType = "base64Binary")]
    public byte[] LabelFoto
    {
      get => this.labelFotoField;
      set => this.labelFotoField = value;
    }
  }
}
