// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Legal.MarkedTypeListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Settings.BackEnd;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Legal
{
  public partial class MarkedTypeListViewModel : ViewModelWithForm
  {
    public ObservableCollection<MarkedTypeListViewModel.MarkGroup> Items { get; set; }

    public void ShowMarkGroups()
    {
      List<MarkGroupSettings> groups = new MarkGroupRepository().GetGroups();
      List<MarkedTypeListViewModel.MarkGroup> source = new List<MarkedTypeListViewModel.MarkGroup>();
      foreach (MarkGroupEnum markGroup1 in this.MarkGroups)
      {
        MarkGroupEnum markGroup = markGroup1;
        if (groups.Any<MarkGroupSettings>((Func<MarkGroupSettings, bool>) (x => x.Group == markGroup)))
          source.Add(new MarkedTypeListViewModel.MarkGroup(groups.Single<MarkGroupSettings>((Func<MarkGroupSettings, bool>) (x => x.Group == markGroup))));
        else
          source.Add(new MarkedTypeListViewModel.MarkGroup(new MarkGroupSettings()
          {
            Group = markGroup
          }));
      }
      this.Items = new ObservableCollection<MarkedTypeListViewModel.MarkGroup>((IEnumerable<MarkedTypeListViewModel.MarkGroup>) source.OrderByDescending<MarkedTypeListViewModel.MarkGroup, bool>((Func<MarkedTypeListViewModel.MarkGroup, bool>) (x => x.IsActive)).ThenBy<MarkedTypeListViewModel.MarkGroup, string>((Func<MarkedTypeListViewModel.MarkGroup, string>) (x => x.Name)));
      this.FormToSHow = (WindowWithSize) new FrmMarkedTypeList();
      this.ShowForm();
    }

    public void Save()
    {
      List<MarkGroupSettings> list = this.Items.Select<MarkedTypeListViewModel.MarkGroup, MarkGroupSettings>((Func<MarkedTypeListViewModel.MarkGroup, MarkGroupSettings>) (x => x.Item)).ToList<MarkGroupSettings>();
      new MarkGroupRepository().Save(list);
      CheckCodesHelper.MarkGroups = new List<MarkGroupSettings>((IEnumerable<MarkGroupSettings>) list);
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Save();
          this.CloseAction();
        }));
      }
    }

    public ICommand CloseCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.CloseAction()));
    }

    private static string GetNameForGroup(MarkGroupEnum group)
    {
      switch (group)
      {
        case MarkGroupEnum.Lp:
          return "Легкая промышленность";
        case MarkGroupEnum.Shoes:
          return "Обувные товары";
        case MarkGroupEnum.Tobacco:
          return "Табачная продукция";
        case MarkGroupEnum.Perfumery:
          return "Духи и туалетная вода";
        case MarkGroupEnum.Tires:
          return "Шины и покрышки";
        case MarkGroupEnum.Electronics:
          return "Фотокамеры";
        case MarkGroupEnum.Pharma:
          return "Лекарственные препараты";
        case MarkGroupEnum.Milk:
          return "Молочная продукция";
        case MarkGroupEnum.Bicycle:
          return "Велосипеды и рамы";
        case MarkGroupEnum.Wheelchairs:
          return "Медицинские изделия";
        case MarkGroupEnum.Otp:
          return "Альтернативная табачная продукция";
        case MarkGroupEnum.Water:
          return "Упакованная вода";
        case MarkGroupEnum.Furs:
          return "Товары из натурального меха";
        case MarkGroupEnum.Beer:
          return "Пивная продукция";
        case MarkGroupEnum.Ncp:
          return "Никотиносодержая продукция";
        case MarkGroupEnum.Bio:
          return "БАДы";
        case MarkGroupEnum.Antiseptic:
          return "Антиспетические средства";
        case MarkGroupEnum.PetFood:
          return "Корма для животных";
        case MarkGroupEnum.SeaFood:
          return "Морепродукты";
        case MarkGroupEnum.NaBeer:
          return "Безалкогольное пиво";
        case MarkGroupEnum.SoftDrinks:
          return "Соки / безалкогольные напитки";
        case MarkGroupEnum.VetPharma:
          return "Ветеринарные препараты";
        case MarkGroupEnum.Conserve:
          return "Консервированная продукция";
        case MarkGroupEnum.VegetableOil:
          return "Растительные масла";
        default:
          throw new ArgumentOutOfRangeException(nameof (group), (object) group, (string) null);
      }
    }

    public List<MarkGroupEnum> MarkGroups { get; set; } = new List<MarkGroupEnum>()
    {
      MarkGroupEnum.Lp,
      MarkGroupEnum.Shoes,
      MarkGroupEnum.Tobacco,
      MarkGroupEnum.Perfumery,
      MarkGroupEnum.Tires,
      MarkGroupEnum.Electronics,
      MarkGroupEnum.Pharma,
      MarkGroupEnum.Milk,
      MarkGroupEnum.Bicycle,
      MarkGroupEnum.Wheelchairs,
      MarkGroupEnum.Otp,
      MarkGroupEnum.Water,
      MarkGroupEnum.Furs,
      MarkGroupEnum.Beer,
      MarkGroupEnum.Ncp,
      MarkGroupEnum.Bio,
      MarkGroupEnum.Antiseptic,
      MarkGroupEnum.PetFood,
      MarkGroupEnum.SeaFood,
      MarkGroupEnum.NaBeer,
      MarkGroupEnum.SoftDrinks,
      MarkGroupEnum.VetPharma,
      MarkGroupEnum.Conserve,
      MarkGroupEnum.VegetableOil
    };

    public class MarkGroup : ViewModel
    {
      public MarkGroupSettings Item { get; set; }

      public string Name { get; set; }

      public bool IsActive
      {
        get => this.Item.IsActive;
        set
        {
          this.Item.IsActive = value;
          this.OnPropertyChanged("IsEnabledAllowedMode");
          this.OnPropertyChanged("IsEnabledOwner");
          if (value)
            return;
          this.IsAllowedMode = false;
        }
      }

      public bool IsAllowedMode
      {
        get => this.Item.IsAllowedMode;
        set
        {
          this.Item.IsAllowedMode = value;
          this.OnPropertyChanged(nameof (IsAllowedMode));
          this.OnPropertyChanged("IsEnabledOwner");
          if (value)
            return;
          this.IsCheckOwner = false;
        }
      }

      public bool IsCheckOwner
      {
        get => this.Item.IsCheckOwner;
        set
        {
          this.Item.IsCheckOwner = value;
          this.OnPropertyChanged(nameof (IsCheckOwner));
        }
      }

      public bool IsEnabledAllowedMode => this.Item.IsActive;

      public bool IsEnabledOwner => this.Item.IsAllowedMode;

      public MarkGroup(MarkGroupSettings item)
      {
        this.Item = item;
        this.Name = MarkedTypeListViewModel.GetNameForGroup(item.Group);
      }
    }
  }
}
