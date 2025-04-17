// Decompiled with JetBrains decompiler
// Type: Gbs.Helpers.MVVM.RelayCommand
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Forms._shared;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.Windows.Input;

#nullable disable
namespace Gbs.Helpers.MVVM
{
  public class RelayCommand : ICommand
  {
    private readonly Func<object, bool> _canExecute;
    private readonly Action<object> _execute;
    private bool _isRunning;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
      add => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
      return (this._canExecute == null || this._canExecute(parameter)) && !this._isRunning;
    }

    public void Execute(object parameter)
    {
      try
      {
        if (!this.CanExecute(parameter))
          return;
        this._isRunning = true;
        this._execute(parameter);
      }
      catch (Exception ex)
      {
        if (DevelopersHelper.IsUnitTest())
        {
          this._isRunning = false;
          throw ex;
        }
        LogHelper.WriteError(ex, "Ошибка выполнения команды", false);
        string действиеНеБылоВыполнено = Translate.RelayCommand_Execute_Действие_не_было_выполнено_;
        LogHelper.Error(ex.InnerException ?? ex, действиеНеБылоВыполнено);
        ProgressBarHelper.Close();
      }
      finally
      {
        this._isRunning = false;
      }
    }
  }
}
