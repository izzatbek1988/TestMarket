// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FirstSetupPage.SelectModeViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using WebSocketSharp;

#nullable disable
namespace Gbs.Forms.Main.FirstSetupPage
{
  public partial class SelectModeViewModel : ViewModelWithForm
  {
    public string PathBackUp { get; set; }

    public ICommand SelectBackUpPathCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = Translate.DataBasePageViewModel_Архив_с_резервной_копией_ + "(*.zip)|*.zip;",
            Multiselect = false
          };
          bool? nullable = openFileDialog.ShowDialog();
          bool flag = false;
          if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
            return;
          this.PathBackUp = openFileDialog.FileName;
          this.OnPropertyChanged("PathBackUp");
        }));
      }
    }

    public string TextButton
    {
      get
      {
        return this.VisibilityMode != Visibility.Visible ? Translate.SelectModeViewModel_TextButton_Выбор_режима_работы_программы : Translate.SelectModeViewModel_TextButton_Восстановление_из_резервной_копии;
      }
    }

    public ICommand VisibilityCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.VisibilityMode = this.VisibilityMode == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
          this.VisibilityLoadingBackUp = this.VisibilityLoadingBackUp == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
          if (this.VisibilityMode == Visibility.Visible)
          {
            this.IsSaleMode = true;
            this.IsLoadingBackUp = false;
          }
          else
          {
            this.IsSaleMode = false;
            this.IsLoadingBackUp = true;
          }
          this.OnPropertyChanged("VisibilityMode");
          this.OnPropertyChanged("VisibilityLoadingBackUp");
          this.OnPropertyChanged("IsSaleMode");
          this.OnPropertyChanged("IsLoadingBackUp");
          this.OnPropertyChanged("TextButton");
        }));
      }
    }

    public Visibility VisibilityMode { get; set; }

    public Visibility VisibilityLoadingBackUp { get; set; } = Visibility.Collapsed;

    public bool IsLoadingBackUp { get; set; } = true;

    public bool IsSaleMode { get; set; } = true;

    public bool IsCafeMode { get; set; }

    public bool IsHomeMode { get; set; }

    public (bool, GlobalDictionaries.Mode) GetModeProgram()
    {
      if (this.VisibilityLoadingBackUp == Visibility.Visible)
      {
        if (this.IsLoadingBackUp)
        {
          if (Ext.IsNullOrEmpty(this.PathBackUp))
          {
            int num = (int) MessageBoxHelper.Show(Translate.SelectModeViewModel_GetModeProgram_Для_восстановления_данных_из_архива_нужно_указать_путь_к_резервной_копии_, icon: MessageBoxImage.Exclamation);
            return (false, GlobalDictionaries.Mode.Shop);
          }
          DataBaseHelper.RestoreAllBackUp(this.PathBackUp);
          return (true, GlobalDictionaries.Mode.None);
        }
      }
      else
      {
        if (this.IsSaleMode)
          return (true, GlobalDictionaries.Mode.Shop);
        if (this.IsCafeMode)
          return (true, GlobalDictionaries.Mode.Cafe);
        if (this.IsHomeMode)
          return (true, GlobalDictionaries.Mode.Home);
      }
      return (false, GlobalDictionaries.Mode.Shop);
    }
  }
}
