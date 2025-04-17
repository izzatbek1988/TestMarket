// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.LoginUsersViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Emails;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Users
{
  public partial class LoginUsersViewModel : ViewModelWithForm
  {
    private LoginUsersViewModel.UsersLogin _user;

    public static string AlsoMenuKey => nameof (AlsoMenuKey);

    public static object IconEnter { get; set; }

    public static object IconExit { get; set; }

    public Visibility VisibilityClose { get; set; }

    public List<LoginUsersViewModel.UsersLogin> UserList { get; set; }

    public LoginUsersViewModel.UsersLogin SelectedUser
    {
      get => this._user;
      set
      {
        this._user = value;
        this.OnPropertyChanged(nameof (SelectedUser));
        this.OnPropertyChanged("IconButton");
      }
    }

    public ICommand ActionUser { get; set; }

    public ICommand CloseCommand { get; set; }

    public ICommand DoneCommand { get; set; }

    public ICommand PressEnter { get; set; }

    public Action Close { get; set; }

    public LoginUsersViewModel()
    {
    }

    public LoginUsersViewModel(bool visBool)
    {
      this.UserList = LoginUsersViewModel.GetListUser().OrderBy<LoginUsersViewModel.UsersLogin, DateTime>((Func<LoginUsersViewModel.UsersLogin, DateTime>) (x => x.User.DateIn)).ToList<LoginUsersViewModel.UsersLogin>();
      if (!this.UserList.Any<LoginUsersViewModel.UsersLogin>())
      {
        MessageBoxHelper.Warning(Translate.LoginUsersViewModel_В_базе_нет_пользователей__добавьте_);
        using (DataBase dataBase = Data.GetDataBase())
        {
          GroupRepository groupRepository = new GroupRepository(dataBase);
          if (!groupRepository.GetActiveItems().Any<Gbs.Core.Entities.Clients.Group>())
            groupRepository.CreateDefaultClientGroup();
          if (!UserGroups.GetListUserGroups().Any<UserGroups.UserGroup>((Func<UserGroups.UserGroup, bool>) (x => !x.IsDeleted)))
            UserGroups.AddDefaultUserGroup();
          new FrmUserList().ShowDialog();
          this.UserList = LoginUsersViewModel.GetListUser().ToList<LoginUsersViewModel.UsersLogin>();
        }
      }
      this.VisibilityClose = visBool ? Visibility.Visible : Visibility.Collapsed;
      this.PressEnter = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        Guid? onlineOnSectionUid = this.SelectedUser?.User.OnlineOnSectionUid;
        Guid uid = Sections.GetCurrentSection().Uid;
        if ((onlineOnSectionUid.HasValue ? (onlineOnSectionUid.GetValueOrDefault() == uid ? 1 : 0) : 0) != 0)
          this.Close();
        else
          this.ActionUsers();
      }));
      this.ActionUser = (ICommand) new RelayCommand((Action<object>) (obj => this.ActionUsers()));
      this.DoneCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.DoneLogin()));
      this.CloseCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (MessageBoxHelper.Question(Translate.LoginUsersViewModel_Закрыть_программу_) != MessageBoxResult.Yes)
          return;
        using (DataBase dataBase = Data.GetDataBase())
          new UsersRepository(dataBase).DisconnectUsers();
        Other.SetCorrectExit();
        System.Environment.Exit(0);
      }));
    }

    private void ActionUsers()
    {
      if (this.SelectedUser == null)
      {
        MessageBoxHelper.Warning(Translate.LoginUsersViewModel_Выберите_пользователя);
      }
      else
      {
        if (this.SelectedUser.User.OnlineOnSectionUid != Guid.Empty)
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            if (this.SelectedUser.User.OnlineOnSectionUid != Sections.GetCurrentSection().Uid)
            {
              if (MessageBoxHelper.Question(Translate.LoginUsersViewModel_Выполнен_вход_на_другом_устройстве__Вы_уверены__что_хотите_выйти_) == MessageBoxResult.No)
                return;
              Gbs.Core.Entities.Users.User user = this.SelectedUser.User;
              if (new Authorization().LoginUser(ref user) && user.Uid == this.SelectedUser.User.Uid)
              {
                this.SelectedUser.User.OnlineOnSectionUid = Guid.Empty;
                new UsersRepository(dataBase).Save(this.SelectedUser.User);
                new UserHistoryRepository(dataBase).AddHistoryInputUser(this.SelectedUser.User);
              }
              else
              {
                MessageBoxHelper.Warning(Translate.LoginUsersViewModel_Вход_выполнен_через_другого_сотрудника);
                return;
              }
            }
            this.SelectedUser.Color = "Red";
            this.SelectedUser.User.OnlineOnSectionUid = Guid.Empty;
            new UsersRepository(dataBase).Save(this.SelectedUser.User);
          }
        }
        else
        {
          Gbs.Core.Entities.Users.User users = this.SelectedUser.User;
          if (new Authorization().LoginUser(ref users))
          {
            this.UserList[this.UserList.ToList<LoginUsersViewModel.UsersLogin>().FindIndex((Predicate<LoginUsersViewModel.UsersLogin>) (x => x.User.Uid == users.Uid))].Color = "Green";
            this.UserList[this.UserList.ToList<LoginUsersViewModel.UsersLogin>().FindIndex((Predicate<LoginUsersViewModel.UsersLogin>) (x => x.User.Uid == users.Uid))].User = users;
            users.OnlineOnSectionUid = Sections.GetCurrentSection().Uid;
            using (DataBase dataBase = Data.GetDataBase())
            {
              new UsersRepository(dataBase).Save(users);
              new UserHistoryRepository(dataBase).AddHistoryInputUser(users);
              if (this.UserList.Count == 1)
                this.DoneLogin();
            }
          }
        }
        this.OnPropertyChanged("IconButton");
      }
    }

    private void DoneLogin()
    {
      if (this.UserList.Any<LoginUsersViewModel.UsersLogin>((Func<LoginUsersViewModel.UsersLogin, bool>) (x => x.User.OnlineOnSectionUid == Sections.GetCurrentSection().Uid)))
        this.Close();
      else
        MessageBoxHelper.Warning(Translate.LoginUsersViewModel_Необходимо_авторизоваться_хотя_бы_одному_пользователю);
    }

    private static IEnumerable<LoginUsersViewModel.UsersLogin> GetListUser()
    {
      using (DataBase db = Data.GetDataBase())
      {
        Guid currentSectionUid = Sections.GetCurrentSection().Uid;
        foreach (Gbs.Core.Entities.Users.User user in new UsersRepository(db).GetActiveItems().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsDeleted && !x.IsKicked)))
        {
          if (user.OnlineOnSectionUid == Guid.Empty)
            yield return new LoginUsersViewModel.UsersLogin()
            {
              User = user,
              Color = "Red"
            };
          else if (user.OnlineOnSectionUid == currentSectionUid)
            yield return new LoginUsersViewModel.UsersLogin()
            {
              User = user,
              Color = "Green"
            };
          else
            yield return new LoginUsersViewModel.UsersLogin()
            {
              User = user,
              Color = "Orange"
            };
        }
      }
    }

    public ICommand RestorePasswordForUserCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedUser == null)
            MessageBoxHelper.Warning(Translate.LoginUsersViewModel_Выберите_пользователя__чей_пароль_Вы_хотите_восстановить_);
          else if (this.SelectedUser.User.Client.Email.IsNullOrEmpty())
            MessageBoxHelper.Warning(Translate.LoginUsersViewModel_Не_указана_почта_в_карточке_сотрудника__отправить_пароль_невозможно___Для_восстановления_пароля_обратитесь_в_техническую_поддержку_);
          else if (!Other.IsValidateEmail(this.SelectedUser.User.Client.Email))
            MessageBoxHelper.Warning(Translate.LoginUsersViewModel_Указанная_почта_является_некорректной__отправить_пароль_невозможно___Для_восстановления_пароля_обратитесь_в_техническую_поддержку_);
          else if (!NetworkHelper.IsWorkInternet())
          {
            MessageBoxHelper.Warning(Translate.LoginUsersViewModel_Нет_соединения_с_интернетом__Для_восстановления_пароля_обратитесь_в_техническую_поддержку_);
          }
          else
          {
            if (MessageBoxHelper.Question(string.Format(Translate.LoginUsersViewModel_RestorePasswordForUserCommand_, (object) this.SelectedUser.User.Alias, (object) this.GetSecretEmail(this.SelectedUser.User.Client.Email))) == MessageBoxResult.No)
              return;
            string str = BarcodeHelper.RandomPass();
            if (SmtpHelper.Send(new Email()
            {
              AddressTo = this.SelectedUser.User.Client.Email,
              Subject = string.Format(Translate.LoginUsersViewModel_RestorePasswordForUserCommand_Восстановление_пароля_для__0_, (object) PartnersHelper.ProgramName()),
              Body = (object) string.Format(Translate.LoginUsersViewModel_, (object) this.SelectedUser.User.Alias, (object) str, UidDb.GetUid().Value)
            }))
            {
              using (DataBase dataBase = Data.GetDataBase())
              {
                this.SelectedUser.User.Password = str;
                new UsersRepository(dataBase).Save(this.SelectedUser.User);
                ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateTextHistory(Translate.LoginUsersViewModel_RestorePasswordForUserCommand_Отправлен_пароль_на_почту_для_восстановления_доступа, GlobalDictionaries.EntityTypes.User, this.SelectedUser.User), false);
                int num = (int) MessageBoxHelper.Show(string.Format(Translate.LoginUsersViewModel_Пароль_был_отправлен_на_почту__0___Если_письмо_не_поступило__обратитесь_в_службу_технической_поддержки_, (object) this.GetSecretEmail(this.SelectedUser.User.Client.Email)));
              }
              CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllUsers);
            }
            else
            {
              int num1 = (int) MessageBoxHelper.Show(Translate.LoginUsersViewModel_При_отправке_пароля_возникла_ошибка__Для_восстановления_пароля_обратитесь_в_техническую_поддержку_);
            }
          }
        }));
      }
    }

    public object IconButton
    {
      get
      {
        return this.SelectedUser != null && !(this.SelectedUser.User.OnlineOnSectionUid == Guid.Empty) ? LoginUsersViewModel.IconExit : LoginUsersViewModel.IconEnter;
      }
    }

    private string GetSecretEmail(string email)
    {
      string str = email.Remove(0, email.IndexOf('@'));
      string source = email.Remove(email.Length - str.Length);
      return (source.Length < 4 ? source.First<char>().ToString() : new string(source.Take<char>(3).ToArray<char>())) + new string('*', 5) + source.Last<char>().ToString() + str;
    }

    public ICommand ShowMenuCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.ShowMenu()));
    }

    public Action ShowMenu { get; set; }

    public class UsersLogin : ViewModelWithForm
    {
      private string _color = "Red";

      public Gbs.Core.Entities.Users.User User { get; set; }

      public string Color
      {
        get => this._color;
        set
        {
          this._color = value;
          this.OnPropertyChanged(nameof (Color));
        }
      }
    }
  }
}
