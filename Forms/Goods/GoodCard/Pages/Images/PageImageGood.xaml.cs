// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.ImageGoodViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard
{
  public partial class ImageGoodViewModel : ViewModelWithForm
  {
    private LinkedListNode<ImageGoodViewModel.ImageSource> _currentImage;
    private LinkedList<ImageGoodViewModel.ImageSource> _images;
    private const string allowFilesExtensions = "*.png; *.bmp; *.jpg; *.jpeg;";

    public List<string> ImageDelete { get; set; } = new List<string>();

    public List<string> ImageSave { get; set; } = new List<string>();

    public LinkedListNode<ImageGoodViewModel.ImageSource> CurrentImage
    {
      get => this._currentImage;
      set
      {
        this._currentImage = value;
        this.OnPropertyChanged(nameof (CurrentImage));
      }
    }

    private Guid GoodUid { get; set; }

    public ImageGoodViewModel()
    {
    }

    public Visibility VisibilityFunctionButton { get; set; }

    public bool IsReadOnlyImage { get; set; }

    public ImageGoodViewModel(Guid goodUid, bool isVisibilityFunctionButton = true)
    {
      this.GoodUid = goodUid;
      this.VisibilityFunctionButton = isVisibilityFunctionButton ? Visibility.Visible : Visibility.Collapsed;
      this.IsReadOnlyImage = !isVisibilityFunctionButton;
      try
      {
        this.ImageLoad(Path.Combine(new ConfigsRepository<DataBase>().Get().GoodsImagesPath, this.GoodUid.ToString()));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка загрузки изображения товара");
      }
      this.AddImageCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.AddImage()));
      this.NextImageCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.CurrentImage = this.CurrentImage?.Next ?? this._images?.First));
      this.LastImageCommand = (ICommand) new RelayCommand((Action<object>) (obj => this.CurrentImage = this.CurrentImage?.Previous ?? this._images?.Last));
      this.DeleteImageCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (this.CurrentImage != null)
        {
          this.ImageDelete.Add(this.CurrentImage.Value.Path);
          this.ImageSave.RemoveAll((Predicate<string>) (x => x == this.CurrentImage.Value.Path));
          this.ImageLoad(Path.Combine(new ConfigsRepository<DataBase>().Get().GoodsImagesPath, this.GoodUid.ToString()));
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.ImageGoodViewModel_Необходимо_выбрать_фото_для_товара_);
        }
      }));
    }

    private void AddImage()
    {
      if (this.IsReadOnlyImage)
        return;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = Translate.FrmGoodCard_Изображения + "(*.png; *.bmp; *.jpg; *.jpeg;) | *.png; *.bmp; *.jpg; *.jpeg;";
      openFileDialog1.Multiselect = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      openFileDialog2.ShowDialog();
      if (!((IEnumerable<string>) openFileDialog2.FileNames).Any<string>())
        return;
      this.CopyImagesToGoodFolder(((IEnumerable<string>) openFileDialog2.FileNames).ToList<string>());
    }

    public void CopyImagesToGoodFolder(List<string> images)
    {
      if (this.IsReadOnlyImage)
        return;
      string str1 = Path.Combine(new ConfigsRepository<DataBase>().Get().GoodsImagesPath, this.GoodUid.ToString());
      List<string> stringList = new List<string>();
      string str2 = "*.png; *.bmp; *.jpg; *.jpeg;".Trim();
      char[] chArray = new char[1]{ ';' };
      foreach (string str3 in str2.Split(chArray))
      {
        string str4 = str3.Trim().Replace("*", "");
        if (!string.IsNullOrEmpty(str4))
          stringList.Add(str4);
      }
      foreach (string image in images)
      {
        string lower = Path.GetExtension(image).ToLower();
        if (!stringList.Contains(lower))
        {
          MessageBoxHelper.Warning(string.Format(Translate.ImageGoodViewModel_CopyImagesToGoodFolder_, (object) lower, (object) "*.png; *.bmp; *.jpg; *.jpeg;"));
          return;
        }
      }
      if (!Directory.Exists(str1))
        Directory.CreateDirectory(str1);
      int num = 0;
      foreach (string image in images)
      {
        string lower = Path.GetExtension(image).ToLower();
        string destPath = Path.Combine(str1, DateTime.Now.ToString("ddMMyyyyHHmmss") + num.ToString() + lower);
        ImagesHelpers.CompressImage(image, destPath);
        this.ImageSave.Add(destPath);
        ++num;
      }
      this.ImageLoad(str1);
    }

    private void ImageLoad(string path)
    {
      this.CurrentImage = (LinkedListNode<ImageGoodViewModel.ImageSource>) null;
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      if (!directoryInfo.Exists)
        return;
      this._images = new LinkedList<ImageGoodViewModel.ImageSource>();
      foreach (string path1 in ((IEnumerable<string>) Directory.GetFiles(directoryInfo.FullName)).Where<string>((Func<string, bool>) (x => this.ImageDelete.All<string>((Func<string, bool>) (p => p != x)))).ToList<string>())
      {
        MemoryStream destination = new MemoryStream();
        using (FileStream fileStream = new FileStream(path1, FileMode.Open, FileAccess.Read))
        {
          fileStream.CopyTo((Stream) destination);
          destination.Seek(0L, SeekOrigin.Begin);
          BitmapImage bitmapImage = new BitmapImage();
          bitmapImage.BeginInit();
          bitmapImage.StreamSource = (Stream) destination;
          bitmapImage.EndInit();
          this._images.AddLast(new LinkedListNode<ImageGoodViewModel.ImageSource>(new ImageGoodViewModel.ImageSource()
          {
            Image = bitmapImage,
            Path = path1
          }));
        }
      }
      if (!this._images.Any<ImageGoodViewModel.ImageSource>())
        return;
      this.CurrentImage = this._images.First;
    }

    public ICommand AddImageCommand { get; set; }

    public ICommand DeleteImageCommand { get; set; }

    public ICommand LastImageCommand { get; set; }

    public ICommand NextImageCommand { get; set; }

    public class ImageSource
    {
      public BitmapImage Image { get; set; }

      public string Path { get; set; }
    }
  }
}
