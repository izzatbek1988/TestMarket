// Decompiled with JetBrains decompiler
// Type: Gbs.Core.Config.PlanfixSetting
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Gbs.Core.Config
{
  public class PlanfixSetting
  {
    public bool IsActive { get; set; }

    public bool IsSaveSale { get; set; }

    [JsonProperty]
    public string KeyApi { get; set; }

    [JsonIgnore]
    public string DecryptedKeyApi
    {
      get => this._decryptedKeyApi;
      set => this.KeyApi = CryptoHelper.StringCrypter.Encrypt(value);
    }

    [JsonProperty]
    public string Token { get; set; }

    [JsonIgnore]
    public string DecryptedToken
    {
      get => this._decryptedToken;
      set => this.Token = CryptoHelper.StringCrypter.Encrypt(value);
    }

    public string AccountName { get; set; }

    public string ApiUrl { get; set; } = "https://apiru.planfix.ru/xml/";

    public int ProjectId { get; set; }

    public int TemplateTaskId { get; set; }

    public TaskGood TemplateGoodAsTask { get; set; } = new TaskGood();

    public int TemplateGoodsGroupsAsTaskId { get; set; }

    public int ContactGroupId { get; set; }

    public int IntervalAutoSave { get; set; } = 15;

    public DateTime DateStart { get; set; } = DateTime.Now;

    public HandbookGood HandbookGood { get; set; } = new HandbookGood();

    public SaleAnalitic SaleAnalitic { get; set; } = new SaleAnalitic();

    public PaymentsAnalitic PaymentsAnalitic { get; set; } = new PaymentsAnalitic();

    public PlanfixSetting.GoodsEntityTypes GoodEntityType { get; set; }

    private string _decryptedKeyApi
    {
      get => !this.KeyApi.IsNullOrEmpty() ? CryptoHelper.StringCrypter.Decrypt(this.KeyApi) : "";
    }

    private string _decryptedToken
    {
      get => !this.Token.IsNullOrEmpty() ? CryptoHelper.StringCrypter.Decrypt(this.Token) : "";
    }

    public bool ValidateSettingPlanfix()
    {
      if (!this.IsActive)
        return true;
      PaymentsAnalitic paymentsAnalitic = this.PaymentsAnalitic;
      if ((paymentsAnalitic != null ? paymentsAnalitic.Id : 0) != 0)
      {
        SaleAnalitic saleAnalitic = this.SaleAnalitic;
        if ((saleAnalitic != null ? saleAnalitic.Id : 0) != 0 && this.ContactGroupId != 0 && this.ProjectId != 0 && this.TemplateTaskId != 0 && this.SaleAnalitic.CommentId != 0 && this.SaleAnalitic.DiscountId != 0 && this.SaleAnalitic.GoodHandbook != 0 && this.SaleAnalitic.PriceId != 0 && this.SaleAnalitic.QuantityId != 0 && this.PaymentsAnalitic.PaymentNameId != 0 && this.PaymentsAnalitic.SumId != 0)
        {
          if (this.GoodEntityType == PlanfixSetting.GoodsEntityTypes.Handbook)
          {
            HandbookGood handbookGood = this.HandbookGood;
            if ((handbookGood != null ? handbookGood.Id : 0) == 0 || this.HandbookGood.BarcodeId == 0 || this.HandbookGood.NameId == 0 || this.HandbookGood.PriceId == 0 || this.HandbookGood.QuantityId == 0)
              return false;
          }
          else
          {
            TaskGood templateGoodAsTask = this.TemplateGoodAsTask;
            if ((templateGoodAsTask != null ? templateGoodAsTask.Id : 0) == 0 || this.TemplateGoodAsTask.BarcodeId == 0 || this.TemplateGoodAsTask.GroupNameId == 0 || this.TemplateGoodAsTask.PriceId == 0 || this.TemplateGoodAsTask.QuantityId == 0)
              return false;
          }
          return true;
        }
      }
      return false;
    }

    public enum GoodsEntityTypes
    {
      Handbook,
      Task,
    }
  }
}
