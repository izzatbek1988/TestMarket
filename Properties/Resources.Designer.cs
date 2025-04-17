// Decompiled with JetBrains decompiler
// Type: Gbs.Properties.Resources
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Gbs.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Gbs.Properties.Resources.resourceMan == null)
          Gbs.Properties.Resources.resourceMan = new ResourceManager("Gbs.Properties.Resources", typeof (Gbs.Properties.Resources).Assembly);
        return Gbs.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Gbs.Properties.Resources.resourceCulture;
      set => Gbs.Properties.Resources.resourceCulture = value;
    }

    internal static UnmanagedMemoryStream error
    {
      get => Gbs.Properties.Resources.ResourceManager.GetStream(nameof (error), Gbs.Properties.Resources.resourceCulture);
    }

    internal static UnmanagedMemoryStream info
    {
      get => Gbs.Properties.Resources.ResourceManager.GetStream(nameof (info), Gbs.Properties.Resources.resourceCulture);
    }

    internal static UnmanagedMemoryStream question
    {
      get => Gbs.Properties.Resources.ResourceManager.GetStream(nameof (question), Gbs.Properties.Resources.resourceCulture);
    }

    internal static UnmanagedMemoryStream warning
    {
      get => Gbs.Properties.Resources.ResourceManager.GetStream(nameof (warning), Gbs.Properties.Resources.resourceCulture);
    }
  }
}
