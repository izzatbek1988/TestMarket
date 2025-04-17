// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.Users
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Resources.Localizations;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Config
{
  public class Users
  {
    public bool IsCutUserNameForPrint { get; set; }

    public Users.AuthorizationMethods DefaultAuthorizationMethod { get; set; }

    public bool NotRequestAuthorizationForSingleOnlineUser { get; set; } = true;

    public bool RequestAuthorizationOnSale { get; set; } = true;

    public bool IsSellerReport { get; set; }

    public static Dictionary<Users.AuthorizationMethods, string> AuthorizationMethodsDictionary()
    {
      return new Dictionary<Users.AuthorizationMethods, string>()
      {
        {
          Users.AuthorizationMethods.LoginPassword,
          Translate.Users_Пароль
        },
        {
          Users.AuthorizationMethods.Barcode,
          Translate.Users_Штрих_код
        }
      };
    }

    public int CountDayActionHistory { get; set; } = 30;

    public enum AuthorizationMethods
    {
      LoginPassword,
      Barcode,
    }
  }
}
