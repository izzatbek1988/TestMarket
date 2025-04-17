// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsAero
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
  public class SmsAero : ISms
  {
    private readonly SmsAeroRepository _repository;
    private readonly Integrations _integrations;

    public SmsAero(Integrations integrations)
    {
      this._integrations = integrations;
      this._repository = new SmsAeroRepository(integrations.Sms.ApiKey.DecryptedValue, integrations.Sms.Login.DecryptedValue);
    }

    public int SendSms(string text, List<string> phones, out List<string> errorList)
    {
      SmsAeroRepository.SendSmsCommand sendSmsCommand = new SmsAeroRepository.SendSmsCommand()
      {
        SmsText = text,
        Phones = phones,
        Sender = this._integrations.Sms.Sender
      };
      this._repository.DoCommand((SmsAeroRepository.Command) sendSmsCommand);
      if (!sendSmsCommand.AnswerError.Success)
      {
        errorList = new List<string>()
        {
          sendSmsCommand.AnswerError.Message
        };
        return 0;
      }
      errorList = new List<string>(sendSmsCommand.Answer.Data.Where<SmsAeroRepository.SendSmsCommand.PhoneAnswer>((Func<SmsAeroRepository.SendSmsCommand.PhoneAnswer, bool>) (x => x.Status.IsEither<int>(2, 6))).Select<SmsAeroRepository.SendSmsCommand.PhoneAnswer, string>((Func<SmsAeroRepository.SendSmsCommand.PhoneAnswer, string>) (x =>
      {
        string number = x.Number;
        string str;
        switch (x.Status)
        {
          case 2:
            str = Translate.SmsAero_SendSms_не_доставлено;
            break;
          case 6:
            str = Translate.SmsAero_SendSms_сообщение_отклонено;
            break;
          default:
            str = "";
            break;
        }
        return number + " - " + str;
      })));
      errorList = new List<string>();
      return sendSmsCommand.Answer.Data.Count<SmsAeroRepository.SendSmsCommand.PhoneAnswer>((Func<SmsAeroRepository.SendSmsCommand.PhoneAnswer, bool>) (x => !x.Status.IsEither<int>(2, 6)));
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
      SmsAeroRepository.SendViberCommand sendViberCommand = new SmsAeroRepository.SendViberCommand()
      {
        Text = msg,
        Phones = new List<string>() { phone },
        Sign = this._integrations.Sms.Sender
      };
      this._repository.DoCommand((SmsAeroRepository.Command) sendViberCommand);
      if (!sendViberCommand.AnswerError.Success)
      {
        error = sendViberCommand.AnswerError.Message;
        return false;
      }
      if (sendViberCommand.Answer.Data.Status.IsEither<int>(2, 6))
      {
        object id = (object) sendViberCommand.Answer.Data.Id;
        string str;
        switch (sendViberCommand.Answer.Data.Status)
        {
          case 2:
            str = Translate.SmsAero_SendSms_не_доставлено;
            break;
          case 6:
            str = Translate.SmsAero_SendSms_сообщение_отклонено;
            break;
          default:
            str = "";
            break;
        }
        error = string.Format("{0} - {1}", id, (object) str);
      }
      else
        error = "";
      return !sendViberCommand.Answer.Data.Status.IsEither<int>(2, 6);
    }

    public bool SendMsgByWhatsApp(string phone, string msg, out string error)
    {
      SmsAeroRepository.SendWhatsAppCommand sendWhatsAppCommand = new SmsAeroRepository.SendWhatsAppCommand()
      {
        Text = msg,
        Address = phone,
        Sign = this._integrations.Sms.Sender
      };
      this._repository.DoCommand((SmsAeroRepository.Command) sendWhatsAppCommand);
      if (!sendWhatsAppCommand.AnswerError.Success)
      {
        error = sendWhatsAppCommand.AnswerError.Message;
        return false;
      }
      if (sendWhatsAppCommand.Answer.Data.Status.IsEither<int>(2, 6))
      {
        object id = (object) sendWhatsAppCommand.Answer.Data.Id;
        string str;
        switch (sendWhatsAppCommand.Answer.Data.Status)
        {
          case 2:
            str = Translate.SmsAero_SendSms_не_доставлено;
            break;
          case 6:
            str = Translate.SmsAero_SendSms_сообщение_отклонено;
            break;
          default:
            str = "";
            break;
        }
        error = string.Format("{0} - {1}", id, (object) str);
      }
      else
        error = "";
      return !sendWhatsAppCommand.Answer.Data.Status.IsEither<int>(2, 6);
    }

    public Decimal GetBalance()
    {
      SmsAeroRepository.GetBalanceCommand getBalanceCommand = new SmsAeroRepository.GetBalanceCommand();
      this._repository.DoCommand((SmsAeroRepository.Command) getBalanceCommand);
      return !getBalanceCommand.Answer.Success ? -1M : getBalanceCommand.Answer.Data.Balance;
    }
  }
}
