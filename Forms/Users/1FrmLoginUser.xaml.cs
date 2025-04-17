// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.FrmLoginUser
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Users
{
  public class FrmLoginUser : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid ListUsersAuth;
    internal Button ButtonInOut;
    internal Button ButtonMore;
    internal ConfirmPanelControl1 ConfirmPanelControl;
    private bool _contentLoaded;

    private LoginUsersViewModel Model { get; set; }

    public FrmLoginUser() => this.InitializeComponent();

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand = new RelayCommand((Action<object>) (o => this.Model.Close()));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
          },
          {
            hotKeys.OkAction,
            this.Model.DoneCommand
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public bool ShowLogin(out Gbs.Core.Entities.Users.User user, bool isClose = false)
    {
      try
      {
        CacheHelper.Clear(CacheHelper.CacheTypes.AllUsers);
        this.Model = new LoginUsersViewModel(isClose)
        {
          Close = new Action(((Window) this).Close),
          ShowMenu = new Action(this.ShowMoreMenu)
        };
        this.SetHotKeys();
        TooltipsSetter.Set(this);
        this.DataContext = (object) this.Model;
        LoginUsersViewModel.IconExit = this.FindResource((object) "Exit");
        LoginUsersViewModel.IconEnter = this.FindResource((object) "Enter");
        if (this.Model.UserList.Any<LoginUsersViewModel.UsersLogin>())
          this.ShowDialog();
        else
          this.Close();
        CacheHelper.Clear(CacheHelper.CacheTypes.AllUsers);
        CachesBox.AllUsers();
        Sections.Section currSection = Sections.GetCurrentSection();
        user = this.Model.UserList.Count<LoginUsersViewModel.UsersLogin>((Func<LoginUsersViewModel.UsersLogin, bool>) (x => x.User.OnlineOnSectionUid == currSection.Uid)) == 1 ? this.Model.UserList.Single<LoginUsersViewModel.UsersLogin>((Func<LoginUsersViewModel.UsersLogin, bool>) (x => x.User.OnlineOnSectionUid == currSection.Uid)).User : (Gbs.Core.Entities.Users.User) null;
        return this.Model.UserList.Any<LoginUsersViewModel.UsersLogin>((Func<LoginUsersViewModel.UsersLogin, bool>) (x => x.User.OnlineOnSectionUid != Guid.Empty));
      }
      catch (Exception ex)
      {
        user = (Gbs.Core.Entities.Users.User) null;
        LogHelper.Error(ex, "Ошибка в форме входа");
        return false;
      }
    }

    private void FrmLoginUser_OnClosing(object sender, CancelEventArgs e)
    {
      if (this.Model == null || this.Model.UserList.Any<LoginUsersViewModel.UsersLogin>((Func<LoginUsersViewModel.UsersLogin, bool>) (x => x.User.OnlineOnSectionUid == Sections.GetCurrentSection().Uid)))
        return;
      int num = (int) MessageBoxHelper.Show(Translate.LoginUsersViewModel_Необходимо_авторизоваться_хотя_бы_одному_пользователю);
      e.Cancel = true;
    }

    private void FrmLoginUser_OnLoaded(object sender, RoutedEventArgs e)
    {
      Gbs.Helpers.ControlsHelpers.DataGrid.Other.SelectFirstRow((object) this.ListUsersAuth);
      this.Activate();
      this.Focus();
    }

    private void ShowMoreMenu()
    {
      if (!(this.FindResource((object) LoginUsersViewModel.AlsoMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) this.ButtonMore;
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/users/frmloginuser.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ListUsersAuth = (System.Windows.Controls.DataGrid) target;
          break;
        case 2:
          this.ButtonInOut = (Button) target;
          break;
        case 3:
          this.ButtonMore = (Button) target;
          break;
        case 4:
          this.ConfirmPanelControl = (ConfirmPanelControl1) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
