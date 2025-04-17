// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Devices.ExtraPrinterCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms.Excel;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Devices
{
  public partial class ExtraPrinterCardViewModel : ViewModelWithForm
  {
    private ObservableCollection<GoodGroups.Group> _groupsListFilter = new ObservableCollection<GoodGroups.Group>();
    private IEnumerable _printersList;
    private bool _isSave;

    public ExtraPrinters.ExtraPrinter Printer { get; set; }

    public ObservableCollection<GoodGroups.Group> GroupsListFilter
    {
      get => this._groupsListFilter;
      set
      {
        this._groupsListFilter = value;
        this.OnPropertyChanged(nameof (GroupsListFilter));
      }
    }

    public IEnumerable PrintersList
    {
      get => this._printersList != null ? this._printersList : (IEnumerable) new List<string>();
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.ValidPrinter())
            return;
          this._isSave = true;
          this.CloseAction();
        }));
      }
    }

    private bool ValidPrinter()
    {
      List<string> source = new List<string>();
      if (this.Printer.Name.IsNullOrEmpty())
        source.Add(Translate.ExtraPrinterCardViewModel_ValidPrinter_Необходимо_указать_название_устройства_);
      if (this.Printer.CheckTemplate.IsNullOrEmpty())
        source.Add(Translate.ExtraPrinterCardViewModel_ValidPrinter_Необходимо_указать_шаблон_для_печати_);
      if (this.Printer.PrinterName.IsNullOrEmpty())
        source.Add(Translate.ExtraPrinterCardViewModel_ValidPrinter_Необходимо_указать_принтер_для_печати_);
      if (!this.GroupsListFilter.Any<GoodGroups.Group>())
        source.Add(Translate.ExtraPrinterCardViewModel_ValidPrinter_Необходимо_выбрать_хотя_бы_одну_категорию_для_данного_принтера_);
      if (source.Count <= 0)
        return true;
      MessageBoxHelper.Warning(source.Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, m) => current + m + "\r\n")));
      return false;
    }

    public ICommand CancelCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isSave = false;
          this.CloseAction();
        }));
      }
    }

    public List<WaybillInExcelViewModel.FileInfoView> ListTemplate { get; set; } = new List<WaybillInExcelViewModel.FileInfoView>();

    public bool ShowCard(bool isNewCard, ref ExtraPrinters.ExtraPrinter printer)
    {
      this.Printer = !isNewCard ? printer.Clone<ExtraPrinters.ExtraPrinter>() : new ExtraPrinters.ExtraPrinter();
      this.EntityClone = this.Printer.Clone<ExtraPrinters.ExtraPrinter>();
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        GoodGroupsRepository groupsRepository = new GoodGroupsRepository(dataBase);
        this.GroupsListFilter = isNewCard ? new ObservableCollection<GoodGroups.Group>() : new ObservableCollection<GoodGroups.Group>(groupsRepository.GetActiveItems().Where<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => this.Printer.GoodGroups.Any<Guid>((Func<Guid, bool>) (g => g == x.Uid)))));
        try
        {
          this._printersList = (IEnumerable) PrinterSettings.InstalledPrinters;
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка получения списка принтеров");
          this._printersList = (IEnumerable) new List<string>();
        }
        this.ListTemplate = new List<WaybillInExcelViewModel.FileInfoView>(((IEnumerable<FileInfo>) ReportType.ExtraPrinter.Directory.GetFiles("*.frx")).Select<FileInfo, WaybillInExcelViewModel.FileInfoView>((Func<FileInfo, WaybillInExcelViewModel.FileInfoView>) (x => new WaybillInExcelViewModel.FileInfoView(x.FullName))));
        this.OnPropertyChanged("ListTemplate");
        this.FormToSHow = (WindowWithSize) new FrmExtraPrinterCard();
        ((FrmExtraPrinterCard) this.FormToSHow).CategorySelectionControl.UpdateTextButton(this.GroupsListFilter);
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        this.ShowForm();
        if (this._isSave)
        {
          printer = this.Printer;
          printer.GoodGroups = new List<Guid>(this.GroupsListFilter.Select<GoodGroups.Group, Guid>((Func<GoodGroups.Group, Guid>) (x => x.Uid)));
        }
        return this._isSave;
      }
    }

    public ExtraPrinters.ExtraPrinter EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      return Functions.IsObjectEqual<ExtraPrinters.ExtraPrinter>(this.EntityClone, this.Printer) || this._isSave;
    }
  }
}
