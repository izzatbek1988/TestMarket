// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.FtpHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FluentFTP;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.IO;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public static class FtpHelper
  {
    public static bool UploadFile(
      FileInfo localFile,
      string ftpPath,
      string url,
      int? portNum,
      string login,
      string pass)
    {
      try
      {
        NetworkCredential credentials = new NetworkCredential(login, pass);
        FtpClient ftpClient = new FtpClient(url, credentials, portNum.GetValueOrDefault());
        LogHelper.Debug("Подключаемся к FTP серверу");
        LogHelper.Debug("Сервер: " + url + "   Логин: " + login + "   Пароль: " + pass);
        ftpClient.Connect();
        if (!Directory.Exists(localFile.DirectoryName))
          Directory.CreateDirectory("keys");
        if (ftpClient.UploadFile(localFile.FullName, ftpPath, FtpRemoteExists.Overwrite, true, FtpVerify.Retry, (Action<FtpProgress>) null) == FtpStatus.Success)
          LogHelper.Debug("Файл загружен на сервер");
        else
          LogHelper.Error(new Exception(), "Файл " + localFile.Name + " не удалось загрузить на сервер");
        LogHelper.Debug("Отключаемся от FTP сервера");
        ftpClient.Disconnect();
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при работе с FTP-сервером", false);
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = Translate.FtpHelper_FTP_сервер,
          Text = Translate.FtpHelper_Ошибка_при_работе_с_FTP_сервером
        });
        return false;
      }
    }
  }
}
