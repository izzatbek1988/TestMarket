// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.SettingsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.Goods.GoodCard;
using Gbs.Forms.Settings.Pages;
using Gbs.Helpers;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings
{
  public class SettingsViewModel : ViewModelWithForm
  {
    private Visibility _testPageVisibility = Visibility.Collapsed;

    public ICommand GetErrorGoodsStock
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.GetErrorsGoods()));
    }

    private void GetErrorsGoods()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SettingsViewModel.\u003C\u003Ec__DisplayClass2_0 cDisplayClass20 = new SettingsViewModel.\u003C\u003Ec__DisplayClass2_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass20.db = Data.GetDataBase();
      try
      {
        // ISSUE: reference to a compiler-generated field
        List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(cDisplayClass20.db).GetActiveItems();
        List<string> contents = new List<string>();
        string path = string.Format("C:\\temp\\{0:yyyy-MM-dd HH-mm-ss}.txt", (object) DateTime.Now);
        int count = activeItems.Count;
        int num1 = 0;
        foreach (Gbs.Core.Entities.Goods.Good good in activeItems)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          SettingsViewModel.\u003C\u003Ec__DisplayClass2_1 cDisplayClass21 = new SettingsViewModel.\u003C\u003Ec__DisplayClass2_1();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass21.CS\u0024\u003C\u003E8__locals1 = cDisplayClass20;
          // ISSUE: reference to a compiler-generated field
          cDisplayClass21.good = good;
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          SettingsViewModel.\u003C\u003Ec__DisplayClass2_2 cDisplayClass22 = new SettingsViewModel.\u003C\u003Ec__DisplayClass2_2();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass22.CS\u0024\u003C\u003E8__locals2 = cDisplayClass21;
          ++num1;
          IQueryable<DOCUMENTS> source1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass22.CS\u0024\u003C\u003E8__locals2.good.SetStatus == GlobalDictionaries.GoodsSetStatuses.Set)
          {
            ParameterExpression parameterExpression1;
            ParameterExpression parameterExpression2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            source1 = cDisplayClass22.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.db.GetTable<DOCUMENTS>().SelectMany<DOCUMENTS, DOCUMENT_ITEMS, DOCUMENTS>(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, IEnumerable<DOCUMENT_ITEMS>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
            {
              (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass22.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
              (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.DOCUMENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_GOOD_UID))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) cDisplayClass22, typeof (SettingsViewModel.\u003C\u003Ec__DisplayClass2_2)), FieldInfo.GetFieldFromHandle(__fieldref (SettingsViewModel.\u003C\u003Ec__DisplayClass2_2.CS\u0024\u003C\u003E8__locals2))), FieldInfo.GetFieldFromHandle(__fieldref (SettingsViewModel.\u003C\u003Ec__DisplayClass2_1.good))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression2))
            }), parameterExpression1), (Expression<Func<DOCUMENTS, DOCUMENT_ITEMS, DOCUMENTS>>) ((d, di) => d)).Distinct<DOCUMENTS>();
          }
          else
          {
            ParameterExpression parameterExpression3;
            ParameterExpression parameterExpression4;
            ParameterExpression parameterExpression5;
            ParameterExpression parameterExpression6;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: type reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: field reference
            // ISSUE: field reference
            // ISSUE: method reference
            // ISSUE: method reference
            source1 = cDisplayClass22.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.db.GetTable<DOCUMENTS>().SelectMany(System.Linq.Expressions.Expression.Lambda<Func<DOCUMENTS, IEnumerable<DOCUMENT_ITEMS>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
            {
              (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass22.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
              (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<DOCUMENT_ITEMS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.DOCUMENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression3, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression4))
            }), parameterExpression3), (d, di) => new
            {
              d = d,
              di = di
            }).SelectMany(System.Linq.Expressions.Expression.Lambda<Func<\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>, IEnumerable<GOODS_STOCK>>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call((System.Linq.Expressions.Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new System.Linq.Expressions.Expression[2]
            {
              (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Call(cDisplayClass22.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<System.Linq.Expressions.Expression>()),
              (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Quote((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Lambda<Func<GOODS_STOCK, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(x.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression5, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>.get_di), __typeref (\u003C\u003Ef__AnonymousType7<DOCUMENTS, DOCUMENT_ITEMS>))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DOCUMENT_ITEMS.get_STOCK_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression6, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (GOODS_STOCK.get_GOOD_UID))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Field((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) cDisplayClass22, typeof (SettingsViewModel.\u003C\u003Ec__DisplayClass2_2)), FieldInfo.GetFieldFromHandle(__fieldref (SettingsViewModel.\u003C\u003Ec__DisplayClass2_2.CS\u0024\u003C\u003E8__locals2))), FieldInfo.GetFieldFromHandle(__fieldref (SettingsViewModel.\u003C\u003Ec__DisplayClass2_1.good))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality)))), parameterExpression6))
            }), parameterExpression5), (data, gs) => data.d).Distinct<DOCUMENTS>();
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          DocumentsRepository documentsRepository = new DocumentsRepository(cDisplayClass22.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.db);
          List<Document> list1 = documentsRepository.GetByQuery(source1.Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.TYPE != 17))).ToList<Document>();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass22.listDoc = list1;
          // ISSUE: reference to a compiler-generated field
          cDisplayClass22.listDoc.AddRange((IEnumerable<Document>) documentsRepository.GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.ProductionItem));
          List<JournalGoodViewModel.JournalItem> source2 = new List<JournalGoodViewModel.JournalItem>();
          // ISSUE: reference to a compiler-generated field
          foreach (Document document in cDisplayClass22.listDoc.Where<Document>((Func<Document, bool>) (x => !x.IsDeleted && x.Type != GlobalDictionaries.DocumentsTypes.Inventory)))
          {
            Document doc = document;
            // ISSUE: reference to a compiler-generated method
            List<Gbs.Core.Entities.Documents.Item> list2 = doc.Items.Where<Gbs.Core.Entities.Documents.Item>(closure_0 ?? (closure_0 = new Func<Gbs.Core.Entities.Documents.Item, bool>(cDisplayClass21.\u003CGetErrorsGoods\u003Eb__16))).ToList<Gbs.Core.Entities.Documents.Item>();
            try
            {
              source2.AddRange(list2.GroupBy(x => new
              {
                ModificationUid = x.ModificationUid,
                Discount = x.Discount,
                Comment = x.Comment,
                SellPrice = x.SellPrice,
                GoodUid = x.GoodUid,
                BuyPrice = x.BuyPrice
              }).Select<IGrouping<\u003C\u003Ef__AnonymousType8<Guid, Decimal, string, Decimal, Guid, Decimal>, Gbs.Core.Entities.Documents.Item>, JournalGoodViewModel.JournalItem>(j => new JournalGoodViewModel.JournalItem(doc, j.ToList<Gbs.Core.Entities.Documents.Item>(), true, listDoc.ToList<Document>(), new BuyPriceCounter(), good)));
            }
            catch (Exception ex)
            {
              Other.ConsoleWrite((object) ex);
            }
          }
          Decimal num2 = new List<JournalGoodViewModel.JournalItem>(source2.Where<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, bool>) (x => x.Action != null)).ToList<JournalGoodViewModel.JournalItem>().OrderBy<JournalGoodViewModel.JournalItem, DateTime?>((Func<JournalGoodViewModel.JournalItem, DateTime?>) (item => item?.Date)).Reverse<JournalGoodViewModel.JournalItem>()).Sum<JournalGoodViewModel.JournalItem>((Func<JournalGoodViewModel.JournalItem, Decimal>) (x => x.StockDecimal));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Decimal num3 = cDisplayClass22.CS\u0024\u003C\u003E8__locals2.good.StocksAndPrices.Where<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => !x.IsDeleted)).Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (x => x.Stock));
          if (num2 != num3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            contents.Add(string.Format("{0}\\{1} Товар:{2} = {3} ; journal: {4}; stocks: {5}", (object) num1, (object) count, (object) cDisplayClass22.CS\u0024\u003C\u003E8__locals2.good.Barcode, (object) cDisplayClass22.CS\u0024\u003C\u003E8__locals2.good.Name, (object) num2, (object) num3));
          }
          File.AppendAllLines(path, (IEnumerable<string>) contents);
          contents.Clear();
        }
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass20.db != null)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass20.db.Dispose();
        }
      }
    }

    public ICommand RunPfUpdate
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            new DocumentsRepository(dataBase).GetItemsNoLinqPf(new DateTime(2021, 1, 1));
        }));
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public static Gbs.Core.Config.DataBase DataBaseConfig { get; set; }

    public Visibility VisibilitySettingByLegal
    {
      get
      {
        Gbs.Core.Config.Settings settings = this.Settings;
        int num;
        if (settings == null)
          num = 0;
        else
          num = settings.Interface.Country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Kazakhstan) ? 1 : 0;
        return num == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityBlockHome
    {
      get
      {
        Gbs.Core.Config.DataBase dataBaseConfig = SettingsViewModel.DataBaseConfig;
        return (dataBaseConfig != null ? (dataBaseConfig.ModeProgram == GlobalDictionaries.Mode.Home ? 1 : 0) : 0) == 0 ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public TabControl TabControl { get; set; }

    public int SelectedTab { get; set; }

    public ICommand SaveTestingSettings { get; set; }

    public Visibility TestPageVisibility
    {
      get => this._testPageVisibility;
      set
      {
        this._testPageVisibility = value;
        this.OnPropertyChanged(nameof (TestPageVisibility));
      }
    }

    public ICommand ShowTestPageCommand { get; set; }

    public Action Close { get; set; }

    public ICommand SaveSetting { get; set; }

    public PageDevices DevicesPage { get; set; }

    public PageListDevices ListDevicesPage { get; set; }

    public UsersPage UsersPage { get; set; }

    public PageInterface ViewPage { get; set; }

    public PageLegal LegalPage { get; set; }

    public PageDataBase DbPage { get; set; }

    public PageBasic BasicPage { get; set; }

    public PageGoods GoodsPage { get; set; }

    public PagePayments PaymentsPage { get; set; }

    public PageRemoteControl RemoteControlPage { get; set; }

    public PageClients ClientsPage { get; set; }

    public PageDiscountSetting DiscountPage { get; set; }

    public PageIntegrations IntegrationsPage { get; set; }

    public PageOther OtherPage { get; set; }

    public PageActionsGood ActionsGoodPage { get; set; }

    public PageCafe CafePage { get; set; }

    public PageExchangeData ExchangeDataPage { get; set; }

    private Gbs.Core.Config.Settings Settings { get; set; }

    private Integrations Integrations { get; set; }

    private Cafe Cafe { get; set; }

    public void LoadSettings()
    {
      try
      {
        this.Settings = new ConfigsRepository<Gbs.Core.Config.Settings>().Get();
        this.DbPage = new PageDataBase();
        this.BasicPage = new PageBasic();
        this.Integrations = new ConfigsRepository<Integrations>().Get();
        this.ClientsPage = new PageClients(this.Settings, this.Integrations);
        this.Cafe = new ConfigsRepository<Cafe>().Get();
        SettingsViewModel.DataBaseConfig = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
        if (SettingsViewModel.DataBaseConfig.ModeProgram == GlobalDictionaries.Mode.Home)
        {
          this.SelectedTab = 6;
          this.OnPropertyChanged("SelectedTab");
        }
        this.GetStorage = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<Storages.Storage> list = new List<Storages.Storage>();
          new FrmSelectedStorage().GetListSelectedStorages(ref list);
        }));
        this.BackupCreate = (ICommand) new RelayCommand((Action<object>) (obj => BackupHelper.CreateBackup()));
        int num1;
        this.ShowNotifications = (ICommand) new RelayCommand((Action<object>) (obj => ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = "Заголовок",
          Text = "Текст уведомления " + DateTime.Now.ToString(),
          ActionText = "Проверка",
          ActionCommand = (ICommand) new RelayCommand((Action<object>) (_ => num1 = (int) MessageBoxHelper.Show("Проверка")))
        })));
        int num2;
        this.ShowGbsId = (ICommand) new RelayCommand((Action<object>) (obj => num2 = (int) MessageBoxHelper.Show(GbsIdHelperMain.GetGbsId())));
        this.ShowPropertiesKkm = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (KkmHelper kkmHelper = new KkmHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
            kkmHelper.ShowProperties();
        }));
        this.ShowTestPageCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.TestPageVisibility = Visibility.Visible));
        this.SaveSetting = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.Save())
            return;
          this.Close();
        }));
        this.SaveTestingSettings = (ICommand) new RelayCommand((Action<object>) (obj => { }));
        this.UsersPage = new UsersPage(this.Settings);
        this.DbPage = new PageDataBase(SettingsViewModel.DataBaseConfig, this.AuthUser);
        this.ViewPage = new PageInterface(this.Settings);
        this.IntegrationsPage = new PageIntegrations(this.Integrations, this.Settings);
        this.LegalPage = new PageLegal(this.Settings, this.Integrations);
        this.DiscountPage = new PageDiscountSetting(this.Settings);
        this.PaymentsPage = new PagePayments(this.Settings, new Action(((PageDiscountSettingViewModel) this.DiscountPage.DataContext).LoadingPaymentMethods));
        this.RemoteControlPage = new PageRemoteControl(this.Settings);
        this.BasicPage = new PageBasic(this.Settings, SettingsViewModel.DataBaseConfig, new Func<bool>(this.Save));
        this.DevicesPage = new PageDevices(this.Settings, (Page) this.UsersPage, this.BasicPage);
        this.ListDevicesPage = new PageListDevices(((DevicesViewModel) this.DevicesPage.DataContext).DevicesConfig, this.Settings);
        this.GoodsPage = new PageGoods(this.Settings, ((DevicesViewModel) this.DevicesPage.DataContext).PageScale);
        this.OtherPage = new PageOther(this.Settings);
        this.ExchangeDataPage = new PageExchangeData(this.Settings);
        this.CafePage = new PageCafe(this.Cafe, SettingsViewModel.DataBaseConfig, this.BasicPage);
        this.ActionsGoodPage = new PageActionsGood(this.Settings);
        this.testMultiValue();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме настроек");
        ProgressBarHelper.Close();
      }
    }

    public ObservableCollection<MultiValueControl.Value> MultiList { get; set; }

    public ICommand AddCommand { get; set; }

    private void testMultiValue()
    {
      this.MultiList = new ObservableCollection<MultiValueControl.Value>(new List<MultiValueControl.Value>()
      {
        new MultiValueControl.Value() { DisplayedValue = "10:00" },
        new MultiValueControl.Value() { DisplayedValue = "15:30" },
        new MultiValueControl.Value() { DisplayedValue = "21:05" }
      });
      this.OnPropertyChanged("MultiList");
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (_ =>
      {
        string output = MessageBoxHelper.Input("", "Value input").output;
        this.MultiList.Add(new MultiValueControl.Value()
        {
          DisplayedValue = output
        });
        this.OnPropertyChanged("MultiList");
      }));
    }

    private bool Save()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SettingsViewModel_Save_Сохранение_настроек);
      try
      {
        if (!this.RemoteControlPage.Save())
        {
          this.TabControl.Items.Cast<TabItem>().First<TabItem>((Func<TabItem, bool>) (x => x.Name == "RemoteTab")).IsSelected = true;
          return false;
        }
        if (!this.ClientsPage.Save())
        {
          this.TabControl.Items.Cast<TabItem>().First<TabItem>((Func<TabItem, bool>) (x => x.Name == "ClientsPage")).IsSelected = true;
          return false;
        }
        if (this.Settings.Sales.RoundTotals.IsEnable && this.Settings.Sales.RoundTotals.Coefficient == 0M)
        {
          this.TabControl.Items.Cast<TabItem>().First<TabItem>((Func<TabItem, bool>) (x => x.Name == "ActionsGoodTab")).IsSelected = true;
          MessageBoxHelper.Warning(Translate.SettingsViewModel_Save_Необходимо_указать_коэффициент_округления_чека_для_сохранения_настроек__он_должен_быть_более_0_);
          return false;
        }
        if (!this.ActionsGoodPage.Save())
        {
          this.TabControl.Items.Cast<TabItem>().First<TabItem>((Func<TabItem, bool>) (x => x.Name == "ActionsGoodTab")).IsSelected = true;
          return false;
        }
        if (!this.LegalPage.Save())
        {
          this.TabControl.Items.Cast<TabItem>().First<TabItem>((Func<TabItem, bool>) (x => x.Name == "BillTabItem")).IsSelected = true;
          return false;
        }
        FileSystemHelper.SetAutoRunValue(this.Settings.BasicConfig.AutoRunProgram);
        ConfigsRepository<Gbs.Core.Config.Settings> configsRepository = new ConfigsRepository<Gbs.Core.Config.Settings>();
        if (!(this.DbPage.Save() & this.ListDevicesPage.Save() & this.DevicesPage.Save() & this.IntegrationsPage.Save() & this.DiscountPage.Save() & this.CafePage.Save() & this.ListDevicesPage.Save() & this.UsersPage.Save() & this.ExchangeDataPage.Save() & configsRepository.Save(this.Settings)))
          return false;
        ((App) Application.Current).ChangeSkin(this.Settings.Interface.Theme);
        ((App) Application.Current).UpdateColors();
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
        return false;
      }
      finally
      {
        progressBar.Close();
      }
    }

    public ICommand BackupCreate { get; set; }

    public ICommand ShowNotifications { get; set; }

    public ICommand GetStorage { get; set; }

    public ICommand ShowGbsId { get; set; }

    public ICommand ShowPropertiesKkm { get; set; }

    public ICommand CorrectMultiBarcodes
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SettingsViewModel_Корректировка_штрих_кодов);
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            GoodRepository goodRepository = new GoodRepository(dataBase);
            foreach (Gbs.Core.Entities.Goods.Good allItem in goodRepository.GetAllItems())
            {
              string[] source = allItem.Barcode.Split(new char[1]
              {
                ','
              }, StringSplitOptions.RemoveEmptyEntries);
              if (source.Length > 1)
              {
                allItem.Barcode = ((IEnumerable<string>) source).First<string>();
                allItem.Barcodes = allItem.Barcodes.Concat<string>(((IEnumerable<string>) source).Skip<string>(1));
              }
              goodRepository.Save(allItem);
            }
            progressBar.Close();
          }
        }));
      }
    }

    public ICommand ClearAllCache
    {
      get => (ICommand) new RelayCommand((Action<object>) (_ => CacheHelper.ClearAll()));
    }
  }
}
