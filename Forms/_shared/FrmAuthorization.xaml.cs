// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.AuthorizationViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class AuthorizationViewModel : ViewModelWithForm
  {
    private string _barcode;
    private string _pass;
    private Gbs.Core.Entities.Users.User _selectedUser = new Gbs.Core.Entities.Users.User();

    public ICommand AuthDebugCommand
    {
      get
      {
        return (ICommand) new RelayCommand((System.Action<object>) (obj =>
        {
          if (this.IsLogin && this.SelectedUser != null)
            ProgressBarHelper.AddNotification("pass: " + this.SelectedUser.Password);
          this.Result = true;
          this.CloseWindow();
        }));
      }
    }

    public string HeightActionText => !this.ActionText.IsNullOrEmpty() ? "Auto" : "0";

    public string ActionText
    {
      get
      {
        return GlobalDictionaries.ActionsUserDictionary.SingleOrDefault<KeyValuePair<Actions, string>>((Func<KeyValuePair<Actions, string>, bool>) (x => x.Key == this.Action)).Value ?? "";
      }
    }

    public AuthorizationViewModel()
    {
      using (Data.GetDataBase())
        this.ListUsers = CachesBox.AllUsers().Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => !x.IsDeleted && !x.IsKicked)).OrderBy<Gbs.Core.Entities.Users.User, DateTime>((Func<Gbs.Core.Entities.Users.User, DateTime>) (x => x.DateIn)).ToList<Gbs.Core.Entities.Users.User>();
    }

    public WatermarkPasswordBox PasswordBox { private get; set; }

    public bool IsLogin { private get; set; }

    public bool Result { get; private set; }

    public bool ShowExpandedPass { get; set; } = true;

    public string Password
    {
      private get => this._pass;
      set
      {
        this._pass = value;
        this.PassCheck();
      }
    }

    public string Barcode
    {
      get => this._barcode;
      set
      {
        this._barcode = value;
        this.BarcodeCheck();
      }
    }

    public void DoAuthorization()
    {
      LogHelper.Debug("Начинаю авторизацию пользователя");
      switch (new ConfigsRepository<Settings>().Get().Users.DefaultAuthorizationMethod)
      {
        case Gbs.Core.Config.Users.AuthorizationMethods.LoginPassword:
          this.ShowExpandedPass = true;
          break;
        case Gbs.Core.Config.Users.AuthorizationMethods.Barcode:
          this.ShowExpandedPass = false;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      if (this.ListUsers.Count == 1)
        this.SelectedUser = this.ListUsers.First<Gbs.Core.Entities.Users.User>();
      this.PassCheckCommand = (ICommand) new RelayCommand((System.Action<object>) (obj => this.PassCheck(true)));
      this.BarcodeCheckCommand = (ICommand) new RelayCommand((System.Action<object>) (obj => this.BarcodeCheck(true)));
    }

    private void PassCheck(bool enter = false)
    {
      if (this.SelectedUser == null)
        return;
      if (this.SelectedUser.Password == this.Password)
      {
        if (this.IsLogin)
        {
          this.Result = true;
          this.CloseWindow();
        }
        else
          this.GetAccess(this.SelectedUser);
      }
      if (!enter || this.Result)
        return;
      MessageBoxHelper.Warning(Translate.AuthorizationViewModel_Не_правильный_пароль_);
      LogHelper.Debug("Введен неверный пароль");
    }

    private void BarcodeCheck(bool enter = false)
    {
      if (this.Barcode.IsNullOrEmpty())
        return;
      string clearBarcode = new string(this.Barcode.Where<char>((Func<char, bool>) (c => char.IsLetterOrDigit(c))).ToArray<char>());
      if (clearBarcode.Length > 3 && this.ListUsers.Any<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Barcode == clearBarcode)))
      {
        if (this.IsLogin)
        {
          this.Result = true;
          this.SelectedUser = this.ListUsers.First<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Barcode == clearBarcode));
          this.CloseWindow();
        }
        else
        {
          this.SelectedUser = this.ListUsers.First<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Barcode == clearBarcode));
          this.GetAccess(this.ListUsers.First<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.Barcode == clearBarcode)));
        }
      }
      if (!enter || this.Result)
        return;
      MessageBoxHelper.Warning(Translate.AuthorizationViewModel_Не_правильный_штрих_код_);
      LogHelper.Debug("По штрих-коду не найден пользователь");
    }

    private void GetAccess(Gbs.Core.Entities.Users.User user)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        this.Result = new UsersRepository(dataBase).GetAccess(user, this.Action);
        if (!this.Result)
        {
          MessageBoxHelper.Warning(Translate.AuthorizationViewModel_У_вас_недостаточно_прав_для_выполнения_действия_, PartnersHelper.ProgramName());
          LogHelper.Debug("Недостаточно прав для выполнения операции");
        }
        this.CloseWindow();
      }
    }

    public List<Gbs.Core.Entities.Users.User> ListUsers { get; set; } = new List<Gbs.Core.Entities.Users.User>();

    public Gbs.Core.Entities.Users.User SelectedUser
    {
      get => this._selectedUser;
      set
      {
        this._selectedUser = value;
        this.PasswordBox?.Focus();
      }
    }

    public Actions Action { private get; set; }

    public ICommand PassCheckCommand { get; set; }

    public ICommand BarcodeCheckCommand { get; set; }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((System.Action<object>) (obj => this.CloseWindow()));
    }

    public System.Action CloseWindow { private get; set; }
  }
}
