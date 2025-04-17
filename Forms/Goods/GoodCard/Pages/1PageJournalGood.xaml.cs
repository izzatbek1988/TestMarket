// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.GoodCard.PageJournalGood
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods.GoodCard
{
  public class PageJournalGood : Page, IComponentConnector
  {
    internal ComboBox cbJornal;
    internal DataGrid DataGridJournal;
    private bool _contentLoaded;

    public PageJournalGood(
      Gbs.Core.Entities.Goods.Good good,
      List<Document> docs,
      Users.User user,
      BuyPriceCounter counter)
    {
      try
      {
        this.InitializeComponent();
        this.DataGridParent = this.DataGridJournal;
        this.DataContext = (object) new JournalGoodViewModel(good, docs, user, counter);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при загрузке журнала товара");
      }
    }

    public DataGrid DataGridParent { get; set; }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/goodcard/pages/pagejournalgood.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.DataGridJournal = (DataGrid) target;
        else
          this._contentLoaded = true;
      }
      else
        this.cbJornal = (ComboBox) target;
    }
  }
}
