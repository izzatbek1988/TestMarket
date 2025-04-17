// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.IRepository`1
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Gbs.Core.Entities
{
  public interface IRepository<T> where T : class
  {
    List<T> GetAllItems();

    List<T> GetActiveItems();

    T GetByUid(Guid uid);

    bool Save(T item);

    int Save(List<T> itemsList);

    int Delete(List<T> itemsList);

    bool Delete(T item);

    ActionResult Validate(T item);
  }
}
