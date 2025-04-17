// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Users.UsersGroup.FrmCardUserGroup
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Users.UsersGroup
{
  public class FrmCardUserGroup : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    internal ItemsControl tStack;
    private bool _contentLoaded;

    private UserGroupViewModel GroupModel { get; set; }

    public FrmCardUserGroup()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
    }

    public bool ShowCard(Guid groupUid, out UserGroups.UserGroup group)
    {
      try
      {
        group = groupUid == Guid.Empty ? new UserGroups.UserGroup() : UserGroups.GetUserGroupByUid(groupUid);
        this.GroupModel = new UserGroupViewModel(group)
        {
          CloseForm = new Action(((Window) this).Close)
        };
        this.DataContext = (object) this.GroupModel;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
          },
          {
            hotKeys.OkAction,
            this.GroupModel.GroupSaveCommand
          },
          {
            hotKeys.CancelAction,
            (ICommand) new RelayCommand((Action<object>) (obj => this.GroupModel.CloseForm()))
          }
        };
        this.ShowDialog();
        return this.GroupModel.SaveResult;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме карточки группы сотрудников");
        group = (UserGroups.UserGroup) null;
        return false;
      }
    }

    private bool CloseCard()
    {
      return this.GroupModel.HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (!(sender is ScrollViewer scrollViewer))
        return;
      scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (double) e.Delta);
      e.Handled = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/users/usersgroup/frmcardusergroup.xaml", UriKind.Relative));
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
          this.TextBoxName = (TextBox) target;
          break;
        case 2:
          ((UIElement) target).PreviewMouseWheel += new MouseWheelEventHandler(this.UIElement_OnPreviewMouseWheel);
          break;
        case 3:
          this.tStack = (ItemsControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
