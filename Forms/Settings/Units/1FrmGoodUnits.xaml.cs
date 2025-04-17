// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Units.UnitsListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Units
{
  public class UnitsListViewModel : ViewModelWithForm
  {
    public ObservableCollection<GoodsUnits.GoodUnit> UnitsList { get; set; }

    public GoodsUnits.GoodUnit SelectedUnit { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public UnitsListViewModel()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
          this.UnitsList = new ObservableCollection<GoodsUnits.GoodUnit>(GoodsUnits.GetUnitsListWithFilter(dataBase.GetTable<GOODS_UNITS>().Where<GOODS_UNITS>((Expression<Func<GOODS_UNITS, bool>>) (x => !x.IS_DELETED))));
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          GoodsUnits.GoodUnit unit;
          if (!new FrmCardUnit().ShowCard(Guid.Empty, out unit))
            return;
          this.UnitsList.Add(unit);
        }));
        this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedUnit != null)
          {
            GoodsUnits.GoodUnit unit;
            if (!new FrmCardUnit().ShowCard(this.SelectedUnit.Uid, out unit))
              return;
            this.UnitsList[this.UnitsList.ToList<GoodsUnits.GoodUnit>().FindIndex((Predicate<GoodsUnits.GoodUnit>) (x => x.Uid == unit.Uid))] = unit;
          }
          else
          {
            int num = (int) MessageBoxHelper.Show(Translate.UnitsListViewModel_Требуется_выбрать_нужные_ед__измерения, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
        }));
        this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedUnit != null)
          {
            if (MessageBoxHelper.Show(Translate.UnitsListViewModel_Вы_уверены__что_хотите_удалить_ед__измерения_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
              return;
            this.SelectedUnit.IsDeleted = true;
            this.SelectedUnit.Save();
            this.UnitsList.Remove(this.SelectedUnit);
          }
          else
          {
            int num = (int) MessageBoxHelper.Show(Translate.UnitsListViewModel_Требуется_выбрать_нужные_ед__измерения, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
        }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка ед. измерения");
      }
    }
  }
}
