// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Storage.StorageCardModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers.MVVM;
using System;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Storage
{
  public partial class StorageCardModelView : ViewModelWithForm
  {
    public Storages.Storage Storage { get; set; }

    public Action CloseCardAction { get; set; }

    public ICommand SaveWarehouseCommand { get; set; }

    public bool SaveResult { get; private set; }

    public StorageCardModelView()
    {
    }

    public StorageCardModelView(Storages.Storage storage)
    {
      this.Storage = storage;
      this.SaveWarehouseCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    private void Save()
    {
      this.SaveResult = this.Storage.Save();
      if (!this.SaveResult)
        return;
      this.CloseCardAction();
    }
  }
}
