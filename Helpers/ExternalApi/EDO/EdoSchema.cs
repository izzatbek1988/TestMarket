// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.Methods
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  public static class Methods
  {
    public static string GetXmlEnumAttributeValueFromEnum<TEnum>(this TEnum value) where TEnum : struct, IConvertible
    {
      Type type = typeof (TEnum);
      if (!type.IsEnum)
        return (string) null;
      MemberInfo memberInfo = ((IEnumerable<MemberInfo>) type.GetMember(value.ToString())).FirstOrDefault<MemberInfo>();
      if (memberInfo == (MemberInfo) null)
        return (string) null;
      return memberInfo.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault<XmlEnumAttribute>()?.Name;
    }
  }
}
