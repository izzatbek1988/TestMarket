// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Excel.ExcelClientsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Excel;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Excel
{
  public partial class ExcelClientsViewModel : ViewModelWithForm
  {
    private string _path;
    public static IWorkbook Wb;
    private ISheet _sh;
    private IRow _iRow;
    private int _columnIndexName;
    private int _columnIndexBarcode;
    private int _columnIndexDiscount;
    private int _columnIndexSum;
    private int _columnIndexBonuses;

    public Dictionary<GlobalDictionaries.BarcodeIfReplay, string> ListActionBarcode { get; set; } = GlobalDictionaries.BarcodeClientActionDictionary();

    public bool IsEnabledData { get; set; }

    public string SelectedTemplate { get; set; }

    public List<Client> ClientsImport { get; set; } = new List<Client>();

    public ExcelClientsViewModel()
    {
      try
      {
        this.ShowInfoClients();
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
        this.SaveClients = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки клиентов из Ексель");
      }
    }

    private void LoadDataFromDocument()
    {
      try
      {
        if (this.ExcelFilePath.IsNullOrEmpty())
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.ExcelClientsViewModel_Требуется_выбрать_файл_Excel);
        }
        else
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ExcelClientsViewModel_Загрузка_данных_из_Ексель);
          ExcelClientsViewModel.Wb = ExcelFile.Read(this.ExcelFilePath);
          this._sh = ExcelClientsViewModel.Wb.GetSheetAt(this.SelectedSheetNum);
          int num2 = this._sh.LastRowNum + 1;
          List<ClientAdnSum> clientAdnSumList = new List<ClientAdnSum>();
          for (int rownum = 0; rownum < num2; ++rownum)
          {
            this._iRow = this._sh.GetRow(rownum);
            if (this._iRow != null)
            {
              int num3 = rownum % 10;
              string stringCellValue1 = ExcelCellValueReader.GetStringCellValue(this._iRow, this.ColumnIndexName);
              if (!string.IsNullOrEmpty(stringCellValue1))
              {
                string barcode = ExcelClientsViewModel.HelpClassExcel.GetBarcode(this._iRow, this.ColumnIndexBarcode, this.SelectedIfBarcodeReplay, clientAdnSumList);
                if (this.SelectedIfBarcodeReplay != 2 || !string.IsNullOrEmpty(barcode))
                {
                  Gbs.Core.Entities.Clients.Group groupUid = ExcelClientsViewModel.HelpClassExcel.GetGroupUid(this._iRow, this.ColumnIndexDiscount, this.IsSupplierGroup);
                  if (!this.DiscountIsCheckedEmpty || groupUid != null)
                  {
                    string stringCellValue2 = ExcelCellValueReader.GetStringCellValue(this._iRow, this.ColumnIndexSum);
                    ClientAdnSum clientAdnSum = new ClientAdnSum()
                    {
                      Client = new Client()
                      {
                        Name = stringCellValue1,
                        Barcode = barcode,
                        Group = groupUid
                      }
                    };
                    if (!stringCellValue2.IsNullOrEmpty())
                    {
                      Decimal result;
                      if (Decimal.TryParse(stringCellValue2, out result))
                        clientAdnSum.CurrentSalesSum = result;
                    }
                    else if (this.SumIsCheckedEmpty)
                      continue;
                    string stringCellValue3 = ExcelCellValueReader.GetStringCellValue(this._iRow, this.ColumnIndexBonuses);
                    if (!stringCellValue3.IsNullOrEmpty())
                    {
                      Decimal result;
                      if (Decimal.TryParse(stringCellValue3, out result))
                        clientAdnSum.CurrentBonusSum = result;
                    }
                    else if (this.BonusesIsCheckedEmpty)
                      continue;
                    bool flag = true;
                    List<EntityProperties.PropertyValue> list = clientAdnSum.Client.Properties.ToList<EntityProperties.PropertyValue>();
                    foreach (ExcelClientsViewModel.InfoClient valuesDataClient in this.ValuesDataClientList)
                    {
                      switch (valuesDataClient.PropType)
                      {
                        case ExcelClientsViewModel.ClientsProp.Phone:
                          string stringCellValue4 = ExcelCellValueReader.GetStringCellValue(this._iRow, valuesDataClient.ColumnIndex);
                          if (valuesDataClient.IsChecked && string.IsNullOrEmpty(stringCellValue4))
                          {
                            flag = false;
                            continue;
                          }
                          clientAdnSum.Client.Phone = stringCellValue4;
                          continue;
                        case ExcelClientsViewModel.ClientsProp.Email:
                          string stringCellValue5 = ExcelCellValueReader.GetStringCellValue(this._iRow, valuesDataClient.ColumnIndex);
                          if (valuesDataClient.IsChecked && string.IsNullOrEmpty(stringCellValue5))
                          {
                            flag = false;
                            continue;
                          }
                          clientAdnSum.Client.Email = stringCellValue5;
                          continue;
                        case ExcelClientsViewModel.ClientsProp.Birthday:
                          string stringCellValue6 = ExcelCellValueReader.GetStringCellValue(this._iRow, valuesDataClient.ColumnIndex);
                          if (valuesDataClient.IsChecked && string.IsNullOrEmpty(stringCellValue6))
                          {
                            flag = false;
                            continue;
                          }
                          DateTime result;
                          if (DateTime.TryParse(stringCellValue6, out result))
                          {
                            clientAdnSum.Client.Birthday = new DateTime?(result);
                            continue;
                          }
                          continue;
                        case ExcelClientsViewModel.ClientsProp.Adress:
                          string stringCellValue7 = ExcelCellValueReader.GetStringCellValue(this._iRow, valuesDataClient.ColumnIndex);
                          if (valuesDataClient.IsChecked && string.IsNullOrEmpty(stringCellValue7))
                          {
                            flag = false;
                            continue;
                          }
                          clientAdnSum.Client.Address = stringCellValue7;
                          continue;
                        case ExcelClientsViewModel.ClientsProp.ExtraProp:
                          EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
                          EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
                          propertyType.Uid = valuesDataClient.UidExtraProp;
                          propertyValue1.Type = propertyType;
                          propertyValue1.EntityUid = clientAdnSum.Client.Uid;
                          propertyValue1.Value = (object) ExcelCellValueReader.GetStringCellValue(this._iRow, valuesDataClient.ColumnIndex);
                          EntityProperties.PropertyValue propertyValue2 = propertyValue1;
                          list.Add(propertyValue2);
                          if (string.IsNullOrEmpty(propertyValue2.Value.ToString()) && valuesDataClient.IsChecked)
                          {
                            flag = false;
                            continue;
                          }
                          continue;
                        default:
                          continue;
                      }
                    }
                    if (flag)
                    {
                      clientAdnSum.Client.Properties = list;
                      clientAdnSumList.Add(clientAdnSum);
                    }
                  }
                }
              }
            }
          }
          new Stopwatch().Start();
          // ISSUE: variable of a compiler-generated type
          ExcelClientsViewModel.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180;
          // ISSUE: reference to a compiler-generated field
          cDisplayClass180.db = Data.GetDataBase();
          try
          {
            // ISSUE: reference to a compiler-generated field
            ClientsRepository clientsRepository = new ClientsRepository(cDisplayClass180.db);
            GoodGroups.Group group1 = new GoodGroups.Group();
            group1.Name = Translate.ExcelClientsViewModel_LoadDataFromDocument_Импорт_сумм_покупок_из_Excel;
            group1.IsDeleted = true;
            GoodGroups.Group group2 = group1;
            // ISSUE: reference to a compiler-generated field
            new GoodGroupsRepository(cDisplayClass180.db).Save(group2);
            Storages.Storage storage1 = new Storages.Storage();
            storage1.Name = Translate.ExcelClientsViewModel_LoadDataFromDocument_Импорт_сумм_покупок_из_Excel;
            storage1.IsDeleted = true;
            Storages.Storage storage2 = storage1;
            storage2.Save();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass180.firstStorage = Storages.GetStorages().First<Storages.Storage>();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass180.currentSection = Sections.GetCurrentSection();
            ref ExcelClientsViewModel.\u003C\u003Ec__DisplayClass18_0 local = ref cDisplayClass180;
            Gbs.Core.Entities.Goods.Good good = new Gbs.Core.Entities.Goods.Good();
            good.Name = Translate.ExcelClientsViewModel_LoadDataFromDocument_Импорт_сумм_покупок_из_Excel;
            good.StocksAndPrices = new List<GoodsStocks.GoodStock>()
            {
              new GoodsStocks.GoodStock()
              {
                Price = 0M,
                Stock = 999999M,
                Storage = storage2
              }
            };
            good.Group = group2;
            good.IsDeleted = true;
            // ISSUE: reference to a compiler-generated field
            local.oldSaleGood = good;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            new GoodRepository(cDisplayClass180.db).Save(cDisplayClass180.oldSaleGood);
            if (clientAdnSumList.Any<ClientAdnSum>())
            {
              HomeOfficeHelper homeOfficeHelper = new HomeOfficeHelper();
              homeOfficeHelper.Prepare();
              homeOfficeHelper.CreateEditFile<List<Client>>(clientAdnSumList.Select<ClientAdnSum, Client>((Func<ClientAdnSum, Client>) (x => x.Client)).ToList<Client>(), HomeOfficeHelper.EntityEditHome.ClientList);
              homeOfficeHelper.CreateEditFile<List<Gbs.Core.Entities.Clients.Group>>(clientAdnSumList.Select<ClientAdnSum, Gbs.Core.Entities.Clients.Group>((Func<ClientAdnSum, Gbs.Core.Entities.Clients.Group>) (x => x.Client.Group)).GroupBy<Gbs.Core.Entities.Clients.Group, Guid>((Func<Gbs.Core.Entities.Clients.Group, Guid>) (x => x.Uid)).Select<IGrouping<Guid, Gbs.Core.Entities.Clients.Group>, Gbs.Core.Entities.Clients.Group>((Func<IGrouping<Guid, Gbs.Core.Entities.Clients.Group>, Gbs.Core.Entities.Clients.Group>) (x => x.First<Gbs.Core.Entities.Clients.Group>())).ToList<Gbs.Core.Entities.Clients.Group>(), HomeOfficeHelper.EntityEditHome.ClientGroupList);
              homeOfficeHelper.Send();
            }
            GlobalData.IsMarket5ImportAcitve = true;
            foreach (ClientAdnSum clientAdnSum in clientAdnSumList)
            {
              this.SaveResult = clientsRepository.Save(clientAdnSum.Client, false);
              if (this.SaveResult)
              {
                ExcelClientsViewModel.\u003CLoadDataFromDocument\u003Eg__SaveSumSale\u007C18_0(clientAdnSum.CurrentSalesSum, clientAdnSum.CurrentBonusSum, clientAdnSum.Client, ref cDisplayClass180);
                this.ClientsImport.Add(clientAdnSum.Client);
              }
            }
            GlobalData.IsMarket5ImportAcitve = false;
            progressBar.Close();
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.ExcelClientsViewModel_Загрузка_контактов_из_Excel,
              Text = string.Format(Translate.ExcelClientsViewModel_Загружено__0__из__1__записей, (object) this.ClientsImport.Count, (object) clientAdnSumList.Count)
            });
            this.CloseAction();
          }
          finally
          {
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass180.db != null)
            {
              // ISSUE: reference to a compiler-generated field
              cDisplayClass180.db.Dispose();
            }
          }
        }
      }
      finally
      {
        GlobalData.IsMarket5ImportAcitve = false;
        ProgressBarHelper.Close();
      }
    }

    private void ShowProperty()
    {
      EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Client).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted)).ToList<EntityProperties.PropertyType>().ForEach((Action<EntityProperties.PropertyType>) (x => this.ValuesDataClientList.Add(new ExcelClientsViewModel.InfoClient(x.Name, ExcelClientsViewModel.ClientsProp.ExtraProp, x.Uid))));
    }

    private void ShowInfoClients()
    {
      this.ValuesDataClientList = new List<ExcelClientsViewModel.InfoClient>()
      {
        new ExcelClientsViewModel.InfoClient(Translate.ExcelClientsViewModel_Телефон, ExcelClientsViewModel.ClientsProp.Phone),
        new ExcelClientsViewModel.InfoClient(Translate.ExcelClientsViewModel_E_mail, ExcelClientsViewModel.ClientsProp.Email),
        new ExcelClientsViewModel.InfoClient(Translate.ExcelClientsViewModel_День_рождения, ExcelClientsViewModel.ClientsProp.Birthday),
        new ExcelClientsViewModel.InfoClient(Translate.ExcelClientsViewModel_Адрес, ExcelClientsViewModel.ClientsProp.Adress)
      };
      this.ShowProperty();
    }

    private void GetListSheet()
    {
      ExcelClientsViewModel.Wb = ExcelFile.Read(this.ExcelFilePath);
      this.SelectedSheetNum = ExcelClientsViewModel.Wb.ActiveSheetIndex;
      this.ListSheet = new List<string>();
      int numberOfSheets = ExcelClientsViewModel.Wb.NumberOfSheets;
      for (int sheet = 0; sheet < numberOfSheets; ++sheet)
        this.ListSheet.Add(ExcelClientsViewModel.Wb.GetSheetName(sheet));
      this.OnPropertyChanged("ListSheet");
      this.OnPropertyChanged("SelectedSheetNum");
    }

    public void Save() => this.LoadDataFromDocument();

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

    public int SelectedSheetNum { get; set; }

    public List<string> ListSheet { get; set; }

    public ICommand SelectFileExcel { get; set; }

    public ICommand SaveClients { get; set; }

    public bool SaveResult { get; set; }

    public bool IsSupplierGroup { get; set; }

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

    public int SelectedIfBarcodeReplay { get; set; }

    public int ColumnIndexDiscount
    {
      get => this._columnIndexDiscount;
      set
      {
        this._columnIndexDiscount = value;
        this.ColumnIndexDiscountChar = ExcelHelper.ColumnNumberToAbc(value);
        this.OnPropertyChanged(nameof (ColumnIndexDiscount));
        this.OnPropertyChanged("ColumnIndexDiscountChar");
      }
    }

    public string ColumnIndexDiscountChar { get; set; } = string.Empty;

    public bool DiscountIsCheckedEmpty { get; set; }

    public int ColumnIndexSum
    {
      get => this._columnIndexSum;
      set
      {
        this._columnIndexSum = value;
        this.ColumnIndexSumChar = ExcelHelper.ColumnNumberToAbc(value);
        this.OnPropertyChanged(nameof (ColumnIndexSum));
        this.OnPropertyChanged("ColumnIndexSumChar");
      }
    }

    public string ColumnIndexSumChar { get; set; } = string.Empty;

    public bool SumIsCheckedEmpty { get; set; }

    public int ColumnIndexBonuses
    {
      get => this._columnIndexBonuses;
      set
      {
        this._columnIndexBonuses = value;
        this.ColumnIndexBonusesChar = ExcelHelper.ColumnNumberToAbc(value);
        this.OnPropertyChanged(nameof (ColumnIndexBonuses));
        this.OnPropertyChanged("ColumnIndexBonusesChar");
      }
    }

    public string ColumnIndexBonusesChar { get; set; } = string.Empty;

    public bool BonusesIsCheckedEmpty { get; set; }

    public List<ExcelClientsViewModel.InfoClient> ValuesDataClientList { get; set; }

    public enum ClientsProp
    {
      Phone,
      Email,
      Birthday,
      Adress,
      ExtraProp,
    }

    private static class HelpClassExcel
    {
      private static List<Gbs.Core.Entities.Clients.Group> Groups { get; }

      private static List<ClientAdnSum> ListClient { get; }

      private static Gbs.Core.Config.Devices ConfigDevices { get; set; } = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();

      static HelpClassExcel()
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          ExcelClientsViewModel.HelpClassExcel.Groups = new GroupRepository(dataBase).GetActiveItems().ToList<Gbs.Core.Entities.Clients.Group>();
          ExcelClientsViewModel.HelpClassExcel.ListClient = new ClientsRepository(dataBase).GetActiveItems().Select<Client, ClientAdnSum>((Func<Client, ClientAdnSum>) (x => new ClientAdnSum()
          {
            Client = x
          })).ToList<ClientAdnSum>();
        }
      }

      public static string GetBarcode(
        IRow row,
        int barcodeIndex,
        int SelectedIndexIfReplay,
        List<ClientAdnSum> listClientLoad)
      {
        ExcelClientsViewModel.HelpClassExcel.ListClient.AddRange((IEnumerable<ClientAdnSum>) listClientLoad);
        string barcode = ExcelCellValueReader.GetStringCellValue(row, barcodeIndex);
        string[] strArray = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.RandomGenerated.Split(GlobalDictionaries.SplitArr);
        string prefix = "";
        if (strArray.Length != 0)
          prefix = strArray[0];
        if (ExcelClientsViewModel.HelpClassExcel.ListClient.All<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => x.Client.Barcode != barcode)))
          return barcode;
        switch (SelectedIndexIfReplay)
        {
          case 0:
            return string.Empty;
          case 1:
            return BarcodeHelper.RandomBarcode(prefix);
          case 2:
            return (string) null;
          default:
            return (string) null;
        }
      }

      public static Gbs.Core.Entities.Clients.Group GetGroupUid(
        IRow row,
        int discountIndex,
        bool isSupplier)
      {
        string stringCellValue = ExcelCellValueReader.GetStringCellValue(row, discountIndex);
        Decimal discount = 0M;
        if (!stringCellValue.IsNullOrEmpty())
        {
          Decimal result;
          if (!Decimal.TryParse(stringCellValue, out result))
            result = 0M;
          discount = result;
        }
        if (discount > 100M)
          discount = 100M;
        if (discount < 0M)
          discount = 0M;
        if (ExcelClientsViewModel.HelpClassExcel.Groups.Any<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (x => x.Discount == discount && x.Price.Uid == Guid.Empty && x.IsSupplier == isSupplier)))
          return ExcelClientsViewModel.HelpClassExcel.Groups.First<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (x => x.Discount == discount && x.Price.Uid == Guid.Empty && x.IsSupplier == isSupplier));
        Gbs.Core.Entities.Clients.Group groupUid = new Gbs.Core.Entities.Clients.Group()
        {
          Discount = discount,
          Name = string.Format(Translate.HelpClassExcel_Группа___0_N2____, (object) discount),
          IsSupplier = isSupplier
        };
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          new GroupRepository(dataBase).Save(groupUid);
          ExcelClientsViewModel.HelpClassExcel.Groups.Add(groupUid);
          return groupUid;
        }
      }
    }

    public class InfoClient : ViewModelWithForm
    {
      private int _columnIndex;
      private bool _isChecked;

      public string NameData { get; set; }

      public ExcelClientsViewModel.ClientsProp PropType { get; set; }

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

      public InfoClient(string name, ExcelClientsViewModel.ClientsProp prop, Guid extraPropUid = default (Guid))
      {
        this.NameData = name;
        this.PropType = prop;
        this.UidExtraProp = extraPropUid;
      }

      public Guid UidExtraProp { get; set; }
    }
  }
}
