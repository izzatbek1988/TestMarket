// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Units.FrmCardUnit
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Goods;
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
namespace Gbs.Forms.Settings.Units
{
  public class FrmCardUnit : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    private bool _contentLoaded;

    public FrmCardUnit() => this.InitializeComponent();

    public bool ShowCard(Guid unitUid, out GoodsUnits.GoodUnit unit)
    {
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          ref GoodsUnits.GoodUnit local = ref unit;
          GoodsUnits.GoodUnit goodUnit;
          if (!(unitUid == Guid.Empty))
            goodUnit = GoodsUnits.GetUnitsListWithFilter(dataBase.GetTable<GOODS_UNITS>().Where<GOODS_UNITS>((Expression<Func<GOODS_UNITS, bool>>) (x => x.UID == unitUid))).First<GoodsUnits.GoodUnit>();
          else
            goodUnit = new GoodsUnits.GoodUnit();
          local = goodUnit;
          UnitCardModelView unitCardModelView = new UnitCardModelView();
          unitCardModelView.Unit = unit;
          unitCardModelView.CloseAction = new Action(((Window) this).Close);
          UnitCardModelView modelView = unitCardModelView;
          this.DataContext = (object) modelView;
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              hotKeys.OkAction,
              modelView.SaveCommand
            },
            {
              hotKeys.CancelAction,
              (ICommand) new RelayCommand((Action<object>) (obj => modelView.CloseAction()))
            }
          };
          this.ShowDialog();
          return modelView.SaveResult;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме карточки ед. изменения");
        unit = (GoodsUnits.GoodUnit) null;
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/units/frmcardunit.xaml", UriKind.Relative));
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
