// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.UserListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Users
{
  public partial class UserListViewModel : ViewModelWithForm
  {
    public ObservableCollection<Gbs.Core.Entities.Users.User> UsersList { get; set; } = new ObservableCollection<Gbs.Core.Entities.Users.User>();

    public Gbs.Core.Entities.Users.User SelectedUser { get; set; }

    public int ActiveUserCount
    {
      get => this.UsersList.Count<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsKicked));
    }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public UserListViewModel()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          this.UsersList = new ObservableCollection<Gbs.Core.Entities.Users.User>((IEnumerable<Gbs.Core.Entities.Users.User>) new UsersRepository(dataBase).GetActiveItems().OrderBy<Gbs.Core.Entities.Users.User, DateTime>((Func<Gbs.Core.Entities.Users.User, DateTime>) (x => x.DateIn)));
          this.OnPropertyChanged(nameof (ActiveUserCount));
          this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddUser()));
          this.EditCommand = (ICommand) new RelayCommand(new Action<object>(this.EditUser));
          this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteUser));
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка сотрудников");
      }
    }

    private void DeleteUser(object obj)
    {
      if (this.SelectedUser != null)
      {
        List<Gbs.Core.Entities.Users.User> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Users.User>().ToList<Gbs.Core.Entities.Users.User>();
        if (MessageBoxHelper.Show(string.Format(Translate.UserListViewModel_Вы_уверены__что_хотите_удалить__0__сотрудников_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
          return;
        foreach (Gbs.Core.Entities.Users.User user in list)
        {
          if (this.UsersList.Count<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsKicked && x.Group.IsSuper)) == 1 && user.Group.IsSuper && !user.IsKicked)
          {
            int num = (int) MessageBoxHelper.Show(string.Format(Translate.UserListViewModel_При_удалении_пользователя__0___не_останется_ни_одного_пользователя_с_правами_СуперАдминистратора_, (object) user.Alias), PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
            return;
          }
          if (user.OnlineOnSectionUid != Guid.Empty && MessageBoxHelper.Show(string.Format(Translate.UserListViewModel_Сотрудник__0__сейчас_находится__Онлайн__в_программе__выйти_и_удалить_пользователя_, (object) user.Alias), buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
            return;
          user.IsDeleted = true;
          user.OnlineOnSectionUid = Guid.Empty;
          using (DataBase dataBase = Data.GetDataBase())
          {
            new UsersRepository(dataBase).Save(user);
            this.UsersList.Remove(user);
          }
        }
      }
      else
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.UserListViewModel_Требуется_выбрать_сотрудника, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
      this.OnPropertyChanged("ActiveUserCount");
    }

    private void EditUser(object obj)
    {
      if (this.SelectedUser != null)
      {
        if (((IEnumerable) obj).Cast<Gbs.Core.Entities.Users.User>().ToList<Gbs.Core.Entities.Users.User>().Count > 1)
        {
          MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          return;
        }
        Gbs.Core.Entities.Users.User user;
        if (new FrmUserCard().ShowCard(this.SelectedUser.Uid, out user))
          this.UsersList[this.UsersList.ToList<Gbs.Core.Entities.Users.User>().FindIndex((Predicate<Gbs.Core.Entities.Users.User>) (x => x.Uid == user.Uid))] = user;
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.UserListViewModel_Требуется_выбрать_сотрудника, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
      this.OnPropertyChanged("ActiveUserCount");
    }

    private void AddUser()
    {
      Gbs.Core.Entities.Users.User user;
      if (new FrmUserCard().ShowCard(Guid.Empty, out user))
        this.UsersList.Add(user);
      this.OnPropertyChanged("ActiveUserCount");
    }
  }
}
