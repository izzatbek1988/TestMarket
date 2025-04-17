// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WebOffice.Client
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;

#nullable disable
namespace Gbs.Helpers.WebOffice
{
  public class Client : ClientSimple, IEntity
  {
    public bool IsDeleted { get; set; }

    public ClientGroup Group { get; set; }

    public string Barcode { get; set; }

    public string Name { get; set; }

    public string PhoneHash { get; set; }

    public ClientSums Sums { get; set; } = new ClientSums();

    public Client(ClientAdnSum client)
    {
      this.Uid = client.Client.Uid;
      this.Barcode = client.Client.Barcode;
      this.Name = Functions.CutName(client.Client.Name, true);
      this.PhoneHash = CryptoHelper.GetMd5Hash(client.Client.Phone);
      this.Sums.Bonuses = client.CurrentBonusSum;
      this.Sums.Credit = client.CurrentCreditSum < 0.03M ? 0M : client.CurrentCreditSum;
      this.Sums.Sales = client.CurrentSalesSum;
      this.IsDeleted = client.Client.IsDeleted;
      this.Group = new ClientGroup()
      {
        Name = client.Client.Group.Name,
        Uid = client.Client.Group.Uid
      };
    }
  }
}
