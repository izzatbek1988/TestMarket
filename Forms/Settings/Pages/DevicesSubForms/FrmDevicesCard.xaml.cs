// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubForms.DevicesCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubForms
{
  public partial class DevicesCardViewModel : ViewModelWithForm
  {
    private bool _isSave;
    private Action<DevicesCardViewModel> SetContentEditPage;

    public Dictionary<IDevice.DeviceTypes, string> DeviceTypes { get; set; } = Device.DictionaryDeviceTypes;

    public Gbs.Core.Config.Settings Settings { get; set; }

    public ICommand CancelCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isSave = false;
          this.CloseAction();
        }));
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this._isSave = true;
          this.CloseAction();
        }));
      }
    }

    public ItemDevice ItemDevice { get; set; }

    public IDevice.DeviceTypes DeviceType
    {
      get
      {
        ItemDevice itemDevice = this.ItemDevice;
        return itemDevice == null ? IDevice.DeviceTypes.Other : itemDevice.Type;
      }
      set
      {
        IDevice.DeviceTypes type = this.ItemDevice.Type;
        if (value == type)
          return;
        this.ItemDevice.Type = value;
        this.SetContentEditPage(this);
      }
    }

    public Page UserPage { get; set; }

    public bool IsEnabledStatus { get; set; }

    public bool ShowCard(bool isNewCard, Gbs.Core.Config.Settings settings, ref ItemDevice item)
    {
      this.IsEnabledStatus = isNewCard;
      this.ItemDevice = !isNewCard ? item.Clone<ItemDevice>() : new ItemDevice();
      this.Settings = settings;
      this.EntityClone = this.ItemDevice.Clone<ItemDevice>();
      this.FormToSHow = (WindowWithSize) new FrmDevicesCard();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      this.SetContentEditPage = new Action<DevicesCardViewModel>(((FrmDevicesCard) this.FormToSHow).SetContentEditPage);
      this.SetContentEditPage(this);
      this.ShowForm();
      if (this._isSave)
      {
        item = this.ItemDevice;
        item.Device = ((FrmDevicesCard) this.FormToSHow).DevicesCnf;
      }
      return this._isSave;
    }

    public ItemDevice EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      return Functions.IsObjectEqual<ItemDevice>(this.EntityClone, this.ItemDevice) || this._isSave;
    }
  }
}
