// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Excel.FrmExcelData
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Excel
{
  public class FrmExcelData : WindowWithSize, IComponentConnector
  {
    internal RadioButton RbSelectedGroup;
    internal RadioButton RbGroup;
    internal RadioButton RbNewGroup;
    internal RadioButton RbEmptyGroup;
    private bool _contentLoaded;

    private ExcelDataViewModel Model { get; set; }

    public FrmExcelData() => this.InitializeComponent();

    public bool Import(Users.User user)
    {
      try
      {
        ExcelDataViewModel excelDataViewModel = new ExcelDataViewModel();
        excelDataViewModel.CloseAction = new Action(((Window) this).Close);
        excelDataViewModel.AuthUser = user;
        this.Model = excelDataViewModel;
        this.DataContext = (object) this.Model;
        this.ShowDialog();
        return this.Model.GoodsList.Any<Gbs.Core.Entities.Goods.Good>();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки товаров из Ексель");
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
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/excel/frmexcelgoods.xaml", UriKind.Relative));
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
          this.RbSelectedGroup = (RadioButton) target;
          break;
        case 2:
          this.RbGroup = (RadioButton) target;
          break;
        case 3:
          this.RbNewGroup = (RadioButton) target;
          break;
        case 4:
          this.RbEmptyGroup = (RadioButton) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
