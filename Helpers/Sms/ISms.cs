// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ISms
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Helpers
{
  public interface ISms
  {
    int SendSms(string text, List<string> phones, out List<string> errorList);

    int SendSms(Dictionary<int, string> phonesAndMsg, out List<string> errorList);

    bool GetCodeForCall(string phone, out string code, out string error);

    bool SendMsgByViber(string phone, string msg, out string error);

    bool SendMsgByWhatsApp(string phone, string msg, out string error);

    Decimal GetBalance();
  }
}
