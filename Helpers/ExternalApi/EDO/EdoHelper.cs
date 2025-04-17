// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.EdoHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Core.ViewModels.Basket;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace Gbs.Helpers
{
  public class EdoHelper
  {
    public static List<BasketItem> DeserializeUpd(
      string pathUpd,
      GoodGroups.Group group,
      out List<Gbs.Core.Entities.Goods.Good> listForSaveGood)
    {
      listForSaveGood = new List<Gbs.Core.Entities.Goods.Good>();
      using (DataBase dataBase = Data.GetDataBase())
      {
        List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
        string s = File.ReadAllText(pathUpd, System.Text.Encoding.GetEncoding(1251));
        Файл файл;
        try
        {
          XmlReaderSettings settings = new XmlReaderSettings()
          {
            ConformanceLevel = ConformanceLevel.Fragment,
            IgnoreWhitespace = true,
            IgnoreComments = true
          };
          XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(s), settings);
          xmlReader.Read();
          файл = (Файл) new XmlSerializer(typeof (Файл)).Deserialize(xmlReader);
        }
        catch (InvalidOperationException ex)
        {
          LogHelper.WriteError((Exception) ex, "Ошибка десериализации: " + ex.Message);
          if (ex.InnerException != null)
            LogHelper.WriteError((Exception) ex, "Внутреннее исключение: " + ex.InnerException.Message);
          throw new FileFormatException(Translate.EdoHelper_DeserializeUpd_Формат_выбранного_файла_не_соотвесвет_формату_УПД__которое_формирует_ЭДО_);
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Не удалось разобрать файл УПД", false);
          throw new FileFormatException(Translate.EdoHelper_DeserializeUpd_Формат_выбранного_файла_не_соотвесвет_формату_УПД__которое_формирует_ЭДО_);
        }
        List<BasketItem> basketItemList = new List<BasketItem>();
        try
        {
          ФайлДокументТаблСчФактСведТов[] таблСчФактСведТовArray = файл.Документ.ТаблСчФакт?.СведТов ?? файл.Документ.ТаблКСчФ?.СведТов;
          if (таблСчФактСведТовArray == null)
            throw new FileFormatException(Translate.EdoHelper_DeserializeUpd_Формат_выбранного_файла_не_соотвесвет_формату_УПД__которое_формирует_ЭДО_);
          foreach (ФайлДокументТаблСчФактСведТов таблСчФактСведТов in таблСчФактСведТовArray)
          {
            ФайлДокументТаблСчФактСведТов item = таблСчФактСведТов;
            ТекстИнфТип[] инфПолФхЖ2 = item.ИнфПолФХЖ2;
            string d = (инфПолФхЖ2 != null ? ((IEnumerable<ТекстИнфТип>) инфПолФхЖ2).FirstOrDefault<ТекстИнфТип>((Func<ТекстИнфТип, bool>) (x => x.Идентиф.ToLower().Contains("штрих")))?.Значен : (string) null) ?? "";
            if (d.IsNullOrEmpty() && item.ДопСведТов.НомСредИдентТов != null)
            {
              foreach (ФайлДокументТаблСчФактСведТовДопСведТовНомСредИдентТов товНомСредИдентТов in item.ДопСведТов.НомСредИдентТов)
              {
                ItemsChoiceType[] itemsElementName = товНомСредИдентТов.ItemsElementName;
                if ((itemsElementName != null ? (((IEnumerable<ItemsChoiceType>) itemsElementName).Any<ItemsChoiceType>((Func<ItemsChoiceType, bool>) (x => x == ItemsChoiceType.НомУпак)) ? 1 : 0) : 0) != 0)
                {
                  string str = товНомСредИдентТов.Items[((IEnumerable<ItemsChoiceType>) товНомСредИдентТов.ItemsElementName).ToList<ItemsChoiceType>().FindIndex((Predicate<ItemsChoiceType>) (x => x == ItemsChoiceType.НомУпак))];
                  if (str.Length == 19)
                    d = str.Remove(0, 3).Remove(13, 3);
                }
              }
            }
            Gbs.Core.Entities.Goods.Good good = (Gbs.Core.Entities.Goods.Good) null;
            if (!d.IsNullOrEmpty())
            {
              List<Gbs.Core.Entities.Goods.Good> list = activeItems.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcode == d && string.Equals(x.Name, item.НаимТов, StringComparison.CurrentCultureIgnoreCase))).ToList<Gbs.Core.Entities.Goods.Good>();
              good = (list.Count<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (st => st.Stock)) != 0M)) == 1 ? list.Single<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (st => st.Stock)) != 0M)) : list.FirstOrDefault<Gbs.Core.Entities.Goods.Good>()) ?? getGoodDb(activeItems.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.Barcode == d)).ToList<Gbs.Core.Entities.Goods.Good>());
            }
            if (good == null)
              good = getGoodDb(activeItems.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => string.Equals(x.Name, item.НаимТов, StringComparison.CurrentCultureIgnoreCase))).ToList<Gbs.Core.Entities.Goods.Good>());
            if (good == null)
            {
              good = new Gbs.Core.Entities.Goods.Good()
              {
                Name = item.НаимТов,
                Barcode = d ?? string.Empty,
                Group = group
              };
              listForSaveGood.Add(good);
            }
            Decimal num1 = item.СтТовУчНал == 0M ? item.СтТовУчНалОбъект.СтоимУм : item.СтТовУчНал;
            Decimal q = item.КолТов == 0M ? item.КолТовДо : item.КолТов;
            Decimal num2 = 0M;
            if (q != 0M)
              num2 = Math.Round(num1 / q, 2);
            basketItemList.Add(new BasketItem(good, Guid.Empty, 0M, (Storages.Storage) null, q)
            {
              BuyPrice = num2
            });
          }
        }
        catch (Exception ex)
        {
          LogHelper.Error(ex, "Не удалось разобрать файл УПД", false);
          throw new FileFormatException(Translate.EdoHelper_DeserializeUpd_Во_время_разбора_содержимого_в_файле_произошла_ошибка__возможно_формат_файла_некорректный_);
        }
        return basketItemList;
      }

      static Gbs.Core.Entities.Goods.Good getGoodDb(List<Gbs.Core.Entities.Goods.Good> goods)
      {
        List<Gbs.Core.Entities.Goods.Good> list = goods.Where<Gbs.Core.Entities.Goods.Good>((Func<Gbs.Core.Entities.Goods.Good, bool>) (x => x.StocksAndPrices.Sum<GoodsStocks.GoodStock>((Func<GoodsStocks.GoodStock, Decimal>) (s => s.Stock)) > 0M)).ToList<Gbs.Core.Entities.Goods.Good>();
        return list.Any<Gbs.Core.Entities.Goods.Good>() ? list.First<Gbs.Core.Entities.Goods.Good>() : goods.FirstOrDefault<Gbs.Core.Entities.Goods.Good>();
      }
    }
  }
}
