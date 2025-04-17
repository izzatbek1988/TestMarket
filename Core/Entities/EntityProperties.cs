// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.EntityProperties
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Helpers;
using Gbs.Helpers.Extensions.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

#nullable enable
namespace Gbs.Core.Entities
{
  public static class EntityProperties
  {
    public static 
    #nullable disable
    string GetStringFormat(EntityProperties.PropertyType propertyType)
    {
      switch (propertyType.Type)
      {
        case GlobalDictionaries.EntityPropertyTypes.Text:
          return "";
        case GlobalDictionaries.EntityPropertyTypes.Integer:
          return "N0";
        case GlobalDictionaries.EntityPropertyTypes.Decimal:
          return "";
        case GlobalDictionaries.EntityPropertyTypes.DateTime:
          return "dd.MM.yyyy";
        case GlobalDictionaries.EntityPropertyTypes.AutoNum:
          return "";
        default:
          return "";
      }
    }

    public static int GetCountTypesByUid(Guid uid)
    {
      using (DataBase dataBase = Data.GetDataBase())
        return dataBase.GetTable<ENTITY_PROPERTIES_TYPES>().Count<ENTITY_PROPERTIES_TYPES>((Expression<Func<ENTITY_PROPERTIES_TYPES, bool>>) (x => x.UID == uid));
    }

    public static List<EntityProperties.PropertyValue> GetValuesList(
      GlobalDictionaries.EntityTypes entityType,
      IQueryable<ENTITY_PROPERTIES_VALUES> query = null)
    {
      return EntityProperties.GetData(entityType, query);
    }

    private static List<EntityProperties.PropertyValue> GetData(
      GlobalDictionaries.EntityTypes entityType,
      IQueryable<ENTITY_PROPERTIES_VALUES> query)
    {
      Performancer performancer = new Performancer("Загрузка значений доп. полей с запросом для " + entityType.ToString());
      using (DataBase dataBase = Data.GetDataBase())
      {
        if (query == null)
          query = dataBase.GetTable<ENTITY_PROPERTIES_VALUES>();
        IQueryable<ENTITY_PROPERTIES_TYPES> typeTable = dataBase.GetTable<ENTITY_PROPERTIES_TYPES>().Where<ENTITY_PROPERTIES_TYPES>((Expression<Func<ENTITY_PROPERTIES_TYPES, bool>>) (type => type.IS_DELETED == false && type.ENTITY_TYPE == (int) entityType));
        List<ENTITY_PROPERTIES_VALUES> list1 = query.Where<ENTITY_PROPERTIES_VALUES>((Expression<Func<ENTITY_PROPERTIES_VALUES, bool>>) (x => x.IS_DELETED == false)).SelectMany((Expression<Func<ENTITY_PROPERTIES_VALUES, IEnumerable<ENTITY_PROPERTIES_TYPES>>>) (v => typeTable), (v, t) => new
        {
          v = v,
          t = t
        }).Where(data => data.t.UID == data.v.TYPE_UID).Select(data => data.v).ToList<ENTITY_PROPERTIES_VALUES>();
        performancer.AddPoint("Запрос к БД значений. Загружено: " + list1.Count.ToString());
        List<ENTITY_PROPERTIES_VALUES> list2 = list1.DistinctBy<ENTITY_PROPERTIES_VALUES, Guid>((Func<ENTITY_PROPERTIES_VALUES, Guid>) (x => x.UID)).ToList<ENTITY_PROPERTIES_VALUES>();
        List<EntityProperties.PropertyType> list3 = EntityProperties.GetTypesList(entityType).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x => !x.IsDeleted)).ToList<EntityProperties.PropertyType>();
        performancer.AddPoint("Загрузка типов");
        List<EntityProperties.PropertyValue> list4 = list2.Join<ENTITY_PROPERTIES_VALUES, EntityProperties.PropertyType, Guid, EntityProperties.PropertyValue>((IEnumerable<EntityProperties.PropertyType>) list3, (Func<ENTITY_PROPERTIES_VALUES, Guid>) (v => v.TYPE_UID), (Func<EntityProperties.PropertyType, Guid>) (t => t.Uid), (Func<ENTITY_PROPERTIES_VALUES, EntityProperties.PropertyType, EntityProperties.PropertyValue>) ((v, t) =>
        {
          return new EntityProperties.PropertyValue()
          {
            EntityUid = v.ENTITY_UID,
            Uid = v.UID,
            IsDeleted = v.IS_DELETED,
            Type = t,
            Value = EntityProperties.ConvertStringToPropertyValue(v.CONTENT, t.Type)
          };
        })).AsParallel<EntityProperties.PropertyValue>().ToList<EntityProperties.PropertyValue>();
        performancer.AddPoint("Объединение таблиц");
        performancer.Stop();
        return list4;
      }
    }

    public static object ConvertStringToPropertyValue(
      string value,
      GlobalDictionaries.EntityPropertyTypes type)
    {
      JsonSerializerSettings original = new JsonSerializerSettings()
      {
        Error = (EventHandler<ErrorEventArgs>) ((sender, error) => error.ErrorContext.Handled = true)
      };
      switch (type)
      {
        case GlobalDictionaries.EntityPropertyTypes.Text:
        case GlobalDictionaries.EntityPropertyTypes.Decimal:
        case GlobalDictionaries.EntityPropertyTypes.AutoNum:
        case GlobalDictionaries.EntityPropertyTypes.System:
          return JsonConvert.DeserializeObject(value);
        case GlobalDictionaries.EntityPropertyTypes.Integer:
          return (object) JsonConvert.DeserializeObject<long>(value);
        case GlobalDictionaries.EntityPropertyTypes.DateTime:
          string str = value;
          JsonSerializerSettings settings = new JsonSerializerSettings(original);
          settings.Converters.Add((JsonConverter) new IsoDateTimeConverter());
          return (object) JsonConvert.DeserializeObject<DateTime>(str, settings);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public static List<EntityProperties.PropertyType> GetTypesList(
      GlobalDictionaries.EntityTypes entityType,
      bool allType = true)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        IQueryable<ENTITY_PROPERTIES_TYPES> source = dataBase.GetTable<ENTITY_PROPERTIES_TYPES>().Where<ENTITY_PROPERTIES_TYPES>((Expression<Func<ENTITY_PROPERTIES_TYPES, bool>>) (x => x.ENTITY_TYPE == (int) entityType));
        if (!allType)
          source = source.Where<ENTITY_PROPERTIES_TYPES>((Expression<Func<ENTITY_PROPERTIES_TYPES, bool>>) (x => !x.IS_DELETED));
        return source.ToList<ENTITY_PROPERTIES_TYPES>().Select<ENTITY_PROPERTIES_TYPES, EntityProperties.PropertyType>((Func<ENTITY_PROPERTIES_TYPES, EntityProperties.PropertyType>) (t =>
        {
          return new EntityProperties.PropertyType()
          {
            Uid = t.UID,
            IsDeleted = t.IS_DELETED,
            Name = t.NAME,
            Type = (GlobalDictionaries.EntityPropertyTypes) t.TYPE,
            EntityType = (GlobalDictionaries.EntityTypes) t.ENTITY_TYPE
          };
        })).OrderBy<EntityProperties.PropertyType, string>((Func<EntityProperties.PropertyType, string>) (x => x.Name)).ToList<EntityProperties.PropertyType>();
      }
    }

    public class PropertyValue : Entity
    {
      [Required]
      public EntityProperties.PropertyType Type { get; set; }

      [Required]
      public Guid EntityUid { get; set; }

      [StringLength(950)]
      public object Value { get; set; }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool Save()
      {
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        Other.ConsoleWrite("value: " + this.Value?.ToString());
        string str = JsonConvert.ToString(this.Value);
        using (DataBase dataBase = Data.GetDataBase())
        {
          dataBase.InsertOrReplace<ENTITY_PROPERTIES_VALUES>(new ENTITY_PROPERTIES_VALUES()
          {
            UID = this.Uid,
            IS_DELETED = this.IsDeleted,
            ENTITY_UID = this.EntityUid,
            TYPE_UID = this.Type.Uid,
            CONTENT = str
          });
          return true;
        }
      }
    }

    public class PropertyType : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Name { get; set; } = string.Empty;

      public GlobalDictionaries.EntityTypes EntityType { get; set; }

      public GlobalDictionaries.EntityPropertyTypes Type { get; set; }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool Save()
      {
        if (this.Name.Length > 30)
          this.Name = this.Name.Substring(0, 30);
        if (this.VerifyBeforeSave().Result == ActionResult.Results.Error)
          return false;
        using (DataBase dataBase = Data.GetDataBase())
        {
          dataBase.InsertOrReplace<ENTITY_PROPERTIES_TYPES>(new ENTITY_PROPERTIES_TYPES()
          {
            UID = this.Uid,
            IS_DELETED = this.IsDeleted,
            NAME = this.Name,
            ENTITY_TYPE = (int) this.EntityType,
            TYPE = (int) this.Type
          });
          return true;
        }
      }
    }
  }
}
