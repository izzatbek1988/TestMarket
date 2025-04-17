// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.License.FrmLicenseInfo
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.License
{
  public partial class FrmLicenseInfo : WindowWithSize, IComponentConnector
  {
    private bool _contentLoaded;

    public FrmLicenseInfo() => this.InitializeComponent();

    private void WindowWithSize_Drop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        return;
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      if (data.Length != 1)
        MessageBoxHelper.Warning(Translate.FrmLicenseInfo_WindowWithSize_Drop_Перетащите_только_один_файл_ключа_);
      else if (!((IEnumerable<string>) new string[2]
      {
        ".id",
        ".gbs6"
      }).Contains<string>(Path.GetExtension(data[0]).ToLower()))
      {
        MessageBoxHelper.Warning(Translate.FrmLicenseInfo_WindowWithSize_Drop_Доступны_для_перетаскивания_только_файлы_лицензионных_ключей_);
      }
      else
      {
        string dataPath = ApplicationInfo.GetInstance().Paths.DataPath;
        try
        {
          string destFileName = Path.Combine(dataPath, "key.id");
          File.Copy(data[0], destFileName, true);
          ((LicenseInfoViewModel) this.DataContext).InitData();
          ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(Translate.FrmLicenseInfo_WindowWithSize_Drop_Файл_ключа_лицензии_успешено_применен));
        }
        catch (Exception ex)
        {
          LogHelper.WriteError(ex);
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/license/frmlicenseinfo.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
