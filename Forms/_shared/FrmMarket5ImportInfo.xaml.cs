// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.ProgressInfoViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Helpers;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class ProgressInfoViewModel : ViewModelWithForm
  {
    private string _currentTask;
    private int _currentTaskStep;
    private bool _isIndeterminate = true;
    private string _progressLog;
    private int _taskTotalSteps;

    public bool IsIndeterminate
    {
      get => this._isIndeterminate;
      set
      {
        this._isIndeterminate = value;
        this.OnPropertyChanged(nameof (IsIndeterminate));
      }
    }

    public bool CanClose { get; set; } = true;

    public string CurrentTask
    {
      get => this._currentTask;
      set
      {
        this._currentTask = value;
        this.OnPropertyChanged(nameof (CurrentTask));
      }
    }

    public int CurrentTaskStep
    {
      get => this._currentTaskStep;
      set
      {
        this._currentTaskStep = value;
        this.OnPropertyChanged(nameof (CurrentTaskStep));
      }
    }

    public int TaskTotalSteps
    {
      get => this._taskTotalSteps;
      set
      {
        this._taskTotalSteps = value;
        this.OnPropertyChanged(nameof (TaskTotalSteps));
      }
    }

    public string ProgressLog
    {
      get => this._progressLog;
      set
      {
        this._progressLog = value;
        this.OnPropertyChanged(nameof (ProgressLog));
      }
    }

    public void NewProgress(string taskName, int steps)
    {
      this.CanClose = false;
      if (!this.CurrentTask.IsNullOrEmpty())
      {
        string[] strArray = new string[8]
        {
          DateTime.Now.ToString("dd.MM.yyyy HH:mm.ss"),
          " | ",
          Translate.ProgressInfoViewModel_Завершено__,
          this.CurrentTask,
          null,
          null,
          null,
          null
        };
        string str;
        if (this.TaskTotalSteps <= 0)
          str = "";
        else
          str = " (" + this.CurrentTaskStep.ToString("### ##0") + Translate.ProgressInfoViewModel__из_ + this.TaskTotalSteps.ToString("### ##0") + ")";
        strArray[4] = str;
        strArray[5] = this.TaskTotalSteps > this.CurrentTaskStep ? Other.NewLine() + Translate.ProgressInfoViewModel____ВЫПОЛНЕНО_НЕ_ПОЛНОСТЬЮ____ + Other.NewLine() : "";
        strArray[6] = Other.NewLine();
        strArray[7] = this.ProgressLog;
        this.ProgressLog = string.Concat(strArray);
      }
      this.TaskTotalSteps = steps;
      this.CurrentTask = taskName;
      this.CurrentTaskStep = 0;
    }
  }
}
