// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Sections.SectionListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Documents;
using Gbs.Core.Db.Payments;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Licenses.GbsIdHelper;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Sections
{
  public partial class SectionListViewModel : ViewModelWithForm
  {
    public ObservableCollection<SectionListViewModel.SectionView> SectionList { get; set; }

    public SectionListViewModel.SectionView SelectedSection { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    public ICommand JoinCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<SectionListViewModel.SectionView> list1 = ((IEnumerable) obj).Cast<SectionListViewModel.SectionView>().ToList<SectionListViewModel.SectionView>();
          if (list1.Count < 2)
          {
            int num = (int) MessageBoxHelper.Show(Translate.SectionListViewModel_JoinCommand_Для_объединения_необходимо_выбрать_2_или_более_секции_, icon: MessageBoxImage.Exclamation);
          }
          else
          {
            Gbs.Core.Entities.Sections.Section currentSection = Gbs.Core.Entities.Sections.GetCurrentSection();
            if (!list1.Any<SectionListViewModel.SectionView>((Func<SectionListViewModel.SectionView, bool>) (x => x.Section.Uid == currentSection.Uid)))
            {
              MessageBoxHelper.Warning(string.Format(Translate.SectionListViewModel_JoinCommand_Объединить_можно_только_текущую_секцию_с_другими__Для_объедения_выберите_секцию__0_, (object) currentSection.Name));
            }
            else
            {
              MainJoinViewModel.MainItem mainItem = new MainJoinViewModel().SelectEntity(list1.Select<SectionListViewModel.SectionView, MainJoinViewModel.MainItem>((Func<SectionListViewModel.SectionView, MainJoinViewModel.MainItem>) (x => new MainJoinViewModel.MainItem()
              {
                Name = x.Section.Name,
                Uid = x.Section.Uid
              })));
              MainJoinViewModel.MainItem mainItem1 = mainItem;
              if ((mainItem1 != null ? mainItem1.Uid : Guid.Empty) == Guid.Empty || MessageBoxHelper.Show(string.Format(Translate.SectionListViewModel_JoinCommand_, (object) list1.Count, (object) mainItem.Name), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
                return;
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SectionListViewModel_JoinCommand_Объединение_секций);
              SectionListViewModel.SectionView mainSection = list1.Single<SectionListViewModel.SectionView>((Func<SectionListViewModel.SectionView, bool>) (x => x.Section.Uid == mainItem.Uid));
              IEnumerable<SectionListViewModel.SectionView> sectionViews = list1.Where<SectionListViewModel.SectionView>((Func<SectionListViewModel.SectionView, bool>) (x => x.Section.Uid != mainSection.Section.Uid));
              using (DataBase dataBase = Data.GetDataBase())
              {
                List<PaymentMethods.PaymentMethod> actionPaymentsList = PaymentMethods.GetActionPaymentsList();
                List<PaymentsAccounts.PaymentsAccount> paymentsAccountsList = PaymentsAccounts.GetPaymentsAccountsList();
                PaymentMethods.PaymentMethod methodMain = actionPaymentsList.First<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.SectionUid == mainSection.Section.Uid));
                methodMain.IsDeleted = false;
                methodMain.Save();
                PaymentsAccounts.PaymentsAccount paymentsAccount1 = paymentsAccountsList.First<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => !x.IsDeleted && x.Uid == methodMain.AccountUid));
                List<Users.User> allItems = new UsersRepository(dataBase).GetAllItems();
                foreach (SectionListViewModel.SectionView sectionView in sectionViews)
                {
                  SectionListViewModel.SectionView section = sectionView;
                  section.Section.IsDeleted = true;
                  List<DOCUMENTS> list2 = dataBase.GetTable<DOCUMENTS>().Where<DOCUMENTS>((Expression<Func<DOCUMENTS, bool>>) (x => x.SECTION_UID == section.Section.Uid)).ToList<DOCUMENTS>();
                  List<PAYMENTS> list3 = dataBase.GetTable<PAYMENTS>().Where<PAYMENTS>((Expression<Func<PAYMENTS, bool>>) (x => x.SECTION_UID == section.Section.Uid)).ToList<PAYMENTS>();
                  foreach (DOCUMENTS documents in list2)
                  {
                    documents.SECTION_UID = mainSection.Section.Uid;
                    dataBase.InsertOrReplace<DOCUMENTS>(documents);
                  }
                  foreach (Users.User user in allItems.Where<Users.User>((Func<Users.User, bool>) (x => x.OnlineOnSectionUid == section.Section.Uid)))
                  {
                    user.OnlineOnSectionUid = mainSection.Section.Uid;
                    new UsersRepository(dataBase).Save(user);
                  }
                  List<PaymentMethods.PaymentMethod> methods = actionPaymentsList.Where<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.SectionUid == section.Section.Uid)).ToList<PaymentMethods.PaymentMethod>();
                  foreach (PAYMENTS payments in list3)
                  {
                    PAYMENTS payment = payments;
                    payment.SECTION_UID = mainSection.Section.Uid;
                    if (methods.Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.Uid == payment.METHOD_UID)))
                      payment.METHOD_UID = methodMain.Uid;
                    dataBase.InsertOrReplace<PAYMENTS>(payment);
                  }
                  foreach (PaymentsAccounts.PaymentsAccount paymentsAccount2 in paymentsAccountsList.Where<PaymentsAccounts.PaymentsAccount>((Func<PaymentsAccounts.PaymentsAccount, bool>) (x => methods.Any<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (m => m.AccountUid == x.Uid)))))
                  {
                    PaymentsAccounts.PaymentsAccount account = paymentsAccount2;
                    account.IsDeleted = true;
                    IQueryable<PAYMENTS> table = dataBase.GetTable<PAYMENTS>();
                    Expression<Func<PAYMENTS, bool>> predicate = (Expression<Func<PAYMENTS, bool>>) (x => x.ACCOUNT_IN_UID == account.Uid || x.ACCOUNT_OUT_UID == account.Uid);
                    foreach (PAYMENTS payments in table.Where<PAYMENTS>(predicate).ToList<PAYMENTS>())
                    {
                      payments.ACCOUNT_IN_UID = payments.ACCOUNT_IN_UID == account.Uid ? paymentsAccount1.Uid : payments.ACCOUNT_IN_UID;
                      payments.ACCOUNT_OUT_UID = payments.ACCOUNT_OUT_UID == account.Uid ? paymentsAccount1.Uid : payments.ACCOUNT_OUT_UID;
                      dataBase.InsertOrReplace<PAYMENTS>(payments);
                    }
                    account.Save();
                  }
                  section.Section.GbsId = section.Section.GbsId.Remove(0, 2);
                  section.Section.Save();
                  this.SectionList.Remove(section);
                  foreach (PaymentMethods.PaymentMethod paymentMethod in methods)
                  {
                    paymentMethod.IsDeleted = true;
                    paymentMethod.Save();
                  }
                }
                mainSection.Section.GbsId = GbsIdHelperMain.GetGbsId();
                mainSection.Section.Save();
                this.SectionList[this.SectionList.ToList<SectionListViewModel.SectionView>().FindIndex((Predicate<SectionListViewModel.SectionView>) (x => x.Section.Uid == mainSection.Section.Uid))] = mainSection;
                Gbs.Core.Entities.Sections.LoadCurrentSection();
                this.SectionList = new ObservableCollection<SectionListViewModel.SectionView>((IEnumerable<SectionListViewModel.SectionView>) this.SectionList);
                this.OnPropertyChanged("SectionList");
                progressBar.Close();
                CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.AllUsers);
              }
            }
          }
        }));
      }
    }

    public SectionListViewModel()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.SectionListViewModel_SectionListViewModel_Загрузка_списка_секций);
      try
      {
        List<Gbs.Core.Entities.Sections.Section> list1 = new ObservableCollection<Gbs.Core.Entities.Sections.Section>(Gbs.Core.Entities.Sections.GetSectionsList().Where<Gbs.Core.Entities.Sections.Section>((Func<Gbs.Core.Entities.Sections.Section, bool>) (obj => !obj.IsDeleted))).ToList<Gbs.Core.Entities.Sections.Section>();
        string gbsId = "6:" + LicenseHelper.GetInfo().GbsId;
        list1.ForEach((Action<Gbs.Core.Entities.Sections.Section>) (x => x.GbsId = "6:" + x.GbsId));
        using (DataBase dataBase = Data.GetDataBase())
        {
          IQueryable<DOCUMENTS> documentsEdit = dataBase.GetTable<DOCUMENTS>();
          this.SectionList = new ObservableCollection<SectionListViewModel.SectionView>(list1.Select<Gbs.Core.Entities.Sections.Section, SectionListViewModel.SectionView>((Func<Gbs.Core.Entities.Sections.Section, SectionListViewModel.SectionView>) (x =>
          {
            SectionListViewModel.SectionView sectionView1 = new SectionListViewModel.SectionView();
            sectionView1.IsCurrentSection = x.GbsId == gbsId;
            sectionView1.Section = x;
            SectionListViewModel.SectionView sectionView2 = sectionView1;
            string str;
            if (!(x.GbsId == gbsId))
              str = documentsEdit.OrderBy<DOCUMENTS, DateTime>((Expression<Func<DOCUMENTS, DateTime>>) (d => d.DATE_TIME)).ToList<DOCUMENTS>().LastOrDefault<DOCUMENTS>((Func<DOCUMENTS, bool>) (d => d.SECTION_UID == x.Uid))?.DATE_TIME.ToString("dd.MM.yyy HH:mm") ?? (string) null;
            else
              str = Translate.SectionListViewModel_SectionListViewModel_текущая_секция;
            sectionView2.DateTimeLast = str;
            return sectionView1;
          })));
          this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
          {
            List<SectionListViewModel.SectionView> list2 = ((IEnumerable) obj).Cast<SectionListViewModel.SectionView>().ToList<SectionListViewModel.SectionView>();
            if (!list2.Any<SectionListViewModel.SectionView>())
            {
              int num1 = (int) MessageBoxHelper.Show(Translate.SectionListViewModel_Необходимо_выбрать_секцию);
            }
            else if (list2.Count<SectionListViewModel.SectionView>() > 1)
            {
              int num2 = (int) MessageBoxHelper.Show(Translate.SectionListViewModel_Нужно_выбрать_только_одну_секцию_для_редактирования);
            }
            else
            {
              Gbs.Core.Entities.Sections.Section section;
              if (!new FrmSectionCard().ShowCard(this.SelectedSection.Section.Uid, out section))
                return;
              this.SectionList[this.SectionList.ToList<SectionListViewModel.SectionView>().FindIndex((Predicate<SectionListViewModel.SectionView>) (x => x.Section.Uid == section.Uid))].Section = section;
              this.SectionList = new ObservableCollection<SectionListViewModel.SectionView>((IEnumerable<SectionListViewModel.SectionView>) this.SectionList);
              this.OnPropertyChanged(nameof (SectionList));
            }
          }));
          this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
          {
            List<SectionListViewModel.SectionView> list3 = ((IEnumerable) obj).Cast<SectionListViewModel.SectionView>().ToList<SectionListViewModel.SectionView>();
            if (!list3.Any<SectionListViewModel.SectionView>())
            {
              MessageBoxHelper.Warning(Translate.SectionListViewModel_Необходимо_выбрать_секцию);
            }
            else
            {
              if (MessageBoxHelper.Show(string.Format(Translate.SectionListViewModel_Вы_уверены__что_хотите_удалить__0__1_, (object) list3.Count, list3.Count == 1 ? (object) Translate.SectionListViewModel__секцию : (list3.Count > 4 ? (object) Translate.SectionListViewModel__секций : (object) Translate.SectionListViewModel__секции)), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
                return;
              foreach (SectionListViewModel.SectionView sectionView in list3)
              {
                if (sectionView.Section.Uid == Gbs.Core.Entities.Sections.GetCurrentSection().Uid)
                {
                  MessageBoxHelper.Warning(string.Format(Translate.SectionListViewModel_Невозможно_удалить_секцию__0___т_к__она_является_основной_на_данном_устройстве, (object) sectionView.Section.Name));
                }
                else
                {
                  sectionView.Section.GbsId = sectionView.Section.GbsId.Remove(0, 2);
                  sectionView.Section.IsDeleted = true;
                  sectionView.Section.Save();
                  this.SectionList.Remove(sectionView);
                }
              }
            }
          }));
          progressBar.Close();
        }
      }
      catch (Exception ex)
      {
        progressBar.Close();
        LogHelper.Error(ex, "Ошибка в форме списка секций");
      }
    }

    public class SectionView
    {
      public string FontWeight => !this.IsCurrentSection ? "Normal" : "Bold";

      public string DisplayName => this.Section.Name;

      public bool IsCurrentSection { get; set; }

      public Gbs.Core.Entities.Sections.Section Section { get; set; }

      public string DateTimeLast { get; set; }
    }
  }
}
