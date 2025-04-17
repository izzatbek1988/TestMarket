// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Reports.SummaryReport.Other.UsersStatisticViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Gbs.Forms.Reports.SummaryReport.Other
{
  public partial class UsersStatisticViewModel : ViewModelWithForm
  {
    private DateTime _valueDateTimeEnd = DateTime.Now.Date;
    private DateTime _valueDateTimeStart = DateTime.Now.Date;

    public List<UsersStatisticViewModel.UserInfo> Users { get; set; } = new List<UsersStatisticViewModel.UserInfo>();

    public DateTime ValueDateTimeStart
    {
      get => this._valueDateTimeStart;
      set
      {
        this._valueDateTimeStart = value;
        this.GetStatisticByUser();
      }
    }

    public DateTime ValueDateTimeEnd
    {
      get => this._valueDateTimeEnd;
      set
      {
        this._valueDateTimeEnd = value;
        this.GetStatisticByUser();
      }
    }

    private void GetStatisticByUser()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IEnumerable<Document> documents = new DocumentsRepository(dataBase).GetAllItems().Where<Document>((Func<Document, bool>) (x => !x.IsDeleted && x.Type == GlobalDictionaries.DocumentsTypes.Sale && x.DateTime.Date >= this.ValueDateTimeStart && x.DateTime.Date <= this.ValueDateTimeEnd));
        this.Users = new UsersRepository(dataBase).GetByQuery(dataBase.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => !x.IS_DELETED))).Select<Gbs.Core.Entities.Users.User, UsersStatisticViewModel.UserInfo>((Func<Gbs.Core.Entities.Users.User, UsersStatisticViewModel.UserInfo>) (x => new UsersStatisticViewModel.UserInfo()
        {
          User = x
        })).ToList<UsersStatisticViewModel.UserInfo>();
        foreach (Document document1 in documents)
        {
          Document document = document1;
          int index = this.Users.FindIndex((Predicate<UsersStatisticViewModel.UserInfo>) (x => x.User.Uid == document.UserUid));
          if (index == -1)
            return;
          ++this.Users[index].SaleCount;
          this.Users[index].GoodTotal += document.Items.Sum<Item>((Func<Item, Decimal>) (x => x.Quantity));
          this.Users[index].SaleSum += document.Items.Sum<Item>((Func<Item, Decimal>) (x => (x.SellPrice - x.SellPrice * x.Discount / 100M) * x.Quantity));
        }
        this.Users = this.Users.Where<UsersStatisticViewModel.UserInfo>((Func<UsersStatisticViewModel.UserInfo, bool>) (x => x.SaleCount > 0)).ToList<UsersStatisticViewModel.UserInfo>();
        this.OnPropertyChanged("Users");
      }
    }

    public class UserInfo
    {
      public Gbs.Core.Entities.Users.User User { get; set; }

      public int SaleCount { get; set; }

      public Decimal GoodTotal { get; set; }

      public Decimal SaleSum { get; set; }
    }
  }
}
