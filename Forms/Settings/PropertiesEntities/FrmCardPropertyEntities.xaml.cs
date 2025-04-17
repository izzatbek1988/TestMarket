// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PropertiesEntities.PropertyCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings.PropertiesEntities
{
  public partial class PropertyCardViewModel : ViewModelWithForm
  {
    public static string MenuKey => nameof (MenuKey);

    public ICommand CopyUidCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => Clipboard.SetText(this.Uid)));
    }

    public ICommand GenerateUidCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.Uid = Guid.NewGuid().ToString();
          this.OnPropertyChanged("Uid");
        }));
      }
    }

    public ICommand EditUidCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          this.IsReadOnlyUid = false;
          this.VisibilityEditItemMenu = Visibility.Collapsed;
          this.OnPropertyChanged("IsReadOnlyUid");
          this.OnPropertyChanged("VisibilityEditItemMenu");
        }));
      }
    }

    public Visibility VisibilityEditItemMenu { get; set; }

    public bool IsReadOnlyUid { get; set; } = true;

    public bool SaveResult { get; set; }

    public EntityProperties.PropertyType PropertyType { get; set; }

    public Dictionary<GlobalDictionaries.EntityPropertyTypes, string> Types { get; } = GlobalDictionaries.PropertyDictionary();

    public bool IsEnabledType { get; set; }

    public ICommand SaveProp { get; set; }

    public Action Close { get; set; }

    public PropertyCardViewModel()
    {
    }

    public PropertyCardViewModel(GlobalDictionaries.EntityTypes entityType)
    {
      this.PropertyType = new EntityProperties.PropertyType()
      {
        EntityType = entityType
      };
      this.SaveProp = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
    }

    public void Save()
    {
      Guid uid;
      if (!Guid.TryParse(this.Uid, out uid))
        MessageBoxHelper.Warning(Translate.PropertyCardViewModel_Save_Некорректно_указан_уникальный_идентификатор__UID__доп__поля__Необходимо_указать_идентифакатор_в_формате_xxxxxxxx_xxxx_xxxx_xxxx_xxxxxxxxxxxx);
      else if (this.PropertyType.Uid != uid && EntityProperties.GetCountTypesByUid(uid) > 0)
      {
        MessageBoxHelper.Warning(Translate.PropertyCardViewModel_Save_Указанный_идентификатор__UID__доп__поля_уже_существует__Необходимо_указать_новый_идентифакатор_в_формате_xxxxxxxx_xxxx_xxxx_xxxx_xxxxxxxxxxxx__которого_еще_нет_в_базе_);
      }
      else
      {
        ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.PropertyCardViewModel_Save_Сохранение_дополнительного_поля);
        this.SaveResult = this.PropertyType.Save();
        if (!this.SaveResult)
        {
          progressBar.Close();
        }
        else
        {
          if (this.PropertyType.Uid != uid)
          {
            using (DataBase db = Gbs.Core.Data.GetDataBase())
            {
              DataConnectionTransaction connectionTransaction = db.BeginTransaction();
              try
              {
                db.GetTable<ENTITY_PROPERTIES_VALUES>().Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.TYPE_UID == this.PropertyType.Uid)).ToList<ENTITY_PROPERTIES_VALUES>().ForEach((Action<ENTITY_PROPERTIES_VALUES>) (x =>
                {
                  x.TYPE_UID = uid;
                  db.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(x);
                }));
                ENTITY_PROPERTIES_TYPES entityPropertiesTypes = db.GetTable<ENTITY_PROPERTIES_TYPES>().Single<ENTITY_PROPERTIES_TYPES>((Expression<Func<ENTITY_PROPERTIES_TYPES, bool>>) (x => x.UID == this.PropertyType.Uid));
                entityPropertiesTypes.UID = uid;
                db.InsertOrReplace<ENTITY_PROPERTIES_TYPES>(entityPropertiesTypes);
                db.GetTable<ENTITY_PROPERTIES_TYPES>().Delete<ENTITY_PROPERTIES_TYPES>((Expression<Func<ENTITY_PROPERTIES_TYPES, bool>>) (x => x.UID == this.PropertyType.Uid));
                this.PropertyType.Uid = uid;
                connectionTransaction.Commit();
              }
              catch
              {
                progressBar.Close();
                connectionTransaction.Rollback();
                throw;
              }
            }
          }
          progressBar.Close();
          this.Close();
        }
      }
    }

    public string Uid { get; set; }
  }
}
