// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.ConclusionType1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers.Egais
{
  [GeneratedCode("xsd", "4.8.3928.0")]
  [XmlType(TypeName = "ConclusionType", Namespace = "http://fsrar.ru/WEGAIS/ConfirmTicket")]
  [Serializable]
  public enum ConclusionType1
  {
    Accepted,
    Rejected,
  }
}
