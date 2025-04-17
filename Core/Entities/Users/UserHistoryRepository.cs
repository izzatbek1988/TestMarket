// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.UserHistoryRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db.Tables.Users;
using Gbs.Core.Db.Users;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public class UserHistoryRepository : IEntityRepository<UserHistory, USER_HISTORY>
  {
    private Gbs.Core.Db.DataBase _db;

    public UserHistoryRepository(Gbs.Core.Db.DataBase db) => this._db = db;

    public void AddHistoryInputUser(Gbs.Core.Entities.Users.User user)
    {
      try
      {
        Gbs.Core.Config.DataBase dataBase = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
        if ((dataBase != null ? (dataBase.ModeProgram != GlobalDictionaries.Mode.Home ? 1 : 0) : 1) == 0)
          return;
        Other.ConsoleWrite("Заись о входе пользователя " + user.Alias);
        this._db.InsertOrReplace<USER_HISTORY>(new USER_HISTORY()
        {
          UID = Guid.NewGuid(),
          USER_UID = user.Uid,
          DATE_IN = DateTime.Now,
          DATE_OUT = DateTime.Now.AddSeconds(1.0),
          SECTION_UID = user.OnlineOnSectionUid
        });
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в репозитории пользователей");
      }
    }

    public List<UserHistory> GetByQuery(IQueryable<USER_HISTORY> query = null)
    {
      if (query == null)
        query = this._db.GetTable<USER_HISTORY>();
      ParameterExpression right;
      ParameterExpression parameterExpression;
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      List<Gbs.Core.Entities.Users.User> byQuery = new UsersRepository(this._db).GetByQuery(query.Select<USER_HISTORY, Guid>((Expression<Func<USER_HISTORY, Guid>>) (x => x.USER_UID)).Distinct<Guid>().SelectMany<Guid, USERS, USERS>(Expression.Lambda<Func<Guid, IEnumerable<USERS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
      {
        (Expression) Expression.Call(this._db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Gbs.Core.Db.DataBase.GetTable)), Array.Empty<Expression>()),
        (Expression) Expression.Quote((Expression) Expression.Lambda<Func<USERS, bool>>((Expression) Expression.Equal(x.UID, (Expression) right, false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression))
      }), right), (Expression<Func<Guid, USERS, USERS>>) ((u, c) => c)));
      return query.ToList<USER_HISTORY>().Join<USER_HISTORY, Gbs.Core.Entities.Users.User, Guid, UserHistory>((IEnumerable<Gbs.Core.Entities.Users.User>) byQuery, (Func<USER_HISTORY, Guid>) (h => h.USER_UID), (Func<Gbs.Core.Entities.Users.User, Guid>) (u => u.Uid), (Func<USER_HISTORY, Gbs.Core.Entities.Users.User, UserHistory>) ((h, u) =>
      {
        return new UserHistory()
        {
          Uid = h.UID,
          SectionUid = h.SECTION_UID,
          IsDeleted = h.IS_DELETED,
          DateIn = h.DATE_IN,
          DateOut = h.DATE_OUT,
          User = u
        };
      })).AsParallel<UserHistory>().ToList<UserHistory>();
    }

    public List<UserHistory> GetAllItems() => throw new NotImplementedException();

    public List<UserHistory> GetActiveItems()
    {
      return this.GetByQuery(this._db.GetTable<USER_HISTORY>().Where<USER_HISTORY>((Expression<Func<USER_HISTORY, bool>>) (x => !x.IS_DELETED)));
    }

    public UserHistory GetByUid(Guid uid) => throw new NotImplementedException();

    public bool Save(UserHistory item)
    {
      if (this.Validate(item).Result == ActionResult.Results.Error)
        return false;
      try
      {
        Gbs.Core.Config.DataBase dataBase = new ConfigsRepository<Gbs.Core.Config.DataBase>().Get();
        if ((dataBase != null ? (dataBase.ModeProgram != GlobalDictionaries.Mode.Home ? 1 : 0) : 1) != 0)
          this._db.InsertOrReplace<USER_HISTORY>(new USER_HISTORY()
          {
            UID = item.Uid,
            IS_DELETED = item.IsDeleted,
            USER_UID = item.User.Uid,
            DATE_OUT = item.DateOut,
            DATE_IN = item.DateIn,
            SECTION_UID = item.SectionUid
          });
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в репозитории пользователей");
      }
      return true;
    }

    public int Save(List<UserHistory> itemsList)
    {
      return itemsList.Count<UserHistory>(new Func<UserHistory, bool>(this.Save));
    }

    public int Delete(List<UserHistory> itemsList) => throw new NotImplementedException();

    public bool Delete(UserHistory item) => throw new NotImplementedException();

    public ActionResult Validate(UserHistory item)
    {
      return ValidationHelper.DataValidation((Entity) item);
    }
  }
}
