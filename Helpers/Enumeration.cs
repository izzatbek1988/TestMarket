// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Enumeration
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Gbs.Helpers
{
  public abstract class Enumeration : IComparable
  {
    public string Name { get; private set; }

    public int Id { get; private set; }

    protected Enumeration(int id, string name)
    {
      this.Id = id;
      this.Name = name;
    }

    public override string ToString() => this.Name;

    protected static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
      return ((IEnumerable<FieldInfo>) typeof (T).GetFields()).Select<FieldInfo, object>((Func<FieldInfo, object>) (f => f.GetValue((object) null))).Cast<T>();
    }

    public override bool Equals(object obj)
    {
      return obj is Enumeration enumeration && this.GetType() == obj.GetType() & this.Id.Equals(enumeration.Id);
    }

    public int CompareTo(object other) => this.Id.CompareTo(((Enumeration) other).Id);
  }
}
