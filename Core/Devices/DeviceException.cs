// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.DeviceException
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers.ErrorHandler.Exceptions;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Core.Devices
{
  public class DeviceException : Exception, IExceptionWithExtMessage
  {
    public IDevice Device;

    public DeviceException()
    {
    }

    public DeviceException(string message)
      : base(message)
    {
    }

    public DeviceException(string message, IDevice device)
      : base(message)
    {
      this.Device = device;
    }

    public DeviceException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected DeviceException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public string ExtMessage { get; set; }
  }
}
