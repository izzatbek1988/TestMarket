// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.ActionHistoryRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities
{
  public class ActionHistoryRepository : IEntityRepository<ActionHistory, ACTIONS_HISTORY>
  {
    private readonly Gbs.Core.Db.DataBase _db;

    public ActionHistoryRepository(Gbs.Core.Db.DataBase db) => this._db = db;

    public List<ActionHistory> GetByQuery(IQueryable<ACTIONS_HISTORY> query)
    {
      LogHelper.OnBegin();
      List<Users.User> users = new UsersRepository(this._db).GetAllItems();
      List<Sections.Section> sections = Sections.GetSectionsList();
      List<ActionHistory> list = query.ToList<ACTIONS_HISTORY>().Select<ACTIONS_HISTORY, ActionHistory>((Func<ACTIONS_HISTORY, ActionHistory>) (x =>
      {
        return new ActionHistory()
        {
          Uid = x.UID,
          DateTime = x.DATE_TIME,
          ActionType = (ActionType) x.ACTION_TYPE,
          Data = ((IEnumerable<string>) System.Text.Encoding.UTF8.GetString(x.DATA_BLOB).Split('\n')).ToList<string>(),
          EntityType = (GlobalDictionaries.EntityTypes) x.ENTITY_TYPE,
          EntityUid = x.ENTITY_UID,
          User = users.SingleOrDefault<Users.User>((Func<Users.User, bool>) (u => u.Uid == x.USER_UID)),
          Section = sections.SingleOrDefault<Sections.Section>((Func<Sections.Section, bool>) (u => u.Uid == x.SECTION_UID)),
          IsDeleted = x.IS_DELETED
        };
      })).ToList<ActionHistory>();
      LogHelper.OnEnd();
      return list;
    }

    public List<ActionHistory> GetAllItems()
    {
      return this.GetByQuery(this._db.GetTable<ACTIONS_HISTORY>());
    }

    public List<ActionHistory> GetActiveItems()
    {
      return this.GetByQuery(this._db.GetTable<ACTIONS_HISTORY>().Where<ACTIONS_HISTORY>((Expression<Func<ACTIONS_HISTORY, bool>>) (x => !x.IS_DELETED)));
    }

    public ActionHistory GetByUid(Guid uid)
    {
      return this.GetByQuery(this._db.GetTable<ACTIONS_HISTORY>().Where<ACTIONS_HISTORY>((Expression<Func<ACTIONS_HISTORY, bool>>) (x => x.UID == uid))).FirstOrDefault<ActionHistory>();
    }

    public bool Save(ActionHistory item)
    {
      LogHelper.OnBegin();
      ActionResult actionResult = this.Validate(item);
      if (actionResult.Result != ActionResult.Results.Ok)
      {
        int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages));
      }
      try
      {
        Gbs.Core.Config.DataBase dataBase = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
        if ((dataBase != null ? (dataBase.ModeProgram != GlobalDictionaries.Mode.Home ? 1 : 0) : 1) != 0)
        {
          Gbs.Core.Db.DataBase db = this._db;
          ACTIONS_HISTORY actionsHistory = new ACTIONS_HISTORY();
          actionsHistory.UID = item.Uid;
          actionsHistory.ENTITY_UID = item.EntityUid;
          actionsHistory.ACTION_TYPE = (int) item.ActionType;
          actionsHistory.DATA_BLOB = System.Text.Encoding.UTF8.GetBytes(string.Join(Other.NewLine(), (IEnumerable<string>) item.Data));
          actionsHistory.DATE_TIME = item.DateTime;
          actionsHistory.ENTITY_TYPE = (int) item.EntityType;
          actionsHistory.SECTION_UID = item.Section.Uid;
          Users.User user = item.User;
          // ISSUE: explicit non-virtual call
          actionsHistory.USER_UID = user != null ? __nonvirtual (user.Uid) : Guid.Empty;
          actionsHistory.IS_DELETED = item.IsDeleted;
          db.InsertOrReplace<ACTIONS_HISTORY>(actionsHistory);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в репозитории действий");
      }
      LogHelper.OnEnd();
      return true;
    }

    public int Save(List<ActionHistory> itemsList)
    {
      return itemsList.Count<ActionHistory>(new Func<ActionHistory, bool>(this.Save));
    }

    public int Delete(List<ActionHistory> itemsList)
    {
      return itemsList.Count<ActionHistory>(new Func<ActionHistory, bool>(this.Delete));
    }

    public bool Delete(ActionHistory item)
    {
      try
      {
        LogHelper.OnBegin();
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          dataBase.GetTable<ACTIONS_HISTORY>().Where<ACTIONS_HISTORY>((Expression<Func<ACTIONS_HISTORY, bool>>) (x => x.UID == item.Uid)).Delete<ACTIONS_HISTORY>();
          LogHelper.OnEnd();
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка удаления записи из истории действий");
        return false;
      }
    }

    public ActionResult Validate(ActionHistory item)
    {
      return ValidationHelper.DataValidation((Entity) item);
    }
  }
}
