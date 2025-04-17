// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Egais.EgaisWriteOffViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Egais;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Egais
{
  public partial class EgaisWriteOffViewModel : ViewModelWithForm
  {
    public List<EgaisWriteOffViewModel.EgaisWriteOffItem> Items { get; set; } = new List<EgaisWriteOffViewModel.EgaisWriteOffItem>();

    public ICommand DoWriteOffCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.Items.Any<EgaisWriteOffViewModel.EgaisWriteOffItem>((Func<EgaisWriteOffViewModel.EgaisWriteOffItem, bool>) (x => x.IsSend)))
          {
            MessageBoxHelper.Warning("Необходимо выбрать хотя бы один товар для отправки в ЕГАИС, укажите товары для передачи и повторите отправку формирования акта.");
          }
          else
          {
            ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar("Отправка акта списания в УТМ");
            try
            {
              foreach (IGrouping<TypeWriteOff1, EgaisWriteOffViewModel.EgaisWriteOffItem> source in this.Items.Where<EgaisWriteOffViewModel.EgaisWriteOffItem>((Func<EgaisWriteOffViewModel.EgaisWriteOffItem, bool>) (x => x.IsSend)).GroupBy<EgaisWriteOffViewModel.EgaisWriteOffItem, TypeWriteOff1>((Func<EgaisWriteOffViewModel.EgaisWriteOffItem, TypeWriteOff1>) (x => x.ActItem.ActType)))
              {
                new EgaisRepository().DoWriteOffItem(source.Select<EgaisWriteOffViewModel.EgaisWriteOffItem, EgaisWriteOffActsItems>((Func<EgaisWriteOffViewModel.EgaisWriteOffItem, EgaisWriteOffActsItems>) (x => x.ActItem)).ToList<EgaisWriteOffActsItems>(), source.Key);
                Thread.Sleep(5000);
              }
              if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
                new HomeOfficeHelper().PrepareAndSend<List<EgaisWriteOffActsItems>>(this.Items.Where<EgaisWriteOffViewModel.EgaisWriteOffItem>((Func<EgaisWriteOffViewModel.EgaisWriteOffItem, bool>) (x => x.IsSend)).Select<EgaisWriteOffViewModel.EgaisWriteOffItem, EgaisWriteOffActsItems>((Func<EgaisWriteOffViewModel.EgaisWriteOffItem, EgaisWriteOffActsItems>) (x => x.ActItem)).ToList<EgaisWriteOffActsItems>(), HomeOfficeHelper.EntityEditHome.EgaisWriteOffActsItemsList);
              progressBar.Close();
              int num = (int) MessageBoxHelper.Show(Translate.EgaisWriteOffViewModel_DoWriteOffCommand_Акт_списания_успешно_сформирован_и_отправлен_в_УТМ);
              this.CloseAction();
            }
            catch (Exception ex)
            {
              progressBar.Close();
              LogHelper.Error(ex, "Не удалось отправить акт списания, проверьте подключение к УТМ или обратитесь в поддержку.");
            }
          }
        }));
      }
    }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<EgaisWriteOffViewModel.EgaisWriteOffItem> list = ((IEnumerable) obj).Cast<EgaisWriteOffViewModel.EgaisWriteOffItem>().ToList<EgaisWriteOffViewModel.EgaisWriteOffItem>();
          if (!list.Any<EgaisWriteOffViewModel.EgaisWriteOffItem>())
          {
            MessageBoxHelper.Warning("Необходимо выбрать хотя бы одну позицию из списка для удаления.");
          }
          else
          {
            if (MessageBoxHelper.Question(string.Format("Вы уверены, что хотите удалить из списка для формирования акта списания в ЕГАИС {0} позиций?\n\n", (object) list.Count) + "После этого алкоголь можно будет списать только вручную через сторонние сервисы ЕГАИС.") == MessageBoxResult.No)
              return;
            EgaisWriteOffActsItemRepository actsItemRepository = new EgaisWriteOffActsItemRepository();
            List<EgaisWriteOffViewModel.EgaisWriteOffItem> collection = new List<EgaisWriteOffViewModel.EgaisWriteOffItem>((IEnumerable<EgaisWriteOffViewModel.EgaisWriteOffItem>) this.Items);
            List<EgaisWriteOffActsItems> command = new List<EgaisWriteOffActsItems>();
            foreach (EgaisWriteOffActsItems writeOffActsItems in list.Select<EgaisWriteOffViewModel.EgaisWriteOffItem, EgaisWriteOffActsItems>((Func<EgaisWriteOffViewModel.EgaisWriteOffItem, EgaisWriteOffActsItems>) (egaisWriteOffItem => egaisWriteOffItem.ActItem.Clone<EgaisWriteOffActsItems>())))
            {
              EgaisWriteOffActsItems item = writeOffActsItems;
              item.IsDeleted = true;
              actsItemRepository.Save(item);
              command.Add(item);
              collection.RemoveAll((Predicate<EgaisWriteOffViewModel.EgaisWriteOffItem>) (x => x.ActItem.Uid == item.Uid));
            }
            if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram == GlobalDictionaries.Mode.Home)
              new HomeOfficeHelper().PrepareAndSend<List<EgaisWriteOffActsItems>>(command, HomeOfficeHelper.EntityEditHome.EgaisWriteOffActsItemsList);
            this.Items = new List<EgaisWriteOffViewModel.EgaisWriteOffItem>((IEnumerable<EgaisWriteOffViewModel.EgaisWriteOffItem>) collection);
            this.OnPropertyChanged("Items");
          }
        }));
      }
    }

    public void Show()
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.EgaisWriteOffViewModel_Show_Загрузка_списка_товаров_для_списания);
      List<EgaisWriteOffActsItems> list = new EgaisWriteOffActsItemRepository().GetActiveItems().Where<EgaisWriteOffActsItems>((Func<EgaisWriteOffActsItems, bool>) (x => x.ActUid == Guid.Empty)).ToList<EgaisWriteOffActsItems>();
      if (!list.Any<EgaisWriteOffActsItems>())
      {
        progressBar.Close();
        MessageBoxHelper.Warning(Translate.EgaisWriteOffViewModel_Show_Нет_доступных_позиций_для_списания__Перед_списанием_необходимо_продать_товары_);
      }
      else
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          List<Gbs.Core.Entities.Goods.Good> allGood = new GoodRepository(dataBase).GetAllItems();
          List<GoodsStocks.GoodStock> allStocks = GoodsStocks.GetGoodStockList();
          this.Items = new List<EgaisWriteOffViewModel.EgaisWriteOffItem>(list.Select<EgaisWriteOffActsItems, EgaisWriteOffViewModel.EgaisWriteOffItem>((Func<EgaisWriteOffActsItems, EgaisWriteOffViewModel.EgaisWriteOffItem>) (x => new EgaisWriteOffViewModel.EgaisWriteOffItem(x, allGood, allStocks))));
          progressBar.Close();
          this.FormToSHow = (WindowWithSize) new FrmEgaisWriteOff();
          this.ShowForm();
        }
      }
    }

    public class EgaisWriteOffItem
    {
      public bool IsSend { get; set; } = true;

      public Gbs.Core.Entities.Goods.Good Good { get; set; }

      public EgaisWriteOffActsItems ActItem { get; set; }

      public string ActType { get; set; }

      public EgaisWriteOffItem(
        EgaisWriteOffActsItems actItem,
        List<Gbs.Core.Entities.Goods.Good> goods,
        List<GoodsStocks.GoodStock> stocks)
      {
        try
        {
          this.ActItem = actItem;
          this.ActType = GlobalDictionaries.EgaisTypeWriteOffDictionary[this.ActItem.ActType];
          GoodsStocks.GoodStock stock = stocks.FirstOrDefault<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, bool>) (x => x.Uid == actItem.StockUid));
          if (stock == null)
            return;
          this.Good = goods.First<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Uid == stock.GoodUid));
          if (this.Good.Group.RuMarkedProductionType == GlobalDictionaries.RuMarkedProductionTypes.Alcohol && EgaisHelper.GetAlcoholType(this.Good) == EgaisHelper.AlcoholTypeGorEgais.StrongAlcohol)
            return;
          this.ActItem.MarkInfo = "";
        }
        catch
        {
          LogHelper.Debug("");
        }
      }
    }
  }
}
