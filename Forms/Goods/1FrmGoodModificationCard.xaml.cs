// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodModificationViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Goods
{
  public class GoodModificationViewModel : ViewModelWithForm
  {
    public ICommand GeneratedBarcode
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string[] strArray = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.Modifications.Split(GlobalDictionaries.SplitArr);
          if (strArray.Length == 0)
          {
            MessageBoxHelper.Warning(Translate.GoodModificationViewModel_GeneratedBarcode_В_разделе_Файл___Настройки___Оборудование___Сканер_ШК_требуется_указать_префикс_для_генерации_штрих_кода_модификации_);
          }
          else
          {
            this.Modification.Barcode = BarcodeHelper.RandomBarcode(strArray[0]);
            this.OnPropertyChanged("Modification");
          }
        }));
      }
    }

    public (bool, GoodsModifications.GoodModification) Result { get; set; } = (false, (GoodsModifications.GoodModification) null);

    public GoodsModifications.GoodModification Modification { get; set; }

    public ICommand SaveCommand { get; set; }

    public ICommand CloseCommand { get; set; }

    public Action CloseFrm { get; set; }

    public GoodModificationViewModel()
    {
    }

    private void ComPortScannerOnBarcodeChanged(string barcode)
    {
      if (!Other.IsActiveForm<FrmGoodModificationCard>())
        return;
      this.Modification.Barcode = barcode;
      this.OnPropertyChanged("Modification");
    }

    public GoodModificationViewModel(GoodsModifications.GoodModification modification)
    {
      this.Modification = modification;
      ComPortScanner.SetDelegat(new ComPortScanner.BarcodeChangeHandler(this.ComPortScannerOnBarcodeChanged));
      this.SaveCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = (true, this.Modification);
        this.CloseFrm();
      }));
      this.CloseCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.Result = (false, this.Modification);
        this.CloseFrm();
      }));
    }
  }
}
