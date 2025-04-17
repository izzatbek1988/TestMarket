// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.UsersGroup.UserGroupViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Users.UsersGroup
{
  public partial class UserGroupViewModel : ViewModelWithForm, ICheckChangesViewModel
  {
    public bool SaveResult { get; set; }

    public List<UserGroupViewModel.ActionGrid> ListAction { get; set; } = new List<UserGroupViewModel.ActionGrid>();

    public UserGroups.UserGroup Group { get; set; }

    public Action CloseForm { get; set; }

    public ICommand GroupSaveCommand { get; set; }

    public UserGroupViewModel()
    {
    }

    public UserGroupViewModel(UserGroups.UserGroup group)
    {
      this.Group = group;
      this.EntityClone = (IEntity) group.Clone<UserGroups.UserGroup>();
      this.GroupSaveCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        foreach (UserGroupViewModel.ActionGrid actionGrid in this.ListAction)
        {
          foreach (UserGroupViewModel.ActionItem actionItem in (Collection<UserGroupViewModel.ActionItem>) actionGrid.ListActionGroup)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            UserGroupViewModel.\u003C\u003Ec__DisplayClass21_0 cDisplayClass210 = new UserGroupViewModel.\u003C\u003Ec__DisplayClass21_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass210.actionItem = actionItem;
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            this.Group.Permissions.Single<PermissionRules.PermissionRule>(new Func<PermissionRules.PermissionRule, bool>(cDisplayClass210.\u003C\u002Ector\u003Eb__1)).IsGranted = cDisplayClass210.actionItem.IsChecked;
          }
        }
        if (!this.Group.IsSuper)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          UserGroupViewModel.\u003C\u003Ec__DisplayClass21_1 cDisplayClass211 = new UserGroupViewModel.\u003C\u003Ec__DisplayClass21_1();
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            IQueryable<USERS_GROUPS> queryable = dataBase.GetTable<USERS_GROUPS>().Where<USERS_GROUPS>((Expression<Func<USERS_GROUPS, bool>>) (x => x.IS_SUPER && x.UID != this.Group.Uid && !x.IS_DELETED));
            // ISSUE: reference to a compiler-generated field
            cDisplayClass211.userGroups = queryable;
            ParameterExpression parameterExpression1;
            ParameterExpression parameterExpression2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method reference
            // ISSUE: method reference
            if (!dataBase.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => !x.IS_DELETED && !x.IS_KICKED && cDisplayClass211.userGroups.Any<USERS_GROUPS>(System.Linq.Expressions.Expression.Lambda<Func<USERS_GROUPS, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal(g.UID, (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (USERS.get_GROUP_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression2)))).Any<USERS>())
            {
              int num = (int) MessageBoxHelper.Show(Translate.UserGroupViewModel_Невозможно_сохранить_данную_группу__так_как_не_останется_не_одного_супер_администратора, icon: MessageBoxImage.Exclamation);
              return;
            }
          }
        }
        this.SaveResult = this.Group.Save();
        if (!this.SaveResult)
          return;
        WindowWithSize.IsCancel = false;
        this.CloseForm();
      }));
      List<UserGroupViewModel.ActionGrid> collection = new List<UserGroupViewModel.ActionGrid>()
      {
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.UserGroupViewModel_UserGroupViewModel_Товары,
          Actions = new List<Actions>()
          {
            Actions.GoodsCreate,
            Actions.GoodsCatalogShow,
            Actions.GoodsDelete,
            Actions.GoodsEdit,
            Actions.GoodsJoin,
            Actions.GroupEditingGoodAndCategories,
            Actions.ShowBuyPrice,
            Actions.ViewHistory
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.UserGroupViewModel_UserGroupViewModel_Товарные_остатки,
          Actions = new List<Actions>()
          {
            Actions.AddGoodStock,
            Actions.DeleteGoodStock,
            Actions.ViewStock,
            Actions.EditQuantityGoodStock,
            Actions.EditSalePriceGoodStock
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FormGroup_КатегорииТоваров,
          Actions = new List<Actions>()
          {
            Actions.AddGoodGroup,
            Actions.EditGoodGroup,
            Actions.DeleteGoodGroup
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FrmMainWindow_Настройки,
          Actions = new List<Actions>()
          {
            Actions.SettingsShowAndEdit
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FrmSummaryReport_Продажи,
          Actions = new List<Actions>()
          {
            Actions.SaleSave,
            Actions.ReturnSale,
            Actions.CreditReturn,
            Actions.DeleteSale,
            Actions.ShowJournalSale
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FrmMainWindow_Отчеты,
          Actions = new List<Actions>()
          {
            Actions.ShowSummaryReport,
            Actions.ShowMasterReport,
            Actions.ShowSellerReport
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FrmSummaryReport_ДвижениеСредств,
          Actions = new List<Actions>()
          {
            Actions.InsertCash,
            Actions.RemoveCash,
            Actions.SendCash,
            Actions.CorrectSumByAcc,
            Actions.DeletePayment,
            Actions.CorrectBalanceSum
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.PageVisualModelView_Поступления,
          Actions = new List<Actions>()
          {
            Actions.WaybillListShow,
            Actions.WaybillAdd,
            Actions.WaybillEdit,
            Actions.WaybillDelete
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.GlobalDictionaries_Перемещение,
          Actions = new List<Actions>()
          {
            Actions.AddMoveWaybill,
            Actions.DeleteMoveWaybill
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.GlobalDictionaries_Перемещение_между_складами,
          Actions = new List<Actions>()
          {
            Actions.AddMoveStorage,
            Actions.DeleteMoveStorage
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.JournalGoodViewModel_JournalGoodViewModel_Производство,
          Actions = new List<Actions>()
          {
            Actions.AddProduction,
            Actions.DeleteProduction,
            Actions.ShowProduction,
            Actions.AddSpeedProduction
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.UserGroupViewModel_Покупатели,
          Actions = new List<Actions>()
          {
            Actions.ClientsAdd,
            Actions.ClientsCatalogShow,
            Actions.ClientsDelete,
            Actions.ClientsEdit,
            Actions.ClientJoin,
            Actions.ShowCredits,
            Actions.ClientsBonusesEdit
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FrmClientGroup_ГруппыКонтактов,
          Actions = new List<Actions>()
          {
            Actions.AddClientGroup,
            Actions.EditClientGroup,
            Actions.DeleteClientGroup
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FrmMainWindow_Инвентаризация,
          Actions = new List<Actions>()
          {
            Actions.CreateInventory,
            Actions.EditInventory,
            Actions.DeleteInventory,
            Actions.ShowJournalInventory
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.UserGroupViewModel_Списание_товаров,
          Actions = new List<Actions>()
          {
            Actions.CreateWriteOff,
            Actions.EditWriteOff,
            Actions.DeleteWriteOff,
            Actions.ShowJournalWriteOff
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.FrmMainWindow_ЗаказыРезервы,
          Actions = new List<Actions>()
          {
            Actions.CreateClientOrder,
            Actions.EditClientOrder,
            Actions.DeleteClientOrder
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.UserGroupViewModel_Режим__Кафе_,
          Actions = new List<Actions>()
          {
            Actions.DeleteOrderCafe
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.UserGroupViewModel_UserGroupViewModel_Чек_счёт,
          Actions = new List<Actions>()
          {
            Actions.DeleteItemBasket,
            Actions.EditCountItemBasket,
            Actions.EditDiscountItem,
            Actions.CancelSale
          }
        },
        new UserGroupViewModel.ActionGrid()
        {
          _name = Translate.UserGroupViewModel_UserGroupViewModel_Другое,
          Actions = new List<Actions>()
          {
            Actions.ExecuteScript,
            Actions.PrintKkmReport,
            Actions.EditFrReport
          }
        }
      };
      if (new ConfigsRepository<Integrations>().Get().Egais.IsActive)
        collection.Add(new UserGroupViewModel.ActionGrid()
        {
          _name = "ЕГАИС",
          Actions = new List<Actions>()
          {
            Actions.ShowEgaisWaybill,
            Actions.AcceptEgaisWaybill,
            Actions.ActionsToBeerTap
          }
        });
      if (new ConfigsRepository<Integrations>().Get().Sms.Type != GlobalDictionaries.SmsServiceType.None)
        collection.Add(new UserGroupViewModel.ActionGrid()
        {
          _name = "СМС-подтверждение",
          Actions = new List<Actions>()
          {
            Actions.DoUseBonusesIfOffSmsCode,
            Actions.DoSaleCreditIfOffSmsCode
          }
        });
      this.ListAction.AddRange((IEnumerable<UserGroupViewModel.ActionGrid>) collection);
      foreach (UserGroupViewModel.ActionGrid actionGrid in this.ListAction)
      {
        foreach (Actions action1 in actionGrid.Actions)
        {
          Actions action = action1;
          actionGrid.ListActionGroup.Add(new UserGroupViewModel.ActionItem()
          {
            Name = GlobalDictionaries.ActionsUserDictionary.Single<KeyValuePair<Actions, string>>((Func<KeyValuePair<Actions, string>, bool>) (x => x.Key == action)).Value,
            Actions = action,
            IsChecked = group.Permissions.Single<PermissionRules.PermissionRule>((Func<PermissionRules.PermissionRule, bool>) (x => x.Action == action)).IsGranted,
            UpdateName = new Action<string>(this.UpdateName)
          });
        }
      }
    }

    private void UpdateName(string name)
    {
      UserGroupViewModel.ActionGrid actionGrid = this.ListAction.FirstOrDefault<UserGroupViewModel.ActionGrid>((Func<UserGroupViewModel.ActionGrid, bool>) (x => x.ListActionGroup.Any<UserGroupViewModel.ActionItem>((Func<UserGroupViewModel.ActionItem, bool>) (a => a.Name == name))));
      if (actionGrid == null)
        return;
      actionGrid.ListActionGroup = new ObservableCollection<UserGroupViewModel.ActionItem>((IEnumerable<UserGroupViewModel.ActionItem>) actionGrid.ListActionGroup);
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      foreach (UserGroupViewModel.ActionGrid actionGrid in this.ListAction)
      {
        foreach (UserGroupViewModel.ActionItem actionItem1 in (Collection<UserGroupViewModel.ActionItem>) actionGrid.ListActionGroup)
        {
          UserGroupViewModel.ActionItem actionItem = actionItem1;
          this.Group.Permissions.Single<PermissionRules.PermissionRule>((Func<PermissionRules.PermissionRule, bool>) (x => x.Action == actionItem.Actions)).IsGranted = actionItem.IsChecked;
        }
      }
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Group);
    }

    public class ActionGrid : ViewModel
    {
      private ObservableCollection<UserGroupViewModel.ActionItem> _listActionGroup = new ObservableCollection<UserGroupViewModel.ActionItem>();

      public string _name { get; set; }

      public string Name
      {
        get
        {
          return this._name + string.Format(" ({0}/{1})", (object) this.ListActionGroup.Count<UserGroupViewModel.ActionItem>((Func<UserGroupViewModel.ActionItem, bool>) (x => x.IsChecked)), (object) this.ListActionGroup.Count);
        }
      }

      public List<Actions> Actions { get; set; }

      public ObservableCollection<UserGroupViewModel.ActionItem> ListActionGroup
      {
        get => this._listActionGroup;
        set
        {
          this._listActionGroup = value;
          this.OnPropertyChanged("Name");
        }
      }
    }

    public class ActionItem : ViewModel
    {
      private bool _isChecked;

      public string Name { get; set; }

      public Action<string> UpdateName { get; set; }

      public bool IsChecked
      {
        get => this._isChecked;
        set
        {
          this._isChecked = value;
          Action<string> updateName = this.UpdateName;
          if (updateName == null)
            return;
          updateName(this.Name);
        }
      }

      public Actions Actions { get; set; }
    }
  }
}
