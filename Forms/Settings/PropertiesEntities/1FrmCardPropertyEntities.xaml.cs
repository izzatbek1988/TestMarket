// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PropertiesEntities.FrmCardProperty
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Settings.PropertiesEntities
{
  public class FrmCardProperty : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    private bool _contentLoaded;

    public FrmCardProperty() => this.InitializeComponent();

    public bool ShowCard(
      Guid propertyUid,
      out EntityProperties.PropertyType property,
      GlobalDictionaries.EntityTypes entityType,
      bool isEnabledType = true)
    {
      ref EntityProperties.PropertyType local = ref property;
      EntityProperties.PropertyType propertyType;
      if (!(propertyUid == Guid.Empty))
      {
        propertyType = EntityProperties.GetTypesList(entityType).First<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == propertyUid));
      }
      else
      {
        propertyType = new EntityProperties.PropertyType();
        propertyType.EntityType = entityType;
      }
      local = propertyType;
      PropertyCardViewModel modelView = new PropertyCardViewModel(entityType)
      {
        PropertyType = property,
        IsEnabledType = isEnabledType,
        Close = new Action(((Window) this).Close),
        Uid = property.Uid.ToString()
      };
      this.DataContext = (object) modelView;
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          hotKeys.OkAction,
          modelView.SaveProp
        },
        {
          hotKeys.CancelAction,
          (ICommand) new RelayCommand((Action<object>) (obj => modelView.Close()))
        }
      };
      this.ShowDialog();
      return modelView.SaveResult;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(this.FindResource((object) PropertyCardViewModel.MenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) (sender as Button);
      resource.IsOpen = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/settings/propertiesentities/frmcardpropertyentities.xaml", UriKind.Relative));
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
      if (connectionId != 1)
      {
        if (connectionId == 2)
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClick);
        else
          this._contentLoaded = true;
      }
      else
        this.TextBoxName = (TextBox) target;
    }
  }
}
