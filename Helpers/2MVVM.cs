// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.MVVM.ViewModelWithForm
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers.MVVM
{
  public abstract class ViewModelWithForm : ViewModel
  {
    private bool _isEnabledForm = true;

    [JsonIgnore]
    public Visibility VisibilityPanelSearch { get; set; } = Visibility.Collapsed;

    [JsonIgnore]
    public ICommand SetVisibilityPanelSearch
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.VisibilityPanelSearch = this.VisibilityPanelSearch == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
          this.OnPropertyChanged("VisibilityPanelSearch");
        }));
      }
    }

    [JsonIgnore]
    public bool IsEnabledForm
    {
      get => this._isEnabledForm;
      set
      {
        this._isEnabledForm = value;
        this.OnPropertyChanged(nameof (IsEnabledForm));
      }
    }

    [JsonIgnore]
    public Action CloseAction { get; set; } = (Action) (() => { });

    [JsonIgnore]
    public ICommand ExportFile
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => Other.ExportInFileDataGrid((DataGrid) obj)));
      }
    }

    [JsonIgnore]
    public ICommand NotImplementedCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          throw new NotImplementedException();
        }));
      }
    }

    [JsonIgnore]
    public WindowWithSize FormToSHow { get; set; }

    protected void ShowForm(bool isModal = true)
    {
      if (this.FormToSHow == null)
      {
        int num = (int) MessageBoxHelper.Show(Translate.ViewModelWithForm_Форма_для_отображения_данных_не_задана);
      }
      else
      {
        LogHelper.Debug("Отображение формы. Title: " + this.FormToSHow.Title + ", name: " + this.FormToSHow.Name);
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        if (DevelopersHelper.IsUnitTest())
          this.CloseAction = (Action) (() => { });
        this.FormToSHow.DataContext = (object) this;
        if (isModal)
          this.FormToSHow.ShowDialog();
        else
          this.FormToSHow.Show();
      }
    }
  }
}
