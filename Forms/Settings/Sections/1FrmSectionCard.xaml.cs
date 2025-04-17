// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Sections.FrmSectionCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.Sections
{
  public class FrmSectionCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    private bool _contentLoaded;

    public FrmSectionCard() => this.InitializeComponent();

    public bool ShowCard(Guid selectionUid, out Gbs.Core.Entities.Sections.Section section)
    {
      try
      {
        section = selectionUid == Guid.Empty ? new Gbs.Core.Entities.Sections.Section() : Gbs.Core.Entities.Sections.GetSectionByUid(selectionUid);
        SectionCardModelView myViewModel = new SectionCardModelView(section)
        {
          CloseCardAction = new Action(((Window) this).Close)
        };
        this.DataContext = (object) myViewModel;
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            myViewModel.SaveSelectoinCommand
          },
          {
            hotKeys.CancelAction,
            (ICommand) new RelayCommand((Action<object>) (obj => myViewModel.CloseCardAction()))
          }
        };
        this.ShowDialog();
        return myViewModel.SaveResult;
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме карточки секции");
        section = (Gbs.Core.Entities.Sections.Section) null;
        return false;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/sections/frmsectioncard.xaml", UriKind.Relative));
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
