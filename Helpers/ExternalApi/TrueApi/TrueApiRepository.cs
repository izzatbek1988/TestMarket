// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.TrueApiRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MarkCodes;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

#nullable disable
namespace Gbs.Helpers
{
  public static class TrueApiRepository
  {
    public static bool IsDisabledVerification;

    private static Gbs.Core.Config.Crpt ConfigCrpt
    {
      get => new ConfigsRepository<Integrations>().Get().Crpt;
    }

    public static bool CheckCode(
      string code,
      out List<string> listError,
      GlobalDictionaries.DocumentsTypes documentType,
      out bool isErrorCheck,
      out bool isSkipVerified,
      out Decimal tobaccoSalePrice,
      bool isShowNotif = true)
    {
      isErrorCheck = false;
      tobaccoSalePrice = 0M;
      isSkipVerified = false;
      if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia)
      {
        listError = new List<string>();
        return true;
      }
      if (TrueApiRepository.IsDisabledVerification)
      {
        LogHelper.WriteToCrptLog("Не проводим проверку кода " + code + " в ЦРПТ, так как включен аварийный режим на стороне ЦРПТ.", NLog.LogLevel.Info);
        isErrorCheck = true;
        listError = new List<string>();
        return true;
      }
      if (!documentType.IsEither<GlobalDictionaries.DocumentsTypes>(GlobalDictionaries.DocumentsTypes.Sale, GlobalDictionaries.DocumentsTypes.BeerProductionList) || TrueApiRepository.IsDisabledVerification)
      {
        listError = new List<string>();
        return true;
      }
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.TrueApiHelper_CheckCode_Проверка_кода_маркировки_в_системе_Честный_знак);
      string fullCode = DataMatrixHelper.ReplaceSomeCharsToFNC1(code.Trim());
      try
      {
        TrueApiHelper.CheckCodeCommand checkCodeCommand = new TrueApiHelper.CheckCodeCommand();
        checkCodeCommand.Codes = new List<string>()
        {
          fullCode
        };
        checkCodeCommand.Timeout = TrueApiRepository.ConfigCrpt.Timeout;
        TrueApiHelper.CheckCodeCommand command = checkCodeCommand;
        TrueApiHelper trueApiHelper = new TrueApiHelper();
        CheckCodesHelper.GetTypeVerification(TrueApiRepository.ConfigCrpt, trueApiHelper, command);
        bool flag = CheckCodesHelper.DoCommand(command, trueApiHelper);
        if (command.StatusCode == HttpStatusCode.Unauthorized)
        {
          listError = new List<string>()
          {
            "Указан некорректный токен для работы с Честным знаком"
          };
          progressBar.Close();
          return false;
        }
        if (!flag || CheckCodesHelper.CheckAnswerError((TrueApiHelper.GeneralAnswer) command?.Result, command.StatusCode))
        {
          progressBar.Close();
          if (isShowNotif)
            ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.TrueApiHelper_CheckCode_Не_удалось_проверить_код_маркировки_в_режиме__онлайн__));
          isErrorCheck = true;
          listError = new List<string>();
          return true;
        }
        TrueApiHelper.CheckCodeCommand.Answer.CodeItem codeItem = command.Result.Codes.Single<TrueApiHelper.CheckCodeCommand.Answer.CodeItem>((Func<TrueApiHelper.CheckCodeCommand.Answer.CodeItem, bool>) (x => x.Cis == fullCode));
        listError = CheckCodesHelper.VerifiedMarkCode(codeItem, out tobaccoSalePrice, out isSkipVerified);
        progressBar.Close();
        return !listError.Any<string>();
      }
      catch (Exception ex)
      {
        progressBar.Close();
        LogHelper.WriteToCrptLog("Не удалось проверить код маркировки ЧЗ", NLog.LogLevel.Error, ex);
        if (isShowNotif)
        {
          string str = "Онлайн-проверку кода маркировки в Честном Знаке не удалась выполнить.";
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(!(ex is WebException webException) || webException.Status != WebExceptionStatus.Timeout ? (!(ex is ArgumentException) ? str + "\n\nПроверьте наличие интернет - соединения или обратитесь в службу технической поддержки." : str + "\n\n" + ex.Message) : str + "\n\nУвеличьте таймаут для выполнения запроса  или обратитесь в службу технической поддержки."));
        }
        listError = new List<string>();
        isErrorCheck = true;
        return true;
      }
    }

    public static List<CheckCodesHelper.CheckCodeInfoItem> CheckCodes(
      List<CheckCodesHelper.CheckCodeInfoItem> codes,
      out string infoForKkm)
    {
      if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia)
      {
        infoForKkm = "";
        return new List<CheckCodesHelper.CheckCodeInfoItem>();
      }
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.TrueApiHelper_CheckCode_Проверка_кода_маркировки_в_системе_Честный_знак);
      foreach (CheckCodesHelper.CheckCodeInfoItem code in codes)
        code.MarkedInfo = DataMatrixHelper.ReplaceSomeCharsToFNC1(code.MarkedInfo.Trim());
      TrueApiHelper.CheckCodeCommand checkCodeCommand = new TrueApiHelper.CheckCodeCommand();
      checkCodeCommand.Codes = codes.Select<CheckCodesHelper.CheckCodeInfoItem, string>((Func<CheckCodesHelper.CheckCodeInfoItem, string>) (x => x.MarkedInfo)).ToList<string>();
      checkCodeCommand.ListGuid = codes.Select<CheckCodesHelper.CheckCodeInfoItem, Guid>((Func<CheckCodesHelper.CheckCodeInfoItem, Guid>) (x => x.Uid)).ToList<Guid>();
      checkCodeCommand.Timeout = TrueApiRepository.ConfigCrpt.Timeout;
      TrueApiHelper.CheckCodeCommand command = checkCodeCommand;
      TrueApiHelper trueApiHelper = new TrueApiHelper();
      try
      {
        CheckCodesHelper.GetTypeVerification(TrueApiRepository.ConfigCrpt, trueApiHelper, command);
        if (!CheckCodesHelper.DoCommand(command, trueApiHelper) || CheckCodesHelper.CheckAnswerError((TrueApiHelper.GeneralAnswer) command.Result, command.StatusCode) || command.StatusCode == HttpStatusCode.Unauthorized)
        {
          progressBar.Close();
          infoForKkm = "";
          return new List<CheckCodesHelper.CheckCodeInfoItem>();
        }
        List<CheckCodesHelper.CheckCodeInfoItem> checkCodeInfoItemList = new List<CheckCodesHelper.CheckCodeInfoItem>();
        int index = 0;
        foreach (TrueApiHelper.CheckCodeCommand.Answer.CodeItem code in command.Result.Codes)
        {
          Decimal tobaccoSalePrice;
          bool isSkipVerified;
          List<string> source = CheckCodesHelper.VerifiedMarkCode(code, out tobaccoSalePrice, out isSkipVerified);
          checkCodeInfoItemList.Add(new CheckCodesHelper.CheckCodeInfoItem()
          {
            ListError = source,
            MarkedInfo = code.Cis,
            Uid = command.ListGuid[index],
            TobaccoSalePrice = tobaccoSalePrice,
            Color = isSkipVerified ? Gbs.Core.ViewModels.Basket.Basket.CustomColors.None : (source.Any<string>() ? Gbs.Core.ViewModels.Basket.Basket.CustomColors.Red : Gbs.Core.ViewModels.Basket.Basket.CustomColors.Green)
          });
          ++index;
        }
        progressBar.Close();
        infoForKkm = string.Format("UUID={0}&Time={1}", (object) command.Result.ReqId, (object) command.Result.ReqTimestamp);
        return checkCodeInfoItemList;
      }
      catch (Exception ex)
      {
        infoForKkm = "";
        progressBar.Close();
        LogHelper.WriteToCrptLog("Не удалось проверить код маркировки ЧЗ", NLog.LogLevel.Error, ex);
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Не удалось проверить код маркировки.\n\n" + ex.Message, ProgressBarViewModel.Notification.NotificationsTypes.Warning));
        return new List<CheckCodesHelper.CheckCodeInfoItem>();
      }
    }

    public static bool ConnectTapBeerKega(
      List<TrueApiHelper.ConnectTapDocument.CodeBeer> codes)
    {
      try
      {
        TrueApiHelper trueApiHelper = new TrueApiHelper()
        {
          Url = "https://markirovka.crpt.ru"
        };
        TrueApiHelper.AuthKeyCommand authKeyCommand = new TrueApiHelper.AuthKeyCommand();
        trueApiHelper.DoCommand((TrueApiHelper.Command) authKeyCommand);
        if (authKeyCommand.Result.Data.IsNullOrEmpty() || authKeyCommand.Result.Uid.IsNullOrEmpty())
          return false;
        OtherConfig other = new ConfigsRepository<Settings>().Get().Other;
        string str1 = CryptoHelper.SignDoc(authKeyCommand.Result.Data, other.Thumbprint);
        TrueApiHelper.SimpleSignInCommand simpleSignInCommand = new TrueApiHelper.SimpleSignInCommand()
        {
          Uid = authKeyCommand.Result.Uid,
          Data = str1
        };
        trueApiHelper.DoCommand((TrueApiHelper.Command) simpleSignInCommand);
        if (simpleSignInCommand.Result.Code != 0 || !simpleSignInCommand.Result.ErrorMessage.IsNullOrEmpty())
          return false;
        Client organization = SalePoints.GetSalePointList().First<SalePoints.SalePoint>().Organization;
        string str2 = organization.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.KppUid))?.Value.ToString() ?? "";
        string str3 = organization.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.FiasUid))?.Value.ToString() ?? "";
        codes.ForEach((Action<TrueApiHelper.ConnectTapDocument.CodeBeer>) (x => x.Cis = x.Cis.Trim()));
        TrueApiHelper.ConnectTapDocument connectTapDocument = new TrueApiHelper.ConnectTapDocument()
        {
          Codes = codes,
          ParticipantInn = organization.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? "",
          ParticipantKpp = str2.IsNullOrEmpty() ? (string) null : str2,
          FiasId = str3.IsNullOrEmpty() ? (string) null : str3
        };
        string str4 = CryptoHelper.SignDoc(connectTapDocument.ToJsonString(), other.Thumbprint);
        TrueApiHelper.CreateDocumentCommand createDocumentCommand1 = new TrueApiHelper.CreateDocumentCommand();
        createDocumentCommand1.GoodGroup = "beer";
        createDocumentCommand1.ProductDocument = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(connectTapDocument.ToJsonString()));
        createDocumentCommand1.Type = connectTapDocument.Type;
        createDocumentCommand1.Signature = str4;
        createDocumentCommand1.Token = simpleSignInCommand.Result.Token;
        TrueApiHelper.CreateDocumentCommand createDocumentCommand2 = createDocumentCommand1;
        trueApiHelper.DoCommand((TrueApiHelper.Command) createDocumentCommand2);
        try
        {
          TrueApiHelper.CreateDocumentCommand.Answer result = createDocumentCommand2.Result;
          if ((result != null ? (!result.ErrorMessage.IsNullOrEmpty() ? 1 : 0) : 0) != 0)
            return false;
        }
        catch
        {
        }
        return !createDocumentCommand2.ResultStr.IsNullOrEmpty();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось поставить кегу на кран, обратитесь в службу технической поддержки.");
        return false;
      }
    }
  }
}
