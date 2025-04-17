// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.NotificationsBeginningTurnoverType
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
  public class NotificationsBeginningTurnoverType
  {
    private string identityField;
    private NotificationsBeginningTurnoverTypeHeader headerField;
    private PositionCompositionProductsType[] compositionProductsField;
    private PositionIdentifyingDocumentsType[] identifyingDocumentsField;
    private PositionDeclarationType[] declarationField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public NotificationsBeginningTurnoverTypeHeader Header
    {
      get => this.headerField;
      set => this.headerField = value;
    }

    [XmlArrayItem("Position", IsNullable = false)]
    public PositionCompositionProductsType[] CompositionProducts
    {
      get => this.compositionProductsField;
      set => this.compositionProductsField = value;
    }

    [XmlArrayItem("Position", IsNullable = false)]
    public PositionIdentifyingDocumentsType[] IdentifyingDocuments
    {
      get => this.identifyingDocumentsField;
      set => this.identifyingDocumentsField = value;
    }

    [XmlArrayItem("Position", IsNullable = false)]
    public PositionDeclarationType[] Declaration
    {
      get => this.declarationField;
      set => this.declarationField = value;
    }
  }
}
