// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.UsersGroup.ListUserGroupsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
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
namespace Gbs.Forms.Users.UsersGroup
{
  public partial class ListUserGroupsViewModel : ViewModelWithForm
  {
    public ObservableCollection<ListUserGroupsViewModel.UserGroupView> Groups { get; set; } = new ObservableCollection<ListUserGroupsViewModel.UserGroupView>();

    public ListUserGroupsViewModel.UserGroupView SelectedGroup { get; set; }

    public ListUserGroupsViewModel()
    {
      try
      {
        List<Actions> allActions = this.GetListAllActions();
        this.Groups = new ObservableCollection<ListUserGroupsViewModel.UserGroupView>(UserGroups.GetListUserGroups().Where<UserGroups.UserGroup>((Func<UserGroups.UserGroup, bool>) (x => !x.IsDeleted)).Select<UserGroups.UserGroup, ListUserGroupsViewModel.UserGroupView>((Func<UserGroups.UserGroup, ListUserGroupsViewModel.UserGroupView>) (x => new ListUserGroupsViewModel.UserGroupView(x, allActions))));
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddGroup()));
        this.EditCommand = (ICommand) new RelayCommand(new Action<object>(this.EditGroup));
        this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteGroup));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме списка групп сотрудников");
      }
    }

    public List<Actions> GetListAllActions()
    {
      List<Actions> list = Enum.GetValues(typeof (Actions)).Cast<Actions>().ToList<Actions>();
      Integrations integrations = new ConfigsRepository<Integrations>().Get();
      if (!integrations.Egais.IsActive)
      {
        list.Remove(Actions.ShowEgaisWaybill);
        list.Remove(Actions.AcceptEgaisWaybill);
        list.Remove(Actions.ActionsToBeerTap);
      }
      if (integrations.Sms.Type == GlobalDictionaries.SmsServiceType.None)
      {
        list.Remove(Actions.DoSaleCreditIfOffSmsCode);
        list.Remove(Actions.DoUseBonusesIfOffSmsCode);
      }
      list.Remove(Actions.WaybillReturnAdd);
      list.Remove(Actions.WaybillReturnDelete);
      list.Remove(Actions.WaybillReturnEdit);
      list.Remove(Actions.WaybillReturnListShow);
      return list;
    }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ICommand CopyCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedGroup != null)
          {
            List<ListUserGroupsViewModel.UserGroupView> list = ((IEnumerable) obj).Cast<ListUserGroupsViewModel.UserGroupView>().ToList<ListUserGroupsViewModel.UserGroupView>();
            if (list.Count > 1)
            {
              MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
            }
            else
            {
              ListUserGroupsViewModel.UserGroupView userGroupView = list.Single<ListUserGroupsViewModel.UserGroupView>().Clone<ListUserGroupsViewModel.UserGroupView>();
              userGroupView.Group.Uid = Guid.NewGuid();
              UserGroups.UserGroup group = userGroupView.Group;
              group.Name = group.Name + " " + Translate.ListUserGroupsViewModel_CopyCommand__копия_;
              if (userGroupView.Group.Save())
                this.Groups.Add(userGroupView);
              else
                MessageBoxHelper.Error(Translate.ListUserGroupsViewModel_CopyCommand_Не_удалось_сохранить_группу_в_базу_данных_при_копировании);
            }
          }
          else
          {
            int num = (int) MessageBoxHelper.Show(Translate.ClientGroupsModelView_Требуется_выбрать_группу, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
        }));
      }
    }

    private void DeleteGroup(object obj)
    {
      if (this.SelectedGroup != null)
      {
        List<ListUserGroupsViewModel.UserGroupView> list = ((IEnumerable) obj).Cast<ListUserGroupsViewModel.UserGroupView>().ToList<ListUserGroupsViewModel.UserGroupView>();
        if (MessageBoxHelper.Show(string.Format(Translate.ListUserGroupsViewModel_Вы_уверены__что_хотите_удалить__0__групп_сотрудников_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
          return;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Users.User> activeItems = new UsersRepository(dataBase).GetActiveItems();
          int num = 0;
          foreach (ListUserGroupsViewModel.UserGroupView userGroupView in list)
          {
            ListUserGroupsViewModel.UserGroupView group = userGroupView;
            if (activeItems.Any<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Group.Uid == group.Group.Uid)))
            {
              ++num;
            }
            else
            {
              group.Group.IsDeleted = true;
              if (!group.Group.Save())
                ++num;
              else
                this.Groups.Remove(group);
            }
          }
          if (num <= 0)
            return;
          MessageBoxHelper.Error(string.Format(Translate.ListUserGroupsViewModel_DeleteGroup_Не_удалось_удалить__0__групп__возможно__что_в_них_есть_сотрудники__Сначала_необходимо_перенести_сотрудников_в_другую_группу_или_удалить_их_, (object) num));
        }
      }
      else
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.ClientGroupsModelView_Требуется_выбрать_группу, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    private void EditGroup(object obj)
    {
      if (this.SelectedGroup != null)
      {
        if (((IEnumerable) obj).Cast<ListUserGroupsViewModel.UserGroupView>().ToList<ListUserGroupsViewModel.UserGroupView>().Count > 1)
        {
          MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
        }
        else
        {
          UserGroups.UserGroup group;
          if (!new FrmCardUserGroup().ShowCard(this.SelectedGroup.Group.Uid, out group))
            return;
          this.Groups[this.Groups.ToList<ListUserGroupsViewModel.UserGroupView>().FindIndex((Predicate<ListUserGroupsViewModel.UserGroupView>) (x => x.Group.Uid == group.Uid))] = new ListUserGroupsViewModel.UserGroupView(group, this.GetListAllActions());
        }
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.ClientGroupsModelView_Требуется_выбрать_группу, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    private void AddGroup()
    {
      UserGroups.UserGroup group;
      if (!new FrmCardUserGroup().ShowCard(Guid.Empty, out group))
        return;
      this.Groups.Add(new ListUserGroupsViewModel.UserGroupView(group, this.GetListAllActions()));
    }

    public class UserGroupView
    {
      public UserGroupView(UserGroups.UserGroup group, List<Actions> allActions)
      {
        this.Group = group;
        if (group.IsSuper)
          this.CountPermissionsString = Translate.FrmCardUserGroup_НеограниченныйДоступ;
        else
          this.CountPermissionsString = string.Format("{0}/{1}", (object) group.Permissions.Count<PermissionRules.PermissionRule>((Func<PermissionRules.PermissionRule, bool>) (x => x.IsGranted && allActions.Any<Actions>((Func<Actions, bool>) (p => p == x.Action)))), (object) allActions.Count);
      }

      public UserGroups.UserGroup Group { get; set; }

      public string CountPermissionsString { get; set; }
    }
  }
}
