// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.SendWaybills.MoveListViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Db;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Forms.Waybills;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Gbs.Forms.SendWaybills
{
  public partial class MoveListViewModel : ViewModelWithForm
  {
    private MoveHelper.DisplayDocumentSend _document;

    public ObservableCollection<MoveHelper.DisplayDocumentSend> Sends { get; set; } = new ObservableCollection<MoveHelper.DisplayDocumentSend>();

    public MoveHelper.DisplayDocumentSend GetMoveDocument(Users.User user)
    {
      using (DataBase dataBase = Data.GetDataBase())
      {
        int num = new UsersRepository(dataBase).GetAccess(user, Actions.ShowBuyPrice) ? 1 : 0;
        this.Sends = new ObservableCollection<MoveHelper.DisplayDocumentSend>(MoveHelper.GetDisplayDocument());
        this.FormToSHow = (WindowWithSize) new FrmMoveList();
        this.CloseAction = new Action(((Window) this.FormToSHow).Close);
        if (num == 0)
        {
          DataGrid dataGridMove = ((FrmMoveList) this.FormToSHow).DataGridMove;
          dataGridMove.Columns.Remove(dataGridMove.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == WaybillCardViewModel.UidBuySum)));
        }
        this.ShowForm();
        return this._document;
      }
    }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<MoveHelper.DisplayDocumentSend> list = ((IEnumerable) obj).Cast<MoveHelper.DisplayDocumentSend>().ToList<MoveHelper.DisplayDocumentSend>();
          if (!list.Any<MoveHelper.DisplayDocumentSend>())
          {
            int num = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_строку_);
          }
          else
          {
            if (MessageBoxHelper.Show(string.Format(Translate.MoveListViewModel_Вы_уверены__что_хотите_удалить__0__перемещений__Для_корректной_отмены_Вам_нужно_будет_также_удалить_перемещение_на_точке_отправителе_, (object) list.Count), buttons: MessageBoxButton.YesNo, icon: MessageBoxImage.Question) == MessageBoxResult.No)
              return;
            foreach (MoveHelper.DisplayDocumentSend displayDocumentSend in list)
            {
              File.Delete(displayDocumentSend.Document.PathFile);
              this.Sends.Remove(displayDocumentSend);
            }
            this.OnPropertyChanged("Sends");
          }
        }));
      }
    }

    public ICommand AddSendWaybill
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          List<MoveHelper.DisplayDocumentSend> list = ((IEnumerable) obj).Cast<MoveHelper.DisplayDocumentSend>().ToList<MoveHelper.DisplayDocumentSend>();
          if (!list.Any<MoveHelper.DisplayDocumentSend>())
          {
            int num1 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_строку_);
          }
          else if (list.Count<MoveHelper.DisplayDocumentSend>() > 1)
          {
            int num2 = (int) MessageBoxHelper.Show(Translate.CreditListViewModel_Необходимо_выбрать_только_одну_запись_);
          }
          else
          {
            using (DataBase dataBase = Data.GetDataBase())
            {
              if (new DocumentsRepository(dataBase).GetByParentUid(list.Single<MoveHelper.DisplayDocumentSend>().Document.Document.Uid).Any<Document>())
              {
                int num3 = (int) MessageBoxHelper.Show(Translate.MoveListViewModel_Данное_перемещение_уже_было_принято_ранее_);
                MoveHelper.DeleteDocumentCloud(list.Single<MoveHelper.DisplayDocumentSend>().Document.Uid);
                this.Sends.Remove(list.Single<MoveHelper.DisplayDocumentSend>());
                this.OnPropertyChanged("Sends");
              }
              else
              {
                this._document = list.Single<MoveHelper.DisplayDocumentSend>();
                LogHelper.Debug("В накладную добавлено перемещение из точки " + this._document.Document.SenderPointName);
                LogHelper.Trace(this._document.Document.Document.ToJsonString(true));
                this.CloseAction();
              }
            }
          }
        }));
      }
    }
  }
}
