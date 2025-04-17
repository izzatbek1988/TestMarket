// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Documents.IDocumentViewModel`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Documents;
using System.Collections.ObjectModel;

#nullable disable
namespace Gbs.Core.ViewModels.Documents
{
  public interface IDocumentViewModel<TItems>
  {
    Document Document { get; set; }

    ObservableCollection<TItems> Items { get; set; }

    ActionResult Save();
  }
}
