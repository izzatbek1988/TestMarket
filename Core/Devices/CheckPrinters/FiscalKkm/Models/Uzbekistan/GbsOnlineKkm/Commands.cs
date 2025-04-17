// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Commands_NO_USE
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan.Entities;
using Gbs.Resources.Localizations;
using System;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Uzbekistan
{
  public static class Commands_NO_USE
  {
    public static void SendSaleCheck(Receipt receipt)
    {
      try
      {
        SendSaleRequest sendSaleRequest = new SendSaleRequest();
        sendSaleRequest.Params = (object) new ParamsSendSale(receipt)
        {
          Receipt = receipt
        };
        DataExchangeHelper.SendCommand((IRequest) sendSaleRequest);
      }
      catch
      {
        throw new Exception(Translate.Commands_NO_USE_Произошла_ошибка_при_создании_чека_продажи);
      }
    }

    public static void SendReturnCheck(Receipt receipt)
    {
      try
      {
        SendReturnRequest sendReturnRequest = new SendReturnRequest();
        sendReturnRequest.Params = (object) new ParamsSendSale(receipt)
        {
          Receipt = receipt
        };
        DataExchangeHelper.SendCommand((IRequest) sendReturnRequest);
      }
      catch
      {
        throw new Exception(Translate.Commands_NO_USE_Произошла_ошибка_при_создании_чека_возврата);
      }
    }

    public static void OpenSession()
    {
      try
      {
        DataExchangeHelper.SendCommand((IRequest) new OpenSessionRequest());
      }
      catch
      {
        throw new Exception(Translate.Commands_NO_USE_Произошла_ошибка_при_открытии_смены);
      }
    }

    public static void CloseSession()
    {
      try
      {
        DataExchangeHelper.SendCommand((IRequest) new CloseSessionRequest());
      }
      catch
      {
        throw new Exception(Translate.Commands_NO_USE_Произошла_ошибка_при_закрытии_смены);
      }
    }
  }
}
