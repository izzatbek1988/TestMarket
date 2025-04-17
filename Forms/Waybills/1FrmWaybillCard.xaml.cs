// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Waybills.FrmWaybillCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Documents;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.GoodGroups;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Egais;
using Gbs.Helpers.Logging;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Waybills
{
  public class FrmWaybillCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxNum;
    internal System.Windows.Controls.DataGrid GridWaybillItems;
    internal Button ButtonMore;
    internal System.Windows.Controls.DataGrid GridWaybillPayments;
    private bool _contentLoaded;

    private WaybillCardViewModel Model { get; set; }

    public FrmWaybillCard()
    {
      this.InitializeComponent();
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
      WaybillCardViewModel.IconNew = (object) "[NEW]";
      this.GridWaybillItems.AddGoodsPropertiesColumns();
    }

    public bool ShowCardWaybill(
      Guid docUid,
      out Document _doc,
      Gbs.Core.Entities.Users.User authUser = null,
      List<WaybillsViewModel.WaybillItemsInfoGrid> documents = null,
      Action updateAction = null,
      bool isReturnBuy = false)
    {
      try
      {
        if (!Gbs.Helpers.Other.IsActiveAndShowForm<FrmWaybillCard>(docUid.ToString()))
        {
          _doc = (Document) null;
          return false;
        }
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          if (docUid == Guid.Empty)
          {
            _doc = new Document();
          }
          else
          {
            DocumentsRepository documentsRepository = new DocumentsRepository(dataBase);
            _doc = documentsRepository.GetByUid(docUid);
          }
          EntityProperties.PropertyValue propertyValue = _doc.Properties.FirstOrDefault<EntityProperties.PropertyValue>((Func<EntityProperties.PropertyValue, bool>) (x => x.Type.Uid == GlobalDictionaries.ReplayUidEgais));
          if (propertyValue != null && propertyValue.Value != null && !propertyValue.Value.ToString().IsNullOrEmpty() && MessageBoxHelper.Question("Данная накладная была принята из ЕГАИС, акт подтверждения уже отправлен, изменения накладной никак не отобразятся в ЕГАИС.\n\nПродолжить редактирование накладной?") == MessageBoxResult.No)
            return false;
          bool visibilityBuyPrice = new UsersRepository(dataBase).GetAccess(authUser, Actions.ShowBuyPrice) || docUid == Guid.Empty;
          if (!visibilityBuyPrice)
          {
            this.GridWaybillItems.Columns.Remove(this.GridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C")));
            this.GridWaybillItems.Columns.Remove(this.GridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == WaybillCardViewModel.UidBuySum)));
          }
          if (!new ConfigsRepository<Settings>().Get().Interface.IsVisibilityExtraPercent || !visibilityBuyPrice)
            this.GridWaybillItems.Columns.Remove(this.GridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "7A9EACED-EEA0-4656-88A1-A4591ED2F7AA")));
          this.Uid = _doc?.Uid.ToString();
          WaybillCardViewModel waybillCardViewModel = new WaybillCardViewModel(_doc, visibilityBuyPrice, isReturnBuy, this);
          waybillCardViewModel.CloseAction = new Action(((Window) this).Close);
          waybillCardViewModel.IsEnabledStorage = docUid == Guid.Empty;
          waybillCardViewModel.ListDoc = documents;
          waybillCardViewModel.ShowMenu = new Action(this.ShowMoreMenu);
          waybillCardViewModel.AuthUser = authUser;
          waybillCardViewModel.IsEdit = docUid != Guid.Empty;
          waybillCardViewModel.UpdateSortGrid = updateAction;
          waybillCardViewModel.VisibilityBuyPrice = visibilityBuyPrice ? Visibility.Visible : Visibility.Collapsed;
          waybillCardViewModel.FormToSHow = (WindowWithSize) this;
          this.Model = waybillCardViewModel;
          this.DataContext = (object) this.Model;
          this.Show();
          return this.Model.SaveResult;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме карточки накладной");
        _doc = (Document) null;
        return false;
      }
    }

    public bool ShowCardWaybillForEgais(
      EgaisDocument document,
      Gbs.Core.Entities.Users.User authUser,
      Action updateAction)
    {
      try
      {
        if (!Gbs.Helpers.Other.IsActiveAndShowForm<FrmWaybillCard>(document.Waybill.Identity))
          return false;
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          if (!new UsersRepository(dataBase).GetAccess(authUser, Actions.AcceptEgaisWaybill))
          {
            (bool Result, Gbs.Core.Entities.Users.User User) access = new Authorization().GetAccess(Actions.AcceptEgaisWaybill);
            if (!access.Result)
              return false;
            authUser = access.User;
          }
          Gbs.Core.Entities.GoodGroups.Group group = new Gbs.Core.Entities.GoodGroups.Group();
          while (new FormSelectGroup().GetSingleSelectedGroupUid(authUser, out group))
          {
            if (group.GoodsType.IsEither<GlobalDictionaries.GoodTypes>(GlobalDictionaries.GoodTypes.Certificate, GlobalDictionaries.GoodTypes.Service))
            {
              MessageBoxHelper.Warning("Для позиций из ЕГАИС можно выбрать категорию только с типом товаров: обычные или весовые.");
            }
            else
            {
              ProgressBarHelper.ProgressBar progressBar = new ProgressBarHelper.ProgressBar(Translate.FrmWaybillCard_ShowCardWaybillForEgais_Создание_накладной_на_поступление_из_ЕГАИС);
              foreach (EntityProperties.PropertyType propertyType in EntityProperties.GetTypesList(GlobalDictionaries.EntityTypes.Good).Where<EntityProperties.PropertyType>((Func<EntityProperties.PropertyType, bool>) (x =>
              {
                if (x.IsDeleted)
                  return false;
                return x.Uid.IsEither<Guid>(GlobalDictionaries.AlcCodeUid, GlobalDictionaries.AlcVolumeUid, GlobalDictionaries.CapacityUid, GlobalDictionaries.ProductCodeUid);
              })))
              {
                DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
                dataGridTextColumn.Header = (object) propertyType.Name;
                dataGridTextColumn.Width = new DataGridLength(100.0);
                Binding binding = new Binding(string.Format("Good.PropertiesDictionary[{0}]", (object) propertyType.Uid));
                binding.StringFormat = EntityProperties.GetStringFormat(propertyType);
                dataGridTextColumn.Binding = (BindingBase) binding;
                DataGridTextColumn element = dataGridTextColumn;
                Gbs.Helpers.Extensions.UIElement.Extensions.SetGuid((DependencyObject) element, propertyType.Uid.ToString());
                this.GridWaybillItems.Columns.Add((DataGridColumn) element);
              }
              List<Gbs.Core.Entities.Goods.Good> activeItems = new GoodRepository(dataBase).GetActiveItems();
              List<Gbs.Core.Entities.Documents.Item> objList = new List<Gbs.Core.Entities.Documents.Item>();
              List<Gbs.Core.Entities.Goods.Good> goodList = new List<Gbs.Core.Entities.Goods.Good>();
              foreach (PositionType positionType in document.Waybill.Items)
              {
                bool isSaveGood;
                Gbs.Core.Entities.Goods.Good goodGorEgais = SharedRepository.GetGoodGorEgais(activeItems, positionType.Product.AlcCode, positionType.Product.AlcVolume, positionType.Product.Capacity, positionType.Product.ProductVCode, positionType.Product.FullName, group, out isSaveGood);
                objList.Add(new Gbs.Core.Entities.Documents.Item()
                {
                  Good = goodGorEgais,
                  Quantity = positionType.Quantity,
                  BuyPrice = positionType.Price,
                  Comment = positionType.Identity
                });
                positionType.UidGoodInDb = goodGorEgais.Uid;
                if (isSaveGood)
                  goodList.Add(goodGorEgais);
              }
              Document doc = new Document()
              {
                Number = document.Waybill.WBNUMBER,
                Comment = document.Form2.WBRegId + "\n" + document.Waybill.ShipperName,
                Items = objList
              };
              bool access = new UsersRepository(dataBase).GetAccess(authUser, Actions.ShowBuyPrice);
              if (!access)
              {
                this.GridWaybillItems.Columns.Remove(this.GridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "9E89249B-F0F7-4D0D-ADB8-D89D48DB1C4C")));
                this.GridWaybillItems.Columns.Remove(this.GridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == WaybillCardViewModel.UidBuySum)));
              }
              if (!new ConfigsRepository<Settings>().Get().Interface.IsVisibilityExtraPercent)
                this.GridWaybillItems.Columns.Remove(this.GridWaybillItems.Columns.Single<DataGridColumn>((Func<DataGridColumn, bool>) (x => Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x) == "7A9EACED-EEA0-4656-88A1-A4591ED2F7AA")));
              this.Uid = document.Waybill.Identity;
              WaybillCardViewModel waybillCardViewModel = new WaybillCardViewModel(doc, access, false, this);
              waybillCardViewModel.CloseAction = new Action(((Window) this).Close);
              waybillCardViewModel.IsEnabledStorage = true;
              waybillCardViewModel.ShowMenu = new Action(this.ShowMoreMenu);
              waybillCardViewModel.AuthUser = authUser;
              waybillCardViewModel.IsEdit = false;
              waybillCardViewModel.VisibilityBuyPrice = access ? Visibility.Visible : Visibility.Collapsed;
              waybillCardViewModel.ListGoodSave = goodList;
              waybillCardViewModel.EgaisDocument = document;
              waybillCardViewModel.FormToSHow = (WindowWithSize) this;
              waybillCardViewModel.UpdateEgaisAction = updateAction;
              this.Model = waybillCardViewModel;
              this.Model.Waybill.IsEgaisThis = true;
              this.Model.Waybill.EgaisDocument = document;
              this.DataContext = (object) this.Model;
              progressBar.Close();
              this.Show();
              return this.Model.SaveResult;
            }
          }
          ProgressBarHelper.Close();
          this.Close();
          return false;
        }
      }
      catch (Exception ex)
      {
        LogHelper.Error(ex, "Ошибка в форме карточки накладной");
        return false;
      }
    }

    private void Buttonx_OnClick(object sender, RoutedEventArgs e)
    {
      if (!(this.FindResource((object) WaybillCardViewModel.AlsoMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) (sender as Button);
      resource.IsOpen = true;
    }

    private void ShowMoreMenu()
    {
      if (!(this.FindResource((object) WaybillCardViewModel.MoreMenuKey) is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (System.Windows.UIElement) this.ButtonMore;
      resource.IsOpen = true;
    }

    private bool CloseCard()
    {
      WaybillCardViewModel model1 = this.Model;
      // ISSUE: explicit non-virtual call
      int num = (model1 != null ? (__nonvirtual (model1.HasNoSavedChanges()) ? 1 : 0) : 1) != 0 ? 1 : (MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes ? 1 : 0);
      if (num == 0)
        return num != 0;
      WaybillCardViewModel model2 = this.Model;
      if (!((model2 != null ? model2.DocMoveUid : Guid.Empty) != Guid.Empty))
        return num != 0;
      WaybillCardViewModel._listUidSendWaybill.RemoveAll((Predicate<Guid>) (x => x == this.Model.DocMoveUid));
      return num != 0;
    }

    private void FrmWaybillCard_OnLoaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.GridWaybillItems.FindResource((object) "ContextMenuGrid");
      string[] blockUid = new string[5]
      {
        "4FBA7326-98D1-46CC-B620-54701A129F45",
        "14B14AEF-C1A9-497E-85B9-9ACCFC13E229",
        "622C050C-DCBC-4051-990E-3DD880AC1039",
        "A6E2976C-B8F0-4A60-A68A-530F6728B631",
        "43450469-B88F-4A65-BA2A-44085EEA77D9"
      };
      foreach (DataGridColumn element in this.GridWaybillItems.Columns.Where<DataGridColumn>((Func<DataGridColumn, bool>) (x => ((IEnumerable<string>) blockUid).All<string>((Func<string, bool>) (u => u != Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) x))))))
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Header = element.Header;
        newItem.Uid = Gbs.Helpers.Extensions.UIElement.Extensions.GetGuid((DependencyObject) element);
        newItem.IsCheckable = true;
        newItem.IsChecked = element.Visibility == Visibility.Visible;
        items.Add((object) newItem);
      }
      resource.Closed += new RoutedEventHandler(this.CmOnClosed);
      HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
      this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
      {
        {
          F1help.HelpHotKey,
          (ICommand) F1help.OpenPage((System.Windows.UIElement) this)
        },
        {
          hotKeys.OkAction,
          this.Model.SaveWaybillCommand
        },
        {
          hotKeys.CancelAction,
          this.Model.CloseCommand
        },
        {
          hotKeys.EditItem,
          (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Waybill.EditQuantityCommand.Execute((object) this.GridWaybillItems.SelectedItems)))
        },
        {
          hotKeys.DeleteItem,
          (ICommand) new RelayCommand((Action<object>) (obj => this.Model.Waybill.DeleteItemCommand.Execute((object) this.GridWaybillItems.SelectedItems)))
        },
        {
          hotKeys.AddItem,
          this.Model.AddItemFromNewGoodCommand
        }
      };
    }

    private void CmOnClosed(object sender, RoutedEventArgs e)
    {
      Gbs.Helpers.Other.IsVisibilityDataGridColumn(this.GridWaybillItems, (ContextMenu) sender);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/waybills/frmwaybillcard.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.TextBoxNum = (TextBox) target;
          break;
        case 2:
          this.GridWaybillItems = (System.Windows.Controls.DataGrid) target;
          break;
        case 3:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Buttonx_OnClick);
          break;
        case 4:
          this.ButtonMore = (Button) target;
          break;
        case 5:
          this.GridWaybillPayments = (System.Windows.Controls.DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
