// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2.CheckVisualizer
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters.CheckData;
using Gbs.Core.Entities;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckPrepare_v2
{
  public class CheckVisualizer
  {
    private const int PaperWidth = 60;
    private readonly string _smallSeparator = new string('-', 60);
    private readonly string _bigSeparator = new string('=', 60);

    private Gbs.Core.Devices.CheckPrinters.CheckData.CheckData Data { get; }

    private CheckPrinter CheckPrinterConfig { get; }

    private List<CheckVisualizer.CheckString> StringsList { get; } = new List<CheckVisualizer.CheckString>();

    public CheckVisualizer(Gbs.Core.Devices.CheckPrinters.CheckData.CheckData data, CheckPrinter checkPrinterConfig)
    {
      this.Data = data;
      this.CheckPrinterConfig = checkPrinterConfig;
    }

    public string Do()
    {
      this.PrepareCheckHeader();
      this.StringsList.Add(new CheckVisualizer.CheckString(this._bigSeparator));
      this.PrepareGoods();
      this.StringsList.Add(new CheckVisualizer.CheckString(this._bigSeparator));
      this.PreparePayments();
      this.StringsList.Add(new CheckVisualizer.CheckString(this._bigSeparator));
      this.PrepareFooter();
      this.StringsList.Add(new CheckVisualizer.CheckString(new string(new char[2]
      {
        '\r',
        '\n'
      })));
      return string.Join(new string(new char[2]
      {
        '\r',
        '\n'
      }), this.StringsList.Select<CheckVisualizer.CheckString, string>((Func<CheckVisualizer.CheckString, string>) (x => x.Value)));
    }

    private void PrepareFooter()
    {
      GlobalDictionaries.RuTaxSystems ruTaxSystem = this.Data.RuTaxSystem;
      string footerНеОпределенаTax = Translate.CheckVisualizer_PrepareFooter_НЕ_ОПРЕДЕЛЕНА__tax_;
      if (!GlobalDictionaries.RuTaxSystemsDictionary().ContainsKey(ruTaxSystem))
        throw new ArgumentOutOfRangeException("СНО не определена");
      this.StringsList.Add(new CheckVisualizer.CheckString("Кассир: " + this.Data.Cashier.Name + "; " + this.Data.Cashier.Inn + "         СНО: " + GlobalDictionaries.RuTaxSystemsDictionary()[ruTaxSystem]));
      if (this.CheckPrinterConfig.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
      {
        if (this.Data.Client != null)
          this.StringsList.Add(new CheckVisualizer.CheckString("Покупатель: " + this.Data.Client?.Client.Name));
        if (!this.Data.AddressForDigitalCheck.IsNullOrEmpty())
          this.StringsList.Add(new CheckVisualizer.CheckString("Адрес для отправки чека: " + this.Data.AddressForDigitalCheck));
      }
      this.StringsList.Add(new CheckVisualizer.CheckString(string.Format("{0}; {1}", (object) this.Data.FiscalType, (object) this.Data.CheckType)));
      this.StringsList.Add(new CheckVisualizer.CheckString(string.Format("ТИП: {0}; ККМ: {1}; ФФД: {2}", (object) this.CheckPrinterConfig.Type, (object) this.CheckPrinterConfig.FiscalKkm.KkmType, (object) this.CheckPrinterConfig.FiscalKkm.FfdVersion)));
    }

    private void PreparePayments()
    {
      if (this.Data.DiscountSum != 0M)
      {
        this.StringsList.Add(new CheckVisualizer.CheckString("ИТОГО: " + this.Data.GetTotalGoodsSum().ToString("N2").PadLeft(10), TextAlignment.Right));
        this.StringsList.Add(new CheckVisualizer.CheckString("Скидка на чек: " + this.Data.DiscountSum.ToString("N2").PadLeft(10), TextAlignment.Right));
      }
      this.StringsList.Add(new CheckVisualizer.CheckString("К ОПЛАТЕ: " + (this.Data.GetTotalGoodsSum() - this.Data.DiscountSum).ToString("N2").PadLeft(10), TextAlignment.Right));
      this.StringsList.Add(new CheckVisualizer.CheckString(""));
      foreach (CheckPayment payments in this.Data.PaymentsList)
      {
        string str = payments.Sum.ToString("N2").PadLeft(10);
        this.StringsList.Add(new CheckVisualizer.CheckString(string.Format("{0} ({1}): {2}", (object) payments.Name, (object) payments.Method, (object) str), TextAlignment.Right));
      }
      this.StringsList.Add(new CheckVisualizer.CheckString("Сдача: " + this.Data.Delivery.ToString("N2").PadLeft(10), TextAlignment.Right));
    }

    private void PrepareCheckHeader()
    {
      this.StringsList.Add(new CheckVisualizer.CheckString("\r\n"));
      this.StringsList.Add(new CheckVisualizer.CheckString(string.Format("Чек № {0}; {1:dd-MM-yyyy HH:mm}", (object) this.Data.Number, (object) DateTime.Now)));
    }

    private void PrepareGoods()
    {
      foreach (CheckGood goods in this.Data.GoodsList)
      {
        this.StringsList.Add(new CheckVisualizer.CheckString(goods.Name));
        string str1 = string.Empty;
        if (goods.Discount > 0M)
          str1 = string.Format("- {0:N2} ({1:N2}%)", (object) goods.DiscountSum, (object) goods.Discount);
        this.StringsList.Add(new CheckVisualizer.CheckString(string.Format(" {0:N2} * {1:N3} {2} = {3:N2}", (object) goods.Price, (object) goods.Quantity, (object) str1, (object) goods.Sum), TextAlignment.Right));
        int defaultTaxRate = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.DefaultTaxRate;
        int key = goods.TaxRateNumber == 0 ? defaultTaxRate : goods.TaxRateNumber;
        FiscalKkm.TaxRate taxRate = new FiscalKkm.TaxRate(0M, "[ERROR]", 0);
        if (this.CheckPrinterConfig.FiscalKkm.TaxRates.ContainsKey(key))
          taxRate = this.CheckPrinterConfig.FiscalKkm.TaxRates[key];
        this.StringsList.Add(new CheckVisualizer.CheckString(string.Format("Налог {0}: {1} ({2}%) ", (object) key, (object) taxRate.Name, (object) taxRate.TaxValue), TextAlignment.Right));
        if (this.CheckPrinterConfig.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.OfflineKkm)
        {
          this.StringsList.Add(new CheckVisualizer.CheckString(GlobalDictionaries.RuFfdGoodsTypesDictionary()[goods.RuFfdGoodTypeCode] + "   |   " + GlobalDictionaries.RuFfdPaymentModesDictionary()[goods.RuFfdPaymentModeCode], TextAlignment.Right));
          if (goods.MarkedInfo != null && goods.MarkedInfo.Type != GlobalDictionaries.RuMarkedProductionTypes.None)
            this.StringsList.Add(new CheckVisualizer.CheckString(string.Format("[M] : {0}; {1}", (object) goods.MarkedInfo.Type, (object) goods.MarkedInfo.FullCode)));
        }
        foreach (string str2 in goods.CommentForFiscalCheck)
          this.StringsList.Add(new CheckVisualizer.CheckString(str2 ?? ""));
        this.StringsList.Add(new CheckVisualizer.CheckString(this._smallSeparator));
      }
    }

    private class CheckString
    {
      public string Value { get; set; }

      [Localizable(false)]
      public CheckString(string s, TextAlignment alignment = TextAlignment.Left)
      {
        switch (alignment)
        {
          case TextAlignment.Left:
          case TextAlignment.Center:
          case TextAlignment.Justify:
            this.Value = s;
            break;
          case TextAlignment.Right:
            s = s.PadLeft(60);
            goto case TextAlignment.Left;
          default:
            throw new ArgumentOutOfRangeException(nameof (alignment), (object) alignment, (string) null);
        }
      }
    }
  }
}
