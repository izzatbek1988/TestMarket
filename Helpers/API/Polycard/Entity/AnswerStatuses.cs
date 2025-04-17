// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.API.Polycard.Entity.AnswerStatuses
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Runtime.Serialization;

#nullable disable
namespace Gbs.Helpers.API.Polycard.Entity
{
  public enum AnswerStatuses
  {
    [EnumMember(Value = "OK")] Ok,
    [EnumMember(Value = "NOK")] Nok,
    [EnumMember(Value = "EXISTED")] Existed,
    [EnumMember(Value = "NOT FOUND")] NotFound,
    [EnumMember(Value = "NOT AUTHORIZED")] NotAuthorized,
  }
}
