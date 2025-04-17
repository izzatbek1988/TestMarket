// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.FrmClientGroupsCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.Logging;
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
namespace Gbs.Forms.Clients
{
  public class FrmClientGroupsCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    private bool _contentLoaded;

    private GroupClientCardModelView ViewModel { get; set; }

    public FrmClientGroupsCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
    }

    public bool ShowCard(Guid groupUid, out Gbs.Core.Entities.Clients.Group group, Gbs.Core.Entities.Users.User authUser = null)
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(ref authUser, groupUid == Guid.Empty ? Actions.AddClientGroup : Actions.EditClientGroup))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(groupUid == Guid.Empty ? Actions.AddClientGroup : Actions.EditClientGroup);
            if (!access.Result)
            {
              group = (Gbs.Core.Entities.Clients.Group) null;
              return false;
            }
            authUser = access.User;
          }
          group = groupUid == Guid.Empty ? new Gbs.Core.Entities.Clients.Group() : new GroupRepository(dataBase).GetByUid(groupUid);
          this.ViewModel = new GroupClientCardModelView(new Action(((Window) this).Close))
          {
            Group = group,
            EntityClone = (IEntity) group.Clone<Gbs.Core.Entities.Clients.Group>(),
            AuthUser = authUser,
            _isEdit = groupUid != Guid.Empty
          };
          this.DataContext = (object) this.ViewModel;
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              F1help.HelpHotKey,
              (ICommand) F1help.OpenPage((UIElement) this)
            },
            {
              hotKeys.OkAction,
              this.ViewModel.SaveGroup
            },
            {
              hotKeys.CancelAction,
              this.ViewModel.CloseFormCommand
            }
          };
          this.ShowDialog();
          return this.ViewModel.SaveResult;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка редактирования группы покупателей");
        group = (Gbs.Core.Entities.Clients.Group) null;
        return false;
      }
    }

    private bool CloseCard()
    {
      return this.ViewModel.HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/clients/groups/frmclientgroupscard.xaml", UriKind.Relative));
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
      if (connectionId == 1)
        this.TextBoxName = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
