// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.AcquiringTerminals.Models.Arcus2
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.AcquiringTerminals.Models
{
  public class Arcus2 : IAcquiringTerminal, IDevice
  {
    private const string PathDriver = "C:\\Arcus2\\";

    public void EmergencyCancel() => LogHelper.Debug("Аварийная отмена платежа: не реализовано");

    private (string slip, string errorCode) ReadSlipInFile()
    {
      string str = File.Exists("C:\\Arcus2\\rc.out") ? File.ReadAllText("C:\\Arcus2\\rc.out", Encoding.GetEncoding("windows-1251")).Remove(3) : string.Empty;
      if (str != "000")
        throw new DeviceException(Translate.Arcus2_Во_время_выполнения_операции_произошла_ошибка_ + str);
      string message = File.Exists("C:\\Arcus2\\cheq.out") ? File.ReadAllText("C:\\Arcus2\\cheq.out", Encoding.GetEncoding("windows-1251")) : string.Empty;
      LogHelper.Debug(message);
      return (message, str);
    }

    private void RunCommand(string command)
    {
      string path = "C:\\Arcus2\\CommandLineTool\\bin\\CommandLineTool.exe";
      if (!File.Exists(path))
        throw new AcquiringException((IDevice) this, string.Format(Translate.Arcus2_RunCommand_, (object) path), AcquiringException.ErrorTypes.DeviceNoFound);
      Process.Start(new ProcessStartInfo()
      {
        FileName = path,
        Arguments = command
      }).WaitForExit();
    }

    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.AcquiringTerminal;

    public string Name => "ARCUS2";

    public void ShowProperties()
    {
      int num = (int) MessageBoxHelper.Show(Translate.Arcus2_ShowProperties_Настроить_параметры_подключения_по_протоколу_Arcus2_нужно_в_файле_cashreg_ini_);
    }

    public void ShowServiceMenu(out string slip)
    {
      this.RunCommand("/o98");
      (slip, _) = this.ReadSlipInFile();
    }

    public bool DoPayment(Decimal sum, out string slip, out string rrn, out string method)
    {
      method = string.Empty;
      this.RunCommand(string.Format("/o1 /c643 /a{0}", (object) (int) (sum * 100M)));
      (slip, _) = this.ReadSlipInFile();
      rrn = "";
      return true;
    }

    public bool ReturnPayment(Decimal sum, out string slip, string rrn, string method)
    {
      this.RunCommand(string.Format("/o3 /c643 /a{0}", (object) (int) (sum * 100M)));
      (slip, _) = this.ReadSlipInFile();
      return true;
    }

    public bool GetReport(out string slip)
    {
      this.RunCommand("/o8");
      (slip, _) = this.ReadSlipInFile();
      return true;
    }

    public bool CloseSession(out string slip)
    {
      this.RunCommand("/o11");
      (slip, _) = this.ReadSlipInFile();
      return true;
    }

    public bool Connect()
    {
      if (!Directory.Exists("C:\\Arcus2"))
        throw new DeviceException("Не удалось найти папку C:\\Arcus2, подключение невозможно");
      return true;
    }

    public bool Disconnect() => true;
  }
}
