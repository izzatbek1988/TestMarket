// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SmtpHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Emails;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;

#nullable disable
namespace Gbs.Helpers
{
  public static class SmtpHelper
  {
    public static bool Send(Email email)
    {
      try
      {
        if (!Other.IsValidateEmail(email.AddressTo))
          return false;
        if (!NetworkHelper.IsWorkInternet())
        {
          LogHelper.Debug("Нет подключения к интернету, не отправляем письмо на почту.");
          return false;
        }
        LogHelper.Debug("Отправляем письмо \"" + email.Subject + "\" на адрес " + email.AddressTo);
        bool flag = TelegramHelper.SendEmail(email);
        try
        {
          if (flag)
          {
            foreach (string fileName in email != null ? email.Attach.ToList<string>() : (List<string>) null)
            {
              FileInfo fileInfo = new FileInfo(fileName);
              if (fileInfo.Exists && fileInfo.DirectoryName.IsNullOrEmpty())
                Directory.Delete(fileInfo?.DirectoryName, true);
            }
          }
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, "Не удалось удалить временные файлы вложений");
        }
        return true;
      }
      catch (SmtpException ex)
      {
        LogHelper.Error((Exception) ex, "Ошибка отправки письма", false, false);
        return false;
      }
      catch (SocketException ex)
      {
        LogHelper.Error((Exception) ex, "Ошибка отправки письма", false, false);
        return false;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при отправке письма на адрес", false);
        return false;
      }
    }
  }
}
