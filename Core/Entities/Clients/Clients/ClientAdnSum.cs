// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Clients.ClientAdnSum
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using System;

#nullable disable
namespace Gbs.Core.Entities.Clients
{
  public class ClientAdnSum
  {
    public Client Client { get; set; } = new Client();

    public Decimal TotalSalesSum => Math.Round(this.CurrentSalesSum + this.CloudSalesSum, 2);

    public Decimal CurrentSalesSum { get; set; }

    public Decimal CloudSalesSum { get; set; }

    public Decimal TotalCreditSum => Math.Round(this.CurrentCreditSum + this.CloudCreditSum, 2);

    public Decimal CurrentCreditSum { get; set; }

    public Decimal CloudCreditSum { get; set; }

    public Decimal TotalBonusSum => Math.Round(this.CurrentBonusSum + this.CloudBonusSum, 2);

    public Decimal CurrentBonusSum { get; set; }

    public Decimal CloudBonusSum { get; set; }

    public string InfoSaleSum
    {
      get
      {
        return string.Format("{0:N2}", (object) this.CurrentSalesSum) + (this.CloudSalesSum != 0M ? string.Format(" ({0} = {1:N2})", (object) Functions.GetSignedNumber(this.CloudSalesSum), (object) this.TotalSalesSum) : "");
      }
    }

    public string InfoCreditSum
    {
      get
      {
        return string.Format("{0:N2}", (object) this.CurrentCreditSum) + (this.CloudCreditSum != 0M ? string.Format(" ({0} = {1:N2})", (object) Functions.GetSignedNumber(this.CloudCreditSum), (object) this.TotalCreditSum) : "");
      }
    }

    public string InfoBonusSum
    {
      get
      {
        return string.Format("{0:N2}", (object) this.CurrentBonusSum) + (this.CloudBonusSum != 0M ? string.Format(" ({0} = {1:N2})", (object) Functions.GetSignedNumber(this.CloudBonusSum), (object) this.TotalBonusSum) : "");
      }
    }
  }
}
