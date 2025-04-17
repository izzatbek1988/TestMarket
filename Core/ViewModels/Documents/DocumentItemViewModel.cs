// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Documents.DocumentItemViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers.MVVM;
using System;

#nullable disable
namespace Gbs.Core.ViewModels.Documents
{
  public class DocumentItemViewModel : ViewModel, IDocumentItemViewModel
  {
    private Good _good;
    private Decimal _quantity;

    public virtual string DisplayedName
    {
      get
      {
        return this.Good.Name + (this.GoodModification == null || !(this.GoodModification.Uid != Guid.Empty) ? string.Empty : " [" + this.GoodModification?.Name + "]");
      }
    }

    public Good Good
    {
      get => this._good;
      set
      {
        this._good = value;
        this.OnPropertyChanged(nameof (Good));
      }
    }

    public GoodsModifications.GoodModification GoodModification { get; set; }

    public Decimal Quantity
    {
      get => this._quantity;
      set
      {
        this._quantity = value;
        this.OnPropertyChanged(isUpdateAllProp: true);
      }
    }

    public Guid Uid { get; set; } = Guid.NewGuid();
  }
}
