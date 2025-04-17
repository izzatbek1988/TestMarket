// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Excel.WaybillInExcelViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.GoodGroups;
using Gbs.Helpers;
using Gbs.Helpers.Excel;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Excel
{
  public partial class WaybillInExcelViewModel : ViewModelWithForm
  {
    private string _path;
    private IWorkbook _wb;
    private ISheet _sh;
    private ObservableCollection<WaybillInExcelViewModel.FileInfoView> _tempList = new ObservableCollection<WaybillInExcelViewModel.FileInfoView>(((IEnumerable<string>) Directory.GetFiles(ApplicationInfo.GetInstance().Paths.WaybillExcelTemplatesPath, "*.json")).ToList<string>().Select<string, WaybillInExcelViewModel.FileInfoView>((Func<string, WaybillInExcelViewModel.FileInfoView>) (x => new WaybillInExcelViewModel.FileInfoView(x))));

    public WaybillInExcelViewModel.TemplateItem Template { get; set; } = new WaybillInExcelViewModel.TemplateItem();

    public bool Result { get; set; }

    public ICommand SaveGoodsCommand { get; set; }

    public ICommand SelectedFileCommand { get; set; }

    public string Path
    {
      get => this._path;
      set
      {
        this._path = value;
        this.OnPropertyChanged(nameof (Path));
        this.OnPropertyChanged("IsEnabledData");
      }
    }

    public Dictionary<GlobalDictionaries.BarcodeIfEmpty, string> ListActionBarcodes { get; set; } = GlobalDictionaries.BarcodeActionDictionary();

    public bool IsEnabledData { get; set; }

    public List<GoodItem> ItemsGood { get; set; } = new List<GoodItem>();

    public List<GoodItem> ItemsGoodInDb { get; set; } = new List<GoodItem>();

    public ICommand SaveTemplate
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Path == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.ExcelDataViewModel_Перед_сохранением_шаблона_необходимо_выбрать_документ);
          }
          else
          {
            (bool result, string output) tuple = MessageBoxHelper.Input(this.SelectedTemplate == null ? Translate.ExcelDataViewModel_Новый_шаблон : System.IO.Path.GetFileNameWithoutExtension(this.SelectedTemplate), Translate.ExcelDataViewModel_Введите_название_шаблона, 3);
            if (!tuple.result)
              return;
            tuple.output = ApplicationInfo.GetInstance().Paths.WaybillExcelTemplatesPath + tuple.output + ".json";
            string contents = JsonConvert.SerializeObject((object) this.Template, Formatting.Indented);
            if (File.Exists(tuple.output))
            {
              if (MessageBoxHelper.Show(Translate.ExcelDataViewModel_Шаблон_с_таким_именем_существует__перезаписать_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
              File.WriteAllText(tuple.output, contents);
            }
            else
            {
              File.WriteAllText(tuple.output, contents);
              this.TemplateList.Add(new WaybillInExcelViewModel.FileInfoView(tuple.output));
            }
          }
        }));
      }
    }

    public ICommand LoadTemplate
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Path == null)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.ExcelDataViewModel_Сначала_необходимо_выбрать_документ);
          }
          else if (this.SelectedTemplate == null)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.ExcelDataViewModel_Не_выбран_шаблон_для_применения);
          }
          else
          {
            WaybillInExcelViewModel.TemplateItem template = JsonConvert.DeserializeObject<WaybillInExcelViewModel.TemplateItem>(File.ReadAllText(this.SelectedTemplate), new JsonSerializerSettings()
            {
              ObjectCreationHandling = ObjectCreationHandling.Replace
            });
            IEnumerable<EntityProperties.PropertyType> source = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
            {
              if (x.IsDeleted)
                return false;
              return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.AlcCodeUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid);
            }));
            foreach (WaybillInExcelViewModel.InfoGood infoGood1 in new List<WaybillInExcelViewModel.InfoGood>((IEnumerable<WaybillInExcelViewModel.InfoGood>) template.ValuesDataGoodList))
            {
              WaybillInExcelViewModel.InfoGood infoGood = infoGood1;
              Guid uid;
              if (Guid.TryParse(infoGood.NameDataInTemp, out uid))
              {
                if (source.All<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid != uid)))
                {
                  template.ValuesDataGoodList.RemoveAll((Predicate<WaybillInExcelViewModel.InfoGood>) (x => x.NameDataInTemp == uid.ToString()));
                  template.ParametersGoods.RemoveAll((Predicate<WaybillInExcelViewModel.InfoGood>) (x => x.NameDataInTemp == uid.ToString()));
                }
              }
              else
              {
                template.ValuesDataGoodList.RemoveAll((Predicate<WaybillInExcelViewModel.InfoGood>) (x => x.NameDataInTemp == infoGood.NameDataInTemp));
                template.ValuesDataGoodList.Add(infoGood);
              }
            }
            if (template.ValuesDataGoodList.All<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (x => x.NameDataInTemp != "Barcodes")))
              template.ValuesDataGoodList.Add(new WaybillInExcelViewModel.InfoGood(Translate.ExcelDataViewModel_Доп__штрих_коды, "Barcodes"));
            if (template.ValuesDataGoodList.All<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (x => x.NameDataInTemp != "Description")))
              template.ValuesDataGoodList.Add(new WaybillInExcelViewModel.InfoGood(Translate.ExcelDataViewModel_Описание, "Description"));
            template.ValuesDataGoodList.AddRange(source.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => template.ValuesDataGoodList.All<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (d => d.NameDataInTemp != x.Uid.ToString())))).Select<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>((Func<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>) (x => new WaybillInExcelViewModel.InfoGood(x.Name, x.Uid.ToString()))));
            template.ParametersGoods.AddRange(source.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => template.ParametersGoods.All<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (d => d.NameDataInTemp != x.Uid.ToString())))).Select<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>((Func<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>) (x => new WaybillInExcelViewModel.InfoGood(x.Name, x.Uid.ToString()))));
            int? nullable1 = template.SelectedSheetNum;
            if (nullable1.HasValue)
            {
              WaybillInExcelViewModel.TemplateItem templateItem1 = template;
              int count = this.ListSheet.Count;
              nullable1 = template.SelectedSheetNum;
              int valueOrDefault = nullable1.GetValueOrDefault();
              string selectedSheetName;
              if (!(count > valueOrDefault & nullable1.HasValue))
              {
                selectedSheetName = this.Template.SelectedSheetName;
              }
              else
              {
                List<string> listSheet = this.ListSheet;
                nullable1 = template.SelectedSheetNum;
                int index = nullable1.Value;
                selectedSheetName = listSheet[index];
              }
              templateItem1.SelectedSheetName = selectedSheetName;
              WaybillInExcelViewModel.TemplateItem templateItem2 = template;
              nullable1 = new int?();
              int? nullable2 = nullable1;
              templateItem2.SelectedSheetNum = nullable2;
              File.WriteAllText(this.SelectedTemplate, template.ToJsonString(true));
            }
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              template.EmptyGroupUid = ValidGroupUid(template.EmptyGroupUid, dataBase);
              template.DefaultGroupUid = ValidGroupUid(template.DefaultGroupUid, dataBase);
              template.ExtraGroupUid = ValidGroupUid(template.ExtraGroupUid, dataBase);
              this.Template = template;
              this.OnPropertyChanged(isUpdateAllProp: true);
            }
          }
        }));

        static Guid ValidGroupUid(Guid groupUid, Gbs.Core.Db.DataBase db)
        {
          if (groupUid != Guid.Empty)
            groupUid = db.GetTable<GOODS_GROUPS>().Any<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => x.UID == groupUid && !x.IS_DELETED)) ? groupUid : Guid.Empty;
          return groupUid;
        }
      }
    }

    public ICommand DeleteTemplate
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedTemplate == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.ExcelDataViewModel_Не_выбран_шаблон_для_применения);
          }
          else
          {
            if (MessageBoxHelper.Show(Translate.ExcelDataViewModel_Удалить_шаблон_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo) != MessageBoxResult.Yes)
              return;
            File.Delete(this.SelectedTemplate);
            this.TemplateList.Remove(this.TemplateList.First<WaybillInExcelViewModel.FileInfoView>((Func<WaybillInExcelViewModel.FileInfoView, bool>) (x => x.FileInfo.FullName == this.SelectedTemplate)));
          }
        }));
      }
    }

    private List<Gbs.Core.Entities.Goods.Good> CashDbGoods { get; set; } = new List<Gbs.Core.Entities.Goods.Good>();

    public WaybillInExcelViewModel()
    {
      this.SelectedFileCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.GetFilePath()));
      this.SelectedDefaultGroup = (ICommand) new RelayCommand((Action<object>) (obj => this.Template.DefaultGroupUid = this.GetGoodGroup()));
      this.SelectedExtraGroup = (ICommand) new RelayCommand((Action<object>) (obj => this.Template.ExtraGroupUid = this.GetGoodGroup()));
      this.SelectedEmptyGroup = (ICommand) new RelayCommand((Action<object>) (obj => this.Template.EmptyGroupUid = this.GetGoodGroup()));
      this.SaveGoodsCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.ReadExcelData()));
      this.Template.ParametersGoods.AddRange(EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
      {
        if (x.IsDeleted)
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.GoodIdUid, GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.AlcCodeUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid);
      })).Select<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>((Func<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>) (p => new WaybillInExcelViewModel.InfoGood(p.Name, p.Uid.ToString()))));
      this.Template.ValuesDataGoodList.AddRange(EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
      {
        if (x.IsDeleted)
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.AlcCodeUid, GlobalDictionaries.GoodIdUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid);
      })).Select<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>((Func<EntityProperties.PropertyType, WaybillInExcelViewModel.InfoGood>) (p => new WaybillInExcelViewModel.InfoGood(p.Name, p.Uid.ToString()))));
    }

    private void GetFilePath()
    {
      this.IsEnabledData = false;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = Translate.ExcelClientsViewModel_Книга_Excel____xlsx____xls____xlsx___xls_;
      openFileDialog1.Multiselect = false;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (openFileDialog2.ShowDialog().GetValueOrDefault())
      {
        this.IsEnabledData = true;
        this.Path = openFileDialog2.FileName;
        try
        {
          GetListSheet();
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Невозможно открыть указанный файл");
          this.Path = string.Empty;
        }
      }
      else
        this.Path = string.Empty;

      void GetListSheet()
      {
        Performancer performancer = new Performancer("Загрузка файла Excel");
        this._wb = ExcelFile.Read(this.Path);
        performancer.AddPoint("Чтение файла");
        if (this._wb == null)
        {
          this.IsEnabledData = false;
          this.Path = string.Empty;
          performancer.Stop();
        }
        else
        {
          this.Template.SelectedSheetName = this._wb.GetSheetName(this._wb.ActiveSheetIndex);
          performancer.AddPoint("Получение основного листа");
          this.ListSheet = new List<string>();
          int numberOfSheets = this._wb.NumberOfSheets;
          for (int sheet = 0; sheet < numberOfSheets; ++sheet)
            this.ListSheet.Add(this._wb.GetSheetName(sheet));
          performancer.AddPoint("Добаление листов");
          this.OnPropertyChanged("ListSheet");
          this.OnPropertyChanged(isUpdateAllProp: true);
          performancer.Stop();
        }
      }
    }

    private Guid GetGoodGroup()
    {
      Gbs.Core.Entities.GoodGroups.Group group;
      return !new FormSelectGroup().GetSingleSelectedGroupUid(this.AuthUser, out group) ? Guid.Empty : group.Uid;
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    private void ReadExcelData()
    {
      try
      {
        HelpClassExcel helpClass = new HelpClassExcel();
        if (!IsValidation())
          return;
        ProgressBarHelper.ProgressBar progressBar1 = new ProgressBarHelper.ProgressBar(Translate.WaybillInExcelViewModel_Кэширование_базы_товаров);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          this.CashDbGoods = new GoodRepository(dataBase).GetActiveItems().ToList<Gbs.Core.Entities.Goods.Good>();
        progressBar1.Close();
        ProgressBarHelper.ProgressBar progressBar2 = new ProgressBarHelper.ProgressBar(Translate.ExcelClientsViewModel_Загрузка_данных_из_Ексель);
        this._wb = ExcelFile.Read(this.Path);
        if (this._wb == null)
        {
          this.IsEnabledData = false;
          this.Path = string.Empty;
        }
        else
        {
          this._sh = this._wb.GetSheet(this.Template.SelectedSheetName);
          LogHelper.Debug(helpClass.Groups.ToJsonString(true));
          int num1 = 0;
          for (int rownum = this.Template.FirstNumRow - 1; rownum < this._sh.LastRowNum + 1; ++rownum)
          {
            ++num1;
            SaveGood(this._sh.GetRow(rownum), helpClass);
          }
          int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.ExcelDataViewModel_LoadDataFromDocument_Успешно_сохранено__0__товаров_из__1___загруженных_из_файла_, (object) this.ItemsGood.Count, (object) num1));
          progressBar2.Close();
          this.Result = true;
          this.CloseAction();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при сохранение данных из Excel");
        ProgressBarHelper.Close();
      }

      void SaveGood(IRow row, HelpClassExcel helpClass)
      {
        Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good();
        if (row == null)
          return;
        Gbs.Core.Entities.Goods.Good goodInDb = GetGoodInDb(row);
        if (goodInDb != null)
        {
          GoodItem goodItem = ConvertGood(goodInDb, row, true);
          if (goodItem == null)
            return;
          this.ItemsGood.Add(goodItem);
        }
        else
        {
          good.Name = ExcelCellValueReader.GetStringCellValue(row, this.Template.ColumnIndexName);
          if (string.IsNullOrEmpty(good.Name))
            return;
          good.Group = helpClass.GetGroup(row, this.Template.ColumnGroupIndex, this.Template.IsEnableGroup, this.Template.IsNewGroupAdd, this.Template.DefaultGroupUid, this.Template.EmptyGroupUid, this.Template.ExtraGroupUid, new List<Gbs.Core.Entities.GoodGroups.Group>());
          if (!this.Template.IsEmptyGroup && good.Group == null)
            return;
          good.Barcode = helpClass.GetBarcode(row, this.Template.ColumnIndexBarcode, this.Template.SelectedIfBarcodeEmpty);
          if (this.Template.SelectedIfBarcodeEmpty == GlobalDictionaries.BarcodeIfEmpty.Skip && string.IsNullOrEmpty(good.Barcode))
            return;
          WaybillInExcelViewModel.InfoGood infoGood1 = this.Template.ValuesDataGoodList.Single<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (x => x.NameDataInTemp == "Barcodes"));
          good.Barcodes = helpClass.GetAllBarcodes(row, infoGood1.ColumnIndex);
          if (!good.Barcodes.Any<string>() && infoGood1.IsChecked)
            return;
          WaybillInExcelViewModel.InfoGood infoGood2 = this.Template.ValuesDataGoodList.Single<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (x => x.NameDataInTemp == "Description"));
          good.Description = ExcelCellValueReader.GetStringCellValue(row, infoGood2.ColumnIndex);
          if (string.IsNullOrEmpty(good.Description) && infoGood2.IsChecked)
            return;
          List<EntityProperties.PropertyType> typesList = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good);
          foreach (WaybillInExcelViewModel.InfoGood infoGood3 in this.Template.ValuesDataGoodList.Where<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (x => Guid.TryParse(x.NameDataInTemp, out Guid _))))
          {
            WaybillInExcelViewModel.InfoGood prop = infoGood3;
            EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue()
            {
              Type = typesList.First<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid.ToString() == prop.NameDataInTemp)),
              EntityUid = good.Uid,
              Value = (object) ExcelCellValueReader.GetStringCellValue(row, prop.ColumnIndex)
            };
            if (!VerifyProp(propertyValue.Value.ToString(), propertyValue.Type.Type))
              propertyValue.Value = (object) string.Empty;
            if (string.IsNullOrEmpty(propertyValue.Value.ToString()) && prop.IsChecked)
              return;
            good.Properties.Add(propertyValue);
          }
          GoodItem goodItem = ConvertGood(good, row, false);
          if (goodItem == null)
            return;
          this.ItemsGood.Add(goodItem);
          this.ItemsGoodInDb.Add(goodItem);
        }
      }

      static bool VerifyProp(string value, GlobalDictionaries.EntityPropertyTypes type)
      {
        switch (type)
        {
          case GlobalDictionaries.EntityPropertyTypes.Text:
            return true;
          case GlobalDictionaries.EntityPropertyTypes.Integer:
            return int.TryParse(value, out int _);
          case GlobalDictionaries.EntityPropertyTypes.Decimal:
            return Decimal.TryParse(value, out Decimal _);
          case GlobalDictionaries.EntityPropertyTypes.DateTime:
            return DateTime.TryParse(value, out DateTime _);
          case GlobalDictionaries.EntityPropertyTypes.AutoNum:
            return false;
          case GlobalDictionaries.EntityPropertyTypes.System:
            return true;
          default:
            return true;
        }
      }

      Gbs.Core.Entities.Goods.Good GetGoodInDb(IRow row)
      {
        List<Gbs.Core.Entities.Goods.Good> source1 = new List<Gbs.Core.Entities.Goods.Good>();
        foreach (WaybillInExcelViewModel.InfoGood infoGood in this.Template.ParametersGoods.Where<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (x => x.ColumnIndex > 0)))
        {
          string str = ExcelCellValueReader.GetStringCellValue(row, infoGood.ColumnIndex);
          if (str != null && !str.IsNullOrEmpty())
          {
            str = str.Trim();
            Guid guid;
            if (Guid.TryParse(infoGood.NameDataInTemp, out guid))
            {
              source1.AddRange(this.CashDbGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => object.Equals((object) x.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid == guid))?.Value.ToString().Trim(), (object) str))));
            }
            else
            {
              switch (infoGood.NameDataInTemp)
              {
                case "Name":
                  source1.AddRange(this.CashDbGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => string.Equals(x.Name.Trim(), str, StringComparison.CurrentCultureIgnoreCase))));
                  break;
                case "Barcode":
                  source1.AddRange(this.CashDbGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => string.Equals(x.Barcode?.Trim(), str, StringComparison.CurrentCultureIgnoreCase))));
                  break;
                case "Description":
                  source1.AddRange(this.CashDbGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => string.Equals(x.Description?.Trim(), str, StringComparison.CurrentCultureIgnoreCase))));
                  break;
                case "Group":
                  source1.AddRange(this.CashDbGoods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => string.Equals(x.Group.Name?.Trim(), str, StringComparison.CurrentCultureIgnoreCase))));
                  break;
              }
            }
            switch (this.Template.SelectedConditionСompare)
            {
              case WaybillInExcelViewModel.TypeConditions.One:
                if (source1.Any<Gbs.Core.Entities.Goods.Good>())
                  return source1.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (st => st.Stock > 0M)))) ?? source1.First<Gbs.Core.Entities.Goods.Good>();
                continue;
              case WaybillInExcelViewModel.TypeConditions.SameEvery:
                if (!source1.Any<Gbs.Core.Entities.Goods.Good>())
                  return (Gbs.Core.Entities.Goods.Good) null;
                continue;
              default:
                continue;
            }
          }
        }
        IGrouping<Guid, Gbs.Core.Entities.Goods.Good> source2 = source1.GroupBy<Gbs.Core.Entities.Goods.Good, Guid>((Func<Gbs.Core.Entities.Goods.Good, Guid>) (x => x.Uid)).FirstOrDefault<IGrouping<Guid, Gbs.Core.Entities.Goods.Good>>((Func<IGrouping<Guid, Gbs.Core.Entities.Goods.Good>, bool>) (x => x.Count<Gbs.Core.Entities.Goods.Good>() == this.Template.ParametersGoods.Count<WaybillInExcelViewModel.InfoGood>((Func<WaybillInExcelViewModel.InfoGood, bool>) (p => p.ColumnIndex > 0))));
        Gbs.Core.Entities.Goods.Good goodInDb = source2 != null ? source2.FirstOrDefault<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Any<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (st => st.Stock > 0M)))) : (Gbs.Core.Entities.Goods.Good) null;
        if (goodInDb != null)
          return goodInDb;
        return source2 == null ? (Gbs.Core.Entities.Goods.Good) null : source2.First<Gbs.Core.Entities.Goods.Good>();
      }

      (Decimal? price, Decimal? stock, Decimal? buyPrice) GetGoodStock(IRow row)
      {
        Decimal? cellValue1 = (Decimal?) ExcelCellValueReader.GetCellValue(row, this.Template.ColumnStockIndex, ExcelCellValueReader.CellValueTypes.Decimal, this.Template.IsNextIfEmptyStock);
        Decimal? nullable1;
        Decimal? nullable2;
        if (this.Template.IsCheckedBuyPrice)
        {
          nullable1 = (Decimal?) ExcelCellValueReader.GetCellValue(row, this.Template.ColumnBuyPriceIndex, ExcelCellValueReader.CellValueTypes.Decimal, this.Template.IsNextIfEmptyBuyPrice);
        }
        else
        {
          Decimal? cellValue2 = (Decimal?) ExcelCellValueReader.GetCellValue(row, this.Template.ColumnBuySumIndex, ExcelCellValueReader.CellValueTypes.Decimal, this.Template.IsNextIfEmptyBuySum);
          if (!cellValue2.HasValue)
          {
            nullable1 = new Decimal?();
          }
          else
          {
            Decimal? nullable3;
            if (!(cellValue1.GetValueOrDefault() <= 0M))
            {
              Decimal? nullable4 = cellValue2;
              nullable2 = cellValue1;
              nullable3 = nullable4.HasValue & nullable2.HasValue ? new Decimal?(nullable4.GetValueOrDefault() / nullable2.GetValueOrDefault()) : new Decimal?();
            }
            else
              nullable3 = new Decimal?(0M);
            nullable1 = nullable3;
          }
        }
        Decimal? nullable5 = this.Template.IsPriceInExcel ? (Decimal?) ExcelCellValueReader.GetCellValue(row, this.Template.ColumnSalePriceIndex, ExcelCellValueReader.CellValueTypes.Decimal) : new Decimal?(this.Template.IsRoundSalePrice ? HelpClassExcel.RoundSum(nullable1.GetValueOrDefault() * this.Template.MultiplierSale, this.Template.RoundValue) : Math.Round(nullable1.GetValueOrDefault() * this.Template.MultiplierSale, 2));
        Decimal? nullable6 = cellValue1;
        Decimal? nullable7;
        if (nullable1.HasValue)
        {
          nullable7 = new Decimal?(Math.Round(nullable1.GetValueOrDefault(), 4, MidpointRounding.AwayFromZero));
        }
        else
        {
          nullable2 = new Decimal?();
          nullable7 = nullable2;
        }
        return (nullable5, nullable6, nullable7);
      }

      GoodItem ConvertGood(Gbs.Core.Entities.Goods.Good good, IRow row, bool oldGood)
      {
        (Decimal? price, Decimal? stock, Decimal? buyPrice) = GetGoodStock(row);
        if (!price.HasValue && this.Template.IsNextIfEmptySalePrice)
          return (GoodItem) null;
        if (!stock.HasValue && this.Template.IsNextIfEmptyStock)
          return (GoodItem) null;
        if (!buyPrice.HasValue && (this.Template.IsNextIfEmptyBuySum || this.Template.IsNextIfEmptyBuyPrice))
          return (GoodItem) null;
        GoodItem goodItem1 = GoodItem.ItemForBuy(good, stock.GetValueOrDefault(), buyPrice.GetValueOrDefault());
        if (this.Template.IsNoEditSalePrice & oldGood)
        {
          GoodItem goodItem2 = goodItem1;
          List<GoodsStocks.GoodStock> stocksAndPrices1 = good.StocksAndPrices;
          Decimal num;
          if ((stocksAndPrices1 != null ? (stocksAndPrices1.Any<GoodsStocks.GoodStock>() ? 1 : 0) : 0) == 0)
          {
            num = 0M;
          }
          else
          {
            List<GoodsStocks.GoodStock> stocksAndPrices2 = good.StocksAndPrices;
            num = (stocksAndPrices2 != null ? new Decimal?(stocksAndPrices2.Max<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Price))) : new Decimal?()).Value;
          }
          goodItem2.SalePrice = num;
        }
        else
          goodItem1.SalePrice = price.GetValueOrDefault();
        goodItem1.GoodStock = new GoodsStocks.GoodStock()
        {
          Price = goodItem1.SalePrice,
          Stock = stock.GetValueOrDefault(),
          GoodUid = good.Uid
        };
        return goodItem1;
      }

      bool IsValidation()
      {
        List<string> stringList = new List<string>();
        if (this.Path.IsNullOrEmpty())
        {
          MessageBoxHelper.Warning(Translate.WaybillInExcelViewModel_Требуется_указать_путь_к_файлу);
          return false;
        }
        if ((this.Template.SelectedSheetNum.GetValueOrDefault() == -1 || !this.Template.SelectedSheetNum.HasValue) && this.Template.SelectedSheetName.IsNullOrEmpty())
        {
          MessageBoxHelper.Warning(Translate.WaybillInExcelViewModel_ReadExcelData_Требуется_выбрать_лист_с_данными__где_находится_информация_о_товарах_);
          return false;
        }
        if (this.Template.IsGroupSet && this.Template.DefaultGroupUid == Guid.Empty)
        {
          int num = (int) MessageBoxHelper.Show(Translate.WaybillInExcelViewModel_Требуется_указать_категорию__в_которую_будут_загружены_товары, icon: MessageBoxImage.Exclamation);
          return false;
        }
        if (this.Template.IsEnableGroup)
        {
          if (this.Template.IsEmptyGroup && this.Template.EmptyGroupUid == Guid.Empty)
            stringList.Add(Translate.WaybillInExcelViewModel_Требуется_указать_категорию_при_пустом_значении_ячейки);
          if (this.Template.IsGroupSetIfNew && this.Template.ExtraGroupUid == Guid.Empty)
            stringList.Add(Translate.WaybillInExcelViewModel_Требуется_указать_категорию__если_категории_из_ячейки_нет_в_документе);
        }
        if (!stringList.Any<string>())
          return true;
        int num1 = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) stringList));
        return false;
      }
    }

    public ICommand SelectedDefaultGroup { get; set; }

    public ICommand SelectedExtraGroup { get; set; }

    public ICommand SelectedEmptyGroup { get; set; }

    public string SelectedTemplate { get; set; }

    public ObservableCollection<WaybillInExcelViewModel.FileInfoView> TemplateList
    {
      get => this._tempList;
      set
      {
        this._tempList = value;
        this.OnPropertyChanged(nameof (TemplateList));
      }
    }

    public List<string> ListSheet { get; set; }

    public List<WaybillInExcelViewModel.ConditionsСompare> ConditionsСompareList { get; set; } = new List<WaybillInExcelViewModel.ConditionsСompare>()
    {
      new WaybillInExcelViewModel.ConditionsСompare()
      {
        Name = Translate.WaybillInExcelViewModel_ConditionsСompareList_Совпадают_все_параметры,
        Conditions = WaybillInExcelViewModel.TypeConditions.SameEvery
      },
      new WaybillInExcelViewModel.ConditionsСompare()
      {
        Name = Translate.WaybillInExcelViewModel_ConditionСompare_Совпадает_хотя_бы_один_параметр,
        Conditions = WaybillInExcelViewModel.TypeConditions.One
      }
    };

    public class TemplateItem : ViewModel
    {
      [JsonIgnore]
      private int _columnIndexName;
      [JsonIgnore]
      private int _columnIndexBarcode;
      [JsonIgnore]
      private Guid _defaultGroupUid = Guid.Empty;
      [JsonIgnore]
      private Guid _extraGroupUid = Guid.Empty;
      [JsonIgnore]
      private Guid _emptyGroupUid = Guid.Empty;
      [JsonIgnore]
      private bool _isEnable;
      [JsonIgnore]
      private Visibility _isVisibilityGroup = Visibility.Collapsed;
      [JsonIgnore]
      private int _columnGroupIndex;
      [JsonIgnore]
      private int _columnStockIndex;
      [JsonIgnore]
      private int _columnBuyPriceIndex;
      [JsonIgnore]
      private int _columnBuySumIndex;
      [JsonIgnore]
      private int _columnSalePriceIndex;

      public int? SelectedSheetNum { get; set; }

      public string SelectedSheetName { get; set; }

      public int FirstNumRow { get; set; } = 1;

      public List<WaybillInExcelViewModel.InfoGood> ValuesDataGoodList { get; set; } = new List<WaybillInExcelViewModel.InfoGood>()
      {
        new WaybillInExcelViewModel.InfoGood(Translate.ExcelDataViewModel_Доп__штрих_коды, "Barcodes"),
        new WaybillInExcelViewModel.InfoGood(Translate.ExcelDataViewModel_Описание, "Description")
      };

      public int ColumnIndexName
      {
        get => this._columnIndexName;
        set
        {
          this._columnIndexName = value;
          this.OnPropertyChanged("ColumnNameIndexChar");
        }
      }

      [JsonIgnore]
      public string ColumnNameIndexChar => ExcelHelper.ColumnNumberToAbc(this.ColumnIndexName);

      public WaybillInExcelViewModel.TypeConditions SelectedConditionСompare { get; set; } = WaybillInExcelViewModel.TypeConditions.SameEvery;

      public List<WaybillInExcelViewModel.InfoGood> ParametersGoods { get; set; } = new List<WaybillInExcelViewModel.InfoGood>()
      {
        new WaybillInExcelViewModel.InfoGood(Translate.FrmClientCard_Наименование, "Name"),
        new WaybillInExcelViewModel.InfoGood(Translate.FrmAuthorization_ШтрихКод, "Barcode"),
        new WaybillInExcelViewModel.InfoGood(Translate.FrmGoodModificationCard_Описание, "Description"),
        new WaybillInExcelViewModel.InfoGood(Translate.Client_Категория, "Group")
      };

      public int ColumnIndexBarcode
      {
        get => this._columnIndexBarcode;
        set
        {
          this._columnIndexBarcode = value;
          this.OnPropertyChanged("ColumnBarcodeIndexChar");
        }
      }

      [JsonIgnore]
      public string ColumnBarcodeIndexChar
      {
        get => ExcelHelper.ColumnNumberToAbc(this.ColumnIndexBarcode);
      }

      public GlobalDictionaries.BarcodeIfEmpty SelectedIfBarcodeEmpty { get; set; }

      [JsonIgnore]
      public Dictionary<GlobalDictionaries.BarcodeIfEmpty, string> ListActionBarcodes { get; set; } = GlobalDictionaries.BarcodeActionDictionary();

      public Guid DefaultGroupUid
      {
        get => this._defaultGroupUid;
        set
        {
          this._defaultGroupUid = value;
          this.OnPropertyChanged("GroupNameDefault");
        }
      }

      [JsonIgnore]
      public string GroupNameDefault
      {
        get
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            Gbs.Core.Entities.GoodGroups.Group byUid = new GoodGroupsRepository(dataBase).GetByUid(this.DefaultGroupUid);
            if (!(this.DefaultGroupUid != Guid.Empty) || byUid != null)
              return this.DefaultGroupUid == Guid.Empty ? Translate.TemplateExcelFile_Выбрать : byUid.Name;
            this.DefaultGroupUid = Guid.Empty;
            return Translate.TemplateExcelFile_Выбрать;
          }
        }
      }

      public Guid ExtraGroupUid
      {
        get => this._extraGroupUid;
        set
        {
          this._extraGroupUid = value;
          this.OnPropertyChanged("GroupNameExtra");
        }
      }

      [JsonIgnore]
      public string GroupNameExtra
      {
        get
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            Gbs.Core.Entities.GoodGroups.Group byUid = new GoodGroupsRepository(dataBase).GetByUid(this.ExtraGroupUid);
            if (!(this.ExtraGroupUid != Guid.Empty) || byUid != null)
              return this.ExtraGroupUid == Guid.Empty ? Translate.TemplateExcelFile_Выбрать : byUid.Name;
            this.ExtraGroupUid = Guid.Empty;
            return Translate.TemplateExcelFile_Выбрать;
          }
        }
      }

      public Guid EmptyGroupUid
      {
        get => this._emptyGroupUid;
        set
        {
          this._emptyGroupUid = value;
          this.OnPropertyChanged("GroupNameEmpty");
        }
      }

      [JsonIgnore]
      public string GroupNameEmpty
      {
        get
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            Gbs.Core.Entities.GoodGroups.Group byUid = new GoodGroupsRepository(dataBase).GetByUid(this.EmptyGroupUid);
            if (!(this.EmptyGroupUid != Guid.Empty) || byUid != null)
              return this.EmptyGroupUid == Guid.Empty ? Translate.TemplateExcelFile_Выбрать : byUid.Name;
            this.EmptyGroupUid = Guid.Empty;
            return Translate.TemplateExcelFile_Выбрать;
          }
        }
      }

      public bool IsEnableGroup
      {
        get => this._isEnable;
        set
        {
          this._isEnable = value;
          this.IsVisibilityGroup = value ? Visibility.Visible : Visibility.Collapsed;
          this.OnPropertyChanged(nameof (IsEnableGroup));
        }
      }

      public Visibility IsVisibilityGroup
      {
        get => this._isVisibilityGroup;
        set
        {
          this._isVisibilityGroup = value;
          this.OnPropertyChanged(nameof (IsVisibilityGroup));
        }
      }

      public int ColumnGroupIndex
      {
        get => this._columnGroupIndex;
        set
        {
          this._columnGroupIndex = value;
          this.OnPropertyChanged("ColumnGroupIndexChar");
        }
      }

      [JsonIgnore]
      public string ColumnGroupIndexChar => ExcelHelper.ColumnNumberToAbc(this.ColumnGroupIndex);

      public bool IsGroupSet { get; set; } = true;

      public bool IsGroupSetIfNew { get; set; }

      public bool IsGroupNextIfEmpty { get; set; } = true;

      public bool IsEmptyGroup { get; set; }

      public bool IsNewGroupAdd { get; set; } = true;

      public int ColumnStockIndex
      {
        get => this._columnStockIndex;
        set
        {
          this._columnStockIndex = value;
          this.OnPropertyChanged("ColumnStockIndexChar");
        }
      }

      [JsonIgnore]
      public string ColumnStockIndexChar => ExcelHelper.ColumnNumberToAbc(this.ColumnStockIndex);

      public bool IsNextIfEmptyStock { get; set; }

      public bool IsCheckedBuyPrice { get; set; } = true;

      public bool IsCheckedBuySum { get; set; }

      public int ColumnBuyPriceIndex
      {
        get => this._columnBuyPriceIndex;
        set
        {
          this._columnBuyPriceIndex = value;
          this.OnPropertyChanged("ColumnBuyPriceIndexChar");
        }
      }

      [JsonIgnore]
      public string ColumnBuyPriceIndexChar
      {
        get => ExcelHelper.ColumnNumberToAbc(this.ColumnBuyPriceIndex);
      }

      public bool IsNextIfEmptyBuyPrice { get; set; }

      public int ColumnBuySumIndex
      {
        get => this._columnBuySumIndex;
        set
        {
          this._columnBuySumIndex = value;
          this.OnPropertyChanged("ColumnBuySumIndexChar");
        }
      }

      [JsonIgnore]
      public string ColumnBuySumIndexChar => ExcelHelper.ColumnNumberToAbc(this.ColumnBuySumIndex);

      public bool IsNextIfEmptyBuySum { get; set; }

      public bool IsPriceInExcel { get; set; } = true;

      public int ColumnSalePriceIndex
      {
        get => this._columnSalePriceIndex;
        set
        {
          this._columnSalePriceIndex = value;
          this.OnPropertyChanged("ColumnSalePriceIndexChar");
        }
      }

      [JsonIgnore]
      public string ColumnSalePriceIndexChar
      {
        get => ExcelHelper.ColumnNumberToAbc(this.ColumnSalePriceIndex);
      }

      public bool IsNextIfEmptySalePrice { get; set; }

      public bool IsCounterPrice { get; set; }

      public Decimal MultiplierSale { get; set; } = 1M;

      public Decimal RoundValue { get; set; }

      public bool IsNoEditSalePrice { get; set; }

      public bool IsRoundSalePrice { get; set; }
    }

    public class InfoGood : ViewModelWithForm
    {
      private int _columnIndex;
      private bool _isChecked;

      public string NameData { get; set; }

      public string NameDataInTemp { get; set; }

      public int ColumnIndex
      {
        get => this._columnIndex;
        set
        {
          this._columnIndex = value;
          this.OnPropertyChanged("ColumnIndexText");
        }
      }

      public string ColumnIndexText => ExcelHelper.ColumnNumberToAbc(this.ColumnIndex);

      public bool IsChecked
      {
        get => this._isChecked;
        set
        {
          this._isChecked = value;
          this.OnPropertyChanged(nameof (IsChecked));
        }
      }

      public InfoGood(string name, string nameInTemp)
      {
        this.NameData = name;
        this.NameDataInTemp = nameInTemp;
      }
    }

    public class FileInfoView
    {
      public string Name => System.IO.Path.GetFileNameWithoutExtension(this.FileInfo?.FullName) ?? "";

      public FileInfo FileInfo { get; set; }

      public FileInfoView(string path) => this.FileInfo = new FileInfo(path);
    }

    public class ConditionsСompare
    {
      public string Name { get; set; }

      public WaybillInExcelViewModel.TypeConditions Conditions { get; set; }
    }

    public enum TypeConditions
    {
      One = 1,
      SameEvery = 2,
    }
  }
}
