// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ControlsHelpers.DataGrid.BulkObservableCollection`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

#nullable disable
namespace Gbs.Helpers.ControlsHelpers.DataGrid
{
  public class BulkObservableCollection<T> : ObservableCollection<T>
  {
    private bool _suppressNotification;

    public void AddRange(IEnumerable<T> items)
    {
      this._suppressNotification = true;
      foreach (T obj in items)
        this.Add(obj);
      this._suppressNotification = false;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this._suppressNotification)
        return;
      base.OnCollectionChanged(e);
    }
  }
}
