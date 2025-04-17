// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ViewModels.Cafe.CafeBasket
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Basket;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Factories;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Core.ViewModels.Cafe
{
  public class CafeBasket : Gbs.Core.ViewModels.Basket.Basket
  {
    private int _countGuest = 1;
    private int _numTable;

    public Guid CafeOrderUid { get; set; } = Guid.Empty;

    public CafeBasket()
    {
      this.SaleNumber = SalePoints.GetSalePointList().First<SalePoints.SalePoint>()?.Number.SaleNumber.ToString() ?? "";
    }

    public int CountGuest
    {
      get => this._countGuest;
      set
      {
        this._countGuest = value;
        this.OnPropertyChanged(nameof (CountGuest));
      }
    }

    public int NumTable
    {
      get => this._numTable;
      set
      {
        this._numTable = value;
        this.OnPropertyChanged(nameof (NumTable));
      }
    }

    public ICommand DeleteCommand
    {
      get => (ICommand) new RelayCommand(new Action<object>(this.DeleteItems));
    }

    private void DeleteItems(object obj)
    {
      List<BasketItem> castedList;
      if (!new Authorization().GetAccess(Actions.DeleteItemBasket).Result || !this.CheckSelectedItems(obj, out castedList) || MessageBoxHelper.Show(string.Format(Translate.GoodsList_Вы_уверены__что_хотите_удалить__0__записей_, (object) castedList.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
        return;
      Gbs.Core.Config.Cafe cafe = new ConfigsRepository<Gbs.Core.Config.Cafe>().Get();
      string str = string.Empty;
      if (cafe.IsNeedCommentForDelete)
      {
        (bool result, string output) tuple = MessageBoxHelper.Input("", Translate.CafeBasket_Укажите_причину_удаления_товаров_из_списка);
        str = tuple.result ? tuple.output : "";
      }
      foreach (BasketItem basketItem in castedList)
      {
        if (basketItem.Good.Uid == GlobalDictionaries.PercentForServiceGoodUid)
          this.IsDeletedPercentForServiceGood = true;
        basketItem.Comment = str;
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory((IEntity) basketItem, (IEntity) basketItem, ActionType.Delete, GlobalDictionaries.EntityTypes.ItemList, this.User), false);
        this.Items.Remove(basketItem);
      }
      if (!this.Items.Any<BasketItem>())
        this.Storage = (Storages.Storage) null;
      this.ReCalcTotals();
      this.OnPropertyChanged("Items");
    }

    public override void ReCalcTotals()
    {
      if (new ConfigsRepository<Gbs.Core.Config.Cafe>().Get().IsPercentForService)
        this.SetPercentForServiceGood();
      base.ReCalcTotals();
    }

    public ActionResult SaveCafe(bool isSaveClose, Guid uidOrder, bool isPrintExtra)
    {
      try
      {
        ActionResult actionResult1 = this.PrepareCheck();
        if (actionResult1.Result != ActionResult.Results.Ok)
          return actionResult1;
        if (isSaveClose)
        {
          ActionResult actionResult2 = this.CheckStatusKkm(this._devices.CheckPrinter.PrintCheckOnEverySale || this._devices.CheckPrinter.IsShowPrintConfirmationForm);
          if (actionResult2.Result != ActionResult.Results.Ok)
            return actionResult2;
          ActionResult paymentsByDocument = this.GetPaymentsByDocument(true);
          if (paymentsByDocument.Result != ActionResult.Results.Ok)
            return paymentsByDocument;
        }
        Document document = this.Document;
        this.SaleNumber = (document != null ? (document.Number.IsNullOrEmpty() ? 1 : 0) : 1) != 0 ? Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.Sale) : this.Document.Number;
        this.Document = new DocumentsFactory().Create((Gbs.Core.ViewModels.Basket.Basket) this, true, isSaveClose, uidOrder);
        List<EntityProperties.PropertyValue> collection1 = new List<EntityProperties.PropertyValue>();
        EntityProperties.PropertyValue propertyValue1 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType1 = new EntityProperties.PropertyType();
        propertyType1.Uid = GlobalDictionaries.CountGuestUid;
        propertyValue1.Type = propertyType1;
        propertyValue1.Value = (object) this.CountGuest;
        propertyValue1.EntityUid = this.Document.Uid;
        collection1.Add(propertyValue1);
        EntityProperties.PropertyValue propertyValue2 = new EntityProperties.PropertyValue();
        EntityProperties.PropertyType propertyType2 = new EntityProperties.PropertyType();
        propertyType2.Uid = GlobalDictionaries.NumTableUid;
        propertyValue2.Type = propertyType2;
        propertyValue2.Value = (object) this.NumTable;
        propertyValue2.EntityUid = this.Document.Uid;
        collection1.Add(propertyValue2);
        this.Document.Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) collection1);
        this.UpdatePaymentListForCertificate(this.Payments, this.CertificatesPaymentSum);
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
          if (documentsRepository.Validate(this.Document).Result != ActionResult.Results.Ok)
            return new ActionResult(ActionResult.Results.Cancel);
          List<EntityProperties.PropertyValue> collection2 = this.Document.Properties.Clone<List<EntityProperties.PropertyValue>>();
          Document docSale = new DocumentsFactory().Create((Gbs.Core.ViewModels.Basket.Basket) this);
          docSale.Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) this.Document.Properties.Clone<List<EntityProperties.PropertyValue>>());
          docSale.Properties.ForEach((Action<EntityProperties.PropertyValue>) (x =>
          {
            x.Uid = Guid.NewGuid();
            x.EntityUid = docSale.Uid;
          }));
          if (isSaveClose)
          {
            docSale.ParentUid = this.Document.Uid;
            if (documentsRepository.Validate(docSale).Result != ActionResult.Results.Ok)
              return new ActionResult(ActionResult.Results.Cancel);
            bool isShowForm = this._devices.CheckPrinter.Type != GlobalDictionaries.Devices.CheckPrinterTypes.None && this._isPrintCheckLocal && (this._devices.CheckPrinter.IsShowPrintConfirmationForm || this._devices.CheckPrinter.PrintCheckOnEverySale && this._devices.CheckPrinter.FiscalKkm.AllowSalesWithoutCheck);
            ActionPrintViewModel.TypePrint type = ActionPrintViewModel.TypePrint.NoCheck;
            if (isShowForm)
            {
              (ActionPrintViewModel.TypePrint type, bool isPrint) typePrint = new ActionPrintViewModel().GetTypePrint((Gbs.Core.ViewModels.Basket.Basket) this);
              type = typePrint.type;
              if (!typePrint.isPrint)
                return new ActionResult(ActionResult.Results.Cancel);
            }
            if (!this.PayAcquiring(this._sumForAcquiring, docSale))
            {
              ProgressBarHelper.Close();
              return new ActionResult(ActionResult.Results.Cancel);
            }
            this.Document.Properties.AddRange(docSale.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => this.Document.Properties.All<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid != x.Type.Uid)))));
            if (!this.PaySBP(this._sumForSbp, docSale))
            {
              ProgressBarHelper.Close();
              return new ActionResult(ActionResult.Results.Cancel);
            }
            ActionResult actionResult3 = this.PrintOrShowCheck(this._devices.CheckPrinter.PrintCheckOnEverySale, isShowForm, type);
            if (actionResult3.Result != ActionResult.Results.Ok)
              return actionResult3;
            this.Document.Properties.AddRange(docSale.Properties.Where<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => this.Document.Properties.All<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Type.Uid != x.Type.Uid)))));
            this.Document.Properties.ForEach((Action<EntityProperties.PropertyValue>) (x => x.Uid = Guid.NewGuid()));
            if (this.Document.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FiscalNumUid)))
              docSale.Properties.Add(this.Document.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FiscalNumUid)));
            docSale.Properties.ForEach((Action<EntityProperties.PropertyValue>) (x => x.EntityUid = docSale.Uid));
          }
          if (isPrintExtra)
            ExtraPrinters.PrepareExtraPrint(this.Document);
          this.Document.Payments.Clear();
          this.Document.Properties = new List<EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyValue>) collection2);
          if (!documentsRepository.Save(this.Document))
            return new ActionResult(ActionResult.Results.Error, Translate.Basket_Не_удалось_сохранить_продажу_в_базу_данных);
          if (isPrintExtra)
            ExtraPrinters.Print(this.Document);
          if (isSaveClose)
          {
            docSale.IsFiscal = this.Document.IsFiscal;
            if (!this.TrueApiInfoForKkm.IsNullOrEmpty())
            {
              List<EntityProperties.PropertyValue> properties = docSale.Properties;
              EntityProperties.PropertyValue propertyValue3 = new EntityProperties.PropertyValue();
              EntityProperties.PropertyType propertyType3 = new EntityProperties.PropertyType();
              propertyType3.Uid = GlobalDictionaries.InfoWithTrueApiUid;
              propertyValue3.Type = propertyType3;
              propertyValue3.Value = (object) this.TrueApiInfoForKkm;
              propertyValue3.EntityUid = docSale.Uid;
              properties.Add(propertyValue3);
            }
            if (!documentsRepository.Save(docSale))
              return new ActionResult(ActionResult.Results.Error, Translate.Basket_Не_удалось_сохранить_продажу_в_базу_данных);
            new BonusHelper().UpdateSumBonusesForSale(docSale);
            this.CommitRotationCheck();
            this.SaveEgaisItems(docSale);
            this.SaveTapBeerItems(this.Items.ToList<BasketItem>());
          }
          this.SaveCertificates();
          this.RemoteControlActions();
          return new ActionResult(ActionResult.Results.Ok);
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.WriteError(ex, "Ошибка сохранения корзины в продажу");
        return new ActionResult(ActionResult.Results.Error, Translate.Basket_Не_удалось_сохранить_корзину_в_продажу + Other.NewLine(2) + ex.Message);
      }
    }
  }
}
