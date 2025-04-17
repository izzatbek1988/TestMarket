// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.SoundPlayerHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Gbs.Helpers
{
  internal static class SoundPlayerHelper
  {
    private static GCHandle? gcHandle;
    private static byte[] bytesToPlay;

    [DllImport("winmm.dll", SetLastError = true)]
    private static extern bool PlaySound(byte[] ptrToSound, UIntPtr hmod, uint fdwSound);

    [DllImport("winmm.dll", SetLastError = true)]
    private static extern bool PlaySound(IntPtr ptrToSound, UIntPtr hmod, uint fdwSound);

    private static byte[] BytesToPlay
    {
      get => SoundPlayerHelper.bytesToPlay;
      set
      {
        SoundPlayerHelper.FreeHandle();
        SoundPlayerHelper.bytesToPlay = value;
      }
    }

    public static void PlaySound(Stream stream)
    {
      SoundPlayerHelper.PlaySound(stream, SoundFlags.SND_ASYNC | SoundFlags.SND_MEMORY);
    }

    private static void PlaySound(Stream stream, SoundFlags flags)
    {
      SoundPlayerHelper.LoadStream(stream);
      flags |= SoundFlags.SND_ASYNC;
      flags |= SoundFlags.SND_MEMORY;
      if (SoundPlayerHelper.BytesToPlay != null)
      {
        SoundPlayerHelper.gcHandle = new GCHandle?(GCHandle.Alloc((object) SoundPlayerHelper.BytesToPlay, GCHandleType.Pinned));
        SoundPlayerHelper.PlaySound(SoundPlayerHelper.gcHandle.Value.AddrOfPinnedObject(), (UIntPtr) 0UL, (uint) flags);
      }
      else
        SoundPlayerHelper.PlaySound((byte[]) null, (UIntPtr) 0UL, (uint) flags);
    }

    private static void LoadStream(Stream stream)
    {
      if (stream != null)
      {
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, (int) stream.Length);
        SoundPlayerHelper.BytesToPlay = buffer;
      }
      else
        SoundPlayerHelper.BytesToPlay = (byte[]) null;
    }

    private static void FreeHandle()
    {
      if (!SoundPlayerHelper.gcHandle.HasValue)
        return;
      SoundPlayerHelper.PlaySound((byte[]) null, (UIntPtr) 0UL, 0U);
      SoundPlayerHelper.gcHandle.Value.Free();
      SoundPlayerHelper.gcHandle = new GCHandle?();
    }
  }
}
