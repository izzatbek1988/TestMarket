// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.FrmUserCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Users
{
  public class FrmUserCard : WindowWithSize, IComponentConnector
  {
    internal WatermarkPasswordBox PasswordBox;
    private bool _contentLoaded;

    private UserViewModel ModelUser { get; set; }

    public FrmUserCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>();
    }

    public bool ShowCard(Guid userUid, out Gbs.Core.Entities.Users.User user)
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          user = userUid == Guid.Empty ? new Gbs.Core.Entities.Users.User() : new UsersRepository(dataBase).GetByUid(userUid);
        this.ModelUser = new UserViewModel(user)
        {
          CloseWindow = new Action(((Window) this).Close),
          PasswordBox = this.PasswordBox
        };
        this.PasswordBox.Password = user.Password;
        this.DataContext = (object) this.ModelUser;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            this.ModelUser.SaveClient
          },
          {
            hotKeys.CancelAction,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Close()))
          },
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
          }
        };
        this.ShowDialog();
        return this.ModelUser.SaveResult;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме карточки сотрудника");
        user = (Gbs.Core.Entities.Users.User) null;
        return false;
      }
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      ((UserViewModel) this.DataContext).EditClientCard();
    }

    private bool CloseCard()
    {
      return this.ModelUser.HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    private void WatermarkPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      this.ModelUser.User.Password = ((WatermarkPasswordBox) sender).Password;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/users/frmusercard.xaml", UriKind.Relative));
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
      if (connectionId != 1)
      {
        if (connectionId == 2)
        {
          this.PasswordBox = (WatermarkPasswordBox) target;
          this.PasswordBox.PasswordChanged += new RoutedEventHandler(this.WatermarkPasswordBox_OnPasswordChanged);
        }
        else
          this._contentLoaded = true;
      }
      else
        ((UIElement) target).PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.Control_OnMouseDoubleClick);
    }
  }
}
