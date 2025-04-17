// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.UserGroups
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Users;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class UserGroups
  {
    public static UserGroups.UserGroup AddDefaultUserGroup()
    {
      UserGroups.UserGroup userGroup = new UserGroups.UserGroup();
      userGroup.IsSuper = true;
      userGroup.Name = Translate.UserGroups_Сотрудники;
      userGroup.Permissions = UserGroups.GetDefaultPermissions();
      userGroup.Save();
      return userGroup;
    }

    private static List<PermissionRules.PermissionRule> GetDefaultPermissions()
    {
      List<PermissionRules.PermissionRule> list = ((IEnumerable<Actions>) Enum.GetValues(typeof (Actions))).Select<Actions, PermissionRules.PermissionRule>((Func<Actions, PermissionRules.PermissionRule>) (action =>
      {
        return new PermissionRules.PermissionRule()
        {
          Action = action,
          IsGranted = false,
          Uid = Guid.NewGuid()
        };
      })).ToList<PermissionRules.PermissionRule>();
      foreach (PermissionRules.PermissionRule permissionRule in list.Where<PermissionRules.PermissionRule>(new Func<PermissionRules.PermissionRule, bool>(Predicate)))
        permissionRule.IsGranted = true;
      return list;

      static bool Predicate(PermissionRules.PermissionRule x)
      {
        return x.Action.IsEither<Actions>(Actions.DeleteItemBasket, Actions.EditCountItemBasket, Actions.EditDiscountItem, Actions.CancelSale, Actions.ShowSellerReport, Actions.PrintKkmReport);
      }
    }

    private static IEnumerable<PermissionRules.PermissionRule> UnionPermissionWithDefault(
      IEnumerable<PermissionRules.PermissionRule> permissionsList)
    {
      List<PermissionRules.PermissionRule> defaultPermissions = UserGroups.GetDefaultPermissions();
      foreach (PermissionRules.PermissionRule permissions in permissionsList)
      {
        PermissionRules.PermissionRule rule = permissions;
        PermissionRules.PermissionRule permissionRule = defaultPermissions.SingleOrDefault<PermissionRules.PermissionRule>((Func<PermissionRules.PermissionRule, bool>) (p => p.Action == rule.Action));
        if (permissionRule != null)
        {
          permissionRule.GroupUid = rule.GroupUid;
          permissionRule.IsGranted = rule.IsGranted;
          permissionRule.Uid = rule.Uid;
          defaultPermissions[defaultPermissions.IndexOf(permissionRule)] = permissionRule;
        }
      }
      return (IEnumerable<PermissionRules.PermissionRule>) defaultPermissions;
    }

    public static List<UserGroups.UserGroup> GetListUserGroups(IQueryable<USERS_GROUPS> query = null)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UserGroups.\u003C\u003Ec__DisplayClass3_0 cDisplayClass30 = new UserGroups.\u003C\u003Ec__DisplayClass3_0();
      Performancer performancer = new Performancer("Загрузка групп пользователей");
      // ISSUE: reference to a compiler-generated field
      cDisplayClass30.db = Data.GetDataBase();
      try
      {
        if (query == null)
        {
          // ISSUE: reference to a compiler-generated field
          query = cDisplayClass30.db.GetTable<USERS_GROUPS>();
        }
        query = query.Distinct<USERS_GROUPS>();
        List<USERS_GROUPS> list1 = query.ToList<USERS_GROUPS>();
        performancer.AddPoint(string.Format("Список групп: {0}", (object) list1.Count));
        ParameterExpression parameterExpression1;
        ParameterExpression parameterExpression2;
        // ISSUE: method reference
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        List<PermissionRules.PermissionRule> permissionList = PermissionRules.GetPermissionList(query.SelectMany<USERS_GROUPS, SETTINGS, SETTINGS>(Expression.Lambda<Func<USERS_GROUPS, IEnumerable<SETTINGS>>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Queryable.Where)), new Expression[2]
        {
          (Expression) Expression.Call(cDisplayClass30.db, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DataBase.GetTable)), Array.Empty<Expression>()),
          (Expression) Expression.Quote((Expression) Expression.Lambda<Func<SETTINGS, bool>>((Expression) Expression.AndAlso((Expression) Expression.Equal(p.ENTITY_UID, (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (USERS_GROUPS.get_UID))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (SETTINGS.get_TYPE))), (Expression) Expression.Constant((object) 0, typeof (int)))), parameterExpression2))
        }), parameterExpression1), (Expression<Func<USERS_GROUPS, SETTINGS, SETTINGS>>) ((g, p) => p)).Distinct<SETTINGS>());
        performancer.AddPoint(string.Format("Список прав доступа: {0}", (object) permissionList.Count));
        List<UserGroups.UserGroup> list2 = list1.GroupJoin<USERS_GROUPS, PermissionRules.PermissionRule, Guid, UserGroups.UserGroup>((IEnumerable<PermissionRules.PermissionRule>) permissionList, (Func<USERS_GROUPS, Guid>) (g => g.UID), (Func<PermissionRules.PermissionRule, Guid>) (p => p.GroupUid), (Func<USERS_GROUPS, IEnumerable<PermissionRules.PermissionRule>, UserGroups.UserGroup>) ((g, p) =>
        {
          UserGroups.UserGroup listUserGroups = new UserGroups.UserGroup()
          {
            IsDeleted = g.IS_DELETED,
            Uid = g.UID,
            Name = g.NAME,
            IsSuper = g.IS_SUPER,
            Permissions = UserGroups.UnionPermissionWithDefault(p).ToList<PermissionRules.PermissionRule>()
          };
          if (!listUserGroups.IsSuper)
            return listUserGroups;
          foreach (PermissionRules.PermissionRule permission in listUserGroups.Permissions)
            permission.IsGranted = true;
          return listUserGroups;
        })).ToList<UserGroups.UserGroup>();
        performancer.Stop(string.Format("Всего записей: {0}", (object) list2.Count));
        return list2;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass30.db != null)
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClass30.db.Dispose();
        }
      }
    }

    public static UserGroups.UserGroup GetUserGroupByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return UserGroups.GetListUserGroups(dataBase.GetTable<USERS_GROUPS>().Where<USERS_GROUPS>((Expression<Func<USERS_GROUPS, bool>>) (g => g.UID == uid))).Single<UserGroups.UserGroup>();
    }

    public class UserGroup : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Name { get; set; } = string.Empty;

      public bool IsSuper { get; set; }

      [JsonIgnore]
      public List<PermissionRules.PermissionRule> Permissions { get; set; } = UserGroups.GetDefaultPermissions();

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool Save()
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
            return false;
          foreach (PermissionRules.PermissionRule permissionRule in this.Permissions.ToList<PermissionRules.PermissionRule>())
          {
            permissionRule.GroupUid = this.Uid;
            if (!permissionRule.Save())
              return false;
          }
          dataBase.InsertOrReplace<USERS_GROUPS>(new USERS_GROUPS()
          {
            UID = this.Uid,
            IS_DELETED = this.IsDeleted,
            NAME = this.Name,
            IS_SUPER = this.IsSuper
          });
          return true;
        }
      }
    }
  }
}
