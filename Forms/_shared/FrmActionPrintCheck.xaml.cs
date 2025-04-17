// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.ActionPrintViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Factories;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class ActionPrintViewModel : ViewModelWithForm
  {
    private ActionPrintViewModel.TypePrint _typePrint;
    private Gbs.Core.ViewModels.Basket.Basket _basket;
    private bool _isPrint;

    private bool IsGetType { get; set; }

    public Visibility VisibilityPrintButton
    {
      get
      {
        return this.VisibilityNonFiscalPrint != Visibility.Collapsed ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityNonFiscalPrint { get; set; } = Visibility.Collapsed;

    public Visibility VisibilityContinueWithoutCheck { get; set; } = Visibility.Collapsed;

    public ICommand PrintFiscalCheck
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._typePrint = ActionPrintViewModel.TypePrint.Fiscal;
          if (this.IsGetType)
          {
            this._isPrint = true;
            this.CloseAction();
          }
          else
          {
            LogHelper.Debug("Печатаем ФИСКАЛЬНЫЙ чек");
            this._isPrint = this._basket.TryPrintCheck(out MessageBoxResult _);
          }
        }));
      }
    }

    public ICommand PrintNoFiscalCheck
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._typePrint = ActionPrintViewModel.TypePrint.NonFiscal;
          if (this.IsGetType)
          {
            this._isPrint = true;
            this.CloseAction();
          }
          else
          {
            LogHelper.Debug("Печатаем НЕ ФИСКАЛЬНЫЙ чек");
            this._isPrint = this._basket.TryPrintCheck(out MessageBoxResult _, CheckFiscalTypes.NonFiscal);
          }
        }));
      }
    }

    public ICommand PrintCancel
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isPrint = false;
          this.CloseAction();
        }));
      }
    }

    public ICommand PrintDocument
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (bool Result2, Gbs.Core.Entities.Users.User User2) = Other.GetUserForDocument(Gbs.Core.Entities.Actions.SaleSave);
          if (!Result2)
            return;
          this._basket.User = User2;
          new FastReportFacade().SelectTemplateAndShowReport(new PrintableReportFactory().CreateForSaleDocument(new DocumentsFactory().Create(this._basket)), (Gbs.Core.Entities.Users.User) null);
        }));
      }
    }

    public ICommand ContinueWithoutCheck
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._typePrint = ActionPrintViewModel.TypePrint.NoCheck;
          if (this.IsGetType)
          {
            this._isPrint = true;
            this.CloseAction();
          }
          else
          {
            LogHelper.Debug("Не печатаем чек, так как выбран этот вариант пользователем.");
            this._isPrint = true;
          }
        }));
      }
    }

    public bool Print(Gbs.Core.ViewModels.Basket.Basket basket, ActionPrintViewModel.TypePrint typePrint)
    {
      this._basket = basket;
      switch (typePrint)
      {
        case ActionPrintViewModel.TypePrint.Fiscal:
          this.PrintFiscalCheck.Execute((object) null);
          break;
        case ActionPrintViewModel.TypePrint.NonFiscal:
          this.PrintNoFiscalCheck.Execute((object) null);
          break;
        case ActionPrintViewModel.TypePrint.Document:
          this.PrintDocument.Execute((object) null);
          break;
        case ActionPrintViewModel.TypePrint.NoCheck:
          this.ContinueWithoutCheck.Execute((object) null);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (typePrint), (object) typePrint, (string) null);
      }
      return this._isPrint;
    }

    public (ActionPrintViewModel.TypePrint type, bool isPrint) GetTypePrint(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      this._basket = basket;
      this.IsGetType = true;
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.VisibilityNonFiscalPrint = !devices.CheckPrinter.FiscalKkm.IsLetNonFiscal || devices.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm ? Visibility.Collapsed : Visibility.Visible;
      this.VisibilityContinueWithoutCheck = devices.CheckPrinter.FiscalKkm.AllowSalesWithoutCheck ? Visibility.Visible : Visibility.Collapsed;
      this.FormToSHow = (WindowWithSize) new FrmActionPrintCheck(this);
      this.ShowForm();
      return (this._typePrint, this._isPrint);
    }

    public enum TypePrint
    {
      Fiscal,
      NonFiscal,
      Document,
      NoCheck,
    }
  }
}
