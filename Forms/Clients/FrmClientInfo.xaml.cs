// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Clients.ClientInfoViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Settings;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Clients
{
  public partial class ClientInfoViewModel : ViewModelWithForm
  {
    private bool _isResult = true;
    private Decimal _bonuses;
    private Visibility _visibilityBonuses;

    public ClientAdnSum Client { get; set; } = new ClientAdnSum();

    public Decimal AccessBonuses { get; set; }

    public Decimal Bonuses
    {
      get => this._bonuses;
      set
      {
        this._bonuses = value;
        this.OnPropertyChanged(nameof (Bonuses));
      }
    }

    public string TextAccessBonuses { get; set; } = Translate.Доступно;

    public Visibility VisibilityBonuses
    {
      get => this._visibilityBonuses;
      set
      {
        this._visibilityBonuses = value;
        this.OnPropertyChanged(nameof (VisibilityBonuses));
      }
    }

    public bool GetSumBonuses(
      ClientAdnSum client,
      Decimal sumDoc,
      ref Decimal bonusesSum,
      bool isShowUser = false)
    {
      this.Client = client;
      Decimal num = Convert.ToDecimal(new SettingsRepository().GetSettingByType(Types.MaxValueBonuses).Value);
      if (!(bool) new SettingsRepository().GetSettingByType(Types.ActiveBonuses).Value || client.TotalBonusSum <= 0M)
      {
        this.VisibilityBonuses = Visibility.Collapsed;
        bonusesSum = 0M;
        if (!isShowUser)
          return true;
      }
      this.AccessBonuses = Math.Round(sumDoc * num / 100M, 4);
      this.AccessBonuses = client.TotalBonusSum < this.AccessBonuses ? client.TotalBonusSum : this.AccessBonuses;
      this.AccessBonuses = this.AccessBonuses < 0M ? 0M : this.AccessBonuses;
      this.Bonuses = bonusesSum;
      this.TextAccessBonuses += string.Format(Translate.ClientInfoViewModel_GetSumBonuses____0___от_суммы_покупки__, (object) num);
      this.FormToSHow = (WindowWithSize) new FrmClientInfo();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
      bonusesSum = this._isResult ? this.Bonuses : bonusesSum;
      return this._isResult;
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isResult = true;
          this.CloseAction();
        }));
      }
    }

    public ICommand CancelCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isResult = false;
          this.CloseAction();
        }));
      }
    }
  }
}
