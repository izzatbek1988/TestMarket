// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Storage.FrmCardStorage
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Storage
{
  public class FrmCardStorage : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    private bool _contentLoaded;

    public FrmCardStorage() => this.InitializeComponent();

    public bool ShowCard(Guid storageUid, out Storages.Storage storage)
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          ref Storages.Storage local = ref storage;
          Storages.Storage storage1;
          if (!(storageUid == Guid.Empty))
            storage1 = Storages.GetStorages(dataBase.GetTable<STORAGES>().Where<STORAGES>((Expression<Func<STORAGES, bool>>) (x => x.UID == storageUid))).First<Storages.Storage>();
          else
            storage1 = new Storages.Storage();
          local = storage1;
          StorageCardModelView myViewModel = new StorageCardModelView(storage)
          {
            CloseCardAction = new Action(((Window) this).Close)
          };
          this.DataContext = (object) myViewModel;
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              hotKeys.OkAction,
              myViewModel.SaveWarehouseCommand
            },
            {
              hotKeys.CancelAction,
              (ICommand) new RelayCommand((Action<object>) (obj => myViewModel.CloseCardAction()))
            }
          };
          this.ShowDialog();
          return myViewModel.SaveResult;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "ошибка в форме карточки склада");
        storage = (Storages.Storage) null;
        return false;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/storage/frmcardstorage.xaml", UriKind.Relative));
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
