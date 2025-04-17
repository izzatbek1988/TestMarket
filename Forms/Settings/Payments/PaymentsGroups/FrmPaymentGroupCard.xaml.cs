// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Payments.PaymentsGroups.GroupPaymentCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Payments.PaymentsGroups
{
  public partial class GroupPaymentCardViewModel : ViewModelWithForm
  {
    private string _parentName = string.Empty;

    public bool SaveResult { get; set; }

    public string ParentName
    {
      get => this.Group.ParentGroup != null ? this.Group.ParentGroup.Name : Translate.Devices_Нет;
    }

    public ICommand SelectParent { get; set; }

    public ICommand SaveGroup { get; set; }

    public ICommand CloseCard { get; set; }

    public Action CloseFrm { get; set; }

    public PaymentGroups.PaymentGroup Group { get; set; } = new PaymentGroups.PaymentGroup();

    public Dictionary<PaymentGroups.VisiblePaymentGroup, string> VisiblePayment { get; } = PaymentGroups.GetDictionaryVisiblePayment();

    public GroupPaymentCardViewModel()
    {
    }

    public GroupPaymentCardViewModel(PaymentGroups.PaymentGroup group)
    {
      this.Group = group;
      this.SelectParent = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        PaymentGroups.PaymentGroup group1;
        this.Group.ParentGroup = new FrmSelectedPayGroup().GetSingleSelectedGroupUid(out group1, this.Group.VisibleIn) ? group1 : (PaymentGroups.PaymentGroup) null;
        this.OnPropertyChanged(nameof (ParentName));
      }));
      this.SaveGroup = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      this.CloseCard = (ICommand) new RelayCommand((Action<object>) (obj => this.Close()));
    }

    public void Save()
    {
      this.SaveResult = this.Group.Save();
      if (!this.SaveResult)
        return;
      this.CloseFrm();
    }

    private void Close()
    {
      if (!Functions.IsObjectEqual<PaymentGroups.PaymentGroup>(PaymentGroups.GetPaymentGroupByUid(this.Group.Uid), this.Group) && MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
        return;
      this.CloseFrm();
    }
  }
}
