// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageCafe
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class PageCafe : Page, IComponentConnector
  {
    internal CheckBox PercentForServiceCheckBox;
    private bool _contentLoaded;

    private CafeViewModel MyViewModel { get; }

    public PageCafe() => this.InitializeComponent();

    public PageCafe(Cafe cafe, Gbs.Core.Config.DataBase dataBase, PageBasic page)
    {
      this.InitializeComponent();
      this.MyViewModel = new CafeViewModel(cafe, dataBase)
      {
        SetModeActionProgram = new Action<GlobalDictionaries.Mode>(page.SetModeProgram)
      };
      this.DataContext = (object) this.MyViewModel;
      using (Gbs.Core.Db.DataBase dataBase1 = Data.GetDataBase())
      {
        Good byUid = new GoodRepository(dataBase1).GetByUid(GlobalDictionaries.PercentForServiceGoodUid);
        if (byUid == null)
          return;
        this.MyViewModel.PercentForServiceGood = byUid;
      }
    }

    public bool Save()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        GoodGroups.Group group1 = new GoodGroupsRepository(dataBase).GetByUid(GlobalDictionaries.PercentForServiceGroupUid);
        if (group1 == null)
        {
          GoodGroups.Group group2 = new GoodGroups.Group();
          group2.Uid = GlobalDictionaries.PercentForServiceGroupUid;
          group2.Name = Translate.PageCafe_Save__S__Услуги;
          group2.GoodsType = GlobalDictionaries.GoodTypes.Service;
          group1 = group2;
        }
        GoodGroups.Group group3 = group1;
        group3.IsDeleted = this.MyViewModel.PercentForServiceGood.IsDeleted;
        bool flag = true;
        if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home)
          flag = new GoodGroupsRepository(dataBase).Save(group3) && new GoodRepository(dataBase).Save(this.MyViewModel.PercentForServiceGood);
        return this.MyViewModel.Save() & flag;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pagecafe.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.PercentForServiceCheckBox = (CheckBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
