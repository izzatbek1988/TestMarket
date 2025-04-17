// Decompiled with JetBrains decompiler
// Type: FiscalBoxClient
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

#nullable disable
public class FiscalBoxClient
{
  private Gbs.Core.Config.Devices _config;

  public FiscalBoxClient(Gbs.Core.Config.Devices devices) => this._config = devices;

  public void DoCommand(FiscalBoxClient.FiscalBoxCommand command)
  {
    LogHelper.Debug("Начинаю выполнять комманду HiPos: " + command.Method);
    RestHelper restHelper = new RestHelper(this._config.CheckPrinter.Connection.LanPort.UrlAddress, this._config.CheckPrinter.Connection.LanPort.PortNumber, command.ToJsonString());
    restHelper.CreateCommand(command.Method, command.HttpMethod);
    restHelper.DoCommand();
    command.AnswerString = restHelper.Answer;
  }

  public class FiscalBoxCommand
  {
    public TypeRestRequest HttpMethod { get; }

    public string Method { get; }

    public string AnswerString { get; set; }
  }

  [Localizable(false)]
  public class SaleCommand : FiscalBoxClient.FiscalBoxCommand
  {
    public string AccessToken { get; set; }

    public string Cashier { get; set; }

    public string Currency { get; set; }

    public double Sum { get; set; }

    public JArray Items { get; set; }

    public JArray VatAmounts { get; set; }

    public string Endpoint => "createDocument";

    public JObject BuildRequest()
    {
      return new JObject()
      {
        {
          "parameters",
          (JToken) new JObject()
          {
            {
              "access_token",
              (JToken) this.AccessToken
            },
            {
              "doc_type",
              (JToken) "sale"
            },
            {
              "data",
              (JToken) new JObject()
              {
                {
                  "cashier",
                  (JToken) this.Cashier
                },
                {
                  "currency",
                  (JToken) this.Currency
                },
                {
                  "sum",
                  (JToken) this.Sum
                },
                {
                  "items",
                  (JToken) this.Items
                },
                {
                  "vatAmounts",
                  (JToken) this.VatAmounts
                }
              }
            }
          }
        },
        {
          "operationId",
          (JToken) "createDocument"
        },
        {
          "version",
          (JToken) 1
        }
      };
    }
  }

  [Localizable(false)]
  public class MoneyBackCommand : FiscalBoxClient.FiscalBoxCommand
  {
    public string AccessToken { get; set; }

    public string Cashier { get; set; }

    public string Currency { get; set; }

    public string ParentDocument { get; set; }

    public int MoneyBackType { get; set; }

    public JArray Items { get; set; }

    public JArray VatAmounts { get; set; }

    public string Endpoint => "createDocument";

    public JObject BuildRequest()
    {
      return new JObject()
      {
        {
          "parameters",
          (JToken) new JObject()
          {
            {
              "access_token",
              (JToken) this.AccessToken
            },
            {
              "doc_type",
              (JToken) "money_back"
            },
            {
              "data",
              (JToken) new JObject()
              {
                {
                  "cashier",
                  (JToken) this.Cashier
                },
                {
                  "currency",
                  (JToken) this.Currency
                },
                {
                  "parentDocument",
                  (JToken) this.ParentDocument
                },
                {
                  "moneyBackType",
                  (JToken) this.MoneyBackType
                },
                {
                  "items",
                  (JToken) this.Items
                },
                {
                  "vatAmounts",
                  (JToken) this.VatAmounts
                }
              }
            }
          }
        },
        {
          "operationId",
          (JToken) "createDocument"
        },
        {
          "version",
          (JToken) 1
        }
      };
    }
  }

  [Localizable(false)]
  public class DepositCommand : FiscalBoxClient.FiscalBoxCommand
  {
    public string AccessToken { get; set; }

    public string Cashier { get; set; }

    public string Currency { get; set; }

    public double Sum { get; set; }

    public string Endpoint => "createDocument";

    public JObject BuildRequest()
    {
      return new JObject()
      {
        {
          "parameters",
          (JToken) new JObject()
          {
            {
              "access_token",
              (JToken) this.AccessToken
            },
            {
              "doc_type",
              (JToken) "deposit"
            },
            {
              "data",
              (JToken) new JObject()
              {
                {
                  "cashier",
                  (JToken) this.Cashier
                },
                {
                  "currency",
                  (JToken) this.Currency
                },
                {
                  "sum",
                  (JToken) this.Sum
                }
              }
            }
          }
        },
        {
          "operationId",
          (JToken) "createDocument"
        },
        {
          "version",
          (JToken) 1
        }
      };
    }
  }

  [Localizable(false)]
  public class WithdrawCommand : FiscalBoxClient.FiscalBoxCommand
  {
    public string AccessToken { get; set; }

    public string Cashier { get; set; }

    public string Currency { get; set; }

    public double Sum { get; set; }

    public string Endpoint => "createDocument";

    public JObject BuildRequest()
    {
      return new JObject()
      {
        {
          "parameters",
          (JToken) new JObject()
          {
            {
              "access_token",
              (JToken) this.AccessToken
            },
            {
              "doc_type",
              (JToken) "withdraw"
            },
            {
              "data",
              (JToken) new JObject()
              {
                {
                  "cashier",
                  (JToken) this.Cashier
                },
                {
                  "currency",
                  (JToken) this.Currency
                },
                {
                  "sum",
                  (JToken) this.Sum
                }
              }
            }
          }
        },
        {
          "operationId",
          (JToken) "createDocument"
        },
        {
          "version",
          (JToken) 1
        }
      };
    }
  }
}
