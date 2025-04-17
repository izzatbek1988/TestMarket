// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Inventory.InventorySaveViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.MVVM;
using System;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Inventory
{
  public class InventorySaveViewModel : ViewModelWithForm
  {
    public ICommand BackCommand { get; set; }

    public ICommand PauseCommand { get; set; }

    public ICommand FinishCommand { get; set; }

    public InventorySaveViewModel.ResultTypes Result { get; private set; }

    public string Comment { get; set; } = string.Empty;

    public InventorySaveViewModel()
    {
      this.BackCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = InventorySaveViewModel.ResultTypes.Cancel;
        this.CloseAction();
      }));
      this.PauseCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = InventorySaveViewModel.ResultTypes.Pause;
        this.CloseAction();
      }));
      this.FinishCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = InventorySaveViewModel.ResultTypes.Finish;
        this.CloseAction();
      }));
    }

    public enum ResultTypes
    {
      Cancel,
      Pause,
      Finish,
    }
  }
}
