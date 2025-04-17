// Decompiled with JetBrains decompiler
// Type: Gbs.Core.ActionResult
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using System.Collections.Generic;

#nullable disable
namespace Gbs.Core
{
  public class ActionResult
  {
    public List<string> Messages { get; } = new List<string>();

    public ActionResult.Results Result { get; set; }

    public ActionResult(ActionResult.Results result) => this.Result = result;

    public ActionResult(ActionResult.Results result, List<string> messages)
    {
      this.Result = result;
      this.Messages = messages;
    }

    public ActionResult(ActionResult.Results result, string message)
    {
      this.Result = result;
      this.Messages.Add(message);
    }

    public void AddMessage(ActionResult.Results result, string message)
    {
      this.Result = result > this.Result ? result : this.Result;
      this.Messages.Add(message);
    }

    public ActionResult Concat(ActionResult result)
    {
      int result1 = result.Result > this.Result ? (int) result.Result : (int) this.Result;
      result.Messages.AddRange((IEnumerable<string>) this.Messages);
      List<string> messages = result.Messages;
      return new ActionResult((ActionResult.Results) result1, messages);
    }

    public enum Results
    {
      Ok,
      Cancel,
      Warning,
      Error,
    }
  }
}
