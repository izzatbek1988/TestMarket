// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.PageDataBase
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class PageDataBase : Page, IComponentConnector
  {
    internal WatermarkPasswordBox PasswordBox;
    internal CheckBox DeleteCheckBox;
    internal Button DeleteButton;
    private bool _contentLoaded;

    public PageDataBase() => this.InitializeComponent();

    public PageDataBase(DataBase confDataBase, Gbs.Core.Entities.Users.User authUser)
    {
      this.InitializeComponent();
      this.DataContext = (object) new DataBasePageViewModel(authUser, confDataBase);
      this.PasswordBox.Password = ((DataBasePageViewModel) this.DataContext).DataBaseConfig.Connection.DecryptedPassword;
      ContextMenu resource = (ContextMenu) this.DeleteButton.FindResource((object) "ContextMenuGrid");
      foreach (DataBasePageViewModel.DeleteTableClass deleteTable in ((DataBasePageViewModel) this.DataContext).DeleteTableDictionary)
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = (object) deleteTable.Name;
        newItem.IsChecked = false;
        newItem.IsCheckable = true;
        newItem.Tag = (object) deleteTable.Type;
        items.Add((object) newItem);
      }
      resource.Closed += new RoutedEventHandler(this.CmButtonOnClosed);
    }

    private void CmButtonOnClosed(object sender, RoutedEventArgs e)
    {
      ((DataBasePageViewModel) this.DataContext).DeleteTableDictionary = new List<DataBasePageViewModel.DeleteTableClass>(((ItemsControl) sender).Items.Cast<MenuItem>().Select<MenuItem, DataBasePageViewModel.DeleteTableClass>((Func<MenuItem, DataBasePageViewModel.DeleteTableClass>) (x => new DataBasePageViewModel.DeleteTableClass()
      {
        Name = x.Header.ToString(),
        Type = (DataBasePageViewModel.TableType) x.Tag,
        IsChecked = x.IsChecked
      })));
    }

    public bool Save() => ((DataBasePageViewModel) this.DataContext).Save();

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      ((DataBasePageViewModel) this.DataContext).DataBaseConfig.Connection.DecryptedPassword = ((WatermarkPasswordBox) sender).Password;
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) sender;
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/pages/pagedatabase.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.PasswordBox = (WatermarkPasswordBox) target;
          this.PasswordBox.PasswordChanged += new RoutedEventHandler(this.PasswordBox_OnPasswordChanged);
          break;
        case 2:
          this.DeleteCheckBox = (CheckBox) target;
          break;
        case 3:
          this.DeleteButton = (Button) target;
          this.DeleteButton.Click += new RoutedEventHandler(this.DeleteButton_OnClick);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
