// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.PermissionRules
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Db;
using Gbs.Core.Entities.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class PermissionRules
  {
    public static List<PermissionRules.PermissionRule> GetPermissionList(IQueryable<SETTINGS> query)
    {
      return query.ToList<SETTINGS>().Select<SETTINGS, PermissionRules.PermissionRule>((Func<SETTINGS, PermissionRules.PermissionRule>) (p =>
      {
        return new PermissionRules.PermissionRule()
        {
          Uid = p.UID,
          Action = (Actions) int.Parse(p.PARAM),
          GroupUid = p.ENTITY_UID,
          IsGranted = JsonConvert.DeserializeObject<bool>(p.VAL)
        };
      })).ToList<PermissionRules.PermissionRule>();
    }

    public class PermissionRule : Entity
    {
      [Required]
      public Guid GroupUid { get; set; }

      [Required]
      public Actions Action { get; set; }

      public bool IsGranted { get; set; }

      public new bool IsDeleted { get; }

      public ActionResult VerifyBeforeSave() => this.DataValidation();

      public bool Save()
      {
        return new SettingsRepository().Save(new Setting()
        {
          Type = Types.UserAccessRules,
          EntityUid = this.GroupUid,
          Parameter = ((int) this.Action).ToString(),
          Value = (object) this.IsGranted
        });
      }
    }
  }
}
