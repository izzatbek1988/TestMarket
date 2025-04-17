// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.HandbookGoodSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Planfix.Api.Entities.Handbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings
{
  public class HandbookGoodSettingViewModel : ViewModelWithForm
  {
    public string Title { get; set; }

    public List<HandbookGoodSettingViewModel.HandbookItem> Items { get; set; }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Items.Any<HandbookGoodSettingViewModel.HandbookItem>((Func<HandbookGoodSettingViewModel.HandbookItem, bool>) (x => x.PlanFixId == 0 || this.Handbook.fields.Field.All<HandField>((Func<HandField, bool>) (f => f.Id != x.PlanFixId)))))
          {
            int num = (int) MessageBoxHelper.Show(Translate.AnaliticSettingViewModel_Требуется_для_всех_полей_указать_связь_);
          }
          else
          {
            foreach (HandbookGoodSettingViewModel.HandbookItem handbookItem in this.Items)
            {
              switch (handbookItem.Type)
              {
                case HandbookGoodSettingViewModel.TypeId.None:
                  continue;
                case HandbookGoodSettingViewModel.TypeId.GoodName:
                  this.Setting.Planfix.HandbookGood.NameId = handbookItem.PlanFixId;
                  continue;
                case HandbookGoodSettingViewModel.TypeId.MaxPrice:
                  this.Setting.Planfix.HandbookGood.PriceId = handbookItem.PlanFixId;
                  continue;
                case HandbookGoodSettingViewModel.TypeId.Quantity:
                  this.Setting.Planfix.HandbookGood.QuantityId = handbookItem.PlanFixId;
                  continue;
                case HandbookGoodSettingViewModel.TypeId.GoodBarcode:
                  this.Setting.Planfix.HandbookGood.BarcodeId = handbookItem.PlanFixId;
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

    private Handbook Handbook { get; set; }

    private Integrations Setting { get; set; }

    public HandbookGoodSettingViewModel()
    {
    }

    public HandbookGoodSettingViewModel(Integrations setting, Handbook handbook)
    {
      this.Setting = setting;
      this.Title = Translate.HandbookGoodSettingViewModel_Справочник_товаров;
      this.Handbook = new HandbookRepository().GetHandbookStructure(handbook).handbook;
      List<HandbookGoodSettingViewModel.HandbookItem> handbookItemList = new List<HandbookGoodSettingViewModel.HandbookItem>();
      HandbookGoodSettingViewModel.HandbookItem handbookItem1 = new HandbookGoodSettingViewModel.HandbookItem();
      handbookItem1.Type = HandbookGoodSettingViewModel.TypeId.GoodName;
      handbookItem1.NameItem = Translate.HandbookGoodSettingViewModel_Название_товара;
      Integrations setting1 = this.Setting;
      handbookItem1.PlanFixId = setting1 != null ? setting1.Planfix.HandbookGood.NameId : 0;
      handbookItemList.Add(handbookItem1);
      HandbookGoodSettingViewModel.HandbookItem handbookItem2 = new HandbookGoodSettingViewModel.HandbookItem();
      handbookItem2.Type = HandbookGoodSettingViewModel.TypeId.GoodBarcode;
      handbookItem2.NameItem = Translate.HandbookGoodSettingViewModel_Штрих_код_товара;
      Integrations setting2 = this.Setting;
      handbookItem2.PlanFixId = setting2 != null ? setting2.Planfix.HandbookGood.BarcodeId : 0;
      handbookItemList.Add(handbookItem2);
      HandbookGoodSettingViewModel.HandbookItem handbookItem3 = new HandbookGoodSettingViewModel.HandbookItem();
      handbookItem3.Type = HandbookGoodSettingViewModel.TypeId.MaxPrice;
      handbookItem3.NameItem = Translate.HandbookGoodSettingViewModel_Максимальная_цена;
      Integrations setting3 = this.Setting;
      handbookItem3.PlanFixId = setting3 != null ? setting3.Planfix.HandbookGood.PriceId : 0;
      handbookItemList.Add(handbookItem3);
      HandbookGoodSettingViewModel.HandbookItem handbookItem4 = new HandbookGoodSettingViewModel.HandbookItem();
      handbookItem4.Type = HandbookGoodSettingViewModel.TypeId.Quantity;
      handbookItem4.NameItem = Translate.FrmGoodsQuantity_Количество;
      Integrations setting4 = this.Setting;
      handbookItem4.PlanFixId = setting4 != null ? setting4.Planfix.HandbookGood.QuantityId : 0;
      handbookItemList.Add(handbookItem4);
      this.Items = handbookItemList;
      this.Items.ForEach((Action<HandbookGoodSettingViewModel.HandbookItem>) (x => x.Fields = this.Handbook.fields.Field));
    }

    public enum TypeId
    {
      None,
      GoodName,
      MaxPrice,
      Quantity,
      GoodBarcode,
    }

    public class HandbookItem
    {
      public string NameItem { get; set; }

      public int PlanFixId { get; set; }

      public HandbookGoodSettingViewModel.TypeId Type { get; set; }

      public List<HandField> Fields { get; set; }
    }
  }
}
