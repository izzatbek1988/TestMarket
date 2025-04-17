// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.PaymentGroups
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class PaymentGroups
  {
    public static List<PaymentGroups.PaymentGroup> GetPaymentGroupsList(
      IQueryable<PAYMENTS_GROUP> query = null)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<PAYMENTS_GROUP>();
        return query.ToList<PAYMENTS_GROUP>().Select<PAYMENTS_GROUP, PaymentGroups.PaymentGroup>((Func<PAYMENTS_GROUP, PaymentGroups.PaymentGroup>) (u =>
        {
          return new PaymentGroups.PaymentGroup()
          {
            Uid = u.UID,
            Comment = u.COMMENT,
            Name = u.NAME,
            VisibleIn = (PaymentGroups.VisiblePaymentGroup) u.VISIBLE_IN,
            ParentGroup = u.PARENT_UID == Guid.Empty ? (PaymentGroups.PaymentGroup) null : PaymentGroups.GetPaymentGroupByUid(u.PARENT_UID),
            IsDeleted = u.IS_DELETED,
            IsUseForProfit = u.IS_USE_FOR_PROFIT
          };
        })).ToList<PaymentGroups.PaymentGroup>();
      }
    }

    public static PaymentGroups.PaymentGroup GetPaymentGroupByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return PaymentGroups.GetPaymentGroupsList(dataBase.GetTable<PAYMENTS_GROUP>().Where<PAYMENTS_GROUP>((Expression<Func<PAYMENTS_GROUP, bool>>) (x => x.UID == uid))).FirstOrDefault<PaymentGroups.PaymentGroup>();
    }

    public static Dictionary<PaymentGroups.VisiblePaymentGroup, string> GetDictionaryVisiblePayment()
    {
      return new Dictionary<PaymentGroups.VisiblePaymentGroup, string>()
      {
        {
          PaymentGroups.VisiblePaymentGroup.Income,
          Translate.PaymentGroups_В_документе_дохода
        },
        {
          PaymentGroups.VisiblePaymentGroup.Expense,
          Translate.PaymentGroups_В_документе_расхода
        }
      };
    }

    public enum VisiblePaymentGroup
    {
      Income,
      Expense,
    }

    public class PaymentGroup : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Name { get; set; } = string.Empty;

      public PaymentGroups.PaymentGroup ParentGroup { get; set; }

      [Required]
      public PaymentGroups.VisiblePaymentGroup VisibleIn { get; set; }

      public bool IsUseForProfit { get; set; }

      public string Comment { get; set; } = string.Empty;

      public bool Save()
      {
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        using (DataBase dataBase1 = Data.GetDataBase())
        {
          DataBase dataBase2 = dataBase1;
          PAYMENTS_GROUP paymentsGroup = new PAYMENTS_GROUP();
          paymentsGroup.UID = this.Uid;
          paymentsGroup.NAME = this.Name;
          paymentsGroup.COMMENT = this.Comment;
          paymentsGroup.VISIBLE_IN = (int) this.VisibleIn;
          PaymentGroups.PaymentGroup parentGroup = this.ParentGroup;
          // ISSUE: explicit non-virtual call
          paymentsGroup.PARENT_UID = parentGroup != null ? __nonvirtual (parentGroup.Uid) : Guid.Empty;
          paymentsGroup.IS_DELETED = this.IsDeleted;
          paymentsGroup.IS_USE_FOR_PROFIT = this.IsUseForProfit;
          dataBase2.InsertOrReplace<PAYMENTS_GROUP>(paymentsGroup);
          return true;
        }
      }

      public ActionResult VerifyBeforeSave()
      {
        List<string> stringList = new List<string>();
        if (this.ParentGroup != null)
        {
          if (this.Uid == this.ParentGroup.Uid)
            stringList.Add(Translate.Group_Категория_не_может_быть_родителем_сама_себе);
          if (this.VisibleIn != this.ParentGroup.VisibleIn)
            stringList.Add(Translate.PaymentGroup_Родительская_категория_отличается_от_дочерней_типом);
          if (this.GetAllChild().Any<PaymentGroups.PaymentGroup>((Func<PaymentGroups.PaymentGroup, bool>) (x => x.Uid == this.ParentGroup.Uid)))
            stringList.Add(Translate.Group_Родительская_категория_является_потомком_текущий_и_не_может_быть_ее_родителем);
        }
        if (!stringList.Any<string>())
          return this.DataValidation();
        int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) stringList), Translate.Entity_Ошибка_валидации_данных, icon: MessageBoxImage.Exclamation);
        return new ActionResult(ActionResult.Results.Error, stringList);
      }

      public IEnumerable<PaymentGroups.PaymentGroup> GetAllChild(bool includeSubChild = true)
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<PaymentGroups.PaymentGroup> list = PaymentGroups.GetPaymentGroupsList(dataBase.GetTable<PAYMENTS_GROUP>()).ToList<PaymentGroups.PaymentGroup>();
          List<PaymentGroups.PaymentGroup> allChild = new List<PaymentGroups.PaymentGroup>();
          Func<PaymentGroups.PaymentGroup, bool> predicate = (Func<PaymentGroups.PaymentGroup, bool>) (x =>
          {
            Guid? uid1 = x.ParentGroup?.Uid;
            Guid uid2 = this.Uid;
            return uid1.HasValue && uid1.GetValueOrDefault() == uid2;
          });
          foreach (PaymentGroups.PaymentGroup paymentGroup in list.Where<PaymentGroups.PaymentGroup>(predicate))
          {
            allChild.Add(paymentGroup);
            if (includeSubChild)
              allChild.AddRange(paymentGroup.GetAllChild());
          }
          return (IEnumerable<PaymentGroups.PaymentGroup>) allChild;
        }
      }
    }
  }
}
