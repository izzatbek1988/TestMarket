// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Armenia.HdmDriver
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Gbs.Core.Devices.CheckPrinters.FiscalKkm.Models.Armenia
{
  public class HdmDriver : IDisposable
  {
    public static byte[] _cryptKey;
    private TcpClient _tcpClient;
    public static string _password;
    public static int _seq = 1;
    public int _port;
    public string _host;
    [Localizable(false)]
    private Dictionary<int, string> Errors = new Dictionary<int, string>()
    {
      {
        500,
        "ՀԴՄ ներքին սխալ"
      },
      {
        400,
        "Հարցման սխալ"
      },
      {
        402,
        "Սխալ արձանագրության տարբերակ"
      },
      {
        403,
        "Չարտոնագրված միացում"
      },
      {
        404,
        "Սխալ գործողության կոդ"
      },
      {
        101,
        "Գաղտնաբառով կոդավորման սխալ"
      },
      {
        102,
        "Սեսիայի բանալիով կոդավորման սխալ"
      },
      {
        103,
        "Գլխագրի ֆորմատի սխալ"
      },
      {
        104,
        "հարցման հերթական"
      },
      {
        105,
        "JSON ֆորմատավորման սխալ"
      },
      {
        141,
        "Վերջին կտրոնի գրառումը բացակայում է"
      },
      {
        142,
        "Վերջին կտրոնը պատկանում է այլ օգտատերոջը"
      },
      {
        143,
        "Տպիչի ընդհանուր սխալ"
      },
      {
        144,
        "Տպիչի ինիցիալիզացիայի սխալ"
      },
      {
        145,
        "Տպիչում վերջացել է թուղթը"
      },
      {
        151,
        "Այդպիսի բաժին գոյություն չունի"
      },
      {
        152,
        "Մուծված գումարը ընդհանուր գումարից պակաս"
      },
      {
        153,
        "Կտրոնի գումարը գերազանզում է սահմանված շեմը"
      },
      {
        154,
        "Կտրոնի գումարը պետք է լինի դրական թիվ"
      },
      {
        155,
        "Անհրաժեշտ է համաժամանակեցնել ՀԴՄ-ն"
      },
      {
        157,
        "Սխալ վերադարձի կտրոնի համար"
      },
      {
        158,
        "Կտրոնը արդեն վերադարձված է"
      },
      {
        159,
        "Ապրանքի գինը և քանակը չի կարող լինել ոչ դրական"
      },
      {
        160,
        "Զեղչի տոկոսը պետք է լինի ոչ բաց"
      },
      {
        161,
        "Ապրանքի կոդը չի կարող լինել դատարկ"
      },
      {
        162,
        "Ապրանքի անվանումը չի կարող լինել դատարկ"
      },
      {
        163,
        "Ապրանքի չափման միավորը չի կարող լինել դատարկ"
      },
      {
        164,
        "Անկանխիկ վճարման խափանում"
      },
      {
        165,
        "Ապրանքի գինը չի կարող լինել 0"
      },
      {
        166,
        "Վերջնական գնի հաշվարկի սխալ"
      },
      {
        167,
        "Անկանխիկ գումարը ավելի մեծ է քան կտրոնի ընդհան"
      },
      {
        168,
        "Անկանխիկ գումարը ծածկում է ընդհանուր գումարը (Կանխիկ գումարը ավելորդ է)"
      },
      {
        169,
        "Ֆիսկալ հաշվետվության ֆիլտերների սխալ ընտրություն (մեկից ավել ֆիլտերի դաշտ է ուղարկվել)"
      },
      {
        170,
        "Ֆիսկալ հաշվետվության ժամանակ սխալ ամսաթվային միջակայք է ուղարկվել: Միջակայքը չպետք է գերազանցի 2 ամիսը"
      },
      {
        171,
        "Արագ գնի արգելական փաստաթուղթի սխալ"
      },
      {
        173,
        "Հ-երթ խափանվում է"
      },
      {
        174,
        "Հարցման ընթացքում առավելագույնից շատ գրառումների քանակը երրորդում է"
      },
      {
        175,
        "Պայմանագրի տեսակի սխալ"
      },
      {
        176,
        "Պայմանագրված տվյալները անվավեր են"
      },
      {
        177,
        "Տեղափոխել չափի վերաբեռնող ֆայլի անունը չի հասանելի"
      },
      {
        180,
        "Պարտադիր տվյալները բացակայում են"
      },
      {
        181,
        "Վերադարձվող ապրանքի սխալ քանակ"
      },
      {
        182,
        "Վերադարձվող կտրոնը իրենից վերադարձ տիպի կտրոն է ներկայացնում "
      },
      {
        183,
        "Սխալ ԱԴԳ կոդ"
      },
      {
        184,
        "Կանխավճարի վերադարձի անթույլատրելի հարցում"
      },
      {
        185,
        "Հնարավոր չէ կատարել տվյալ կտրոնի վերադարձը: Անհրաժեշտ է ՀԴՄ ծրագրի համաժամանակեցում"
      },
      {
        186,
        "Կանխավճարի դեպքում սխալ գումար"
      },
      {
        187,
        "Կանխավճարի դեպքում սխալ ցուցակ"
      },
      {
        188,
        "Սխալ գումարներ"
      },
      {
        189,
        "Սխալ կլորացում"
      },
      {
        190,
        "Վճարումը հասանելի չէ"
      },
      {
        191,
        "Կանխիկի մուտք ելքի ժամանակ գումարը պետքէ լինի մեծ 0-ից"
      }
    };

    private void CheckError(byte b1, byte b2)
    {
      int key = (int) b1 << 8 | (int) b2;
      if (key != 200)
      {
        string str1;
        string str2 = this.Errors.TryGetValue(key, out str1) ? str1 : Translate.HdmDriver_CheckError_Возникла_неизвестная_ошибка__обратитесь_в_службу_технической_поддержки_;
        LogHelper.Debug("Возникла ошибка на HDM (Армения).");
        LogHelper.Debug(str2 + string.Format(" (код ошибки {0})", (object) key));
        throw new DeviceException(string.Format(Translate.HdmDriver_CheckError__0___код_ошибки__1__, (object) str2, (object) key));
      }
    }

    private byte[] HashPassword(string password)
    {
      using (SHA256 shA256 = SHA256.Create())
      {
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        return ((IEnumerable<byte>) shA256.ComputeHash(bytes)).Take<byte>(24).ToArray<byte>();
      }
    }

    private byte[] Decrypt(byte[] cipher)
    {
      byte[] numArray = cipher.Length >= 15 ? new byte[cipher.Length - 11] : throw new Exception("Invalid cipher");
      Array.Copy((Array) cipher, 11, (Array) numArray, 0, numArray.Length);
      using (TripleDES tripleDes = TripleDES.Create())
      {
        tripleDes.Key = HdmDriver._cryptKey;
        tripleDes.Mode = CipherMode.ECB;
        tripleDes.Padding = PaddingMode.PKCS7;
        using (ICryptoTransform decryptor = tripleDes.CreateDecryptor())
          return decryptor.TransformFinalBlock(numArray, 0, numArray.Length);
      }
    }

    private byte[] Encrypt(byte[] plain)
    {
      using (TripleDES tripleDes = TripleDES.Create())
      {
        tripleDes.Key = HdmDriver._cryptKey;
        tripleDes.Mode = CipherMode.ECB;
        tripleDes.Padding = PaddingMode.PKCS7;
        using (ICryptoTransform encryptor = tripleDes.CreateEncryptor())
          return encryptor.TransformFinalBlock(plain, 0, plain.Length);
      }
    }

    public HdmDriver(string host, int port, string password)
    {
      this._host = host;
      this._port = port;
      HdmDriver._cryptKey = this.HashPassword(password);
      HdmDriver._password = password;
    }

    public void DoCommand(HdmDriver.HdmCommand command)
    {
      this._tcpClient = new TcpClient(this._host, this._port);
      try
      {
        string hexString = "D580D4B4D584000501000000";
        byte[] array1 = Enumerable.Range(0, hexString.Length).Where<int>((Func<int, bool>) (x => x % 2 == 0)).Select<int, byte>((Func<int, byte>) (x => Convert.ToByte(hexString.Substring(x, 2), 16))).ToArray<byte>();
        string jsonString = command.ToJsonString(isIgnoreNull: true);
        LogHelper.Debug("JSON для передачи " + jsonString);
        byte[] second = this.Encrypt(Encoding.UTF8.GetBytes(jsonString));
        byte[] array2 = ((IEnumerable<byte>) array1).Concat<byte>((IEnumerable<byte>) second).ToArray<byte>();
        array2[8] = command.CodeOperation;
        array2[10] = (byte) ((array2.Length - 12) / 256);
        array2[11] = (byte) ((array2.Length - 12) % 256);
        ServicePointManager.Expect100Continue = false;
        LogHelper.Debug("Data to send: " + string.Join<byte>(" ", (IEnumerable<byte>) array2));
        NetworkStream stream = this._tcpClient.GetStream();
        LogHelper.Debug("Подключение");
        stream.Write(array2, 0, array2.Length);
        LogHelper.Debug("Данные записаны");
        List<byte> byteList = new List<byte>();
        byte[] numArray = new byte[256];
        do
        {
          int count = stream.Read(numArray, 0, numArray.Length);
          byteList.AddRange(((IEnumerable<byte>) numArray).Take<byte>(count));
        }
        while (stream.DataAvailable);
        byte[] array3 = byteList.ToArray();
        LogHelper.Debug(string.Join(", ", ((IEnumerable<byte>) array3).Select<byte, string>((Func<byte, string>) (item => "0x" + item.ToString("x2")))));
        if (array3.Length < 11)
          throw new KkmException((IDevice) new Mercury(), Translate.HrantDriver_DoCommand_Ответ_от_ККМ_не_содержит_данных__Возможно__связь_с_ККМ_не_была_установлена);
        this.CheckError(array3[5], array3[6]);
        if (array3.Length == 11)
        {
          LogHelper.Debug("Метод предполагаемо ничего не вернул");
        }
        else
        {
          byte[] bytes = this.Decrypt(array3);
          command.AnswerString = Encoding.UTF8.GetString(bytes);
          LogHelper.Debug("AnswerString = " + command.AnswerString);
          if (!(command is HdmDriver.LoginCommand loginCommand))
            return;
          HdmDriver._cryptKey = Convert.FromBase64String(loginCommand.Result.Key);
          LogHelper.Debug("Новый крипто кей " + HdmDriver._cryptKey?.ToString());
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при передаче данных Hdm", false);
        throw ex;
      }
      finally
      {
        this._tcpClient?.Close();
        this._tcpClient?.Dispose();
      }
    }

    public void Dispose()
    {
      this._tcpClient?.Close();
      this._tcpClient?.Dispose();
    }

    public class HdmCommand
    {
      [JsonIgnore]
      public virtual byte CodeOperation { get; }

      [JsonIgnore]
      public string AnswerString { get; set; }
    }

    public class HdmAnswer
    {
    }

    public class LoginCommand : HdmDriver.HdmCommand
    {
      public override byte CodeOperation => 2;

      [JsonProperty("password")]
      public string Password => HdmDriver._password;

      [JsonProperty("cashier")]
      public int CashierId { get; set; }

      [JsonProperty("pin")]
      public string Pin { get; set; }

      [JsonIgnore]
      public HdmDriver.LoginCommand.LoginAnswer Result
      {
        get => JsonConvert.DeserializeObject<HdmDriver.LoginCommand.LoginAnswer>(this.AnswerString);
      }

      public class LoginAnswer : HdmDriver.HdmAnswer
      {
        [JsonProperty("key")]
        public string Key { get; set; }
      }
    }

    public class LoginOutCommand : HdmDriver.HdmCommand
    {
      public override byte CodeOperation => 3;

      [JsonProperty("seq")]
      public int Seq => HdmDriver._seq;

      [JsonIgnore]
      public HdmDriver.HdmAnswer Result
      {
        get => JsonConvert.DeserializeObject<HdmDriver.HdmAnswer>(this.AnswerString);
      }
    }

    public class MoneyCommand : HdmDriver.HdmCommand
    {
      public override byte CodeOperation => 11;

      [JsonProperty("seq")]
      public int Seq => HdmDriver._seq;

      [JsonProperty("amount")]
      public double Amount { get; set; }

      [JsonProperty("isCashIn")]
      public bool IsCashIn { get; set; }

      [JsonProperty("cashierId")]
      public int CashierId { get; set; }

      [JsonProperty("description")]
      public string Description { get; set; }

      [JsonIgnore]
      public HdmDriver.HdmAnswer Result
      {
        get => JsonConvert.DeserializeObject<HdmDriver.HdmAnswer>(this.AnswerString);
      }
    }

    public class ReportCommand : HdmDriver.HdmCommand
    {
      public override byte CodeOperation => 9;

      [JsonProperty("seq")]
      public int Seq => HdmDriver._seq;

      [JsonProperty("reportType")]
      public HdmDriver.ReportCommand.ReportType Type { get; set; }

      [JsonProperty("startDate")]
      public double? StartDate { get; set; }

      [JsonProperty("endDate")]
      public double? EndDate { get; set; }

      [JsonIgnore]
      public HdmDriver.HdmAnswer Result
      {
        get => JsonConvert.DeserializeObject<HdmDriver.HdmAnswer>(this.AnswerString);
      }

      public enum ReportType
      {
        XReport = 1,
        ZReport = 2,
      }
    }

    public class CheckCommand : HdmDriver.HdmCommand
    {
      public override byte CodeOperation => 4;

      [JsonProperty("seq")]
      public int Seq => HdmDriver._seq;

      [JsonProperty("paidAmount")]
      public double SumCash { get; set; }

      [JsonProperty("paidAmountCard")]
      public double SumCard { get; set; }

      [JsonProperty("partialAmount")]
      public double PartialAmount { get; set; }

      [JsonProperty("prePaymentAmount")]
      public double SumPrepaid { get; set; }

      [JsonProperty("mode")]
      public HdmDriver.ModePrint Mode { get; set; } = HdmDriver.ModePrint.InGood;

      [JsonProperty("useExtPOS")]
      public bool UseExtCardTerminal { get; set; }

      [JsonProperty("partnerTin")]
      public string ClientTin { get; set; }

      [JsonProperty("dep")]
      public int? Department { get; set; }

      [JsonProperty("eMarks")]
      public List<string> Marks { get; set; }

      [JsonProperty("items")]
      public List<HdmDriver.Item> Items { get; set; }

      [JsonIgnore]
      public HdmDriver.CheckCommand.Answer Result
      {
        get => JsonConvert.DeserializeObject<HdmDriver.CheckCommand.Answer>(this.AnswerString);
      }

      public class Answer : HdmDriver.HdmAnswer
      {
        [JsonProperty("rseq")]
        public long CheckNumber { get; set; }

        [JsonProperty("crn")]
        public string Crn { get; set; }

        [JsonProperty("sn")]
        public string SerialNumber { get; set; }

        [JsonProperty("tin")]
        public string Tin { get; set; }

        [JsonProperty("taxpayer")]
        public string TaxPayer { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("time")]
        public double Time { get; set; }

        [JsonProperty("fiscal")]
        public string FiscalNumber { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("change")]
        public double Change { get; set; }

        [JsonProperty("qr")]
        public string Qr { get; set; }
      }
    }

    public class CheckInfoCommand : HdmDriver.HdmCommand
    {
      public override byte CodeOperation => 10;

      [JsonProperty("seq")]
      public int Seq => HdmDriver._seq;

      [JsonProperty("receiptID")]
      public string FiscalNumber { get; set; }

      [JsonProperty("crn")]
      public string Сrn { get; set; }

      [JsonIgnore]
      public HdmDriver.CheckInfoCommand.Answer Result
      {
        get => JsonConvert.DeserializeObject<HdmDriver.CheckInfoCommand.Answer>(this.AnswerString);
      }

      public class Answer : HdmDriver.HdmAnswer
      {
        [JsonProperty("rseq")]
        public int CheckNumber { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("cid")]
        public string UserId { get; set; }

        [JsonProperty("ta")]
        public string TotalSum { get; set; }

        [JsonProperty("cash")]
        public string TotalCash { get; set; }

        [JsonProperty("card")]
        public string TotalCard { get; set; }

        [JsonProperty("ppu")]
        public string TotalPpu { get; set; }

        [JsonProperty("ppa")]
        public string TotalPrepaid { get; set; }

        [JsonProperty("saleType")]
        public string SaleType { get; set; }

        [JsonProperty("pTin")]
        public string ClientTin { get; set; }

        [JsonProperty("eMarks")]
        public List<string> Marks { get; set; }

        [JsonProperty("totals")]
        public List<HdmDriver.CheckInfoCommand.Answer.TotalItem> TotalItems { get; set; }

        public class TotalItem
        {
          [JsonProperty("gc")]
          public string GoodCode { get; set; }

          [JsonProperty("gn")]
          public string Name { get; set; }

          [JsonProperty("qty")]
          public string Qty { get; set; }

          [JsonProperty("p")]
          public string Price { get; set; }

          [JsonProperty("mu")]
          public string UnitName { get; set; }

          [JsonProperty("rpid")]
          public string Id { get; set; }

          [JsonProperty("dsc")]
          public string Discount { get; set; }

          [JsonProperty("dsct")]
          public string DiscountTypeStr { get; set; }

          [JsonProperty("did")]
          public string Department { get; set; }

          [JsonProperty("t")]
          public string TotalWithNdsSum { get; set; }

          [JsonProperty("tt")]
          public string TotalSum { get; set; }
        }
      }
    }

    public class ReturnCheckCommand : HdmDriver.HdmCommand
    {
      public override byte CodeOperation => 6;

      [JsonProperty("seq")]
      public int Seq => HdmDriver._seq;

      [JsonProperty("returnTicketId")]
      public long FiscalNumber { get; set; }

      [JsonProperty("crn")]
      public string Crn { get; set; }

      [JsonProperty("cashAmountForReturn")]
      public Decimal CashAmountForReturn { get; set; }

      [JsonProperty("cardAmountForReturn")]
      public Decimal CardAmountForReturn { get; set; }

      [JsonProperty("prePaymentAmountForReturn")]
      public Decimal PrePaymentAmountForReturn { get; set; }

      [JsonProperty("eMarks")]
      public List<string> Marks { get; set; }

      [JsonProperty("returnItemList")]
      public List<HdmDriver.ReturnCheckCommand.ReturnItem> ReturnItems { get; set; }

      [JsonIgnore]
      public HdmDriver.CheckCommand.Answer Result
      {
        get => JsonConvert.DeserializeObject<HdmDriver.CheckCommand.Answer>(this.AnswerString);
      }

      public class ReturnItem
      {
        [JsonProperty("rpid")]
        public long Id { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }
      }
    }

    public class Item
    {
      [JsonProperty("dep")]
      public int Department { get; set; }

      [JsonProperty("qty")]
      public double Qty { get; set; }

      [JsonProperty("price")]
      public double Price { get; set; }

      [JsonProperty("productCode")]
      public string ProductCode { get; set; }

      [JsonProperty("discount")]
      public Decimal? Discount { get; set; }

      [JsonProperty("discountType")]
      public HdmDriver.DiscountType? DiscountType { get; set; }

      [JsonProperty("additionalDiscount ")]
      public Decimal? AdditionalDiscount { get; set; }

      [JsonProperty("additionalDiscountType ")]
      public int? AdditionalDiscountType { get; set; }

      [JsonProperty("productName")]
      public string Name { get; set; }

      [JsonProperty("unit")]
      public string UnitName { get; set; }

      [JsonProperty("adgCode")]
      public string AdgCode { get; set; }
    }

    public enum ModePrint
    {
      WithoutGood = 1,
      InGood = 2,
      Prepaid = 3,
    }

    public enum DiscountType
    {
      PercentByPrice = 1,
      ValueByPrice = 2,
      ValueBySum = 4,
    }
  }
}
