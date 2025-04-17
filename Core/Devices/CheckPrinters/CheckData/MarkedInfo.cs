// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.CheckData.MarkedInfo
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.CheckData
{
  public class MarkedInfo
  {
    private string _gtin;
    private string _serial;
    private string _checkKey = string.Empty;
    private const int GtinLen = 14;
    private const int SerialLen = 13;
    private const int KeyLen = 4;

    public GlobalDictionaries.RuMarkedProductionTypes Type { get; set; }

    public object ValidationResultKkm { get; set; }

    public string Gtin
    {
      get
      {
        this.ParseMark();
        return this._gtin;
      }
    }

    public string Serial
    {
      get
      {
        this.ParseMark();
        return this._serial;
      }
    }

    public string CheckKey
    {
      get
      {
        this.ParseMark();
        return this._checkKey;
      }
    }

    public string FullCode { get; set; }

    public MarkedInfo(string fullCode, GlobalDictionaries.RuMarkedProductionTypes type)
    {
      if (type == GlobalDictionaries.RuMarkedProductionTypes.None)
        return;
      fullCode = fullCode.Trim();
      fullCode = this.CorrectFncSymbols(fullCode, type);
      this.FullCode = fullCode;
      this.Type = type;
      LogHelper.Debug(string.Format("Код маркировки: {0}, тип: {1}", (object) this.ToJsonString(true), (object) type));
    }

    private string CorrectFncSymbols(string code, GlobalDictionaries.RuMarkedProductionTypes type)
    {
      string str = code;
      if (new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Russia || new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120 || code.Contains<char>(' ') || code.Contains<char>(Convert.ToChar(29)))
        return code;
      switch (type)
      {
        case GlobalDictionaries.RuMarkedProductionTypes.None:
          return code;
        case GlobalDictionaries.RuMarkedProductionTypes.Fur:
label_37:
          LogHelper.Trace("Код маркировки был скорректирован. Old: " + str + "; new: " + code);
          return code;
        case GlobalDictionaries.RuMarkedProductionTypes.Drugs:
          if (code.Length == 83)
            break;
          goto default;
        case GlobalDictionaries.RuMarkedProductionTypes.Tobacco:
          if (code.Length > 40)
          {
            if (code.Substring(25, 4) == "8005")
              code = this.AddSpaceInCode(code, 25);
            if (code.Substring(36, 2) == "93")
            {
              code = this.AddSpaceInCode(code, 36);
              goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
            }
            else
              goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
          }
          else if (code.Length == 31)
            goto label_26;
          else
            goto default;
        case GlobalDictionaries.RuMarkedProductionTypes.Shoes:
          if (code.Length == (int) sbyte.MaxValue)
            break;
          goto default;
        case GlobalDictionaries.RuMarkedProductionTypes.Perfume:
          if (code.Length == 83)
            break;
          goto default;
        case GlobalDictionaries.RuMarkedProductionTypes.Tires:
          if (code.Length == 83)
            break;
          goto default;
        case GlobalDictionaries.RuMarkedProductionTypes.LightIndustry:
          if (code.Length == (int) sbyte.MaxValue || code.Length == 83)
            break;
          goto default;
        case GlobalDictionaries.RuMarkedProductionTypes.Milk:
          if ((code.Length == 30 || code.Length == 40) && code.Substring(24, 2) == "93")
            code = this.AddSpaceInCode(code, 24);
          if (code.Length == 41 && code.Substring(31, 4) == "3103")
          {
            code = this.AddSpaceInCode(code, 31);
            goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
          }
          else
            goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
        case GlobalDictionaries.RuMarkedProductionTypes.Water:
          if (code.Length == 37 && code.Substring(31, 2) == "93")
          {
            code = this.AddSpaceInCode(code, 31);
            goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
          }
          else
            goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
        case GlobalDictionaries.RuMarkedProductionTypes.Photo:
          if (code.Length == 90)
          {
            if (code.Substring(38, 2) == "91")
              code = this.AddSpaceInCode(code, 38);
            if (code.Substring(45, 2) == "92")
            {
              code = this.AddSpaceInCode(code, 45);
              goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
            }
            else
              goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
          }
          else
            goto case GlobalDictionaries.RuMarkedProductionTypes.Fur;
        case GlobalDictionaries.RuMarkedProductionTypes.Other:
          if (code.Length != 83)
          {
            if (code.Length != 30)
              goto case GlobalDictionaries.RuMarkedProductionTypes.Water;
            else
              goto case GlobalDictionaries.RuMarkedProductionTypes.Milk;
          }
          else
            break;
        case GlobalDictionaries.RuMarkedProductionTypes.Alcohol:
          if (code.Length != 31)
            goto case GlobalDictionaries.RuMarkedProductionTypes.Water;
          else
            goto label_26;
        case GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol:
        case GlobalDictionaries.RuMarkedProductionTypes.Kz_Shoes:
        case GlobalDictionaries.RuMarkedProductionTypes.Arm_Alcohol:
        case GlobalDictionaries.RuMarkedProductionTypes.Arm_Tobacco:
          return code;
        default:
          return code;
      }
      if (code.Substring(31, 2) == "91")
        code = this.AddSpaceInCode(code, 31);
      if (code.Substring(38, 2) == "92")
      {
        code = this.AddSpaceInCode(code, 38);
        goto label_37;
      }
      else
        goto label_37;
label_26:
      if (code.Length == 31 && code.Substring(25, 2) == "93")
      {
        code = this.AddSpaceInCode(code, 25);
        goto label_37;
      }
      else
        goto label_37;
    }

    private string AddSpaceInCode(string code, int index)
    {
      return code.Substring(0, index) + " " + code.Substring(index);
    }

    private bool CheckValidLenght()
    {
      if (this.Type.IsEither<GlobalDictionaries.RuMarkedProductionTypes>(GlobalDictionaries.RuMarkedProductionTypes.None, GlobalDictionaries.RuMarkedProductionTypes.Fur))
        return true;
      if (this.Type == GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol)
        return this.FullCode.Length >= 9;
      return this.FullCode.Length >= 25 && this.FullCode.Length <= 200;
    }

    public bool IsValidCode()
    {
      this.FullCode = this.FullCode.Trim();
      if (new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.FfdVersion != GlobalDictionaries.Devices.FfdVersions.Ffd120)
        this.FullCode = this.FullCode.Replace(" ", string.Empty);
      if (new ConfigsRepository<Settings>().Get().Interface.Country == GlobalDictionaries.Countries.Ukraine)
      {
        this.FullCode = this.FullCode.ToUpper();
        if (new Regex("^[A-Z]{4}\\d{6}$").Match(this.FullCode).Success)
          return true;
        LogHelper.Debug("Код маркировки содержит недопустимые символы для региона UA");
        return false;
      }
      if (!this.CheckValidLenght())
        return false;
      if (new Regex("[АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя]").Match(this.FullCode).Success)
      {
        LogHelper.Debug("Код маркировки содержит недопустимые символы (кириллица)");
        return false;
      }
      if (this.Gtin == null || !this.Gtin.Any<char>((Func<char, bool>) (g => !char.IsDigit(g))))
        return true;
      LogHelper.Debug("GTIN должен содержать только цифры");
      return false;
    }

    private void ParseMark()
    {
      if (!this.CheckValidLenght())
      {
        LogHelper.Debug("Некорректная длина кода маркировки");
      }
      else
      {
        switch (this.Type)
        {
          case GlobalDictionaries.RuMarkedProductionTypes.None:
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Fur:
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Drugs:
          case GlobalDictionaries.RuMarkedProductionTypes.Shoes:
          case GlobalDictionaries.RuMarkedProductionTypes.Perfume:
          case GlobalDictionaries.RuMarkedProductionTypes.Tires:
          case GlobalDictionaries.RuMarkedProductionTypes.LightIndustry:
          case GlobalDictionaries.RuMarkedProductionTypes.Milk:
          case GlobalDictionaries.RuMarkedProductionTypes.Photo:
          case GlobalDictionaries.RuMarkedProductionTypes.Other:
          case GlobalDictionaries.RuMarkedProductionTypes.Alcohol:
            this.VariousCodeParse();
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Tobacco:
            this.TobaccoCodeParse();
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Water:
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Ua_Alcohol:
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Kz_Shoes:
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Arm_Alcohol:
            break;
          case GlobalDictionaries.RuMarkedProductionTypes.Arm_Tobacco:
            break;
          default:
            throw new ArgumentOutOfRangeException("Type", (object) this.Type, (string) null);
        }
      }
    }

    private void VariousCodeParse()
    {
      if (this.FullCode.Length < 31)
        return;
      this._gtin = this.FullCode.Substring(2, 14);
      this._serial = this.FullCode.Substring(18, 13);
      if (this.FullCode.Length < 37)
        return;
      this._checkKey = this.FullCode.Substring(33, 4);
    }

    private void TobaccoCodeParse()
    {
      this._gtin = this.FullCode.Substring(0, 14);
      this._serial = this.FullCode.Substring(14, 11) + new string(' ', 2);
      if (this.FullCode.Length != 29)
        return;
      this._checkKey = this.FullCode.Substring(25, 4);
    }

    public string GetHexStringAttribute()
    {
      string prefixAsString = this.GetPrefixAsString();
      if (prefixAsString == string.Empty)
        return string.Empty;
      string str1 = BigInteger.Parse(this.Gtin).ToString("X").PadLeft(12, '0');
      foreach (int num in this.Serial)
      {
        string str2 = num.ToString("X").PadLeft(2, '0');
        str1 += str2;
      }
      return Regex.Replace(prefixAsString + str1, ".{2}", "$0 ").Trim().ToUpper();
    }

    public string GetPrefixAsString()
    {
      string prefixAsString;
      switch (this.Type)
      {
        case GlobalDictionaries.RuMarkedProductionTypes.None:
          prefixAsString = string.Empty;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Fur:
          prefixAsString = string.Empty;
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Drugs:
          prefixAsString = "444d";
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Tobacco:
          prefixAsString = "444d";
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Shoes:
          prefixAsString = "444d";
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Perfume:
          prefixAsString = "444d";
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.Tires:
          prefixAsString = "444d";
          break;
        case GlobalDictionaries.RuMarkedProductionTypes.LightIndustry:
          prefixAsString = "444d";
          break;
        default:
          prefixAsString = string.Empty;
          break;
      }
      return prefixAsString;
    }

    public int GetPrefixAsInt()
    {
      string prefixAsString = this.GetPrefixAsString();
      return !prefixAsString.IsNullOrEmpty() ? Convert.ToInt32(prefixAsString, 16) : 0;
    }
  }
}
