// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RouteType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/RequestAdjustmentData")]
  [Serializable]
  public class RouteType
  {
    private string regIdField;
    private DateTime dateField;
    private RouteTypeTRAN_TYPE tRAN_TYPEField;
    private string tRAN_COMPANYField;
    private string tRAN_CARField;
    private string tRAN_TRAILERField;
    private string tRAN_CUSTOMERField;
    private string tRAN_DRIVERField;
    private string tRAN_LOADPOINTField;
    private string tRAN_UNLOADPOINTField;
    private string tRAN_REDIRECTField;
    private string tRAN_FORWARDERField;

    public string RegId
    {
      get => this.regIdField;
      set => this.regIdField = value;
    }

    [XmlElement(DataType = "date")]
    public DateTime Date
    {
      get => this.dateField;
      set => this.dateField = value;
    }

    public RouteTypeTRAN_TYPE TRAN_TYPE
    {
      get => this.tRAN_TYPEField;
      set => this.tRAN_TYPEField = value;
    }

    public string TRAN_COMPANY
    {
      get => this.tRAN_COMPANYField;
      set => this.tRAN_COMPANYField = value;
    }

    public string TRAN_CAR
    {
      get => this.tRAN_CARField;
      set => this.tRAN_CARField = value;
    }

    public string TRAN_TRAILER
    {
      get => this.tRAN_TRAILERField;
      set => this.tRAN_TRAILERField = value;
    }

    public string TRAN_CUSTOMER
    {
      get => this.tRAN_CUSTOMERField;
      set => this.tRAN_CUSTOMERField = value;
    }

    public string TRAN_DRIVER
    {
      get => this.tRAN_DRIVERField;
      set => this.tRAN_DRIVERField = value;
    }

    public string TRAN_LOADPOINT
    {
      get => this.tRAN_LOADPOINTField;
      set => this.tRAN_LOADPOINTField = value;
    }

    public string TRAN_UNLOADPOINT
    {
      get => this.tRAN_UNLOADPOINTField;
      set => this.tRAN_UNLOADPOINTField = value;
    }

    public string TRAN_REDIRECT
    {
      get => this.tRAN_REDIRECTField;
      set => this.tRAN_REDIRECTField = value;
    }

    public string TRAN_FORWARDER
    {
      get => this.tRAN_FORWARDERField;
      set => this.tRAN_FORWARDERField = value;
    }
  }
}
