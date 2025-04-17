// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.HelpPopUpViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public class HelpPopUpViewModel : ViewModel
  {
    public string Text { get; set; }

    public string Header { get; set; }

    public PlacementMode Placement { get; set; }

    public ICommand DisableTooltipsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (MessageBoxHelper.Question(Translate.HelpPopUpViewModel_DisableTooltipsCommand_Отключить_всплывающие_подсказки__При_необходимости_вы_сможете_их_включить_в_настройках_) != MessageBoxResult.Yes)
            return;
          ConfigsRepository<Settings> configsRepository = new ConfigsRepository<Settings>();
          Settings config = configsRepository.Get();
          config.Interface.IsShowHelpTooltip = false;
          configsRepository.Save(config);
        }));
      }
    }

    public string HotKey { get; set; }

    public Visibility HotkeyVisibility
    {
      get
      {
        string hotKey = this.HotKey;
        return (hotKey != null ? (hotKey.Length > 0 ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }
  }
}
