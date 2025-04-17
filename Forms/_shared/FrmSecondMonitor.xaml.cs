// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.SecondMonitorViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Settings.Facade;
using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class SecondMonitorViewModel : ViewModelWithForm
  {
    private readonly Bonuses _bonusesSetting = new Bonuses();
    private System.Timers.Timer _imageTimer = new System.Timers.Timer();
    private Visibility _visibilityCloseButton = Visibility.Collapsed;

    public Visibility VisibilityClientInfo
    {
      get
      {
        return !this._bonusesSetting.IsActiveBonuses || this.Basket?.Client == null || this.VisibilityBasket == Visibility.Collapsed ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public LinkedListNode<BitmapSource> ImageCurrent { get; set; }

    private LinkedList<BitmapSource> Images { get; set; } = new LinkedList<BitmapSource>();

    private void InitializeTimer()
    {
      if (this._imageTimer != null)
      {
        this._imageTimer.Stop();
        this._imageTimer.Dispose();
      }
      SecondMonitor secondMonitor = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SecondMonitor;
      if (!secondMonitor.IsActivePhoto)
      {
        this.Images = new LinkedList<BitmapSource>();
        this.SetVisibilityImage();
      }
      else
      {
        if (Directory.Exists(secondMonitor.PathImages))
        {
          this.ImageLoad(secondMonitor.PathImages);
          this._imageTimer = new System.Timers.Timer()
          {
            Interval = (double) (secondMonitor.Interval * 1000)
          };
          this._imageTimer.Elapsed += new ElapsedEventHandler(this.TimerElapsed);
          this._imageTimer.Start();
        }
        this.SetVisibilityImage();
      }
    }

    private void ImageLoad(string path)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.ЗагрузкаИзображенийДляВторогоМонитора);
      this.ImageCurrent = (LinkedListNode<BitmapSource>) null;
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      if (!directoryInfo.Exists)
        return;
      List<string> ext = new List<string>()
      {
        ".png",
        ".bmp",
        ".jpg",
        ".jpeg"
      };
      List<BitmapSource> collection = new List<BitmapSource>();
      foreach (FileInfo fileInfo in ((IEnumerable<FileInfo>) directoryInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)).Where<FileInfo>((Func<FileInfo, bool>) (x => ext.Any<string>((Func<string, bool>) (e => x.Name.ToLower().EndsWith(e))))))
      {
        try
        {
          BitmapImage image = ImagesHelpers.ConvertToImage(fileInfo.FullName);
          collection.Add((BitmapSource) image);
        }
        catch (Exception ex)
        {
          Other.ConsoleWrite(ex.ToString());
        }
      }
      this.Images = new LinkedList<BitmapSource>((IEnumerable<BitmapSource>) collection);
      if (this.Images.Any<BitmapSource>())
      {
        this.ImageCurrent = new LinkedListNode<BitmapSource>(this.Images.First<BitmapSource>());
        this.OnPropertyChanged("ImageCurrent");
      }
      progressBar.Close();
    }

    public ICommand CloseMeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          Action closeAction = this.CloseAction;
          if (closeAction == null)
            return;
          closeAction();
        }));
      }
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
      this._imageTimer.Stop();
      if (this.Images.Any<BitmapSource>())
      {
        this.ImageCurrent = this.ImageCurrent?.Next ?? this.Images?.First;
        this.OnPropertyChanged("ImageCurrent");
      }
      this._imageTimer.Start();
    }

    public Gbs.Core.ViewModels.Basket.Basket Basket { get; set; } = new Gbs.Core.ViewModels.Basket.Basket();

    public void ShowSecondForm()
    {
      try
      {
        SecondMonitor config = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().SecondMonitor;
        this._bonusesSetting.Load();
        Screen screen = ((IEnumerable<Screen>) Screen.AllScreens).FirstOrDefault<Screen>((Func<Screen, bool>) (x => x.DeviceName == config.MonitorName));
        if (screen == null)
          return;
        this.FormToSHow = (WindowWithSize) new FrmSecondMonitor();
        this.InitializeTimer();
        this.ShowForm(false);
        this.FontSize = 30.0;
        this.OnPropertyChanged("FontSize");
        this.FormToSHow.WindowStyle = WindowStyle.None;
        this.FormToSHow.WindowStartupLocation = WindowStartupLocation.Manual;
        this.FormToSHow.WindowState = WindowState.Normal;
        this.FormToSHow.Left = (double) screen.Bounds.Left;
        this.FormToSHow.Top = (double) screen.Bounds.Top;
        this.FormToSHow.WindowState = WindowState.Maximized;
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    public void UpdateBasket(Gbs.Core.ViewModels.Basket.Basket basket)
    {
      this.Basket = basket;
      this.OnPropertyChanged("Basket");
      this.OnPropertyChanged("VisibilityClientInfo");
      this.SetVisibilityImage();
    }

    public double FontSize { get; set; } = 30.0;

    public Visibility VisibilityImage { get; set; }

    public Visibility VisibilityBasket { get; set; } = Visibility.Collapsed;

    public Visibility VisibilityCloseButton
    {
      get => this._visibilityCloseButton;
      set
      {
        this._visibilityCloseButton = value;
        this.OnPropertyChanged(nameof (VisibilityCloseButton));
      }
    }

    public void SetVisibilityImage()
    {
      try
      {
        if (!this.Basket.Items.Any<BasketItem>() && this.Images.Any<BitmapSource>())
        {
          this.VisibilityBasket = Visibility.Collapsed;
          this.VisibilityImage = Visibility.Visible;
        }
        else
        {
          this.VisibilityBasket = Visibility.Visible;
          this.VisibilityImage = Visibility.Collapsed;
        }
        this.OnPropertyChanged("Basket");
        this.OnPropertyChanged("VisibilityImage");
        this.OnPropertyChanged("VisibilityBasket");
        this.OnPropertyChanged("VisibilityClientInfo");
        if (this.FormToSHow == null)
          return;
        System.Windows.Application.Current.Dispatcher.Invoke((Action) (() => ((FrmSecondMonitor) this.FormToSHow).ItemsDataGrid.Columns.First<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "0570736C-05AD-4A2C-9ACC-06DDE8491CC4")).Visibility = this.Basket.Items.Any<BasketItem>((Func<BasketItem, bool>) (x => x.DiscountSum > 0M)) ? Visibility.Visible : Visibility.Collapsed));
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }
  }
}
