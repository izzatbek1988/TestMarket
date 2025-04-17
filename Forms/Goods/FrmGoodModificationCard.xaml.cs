// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.FrmGoodModificationCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods
{
  public partial class FrmGoodModificationCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    private bool _contentLoaded;

    private GoodModificationViewModel MyViewModel { get; set; }

    public FrmGoodModificationCard() => this.InitializeComponent();

    public (bool, GoodsModifications.GoodModification) ShowCard(
      GoodsModifications.GoodModification modification)
    {
      try
      {
        this.MyViewModel = new GoodModificationViewModel((modification != null ? modification.Clone<GoodsModifications.GoodModification>() : (GoodsModifications.GoodModification) null) ?? new GoodsModifications.GoodModification())
        {
          CloseFrm = new Action(((Window) this).Close)
        };
        this.DataContext = (object) this.MyViewModel;
        this.ShowDialog();
        return this.MyViewModel.Result;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в карточке модификации");
        return (false, (GoodsModifications.GoodModification) null);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/frmgoodmodificationcard.xaml", UriKind.Relative));
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
        this.TextBoxName = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
