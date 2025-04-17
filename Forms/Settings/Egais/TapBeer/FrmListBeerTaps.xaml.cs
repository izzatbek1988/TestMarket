// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Egais.ListBeerTapsViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Egais;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Egais
{
  public partial class ListBeerTapsViewModel : ViewModelWithForm
  {
    public ObservableCollection<TapBeer> BeerTaps { get; set; }

    public void Show()
    {
      this.BeerTaps = new ObservableCollection<TapBeer>(new TapBeerRepository().GetActiveItems());
      this.FormToSHow = (WindowWithSize) new FrmListBeerTaps();
      this.ShowForm();
    }

    public ICommand AddCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          TapBeer tapBeer;
          if (!new BeerTapCardModelView().Show(Guid.Empty, out tapBeer))
            return;
          this.BeerTaps.Add(tapBeer);
          this.OnPropertyChanged("BeerTaps");
        }));
      }
    }

    public ICommand EditCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<TapBeer> list = ((IEnumerable) obj).Cast<TapBeer>().ToList<TapBeer>();
          if (list.Count != 1)
          {
            MessageBoxHelper.Warning("Для редактирование требуется выбрать одну запись пивного крана.");
          }
          else
          {
            TapBeer tapBeer;
            if (!new BeerTapCardModelView().Show(list.Single<TapBeer>().Uid, out tapBeer))
              return;
            this.BeerTaps[this.BeerTaps.ToList<TapBeer>().FindIndex((Predicate<TapBeer>) (x => x.Uid == tapBeer.Uid))] = tapBeer;
            this.OnPropertyChanged("BeerTaps");
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
          List<TapBeer> list1 = ((IEnumerable) obj).Cast<TapBeer>().ToList<TapBeer>();
          if (!list1.Any<TapBeer>())
          {
            MessageBoxHelper.Warning("Для удаления требуется выбрать хотя бы одну запись пивного крана.");
          }
          else
          {
            if (MessageBoxHelper.Question(string.Format("Вы уверены, что хотите удалить {0} пивных крана из программы?", (object) list1.Count)) == MessageBoxResult.No)
              return;
            TapBeerRepository tapBeerRepository = new TapBeerRepository();
            List<InfoToTapBeer> list2 = new InfoTapBeerRepository().GetActiveItems().Where<InfoToTapBeer>((Func<InfoToTapBeer, bool>) (x => !x.IsDeleted)).ToList<InfoToTapBeer>();
            List<TapBeer> list3 = this.BeerTaps.Clone<ObservableCollection<TapBeer>>().ToList<TapBeer>();
            List<string> stringList = new List<string>();
            foreach (TapBeer tapBeer in list1)
            {
              TapBeer tap = tapBeer;
              if (list2.Any<InfoToTapBeer>((Func<InfoToTapBeer, bool>) (x => x.Tap.Uid == tap.Uid)))
              {
                stringList.Add("- " + tap.Name);
              }
              else
              {
                tap.IsDeleted = true;
                if (tapBeerRepository.Save(tap))
                  list3.RemoveAll((Predicate<TapBeer>) (x => x.Uid == tap.Uid));
              }
            }
            this.BeerTaps = new ObservableCollection<TapBeer>(list3);
            this.OnPropertyChanged("BeerTaps");
            if (!stringList.Any<string>())
              return;
            MessageBoxHelper.Warning("Следующие краны нельзя удалить, пока к ним подключены кеги:\n\n" + string.Join("\n", (IEnumerable<string>) stringList));
          }
        }));
      }
    }
  }
}
