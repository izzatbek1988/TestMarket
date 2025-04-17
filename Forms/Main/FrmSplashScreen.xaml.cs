// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FrmSplashScreen
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Forms.Main
{
  public partial class FrmSplashScreen : Window, IComponentConnector
  {
    public static bool IsUnpackDataZip;
    internal Grid MainGrid;
    internal System.Windows.Controls.Label AppVersionLabel;
    internal System.Windows.Controls.Label ProgramNameLabel;
    internal TextBlock LoadingProcessLabel;
    private bool _contentLoaded;

    public FrmSplashScreen()
    {
      try
      {
        FrmSplashScreen.CheckNetVersion();
        try
        {
          FrmSplashScreen.CheckWMIServiceRunning();
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Ошибка проверки или запуска службы WMI", false);
        }
        this.InitializeComponent();
        try
        {
          VendorConfig config = Vendor.GetConfig();
          ImageBrush imageBrush = (ImageBrush) null;
          if (config != null)
          {
            string str = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "logo.png");
            if (File.Exists(str))
            {
              string shA256Hash = CryptoHelper.GetSHA256Hash(str, false);
              if (config.Logo != null && shA256Hash == config.Logo.LogoHash)
              {
                imageBrush = new ImageBrush((ImageSource) ImagesHelpers.ConvertToImage(str));
                this.ProgramNameLabel.Visibility = Visibility.Collapsed;
                SolidColorBrush solidColorBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(config.Logo.ForegroundColor));
                this.AppVersionLabel.Foreground = (Brush) solidColorBrush;
                this.LoadingProcessLabel.Foreground = (Brush) solidColorBrush;
              }
            }
          }
          if (imageBrush != null)
            this.MainGrid.Background = (Brush) imageBrush;
        }
        catch (Exception ex)
        {
        }
        try
        {
          ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof (FrameworkElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex, sendToZidium: false);
        }
        FileSystemHelper.CheckActiveThisPath();
        LogHelper.Debug("Инициализация");
        this.UnpackDataZip();
        FrmSplashScreen.SetVendorConfig();
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof (FrameworkElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomainOnUnhandledException);
        System.Windows.Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.CurrentOnDispatcherUnhandledException);
        System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(this.Application_ThreadException);
        TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(this.TaskScheduler_UnobservedTaskException);
        FrmSplashScreen.SetLocalization();
        SplashScreenViewModel splashScreenViewModel = new SplashScreenViewModel();
        splashScreenViewModel.Run(new Action(((Window) this).Close));
        this.DataContext = (object) splashScreenViewModel;
        this.ShowDialog();
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.MessageBox.Show(Translate.FrmSplashScreen_Ошибка_запуска_программы__ + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        System.Environment.Exit(0);
      }
    }

    private static void SetVendorConfig()
    {
      VendorConfig config = Vendor.GetConfig();
      if (config == null)
        return;
      ConfigsRepository<Settings> configsRepository = new ConfigsRepository<Settings>();
      Settings c = configsRepository.Get();
      c.Interface.Country = config.Country;
      GlobalDictionaries.Languages? language = config.Language;
      if (language.HasValue)
      {
        Interface @interface = c.Interface;
        language = config.Language;
        int num = (int) language.Value;
        @interface.Language = (GlobalDictionaries.Languages) num;
      }
      if (config.Languages != null && config.Languages.Any<GlobalDictionaries.Languages>() && !config.Languages.Any<GlobalDictionaries.Languages>((Func<GlobalDictionaries.Languages, bool>) (x => x == c.Interface.Language)))
        c.Interface.Language = config.Languages.First<GlobalDictionaries.Languages>();
      configsRepository.Save(c);
    }

    private static void CheckWMIServiceRunning()
    {
      LogHelper.Debug("Проверка статуса службы Winmgmt");
      ServiceController service1 = FileSystemHelper.GetService("Winmgmt");
      LogHelper.Debug("WMI  service status: " + service1.Status.ToString());
      if (service1.Status != ServiceControllerStatus.Running)
        FileSystemHelper.StartService("Winmgmt");
      ServiceController service2 = FileSystemHelper.GetService("Winmgmt");
      LogHelper.Debug("WMI service status: " + service2.Status.ToString());
      if (service2.Status != ServiceControllerStatus.Running)
        throw new InvalidOperationException(string.Format(Translate.DataBaseHelper_Служба__0__не_запущена, (object) "Winmgmt"));
    }

    private static void CheckNetVersion()
    {
      try
      {
        if (FileSystemHelper.IsFramework47() || MessageBoxHelper.Show(Translate.FrmSplashScreen_FrmSplashScreen_, buttons: MessageBoxButton.YesNo) != MessageBoxResult.No)
          return;
        System.Environment.Exit(0);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при получении версии .net");
      }
    }

    private void UnpackDataZip()
    {
      try
      {
        FrmSplashScreen.IsUnpackDataZip = false;
        ApplicationInfo.PathsInfo paths = ApplicationInfo.GetInstance().Paths;
        string str = Path.Combine(paths.ApplicationPath, "data.zip");
        if (!File.Exists(str))
          return;
        if (!Directory.Exists(paths.DataPath))
          Directory.CreateDirectory(paths.DataPath);
        string[] directories = Directory.GetDirectories(paths.DataPath);
        if (directories.Length > 1)
          return;
        if (directories.Length == 1)
        {
          if (directories[0].Trim('\\').ToLower() != paths.LogsPath.Trim('\\').ToLower())
            return;
        }
        if (Directory.GetFiles(paths.DataPath).Length != 0)
          return;
        Thread.Sleep(1000);
        LogHelper.Debug("Распаковка data.zip");
        Directory.CreateDirectory(paths.DataPath);
        FileSystemHelper.ExtractAllFile(str, paths.DataPath);
        new ConfigsRepository<Settings>().ReloadCache();
        new ConfigsRepository<DataBase>().ReloadCache();
        new ConfigsRepository<Gbs.Core.Config.Devices>().ReloadCache();
        new ConfigsRepository<Integrations>().ReloadCache();
        FrmSplashScreen.IsUnpackDataZip = true;
      }
      catch (Exception ex)
      {
        FrmSplashScreen.IsUnpackDataZip = false;
        LogHelper.WriteError(ex, "Не удалось распаковать Data.zip");
      }
    }

    public static void SetLocalization()
    {
      try
      {
        GlobalDictionaries.Languages currentLanguage = new ConfigsRepository<Settings>().Get().Interface.Language;
        if (currentLanguage.IsEither<GlobalDictionaries.Languages>(GlobalDictionaries.Languages.System, GlobalDictionaries.Languages.Russian))
          return;
        GlobalDictionaries.Language language = GlobalDictionaries.LanguagesList().SingleOrDefault<GlobalDictionaries.Language>((Func<GlobalDictionaries.Language, bool>) (x => x.Value == currentLanguage));
        if (language == null)
          return;
        LogHelper.Debug("Активация локализацииЖ " + language.ToJsonString());
        CultureInfo cultureInfo = new CultureInfo(language.Key);
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Не удалось установить локализацию приложения");
      }
    }

    private void TaskScheduler_UnobservedTaskException(
      object sender,
      UnobservedTaskExceptionEventArgs e)
    {
      this.UnhandledEx((Exception) e.Exception, "TaskUnobservedException");
    }

    private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      this.UnhandledEx(e.Exception, "ThreadException");
    }

    private void CurrentOnDispatcherUnhandledException(
      object sender,
      DispatcherUnhandledExceptionEventArgs e)
    {
      this.UnhandledEx(e.Exception, "DispatcherUnhandledException");
    }

    private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      this.UnhandledEx(e.ExceptionObject as Exception, "DomainUnhandledException");
    }

    private void UnhandledEx(Exception ex, string description, bool isShow = true)
    {
      LogHelper.Error(ex, description, isShow);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/Market;component/forms/main/frmsplashscreen.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.MainGrid = (Grid) target;
          break;
        case 2:
          this.AppVersionLabel = (System.Windows.Controls.Label) target;
          break;
        case 3:
          this.ProgramNameLabel = (System.Windows.Controls.Label) target;
          break;
        case 4:
          this.LoadingProcessLabel = (TextBlock) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
