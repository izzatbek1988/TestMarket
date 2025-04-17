// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Main.NikitaConfig
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Gbs.Forms.Main
{
  [JsonObject]
  public class NikitaConfig
  {
    public bool IsSendClientInNewGroup { get; set; }

    public Decimal Sum { get; set; }

    public List<Guid> ListOldUidGroup { get; set; } = new List<Guid>();

    public Guid UidNewGroup { get; set; }

    public static void Do()
    {
      if (!File.Exists(nameof (NikitaConfig)))
        return;
      NikitaConfig config = JsonConvert.DeserializeObject<NikitaConfig>(File.ReadAllText(nameof (NikitaConfig)));
      if (!config.IsSendClientInNewGroup)
        return;
      using (DataBase dataBase = Data.GetDataBase())
      {
        ClientsRepository clientsRepository = new ClientsRepository(dataBase);
        List<ClientAdnSum> list = clientsRepository.GetListActiveItemAndSum().Where<ClientAdnSum>((Func<ClientAdnSum, bool>) (x => config.ListOldUidGroup.Any<Guid>((Func<Guid, bool>) (g => g == x.Client.Group.Uid)) && x.TotalSalesSum >= config.Sum)).ToList<ClientAdnSum>();
        Group byUid = new GroupRepository(dataBase).GetByUid(config.UidNewGroup);
        if (byUid == null)
          return;
        foreach (ClientAdnSum clientAdnSum in list)
        {
          try
          {
            clientAdnSum.Client.Group = byUid;
            clientsRepository.Save(clientAdnSum.Client);
          }
          catch
          {
          }
        }
      }
    }
  }
}
