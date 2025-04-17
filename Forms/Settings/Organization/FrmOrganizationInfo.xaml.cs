// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Organization.OrganizationInfoViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms.Clients;
using Gbs.Helpers.MVVM;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Organization
{
  public partial class OrganizationInfoViewModel : ViewModelWithForm
  {
    public ICommand SavePoint { get; set; }

    public ICommand SelectOrganization { get; set; }

    public Action Close { get; set; }

    public SalePoints.SalePoint Point { get; set; }

    public OrganizationInfoViewModel()
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        this.Point = SalePoints.GetSalePointList(dataBase.GetTable<SALE_POINTS>().Where<SALE_POINTS>((Expression<Func<SALE_POINTS, bool>>) (x => x.IS_DELETED == false))).FirstOrDefault<SalePoints.SalePoint>() ?? new SalePoints.SalePoint();
        this.SavePoint = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.Point.Save())
            return;
          this.Close();
        }));
        this.SelectOrganization = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          (Client client, bool result) client1 = new FrmSearchClient().GetClient();
          Client client2 = client1.client;
          if (!client1.result)
            return;
          this.Point.Organization = client2;
          this.OnPropertyChanged(nameof (Point));
        }));
      }
    }

    public void EditClientCard()
    {
      if (this.Point.Organization == null)
      {
        this.SelectOrganization.Execute((object) null);
      }
      else
      {
        ClientAdnSum client;
        if (!new FrmClientCard().ShowCard(this.Point.Organization.Uid, out client, action: Actions.ClientsEdit))
          return;
        this.Point.Organization = client.Client;
        this.OnPropertyChanged("Point");
      }
    }
  }
}
