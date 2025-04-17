// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.TableCafeViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Cafe
{
  public partial class TableCafeViewModel : ViewModelWithForm
  {
    private bool _isResult;

    public int NumTable { get; set; }

    public int CountGuest { get; set; }

    public bool Show(ref int numTable, ref int countGuest)
    {
      this.NumTable = numTable;
      this.CountGuest = countGuest;
      this.FormToSHow = (WindowWithSize) new FrmTableCafe();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
      if (this._isResult)
      {
        numTable = this.NumTable;
        countGuest = this.CountGuest;
      }
      return this._isResult;
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isResult = true;
          this.CloseAction();
        }));
      }
    }

    public ICommand CancelCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isResult = false;
          this.CloseAction();
        }));
      }
    }
  }
}
