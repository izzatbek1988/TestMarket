// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PropertiesEntities.PropertiesListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.PropertiesEntities
{
  public class PropertiesListViewModel
  {
    public ObservableCollection<PropertiesListViewModel.PropertyView> PropertyList { get; set; }

    public EntityProperties.PropertyType SelectedProperty { get; set; }

    public ICommand AddCommand { get; set; }

    public ICommand EditCommand { get; set; }

    public ICommand DeleteCommand { get; set; }

    private Dictionary<GlobalDictionaries.EntityPropertyTypes, string> DictionaryType { get; } = GlobalDictionaries.PropertyDictionary();

    public PropertiesListViewModel()
    {
    }

    public PropertiesListViewModel(GlobalDictionaries.EntityTypes entityType)
    {
      PropertiesListViewModel propertiesListViewModel = this;
      this.PropertyList = new ObservableCollection<PropertiesListViewModel.PropertyView>((IEnumerable<PropertiesListViewModel.PropertyView>) EntityProperties.GetTypesList(entityType).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
      {
        if (x.IsDeleted || x.Type == GlobalDictionaries.EntityPropertyTypes.System)
          return false;
        return !x.Uid.IsEither<Guid>(GlobalDictionaries.CertificateNominalUid, GlobalDictionaries.CertificateReusableUid, GlobalDictionaries.GoodIdUid, GlobalDictionaries.NumTableUid, GlobalDictionaries.TsdDocumentNumberUid, GlobalDictionaries.CountGuestUid, GlobalDictionaries.FiscalNumUid, GlobalDictionaries.FrNumber, GlobalDictionaries.RrnUid, GlobalDictionaries.RrnSbpUid, GlobalDictionaries.AlcCodeUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid, GlobalDictionaries.ReplayUidEgais, GlobalDictionaries.TTNEgais, GlobalDictionaries.TypeCardMethodUid, GlobalDictionaries.StatusEgais, GlobalDictionaries.TerminalIdPropertyUid, GlobalDictionaries.ReceiptSeqPropertyUid, GlobalDictionaries.DateTimePropertyUid, GlobalDictionaries.FiscalSignPropertyUid, GlobalDictionaries.IkpuUid, GlobalDictionaries.OptionalClientOrderUid, GlobalDictionaries.InfoWithTrueApiUid, GlobalDictionaries.FiasUid);
      })).Select<EntityProperties.PropertyType, PropertiesListViewModel.PropertyView>((Func<EntityProperties.PropertyType, PropertiesListViewModel.PropertyView>) (x => new PropertiesListViewModel.PropertyView()
      {
        PropertyType = x,
        Type = propertiesListViewModel.DictionaryType.Single<KeyValuePair<GlobalDictionaries.EntityPropertyTypes, string>>((Func<KeyValuePair<GlobalDictionaries.EntityPropertyTypes, string>, bool>) (d => d.Key == x.Type)).Value
      })).OrderBy<PropertiesListViewModel.PropertyView, string>((Func<PropertiesListViewModel.PropertyView, string>) (x => x.PropertyType.Name)));
      this.AddCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        EntityProperties.PropertyType property;
        if (!new FrmCardProperty().ShowCard(Guid.Empty, out property, entityType))
          return;
        propertiesListViewModel.PropertyList.Add(new PropertiesListViewModel.PropertyView()
        {
          PropertyType = property,
          Type = propertiesListViewModel.DictionaryType.Single<KeyValuePair<GlobalDictionaries.EntityPropertyTypes, string>>((Func<KeyValuePair<GlobalDictionaries.EntityPropertyTypes, string>, bool>) (x => x.Key == property.Type)).Value
        });
      }));
      this.EditCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (propertiesListViewModel.SelectedProperty != null)
        {
          if (((IEnumerable) obj).Cast<PropertiesListViewModel.PropertyView>().ToList<PropertiesListViewModel.PropertyView>().Count > 1)
            MessageBoxHelper.Warning(Translate.PropertiesListViewModel_Необходимо_выбрать_только_одно_доп__поле_для_редактирования);
          else if (!propertiesListViewModel.IsEditedProp(propertiesListViewModel.SelectedProperty.Uid))
          {
            MessageBoxHelper.Warning(Translate.PropertiesListViewModel_Невозможно_редактировать_данное_поле_);
          }
          else
          {
            Guid oldUid = propertiesListViewModel.SelectedProperty.Uid;
            EntityProperties.PropertyType property;
            if (!new FrmCardProperty().ShowCard(propertiesListViewModel.SelectedProperty.Uid, out property, entityType, false))
              return;
            propertiesListViewModel.PropertyList[propertiesListViewModel.PropertyList.ToList<PropertiesListViewModel.PropertyView>().FindIndex((Predicate<PropertiesListViewModel.PropertyView>) (x => x.PropertyType.Uid == oldUid))] = new PropertiesListViewModel.PropertyView()
            {
              PropertyType = property,
              Type = propertiesListViewModel.DictionaryType.Single<KeyValuePair<GlobalDictionaries.EntityPropertyTypes, string>>((Func<KeyValuePair<GlobalDictionaries.EntityPropertyTypes, string>, bool>) (x => x.Key == property.Type)).Value
            };
          }
        }
        else
        {
          int num = (int) MessageBoxHelper.Show(Translate.PropertiesListViewModel_PropertiesListViewModel_Требуется_выбрать_доп__поле, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
      this.DeleteCommand = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (propertiesListViewModel.SelectedProperty != null)
        {
          List<PropertiesListViewModel.PropertyView> list = ((IEnumerable) obj).Cast<PropertiesListViewModel.PropertyView>().ToList<PropertiesListViewModel.PropertyView>();
          if (MessageBoxHelper.Show(string.Format(Translate.PropertiesListViewModel_PropertiesListViewModel_Вы_уверены__что_Вы_хотите_удалить__0__доп__поля_, (object) list.Count), PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            return;
          foreach (PropertiesListViewModel.PropertyView propertyView in list)
          {
            if (!propertiesListViewModel.IsEditedProp(propertyView.PropertyType.Uid))
            {
              int num = (int) MessageBoxHelper.Show(string.Format(Translate.PropertiesListViewModel_PropertiesListViewModel_Невозможно_удалить_поле__0___оно_является_системным, (object) propertyView.PropertyType.Name), icon: MessageBoxImage.Hand);
            }
            else
            {
              propertyView.PropertyType.IsDeleted = true;
              propertyView.PropertyType.Save();
              propertiesListViewModel.PropertyList.Remove(propertyView);
            }
          }
        }
        else
        {
          int num1 = (int) MessageBoxHelper.Show(Translate.PropertiesListViewModel_PropertiesListViewModel_Требуется_выбрать_доп__поле, PartnersHelper.ProgramName(), icon: MessageBoxImage.Exclamation);
        }
      }));
    }

    private bool IsEditedProp(Guid uid)
    {
      return GlobalDictionaries.RequisitesUidList.All<Guid>((Func<Guid, bool>) (x => x != uid));
    }

    public class PropertyView
    {
      public EntityProperties.PropertyType PropertyType { get; set; }

      public string Type { get; set; }
    }
  }
}
