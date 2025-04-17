// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.MainJoinViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class MainJoinViewModel : ViewModelWithForm
  {
    public List<MainJoinViewModel.MainItem> Items { get; set; }

    public MainJoinViewModel.MainItem SelectedItem { get; set; }

    public MainJoinViewModel.MainItem SelectEntity(IEnumerable<MainJoinViewModel.MainItem> items)
    {
      this.Items = new List<MainJoinViewModel.MainItem>(items);
      this.FormToSHow = (WindowWithSize) new FrmSelectMainJoin();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.ShowForm();
      return this.SelectedItem;
    }

    public ICommand CloseCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.SelectedItem = new MainJoinViewModel.MainItem()
          {
            Uid = Guid.Empty
          };
          this.CloseAction();
        }));
      }
    }

    public ICommand SelectCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedItem == null)
            MessageBoxHelper.Warning(Translate.MainJoinViewModel_SelectCommand_Необходимо_выбрать_основную_сущность_для_объединения_);
          else
            this.CloseAction();
        }));
      }
    }

    public class MainItem
    {
      public Guid Uid { get; set; }

      public string Name { get; set; }
    }
  }
}
