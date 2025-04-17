// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FR.BackEnd.Entities.ReportProperties.CustomReportProperty
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Helpers.FR.BackEnd.Entities.ReportProperties
{
  public class CustomReportProperty : ReportProperty
  {
    private CustomReportProperty(object value)
      : base(value)
    {
    }

    public override string Name { get; }

    public CustomReportProperty(string name, object value)
      : base(value)
    {
      this.Name = name;
    }
  }
}
