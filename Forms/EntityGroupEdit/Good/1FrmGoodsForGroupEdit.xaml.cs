// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodGroupEdit.FrmGoodsForGroupEdit
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods.GoodGroupEdit
{
  public class FrmGoodsForGroupEdit : WindowWithSize, IComponentConnector
  {
    internal DataGrid ListGoodsLable;
    private bool _contentLoaded;

    private GoodsGroupEditViewModel Model { get; set; }

    public FrmGoodsForGroupEdit()
    {
      this.InitializeComponent();
      this.Closing += new CancelEventHandler(this.OnClosing);
    }

    private void OnClosing(object sender, CancelEventArgs e)
    {
      if (!this.Model.IsVisibleMessage || MessageBoxHelper.Question(Translate.FrmGoodsForGroupEdit_Вы_уверены__что_хотите_закрыть_) != MessageBoxResult.No)
        return;
      e.Cancel = true;
    }

    public void DoEdit(List<BasketItem> goods = null, Gbs.Core.Entities.Users.User authUser = null)
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        if (!new UsersRepository(dataBase).GetAccess(ref authUser, Actions.GroupEditingGoodAndCategories))
        {
          (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GroupEditingGoodAndCategories);
          if (!access.Result)
            return;
          authUser = access.User;
        }
        GoodsGroupEditViewModel groupEditViewModel = new GoodsGroupEditViewModel();
        groupEditViewModel.CloseAction = new Action(((Window) this).Close);
        groupEditViewModel.AuthUser = authUser;
        this.Model = groupEditViewModel;
        if (goods != null)
        {
          List<BasketItem> basketItemList = new List<BasketItem>();
          foreach (BasketItem good1 in goods)
          {
            BasketItem good = good1;
            if (basketItemList.All<BasketItem>((Func<BasketItem, bool>) (x => x.Good.Uid != good.Good.Uid)))
              basketItemList.Add(good);
          }
          this.Model.GroupEditing.Items = new ObservableCollection<BasketItem>(basketItemList);
        }
        this.DataContext = (object) this.Model;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            F1help.HelpHotKey,
            (ICommand) F1help.OpenPage((UIElement) this)
          },
          {
            hotKeys.OkAction,
            this.Model.DoGroupEditingCommand
          },
          {
            hotKeys.CancelAction,
            this.Model.CloseCommand
          },
          {
            hotKeys.AddItem,
            this.Model.AddItem
          },
          {
            hotKeys.DeleteItem,
            (ICommand) new RelayCommand((Action<object>) (obj => this.Model.GroupEditing.DeleteItemCommand.Execute((object) this.ListGoodsLable.SelectedItems)))
          }
        };
        this.Show();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/entitygroupedit/good/frmgoodsforgroupedit.xaml", UriKind.Relative));
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
        this.ListGoodsLable = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
