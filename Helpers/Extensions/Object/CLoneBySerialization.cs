// Decompiled with JetBrains decompiler
// Type: System.ObjectCopier
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Newtonsoft.Json;

#nullable disable
namespace System
{
  public static class ObjectCopier
  {
    public static T Clone<T>(this T source)
    {
      if ((object) source == null)
        return default (T);
      JsonSerializerSettings settings = new JsonSerializerSettings()
      {
        ObjectCreationHandling = ObjectCreationHandling.Replace
      };
      return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject((object) source), settings);
    }
  }
}
