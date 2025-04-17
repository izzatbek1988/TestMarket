// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Excel.FrmExcelClients
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Excel
{
  public class FrmExcelClients : WindowWithSize, IComponentConnector
  {
    internal DataGrid GridExcelPropClients;
    private bool _contentLoaded;

    public FrmExcelClients() => this.InitializeComponent();

    public List<Client> Import()
    {
      ExcelClientsViewModel clientsViewModel1 = new ExcelClientsViewModel();
      clientsViewModel1.CloseAction = new Action(((Window) this).Close);
      ExcelClientsViewModel clientsViewModel2 = clientsViewModel1;
      this.DataContext = (object) clientsViewModel2;
      this.ShowDialog();
      return clientsViewModel2.ClientsImport;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/excel/frmexcelclients.xaml", UriKind.Relative));
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
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.GridExcelPropClients = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
