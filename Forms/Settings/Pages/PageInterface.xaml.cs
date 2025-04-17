// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageVisualModelView
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Forms.Excel;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class PageVisualModelView : ViewModelWithForm
  {
    private ReportType _selectedTemplateType;
    private Gbs.Core.Config.Settings _settings = new Gbs.Core.Config.Settings();

    public Visibility VisibilityRegionSetting
    {
      get
      {
        return this.CountrySelectorVisibility == Visibility.Visible || this.LanguageSelectorVisibility == Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    public Visibility CountrySelectorVisibility => PartnersHelper.GetVisibilityRegionSetting();

    public Visibility LanguageSelectorVisibility => PartnersHelper.GetLanguageSelectorVisibility();

    public Visibility VisibilityDebug
    {
      get => !DevelopersHelper.IsDebug() ? Visibility.Collapsed : Visibility.Visible;
    }

    public Dictionary<ViewSaleJournal, string> DictionaryViewSale
    {
      get
      {
        return new Dictionary<ViewSaleJournal, string>()
        {
          {
            ViewSaleJournal.ListGood,
            Translate.FrmCardSale_СписокТоваров
          },
          {
            ViewSaleJournal.ListSale,
            Translate.СписокЧеков
          }
        };
      }
    }

    public GlobalDictionaries.Skin SelectedTheme
    {
      get => this.Settings.Interface.Theme;
      set
      {
        this.Settings.Interface.Theme = value;
        this.OnPropertyChanged(nameof (SelectedTheme));
        this.OnPropertyChanged("AvailableBackgroundColors");
      }
    }

    public string TemplatesFrPath
    {
      get => this.Settings.Interface.TemplatesFrPath;
      set
      {
        this.Settings.Interface.TemplatesFrPath = value;
        this.ListTemplateType = ReportType.GetAll();
        this.OnPropertyChanged("ListTemplateType");
        this.OnPropertyChanged(nameof (TemplatesFrPath));
      }
    }

    public Dictionary<GlobalDictionaries.Skin, string> Skins
    {
      get => GlobalDictionaries.GetSkinDictionary();
    }

    public ICommand EditTemplate { get; set; }

    public ICommand ShowDirectoryTemplate { get; set; }

    public List<ReportType> ListTemplateType { get; set; } = ReportType.GetAll();

    public List<WaybillInExcelViewModel.FileInfoView> ListTemplate { get; set; } = new List<WaybillInExcelViewModel.FileInfoView>();

    public string SelectedTemplate { get; set; }

    public Dictionary<GlobalDictionaries.Countries, string> Countries { get; set; } = GlobalDictionaries.CountriesDictionary();

    public List<GlobalDictionaries.Language> Languages { get; set; } = GlobalDictionaries.LanguagesList().Where<GlobalDictionaries.Language>((Func<GlobalDictionaries.Language, bool>) (x => PartnersHelper.GetAllowedLanguagesList().Contains(x.Value))).ToList<GlobalDictionaries.Language>();

    public string BackgroundColor
    {
      get => this._settings.Interface.BackgroundColor;
      set => this._settings.Interface.BackgroundColor = value;
    }

    public string SelectionColor
    {
      get => this._settings.Interface.SelectionColor;
      set => this._settings.Interface.SelectionColor = value;
    }

    public GlobalDictionaries.Languages SelectedLanguage
    {
      get
      {
        return !this.Languages.Any<GlobalDictionaries.Language>() || this.Languages.Any<GlobalDictionaries.Language>((Func<GlobalDictionaries.Language, bool>) (x => x.Value == this.Settings.Interface.Language)) ? this.Settings.Interface.Language : this.Languages.First<GlobalDictionaries.Language>().Value;
      }
      set
      {
        this.Settings.Interface.Language = value;
        ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification()
        {
          Title = PartnersHelper.ProgramName(),
          Text = Translate.PageVisualModelView_Для_смены_языка_интерфейса_необходимо_перезапустить_программу
        });
        this.OnPropertyChanged(nameof (SelectedLanguage));
      }
    }

    public ReportType SelectedTemplateType
    {
      get => this._selectedTemplateType;
      set
      {
        this._selectedTemplateType = value;
        if (!value.Directory.Exists)
          return;
        this.ListTemplate = new List<WaybillInExcelViewModel.FileInfoView>(((IEnumerable<FileInfo>) value.Directory.GetFiles("*.frx")).Select<FileInfo, WaybillInExcelViewModel.FileInfoView>((Func<FileInfo, WaybillInExcelViewModel.FileInfoView>) (x => new WaybillInExcelViewModel.FileInfoView(x.FullName))));
        this.OnPropertyChanged("ListTemplate");
      }
    }

    public Gbs.Core.Config.Settings Settings
    {
      get => this._settings;
      set
      {
        this._settings = value;
        this.OnPropertyChanged(nameof (Settings));
      }
    }

    public bool IsOpenDesign
    {
      get => this.Settings.Interface.IsVisibilityDesign;
      set
      {
        if (value && MessageBoxHelper.Show(Translate.PageVisualModelView_, buttons: MessageBoxButton.YesNo) == MessageBoxResult.Yes)
          this.Settings.Interface.IsVisibilityDesign = true;
        else
          this.Settings.Interface.IsVisibilityDesign = false;
      }
    }

    public ICommand GetTemplatePath
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
          {
            ShowNewFolderButton = true,
            Description = Translate.PageVisualModelView_Выберите_папку__где_хранятся_шаблоны_печатных_форм_
          };
          if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            return;
          this.TemplatesFrPath = folderBrowserDialog.SelectedPath;
        }));
      }
    }

    public Visibility CheckboxTooltipVisibility
    {
      get => !ToolTipsHelper.IsToolTipsEnable() ? Visibility.Collapsed : Visibility.Visible;
    }

    public static PageVisualModelView.ColorsRgb[] DarkColors
    {
      get
      {
        PageVisualModelView.ColorsRgb[] lightColors = PageVisualModelView.LightColors;
        List<PageVisualModelView.ColorsRgb> colorsRgbList = new List<PageVisualModelView.ColorsRgb>();
        foreach (PageVisualModelView.ColorsRgb colorsRgb in lightColors)
        {
          System.Drawing.Color c = System.Drawing.Color.FromArgb((int) colorsRgb.R, (int) colorsRgb.G, (int) colorsRgb.B).InvertBrightness();
          colorsRgbList.Add(new PageVisualModelView.ColorsRgb(ColorTranslator.ToHtml(c), colorsRgb.Name));
        }
        return colorsRgbList.ToArray();
      }
    }

    public static PageVisualModelView.ColorsRgb[] LightColors
    {
      get
      {
        return new PageVisualModelView.ColorsRgb[19]
        {
          new PageVisualModelView.ColorsRgb("#FFCDD2", "Red"),
          new PageVisualModelView.ColorsRgb("#F8BBD0", "Pink"),
          new PageVisualModelView.ColorsRgb("#E1BEE7", "Purple"),
          new PageVisualModelView.ColorsRgb("#D1C4E9", "Deep Purple"),
          new PageVisualModelView.ColorsRgb("#C5CAE9", "Indigo"),
          new PageVisualModelView.ColorsRgb("#BBDEFB", "Blue"),
          new PageVisualModelView.ColorsRgb("#B3E5FC", "Light Blue"),
          new PageVisualModelView.ColorsRgb("#B2EBF2", "Cyan"),
          new PageVisualModelView.ColorsRgb("#B2DFDB", "Teal"),
          new PageVisualModelView.ColorsRgb("#C8E6C9", "Green"),
          new PageVisualModelView.ColorsRgb("#DCEDC8", "Light Green"),
          new PageVisualModelView.ColorsRgb("#F0F4C3", "Lime"),
          new PageVisualModelView.ColorsRgb("#FFF9C4", "Yellow"),
          new PageVisualModelView.ColorsRgb("#FFECB3", "Amber"),
          new PageVisualModelView.ColorsRgb("#FFE0B2", "Orange"),
          new PageVisualModelView.ColorsRgb("#FFCCBC", "Deep Orange"),
          new PageVisualModelView.ColorsRgb("#D7CCC8", "Brown"),
          new PageVisualModelView.ColorsRgb("#F5F5F5", "Grey"),
          new PageVisualModelView.ColorsRgb("#CFD8DC", "Blue Grey")
        };
      }
    }

    public ObservableCollection<ColorItem> AvailableBackgroundColors
    {
      get
      {
        List<ColorItem> list = new List<ColorItem>();
        list.Add(new ColorItem(new System.Windows.Media.Color?(System.Windows.Media.Color.FromArgb((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue)), Translate.PageVisualModelView_По_умолчанию));
        List<PageVisualModelView.ColorsRgb> source = new List<PageVisualModelView.ColorsRgb>();
        if (this.Settings.Interface.Theme == GlobalDictionaries.Skin.Dark)
          source.AddRange((IEnumerable<PageVisualModelView.ColorsRgb>) PageVisualModelView.DarkColors);
        else
          source.AddRange((IEnumerable<PageVisualModelView.ColorsRgb>) PageVisualModelView.LightColors);
        list.AddRange(source.Select<PageVisualModelView.ColorsRgb, ColorItem>((Func<PageVisualModelView.ColorsRgb, ColorItem>) (rgb => new ColorItem(new System.Windows.Media.Color?(System.Windows.Media.Color.FromRgb(rgb.R, rgb.G, rgb.B)), rgb.Name))));
        return new ObservableCollection<ColorItem>(list);
      }
    }

    public ObservableCollection<ColorItem> AvailableSelectionColors
    {
      get
      {
        List<ColorItem> list = new List<ColorItem>();
        list.Add(new ColorItem(new System.Windows.Media.Color?(System.Windows.Media.Color.FromArgb((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue)), Translate.PageVisualModelView_По_умолчанию));
        list.AddRange(new List<PageVisualModelView.ColorsRgb>()
        {
          new PageVisualModelView.ColorsRgb("#00BFFF", ""),
          new PageVisualModelView.ColorsRgb("#1E90FF", ""),
          new PageVisualModelView.ColorsRgb("#8A2BE2", ""),
          new PageVisualModelView.ColorsRgb("#A0522D", ""),
          new PageVisualModelView.ColorsRgb("#A52A2A", ""),
          new PageVisualModelView.ColorsRgb("#C71585", "")
        }.Select<PageVisualModelView.ColorsRgb, ColorItem>((Func<PageVisualModelView.ColorsRgb, ColorItem>) (rgb => new ColorItem(new System.Windows.Media.Color?(System.Windows.Media.Color.FromRgb(rgb.R, rgb.G, rgb.B)), rgb.Name))));
        return new ObservableCollection<ColorItem>(list);
      }
    }

    public ICommand TryColorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => ((App) System.Windows.Application.Current).ChangeSkin(this.SelectedTheme)));
      }
    }

    public PageVisualModelView()
    {
    }

    public PageVisualModelView(Gbs.Core.Config.Settings settings)
    {
      this.Settings = settings;
      this.SelectedTheme = this.Settings.Interface.Theme;
      this.EditTemplate = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.SelectedTemplate == null)
          MessageBoxHelper.Warning(Translate.PageVisualModelView_Выберите_шаблон);
        else
          new FastReportFacade().ShowDesigner(this.SelectedTemplate);
      }));
      this.ShowDirectoryTemplate = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.TemplatesFrPath.IsNullOrEmpty())
          MessageBoxHelper.Warning(Translate.PageVisualModelView_Невозможно_открыть_папку_с_шаблонами__так_как_не_указан_путь_до_нее_);
        else
          Process.Start("explorer.exe", this.TemplatesFrPath);
      }));
    }

    public class ColorsRgb
    {
      public byte R { get; set; }

      public byte G { get; set; }

      public byte B { get; set; }

      public string Name { get; set; }

      public ColorsRgb(string hex, string name)
      {
        System.Drawing.Color color = ColorTranslator.FromHtml(hex);
        this.R = color.R;
        this.G = color.G;
        this.B = color.B;
        this.Name = name;
      }
    }
  }
}
