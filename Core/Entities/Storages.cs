// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Storages
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class Storages
  {
    public static IEnumerable<Storages.Storage> GetStorages(IQueryable<STORAGES> query = null)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<STORAGES> source = query ?? dataBase.GetTable<STORAGES>();
        ParameterExpression parameterExpression = Expression.Parameter(typeof (STORAGES), "storage");
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        // ISSUE: method reference
        MemberInitExpression body = Expression.MemberInit(Expression.New(typeof (Storages.Storage)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.set_Uid)), (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (STORAGES.get_UID)))), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.set_IsDeleted)), (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (STORAGES.get_IS_DELETED)))), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Storages.Storage.set_Name)), (Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (STORAGES.get_NAME)))));
        ParameterExpression[] parameterExpressionArray = new ParameterExpression[1]
        {
          parameterExpression
        };
        return (IEnumerable<Storages.Storage>) source.Select<STORAGES, Storages.Storage>(Expression.Lambda<Func<STORAGES, Storages.Storage>>((Expression) body, parameterExpressionArray)).ToList<Storages.Storage>();
      }
    }

    public class Storage : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Name { get; set; } = string.Empty;

      public Storage()
      {
      }

      public Storage(STORAGES dbItem)
      {
        this.Uid = dbItem.UID;
        this.Name = dbItem.NAME;
        this.IsDeleted = dbItem.IS_DELETED;
      }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool Save()
      {
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        using (DataBase dataBase = Data.GetDataBase())
        {
          dataBase.InsertOrReplace<STORAGES>(new STORAGES()
          {
            IS_DELETED = this.IsDeleted,
            UID = this.Uid,
            NAME = this.Name
          });
          return true;
        }
      }
    }
  }
}
