// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.PositionCompositionProductsType
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
  public class PositionCompositionProductsType
  {
    private string identityField;
    private WbIngredientType ingredientTypeField;
    private string ingredientCodeField;
    private string ingredientNameField;

    public string Identity
    {
      get => this.identityField;
      set => this.identityField = value;
    }

    public WbIngredientType IngredientType
    {
      get => this.ingredientTypeField;
      set => this.ingredientTypeField = value;
    }

    public string IngredientCode
    {
      get => this.ingredientCodeField;
      set => this.ingredientCodeField = value;
    }

    public string IngredientName
    {
      get => this.ingredientNameField;
      set => this.ingredientNameField = value;
    }
  }
}
