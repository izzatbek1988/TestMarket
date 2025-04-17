// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.UserControls.ClientSelectionControl
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;
using Gbs.Forms.Clients;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Helpers.UserControls
{
  public partial class ClientSelectionControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty ClientNameProperty = DependencyProperty.Register(nameof (ClientName), typeof (string), typeof (ClientSelectionControl), new PropertyMetadata((object) Translate.MainWindowViewModel_Выберите_клиента));
    public static readonly DependencyProperty ClientProperty = DependencyProperty.Register(nameof (Client), typeof (Client), typeof (ClientSelectionControl), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsCheckedClientProperty = DependencyProperty.Register(nameof (IsCheckedClient), typeof (bool), typeof (ClientSelectionControl), new PropertyMetadata((object) false));
    internal CheckBox ClientCheckBox;
    private bool _contentLoaded;

    public string ClientName
    {
      get => (string) this.GetValue(ClientSelectionControl.ClientNameProperty);
      set => this.SetValue(ClientSelectionControl.ClientNameProperty, (object) value);
    }

    public Client Client
    {
      get => (Client) this.GetValue(ClientSelectionControl.ClientProperty);
      set => this.SetValue(ClientSelectionControl.ClientProperty, (object) value);
    }

    public bool IsCheckedClient
    {
      get
      {
        this.ClientName = (bool) this.GetValue(ClientSelectionControl.IsCheckedClientProperty) ? this.Client?.Name : Translate.MainWindowViewModel_Выберите_клиента;
        return (bool) this.GetValue(ClientSelectionControl.IsCheckedClientProperty);
      }
      set
      {
        this.ClientName = value ? this.Client?.Name : Translate.MainWindowViewModel_Выберите_клиента;
        this.SetValue(ClientSelectionControl.IsCheckedClientProperty, (object) value);
      }
    }

    public ICommand GetClientCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (!this.IsCheckedClient)
          {
            this.Client = (Client) null;
          }
          else
          {
            (Client client, bool result) client1 = new FrmSearchClient().GetClient();
            Client client2 = client1.client;
            if (!client1.result)
            {
              this.IsCheckedClient = false;
            }
            else
            {
              this.Client = client2;
              this.ClientName = client2?.Name ?? Translate.MainWindowViewModel_Выберите_клиента;
              this.IsCheckedClient = this.Client != null;
            }
          }
        }));
      }
    }

    public ClientSelectionControl() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/helpers/usercontrols/clientselectioncontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.ClientCheckBox = (CheckBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
