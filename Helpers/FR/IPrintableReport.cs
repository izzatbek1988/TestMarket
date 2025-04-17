// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.IPrintableReport
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers.FR
{
  public interface IPrintableReport
  {
    ReportType Type { get; set; }

    Dictionary<string, IEnumerable> DataDictionary { get; }

    IEnumerable<IReportProperty> Properties { get; }

    void AddData(string dataName, IEnumerable data);

    void AddProperties(IEnumerable<IReportProperty> properties);
  }
}
