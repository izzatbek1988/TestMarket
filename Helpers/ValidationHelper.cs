// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.ValidationHelper
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;

#nullable disable
namespace Gbs.Helpers
{
  public static class ValidationHelper
  {
    public static ActionResult DataValidation(Entity item)
    {
      try
      {
        foreach (PropertyInfo property in item.GetType().GetProperties())
        {
          if (((IEnumerable<object>) property.GetCustomAttributes(typeof (StringLengthAttribute), true)).FirstOrDefault<object>() is StringLengthAttribute stringLengthAttribute && property.GetValue((object) item) != null && !(property.GetValue((object) item) is bool) && !(property.GetValue((object) item) is DateTime) && (!property.GetValue((object) item).ToString().All<char>(new Func<char, bool>(char.IsDigit)) || !(property.PropertyType != typeof (string))))
          {
            if (property.GetValue((object) item).ToString().Length > stringLengthAttribute.MaximumLength)
              ProgressBarHelper.AddNotification(new ProgressBarViewModel.Notification(string.Format(Translate.ValidationHelper_DataValidation_Превышена_максимальная_длина_поля__0___длина_будет_сокращена_до__1__символов, (object) property.Name, (object) stringLengthAttribute.MaximumLength)));
            property.SetValue((object) item, (object) ValidationHelper.WithMaxLength(property.GetValue((object) item).ToString(), stringLengthAttribute.MaximumLength, stringLengthAttribute.MinimumLength));
          }
        }
        List<string> stringList = new List<string>();
        item.Uid = item.Uid == Guid.Empty ? Guid.NewGuid() : item.Uid;
        List<ValidationResult> validationResultList = new List<ValidationResult>();
        try
        {
          if (Validator.TryValidateObject((object) item, new ValidationContext((object) item), (ICollection<ValidationResult>) validationResultList, true))
            return new ActionResult(ActionResult.Results.Ok);
        }
        catch
        {
          return new ActionResult(ActionResult.Results.Ok);
        }
        foreach (ValidationResult validationResult in validationResultList)
        {
          LogHelper.Debug("Валидация не пройдена. Объект: " + item.GetType().FullName + ", " + validationResult.ErrorMessage);
          stringList.Add(Translate.ValidationHelper_Объект__ + item.GetType().Name + ". " + validationResult.ErrorMessage);
        }
        if (stringList.Any<string>())
        {
          if (!GlobalData.IsMarket5ImportAcitve)
          {
            int num = (int) MessageBoxHelper.Show(string.Join(Other.NewLine(), (IEnumerable<string>) stringList), Translate.Entity_Ошибка_валидации_данных, icon: MessageBoxImage.Exclamation);
          }
          else
            Other.ConsoleWrite("Ошибка валидации в " + item.GetType().Name + Other.NewLine() + string.Join(Other.NewLine(), (IEnumerable<string>) stringList));
        }
        return new ActionResult(ActionResult.Results.Error, stringList);
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка валидации данных");
        return new ActionResult(ActionResult.Results.Error);
      }
    }

    private static string WithMaxLength(string value, int maxLength, int minLength)
    {
      if (value == null)
        return (string) null;
      string str = value.Substring(0, Math.Min(value.Length, maxLength));
      int length = str.Length;
      if (length < minLength)
        str += string.Join("", Enumerable.Repeat<string>("_", minLength - length));
      return str;
    }

    public class ValidateObjectAttribute : ValidationAttribute
    {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
        List<ValidationResult> validationResultList = new List<ValidationResult>();
        ValidationContext validationContext1 = new ValidationContext(value, (IServiceProvider) null, (IDictionary<object, object>) null);
        Validator.TryValidateObject(value, validationContext1, (ICollection<ValidationResult>) validationResultList, true);
        if (validationResultList.Count == 0)
          return ValidationResult.Success;
        ValidationHelper.CompositeValidationResult validationResult = new ValidationHelper.CompositeValidationResult(string.Format(Translate.ValidateObjectAttribute_Данные_в__0__некорректны_, (object) validationContext.DisplayName));
        validationResultList.ForEach(new Action<ValidationResult>(validationResult.AddResult));
        return (ValidationResult) validationResult;
      }
    }

    private class CompositeValidationResult : ValidationResult
    {
      private readonly List<ValidationResult> _results = new List<ValidationResult>();

      public CompositeValidationResult(string errorMessage)
        : base(errorMessage)
      {
      }

      public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames)
        : base(errorMessage, memberNames)
      {
      }

      protected CompositeValidationResult(ValidationResult validationResult)
        : base(validationResult)
      {
      }

      public void AddResult(ValidationResult validationResult)
      {
        this._results.Add(validationResult);
      }
    }
  }
}
