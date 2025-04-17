// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionIdentifyingDocumentsType
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
  public class PositionIdentifyingDocumentsType
  {
    private string identityField;
    private DocumentVidType documentVidField;
    private string documentNumberField;
    private byte[] electronicViewField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public DocumentVidType DocumentVid
    {
      get => this.documentVidField;
      set => this.documentVidField = value;
    }

    public string DocumentNumber
    {
      get => this.documentNumberField;
      set => this.documentNumberField = value;
    }

    [XmlElement(DataType = "base64Binary")]
    public byte[] ElectronicView
    {
      get => this.electronicViewField;
      set => this.electronicViewField = value;
    }
  }
}
