// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.ReturnListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Forms._shared;
using Gbs.Forms.Sale;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport
{
  public partial class ReturnListViewModel : ViewModelWithForm
  {
    private DateTime _dateFinish;
    private DateTime _dateStart;
    private Guid _sectionSelectedUid;
    private Guid _userSelectedUid;

    public ICommand ShowCard { get; set; }

    public List<ReturnListViewModel.ReturnItem> CashedItems { get; set; }

    public List<ReturnListViewModel.ReturnItem> Items { get; set; }

    public Decimal TotalReturnCount
    {
      get
      {
        List<ReturnListViewModel.ReturnItem> items = this.Items;
        return items == null ? 0M : items.Sum<ReturnListViewModel.ReturnItem>((Func<ReturnListViewModel.ReturnItem, Decimal>) (x => x.Sum));
      }
    }

    public Decimal TotalCountItems
    {
      get
      {
        List<ReturnListViewModel.ReturnItem> items = this.Items;
        return items == null ? 0M : items.Sum<ReturnListViewModel.ReturnItem>((Func<ReturnListViewModel.ReturnItem, Decimal>) (x => x.CountGood));
      }
    }

    public ReturnListViewModel()
    {
      List<Sections.Section> sectionList = new List<Sections.Section>();
      Sections.Section section = new Sections.Section();
      section.Uid = Guid.Empty;
      section.Name = Translate.PaymentsActionListViewModel__selectedSection_Все_секции;
      sectionList.Add(section);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListSections\u003Ek__BackingField = sectionList;
      List<Users.User> userList = new List<Users.User>();
      Users.User user = new Users.User();
      user.Uid = Guid.Empty;
      user.Alias = Translate.SaleJournalViewModel__selectedUser_Все_сотрудники;
      userList.Add(user);
      // ISSUE: reference to a compiler-generated field
      this.\u003CListUsers\u003Ek__BackingField = userList;
      this._sectionSelectedUid = Guid.Empty;
      this._userSelectedUid = Guid.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      using (DataBase dataBase1 = Data.GetDataBase())
      {
        this.ListSections.AddRange(Sections.GetSectionsList().Where<Sections.Section>((Func<Sections.Section, bool>) (x => !x.IsDeleted)));
        this.ListUsers.AddRange((IEnumerable<Users.User>) new UsersRepository(dataBase1).GetActiveItems());
        this.ShowCard = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ReturnListViewModel.ReturnItem> list = ((IEnumerable) obj).Cast<ReturnListViewModel.ReturnItem>().ToList<ReturnListViewModel.ReturnItem>();
          if (!list.Any<ReturnListViewModel.ReturnItem>())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.ReturnListViewModel_Нужно_выбрать_запись_);
          }
          else if (list.Count > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.ReturnListViewModel_Нужно_выбрать_только_одну_запись_);
          }
          else
          {
            using (DataBase dataBase2 = Data.GetDataBase())
              new FrmCardSale().ShowSaleCard(new DocumentsRepository(dataBase2).GetByUid(list.Single<ReturnListViewModel.ReturnItem>().Document.ParentUid));
          }
        }));
      }
    }

    public DateTime DateStart
    {
      get => this._dateStart;
      set
      {
        this._dateStart = value;
        this.GetCachedReturn();
      }
    }

    public DateTime DateFinish
    {
      get => this._dateFinish;
      set
      {
        this._dateFinish = value;
        this.GetCachedReturn();
      }
    }

    private void SearchForFilter()
    {
      List<ReturnListViewModel.ReturnItem> source = new List<ReturnListViewModel.ReturnItem>((IEnumerable<ReturnListViewModel.ReturnItem>) this.CashedItems);
      if (this.SectionSelectedUid != Guid.Empty)
        source = source.Where<ReturnListViewModel.ReturnItem>((Func<ReturnListViewModel.ReturnItem, bool>) (x => x.Document.Section.Uid == this.SectionSelectedUid)).ToList<ReturnListViewModel.ReturnItem>();
      if (this.UserSelectedUid != Guid.Empty)
        source = source.Where<ReturnListViewModel.ReturnItem>((Func<ReturnListViewModel.ReturnItem, bool>) (x => x.Document.UserUid == this.UserSelectedUid)).ToList<ReturnListViewModel.ReturnItem>();
      this.Items = new List<ReturnListViewModel.ReturnItem>((IEnumerable<ReturnListViewModel.ReturnItem>) source.OrderByDescending<ReturnListViewModel.ReturnItem, DateTime>((Func<ReturnListViewModel.ReturnItem, DateTime>) (x => x.Date)));
      this.OnPropertyChanged("Items");
      this.OnPropertyChanged("TotalReturnCount");
    }

    public List<Sections.Section> ListSections { get; set; }

    public List<Users.User> ListUsers { get; set; }

    public Guid SectionSelectedUid
    {
      get => this._sectionSelectedUid;
      set
      {
        this._sectionSelectedUid = value;
        this.OnPropertyChanged(nameof (SectionSelectedUid));
        this.SearchForFilter();
      }
    }

    public Guid UserSelectedUid
    {
      get => this._userSelectedUid;
      set
      {
        this._userSelectedUid = value;
        this.OnPropertyChanged(nameof (UserSelectedUid));
        this.SearchForFilter();
      }
    }

    public void ShowListReturn(DateTime start, DateTime finish)
    {
      this._dateStart = start;
      this._dateFinish = finish;
      this.GetCachedReturn();
      this.FormToSHow = (WindowWithSize) new FrmListReturn();
      this.ShowForm();
    }

    private void GetCachedReturn()
    {
      try
      {
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmListReturn_Загрузка_списка_возвратов);
        using (DataBase dataBase = Data.GetDataBase())
        {
          this.CashedItems = new List<ReturnListViewModel.ReturnItem>(new DocumentsRepository(dataBase).GetItemsWithFilter(this.DateStart, this.DateFinish, GlobalDictionaries.DocumentsTypes.SaleReturn, false).Select<Document, ReturnListViewModel.ReturnItem>((Func<Document, ReturnListViewModel.ReturnItem>) (x => new ReturnListViewModel.ReturnItem(x))));
          this.SearchForFilter();
          progressBar.Close();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при загрузке списка возвратов");
        ProgressBarHelper.Close();
      }
    }

    public class ReturnItem
    {
      public Document Document { get; set; }

      public Document SaleDocument { get; set; }

      public DateTime Date { get; set; }

      public Decimal CountName { get; set; }

      public Decimal CountGood { get; set; }

      public Decimal Sum { get; set; }

      public string Comment { get; set; }

      public Users.User User { get; set; }

      public ReturnItem(Document doc)
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          this.Document = doc;
          this.Date = doc.DateTime;
          this.Comment = doc.Comment;
          this.CountGood = doc.Items.Sum<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, Decimal>) (x => x.Quantity));
          this.CountName = (Decimal) doc.Items.Count;
          this.Sum = SaleHelper.GetSumDocument(doc);
          this.User = new UsersRepository(dataBase).GetByUid(doc.UserUid);
          this.SaleDocument = new DocumentsRepository(dataBase).GetByUid(doc.ParentUid);
        }
      }
    }
  }
}
