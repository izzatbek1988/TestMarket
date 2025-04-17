// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.PreSaveOrderViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Cafe;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Cafe
{
  public partial class PreSaveOrderViewModel : ViewModelWithForm
  {
    private readonly Gbs.Core.Config.Cafe _cafeSettings = new ConfigsRepository<Gbs.Core.Config.Cafe>().Get();
    private bool _isResult;

    public Visibility VisibilityPanelTableInfo
    {
      get
      {
        Gbs.Core.Config.Cafe cafeSettings = this._cafeSettings;
        return (cafeSettings != null ? (cafeSettings.IsTableAndGuest ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public CafeBasket Basket { get; set; }

    public bool ShowPreSave(CafeBasket basket)
    {
      this.Basket = basket;
      this.FormToSHow = (WindowWithSize) new FrmCafePreSave();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      if (new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().IsSpeedCafeOrder)
        this.SaveCloseOrderCommand.Execute((object) null);
      else
        this.ShowForm();
      return this._isResult;
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    public ICommand SaveDraftOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          CafeBasket basket = this.Basket;
          Document document = this.Basket.Document;
          Guid uidOrder = document != null ? __nonvirtual (document.Uid) : Guid.Empty;
          ActionResult actionResult = basket.SaveCafe(false, uidOrder, true);
          if (actionResult.Result != ActionResult.Results.Ok)
          {
            string str = string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages);
            if (str.IsNullOrEmpty())
              return;
            switch (actionResult.Result)
            {
              case ActionResult.Results.Cancel:
                break;
              case ActionResult.Results.Warning:
                MessageBoxHelper.Warning(str);
                break;
              case ActionResult.Results.Error:
                MessageBoxHelper.Error(str);
                break;
              default:
                throw new ArgumentOutOfRangeException();
            }
          }
          else
          {
            if (actionResult.Result != ActionResult.Results.Ok)
              return;
            this._isResult = true;
            this.CloseAction();
          }
        }));
      }
    }

    public ICommand SaveCloseOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          MainWindowViewModel.DoWithPause();
          CafeBasket basket = this.Basket;
          Document document = this.Basket.Document;
          Guid uidOrder = document != null ? __nonvirtual (document.Uid) : Guid.Empty;
          ActionResult actionResult = basket.SaveCafe(true, uidOrder, true);
          if (actionResult.Result != ActionResult.Results.Ok)
          {
            string str = string.Join(Other.NewLine(), (IEnumerable<string>) actionResult.Messages);
            if (str.IsNullOrEmpty())
              return;
            switch (actionResult.Result)
            {
              case ActionResult.Results.Cancel:
                break;
              case ActionResult.Results.Warning:
                MessageBoxHelper.Warning(str);
                break;
              case ActionResult.Results.Error:
                MessageBoxHelper.Error(str);
                break;
              default:
                throw new ArgumentOutOfRangeException();
            }
          }
          else
          {
            if (actionResult.Result != ActionResult.Results.Ok)
              return;
            this._isResult = true;
            this.CloseAction();
          }
        }));
      }
    }
  }
}
