// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.ManagementForTapBeerViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Egais
{
  public partial class ManagementForTapBeerViewModel : ViewModelWithForm
  {
    private Users.User _authUser;
    public bool IsEditing;

    public ICommand EditTapCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ManagementForTapBeerViewModel.InfoTapBeerItem> list = ((IEnumerable) obj).Cast<ManagementForTapBeerViewModel.InfoTapBeerItem>().ToList<ManagementForTapBeerViewModel.InfoTapBeerItem>();
          if (list.Count != 1)
          {
            MessageBoxHelper.Warning(Translate.ManagementForTapBeerViewModel_EditTapCommand_Для_редактирование_можно_выбрать_только_одну_запись);
          }
          else
          {
            InfoToTapBeer item = list.Single<ManagementForTapBeerViewModel.InfoTapBeerItem>().Info.Clone<InfoToTapBeer>();
            if (!new ConnectBeerToTapViewModel().Show(item, this._authUser))
              return;
            using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
            {
              this.Items[this.Items.ToList<ManagementForTapBeerViewModel.InfoTapBeerItem>().FindIndex((Predicate<ManagementForTapBeerViewModel.InfoTapBeerItem>) (x => x.Info.Tap.Uid == item.Tap.Uid))] = new ManagementForTapBeerViewModel.InfoTapBeerItem(item, dataBase);
              this.OnPropertyChanged("Items");
            }
          }
        }));
      }
    }

    public ICommand ReplaceTapCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.DeleteTap())
            return;
          this.EditTapCommand.Execute((object) new List<ManagementForTapBeerViewModel.InfoTapBeerItem>()
          {
            this.SelectedItem
          });
        }));
      }
    }

    public ICommand DeleteTapCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.DeleteTap()));
    }

    private bool DeleteTap()
    {
      if (this.SelectedItem.Good == null)
      {
        MessageBoxHelper.Warning("На данном кране нет подключенного кега");
        return true;
      }
      using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        DataConnectionTransaction connectionTransaction = dataBase.BeginTransaction();
        bool flag = false;
        if (this.SelectedItem.Stock != 0M)
        {
          flag = true;
          if (MessageBoxHelper.Question(string.Format("Кег на выбранном кране еще не закончился, на остатке доступно еще {0:N2} л., уверены, что хотите снять его с крана?\n\n", (object) this.SelectedItem.Stock) + Translate.ManagementForTapBeerViewModel_DeleteTap_В_этом_случае_весь_указанный_остаток_будет_списан__информация_о_списании_будет_доступна_в_разделе_Документы___Журнал_списаний_) == MessageBoxResult.No)
            return false;
          Gbs.Core.Entities.Goods.Good byUid = new GoodRepository(dataBase).GetByUid(this.SelectedItem.Info.ChildGoodUid);
          if (byUid != null)
          {
            Guid guid = Guid.NewGuid();
            Document document1 = new Document();
            document1.Uid = guid;
            document1.Comment = Translate.ManagementForTapBeerViewModel_DeleteTap_Списание_остатков_разливного_пива_при_отключении_от_крана;
            document1.Type = GlobalDictionaries.DocumentsTypes.WriteOff;
            document1.Number = Gbs.Helpers.Other.GetNumberDocument(GlobalDictionaries.DocumentsTypes.WriteOff);
            Storages.Storage storage = new Storages.Storage();
            storage.Uid = this.SelectedItem.Info.StorageUid;
            document1.Storage = storage;
            document1.Section = Sections.GetCurrentSection();
            document1.Items = new List<Gbs.Core.Entities.Documents.Item>()
            {
              new Gbs.Core.Entities.Documents.Item()
              {
                Good = byUid,
                SellPrice = this.SelectedItem.Info.Price.GetValueOrDefault(),
                Quantity = this.SelectedItem.Stock,
                DocumentUid = guid,
                Comment = this.SelectedItem.Info.MarkedInfo
              }
            };
            document1.UserUid = this._authUser.Uid;
            Document document2 = document1;
            if (!new DocumentsRepository(dataBase).Save(document2))
            {
              connectionTransaction.Rollback();
              return false;
            }
          }
          else if (MessageBoxHelper.Show("Не удалось сформировать акт списания остатков кеги, обратитесь в техническую поддержку для уточнения причины.\n\nПродолжить снятие кега с крана?", buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Exclamation) == MessageBoxResult.No)
            return false;
        }
        if (!this.SelectedItem.Info.IsSendToCrpt)
        {
          flag = true;
          if (MessageBoxHelper.Question(Translate.ManagementForTapBeerViewModel_DeleteTap_) == MessageBoxResult.No)
          {
            connectionTransaction.Rollback();
            return false;
          }
        }
        if (!flag && MessageBoxHelper.Question(Translate.ManagementForTapBeerViewModel_DeleteTap_Вы_уверены__что_хотите_снять_данный_кег_с_крана_) == MessageBoxResult.No)
        {
          connectionTransaction.Rollback();
          return false;
        }
        this.SelectedItem.Info.IsDeleted = true;
        if (!new InfoTapBeerRepository().Save(this.SelectedItem.Info))
        {
          connectionTransaction.Rollback();
          return false;
        }
        connectionTransaction.Commit();
        int index = this.Items.ToList<ManagementForTapBeerViewModel.InfoTapBeerItem>().FindIndex((Predicate<ManagementForTapBeerViewModel.InfoTapBeerItem>) (x => x.Info.Uid == this.SelectedItem.Info.Uid));
        InfoToTapBeer info = new InfoToTapBeer();
        info.Uid = Guid.Empty;
        info.Tap = this.SelectedItem.Info.Tap.Clone<TapBeer>();
        this.SelectedItem = new ManagementForTapBeerViewModel.InfoTapBeerItem(info, (DataBase) null);
        this.Items[index] = this.SelectedItem.Clone<ManagementForTapBeerViewModel.InfoTapBeerItem>();
        this.OnPropertyChanged("Items");
        return true;
      }
    }

    private ManagementForTapBeerViewModel.InfoTapBeerItem SelectedItem { get; set; }

    public static string MoreMenuKey => "MoreMenu";

    public ICommand ShowMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ManagementForTapBeerViewModel.InfoTapBeerItem> list = ((IEnumerable) obj).Cast<ManagementForTapBeerViewModel.InfoTapBeerItem>().ToList<ManagementForTapBeerViewModel.InfoTapBeerItem>();
          if (list.Count != 1)
          {
            MessageBoxHelper.Warning(Translate.ManagementForTapBeerViewModel_ShowMenuCommand_Необходимо_выбрать_только_одну_запись__чтобы_выполнить_действие);
          }
          else
          {
            this.SelectedItem = list.Single<ManagementForTapBeerViewModel.InfoTapBeerItem>();
            this.ShowMenu();
          }
        }));
      }
    }

    public Action ShowMenu { get; set; }

    public ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem> Items { get; set; }

    public void Show()
    {
      (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.ActionsToBeerTap);
      if (!access.Result)
        return;
      this._authUser = access.User;
      this.LoadInfo();
      this.FormToSHow = (WindowWithSize) new FrmManagementForTapBeer();
      Gbs.Helpers.ControlsHelpers.DataGrid.Other.FocusRow(((FrmManagementForTapBeer) this.FormToSHow).TapDataGrid);
      this.ShowMenu = new Action(((FrmManagementForTapBeer) this.FormToSHow).ShowMoreMenu);
      this.ShowForm();
    }

    private void LoadInfo()
    {
      List<TapBeer> activeItems1 = new TapBeerRepository().GetActiveItems();
      List<InfoToTapBeer> activeItems2 = new InfoTapBeerRepository().GetActiveItems();
      using (DataBase dataBase = Gbs.Core.Data.GetDataBase())
      {
        List<ManagementForTapBeerViewModel.InfoTapBeerItem> source = new List<ManagementForTapBeerViewModel.InfoTapBeerItem>();
        foreach (TapBeer tapBeer1 in activeItems1)
        {
          TapBeer tapBeer = tapBeer1;
          InfoToTapBeer info = activeItems2.OrderByDescending<InfoToTapBeer, DateTime?>((Func<InfoToTapBeer, DateTime?>) (x => x.ConnectingDateTime)).FirstOrDefault<InfoToTapBeer>((Func<InfoToTapBeer, bool>) (x => x.Tap.Uid == tapBeer.Uid));
          if (info == null)
          {
            List<ManagementForTapBeerViewModel.InfoTapBeerItem> infoTapBeerItemList = source;
            ManagementForTapBeerViewModel.InfoTapBeerItem infoTapBeerItem = new ManagementForTapBeerViewModel.InfoTapBeerItem();
            InfoToTapBeer infoToTapBeer = new InfoToTapBeer();
            infoToTapBeer.Uid = Guid.Empty;
            infoToTapBeer.Tap = tapBeer;
            infoTapBeerItem.Info = infoToTapBeer;
            infoTapBeerItemList.Add(infoTapBeerItem);
          }
          else
            source.Add(new ManagementForTapBeerViewModel.InfoTapBeerItem(info, dataBase));
        }
        this.Items = new ObservableCollection<ManagementForTapBeerViewModel.InfoTapBeerItem>((IEnumerable<ManagementForTapBeerViewModel.InfoTapBeerItem>) source.OrderBy<ManagementForTapBeerViewModel.InfoTapBeerItem, int>((Func<ManagementForTapBeerViewModel.InfoTapBeerItem, int>) (x => x.Info.Tap.Index)));
      }
    }

    public class InfoTapBeerItem
    {
      public Decimal Stock { get; set; }

      public InfoToTapBeer Info { get; set; }

      public Gbs.Core.Entities.Goods.Good Good { get; set; }

      public InfoTapBeerItem(InfoToTapBeer info, DataBase db)
      {
        this.Info = info;
        if (info.GoodUid != Guid.Empty)
          this.Good = new GoodRepository(db).GetByUid(info.GoodUid);
        this.Stock = info.Quantity.GetValueOrDefault() - info.SaleQuantity;
      }

      public InfoTapBeerItem()
      {
      }
    }
  }
}
