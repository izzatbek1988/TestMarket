// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.ConfigsCorrector
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Forms.Main
{
  public class ConfigsCorrector
  {
    public void Do()
    {
      this.AddIndexesToTaxRatesConfigs();
      this.ConvertPlanfixSetting();
      this.CopyPluUid();
      this.TransferSyncClient();
      this.SetDefaultMethodPaymentUid();
      this.CorrectUrlForEgais();
      this.CorrectNdsForRus();
    }

    private void SetDefaultMethodPaymentUid()
    {
      ConfigsRepository<Settings> configsRepository = new ConfigsRepository<Settings>();
      Settings config = configsRepository.Get();
      if (config.Payments.DefaultMethodPaymentUid.HasValue)
        return;
      LogHelper.Debug("Указываем способ оплаты по-умолчанию");
      Sections.Section section = Sections.GetCurrentSection();
      PaymentMethods.PaymentMethod paymentMethod = PaymentMethods.GetActionPaymentsList().FirstOrDefault<PaymentMethods.PaymentMethod>((Func<PaymentMethods.PaymentMethod, bool>) (x => x.SectionUid == section.Uid));
      // ISSUE: explicit non-virtual call
      config.Payments.DefaultMethodPaymentUid = new Guid?(paymentMethod != null ? __nonvirtual (paymentMethod.Uid) : Guid.Empty);
      configsRepository.Save(config);
    }

    private void TransferSyncClient()
    {
      ConfigsRepository<Settings> configsRepository = new ConfigsRepository<Settings>();
      Settings config = configsRepository.Get();
      if (config.RemoteControl.Client == null)
        return;
      LogHelper.Debug("Переносим опцию синхронизации контактов в новую ветку");
      config.Clients.SyncMode = config.RemoteControl.Client.IsActive ? GlobalDictionaries.ClientSyncModes.FileSync : GlobalDictionaries.ClientSyncModes.None;
      config.RemoteControl.Client = (Gbs.Core.Config.ClientInfo) null;
      configsRepository.Save(config);
    }

    private void AddIndexesToTaxRatesConfigs()
    {
      ConfigsRepository<Gbs.Core.Config.Devices> configsRepository = new ConfigsRepository<Gbs.Core.Config.Devices>();
      Gbs.Core.Config.Devices config = configsRepository.Get();
      Dictionary<int, FiscalKkm.TaxRate> taxRates = config.CheckPrinter.FiscalKkm.TaxRates;
      if (taxRates.Any<KeyValuePair<int, FiscalKkm.TaxRate>>((Func<KeyValuePair<int, FiscalKkm.TaxRate>, bool>) (x => x.Value.KkmIndex != 0)))
        return;
      LogHelper.Debug("Корректировка настроек налоговых ставок");
      foreach (KeyValuePair<int, FiscalKkm.TaxRate> keyValuePair in taxRates)
        keyValuePair.Value.KkmIndex = keyValuePair.Key;
      configsRepository.Save(config);
    }

    private void CopyPluUid()
    {
      ConfigsRepository<Settings> configsRepository1 = new ConfigsRepository<Settings>();
      Settings config1 = configsRepository1.Get();
      if (!config1.GoodsConfig.PluUid.HasValue)
        return;
      LogHelper.Debug("Переносим код Plu в новые настройки");
      ConfigsRepository<Gbs.Core.Config.Devices> configsRepository2 = new ConfigsRepository<Gbs.Core.Config.Devices>();
      Gbs.Core.Config.Devices config2 = configsRepository2.Get();
      ScaleWithLable scaleWithLable = config2.ScaleWithLable;
      Guid? nullable1 = config1.GoodsConfig.PluUid;
      Guid guid = nullable1 ?? Guid.Empty;
      scaleWithLable.PluUid = guid;
      GoodsConfig goodsConfig = config1.GoodsConfig;
      nullable1 = new Guid?();
      Guid? nullable2 = nullable1;
      goodsConfig.PluUid = nullable2;
      configsRepository2.Save(config2);
      configsRepository1.Save(config1);
    }

    private void ConvertPlanfixSetting()
    {
      ConfigsRepository<Integrations> configsRepository = new ConfigsRepository<Integrations>();
      Integrations config = configsRepository.Get();
      string str1 = config.Planfix.KeyApi.IsNullOrEmpty() ? "" : CryptoHelper.StringCrypter.Decrypt(config.Planfix.KeyApi);
      string str2 = config.Planfix.Token.IsNullOrEmpty() ? "" : CryptoHelper.StringCrypter.Decrypt(config.Planfix.Token);
      if (!str1.IsNullOrEmpty() && !str2.IsNullOrEmpty())
        return;
      LogHelper.Debug("Корректировка настроек Планфикса");
      if (str1.IsNullOrEmpty())
        config.Planfix.KeyApi = CryptoHelper.StringCrypter.Encrypt(config.Planfix.KeyApi);
      if (str2.IsNullOrEmpty())
        config.Planfix.Token = CryptoHelper.StringCrypter.Encrypt(config.Planfix.Token);
      configsRepository.Save(config);
    }

    private void CorrectUrlForEgais()
    {
      Integrations config = new ConfigsRepository<Integrations>().Get();
      if (!config.Egais.Port.HasValue || config.Egais.PathUtm.IsNullOrEmpty())
        return;
      LogHelper.Debug("Корректировка настроек подключения к ЕГАИС");
      config.Egais.PathUtm = !config.Egais.PathUtm.EndsWith(":") ? config.Egais.PathUtm + ":" + config.Egais.Port.ToString() : config.Egais.PathUtm + config.Egais.Port.ToString();
      config.Egais.Port = new int?();
      new ConfigsRepository<Integrations>().Save(config);
    }

    private void CorrectNdsForRus()
    {
      if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia)
        return;
      Gbs.Core.Config.Devices config = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
      if (config.CheckPrinter.FiscalKkm.TaxRates.Count != 6)
        return;
      LogHelper.Debug("Добавляем новые ставки НДС");
      config.CheckPrinter.FiscalKkm.TaxRates.Add(7, new FiscalKkm.TaxRate(5M, Translate.FiscalKkm_TaxRates_НДС_5_, 7));
      config.CheckPrinter.FiscalKkm.TaxRates.Add(8, new FiscalKkm.TaxRate(7M, Translate.FiscalKkm_TaxRates_НДС_7_, 8));
      config.CheckPrinter.FiscalKkm.TaxRates.Add(9, new FiscalKkm.TaxRate(5M, Translate.FiscalKkm_TaxRates_НДС_5_105, 9));
      config.CheckPrinter.FiscalKkm.TaxRates.Add(10, new FiscalKkm.TaxRate(7M, Translate.FiscalKkm_TaxRates_НДС_7_107, 10));
      new ConfigsRepository<Gbs.Core.Config.Devices>().Save(config);
    }
  }
}
