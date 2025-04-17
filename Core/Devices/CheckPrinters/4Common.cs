// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.Cashier
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Resources.Localizations;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters
{
  public class Cashier
  {
    private string _name = Translate.CheckData_Кассир;
    private string _inn = string.Empty;

    public Guid UserUid { get; set; } = Guid.Empty;

    public string Name
    {
      get => !this._name.IsNullOrEmpty() ? this._name : Translate.CheckData_Кассир;
      set => this._name = value;
    }

    public string Inn
    {
      get => !this._inn.IsNullOrEmpty() ? this._inn : new string('0', 12);
      set => this._inn = value;
    }

    public Cashier()
    {
    }

    public Cashier(Users.User user)
    {
      this.UserUid = user.Uid;
      this.Name = user.Client.Name;
      this.Inn = user.Client.Properties.SingleOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? "";
    }
  }
}
