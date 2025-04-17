// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.ExtraPrintersViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
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
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public partial class ExtraPrintersViewModel : ViewModel
  {
    public ObservableCollection<ExtraPrinters.ExtraPrinter> Printers { get; set; } = new ObservableCollection<ExtraPrinters.ExtraPrinter>();

    public Gbs.Core.Config.Devices Devices { get; set; }

    public ICommand AddPrinterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          ExtraPrinters.ExtraPrinter printer = new ExtraPrinters.ExtraPrinter();
          if (!new ExtraPrinterCardViewModel().ShowCard(true, ref printer))
            return;
          this.Printers.Add(printer);
          this.OnPropertyChanged("Printers");
        }));
      }
    }

    public ICommand EditPrinterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ExtraPrinters.ExtraPrinter> listPrinters = ((IEnumerable) obj).Cast<ExtraPrinters.ExtraPrinter>().ToList<ExtraPrinters.ExtraPrinter>();
          if (listPrinters.Count != 1)
          {
            MessageBoxHelper.Warning(Translate.ExtraPrintersViewModel_EditPrinterCommand_Необходимо_выбрать_один_принтер_для_изменения_);
          }
          else
          {
            ExtraPrinters.ExtraPrinter printer = listPrinters.Single<ExtraPrinters.ExtraPrinter>();
            if (!new ExtraPrinterCardViewModel().ShowCard(false, ref printer))
              return;
            this.Printers[this.Printers.ToList<ExtraPrinters.ExtraPrinter>().FindIndex((Predicate<ExtraPrinters.ExtraPrinter>) (x => x == listPrinters.Single<ExtraPrinters.ExtraPrinter>()))] = printer;
            this.OnPropertyChanged("Printers");
          }
        }));
      }
    }

    public ICommand DeletePrinterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<ExtraPrinters.ExtraPrinter> list = ((IEnumerable) obj).Cast<ExtraPrinters.ExtraPrinter>().ToList<ExtraPrinters.ExtraPrinter>();
          if (!list.Any<ExtraPrinters.ExtraPrinter>())
          {
            MessageBoxHelper.Warning(Translate.ExtraPrintersViewModel_DeletePrinterCommand_Необходимо_выбрать_хотя_бы_один_принтер_для_удаления_);
          }
          else
          {
            if (MessageBoxHelper.Show(string.Format(Translate.ExtraPrintersViewModel_DeletePrinterCommand_Вы_уверены__что_хотите_удалить__0__принтеров_, (object) list.Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            foreach (ExtraPrinters.ExtraPrinter extraPrinter in list)
              this.Printers.Remove(extraPrinter);
            this.OnPropertyChanged("Printers");
          }
        }));
      }
    }

    public ExtraPrintersViewModel()
    {
    }

    public ExtraPrintersViewModel(Gbs.Core.Config.Devices devices)
    {
      this.Devices = devices;
      this.Printers = new ObservableCollection<ExtraPrinters.ExtraPrinter>(devices.ExtraPrinters.Printers);
    }

    public void Save()
    {
      this.Devices.ExtraPrinters.Printers = new List<ExtraPrinters.ExtraPrinter>((IEnumerable<ExtraPrinters.ExtraPrinter>) this.Printers);
    }
  }
}
