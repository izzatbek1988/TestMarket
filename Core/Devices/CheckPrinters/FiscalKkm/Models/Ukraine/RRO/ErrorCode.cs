// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO.ErrorCode
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.RRO
{
  public enum ErrorCode
  {
    Ok = 0,
    TransactionsRegistrarAbsent = 1,
    OperatorAccessToTransactionsRegistrarNotGranted = 2,
    InvalidTin = 3,
    ShiftAlreadyOpened = 4,
    ShiftNotOpened = 5,
    LastDocumentMustBeZRep = 6,
    CheckLocalNumberInvalid = 7,
    ZRepAlreadyRegistered = 8,
    DocumentValidationError = 9,
    PackageValidationError = 10, // 0x0000000A
    InvalidQueryParameter = 11, // 0x0000000B
    CryptographyError = 12, // 0x0000000C
    FiscalServerNotAvailable = 100, // 0x00000064
    FiscalServerError = 101, // 0x00000065
    OfflineModeNotSupported = 102, // 0x00000066
    InconsistentRegistrarState = 103, // 0x00000067
    LicenseViolation = 1000, // 0x000003E8
  }
}
