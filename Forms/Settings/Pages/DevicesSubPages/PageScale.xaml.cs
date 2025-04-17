// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.DevicesSubPages.ScaleSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.Scales;
using Gbs.Core.Devices.ScalesWIthLabels;
using Gbs.Core.Entities;
using Gbs.Forms.Settings.Devices;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages.DevicesSubPages
{
  public partial class ScaleSettingViewModel : ViewModel
  {
    public void LoadingPlu()
    {
      List<EntityProperties.PropertyType> first = new List<EntityProperties.PropertyType>();
      EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
      propertyType.Name = Translate.GlobalDictionaries_Не_указано;
      propertyType.Uid = Guid.Empty;
      first.Add(propertyType);
      this.ListPlu = first.Concat<EntityProperties.PropertyType>(EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good, false).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
      {
        if (x.Type != GlobalDictionaries.EntityPropertyTypes.Integer)
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid);
      }))).ToList<EntityProperties.PropertyType>();
      this.ComboBoxPluSelectedUid = this.ListPlu.Find((Predicate<EntityProperties.PropertyType>) (x => x.Uid == this.DevicesConfig.ScaleWithLable.PluUid));
      this.OnPropertyChanged("ListPlu");
    }

    public EntityProperties.PropertyType ComboBoxPluSelectedUid
    {
      get
      {
        List<EntityProperties.PropertyType> listPlu = this.ListPlu;
        return (listPlu != null ? listPlu.SingleOrDefault<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => x.Uid == this.DevicesConfig.ScaleWithLable.PluUid)) : (EntityProperties.PropertyType) null) ?? new EntityProperties.PropertyType();
      }
      set
      {
        this.DevicesConfig.ScaleWithLable.PluUid = value != null ? __nonvirtual (value.Uid) : Guid.Empty;
        this.OnPropertyChanged(nameof (ComboBoxPluSelectedUid));
      }
    }

    public List<EntityProperties.PropertyType> ListPlu { get; set; }

    public ScaleSettingViewModel()
    {
    }

    public ScaleSettingViewModel(Gbs.Core.Config.Devices devicesConfig, ComboBox box)
    {
      this.DevicesConfig = devicesConfig;
      this.ComboBoxPlu = box;
      this.LoadingPlu();
    }

    public ICommand TestConnectionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj => new ScalesTestViewModel().TestScales(this.DevicesConfig)));
      }
    }

    public Visibility VisibilityButtonShowDriverForScales
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        int num;
        if (devicesConfig == null)
          num = 0;
        else
          num = devicesConfig.Scale.Type.IsEither<GlobalDictionaries.Devices.ScaleTypes>(GlobalDictionaries.Devices.ScaleTypes.Bta, GlobalDictionaries.Devices.ScaleTypes.Wintec) ? 1 : 0;
        return num == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityButtonShowDriverForLableScales
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        int num;
        if (devicesConfig == null)
          num = 0;
        else
          num = devicesConfig.ScaleWithLable.Type.IsEither<GlobalDictionaries.Devices.ScaleLableTypes>(GlobalDictionaries.Devices.ScaleLableTypes.Rongta, GlobalDictionaries.Devices.ScaleLableTypes.MettlerToledo) ? 1 : 0;
        return num == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility VisibilityConfigForRongta
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        int num;
        if (devicesConfig == null)
          num = 0;
        else
          num = devicesConfig.ScaleWithLable.Type.IsEither<GlobalDictionaries.Devices.ScaleLableTypes>(GlobalDictionaries.Devices.ScaleLableTypes.Rongta) ? 1 : 0;
        return num == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public ICommand ShowFolderDriver
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          string path2 = "dll\\scale\\";
          string str;
          switch (this.DevicesConfig.Scale.Type)
          {
            case GlobalDictionaries.Devices.ScaleTypes.Wintec:
              str = "wintec";
              break;
            case GlobalDictionaries.Devices.ScaleTypes.Bta:
              str = "bta";
              break;
            default:
              str = string.Empty;
              break;
          }
          string path3 = str;
          if (path3 == string.Empty)
            return;
          FileSystemHelper.ShowFolderDriver(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, path2, path3));
        }));
      }
    }

    public ICommand ShowFolderDriverLableScales
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          switch (this.DevicesConfig.ScaleWithLable.Type)
          {
            case GlobalDictionaries.Devices.ScaleLableTypes.None:
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.ShtrihM:
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.Cas:
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.Rongta:
              FileSystemHelper.ShowFolderDriver(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\rongta"));
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.MettlerToledo:
              FileSystemHelper.ShowFolderDriver(Path.Combine(ApplicationInfo.GetInstance().Paths.ApplicationPath, "dll\\label_scale\\mettler"));
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.ScaleManager:
              break;
            case GlobalDictionaries.Devices.ScaleLableTypes.ShtrihM200:
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }));
      }
    }

    public Gbs.Core.Config.Devices DevicesConfig { get; set; }

    public Dictionary<GlobalDictionaries.Devices.ScaleTypes, string> ScalesTypes { get; set; } = GlobalDictionaries.Devices.ScaleTypesDictionary();

    public Visibility ConnectionsConfigScaleVisible
    {
      get
      {
        return this.ScaleType == GlobalDictionaries.Devices.ScaleTypes.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public GlobalDictionaries.Devices.ScaleTypes ScaleType
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        return devicesConfig == null ? GlobalDictionaries.Devices.ScaleTypes.None : devicesConfig.Scale.Type;
      }
      set
      {
        this.DevicesConfig.Scale.Type = value;
        this.OnPropertyChanged(nameof (ScaleType));
        this.OnPropertyChanged("ConnectionsConfigScaleVisible");
        this.OnPropertyChanged("VisibilityButtonShowDriverForScales");
      }
    }

    public ICommand ShowScaleSetting
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (ScalesHelper scalesHelper = new ScalesHelper((IConfig) this.DevicesConfig))
            scalesHelper.ShowProperties();
        }));
      }
    }

    public Dictionary<GlobalDictionaries.Devices.ScaleLableTypes, string> ScalesLableTypes { get; set; } = GlobalDictionaries.Devices.ScaleLableTypesDictionary();

    public Dictionary<GlobalDictionaries.Devices.CasScaleLableTypes, string> CasScalesLableTypes { get; set; } = GlobalDictionaries.Devices.CasScaleLableTypesDictionary();

    public Visibility VisibilityCasType
    {
      get
      {
        return this.ScaleLableType != GlobalDictionaries.Devices.ScaleLableTypes.Cas ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility ConnectionsConfigScaleLableVisible
    {
      get
      {
        return this.ScaleLableType == GlobalDictionaries.Devices.ScaleLableTypes.None ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public GlobalDictionaries.Devices.ScaleLableTypes ScaleLableType
    {
      get
      {
        Gbs.Core.Config.Devices devicesConfig = this.DevicesConfig;
        return devicesConfig == null ? GlobalDictionaries.Devices.ScaleLableTypes.None : devicesConfig.ScaleWithLable.Type;
      }
      set
      {
        this.DevicesConfig.ScaleWithLable.Type = value;
        this.OnPropertyChanged(nameof (ScaleLableType));
        this.OnPropertyChanged("ConnectionsConfigScaleLableVisible");
        this.OnPropertyChanged("VisibilityButtonShowDriverForLableScales");
        this.OnPropertyChanged("VisibilityCasType");
        this.OnPropertyChanged("VisibilityConfigForRongta");
      }
    }

    public ICommand ShowScaleLableSetting
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          using (ScalesWIthLabelsHelper withLabelsHelper = new ScalesWIthLabelsHelper((IConfig) this.DevicesConfig))
            withLabelsHelper.ShowProperties();
        }));
      }
    }

    public ComboBox ComboBoxPlu { get; set; }
  }
}
