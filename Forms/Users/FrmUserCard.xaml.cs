// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.UserViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Users;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms.Clients;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Users
{
  public partial class UserViewModel : ViewModelWithForm, ICheckChangesViewModel
  {
    private readonly Gbs.Core.Config.Devices _devicesConfig = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
    private Gbs.Core.Entities.Users.User _user;
    public WatermarkPasswordBox PasswordBox;

    public Visibility VisibilityAuthForKkm
    {
      get
      {
        return !this._devicesConfig.CheckPrinter.FiscalKkm.KkmType.IsEither<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.HiPos, GlobalDictionaries.Devices.FiscalKkmTypes.Hdm, GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas) || this._devicesConfig.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public bool SaveResult { get; private set; }

    public Gbs.Core.Entities.Users.User User
    {
      get => this._user;
      set
      {
        this._user = value;
        this.OnPropertyChanged(nameof (User));
      }
    }

    public bool IsKicked
    {
      get
      {
        Gbs.Core.Entities.Users.User user = this.User;
        return user != null && user.IsKicked;
      }
      set
      {
        this.User.IsKicked = value;
        this.User.DateOut = !value ? new DateTime?() : new DateTime?(DateTime.Now);
        this.OnPropertyChanged("User");
      }
    }

    public List<UserGroups.UserGroup> ListGroups { get; set; } = UserGroups.GetListUserGroups().Where<UserGroups.UserGroup>((Func<UserGroups.UserGroup, bool>) (x => !x.IsDeleted)).ToList<UserGroups.UserGroup>();

    public ICommand GetClient { get; set; }

    public ICommand GeneratedBarcode { get; set; }

    public ICommand GeneratedPass { get; set; }

    public ICommand SaveClient { get; set; }

    public Action CloseWindow { private get; set; }

    public UserViewModel()
    {
    }

    public UserViewModel(Gbs.Core.Entities.Users.User user)
    {
      UserViewModel userViewModel = this;
      this.User = user;
      UserGroups.UserGroup firstGroup = user.Group.Clone<UserGroups.UserGroup>() ?? new UserGroups.UserGroup();
      if (this.ListGroups.Count == 1)
        this.User.Group = this.ListGroups.First<UserGroups.UserGroup>();
      this.EntityClone = (IEntity) this.User.Clone();
      this.GetClient = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (Client client, bool result) client = new FrmSearchClient().GetClient();
        if (!client.result)
          return;
        userViewModel.User.Client = client.client;
        if (userViewModel.User.Alias.IsNullOrEmpty())
          userViewModel.User.Alias = client.client.Name;
        userViewModel.OnPropertyChanged(nameof (User));
      }));
      this.GeneratedBarcode = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        string[] strArray = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.Users.Split(GlobalDictionaries.SplitArr);
        string prefix = "";
        if (strArray.Length != 0)
          prefix = strArray[0];
        userViewModel.User.Barcode = BarcodeHelper.RandomBarcode(prefix);
        userViewModel.OnPropertyChanged(nameof (User));
      }));
      this.GeneratedPass = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        userViewModel.User.Password = BarcodeHelper.RandomPass();
        int num = (int) MessageBoxHelper.Show(string.Format(Translate.UserViewModel_, (object) userViewModel.User.Alias, (object) user.Password));
        userViewModel.PasswordBox.Password = userViewModel.User.Password;
        userViewModel.OnPropertyChanged(nameof (User));
        userViewModel.OnPropertyChanged("Password");
      }));
      this.SaveClient = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (new UsersRepository(dataBase).GetByQuery(dataBase.GetTable<USERS>().Where<USERS>((Expression<Func<USERS, bool>>) (x => !x.IS_DELETED && !x.IS_KICKED))).ToList<Gbs.Core.Entities.Users.User>().Count<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Group.IsSuper)) == 1 && firstGroup.IsSuper && (!closure_0.User.Group.IsSuper || closure_0.User.IsKicked))
          {
            int num = (int) MessageBoxHelper.Show(Translate.UserViewModel_При_выполнении_данного_действия__не_останется_ни_одного_пользователя_с_правами_СуперАдминистратора, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
          }
          else
          {
            if (closure_0.User.OnlineOnSectionUid != Guid.Empty && closure_0.User.IsKicked && MessageBoxHelper.Show(Translate.UserViewModel_Данный_сотрудник_сейчас_находится__Онлайн__в_программе__выйти_и_уволить_пользователя_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.No || new UsersRepository(dataBase).Validate(closure_0.User).Result == ActionResult.Results.Error)
              return;
            closure_0.SaveResult = new UsersRepository(dataBase).Save(closure_0.User);
            if (!closure_0.SaveResult)
              return;
            WindowWithSize.IsCancel = false;
            closure_0.CloseWindow();
          }
        }
      }));
    }

    public void EditClientCard()
    {
      if (this.User.Client == null)
      {
        this.GetClient.Execute((object) null);
      }
      else
      {
        ClientAdnSum client;
        if (!new FrmClientCard().ShowCard(this.User.Client.Uid, out client, action: Actions.ClientsEdit))
          return;
        this.User.Client = client.Client;
        this.OnPropertyChanged("User");
      }
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.User);
    }
  }
}
