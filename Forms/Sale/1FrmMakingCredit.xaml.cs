// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Sale.FrmMakingCredit
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Clients;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Sale
{
  public class FrmMakingCredit : WindowWithSize, IComponentConnector
  {
    private CreditViewModel Model;
    private bool _contentLoaded;

    public FrmMakingCredit() => this.InitializeComponent();

    public (bool result, Client client, string comment) GetCredit(Decimal sum, Client client = null)
    {
      this.Model = new CreditViewModel()
      {
        Client = client,
        SumSale = sum,
        Close = new Action(((Window) this).Close),
        IsEnabled = client == null
      };
      this.DataContext = (object) this.Model;
      this.SetHotKeys();
      this.ShowDialog();
      if (this.Model.Result)
        CacheHelper.UpdateCacheAsync(CacheHelper.CacheTypes.ClientsCredits);
      return (this.Model.Result, this.Model.Client, this.Model.Comment);
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        RelayCommand relayCommand1 = new RelayCommand((Action<object>) (o => this.Close()));
        RelayCommand relayCommand2 = new RelayCommand((Action<object>) (o =>
        {
          if (!this.Model.IsEnabled)
            return;
          this.Model.SelectClient.Execute((object) null);
        }));
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            this.Model.SaveCredit
          },
          {
            hotKeys.CancelAction,
            (ICommand) relayCommand1
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            (ICommand) relayCommand1
          },
          {
            hotKeys.SelectClient,
            (ICommand) relayCommand2
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/sale/frmmakingcredit.xaml", UriKind.Relative));
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
