// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.EntityGroupEdit.GoodGroup.CategoriesGroupEditViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.EntityGroupEdit.GoodGroup
{
  public partial class CategoriesGroupEditViewModel : ViewModel
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter = new ObservableCollection<GoodGroups.Group>();

    public Visibility VisibilityRuParameter
    {
      get
      {
        return new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public bool IsEditUnit { get; set; }

    public IEnumerable<GoodsUnits.GoodUnit> GoodUnits { get; }

    public Guid UnitsUid { get; set; }

    public bool IsEditNds { get; set; }

    public Dictionary<int, FiscalKkm.TaxRate> TaxTypes { get; } = new Dictionary<int, FiscalKkm.TaxRate>()
    {
      {
        0,
        new FiscalKkm.TaxRate(0M, Translate.GroupCardViewModel_По_умолчанию, -1)
      }
    };

    public int TaxRateNumber { get; set; }

    public bool IsEditTaxNum { get; set; }

    public int KkmSectionNumber { get; set; }

    public bool IsEditRuTaxSystem { get; set; }

    public bool IsEditMarking { get; set; }

    public GlobalDictionaries.RuTaxSystems RuTaxSystem { get; set; } = GlobalDictionaries.RuTaxSystems.None;

    public Dictionary<GlobalDictionaries.RuTaxSystems, string> RuTaxSystems { get; } = GlobalDictionaries.RuTaxSystemsDictionary();

    public GlobalDictionaries.RuMarkedProductionTypes RuMarkedProduction { get; set; }

    public Dictionary<GlobalDictionaries.RuMarkedProductionTypes, string> RuMarkedProductionDictionary { get; } = GlobalDictionaries.MarkedProductionTypesList.Where<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, bool>) (x => x.Type != GlobalDictionaries.RuMarkedProductionTypes.Fur)).ToDictionary<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, GlobalDictionaries.RuMarkedProductionTypes, string>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, GlobalDictionaries.RuMarkedProductionTypes>) (x => x.Type), (Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, string>) (x => x.TypeName));

    public bool IsEditFreePrice { get; set; }

    public bool IsEditRequestCount { get; set; }

    public bool IsEditParent { get; set; }

    public bool IsDeletedGroup { get; set; }

    public Dictionary<MessageBoxResult, string> DictionaryEdit { get; set; } = new Dictionary<MessageBoxResult, string>()
    {
      {
        MessageBoxResult.Yes,
        Translate.CategoriesGroupEditViewModel_Да
      },
      {
        MessageBoxResult.No,
        Translate.CategoriesGroupEditViewModel_Нет
      }
    };

    public MessageBoxResult OptionFreePrice { get; set; } = MessageBoxResult.Yes;

    public MessageBoxResult OptionRequestCount { get; set; } = MessageBoxResult.Yes;

    public MessageBoxResult OptionParent { get; set; } = MessageBoxResult.Yes;

    public ICommand DoEdit
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            if (!this.IsEditUnit && !this.IsEditNds && !this.IsEditRuTaxSystem && !this.IsEditTaxNum && !this.IsEditFreePrice && !this.IsEditRequestCount && !this.IsDeletedGroup && !this.IsEditParent && !this.IsEditMarking)
              MessageBoxHelper.Warning(Translate.CategoriesGroupEditViewModel_Требуется_выбрать_хотя_бы_одно_действие_для_редактирования_категории_);
            else if (!this.GroupsListFilter.Any<GoodGroups.Group>())
            {
              MessageBoxHelper.Warning(Translate.CategoriesGroupEditViewModel_Необходимо_выбрать_категории__которые_Вы_хотите_изменить_);
            }
            else
            {
              if (this.IsDeletedGroup && MessageBoxHelper.Question(Translate.CategoriesGroupEditViewModel_Будут_удалены_все_выбранные_категории_товаров__Вы_уверены_) == MessageBoxResult.No || MessageBoxHelper.Show(string.Format(Translate.CategoriesGroupEditViewModel_, (object) this.GroupsListFilter.Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
                return;
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ActionGoodEditViewModel_Групповое_редактирование);
              using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
              {
                GoodGroupsRepository rep = new GoodGroupsRepository(dataBase);
                foreach (GoodGroups.Group group1 in (Collection<GoodGroups.Group>) this.GroupsListFilter)
                {
                  GoodGroups.Group group = group1;
                  if (this.IsEditUnit)
                    group.UnitsUid = this.UnitsUid;
                  if (this.IsEditNds)
                    group.TaxRateNumber = this.TaxRateNumber;
                  if (this.IsEditRuTaxSystem)
                    group.RuTaxSystem = this.RuTaxSystem;
                  if (this.IsEditTaxNum)
                    group.KkmSectionNumber = this.KkmSectionNumber;
                  if (this.IsEditFreePrice)
                    group.IsFreePrice = this.OptionFreePrice == MessageBoxResult.Yes;
                  if (this.IsEditRequestCount && group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(new GlobalDictionaries.GoodTypes[1]))
                    group.IsRequestCount = this.OptionRequestCount == MessageBoxResult.Yes;
                  if (this.IsEditParent && group.ParentGroupUid != Guid.Empty)
                    group.IsDataParent = this.OptionParent == MessageBoxResult.Yes;
                  if (this.IsEditMarking)
                    group.RuMarkedProductionType = this.RuMarkedProduction;
                  if (this.IsDeletedGroup)
                  {
                    List<GoodGroups.Group> list = rep.GetByQuery(dataBase.GetTable<GOODS_GROUPS>().Where<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => x.PARENT_UID == group.Uid))).ToList<GoodGroups.Group>();
                    list.ForEach((Action<GoodGroups.Group>) (x => x.ParentGroupUid = Guid.Empty));
                    list.ForEach((Action<GoodGroups.Group>) (x => rep.Save(x)));
                    group.IsDeleted = true;
                  }
                }
                this.GroupsListFilter.ToList<GoodGroups.Group>().ForEach((Action<GoodGroups.Group>) (x => rep.Save(x)));
                progressBar.Close();
                CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllGoods);
                CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.CafeMenu);
                this.AddHistoryInfo();
                this.CloseAction();
              }
            }
          }
          catch (Exception ex)
          {
            ProgressBarHelper.Close();
            LogHelper.Error(ex, "Ошибка при групповом редактировании категорий", false);
            MessageBoxHelper.Error(Translate.CategoriesGroupEditViewModel_Возникла_ошибка_при_групповом_редактировании__пожалуйста__обратитесь_в_службу_технической_поддержки_);
          }
        }));
      }
    }

    private void AddHistoryInfo()
    {
      List<string> values = new List<string>();
      if (this.IsEditUnit)
        values.Add("- " + Translate.FrmGoodGroupCard_ЕдиницыИзмерения + " => " + this.GoodUnits.Single<GoodsUnits.GoodUnit>((Func<GoodsUnits.GoodUnit, bool>) (x => x.Uid == this.UnitsUid)).FullName + ";");
      if (this.IsEditNds)
        values.Add("- " + Translate.CategoriesGroupEditViewModel_AddHistoryInfo_НДС + " => " + this.TaxTypes.Single<KeyValuePair<int, FiscalKkm.TaxRate>>((Func<KeyValuePair<int, FiscalKkm.TaxRate>, bool>) (x => x.Key == this.TaxRateNumber)).Value.Name + ";");
      if (this.IsEditRuTaxSystem)
        values.Add("- " + Translate.CategoriesGroupEditViewModel_AddHistoryInfo_СНО + " => " + this.RuTaxSystems.Single<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>>((Func<KeyValuePair<GlobalDictionaries.RuTaxSystems, string>, bool>) (x => x.Key == this.RuTaxSystem)).Value + ";");
      if (this.IsEditTaxNum)
        values.Add("- " + Translate.FrmMagazineSale_Секция + " => " + this.KkmSectionNumber.ToString() + ";");
      if (this.IsEditFreePrice)
        values.Add("- " + Translate.СвободнаяЦена + " => " + (this.OptionFreePrice == MessageBoxResult.Yes ? Translate.CategoriesGroupEditViewModel_AddHistoryInfo_разрешить_ : Translate.CategoriesGroupEditViewModel_AddHistoryInfo_запретить_));
      if (this.IsEditRequestCount)
        values.Add("- " + Translate.CategoriesGroupEditViewModel_AddHistoryInfo_Количество_при_добавлении + " => " + (this.OptionRequestCount == MessageBoxResult.Yes ? Translate.CategoriesGroupEditViewModel_AddHistoryInfo_запрашивать_ : Translate.CategoriesGroupEditViewModel_AddHistoryInfo_не_запрашивать_));
      if (this.IsEditParent)
        values.Add("- " + Translate.CategoriesGroupEditViewModel_AddHistoryInfo_Свойства_родителя + " => " + (this.OptionParent == MessageBoxResult.Yes ? Translate.CategoriesGroupEditViewModel_AddHistoryInfo_использовать_ : Translate.CategoriesGroupEditViewModel_AddHistoryInfo_не_использовать_));
      if (this.IsEditMarking)
        values.Add("- " + Translate.CategoriesGroupEditViewModel_AddHistoryInfo_Маркировка + " => " + this.RuMarkedProductionDictionary.Single<KeyValuePair<GlobalDictionaries.RuMarkedProductionTypes, string>>((Func<KeyValuePair<GlobalDictionaries.RuMarkedProductionTypes, string>, bool>) (x => x.Key == this.RuMarkedProduction)).Value + ";");
      if (this.IsDeletedGroup)
        values.Add(Translate.CategoriesGroupEditViewModel_AddHistoryInfo___Удалить_из_базы_данных);
      ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateTextHistory((this.GroupsListFilter.Count <= 10 ? Translate.PageInventoryStart_Категории + ": " + string.Join(", ", this.GroupsListFilter.Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))) + "\n\r" : Translate.PageInventoryStart_Категории + ": " + string.Join(", ", this.GroupsListFilter.Take<GoodGroups.Group>(10).Select<GoodGroups.Group, string>((Func<GoodGroups.Group, string>) (x => x.Name))) + string.Format(Translate.CategoriesGroupEditViewModel_AddHistoryInfo_, (object) (this.GroupsListFilter.Count - 10))) + Translate.CategoriesGroupEditViewModel_AddHistoryInfo_Изменения + ":\n\r" + string.Join("\n\r", (IEnumerable<string>) values), GlobalDictionaries.EntityTypes.GroupEditGoodGroup, this.AuthUser), false);
    }

    public Action CloseAction { get; set; }

    public ObservableCollection<GoodGroups.Group> GroupsListFilter
    {
      get => this._groupsListFilter;
      set
      {
        this._groupsListFilter = value;
        this.OnPropertyChanged(nameof (GroupsListFilter));
      }
    }

    public CategoriesGroupEditViewModel()
    {
      foreach (KeyValuePair<int, FiscalKkm.TaxRate> taxRate in new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.TaxRates)
        this.TaxTypes.Add(taxRate.Key, taxRate.Value);
      List<GoodsUnits.GoodUnit> goodUnitList = new List<GoodsUnits.GoodUnit>();
      GoodsUnits.GoodUnit goodUnit = new GoodsUnits.GoodUnit();
      goodUnit.Uid = Guid.Empty;
      goodUnit.FullName = Translate.GlobalDictionaries_Не_указано;
      goodUnitList.Add(goodUnit);
      List<GoodsUnits.GoodUnit> collection = goodUnitList;
      collection.AddRange(GoodsUnits.GetUnitsListWithFilter(isDeletedLoad: false));
      this.GoodUnits = (IEnumerable<GoodsUnits.GoodUnit>) new List<GoodsUnits.GoodUnit>((IEnumerable<GoodsUnits.GoodUnit>) collection);
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }
  }
}
