// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Settings.PlanfixSettingViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Forms._shared;
using Gbs.Forms.Settings.PlanfixSettings;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Planfix.Api.Entities.Analitics;
using Planfix.Api.Entities.ContactGroups;
using Planfix.Api.Entities.Handbooks;
using Planfix.Api.Entities.Process;
using Planfix.Api.Entities.Projects;
using Planfix.Api.Entities.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.Settings
{
  public partial class PlanfixSettingViewModel : ViewModelWithForm
  {
    private PlanfixSetting.GoodsEntityTypes _goodsEntityType;

    public Integrations Setting { get; set; }

    public List<Project> Projects { get; set; }

    public List<Planfix.Api.Entities.Task.Task> Templates { get; set; }

    public List<Planfix.Api.Entities.ContactGroups.Group> Groups { get; set; }

    public List<AnaliticInfo> Analitics { get; set; }

    public List<Handbook> Handbooks { get; set; }

    public List<TaskStatusSet> StatusSets { get; set; }

    public Dictionary<PlanfixSetting.GoodsEntityTypes, string> GoodsEntityTypesDictionary
    {
      get
      {
        return new Dictionary<PlanfixSetting.GoodsEntityTypes, string>()
        {
          {
            PlanfixSetting.GoodsEntityTypes.Handbook,
            Translate.PlanfixSettingViewModel_Запись_справочника
          },
          {
            PlanfixSetting.GoodsEntityTypes.Task,
            Translate.PlanfixSettingViewModel_Задача
          }
        };
      }
    }

    public Visibility IsGoodsAsTask
    {
      get
      {
        return this._goodsEntityType != PlanfixSetting.GoodsEntityTypes.Task ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility IsGoodAsHandbook
    {
      get
      {
        return this._goodsEntityType != PlanfixSetting.GoodsEntityTypes.Handbook ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public PlanfixSetting.GoodsEntityTypes GoodsEntityType
    {
      get
      {
        this.OnPropertyChanged("IsGoodsAsTask");
        this.OnPropertyChanged("IsGoodAsHandbook");
        return this._goodsEntityType;
      }
      set
      {
        this._goodsEntityType = value;
        this.OnPropertyChanged("IsGoodsAsTask");
        this.OnPropertyChanged("IsGoodAsHandbook");
      }
    }

    public ICommand SaveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          PlanfixSetting planfix = this.Setting.Planfix;
          HandbookGood handbookGood = planfix.HandbookGood;
          if ((handbookGood != null ? handbookGood.Id : 0) != 0 || this.GoodsEntityType != PlanfixSetting.GoodsEntityTypes.Handbook)
          {
            PaymentsAnalitic paymentsAnalitic = planfix.PaymentsAnalitic;
            if ((paymentsAnalitic != null ? paymentsAnalitic.Id : 0) != 0)
            {
              SaleAnalitic saleAnalitic = planfix.SaleAnalitic;
              if ((saleAnalitic != null ? saleAnalitic.Id : 0) != 0 && planfix.ContactGroupId != 0 && planfix.ProjectId != 0)
              {
                TaskGood templateGoodAsTask = planfix.TemplateGoodAsTask;
                if ((templateGoodAsTask != null ? templateGoodAsTask.Id : 0) != 0 || this.GoodsEntityType != PlanfixSetting.GoodsEntityTypes.Task)
                {
                  this.Setting.Planfix.GoodEntityType = this.GoodsEntityType;
                  if (!new ConfigsRepository<Integrations>().Save(this.Setting))
                    return;
                  this.CloseAction();
                  return;
                }
              }
            }
          }
          int num = (int) MessageBoxHelper.Show(Translate.PlanfixSettingViewModel_Требуется_для_всех_сущностей_указать_связь);
        }));
      }
    }

    public ICommand SettingSaleAnalitic
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<AnaliticInfo> analitics = this.Analitics;
          AnaliticInfo analiticInfo = analitics != null ? analitics.FirstOrDefault<AnaliticInfo>((Func<AnaliticInfo, bool>) (x => x.Id == this.Setting.Planfix.SaleAnalitic.Id)) : (AnaliticInfo) null;
          if (this.Setting.Planfix.SaleAnalitic.Id <= 0 || analiticInfo == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.PlanfixSettingViewModel_Необходимо_выбрать_аналитику);
          }
          else
            new FrmAnaliticSetting().ShowSettingAnalitic(this.Setting, AnaliticSettingViewModel.TypeAnalitic.Sale, analiticInfo);
        }));
      }
    }

    public ICommand SettingGroupTask
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          TaskGood templateGoodAsTask = this.Setting.Planfix.TemplateGoodAsTask;
          if ((templateGoodAsTask != null ? templateGoodAsTask.Id : 0) <= 0)
          {
            int num = (int) MessageBoxHelper.Show(Translate.PlanfixSettingViewModel_Необходимо_выбрать_задачу);
          }
          else
            new TaskPlanFixViewModel().ShowSettingATaskGood(this.Setting, this.Setting.Planfix.TemplateGoodAsTask);
        }));
      }
    }

    public ICommand SettingPaymentAnalitic
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<AnaliticInfo> analitics = this.Analitics;
          AnaliticInfo analiticInfo = analitics != null ? analitics.FirstOrDefault<AnaliticInfo>((Func<AnaliticInfo, bool>) (x => x.Id == this.Setting.Planfix.SaleAnalitic.Id)) : (AnaliticInfo) null;
          if (this.Setting.Planfix.PaymentsAnalitic.Id <= 0 || analiticInfo == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.PlanfixSettingViewModel_Необходимо_выбрать_аналитику);
          }
          else
            new FrmAnaliticSetting().ShowSettingAnalitic(this.Setting, AnaliticSettingViewModel.TypeAnalitic.Payment, this.Analitics.Single<AnaliticInfo>((Func<AnaliticInfo, bool>) (x => x.Id == this.Setting.Planfix.PaymentsAnalitic.Id)));
        }));
      }
    }

    public ICommand SettingHandbookGood
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<Handbook> handbooks = this.Handbooks;
          Handbook handbook = handbooks != null ? handbooks.FirstOrDefault<Handbook>((Func<Handbook, bool>) (x => x.id == this.Setting.Planfix.HandbookGood.Id)) : (Handbook) null;
          if (this.Setting.Planfix.HandbookGood.Id <= 0 || handbook == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.PlanfixSettingViewModel_Необходимо_выбрать_справочник_товаров);
          }
          else
            new FrmAnaliticSetting().ShowSettingGoodHandbook(this.Setting, handbook);
        }));
      }
    }

    public void ShowSetting(Integrations setting)
    {
      ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.PlanfixSettingViewModel_Получение_данных_из_ПланФикс);
      this.Setting = setting;
      this.GoodsEntityType = this.Setting.Planfix.GoodEntityType;
      this.FormToSHow = (WindowWithSize) new FrmPlanFixSetting();
      this.CloseAction = new Action(((Window) this.FormToSHow).Close);
      try
      {
        (Planfix.Api.Answer answer, List<Project> entitiesList) list1 = new ProjectRepository().GetList();
        LogHelper.Debug("prj: " + list1.answer.ToJsonString(true));
        (Planfix.Api.Answer answer, List<Planfix.Api.Entities.Task.Task>) listTemplate = new TaskRepository().GetListTemplate();
        LogHelper.Debug("tsk:" + listTemplate.answer.ToJsonString(true));
        (Planfix.Api.Answer answer, List<Planfix.Api.Entities.ContactGroups.Group> entitiesList) list2 = new GroupRepository().GetList();
        LogHelper.Debug("grp:" + list2.answer.ToJsonString(true));
        (Planfix.Api.Answer answer, List<AnaliticInfo>) listInfo = new AnaliticRepository().GetListInfo();
        LogHelper.Debug("anl:" + listInfo.answer.ToJsonString(true));
        (Planfix.Api.Answer answer, List<TaskStatusSet> entitiesList) list3 = new ProcessRepository().GetList();
        LogHelper.Debug("sts:" + list3.answer.ToJsonString(true));
        (Planfix.Api.Answer answer, List<Handbook> entitiesList) list4 = new HandbookRepository().GetList();
        LogHelper.Debug("hnb:" + list4.answer.ToJsonString(true));
        if (list1.answer.Result == Planfix.Api.Answer.ResultTypes.Error || listTemplate.answer.Result == Planfix.Api.Answer.ResultTypes.Error || list2.answer.Result == Planfix.Api.Answer.ResultTypes.Error || listInfo.answer.Result == Planfix.Api.Answer.ResultTypes.Error || list3.answer.Result == Planfix.Api.Answer.ResultTypes.Error || list4.answer.Result == Planfix.Api.Answer.ResultTypes.Error)
        {
          int num = (int) MessageBoxHelper.Show(Translate.PlanfixSettingViewModel_При_загрузке_данных_из_ПланФикс_произошла_ошибка__возможно_нет_интернет_соединения, icon: MessageBoxImage.Hand);
          this.CloseAction();
          progressBar.Close();
          return;
        }
        this.Projects = new List<Project>((IEnumerable<Project>) list1.entitiesList);
        this.Templates = new List<Planfix.Api.Entities.Task.Task>((IEnumerable<Planfix.Api.Entities.Task.Task>) listTemplate.Item2);
        this.Groups = new List<Planfix.Api.Entities.ContactGroups.Group>((IEnumerable<Planfix.Api.Entities.ContactGroups.Group>) list2.entitiesList);
        this.Analitics = new List<AnaliticInfo>((IEnumerable<AnaliticInfo>) listInfo.Item2);
        this.StatusSets = new List<TaskStatusSet>((IEnumerable<TaskStatusSet>) list3.entitiesList);
        this.Handbooks = new List<Handbook>((IEnumerable<Handbook>) list4.entitiesList);
      }
      catch (Exception ex)
      {
        progressBar.Close();
        LogHelper.WriteError(ex);
        string настройкаПланФикс = Translate.PlanfixSettingViewModel_Ошибка_при_попытке_открыть_настройка_ПланФикс;
        LogHelper.ShowErrorMgs(ex, настройкаПланФикс, LogHelper.MsgTypes.MessageBox);
        return;
      }
      progressBar.Close();
      this.ShowForm();
    }
  }
}
