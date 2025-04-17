// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.FirstSetupViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Forms.Main.FirstSetupPage;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Main
{
  public partial class FirstSetupViewModel : ViewModelWithForm
  {
    private readonly LinkedList<Page> _pagesShop = new LinkedList<Page>();
    private Page _currentPage;

    public Page CurrentPage
    {
      get => this._currentPage;
      set
      {
        this._currentPage = value;
        this.OnPropertyChanged(nameof (CurrentPage));
      }
    }

    public bool ConfirmClose { get; set; }

    public ICommand NextPage { get; set; }

    public ICommand LastPage { get; set; }

    public PageTitleFirstSetup TitlePage { get; set; } = new PageTitleFirstSetup();

    public PageInsertUser UserPage { get; set; } = new PageInsertUser();

    public PageSelectMode SelectModePage { get; set; } = new PageSelectMode();

    public FirstSetupViewModel()
    {
      try
      {
        this.ConfirmClose = true;
        this._pagesShop.AddLast(new LinkedListNode<Page>((Page) this.TitlePage));
        this._pagesShop.AddLast(new LinkedListNode<Page>((Page) this.SelectModePage));
        this._pagesShop.AddLast(new LinkedListNode<Page>((Page) this.UserPage));
        this.CurrentPage = (Page) this.TitlePage;
        this.NextPage = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.CurrentPage == this.TitlePage)
          {
            this.CurrentPage = (Page) this.SelectModePage;
            this.VisibilityCancelButton = Visibility.Visible;
            this.OnPropertyChanged(nameof (VisibilityCancelButton));
          }
          else if (this.CurrentPage == this.SelectModePage)
          {
            (bool, GlobalDictionaries.Mode) modeProgram = ((SelectModeViewModel) this.CurrentPage.DataContext).GetModeProgram();
            if (!modeProgram.Item1)
              return;
            if (modeProgram.Item2 == GlobalDictionaries.Mode.None)
            {
              this.ConfirmClose = false;
              this.CloseAction();
            }
            this.CurrentPage = (Page) this.UserPage;
            ((PageInsertUser) this.CurrentPage).UpdateMode(modeProgram.Item2);
          }
          else
          {
            if (this.CurrentPage != this.UserPage || !this.UserPage.Save())
              return;
            this.ConfirmClose = false;
            this.CloseAction();
          }
        }));
        this.LastPage = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.CurrentPage == this.TitlePage)
            this.CloseAction();
          else if (this.CurrentPage == this.SelectModePage)
          {
            this.CurrentPage = (Page) this.TitlePage;
            this.VisibilityCancelButton = Visibility.Collapsed;
            this.OnPropertyChanged(nameof (VisibilityCancelButton));
          }
          else
          {
            if (this.CurrentPage != this.UserPage)
              return;
            this.CurrentPage = (Page) this.SelectModePage;
          }
        }));
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме первичной настройки");
      }
    }

    public Visibility VisibilityCancelButton { get; set; } = Visibility.Collapsed;
  }
}
