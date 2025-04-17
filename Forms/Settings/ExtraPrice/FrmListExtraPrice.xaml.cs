// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.ExtraPrice.ListExtraPriceViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.ExtraPrice
{
  public partial class ListExtraPriceViewModel : ViewModelWithForm
  {
    public ObservableCollection<GoodsExtraPrice.GoodExtraPrice> ExtraPriceList { get; set; } = new ObservableCollection<GoodsExtraPrice.GoodExtraPrice>(GoodsExtraPrice.GetGoodExtraPriceList().Where<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => !x.IsDeleted)));

    public GoodsExtraPrice.GoodExtraPrice SelectedPrice { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ListExtraPriceViewModel()
    {
      try
      {
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          GoodsExtraPrice.GoodExtraPrice price = new GoodsExtraPrice.GoodExtraPrice();
          if (!new FrmCardExtraPrice().ShowCard(ref price))
            return;
          this.ExtraPriceList.Add(price);
        }));
        this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedPrice != null)
          {
            if (((IEnumerable) obj).Cast<GoodsExtraPrice.GoodExtraPrice>().ToList<GoodsExtraPrice.GoodExtraPrice>().Count != 1)
            {
              int num1 = (int) MessageBoxHelper.Show(Translate.ListExtraPriceViewModel_Требуется_выбрать_только_одну_доп__цену_для_редактирования);
            }
            else
            {
              GoodsExtraPrice.GoodExtraPrice selectedPrice = this.SelectedPrice;
              if (!new FrmCardExtraPrice().ShowCard(ref selectedPrice))
                return;
              this.SelectedPrice = selectedPrice;
            }
          }
          else
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.ListExtraPriceViewModel_Требуется_выбрать_доп__цену, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
        }));
        this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedPrice != null)
          {
            List<GoodsExtraPrice.GoodExtraPrice> list1 = ((IEnumerable) obj).Cast<GoodsExtraPrice.GoodExtraPrice>().ToList<GoodsExtraPrice.GoodExtraPrice>();
            if (MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) list1.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
              return;
            using (DataBase dataBase = Data.GetDataBase())
            {
              List<Gbs.Core.Entities.Clients.Group> list2 = new GroupRepository(dataBase).GetActiveItems().ToList<Gbs.Core.Entities.Clients.Group>();
              foreach (GoodsExtraPrice.GoodExtraPrice goodExtraPrice in list1)
              {
                GoodsExtraPrice.GoodExtraPrice p = goodExtraPrice;
                if (list2.Any<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (x => x.Price.Uid == p.Uid)))
                {
                  int num = (int) MessageBoxHelper.Show(string.Format(Translate.ListExtraPriceViewModel_Доп__цену___0___удалить_невозможно__так_как_она_используется_в_группах_клиентов_, (object) p.Name));
                }
                else
                {
                  p.IsDeleted = true;
                  p.Save();
                  this.ExtraPriceList.Remove(p);
                }
              }
            }
          }
          else
          {
            int num3 = (int) MessageBoxHelper.Show(Translate.ListExtraPriceViewModel_Требуется_выбрать_доп__цену, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
        }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка доп. цен");
      }
    }
  }
}
