// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.FrmSettings
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Goods;
using Gbs.Core.Devices.BarcodeScanners;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Devices.DisplayBuyers.Models;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.Cafe;
using Gbs.Forms.Other;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.DB;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Tooltips;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings
{
  public partial class FrmSettings : WindowWithSize, IComponentConnector
  {
    private int totalTryCount;
    internal TabControl TabControl;
    internal TabItem TabMainSettings;
    internal TabItem ActionsGoodTab;
    internal System.Windows.Controls.Frame UsersPage;
    internal TabItem RemoteTab;
    internal TabItem ClientsPage;
    internal System.Windows.Controls.Frame DevicesPage;
    internal System.Windows.Controls.Frame NewDevicesPage;
    internal TabItem BillTabItem;
    internal TabItem CafeTabItem;
    private bool _contentLoaded;

    public FrmSettings()
    {
      this.InitializeComponent();
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((UIElement) this)
        }
      };
      TooltipsSetter.Set(this);
    }

    public void ShowSetting()
    {
      (bool Result, Gbs.Core.Entities.Users.User user) = new Authorization().GetAccess(Gbs.Core.Entities.Actions.SettingsShowAndEdit);
      if (!Result)
        return;
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ЗагрузкаНастроекПрограммы);
      KkmHelper.UserUid = user.Uid;
      ConfigsCache.GetInstance().ReloadAllCache();
      ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateShowHistory(Translate.FrmSettings_ShowSetting_Открыты_настройки_программы, user), true);
      SettingsViewModel settingsViewModel = new SettingsViewModel()
      {
        Close = new Action(((Window) this).Close),
        TabControl = this.TabControl,
        AuthUser = user
      };
      settingsViewModel.LoadSettings();
      this.DataContext = (object) settingsViewModel;
      progressBar.Close();
      this.ShowDialog();
      ConfigsCache.GetInstance().ReloadAllCache();
      ((App) Application.Current).ChangeSkin(new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Theme);
      CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllUsers);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      int num1 = (int) MessageBoxHelper.Show("warning", icon: MessageBoxImage.Exclamation);
      int num2 = (int) MessageBoxHelper.Show("error", icon: MessageBoxImage.Hand);
      int num3 = (int) MessageBoxHelper.Show("question", icon: MessageBoxImage.Question);
      int num4 = (int) MessageBoxHelper.Show("info");
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("az");
      Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("az");
      int num = (int) MessageBoxHelper.Show(Translate.FrmGoodsGroupsEdit_NeedToSelectGroup);
    }

    private void ButtonCafe_OnClick(object sender, RoutedEventArgs e)
    {
      new FrmCafeMain().ShowDialog();
    }

    private void startComPortScanner(object sender, RoutedEventArgs e) => ComPortScanner.Start();

    private void Sberbank_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void TestDisplay_OnClick(object sender, RoutedEventArgs e)
    {
      Atol8 atol8 = new Atol8();
      atol8.Connect(false);
      atol8.ShowProperties();
    }

    private void mercuryTest(object sender, RoutedEventArgs e)
    {
    }

    private void testPostProtocol(object sender, RoutedEventArgs e)
    {
      try
      {
        SerialPort serialPort = new SerialPort()
        {
          PortName = "COM2",
          BaudRate = 9600,
          DataBits = 7,
          Parity = Parity.Odd,
          RtsEnable = true,
          StopBits = StopBits.One,
          ReadTimeout = 3000,
          WriteBufferSize = 3000
        };
        serialPort.Open();
        if (!serialPort.IsOpen)
        {
          LogHelper.Debug("Порт не удалось октрыть");
        }
        else
        {
          LogHelper.Debug("Порт открыт, отправляю запрос");
          serialPort.WriteLine("W");
          LogHelper.Debug("Начинаю читать данные из порта");
          LogHelper.Debug("port answer: " + serialPort.ReadExisting());
          serialPort.Close();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка");
      }
    }

    private void ButtonBase_OnClick_KbTest(object sender, RoutedEventArgs e)
    {
      new FrmKeyboard().Show();
    }

    private void ButtonBase_OnClick_polycloudtest(object sender, RoutedEventArgs e)
    {
      new Gbs.Helpers.ExternalApi.PolycardCloud.PolyCloud().GetClientByCard("2210367187612");
    }

    private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmSettings_ButtonBase2_OnClick_Выполнение_корректировок);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        foreach (ClientAdnSum clientAdnSum in new ClientsRepository(dataBase).GetListActiveItemAndSum().Where<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => x.TotalBonusSum < 0M)))
        {
          Decimal num = Math.Abs(clientAdnSum.TotalBonusSum) + 100M;
          new Gbs.Core.Entities.Payments.Payment()
          {
            Type = GlobalDictionaries.PaymentTypes.BonusesCorrection,
            SumOut = num,
            Client = clientAdnSum.Client
          }.Save();
        }
        progressBar.Close();
      }
    }

    private void buttonCleareStocks_OnClick(object sender, RoutedEventArgs e)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        IEnumerable<Gbs.Core.Entities.Goods.Good> goods = new GoodRepository(dataBase).GetActiveItems().Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Any<GoodsStocks.GoodStock>(new Func<GoodsStocks.GoodStock, bool>(this.IsZeroAndNonDeletedStock))));
        List<Document> list = new DocumentsRepository(dataBase).GetItemsWithFilter(GlobalDictionaries.DocumentsTypes.Buy).OrderByDescending<Document, DateTime>((Func<Document, DateTime>) (x => x.DateTime)).ToList<Document>();
        foreach (Gbs.Core.Entities.Goods.Good good1 in goods)
        {
          Gbs.Core.Entities.Goods.Good good = good1;
          good.StocksAndPrices.Where<GoodsStocks.GoodStock>(new Func<GoodsStocks.GoodStock, bool>(this.IsZeroAndNonDeletedStock)).Clone<IEnumerable<GoodsStocks.GoodStock>>();
          list.Select<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, Gbs.Core.Entities.Documents.Item>) (x => x.Items.First<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid == good.Uid)))).First<Gbs.Core.Entities.Documents.Item>();
          foreach (GoodsStocks.GoodStock goodStock in good.StocksAndPrices.Where<GoodsStocks.GoodStock>(new Func<GoodsStocks.GoodStock, bool>(this.IsZeroAndNonDeletedStock)))
            ;
        }
      }
    }

    private bool IsZeroAndNonDeletedStock(GoodsStocks.GoodStock x) => x.IsDeleted && x.Stock == 0M;

    private void ButtonBase_OnClick_lev(object sender, RoutedEventArgs e)
    {
      string str = MessageBoxHelper.Input(string.Empty, "test levinshtain").output;
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        IEnumerable<Gbs.Core.Entities.Goods.Good> goods = new GoodRepository(dataBase).GetActiveItems().OrderBy<Gbs.Core.Entities.Goods.Good, int>((Func<Gbs.Core.Entities.Goods.Good, int>) (x => LevinshtaingHelper.IsSimilarTo(x.Name, str))).Take<Gbs.Core.Entities.Goods.Good>(30);
        Gbs.Helpers.Other.ConsoleWrite("search for: " + str);
        foreach (Gbs.Core.Entities.Goods.Good good in goods)
          Gbs.Helpers.Other.ConsoleWrite(good.Name);
      }
    }

    private void ButtonBase_DbDeadLock(object sender, RoutedEventArgs e)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        Gbs.Core.Entities.Goods.Good activeItem = new GoodRepository(dataBase).GetActiveItems()[10];
        List<Task> list = new List<Task>();
        this.totalTryCount = 0;
        for (int index = 0; index < 10; ++index)
        {
          Task task = new Task((Action) (() =>
          {
            LinqToDB.DataContext dc = Data.GetDataContext();
            TransactionHelper.RunActionWithDbTransaction((Action) (() => dc.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.IS_DELETED == true)).Set<GOODS, bool>((Expression<Func<GOODS, bool>>) (x => x.IS_DELETED), true).Update<GOODS>()), dc.BeginTransaction());
          }));
          list.Add(task);
        }
        list.RunList(true);
        DevelopersHelper.ShowNotification(string.Format("Total try count: {0}", (object) this.totalTryCount));
      }
    }

    private void ButtonBase_OnClickMsgBoxCommandsList(object sender, RoutedEventArgs e)
    {
      Gbs.Forms.Other.MessageBox.MsgBoxResult msgBoxResult = MessageBoxHelper.ShowWithCommands("Lorem ipsum dolor sit amet, consectetur adipiscing elit.In vel tincidunt lorem.Phasellus ut enim odio.Donec id elementum ipsum.Morbi ut vehicula nibh.Aliquam vel sem sit amet nisi congue vestibulum.", commands: new Dictionary<int, string>()
      {
        {
          0,
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In vel tincidunt lorem. Phasellus ut enim odio. Donec id elementum ipsum. Morbi ut vehicula nibh. Aliquam vel sem sit amet nisi congue vestibulum"
        },
        {
          1,
          "command 1"
        },
        {
          2,
          "command 2"
        },
        {
          3,
          "command 3"
        }
      });
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format("r:{0}, i:{1}", (object) msgBoxResult.Result, (object) msgBoxResult.SelectedIndex)));
    }

    private void ButtonBase_OnClick_checkboxes(object sender, RoutedEventArgs e)
    {
      Gbs.Forms.Other.MessageBox.MsgBoxResult msgBoxResult = MessageBoxHelper.ShowWithCheckboxes("Lorem ipsum dolor sit amet, consectetur adipiscing elit.In vel tincidunt lorem.Phasellus ut enim odio.Donec id elementum ipsum.Morbi ut vehicula nibh.Aliquam vel sem sit amet nisi congue vestibulum.", checkboxes: new List<MessBoxViewModel.CheckboxItem>()
      {
        new MessBoxViewModel.CheckboxItem(0, "check box 0", false),
        new MessBoxViewModel.CheckboxItem(1, "check box 1", true),
        new MessBoxViewModel.CheckboxItem(2, "check box 2", false)
      });
      string str = "";
      foreach (MessBoxViewModel.CheckboxItem checkbox in msgBoxResult.Checkboxes)
        str = str + checkbox.Text + ": " + checkbox.IsChecked.ToString() + "\n";
      ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format("r:{0}, list:{1}", (object) msgBoxResult.Result, (object) str)));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/frmsettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TabControl = (TabControl) target;
          break;
        case 2:
          this.TabMainSettings = (TabItem) target;
          break;
        case 3:
          this.ActionsGoodTab = (TabItem) target;
          break;
        case 4:
          this.UsersPage = (System.Windows.Controls.Frame) target;
          break;
        case 5:
          this.RemoteTab = (TabItem) target;
          break;
        case 6:
          this.ClientsPage = (TabItem) target;
          break;
        case 7:
          this.DevicesPage = (System.Windows.Controls.Frame) target;
          break;
        case 8:
          this.NewDevicesPage = (System.Windows.Controls.Frame) target;
          break;
        case 9:
          this.BillTabItem = (TabItem) target;
          break;
        case 10:
          this.CafeTabItem = (TabItem) target;
          break;
        case 11:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase2_OnClick);
          break;
        case 12:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.buttonCleareStocks_OnClick);
          break;
        case 13:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClick_polycloudtest);
          break;
        case 14:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClick_lev);
          break;
        case 15:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_DbDeadLock);
          break;
        case 16:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClickMsgBoxCommandsList);
          break;
        case 17:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClick_checkboxes);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
