// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.Goods.FrmGoodCard
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core.Config;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms._shared;
using Gbs.Forms.Goods.GoodCard;
using Gbs.Helpers;
using Gbs.Helpers.ContextHelp;
using Gbs.Helpers.ControlsHelpers.DataGrid;
using Gbs.Helpers.Logging;
using Gbs.Resources.Localizations;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Gbs.Forms.Goods
{
  public class FrmGoodCard : WindowWithSize, IComponentConnector
  {
    internal TextBox TextBoxName;
    internal ComboBox ComboBoxName;
    internal System.Windows.Controls.DataGrid StockGrid;
    internal Button StockOptionButton;
    internal ComboBox SetStatusesComboBox;
    internal System.Windows.Controls.Frame ModificationControlPanel;
    private bool _contentLoaded;

    private GoodCardModelView MyViewModel { get; set; }

    public FrmGoodCard()
    {
      this.InitializeComponent();
      List<GoodsExtraPrice.GoodExtraPrice> list = GoodsExtraPrice.GetGoodExtraPriceList().Where<GoodsExtraPrice.GoodExtraPrice>((Func<GoodsExtraPrice.GoodExtraPrice, bool>) (x => !x.IsDeleted)).ToList<GoodsExtraPrice.GoodExtraPrice>();
      int num = 0;
      foreach (GoodsExtraPrice.GoodExtraPrice goodExtraPrice in list)
      {
        ObservableCollection<DataGridColumn> columns = this.StockGrid.Columns;
        DataGridTextColumn dataGridTextColumn = new DataGridTextColumn();
        dataGridTextColumn.Header = (object) goodExtraPrice.Name;
        dataGridTextColumn.Width = new DataGridLength(80.0, DataGridLengthUnitType.Pixel);
        dataGridTextColumn.Binding = (BindingBase) new Binding("ExtraPrice")
        {
          Converter = (IValueConverter) new ExtraPriceToRowConverter(),
          ConverterParameter = (object) num
        };
        columns.Add((DataGridColumn) dataGridTextColumn);
        ++num;
      }
      this.QuestionCloseAction = new Func<bool>(this.CloseCard);
    }

    public bool ShowGoodCard(
      Guid goodUid,
      out Gbs.Core.Entities.Goods.Good good,
      bool isEdit = false,
      Gbs.Core.Entities.Users.User authUser = null,
      Gbs.Core.Entities.Goods.Good goodNew = null,
      bool isSave = true,
      GlobalDictionaries.DocumentsTypes documentTypes = GlobalDictionaries.DocumentsTypes.None)
    {
      try
      {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        using (Gbs.Core.Db.DataBase dataBase = Gbs.Core.Data.GetDataBase())
        {
          good = goodNew;
          if (goodNew == null)
            good = goodUid == Guid.Empty ? new Gbs.Core.Entities.Goods.Good() : new GoodRepository(dataBase).GetByUid(goodUid);
          GoodCardModelView goodCardModelView = new GoodCardModelView(good, isEdit, documentTypes);
          goodCardModelView.SetContentEditPageAction = new Action(this.SetContentEditPage);
          goodCardModelView.AuthUser = authUser;
          goodCardModelView.CloseAction = new Action(((Window) this).Close);
          goodCardModelView.IsSaveGood = isSave;
          goodCardModelView.IsEditCard = goodUid != Guid.Empty;
          this.MyViewModel = goodCardModelView;
          this.DataContext = (object) this.MyViewModel;
          HotKeys hotKeys = new ConfigsRepository<Gbs.Core.Config.Devices>().Get().Keyboard.HotKeys;
          this.HotKeysDictionary = new Dictionary<HotKeysHelper.Hotkey, ICommand>()
          {
            {
              F1help.HelpHotKey,
              (ICommand) F1help.OpenPage((UIElement) this)
            },
            {
              hotKeys.OkAction,
              this.MyViewModel.SaveGood
            },
            {
              hotKeys.CancelAction,
              this.MyViewModel.CloseCommand
            }
          };
          if (this.MyViewModel.IsShowCard)
          {
            Gbs.Helpers.Other.ConsoleWrite("Карточка товара загружена за: " + ((Decimal) stopwatch.ElapsedMilliseconds / 1000M).ToString());
            this.ShowDialog();
          }
          new ConfigsRepository<FilterOptions>().Save(this.MyViewModel.Setting);
          return this.MyViewModel.SaveGoodResult;
        }
      }
      catch (Exception ex)
      {
        ProgressBarHelper.Close();
        LogHelper.Error(ex, "Ошибка в карточке товара");
        good = (Gbs.Core.Entities.Goods.Good) null;
        return false;
      }
    }

    private void SetContentEditPage()
    {
      Gbs.Core.Entities.Goods.Good good = this.MyViewModel.Good;
      this.ModificationControlPanel.Content = (object) null;
      switch (good.SetStatus)
      {
        case GlobalDictionaries.GoodsSetStatuses.Set:
        case GlobalDictionaries.GoodsSetStatuses.Production:
          this.ModificationControlPanel.Content = (object) new PageGoodSetContent(good);
          break;
        case GlobalDictionaries.GoodsSetStatuses.Kit:
          this.ModificationControlPanel.Content = (object) new PageGoodKit(good);
          break;
        case GlobalDictionaries.GoodsSetStatuses.Range:
          this.ModificationControlPanel.Content = (object) new PageGoodModifications(good);
          break;
      }
    }

    private bool CloseCard()
    {
      int num = this.MyViewModel.HasNoSavedChanges() ? 1 : (MessageBoxHelper.Show(Translate.GroupCardViewModel_Закрыть_без_сохранения_изменений_, PartnersHelper.ProgramName(), MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes ? 1 : 0);
      if (num == 0)
        return num != 0;
      ((ImageGoodViewModel) this.MyViewModel.ImageGood.DataContext).ImageSave.ForEach(new Action<string>(File.Delete));
      return num != 0;
    }

    private void StockOptionButton_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) sender).FindResource((object) "ContextMenuGrid") is ContextMenu resource))
        return;
      resource.Placement = PlacementMode.Bottom;
      resource.PlacementTarget = (UIElement) sender;
      resource.IsOpen = true;
    }

    private void WindowWithSize_Loaded(object sender, RoutedEventArgs e)
    {
      ContextMenu resource = (ContextMenu) this.StockOptionButton.FindResource((object) "ContextMenuGrid");
      foreach (GoodCardModelView.StockOptionItem filterProperty in (Collection<GoodCardModelView.StockOptionItem>) this.MyViewModel.FilterProperties)
      {
        ItemCollection items = resource.Items;
        MenuItem newItem = new MenuItem();
        newItem.Tag = (object) filterProperty.Option;
        newItem.Header = (object) filterProperty.Text;
        newItem.IsChecked = filterProperty.IsChecked;
        newItem.IsCheckable = true;
        items.Add((object) newItem);
      }
      resource.Closed += new RoutedEventHandler(this.CmButton_Closed);
    }

    private void CmButton_Closed(object sender, RoutedEventArgs e)
    {
      this.MyViewModel.FilterProperties = new ObservableCollection<GoodCardModelView.StockOptionItem>(((ItemsControl) sender).Items.Cast<MenuItem>().Select<MenuItem, GoodCardModelView.StockOptionItem>((Func<MenuItem, GoodCardModelView.StockOptionItem>) (x => new GoodCardModelView.StockOptionItem()
      {
        Option = (GoodCardModelView.StockOption) x.Tag,
        IsChecked = x.IsChecked,
        Text = this.MyViewModel.FilterProperties.SingleOrDefault<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (p => p.Option == (GoodCardModelView.StockOption) x.Tag))?.Text ?? string.Empty
      })));
      this.MyViewModel.Setting.GoodsCatalog.IsGroupStock = this.MyViewModel.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.Group)).IsChecked;
      this.MyViewModel.Setting.GoodsCatalog.IsCollapsedMinusStock = this.MyViewModel.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.CollapsedMinusStock)).IsChecked;
      this.MyViewModel.Setting.GoodsCatalog.IsCollapsedNullStock = this.MyViewModel.FilterProperties.Single<GoodCardModelView.StockOptionItem>((Func<GoodCardModelView.StockOptionItem, bool>) (x => x.Option == GoodCardModelView.StockOption.CollapsedNullStock)).IsChecked;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Market;component/forms/goods/goodcard/frmgoodcard.xaml", UriKind.Relative));
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
          this.TextBoxName = (TextBox) target;
          break;
        case 2:
          this.ComboBoxName = (ComboBox) target;
          break;
        case 3:
          this.StockGrid = (System.Windows.Controls.DataGrid) target;
          break;
        case 4:
          this.StockOptionButton = (Button) target;
          this.StockOptionButton.Click += new RoutedEventHandler(this.StockOptionButton_Click);
          break;
        case 5:
          this.SetStatusesComboBox = (ComboBox) target;
          break;
        case 6:
          this.ModificationControlPanel = (System.Windows.Controls.Frame) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
