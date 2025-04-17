// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ScaleWithLable
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using System;

#nullable disable
namespace Gbs.Core.Config
{
  public class ScaleWithLable
  {
    public Guid PluUid { get; set; }

    public GlobalDictionaries.Devices.ScaleLableTypes Type { get; set; }

    public GlobalDictionaries.Devices.CasScaleLableTypes CasType { get; set; } = GlobalDictionaries.Devices.CasScaleLableTypes.CL5000;

    public bool CorrectPriceForRongta { get; set; } = true;

    public DeviceConnection Connection { get; set; } = new DeviceConnection();
  }
}
