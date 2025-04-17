// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.FrmSelectedStorage
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms._shared
{
  public class FrmSelectedStorage : WindowWithSize, IComponentConnector
  {
    private bool _contentLoaded;

    public FrmSelectedStorage() => this.InitializeComponent();

    public bool GetListSelectedStorages(ref List<Storages.Storage> list)
    {
      StorageSelectedViewModel selectedViewModel = new StorageSelectedViewModel(list)
      {
        Close = new Action(((Window) this).Close)
      };
      this.DataContext = (object) selectedViewModel;
      this.ShowDialog();
      list = selectedViewModel.SelectedStorages;
      return selectedViewModel.Result;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/_shared/frmselectedstorage.xaml", UriKind.Relative));
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
