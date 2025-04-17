// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.KkmViewHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Devices.CheckPrinters;
using Gbs.Core.Devices.CheckPrinters.FiscalKkm;
using Gbs.Core.Entities;
using Gbs.Forms.License;
using System;
using System.Linq;

#nullable disable
namespace Gbs.Helpers
{
  internal class KkmViewHelper
  {
    public static void GetKkmXReport()
    {
      if (!LicenseHelper.GetInfo().IsActive)
        new LicenseNotificationViewModel().Show();
      (bool Result, Gbs.Core.Entities.Users.User User) userForDocument = Other.GetUserForDocument(Gbs.Core.Entities.Actions.PrintKkmReport);
      int num = userForDocument.Result ? 1 : 0;
      Gbs.Core.Entities.Users.User user = userForDocument.User;
      if (num == 0)
        return;
      using (KkmHelper kkmHelper = new KkmHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
      {
        KkmHelper.UserUid = user.Uid;
        kkmHelper.GetReport(ReportTypes.XReport, new Cashier()
        {
          Name = user.Client.Name,
          Inn = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.EntityUid == user.Client.Uid && x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? "",
          UserUid = user.Uid
        });
      }
    }

    public static void GetKkmZReport()
    {
      if (!LicenseHelper.GetInfo().IsActive)
        new LicenseNotificationViewModel().Show();
      (bool Result, Gbs.Core.Entities.Users.User User) userForDocument = Other.GetUserForDocument(Gbs.Core.Entities.Actions.PrintKkmReport);
      int num = userForDocument.Result ? 1 : 0;
      Gbs.Core.Entities.Users.User user = userForDocument.User;
      if (num == 0)
        return;
      using (KkmHelper kkmHelper = new KkmHelper(new ConfigsRepository<Gbs.Core.Config.Devices>().Get()))
        kkmHelper.GetReport(ReportTypes.ZReport, new Cashier()
        {
          Name = user.Client.Name,
          Inn = EntityProperties.GetValuesList(GlobalDictionaries.EntityTypes.Client).FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.EntityUid == user.Client.Uid && x.Type.Uid == GlobalDictionaries.InnUid))?.Value.ToString() ?? "",
          UserUid = user.Uid
        });
    }
  }
}
