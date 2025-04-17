// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionDeclarationType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/NotificationsBeginningTurnover")]
  [Serializable]
  public class PositionDeclarationType
  {
    private string identityField;
    private DeclarationVidType declarationtVidField;
    private string declarationNumberField;
    private DateTime dateValidityField;
    private bool dateValidityFieldSpecified;
    private DateTime dateExpirationField;
    private bool dateExpirationFieldSpecified;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public DeclarationVidType DeclarationtVid
    {
      get => this.declarationtVidField;
      set => this.declarationtVidField = value;
    }

    public string DeclarationNumber
    {
      get => this.declarationNumberField;
      set => this.declarationNumberField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime DateValidity
    {
      get => this.dateValidityField;
      set => this.dateValidityField = value;
    }

    [XmlIgnore]
    public bool DateValiditySpecified
    {
      get => this.dateValidityFieldSpecified;
      set => this.dateValidityFieldSpecified = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime DateExpiration
    {
      get => this.dateExpirationField;
      set => this.dateExpirationField = value;
    }

    [XmlIgnore]
    public bool DateExpirationSpecified
    {
      get => this.dateExpirationFieldSpecified;
      set => this.dateExpirationFieldSpecified = value;
    }
  }
}
