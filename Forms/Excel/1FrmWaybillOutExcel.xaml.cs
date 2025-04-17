// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Excel.HelpClassExcel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Helpers;
using Gbs.Helpers.Excel;
using Gbs.Helpers.Logging;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gbs.Forms.Excel
{
  public class HelpClassExcel
  {
    private Gbs.Core.Config.Devices _configDevices;

    public List<GoodGroups.Group> Groups { get; }

    private Gbs.Core.Config.Devices ConfigDevices
    {
      get
      {
        if (this._configDevices == null)
          this._configDevices = new ConfigsRepository<Gbs.Core.Config.Devices>().Get();
        return this._configDevices;
      }
    }

    public HelpClassExcel()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        this.Groups = new GoodGroupsRepository(dataBase).GetActiveItems();
    }

    public static Decimal RoundSum(Decimal sum, Decimal coeff)
    {
      try
      {
        return coeff == 0M ? sum : Math.Round(Math.Ceiling(sum / coeff) * coeff, 2);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при округлении цены", false);
        return 0M;
      }
    }

    public string GetBarcode(
      IRow row,
      int barcodeIndex,
      GlobalDictionaries.BarcodeIfEmpty selectedIndexIfEmpty)
    {
      string[] strArray = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().BarcodeScanner.Prefixes.RandomGenerated.Split(GlobalDictionaries.SplitArr);
      string prefix = "";
      if (strArray.Length != 0)
        prefix = strArray[0];
      string stringCellValue = ExcelCellValueReader.GetStringCellValue(row, barcodeIndex);
      if (!string.IsNullOrEmpty(stringCellValue))
        return stringCellValue;
      switch (selectedIndexIfEmpty)
      {
        case GlobalDictionaries.BarcodeIfEmpty.Empty:
          return string.Empty;
        case GlobalDictionaries.BarcodeIfEmpty.Generate:
          return BarcodeHelper.RandomBarcode(prefix);
        case GlobalDictionaries.BarcodeIfEmpty.Skip:
          return (string) null;
        default:
          return (string) null;
      }
    }

    public IEnumerable<string> GetAllBarcodes(IRow row, int barcodesIndex)
    {
      string stringCellValue = ExcelCellValueReader.GetStringCellValue(row, barcodesIndex);
      if (string.IsNullOrEmpty(stringCellValue))
        return (IEnumerable<string>) new List<string>();
      return (IEnumerable<string>) ((IEnumerable<string>) stringCellValue.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
    }

    public GoodGroups.Group GetGroup(
      IRow row,
      int groupIndex,
      bool isEnabled,
      bool isNewGroupAdd,
      Guid defaultGroupUid,
      Guid emptyGroupUid,
      Guid extraGroupUid,
      List<GoodGroups.Group> saveGroup)
    {
      if (!isEnabled)
      {
        LogHelper.Debug("Присвоена группа по-умлочанию при добавлении из Excel, UID = " + defaultGroupUid.ToString());
        return this.Groups.FirstOrDefault<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.Uid == defaultGroupUid));
      }
      string groupName = ExcelCellValueReader.GetStringCellValue(row, groupIndex);
      if (string.IsNullOrEmpty(groupName))
      {
        LogHelper.Debug("Присвоена группа 'если пусто', которая уже есть в программе (ячейка с категорией пустая), UID = " + emptyGroupUid.ToString());
        return this.Groups.FirstOrDefault<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.Uid == emptyGroupUid));
      }
      if (this.Groups.Any<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => string.Equals(x.Name.Trim(), groupName.Trim(), StringComparison.CurrentCultureIgnoreCase))))
      {
        GoodGroups.Group group = this.Groups.FirstOrDefault<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => string.Equals(x.Name.Trim(), groupName.Trim(), StringComparison.CurrentCultureIgnoreCase)));
        LogHelper.Debug("Присвоена группа из файла, которая уже есть в программе (совпадение по названию), UID = " + group?.Uid.ToString());
        return group;
      }
      if (!isNewGroupAdd)
      {
        LogHelper.Debug("Присвоена группа 'extraGroupUid', которая уже есть в программе, UID = " + extraGroupUid.ToString());
        return this.Groups.FirstOrDefault<GoodGroups.Group>((Func<GoodGroups.Group, bool>) (x => x.Uid == extraGroupUid));
      }
      LogHelper.Debug("Не нашли совпадение для категории " + groupName);
      GoodGroups.Group group1 = new GoodGroups.Group()
      {
        Name = groupName
      };
      this.Groups.Add(group1);
      saveGroup.Add(group1);
      LogHelper.Debug("Присвоена новая категория, создана из файла Excel, UID = " + group1.Uid.ToString());
      return group1;
    }
  }
}
