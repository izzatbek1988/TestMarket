// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.CheckCodesHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Settings.BackEnd;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

#nullable disable
namespace Gbs.Helpers
{
  public class CheckCodesHelper
  {
    public static List<MarkGroupSettings> MarkGroups = new MarkGroupRepository().GetGroups();
    private static readonly List<char> Alphabet = ((IEnumerable<char>) "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!\"%&'*+-./_,:;=<>?".ToCharArray()).ToList<char>();

    private static List<CheckCodesHelper.CdnInfo> CdnInfos { get; set; }

    private static CheckCodesHelper.CdnInfo PriorityCdn { get; set; }

    public static string GetPriorityCdn(string token)
    {
      TrueApiHelper trueApiHelper = new TrueApiHelper();
      try
      {
        if (CheckCodesHelper.PriorityCdn != null)
          return CheckCodesHelper.PriorityCdn.Url;
        TrueApiHelper.UrlForCheckCodeCommand checkCodeCommand1 = new TrueApiHelper.UrlForCheckCodeCommand();
        checkCodeCommand1.ApiKey = token;
        TrueApiHelper.UrlForCheckCodeCommand checkCodeCommand2 = checkCodeCommand1;
        trueApiHelper.Url = !DevelopersHelper.IsDebug() ? "https://cdn.crpt.ru" : trueApiHelper.Url;
        trueApiHelper.DoCommand((TrueApiHelper.Command) checkCodeCommand2);
        if (CheckCodesHelper.CheckAnswerError((TrueApiHelper.GeneralAnswer) checkCodeCommand2.Result, checkCodeCommand2.StatusCode))
        {
          LogHelper.Debug("Не удалось получить CDN площадку для выполнения запроса в Честном знаке.\n" + checkCodeCommand2.Result.ErrorMessage + string.Format(" ({0} - {1}) ", (object) checkCodeCommand2.Result.Code, (object) checkCodeCommand2.Result.Description));
          CheckCodesHelper.PriorityCdn = (CheckCodesHelper.CdnInfo) null;
          return "";
        }
        TrueApiHelper.GetInfoUrlCommand getInfoUrlCommand1 = new TrueApiHelper.GetInfoUrlCommand();
        getInfoUrlCommand1.ApiKey = token;
        TrueApiHelper.GetInfoUrlCommand getInfoUrlCommand2 = getInfoUrlCommand1;
        CheckCodesHelper.CdnInfos = new List<CheckCodesHelper.CdnInfo>();
        foreach (TrueApiHelper.UrlForCheckCodeCommand.Answer.HostItem host in checkCodeCommand2.Result.Hosts)
        {
          trueApiHelper.Url = host.Host;
          DateTime now1 = DateTime.Now;
          try
          {
            trueApiHelper.DoCommand((TrueApiHelper.Command) getInfoUrlCommand2);
            if (CheckCodesHelper.CheckAnswerError((TrueApiHelper.GeneralAnswer) getInfoUrlCommand2.Result, getInfoUrlCommand2.StatusCode))
              continue;
          }
          catch
          {
            continue;
          }
          DateTime now2 = DateTime.Now;
          CheckCodesHelper.CdnInfos.Add(new CheckCodesHelper.CdnInfo()
          {
            Url = host.Host,
            ConnectTime = (now2 - now1).TotalMilliseconds
          });
        }
        if (!CheckCodesHelper.CdnInfos.Any<CheckCodesHelper.CdnInfo>())
        {
          CheckCodesHelper.PriorityCdn = (CheckCodesHelper.CdnInfo) null;
          return "";
        }
        CheckCodesHelper.PriorityCdn = CheckCodesHelper.CdnInfos.OrderBy<CheckCodesHelper.CdnInfo, double>((Func<CheckCodesHelper.CdnInfo, double>) (x => x.ConnectTime)).First<CheckCodesHelper.CdnInfo>();
        return CheckCodesHelper.PriorityCdn.Url;
      }
      catch (Exception ex)
      {
        LogHelper.WriteToCrptLog("Не удалось получить приоритетную CDN площадку", NLog.LogLevel.Error, ex);
        CheckCodesHelper.PriorityCdn = (CheckCodesHelper.CdnInfo) null;
        return "";
      }
    }

    public static string ReplacePriorityCdn(out bool isEndCdn)
    {
      if (CheckCodesHelper.PriorityCdn == null)
      {
        CheckCodesHelper.PriorityCdn = new CheckCodesHelper.CdnInfo()
        {
          Url = new TrueApiHelper().Url
        };
        isEndCdn = true;
        return CheckCodesHelper.PriorityCdn.Url;
      }
      DateTime now = DateTime.Now;
      foreach (CheckCodesHelper.CdnInfo cdnInfo1 in CheckCodesHelper.CdnInfos)
      {
        if (cdnInfo1.Url == CheckCodesHelper.PriorityCdn.Url)
        {
          cdnInfo1.TimeOff = new DateTime?(now);
        }
        else
        {
          DateTime? nullable1 = cdnInfo1.TimeOff;
          if (nullable1.HasValue)
          {
            DateTime dateTime = now;
            nullable1 = cdnInfo1.TimeOff;
            TimeSpan? nullable2;
            TimeSpan? nullable3;
            if (!nullable1.HasValue)
            {
              nullable2 = new TimeSpan?();
              nullable3 = nullable2;
            }
            else
              nullable3 = new TimeSpan?(dateTime - nullable1.GetValueOrDefault());
            nullable2 = nullable3;
            if (nullable2.Value.TotalMinutes > 15.0)
            {
              CheckCodesHelper.CdnInfo cdnInfo2 = cdnInfo1;
              nullable1 = new DateTime?();
              DateTime? nullable4 = nullable1;
              cdnInfo2.TimeOff = nullable4;
            }
          }
        }
      }
      if (!CheckCodesHelper.CdnInfos.Any<CheckCodesHelper.CdnInfo>((Func<CheckCodesHelper.CdnInfo, bool>) (x => !x.TimeOff.HasValue)))
      {
        CheckCodesHelper.PriorityCdn = new CheckCodesHelper.CdnInfo()
        {
          Url = new TrueApiHelper().Url
        };
        isEndCdn = true;
        return CheckCodesHelper.PriorityCdn.Url;
      }
      isEndCdn = false;
      CheckCodesHelper.PriorityCdn = CheckCodesHelper.CdnInfos.Where<CheckCodesHelper.CdnInfo>((Func<CheckCodesHelper.CdnInfo, bool>) (x => !x.TimeOff.HasValue)).OrderBy<CheckCodesHelper.CdnInfo, double>((Func<CheckCodesHelper.CdnInfo, double>) (x => x.ConnectTime)).First<CheckCodesHelper.CdnInfo>();
      return CheckCodesHelper.PriorityCdn.Url;
    }

    public static bool DoCommand(TrueApiHelper.CheckCodeCommand command, TrueApiHelper helper)
    {
      Task task = new Task((Action) (() =>
      {
        try
        {
          bool flag = true;
          bool isEndCdn = false;
          while (!isEndCdn)
          {
            helper.DoCommand((TrueApiHelper.Command) command);
            if (CheckCodesHelper.IsInternalServerError((TrueApiHelper.GeneralAnswer) command.Result, command.StatusCode))
            {
              if (flag)
              {
                flag = false;
              }
              else
              {
                flag = true;
                CheckCodesHelper.ReplacePriorityCdn(out isEndCdn);
                helper.Url = CheckCodesHelper.PriorityCdn.Url;
              }
            }
            else
            {
              if (command.StatusCode == HttpStatusCode.Unauthorized || command.StatusCode == HttpStatusCode.OK)
                return;
              break;
            }
          }
          helper.DoCommand((TrueApiHelper.Command) command);
        }
        catch (Exception ex)
        {
          command.AnswerString = new TrueApiHelper.GeneralAnswer().ToJsonString();
          command.StatusCode = HttpStatusCode.InternalServerError;
          LogHelper.WriteError(ex, "Ошибка при запросе ЧЗ");
        }
      }));
      task.Start();
      return task.Wait((int) new ConfigsRepository<Integrations>().Get().Crpt.Timeout * 1000);
    }

    public static bool CheckAnswerError(TrueApiHelper.GeneralAnswer answer, HttpStatusCode code)
    {
      if (code == HttpStatusCode.NonAuthoritativeInformation)
      {
        TrueApiRepository.IsDisabledVerification = true;
        return true;
      }
      if (code >= HttpStatusCode.InternalServerError || code == HttpStatusCode.RequestTimeout || answer == null)
        return true;
      if (answer.Code == 0 && answer.ErrorMessage.IsNullOrEmpty())
        return false;
      if (answer.Code == 203)
      {
        TrueApiRepository.IsDisabledVerification = true;
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification("Получена информация о введении аварийного режима на стороне системы Честный знак, проверки кодов маркировки в ближайшие 72 часа (или до перезагрузки программы) производиться не будут."));
      }
      return true;
    }

    public static bool IsInternalServerError(
      TrueApiHelper.GeneralAnswer answer,
      HttpStatusCode code)
    {
      return (answer == null || answer.Code != 5000) && code >= HttpStatusCode.InternalServerError;
    }

    public static List<string> VerifiedMarkCode(
      TrueApiHelper.CheckCodeCommand.Answer.CodeItem item,
      out Decimal tobaccoSalePrice,
      out bool isSkipVerified)
    {
      tobaccoSalePrice = 0M;
      isSkipVerified = false;
      if (item.GroupIds.Any<MarkGroupEnum>() && item.GroupIds.All<MarkGroupEnum>((Func<MarkGroupEnum, bool>) (x => CheckCodesHelper.MarkGroups.Any<MarkGroupSettings>((Func<MarkGroupSettings, bool>) (g => g.Group == x)) && !CheckCodesHelper.MarkGroups.Single<MarkGroupSettings>((Func<MarkGroupSettings, bool>) (g => g.Group == x)).IsAllowedMode)))
      {
        LogHelper.WriteToCrptLog("Пропускаем проверку КМ - " + item.Cis + ", так как проверка товарной группы " + item.GroupIds.ToJsonString(true) + " отключены в настройках программы.", NLog.LogLevel.Info);
        isSkipVerified = true;
        return new List<string>();
      }
      List<string> errorList = new List<string>();
      if (!item.Found)
      {
        errorList.Add("– Код маркировки не найден в ГИС МТ;");
        return errorList;
      }
      if (!item.Utilised)
        errorList.Add("– Код маркировки эмитирован, но нет информации о его нанесении;");
      if (!item.Verified || !item.Valid)
        errorList.Add("– Не пройдена криптографическая проверка кода маркировки;");
      if (item.Sold)
        errorList.Add("– Код маркировки выведен из оборота;");
      if (item.IsBlocked)
        errorList.Add("– Код маркировки заблокирован по решению ОГВ;");
      if (!item.Realizable && !item.Sold && !item.GrayZone)
        errorList.Add("– Нет информации о вводе в оборот кода маркировки;");
      if (!item.IsOwner && item.GroupIds.Any<MarkGroupEnum>((Func<MarkGroupEnum, bool>) (x => CheckCodesHelper.MarkGroups.Any<MarkGroupSettings>((Func<MarkGroupSettings, bool>) (g => g.Group == x)) && CheckCodesHelper.MarkGroups.Single<MarkGroupSettings>((Func<MarkGroupSettings, bool>) (g => g.Group == x)).IsCheckOwner)))
        errorList.Add("– Код маркировки принадлежит другой организации;");
      VerifiedMarkCodeTobacco(item, errorList, out tobaccoSalePrice);
      VerifiedExpireDate(item, errorList);
      LogHelper.WriteToCrptLog("Результат проверки КМ:" + item.ToJsonString(true) + "; ", NLog.LogLevel.Trace);
      return errorList;

      static void VerifiedMarkCodeTobacco(
        TrueApiHelper.CheckCodeCommand.Answer.CodeItem item,
        List<string> errorList,
        out Decimal tobaccoSalePrice)
      {
        tobaccoSalePrice = 0M;
        if (!item.GroupIds.Contains(MarkGroupEnum.Tobacco))
          return;
        tobaccoSalePrice = (Decimal) CheckCodesHelper.GetPriceForTobacco(item.Cis);
        Decimal? nullable = item.Smp;
        if (!nullable.HasValue)
          return;
        nullable = item.Smp;
        Decimal num1 = 0M;
        if (nullable.GetValueOrDefault() == num1 & nullable.HasValue || tobaccoSalePrice == 0M)
          return;
        nullable = item.Smp;
        Decimal num2 = nullable.Value;
        if (item.PackageType == "GROUP")
        {
          nullable = item.PackageQuantity;
          if (nullable.HasValue)
          {
            nullable = item.PackageQuantity;
            Decimal num3 = 0M;
            if (!(nullable.GetValueOrDefault() == num3 & nullable.HasValue))
            {
              Decimal num4 = num2;
              nullable = item.PackageQuantity;
              Decimal num5 = nullable.Value;
              num2 = num4 * num5;
            }
          }
        }
        if (!(tobaccoSalePrice * 100M < num2))
          return;
        errorList.Add(string.Format("– МРЦ меньше, чем минимальная для данного вида продукции (МРЦ: {0:N2}, минимальная: {1:N2});", (object) tobaccoSalePrice, (object) item.Smp));
      }

      static void VerifiedExpireDate(
        TrueApiHelper.CheckCodeCommand.Answer.CodeItem item,
        List<string> errorList)
      {
        if (!item.GroupIds.Any<MarkGroupEnum>((Func<MarkGroupEnum, bool>) (x => x.IsEither<MarkGroupEnum>(MarkGroupEnum.Water, MarkGroupEnum.Milk, MarkGroupEnum.Beer))) || (item.ExpireDate - DateTime.Now).TotalHours > 0.0 || !(item.ExpireDate != DateTime.MinValue))
          return;
        string str = item.ExpireDate.ToString(item.ExpireDate.Date == DateTime.Now.Date ? "dd.MM.yyyy HH:mm" : "dd.MM.yyyy");
        errorList.Add("– У товара с данным кодом маркировки истек срок годности (срок годности до " + str + ");");
      }
    }

    public static void GetTypeVerification(
      Gbs.Core.Config.Crpt configCrpt,
      TrueApiHelper trueApiHelper,
      TrueApiHelper.CheckCodeCommand command)
    {
      trueApiHelper.Url = !configCrpt.Token.IsNullOrEmpty() ? CheckCodesHelper.GetPriorityCdn(configCrpt.Token) : throw new ArgumentException("Для онлайн-проверки кода маркировки в Честном знаке, нужно в  разделе программы Файл - Настройки - Законодательство - Честный знак указать токен организаци.");
      command.ApiKey = configCrpt.Token;
    }

    private static int GetIndex(char s)
    {
      return CheckCodesHelper.Alphabet.FindIndex((Predicate<char>) (x => (int) x == (int) s));
    }

    public static double GetPriceForTobacco(string code)
    {
      if (code.Contains("\u001D"))
      {
        double result;
        if (!double.TryParse(code.Substring(30, code.Substring(30).IndexOf("\u001D")), out result))
          return 0.0;
        result = Math.Round(result / 100.0, 2);
        return result;
      }
      string str = code.Substring(21, 4);
      double num = 0.0;
      for (int index1 = 0; index1 < str.Length; ++index1)
      {
        int index2 = CheckCodesHelper.GetIndex(str[index1]);
        num += Math.Pow(80.0, (double) (str.Length - (index1 + 1))) * (double) index2;
      }
      return num == 0.0 ? 0.0 : Math.Round(num / 100.0, 2);
    }

    public class CdnInfo
    {
      public string Url { get; set; }

      public double ConnectTime { get; set; }

      public DateTime? TimeOff { get; set; }
    }

    public class CheckCodeInfoItem
    {
      public Guid Uid { get; set; }

      public string MarkedInfo { get; set; }

      public List<string> ListError { get; set; }

      public Decimal TobaccoSalePrice { get; set; }

      public bool AllowForSale => !this.ListError.Any<string>();

      public Gbs.Core.ViewModels.Basket.Basket.CustomColors Color { get; set; }
    }
  }
}
