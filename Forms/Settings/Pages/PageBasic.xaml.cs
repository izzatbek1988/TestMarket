// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.BasicPageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Organization;
using Gbs.Forms.Settings.Sections;
using Gbs.Forms.Settings.Warehouse;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class BasicPageViewModel : ViewModelWithForm
  {
    private DataBase _dbConfigs;
    private Func<bool> CloseSetting;
    public bool IsCloseForHome;
    private Gbs.Core.Config.Settings _settings = new Gbs.Core.Config.Settings();

    public Visibility VisibilityBlockHome
    {
      get
      {
        DataBase dbConfigs = this._dbConfigs;
        return (dbConfigs != null ? (dbConfigs.ModeProgram == GlobalDictionaries.Mode.Home ? 1 : 0) : 0) == 0 ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Dictionary<GlobalDictionaries.Mode, string> ModesProgram
    {
      get => GlobalDictionaries.ModeProgramDictionary();
    }

    public GlobalDictionaries.Mode Mode
    {
      get
      {
        DataBase dbConfigs = this._dbConfigs;
        return dbConfigs == null ? GlobalDictionaries.Mode.Shop : dbConfigs.ModeProgram;
      }
      set
      {
        GlobalDictionaries.Mode origValue = this._dbConfigs.ModeProgram;
        if (this._dbConfigs.ModeProgram != GlobalDictionaries.Mode.Home && value == GlobalDictionaries.Mode.Home || this._dbConfigs.ModeProgram == GlobalDictionaries.Mode.Home && value != GlobalDictionaries.Mode.Home)
        {
          if (MessageBoxHelper.Show(Translate.DataBasePageViewModel_Для_изменения_режима_работы_необходимо_перезагрузить_программу_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.Yes)
          {
            this.IsCloseForHome = true;
            if (this.CloseSetting())
            {
              this._dbConfigs.ModeProgram = value;
              new ConfigsRepository<DataBase>().Save(this._dbConfigs);
              Other.RestartApplication();
              return;
            }
          }
          else
          {
            Application.Current?.Dispatcher?.BeginInvoke((Delegate) (() =>
            {
              this._dbConfigs.ModeProgram = origValue;
              this.OnPropertyChanged(nameof (Mode));
            }), DispatcherPriority.ContextIdle, (object[]) null);
            return;
          }
        }
        this._dbConfigs.ModeProgram = value;
      }
    }

    public bool AutoRunProgram
    {
      get => this._settings.BasicConfig.AutoRunProgram;
      set => this._settings.BasicConfig.AutoRunProgram = value;
    }

    public ICommand ShowPointInfo { get; set; }

    public ICommand ShowSectionsList { get; set; }

    public ICommand ShowStorageList { get; set; }

    public BasicPageViewModel()
    {
    }

    public BasicPageViewModel(Gbs.Core.Config.Settings settings, DataBase confDataBase, Func<bool> closeSetting)
    {
      this._settings = settings;
      this._dbConfigs = confDataBase;
      this.CloseSetting = closeSetting;
      this.ShowPointInfo = (ICommand) new RelayCommand((Action<object>) (obj => new FrmOrganizationInfo().ShowDialog()));
      this.ShowSectionsList = (ICommand) new RelayCommand((Action<object>) (obj => new FrmSectionsList().ShowDialog()));
      this.ShowStorageList = (ICommand) new RelayCommand((Action<object>) (obj => new FrmStorageList().ShowDialog()));
    }

    public void SetModeProgram(GlobalDictionaries.Mode mode)
    {
      this.Mode = mode;
      this.OnPropertyChanged("Mode");
    }
  }
}
