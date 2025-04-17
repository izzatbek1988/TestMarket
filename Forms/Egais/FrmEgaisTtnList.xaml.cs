// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.EgaisTtnListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Forms.Waybills;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Egais
{
  public partial class EgaisTtnListViewModel : ViewModelWithForm
  {
    private List<EgaisDocument> _selectedDocuments;
    private EgaisDocument.WaybillStatuses _selectedWaybillStatuses;
    private EgaisHelper.StatusEgaisTTN _selectedStatusEgaisTTN;
    private Users.User _user;

    public ICommand DoOldWaybillsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.EgaisTtnListViewModel_DoOldWaybillsCommand_Отправляем_запрос_на_получение_неотработанных_накладных_);
          try
          {
            new EgaisRepository().GetOldWaybill();
            progressBar.Close();
            int num = (int) MessageBoxHelper.Show(Translate.EgaisTtnListViewModel_DoOldWaybillsCommand_Запрос_на_получение_необработанных_накладных_успешно_отправлен_в_УТМ__Сейчас_запрос_обрабатывается__Рекомендуем_проверить_наличие_накладных_через_5_10_минут_);
          }
          catch (Exception ex)
          {
            progressBar.Close();
            string str = string.Format(Translate.EgaisTtnListViewModel_DoOldWaybillsCommand_, (object) ex.Message);
            MessageBoxHelper.Error(str);
            LogHelper.WriteToEgaisLog(str, ex);
            LogHelper.Error(ex, str, false);
          }
        }));
      }
    }

    public ICommand GetSingleWaybillsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string str1 = "TTN-";
          if (this._selectedDocuments.Count == 1)
            str1 = this._selectedDocuments.Single<EgaisDocument>().Form2.WBRegId;
          while (true)
          {
            (bool result, string output) tuple = MessageBoxHelper.Input(str1, Translate.EgaisTtnListViewModel_GetSingleWaybillsCommand_Введите_номер_накладной__которую_хотите_повторно_запросить_в_УТМ_, 10);
            if (tuple.result)
            {
              str1 = tuple.output;
              if (!tuple.output.StartsWith("TTN-"))
                MessageBoxHelper.Error(Translate.EgaisTtnListViewModel_GetSingleWaybillsCommand_Введен_некорректный_номер_накладной__пример_правильного_формата__TTN_0000000000__повторите_ввод_или_отмените_операцию_);
              else
                goto label_5;
            }
            else
              break;
          }
          return;
label_5:
          ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.EgaisTtnListViewModel_GetSingleWaybillsCommand_Отправляем_запрос_на_повторное_получение_накладной_);
          try
          {
            new EgaisRepository().GetSingleWaybill(str1);
            progressBar.Close();
            int num = (int) MessageBoxHelper.Show(string.Format(Translate.EgaisTtnListViewModel_GetSingleWaybillsCommand_Запрос_на_повторное_получение_накладной__0__успешно_отправлен_в_УТМ__Сейчас_запрос_обрабатывается__Рекомендуем_проверить_наличие_данных_через_5_10_минут_, (object) str1));
          }
          catch (Exception ex)
          {
            progressBar.Close();
            string str2 = string.Format(Translate.EgaisTtnListViewModel_GetSingleWaybillsCommand_, (object) ex.Message);
            MessageBoxHelper.Error(str2);
            LogHelper.WriteToEgaisLog(str2, ex);
          }
        }));
      }
    }

    public Action ShowMenu { get; set; }

    public ICommand ShowMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._selectedDocuments = ((IEnumerable) obj).Cast<EgaisDocument>().ToList<EgaisDocument>();
          this.ShowMenu();
        }));
      }
    }

    public static string MoreMenuKey => "MoreMenu";

    public DateTime ValueDateTimeStart { get; set; }

    public DateTime ValueDateTimeEnd { get; set; }

    public EgaisDocument.WaybillStatuses SelectedWaybillStatuses
    {
      get => this._selectedWaybillStatuses;
      set
      {
        this._selectedWaybillStatuses = value;
        this.Search();
      }
    }

    public EgaisHelper.StatusEgaisTTN SelectedStatusEgaisTTN
    {
      get => this._selectedStatusEgaisTTN;
      set
      {
        this._selectedStatusEgaisTTN = value;
        this.Search();
      }
    }

    public ICommand SearchCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (o => this.Search()));
    }

    private void Search()
    {
      List<EgaisDocument> egaisDocumentList = new List<EgaisDocument>();
      List<EgaisDocument> source = new List<EgaisDocument>(this.CacheDocuments.Where<EgaisDocument>((Func<EgaisDocument, bool>) (x =>
      {
        DateTime dateTime1 = x.Waybill.WBDate;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.ValueDateTimeStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.Waybill.WBDate;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.ValueDateTimeEnd;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      })));
      if (this.SelectedStatusEgaisTTN != EgaisHelper.StatusEgaisTTN.All)
        source = new List<EgaisDocument>(source.Where<EgaisDocument>((Func<EgaisDocument, bool>) (x => x.StatusTtn == this.SelectedStatusEgaisTTN)));
      if (this.SelectedWaybillStatuses != EgaisDocument.WaybillStatuses.All)
        source = new List<EgaisDocument>(source.Where<EgaisDocument>((Func<EgaisDocument, bool>) (x => x.StatusWaybill == this.SelectedWaybillStatuses)));
      this.Documents = new ObservableCollection<EgaisDocument>((IEnumerable<EgaisDocument>) source.OrderBy<EgaisDocument, DateTime>((Func<EgaisDocument, DateTime>) (x => x.Waybill.WBDate)));
      this.OnPropertyChanged("Documents");
    }

    public Dictionary<EgaisDocument.WaybillStatuses, string> DictionaryWaybillStatuses
    {
      get => EgaisDocument.DictionaryWaybillStatuses;
    }

    public Dictionary<EgaisHelper.StatusEgaisTTN, string> DictionaryStatusEgaisTtn
    {
      get => EgaisDocument.DictionaryStatusEgaisTTN;
    }

    private List<EgaisDocument> CacheDocuments { get; set; }

    public ObservableCollection<EgaisDocument> Documents { get; set; }

    public void ShowEgaisTtn()
    {
      if (!Other.IsActiveAndShowForm<FrmEgaisTtnList>())
        return;
      (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.ShowEgaisWaybill);
      if (!access.Result)
        return;
      this._user = access.User;
      this.GetEgaisWaybill();
      this.FormToSHow = (WindowWithSize) new FrmEgaisTtnList();
      this.ShowMenu = new Action(((FrmEgaisTtnList) this.FormToSHow).ShowMoreMenu);
      this.ShowForm(false);
    }

    public ICommand CreateWaybillCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<EgaisDocument> list = ((IEnumerable) obj).Cast<EgaisDocument>().ToList<EgaisDocument>();
          if (list.Count != 1)
          {
            MessageBoxHelper.Warning(Translate.EgaisTtnListViewModel_CreateWaybillCommand_Требуется_выбрать_одну_накладную_для_создания_нового_поступления_);
          }
          else
          {
            EgaisDocument document = list.Single<EgaisDocument>();
            if (document.StatusTtn != EgaisHelper.StatusEgaisTTN.New)
              MessageBoxHelper.Warning(Translate.EgaisTtnListViewModel_CreateWaybillCommand_Невозможно_повторно_принять_данную_накладную__так_как_ранее_уже_был_отправлен_акт__Требуется_выбрать_другую_накладную_или_сбросить_статус_по_этой_накладной_);
            else
              new FrmWaybillCard().ShowCardWaybillForEgais(document, this._user, new Action(this.GetEgaisWaybill));
          }
        }));
      }
    }

    public ICommand ResetStatusCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<EgaisDocument> list = ((IEnumerable) obj).Cast<EgaisDocument>().ToList<EgaisDocument>();
          if (list.Count != 1)
          {
            MessageBoxHelper.Warning(Translate.EgaisTtnListViewModel_ResetStatusCommand_Необходимо_выбрать_одну_накладную_для_сброса_статуса_);
          }
          else
          {
            EgaisDocument ttnEgais = list.Single<EgaisDocument>();
            if (ttnEgais.StatusTtn == EgaisHelper.StatusEgaisTTN.New)
              MessageBoxHelper.Warning(Translate.EgaisTtnListViewModel_ResetStatusCommand_Сбрасывать_статус_по_данной_накладной_не_требуется__она_не_была_принята_ранее_);
            else if (ttnEgais.WaybillDocument == null)
            {
              MessageBoxHelper.Warning(Translate.EgaisTtnListViewModel_ResetStatusCommand_У_данной_накладной_нельзя_сбросить_статус__обратитесь_в_службу_технической_поддержки_);
            }
            else
            {
              if (MessageBoxHelper.Show(string.Format(Translate.EgaisTtnListViewModel_ResetStatusCommand_Вы_уверены__что_хотите_сбросить_статус_накладной__0___При_сбросе_статуса_будет_удалена_накладная_на_поступление__которая_была_создана_из_этого_документа_, (object) ttnEgais.Form2.WBRegId), buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;
              using (DataBase dataBase = Data.GetDataBase())
              {
                dataBase.GetTable<ENTITY_PROPERTIES_VALUES>().Delete<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.ENTITY_UID == ttnEgais.WaybillDocument.Uid));
                Document document = ttnEgais.WaybillDocument.Clone<Document>();
                document.Properties.Clear();
                document.IsDeleted = true;
                new DocumentsRepository(dataBase).Save(document);
                this.GetEgaisWaybill();
              }
            }
          }
        }));
      }
    }

    private void GetEgaisWaybill()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.EgaisTtnListViewModel_GetEgaisWaybill_Загрузка_накладных_ЕГАИС);
      this.CacheDocuments = new List<EgaisDocument>(new EgaisRepository().GetListDocuments().Where<EgaisDocument>((Func<EgaisDocument, bool>) (x => x.Waybill.Version >= 4)));
      this.Search();
      progressBar.Close();
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public EgaisTtnListViewModel()
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      // ISSUE: reference to a compiler-generated field
      this.\u003CValueDateTimeStart\u003Ek__BackingField = dateTime.AddYears(-1);
      // ISSUE: reference to a compiler-generated field
      this.\u003CValueDateTimeEnd\u003Ek__BackingField = DateTime.Now.Date;
      this._selectedWaybillStatuses = EgaisDocument.WaybillStatuses.New;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCacheDocuments\u003Ek__BackingField = new List<EgaisDocument>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CDocuments\u003Ek__BackingField = new ObservableCollection<EgaisDocument>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
