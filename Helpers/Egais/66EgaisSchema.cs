// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Egais.RawAgedType
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
  [XmlType(Namespace = "http://fsrar.ru/WEGAIS/ClaimIssueFSM")]
  [Serializable]
  public class RawAgedType
  {
    private VolumeLocateAgainRawType exposureOneYearField;
    private VolumeLocateAgainRawType exposureThreeYearField;
    private VolumeLocateAgainRawType exposureFiveYearField;
    private VolumeLocateAgainRawType exposureSevenYearField;

    public VolumeLocateAgainRawType ExposureOneYear
    {
      get => this.exposureOneYearField;
      set => this.exposureOneYearField = value;
    }

    public VolumeLocateAgainRawType ExposureThreeYear
    {
      get => this.exposureThreeYearField;
      set => this.exposureThreeYearField = value;
    }

    public VolumeLocateAgainRawType ExposureFiveYear
    {
      get => this.exposureFiveYearField;
      set => this.exposureFiveYearField = value;
    }

    public VolumeLocateAgainRawType ExposureSevenYear
    {
      get => this.exposureSevenYearField;
      set => this.exposureSevenYearField = value;
    }
  }
}
