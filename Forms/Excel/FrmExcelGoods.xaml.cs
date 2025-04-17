// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Excel.ExcelDataViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.GoodGroups;
using Gbs.Helpers;
using Gbs.Helpers.Excel;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Excel
{
  public partial class ExcelDataViewModel : ViewModelWithForm
  {
    private string _path;
    private IWorkbook _wb;
    private ISheet _sh;
    private IRow _iRow;
    private ObservableCollection<WaybillInExcelViewModel.FileInfoView> _tempList;

    public ExcelDataViewModel.TemplateExcelFile TemplateExcel { get; set; } = new ExcelDataViewModel.TemplateExcelFile();

    public ICommand SaveGoods { get; set; }

    public ICommand CancelCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (MessageBoxHelper.Show(Translate.ExcelDataViewModel_CancelCommand_Уверены__что_хотите_отменить_процесс_добавления_товаров_из_Excel_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
            return;
          this.CloseAction();
        }));
      }
    }

    public Dictionary<GlobalDictionaries.BarcodeIfEmpty, string> ListActionBarcode { get; set; } = GlobalDictionaries.BarcodeActionDictionary();

    public bool IsEnabledData { get; set; }

    public string SelectedTemplate { get; set; }

    public List<Gbs.Core.Entities.Goods.Good> GoodsList { get; set; } = new List<Gbs.Core.Entities.Goods.Good>();

    public ExcelDataViewModel()
    {
      this.ShowProperty();
      this.GetSourseListTemplate();
      this.SaveGoods = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      this.SelectFileExcel = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        this.IsEnabledData = false;
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
          Filter = Translate.ExcelClientsViewModel_Книга_Excel____xlsx____xls____xlsx___xls_,
          Multiselect = false
        };
        if (openFileDialog.ShowDialog().GetValueOrDefault())
        {
          this.IsEnabledData = true;
          this.ExcelFilePath = openFileDialog.FileName;
          this.GetListSheet();
        }
        else
          this.ExcelFilePath = string.Empty;
      }));
      this.SelectedDefaultGroup = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Gbs.Core.Entities.GoodGroups.Group group;
        if (!new FormSelectGroup().GetSingleSelectedGroupUid(this.AuthUser, out group))
          return;
        this.TemplateExcel.DefaultGroupUid = group.Uid;
      }));
      this.SelectExtraGroups = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Gbs.Core.Entities.GoodGroups.Group group;
        if (!new FormSelectGroup().GetSingleSelectedGroupUid(this.AuthUser, out group))
          return;
        this.TemplateExcel.ExtraGroupUid = group.Uid;
      }));
      this.SelectEmptyGroups = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Gbs.Core.Entities.GoodGroups.Group group;
        if (!new FormSelectGroup().GetSingleSelectedGroupUid(this.AuthUser, out group))
          return;
        this.TemplateExcel.EmptyGroupUid = group.Uid;
      }));
      this.SaveTemplate = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.ExcelFilePath == null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.ExcelDataViewModel_Перед_сохранением_шаблона_необходимо_выбрать_документ);
        }
        else
        {
          (bool result, string output) tuple = MessageBoxHelper.Input(Translate.ExcelDataViewModel_Новый_шаблон, Translate.ExcelDataViewModel_Введите_название_шаблона, 3);
          if (!tuple.result)
            return;
          tuple.output = ApplicationInfo.GetInstance().Paths.GoodCatalogExcelTemplatesPath + tuple.output + ".json";
          string contents = JsonConvert.SerializeObject((object) this.TemplateExcel, Formatting.Indented);
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
      this.LoadTemplate = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.ExcelFilePath == null)
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.ExcelDataViewModel_Сначала_необходимо_выбрать_документ);
        }
        else if (this.SelectedTemplate == null)
        {
          int num2 = (int) MessageBoxHelper.Show(Translate.ExcelDataViewModel_Не_выбран_шаблон_для_применения);
        }
        else
        {
          ExcelDataViewModel.TemplateExcelFile template = JsonConvert.DeserializeObject<ExcelDataViewModel.TemplateExcelFile>(File.ReadAllText(this.SelectedTemplate), new JsonSerializerSettings()
          {
            ObjectCreationHandling = ObjectCreationHandling.Replace
          });
          List<EntityProperties.PropertyType> list = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted && x.Uid != GlobalDictionaries.CertificateNominalUid && x.Uid != GlobalDictionaries.CertificateReusableUid)).ToList<EntityProperties.PropertyType>();
          foreach (ExcelDataViewModel.TemplateExcelFile.InfoGood infoGood in new List<ExcelDataViewModel.TemplateExcelFile.InfoGood>((IEnumerable<ExcelDataViewModel.TemplateExcelFile.InfoGood>) template.ValuesDataGoodList))
          {
            Guid uid;
            if (Guid.TryParse(infoGood.NameDataInTemp, out uid) && list.All<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid != uid)))
              template.ValuesDataGoodList.RemoveAll((Predicate<ExcelDataViewModel.TemplateExcelFile.InfoGood>) (x => x.NameDataInTemp == uid.ToString()));
          }
          template.ValuesDataGoodList.AddRange(list.Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => template.ValuesDataGoodList.All<ExcelDataViewModel.TemplateExcelFile.InfoGood>((Func<ExcelDataViewModel.TemplateExcelFile.InfoGood, bool>) (d => d.NameDataInTemp != x.Uid.ToString())))).Select<EntityProperties.PropertyType, ExcelDataViewModel.TemplateExcelFile.InfoGood>((Func<EntityProperties.PropertyType, ExcelDataViewModel.TemplateExcelFile.InfoGood>) (x => new ExcelDataViewModel.TemplateExcelFile.InfoGood(x.Name, x.Uid.ToString()))));
          this.TemplateExcel = template;
          this.OnPropertyChanged(isUpdateAllProp: true);
        }
      }));
      this.DeleteTemplate = (ICommand) new RelayCommand((Action<object>) (obj =>
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

    private bool LoadDataFromDocument()
    {
      if (!IsValidation())
        return false;
      this._wb = ExcelFile.Read(this.ExcelFilePath);
      if (this._wb == null)
      {
        this.IsEnabledData = false;
        this.ExcelFilePath = string.Empty;
        return false;
      }
      this._sh = this._wb.GetSheetAt(this.TemplateExcel.SelectedSheetNum);
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ExcelDataViewModel_LoadDataFromDocument_Загрузка_наименований_из_файла);
      HelpClassExcel helpClassExcel = new HelpClassExcel();
      List<Gbs.Core.Entities.GoodGroups.Group> groupList = new List<Gbs.Core.Entities.GoodGroups.Group>();
      for (int rownum = this.TemplateExcel.FirstNumRow - 1; rownum < this._sh.LastRowNum + 1; ++rownum)
      {
        this._iRow = this._sh.GetRow(rownum);
        if (this._iRow != null)
        {
          string stringCellValue = ExcelCellValueReader.GetStringCellValue(this._iRow, this.TemplateExcel.ColumnIndexName);
          if (!string.IsNullOrEmpty(stringCellValue))
          {
            Gbs.Core.Entities.GoodGroups.Group group = helpClassExcel.GetGroup(this._iRow, this.TemplateExcel.ColumnGroup, this.TemplateExcel.IsEnableGroup, this.TemplateExcel.IsNewGroupAdd, this.TemplateExcel.DefaultGroupUid, this.TemplateExcel.EmptyGroupUid, this.TemplateExcel.ExtraGroupUid, groupList);
            if (this.TemplateExcel.IsEmptyGroup || group != null)
            {
              string barcode = helpClassExcel.GetBarcode(this._iRow, this.TemplateExcel.ColumnIndexBarcode, this.TemplateExcel.SelectedIfBarcodeEmpty);
              if (this.TemplateExcel.SelectedIfBarcodeEmpty != GlobalDictionaries.BarcodeIfEmpty.Skip || !string.IsNullOrEmpty(barcode))
              {
                Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good()
                {
                  Name = stringCellValue,
                  Barcode = barcode,
                  Group = group
                };
                ExcelDataViewModel.TemplateExcelFile.InfoGood infoGood1 = this.TemplateExcel.ValuesDataGoodList.Single<ExcelDataViewModel.TemplateExcelFile.InfoGood>((Func<ExcelDataViewModel.TemplateExcelFile.InfoGood, bool>) (x => x.NameDataInTemp == "Barcodes"));
                good.Barcodes = helpClassExcel.GetAllBarcodes(this._iRow, infoGood1.ColumnIndex);
                if (good.Barcodes.Any<string>() || !infoGood1.IsChecked)
                {
                  ExcelDataViewModel.TemplateExcelFile.InfoGood infoGood2 = this.TemplateExcel.ValuesDataGoodList.Single<ExcelDataViewModel.TemplateExcelFile.InfoGood>((Func<ExcelDataViewModel.TemplateExcelFile.InfoGood, bool>) (x => x.NameDataInTemp == "Description"));
                  good.Description = ExcelCellValueReader.GetStringCellValue(this._iRow, infoGood2.ColumnIndex);
                  if (!string.IsNullOrEmpty(good.Description) || !infoGood2.IsChecked)
                  {
                    bool flag = true;
                    foreach (ExcelDataViewModel.TemplateExcelFile.InfoGood infoGood3 in this.TemplateExcel.ValuesDataGoodList.Where<ExcelDataViewModel.TemplateExcelFile.InfoGood>((Func<ExcelDataViewModel.TemplateExcelFile.InfoGood, bool>) (x => Guid.TryParse(x.NameDataInTemp, out Guid _))))
                    {
                      ExcelDataViewModel.TemplateExcelFile.InfoGood prop = infoGood3;
                      EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue()
                      {
                        Type = EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Single<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid.ToString() == prop.NameDataInTemp)),
                        EntityUid = good.Uid,
                        Value = (object) ExcelCellValueReader.GetStringCellValue(this._iRow, prop.ColumnIndex)
                      };
                      if (string.IsNullOrEmpty(propertyValue.Value.ToString()) && prop.IsChecked)
                      {
                        flag = false;
                        break;
                      }
                      good.Properties.Add(propertyValue);
                    }
                    if (flag)
                      this.GoodsList.Add(good);
                  }
                }
              }
            }
          }
        }
      }
      new Stopwatch().Start();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        GlobalData.IsMarket5ImportAcitve = true;
        int num1 = new GoodRepository(dataBase).Save(this.GoodsList);
        new GoodGroupsRepository(dataBase).Save(groupList);
        GlobalData.IsMarket5ImportAcitve = false;
        progressBar.Close();
        int num2 = (int) MessageBoxHelper.Show(string.Format(Translate.ExcelDataViewModel_LoadDataFromDocument_Успешно_сохранено__0__товаров_из__1___загруженных_из_файла_, (object) num1, (object) this.GoodsList.Count));
        foreach (Gbs.Core.Entities.Goods.Good goods in this.GoodsList)
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) goods, (IEntity) goods, ActionType.Add, GlobalDictionaries.EntityTypes.Good, this.AuthUser), true);
        return true;
      }

      bool IsValidation()
      {
        List<string> stringList = new List<string>();
        if (this.ExcelFilePath.IsNullOrEmpty())
        {
          int num = (int) MessageBoxHelper.Show(Translate.WaybillInExcelViewModel_Требуется_указать_путь_к_файлу, icon: MessageBoxImage.Exclamation);
          return false;
        }
        if (this.TemplateExcel.SelectedSheetNum == -1)
        {
          MessageBoxHelper.Warning(Translate.WaybillInExcelViewModel_ReadExcelData_Требуется_выбрать_лист_с_данными__где_находится_информация_о_товарах_);
          return false;
        }
        if (this.TemplateExcel.IsGroupSet && this.TemplateExcel.DefaultGroupUid == Guid.Empty)
        {
          int num = (int) MessageBoxHelper.Show(Translate.WaybillInExcelViewModel_Требуется_указать_категорию__в_которую_будут_загружены_товары, icon: MessageBoxImage.Exclamation);
          return false;
        }
        if (!this.TemplateExcel.IsGroupSet)
        {
          if (this.TemplateExcel.IsEmptyGroup && this.TemplateExcel.EmptyGroupUid == Guid.Empty)
            stringList.Add(Translate.WaybillInExcelViewModel_Требуется_указать_категорию_при_пустом_значении_ячейки);
          if (this.TemplateExcel.IsGroupSetIfNew && this.TemplateExcel.ExtraGroupUid == Guid.Empty)
            stringList.Add(Translate.WaybillInExcelViewModel_Требуется_указать_категорию__если_категории_из_ячейки_нет_в_документе);
        }
        if (!stringList.Any<string>())
          return true;
        int num1 = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) stringList));
        return false;
      }
    }

    private void ShowProperty()
    {
      this.TemplateExcel.ValuesDataGoodList.AddRange(EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
      {
        if (x.IsDeleted)
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.GoodIdUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid, GlobalDictionaries.AlcCodeUid);
      })).Select<EntityProperties.PropertyType, ExcelDataViewModel.TemplateExcelFile.InfoGood>((Func<EntityProperties.PropertyType, ExcelDataViewModel.TemplateExcelFile.InfoGood>) (p => new ExcelDataViewModel.TemplateExcelFile.InfoGood(p.Name, p.Uid.ToString()))));
    }

    private void GetListSheet()
    {
      this._wb = ExcelFile.Read(this.ExcelFilePath);
      if (this._wb == null)
      {
        this.IsEnabledData = false;
        this.ExcelFilePath = string.Empty;
      }
      else
      {
        this.TemplateExcel.SelectedSheetNum = this._wb.ActiveSheetIndex;
        this.ListSheet = new List<string>();
        int numberOfSheets = this._wb.NumberOfSheets;
        for (int sheet = 0; sheet < numberOfSheets; ++sheet)
          this.ListSheet.Add(this._wb.GetSheetName(sheet));
        this.OnPropertyChanged("ListSheet");
        this.OnPropertyChanged("SelectedSheetNum");
      }
    }

    private void GetSourseListTemplate()
    {
      this.TemplateList = new ObservableCollection<WaybillInExcelViewModel.FileInfoView>();
      ((IEnumerable<string>) Directory.GetFiles(ApplicationInfo.GetInstance().Paths.GoodCatalogExcelTemplatesPath)).ToList<string>().ForEach((Action<string>) (x => this.TemplateList.Add(new WaybillInExcelViewModel.FileInfoView(x))));
    }

    private void Save()
    {
      if (!new bool?(this.LoadDataFromDocument()).GetValueOrDefault())
        return;
      this.CloseAction();
    }

    public string ExcelFilePath
    {
      get => this._path;
      set
      {
        this._path = value;
        this.OnPropertyChanged(nameof (ExcelFilePath));
        this.OnPropertyChanged("IsEnabledData");
      }
    }

    public ICommand SelectFileExcel { get; set; }

    public ICommand SelectedDefaultGroup { get; set; }

    public ICommand SelectEmptyGroups { get; set; }

    public ICommand SelectExtraGroups { get; set; }

    public ICommand SaveTemplate { get; set; }

    public ICommand LoadTemplate { get; set; }

    public ICommand DeleteTemplate { get; set; }

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

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public class TemplateExcelFile : ViewModelWithForm
    {
      private int _columnIndexName;
      private int _columnIndexBarcode;
      private Guid _defaultGroupUid = Guid.Empty;
      private Guid _extraGroupUid = Guid.Empty;
      private Guid _emptyGroupUid = Guid.Empty;
      private bool _isEnable;
      private Visibility _isVis = Visibility.Collapsed;
      private int _columnGroup;

      public int FirstNumRow { get; set; } = 1;

      public int SelectedSheetNum { get; set; }

      public List<ExcelDataViewModel.TemplateExcelFile.InfoGood> ValuesDataGoodList { get; set; } = new List<ExcelDataViewModel.TemplateExcelFile.InfoGood>()
      {
        new ExcelDataViewModel.TemplateExcelFile.InfoGood(Translate.ExcelDataViewModel_Доп__штрих_коды, "Barcodes"),
        new ExcelDataViewModel.TemplateExcelFile.InfoGood(Translate.ExcelDataViewModel_Описание, "Description")
      };

      public int ColumnIndexName
      {
        get => this._columnIndexName;
        set
        {
          this._columnIndexName = value;
          this.ColumnNameIndexChar = ExcelHelper.ColumnNumberToAbc(value);
          this.OnPropertyChanged(nameof (ColumnIndexName));
          this.OnPropertyChanged("ColumnNameIndexChar");
        }
      }

      public string ColumnNameIndexChar { get; set; } = string.Empty;

      public int ColumnIndexBarcode
      {
        get => this._columnIndexBarcode;
        set
        {
          this._columnIndexBarcode = value;
          this.ColumnBarcodeIndexChar = ExcelHelper.ColumnNumberToAbc(value);
          this.OnPropertyChanged(nameof (ColumnIndexBarcode));
          this.OnPropertyChanged("ColumnBarcodeIndexChar");
        }
      }

      public string ColumnBarcodeIndexChar { get; set; } = string.Empty;

      public GlobalDictionaries.BarcodeIfEmpty SelectedIfBarcodeEmpty { get; set; }

      public Guid DefaultGroupUid
      {
        get => this._defaultGroupUid;
        set
        {
          this._defaultGroupUid = value;
          this.OnPropertyChanged(nameof (DefaultGroupUid));
          this.OnPropertyChanged("GroupNameDefault");
        }
      }

      public string GroupNameDefault
      {
        get
        {
          if (this.DefaultGroupUid == Guid.Empty)
            return Translate.TemplateExcelFile_Выбрать;
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            return new GoodGroupsRepository(dataBase).GetByUid(this.DefaultGroupUid)?.Name ?? Translate.TemplateExcelFile_Выбрать;
        }
      }

      public Guid ExtraGroupUid
      {
        get => this._extraGroupUid;
        set
        {
          this._extraGroupUid = value;
          this.OnPropertyChanged(nameof (ExtraGroupUid));
          this.OnPropertyChanged("GroupNameExtra");
        }
      }

      public string GroupNameExtra
      {
        get
        {
          if (this.ExtraGroupUid == Guid.Empty)
            return Translate.TemplateExcelFile_Выбрать;
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            return new GoodGroupsRepository(dataBase).GetByUid(this.ExtraGroupUid)?.Name ?? Translate.TemplateExcelFile_Выбрать;
        }
      }

      public Guid EmptyGroupUid
      {
        get => this._emptyGroupUid;
        set
        {
          this._emptyGroupUid = value;
          this.OnPropertyChanged(nameof (EmptyGroupUid));
          this.OnPropertyChanged("GroupNameEmpty");
        }
      }

      public string GroupNameEmpty
      {
        get
        {
          if (this.EmptyGroupUid == Guid.Empty)
            return Translate.TemplateExcelFile_Выбрать;
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            return new GoodGroupsRepository(dataBase).GetByUid(this.EmptyGroupUid)?.Name ?? Translate.TemplateExcelFile_Выбрать;
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
        get => this._isVis;
        set
        {
          this._isVis = value;
          this.OnPropertyChanged(nameof (IsVisibilityGroup));
        }
      }

      public int ColumnGroup
      {
        get => this._columnGroup;
        set
        {
          this._columnGroup = value;
          this.ColumnGroupIndexChar = ExcelHelper.ColumnNumberToAbc(value);
          this.OnPropertyChanged("ColumnGroupIndexChar");
          this.OnPropertyChanged(nameof (ColumnGroup));
        }
      }

      public string ColumnGroupIndexChar { get; set; } = string.Empty;

      public bool IsGroupSet { get; set; } = true;

      public bool IsGroupSetIfNew { get; set; }

      public bool IsGroupNextIfEmpty { get; set; }

      public bool IsEmptyGroup { get; set; } = true;

      public bool IsNewGroupAdd { get; set; } = true;

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
            this.ColumnIndexText = ExcelHelper.ColumnNumberToAbc(value);
            this.OnPropertyChanged(nameof (ColumnIndex));
            this.OnPropertyChanged("ColumnIndexText");
          }
        }

        public string ColumnIndexText { get; set; }

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
    }
  }
}
