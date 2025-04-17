// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsCenter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  public class SmsCenter : ISms
  {
    private readonly SmsCenterRepository _repository;
    private readonly Integrations _integrations;

    public SmsCenter(Integrations integrations)
    {
      this._integrations = integrations;
      this._repository = new SmsCenterRepository(integrations.Sms.Login.DecryptedValue, integrations.Sms.Password.DecryptedValue);
    }

    public int SendSms(string text, List<string> phones, out List<string> errorList)
    {
      SendSmsAnswer sendSmsAnswer = this._repository.send_sms(string.Join(",", (IEnumerable<string>) phones), text, this._integrations.Sms.Sender);
      if (sendSmsAnswer.ErrorCode != 0)
      {
        errorList = new List<string>()
        {
          string.Format(Translate.SmsCenter_SendSms_Ошибка_отправки_СМС__0___код_ошибки__1__, (object) sendSmsAnswer.Error, (object) sendSmsAnswer.ErrorCode)
        };
        return 0;
      }
      errorList = new List<string>(sendSmsAnswer.Phones.Where<SendSmsAnswer.PhoneAnswer>((Func<SendSmsAnswer.PhoneAnswer, bool>) (x => !x.Error.IsNullOrEmpty())).Select<SendSmsAnswer.PhoneAnswer, string>((Func<SendSmsAnswer.PhoneAnswer, string>) (x => x.Phone + " - " + x.Error)));
      return sendSmsAnswer.Phones.Count<SendSmsAnswer.PhoneAnswer>((Func<SendSmsAnswer.PhoneAnswer, bool>) (x => x.Error.IsNullOrEmpty()));
    }

    public int SendSms(Dictionary<int, string> phonesAndMsg, out List<string> errorList)
    {
      throw new NotImplementedException();
    }

    public bool GetCodeForCall(string phone, out string code, out string error)
    {
      throw new NotImplementedException(Translate.SmsCenter_GetCodeForCall_К_сожалению__осуществлять_подтверждение_звонком_через_СМС_Центр_невозможно__Воспользуйтесь_другим_СМС_провайдером_или_выберите_вариант_подтвержления_по_СМС_);
    }

    public bool SendMsgByViber(string phone, string msg, out string error)
    {
      SendSmsAnswer sendSmsAnswer = this._repository.send_sms(phone, msg, this._integrations.Sms.Sender, format: 10);
      if (sendSmsAnswer.ErrorCode != 0)
      {
        error = string.Format(Translate.SmsCenter_SendMsgByViber_Ошибка_отправки_сообщения_Viber__0___код_ошибки__1__, (object) sendSmsAnswer.Error, (object) sendSmsAnswer.ErrorCode);
        return false;
      }
      error = string.Join('\n'.ToString(), sendSmsAnswer.Phones.Where<SendSmsAnswer.PhoneAnswer>((Func<SendSmsAnswer.PhoneAnswer, bool>) (x => !x.Error.IsNullOrEmpty())).Select<SendSmsAnswer.PhoneAnswer, string>((Func<SendSmsAnswer.PhoneAnswer, string>) (x => x.Phone + " - " + x.Error)));
      return sendSmsAnswer.Phones.All<SendSmsAnswer.PhoneAnswer>((Func<SendSmsAnswer.PhoneAnswer, bool>) (x => x.Error.IsNullOrEmpty()));
    }

    public bool SendMsgByWhatsApp(string phone, string msg, out string error)
    {
      SendSmsAnswer sendSmsAnswer = this._repository.send_sms(phone, msg, this._integrations.Sms.Sender, format: 12);
      if (sendSmsAnswer.ErrorCode != 0)
      {
        error = string.Format(Translate.SmsCenter_SendMsgByWhatsApp_Ошибка_отправки_сообщения_WA__0___код_ошибки__1__, (object) sendSmsAnswer.Error, (object) sendSmsAnswer.ErrorCode);
        return false;
      }
      error = string.Join('\n'.ToString(), sendSmsAnswer.Phones.Where<SendSmsAnswer.PhoneAnswer>((Func<SendSmsAnswer.PhoneAnswer, bool>) (x => !x.Error.IsNullOrEmpty())).Select<SendSmsAnswer.PhoneAnswer, string>((Func<SendSmsAnswer.PhoneAnswer, string>) (x => x.Phone + " - " + x.Error)));
      return sendSmsAnswer.Phones.All<SendSmsAnswer.PhoneAnswer>((Func<SendSmsAnswer.PhoneAnswer, bool>) (x => x.Error.IsNullOrEmpty()));
    }

    public Decimal GetBalance()
    {
      BalanceAnswer balance = this._repository.get_balance();
      if (balance.ErrorCode != 0)
        throw new Exception(string.Format(Translate.SmsCenter_GetBalance_Ошибка_проверки_баланса___0___код_ошибки__1__, (object) balance.Error, (object) balance.ErrorCode));
      Decimal result;
      return Decimal.TryParse(balance.Balance.Replace('.', ','), out result) ? result : -1M;
    }
  }
}
