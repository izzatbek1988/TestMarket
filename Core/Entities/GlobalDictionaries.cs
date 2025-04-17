// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Entities.GlobalDictionaries
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities.Egais;
using Gbs.Helpers;
using Gbs.Helpers.Egais;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Gbs.Core.Entities
{
  public static class GlobalDictionaries
  {
    public static char[] SplitArr = new char[4]
    {
      ' ',
      ';',
      '.',
      ','
    };
    public static readonly Guid AlcCodeUid = new Guid("ADC71CF3-1761-4679-B94C-8443761D4F9A");
    public static readonly Guid CapacityUid = new Guid("5E0807F5-1957-484E-B0EC-66010AED01E1");
    public static readonly Guid AlcVolumeUid = new Guid("FCF71134-9868-4B7F-A7E5-AAB40F5CDEF5");
    public static readonly Guid ProductCodeUid = new Guid("648B30B8-0026-4D1F-8443-57F9668654A7");
    public static readonly Guid IdentityGoodUid = new Guid("226F188C-5E0A-418C-8771-2D4B567E160B");
    public static readonly Guid PercentForServiceGoodUid = new Guid("9DB91ACC-339E-4212-916D-FB366A7E6E27");
    public static readonly Guid PercentForServiceGroupUid = new Guid("C2822461-AA48-45B6-A9A3-A81D993758C8");
    public static readonly Guid PercentForServiceStockUid = new Guid("27C5DA07-BDD2-4CDD-B970-185C445196CE");
    public const string StockColumnsUid = "3D205A8F-AD38-489A-BF93-3C3498CF57BB";
    public const string BuyPriceUid = "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C";
    public static readonly Guid InnUid = new Guid("5EB4EBA4-B8D8-45BF-8BE5-4BB806A1CE46");
    public static readonly Guid OgrnUid = new Guid("6A86CCFB-75A1-4B36-9CFF-0636EADEE6B7");
    public static readonly Guid KppUid = new Guid("FFE70ADD-BA07-42D8-BEEE-1BB0D4CC6A36");
    public static readonly Guid RsUid = new Guid("CC61ABC0-F4FB-4E5F-B354-6D494B27B031");
    public static readonly Guid KsUid = new Guid("42C438A3-A300-48BC-8234-686DDA3DAC2B");
    public static readonly Guid BikUid = new Guid("53836F50-E085-41BF-BA9C-A16010842BF0");
    public static readonly Guid BankNameUid = new Guid("3461C1D7-8E87-4D0E-9FA3-46FE54A36548");
    public static readonly Guid FiasUid = new Guid("E2ED9975-CC33-4DBF-9B1C-9D54ECB109D9");
    public static readonly List<Guid> RequisitesUidList = new List<Guid>()
    {
      GlobalDictionaries.InnUid,
      GlobalDictionaries.OgrnUid,
      GlobalDictionaries.KppUid,
      GlobalDictionaries.RsUid,
      GlobalDictionaries.KsUid,
      GlobalDictionaries.BikUid,
      GlobalDictionaries.BankNameUid,
      GlobalDictionaries.FiasUid
    };
    public static readonly Guid CertificateReusableUid = new Guid("FF839332-0A3E-42AE-9CC0-5FEECC6E17EB");
    public static readonly Guid CertificateNominalUid = new Guid("E9B01E88-6EF7-4D58-9D4C-AE1A259A7474");
    public static readonly Guid CertificatePaymentUid = new Guid("362E685C-F04C-4CA5-8310-8106DE16840B");
    public static readonly Guid BonusesPaymentUid = new Guid("DC1D8BCF-CFAE-4912-A909-BB7E32BBF82E");
    public static readonly Guid AmClassifierIdUid = new Guid("56B4F20A-CF9E-49A6-9753-C438B34F0D42");
    public static readonly Guid GoodIdUid = new Guid("24aaef1f-d733-47f1-ada7-ac3627cf4068");
    public static readonly Guid MarkedInfoGood = new Guid("59FD40F9-2637-4A32-B5E0-1CF5D1307E64");
    public static readonly Guid GroupGoodIdUid = new Guid("b7e7ddde-8bb9-4251-8857-a4f54620cdb0");
    public static readonly Guid IkpuUid = new Guid("EECE83C5-1880-4ED0-95FD-CEF7F228E87C");
    public static Guid UktZedUid = new Guid("0EE3BBFC-DDC3-4CD1-9B89-6FA281F3A5A0");
    public static readonly Guid DefaultExtraRuleUid = new Guid("F57258FB-D33E-4B14-9F74-A97F8D2B976D");
    public static readonly Guid DefaultGoodGroupUid = new Guid("EFFB1EE6-CE68-4687-9C62-E0018FDC2CAB");
    public static readonly Guid NumTableUid = new Guid("097F526E-5613-4D5B-A117-FE563BF1D677");
    public static readonly Guid CountGuestUid = new Guid("0B73C8A7-B468-4135-8F0D-0FEED00420F8");
    public static readonly Guid FiscalNumUid = new Guid("8F06994F-23AF-4F29-AD8E-2740F144044E");
    public static readonly Guid FrNumber = new Guid("D23459EE-FDC7-42C9-9EB0-00879717D58C");
    public static readonly Guid RrnUid = new Guid("1E5DD9D2-D35E-4A8D-97A0-308345CA81DC");
    public static readonly Guid TypeCardMethodUid = new Guid("C7CE4BA1-F5D4-4AAE-AF59-E9EE3F2503F2");
    public static readonly Guid RrnSbpUid = new Guid("069BE78E-691A-4CD6-B432-1A2CE664166A");
    public static readonly Guid PointAcceptUid = new Guid("4D4BECC8-902F-4201-A924-462B42E2D2CC");
    public static readonly Guid ApprovalCodeUid = new Guid("7F7D24B9-E4C8-4BB7-AAFC-E4EF294915E8");
    public static readonly Guid IssuerNameUid = new Guid("7AEA281E-7A75-4302-91C2-0826A86A7D8E");
    public static readonly Guid TerminalIdUid = new Guid("8611CDAE-99E0-48D4-9298-EED4B063CC80");
    public static readonly Guid CardNumberUid = new Guid("95DC1878-FE9B-4E9F-815A-6785A8E50CC2");
    public static readonly Guid PaymentSystemUid = new Guid("6E971B84-0856-4147-A8D8-273DCA897B74");
    public static readonly Guid InfoWithTrueApiUid = new Guid("E42580CD-D6BC-421C-AF91-A51A191F6C18");
    public static readonly Guid TsdDocumentNumberUid = new Guid("26F5227C-34D7-49E3-95D3-7205F8076FAA");
    public static readonly Guid TTNEgais = new Guid("0A3F2091-5343-4DF7-A6A2-25916FFF75EE");
    public static readonly Guid StatusEgais = new Guid("D353B875-EC68-499F-9E8A-A178CFAB636D");
    public static readonly Guid ReplayUidEgais = new Guid("09060088-5FC5-4C61-B673-7BCFF8D62510");
    public static readonly Guid OptionalClientOrderUid = new Guid("57A66E8A-B05A-4514-948B-19C442A1D982");
    public static readonly Guid RegIdForGoodStockUidEgais = new Guid("0A97BE43-DD6B-4F30-8722-81BD922D6483");
    public static readonly Guid DocumentInCreditUid = new Guid("88F2C3B5-E355-403A-8082-7186E416D83E");
    public static readonly Guid FiscalSignPropertyUid = new Guid("E069234E-35D4-4910-8066-E2B7A4EDC816");
    public static readonly Guid TerminalIdPropertyUid = new Guid("1696134D-7594-42E5-B371-BB37F4EE020D");
    public static readonly Guid ReceiptSeqPropertyUid = new Guid("18E27784-E402-4062-99C2-2A3F9EDCB78D");
    public static readonly Guid DateTimePropertyUid = new Guid("C76B45C3-7758-4778-BF14-2F7F9B5D37F8");
    public static Dictionary<Actions, string> ActionsUserDictionary = new Dictionary<Actions, string>()
    {
      {
        Actions.GoodsCreate,
        Translate.UserGroupViewModel_Создание_или_копирование_товаров
      },
      {
        Actions.GoodsCatalogShow,
        Translate.UserGroupViewModel_Просмотр_каталога_товаров
      },
      {
        Actions.GoodsDelete,
        Translate.UserGroupViewModel_Удаление_товаров
      },
      {
        Actions.GoodsEdit,
        Translate.UserGroupViewModel_Редактирование_товаров
      },
      {
        Actions.ShowBuyPrice,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_закупочных_цен
      },
      {
        Actions.SettingsShowAndEdit,
        Translate.UserGroupViewModel_Просмотр_и_редактирование_настроек
      },
      {
        Actions.SaleSave,
        Translate.UserGroupViewModel_Проведение_продаж
      },
      {
        Actions.ReturnSale,
        Translate.UserGroupViewModel_Возврат_товара
      },
      {
        Actions.InsertCash,
        Translate.UserGroupViewModel_Внесение_средств
      },
      {
        Actions.RemoveCash,
        Translate.UserGroupViewModel_Снятие_средств
      },
      {
        Actions.SendCash,
        Translate.UserGroupViewModel_Перемещения_средств
      },
      {
        Actions.WaybillAdd,
        Translate.UserGroupViewModel_Создание_накладной
      },
      {
        Actions.WaybillEdit,
        Translate.UserGroupViewModel_UserGroupViewModel_Редактирование_накладной
      },
      {
        Actions.WaybillDelete,
        Translate.UserGroupViewModel_UserGroupViewModel_Удаление_накладной
      },
      {
        Actions.WaybillListShow,
        Translate.UserGroupViewModel_Просмотр_журнала_поступлений
      },
      {
        Actions.CorrectSumByAcc,
        Translate.UserGroupViewModel_Корректировка_суммы_на_счете
      },
      {
        Actions.ClientsCatalogShow,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_списка_покупателей
      },
      {
        Actions.ClientsAdd,
        Translate.UserGroupViewModel_UserGroupViewModel_Добавление_покупателей
      },
      {
        Actions.ClientsEdit,
        Translate.UserGroupViewModel_UserGroupViewModel_Редактирование_покупателей
      },
      {
        Actions.ClientsDelete,
        Translate.UserGroupViewModel_UserGroupViewModel_Удаление_покупателей
      },
      {
        Actions.ClientJoin,
        Translate.UserGroupViewModel_UserGroupViewModel_Объединение_покупателей
      },
      {
        Actions.CreditReturn,
        Translate.UserGroupViewModel_UserGroupViewModel_Возврат_долга_покупателя
      },
      {
        Actions.ShowCredits,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_списка_должников
      },
      {
        Actions.ViewStock,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_остатков
      },
      {
        Actions.ViewHistory,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_истории_товара
      },
      {
        Actions.ExecuteScript,
        Translate.UserGroupViewModel_Выполнение_SQL_скриптов
      },
      {
        Actions.PrintKkmReport,
        Translate.UserGroupViewModel_Печать_отчетов_на_ККМ
      },
      {
        Actions.CreateInventory,
        Translate.UserGroupViewModel_Создание_инвентаризации
      },
      {
        Actions.EditInventory,
        Translate.UserGroupViewModel_Продолжение_инвентаризации
      },
      {
        Actions.DeleteInventory,
        Translate.UserGroupViewModel_Удаление_инвентаризации
      },
      {
        Actions.CreateWriteOff,
        Translate.UserGroupViewModel_Создание_списания_товаров
      },
      {
        Actions.EditWriteOff,
        Translate.UserGroupViewModel_Редактирование_списания_товаров
      },
      {
        Actions.DeleteWriteOff,
        Translate.UserGroupViewModel_Удаление_списания_товаров
      },
      {
        Actions.ShowJournalWriteOff,
        Translate.UserGroupViewModel_Просмотр_журнала_списания_товаров
      },
      {
        Actions.DeleteSale,
        Translate.UserGroupViewModel_Удаление_продажи
      },
      {
        Actions.DeletePayment,
        Translate.UserGroupViewModel_Удаление_платежа
      },
      {
        Actions.ShowJournalInventory,
        Translate.UserGroupViewModel_Просмотр_журнала_инвентаризаций
      },
      {
        Actions.ShowSummaryReport,
        Translate.UserGroupViewModel_Доступ_к_сводному_отчету
      },
      {
        Actions.ShowJournalSale,
        Translate.UserGroupViewModel_Просмотр_журнала_продаж
      },
      {
        Actions.GroupEditingGoodAndCategories,
        Translate.UserGroupViewModel_Групповое_редактирование_товаров_и_категорий
      },
      {
        Actions.CreateClientOrder,
        Translate.UserGroupViewModel_Добавление_заказа_резерва
      },
      {
        Actions.EditClientOrder,
        Translate.UserGroupViewModel_Редактирование_заказа_резерва
      },
      {
        Actions.DeleteClientOrder,
        Translate.UserGroupViewModel_Удаление_заказа_резерва
      },
      {
        Actions.AddGoodGroup,
        Translate.UserGroupViewModel_Добавление_категории_товаров
      },
      {
        Actions.EditGoodGroup,
        Translate.UserGroupViewModel_Редактирование_категории_товаров
      },
      {
        Actions.DeleteGoodGroup,
        Translate.UserGroupViewModel_Удаление_категории_товаров
      },
      {
        Actions.AddClientGroup,
        Translate.UserGroupViewModel_Добавление_группы_контактов
      },
      {
        Actions.EditClientGroup,
        Translate.UserGroupViewModel_Редактирование_группы_контактов
      },
      {
        Actions.DeleteClientGroup,
        Translate.UserGroupViewModel_Удаление_группы_контактов
      },
      {
        Actions.AddMoveWaybill,
        Translate.UserGroupViewModel_Добавление_перемещения
      },
      {
        Actions.DeleteMoveWaybill,
        Translate.UserGroupViewModel_Удаление_перемещений
      },
      {
        Actions.GoodsJoin,
        Translate.UserGroupViewModel_Объединение_товаров
      },
      {
        Actions.ShowMasterReport,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_мастера_отчетов
      },
      {
        Actions.DeleteItemBasket,
        Translate.UserGroupViewModel_UserGroupViewModel_Удаление_позиции_из_корзины
      },
      {
        Actions.EditCountItemBasket,
        Translate.UserGroupViewModel_UserGroupViewModel_Измение_количества_в_корзине
      },
      {
        Actions.EditDiscountItem,
        Translate.UserGroupViewModel_UserGroupViewModel_Измнение_скидки_вручную
      },
      {
        Actions.CancelSale,
        Translate.UserGroupViewModel_UserGroupViewModel_Отмена_продажи_чека
      },
      {
        Actions.AddMoveStorage,
        Translate.UserGroupViewModel_UserGroupViewModel_Перемещение_товаров_между_складами
      },
      {
        Actions.DeleteMoveStorage,
        Translate.UserGroupViewModel_UserGroupViewModel_Удаление_перемещения_между_складами
      },
      {
        Actions.ShowEgaisWaybill,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_входящих_накладных
      },
      {
        Actions.AcceptEgaisWaybill,
        Translate.UserGroupViewModel_UserGroupViewModel_Прием_входящих_накладных
      },
      {
        Actions.AddGoodStock,
        Translate.UserGroupViewModel_UserGroupViewModel_Добавление_товарных_остатков
      },
      {
        Actions.DeleteGoodStock,
        Translate.UserGroupViewModel_UserGroupViewModel_Удаление_товарных_остатков
      },
      {
        Actions.ClientsBonusesEdit,
        Translate.UserGroupViewModel_UserGroupViewModel_Редактирование_суммы_баллов_вручную
      },
      {
        Actions.ShowSellerReport,
        Translate.UserGroupViewModel_UserGroupViewModel_Просмотр_отчета_продавца
      },
      {
        Actions.EditSalePriceGoodStock,
        Translate.UserGroupViewModel_Изменение_розничной_цены_в_карточке_остатка
      },
      {
        Actions.EditQuantityGoodStock,
        Translate.UserGroupViewModel_Изменение_количества_в_карточке_остатка
      },
      {
        Actions.AddProduction,
        Translate.UserGroupViewModel_Добавление_производства
      },
      {
        Actions.DeleteProduction,
        Translate.UserGroupViewModel_Удаление_производства
      },
      {
        Actions.ShowProduction,
        Translate.UserGroupViewModel_Просмотр_списка_производств
      },
      {
        Actions.AddSpeedProduction,
        Translate.БыстроеПроизводство
      },
      {
        Actions.DeleteOrderCafe,
        Translate.УдалениеЗаказовВРежимеКафе
      },
      {
        Actions.EditFrReport,
        Translate.РедактированиеВОкнеВыбораШаблонов
      },
      {
        Actions.DoUseBonusesIfOffSmsCode,
        Translate.UserGroupViewModel_ActionsUserDictionary_Использование_баллов_без_СМС_подтверждения
      },
      {
        Actions.DoSaleCreditIfOffSmsCode,
        Translate.UserGroupViewModel_ActionsUserDictionary_Продажа_в_долг_без_СМС_подтверждения
      },
      {
        Actions.ActionsToBeerTap,
        Translate.GlobalDictionaries_ActionsUserDictionary_Действия_с_пивными_кранами
      },
      {
        Actions.CorrectBalanceSum,
        Translate.GlobalDictionaries_ActionsUserDictionary_Корректировка_баланса_суммы_в_кассе
      }
    };

    public static Dictionary<GlobalDictionaries.Skin, string> GetSkinDictionary()
    {
      return new Dictionary<GlobalDictionaries.Skin, string>()
      {
        {
          GlobalDictionaries.Skin.Default,
          Translate.Светлая
        },
        {
          GlobalDictionaries.Skin.Dark,
          Translate.Темная
        }
      };
    }

    public static string NumTableUidString => GlobalDictionaries.NumTableUid.ToString();

    public static string CountGuestUidString => GlobalDictionaries.CountGuestUid.ToString();

    public static Dictionary<GlobalDictionaries.Encoding, string> EncodingDictionary
    {
      get
      {
        return new Dictionary<GlobalDictionaries.Encoding, string>()
        {
          {
            GlobalDictionaries.Encoding.CP866,
            "CP866"
          },
          {
            GlobalDictionaries.Encoding.CPTysso,
            "CP Tysso"
          },
          {
            GlobalDictionaries.Encoding.W1251,
            "Windows-1251"
          },
          {
            GlobalDictionaries.Encoding.Utf8,
            "UTF-8"
          },
          {
            GlobalDictionaries.Encoding.KOI8R,
            "КОИ8-Р"
          }
        };
      }
    }

    public static Dictionary<TypeWriteOff1, string> EgaisTypeWriteOffDictionary
    {
      get
      {
        return new Dictionary<TypeWriteOff1, string>()
        {
          {
            TypeWriteOff1.Реализация,
            "Реализация"
          },
          {
            TypeWriteOff1.Производственныепотери,
            "Производственные потери"
          },
          {
            TypeWriteOff1.Арест,
            "Арест"
          },
          {
            TypeWriteOff1.Иныецели,
            "Иные цели"
          },
          {
            TypeWriteOff1.Недостача,
            "Недостача"
          },
          {
            TypeWriteOff1.Пересортица,
            "Пересортица"
          },
          {
            TypeWriteOff1.Порча,
            "Порча"
          },
          {
            TypeWriteOff1.Потери,
            "Потери"
          },
          {
            TypeWriteOff1.Проверки,
            "Проверки"
          },
          {
            TypeWriteOff1.Уценка,
            "Уценка"
          }
        };
      }
    }

    public static Dictionary<EgaisWriteOffActStatus, string> EgaisWriteOffActStatusDictionary
    {
      get
      {
        return new Dictionary<EgaisWriteOffActStatus, string>()
        {
          {
            EgaisWriteOffActStatus.Transfer,
            "Обрабатывается ЕГАИС"
          },
          {
            EgaisWriteOffActStatus.All,
            Translate.WaybillsViewModel_Statuses_Все_статусы
          },
          {
            EgaisWriteOffActStatus.Done,
            Translate.GlobalDictionaries_EgaisWriteOffActStatusDictionary_Проведен_в_ЕГАИС
          },
          {
            EgaisWriteOffActStatus.Error,
            Translate.GlobalDictionaries_EgaisWriteOffActStatusDictionary_Ошибка_при_передаче_проведении_в_ЕГАИС
          },
          {
            EgaisWriteOffActStatus.Send,
            Translate.GlobalDictionaries_EgaisWriteOffActStatusDictionary_Передан_в_УТМ
          },
          {
            EgaisWriteOffActStatus.Unknown,
            Translate.GlobalDictionaries_EgaisWriteOffActStatusDictionary_Не_удалось_определить
          }
        };
      }
    }

    public static Dictionary<GlobalDictionaries.DocumentsStatuses, string> ClientOrderStatusDictionary
    {
      get
      {
        return new Dictionary<GlobalDictionaries.DocumentsStatuses, string>()
        {
          {
            GlobalDictionaries.DocumentsStatuses.Open,
            Translate.GlobalDictionaries_Открыт
          },
          {
            GlobalDictionaries.DocumentsStatuses.Close,
            Translate.GlobalDictionaries_Выполнен__закрыт_
          }
        };
      }
    }

    public static Dictionary<GlobalDictionaries.DocumentsStatuses, string> CafeOrderStatusDictionary
    {
      get
      {
        return new Dictionary<GlobalDictionaries.DocumentsStatuses, string>()
        {
          {
            GlobalDictionaries.DocumentsStatuses.Draft,
            Translate.GlobalDictionaries_CafeOrderStatusDictionary_В_работе
          },
          {
            GlobalDictionaries.DocumentsStatuses.Close,
            Translate.GlobalDictionaries_Выполнен__закрыт_
          }
        };
      }
    }

    public static Dictionary<GlobalDictionaries.DocumentsStatuses, string> DocumentStatusesDictionary
    {
      get
      {
        return new Dictionary<GlobalDictionaries.DocumentsStatuses, string>()
        {
          {
            GlobalDictionaries.DocumentsStatuses.None,
            Translate.SaleJournalViewModel_Любой
          },
          {
            GlobalDictionaries.DocumentsStatuses.Draft,
            Translate.GlobalDictionaries_DocumentStatusesDictionary_Черновик
          },
          {
            GlobalDictionaries.DocumentsStatuses.Open,
            Translate.GlobalDictionaries_DocumentStatusesDictionary_Открыт
          },
          {
            GlobalDictionaries.DocumentsStatuses.Close,
            Translate.GlobalDictionaries_DocumentStatusesDictionary_Закрыт
          }
        };
      }
    }

    public static Dictionary<GlobalDictionaries.Format, string> FormatDictionary()
    {
      Dictionary<GlobalDictionaries.Format, string> dictionary = new Dictionary<GlobalDictionaries.Format, string>()
      {
        {
          GlobalDictionaries.Format.Excel,
          "Excel"
        },
        {
          GlobalDictionaries.Format.Csv,
          "Csv"
        },
        {
          GlobalDictionaries.Format.Json,
          "Json"
        },
        {
          GlobalDictionaries.Format.Xml,
          "Xml"
        },
        {
          GlobalDictionaries.Format.Yml,
          "YML (Яндекс)"
        }
      };
      if (new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Ukraine)
        dictionary.Remove(GlobalDictionaries.Format.Yml);
      return dictionary;
    }

    public static Dictionary<GlobalDictionaries.VersionUpdate, string> GetVersionUpdateDictionary
    {
      get
      {
        return new Dictionary<GlobalDictionaries.VersionUpdate, string>()
        {
          {
            GlobalDictionaries.VersionUpdate.Release,
            Translate.GlobalDictionaries_Релиз
          },
          {
            GlobalDictionaries.VersionUpdate.Beta,
            Translate.GlobalDictionaries_Бета
          }
        };
      }
    }

    public static Dictionary<GlobalDictionaries.Mode, string> ModeProgramDictionary()
    {
      return new Dictionary<GlobalDictionaries.Mode, string>()
      {
        {
          GlobalDictionaries.Mode.Shop,
          Translate.GlobalDictionaries_Магазин___Склад
        },
        {
          GlobalDictionaries.Mode.Cafe,
          Translate.GlobalDictionaries_ModeProgramDictionary_Кафе
        },
        {
          GlobalDictionaries.Mode.Home,
          Translate.GlobalDictionaries_Дом___Офис
        }
      };
    }

    public static Dictionary<GlobalDictionaries.ClientSyncModes, string> ClientsSyncModesDictionary()
    {
      return new Dictionary<GlobalDictionaries.ClientSyncModes, string>()
      {
        {
          GlobalDictionaries.ClientSyncModes.None,
          Translate.SecondMonitorPageViewModel_MonitorList_Нет
        },
        {
          GlobalDictionaries.ClientSyncModes.FileSync,
          Translate.GlobalDictionaries_Файловый_обмен
        }
      };
    }

    public static Dictionary<GlobalDictionaries.GoodTypes, string> GoodTypesDictionary()
    {
      return new Dictionary<GlobalDictionaries.GoodTypes, string>()
      {
        {
          GlobalDictionaries.GoodTypes.Single,
          Translate.GlobalDictionaries_Обычный_товар
        },
        {
          GlobalDictionaries.GoodTypes.Weight,
          Translate.GlobalDictionaries_Весовой_товар
        },
        {
          GlobalDictionaries.GoodTypes.Service,
          Translate.GlobalDictionaries_Услуги
        },
        {
          GlobalDictionaries.GoodTypes.Certificate,
          Translate.GlobalDictionaries_Подарочные_сертификаты
        }
      };
    }

    public static List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>> MarkedProductionTypesList
    {
      get
      {
        return new List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>>()
        {
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.Countries.NotSet, Translate.GlobalDictionaries_Не_указано),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Fur, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_RuMarkedProductionTypesDictionary_Изделия_из_меха),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Drugs, new List<GlobalDictionaries.Countries>()
          {
            GlobalDictionaries.Countries.Russia,
            GlobalDictionaries.Countries.Kazakhstan
          }, Translate.GlobalDictionaries_RuMarkedProductionTypesDictionary_Лекарства),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Tobacco, new List<GlobalDictionaries.Countries>()
          {
            GlobalDictionaries.Countries.Russia,
            GlobalDictionaries.Countries.Kazakhstan
          }, Translate.GlobalDictionaries_RuMarkedProductionTypesDictionary_Табачные_изделия),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Shoes, new List<GlobalDictionaries.Countries>()
          {
            GlobalDictionaries.Countries.Russia,
            GlobalDictionaries.Countries.Kazakhstan
          }, Translate.GlobalDictionaries_RuMarkedProductionTypesDictionary_Обувь),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Perfume, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_Парфюм),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Tires, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_Шины_покрышки),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.LightIndustry, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_Легкая_промышленность_одежда),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Milk, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_MarkedProductionTypesList_Молочная_продукция),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Water, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_MarkedProductionTypesList_Упакованная_воды),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Photo, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_MarkedProductionTypesList_Фототехника),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Alcohol, new List<GlobalDictionaries.Countries>()
          {
            GlobalDictionaries.Countries.Russia,
            GlobalDictionaries.Countries.Kazakhstan
          }, Translate.GlobalDictionaries_MarkedProductionTypesList_Алкогольная_продукция),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Other, GlobalDictionaries.Countries.Russia, Translate.GlobalDictionaries_MarkedProductionTypesList_Другие_товары),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol, GlobalDictionaries.Countries.Ukraine, Translate.GlobalDictionaries_MarkedProductionTypesList_Алкогольная_продукция),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Arm_Alcohol, GlobalDictionaries.Countries.Armenia, Translate.GlobalDictionaries_MarkedProductionTypesList_Алкогольная_продукция),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.Arm_Tobacco, GlobalDictionaries.Countries.Armenia, Translate.GlobalDictionaries_RuMarkedProductionTypesDictionary_Табачные_изделия)
        };
      }
    }

    public static Dictionary<GlobalDictionaries.RuTaxSystems, string> RuTaxSystemsDictionary()
    {
      return new Dictionary<GlobalDictionaries.RuTaxSystems, string>()
      {
        {
          GlobalDictionaries.RuTaxSystems.None,
          "Не указана"
        },
        {
          GlobalDictionaries.RuTaxSystems.Osn,
          "ОСН"
        },
        {
          GlobalDictionaries.RuTaxSystems.UsnDohod,
          "УСН: доход"
        },
        {
          GlobalDictionaries.RuTaxSystems.UsnDohodMinusRashod,
          "УСН: доход - расход"
        },
        {
          GlobalDictionaries.RuTaxSystems.Envd,
          "ЕНВД"
        },
        {
          GlobalDictionaries.RuTaxSystems.Esn,
          "ЕСН"
        },
        {
          GlobalDictionaries.RuTaxSystems.Psn,
          "ПСН"
        }
      };
    }

    public static Dictionary<GlobalDictionaries.RuFfdGoodsTypes, string> RuFfdGoodsTypesDictionary()
    {
      return new Dictionary<GlobalDictionaries.RuFfdGoodsTypes, string>()
      {
        {
          GlobalDictionaries.RuFfdGoodsTypes.None,
          Translate.GlobalDictionaries_Не_указано
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.SimpleGood,
          "Товары, кроме подакцизных"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.ExcisableGood,
          "Подакцизный товар"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Work,
          "Работа"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Service,
          "Услуга"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.GamePayment,
          "Ставка азартной игры"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.GameWin,
          "Выигрыш азартной игры"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.LotteryPayment,
          "Лотерейный билет"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.LotteryWin,
          "Выигрыш в лотереи"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Rid,
          "Предоставление РИД"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Payment,
          "Платеж"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.AgentPayment,
          "Агентское вознаграждение"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.WithPayment,
          "Выплата"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Other,
          "Иной предмет расчета"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.PropertyLaw,
          "Имущественное право"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.NonOperatingIncome,
          "Внереализационный доход"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.OtherPayment,
          "Иные платежи и взносы"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.TradeFee,
          "Торговый сбор"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.ResortFee,
          "Курортный сбор"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Deposit,
          "Залог"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Expenditure,
          "Расход"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.PensionInsuranceIP,
          "Взносы на ОПС ИП"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.PensionInsurance,
          "Взносы на ОПС"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.MedicalInsuranceIP,
          "Взносы на ОМС ИП"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.MedicalInsurance,
          "Взносы на ОМС"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.SocialInsurance,
          "Взносы на ОСС"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.CasinoPayment,
          "Платеж казино"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.OutOfFunds,
          "Выдача денежных средств"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Atnm,
          "АТНМ"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Atm,
          "АТМ"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Tnm,
          "ТНМ"
        },
        {
          GlobalDictionaries.RuFfdGoodsTypes.Tm,
          "ТМ"
        }
      };
    }

    public static Dictionary<GlobalDictionaries.KkmPaymentMethods, string> KkmPaymentMethodsDictionary()
    {
      return new Dictionary<GlobalDictionaries.KkmPaymentMethods, string>()
      {
        {
          GlobalDictionaries.KkmPaymentMethods.Cash,
          Translate.GlobalDictionaries_Наличные
        },
        {
          GlobalDictionaries.KkmPaymentMethods.Card,
          Translate.GlobalDictionaries_Карта
        },
        {
          GlobalDictionaries.KkmPaymentMethods.Bank,
          Translate.GlobalDictionaries_Безналично
        },
        {
          GlobalDictionaries.KkmPaymentMethods.Bonus,
          Translate.GlobalDictionaries_Бонусы
        },
        {
          GlobalDictionaries.KkmPaymentMethods.Certificate,
          Translate.GlobalDictionaries_Сертификат
        },
        {
          GlobalDictionaries.KkmPaymentMethods.Credit,
          Translate.GlobalDictionaries_Кредит
        },
        {
          GlobalDictionaries.KkmPaymentMethods.PrePayment,
          Translate.GlobalDictionaries_Предоплата
        },
        {
          GlobalDictionaries.KkmPaymentMethods.EMoney,
          Translate.GlobalDictionaries_Электронными
        }
      };
    }

    public static Dictionary<GlobalDictionaries.PaymentMethodsType, string> PaymentMethodsTypeDictionary()
    {
      return new Dictionary<GlobalDictionaries.PaymentMethodsType, string>()
      {
        {
          GlobalDictionaries.PaymentMethodsType.NoBroker,
          Translate.Devices_Нет
        },
        {
          GlobalDictionaries.PaymentMethodsType.Acquiring,
          Translate.GlobalDictionaries_Интегрированный_эквайринг_терминал
        },
        {
          GlobalDictionaries.PaymentMethodsType.Sbp,
          Translate.GlobalDictionaries_СБП__по_QR_коду_
        }
      };
    }

    public static Dictionary<GlobalDictionaries.RuFfdPaymentModes, string> RuFfdPaymentModesDictionary()
    {
      return new Dictionary<GlobalDictionaries.RuFfdPaymentModes, string>()
      {
        {
          GlobalDictionaries.RuFfdPaymentModes.None,
          Translate.PaymentsActionsViewModel_Не_указан
        },
        {
          GlobalDictionaries.RuFfdPaymentModes.PrePaymentFull,
          "Предоплата 100%"
        },
        {
          GlobalDictionaries.RuFfdPaymentModes.Prepayment,
          "Предоплата"
        },
        {
          GlobalDictionaries.RuFfdPaymentModes.AdvancePayment,
          "Аванс"
        },
        {
          GlobalDictionaries.RuFfdPaymentModes.FullPayment,
          "Полный расчет"
        },
        {
          GlobalDictionaries.RuFfdPaymentModes.PartPaymentAndCredit,
          "Частичный расчет и кредит"
        },
        {
          GlobalDictionaries.RuFfdPaymentModes.FullCredit,
          "Передача в кредит"
        },
        {
          GlobalDictionaries.RuFfdPaymentModes.PaymentForCredit,
          "Оплата кредита"
        }
      };
    }

    public static Dictionary<GlobalDictionaries.EntityPropertyTypes, string> PropertyDictionary()
    {
      return new Dictionary<GlobalDictionaries.EntityPropertyTypes, string>()
      {
        {
          GlobalDictionaries.EntityPropertyTypes.Text,
          Translate.GlobalDictionaries_Строка
        },
        {
          GlobalDictionaries.EntityPropertyTypes.Integer,
          Translate.GlobalDictionaries_Целое_число
        },
        {
          GlobalDictionaries.EntityPropertyTypes.Decimal,
          Translate.GlobalDictionaries_Дробное_число
        },
        {
          GlobalDictionaries.EntityPropertyTypes.DateTime,
          Translate.GlobalDictionaries_Дата
        }
      };
    }

    public static Dictionary<GlobalDictionaries.GoodsSetStatuses, string> GoodsSetStatusesDictionary()
    {
      return new Dictionary<GlobalDictionaries.GoodsSetStatuses, string>()
      {
        {
          GlobalDictionaries.GoodsSetStatuses.None,
          Translate.GlobalDictionaries_Обычный_товар__продавать_и_списывать_этот_товар_
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Set,
          Translate.GlobalDictionaries_Комплект__продавать_этот__а_списывать_из_состава_
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Kit,
          Translate.GlobalDictionaries_Набор__продавать_и_списывать_из_состава_
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Range,
          Translate.GlobalDictionaries_Ассортимент__предлагать_варианты_
        },
        {
          GlobalDictionaries.GoodsSetStatuses.Production,
          Translate.GlobalDictionaries_Рецепт_производства
        }
      };
    }

    public static Dictionary<GlobalDictionaries.BarcodeIfEmpty, string> BarcodeActionDictionary()
    {
      return new Dictionary<GlobalDictionaries.BarcodeIfEmpty, string>()
      {
        {
          GlobalDictionaries.BarcodeIfEmpty.Empty,
          Translate.GlobalDictionaries_Оставить_пустым
        },
        {
          GlobalDictionaries.BarcodeIfEmpty.Generate,
          Translate.GlobalDictionaries_Генерировать_штрих_код_автоматически
        },
        {
          GlobalDictionaries.BarcodeIfEmpty.Skip,
          Translate.GlobalDictionaries_Пропустить_строку
        }
      };
    }

    public static Dictionary<GlobalDictionaries.BarcodeIfReplay, string> BarcodeClientActionDictionary()
    {
      return new Dictionary<GlobalDictionaries.BarcodeIfReplay, string>()
      {
        {
          GlobalDictionaries.BarcodeIfReplay.Empty,
          Translate.GlobalDictionaries_Оставить_пустым
        },
        {
          GlobalDictionaries.BarcodeIfReplay.Generate,
          Translate.GlobalDictionaries_Генерировать_штрих_код_автоматически
        },
        {
          GlobalDictionaries.BarcodeIfReplay.Skip,
          Translate.GlobalDictionaries_Пропустить_строку
        }
      };
    }

    public static Dictionary<GlobalDictionaries.CertificateStatus, string> CertificateStatusDictionary()
    {
      return new Dictionary<GlobalDictionaries.CertificateStatus, string>()
      {
        {
          GlobalDictionaries.CertificateStatus.All,
          Translate.WaybillsViewModel_Statuses_Все_статусы
        },
        {
          GlobalDictionaries.CertificateStatus.Close,
          Translate.GlobalDictionaries_Закрыт
        },
        {
          GlobalDictionaries.CertificateStatus.Open,
          Translate.GlobalDictionaries_Открыт
        },
        {
          GlobalDictionaries.CertificateStatus.Saled,
          Translate.GlobalDictionaries_Продан
        }
      };
    }

    public static Dictionary<GlobalDictionaries.PaymentTypes, string> PaymentTypesDictionary()
    {
      return new Dictionary<GlobalDictionaries.PaymentTypes, string>()
      {
        {
          GlobalDictionaries.PaymentTypes.MoneyPayment,
          Translate.GlobalDictionaries_Платеж
        },
        {
          GlobalDictionaries.PaymentTypes.MoneyDocumentPayment,
          Translate.GlobalDictionaries_Платеж_по_документу
        },
        {
          GlobalDictionaries.PaymentTypes.MoneyMovement,
          Translate.GlobalDictionaries_Перемещение
        },
        {
          GlobalDictionaries.PaymentTypes.MoneyCorrection,
          Translate.GlobalDictionaries_Корректировка
        }
      };
    }

    [Localizable(false)]
    public static List<GlobalDictionaries.Language> LanguagesList()
    {
      return new List<GlobalDictionaries.Language>()
      {
        new GlobalDictionaries.Language()
        {
          Key = "system",
          Name = Translate.GlobalDictionaries_По_умолчанию_в_Windows,
          Value = GlobalDictionaries.Languages.System
        },
        new GlobalDictionaries.Language()
        {
          Key = "ru",
          Name = "Русский (Russian)",
          Value = GlobalDictionaries.Languages.Russian
        },
        new GlobalDictionaries.Language()
        {
          Key = "kk",
          Name = "Қазақ (Kazakh)",
          Value = GlobalDictionaries.Languages.Kazakh
        },
        new GlobalDictionaries.Language()
        {
          Key = "uk",
          Name = "Український (Ukrainian)",
          Value = GlobalDictionaries.Languages.Ukrainian
        },
        new GlobalDictionaries.Language()
        {
          Key = "az",
          Name = "Azərbaycan (Azerbaijani)",
          Value = GlobalDictionaries.Languages.Azerbaijani
        },
        new GlobalDictionaries.Language()
        {
          Key = "uz",
          Name = "O'zbek tili (Uzbek)",
          Value = GlobalDictionaries.Languages.Uzbek
        },
        new GlobalDictionaries.Language()
        {
          Key = "hy",
          Name = "Հայկական (Armenian)",
          Value = GlobalDictionaries.Languages.Armenian
        },
        new GlobalDictionaries.Language()
        {
          Key = "en",
          Name = "English",
          Value = GlobalDictionaries.Languages.English
        }
      };
    }

    public static Dictionary<GlobalDictionaries.Countries, string> CountriesDictionary()
    {
      return new Dictionary<GlobalDictionaries.Countries, string>()
      {
        {
          GlobalDictionaries.Countries.NotSet,
          Translate.GlobalDictionaries_Не_указана__Другая_
        },
        {
          GlobalDictionaries.Countries.Russia,
          Translate.GlobalDictionaries_Россия
        },
        {
          GlobalDictionaries.Countries.Ukraine,
          Translate.GlobalDictionaries_Украина
        },
        {
          GlobalDictionaries.Countries.Azerbaijan,
          Translate.GlobalDictionaries_Азербайджан
        },
        {
          GlobalDictionaries.Countries.Kazakhstan,
          Translate.GlobalDictionaries_Казахстан
        },
        {
          GlobalDictionaries.Countries.Belarus,
          Translate.GlobalDictionaries_CountriesDictionary_Беларусь
        },
        {
          GlobalDictionaries.Countries.Uzbekistan,
          Translate.GlobalDictionaries_CountriesDictionary_Узбекистан
        },
        {
          GlobalDictionaries.Countries.Armenia,
          Translate.GlobalDictionaries_CountriesDictionary_Армения
        }
      };
    }

    public static List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>> SmsServiceTypeDictionary()
    {
      return new List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>>()
      {
        new GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.None, GlobalDictionaries.Countries.NotSet, "Не указан"),
        new GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsRu, GlobalDictionaries.Countries.Russia, "SMS.RU"),
        new GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsAgent, GlobalDictionaries.Countries.Russia, "СМС-Агент"),
        new GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsCenter, new List<GlobalDictionaries.Countries>()
        {
          GlobalDictionaries.Countries.Russia,
          GlobalDictionaries.Countries.Kazakhstan
        }, "SMS-Центр"),
        new GlobalDictionaries.ItemForCountry<GlobalDictionaries.SmsServiceType>(GlobalDictionaries.SmsServiceType.SmsAero, GlobalDictionaries.Countries.Russia, "SMS AERO")
      };
    }

    public static Dictionary<GlobalDictionaries.ActionAuthType, string> ActionAuthTypeDictionary()
    {
      return new Dictionary<GlobalDictionaries.ActionAuthType, string>()
      {
        {
          GlobalDictionaries.ActionAuthType.None,
          Translate.GlobalDictionaries_ActionAuthTypeDictionary_не_запрашивать_подтверждение
        },
        {
          GlobalDictionaries.ActionAuthType.Sms,
          Translate.GlobalDictionaries_ActionAuthTypeDictionary_отправлять_код_в_СМС
        },
        {
          GlobalDictionaries.ActionAuthType.Viber,
          Translate.GlobalDictionaries_ActionAuthTypeDictionary_отправлять_код_в_Viber
        },
        {
          GlobalDictionaries.ActionAuthType.Wa,
          Translate.GlobalDictionaries_ActionAuthTypeDictionary_отправлять_код_в_WhatsApp
        },
        {
          GlobalDictionaries.ActionAuthType.Call,
          Translate.GlobalDictionaries_ActionAuthTypeDictionary_осуществлять_звонок
        }
      };
    }

    public enum Skin
    {
      Default,
      Dark,
      Christmas,
    }

    public enum DocumentsStatuses
    {
      None,
      Draft,
      Open,
      Close,
      NoClose,
    }

    public enum DocumentsTypes
    {
      None,
      Sale,
      SaleReturn,
      Buy,
      BuyReturn,
      Move,
      MoveReturn,
      WriteOff,
      UserStockEdit,
      Inventory,
      InventoryAct,
      CafeOrder,
      ClientOrder,
      SetChildStockChange,
      MoveStorage,
      MoveStorageChild,
      LablePrint,
      ProductionItem,
      ProductionSet,
      ProductionList,
      ClientOrderReserve,
      BeerProductionItem,
      BeerProductionSet,
      BeerProductionList,
    }

    public enum EntityTypes
    {
      NotSet,
      Good,
      Client,
      Document,
      Window,
      GoodGroup,
      ClientGroup,
      Payment,
      User,
      ItemList,
      GoodStock,
      GroupEditGood,
      GroupEditGoodGroup,
    }

    public enum Format
    {
      Excel,
      Csv,
      Xml,
      Json,
      Yml,
    }

    public enum Encoding
    {
      CP866,
      CPTysso,
      W1251,
      Utf8,
      KOI8R,
    }

    public enum VersionUpdate
    {
      Release,
      Beta,
      Develop,
    }

    public static class Devices
    {
      public static Dictionary<GlobalDictionaries.Devices.TypePrintLable, string> TypePrintLableDictionary
      {
        get
        {
          return new Dictionary<GlobalDictionaries.Devices.TypePrintLable, string>()
          {
            {
              GlobalDictionaries.Devices.TypePrintLable.Standard,
              Translate.Devices_Стандартная_печать
            },
            {
              GlobalDictionaries.Devices.TypePrintLable.Zpl,
              Translate.Devices_ZPL_печать
            }
          };
        }
      }

      public static List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>> AcquiringTerminalTypesDictionary()
      {
        return new List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>>()
        {
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.None, GlobalDictionaries.Countries.NotSet, Translate.Devices_Нет),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.KkmServer, GlobalDictionaries.Countries.Russia, Translate.Devices_AcquiringTerminalTypesDictionary_ККМ_Сервер__kkmserver_ru_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.Sberbank, GlobalDictionaries.Countries.Russia, Translate.Devices_Сбербанк__pilot_nt_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.SBRF, GlobalDictionaries.Countries.Russia, "Сбербанк (SBRF)"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.Inpas, GlobalDictionaries.Countries.Russia, Translate.Devices__INPAS__DualConnector_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.Acrus2, GlobalDictionaries.Countries.Russia, "Arcus2"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.PrivatBank, GlobalDictionaries.Countries.Ukraine, Translate.Devices_ПриватБанк__Украина_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.OschadBank, GlobalDictionaries.Countries.Ukraine, Translate.Devices_AcquiringTerminalTypesDictionary_Ощадбанк__Украина_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.SmartPos, GlobalDictionaries.Countries.Kazakhstan, "Smart POS"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.AcquiringTerminalTypes>(GlobalDictionaries.Devices.AcquiringTerminalTypes.BPOS, GlobalDictionaries.Countries.Ukraine, "BPOS")
        };
      }

      public static List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>> SbpTypesDictionary()
      {
        return new List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>>()
        {
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>(GlobalDictionaries.Devices.SbpTypes.None, GlobalDictionaries.Countries.NotSet, Translate.Devices_Нет),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>(GlobalDictionaries.Devices.SbpTypes.NewPay, GlobalDictionaries.Countries.Russia, "Яндекс Пэй"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>(GlobalDictionaries.Devices.SbpTypes.OpenBank, GlobalDictionaries.Countries.Russia, "ОТКРЫТИЕ"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>(GlobalDictionaries.Devices.SbpTypes.PayMaster, GlobalDictionaries.Countries.Russia, "PayMaster"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.SbpTypes>(GlobalDictionaries.Devices.SbpTypes.AtolPay, GlobalDictionaries.Countries.Russia, "Atol Pay")
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.SbpVisualizationTypes, string> SbpVisualizationTypesDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.SbpVisualizationTypes, string>()
        {
          {
            GlobalDictionaries.Devices.SbpVisualizationTypes.Screen,
            Translate.Devices_SbpVisualizationTypesDictionary_Экран_монитора
          },
          {
            GlobalDictionaries.Devices.SbpVisualizationTypes.DisplayQR,
            Translate.Devices_SbpVisualizationTypesDictionary_Дисплей_для_вывода_QR_кода
          }
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.ScaleTypes, string> ScaleTypesDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.ScaleTypes, string>()
        {
          {
            GlobalDictionaries.Devices.ScaleTypes.None,
            Translate.Devices_Нет
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.CasComPort,
            Translate.Devices_CAS__через_COM_порт_
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.Wintec,
            Translate.Devices_Wintec
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.MassaK100,
            Translate.Devices_Масса_К__Драйвер_100_
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.ScalesMassaK,
            Translate.Devices_Масса_К__Драйвер_ScalesMassaK_
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.MassaKProtocol2,
            Translate.Devices_Масса_К__Протокол_2_
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.ShtrihM,
            Translate.Devices_АТОЛ_ШТРИХ_М_
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.IcsNt,
            Translate.Devices_ICS_NT_
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.DatalogicComPort,
            Translate.Devices_Datalogic_Magellan__через_COM_порт_
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.Bta,
            Translate.Devices_ScaleTypesDictionary_BTA_60__5__5A__6A__
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.Rongta,
            Translate.Devices_ScaleTypesDictionary_Rongta
          },
          {
            GlobalDictionaries.Devices.ScaleTypes.Pos2Mertech,
            "POS2-M/POS2-M Pro"
          }
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.ScaleLableTypes, string> ScaleLableTypesDictionary()
      {
        Dictionary<GlobalDictionaries.Devices.ScaleLableTypes, string> dictionary = new Dictionary<GlobalDictionaries.Devices.ScaleLableTypes, string>()
        {
          {
            GlobalDictionaries.Devices.ScaleLableTypes.None,
            Translate.Devices_Нет
          },
          {
            GlobalDictionaries.Devices.ScaleLableTypes.ShtrihM,
            Translate.Devices_ШТРИХ_М2
          },
          {
            GlobalDictionaries.Devices.ScaleLableTypes.ShtrihM200,
            Translate.Devices_ScaleLableTypesDictionary_ШТРИХ_М__РС_200_
          },
          {
            GlobalDictionaries.Devices.ScaleLableTypes.Cas,
            Translate.Devices_CAS
          },
          {
            GlobalDictionaries.Devices.ScaleLableTypes.ScaleManager,
            "ScaleManager"
          },
          {
            GlobalDictionaries.Devices.ScaleLableTypes.Rongta,
            Translate.Devices_ScaleTypesDictionary_Rongta
          },
          {
            GlobalDictionaries.Devices.ScaleLableTypes.MettlerToledo,
            "Mettler Toledo (Тайгер-П)"
          }
        };
        if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Armenia)
          dictionary.Remove(GlobalDictionaries.Devices.ScaleLableTypes.ScaleManager);
        return dictionary;
      }

      public static Dictionary<GlobalDictionaries.Devices.TsdTypes, string> TsdTypesDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.TsdTypes, string>()
        {
          {
            GlobalDictionaries.Devices.TsdTypes.None,
            Translate.Devices_Нет
          },
          {
            GlobalDictionaries.Devices.TsdTypes.MobileSmarts,
            "MobileSmarts (Клеверенс)"
          }
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.CasScaleLableTypes, string> CasScaleLableTypesDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.CasScaleLableTypes, string>()
        {
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.Lp16,
            "CAS LP-1.6 (LP-II)"
          },
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.CL5000JStatic,
            "CAS CL5000J Static memory"
          },
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.CL5000JDynamic,
            "CAS CL5000J Dynamic memory"
          },
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.CL5000,
            "CAS CL5000"
          },
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.CWM4000,
            "CAS CWM-4000"
          },
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.WMNano,
            "CAS WM-Nano"
          },
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.Primer5,
            "CAS Primer 5+"
          },
          {
            GlobalDictionaries.Devices.CasScaleLableTypes.CL3000,
            "CL3000/CL5000-D"
          }
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.ScannerTypes, string> ScannerTypesDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.ScannerTypes, string>()
        {
          {
            GlobalDictionaries.Devices.ScannerTypes.None,
            Translate.Devices_Нет
          },
          {
            GlobalDictionaries.Devices.ScannerTypes.KeyboardEmulation,
            Translate.Devices_Эмуляция_клавиатуры
          },
          {
            GlobalDictionaries.Devices.ScannerTypes.ComPort,
            Translate.Devices_COM_RS_232
          }
        };
      }

      public static Dictionary<DisplayBuyerTypes, string> DisplayBuyerTypesDictionary()
      {
        return new Dictionary<DisplayBuyerTypes, string>()
        {
          {
            DisplayBuyerTypes.None,
            Translate.Devices_Нет
          },
          {
            DisplayBuyerTypes.Atol8,
            Translate.Atol8_Name_АТОЛ_v_8
          },
          {
            DisplayBuyerTypes.ShtrihM,
            Translate.Devices_ШТРИХ_М2
          },
          {
            DisplayBuyerTypes.EscPos,
            Translate.Devices_DisplayBuyerTypesDictionary_ECS_POS
          }
        };
      }

      public static Dictionary<DisplayNumbersTypes, string> DisplayNumbersBuyerTypesDictionary()
      {
        return new Dictionary<DisplayNumbersTypes, string>()
        {
          {
            DisplayNumbersTypes.None,
            Translate.Devices_Нет
          },
          {
            DisplayNumbersTypes.EscPos,
            Translate.Devices_DisplayBuyerTypesDictionary_ECS_POS
          }
        };
      }

      public static Dictionary<DisplayQrTypes, string> DisplayQrTypesDictionary()
      {
        return new Dictionary<DisplayQrTypes, string>()
        {
          {
            DisplayQrTypes.None,
            Translate.Devices_Нет
          },
          {
            DisplayQrTypes.Mertech,
            "MERTECH"
          }
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.CheckPrinterTypes, string> CheckPrinterTypesDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.CheckPrinterTypes, string>()
        {
          {
            GlobalDictionaries.Devices.CheckPrinterTypes.None,
            Translate.Devices_Нет
          },
          {
            GlobalDictionaries.Devices.CheckPrinterTypes.UsualPrinter,
            Translate.Devices_CheckPrinterTypesDictionary_Windows_принтер
          },
          {
            GlobalDictionaries.Devices.CheckPrinterTypes.FiscalKkm,
            Translate.Devices_Фискальный_регистратор_ККМ
          }
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.FfdVersions, string> FfdVersionsDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.FfdVersions, string>()
        {
          {
            GlobalDictionaries.Devices.FfdVersions.OfflineKkm,
            "Оффлайн-касса"
          },
          {
            GlobalDictionaries.Devices.FfdVersions.Ffd100,
            "ФФД 1.0"
          },
          {
            GlobalDictionaries.Devices.FfdVersions.Ffd105,
            "ФФД 1.05"
          },
          {
            GlobalDictionaries.Devices.FfdVersions.Ffd110,
            "ФФД 1.1"
          },
          {
            GlobalDictionaries.Devices.FfdVersions.Ffd120,
            "ФФД 1.2"
          }
        };
      }

      public static Dictionary<GlobalDictionaries.Devices.ConnectionTypes, string> ConnectionTypesDictionary()
      {
        return new Dictionary<GlobalDictionaries.Devices.ConnectionTypes, string>()
        {
          {
            GlobalDictionaries.Devices.ConnectionTypes.NotSet,
            Translate.GlobalDictionaries_Не_указано
          },
          {
            GlobalDictionaries.Devices.ConnectionTypes.ComPort,
            Translate.Devices_Com_порт
          },
          {
            GlobalDictionaries.Devices.ConnectionTypes.Lan,
            Translate.Devices_TCP_IP
          }
        };
      }

      public static List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>> FiscalKkmTypesDictionary()
      {
        List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>> itemForCountryList = new List<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>>()
        {
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.None, GlobalDictionaries.Countries.NotSet, Translate.Devices_Нет),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Atol8, GlobalDictionaries.Countries.Russia, Translate.Devices_АТОЛ__драйвер_v_8___),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Atol10, GlobalDictionaries.Countries.Russia, Translate.Devices_АТОЛ__драйвер_v_10___),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.AtolServer, GlobalDictionaries.Countries.Russia, Translate.Devices_АТОЛ__Веб_сервер_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests, GlobalDictionaries.Countries.Russia, Translate.Devices_FiscalKkmTypesDictionary_АТОЛ__Web_Requests_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Shtrih, GlobalDictionaries.Countries.Russia, Translate.Devices_Штрих_М),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.VikiPrint, GlobalDictionaries.Countries.Russia, Translate.Devices_ВикиПринт__ДримКас_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.VikiDriver, GlobalDictionaries.Countries.Russia, Translate.Devices_FiscalKkmTypesDictionary_ВикиПринт__VikiDriver_WEB_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.KkmServer, GlobalDictionaries.Countries.Russia, Translate.Devices_FiscalKkmTypesDictionary_ККМ_Сервер__kkmserver_ru_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Mercury, GlobalDictionaries.Countries.Russia, Translate.Devices_FiscalKkmTypesDictionary_Меркурий__Служба_INECRMAN_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.HelpMicro, new List<GlobalDictionaries.Countries>()
          {
            GlobalDictionaries.Countries.Ukraine,
            GlobalDictionaries.Countries.Belarus
          }, Translate.Devices_Хелп_Микро__json_http_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.ExellioFP, GlobalDictionaries.Countries.Ukraine, Translate.Devices_Exellio_FP__Украина_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.MiniFP54, GlobalDictionaries.Countries.Ukraine, Translate.Devices_FiscalKkmTypesDictionary_MiniFP54__Украина_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.HiPos, GlobalDictionaries.Countries.Ukraine, Translate.Devices_FiscalKkmTypesDictionary_HiPos__Украина_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.PortFPGKZ, GlobalDictionaries.Countries.Kazakhstan, Translate.Devices__В_РАЗРАБОТКЕ_ + Translate.Devices_ПОРТ__Казахстан_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.WebKassa, GlobalDictionaries.Countries.Kazakhstan, Translate.Devices_FiscalKkmTypesDictionary_WEB_Kassa__Казахстан_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.ReKassa, GlobalDictionaries.Countries.Kazakhstan, Translate.Devices_FiscalKkmTypesDictionary_ReKassa__Казахстан_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.LeoCas, GlobalDictionaries.Countries.Ukraine, Translate.Devices_FiscalKkmTypesDictionary_ЛеоКАС__Украина_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.AzSmart, GlobalDictionaries.Countries.Azerbaijan, Translate.Devices__AZ_Smart__Азербайджан_),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.UzPos, GlobalDictionaries.Countries.Uzbekistan, "UzPos"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Neva, GlobalDictionaries.Countries.Russia, "НЕВА-03-Ф"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.SmartOne, GlobalDictionaries.Countries.Uzbekistan, "SmartOne"),
          new GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>(GlobalDictionaries.Devices.FiscalKkmTypes.Hdm, GlobalDictionaries.Countries.Armenia, "HDM")
        };
        if (!DevelopersHelper.IsDebug())
          itemForCountryList.RemoveAll((Predicate<GlobalDictionaries.ItemForCountry<GlobalDictionaries.Devices.FiscalKkmTypes>>) (x => x.Type == GlobalDictionaries.Devices.FiscalKkmTypes.AtolWebRequests));
        return itemForCountryList;
      }

      public enum TypePrintLable
      {
        Standard,
        Zpl,
      }

      public enum AcquiringTerminalTypes
      {
        None,
        KkmServer,
        Sberbank,
        Inpas,
        PrivatBank,
        Acrus2,
        OschadBank,
        SmartPos,
        BPOS,
        SBRF,
      }

      public enum CheckPrinterTypes
      {
        None = 0,
        UsualPrinter = 1,
        EscPos = 2,
        FiscalKkm = 4,
      }

      public enum ConnectionTypes
      {
        NotSet,
        ComPort,
        Lan,
      }

      public enum FfdVersions
      {
        OfflineKkm,
        Ffd100,
        Ffd105,
        Ffd110,
        Ffd120,
      }

      public enum FiscalKkmTypes
      {
        None,
        Atol8,
        Atol10,
        Shtrih,
        VikiPrint,
        KkmServer,
        Mercury,
        AtolServer,
        HelpMicro,
        ExellioFP,
        MiniFP54,
        UzFiscalModule,
        PortFPGKZ,
        FsPRRO,
        LeoCas,
        AzSmart,
        WebKassa,
        ReKassa,
        HiPos,
        UzPos,
        DevicesConnector,
        MobilKassa,
        Neva,
        AtolWebRequests,
        SmartOne,
        VikiDriver,
        Hdm,
      }

      public enum SbpTypes
      {
        None,
        OpenBank,
        PayMaster,
        NewPay,
        AtolPay,
      }

      public enum SbpVisualizationTypes
      {
        Screen,
        DisplayQR,
      }

      public enum ScaleLableTypes
      {
        None,
        ShtrihM,
        Cas,
        Rongta,
        MettlerToledo,
        ScaleManager,
        ShtrihM200,
      }

      public enum CasScaleLableTypes
      {
        Lp16 = 1,
        CL5000JStatic = 2,
        CL5000JDynamic = 3,
        CL5000 = 4,
        CWM4000 = 5,
        WMNano = 6,
        Primer5 = 7,
        CL3000 = 8,
      }

      public enum TsdTypes
      {
        None,
        Atol,
        MobileSmarts,
      }

      public enum ScaleTypes
      {
        None = 0,
        CasComPort = 1,
        Wintec = 2,
        MassaK100 = 3,
        ScalesMassaK = 4,
        ShtrihM = 5,
        IcsNt = 6,
        AtolScaner = 7,
        DatalogicComPort = 9,
        MassaKProtocol2 = 10, // 0x0000000A
        Bta = 11, // 0x0000000B
        Rongta = 12, // 0x0000000C
        Pos2Mertech = 13, // 0x0000000D
      }

      public enum ScannerTypes
      {
        None,
        KeyboardEmulation,
        ComPort,
      }
    }

    public enum Mode
    {
      Shop,
      Cafe,
      Home,
      None,
    }

    public enum ClientSyncModes
    {
      None,
      FileSync,
      PolyCloud,
    }

    public enum GoodTypes
    {
      Single = 0,
      Weight = 1,
      Service = 2,
      Certificate = 4,
    }

    public enum RuMarkedProductionTypes
    {
      None = 0,
      Fur = 1,
      Drugs = 2,
      Tobacco = 3,
      Shoes = 4,
      Perfume = 5,
      Tires = 6,
      LightIndustry = 7,
      Milk = 8,
      Water = 9,
      Photo = 10, // 0x0000000A
      Other = 11, // 0x0000000B
      Alcohol = 12, // 0x0000000C
      Ua_Alcohol = 100, // 0x00000064
      Kz_Shoes = 200, // 0x000000C8
      Arm_Alcohol = 300, // 0x0000012C
      Arm_Tobacco = 310, // 0x00000136
    }

    public enum RuTaxSystems
    {
      None = -1, // 0xFFFFFFFF
      Osn = 0,
      UsnDohod = 1,
      UsnDohodMinusRashod = 2,
      Envd = 3,
      Esn = 4,
      Psn = 5,
    }

    public enum RuFfdGoodsTypes
    {
      None = 0,
      SimpleGood = 1,
      ExcisableGood = 2,
      Work = 3,
      Service = 4,
      GamePayment = 5,
      GameWin = 6,
      LotteryPayment = 7,
      LotteryWin = 8,
      Rid = 9,
      Payment = 10, // 0x0000000A
      AgentPayment = 11, // 0x0000000B
      WithPayment = 12, // 0x0000000C
      Other = 13, // 0x0000000D
      PropertyLaw = 14, // 0x0000000E
      NonOperatingIncome = 15, // 0x0000000F
      OtherPayment = 16, // 0x00000010
      TradeFee = 17, // 0x00000011
      ResortFee = 18, // 0x00000012
      Deposit = 19, // 0x00000013
      Expenditure = 20, // 0x00000014
      PensionInsuranceIP = 21, // 0x00000015
      PensionInsurance = 22, // 0x00000016
      MedicalInsuranceIP = 23, // 0x00000017
      MedicalInsurance = 24, // 0x00000018
      SocialInsurance = 25, // 0x00000019
      CasinoPayment = 26, // 0x0000001A
      OutOfFunds = 27, // 0x0000001B
      Atnm = 30, // 0x0000001E
      Atm = 31, // 0x0000001F
      Tnm = 32, // 0x00000020
      Tm = 33, // 0x00000021
    }

    public enum KkmPaymentMethods
    {
      Cash,
      Card,
      Bank,
      Bonus,
      Certificate,
      Credit,
      PrePayment,
      EMoney,
    }

    public enum PaymentMethodsType
    {
      NoBroker,
      Acquiring,
      Sbp,
    }

    public enum RuFfdPaymentModes
    {
      None,
      PrePaymentFull,
      Prepayment,
      AdvancePayment,
      FullPayment,
      PartPaymentAndCredit,
      FullCredit,
      PaymentForCredit,
    }

    public enum EntityPropertyTypes
    {
      Text,
      Integer,
      Decimal,
      DateTime,
      AutoNum,
      System,
    }

    public enum GoodsSetStatuses
    {
      None,
      Set,
      Kit,
      Range,
      Production,
      AllStatus,
    }

    public enum BarcodeIfEmpty
    {
      Empty,
      Generate,
      Skip,
    }

    public enum BarcodeIfReplay
    {
      Empty,
      Generate,
      Skip,
    }

    public enum CertificateStatus
    {
      Open,
      Saled,
      Close,
      All,
    }

    public enum PaymentTypes
    {
      MoneyPayment,
      MoneyDocumentPayment,
      MoneyMovement,
      MoneyCorrection,
      BonusesDocumentPayment,
      BonusesDocumentItemPayment,
      BonusesCorrection,
      Prepaid,
      CheckDiscount,
      BalanceCorrection,
      RecountSumCash,
    }

    public enum Languages
    {
      System,
      Russian,
      Ukrainian,
      Azerbaijani,
      Kazakh,
      Uzbek,
      Armenian,
      English,
    }

    public enum Countries
    {
      NotSet,
      Russia,
      Ukraine,
      Azerbaijan,
      Kazakhstan,
      Belarus,
      Uzbekistan,
      Armenia,
    }

    public class Language
    {
      public string Key { get; set; }

      public string Name { get; set; }

      public GlobalDictionaries.Languages Value { get; set; }
    }

    public class ItemForCountry<T>
    {
      public T Type { get; }

      public List<GlobalDictionaries.Countries> Country { get; }

      public string TypeName { get; }

      public ItemForCountry(T type, List<GlobalDictionaries.Countries> country, string name)
      {
        this.Type = type;
        this.Country = country;
        this.TypeName = name;
      }

      public ItemForCountry(T type, GlobalDictionaries.Countries country, string name)
      {
        this.Type = type;
        this.Country = new List<GlobalDictionaries.Countries>()
        {
          country
        };
        this.TypeName = name;
      }
    }

    public enum SmsServiceType
    {
      None,
      SmsRu,
      SmsAgent,
      SmsCenter,
      SmsAero,
    }

    public enum ActionAuthType
    {
      None,
      Sms,
      Call,
      Wa,
      Viber,
    }
  }
}
