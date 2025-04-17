// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Cafe.OrderCombineViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Documents;
using Gbs.Core.ViewModels.Cafe;
using Gbs.Forms.Clients;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Cafe
{
  public partial class OrderCombineViewModel : ViewModelWithForm
  {
    private bool _saveResult;

    public bool ShowCombineOrder(
      List<CafeActiveOrdersViewModel.CafeOrder> ordersList)
    {
      this.Orders = new ObservableCollection<CafeActiveOrdersViewModel.CafeOrder>((IEnumerable<CafeActiveOrdersViewModel.CafeOrder>) ordersList.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.Status == GlobalDictionaries.DocumentsStatuses.Draft)).ToList<CafeActiveOrdersViewModel.CafeOrder>().OrderByDescending<CafeActiveOrdersViewModel.CafeOrder, DateTime>((Func<CafeActiveOrdersViewModel.CafeOrder, DateTime>) (x => x.Document.DateTime)));
      this.FormToSHow = (WindowWithSize) new FrmOrderCombine();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
      return this._saveResult;
    }

    public ObservableCollection<CafeActiveOrdersViewModel.CafeOrder> Orders { get; set; } = new ObservableCollection<CafeActiveOrdersViewModel.CafeOrder>();

    public ICommand CombineOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<CafeActiveOrdersViewModel.CafeOrder> listItems = ((IEnumerable) obj).Cast<CafeActiveOrdersViewModel.CafeOrder>().ToList<CafeActiveOrdersViewModel.CafeOrder>();
          if (listItems.Count < 2)
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.OrderCombineViewModel_Необходимо_выбрать_2_или_более_заказов_для_объединения_);
          }
          else
          {
            Guid guid = Guid.Empty;
            if (listItems.Any<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.ContractorUid != Guid.Empty)))
            {
              if (listItems.All<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.ContractorUid == listItems.First<CafeActiveOrdersViewModel.CafeOrder>().Document.ContractorUid)))
              {
                guid = listItems.First<CafeActiveOrdersViewModel.CafeOrder>().Document.ContractorUid;
              }
              else
              {
                List<CafeActiveOrdersViewModel.CafeOrder> list = listItems.Where<CafeActiveOrdersViewModel.CafeOrder>((Func<CafeActiveOrdersViewModel.CafeOrder, bool>) (x => x.Document.ContractorUid != Guid.Empty)).ToList<CafeActiveOrdersViewModel.CafeOrder>();
                if (list.Count<CafeActiveOrdersViewModel.CafeOrder>() == 1)
                {
                  guid = list.Single<CafeActiveOrdersViewModel.CafeOrder>().Document.ContractorUid;
                }
                else
                {
                  (Client client, bool result) client1 = new FrmSearchClient().GetClient();
                  Client client2 = client1.client;
                  if (!client1.result)
                    return;
                  guid = client2.Uid;
                }
              }
            }
            Document document = new Document()
            {
              ContractorUid = guid
            };
            if (listItems.SelectMany<CafeActiveOrdersViewModel.CafeOrder, Gbs.Core.Entities.Documents.Item>((Func<CafeActiveOrdersViewModel.CafeOrder, IEnumerable<Gbs.Core.Entities.Documents.Item>>) (x => (IEnumerable<Gbs.Core.Entities.Documents.Item>) x.Document.Items)).Any<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid == GlobalDictionaries.PercentForServiceGoodUid)) && MessageBoxHelper.Show(Translate.OrderCombineViewModel_CombineOrderCommand_При_объединении_будет_заново_пересчитан_процент_за_обслуживание_по_этим_заказам__продолжить_объединение_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            CafeBasket basket = new CafeBasket();
            foreach (CafeActiveOrdersViewModel.CafeOrder cafeOrder in listItems)
              document.Items.AddRange(cafeOrder.Document.Items.Where<Gbs.Core.Entities.Documents.Item>((Func<Gbs.Core.Entities.Documents.Item, bool>) (x => x.GoodUid != GlobalDictionaries.PercentForServiceGoodUid)));
            new CafeHelper(basket).AddItemCafeOrder(document);
            basket.Storage = listItems.First<CafeActiveOrdersViewModel.CafeOrder>().Document.Storage;
            if (basket.SaveCafe(false, Guid.Empty, false).Result != ActionResult.Results.Ok)
              return;
            using (DataBase dataBase = Data.GetDataBase())
            {
              DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
              foreach (CafeActiveOrdersViewModel.CafeOrder cafeOrder in listItems)
              {
                cafeOrder.Document.IsDeleted = true;
                cafeOrder.Document.Status = GlobalDictionaries.DocumentsStatuses.Close;
                cafeOrder.Document.Comment = Translate.OrderCombineViewModel_ОБъединенВЗаказ + basket.Document.Number;
                cafeOrder.Document.Items.Clear();
                documentsRepository.Save(cafeOrder.Document, false);
              }
              int num2 = (int) MessageBoxHelper.Show(Translate.OrderCombineViewModel_ВыбранныеЗАказыОбъединены + basket.Document.Number);
              this._saveResult = true;
              this.CloseAction();
            }
          }
        }));
      }
    }

    public ICommand CancelCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }
  }
}
