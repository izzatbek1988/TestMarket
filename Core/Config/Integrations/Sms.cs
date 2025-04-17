// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Sms
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Resources.Localizations;

#nullable disable
namespace Gbs.Core.Config
{
  public class Sms
  {
    public GlobalDictionaries.SmsServiceType Type { get; set; }

    public CryptoConfig ApiKey { get; set; } = new CryptoConfig();

    public CryptoConfig Password { get; set; } = new CryptoConfig();

    public string Sender { get; set; }

    public CryptoConfig Login { get; set; } = new CryptoConfig();

    public string TextSmsForCode { get; set; } = Translate.Sms_TextSmsForCode_Код_подтверждения_для_списания_баллов__code__;
  }
}
