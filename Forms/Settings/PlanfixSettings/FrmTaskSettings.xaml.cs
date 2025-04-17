// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PlanfixSettings.TaskPlanFixViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Planfix.Api.Entities.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.PlanfixSettings
{
  public partial class TaskPlanFixViewModel : ViewModelWithForm
  {
    private Planfix.Api.Entities.Task.Task Task { get; set; }

    private Integrations Setting { get; set; }

    public string Title { get; set; }

    public List<AnaliticSettingViewModel.AnaliticItem> Items { get; set; }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Items.Any<AnaliticSettingViewModel.AnaliticItem>((Func<AnaliticSettingViewModel.AnaliticItem, bool>) (x => x.PlanFixId == 0 || this.Task.CustomData.CustomValue.All<CustomValue>((Func<CustomValue, bool>) (f => int.Parse(f.Field.Id) != x.PlanFixId)))))
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
                case AnaliticSettingViewModel.TypeId.SalePrice:
                  this.Setting.Planfix.TemplateGoodAsTask.PriceId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.SaleQuantity:
                  this.Setting.Planfix.TemplateGoodAsTask.QuantityId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.Barcode:
                  this.Setting.Planfix.TemplateGoodAsTask.BarcodeId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.GroupId:
                  this.Setting.Planfix.TemplateGoodAsTask.GroupNameId = analiticItem.PlanFixId;
                  continue;
                case AnaliticSettingViewModel.TypeId.BuyPrice:
                  this.Setting.Planfix.TemplateGoodAsTask.BuyPriceId = analiticItem.PlanFixId;
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

    public void ShowSettingATaskGood(Integrations setting, TaskGood taskGood)
    {
      this.Setting = setting;
      (Planfix.Api.Answer answer, Planfix.Api.Entities.Task.Task entity) tuple = new TaskRepository().Get(taskGood.Id);
      if (tuple.answer.Result == Planfix.Api.Answer.ResultTypes.Error)
        return;
      this.Task = tuple.entity;
      this.Title = Translate.TaskPlanFixViewModel_Задача_для_товара;
      List<AnaliticSettingViewModel.AnaliticItem> analiticItemList = new List<AnaliticSettingViewModel.AnaliticItem>();
      AnaliticSettingViewModel.AnaliticItem analiticItem1 = new AnaliticSettingViewModel.AnaliticItem();
      analiticItem1.Type = AnaliticSettingViewModel.TypeId.SalePrice;
      analiticItem1.NameItem = Translate.AnaliticSettingViewModel_Стоимость_товара;
      Integrations setting1 = this.Setting;
      analiticItem1.PlanFixId = setting1 != null ? setting1.Planfix.TemplateGoodAsTask.PriceId : 0;
      analiticItemList.Add(analiticItem1);
      AnaliticSettingViewModel.AnaliticItem analiticItem2 = new AnaliticSettingViewModel.AnaliticItem();
      analiticItem2.Type = AnaliticSettingViewModel.TypeId.BuyPrice;
      analiticItem2.NameItem = Translate.FrmGoodsQuantity_ЗакупочнаяЦена;
      Integrations setting2 = this.Setting;
      analiticItem2.PlanFixId = setting2 != null ? setting2.Planfix.TemplateGoodAsTask.BuyPriceId : 0;
      analiticItemList.Add(analiticItem2);
      AnaliticSettingViewModel.AnaliticItem analiticItem3 = new AnaliticSettingViewModel.AnaliticItem();
      analiticItem3.Type = AnaliticSettingViewModel.TypeId.SaleQuantity;
      analiticItem3.NameItem = Translate.AnaliticSettingViewModel_Кол_во_товара;
      Integrations setting3 = this.Setting;
      analiticItem3.PlanFixId = setting3 != null ? setting3.Planfix.TemplateGoodAsTask.QuantityId : 0;
      analiticItemList.Add(analiticItem3);
      AnaliticSettingViewModel.AnaliticItem analiticItem4 = new AnaliticSettingViewModel.AnaliticItem();
      analiticItem4.Type = AnaliticSettingViewModel.TypeId.Barcode;
      analiticItem4.NameItem = Translate.FrmAuthorization_ШтрихКод;
      Integrations setting4 = this.Setting;
      analiticItem4.PlanFixId = setting4 != null ? setting4.Planfix.TemplateGoodAsTask.BarcodeId : 0;
      analiticItemList.Add(analiticItem4);
      AnaliticSettingViewModel.AnaliticItem analiticItem5 = new AnaliticSettingViewModel.AnaliticItem();
      analiticItem5.Type = AnaliticSettingViewModel.TypeId.GroupId;
      analiticItem5.NameItem = Translate.FrmExcelGoods_КатегорияТовара;
      Integrations setting5 = this.Setting;
      analiticItem5.PlanFixId = setting5 != null ? setting5.Planfix.TemplateGoodAsTask.GroupNameId : 0;
      analiticItemList.Add(analiticItem5);
      this.Items = analiticItemList;
      this.Items.ForEach((Action<AnaliticSettingViewModel.AnaliticItem>) (x => x.Fields = this.Task.CustomData.CustomValue.Select<CustomValue, Planfix.Api.Entities.Analitics.Field>((Func<CustomValue, Planfix.Api.Entities.Analitics.Field>) (f => new Planfix.Api.Entities.Analitics.Field()
      {
        Id = f.Field.Id,
        Name = f.Field.Name
      })).ToList<Planfix.Api.Entities.Analitics.Field>()));
      this.FormToSHow = (WindowWithSize) new FrmTaskSettings();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
    }
  }
}
