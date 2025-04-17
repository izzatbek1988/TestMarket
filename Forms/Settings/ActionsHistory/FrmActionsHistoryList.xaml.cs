// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.ActionsHistory.ActionsHistoryListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers;
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
namespace Gbs.Forms.Settings.ActionsHistory
{
  public partial class ActionsHistoryListViewModel : ViewModelWithForm
  {
    private Guid _sectionUid;
    private Guid _userUid;
    private ActionType _actionType;
    private GlobalDictionaries.EntityTypes _entityType;
    private DateTime _dateFinish;
    private DateTime _dateStart;

    public List<Gbs.Core.Entities.Sections.Section> Sections { get; set; }

    public Guid SectionUid
    {
      get => this._sectionUid;
      set
      {
        this._sectionUid = value;
        this.SearchForFilter();
      }
    }

    public List<Gbs.Core.Entities.Users.User> Users { get; set; }

    public Guid UserUid
    {
      get => this._userUid;
      set
      {
        this._userUid = value;
        this.SearchForFilter();
      }
    }

    public static Dictionary<ActionType, string> DictionaryActionTypes
    {
      get
      {
        return new Dictionary<ActionType, string>()
        {
          {
            ActionType.None,
            Translate.ActionsHistoryListViewModel_Все_действия
          },
          {
            ActionType.Add,
            Translate.ActionsHistoryListViewModel_Добавление
          },
          {
            ActionType.Edit,
            Translate.ActionsHistoryListViewModel_Изменение
          },
          {
            ActionType.JoinGood,
            Translate.ActionsHistoryListViewModel_Объединение
          },
          {
            ActionType.Delete,
            Translate.ActionsHistoryListViewModel_Удаление
          },
          {
            ActionType.Show,
            Translate.ActionsHistoryListViewModel_Просмотр
          },
          {
            ActionType.InputUser,
            Translate.ActionsHistoryListViewModel_Вход_сотрудника
          },
          {
            ActionType.OutputUser,
            Translate.ActionsHistoryListViewModel_Выход_сотрудника
          }
        };
      }
    }

    public static Dictionary<GlobalDictionaries.EntityTypes, string> DictionaryEntityTypes
    {
      get
      {
        return new Dictionary<GlobalDictionaries.EntityTypes, string>()
        {
          {
            GlobalDictionaries.EntityTypes.NotSet,
            Translate.ActionsHistoryListViewModel_Все_записи
          },
          {
            GlobalDictionaries.EntityTypes.Client,
            Translate.Контакт
          },
          {
            GlobalDictionaries.EntityTypes.ClientGroup,
            Translate.FrmClientGroupsCard_ГруппаКонтактов
          },
          {
            GlobalDictionaries.EntityTypes.Document,
            Translate.ActionsHistoryListViewModel_Документ
          },
          {
            GlobalDictionaries.EntityTypes.Good,
            Translate.AnaliticSettingViewModel_Товар
          },
          {
            GlobalDictionaries.EntityTypes.GoodGroup,
            Translate.FrmGoodGroupCard_КатегорияТоваров
          },
          {
            GlobalDictionaries.EntityTypes.Window,
            Translate.ActionsHistoryListViewModel_Окно
          },
          {
            GlobalDictionaries.EntityTypes.Payment,
            Translate.GlobalDictionaries_Платеж
          },
          {
            GlobalDictionaries.EntityTypes.User,
            Translate.FrmUserStatistic_Сотрудник
          },
          {
            GlobalDictionaries.EntityTypes.ItemList,
            Translate.ActionsHistoryListViewModel_Элемент_списка
          }
        };
      }
    }

    public ActionType ActionType
    {
      get => this._actionType;
      set
      {
        this._actionType = value;
        this.SearchForFilter();
      }
    }

    public GlobalDictionaries.EntityTypes EntityType
    {
      get => this._entityType;
      set
      {
        this._entityType = value;
        this.SearchForFilter();
      }
    }

    private List<ActionsHistoryListViewModel.ActionHistoryView> CashDbList { get; set; }

    public ObservableCollection<ActionsHistoryListViewModel.ActionHistoryView> Histories { get; set; }

    public void ShowHistory()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.CashDbList = new List<ActionsHistoryListViewModel.ActionHistoryView>(new ActionHistoryRepository(dataBase).GetActiveItems().Select<ActionHistory, ActionsHistoryListViewModel.ActionHistoryView>((Func<ActionHistory, ActionsHistoryListViewModel.ActionHistoryView>) (x => new ActionsHistoryListViewModel.ActionHistoryView(x))));
        this.Users = this.Users.Concat<Gbs.Core.Entities.Users.User>(new UsersRepository(dataBase).GetByQuery().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsDeleted))).ToList<Gbs.Core.Entities.Users.User>();
        this.Sections.AddRange((IEnumerable<Gbs.Core.Entities.Sections.Section>) Gbs.Core.Entities.Sections.GetSectionsList(dataBase.GetTable<SECTIONS>().Where<SECTIONS>((Expression<Func<SECTIONS, bool>>) (x => !x.IS_DELETED))));
        List<UserHistory> activeItems = new UserHistoryRepository(dataBase).GetActiveItems();
        foreach (UserHistory userHistory in activeItems)
        {
          UserHistory history = userHistory;
          ActionHistory actionHistory1 = new ActionHistory();
          actionHistory1.ActionType = ActionType.InputUser;
          actionHistory1.DateTime = history.DateIn;
          actionHistory1.EntityType = GlobalDictionaries.EntityTypes.User;
          actionHistory1.Section = this.Sections.SingleOrDefault<Gbs.Core.Entities.Sections.Section>((Func<Gbs.Core.Entities.Sections.Section, bool>) (x => x.Uid == history.SectionUid));
          Gbs.Core.Entities.Users.User user1 = history.User;
          if (user1 == null)
          {
            Gbs.Core.Entities.Users.User user2 = new Gbs.Core.Entities.Users.User();
            user2.Uid = Guid.Empty;
            user2.Alias = Translate.ActionsHistoryListViewModel_ShowHistory_Не_определено;
            user1 = user2;
          }
          actionHistory1.User = user1;
          Gbs.Core.Entities.Users.User user3 = history.User;
          // ISSUE: explicit non-virtual call
          actionHistory1.EntityUid = user3 != null ? __nonvirtual (user3.Uid) : Guid.Empty;
          ActionHistory actionHistory2 = actionHistory1;
          this.CashDbList.Add(new ActionsHistoryListViewModel.ActionHistoryView(actionHistory2));
          if (!(activeItems.Where<UserHistory>((Func<UserHistory, bool>) (x =>
          {
            Guid? uid1 = x.User?.Uid;
            Guid? uid2 = history.User?.Uid;
            if (uid1.HasValue != uid2.HasValue)
              return false;
            return !uid1.HasValue || uid1.GetValueOrDefault() == uid2.GetValueOrDefault();
          })).OrderBy<UserHistory, DateTime>((Func<UserHistory, DateTime>) (x => x.DateIn)).Last<UserHistory>().Uid != history.Uid))
          {
            Guid? onlineOnSectionUid = history.User?.OnlineOnSectionUid;
            Guid empty = Guid.Empty;
            if ((onlineOnSectionUid.HasValue ? (onlineOnSectionUid.GetValueOrDefault() == empty ? 1 : 0) : 0) == 0)
              continue;
          }
          ActionHistory action = actionHistory2.Clone<ActionHistory>();
          action.ActionType = ActionType.OutputUser;
          action.DateTime = history.DateOut;
          this.CashDbList.Add(new ActionsHistoryListViewModel.ActionHistoryView(action));
        }
      }
      this.SearchForFilter();
      this.FormToSHow = (WindowWithSize) new FrmActionsHistoryList();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
    }

    public DateTime DateStart
    {
      get => this._dateStart;
      set => this._dateStart = value;
    }

    public DateTime DateFinish
    {
      get => this._dateFinish;
      set => this._dateFinish = value;
    }

    public ICommand JournalFilerCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (o => this.SearchForFilter()));
    }

    private void SearchForFilter()
    {
      IEnumerable<ActionsHistoryListViewModel.ActionHistoryView> source = this.CashDbList.Where<ActionsHistoryListViewModel.ActionHistoryView>((Func<ActionsHistoryListViewModel.ActionHistoryView, bool>) (x =>
      {
        DateTime dateTime1 = x.ActionHistory.DateTime;
        DateTime date1 = dateTime1.Date;
        dateTime1 = this.DateStart;
        DateTime date2 = dateTime1.Date;
        if (!(date1 >= date2))
          return false;
        DateTime dateTime2 = x.ActionHistory.DateTime;
        DateTime date3 = dateTime2.Date;
        dateTime2 = this.DateFinish;
        DateTime date4 = dateTime2.Date;
        return date3 <= date4;
      }));
      if (this.ActionType != ActionType.None)
        source = source.Where<ActionsHistoryListViewModel.ActionHistoryView>((Func<ActionsHistoryListViewModel.ActionHistoryView, bool>) (x => x.ActionHistory.ActionType == this.ActionType));
      if (this.EntityType != GlobalDictionaries.EntityTypes.NotSet)
        source = source.Where<ActionsHistoryListViewModel.ActionHistoryView>((Func<ActionsHistoryListViewModel.ActionHistoryView, bool>) (x => x.ActionHistory.EntityType == this.EntityType));
      if (this.UserUid != Guid.Empty)
        source = source.Where<ActionsHistoryListViewModel.ActionHistoryView>((Func<ActionsHistoryListViewModel.ActionHistoryView, bool>) (x =>
        {
          Guid? uid = x.ActionHistory.User?.Uid;
          Guid userUid = this.UserUid;
          return uid.HasValue && uid.GetValueOrDefault() == userUid;
        }));
      if (this.SectionUid != Guid.Empty)
        source = source.Where<ActionsHistoryListViewModel.ActionHistoryView>((Func<ActionsHistoryListViewModel.ActionHistoryView, bool>) (x =>
        {
          Guid? uid = x.ActionHistory.Section?.Uid;
          Guid sectionUid = this.SectionUid;
          return uid.HasValue && uid.GetValueOrDefault() == sectionUid;
        }));
      this.Histories = new ObservableCollection<ActionsHistoryListViewModel.ActionHistoryView>((IEnumerable<ActionsHistoryListViewModel.ActionHistoryView>) source.OrderByDescending<ActionsHistoryListViewModel.ActionHistoryView, DateTime>((Func<ActionsHistoryListViewModel.ActionHistoryView, DateTime>) (x => x.ActionHistory.DateTime)));
      this.OnPropertyChanged("Histories");
    }

    public ActionsHistoryListViewModel()
    {
      List<Gbs.Core.Entities.Sections.Section> sectionList = new List<Gbs.Core.Entities.Sections.Section>();
      Gbs.Core.Entities.Sections.Section section = new Gbs.Core.Entities.Sections.Section();
      section.Uid = Guid.Empty;
      section.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
      sectionList.Add(section);
      // ISSUE: reference to a compiler-generated field
      this.\u003CSections\u003Ek__BackingField = sectionList;
      this._sectionUid = Guid.Empty;
      List<Gbs.Core.Entities.Users.User> userList = new List<Gbs.Core.Entities.Users.User>();
      Gbs.Core.Entities.Users.User user = new Gbs.Core.Entities.Users.User();
      user.Uid = Guid.Empty;
      user.Alias = Translate.SaleJournalViewModel__selectedUser_Все_сотрудники;
      userList.Add(user);
      // ISSUE: reference to a compiler-generated field
      this.\u003CUsers\u003Ek__BackingField = userList;
      this._userUid = Guid.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CCashDbList\u003Ek__BackingField = new List<ActionsHistoryListViewModel.ActionHistoryView>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CHistories\u003Ek__BackingField = new ObservableCollection<ActionsHistoryListViewModel.ActionHistoryView>();
      this._dateFinish = DateTime.Now;
      this._dateStart = DateTime.Now.AddYears(-1);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public class ActionHistoryView
    {
      public ActionHistory ActionHistory { get; set; }

      public string EntityTypeString
      {
        get
        {
          return ActionsHistoryListViewModel.DictionaryEntityTypes.Single<KeyValuePair<GlobalDictionaries.EntityTypes, string>>((Func<KeyValuePair<GlobalDictionaries.EntityTypes, string>, bool>) (x => x.Key == this.ActionHistory.EntityType)).Value;
        }
      }

      public string ActionTypeString
      {
        get
        {
          return ActionsHistoryListViewModel.DictionaryActionTypes.Single<KeyValuePair<ActionType, string>>((Func<KeyValuePair<ActionType, string>, bool>) (x => x.Key == this.ActionHistory.ActionType)).Value;
        }
      }

      public ActionHistoryView()
      {
      }

      public ActionHistoryView(ActionHistory action) => this.ActionHistory = action;
    }
  }
}
