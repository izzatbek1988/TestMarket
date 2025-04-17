// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Devices;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Helpers.Sms;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  internal class SmsHelper : ISms
  {
    private ISms Sms;

    public SmsHelper()
    {
      try
      {
        Integrations integrations = new ConfigsRepository<Integrations>().Get();
        if (integrations.Sms.Type == GlobalDictionaries.SmsServiceType.None)
          throw new DeviceException(Translate.SmsHelper_SmsHelper_Не_настроена_рассылка_СМС_в_настройках_программы__проведите_настройку_и_повторите_еще_раз_);
        LogHelper.Debug("Инициализация СМС, тип СМС: " + integrations.Sms.Type.ToString());
        ISms sms;
        switch (integrations.Sms.Type)
        {
          case GlobalDictionaries.SmsServiceType.SmsRu:
            sms = (ISms) new SmsRu(integrations.Sms.ApiKey.DecryptedValue);
            break;
          case GlobalDictionaries.SmsServiceType.SmsAgent:
            sms = (ISms) new SmsAgent(integrations);
            break;
          case GlobalDictionaries.SmsServiceType.SmsCenter:
            sms = (ISms) new SmsCenter(integrations);
            break;
          case GlobalDictionaries.SmsServiceType.SmsAero:
            sms = (ISms) new SmsAero(integrations);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        this.Sms = sms;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка инициализации класса CМС", false);
        throw ex;
      }
    }

    public int SendSms(string text, List<string> phones, out List<string> listError)
    {
      return this.Sms.SendSms(text, phones, out listError);
    }

    public bool SendSms(string text, string phone, out string erorr)
    {
      ISms sms = this.Sms;
      string text1 = text;
      List<string> phones = new List<string>();
      phones.Add(phone);
      List<string> source;
      ref List<string> local = ref source;
      int num = sms.SendSms(text1, phones, out local);
      erorr = source.SingleOrDefault<string>() ?? "";
      return num == 1;
    }

    public int SendSms(Dictionary<int, string> phonesAndMsg, out List<string> listError)
    {
      return this.Sms.SendSms(phonesAndMsg, out listError);
    }

    public bool GetCodeForCall(string phone, out string code, out string error)
    {
      return this.Sms.GetCodeForCall(phone, out code, out error);
    }

    public bool SendMsgByViber(string phone, string msg, out string error)
    {
      return this.Sms.SendMsgByViber(phone, msg, out error);
    }

    public bool SendMsgByWhatsApp(string phone, string msg, out string error)
    {
      return this.Sms.SendMsgByWhatsApp(phone, msg, out error);
    }

    public Decimal GetBalance() => this.Sms.GetBalance();

    public bool CheckingCode(
      string phone,
      GlobalDictionaries.ActionAuthType authType,
      Actions action)
    {
      Gbs.Core.Config.Sms sms = new ConfigsRepository<Integrations>().Get().Sms;
      if (sms.Type == GlobalDictionaries.SmsServiceType.None)
        return true;
      if (phone.IsNullOrEmpty())
      {
        MessageBoxHelper.Warning(Translate.ДляОтправкиКодаПодтвержденияНеобходимоУказатьНомерТелефонаВКарточкеКонтакта);
        return false;
      }
      string code = BarcodeHelper.RandomPass();
      string str1 = sms.TextSmsForCode.Replace("{code}", code);
      string str2 = "";
      bool flag;
      switch (authType)
      {
        case GlobalDictionaries.ActionAuthType.Sms:
          flag = this.SendSms(str1, phone.ClearPhone(), out str2);
          break;
        case GlobalDictionaries.ActionAuthType.Call:
          flag = this.GetCodeForCall(phone.ClearPhone(), out code, out str2);
          break;
        case GlobalDictionaries.ActionAuthType.Wa:
          flag = this.SendMsgByWhatsApp(phone.ClearPhone(), str1, out str2);
          break;
        case GlobalDictionaries.ActionAuthType.Viber:
          flag = this.SendMsgByViber(phone.ClearPhone(), str1, out str2);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (authType), (object) authType, (string) null);
      }
      if (!flag)
      {
        if (MessageBoxHelper.Show(string.Format(Translate.SmsHelper_CheckingCode_, (object) phone, (object) str2), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
          return false;
        using (Data.GetDataBase())
          return new Authorization().GetAccess(action).Result;
      }
      else
      {
        while (true)
        {
          (bool result, string output) tuple = MessageBoxHelper.Input("", authType == GlobalDictionaries.ActionAuthType.Call ? Translate.SmsHelper_CheckingCode_Введите_4_последние_цифры_номера_телефона__с_которого_поступил_звонок_покупателю_ : Translate.SmsHelper_CheckingCode_Введите_код_подтверждения__который_пришел_покупателю_в_СМС_, 4, buttons: MessageBoxButton.OKCancel, icon: MessageBoxImage.Question);
          if (tuple.result)
          {
            if (code != tuple.output)
              MessageBoxHelper.Error(Translate.SmsHelper_CheckingCode_Код_подтверждения_введен_неверно__попробуйте_еще_раз_или_нажмите_кнопку_Отмена_);
            else
              goto label_22;
          }
          else
            break;
        }
        return false;
label_22:
        return true;
      }
    }
  }
}
