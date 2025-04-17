// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ControlsHelpers.DataGrid.AsyncObservableCollection`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

#nullable disable
namespace Gbs.Helpers.ControlsHelpers.DataGrid
{
  public class AsyncObservableCollection<T> : ObservableCollection<T>
  {
    private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

    public AsyncObservableCollection()
    {
    }

    public AsyncObservableCollection(IEnumerable<T> list)
      : base(list)
    {
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this._synchronizationContext == null || SynchronizationContext.Current == this._synchronizationContext)
        this.RaiseCollectionChanged((object) e);
      else
        this._synchronizationContext.Send(new SendOrPostCallback(this.RaiseCollectionChanged), (object) e);
    }

    private void RaiseCollectionChanged(object param)
    {
      base.OnCollectionChanged((NotifyCollectionChangedEventArgs) param);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (this._synchronizationContext == null || SynchronizationContext.Current == this._synchronizationContext)
        this.RaisePropertyChanged((object) e);
      else
        this._synchronizationContext.Send(new SendOrPostCallback(this.RaisePropertyChanged), (object) e);
    }

    private void RaisePropertyChanged(object param)
    {
      base.OnPropertyChanged((PropertyChangedEventArgs) param);
    }
  }
}
