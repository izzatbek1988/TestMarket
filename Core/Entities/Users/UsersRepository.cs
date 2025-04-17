// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.UsersRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db.Clients;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Numeric;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public class UsersRepository : IEntityRepository<Gbs.Core.Entities.Users.User, USERS>
  {
    public UsersRepository(Gbs.Core.Db.DataBase db = null)
    {
    }

    public UsersRepository()
    {
    }

    public List<Gbs.Core.Entities.Users.User> GetAllItems()
    {
      return this.GetByQuery((IQueryable<USERS>) Data.GetDataContext().GetTable<USERS>());
    }

    public List<Gbs.Core.Entities.Users.User> GetActiveItems()
    {
      return this.GetByQuery(Data.GetDataContext().GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => !x.IS_DELETED)));
    }

    public Gbs.Core.Entities.Users.User GetByUid(Guid uid)
    {
      return this.GetByQuery(Data.GetDataContext().GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => x.UID == uid))).SingleOrDefault<Gbs.Core.Entities.Users.User>();
    }

    public bool Save(Gbs.Core.Entities.Users.User item)
    {
      if (item.Alias.IsNullOrEmpty() && item.Client != null)
        item.Alias = Functions.CutName(item.Client.Name);
      if (this.Validate(item).Result == ActionResult.Results.Error)
        return false;
      DataContext dataContext = Data.GetDataContext();
      USERS users = new USERS();
      users.UID = item.Uid;
      users.IS_DELETED = item.IsDeleted;
      users.GROUP_UID = item.Group.Uid;
      users.BARCODE = item.Barcode;
      Client client = item.Client;
      // ISSUE: explicit non-virtual call
      users.CLIENT_UID = client != null ? __nonvirtual (client.Uid) : Guid.Empty;
      users.FONT_SIZE = item.FontSize.ToDbDecimal();
      users.DATE_OUT = item.IsKicked ? item.DateOut ?? DateTime.Now : new DateTime(2001, 1, 1);
      users.DATE_IN = item.DateIn;
      users.IS_KICKED = item.IsKicked;
      users.ALIAS = item.Alias;
      users.PASSWORD = item.Password;
      users.SECTION_UID = item.OnlineOnSectionUid;
      users.LOGIN_FOR_KKM = item.LoginForKkm;
      users.PASSWORD_FOR_KKM = item.PasswordForKkm;
      dataContext.InsertOrReplace<USERS>(users);
      return true;
    }

    public int Save(List<Gbs.Core.Entities.Users.User> itemsList)
    {
      return itemsList.Count<Gbs.Core.Entities.Users.User>(new Func<Gbs.Core.Entities.Users.User, bool>(this.Save));
    }

    public int Delete(List<Gbs.Core.Entities.Users.User> itemsList)
    {
      throw new NotImplementedException();
    }

    public bool Delete(Gbs.Core.Entities.Users.User item) => throw new NotImplementedException();

    public ActionResult Validate(Gbs.Core.Entities.Users.User item)
    {
      DataContext dataContext = Data.GetDataContext();
      if (!item.Barcode.IsNullOrEmpty())
      {
        if (this.GetByQuery(dataContext.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => x.BARCODE == item.Barcode && !x.IS_DELETED && x.UID != item.Uid))).Any<Gbs.Core.Entities.Users.User>())
          return new ActionResult(ActionResult.Results.Error, Translate.User_Такой_штрих_код_уже_есть);
      }
      return ValidationHelper.DataValidation((Entity) item);
    }

    public List<Gbs.Core.Entities.Users.User> GetByQuery(IQueryable<USERS> query = null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UsersRepository.\u003C\u003Ec__DisplayClass10_0 cDisplayClass100 = new UsersRepository.\u003C\u003Ec__DisplayClass10_0();
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass100.dc = Data.GetDataContext();
      if (query == null)
      {
        // ISSUE: reference to a compiler-generated field
        query = (IQueryable<USERS>) cDisplayClass100.dc.GetTable<USERS>();
      }
      // ISSUE: reference to a compiler-generated field
      query = cDisplayClass100.dc.GetQuery<USERS>(query);
      Console.WriteLine(query.ToString());
      Performancer performancer = new Performancer("Загрузка пользователей из БД.");
      List<USERS> list1 = query.ToList<USERS>();
      performancer.AddPoint(string.Format("Выполнен запрос. Users: {0}", (object) list1.Count));
      if (!list1.Any<USERS>())
      {
        performancer.Stop("Прервано, т.к. нет записей контактов");
        return new List<Gbs.Core.Entities.Users.User>();
      }
      ParameterExpression right;
      ParameterExpression parameterExpression;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method reference
      List<Client> byQuery = new ClientsRepository().GetByQuery(query.Select<USERS, Guid>((Expression<Func<USERS, Guid>>) (x => x.CLIENT_UID)).Distinct<Guid>().SelectMany<Guid, CLIENTS, CLIENTS>((Expression<Func<Guid, IEnumerable<CLIENTS>>>) (u => cDisplayClass100.dc.GetTable<CLIENTS>().Where<CLIENTS>(Expression.Lambda<Func<CLIENTS, bool>>((Expression) Expression.Equal(x.UID, (Expression) right, false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression))), (Expression<Func<Guid, CLIENTS, CLIENTS>>) ((u, c) => c)));
      performancer.AddPoint(string.Format("Клиенты: {0}", (object) byQuery.Count));
      // ISSUE: reference to a compiler-generated field
      cDisplayClass100.gr = UserGroups.GetListUserGroups();
      // ISSUE: reference to a compiler-generated field
      performancer.AddPoint(string.Format("Группы пользователей: {0}", (object) cDisplayClass100.gr.Count));
      // ISSUE: reference to a compiler-generated method
      List<Gbs.Core.Entities.Users.User> list2 = list1.Join<USERS, Client, Guid, Gbs.Core.Entities.Users.User>((IEnumerable<Client>) byQuery, (Func<USERS, Guid>) (u => u.CLIENT_UID), (Func<Client, Guid>) (c => c.Uid), new Func<USERS, Client, Gbs.Core.Entities.Users.User>(cDisplayClass100.\u003CGetByQuery\u003Eb__5)).AsParallel<Gbs.Core.Entities.Users.User>().ToList<Gbs.Core.Entities.Users.User>();
      Console.WriteLine(string.Format("time: {0}", (object) ((double) stopwatch.ElapsedMilliseconds / 1000.0)));
      performancer.Stop();
      return list2;
    }

    public bool DisconnectUsers(bool isAll = false)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<USERS> query;
        if (!isAll)
        {
          ParameterExpression parameterExpression;
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          query = dataBase.GetTable<USERS>().Where<USERS>(Expression.Lambda<Func<USERS, bool>>((Expression) Expression.Equal(g.SECTION_UID, (Expression) Expression.Property((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Sections.GetCurrentSection)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression));
        }
        else
          query = dataBase.GetTable<USERS>();
        foreach (Gbs.Core.Entities.Users.User user in this.GetByQuery(query))
        {
          user.OnlineOnSectionUid = Guid.Empty;
          this.Save(user);
        }
        return true;
      }
    }

    public List<Gbs.Core.Entities.Users.User> GetOnlineUsersList()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        ParameterExpression parameterExpression;
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        return this.GetByQuery(dataBase.GetTable<USERS>().Where<USERS>(Expression.Lambda<Func<USERS, bool>>((Expression) Expression.Equal(x.SECTION_UID, (Expression) Expression.Property((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Sections.GetCurrentSection)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression))).ToList<Gbs.Core.Entities.Users.User>();
      }
    }

    public bool GetAccess(ref Gbs.Core.Entities.Users.User item, Actions action)
    {
      return this.getAccess(ref item, action);
    }

    public bool GetAccess(Gbs.Core.Entities.Users.User item, Actions action)
    {
      return this.getAccess(ref item, action);
    }

    private bool getAccess(ref Gbs.Core.Entities.Users.User item, Actions action)
    {
      if (!HomeOfficeHelper.IsAuthRequire && new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        Gbs.Core.Entities.Users.User user1 = item;
        // ISSUE: explicit non-virtual call
        if ((user1 != null ? __nonvirtual (user1.Uid) : Guid.Empty) == Guid.Empty)
        {
          ref Gbs.Core.Entities.Users.User local = ref item;
          Gbs.Core.Entities.Users.User user2 = new Gbs.Core.Entities.Users.User();
          user2.Uid = Guid.Empty;
          local = user2;
          return true;
        }
      }
      if (item?.Group == null)
        return false;
      if (item.Group.IsSuper)
        return true;
      List<PermissionRules.PermissionRule> list = item.Group.Permissions.Where<PermissionRules.PermissionRule>((Func<PermissionRules.PermissionRule, bool>) (p => p.Action == action)).ToList<PermissionRules.PermissionRule>();
      switch (list.Count)
      {
        case 0:
          return false;
        case 1:
          return list.First<PermissionRules.PermissionRule>().IsGranted;
        default:
          return false;
      }
    }
  }
}
