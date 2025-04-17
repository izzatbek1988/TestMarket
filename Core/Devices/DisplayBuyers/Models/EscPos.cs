// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DisplayBuyers.Models.EscPos
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.DisplayBuyers.Models
{
  public class EscPos : IDisplayBuyers, IDevice
  {
    private SerialPort _port = new SerialPort();

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public EscPos(Gbs.Core.Config.Devices devicesConfig) => this.DevicesConfig = devicesConfig;

    public string LastResultCodeDescriptor { get; }

    public bool ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig((LanConnection) null, this.DevicesConfig.DisplayPayer.Port, ConnectionSettingsViewModel.PortsConfig.OnlyCom));
      return true;
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      if (onlyDriverLoad)
        return true;
      ComPort port = this.DevicesConfig.DisplayPayer.Port;
      DeviceHelper.CheckComPortExists(port.PortName, (IDevice) this);
      this._port = new SerialPort(port.PortName)
      {
        BaudRate = port.Speed,
        ReadTimeout = port.TimeOut
      };
      LogHelper.Debug(string.Format("DisplayBuyers COM-port: {0}, {1} / {2}", (object) this._port.PortName, (object) this._port.BaudRate, (object) this._port.ReadTimeout));
      this._port.Open();
      List<byte> byteList = new List<byte>();
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      byteList.Clear();
      byteList.AddRange((IEnumerable<byte>) new byte[2]
      {
        (byte) 27,
        (byte) 64
      });
      byteList.AddRange((IEnumerable<byte>) new byte[3]
      {
        (byte) 27,
        (byte) 61,
        (byte) 2
      });
      byteList.AddRange((IEnumerable<byte>) new byte[3]
      {
        (byte) 27,
        (byte) 116,
        (byte) 17
      });
      byteList.AddRange((IEnumerable<byte>) new byte[3]
      {
        (byte) 27,
        (byte) 82,
        (byte) 0
      });
      byteList.AddRange((IEnumerable<byte>) new byte[3]
      {
        (byte) 31,
        (byte) 67,
        (byte) 0
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }

    public bool Disconnect()
    {
      this._port.Close();
      this._port.Dispose();
      return true;
    }

    public bool WriteText(string line, int index)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 13);
      for (; index != 0; --index)
        byteList.Add((byte) 10);
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      byteList.Clear();
      byte[] collection;
      switch (this.DevicesConfig.DisplayPayer.DisplayEncoding)
      {
        case GlobalDictionaries.Encoding.CP866:
          collection = System.Text.Encoding.GetEncoding(866).GetBytes(line);
          break;
        case GlobalDictionaries.Encoding.CPTysso:
          collection = EscPos.CpTyssoConverter(line);
          break;
        case GlobalDictionaries.Encoding.W1251:
          collection = System.Text.Encoding.GetEncoding(1251).GetBytes(line);
          break;
        case GlobalDictionaries.Encoding.Utf8:
          collection = System.Text.Encoding.UTF8.GetBytes(line);
          break;
        case GlobalDictionaries.Encoding.KOI8R:
          collection = System.Text.Encoding.GetEncoding("KOI8-R").GetBytes(line);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      byteList.AddRange((IEnumerable<byte>) collection);
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }

    private static byte Get866ByteFromLetter(string letter)
    {
      return ((IEnumerable<byte>) System.Text.Encoding.GetEncoding(866).GetBytes(letter)).Single<byte>();
    }

    public static byte[] CpTyssoConverter(string str)
    {
      List<byte> byteList = new List<byte>();
      foreach (char ch in str)
      {
        byte num1;
        switch (ch)
        {
          case 'Ё':
            num1 = (byte) 162;
            break;
          case 'А':
            num1 = EscPos.Get866ByteFromLetter("A");
            break;
          case 'Б':
            num1 = (byte) 160;
            break;
          case 'В':
            num1 = EscPos.Get866ByteFromLetter("B");
            break;
          case 'Г':
            num1 = (byte) 161;
            break;
          case 'Д':
            num1 = (byte) 224;
            break;
          case 'Е':
            num1 = EscPos.Get866ByteFromLetter("E");
            break;
          case 'Ж':
            num1 = (byte) 163;
            break;
          case 'З':
            num1 = (byte) 164;
            break;
          case 'И':
            num1 = (byte) 165;
            break;
          case 'Й':
            num1 = (byte) 166;
            break;
          case 'К':
            num1 = EscPos.Get866ByteFromLetter("K");
            break;
          case 'Л':
            num1 = (byte) 167;
            break;
          case 'М':
            num1 = EscPos.Get866ByteFromLetter("M");
            break;
          case 'Н':
            num1 = EscPos.Get866ByteFromLetter("H");
            break;
          case 'О':
            num1 = EscPos.Get866ByteFromLetter("O");
            break;
          case 'П':
            num1 = (byte) 168;
            break;
          case 'Р':
            num1 = EscPos.Get866ByteFromLetter("P");
            break;
          case 'С':
            num1 = EscPos.Get866ByteFromLetter("C");
            break;
          case 'Т':
            num1 = EscPos.Get866ByteFromLetter("T");
            break;
          case 'У':
            num1 = (byte) 169;
            break;
          case 'Ф':
            num1 = (byte) 170;
            break;
          case 'Х':
            num1 = EscPos.Get866ByteFromLetter("X");
            break;
          case 'Ц':
            num1 = (byte) 227;
            break;
          case 'Ч':
            num1 = (byte) 171;
            break;
          case 'Ш':
            num1 = (byte) 172;
            break;
          case 'Щ':
            num1 = (byte) 226;
            break;
          case 'Ъ':
            num1 = (byte) 173;
            break;
          case 'Ы':
            num1 = (byte) 174;
            break;
          case 'Ь':
            num1 = EscPos.Get866ByteFromLetter("b");
            break;
          case 'Э':
            num1 = (byte) 175;
            break;
          case 'Ю':
            num1 = (byte) 176;
            break;
          case 'Я':
            num1 = (byte) 177;
            break;
          case 'а':
            num1 = EscPos.Get866ByteFromLetter("a");
            break;
          case 'б':
            num1 = (byte) 178;
            break;
          case 'в':
            num1 = (byte) 179;
            break;
          case 'г':
            num1 = (byte) 180;
            break;
          case 'д':
            num1 = (byte) 227;
            break;
          case 'е':
            num1 = EscPos.Get866ByteFromLetter("e");
            break;
          case 'ж':
            num1 = (byte) 182;
            break;
          case 'з':
            num1 = (byte) 183;
            break;
          case 'и':
            num1 = (byte) 184;
            break;
          case 'й':
            num1 = (byte) 185;
            break;
          case 'к':
            num1 = (byte) 186;
            break;
          case 'л':
            num1 = (byte) 187;
            break;
          case 'м':
            num1 = (byte) 188;
            break;
          case 'н':
            num1 = (byte) 189;
            break;
          case 'о':
            num1 = EscPos.Get866ByteFromLetter("o");
            break;
          case 'п':
            num1 = (byte) 190;
            break;
          case 'р':
            num1 = EscPos.Get866ByteFromLetter("p");
            break;
          case 'с':
            num1 = EscPos.Get866ByteFromLetter("c");
            break;
          case 'т':
            num1 = (byte) 191;
            break;
          case 'у':
            num1 = EscPos.Get866ByteFromLetter("y");
            break;
          case 'ф':
            num1 = (byte) 228;
            break;
          case 'х':
            num1 = EscPos.Get866ByteFromLetter("x");
            break;
          case 'ц':
            num1 = (byte) 229;
            break;
          case 'ч':
            num1 = (byte) 192;
            break;
          case 'ш':
            num1 = (byte) 193;
            break;
          case 'щ':
            num1 = (byte) 230;
            break;
          case 'ъ':
            num1 = (byte) 194;
            break;
          case 'ы':
            num1 = (byte) 195;
            break;
          case 'ь':
            num1 = (byte) 196;
            break;
          case 'э':
            num1 = (byte) 197;
            break;
          case 'ю':
            num1 = (byte) 198;
            break;
          case 'я':
            num1 = (byte) 199;
            break;
          case 'ё':
            num1 = (byte) 181;
            break;
          default:
            num1 = EscPos.Get866ByteFromLetter(ch.ToString());
            break;
        }
        byte num2 = num1;
        byteList.Add(num2);
      }
      return byteList.ToArray();
    }

    public bool Clear()
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) new byte[1]
      {
        (byte) 12
      });
      this._port.Write(byteList.ToArray(), 0, byteList.Count);
      return true;
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.DisplayBuyer;

    public string Name { get; }
  }
}
