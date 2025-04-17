// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Sections
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Db.Payments;
using Gbs.Helpers;
using Gbs.Helpers.HomeOffice.Entity;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class Sections
  {
    private static Sections.Section _currentSection;

    public static List<Sections.Section> GetSectionsList(IQueryable<SECTIONS> query = null)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<SECTIONS>();
        return query.ToList<SECTIONS>().Select<SECTIONS, Sections.Section>((Func<SECTIONS, Sections.Section>) (s =>
        {
          return new Sections.Section()
          {
            Uid = s.UID,
            IsDeleted = s.IS_DELETED,
            Name = s.NAME,
            GbsId = s.GBS_ID
          };
        })).ToList<Sections.Section>();
      }
    }

    public static Sections.Section GetSectionByUid(Guid uid)
    {
      Other.ConsoleWrite("Загрузка секции по УИД");
      using (DataBase dataBase = Data.GetDataBase())
        return Sections.GetSectionsList(dataBase.GetTable<SECTIONS>().Where<SECTIONS>((Expression<Func<SECTIONS, bool>>) (g => g.UID == uid))).SingleOrDefault<Sections.Section>();
    }

    public static Sections.Section GetCurrentSection()
    {
      if (Sections._currentSection != null)
        return Sections._currentSection;
      Sections.LoadCurrentSection();
      return Sections._currentSection;
    }

    public static void LoadCurrentSection()
    {
      string gbsId = GbsIdHelperMain.GetGbsId();
      List<Sections.Section> sectionsList = Sections.GetSectionsList();
      List<Sections.Section> list = sectionsList.Where<Sections.Section>((Func<Sections.Section, bool>) (x => string.Equals(x.GbsId, gbsId, StringComparison.CurrentCultureIgnoreCase))).ToList<Sections.Section>();
      if (!list.Any<Sections.Section>())
      {
        LogHelper.Debug("Секция с ID: " + gbsId + " не найдена в БД, будет создана новая");
        int count = sectionsList.Count;
        Sections.Section section = new Sections.Section();
        section.GbsId = gbsId;
        section.IsDeleted = false;
        section.Name = Translate.SplashScreenViewModel_ + (count + 1).ToString();
        section.Save();
        Sections._currentSection = section;
      }
      else
      {
        Sections.Section section1 = list.FirstOrDefault<Sections.Section>((Func<Sections.Section, bool>) (x => !x.IsDeleted));
        if (section1 != null)
        {
          Sections._currentSection = section1;
        }
        else
        {
          Sections.Section section2 = list.FirstOrDefault<Sections.Section>((Func<Sections.Section, bool>) (x => x.IsDeleted));
          section2.IsDeleted = false;
          section2.Save();
          Sections._currentSection = section2;
        }
      }
    }

    public class Section : Gbs.Core.Entities.Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Name { get; set; }

      [Required]
      [StringLength(32, MinimumLength = 32)]
      public string GbsId { get; set; }

      public List<PaymentMethods.PaymentMethod> Methods { get; set; }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public void Save()
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
            return;
          if (!DevelopersHelper.IsUnitTest())
            new HomeOfficeHelper().PrepareAndSend<Sections.Section>(this, HomeOfficeHelper.EntityEditHome.Sections);
          dataBase.InsertOrReplace<SECTIONS>(new SECTIONS()
          {
            UID = this.Uid,
            IS_DELETED = this.IsDeleted,
            NAME = this.Name,
            GBS_ID = this.GbsId
          });
          if (PaymentMethods.GetActionPaymentsList(dataBase.GetTable<PAYMENT_METHODS>().Where<PAYMENT_METHODS>((Expression<Func<PAYMENT_METHODS, bool>>) (x => x.SECTION_UID == this.Uid))).FirstOrDefault<PaymentMethods.PaymentMethod>() != null)
            return;
          PaymentsAccounts.PaymentsAccount paymentsAccount = new PaymentsAccounts.PaymentsAccount()
          {
            Type = PaymentsAccounts.MoneyType.KkmCash,
            Name = string.Format(Translate.Section_Денежный_ящик_для_секции___0_, (object) this.Name)
          };
          paymentsAccount.Save();
          new PaymentMethods.PaymentMethod()
          {
            Name = Translate.Section_Наличными,
            SectionUid = this.Uid,
            AccountUid = paymentsAccount.Uid
          }.Save();
        }
      }

      public Section()
      {
      }

      public Section(SectionHome sectionHome)
      {
        this.Uid = sectionHome.Uid;
        this.Name = sectionHome.Name;
        this.GbsId = sectionHome.GbsId;
        this.IsDeleted = sectionHome.IsDeleted;
      }
    }
  }
}
