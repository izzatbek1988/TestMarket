// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.EntityGroupEdit.GoodGroup.FrmGroupEditCategories
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.EntityGroupEdit.GoodGroup
{
  public class FrmGroupEditCategories : WindowWithSize, IComponentConnector
  {
    internal CategorySelectionControl CategorySelectionControl;
    internal CheckBox CheckBoxUnits;
    internal CheckBox CheckBoxTax;
    internal CheckBox CheckBoxTaxRate;
    internal CheckBox CheckBoxTaxSystem;
    internal CheckBox CheckBoxFreePrice;
    internal CheckBox CheckBoxCount;
    internal CheckBox CheckBoxParent;
    internal CheckBox CheckBoxMarking;
    private bool _contentLoaded;

    public FrmGroupEditCategories()
    {
      this.InitializeComponent();
      ((CategoriesGroupEditViewModel) this.DataContext).CloseAction = new Action(((Window) this).Close);
      this.CategorySelectionControl.ButtonContent = Translate.CategorySelectionControl_GroupsListFilter_Выберите_категории;
    }

    public void DoEdit()
    {
      if (new ConfigsRepository<DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
      {
        int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
      }
      else
      {
        (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.GroupEditingGoodAndCategories);
        if (!access.Result)
          return;
        ((CategoriesGroupEditViewModel) this.DataContext).AuthUser = access.User;
        this.ShowDialog();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/entitygroupedit/goodgroup/frmgroupeditcategories.xaml", UriKind.Relative));
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
          this.CategorySelectionControl = (CategorySelectionControl) target;
          break;
        case 2:
          this.CheckBoxUnits = (CheckBox) target;
          break;
        case 3:
          this.CheckBoxTax = (CheckBox) target;
          break;
        case 4:
          this.CheckBoxTaxRate = (CheckBox) target;
          break;
        case 5:
          this.CheckBoxTaxSystem = (CheckBox) target;
          break;
        case 6:
          this.CheckBoxFreePrice = (CheckBox) target;
          break;
        case 7:
          this.CheckBoxCount = (CheckBox) target;
          break;
        case 8:
          this.CheckBoxParent = (CheckBox) target;
          break;
        case 9:
          this.CheckBoxMarking = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
