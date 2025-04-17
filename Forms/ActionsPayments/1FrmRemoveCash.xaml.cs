// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.ActionsPayments.FrmRemoveCash
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Forms._shared;
using Gbs.Helpers;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

#nullable disable
namespace Gbs.Forms.ActionsPayments
{
  public class FrmRemoveCash : WindowWithSize, IComponentConnector
  {
    internal DecimalUpDown SumTextBox;
    private bool _contentLoaded;

    private PaymentsActionsViewModel Model { get; set; }

    public FrmRemoveCash() => this.InitializeComponent();

    public void RemoveCash(
      ref Gbs.Core.Entities.Payments.Payment payment,
      bool isActiveAccount = false,
      bool isSavePayment = true,
      Gbs.Core.Entities.Users.User user = null,
      bool isVisibilityGroup = true,
      bool isVisibilityClient = true)
    {
      if (!this.CheckModeProgram())
        return;
      try
      {
        this.Title = Translate.FrmRemoveCash_Снятие_денежных_средств;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(user, Actions.RemoveCash))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.RemoveCash);
            if (!access.Result)
            {
              payment = (Gbs.Core.Entities.Payments.Payment) null;
              return;
            }
            user = access.User;
          }
          PaymentsActionsViewModel actionsViewModel = new PaymentsActionsViewModel(PaymentsActionsViewModel.ActionsPayment.Remove, isActive: isActiveAccount, payment: payment);
          actionsViewModel.VisibilityAccIn = Visibility.Collapsed;
          actionsViewModel.Close = new Action(((Window) this).Close);
          actionsViewModel.User = user;
          actionsViewModel.ContentButtonOk = Translate.FrmRemoveCash_Снять;
          Gbs.Core.Entities.Payments.Payment payment1 = payment;
          actionsViewModel.Sum = payment1 != null ? payment1.SumOut : 0M;
          actionsViewModel.IsSavePayment = isSavePayment;
          actionsViewModel.VisibilityGroup = isVisibilityGroup ? Visibility.Visible : Visibility.Collapsed;
          actionsViewModel.VisibilityClients = isVisibilityClient ? Visibility.Visible : Visibility.Collapsed;
          this.Model = actionsViewModel;
          this.DataContext = (object) this.Model;
          this.SetHotKeys();
          this.ShowDialog();
          payment = this.Model.Payment;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при снятие денежных средств");
        payment = (Gbs.Core.Entities.Payments.Payment) null;
      }
    }

    public void InsertCash(bool isActiveAccount = false, Gbs.Core.Entities.Users.User user = null)
    {
      if (!this.CheckModeProgram())
        return;
      try
      {
        this.Title = Translate.FrmRemoveCash_Внесение_денежных_средств;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(user, Actions.InsertCash))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.InsertCash);
            if (!access.Result)
              return;
            user = access.User;
          }
          this.Model = new PaymentsActionsViewModel(PaymentsActionsViewModel.ActionsPayment.Insert, isActive: isActiveAccount)
          {
            VisibilityAccOut = Visibility.Collapsed,
            Close = new Action(((Window) this).Close),
            User = user,
            ContentButtonOk = Translate.FrmRemoveCash_Внести
          };
          this.DataContext = (object) this.Model;
          this.SetHotKeys();
          this.ShowDialog();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при внесение денежных средств");
      }
    }

    public void SendCash(Gbs.Core.Entities.Users.User authUser = null)
    {
      if (!this.CheckModeProgram())
        return;
      try
      {
        this.Title = Translate.FrmRemoveCash_Перемещение_денежных_средств;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(authUser, Actions.SendCash))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.SendCash);
            if (!access.Result)
              return;
            authUser = access.User;
          }
          this.Model = new PaymentsActionsViewModel(PaymentsActionsViewModel.ActionsPayment.Send)
          {
            VisibilityGroup = Visibility.Collapsed,
            Close = new Action(((Window) this).Close),
            VisibilityClients = Visibility.Collapsed,
            User = authUser,
            ContentButtonOk = Translate.FrmRemoveCash_Переместить
          };
          this.DataContext = (object) this.Model;
          this.SetHotKeys();
          this.ShowDialog();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при перемещении денежных средств");
      }
    }

    public void CorrectSumByAccount(
      Decimal sum,
      PaymentsActionsViewModel.ActionsPayment type,
      PaymentsAccounts.PaymentsAccount account,
      bool isEnabled = true,
      Gbs.Core.Entities.Users.User authUser = null,
      bool isAllAccount = false,
      bool isNonCancel = false,
      Decimal? oldSum = null)
    {
      this.IsNonCancelButton = isNonCancel;
      if (!this.CheckModeProgram())
        return;
      try
      {
        this.Title = type == PaymentsActionsViewModel.ActionsPayment.RecountSumCash ? Translate.FrmRemoveCash_CorrectSumByAccount_Пересчет_наличных_в_кассе : Translate.FrmRemoveCash_Корректировка_баланса_на_счете;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(authUser, Actions.CorrectSumByAcc))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.CorrectSumByAcc);
            if (!access.Result)
              return;
            authUser = access.User;
          }
          this.Model = new PaymentsActionsViewModel(type, account, isAllAcountNoDelete: isAllAccount)
          {
            VisibilityCancelButton = isNonCancel ? Visibility.Collapsed : Visibility.Visible,
            VisibilityGroup = Visibility.Collapsed,
            VisibilityAccIn = Visibility.Collapsed,
            VisibilityClients = Visibility.Collapsed,
            Close = new Action(((Window) this).Close),
            IsEnabledAccOut = isEnabled,
            OldSum = oldSum.GetValueOrDefault(sum),
            Sum = sum,
            User = authUser,
            ContentButtonOk = Translate.FrmExcelGoods_Сохранить
          };
          this.DataContext = (object) this.Model;
          this.SetHotKeys();
          this.ShowDialog();
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при корректировке денежных средств");
      }
    }

    public void CorrectBalance(Decimal sum, Gbs.Core.Entities.Users.User authUser = null)
    {
      try
      {
        this.Title = Translate.FrmRemoveCash_CorrectBalance_Корректировка_расхождения_по_кассе;
        if (!this.CheckAccess(Actions.CorrectBalanceSum, ref authUser))
          return;
        this.Model = new PaymentsActionsViewModel(PaymentsActionsViewModel.ActionsPayment.BalanceCorrect)
        {
          VisibilityGroup = Visibility.Collapsed,
          VisibilityAccIn = Visibility.Collapsed,
          VisibilityAccOut = Visibility.Collapsed,
          VisibilityNonFiscal = Visibility.Collapsed,
          VisibilityClients = Visibility.Collapsed,
          Close = new Action(((Window) this).Close),
          IsEnabledAccOut = false,
          OldSum = sum,
          Sum = sum,
          User = authUser,
          ContentButtonOk = Translate.FrmExcelGoods_Сохранить
        };
        this.DataContext = (object) this.Model;
        this.SetHotKeys();
        this.ShowDialog();
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка при корректировке баланса");
      }
    }

    private bool CheckModeProgram()
    {
      if (new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().ModeProgram != GlobalDictionaries.Mode.Home)
        return true;
      int num = (int) MessageBoxHelper.Show(Translate.GroupRepository_В_режиме_дом_офис_данное_действие_невозможно_, icon: MessageBoxImage.Exclamation);
      return false;
    }

    private bool CheckAccess(Actions actions, ref Gbs.Core.Entities.Users.User authUser)
    {
      if (this.IsNonCancelButton)
        return authUser != null || new Authorization().LoginUser(ref authUser);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        bool access = new UsersRepository(dataBase).GetAccess(authUser, actions);
        if (!access)
          (access, authUser) = Other.GetUserForDocument(actions);
        return access;
      }
    }

    private void SetHotKeys()
    {
      try
      {
        HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
        this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
        {
          {
            hotKeys.OkAction,
            this.Model.ActoinsPayment
          },
          {
            hotKeys.CancelAction,
            this.Model.ClosePayment
          },
          {
            hotKeys.SelectClient,
            this.Model.GetClient
          },
          {
            new HotKeysHelper.Hotkey(Key.Escape),
            this.Model.ClosePayment
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.WriteError(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/actionspayments/frmremovecash.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.SumTextBox = (DecimalUpDown) target;
      else
        this._contentLoaded = true;
    }
  }
}
