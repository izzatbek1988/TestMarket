// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.AnaliticSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Planfix.Api.Entities.Analitics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings
{
  public partial class AnaliticSettingViewModel : ViewModelWithForm
  {
    public string Title { get; set; }

    public List<AnaliticSettingViewModel.AnaliticItem> Items { get; set; }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Items.Any<AnaliticSettingViewModel.AnaliticItem>((Func<AnaliticSettingViewModel.AnaliticItem, bool>) (x => x.PlanFixId == 0 || this.Analitic.Fields.Field.All<Field>((Func<Field, bool>) (f => int.Parse(f.Id) != x.PlanFixId)))))
          {
            int num = (int) MessageBoxHelper.Show(Translate.AnaliticSettingViewModel_Требуется_для_всех_полей_указать_связь_);
          }
          else
          {
            foreach (AnaliticSettingViewModel.AnaliticItem analiticItem in this.Items)
            {
              switch (analiticItem.Type)
              {
                case AnaliticSettingViewModel.TypeId.None:
                  continue;
                case AnaliticSettingViewModel.TypeId.SaleGoodHandbook:
                  this.Setting.Planfix.SaleAnalitic.GoodHandbook = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.SalePrice:
                  this.Setting.Planfix.SaleAnalitic.PriceId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.SaleQuantity:
                  this.Setting.Planfix.SaleAnalitic.QuantityId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.SaleDiscount:
                  this.Setting.Planfix.SaleAnalitic.DiscountId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.SaleComment:
                  this.Setting.Planfix.SaleAnalitic.CommentId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.PaymentName:
                  this.Setting.Planfix.PaymentsAnalitic.PaymentNameId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.PaymentSum:
                  this.Setting.Planfix.PaymentsAnalitic.SumId = analiticItem.PlanFixId;
                  continue;
                default:
                  throw new ArgumentOutOfRangeException();
              }
            }
            if (!new ConfigsRepository<Integrations>().Save(this.Setting))
              return;
            this.CloseAction();
          }
        }));
      }
    }

    private AnaliticInfo Analitic { get; set; }

    private Integrations Setting { get; set; }

    private AnaliticSettingViewModel.TypeAnalitic Type { get; set; }

    public AnaliticSettingViewModel()
    {
    }

    public AnaliticSettingViewModel(
      Integrations setting,
      AnaliticSettingViewModel.TypeAnalitic typeAnalitic,
      AnaliticInfo analiticInfo)
    {
      this.Setting = setting;
      this.Type = typeAnalitic;
      this.Analitic = new AnaliticRepository().GetInfo(analiticInfo.Id);
      switch (this.Type)
      {
        case AnaliticSettingViewModel.TypeAnalitic.Sale:
          this.Title = Translate.AnaliticSettingViewModel_Аналитика_по_продажам;
          List<AnaliticSettingViewModel.AnaliticItem> analiticItemList1 = new List<AnaliticSettingViewModel.AnaliticItem>();
          AnaliticSettingViewModel.AnaliticItem analiticItem1 = new AnaliticSettingViewModel.AnaliticItem();
          analiticItem1.Type = AnaliticSettingViewModel.TypeId.SaleGoodHandbook;
          analiticItem1.NameItem = Translate.AnaliticSettingViewModel_Товар;
          Integrations setting1 = this.Setting;
          analiticItem1.PlanFixId = setting1 != null ? setting1.Planfix.SaleAnalitic.GoodHandbook : 0;
          analiticItemList1.Add(analiticItem1);
          AnaliticSettingViewModel.AnaliticItem analiticItem2 = new AnaliticSettingViewModel.AnaliticItem();
          analiticItem2.Type = AnaliticSettingViewModel.TypeId.SalePrice;
          analiticItem2.NameItem = Translate.AnaliticSettingViewModel_Стоимость_товара;
          Integrations setting2 = this.Setting;
          analiticItem2.PlanFixId = setting2 != null ? setting2.Planfix.SaleAnalitic.PriceId : 0;
          analiticItemList1.Add(analiticItem2);
          AnaliticSettingViewModel.AnaliticItem analiticItem3 = new AnaliticSettingViewModel.AnaliticItem();
          analiticItem3.Type = AnaliticSettingViewModel.TypeId.SaleQuantity;
          analiticItem3.NameItem = Translate.AnaliticSettingViewModel_Кол_во_товара;
          Integrations setting3 = this.Setting;
          analiticItem3.PlanFixId = setting3 != null ? setting3.Planfix.SaleAnalitic.QuantityId : 0;
          analiticItemList1.Add(analiticItem3);
          AnaliticSettingViewModel.AnaliticItem analiticItem4 = new AnaliticSettingViewModel.AnaliticItem();
          analiticItem4.Type = AnaliticSettingViewModel.TypeId.SaleDiscount;
          analiticItem4.NameItem = Translate.AnaliticSettingViewModel_Скидка_для_товара;
          Integrations setting4 = this.Setting;
          analiticItem4.PlanFixId = setting4 != null ? setting4.Planfix.SaleAnalitic.DiscountId : 0;
          analiticItemList1.Add(analiticItem4);
          AnaliticSettingViewModel.AnaliticItem analiticItem5 = new AnaliticSettingViewModel.AnaliticItem();
          analiticItem5.Type = AnaliticSettingViewModel.TypeId.SaleComment;
          analiticItem5.NameItem = Translate.AnaliticSettingViewModel_Комментарий_к_товару;
          Integrations setting5 = this.Setting;
          analiticItem5.PlanFixId = setting5 != null ? setting5.Planfix.SaleAnalitic.CommentId : 0;
          analiticItemList1.Add(analiticItem5);
          this.Items = analiticItemList1;
          break;
        case AnaliticSettingViewModel.TypeAnalitic.Payment:
          this.Title = Translate.AnaliticSettingViewModel_Аналитика_по_платежам_в_продаже;
          List<AnaliticSettingViewModel.AnaliticItem> analiticItemList2 = new List<AnaliticSettingViewModel.AnaliticItem>();
          AnaliticSettingViewModel.AnaliticItem analiticItem6 = new AnaliticSettingViewModel.AnaliticItem();
          analiticItem6.Type = AnaliticSettingViewModel.TypeId.PaymentName;
          analiticItem6.NameItem = Translate.AnaliticSettingViewModel_Название_способа_платежа;
          Integrations setting6 = this.Setting;
          analiticItem6.PlanFixId = setting6 != null ? setting6.Planfix.PaymentsAnalitic.PaymentNameId : 0;
          analiticItemList2.Add(analiticItem6);
          AnaliticSettingViewModel.AnaliticItem analiticItem7 = new AnaliticSettingViewModel.AnaliticItem();
          analiticItem7.Type = AnaliticSettingViewModel.TypeId.PaymentSum;
          analiticItem7.NameItem = Translate.AnaliticSettingViewModel_Сумма_платежа;
          Integrations setting7 = this.Setting;
          analiticItem7.PlanFixId = setting7 != null ? setting7.Planfix.PaymentsAnalitic.SumId : 0;
          analiticItemList2.Add(analiticItem7);
          this.Items = analiticItemList2;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      this.Items.ForEach((Action<AnaliticSettingViewModel.AnaliticItem>) (x => x.Fields = this.Analitic.Fields.Field));
    }

    public enum TypeAnalitic
    {
      Sale,
      Payment,
    }

    public enum TypeId
    {
      None,
      SaleGoodHandbook,
      SalePrice,
      SaleQuantity,
      SaleDiscount,
      SaleComment,
      PaymentName,
      PaymentSum,
      Barcode,
      GroupId,
      BuyPrice,
    }

    public class AnaliticItem
    {
      public string NameItem { get; set; }

      public int PlanFixId { get; set; }

      public AnaliticSettingViewModel.TypeId Type { get; set; }

      public List<Field> Fields { get; set; }
    }
  }
}
