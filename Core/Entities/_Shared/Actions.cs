// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Actions
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Entities
{
  public enum Actions
  {
    SettingsShowAndEdit = 10, // 0x0000000A
    GoodsCatalogShow = 20, // 0x00000014
    GoodsCreate = 30, // 0x0000001E
    GoodsEdit = 40, // 0x00000028
    GoodsDelete = 50, // 0x00000032
    GoodsJoin = 52, // 0x00000034
    GroupEditingGoodAndCategories = 55, // 0x00000037
    SaleSave = 60, // 0x0000003C
    ReturnSale = 70, // 0x00000046
    DeleteSale = 75, // 0x0000004B
    ShowJournalSale = 76, // 0x0000004C
    RemoveCash = 80, // 0x00000050
    InsertCash = 90, // 0x0000005A
    SendCash = 100, // 0x00000064
    DeletePayment = 101, // 0x00000065
    WaybillAdd = 110, // 0x0000006E
    WaybillEdit = 111, // 0x0000006F
    WaybillDelete = 112, // 0x00000070
    WaybillListShow = 120, // 0x00000078
    CorrectSumByAcc = 130, // 0x00000082
    CorrectBalanceSum = 135, // 0x00000087
    ClientsCatalogShow = 140, // 0x0000008C
    ClientsAdd = 150, // 0x00000096
    ClientsEdit = 160, // 0x000000A0
    ClientsBonusesEdit = 165, // 0x000000A5
    ClientsDelete = 170, // 0x000000AA
    ClientJoin = 175, // 0x000000AF
    CreditReturn = 180, // 0x000000B4
    ShowCredits = 190, // 0x000000BE
    ViewStock = 200, // 0x000000C8
    ViewHistory = 210, // 0x000000D2
    ExecuteScript = 220, // 0x000000DC
    PrintKkmReport = 230, // 0x000000E6
    CreateInventory = 240, // 0x000000F0
    EditInventory = 250, // 0x000000FA
    DeleteInventory = 260, // 0x00000104
    ShowJournalInventory = 270, // 0x0000010E
    CreateWriteOff = 280, // 0x00000118
    EditWriteOff = 290, // 0x00000122
    DeleteWriteOff = 300, // 0x0000012C
    ShowJournalWriteOff = 310, // 0x00000136
    ShowSummaryReport = 320, // 0x00000140
    CreateClientOrder = 330, // 0x0000014A
    EditClientOrder = 340, // 0x00000154
    DeleteClientOrder = 350, // 0x0000015E
    AddGoodGroup = 360, // 0x00000168
    EditGoodGroup = 370, // 0x00000172
    DeleteGoodGroup = 380, // 0x0000017C
    AddClientGroup = 390, // 0x00000186
    EditClientGroup = 400, // 0x00000190
    DeleteClientGroup = 410, // 0x0000019A
    AddMoveWaybill = 420, // 0x000001A4
    DeleteMoveWaybill = 430, // 0x000001AE
    ShowBuyPrice = 440, // 0x000001B8
    ShowMasterReport = 450, // 0x000001C2
    ShowSellerReport = 455, // 0x000001C7
    DeleteItemBasket = 460, // 0x000001CC
    EditCountItemBasket = 470, // 0x000001D6
    EditDiscountItem = 480, // 0x000001E0
    CancelSale = 490, // 0x000001EA
    AddMoveStorage = 500, // 0x000001F4
    DeleteMoveStorage = 510, // 0x000001FE
    WaybillReturnAdd = 511, // 0x000001FF
    WaybillReturnEdit = 512, // 0x00000200
    WaybillReturnDelete = 513, // 0x00000201
    WaybillReturnListShow = 514, // 0x00000202
    AddProduction = 520, // 0x00000208
    DeleteProduction = 521, // 0x00000209
    ShowProduction = 522, // 0x0000020A
    AddSpeedProduction = 523, // 0x0000020B
    ShowEgaisWaybill = 530, // 0x00000212
    AcceptEgaisWaybill = 531, // 0x00000213
    AddGoodStock = 540, // 0x0000021C
    EditSalePriceGoodStock = 541, // 0x0000021D
    DeleteGoodStock = 542, // 0x0000021E
    EditQuantityGoodStock = 543, // 0x0000021F
    DeleteOrderCafe = 600, // 0x00000258
    EditFrReport = 610, // 0x00000262
    DoSaleCreditIfOffSmsCode = 620, // 0x0000026C
    DoUseBonusesIfOffSmsCode = 630, // 0x00000276
    ActionsToBeerTap = 640, // 0x00000280
  }
}
