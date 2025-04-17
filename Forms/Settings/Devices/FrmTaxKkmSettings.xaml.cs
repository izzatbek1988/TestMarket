// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.TaxKkmViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public partial class TaxKkmViewModel : ViewModelWithForm
  {
    private Gbs.Core.Config.FiscalKkm _kkmConfig;

    public ICommand PrintTestCheckForIndex
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (MessageBoxHelper.Question(string.Format(Translate.TaxKkmViewModel_PrintTestCheckForIndex_, (object) this.TaxList.Count)) == MessageBoxResult.No)
            return;
          List<CheckGood> list = this.TaxList.Select<TaxKkmViewModel.TaxRate, CheckGood>((Func<TaxKkmViewModel.TaxRate, CheckGood>) (x => new CheckGood(new Gbs.Core.Entities.Goods.Good()
          {
            Name = Translate.TaxKkmViewModel_PrintTestCheckForIndex_Ставка__ + x.Index.ToString(),
            Group = new GoodGroups.Group()
            {
              TaxRateNumber = x.Index,
              RuTaxSystem = GlobalDictionaries.RuTaxSystems.None
            }
          }, 1M, 0M, 1M, "", ""))).ToList<CheckGood>();
          List<CheckPayment> checkPaymentList = new List<CheckPayment>()
          {
            new CheckPayment()
            {
              Method = GlobalDictionaries.KkmPaymentMethods.Cash,
              Sum = (Decimal) this.TaxList.Count
            }
          };
          string str = "000000000000";
          List<CheckPayment> paymentsList = checkPaymentList;
          Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data = new Gbs.Core.Devices.CheckPrinters.CheckData.CheckData(list, paymentsList, CheckFiscalTypes.Fiscal, new Cashier()
          {
            Name = "Admin",
            Inn = str
          })
          {
            RuTaxSystem = this._kkmConfig.DefaultRuTaxSystem
          };
          KkmHelper kkmHelper = new KkmHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get());
          kkmHelper.PrintCheck(data);
          data.CheckType = CheckTypes.ReturnSale;
          kkmHelper.PrintCheck(data);
        }));
      }
    }

    public Action Close { get; set; }

    public ICommand Save { get; set; }

    public List<TaxKkmViewModel.TaxRate> TaxList { get; set; }

    public TaxKkmViewModel()
    {
    }

    public TaxKkmViewModel(Gbs.Core.Config.FiscalKkm kkmConfig)
    {
      TaxKkmViewModel taxKkmViewModel = this;
      this._kkmConfig = kkmConfig;
      this.TaxList = new List<TaxKkmViewModel.TaxRate>();
      foreach (KeyValuePair<int, Gbs.Core.Config.FiscalKkm.TaxRate> taxRate in kkmConfig.TaxRates)
        this.TaxList.Add(new TaxKkmViewModel.TaxRate()
        {
          Id = taxRate.Key,
          Name = taxRate.Value.Name,
          Value = taxRate.Value.TaxValue,
          Index = taxRate.Value.KkmIndex
        });
      this.TaxList = this.TaxList.OrderBy<TaxKkmViewModel.TaxRate, int>((Func<TaxKkmViewModel.TaxRate, int>) (x => x.Id)).ToList<TaxKkmViewModel.TaxRate>();
      this.OnPropertyChanged(nameof (TaxList));
      this.Save = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (taxKkmViewModel.TaxList.Any<TaxKkmViewModel.TaxRate>((Func<TaxKkmViewModel.TaxRate, bool>) (item => taxKkmViewModel.TaxList.Count<TaxKkmViewModel.TaxRate>((Func<TaxKkmViewModel.TaxRate, bool>) (x => x.Index == item.Index)) > 1)))
        {
          int num = (int) MessageBoxHelper.Show(Translate.TaxKkmViewModel_Одинаковый_номер_указан_для_нескольких_ставок, icon: MessageBoxImage.Hand);
        }
        else
        {
          kkmConfig.TaxRates = new Dictionary<int, Gbs.Core.Config.FiscalKkm.TaxRate>();
          foreach (TaxKkmViewModel.TaxRate tax in taxKkmViewModel.TaxList)
            kkmConfig.TaxRates.Add(tax.Id, new Gbs.Core.Config.FiscalKkm.TaxRate(tax.Value, tax.Name, tax.Index));
          taxKkmViewModel.Close();
        }
      }));
    }

    public class TaxRate
    {
      public int Id { get; set; }

      public string Name { get; set; }

      public Decimal Value { get; set; }

      public int Index { get; set; }
    }
  }
}
