// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.MettlerToledo
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Gbs.Core.Devices.ScalesWIthLabels.Models
{
  public class MettlerToledo : IScalesWIthLabels, IDevice
  {
    private const string PathDriver = "dll\\label_scale\\mettler\\";

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.ScaleWithLable;

    public string Name => nameof (MettlerToledo);

    public bool ShowProperties()
    {
      MessageBoxHelper.Warning(Translate.MettlerToledo_ShowProperties_Настраивать_параметры_подключения_к_весам__Mettler_Toledo_необходимо_в_файле_SCALEADDRESS_INI);
      return true;
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      return onlyDriverLoad || FileSystemHelper.CheckFileExistWithMsg(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\mettler\\SCALEADDRESS.INI")) && FileSystemHelper.CheckFileExistWithMsg(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\mettler\\TransferEth.dll")) && FileSystemHelper.CheckFileExistWithMsg(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\mettler\\Transfer.exe"));
    }

    public bool Disconnect() => true;

    public int SendGood(List<GoodForWith> goods)
    {
      MatchCollection matchCollection = new Regex("\\[\\d*\\]").Matches(File.ReadAllText("dll\\label_scale\\mettler\\SCALEADDRESS.INI"));
      if (matchCollection.Count == 0)
        return 0;
      string path1 = FileSystemHelper.TempFolderPath();
      string destinationFile1 = Path.Combine("dll\\label_scale\\mettler\\", "trf.out");
      string str1 = Path.Combine(path1, "trf.out");
      string destinationFile2 = Path.Combine("dll\\label_scale\\mettler\\", "Transscale.ini");
      string str2 = Path.Combine(path1, "Transscale.ini");
      List<string> contents1 = new List<string>(goods.Select<GoodForWith, string>((Func<GoodForWith, string>) (x => string.Format("{0}, 1, 1, {1}, 0, 0, 0, 0, 0, 0, 0, 0, 0, {2}", (object) x.Plu, (object) x.Price.ToString("##.00", (IFormatProvider) CultureInfo.GetCultureInfo("en-US")), (object) x.Name.Replace("і", "i").Replace("І", "I")))));
      File.WriteAllLines(str1, (IEnumerable<string>) contents1, Encoding.GetEncoding(1251));
      if (!FileSystemHelper.MoveFile(str1, destinationFile1))
        return 0;
      List<string> contents2 = new List<string>()
      {
        "trf.out"
      };
      foreach (Match match in matchCollection)
        contents2.Add(match.Value.Replace("[", "").Replace("]", ""));
      LogHelper.Debug(contents2.ToJsonString(true));
      File.WriteAllLines(str2, (IEnumerable<string>) contents2, Encoding.GetEncoding(1251));
      if (!FileSystemHelper.MoveFile(str2, destinationFile2))
        return 0;
      string fullName = new DirectoryInfo("dll\\label_scale\\mettler\\").FullName;
      Process.Start(new ProcessStartInfo(fullName + "Transfer.exe")
      {
        WorkingDirectory = fullName,
        UseShellExecute = false
      })?.WaitForExit();
      return goods.Count;
    }
  }
}
