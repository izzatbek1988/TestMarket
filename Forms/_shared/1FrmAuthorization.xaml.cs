// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.Authorization
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Cache;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Tooltips;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms._shared
{
  public class Authorization : WindowWithSize, IComponentConnector
  {
    internal ComboBox ComboBoxUsers;
    internal WatermarkPasswordBox PassBox;
    internal TextBox BarcodeTb;
    private bool _contentLoaded;

    private AuthorizationViewModel MyModel { get; set; }

    public Authorization()
    {
      this.InitializeComponent();
      TooltipsSetter.Set(this);
    }

    public (bool Result, Gbs.Core.Entities.Users.User User) GetAccess(Actions actions)
    {
      try
      {
        if (!HomeOfficeHelper.IsAuthRequire && new ConfigsRepository<DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
        {
          Gbs.Core.Entities.Users.User user = new Gbs.Core.Entities.Users.User();
          user.Uid = Guid.Empty;
          return (true, user);
        }
        List<Gbs.Core.Entities.Users.User> source = CachesBox.AllUsers();
        Gbs.Core.Config.Users users = new ConfigsRepository<Settings>().Get().Users;
        List<Gbs.Core.Entities.Users.User> list = source.Where<Gbs.Core.Entities.Users.User>((Func<Gbs.Core.Entities.Users.User, bool>) (x => x.OnlineOnSectionUid == Sections.GetCurrentSection().Uid)).ToList<Gbs.Core.Entities.Users.User>();
        if (users.NotRequestAuthorizationForSingleOnlineUser && list.Count == 1 && new UsersRepository().GetAccess(list.Single<Gbs.Core.Entities.Users.User>(), actions))
          return (true, list.Single<Gbs.Core.Entities.Users.User>());
        this.MyModel = new AuthorizationViewModel()
        {
          CloseWindow = new Action(((Window) this).Close),
          Action = actions,
          PasswordBox = this.PassBox
        };
        this.MyModel.DoAuthorization();
        this.DataContext = (object) this.MyModel;
        this.ShowDialog();
        this.Close();
        return (this.MyModel.Result, this.MyModel.SelectedUser);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка получения доступа");
        return (false, (Gbs.Core.Entities.Users.User) null);
      }
    }

    public bool LoginUser(ref Gbs.Core.Entities.Users.User user)
    {
      try
      {
        this.MyModel = new AuthorizationViewModel()
        {
          CloseWindow = new Action(((Window) this).Close),
          IsLogin = true,
          SelectedUser = user,
          PasswordBox = this.PassBox
        };
        this.MyModel.DoAuthorization();
        this.DataContext = (object) this.MyModel;
        this.ShowDialog();
        user = this.MyModel.SelectedUser;
        this.Close();
        return this.MyModel.Result;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка входа пользователя");
        return false;
      }
    }

    private void WatermarkPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      this.MyModel.Password = ((WatermarkPasswordBox) sender).Password;
    }

    private void Authorization_OnLoaded(object sender, RoutedEventArgs e)
    {
      switch (new ConfigsRepository<Settings>().Get().Users.DefaultAuthorizationMethod)
      {
        case Gbs.Core.Config.Users.AuthorizationMethods.LoginPassword:
          this.PassBox.Focus();
          break;
        case Gbs.Core.Config.Users.AuthorizationMethods.Barcode:
          this.BarcodeTb.Focus();
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmauthorization.xaml", UriKind.Relative));
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
          this.ComboBoxUsers = (ComboBox) target;
          break;
        case 2:
          this.PassBox = (WatermarkPasswordBox) target;
          this.PassBox.PasswordChanged += new RoutedEventHandler(this.WatermarkPasswordBox_OnPasswordChanged);
          break;
        case 3:
          this.BarcodeTb = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
