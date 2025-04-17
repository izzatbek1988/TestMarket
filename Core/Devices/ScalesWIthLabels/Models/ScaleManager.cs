// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.ScalesWIthLabels.Models.ScaleManager
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Core.Devices.ScalesWIthLabels.Models
{
  public class ScaleManager : IScalesWIthLabels, IDevice
  {
    private const string PathDriver = "dll\\label_scale\\scaleManager\\";

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.ScaleWithLable;

    public string Name => nameof (ScaleManager);

    public bool ShowProperties()
    {
      int num = (int) MessageBoxHelper.Show(Translate.ScaleManager_ShowProperties_Настраивать_параметры_подключения_и_папку_обмена_необходимо_в_приложении_ScaleManager);
      return true;
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      if (onlyDriverLoad)
        return true;
      string path = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\scaleManager\\");
      Directory.CreateDirectory(path);
      return Directory.Exists(path);
    }

    public bool Disconnect() => true;

    public int SendGood(List<GoodForWith> goods)
    {
      string destFileName = Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\scaleManager\\ExportedForScale.xml");
      ScaleManager.NewDataSet o = new ScaleManager.NewDataSet()
      {
        Report = new List<ScaleManager.Report>()
      };
      foreach (GoodForWith good in goods)
        o.Report.Add(new ScaleManager.Report()
        {
          Price = good.Price,
          BarCode = good.Plu,
          Code = good.Plu,
          Caption = good.Name,
          IsPiece = 0
        });
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (ScaleManager.NewDataSet));
      string str = Path.Combine(FileSystemHelper.TempFolderPath(), "ExportedForScale.xml");
      using (FileStream fileStream = new FileStream(str, FileMode.Create))
        xmlSerializer.Serialize((Stream) fileStream, (object) o);
      File.Copy(str, destFileName, true);
      File.Delete(str);
      return goods.Count;
    }

    [XmlRoot(ElementName = "Report")]
    public class Report
    {
      [XmlElement(ElementName = "Code")]
      public int Code { get; set; }

      [XmlElement(ElementName = "Caption")]
      public string Caption { get; set; }

      [XmlElement(ElementName = "Price")]
      public Decimal Price { get; set; }

      [XmlElement(ElementName = "BarCode")]
      public int BarCode { get; set; }

      [XmlElement(ElementName = "IsPiece")]
      public int IsPiece { get; set; }
    }

    [XmlRoot(ElementName = "NewDataSet")]
    public class NewDataSet
    {
      [XmlElement(ElementName = "Report")]
      public List<ScaleManager.Report> Report { get; set; }
    }
  }
}
