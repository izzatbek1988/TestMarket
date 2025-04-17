// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.ComPort
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.IO.Ports;
using System.Linq;

#nullable disable
namespace Gbs.Core.Config
{
  public class ComPort
  {
    public string PortName { get; set; } = "COM1";

    public int PortNumber
    {
      get
      {
        return int.Parse(string.Join<char>("", this.PortName.Where<char>(new Func<char, bool>(char.IsNumber))));
      }
    }

    public int Speed { get; set; } = 9600;

    public int TimeOut { get; set; } = 3000;

    public int DataBit { get; set; } = 8;

    public Parity Parity { get; set; }

    public Handshake Handshake { get; set; }

    public StopBits StopBit { get; set; } = StopBits.One;
  }
}
