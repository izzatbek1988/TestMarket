// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Sms.SmsRu
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

#nullable disable
namespace Gbs.Helpers.Sms
{
  public class SmsRu : ISms
  {
    private readonly SmsRuRepository _repository;

    public SmsRu(string apiKey) => this._repository = new SmsRuRepository(apiKey);

    public int SendSms(string text, List<string> phones, out List<string> errorList)
    {
      SmsRuRepository.SendEqualSmsCommand c = new SmsRuRepository.SendEqualSmsCommand()
      {
        Phones = new List<string>((IEnumerable<string>) phones),
        SmsText = text
      };
      this._repository.DoCommand((SmsRuRepository.Command) c);
      if (c.Result.StatusCode != 100)
        throw new Exception(Translate.SmsRu_SendSms_Во_время_отправки_СМС___произошла_ошибка__ + c.Result.StatusCode.ToString());
      List<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)> list = phones.Select<string, (string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<string, (string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>) (x => (x, SmsRuRepository.SendEqualSmsCommand.GetPropertyValue(c.Answer.SmsAnswers, x)))).ToList<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>();
      errorList = new List<string>();
      if (list.Any<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), bool>) (x => x.Item2.StatusCode != 100)))
        errorList.AddRange(list.Where<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), bool>) (x => x.Item2.StatusCode != 100)).Select<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), string>((Func<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), string>) (x => string.Format("{0}: {1} ({2})", (object) x.x, (object) x.Item2.StatusText, (object) x.Item2.StatusCode))));
      return list.Count<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<(string, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), bool>) (x => x.Item2.StatusCode == 100));
    }

    public int SendSms(Dictionary<int, string> phonesAndMsg, out List<string> errorList)
    {
      SmsRuRepository.SendOtherSmsCommand c = new SmsRuRepository.SendOtherSmsCommand()
      {
        Phones = phonesAndMsg
      };
      this._repository.DoCommand((SmsRuRepository.Command) c);
      if (c.Result.StatusCode != 100)
        throw new Exception(Translate.SmsRu_SendSms_Во_время_отправки_СМС___произошла_ошибка__ + c.Result.StatusCode.ToString());
      List<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)> list = phonesAndMsg.Select<KeyValuePair<int, string>, (KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<KeyValuePair<int, string>, (KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>) (x => (x, SmsRuRepository.SendEqualSmsCommand.GetPropertyValue(c.Answer.SmsAnswers, x.Key.ToString())))).ToList<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>();
      errorList = new List<string>();
      if (list.Any<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), bool>) (x => x.Item2.StatusCode != 100)))
        errorList.AddRange(list.Where<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), bool>) (x => x.Item2.StatusCode != 100)).Select<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), string>((Func<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), string>) (x => string.Format("{0}: {1} ({2})", (object) x.x.Key, (object) x.Item2.StatusText, (object) x.Item2.StatusCode))));
      return list.Count<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer)>((Func<(KeyValuePair<int, string>, SmsRuRepository.SendEqualSmsCommand.AnswerCommand.SmsAnswer), bool>) (x => x.Item2.StatusCode == 100));
    }

    public bool GetCodeForCall(string phone, out string code, out string erorr)
    {
      erorr = "";
      code = "";
      string str = new WebClient().DownloadString("https://ipv4-internet.yandex.net/api/v0/ip").Replace("\"", "");
      SmsRuRepository.SendPhoneCommand sendPhoneCommand = new SmsRuRepository.SendPhoneCommand()
      {
        Phone = phone,
        Ip = str
      };
      this._repository.DoCommand((SmsRuRepository.Command) sendPhoneCommand);
      if (sendPhoneCommand.Answer.Status != "OK")
      {
        erorr = sendPhoneCommand.Answer.Status;
        return false;
      }
      code = sendPhoneCommand.Answer.Code;
      return true;
    }

    public bool SendMsgByViber(string phone, string msg, out string error)
    {
      throw new NotImplementedException(Translate.SmsRu_SendMsgByViber_К_сожалению__отправлять_сообщения_с_помощью_Viber_через_этот_СМС_провайдер_невозможно__Воспользуйтесь_другим_СМС_провайдером_или_выберите_другой_вариант_вариант_отпрвки_данных_);
    }

    public bool SendMsgByWhatsApp(string phone, string msg, out string error)
    {
      throw new NotImplementedException(Translate.SmsCenter_SendMsgByWhatsApp_К_сожалению__отправлять_сообщения_с_помощью_WhatsApp_через_этот_СМС_провайдер_невозможно__Воспользуйтесь_другим_СМС_провайдером_или_выберите_другой_вариант_вариант_отпрвки_данных_);
    }

    public Decimal GetBalance()
    {
      SmsRuRepository.GetBalanceCommand getBalanceCommand = new SmsRuRepository.GetBalanceCommand();
      this._repository.DoCommand((SmsRuRepository.Command) getBalanceCommand);
      return getBalanceCommand.Answer.Balance;
    }
  }
}
