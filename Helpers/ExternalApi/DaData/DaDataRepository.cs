// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.DaDataRepository
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Clients;
using Gbs.Forms._shared;
using Gbs.Forms.Clients;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class DaDataRepository
  {
    public static void Search()
    {
      string inn = "";
      while (true)
      {
        (bool result, string output) tuple = MessageBoxHelper.Input(inn, Translate.DaDataRepository_Search_Введите_ИНН_компании__данные_которой_хотите_найти_, 9, Translate.DaDataRepository_Search_Поиск_по_ИНН, MessageBoxButton.OKCancel);
        if (tuple.result)
        {
          inn = tuple.output;
          if (!Gbs.Helpers.Extensions.String.StringExtensions.IsInn(inn))
            MessageBoxHelper.Warning(Translate.DaDataRepository_Search_Указанный_ИНН_имеет_некорректный_формат__Скорректируйте_значение_ИНН__чтобы_выполнить_поиск_компании_);
          else
            goto label_4;
        }
        else
          break;
      }
      return;
label_4:
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.DaDataRepository_Search_Поиск_организации_по_ИНН);
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Client> list = new ClientsRepository(dataBase).GetActiveItems().Where<Client>((Func<Client, bool>) (c => c.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (p => p.Value?.ToString() == inn)))).ToList<Client>();
        if (list.Any<Client>())
        {
          progressBar.Close();
          if (MessageBoxHelper.Question(string.Format(Translate.DaDataRepository_Search_В_базе_уже_сохранен_контакт_с_указанным_ИНН___0____1____желаете_обновить_имеющиеся_данные_, (object) list.First<Client>().Name, (object) inn)) == MessageBoxResult.No)
            return;
        }
        DaDataHelper.FindOrganizationByInnOrOgrn command = new DaDataHelper.FindOrganizationByInnOrOgrn()
        {
          Inn = inn
        };
        if (new DaDataHelper().DoCommand((DaDataHelper.DaDataCommand) command))
        {
          DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.MainData> obj1 = command.Info.Suggestions.First<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.MainData>>();
          Client client1 = list.Any<Client>() ? list.First<Client>() : new Client();
          client1.Name = obj1.Value.IsNullOrEmpty() ? client1.Name : obj1.Value;
          client1.Address = obj1.Data.Address.Value.IsNullOrEmpty() ? client1.Address : obj1.Data.Address.Value;
          Client client2 = client1;
          List<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.PhoneData>> phones = obj1.Data.Phones;
          bool? nullable1;
          if (phones == null)
          {
            nullable1 = new bool?();
          }
          else
          {
            DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.PhoneData> obj2 = phones.FirstOrDefault<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.PhoneData>>();
            if (obj2 == null)
            {
              nullable1 = new bool?();
            }
            else
            {
              string str = obj2.Value;
              nullable1 = str != null ? new bool?(str.IsNullOrEmpty()) : new bool?();
            }
          }
          bool? nullable2 = nullable1;
          string str1 = nullable2.GetValueOrDefault(true) ? client1.Phone : obj1.Data.Phones.FirstOrDefault<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.PhoneData>>()?.Value;
          client2.Phone = str1;
          Client client3 = client1;
          List<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.EmailData>> emails = obj1.Data.Emails;
          bool? nullable3;
          if (emails == null)
          {
            nullable2 = new bool?();
            nullable3 = nullable2;
          }
          else
          {
            DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.EmailData> obj3 = emails.FirstOrDefault<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.EmailData>>();
            if (obj3 == null)
            {
              nullable2 = new bool?();
              nullable3 = nullable2;
            }
            else
            {
              string str2 = obj3.Value;
              if (str2 == null)
              {
                nullable2 = new bool?();
                nullable3 = nullable2;
              }
              else
                nullable3 = new bool?(str2.IsNullOrEmpty());
            }
          }
          nullable2 = nullable3;
          string str3 = nullable2.GetValueOrDefault(true) ? client1.Email : obj1.Data.Emails.FirstOrDefault<DaDataHelper.FindOrganizationByInnOrOgrn.Item<DaDataHelper.FindOrganizationByInnOrOgrn.EmailData>>()?.Value;
          client3.Email = str3;
          AddPropInfo(client1, GlobalDictionaries.InnUid, obj1.Data.Inn);
          AddPropInfo(client1, GlobalDictionaries.OgrnUid, obj1.Data.Ogrn);
          AddPropInfo(client1, GlobalDictionaries.KppUid, obj1.Data.Kpp);
          progressBar.Close();
          FrmClientCard frmClientCard = new FrmClientCard();
          Guid selectClientUid = list.Any<Client>() ? client1.Uid : Guid.Empty;
          ClientAdnSum clientAdnSum;
          ref ClientAdnSum local = ref clientAdnSum;
          ClientAdnSum _client = new ClientAdnSum();
          _client.Client = client1;
          int action = list.Any<Client>() ? 160 : 150;
          frmClientCard.ShowCard(selectClientUid, out local, _client, action: (Actions) action);
        }
        progressBar.Close();
      }

      static void AddPropInfo(Client client, Guid uidProp, string value)
      {
        if (value.IsNullOrEmpty())
          return;
        if (client.Properties.Any<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == uidProp)))
        {
          client.Properties.First<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == uidProp)).Value = (object) value;
        }
        else
        {
          List<EntityProperties.PropertyValue> properties = client.Properties;
          EntityProperties.PropertyValue propertyValue = new EntityProperties.PropertyValue();
          propertyValue.EntityUid = client.Uid;
          EntityProperties.PropertyType propertyType = new EntityProperties.PropertyType();
          propertyType.Uid = uidProp;
          propertyValue.Type = propertyType;
          propertyValue.Value = (object) value;
          properties.Add(propertyValue);
        }
      }
    }
  }
}
