// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.HomeOffice.FrmSelectPoint
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.HomeOffice
{
  public class FrmSelectPoint : WindowWithSize, IComponentConnector
  {
    internal System.Windows.Controls.DataGrid ListPoint;
    private bool _contentLoaded;

    public FrmSelectPoint() => this.InitializeComponent();

    private void FrmSelectPoint_OnClosing(object sender, CancelEventArgs e)
    {
      try
      {
        PointSelectViewModel dataContext = (PointSelectViewModel) this.DataContext;
        if (dataContext == null)
        {
          Gbs.Helpers.Other.SetCorrectExit();
          System.Environment.Exit(0);
        }
        else
        {
          if (dataContext.IsSuccessSelectedPoint || !dataContext.IsExitApp)
            return;
          if (MessageBoxHelper.Question(Translate.SplashScreenViewModel_Вы_уверены__что_хотите_закрыть_программу_) == MessageBoxResult.No)
          {
            e.Cancel = true;
          }
          else
          {
            Gbs.Helpers.Other.SetCorrectExit();
            System.Environment.Exit(0);
          }
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при закрытии формы выбора торговой точки");
      }
    }

    private void FrmSelectPoint_OnLoaded(object sender, RoutedEventArgs e)
    {
      DataBaseHelper.CloseConnection();
      Gbs.Helpers.ControlsHelpers.DataGrid.Other.SelectFirstRow((object) this.ListPoint);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/homeoffice/frmselectpoint.xaml", UriKind.Relative));
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
        this.ListPoint = (System.Windows.Controls.DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
