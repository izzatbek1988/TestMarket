// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DataBasePageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Clients;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Goods;
using Gbs.Core.Db.Payments;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Core.Entities.Settings;
using Gbs.Forms._shared;
using Gbs.Forms.Other;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Market5;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB;
using LinqToDB.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class DataBasePageViewModel : ViewModelWithForm
  {
    private bool _isActiveDeleteData;
    private List<DataBasePageViewModel.DeleteTableClass> _deleteTableDictionary = new List<DataBasePageViewModel.DeleteTableClass>()
    {
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.Sales, Translate.FrmSummaryReport_Продажи),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.Waybills, Translate.PageVisualModelView_Поступления),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.WriteOffs, Translate.DataBasePageViewModel__deleteTableDictionary_Списания_товаров),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.Inventory, Translate.DataBasePageViewModel__deleteTableDictionary_Инвентаризации),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.ClientOrders, Translate.FrmMainWindow_ЗаказыРезервы),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.Move, Translate.ReportType_SendWaybill_Перемещения_между_точками),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.MoveStorage, Translate.ReportType_SendWaybillStorage_Перемещения_между_складами),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.Production, Translate.JournalGoodViewModel_JournalGoodViewModel_Производство),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.PaymentActions, Translate.FrmSummaryReport_ДвижениеСредств),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.NullStocks, Translate.DataBasePageViewModel__deleteTableDictionary_Обнулить_остатки),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.GoodsCategory, Translate.FormGroup_КатегорииТоваров),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.Goods, Translate.UserGroupViewModel_UserGroupViewModel_Товары),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.ClientCategory, Translate.FrmClientGroup_ГруппыКонтактов),
      new DataBasePageViewModel.DeleteTableClass(DataBasePageViewModel.TableType.Clients, Translate.ClientsGroups_Контакты)
    };
    private Setting _dbSetting = UidDb.GetUid();
    private string _oldServerUrl;
    private bool _isImportImage;

    public ICommand DoSqlCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (MessageBoxHelper.Show(Translate.DataBasePageViewModel_DoSqlCommand_Вы_уверены__что_хотите_выполнить_SQL_скрипт__Данное_действие_можно_отменить_только_восстановлением_из_резервной_копии_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) == MessageBoxResult.No)
            return;
          Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog()
          {
            Filter = "SQL language (*.sql)|*.sql",
            Multiselect = false,
            DefaultExt = ".sql",
            InitialDirectory = ApplicationInfo.GetInstance().Paths.DataPath
          };
          bool? nullable = openFileDialog.ShowDialog();
          bool flag = false;
          if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
            return;
          string fileName = openFileDialog.FileName;
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DataBasePageViewModel_DoSqlCommand_Подготовка_к_выполнению_SQL_скрипта);
          BackupHelper.CreateBackup();
          progressBar.Close();
          DataBaseHelper.DoSql(fileName);
        }));
      }
    }

    public bool IsCreateDbInBackup
    {
      get
      {
        Gbs.Core.Config.DataBase dataBaseConfig = this.DataBaseConfig;
        return dataBaseConfig == null || dataBaseConfig.BackUp.CreateDbInBackup;
      }
      set
      {
        if (!value && this.DataBaseConfig.BackUp.CreateDbInBackup)
        {
          int num = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_IsCreateDbInBackup_Отключение_резервного_копирования_базы_данных_может_привести_к_потере_данных__Рекомендуем_отключать_эту_опцию_только_на_устройствах__которые_по_сети_подключены_к_базе_данных__клиенты__, icon: MessageBoxImage.Exclamation);
        }
        this.DataBaseConfig.BackUp.CreateDbInBackup = value;
      }
    }

    public Visibility VisibilityDebug
    {
      get => !DevelopersHelper.IsDebug() ? Visibility.Collapsed : Visibility.Visible;
    }

    public Visibility VisibilityExtraSetting
    {
      get
      {
        Gbs.Core.Config.DataBase dataBaseConfig = this.DataBaseConfig;
        return (dataBaseConfig != null ? (dataBaseConfig.ModeProgram == GlobalDictionaries.Mode.Home ? 1 : 0) : 0) == 0 ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Gbs.Core.Entities.Users.User AuthUser { get; set; }

    public bool IsActiveDeleteData
    {
      get => this._isActiveDeleteData;
      set
      {
        this._isActiveDeleteData = value;
        int num = value ? 1 : 0;
      }
    }

    public ICommand DeleteAllDataCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            if (MessageBoxHelper.Show(string.Format(Translate.DataBasePageViewModel_DeleteAllDataCommand_, (object) ApplicationInfo.GetInstance().Paths.DataPath), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            BackupHelper.CreateBackup();
            DataBaseHelper.CloseConnection();
            Directory.Delete(ApplicationInfo.GetInstance().Paths.DataPath, true);
            Gbs.Helpers.Other.RestartApplication(false);
          }
          catch (Exception ex)
          {
            LogHelper.Error(ex, "Не удалось удалить все данные и настройки программы. Обратитесь в службу технической поддержки.");
          }
        }));
      }
    }

    public string TextPropButton
    {
      get
      {
        int num = this.DeleteTableDictionary.Count<DataBasePageViewModel.DeleteTableClass>((Func<DataBasePageViewModel.DeleteTableClass, bool>) (x => x.IsChecked));
        switch (num)
        {
          case 0:
            return Translate.DataBasePageViewModel_TextPropButton_Таблицы_не_выбраны;
          case 1:
            return this.DeleteTableDictionary.First<DataBasePageViewModel.DeleteTableClass>((Func<DataBasePageViewModel.DeleteTableClass, bool>) (x => x.IsChecked)).Name;
          default:
            return string.Format(Translate.DataBasePageViewModel_TextPropButton_Таблиц___0_, (object) num);
        }
      }
    }

    public List<DataBasePageViewModel.DeleteTableClass> DeleteTableDictionary
    {
      get => this._deleteTableDictionary;
      set
      {
        this._deleteTableDictionary = value;
        this.OnPropertyChanged("TextPropButton");
      }
    }

    public ICommand DeleteTableCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            if (!this.DeleteTableDictionary.Any<DataBasePageViewModel.DeleteTableClass>((Func<DataBasePageViewModel.DeleteTableClass, bool>) (x => x.IsChecked)))
            {
              int num = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Необходимо_выбрать_хотя_бы_одну_таблицу_для_удаления_данных_);
            }
            else
            {
              if (MessageBoxHelper.Show(Translate.DataBasePageViewModel_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DataBasePageViewModel_Удаление_данных);
              BackupHelper.CreateBackup();
              using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
              {
                DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
                DataBasePageViewModel.\u003C\u003Ec__DisplayClass28_0 cDisplayClass280;
                cDisplayClass280.docs = dataBase.GetTable<DOCUMENTS>();
                cDisplayClass280.items = dataBase.GetTable<DOCUMENT_ITEMS>();
                cDisplayClass280.payments = dataBase.GetTable<PAYMENTS>();
                foreach (DataBasePageViewModel.DeleteTableClass deleteTableClass in this.DeleteTableDictionary.Where<DataBasePageViewModel.DeleteTableClass>((Func<DataBasePageViewModel.DeleteTableClass, bool>) (x => x.IsChecked)))
                {
                  DataBasePageViewModel.\u003C\u003Ec__DisplayClass28_1 cDisplayClass281 = new DataBasePageViewModel.\u003C\u003Ec__DisplayClass28_1();
                  switch (deleteTableClass.Type)
                  {
                    case DataBasePageViewModel.TableType.Sales:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.Sale, ref cDisplayClass280);
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.SaleReturn, ref cDisplayClass280);
                      cDisplayClass280.payments.Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.TYPE == 1 || x.TYPE == 5 || x.TYPE == 4)).Delete<PAYMENTS>();
                      continue;
                    case DataBasePageViewModel.TableType.Waybills:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.Buy, ref cDisplayClass280);
                      continue;
                    case DataBasePageViewModel.TableType.WriteOffs:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.WriteOff, ref cDisplayClass280);
                      continue;
                    case DataBasePageViewModel.TableType.PaymentActions:
                      cDisplayClass280.payments.Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.TYPE == 6 || x.TYPE == 3 || x.TYPE == 2 || x.TYPE == 0 || x.TYPE == 1)).Delete<PAYMENTS>();
                      continue;
                    case DataBasePageViewModel.TableType.Inventory:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.Inventory, ref cDisplayClass280);
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.InventoryAct, ref cDisplayClass280);
                      continue;
                    case DataBasePageViewModel.TableType.Move:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.Move, ref cDisplayClass280);
                      continue;
                    case DataBasePageViewModel.TableType.ClientOrders:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.ClientOrder, ref cDisplayClass280);
                      continue;
                    case DataBasePageViewModel.TableType.Production:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.ProductionList, ref cDisplayClass280);
                      continue;
                    case DataBasePageViewModel.TableType.NullStocks:
                      using (List<GOODS_STOCK>.Enumerator enumerator = dataBase.GetTable<GOODS_STOCK>().ToList<GOODS_STOCK>().GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          GOODS_STOCK current = enumerator.Current;
                          current.STOCK = 0M;
                          dataBase.InsertOrReplace<GOODS_STOCK>(current);
                        }
                        continue;
                      }
                    case DataBasePageViewModel.TableType.GoodsCategory:
                      dataBase.GetTable<GOODS_GROUPS>().Delete<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => x.UID != GlobalDictionaries.PercentForServiceGroupUid));
                      GoodGroups.Group group1 = new GoodGroups.Group()
                      {
                        Name = "_delete"
                      };
                      new GoodGroupsRepository(dataBase).Save(group1);
                      using (List<GOODS>.Enumerator enumerator = dataBase.GetTable<GOODS>().ToList<GOODS>().GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          GOODS current = enumerator.Current;
                          current.GROUP_UID = group1.Uid;
                          dataBase.InsertOrReplace<GOODS>(current);
                        }
                        continue;
                      }
                    case DataBasePageViewModel.TableType.Goods:
                      dataBase.GetTable<GOODS>().Delete<GOODS>();
                      continue;
                    case DataBasePageViewModel.TableType.ClientCategory:
                      dataBase.GetTable<CLIENTS_GROUPS>().Delete<CLIENTS_GROUPS>();
                      Gbs.Core.Entities.Clients.Group group2 = new Gbs.Core.Entities.Clients.Group()
                      {
                        Name = "_delete"
                      };
                      new GroupRepository(dataBase).Save(group2);
                      using (List<CLIENTS>.Enumerator enumerator = dataBase.GetTable<CLIENTS>().ToList<CLIENTS>().GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          CLIENTS current = enumerator.Current;
                          current.GROUP_UID = group2.Uid;
                          dataBase.InsertOrReplace<CLIENTS>(current);
                        }
                        continue;
                      }
                    case DataBasePageViewModel.TableType.Clients:
                      IQueryable<CLIENTS> table = dataBase.GetTable<CLIENTS>();
                      cDisplayClass281.u = dataBase.GetTable<USERS>();
                      SalePoints.SalePoint salePoint = SalePoints.GetSalePointList(dataBase.GetTable<SALE_POINTS>().Where<SALE_POINTS>((Expression<Func<SALE_POINTS, bool>>) (x => x.IS_DELETED == false))).FirstOrDefault<SalePoints.SalePoint>();
                      if (salePoint == null)
                        salePoint = new SalePoints.SalePoint()
                        {
                          Organization = new Gbs.Core.Entities.Clients.Client()
                        };
                      cDisplayClass281.point = salePoint;
                      ParameterExpression parameterExpression1;
                      ParameterExpression parameterExpression2;
                      table.Delete<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => cDisplayClass281.u.Any<USERS>(System.Linq.Expressions.Expression.Lambda<Func<USERS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(user.CLIENT_UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (CLIENTS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2)) == false && x.UID != cDisplayClass281.point.Organization.Uid));
                      continue;
                    case DataBasePageViewModel.TableType.MoveStorage:
                      DataBasePageViewModel.\u003Cget_DeleteTableCommand\u003Eg__deleteDocs\u007C28_2(GlobalDictionaries.DocumentsTypes.MoveStorage, ref cDisplayClass280);
                      continue;
                    default:
                      throw new ArgumentOutOfRangeException();
                  }
                }
                connectionTransaction.Commit();
                progressBar.Close();
                CacheHelper.ClearAll();
                Cache.ClearAndLoadCache();
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.DataBasePageViewModel_Процесс_удаления_выбранных_таблиц_завершен_));
              }
            }
          }
          catch (Exception ex)
          {
            ProgressBarHelper.Close();
            LogHelper.Error(ex, "Не удалось удалить выбранные данные из программы. Обратитесь в службу технической поддержки.");
          }
        }));
      }
    }

    public Gbs.Core.Config.DataBase DataBaseConfig { get; set; }

    public ICommand SelectBackUpPathCommand { get; set; }

    public ICommand SelectImagePathCommand { get; set; }

    public ICommand SelectImagePathForImportCommand { get; set; }

    public ICommand SelectDbFilePathCommand { get; set; }

    public ICommand OpenDataFolder { get; set; }

    public ICommand OpenProgramFolder { get; set; }

    public Dictionary<Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods, string> BackUpPeriodsDictionary
    {
      get
      {
        return new Dictionary<Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods, string>()
        {
          {
            Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.None,
            Translate.DataBasePageViewModel_Не_создавать_доп__резервные_копии
          },
          {
            Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery1Hour,
            Translate.DataBasePageViewModel___каждый_час
          },
          {
            Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery3Hours,
            Translate.DataBasePageViewModel___каждые_3_часа
          },
          {
            Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery6Hours,
            Translate.DataBasePageViewModel___каждые_6_часов
          },
          {
            Gbs.Core.Config.DataBase.DbBackUp.CreateBackupPeriods.AndEvery12Hours,
            Translate.DataBasePageViewModel___каждые_12_часов
          }
        };
      }
    }

    public Setting DataBaseSetting
    {
      get => this._dbSetting;
      set
      {
        this._dbSetting = value;
        this.OnPropertyChanged(nameof (DataBaseSetting));
      }
    }

    public string DataBasePath
    {
      get => this.DataBaseConfig?.Connection?.Path ?? string.Empty;
      set
      {
        Gbs.Core.Config.DataBase dataBaseConfig1 = this.DataBaseConfig;
        int num1;
        if (dataBaseConfig1 == null)
        {
          num1 = 1;
        }
        else
        {
          Gbs.Core.Config.DataBase.DbConnection connection = dataBaseConfig1.Connection;
          if (connection == null)
          {
            num1 = 1;
          }
          else
          {
            int serverPort = connection.ServerPort;
            num1 = 0;
          }
        }
        if (num1 == 0)
        {
          Gbs.Core.Config.DataBase dataBaseConfig2 = this.DataBaseConfig;
          bool? nullable;
          if (dataBaseConfig2 == null)
          {
            nullable = new bool?();
          }
          else
          {
            Gbs.Core.Config.DataBase.DbConnection connection = dataBaseConfig2.Connection;
            if (connection == null)
            {
              nullable = new bool?();
            }
            else
            {
              string serverUrl = connection.ServerUrl;
              nullable = serverUrl != null ? new bool?(serverUrl.IsNullOrEmpty()) : new bool?();
            }
          }
          if (!nullable.GetValueOrDefault(true))
          {
            if ((this.DataBaseConfig.Connection.ServerUrl.Contains("127.0.0.1") || this.DataBaseConfig.Connection.ServerUrl.Contains("localhost")) && !File.Exists(value))
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Выбранный_файл_не_существует__укажите_другой_путь_к_базе_данных_);
              return;
            }
            if (new FileInfo(value).Extension.ToLower() != ".fdb")
            {
              int num3 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Выбранный_файл_не_подходит__формат_базы_данных_должен_быть__FDB__укажите_другой_путь_к_базе_данных_);
              return;
            }
            Gbs.Core.Config.DataBase db = this.DataBaseConfig.Clone<Gbs.Core.Config.DataBase>();
            db.Connection.Path = value;
            if (!this.PingDb(db))
            {
              int num4 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Не_удается_подключиться_к_указанному_файлу__проверьте_настройки_соединения_);
              return;
            }
            this.DataBaseConfig.Connection.Path = value;
            this.OnPropertyChanged(nameof (DataBasePath));
            return;
          }
        }
        int num5 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Для_изменения_пути_к_файлу_базы_данных_требуется_указать_хост_и_порт_для_подключения_);
        this.OnPropertyChanged(nameof (DataBasePath));
      }
    }

    public ICommand TestConnectionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.PingDb(this.DataBaseConfig))
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Соединение_с_базой_данных_было_успешно_установлено_);
          }
          else
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_Не_удалось_подключиться_к_базе_по_указанному_IP_адресу__Возможно__что_служба_Firebird_не_запущена_или_IP_указан_не_верно, icon: MessageBoxImage.Hand);
          }
        }));
      }
    }

    private bool PingDb(Gbs.Core.Config.DataBase db)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DataBasePageViewModel_Проверка_соединения_с_базой_данных_);
      int num = DataBaseHelper.PingToDb(db) ? 1 : 0;
      progressBar.Close();
      return num != 0;
    }

    public ICommand GeneratedIdDatabase
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (MessageBoxHelper.Show(Translate.DataBasePageViewModel_Вы_уверены__что_хотите_сменить_идентификатор_базы_данных_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
            return;
          this.DataBaseSetting.EntityUid = Guid.NewGuid();
          UidDb.SetUid(this.DataBaseSetting);
          this.OnPropertyChanged("DataBaseSetting");
        }));
      }
    }

    public DataBasePageViewModel()
    {
    }

    public DataBasePageViewModel(Gbs.Core.Entities.Users.User authUser, Gbs.Core.Config.DataBase dataBaseConfig)
    {
      this.AuthUser = authUser;
      this.DataBaseConfig = dataBaseConfig;
      this._oldServerUrl = dataBaseConfig.Connection.ServerUrl;
      this.SelectBackUpPathCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
        {
          ShowNewFolderButton = true,
          Description = Translate.DataBasePageViewModel_Выберите_папку_для_сохранения_резервных_копий
        };
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
          return;
        this.DataBaseConfig.BackUp.Path = folderBrowserDialog.SelectedPath;
        this.OnPropertyChanged(nameof (DataBaseConfig));
      }));
      this.SelectDbFilePathCommand = (ICommand) new RelayCommand((Action<object>) (_ =>
      {
        Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
        if (!openFileDialog.ShowDialog().GetValueOrDefault())
          return;
        this.DataBasePath = openFileDialog.FileName;
        this.OnPropertyChanged(nameof (DataBaseConfig));
      }));
      this.OpenDataFolder = (ICommand) new RelayCommand((Action<object>) (obj => FileSystemHelper.OpenFolder(ApplicationInfo.GetInstance().Paths.DataPath)));
      this.OpenProgramFolder = (ICommand) new RelayCommand((Action<object>) (obj => FileSystemHelper.OpenFolder(ApplicationInfo.GetInstance().Paths.ApplicationPath)));
      this.SelectImagePathCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
          return;
        this.DataBaseConfig.GoodsImagesPath = folderBrowserDialog.SelectedPath;
        this.OnPropertyChanged(nameof (DataBaseConfig));
      }));
      this.SelectImagePathForImportCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = Translate.DataBasePageViewModel_DataBasePageViewModel_Укажите_путь_к_папке_с_изображениями__которая_используется_в_v_5;
        folderBrowserDialog.ShowNewFolderButton = false;
        if (folderBrowserDialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
          return;
        this.PathFolderImageForImport = folderBrowserDialog.SelectedPath;
        this.OnPropertyChanged(nameof (PathFolderImageForImport));
      }));
    }

    public bool Save()
    {
      if (this._oldServerUrl != this.DataBaseConfig.Connection.ServerUrl)
      {
        if (!this.DataBaseConfig.Connection.ServerUrl.ToLower().IsEither<string>("localhost", "127.0.0.1") && !DataBaseHelper.PingToDb(this.DataBaseConfig))
        {
          MessageBoxHelper.Warning(string.Format(Translate.DataBasePageViewModel_Save_Не_удалось_подключиться_к_базе_данных_по_адресу__0___Проверьте_корректность_указания_адреса__настройки_брандмауэра_или_антивируса_и_сохраните_настройки_еще_раз_, (object) this.DataBaseConfig.Connection.ServerUrl));
          return false;
        }
      }
      return new ConfigsRepository<Gbs.Core.Config.DataBase>().Save(this.DataBaseConfig) && UidDb.SetUid(this.DataBaseSetting);
    }

    public Visibility VisibilityPathImageFor5 { get; set; } = Visibility.Collapsed;

    public bool IsImportImage
    {
      get => this._isImportImage;
      set
      {
        this.VisibilityPathImageFor5 = value ? Visibility.Visible : Visibility.Collapsed;
        this.OnPropertyChanged("VisibilityPathImageFor5");
        this._isImportImage = value;
      }
    }

    public bool IsImportOnlyImage { get; set; }

    public string PathFolderImageForImport { get; set; } = "C:\\ProgramData\\F-Lab\\GBS.Market\\5\\Goods\\images";

    public ICommand ImportGbs5Command
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.IsImportImage && this.PathFolderImageForImport.IsNullOrEmpty())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_ImportGbs5Command_Для_переноса_фото_из_v_5_необходимо_ввести_путь__который_указан_в_настройках_v_5_в_качестве_папки_с_фото_, icon: MessageBoxImage.Exclamation);
          }
          else if (this.IsImportImage && !Directory.Exists(this.PathFolderImageForImport))
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_ImportGbs5Command_Указанная_папка_с_фото_из_5_й_версии_не_найдена__Проверьте_корректности_указанного_пути_или_отключите_перенос_фото_, icon: MessageBoxImage.Exclamation);
          }
          else if (this.IsImportOnlyImage && this.IsImportImage && !this.PathFolderImageForImport.IsNullOrEmpty())
          {
            LogHelper.Debug("Переносим ТОЛЬКО фото из Market 5");
            FrmProgressInfo frmProgressInfo = new FrmProgressInfo();
            Task.Run((Action) (() =>
            {
              Market5ImportHelper.ImportOnlyImageOfMarket5(this.PathFolderImageForImport);
              int num3 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_ImportGbs5Command_Импорт_данных_из_v_5_завершен);
            }));
            Market5ImportHelper.ProgressInfo = (ProgressInfoViewModel) frmProgressInfo.DataContext;
            frmProgressInfo.ShowDialog();
          }
          else
          {
            new Stopwatch().Start();
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
              Filter = Translate.PageDataBase_Файл_JSON + "(*.json)|*.json;",
              Multiselect = false
            };
            if (!openFileDialog.ShowDialog().GetValueOrDefault())
              return;
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.PageDataBase_Загрузка_файла);
            Market5Data data = JsonConvert.DeserializeObject<Market5Data>(File.ReadAllText(openFileDialog.FileName));
            progressBar.Close();
            FrmProgressInfo frmProgressInfo = new FrmProgressInfo();
            this.PathFolderImageForImport = this.IsImportImage ? this.PathFolderImageForImport : string.Empty;
            Task.Run((Action) (() =>
            {
              Market5ImportHelper.ImportDataOfMarket5(data, this.PathFolderImageForImport);
              int num4 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_ImportGbs5Command_Импорт_данных_из_v_5_завершен);
            }));
            Market5ImportHelper.ProgressInfo = (ProgressInfoViewModel) frmProgressInfo.DataContext;
            frmProgressInfo.ShowDialog();
          }
        }));
      }
    }

    public ICommand CreateBackupCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          try
          {
            using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog()
            {
              Filter = Translate.DataBasePageViewModel_Архив_с_резервной_копией_ + "(*.zip)|*.zip;",
              AddExtension = true
            })
            {
              if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
              FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DataBasePageViewModel_Создание_резервной_копии);
              BackupHelper.CreateBackup(fileInfo.DirectoryName, fileInfo.Name);
              progressBar.Close();
              ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
              {
                Title = Translate.DataBasePageViewModel_Создание_резервной_копии,
                Text = Translate.DataBasePageViewModel_Резервная_копия_успешно_создана_
              });
            }
          }
          catch
          {
            ProgressBarHelper.Close();
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
            {
              Title = Translate.DataBasePageViewModel_Создание_резервной_копии,
              Text = Translate.DataBasePageViewModel_Во_время_создания_резервной_копии_произошла_ошибка_
            });
          }
        }));
      }
    }

    public ICommand RestoreBackupCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog()
          {
            Filter = Translate.DataBasePageViewModel_Архив_с_резервной_копией_ + "(*.zip)|*.zip;",
            Multiselect = false
          };
          bool? nullable = openFileDialog.ShowDialog();
          bool flag = false;
          if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
            return;
          Gbs.Forms.Other.MessageBox.MsgBoxResult msgBoxResult = MessageBoxHelper.ShowWithCheckboxes(Translate.DataBasePageViewModel_RestoreBackupCommand_Выберите_данные__которые_необходимо_восстановить_из_резервной_копии_, icon: MessageBoxImage.Question, checkboxes: new List<MessBoxViewModel.CheckboxItem>()
          {
            new MessBoxViewModel.CheckboxItem(0, Translate.FrmSettings_БазаДанных, true),
            new MessBoxViewModel.CheckboxItem(1, Translate.DataBasePageViewModel_RestoreBackupCommand_Настройки_программы, true)
          });
          if (msgBoxResult.Result == MessageBoxResult.Cancel)
            return;
          if (msgBoxResult.Checkboxes.All<MessBoxViewModel.CheckboxItem>((Func<MessBoxViewModel.CheckboxItem, bool>) (x => x.IsChecked)))
            DataBaseHelper.RestoreAllBackUp(openFileDialog.FileName);
          else if (msgBoxResult.Checkboxes.All<MessBoxViewModel.CheckboxItem>((Func<MessBoxViewModel.CheckboxItem, bool>) (x => !x.IsChecked)))
          {
            MessageBoxHelper.Warning(Translate.DataBasePageViewModel_RestoreBackupCommand_Необходимо_выбрать_хотя_бы_один_вариант_для_восстановления_данных_);
          }
          else
          {
            foreach (MessBoxViewModel.CheckboxItem checkbox in msgBoxResult.Checkboxes)
            {
              if (checkbox.Id == 0 && checkbox.IsChecked)
                DataBaseHelper.RestoreBackUp(openFileDialog.FileName);
              if (checkbox.Id == 1 && checkbox.IsChecked)
                DataBaseHelper.RestoreConfigBackUp(openFileDialog.FileName);
            }
          }
        }));
      }
    }

    public ICommand RemoveNullItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DataBasePageViewModel_RemoveNullItemCommand_Очистка_базы_данных_от__мусора_);
          try
          {
            string filePath = Path.Combine(ApplicationInfo.GetInstance().Paths.DataPath, "main.fdb");
            string sizeFile1 = FileSystemHelper.GetSizeFile(filePath);
            using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
            {
              List<GoodGroups.Group> byQuery = new GoodGroupsRepository(dataBase).GetByQuery(dataBase.GetTable<GOODS_GROUPS>().Where<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => x.IS_DELETED == true)));
              IEnumerable<Gbs.Core.Entities.Clients.Group> source = new GroupRepository().GetAllItems().Where<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (x => x.IsDeleted));
              List<Document> allDocuments = new DocumentsRepository(dataBase).GetActiveItems();
              IEnumerable<Gbs.Core.Entities.Documents.Item> allDocumentsItems = allDocuments.SelectMany<Document, Gbs.Core.Entities.Documents.Item>((Func<Document, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Items));
              List<Gbs.Core.Entities.Goods.Good> allGood = new GoodRepository(dataBase).GetAllItems();
              List<Gbs.Core.Entities.Clients.Client> allClients = new ClientsRepository(dataBase).GetAllItems();
              List<Gbs.Core.Entities.Users.User> allUsers = new UsersRepository(dataBase).GetAllItems();
              List<Gbs.Core.Entities.Goods.Good> list1 = allGood.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.IsDeleted)).ToList<Gbs.Core.Entities.Goods.Good>();
              List<Gbs.Core.Entities.Clients.Client> list2 = allClients.Where<Gbs.Core.Entities.Clients.Client>((Func<Gbs.Core.Entities.Clients.Client, bool>) (x => x.IsDeleted)).ToList<Gbs.Core.Entities.Clients.Client>();
              List<Gbs.Core.Entities.Goods.Good> list3 = list1.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => allDocumentsItems.All<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (i => i.GoodUid != x.Uid)))).ToList<Gbs.Core.Entities.Goods.Good>();
              List<Gbs.Core.Entities.Clients.Client> list4 = list2.Where<Gbs.Core.Entities.Clients.Client>((Func<Gbs.Core.Entities.Clients.Client, bool>) (x => allDocuments.All<Document>((Func<Document, bool>) (i => i.ContractorUid != x.Uid)) && allUsers.All<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (u =>
              {
                Gbs.Core.Entities.Clients.Client client = u.Client;
                return (client != null ? __nonvirtual (client.Uid) : Guid.Empty) != x.Uid;
              })))).ToList<Gbs.Core.Entities.Clients.Client>();
              List<GoodGroups.Group> list5 = byQuery.Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => allGood.All<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (g =>
              {
                GoodGroups.Group group = g.Group;
                return (group != null ? __nonvirtual (group.Uid) : Guid.Empty) != x.Uid;
              })))).ToList<GoodGroups.Group>();
              IEnumerable<Gbs.Core.Entities.Clients.Group> list6 = (IEnumerable<Gbs.Core.Entities.Clients.Group>) source.Where<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (x => allClients.All<Gbs.Core.Entities.Clients.Client>((Func<Gbs.Core.Entities.Clients.Client, bool>) (g =>
              {
                Gbs.Core.Entities.Clients.Group group = g.Group;
                return (group != null ? __nonvirtual (group.Uid) : Guid.Empty) != x.Uid;
              })))).ToList<Gbs.Core.Entities.Clients.Group>();
              foreach (Gbs.Core.Entities.Goods.Good good in list3)
              {
                Gbs.Core.Entities.Goods.Good g = good;
                dataBase.GetTable<GOODS>().Delete<GOODS>((Expression<Func<GOODS, bool>>) (x => x.UID == g.Uid));
              }
              foreach (Gbs.Core.Entities.Clients.Client client in list4)
              {
                Gbs.Core.Entities.Clients.Client g = client;
                dataBase.GetTable<CLIENTS>().Delete<CLIENTS>((Expression<Func<CLIENTS, bool>>) (x => x.UID == g.Uid));
              }
              foreach (GoodGroups.Group group in list5)
              {
                GoodGroups.Group g = group;
                dataBase.GetTable<GOODS_GROUPS>().Delete<GOODS_GROUPS>((Expression<Func<GOODS_GROUPS, bool>>) (x => x.UID == g.Uid));
              }
              foreach (Gbs.Core.Entities.Clients.Group group in list6)
              {
                Gbs.Core.Entities.Clients.Group g = group;
                dataBase.GetTable<CLIENTS_GROUPS>().Delete<CLIENTS_GROUPS>((Expression<Func<CLIENTS_GROUPS, bool>>) (x => x.UID == g.Uid));
              }
              if (!DataBaseHelper.CompressDb())
              {
                progressBar.Close();
                int num = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_CompressDbCommand_Не_удалось_сжать_базу_данных__обратитесь_в_службу_технической_поддержки_);
              }
              else
              {
                progressBar.Close();
                string sizeFile2 = FileSystemHelper.GetSizeFile(filePath);
                ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.DataBasePageViewModel_CompressDbCommand_База_данных_была_успешно_сжата__размер_до_операции_сжатия____0___после____1__, (object) sizeFile1, (object) sizeFile2)));
              }
              progressBar.Close();
              int num1 = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_RemoveNullItemCommand_База_данных_успешно_очищена_);
            }
          }
          catch (Exception ex)
          {
            progressBar.Close();
            MessageBoxHelper.Error(Translate.DataBasePageViewModel_RemoveNullItemCommand_Не_удалось_очистить_базу_данных__попробуйте_еще_раз_или_обратитесь_в_службу_поддержки_);
            LogHelper.Error(ex, "Ошибка очищения БД", false);
          }
        }));
      }
    }

    public ICommand CompressDbCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string filePath = Path.Combine(ApplicationInfo.GetInstance().Paths.DataPath, "main.fdb");
          string sizeFile1 = FileSystemHelper.GetSizeFile(filePath);
          if (MessageBoxHelper.Show(Translate.DataBasePageViewModel_CompressDbCommand_Для_успешного_сжатия_не_должно_быть_активных_подключений_к_базе_данных__Продолжить_восстановление_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
            return;
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SplashScreenViewModel_ApplicationLoad_Сжатие_базы_данных);
          if (!DataBaseHelper.CompressDb())
          {
            progressBar.Close();
            int num = (int) MessageBoxHelper.Show(Translate.DataBasePageViewModel_CompressDbCommand_Не_удалось_сжать_базу_данных__обратитесь_в_службу_технической_поддержки_);
          }
          else
          {
            progressBar.Close();
            string sizeFile2 = FileSystemHelper.GetSizeFile(filePath);
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.DataBasePageViewModel_CompressDbCommand_База_данных_была_успешно_сжата__размер_до_операции_сжатия____0___после____1__, (object) sizeFile1, (object) sizeFile2)));
          }
        }));
      }
    }

    public class DeleteTableClass
    {
      public DataBasePageViewModel.TableType Type { get; set; }

      public string Name { get; set; }

      public bool IsChecked { get; set; }

      public DeleteTableClass(DataBasePageViewModel.TableType type, string name)
      {
        this.Type = type;
        this.Name = name;
      }

      public DeleteTableClass()
      {
      }
    }

    public enum TableType
    {
      Sales,
      Waybills,
      WriteOffs,
      PaymentActions,
      Inventory,
      Move,
      ClientOrders,
      Production,
      NullStocks,
      GoodsCategory,
      Goods,
      ClientCategory,
      Clients,
      MoveStorage,
    }
  }
}
