// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.PrintableReport
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.FR.BackEnd.Entities.ReportProperties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.FR
{
  public class PrintableReport : IPrintableReport
  {
    public string FilePath { get; set; }

    public ReportType Type { get; set; }

    public Dictionary<string, IEnumerable> DataDictionary { get; private set; } = new Dictionary<string, IEnumerable>();

    public IEnumerable<IReportProperty> Properties { get; private set; } = (IEnumerable<IReportProperty>) new List<IReportProperty>();

    public void AddData(string dataName, IEnumerable data)
    {
      this.DataDictionary.Add(dataName, data);
    }

    public void AddProperties(IEnumerable<IReportProperty> properties)
    {
      this.Properties = this.Properties.Concat<IReportProperty>(properties);
    }
  }
}
