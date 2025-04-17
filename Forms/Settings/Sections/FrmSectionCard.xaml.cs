// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Sections.SectionCardModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Payments;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Sections
{
  public partial class SectionCardModelView : ViewModelWithForm
  {
    public Gbs.Core.Entities.Sections.Section Section { get; set; }

    public Action CloseCardAction { get; set; }

    public ICommand SaveSelectoinCommand { get; set; }

    public bool SaveResult { get; private set; }

    public SectionCardModelView()
    {
    }

    public SectionCardModelView(Gbs.Core.Entities.Sections.Section section)
    {
      this.Section = section;
      section.GbsId = "6:" + section.GbsId;
      this.SaveSelectoinCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      this.AddMethodPayment = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        PaymentMethods.PaymentMethod method;
        if (!new FrmCardMethodPayment().ShowCard(Guid.Empty, out method))
          return;
        this.Section.Methods.Add(method);
        this.OnPropertyChanged(nameof (Section));
      }));
      this.EditMethodPayment = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedMethod == null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.SectionCardModelView_необходимо_сначала_выбрать_метод);
        }
        else
        {
          PaymentMethods.PaymentMethod method;
          if (!new FrmCardMethodPayment().ShowCard(this.SelectedMethod.Uid, out method))
            return;
          this.Section.Methods[this.Section.Methods.FindIndex((Predicate<PaymentMethods.PaymentMethod>) (x => x.Uid == this.SelectedMethod.Uid))] = method;
          this.OnPropertyChanged(nameof (Section));
        }
      }));
      this.DeleteMethodPayment = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedMethod == null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.SectionCardModelView_необходимо_сначала_выбрать_метод);
        }
        else
        {
          if (MessageBoxHelper.Show(Translate.PaymentAccountListViewModel_Вы_уверены_, Translate.SectionCardModelView_Удаление_способа_платежа, MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
          this.SelectedMethod.IsDeleted = true;
          this.SelectedMethod.Save();
          this.Section.Methods.Remove(this.SelectedMethod);
          this.OnPropertyChanged(nameof (Section));
        }
      }));
    }

    private void Save()
    {
      this.Section.GbsId = this.Section.GbsId.Remove(0, 2);
      if (this.Section.VerifyBeforeSave().Result != ActionResult.Results.Ok)
        return;
      this.SaveResult = true;
      this.Section.Save();
      this.Section.GbsId = "6:" + this.Section.GbsId;
      this.CloseCardAction();
    }

    public PaymentMethods.PaymentMethod SelectedMethod { get; set; }

    public ICommand AddMethodPayment { get; set; }

    public ICommand EditMethodPayment { get; set; }

    public ICommand DeleteMethodPayment { get; set; }
  }
}
