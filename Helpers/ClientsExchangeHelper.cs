// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ClientsExchangeHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Core.Entities.Db;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class ClientsExchangeHelper
  {
    private static List<ClientCloud> ClientsCash { get; set; } = new List<ClientCloud>();

    public static void UploadFileClients(bool isShowMsg = false)
    {
      LogHelper.OnBegin("Выгрузка данных синхронизации контактов");
      string str1 = FileSystemHelper.TempFolderPath();
      try
      {
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          string jsonString = new ClientsRepository(dataBase).GetListActiveItemAndSum().Select<ClientAdnSum, ClientCloud>((Func<ClientAdnSum, ClientCloud>) (x => new ClientCloud(x))).ToJsonString(true);
          Cloud cloud = new ConfigsRepository<Settings>().Get().RemoteControl.Cloud;
          if (cloud.Path.IsNullOrEmpty())
            throw new FileNotFoundException(Translate.ClientsExchangeHelper_Не_указана_папка_обмена);
          LogHelper.Debug("Папка обмена: " + cloud.Path);
          string str2 = Path.Combine(cloud.Path, "clients");
          if (!FileSystemHelper.ExistsOrCreateFolder(str2, false))
            throw new FileLoadException(string.Format(Translate.ClientsExchangeHelper_Не_удалось_создать_папку__0_, (object) str2));
          string str3 = UidDb.GetUid().EntityUid.ToString();
          string str4 = Path.Combine(str1, str3 + ".json");
          File.WriteAllText(str4, jsonString);
          string str5 = Path.Combine(str2, str3 + ".zip");
          FileSystemHelper.CreateZip(str5, (IEnumerable<FileInfo>) new List<FileInfo>()
          {
            new FileInfo(str4)
          }, "HMWRnLTMKdGUjw46rSFL");
          if (!File.Exists(str5))
            throw new FileNotFoundException(Translate.ClientsExchangeHelper_Файл_обмена_клиентами_не_был_создан);
          if (isShowMsg)
          {
            int num = (int) MessageBoxHelper.Show(Translate.АрхивСДаннымиКонтактовУспешноВыгруженВПапкуОбмена);
          }
          LogHelper.OnEnd("Выгрузка контактов завершена");
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при выгузке файла контактов для синхронизации", false);
      }
      finally
      {
        Directory.Delete(str1, true);
      }
    }

    public static void GetCashClient()
    {
      Settings settings = new ConfigsRepository<Settings>().Get();
      if (settings.RemoteControl.Cloud.Path.IsNullOrEmpty() || settings.Clients.SyncMode != GlobalDictionaries.ClientSyncModes.FileSync)
        return;
      string path = Path.Combine(settings.RemoteControl.Cloud.Path, "clients");
      if (!FileSystemHelper.ExistsOrCreateFolder(path, false))
        return;
      string tempFolder = FileSystemHelper.TempFolderPath();
      ((IEnumerable<string>) Directory.GetFiles(path, "*.zip")).Where<string>((Func<string, bool>) (x => !x.Contains(UidDb.GetUid().EntityUid.ToString()))).Select<string, FileInfo>((Func<string, FileInfo>) (x => new FileInfo(x))).ToList<FileInfo>().Where<FileInfo>((Func<FileInfo, bool>) (x => Guid.TryParse(x.Name.Replace(".zip", ""), out Guid _))).ToList<FileInfo>().ForEach((Action<FileInfo>) (x => FileSystemHelper.ExtractAllFile(x.FullName, tempFolder, "HMWRnLTMKdGUjw46rSFL")));
      List<ClientCloud> list = ((IEnumerable<string>) Directory.GetFiles(tempFolder)).Select<string, List<ClientCloud>>((Func<string, List<ClientCloud>>) (x => JsonConvert.DeserializeObject<List<ClientCloud>>(File.ReadAllText(x)))).SelectMany<List<ClientCloud>, ClientCloud>((Func<List<ClientCloud>, IEnumerable<ClientCloud>>) (x => (IEnumerable<ClientCloud>) x)).ToList<ClientCloud>();
      ClientsExchangeHelper.ClientsCash = new List<ClientCloud>();
      foreach (ClientCloud clientCloud in list)
      {
        clientCloud.Phone = clientCloud.Phone.ClearPhone();
        ClientsExchangeHelper.ClientsCash.Add(clientCloud);
      }
      Directory.Delete(tempFolder, true);
    }

    public static ClientCloud Search(string text)
    {
      return ClientsExchangeHelper.ClientsCash.FirstOrDefault<ClientCloud>((Func<ClientCloud, bool>) (x =>
      {
        if (x.Barcode.Replace("\n", "").Replace("\r", "") == text && !x.Barcode.IsNullOrEmpty())
          return true;
        return x.Phone == text.ClearPhone() && !x.Phone.IsNullOrEmpty();
      }));
    }

    public static List<ClientCloud> GetClientByData(Client client)
    {
      List<ClientCloud> source = new List<ClientCloud>();
      Performancer performancer = new Performancer(string.Format("Получение данных клиента из облака. {0}", (object) ClientsExchangeHelper.ClientsCash.Count), false);
      source.AddRange((IEnumerable<ClientCloud>) ClientsExchangeHelper.ClientsCash.Where<ClientCloud>((Func<ClientCloud, bool>) (x => x.Uid == client.Uid)).ToList<ClientCloud>());
      performancer.AddPoint("Uid");
      if (!client.Barcode.IsNullOrEmpty())
      {
        List<ClientCloud> list = ClientsExchangeHelper.ClientsCash.Where<ClientCloud>((Func<ClientCloud, bool>) (x => !x.Barcode.IsNullOrEmpty() && string.Equals(x.Barcode.Replace("\n", "").Replace("\r", ""), client.Barcode, StringComparison.CurrentCultureIgnoreCase))).ToList<ClientCloud>();
        source.AddRange((IEnumerable<ClientCloud>) list);
        performancer.AddPoint("Barcode");
      }
      string clientPhone = client.Phone.ClearPhone();
      if (!clientPhone.IsNullOrEmpty())
      {
        List<ClientCloud> list = ClientsExchangeHelper.ClientsCash.Where<ClientCloud>((Func<ClientCloud, bool>) (x => x.Phone == clientPhone)).ToList<ClientCloud>();
        source.AddRange((IEnumerable<ClientCloud>) list);
        performancer.AddPoint("Phone ");
      }
      performancer.Stop("Nothing");
      return source.Distinct<ClientCloud>().ToList<ClientCloud>();
    }

    public static Client ConvertToClientEntity(ClientCloud clientCloud)
    {
      Client clientEntity = new Client();
      clientEntity.Uid = clientCloud.Uid;
      clientEntity.Group = clientCloud.Group;
      clientEntity.Name = clientCloud.Name;
      clientEntity.Comment = clientCloud.Comment;
      clientEntity.Barcode = clientCloud.Barcode;
      clientEntity.IsDeleted = clientCloud.IsDeleted;
      clientEntity.Address = clientCloud.Address;
      clientEntity.Birthday = clientCloud.Birthday;
      clientEntity.Email = clientCloud.Email;
      clientEntity.Phone = clientCloud.Phone;
      clientEntity.DateAdd = clientCloud.DateAdd;
      return clientEntity;
    }

    public static Client SaveClient(ClientCloud clientCloud)
    {
      if (MessageBoxHelper.Show(string.Format(Translate.ClientsExchangeHelper_, (object) clientCloud.Name, (object) clientCloud.Barcode, (object) clientCloud.Phone), buttons: MessageBoxButton.YesNo) == MessageBoxResult.No)
        return (Client) null;
      Client clientEntity = ClientsExchangeHelper.ConvertToClientEntity(clientCloud);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        GroupRepository groupRepository = new GroupRepository(dataBase);
        if (groupRepository.GetByUid(clientCloud.Group.Uid) == null)
          groupRepository.Save(clientEntity.Group);
        new ClientsRepository(dataBase).Save(clientEntity);
        return clientEntity;
      }
    }
  }
}
