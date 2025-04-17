// Decompiled with JetBrains decompiler
// Type: Gbs.Forms._shared.TemplateFrViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Helpers;
using Gbs.Helpers.FR;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms._shared
{
  public partial class TemplateFrViewModel : ViewModelWithForm
  {
    public Users.User AuthUser { get; set; }

    public ICommand EditReportCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedFile == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.TemplateFRViewModel_Требуется_выбрать_шаблон);
          }
          else
          {
            using (DataBase dataBase = Data.GetDataBase())
            {
              if (!new UsersRepository(dataBase).GetAccess(this.AuthUser, Actions.EditFrReport))
              {
                (bool Result, Users.User User) access = new Authorization().GetAccess(Actions.EditFrReport);
                if (!access.Result)
                  return;
                this.AuthUser = access.User;
              }
              new FastReportFacade().ShowDesigner(this.SelectedFile.FileInfo.FullName);
            }
          }
        }));
      }
    }

    public Action Close { get; set; }

    public bool ResultAction { get; set; }

    public ICommand SelectedItem { get; set; }

    public ICommand ReturnPrint { get; set; }

    public TemplateFrViewModel.Template SelectedFile { get; set; }

    public List<TemplateFrViewModel.Template> ListTemplates { get; set; } = new List<TemplateFrViewModel.Template>();

    public bool CanShow { get; set; } = true;

    public TemplateFrViewModel()
    {
    }

    public TemplateFrViewModel(string directoryPath, Action close)
    {
      this.Close = close;
      DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
      if (directoryInfo.Exists)
      {
        foreach (FileSystemInfo fileSystemInfo in ((IEnumerable<FileInfo>) directoryInfo.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>) (x => x.Extension == ".frx")))
          this.ListTemplates.Add(new TemplateFrViewModel.Template(new FileInfo(fileSystemInfo.FullName)));
      }
      if (!this.ListTemplates.Any<TemplateFrViewModel.Template>())
      {
        int num = (int) MessageBoxHelper.Show(Translate.TemplateFRViewModel_Для_печати_данного_документа_не_найдены_подходящие_шаблоны, PartnersHelper.ProgramName(), icon: MessageBoxImage.Hand);
        this.CanShow = false;
      }
      else
      {
        this.SelectedFile = this.ListTemplates.First<TemplateFrViewModel.Template>();
        this.SelectedItem = (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.SelectedFile == null)
          {
            int num = (int) MessageBoxHelper.Show(Translate.TemplateFRViewModel_Требуется_выбрать_шаблон);
          }
          else
          {
            this.ResultAction = true;
            this.Close();
          }
        }));
      }
    }

    public class Template
    {
      public FileInfo FileInfo { get; private set; }

      public string Name => Path.GetFileNameWithoutExtension(this.FileInfo?.Name);

      public Template(FileInfo fileInfo) => this.FileInfo = fileInfo;
    }
  }
}
