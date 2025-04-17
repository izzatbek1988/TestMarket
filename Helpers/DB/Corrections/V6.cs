// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V6
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V6 : ICorrection
  {
    public bool Do()
    {
      foreach (Sections.Section section in Sections.GetSectionsList().Where<Sections.Section>((Func<Sections.Section, bool>) (x => !x.IsDeleted)))
        section.Save();
      return true;
    }
  }
}
