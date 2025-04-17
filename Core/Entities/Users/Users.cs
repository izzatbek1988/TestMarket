// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.Users
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Entities.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class Users
  {
    public class User : Entity
    {
      [Required]
      [StringLength(100, MinimumLength = 3)]
      public string Alias { get; set; } = string.Empty;

      [Required]
      public Client Client { get; set; }

      [Required]
      public UserGroups.UserGroup Group { get; set; }

      [Required]
      [StringLength(30, MinimumLength = 4)]
      public string Password { get; set; } = string.Empty;

      public bool IsKicked { get; set; }

      [Required]
      [Range(7, 30)]
      public Decimal FontSize { get; set; } = 12M;

      public DateTime DateIn { get; set; } = DateTime.Now;

      public DateTime? DateOut { get; set; }

      public string Barcode { get; set; } = string.Empty;

      [StringLength(30)]
      public string LoginForKkm { get; set; } = string.Empty;

      [StringLength(30)]
      public string PasswordForKkm { get; set; } = string.Empty;

      public bool IsOnline
      {
        get => this.OnlineOnSectionUid != Guid.Empty;
        set => this.OnlineOnSectionUid = value ? Sections.GetCurrentSection().Uid : Guid.Empty;
      }

      public Guid OnlineOnSectionUid { get; set; } = Guid.Empty;

      public Users.User Clone()
      {
        Users.User user = this.Clone<Users.User>();
        if (this.Group != null)
          user.Group.Permissions = new List<PermissionRules.PermissionRule>((IEnumerable<PermissionRules.PermissionRule>) (this.Group?.Permissions ?? new List<PermissionRules.PermissionRule>()));
        return user;
      }
    }
  }
}
