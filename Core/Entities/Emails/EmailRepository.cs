// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Emails.EmailRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

#nullable disable
namespace Gbs.Core.Entities.Emails
{
  public class EmailRepository : IEntityRepository<Email, EMAIL>
  {
    public bool Send(DateTime date, string emailsTo, bool usePause = true)
    {
      try
      {
        LogHelper.OnBegin();
        bool flag = true;
        string[] strArray = emailsTo.Split(new char[3]
        {
          ',',
          ' ',
          ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Document> list = new DocumentsRepository(dataBase).GetItemsWithFilter(date.Date, isOnTime: false).Where<Document>((Func<Document, bool>) (x => x.Type.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.SaleReturn, GlobalDictionaries.DocumentsTypes.ProductionItem, GlobalDictionaries.DocumentsTypes.BeerProductionItem))).ToList<Document>();
          object obj = EmailRepository.GetBodyMail(date, list);
          string reportSale = EmailRepository.GetReportSale(list.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale)).ToList<Document>());
          if (reportSale.IsNullOrEmpty())
            obj = (object) (obj?.ToString() + Other.NewLine(3) + Translate.EmailRepository_Send_Не_удалось_сформировать_файл_с_отчетом_по_продажам_);
          for (int index = 0; index < strArray.Length; ++index)
          {
            string str = strArray[index];
            Email email = new Email()
            {
              Subject = string.Format(Translate.EmailRepository_Send_Отчет_на__0_g__из__1_, (object) DateTime.Now, UidDb.GetUid().Value),
              Body = obj,
              AddressTo = str,
              Attach = new List<string>() { reportSale }
            };
            email.IsSend = SmtpHelper.Send(email);
            LogHelper.Debug(email.IsSend ? string.Format(Translate.EmailRepository_Send_Отчет_на_почту__0__был_успешно_отправлен, (object) email.AddressTo) : string.Format(Translate.EmailRepository_Send_Во_время_отправки_отчета_на_почту__0__произошла_ошибка, (object) email.AddressTo));
            new EmailRepository().Save(email);
            flag &= email.IsSend;
            if (index + 1 != strArray.Length)
            {
              if (usePause)
                Thread.Sleep(60000);
            }
            else
              break;
          }
          LogHelper.OnEnd();
          return flag;
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        return false;
      }
    }

    public bool SendOldMail(Email mail)
    {
      mail.IsSend = SmtpHelper.Send(mail);
      new EmailRepository().Save(mail);
      return mail.IsSend;
    }

    public static object GetBodyMail(DateTime date, List<Document> documents)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Users.User> onlineUsersList = new UsersRepository(dataBase).GetOnlineUsersList();
        List<Gbs.Core.Entities.Payments.Payment> list = Gbs.Core.Entities.Payments.GetPaymentsList().Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => !x.IsDeleted)).ToList<Gbs.Core.Entities.Payments.Payment>();
        List<PaymentGroups.PaymentGroup> paymentGroupsList = PaymentGroups.GetPaymentGroupsList();
        return (object) (Translate.EmailRepository_GetBodyMail_ПРОДАЖИ_ + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_Всего_продаж___0_, (object) documents.Count<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale))) + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_Всего_товаров___0_, (object) documents.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale)).Sum<Document>((Func<Document, Decimal>) (x => x.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (i => i.Quantity))))) + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_На_сумму___0_N_, (object) documents.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale)).Sum<Document>(new Func<Document, Decimal>(SaleHelper.GetSumDocument))) + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_Прибыль___0_N_, (object) SaleHelper.GetProfitSum(documents, list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Date.Date == date.Date)).ToList<Gbs.Core.Entities.Payments.Payment>(), paymentGroupsList, out Decimal _)) + Other.NewLine() + "-------------------------" + Other.NewLine() + Translate.EmailRepository_GetBodyMail_ВОЗВРАТЫ_ + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_Всего_возвратов___0_, (object) documents.Count<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn))) + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_На_сумму___0_N_, (object) documents.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.SaleReturn)).Sum<Document>(new Func<Document, Decimal>(SaleHelper.GetSumDocument))) + Other.NewLine() + "-------------------------" + Other.NewLine() + Translate.EmailRepository_GetBodyMail_ОПЛАЧЕНО_ + Other.NewLine() + EmailRepository.GetPayments(list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyDocumentPayment && x.Date.Date == date.Date)).ToList<Gbs.Core.Entities.Payments.Payment>()) + "-------------------------" + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_Снято___0_N_, (object) list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyPayment && x.Date.Date == date.Date)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumOut))) + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_Внесено___0_N_, (object) list.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Type == GlobalDictionaries.PaymentTypes.MoneyPayment && x.Date.Date == date.Date)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn))) + Other.NewLine() + "-------------------------" + Other.NewLine() + string.Format(Translate.EmailRepository_GetBodyMail_В_кассе___0_, (object) SaleHelper.GetSumCash()) + Other.NewLine(2) + string.Format(Translate.EmailRepository_GetBodyMail_Работают_сотрудники___0_, (object) string.Join("; ", onlineUsersList.Select<Gbs.Core.Entities.Users.User, string>((Func<Gbs.Core.Entities.Users.User, string>) (x => x.Alias)))) + Other.NewLine(3) + "Report by " + PartnersHelper.ProgramName() + " v." + ApplicationInfo.GetInstance().GbsVersion?.ToString() + "; ID: " + GbsIdHelperMain.GetGbsIdWithPrefix());
      }
    }

    public static string GetReportSale(List<Document> documents)
    {
      try
      {
        IPrintableReport forEmail = new PrintableReportFactory().CreateForEmail((IEnumerable<Document>) documents.OrderByDescending<Document, DateTime>((Func<Document, DateTime>) (x => x.DateTime)));
        DateTime now = DateTime.Now;
        string pathSave = Path.Combine(FileSystemHelper.TempFolderPath(), string.Format("saleReport_{0}-{1}-{2}_{3}-{4}-{5}.pdf", (object) now.Year, (object) now.Month, (object) now.Day, (object) now.Hour, (object) now.Minute, (object) now.Second));
        return new FastReportFacade().SaveReport(forEmail, pathSave) ? pathSave : string.Empty;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось создать файл для отправки на email", false);
        return string.Empty;
      }
    }

    private static string GetPayments(List<Gbs.Core.Entities.Payments.Payment> payments)
    {
      List<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>> list1 = payments.GroupBy<Gbs.Core.Entities.Payments.Payment, PaymentMethods.PaymentMethod>((Func<Gbs.Core.Entities.Payments.Payment, PaymentMethods.PaymentMethod>) (x => x.Method)).ToList<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>();
      List<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>> list2 = list1.Where<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>((Func<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, bool>) (x => x.First<Gbs.Core.Entities.Payments.Payment>().Method.KkmMethod == GlobalDictionaries.KkmPaymentMethods.Certificate)).ToList<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>();
      list2.AddRange((IEnumerable<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>) list1.Where<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>((Func<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, bool>) (x => x.First<Gbs.Core.Entities.Payments.Payment>().Method.DisplayIndex > 0 && x.First<Gbs.Core.Entities.Payments.Payment>().Method.KkmMethod != GlobalDictionaries.KkmPaymentMethods.Certificate)).OrderBy<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, int>((Func<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, int>) (x => x.First<Gbs.Core.Entities.Payments.Payment>().Method.DisplayIndex)).ToList<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>());
      list2.AddRange((IEnumerable<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>) list1.Where<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>>((Func<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, bool>) (x => x.First<Gbs.Core.Entities.Payments.Payment>().Method.DisplayIndex == 0 && x.First<Gbs.Core.Entities.Payments.Payment>().Method.KkmMethod != GlobalDictionaries.KkmPaymentMethods.Certificate)).OrderBy<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, string>((Func<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, string>) (x => x.First<Gbs.Core.Entities.Payments.Payment>().Method.Name)));
      return list2.Aggregate<IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, string>(string.Empty, (Func<string, IGrouping<PaymentMethods.PaymentMethod, Gbs.Core.Entities.Payments.Payment>, string>) ((current, method) => current + string.Format(Translate.EmailRepository_GetPayments__0____1_N__, (object) method.First<Gbs.Core.Entities.Payments.Payment>().Method.Name, (object) payments.Where<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, bool>) (x => x.Method.Uid == method.First<Gbs.Core.Entities.Payments.Payment>().Method.Uid)).Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut))) + Other.NewLine())) + string.Format(Translate.EmailRepository_GetPayments_Всего___0_, (object) payments.Sum<Gbs.Core.Entities.Payments.Payment>((Func<Gbs.Core.Entities.Payments.Payment, Decimal>) (x => x.SumIn - x.SumOut))) + Other.NewLine();
    }

    private bool IsMoneyBox(PaymentsAccounts.PaymentsAccount account)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        int num;
        if (account != null && account.Type == PaymentsAccounts.MoneyType.Cash || account != null && account.Type == PaymentsAccounts.MoneyType.KkmCash)
        {
          ParameterExpression parameterExpression;
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          num = account.Uid == PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>(Expression.Lambda<Func<PAYMENT_METHODS, bool>>((Expression) Expression.Equal(x.SECTION_UID, (Expression) Expression.Property((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Sections.GetCurrentSection)), Array.Empty<Expression>()), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Entity.get_Uid))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), parameterExpression))).First<PaymentMethods.PaymentMethod>().AccountUid ? 1 : 0;
        }
        else
          num = 0;
        return num != 0;
      }
    }

    public int Delete(List<Email> itemsList)
    {
      foreach (Email items in itemsList)
        this.Delete(items);
      return itemsList.Count;
    }

    public bool Delete(Email item)
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          dataBase.GetTable<EMAIL>().Where<EMAIL>((Expression<Func<EMAIL, bool>>) (x => x.UID == item.Uid)).Delete<EMAIL>();
          item.Attach.Where<string>(new Func<string, bool>(File.Exists)).ToList<string>().ForEach(new Action<string>(File.Delete));
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка удаления записи об отчете на почту");
        return false;
      }
    }

    public List<Email> GetActiveItems()
    {
      return this.GetAllItems().Where<Email>((Func<Email, bool>) (x => !x.IsDeleted)).ToList<Email>();
    }

    public List<Email> GetByQuery(IQueryable<EMAIL> query) => throw new NotImplementedException();

    public List<Email> GetAllItems()
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          ParameterExpression parameterExpression;
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          return dataBase.GetTable<EMAIL>().Select<EMAIL, Email>(Expression.Lambda<Func<EMAIL, Email>>((Expression) Expression.MemberInit(Expression.New(typeof (Email)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Email.set_Uid)), )))); // Unable to render the statement
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки списка писем", false, false);
        return new List<Email>();
      }
    }

    public Email GetByUid(Guid uid) => throw new NotImplementedException();

    public bool Save(Email item)
    {
      try
      {
        if (this.Validate(item).Result == ActionResult.Results.Error)
          return false;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          dataBase.InsertOrReplace<EMAIL>(new EMAIL()
          {
            IS_DELETED = item.IsDeleted,
            UID = item.Uid,
            ATTACH = string.Join(";", (IEnumerable<string>) item.Attach),
            BODY = item.Body.ToString(),
            DATE_SEND = item.Date,
            FROM_ADRESS = item.AddressFrom,
            TO_ADRESS = item.AddressTo,
            IS_SEND = item.IsSend,
            SUBJECT = item.Subject
          });
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка сохранения записи об отчете на почту");
        return false;
      }
    }

    public int Save(List<Email> itemsList)
    {
      return itemsList.Count<Email>(new Func<Email, bool>(this.Save));
    }

    public ActionResult Validate(Email item)
    {
      if (item.Subject.Length > 100)
        item.Subject = item.Subject.Remove(100);
      return new ActionResult(ActionResult.Results.Ok);
    }
  }
}
