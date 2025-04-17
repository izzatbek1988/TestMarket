// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.FrmClientCard
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
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Clients
{
  public class FrmClientCard : WindowWithSize, IComponentConnector
  {
    internal TextBox ClientNameTb;
    internal DecimalUpDown BonusesDecimalUpDown;
    private bool _contentLoaded;

    private ClientCardViewModel ClientModel { get; set; }

    public FrmClientCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
    }

    public bool ShowCard(
      Guid selectClientUid,
      out ClientAdnSum client,
      ClientAdnSum _client = null,
      Gbs.Core.Entities.Users.User authUser = null,
      Actions action = Actions.ClientsAdd)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref authUser, action))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(action);
          if (!access.Result)
          {
            client = (ClientAdnSum) null;
            return false;
          }
          authUser = access.User;
        }
        client = this.GetClient(selectClientUid, _client);
        this.ClientModel = new ClientCardViewModel(client)
        {
          CloseForm = new Action(((Window) this).Close),
          AuthUser = authUser,
          IsEdit = selectClientUid != Guid.Empty && _client == null,
          FocusSumBonuses = new Action(this.SelectAllBonusesDecimalUpDown)
        };
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
          },
          {
            hotKeys.OkAction,
            this.ClientModel.SaveClient
          },
          {
            hotKeys.CancelAction,
            this.ClientModel.CloseCommand
          }
        };
        this.DataContext = (object) this.ClientModel;
        this.ShowDialog();
        return this.ClientModel.SaveResult;
      }
    }

    private ClientAdnSum GetClient(Guid clientUid, ClientAdnSum client)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        return client == null ? (clientUid == Guid.Empty ? new ClientAdnSum() : new ClientsRepository(dataBase).GetClientByUidAndSum(clientUid)) : client;
    }

    private bool CloseCard()
    {
      int num = this.ClientModel.HasNoSavedChanges() ? 1 : (MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes ? 1 : 0);
      if (num == 0)
        return num != 0;
      this.ClientModel.CancelTask = true;
      return num != 0;
    }

    private void SelectAllBonusesDecimalUpDown()
    {
      Application.Current.Dispatcher?.Invoke((Action) (() => this.BonusesDecimalUpDown.Focus()));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/clients/frmclientcard.xaml", UriKind.Relative));
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
          this.BonusesDecimalUpDown = (DecimalUpDown) target;
        else
          this._contentLoaded = true;
      }
      else
        this.ClientNameTb = (TextBox) target;
    }
  }
}
