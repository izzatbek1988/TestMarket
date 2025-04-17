// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroupEdit.ActionGoodEditViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Forms.Excel;
using Gbs.Forms.GoodGroups;
using Gbs.Forms.Lable;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.GoodGroupEdit
{
  public partial class ActionGoodEditViewModel : ViewModelWithForm
  {
    private Gbs.Core.Entities.Users.User _authUser;
    private Gbs.Core.Entities.GoodGroups.Group _group;
    private List<string> _listCommentGoodEdit = new List<string>();

    public Dictionary<LablePrintViewModel.Types, string> DictionaryTypePrint { get; set; } = new Dictionary<LablePrintViewModel.Types, string>()
    {
      {
        LablePrintViewModel.Types.Labels,
        Translate.ActionGoodEditViewModel_DictionaryTypePrint_этикетки
      },
      {
        LablePrintViewModel.Types.PriceTags,
        Translate.ActionGoodEditViewModel_DictionaryTypePrint_ценники
      }
    };

    public LablePrintViewModel.Types SelectedTypePrint { get; set; }

    public bool IsPrintLable { get; set; }

    public bool IsEditPrice { get; set; }

    public bool IsSetPriceEqual { get; set; } = true;

    public Decimal? PriceEqualValue { get; set; }

    public bool IsSetPriceСalculate { get; set; }

    public Decimal? CoeffPrice { get; set; }

    public ActionGoodEditViewModel.TypePriceEdit SelectedTypePriceEdit { get; set; }

    public ActionGoodEditViewModel.MethodPriceEdit SelectedMethodPriceEdit { get; set; }

    public Dictionary<ActionGoodEditViewModel.TypePriceEdit, string> TypePriceEditDictionary { get; set; } = new Dictionary<ActionGoodEditViewModel.TypePriceEdit, string>()
    {
      {
        ActionGoodEditViewModel.TypePriceEdit.BuyPrice,
        Translate.FrmWaybillCard_ЗакупочнаяX0aЦена
      },
      {
        ActionGoodEditViewModel.TypePriceEdit.LastBuyPrice,
        Translate.ActionGoodEditViewModel_TypePriceEditDictionary_Последняя_закупочная_цена
      },
      {
        ActionGoodEditViewModel.TypePriceEdit.SalePrice,
        Translate.FrmWaybillCard_РозничнаяX0aЦена
      }
    };

    public Dictionary<ActionGoodEditViewModel.MethodPriceEdit, string> MethodPriceEditDictionary { get; set; } = new Dictionary<ActionGoodEditViewModel.MethodPriceEdit, string>()
    {
      {
        ActionGoodEditViewModel.MethodPriceEdit.Add,
        Translate.ActionGoodEditViewModel_MethodPriceEditDictionary_прибавить_сумму
      },
      {
        ActionGoodEditViewModel.MethodPriceEdit.Minus,
        Translate.ActionGoodEditViewModel_MethodPriceEditDictionary_вычесть_сумму
      },
      {
        ActionGoodEditViewModel.MethodPriceEdit.Multiply,
        Translate.ActionGoodEditViewModel_MethodPriceEditDictionary_умножить_на
      },
      {
        ActionGoodEditViewModel.MethodPriceEdit.Separated,
        Translate.ActionGoodEditViewModel_MethodPriceEditDictionary_поделить_на
      }
    };

    public bool IsRoundPrice { get; set; }

    public Decimal RoundValue { get; set; }

    public bool IsEditOnlyNoNullStock { get; set; }

    public bool IsEditGroup { get; set; }

    public bool IsDeleteNullStock { get; set; }

    public string TextGroup => this.Group?.Name ?? Translate.PageInventoryStart_Выбрать;

    public bool IsGeneratedBarcode { get; set; }

    public ICommand DoActionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            if (!this.IsEditGroup && !this.IsGeneratedBarcode && !this.IsDeleteNullStock && !this.IsEditPrice)
              MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_Требуется_выбрать_хотя_бы_одно_действие_для_редактирования_товаров_);
            else if (this.IsEditGroup && this.Group == null)
            {
              MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_Необходимо_выбрать_категорию__в_которую_будут_перемещены_товары_);
            }
            else
            {
              if (this.IsEditPrice)
              {
                if (this.SelectedMethodPriceEdit == ActionGoodEditViewModel.MethodPriceEdit.None && this.IsSetPriceСalculate)
                {
                  MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_DoActionCommand_Необходимо_указать_способ_изменения_цены_для__завершения__редактирования_);
                  return;
                }
                if (this.SelectedTypePriceEdit == ActionGoodEditViewModel.TypePriceEdit.None && this.IsSetPriceСalculate)
                {
                  MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_DoActionCommand_Необходимо_указать_из_чего_будет_высчитываться_цена_для_завершения_редактирования_);
                  return;
                }
                if (!this.CoeffPrice.HasValue && this.IsSetPriceСalculate)
                {
                  MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_DoActionCommand_Необходимо_указать_указать_коэффициент__из_которого_будет_высчитываться_цена_для_завершения_редактирования_);
                  return;
                }
                Decimal? nullable = this.CoeffPrice;
                Decimal num = 0M;
                if (nullable.GetValueOrDefault() == num & nullable.HasValue && this.IsSetPriceСalculate && this.SelectedMethodPriceEdit == ActionGoodEditViewModel.MethodPriceEdit.Separated)
                {
                  MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_DoActionCommand_При_делении_цены_коэффициент_не_может_быть_равен_нулю_);
                  return;
                }
                nullable = this.PriceEqualValue;
                if (!nullable.HasValue && this.IsSetPriceEqual)
                {
                  MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_DoActionCommand_Необходимо_указать_какую_цену_нужно_установить_для_завершения_редактирования_);
                  return;
                }
              }
              if (MessageBoxHelper.Show(string.Format(Translate.ActionGoodEditViewModel_Вы_уверены__что_хотите_выполнить_данные_изменения_для__0__товаров__1_Данное_действие_нельзя_будет_отменить_, (object) this.Goods.Count, (object) Other.NewLine(2)), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
                return;
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ActionGoodEditViewModel_Групповое_редактирование);
              List<Gbs.Core.Entities.Goods.Good> source1 = new List<Gbs.Core.Entities.Goods.Good>();
              string[] strArray1 = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.RandomGenerated.Split(GlobalDictionaries.SplitArr);
              string prefix = "";
              if (strArray1.Length != 0)
                prefix = strArray1[0];
              BuyPriceCounter buyPriceCounter = (BuyPriceCounter) null;
              if (this.SelectedTypePriceEdit.IsEither<ActionGoodEditViewModel.TypePriceEdit>(ActionGoodEditViewModel.TypePriceEdit.BuyPrice, ActionGoodEditViewModel.TypePriceEdit.LastBuyPrice) && this.IsSetPriceСalculate)
                buyPriceCounter = new BuyPriceCounter();
              List<GoodsStocks.GoodStock> goodStockList = GoodsStocks.GetGoodStockList();
              foreach (Gbs.Core.Entities.Goods.Good good in this.Goods)
              {
                foreach (GoodsStocks.GoodStock goodStock in new List<GoodsStocks.GoodStock>((IEnumerable<GoodsStocks.GoodStock>) good.StocksAndPrices))
                {
                  GoodsStocks.GoodStock stock = goodStock;
                  good.StocksAndPrices[good.StocksAndPrices.FindIndex((Predicate<GoodsStocks.GoodStock>) (x => x.Uid == stock.Uid))] = goodStockList.FirstOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (s => s.Uid == stock.Uid)) ?? stock;
                }
                if (this.IsEditGroup)
                {
                  if (good.Group.GoodsType == this.Group.GoodsType)
                  {
                    good.Group.Uid = this.Group.Uid;
                  }
                  else
                  {
                    if (good.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))
                    {
                      if (this.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))
                      {
                        good.Group.Uid = this.Group.Uid;
                        goto label_34;
                      }
                    }
                    source1.Add(good);
                  }
                }
label_34:
                if (this.IsGeneratedBarcode)
                  good.Barcode = BarcodeHelper.RandomBarcode(prefix);
                if (this.IsDeleteNullStock)
                {
                  foreach (Entity entity in good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Stock == 0M)))
                    entity.IsDeleted = true;
                }
                if (this.IsEditPrice)
                  this.EditPrice(good, buyPriceCounter);
              }
              if (this.IsEditGroup)
                this._listCommentGoodEdit.Add("- " + Translate.FrmRemoveCash_КатегорияРасходов + " => " + this.Group.Name + ";");
              if (this.IsGeneratedBarcode)
                this._listCommentGoodEdit.Add(Translate.ActionGoodEditViewModel_DoActionCommand_);
              if (this.IsDeleteNullStock)
                this._listCommentGoodEdit.Add(Translate.ActionGoodEditViewModel_DoActionCommand_остаткиУдаленыНулевыеОстатки);
              if (this.IsEditPrice)
              {
                if (this.IsSetPriceEqual)
                {
                  this._listCommentGoodEdit.Add("- " + Translate.FrmMainWindow_Цена + string.Format(" => {0};", (object) this.PriceEqualValue.GetValueOrDefault()));
                }
                else
                {
                  string str1;
                  switch (this.SelectedMethodPriceEdit)
                  {
                    case ActionGoodEditViewModel.MethodPriceEdit.None:
                      str1 = "";
                      break;
                    case ActionGoodEditViewModel.MethodPriceEdit.Add:
                      str1 = Translate.ActionGoodEditViewModel_DoActionCommand_прибавить;
                      break;
                    case ActionGoodEditViewModel.MethodPriceEdit.Minus:
                      str1 = Translate.ActionGoodEditViewModel_DoActionCommand_отнять;
                      break;
                    case ActionGoodEditViewModel.MethodPriceEdit.Multiply:
                      str1 = Translate.ActionGoodEditViewModel_MethodPriceEditDictionary_умножить_на;
                      break;
                    case ActionGoodEditViewModel.MethodPriceEdit.Separated:
                      str1 = Translate.ActionGoodEditViewModel_MethodPriceEditDictionary_поделить_на;
                      break;
                    default:
                      throw new ArgumentOutOfRangeException();
                  }
                  string str2 = str1;
                  string str3;
                  switch (this.SelectedTypePriceEdit)
                  {
                    case ActionGoodEditViewModel.TypePriceEdit.None:
                      str3 = "";
                      break;
                    case ActionGoodEditViewModel.TypePriceEdit.SalePrice:
                      str3 = Translate.ActionGoodEditViewModel_DoActionCommand_розничная_цена;
                      break;
                    case ActionGoodEditViewModel.TypePriceEdit.BuyPrice:
                      str3 = Translate.ActionGoodEditViewModel_DoActionCommand_закупочная_цена;
                      break;
                    case ActionGoodEditViewModel.TypePriceEdit.LastBuyPrice:
                      str3 = Translate.ActionGoodEditViewModel_DoActionCommand_последняя_закупочная_цена;
                      break;
                    default:
                      throw new ArgumentOutOfRangeException();
                  }
                  string str4 = str3;
                  string str5 = this.IsRoundPrice ? string.Format(Translate.ActionGoodEditViewModel_DoActionCommand_и_округлить_до__0__разрядов, (object) this.RoundValue) : "";
                  List<string> listCommentGoodEdit = this._listCommentGoodEdit;
                  string[] strArray2 = new string[10]
                  {
                    "- ",
                    Translate.FrmMainWindow_Цена,
                    " => ",
                    str4,
                    " ",
                    str2,
                    " ",
                    null,
                    null,
                    null
                  };
                  Decimal? coeffPrice = this.CoeffPrice;
                  ref Decimal? local = ref coeffPrice;
                  strArray2[7] = local.HasValue ? local.GetValueOrDefault().ToString("N2") : (string) null;
                  strArray2[8] = " ";
                  strArray2[9] = str5;
                  string str6 = string.Concat(strArray2);
                  listCommentGoodEdit.Add(str6);
                }
              }
              if (source1.Any<Gbs.Core.Entities.Goods.Good>())
              {
                MessageBoxHelper.Warning(Translate.ActionGoodEditViewModel_Для_следующих_товаров_невозможно_изменить_категорию__т_к__они_имеют_тип_отличный_от_выбранной_категории__ + Other.NewLine() + string.Join(Other.NewLine(), source1.Select<Gbs.Core.Entities.Goods.Good, string>((Func<Gbs.Core.Entities.Goods.Good, string>) (x => x.Name))));
                this.IsResult = false;
                progressBar.Close();
              }
              else
              {
                using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
                {
                  new GoodRepository(dataBase).Save(this.Goods, false);
                  if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
                    new HomeOfficeHelper().PrepareAndSend<List<Gbs.Core.Entities.Goods.Good>>(this.Goods, HomeOfficeHelper.EntityEditHome.GoodList);
                  progressBar.Close();
                  this.IsResult = true;
                  CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllGoods);
                  CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.CafeMenu);
                  if (this.IsPrintLable)
                  {
                    List<BasketItem> items = new List<BasketItem>();
                    foreach (Gbs.Core.Entities.Goods.Good good in this.Goods)
                    {
                      foreach (IGrouping<\u003C\u003Ef__AnonymousType5<Guid, Decimal>, GoodsStocks.GoodStock> source2 in good.StocksAndPrices.GroupBy(x => new
                      {
                        ModificationUid = x.ModificationUid,
                        Price = x.Price
                      }))
                      {
                        BasketItem basketItem = new BasketItem(good, source2.First<GoodsStocks.GoodStock>().ModificationUid, source2.First<GoodsStocks.GoodStock>().Price, source2.First<GoodsStocks.GoodStock>().Storage, source2.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock)));
                        if (this.SelectedTypePrint == LablePrintViewModel.Types.PriceTags)
                          basketItem.Quantity = 1M;
                        items.Add(basketItem);
                      }
                    }
                    new FrmLablePrint().Print(this.SelectedTypePrint, items);
                  }
                  this.CloseAction();
                }
              }
            }
          }
          catch (Exception ex)
          {
            ProgressBarHelper.Close();
            LogHelper.WriteError(ex, "ОШибка группового редактирования товаров");
            string групповомРедактировании = Translate.ActionGoodEditViewModel_Ошибка_при_групповом_редактировании;
            LogHelper.ShowErrorMgs(ex, групповомРедактировании, LogHelper.MsgTypes.MessageBox);
            this.IsResult = false;
          }
        }));
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public ICommand GetGroupCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.SelectGroup()));
    }

    private bool IsResult { get; set; }

    private List<Gbs.Core.Entities.Goods.Good> Goods { get; set; }

    private Gbs.Core.Entities.GoodGroups.Group Group
    {
      get => this._group;
      set
      {
        this._group = value;
        this.OnPropertyChanged("TextGroup");
      }
    }

    public bool DoAction(List<Gbs.Core.Entities.Goods.Good> goods, Gbs.Core.Entities.Users.User authUser, out List<string> listEdit)
    {
      this.Goods = new List<Gbs.Core.Entities.Goods.Good>((IEnumerable<Gbs.Core.Entities.Goods.Good>) goods);
      this._authUser = authUser;
      this.FormToSHow = (WindowWithSize) new FrmActionForEditGood();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
      listEdit = this._listCommentGoodEdit;
      return this.IsResult;
    }

    private void SelectGroup()
    {
      Gbs.Core.Entities.GoodGroups.Group group;
      if (!new FormSelectGroup().GetSingleSelectedGroupUid(this._authUser, out group) || group == null)
        this.Group = (Gbs.Core.Entities.GoodGroups.Group) null;
      this.Group = group;
    }

    private void EditPrice(Gbs.Core.Entities.Goods.Good good, BuyPriceCounter buyPriceCounter)
    {
      List<GoodsStocks.GoodStock> source = good.StocksAndPrices;
      if (good.Group.GoodsType == GlobalDictionaries.GoodTypes.Service && !good.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted)))
        source.Add(new GoodsStocks.GoodStock()
        {
          GoodUid = good.Uid,
          Price = 0M,
          Storage = Storages.GetStorages().First<Storages.Storage>((Func<Storages.Storage, bool>) (x => !x.IsDeleted))
        });
      if (this.IsEditOnlyNoNullStock)
        source = source.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Stock > 0M)).ToList<GoodsStocks.GoodStock>();
      if (this.IsSetPriceEqual)
      {
        source.ForEach((Action<GoodsStocks.GoodStock>) (x => x.Price = this.PriceEqualValue.GetValueOrDefault()));
      }
      else
      {
        if (!this.IsSetPriceСalculate)
          return;
        foreach (GoodsStocks.GoodStock goodStock in source)
        {
          Decimal num1;
          switch (this.SelectedTypePriceEdit)
          {
            case ActionGoodEditViewModel.TypePriceEdit.None:
              return;
            case ActionGoodEditViewModel.TypePriceEdit.SalePrice:
              num1 = goodStock.Price;
              break;
            case ActionGoodEditViewModel.TypePriceEdit.BuyPrice:
              num1 = buyPriceCounter.GetBuyPrice(goodStock.Uid);
              break;
            case ActionGoodEditViewModel.TypePriceEdit.LastBuyPrice:
              num1 = buyPriceCounter.GetLastBuyPrice(good);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          Decimal num2;
          switch (this.SelectedMethodPriceEdit)
          {
            case ActionGoodEditViewModel.MethodPriceEdit.None:
              return;
            case ActionGoodEditViewModel.MethodPriceEdit.Add:
              num2 = num1 + this.CoeffPrice.GetValueOrDefault();
              break;
            case ActionGoodEditViewModel.MethodPriceEdit.Minus:
              num2 = num1 - this.CoeffPrice.GetValueOrDefault();
              if (num2 < 0M)
              {
                num2 = 0M;
                break;
              }
              break;
            case ActionGoodEditViewModel.MethodPriceEdit.Multiply:
              num2 = num1 * this.CoeffPrice.GetValueOrDefault();
              break;
            case ActionGoodEditViewModel.MethodPriceEdit.Separated:
              num2 = num1 / this.CoeffPrice.GetValueOrDefault(1M);
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          Decimal num3 = this.IsRoundPrice ? HelpClassExcel.RoundSum(num2, this.RoundValue) : Math.Round(num2, 2, MidpointRounding.AwayFromZero);
          goodStock.Price = num3;
        }
      }
    }

    public enum TypePriceEdit
    {
      None,
      SalePrice,
      BuyPrice,
      LastBuyPrice,
    }

    public enum MethodPriceEdit
    {
      None,
      Add,
      Minus,
      Multiply,
      Separated,
    }
  }
}
