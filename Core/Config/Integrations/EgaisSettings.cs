// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.EgaisSettings
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Config
{
  public class EgaisSettings
  {
    public bool IsActive { get; set; }

    public string RarId { get; set; }

    public string PathUtm { get; set; } = "http://localhost:8080";

    [Obsolete("Не использовать. Порт вводится вместе с адресу в строку PathUtm")]
    public int? Port { get; set; }

    public bool IsLimitedForTime { get; set; }

    public bool IsBanOpenNegativeBeerKega { get; set; } = true;

    public bool IsShowTapInSelectGood { get; set; }

    public DateTime StartTimeLimited { get; set; }

    public DateTime FinishTimeLimited { get; set; }

    public ActionResult ValidateSetting()
    {
      if (!this.IsActive)
        return new ActionResult(ActionResult.Results.Ok);
      List<string> stringList = new List<string>();
      if (this.RarId.IsNullOrEmpty())
        stringList.Add("Не указан RAR ID");
      else
        this.RarId = this.RarId.Trim();
      if (this.PathUtm.IsNullOrEmpty())
        stringList.Add("Необходимо указать адрес для подключения к УТМ");
      return stringList.Any<string>() ? new ActionResult(ActionResult.Results.Error, stringList) : new ActionResult(ActionResult.Results.Ok);
    }
  }
}
