// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroups.GroupsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
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
namespace Gbs.Forms.GoodGroups
{
  public partial class GroupsViewModel : ViewModelWithForm
  {
    private string _filterText;
    private ObservableCollection<GroupsViewModel.GroupWithChilds> _groupList;

    public int TotalGroup
    {
      get
      {
        return !this.FilterText.IsNullOrEmpty() ? this.GroupList.Count<GroupsViewModel.GroupWithChilds>() : this.CachedDbGroups.Count;
      }
    }

    public ICommand PrintListGood
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.GroupList.Any<GroupsViewModel.GroupWithChilds>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.GroupsViewModel_В_списке_нет_категорий_);
          }
          else
            new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateGoodGroups(this.FilterText.IsNullOrEmpty() ? this.CachedDbGroups : this.GroupList.Select<GroupsViewModel.GroupWithChilds, Gbs.Core.Entities.GoodGroups.Group>((Func<GroupsViewModel.GroupWithChilds, Gbs.Core.Entities.GoodGroups.Group>) (x => x.Group)).ToList<Gbs.Core.Entities.GoodGroups.Group>()), (Gbs.Core.Entities.Users.User) null);
        }));
      }
    }

    public ObservableCollection<GroupsViewModel.GroupWithChilds> GroupList
    {
      get => this._groupList;
      set
      {
        this._groupList = value;
        this.OnPropertyChanged(nameof (GroupList));
      }
    }

    public GroupsViewModel.GroupWithChilds SelectedGroup { get; set; }

    public List<Gbs.Core.Entities.GoodGroups.Group> ExpanderList { get; set; } = new List<Gbs.Core.Entities.GoodGroups.Group>();

    public string FilterText
    {
      get => this._filterText;
      set
      {
        this._filterText = value;
        this.OnPropertyChanged(nameof (FilterText));
        this.LoadDataFromDb();
        this.OnPropertyChanged("TotalGroup");
      }
    }

    public static Visibility VisibleCheckBox { get; set; }

    private List<Gbs.Core.Entities.GoodGroups.Group> CachedDbGroups { get; set; }

    public GroupsViewModel()
    {
      try
      {
        this.LoadDataFromDb();
        this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddGroup()));
        this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.EditGroup()));
        this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.DeleteGroup()));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки формы для выбора категорий товаров");
      }
    }

    private void LoadDataFromDb()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.CachedDbGroups = new GoodGroupsRepository(dataBase).GetActiveItems();
        this.GroupList = new ObservableCollection<GroupsViewModel.GroupWithChilds>(this.GetGroupsTree());
        this.OnPropertyChanged("TotalGroup");
      }
    }

    private List<GroupsViewModel.GroupWithChilds> GetGroupsTree()
    {
      if (!this.FilterText.IsNullOrEmpty())
        return this.CachedDbGroups.Where<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Name.ToLower().Contains(this.FilterText.ToLower()))).Select<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>((Func<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>) (x => new GroupsViewModel.GroupWithChilds()
        {
          Group = x
        })).OrderBy<GroupsViewModel.GroupWithChilds, string>((Func<GroupsViewModel.GroupWithChilds, string>) (x => x.Group.Name)).ToList<GroupsViewModel.GroupWithChilds>();
      List<GroupsViewModel.GroupWithChilds> groupsTree = new List<GroupsViewModel.GroupWithChilds>();
      foreach (GroupsViewModel.GroupWithChilds goodGroup in this.CachedDbGroups.Where<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (g => g.ParentGroupUid == Guid.Empty && !g.IsDeleted)).ToList<Gbs.Core.Entities.GoodGroups.Group>().OrderBy<Gbs.Core.Entities.GoodGroups.Group, string>((Func<Gbs.Core.Entities.GoodGroups.Group, string>) (x => x.Name)).Select<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>((Func<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>) (group => new GroupsViewModel.GroupWithChilds()
      {
        Group = group,
        IsExpanded = this.ExpanderList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == group.Uid))
      })))
      {
        this.AddChildrens(goodGroup);
        groupsTree.Add(goodGroup);
      }
      return groupsTree;
    }

    private void AddChildrens(GroupsViewModel.GroupWithChilds goodGroup)
    {
      foreach (GroupsViewModel.GroupWithChilds goodGroup1 in this.CachedDbGroups.Where<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (c => c.ParentGroupUid == goodGroup.Group.Uid)).OrderBy<Gbs.Core.Entities.GoodGroups.Group, string>((Func<Gbs.Core.Entities.GoodGroups.Group, string>) (x => x.Name)).Select<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>((Func<Gbs.Core.Entities.GoodGroups.Group, GroupsViewModel.GroupWithChilds>) (group => new GroupsViewModel.GroupWithChilds()
      {
        Group = group,
        IsExpanded = this.ExpanderList.Any<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid == group.Uid))
      })))
      {
        this.AddChildrens(goodGroup1);
        goodGroup.Childrens.Add(goodGroup1);
      }
    }

    public ICommand AddCommand { get; set; }

    private void AddGroup()
    {
      Gbs.Core.Entities.GoodGroups.Group group;
      if (!new FrmGoodGroupCard().ShowGroupCard(Guid.Empty, out group))
        return;
      this.CachedDbGroups.Add(group);
      this.LoadDataFromDb();
    }

    public ICommand EditCommand { get; set; }

    private void EditGroup()
    {
      if (this.SelectedGroup != null)
      {
        Gbs.Core.Entities.GoodGroups.Group group;
        if (!new FrmGoodGroupCard().ShowGroupCard(this.SelectedGroup.Group.Uid, out group))
          return;
        this.CachedDbGroups[this.CachedDbGroups.ToList<Gbs.Core.Entities.GoodGroups.Group>().FindIndex((Predicate<Gbs.Core.Entities.GoodGroups.Group>) (x => x.Uid == group.Uid))] = group;
        this.LoadDataFromDb();
      }
      else
      {
        int num = (int) MessageBoxHelper.Show(Translate.FrmGoodsGroupsEdit_NeedToSelectGroup, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    public ICommand DeleteCommand { get; set; }

    private void DeleteGroup()
    {
      if (this.SelectedGroup != null)
      {
        (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.DeleteGoodGroup);
        if (!access.Result || MessageBoxHelper.Show(Translate.FrmGoodsGroupsDelete, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
          return;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Goods.Good> byQuery = new GoodRepository(dataBase).GetByQuery(dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.GROUP_UID == this.SelectedGroup.Group.Uid && !x.IS_DELETED)));
          if (byQuery.Any<Gbs.Core.Entities.Goods.Good>())
          {
            switch (MessageBoxHelper.Show(Translate.GroupsViewModel_Выбранная_категория_будет_удалена__Хотите_перенести_имеющиеся_в_ней_товары_в_другую_категорию_, PartnersHelper.ProgramName(), MessageBoxButton.OKCancel, MessageBoxImage.Question))
            {
              case MessageBoxResult.OK:
                Gbs.Core.Entities.GoodGroups.Group gr;
                if (!new FormSelectGroup().GetSingleSelectedGroupUid(access.User, out gr))
                {
                  int num = (int) MessageBoxHelper.Show(Translate.GroupsViewModel_Категория_не_удалена__так_как_вы_не_выбрали_категорию_для_замены_, icon: MessageBoxImage.Exclamation);
                  return;
                }
                if (gr.Uid == this.SelectedGroup.Group.Uid)
                {
                  int num = (int) MessageBoxHelper.Show(Translate.GroupsViewModel_Выберите_другую_категорию_для_переноса__так_как_эту_вы_хотите_удалить_, icon: MessageBoxImage.Exclamation);
                  return;
                }
                if (gr.GoodsType != this.SelectedGroup.Group.GoodsType)
                {
                  if (gr.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Single, GlobalDictionaries.GoodTypes.Weight))
                  {
                    if (this.SelectedGroup.Group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Weight, GlobalDictionaries.GoodTypes.Single))
                      goto label_13;
                  }
                  int num = (int) MessageBoxHelper.Show(Translate.GoodCardModelView_Невозможно_сменить_категорию__так_как_разные_типы);
                  return;
                }
label_13:
                byQuery.ForEach((Action<Gbs.Core.Entities.Goods.Good>) (x =>
                {
                  Gbs.Core.Entities.Goods.Good good = x;
                  good.Group = new Gbs.Core.Entities.GoodGroups.Group()
                  {
                    Uid = gr.Uid
                  };
                }));
                new GoodRepository(dataBase).Save(byQuery, false);
                if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
                {
                  new HomeOfficeHelper().PrepareAndSend<List<Gbs.Core.Entities.Goods.Good>>(byQuery, HomeOfficeHelper.EntityEditHome.GoodList);
                  break;
                }
                break;
              case MessageBoxResult.Cancel:
                return;
            }
          }
          GroupsViewModel.GroupWithChilds groupWithChilds = this.SelectedGroup.Clone<GroupsViewModel.GroupWithChilds>();
          this.SelectedGroup.Group.IsDeleted = true;
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          groupsRepository.Save(this.SelectedGroup.Group);
          foreach (GroupsViewModel.GroupWithChilds children in this.SelectedGroup.Childrens)
          {
            children.Group.ParentGroupUid = Guid.Empty;
            groupsRepository.Save(children.Group);
          }
          ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) groupWithChilds.Group, (IEntity) this.SelectedGroup.Group, ActionType.Delete, GlobalDictionaries.EntityTypes.GoodGroup, access.User), false);
          this.CachedDbGroups = this.CachedDbGroups.Where<Gbs.Core.Entities.GoodGroups.Group>((Func<Gbs.Core.Entities.GoodGroups.Group, bool>) (x => x.Uid != this.SelectedGroup.Group.Uid)).ToList<Gbs.Core.Entities.GoodGroups.Group>();
          this.LoadDataFromDb();
        }
      }
      else
      {
        int num1 = (int) MessageBoxHelper.Show(Translate.FrmGoodsGroupsEdit_NeedToSelectGroup, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
      }
    }

    public class GroupWithChilds : ViewModelWithForm
    {
      private bool _isChecked;
      private bool _isExpanded;

      public Gbs.Core.Entities.GoodGroups.Group Group { get; set; } = new Gbs.Core.Entities.GoodGroups.Group();

      public List<GroupsViewModel.GroupWithChilds> Childrens { get; set; } = new List<GroupsViewModel.GroupWithChilds>();

      public bool IsChecked
      {
        get => this._isChecked;
        set
        {
          this._isChecked = value;
          this.OnPropertyChanged(nameof (IsChecked));
        }
      }

      public bool IsExpanded
      {
        get => this._isExpanded;
        set
        {
          this._isExpanded = value;
          this.OnPropertyChanged(nameof (IsExpanded));
        }
      }
    }
  }
}
