// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DB.Corrections.V8
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Helpers.DB.Corrections
{
  public class V8 : ICorrection
  {
    public bool Do()
    {
      string goodsImagesPath = new ConfigsRepository<DataBase>().Get().GoodsImagesPath;
      if (!Directory.Exists(goodsImagesPath) || !((IEnumerable<string>) Directory.GetDirectories(goodsImagesPath)).Any<string>())
        return true;
      foreach (string directory in Directory.GetDirectories(goodsImagesPath))
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        foreach (string file in Directory.GetFiles(directory))
        {
          string extension = Path.GetExtension(file);
          string destPath = Path.Combine(goodsImagesPath, directoryInfo.Name, Path.GetFileNameWithoutExtension(file) + "_1" + extension);
          if (ImagesHelpers.CompressImage(file, destPath))
            File.Delete(file);
        }
      }
      return true;
    }
  }
}
