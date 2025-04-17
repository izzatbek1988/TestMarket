// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Sale.CreditViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;
using Gbs.Forms.Clients;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Sale
{
  public partial class CreditViewModel : ViewModelWithForm
  {
    private Client _client;

    public ICommand SelectClient { get; set; }

    public ICommand SaveCredit { get; set; }

    public Action Close { get; set; }

    public bool Result { get; set; }

    public Decimal SumSale { get; set; }

    public string NameClient
    {
      get
      {
        return this.Client != null ? this.Client.Name + (this.Client.Phone.IsNullOrEmpty() ? "" : " (" + this.Client.Phone + ")") : Translate.FrmJournalSales_ВыберитеКлиента;
      }
    }

    public bool IsEnabled { get; set; } = true;

    public Client Client
    {
      get => this._client;
      set
      {
        this._client = value;
        this.OnPropertyChanged("NameClient");
      }
    }

    public string Comment { get; set; } = string.Empty;

    public CreditViewModel()
    {
      this.SaveCredit = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.Client == null)
        {
          int num = (int) MessageBoxHelper.Show(Translate.CreditViewModel_Нужно_выбрать_клиента);
        }
        else
        {
          this.Result = true;
          this.Close();
        }
      }));
      this.SelectClient = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        (Client client, bool result) client1 = new FrmSearchClient().GetClient();
        Client client2 = client1.client;
        if (!client1.result)
          return;
        this.Client = client2;
      }));
    }
  }
}
