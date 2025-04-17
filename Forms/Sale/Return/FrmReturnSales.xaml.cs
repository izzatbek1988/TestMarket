// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Sale.Return.FrmReturnSales
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Tooltips;
using Gbs.Helpers.UserControls;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Sale.Return
{
  public partial class FrmReturnSales : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid GridSaleItemsForReturn;
    internal System.Windows.Controls.DataGrid GridReturnItems;
    internal Button ButtonAddSelected;
    internal Button ButtonAddAll;
    internal ConfirmPanelControl1 ConfirmPanelControl1;
    internal Button ButtonEdit;
    internal Button ButtonDelete;
    private bool _contentLoaded;

    private ReturnItemsViewModel ModelReturn { get; set; }

    public FrmReturnSales()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
      TooltipsSetter.Set(this);
    }

    public (List<BasketItem> list, bool result) ShowReturns(Document doc, Users.User authUser = null)
    {
      try
      {
        using (DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(authUser, Actions.ReturnSale))
          {
            (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.ReturnSale);
            if (!access.Result)
              return ((List<BasketItem>) null, false);
            authUser = access.User;
          }
          this.ModelReturn = new ReturnItemsViewModel(new Action(((Window) this).Close), doc.Uid)
          {
            AuthUser = authUser
          };
          this.DataContext = (object) this.ModelReturn;
          if (this.ModelReturn.CountNotNull)
          {
            Gbs.Helpers.ControlsHelpers.DataGrid.Other.FocusRow(this.GridSaleItemsForReturn);
            this.ShowDialog();
          }
          return (this.ModelReturn.Return.ReturnList.ToList<BasketItem>(), this.ModelReturn.SaveResult);
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме возврата продажи");
        return (new List<BasketItem>(), false);
      }
    }

    private bool CloseCard()
    {
      return Functions.IsObjectEqual<Document>(new Document(), (Document) null) || MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/sale/return/frmreturnsales.xaml", UriKind.Relative));
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
          this.GridSaleItemsForReturn = (System.Windows.Controls.DataGrid) target;
          break;
        case 2:
          this.GridReturnItems = (System.Windows.Controls.DataGrid) target;
          break;
        case 3:
          this.ButtonAddSelected = (Button) target;
          break;
        case 4:
          this.ButtonAddAll = (Button) target;
          break;
        case 5:
          this.ConfirmPanelControl1 = (ConfirmPanelControl1) target;
          break;
        case 6:
          this.ButtonEdit = (Button) target;
          break;
        case 7:
          this.ButtonDelete = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
