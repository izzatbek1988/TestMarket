// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SoundFlags
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;

#nullable disable
namespace Gbs.Helpers
{
  [Flags]
  internal enum SoundFlags
  {
    SND_SYNC = 0,
    SND_ASYNC = 1,
    SND_NODEFAULT = 2,
    SND_MEMORY = 4,
    SND_LOOP = 8,
    SND_NOSTOP = 16, // 0x00000010
    SND_NOWAIT = 8192, // 0x00002000
    SND_ALIAS = 65536, // 0x00010000
    SND_ALIAS_ID = 1114112, // 0x00110000
    SND_FILENAME = 131072, // 0x00020000
  }
}
