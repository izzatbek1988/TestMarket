// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.ClientGroupsModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms._shared;
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
namespace Gbs.Forms.Clients
{
  public partial class ClientGroupsModelView : ViewModelWithForm
  {
    private string _filter = string.Empty;

    public string Filter
    {
      get => this._filter;
      set
      {
        this._filter = value;
        this.Groups = value.IsNullOrEmpty() ? new ObservableCollection<Gbs.Core.Entities.Clients.Group>(this.CacheGroups) : new ObservableCollection<Gbs.Core.Entities.Clients.Group>(this.CacheGroups.Where<Gbs.Core.Entities.Clients.Group>((Func<Gbs.Core.Entities.Clients.Group, bool>) (x => x.Name.ToLower().Contains(value.ToLower()))));
        this.OnPropertyChanged("Groups");
      }
    }

    private List<Gbs.Core.Entities.Clients.Group> CacheGroups { get; set; }

    public ObservableCollection<Gbs.Core.Entities.Clients.Group> Groups { get; set; } = new ObservableCollection<Gbs.Core.Entities.Clients.Group>();

    public Gbs.Core.Entities.Clients.Group SelectedGroup { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ClientGroupsModelView()
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          this.CacheGroups = new GroupRepository(dataBase).GetActiveItems().OrderBy<Gbs.Core.Entities.Clients.Group, string>((Func<Gbs.Core.Entities.Clients.Group, string>) (x => x.Name)).ToList<Gbs.Core.Entities.Clients.Group>();
          this.Groups = new ObservableCollection<Gbs.Core.Entities.Clients.Group>(this.CacheGroups);
          this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddGroup()));
          this.EditCommand = (ICommand) new RelayCommand(new Action<object>(this.EditGroup));
          this.DeleteCommand = (ICommand) new RelayCommand(new Action<object>(this.DeleteGroup));
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "");
      }
    }

    private void DeleteGroup(object obj)
    {
      if (this.SelectedGroup != null)
      {
        (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.DeleteClientGroup);
        if (!access.Result)
          return;
        List<Gbs.Core.Entities.Clients.Group> list = ((IEnumerable) obj).Cast<Gbs.Core.Entities.Clients.Group>().ToList<Gbs.Core.Entities.Clients.Group>();
        if (MessageBoxHelper.Show(string.Format(Translate.ClientGroupsModelView_Вы_уверены__что_хотите_удалить__0__группы_покупателей_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
          return;
        foreach (Gbs.Core.Entities.Clients.Group source in list)
        {
          using (DataBase dataBase = Data.GetDataBase())
          {
            GroupRepository groupRepository = new GroupRepository(dataBase);
            if (groupRepository.IsExistClientInGroup(source))
            {
              MessageBoxHelper.Warning(string.Format(Translate.ClientGroupsModelView_Невозможно_удалить_группу__0___так_как_в_ней_есть_покупатели_, (object) source.Name));
            }
            else
            {
              Gbs.Core.Entities.Clients.Group oldItem = source.Clone<Gbs.Core.Entities.Clients.Group>();
              source.IsDeleted = true;
              groupRepository.Save(source);
              this.Groups.Remove(source);
              Gbs.Core.Entities.Clients.Group newItem = source;
              Users.User user = access.User;
              ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) oldItem, (IEntity) newItem, ActionType.Delete, GlobalDictionaries.EntityTypes.ClientGroup, user), false);
            }
          }
        }
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.ClientGroupsModelView_Требуется_выбрать_группу, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    private void EditGroup(object obj)
    {
      if (this.SelectedGroup != null)
      {
        if (((IEnumerable) obj).Cast<Gbs.Core.Entities.Clients.Group>().ToList<Gbs.Core.Entities.Clients.Group>().Count > 1)
        {
          MessageBoxHelper.Warning(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
        }
        else
        {
          Gbs.Core.Entities.Clients.Group group;
          if (!new FrmClientGroupsCard().ShowCard(this.SelectedGroup.Uid, out group))
            return;
          this.Groups[this.Groups.ToList<Gbs.Core.Entities.Clients.Group>().FindIndex((Predicate<Gbs.Core.Entities.Clients.Group>) (x => x.Uid == group.Uid))] = group;
        }
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.ClientGroupsModelView_Требуется_выбрать_группу, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    private void AddGroup()
    {
      Gbs.Core.Entities.Clients.Group group;
      if (!new FrmClientGroupsCard().ShowCard(Guid.Empty, out group))
        return;
      this.Groups.Add(group);
    }
  }
}
