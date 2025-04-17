// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ErrorHandler.ExceptionsConverter
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FastReport.Utils;
using FirebirdSql.Data.FirebirdClient;
using Gbs.Core.Config;
using Gbs.Resources.Localizations;
using System;
using System.Net;

#nullable disable
namespace Gbs.Helpers.ErrorHandler
{
  public static class ExceptionsConverter
  {
    public static string Convert(Exception ex)
    {
      string str1;
      string str2;
      switch (ex)
      {
        case NotImplementedException ex1:
          str1 = ExceptionsConverter.Convert(ex1);
          goto label_10;
        case WebException ex2:
          str1 = ExceptionsConverter.Convert(ex2);
          goto label_10;
        case UnauthorizedAccessException ex3:
          str1 = ExceptionsConverter.Convert(ex3);
          goto label_10;
        case InvalidOperationException ex4:
          str1 = ExceptionsConverter.Convert(ex4);
          goto label_10;
        case CompilerException ex5:
          str1 = ExceptionsConverter.Convert(ex5);
          goto label_10;
        case FbException ex6:
          str1 = ExceptionsConverter.Convert(ex6);
          goto label_10;
        case null:
          str2 = (string) null;
          break;
        default:
          str2 = ex.Message;
          break;
      }
      str1 = str2;
label_10:
      return str1;
    }

    private static string Convert(FbException ex)
    {
      string message = ex.Message;
      if (message.StartsWith("Unable to complete network request to host"))
        return Translate.ExceptionsConverter_Не_удалось_установить_сетевое_соединение_с_базой_данных + "\r\nDB Host: " + new ConfigsRepository<DataBase>().Get().Connection.ServerUrl;
      if (message.StartsWith("Your user name and password are not defined"))
        return Translate.ExceptionsConverter_Неверно_указан_логин_или_пароль_для_подключения_к_базе_данных_;
      return message.Contains("is not a valid database") ? Translate.ExceptionsConverter_Файл_базы_данных_имеет_некорректный_формат : message;
    }

    private static string Convert(CompilerException ex)
    {
      return Translate.ExceptionsConverter_Ошибка_при_подготовке_печатной_формы_документа_ + Other.NewLine() + ex.Message;
    }

    private static string Convert(UnauthorizedAccessException ex)
    {
      return Translate.ExceptionsConverter_Не_достаточно_прав_доступа_для_выполнения_операции_ + Other.NewLine() + ex.Message;
    }

    private static string Convert(InvalidOperationException ex)
    {
      string message = ex.Message;
      return message.StartsWith("Connection pool is full.") ? Translate.ExceptionsConverter_Пул_подключений_к_БД_полон__Попробуйте_перезапустить_программу : message;
    }

    private static string Convert(WebException ex)
    {
      return Translate.ExceptionsConverter_Ошибка_сетевого_подключения__ + Other.NewLine() + ex.Message;
    }

    private static string Convert(NotImplementedException ex)
    {
      return Translate.ExceptionsConverter_Convert_Операция_не_реализована_в_программе;
    }
  }
}
