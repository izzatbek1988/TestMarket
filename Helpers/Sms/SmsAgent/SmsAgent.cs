// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmsAgent
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  internal class SmsAgent : ISms
  {
    private readonly SmsAgentRepository _repository;
    private readonly Integrations _integrations;

    public SmsAgent(Integrations integrations)
    {
      this._integrations = integrations;
      this._repository = new SmsAgentRepository(integrations);
    }

    public int SendSms(string text, List<string> phones, out List<string> errorList)
    {
      try
      {
        SmsAgentRepository.SendSmsCommand command = new SmsAgentRepository.SendSmsCommand()
        {
          PayloadsList = phones.Select<string, SmsAgentRepository.SendSmsCommand.Payloads>((Func<string, SmsAgentRepository.SendSmsCommand.Payloads>) (x => new SmsAgentRepository.SendSmsCommand.Payloads()
          {
            Phone = x
          })).ToList<SmsAgentRepository.SendSmsCommand.Payloads>(),
          Text = text,
          Sender = this._integrations.Sms.Sender,
          Type = "sms"
        };
        this._repository.DoCommand((SmsAgentRepository.Command) command);
        errorList = new List<string>((IEnumerable<string>) this.CheckError(command));
        return command.Result.Count<SmsAgentRepository.SendSmsCommand.AnswerForPhone>((Func<SmsAgentRepository.SendSmsCommand.AnswerForPhone, bool>) (x => x.Error.IsNullOrEmpty()));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправки кода по звонку", false);
        errorList = new List<string>() { ex.Message };
        return 0;
      }
    }

    public int SendSms(Dictionary<int, string> phonesAndMsg, out List<string> errorList)
    {
      throw new NotImplementedException();
    }

    public bool GetCodeForCall(string phone, out string code, out string error)
    {
      try
      {
        SmsAgentRepository.SendSmsCommand sendSmsCommand = this.sendMsg(phone, "", "flashcall", out error);
        code = (sendSmsCommand != null ? sendSmsCommand.Result.Single<SmsAgentRepository.SendSmsCommand.AnswerForPhone>().Code : (string) null) ?? "";
        return sendSmsCommand != null;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправки кода по звонку", false);
        error = ex.Message;
        code = "";
        return false;
      }
    }

    public bool SendMsgByViber(string phone, string msg, out string error)
    {
      try
      {
        return this.sendMsg(phone, msg, "viber", out error) != null;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправки кода по Вайберу", false);
        error = ex.Message;
        return false;
      }
    }

    public bool SendMsgByWhatsApp(string phone, string msg, out string error)
    {
      try
      {
        return this.sendMsg(phone, msg, "wa", out error) != null;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправки кода по Вайберу", false);
        error = ex.Message;
        return false;
      }
    }

    public Decimal GetBalance()
    {
      SmsAgentRepository.GetBalanceCommand getBalanceCommand = new SmsAgentRepository.GetBalanceCommand()
      {
        Login = this._integrations.Sms.Login.DecryptedValue,
        Password = this._integrations.Sms.Password.DecryptedValue
      };
      this._repository.DoCommand((SmsAgentRepository.Command) getBalanceCommand);
      return getBalanceCommand.Result.Balance;
    }

    private List<string> CheckError(SmsAgentRepository.SendSmsCommand command)
    {
      if (command.ResultGlobalError != null)
        throw new Exception(command.ResultGlobalError.Error);
      List<string> stringList = new List<string>();
      stringList.AddRange(command.Result.Where<SmsAgentRepository.SendSmsCommand.AnswerForPhone>((Func<SmsAgentRepository.SendSmsCommand.AnswerForPhone, bool>) (x => !x.Error.IsNullOrEmpty())).Select<SmsAgentRepository.SendSmsCommand.AnswerForPhone, string>((Func<SmsAgentRepository.SendSmsCommand.AnswerForPhone, string>) (x => x.Phone + ": " + x.Error)));
      return stringList;
    }

    private SmsAgentRepository.SendSmsCommand sendMsg(
      string phone,
      string msg,
      string typeMsg,
      out string error)
    {
      SmsAgentRepository.SendSmsCommand command = new SmsAgentRepository.SendSmsCommand()
      {
        PayloadsList = new List<SmsAgentRepository.SendSmsCommand.Payloads>()
        {
          new SmsAgentRepository.SendSmsCommand.Payloads()
          {
            Phone = phone
          }
        },
        Type = typeMsg,
        Sender = this._integrations.Sms.Sender,
        Text = msg.IsNullOrEmpty() ? (string) null : msg
      };
      this._repository.DoCommand((SmsAgentRepository.Command) command);
      error = this.CheckError(command).SingleOrDefault<string>() ?? "";
      return error != "" || !command.Result.Any<SmsAgentRepository.SendSmsCommand.AnswerForPhone>() ? (SmsAgentRepository.SendSmsCommand) null : command;
    }
  }
}
