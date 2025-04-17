// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.TelegramHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Db;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Emails;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Gbs.Helpers
{
  public class TelegramHelper
  {
    public static bool SendReport(DateTime date, string chatIdTo, bool usePause = true)
    {
      try
      {
        if (!NetworkHelper.IsWorkInternet())
        {
          LogHelper.Debug("Нет подключения к интернету, не отправляем инфо в ТГ.");
          return false;
        }
        LogHelper.OnBegin();
        bool flag = true;
        string[] strArray = chatIdTo.Split(new char[3]
        {
          ',',
          ' ',
          ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        using (DataBase dataBase = Data.GetDataBase())
        {
          List<Document> list = new DocumentsRepository(dataBase).GetItemsWithFilter(date.Date, isOnTime: false).ToList<Document>();
          object obj = EmailRepository.GetBodyMail(date, list);
          string reportSale = EmailRepository.GetReportSale(list.Where<Document>((Func<Document, bool>) (x => x.Type == GlobalDictionaries.DocumentsTypes.Sale)).ToList<Document>());
          if (reportSale.IsNullOrEmpty())
            obj = (object) (obj?.ToString() + Other.NewLine(3) + Translate.EmailRepository_Send_Не_удалось_сформировать_файл_с_отчетом_по_продажам_);
          for (int index = 0; index < strArray.Length; ++index)
          {
            string chatId = strArray[index];
            Email email = new Email()
            {
              Subject = string.Format(Translate.EmailRepository_Send_Отчет_на__0_g__из__1_, (object) DateTime.Now, UidDb.GetUid().Value),
              Body = obj,
              AddressTo = chatId,
              Attach = new List<string>() { reportSale },
              TypeReport = TypeReport.Telegram
            };
            string msg = email.Subject + new string(new char[2]
            {
              '\n',
              '\n'
            }) + email.Body?.ToString();
            email.IsSend = TelegramHelper.SendTextMessage(chatId, msg);
            LogHelper.Debug(email.IsSend ? string.Format(Translate.TelegramHelper_SendReport_Отчет_в_Телеграмм__0__успешно_отправлен, (object) email.AddressTo) : string.Format(Translate.TelegramHelper_SendReport_При_отправке_сообщения_в_Телеграмм__0__произошла_ошибка_, (object) email.AddressTo));
            if (!reportSale.IsNullOrEmpty())
              email.IsSend = TelegramHelper.SendFileTextAndMessage(chatId, reportSale);
            LogHelper.Debug(email.IsSend ? string.Format(Translate.TelegramHelper_SendReport_Файл_с_отчетом_в_Телеграмм__0__успешно_отправлен, (object) email.AddressTo) : string.Format(Translate.TelegramHelper_SendReport_При_отправке_файла_отчета_в_Телеграмм__0__произошла_ошибка_, (object) email.AddressTo));
            new EmailRepository().Save(email);
            flag &= email.IsSend;
            if (index + 1 != strArray.Length)
            {
              if (usePause)
                Thread.Sleep(10000);
            }
            else
              break;
          }
          LogHelper.OnEnd();
          return flag;
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
        return false;
      }
    }

    public static bool SendFile(string chatIdTo, string path)
    {
      if (!NetworkHelper.IsWorkInternet())
      {
        LogHelper.Debug("Нет подключения к интернету, не отправляем инфо в ТГ.");
        return false;
      }
      string[] strArray = chatIdTo.Split(new char[3]
      {
        ',',
        ' ',
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      bool flag = true;
      foreach (string chatId in strArray)
        flag &= TelegramHelper.SendFileTextAndMessage(chatId, path);
      return flag;
    }

    public static bool SendText(string chatIdTo, string text)
    {
      if (!NetworkHelper.IsWorkInternet())
      {
        LogHelper.Debug("Нет подключения к интернету, не отправляем инфо в ТГ.");
        return false;
      }
      string[] strArray = chatIdTo.Split(new char[3]
      {
        ',',
        ' ',
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      bool flag = true;
      foreach (string chatId in strArray)
        flag &= TelegramHelper.SendTextMessage(chatId, text);
      return flag;
    }

    public static bool SendEmail(Email email)
    {
      if (!NetworkHelper.IsWorkInternet())
      {
        LogHelper.Debug("Нет подключения к интернету, не отправляем инфо в на почту через сервер ТГ.");
        return false;
      }
      new TelegramHelper_v2().SendEmailToServer(email);
      return true;
    }

    public static bool SendTextMessage(string chatId, string msg)
    {
      try
      {
        new TelegramHelper_v2().SendText(chatId, msg);
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправке сообщения Телеграмм", false);
        return false;
      }
    }

    private static bool SendFileTextAndMessage(string chatId, string path)
    {
      try
      {
        new TelegramHelper_v2().SendFile(chatId, path);
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправке сообщения Телеграмм", false);
        return false;
      }
    }
  }
}
