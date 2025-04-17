// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.Tsd.Models.MobileSmarts
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.Tsd.Models
{
  public class MobileSmarts : ITsd, IDevice
  {
    public IDevice.DeviceTypes Type() => IDevice.DeviceTypes.Tsd;

    private Gbs.Core.Config.Devices DevicesConfig { get; set; }

    private MobileSmartsApi Api { get; set; }

    public MobileSmarts(Gbs.Core.Config.Devices devicesConfig)
    {
      this.DevicesConfig = devicesConfig;
    }

    public string Name => Translate.MobileSMARTSМагазин15;

    public void ShowProperties()
    {
      new FrmConnectionSettings().ShowConfig(new ConnectionSettingsViewModel.ConnectionConfig(this.DevicesConfig.Tsd.Lan, (ComPort) null, ConnectionSettingsViewModel.PortsConfig.OnlyLan)
      {
        NeedAuth = false
      });
    }

    public bool Connect(bool onlyDriverLoad = false)
    {
      if (onlyDriverLoad)
        return true;
      this.Api = new MobileSmartsApi(this.DevicesConfig.Tsd.Lan);
      return true;
    }

    public bool Disconnect() => true;

    public List<GoodForTsd> ReadInventory(string idDoc)
    {
      MobileSmartsApi.GetInventoryCommand command = new MobileSmartsApi.GetInventoryCommand()
      {
        DocumentId = idDoc
      };
      this.Api.DoCommand((MobileSmartsApi.MobileSmartsCommand) command);
      return command.Result.Items.GroupBy<MobileSmartsApi.CreatedInventoryCommand.DocumentItem, string>((Func<MobileSmartsApi.CreatedInventoryCommand.DocumentItem, string>) (x => x.ProductId)).Select<IGrouping<string, MobileSmartsApi.CreatedInventoryCommand.DocumentItem>, GoodForTsd>((Func<IGrouping<string, MobileSmartsApi.CreatedInventoryCommand.DocumentItem>, GoodForTsd>) (x => new GoodForTsd()
      {
        Tag = x.Key,
        Quantity = x.Sum<MobileSmartsApi.CreatedInventoryCommand.DocumentItem>((Func<MobileSmartsApi.CreatedInventoryCommand.DocumentItem, Decimal>) (q => q.CurrentQuantity))
      })).ToList<GoodForTsd>();
    }

    public void WriteInventory(List<GoodForTsd> goods, string idDoc)
    {
      List<MobileSmartsApi.AddProductsCommand> list = goods.Select<GoodForTsd, MobileSmartsApi.AddProductsCommand>((Func<GoodForTsd, MobileSmartsApi.AddProductsCommand>) (x => new MobileSmartsApi.AddProductsCommand()
      {
        Uid = x.Tag,
        Barcode = x.Barcode,
        Name = x.Name,
        Packings = new List<MobileSmartsApi.AddProductsCommand.Packing>()
        {
          new MobileSmartsApi.AddProductsCommand.Packing()
          {
            UnitName = x.UnitName,
            UnitBarcode = x.Barcode
          }
        },
        Count = x.Quantity,
        Price = x.Price
      })).ToList<MobileSmartsApi.AddProductsCommand>();
      this.Api.DoCommand((MobileSmartsApi.MobileSmartsCommand) new MobileSmartsApi.BeginOverwriteCommand());
      foreach (MobileSmartsApi.MobileSmartsCommand command in list)
        this.Api.DoCommand(command);
      this.Api.DoCommand((MobileSmartsApi.MobileSmartsCommand) new MobileSmartsApi.EndOverwriteCommand());
      this.Api.DoCommand((MobileSmartsApi.MobileSmartsCommand) new MobileSmartsApi.CreatedInventoryCommand()
      {
        Id = idDoc,
        Name = string.Format(Translate.MobileSmarts_WriteInventory_Инвентаризация_от__0_dd_MMMM_, (object) DateTime.Now),
        Items = list.Select<MobileSmartsApi.AddProductsCommand, MobileSmartsApi.CreatedInventoryCommand.DocumentItem>((Func<MobileSmartsApi.AddProductsCommand, MobileSmartsApi.CreatedInventoryCommand.DocumentItem>) (x => new MobileSmartsApi.CreatedInventoryCommand.DocumentItem()
        {
          DeclaredQuantity = x.Count,
          ProductId = x.Uid,
          Barcode = x.Barcode,
          MainBarcode = x.Barcode,
          OtherBarcode = x.Barcode
        })).ToList<MobileSmartsApi.CreatedInventoryCommand.DocumentItem>()
      });
      this.Api.DoCommand((MobileSmartsApi.MobileSmartsCommand) new MobileSmartsApi.EndUpdateDocumentCommand()
      {
        DocumentId = idDoc
      });
    }

    public void TestConnect()
    {
      MobileSmartsApi.BaseInfoCommand command = new MobileSmartsApi.BaseInfoCommand();
      this.Api.DoCommand((MobileSmartsApi.MobileSmartsCommand) command);
      int num = (int) MessageBoxHelper.Show(Translate.MobileSmarts_TestConnect_Связь_с_ТСД_успешно_установлена__ + "\n\n" + command.Result.AppName + "\nВерсия: " + command.Result.ServerVersion);
    }
  }
}
