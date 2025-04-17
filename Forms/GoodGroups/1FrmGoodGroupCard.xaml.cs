// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroups.FrmGoodGroupCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
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
namespace Gbs.Forms.GoodGroups
{
  public class FrmGoodGroupCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TxtName;
    internal Button BtnSelectParent;
    internal ComboBox CmoGoodsType;
    internal ComboBox cmoUnits;
    private bool _contentLoaded;

    private GroupCardViewModel MyViewModel { get; set; }

    public FrmGoodGroupCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
    }

    public bool ShowGroupCard(Guid groupUid, out Gbs.Core.Entities.GoodGroups.Group group, Gbs.Core.Entities.Users.User authUser = null)
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(ref authUser, groupUid == Guid.Empty ? Actions.AddGoodGroup : Actions.EditGoodGroup))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(groupUid == Guid.Empty ? Actions.AddGoodGroup : Actions.EditGoodGroup);
            if (!access.Result)
            {
              group = (Gbs.Core.Entities.GoodGroups.Group) null;
              return false;
            }
            authUser = access.User;
          }
          GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
          group = groupUid == Guid.Empty ? new Gbs.Core.Entities.GoodGroups.Group() : groupsRepository.GetByUid(groupUid);
          this.MyViewModel = new GroupCardViewModel(group)
          {
            CloseFrm = new Action(((Window) this).Close),
            EntityClone = (IEntity) group.Clone<Gbs.Core.Entities.GoodGroups.Group>(),
            _isEditCard = groupUid != Guid.Empty,
            User = authUser
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
              this.MyViewModel.SaveGroup
            },
            {
              hotKeys.CancelAction,
              this.MyViewModel.CloseCard
            }
          };
          this.DataContext = (object) this.MyViewModel;
          this.ShowDialog();
          return this.MyViewModel.SaveResult;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке категории товаров");
        group = (Gbs.Core.Entities.GoodGroups.Group) null;
        return false;
      }
    }

    private bool CloseCard()
    {
      return this.MyViewModel.HasNoSavedChanges() || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goodgroups/frmgoodgroupcard.xaml", UriKind.Relative));
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
          this.TxtName = (TextBox) target;
          break;
        case 2:
          this.BtnSelectParent = (Button) target;
          break;
        case 3:
          this.CmoGoodsType = (ComboBox) target;
          break;
        case 4:
          this.cmoUnits = (ComboBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
