// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FirstSetupPage.TitlePageViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Pages;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Main.FirstSetupPage
{
  public class TitlePageViewModel : ViewModelWithForm
  {
    public Visibility LanguageSelectorVisibility => PartnersHelper.GetLanguageSelectorVisibility();

    public Visibility VisibilityRegionSetting => PartnersHelper.GetVisibilityRegionSetting();

    public Interface InterfaceConfig { get; set; } = new ConfigsRepository<Gbs.Core.Config.Settings>().Get().Interface;

    public Dictionary<GlobalDictionaries.Countries, string> Countries { get; set; } = GlobalDictionaries.CountriesDictionary();

    public GlobalDictionaries.Countries SelectedCountry
    {
      get => this.InterfaceConfig.Country;
      set
      {
        this.InterfaceConfig.Country = value;
        this.SaveInterfaceConfig();
      }
    }

    public List<GlobalDictionaries.Language> Languages { get; set; } = GlobalDictionaries.LanguagesList().Where<GlobalDictionaries.Language>((Func<GlobalDictionaries.Language, bool>) (x => PartnersHelper.GetAllowedLanguagesList().Contains(x.Value))).ToList<GlobalDictionaries.Language>();

    public GlobalDictionaries.Languages SelectedLanguage
    {
      get
      {
        if (!this.Languages.Any<GlobalDictionaries.Language>() || this.Languages.Any<GlobalDictionaries.Language>((Func<GlobalDictionaries.Language, bool>) (x => x.Value == this.InterfaceConfig.Language)))
          return this.InterfaceConfig.Language;
        this.InterfaceConfig.Language = this.Languages.First<GlobalDictionaries.Language>().Value;
        return this.InterfaceConfig.Language;
      }
      set
      {
        this.InterfaceConfig.Language = value;
        this.SaveInterfaceConfig();
        if (MessageBoxHelper.Show(Translate.ПерезапуститьПрограммуДляСменыЯзыкаИнтерфейса, buttons: MessageBoxButton.YesNo) != MessageBoxResult.Yes)
          return;
        FrmFirstMain frmFirstMain = Application.Current.Windows.OfType<FrmFirstMain>().SingleOrDefault<FrmFirstMain>();
        if (frmFirstMain != null)
          ((FirstSetupViewModel) frmFirstMain.DataContext).ConfirmClose = false;
        FrmSplashScreen.SetLocalization();
        Other.RestartApplication();
        this.OnPropertyChanged(isUpdateAllProp: true);
        this.OnPropertyChanged(nameof (SelectedLanguage));
      }
    }

    private void SaveInterfaceConfig()
    {
      ConfigsRepository<Gbs.Core.Config.Settings> configsRepository = new ConfigsRepository<Gbs.Core.Config.Settings>();
      Gbs.Core.Config.Settings config = configsRepository.Get();
      config.Interface = this.InterfaceConfig;
      configsRepository.Save(config);
    }

    public Dictionary<GlobalDictionaries.Skin, string> Skins
    {
      get => GlobalDictionaries.GetSkinDictionary();
    }

    public GlobalDictionaries.Skin Skin
    {
      get => this.InterfaceConfig.Theme;
      set
      {
        this.InterfaceConfig.Theme = value;
        ((App) Application.Current).ChangeSkin(value);
        this.OnPropertyChanged("AvailableColors");
        this.SaveInterfaceConfig();
      }
    }

    public string BackgroundColor
    {
      get => this.InterfaceConfig.BackgroundColor;
      set
      {
        this.InterfaceConfig.BackgroundColor = value;
        ((App) Application.Current).UpdateColors();
        this.SaveInterfaceConfig();
      }
    }

    public ObservableCollection<ColorItem> AvailableColors
    {
      get
      {
        List<ColorItem> list = new List<ColorItem>();
        list.Add(new ColorItem(new Color?(Color.FromArgb((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue)), Translate.TitlePageViewModel_По_умолчанию));
        List<PageVisualModelView.ColorsRgb> source = new List<PageVisualModelView.ColorsRgb>();
        source.AddRange(this.Skin == GlobalDictionaries.Skin.Dark ? (IEnumerable<PageVisualModelView.ColorsRgb>) PageVisualModelView.DarkColors : (IEnumerable<PageVisualModelView.ColorsRgb>) PageVisualModelView.LightColors);
        list.AddRange(source.Select<PageVisualModelView.ColorsRgb, ColorItem>((Func<PageVisualModelView.ColorsRgb, ColorItem>) (rgb => new ColorItem(new Color?(Color.FromRgb(rgb.R, rgb.G, rgb.B)), rgb.Name))));
        return new ObservableCollection<ColorItem>(list);
      }
    }
  }
}
