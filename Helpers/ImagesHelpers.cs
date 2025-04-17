// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ImagesHelpers
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Gbs.Helpers
{
  public class ImagesHelpers
  {
    public static BitmapSource NullBitmapSource
    {
      get
      {
        return BitmapSource.Create(1, 1, 1.0, 1.0, PixelFormats.Indexed1, new BitmapPalette((IList<System.Windows.Media.Color>) new List<System.Windows.Media.Color>()
        {
          Colors.Transparent
        }), (Array) new byte[4], 1);
      }
    }

    public static bool CompressImage(string sourcePath, string destPath, int quality = 75)
    {
      try
      {
        ImageFormat format;
        switch (Path.GetExtension(sourcePath).ToLower())
        {
          case ".png":
            format = ImageFormat.Png;
            break;
          case ".bmp":
            format = ImageFormat.Bmp;
            break;
          default:
            format = ImageFormat.Jpeg;
            break;
        }
        using (Bitmap bitmap1 = new Bitmap(sourcePath))
        {
          ImageCodecInfo encoder = ImagesHelpers.GetEncoder(format);
          EncoderParameters encoderParameters = new EncoderParameters(1);
          encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (long) quality);
          EncoderParameters encoderParams = encoderParameters;
          int width = 1024;
          int height = (int) ((Decimal) bitmap1.Height * 1.0M / (Decimal) bitmap1.Width * 1024M);
          if (width < bitmap1.Width || height < bitmap1.Height)
          {
            using (Bitmap bitmap2 = new Bitmap(width, height))
            {
              using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
                graphics.DrawImage((Image) bitmap1, 0, 0, width, height);
              bitmap2.Save(destPath, encoder, encoderParams);
            }
          }
          else
            bitmap1.Save(destPath, encoder, encoderParams);
          return true;
        }
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex, "Ошбика сжатия изображения");
        return false;
      }
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
      foreach (ImageCodecInfo imageDecoder in ImageCodecInfo.GetImageDecoders())
      {
        if (imageDecoder.FormatID == format.Guid)
          return imageDecoder;
      }
      return (ImageCodecInfo) null;
    }

    public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
        bmpBitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) bitmapImage));
        bmpBitmapEncoder.Save((Stream) memoryStream);
        return new Bitmap((Image) new Bitmap((Stream) memoryStream));
      }
    }

    public static BitmapImage ConvertToImage(string path, int maxSize = 800)
    {
      using (Image original = Image.FromFile(path))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          double num = Math.Min((double) maxSize / (double) original.Width, (double) maxSize / (double) original.Height);
          int width = (int) ((double) original.Width * num);
          int height = (int) ((double) original.Height * num);
          new Bitmap(original, new Size(width, height)).Save((Stream) memoryStream, ImageFormat.Png);
          memoryStream.Position = 0L;
          BitmapImage image = new BitmapImage();
          image.BeginInit();
          image.StreamSource = (Stream) memoryStream;
          image.CacheOption = BitmapCacheOption.OnLoad;
          image.EndInit();
          image.Freeze();
          return image;
        }
      }
    }
  }
}
