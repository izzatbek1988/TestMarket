// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Extensions.String.StringExtensions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using FastReport;
using FastReport.Barcode;
using FastReport.Export.Image;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Helpers.Extensions.String
{
  public static class StringExtensions
  {
    public static string CorrectUrl(this string url, int port)
    {
      if (!url.ToLower().StartsWith("http://"))
        url = "http://" + url;
      return url + ":" + port.ToString();
    }

    public static string ClearPhone(this string phone)
    {
      return phone.IsNullOrEmpty() ? string.Empty : new string(phone.Where<char>(new Func<char, bool>(char.IsDigit)).ToArray<char>());
    }

    public static bool IsInn(string value) => value.Length == 10 || value.Length == 12;

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
      return source != null && source.IndexOf(toCheck, comp) >= 0;
    }

    public static BitmapImage GetQrCode(this string str, int width = 100, int height = 100)
    {
      Report report = new Report();
      ReportPage reportPage = new ReportPage()
      {
        PaperHeight = (float) width,
        PaperWidth = (float) height,
        BottomMargin = 0.0f,
        LeftMargin = 0.0f,
        RightMargin = 0.0f,
        TopMargin = 0.0f
      };
      reportPage.CreateUniqueName();
      PageHeaderBand pageHeaderBand = new PageHeaderBand();
      pageHeaderBand.CreateUniqueName();
      pageHeaderBand.Parent = (Base) reportPage;
      pageHeaderBand.Height = reportPage.Height;
      pageHeaderBand.Width = reportPage.Width;
      BarcodeObject barcodeObject = new BarcodeObject();
      barcodeObject.Parent = (Base) pageHeaderBand;
      barcodeObject.Barcode = (BarcodeBase) new BarcodeQR();
      barcodeObject.Text = str;
      barcodeObject.ShowText = false;
      barcodeObject.Bounds = new RectangleF(0.0f, 0.0f, reportPage.WidthInPixels, reportPage.HeightInPixels);
      barcodeObject.AutoSize = false;
      barcodeObject.Padding = Padding.Empty;
      report.Pages.Add((Base) reportPage);
      new EnvironmentSettings().ReportSettings.ShowProgress = false;
      report.Prepare();
      ImageExport imageExport = new ImageExport();
      imageExport.ImageFormat = ImageExportFormat.Png;
      imageExport.ShowProgress = false;
      MemoryStream memoryStream = new MemoryStream();
      imageExport.Export(report, (Stream) memoryStream);
      BitmapImage qrCode = new BitmapImage();
      qrCode.BeginInit();
      qrCode.StreamSource = (Stream) memoryStream;
      qrCode.CacheOption = BitmapCacheOption.OnLoad;
      qrCode.EndInit();
      qrCode.Freeze();
      return qrCode;
    }

    public static BitmapImage GetDataMatrixCode(this string str, int width = 100, int height = 100)
    {
      Report report = new Report();
      ReportPage reportPage = new ReportPage()
      {
        PaperHeight = (float) width,
        PaperWidth = (float) height,
        BottomMargin = 0.0f,
        LeftMargin = 0.0f,
        RightMargin = 0.0f,
        TopMargin = 0.0f
      };
      reportPage.CreateUniqueName();
      PageHeaderBand pageHeaderBand = new PageHeaderBand();
      pageHeaderBand.CreateUniqueName();
      pageHeaderBand.Parent = (Base) reportPage;
      pageHeaderBand.Height = reportPage.Height;
      pageHeaderBand.Width = reportPage.Width;
      BarcodeObject barcodeObject = new BarcodeObject();
      barcodeObject.Parent = (Base) pageHeaderBand;
      barcodeObject.Barcode = (BarcodeBase) new BarcodeDatamatrix();
      barcodeObject.Text = str;
      barcodeObject.ShowText = false;
      barcodeObject.Bounds = new RectangleF(15f, 15f, reportPage.WidthInPixels - 30f, reportPage.HeightInPixels - 30f);
      barcodeObject.AutoSize = false;
      barcodeObject.Padding = Padding.Empty;
      report.Pages.Add((Base) reportPage);
      new EnvironmentSettings().ReportSettings.ShowProgress = false;
      report.Prepare();
      ImageExport imageExport = new ImageExport();
      imageExport.ImageFormat = ImageExportFormat.Png;
      imageExport.ShowProgress = false;
      MemoryStream memoryStream = new MemoryStream();
      imageExport.Export(report, (Stream) memoryStream);
      BitmapImage dataMatrixCode = new BitmapImage();
      dataMatrixCode.BeginInit();
      dataMatrixCode.StreamSource = (Stream) memoryStream;
      dataMatrixCode.CacheOption = BitmapCacheOption.OnLoad;
      dataMatrixCode.EndInit();
      dataMatrixCode.Freeze();
      return dataMatrixCode;
    }
  }
}
