// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.PilotNt
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.ErrorHandler;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class PilotNt
  {
    [Localizable(false)]
    private static List<string> CheckSeparator { get; set; } = new List<string>()
    {
      "~S",
      "\u0001"
    };

    public static AuthAnswer13 Authorization(Decimal sum, out string slip)
    {
      int num = (int) (sum * 100M);
      return PilotNt.Authorization(new AuthAnswer13()
      {
        ans = new AuthAnswer()
        {
          TransactionType = TransactionType.Payment,
          Amount = num
        }
      }, (string) null, out slip);
    }

    public static AuthAnswer13 AuthorizationReturn(Decimal sum, out string slip, string rr)
    {
      int num = (int) (sum * 100M);
      return PilotNt.Authorization(new AuthAnswer13()
      {
        ans = new AuthAnswer()
        {
          TransactionType = TransactionType.Return,
          Amount = num
        },
        RRN = rr
      }, (string) null, out slip);
    }

    private static AuthAnswer13 Authorization(AuthAnswer13 answer, string track2, out string slip)
    {
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.CardAuthorize(track2, ref answer)));
      slip = PilotNt.GlobalFree(answer.ans);
      LogHelper.Debug(slip);
      return answer;
    }

    private static string GlobalFree(AuthAnswer answer)
    {
      string stringAnsi = Marshal.PtrToStringAnsi(answer.Cheque);
      List<string> source = new List<string>();
      if (stringAnsi != null)
      {
        PilotNtInterop.GlobalFree(answer.Cheque);
        source = ((IEnumerable<string>) stringAnsi.Split(PilotNt.CheckSeparator.ToArray(), StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
      }
      return !source.Any<string>() ? stringAnsi : source.First<string>();
    }

    private static void CheckError(Func<int> action)
    {
      int num = action();
      AcquiringException.ErrorTypes errorTypes;
      switch (num)
      {
        case 0:
          return;
        case 99:
          errorTypes = AcquiringException.ErrorTypes.DeviceNoFound;
          break;
        case 2000:
          errorTypes = AcquiringException.ErrorTypes.OperationCancelByClient;
          break;
        case 2002:
          errorTypes = AcquiringException.ErrorTypes.OperationCancelByTimeout;
          break;
        case 4119:
          errorTypes = AcquiringException.ErrorTypes.NoLinkToBank;
          break;
        case 4134:
          errorTypes = AcquiringException.ErrorTypes.NeedToTotal;
          break;
        case 4140:
        case 4141:
          LogHelper.Debug("Сообщение с кодом 4141/4140 от терминала проигнорировано");
          return;
        case 4451:
          errorTypes = AcquiringException.ErrorTypes.NotEnoughMoney;
          break;
        case 4455:
          errorTypes = AcquiringException.ErrorTypes.WrongPinCode;
          break;
        default:
          errorTypes = AcquiringException.ErrorTypes.Unknown;
          break;
      }
      AcquiringException.ErrorTypes key = errorTypes;
      string str = string.Format(Translate.PilotNt_Код___0_, (object) num);
      throw new ErrorHelper.GbsException(AcquiringException.ErrorsDictionary[key] + "\n" + str)
      {
        Direction = key == AcquiringException.ErrorTypes.Unknown ? ErrorHelper.ErrorDirections.Unknown : ErrorHelper.ErrorDirections.Outer
      };
    }

    public static void CommitTransaction(Decimal sum)
    {
      int amount = (int) (sum * 100M);
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.CommitTransaction(amount, (string) null)));
    }

    public static void RollbackTransaction(Decimal sum)
    {
      int amount = (int) (sum * 100M);
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.RollBackTransaction(amount, (string) null)));
    }

    private static void SuspendTransaction(Decimal sum)
    {
      int amount = (int) (sum * 100M);
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.SuspendTransaction(amount, (string) null)));
    }

    public static void CloseShift(out string slip)
    {
      AuthAnswer authAnswer = new AuthAnswer()
      {
        TransactionType = TransactionType.CloseDay
      };
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.CloseDay(ref authAnswer)));
      slip = PilotNt.GlobalFree(authAnswer);
    }

    public static void GetStatistics(out string slip)
    {
      AuthAnswer authAnswer = new AuthAnswer()
      {
        TransactionType = TransactionType.StatDetailed
      };
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.GetStatistics(ref authAnswer)));
      slip = PilotNt.GlobalFree(authAnswer);
    }

    public static void ServiceMenu(out string slip)
    {
      AuthAnswer authAnswer = new AuthAnswer();
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.ServiceMenu(ref authAnswer)));
      slip = PilotNt.GlobalFree(authAnswer);
    }

    public static void SetConfig(int comPort)
    {
      byte[] bAuthCode = Encoding.GetEncoding(1251).GetBytes("ComPort=" + comPort.ToString());
      PilotNt.CheckError((Func<int>) (() => PilotNtInterop.SetConfigData(bAuthCode)));
    }
  }
}
