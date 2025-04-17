// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.WebOffice.User
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;

#nullable disable
namespace Gbs.Helpers.WebOffice
{
  public class User : UserSimple, IEntity
  {
    public bool IsDeleted { get; set; }

    public string Alias { get; set; }

    public User(Users.User user)
    {
      this.Uid = user.Uid;
      this.Alias = Functions.CutName(user.Alias, true);
      this.IsDeleted = user.IsDeleted;
    }
  }
}
