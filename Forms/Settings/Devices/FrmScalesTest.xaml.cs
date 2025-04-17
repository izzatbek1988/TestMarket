// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.ScalesTestViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.Scales;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public partial class ScalesTestViewModel : ViewModelWithForm
  {
    private Decimal? _quantity;
    private ScalesHelper _scale;

    public Decimal? Quantity
    {
      get => this._quantity;
      set
      {
        this._quantity = value;
        this.OnPropertyChanged(nameof (Quantity));
      }
    }

    public void TestScales(Gbs.Core.Config.Devices deviceConfig = null)
    {
      if (deviceConfig == null)
        deviceConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (deviceConfig.Scale.Type == GlobalDictionaries.Devices.ScaleTypes.None)
      {
        int num = (int) MessageBoxHelper.Show(Translate.ScalesTestViewModel_TestScales_Не_указаны_весы_в_настройках_программы_);
      }
      else
      {
        this._scale = ScalesFactory.Create((IConfig) deviceConfig);
        this._scale.StartListen();
        this._scale.Notify += (ScalesHelper.ScaleHandler) (weight => this.Quantity = new Decimal?(weight));
        this.FormToSHow = (WindowWithSize) new FrmScalesTest();
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        this.ShowForm();
        this._scale?.StopListen();
        this._scale?.Dispose();
        ScalesFactory.Dispose();
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }
  }
}
