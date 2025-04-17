// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.Pages.OtherSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Helpers.Performance;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.Pages
{
  public partial class OtherSettingViewModel : ViewModel
  {
    public List<string> Separators
    {
      get => new List<string>() { ";", "," };
    }

    public Visibility VisibilityThumbprint
    {
      get
      {
        Gbs.Core.Config.Settings settings = this.Settings;
        return (settings != null ? (settings.Interface?.Country.GetValueOrDefault() == GlobalDictionaries.Countries.Russia ? 1 : 0) : 0) == 0 ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Gbs.Core.Config.Settings Settings { get; set; }

    public Dictionary<GlobalDictionaries.VersionUpdate, string> VersionUpdateDictionary
    {
      get => GlobalDictionaries.GetVersionUpdateDictionary;
    }

    public Dictionary<OtherConfig.UpdateType, string> UpdateTypeDictionary { get; set; } = new Dictionary<OtherConfig.UpdateType, string>()
    {
      {
        OtherConfig.UpdateType.AutoUpdate,
        Translate.АвтоматическиРекомендуется
      },
      {
        OtherConfig.UpdateType.UpdateNotification,
        Translate.УведомлятьОбОбновлениях
      },
      {
        OtherConfig.UpdateType.NoUpdate,
        Translate.Отключено
      }
    };

    public List<OtherSettingViewModel.CertificateView> CertificateList { get; set; } = new List<OtherSettingViewModel.CertificateView>()
    {
      new OtherSettingViewModel.CertificateView()
      {
        FriendlyName = "Нет",
        Thumbprint = ""
      }
    };

    public string SelectedThumbprintCertificate
    {
      get
      {
        return this.CertificateList.FirstOrDefault<OtherSettingViewModel.CertificateView>((Func<OtherSettingViewModel.CertificateView, bool>) (x => x.Thumbprint == this.Settings?.Other.Thumbprint))?.Thumbprint ?? "";
      }
      set
      {
        this.Settings.Other.Thumbprint = value;
        this.OnPropertyChanged(nameof (SelectedThumbprintCertificate));
      }
    }

    public OtherConfig.UpdateType UpdateType
    {
      get
      {
        Gbs.Core.Config.Settings settings = this.Settings;
        return settings == null ? OtherConfig.UpdateType.AutoUpdate : settings.Other.UpdateConfig.UpdateType;
      }
      set
      {
        this.Settings.Other.UpdateConfig.UpdateType = value;
        this.OnPropertyChanged("VersionUpdateVisibility");
      }
    }

    public Visibility VersionUpdateVisibility
    {
      get
      {
        return this.UpdateType != OtherConfig.UpdateType.NoUpdate ? Visibility.Visible : Visibility.Collapsed;
      }
    }

    [Localizable(false)]
    public ICommand CheckPerformanceCommand
    {
      get
      {
        int num;
        return (ICommand) new RelayCommand((Action<object>) (o => num = (int) MessageBoxHelper.Show(HardwarePerformanceHelper.GetPerformanceInfo())));
      }
    }

    public OtherSettingViewModel()
    {
      X509Store x509Store = new X509Store();
      x509Store.Open(OpenFlags.ReadOnly);
      foreach (X509Certificate2 certificate in x509Store.Certificates)
      {
        string str1 = certificate.SubjectName.Name ?? "";
        int startIndex = str1.IndexOf(',');
        if (startIndex != -1)
          str1 = str1?.Remove(startIndex);
        string[] strArray = new string[5]
        {
          str1,
          ", ",
          null,
          null,
          null
        };
        DateTime dateTime = certificate.NotBefore;
        strArray[2] = dateTime.ToString("d");
        strArray[3] = " - ";
        dateTime = certificate.NotAfter;
        strArray[4] = dateTime.ToString("d");
        string str2 = string.Concat(strArray);
        this.CertificateList.Add(new OtherSettingViewModel.CertificateView()
        {
          FriendlyName = str2,
          Thumbprint = certificate.Thumbprint
        });
      }
      x509Store.Close();
      this.OnPropertyChanged(nameof (CertificateList));
    }

    public class CertificateView
    {
      public string Thumbprint { get; set; }

      public string FriendlyName { get; set; }
    }
  }
}
