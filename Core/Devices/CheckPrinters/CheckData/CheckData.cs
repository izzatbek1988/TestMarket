// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckData.CheckData
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.ErrorHandler.Exceptions;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckData
{
  public class CheckData
  {
    public string TrueApiInfoForKkm { get; set; }

    public List<EntityProperties.PropertyValue> Properties { get; set; } = new List<EntityProperties.PropertyValue>();

    public string FiscalNum { get; set; }

    public string FrNumber { get; set; }

    public List<CheckGood> GoodsList { get; set; } = new List<CheckGood>();

    public List<CheckPayment> PaymentsList { get; set; }

    public Decimal GetTotalGoodsSum()
    {
      return this.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
    }

    public Decimal GetTotalPaymentsSum()
    {
      return this.PaymentsList.Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
    }

    public Decimal DiscountSum { get; set; }

    public CheckFiscalTypes FiscalType { get; set; }

    public CheckTypes CheckType { get; set; }

    public Cashier Cashier { get; set; }

    public string AddressForDigitalCheck { get; set; }

    public DateTime DateTime { get; set; } = DateTime.Now;

    public string Comment { get; set; }

    public string Number { get; set; } = string.Empty;

    public ClientAdnSum Client { get; set; }

    public GlobalDictionaries.RuTaxSystems RuTaxSystem { get; set; } = GlobalDictionaries.RuTaxSystems.None;

    public Decimal Delivery
    {
      get
      {
        Decimal num1 = this.PaymentsList.Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
        Decimal num2 = this.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Cash)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
        Decimal num3 = this.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum));
        Decimal delivery = num1 - num3;
        if (delivery > num2)
          delivery = 0M;
        if (num1 < num3)
          delivery = 0M;
        return delivery;
      }
    }

    public Decimal ReceiveSum { get; set; }

    public CheckData(
      List<CheckGood> goodsList,
      List<CheckPayment> paymentsList,
      CheckFiscalTypes fiscalType,
      Cashier cashier)
    {
      this.GoodsList = goodsList;
      this.PaymentsList = paymentsList;
      this.FiscalType = fiscalType;
      this.Cashier = cashier;
    }

    public CheckData()
    {
    }

    public Dictionary<string, object> CustomData { get; set; } = new Dictionary<string, object>();

    public void VerifyData()
    {
      LogHelper.Debug("Начинаю проверку данныех чека");
      Device device = new Device(IDevice.DeviceTypes.Kkm, Translate.CheckData_VerifyData_ККМ);
      if (!this.GoodsList.Any<CheckGood>())
        throw new ErrorHelper.GbsException(Translate.CheckData_В_чеке_должен_быть_хотя_бы_одна_товарная_позиция)
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      if (this.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (x => x.Sum)) == 0M)
        throw new ErrorHelper.GbsException(Translate.CheckData_Сумма_товаров_в_чеке_не_должна_быть_равна_нулю)
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      if (!this.PaymentsList.Any<CheckPayment>())
        throw new ErrorHelper.GbsException(Translate.CheckData_В_чеке_должны_быть_хотя_бы_один_платеж)
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      Decimal num1 = this.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method != GlobalDictionaries.KkmPaymentMethods.Cash && p.Method != GlobalDictionaries.KkmPaymentMethods.Bonus)).Sum<CheckPayment>((Func<CheckPayment, Decimal>) (p => p.Sum));
      Decimal num2 = this.GoodsList.Sum<CheckGood>((Func<CheckGood, Decimal>) (g => g.Sum)) - this.DiscountSum;
      if (this.PaymentsList.Any<CheckPayment>((Func<CheckPayment, bool>) (p => p.Method != 0)) && num1 > num2)
        throw new ErrorHelper.GbsException(string.Format(Translate.CheckData_Сумма_безналичных_платежей__0__больше_суммы_товаров_в_чеке__1_, (object) num1, (object) num2))
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      Decimal num3 = this.PaymentsList.Sum<CheckPayment>((Func<CheckPayment, Decimal>) (x => x.Sum));
      if (num3 < num2)
        throw new ErrorHelper.GbsException(string.Format(Translate.CheckData_VerifyData_, (object) num2, (object) num3))
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      if (num1 == num2)
      {
        foreach (CheckPayment checkPayment in this.PaymentsList.Where<CheckPayment>((Func<CheckPayment, bool>) (x => x.Method == GlobalDictionaries.KkmPaymentMethods.Cash)))
          checkPayment.Sum = 0M;
      }
      if (this.GoodsList.Any<CheckGood>((Func<CheckGood, bool>) (g => g.Discount > 100M || g.Discount < 0M)))
        throw new ErrorHelper.GbsException(Translate.CheckData_Скидка_на_один_или_несколько_товаров_меньше_0__или_больше_100_)
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      if (this.GoodsList.Any<CheckGood>((Func<CheckGood, bool>) (g => g.Price > 100000000000M || g.Price < 0M)))
        throw new ErrorHelper.GbsException(Translate.CheckData_Стоимость_одного_или_нескольких_товаров_в_чеке_меньше_нуля_или_больше_допустимого_значения)
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      if (this.GoodsList.Any<CheckGood>((Func<CheckGood, bool>) (g => g.Quantity > 100000000000M || g.Quantity <= 0M)))
        throw new ErrorHelper.GbsException(Translate.CheckData_Количество_одного_или_нескольких_товаров_в_чеке_меньше_или_равно_нулю_или_больше_допустимого_значения)
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
      if (this.PaymentsList.Any<CheckPayment>((Func<CheckPayment, bool>) (p => p.Sum > 100000000000M || p.Sum < 0M)))
        throw new ErrorHelper.GbsException(Translate.CheckData_Сумма_одного_или_нескольких_платежей_меньше_нуля_или_больше_допустимого_значения)
        {
          Direction = ErrorHelper.ErrorDirections.Outer,
          Level = MsgException.LevelTypes.Warm
        };
    }

    [Obsolete("Метод вынесен в CheckFactory")]
    public static GlobalDictionaries.RuTaxSystems GetRuTaxSystemForGoods(List<Good> goods)
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      LogHelper.Debug(string.Format("Устанавливаю СНО для чека. СНО в натройках по умаолчанию: {0}", (object) devices.CheckPrinter.FiscalKkm.DefaultRuTaxSystem));
      List<GlobalDictionaries.RuTaxSystems> list = goods.Select<Good, GlobalDictionaries.RuTaxSystems>((Func<Good, GlobalDictionaries.RuTaxSystems>) (g => g.Group.RuTaxSystem)).ToList<GlobalDictionaries.RuTaxSystems>().Distinct<GlobalDictionaries.RuTaxSystems>().ToList<GlobalDictionaries.RuTaxSystems>();
      if (list.Any<GlobalDictionaries.RuTaxSystems>((Func<GlobalDictionaries.RuTaxSystems, bool>) (x => x == GlobalDictionaries.RuTaxSystems.None)))
        list.Add(devices.CheckPrinter.FiscalKkm.DefaultRuTaxSystem);
      if (devices.CheckPrinter.FiscalKkm.DefaultRuTaxSystem != GlobalDictionaries.RuTaxSystems.None)
        list.RemoveAll((Predicate<GlobalDictionaries.RuTaxSystems>) (x => x == GlobalDictionaries.RuTaxSystems.None));
      switch (list.Distinct<GlobalDictionaries.RuTaxSystems>().Count<GlobalDictionaries.RuTaxSystems>())
      {
        case 0:
          return devices.CheckPrinter.FiscalKkm.DefaultRuTaxSystem;
        case 1:
          return list.First<GlobalDictionaries.RuTaxSystems>();
        default:
          throw new ErrorHelper.GbsException(Translate.CheckData_Чек_содержит_товары_с_разной_СНО__В_чеке_не_могут_быть_товары_с_разной_СНО)
          {
            Direction = ErrorHelper.ErrorDirections.Outer,
            Level = MsgException.LevelTypes.Warm
          };
      }
    }

    [Obsolete("Метод вынесен в CheckFactory")]
    public void SetAndCheckTaxSystem()
    {
      Gbs.Core.Config.Devices devices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      this.RuTaxSystem = Gbs.Core.Devices.CheckPrinters.CheckData.CheckData.GetRuTaxSystemForGoods(this.GoodsList.Select<CheckGood, Good>((Func<CheckGood, Good>) (x => x.Good)).ToList<Good>());
      LogHelper.Debug(string.Format("Устанавливаю СНО для чека: {0} СНО в натройках по умаолчанию: {1}", (object) this.RuTaxSystem, (object) devices.CheckPrinter.FiscalKkm.DefaultRuTaxSystem));
    }
  }
}
