// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.HotKeysViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public class HotKeysViewModel : ViewModel
  {
    public bool IsEnabledVirtualKeyboard
    {
      get => this.Settings.Keyboard.VirtualKeyboard.IsEnabled;
      set
      {
        this.Settings.Keyboard.VirtualKeyboard.IsEnabled = value;
        this.OnPropertyChanged("VisibilityVirtualKeyboardSetting");
      }
    }

    public Visibility VisibilityVirtualKeyboardSetting
    {
      get
      {
        return !this.Settings.Keyboard.VirtualKeyboard.IsEnabled ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityHotKeysForRu
    {
      get
      {
        return new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Gbs.Core.Config.Devices Settings { get; set; }

    public ICommand SeDefaultCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (MessageBoxHelper.Show(Translate.HotKeysViewModel_SeDefaultCommand_Сбросить_сочетания_горячих_клавиш_по_умолчанию_, buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
          this.Settings.Keyboard.HotKeys = new HotKeys();
          this.OnPropertyChanged("Settings");
        }));
      }
    }

    public HotKeysViewModel() => this.Settings = new Gbs.Core.Config.Devices();

    public HotKeysViewModel(Gbs.Core.Config.Devices settings)
    {
      this.Settings = settings;
      Other.ConsoleWrite(settings.Keyboard.HotKeys.ToJsonString(true));
    }
  }
}
