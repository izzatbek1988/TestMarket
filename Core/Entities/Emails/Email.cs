// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Emails.Email
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Entities.Emails
{
  public class Email : IEntity
  {
    public TypeReport TypeReport { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string Subject { get; set; }

    public object Body { get; set; }

    public List<string> Attach { get; set; } = new List<string>();

    public string AddressFrom => "reports@gbs.market";

    public string AddressTo { get; set; }

    public bool IsSend { get; set; }

    public Guid Uid { get; set; } = Guid.NewGuid();

    public bool IsDeleted { get; set; }
  }
}
