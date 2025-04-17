// Decompiled with JetBrains decompiler
// Type: Gbs.Forms.GoodGroups.GroupCardViewModel
// Assembly: Market, Version=6.6.12.2634, Culture=neutral, PublicKeyToken=null
// MVID: 1F63B1D6-03C1-4223-9A1B-4EA2EB09E32F
// Assembly location: C:\Program Files (x86)\F-Lab\Market 6\Market.exe

using Gbs.Core;
using Gbs.Core.Config;
using Gbs.Core.Db.Goods;
using Gbs.Core.Entities;
using Gbs.Core.Entities.Goods;
using Gbs.Forms.Goods.GoodCard;
using Gbs.Helpers;
using Gbs.Helpers.ExternalApi;
using Gbs.Helpers.MVVM;
using Gbs.Resources.Localizations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace Gbs.Forms.GoodGroups
{
  public partial class GroupCardViewModel : ViewModelWithForm, ICheckChangesViewModel
  {
    private string _imageSave;
    private bool _isDataParent;
    public bool _isEditCard;

    private List<string> ImageDelete { get; set; }

    public bool IsEnabledDecimalPlace
    {
      get
      {
        return new ConfigsRepository<Settings>().Get().Sales.IsLimitedDecimalPlace && this.IsEnabledEditGroup;
      }
    }

    public bool IsEnabledProp
    {
      get
      {
        Gbs.Core.Entities.GoodGroups.Group group1 = this.Group;
        if ((group1 != null ? __nonvirtual (group1.Uid) : Guid.Empty) == GlobalDictionaries.PercentForServiceGroupUid)
          return false;
        if (this.Type == GlobalDictionaries.GoodTypes.Weight)
        {
          this.Group.IsRequestCount = true;
          this.OnPropertyChanged("Group");
        }
        if (this.Type == GlobalDictionaries.GoodTypes.Certificate)
        {
          this.Group.IsRequestCount = false;
          this.OnPropertyChanged("Group");
        }
        Gbs.Core.Entities.GoodGroups.Group group2 = this.Group;
        if ((group2 != null ? (group2.GoodsType != GlobalDictionaries.GoodTypes.Weight ? 1 : 0) : 1) == 0)
          return false;
        Gbs.Core.Entities.GoodGroups.Group group3 = this.Group;
        return group3 == null || group3.GoodsType != GlobalDictionaries.GoodTypes.Certificate;
      }
    }

    public bool SaveResult { get; set; }

    public string ParentName
    {
      get
      {
        if (this.ParentUid == Guid.Empty)
          return Translate.Devices_Нет;
        using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          return new GoodGroupsRepository(dataBase).GetByUid(this.ParentUid)?.Name ?? Translate.Devices_Нет;
      }
    }

    public ICommand SelectParent { get; set; }

    public ICommand SaveGroup { get; set; }

    public ICommand CloseCard { get; set; }

    public Action CloseFrm { get; set; }

    public Gbs.Core.Entities.GoodGroups.Group Group { get; set; }

    public Dictionary<GlobalDictionaries.GoodTypes, string> GoodsTypes { get; }

    public GlobalDictionaries.GoodTypes Type
    {
      get
      {
        Gbs.Core.Entities.GoodGroups.Group group = this.Group;
        return group == null ? GlobalDictionaries.GoodTypes.Single : group.GoodsType;
      }
      set
      {
        GlobalDictionaries.GoodTypes origValue = this.Type;
        if (value == this.Type)
          return;
        this.Group.GoodsType = value;
        switch (value)
        {
          case GlobalDictionaries.GoodTypes.Single:
            if (origValue == GlobalDictionaries.GoodTypes.Weight)
              break;
            goto default;
          case GlobalDictionaries.GoodTypes.Weight:
            if (origValue != GlobalDictionaries.GoodTypes.Single)
              goto default;
            else
              break;
          default:
            using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
            {
              if (dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.GROUP_UID == this.Group.Uid)).Any<GOODS>())
              {
                Application.Current?.Dispatcher?.BeginInvoke((Delegate) (() =>
                {
                  MessageBoxHelper.Warning(Translate.GroupCardViewModel_Невозможно_сменить_тип_категории__так_как_в_ней_есть_уже_товары);
                  this.Group.GoodsType = origValue;
                  this.OnPropertyChanged(nameof (Type));
                }), DispatcherPriority.ContextIdle, (object[]) null);
                return;
              }
              this.OnPropertyChanged("IsEnabledProp");
              this.OnPropertyChanged("Group");
              this.OnPropertyChanged(nameof (Type));
              this.OnPropertyChanged("IsEnabledFreePrice");
            }
            if (value != GlobalDictionaries.GoodTypes.Service)
              return;
            this.Group.IsFreePrice = true;
            this.OnPropertyChanged("Group");
            return;
        }
        this.OnPropertyChanged("IsEnabledProp");
        this.OnPropertyChanged("Group");
        this.OnPropertyChanged(nameof (Type));
      }
    }

    public GlobalDictionaries.RuMarkedProductionTypes RuMarkedProductionType
    {
      get
      {
        Gbs.Core.Entities.GoodGroups.Group group = this.Group;
        return group == null ? GlobalDictionaries.RuMarkedProductionTypes.None : group.RuMarkedProductionType;
      }
      set
      {
        this.Group.RuMarkedProductionType = value;
        if (value != GlobalDictionaries.RuMarkedProductionTypes.None)
        {
          this.Group.NeedComment = true;
          this.OnPropertyChanged("Group");
        }
        this.OnPropertyChanged("IsEnabledComment");
      }
    }

    public bool IsEnabledFreePrice => this.Type != GlobalDictionaries.GoodTypes.Service;

    public bool IsEnabledComment
    {
      get
      {
        Gbs.Core.Entities.GoodGroups.Group group = this.Group;
        return (group != null ? (int) group.RuMarkedProductionType : 0) == 0;
      }
    }

    public Dictionary<GlobalDictionaries.RuFfdGoodsTypes, string> RuFfdGoodsTypes { get; }

    public Dictionary<GlobalDictionaries.RuTaxSystems, string> RuTaxSystems { get; }

    public Dictionary<GlobalDictionaries.RuMarkedProductionTypes, string> RuMarkedProductionTypes
    {
      get
      {
        return GlobalDictionaries.MarkedProductionTypesList.Where<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, bool>) (x => x.Country.Any<GlobalDictionaries.Countries>((Func<GlobalDictionaries.Countries, bool>) (c => c.IsEither<GlobalDictionaries.Countries>(this.Config.Interface.Country, GlobalDictionaries.Countries.NotSet))))).Where<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, bool>) (x => x.Type != GlobalDictionaries.RuMarkedProductionTypes.Fur)).ToDictionary<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, GlobalDictionaries.RuMarkedProductionTypes, string>((Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, GlobalDictionaries.RuMarkedProductionTypes>) (x => x.Type), (Func<GlobalDictionaries.ItemForCountry<GlobalDictionaries.RuMarkedProductionTypes>, string>) (x => x.TypeName));
      }
    }

    private Settings Config => new ConfigsRepository<Settings>().Get();

    public Visibility UaVisibility
    {
      get
      {
        return new ConfigsRepository<Settings>().Get().Interface.Country != GlobalDictionaries.Countries.Ukraine ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility RussiaVisibility
    {
      get
      {
        return !new ConfigsRepository<Settings>().Get().Interface.Country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.NotSet, GlobalDictionaries.Countries.Russia) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Visibility MarkedVisibility
    {
      get
      {
        return !new ConfigsRepository<Settings>().Get().Interface.Country.IsEither<GlobalDictionaries.Countries>(GlobalDictionaries.Countries.NotSet, GlobalDictionaries.Countries.Russia, GlobalDictionaries.Countries.Ukraine, GlobalDictionaries.Countries.Armenia, GlobalDictionaries.Countries.Kazakhstan) ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    public Dictionary<int, FiscalKkm.TaxRate> TaxTypes { get; }

    public IEnumerable<GoodsUnits.GoodUnit> GoodUnits { get; }

    public bool IsDataParent
    {
      get => this._isDataParent;
      set
      {
        if (value && this.ParentUid == Guid.Empty)
        {
          MessageBoxHelper.Warning(Translate.GroupCardViewModel_Для_активации_данной_опции_требуется_выбрать_родителя);
        }
        else
        {
          using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
          {
            IQueryable<GOODS> source = dataBase.GetTable<GOODS>().Where<GOODS>((Expression<Func<GOODS, bool>>) (x => x.GROUP_UID == this.Group.Uid));
            Gbs.Core.Entities.GoodGroups.Group byUid = new GoodGroupsRepository(dataBase).GetByUid(this.ParentUid);
            if (source.Any<GOODS>())
            {
              if ((byUid != null ? byUid.GoodsType : this.Type) != this.Type)
              {
                switch (this.Type)
                {
                  case GlobalDictionaries.GoodTypes.Single:
                    if (byUid.GoodsType == GlobalDictionaries.GoodTypes.Weight)
                      goto label_12;
                    else
                      break;
                  case GlobalDictionaries.GoodTypes.Weight:
                    if (byUid.GoodsType == GlobalDictionaries.GoodTypes.Single)
                      goto label_12;
                    else
                      break;
                }
                MessageBoxHelper.Warning(Translate.GroupCardViewModel_Невозможно_активировать_данную_опцию__так_как_в_этой_категории_уже_есть_товары_);
                return;
              }
            }
          }
label_12:
          this._isDataParent = value;
          this.Group.IsDataParent = value;
          this.OnPropertyChanged(isUpdateAllProp: true);
        }
      }
    }

    public bool IsEnabledTab => !this.IsDataParent;

    private Guid ParentUid
    {
      get
      {
        Gbs.Core.Entities.GoodGroups.Group group = this.Group;
        return group == null ? Guid.Empty : group.ParentGroupUid;
      }
      set
      {
        this.Group.ParentGroupUid = value;
        if (value == Guid.Empty && this.IsDataParent)
          this.IsDataParent = false;
        this.OnPropertyChanged(isUpdateAllProp: true);
      }
    }

    public GroupCardViewModel()
    {
      List<GoodsUnits.GoodUnit> goodUnitList = new List<GoodsUnits.GoodUnit>();
      GoodsUnits.GoodUnit goodUnit = new GoodsUnits.GoodUnit();
      goodUnit.Uid = Guid.Empty;
      goodUnit.FullName = Translate.GroupCardViewModel_не_указаны;
      goodUnitList.Add(goodUnit);
      this.GoodUnits = (IEnumerable<GoodsUnits.GoodUnit>) goodUnitList;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool IsEnabledEditGroup
    {
      get
      {
        Gbs.Core.Entities.GoodGroups.Group group = this.Group;
        return (group != null ? __nonvirtual (group.Uid) : Guid.Empty) != GlobalDictionaries.PercentForServiceGroupUid;
      }
    }

    public GroupCardViewModel(Gbs.Core.Entities.GoodGroups.Group group)
    {
      List<GoodsUnits.GoodUnit> goodUnitList = new List<GoodsUnits.GoodUnit>();
      GoodsUnits.GoodUnit goodUnit = new GoodsUnits.GoodUnit();
      goodUnit.Uid = Guid.Empty;
      goodUnit.FullName = Translate.GroupCardViewModel_не_указаны;
      goodUnitList.Add(goodUnit);
      this.GoodUnits = (IEnumerable<GoodsUnits.GoodUnit>) goodUnitList;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Group = group;
      if (this.Group.Uid == GlobalDictionaries.PercentForServiceGroupUid)
        MessageBoxHelper.Warning(Translate.GroupCardViewModel_Данная_категория_является_системной__у_нее_нельзя_изменять_некоторые_параметры_);
      this.IsDataParent = group.IsDataParent;
      this.TaxTypes = new Dictionary<int, FiscalKkm.TaxRate>()
      {
        {
          0,
          new FiscalKkm.TaxRate(0M, Translate.GroupCardViewModel_По_умолчанию, -1)
        }
      };
      foreach (KeyValuePair<int, FiscalKkm.TaxRate> taxRate in new ConfigsRepository<Gbs.Core.Config.Devices>().Get().CheckPrinter.FiscalKkm.TaxRates)
        this.TaxTypes.Add(taxRate.Key, taxRate.Value);
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
        this.GoodUnits = this.GoodUnits.Concat<GoodsUnits.GoodUnit>(GoodsUnits.GetUnitsListWithFilter(dataBase.GetTable<GOODS_UNITS>().Where<GOODS_UNITS>((Expression<Func<GOODS_UNITS, bool>>) (x => !x.IS_DELETED))));
      Gbs.Core.Entities.GoodGroups.Group group1;
      this.SelectParent = (ICommand) new RelayCommand((Action<object>) (obj => this.ParentUid = new FormSelectGroup().GetSingleSelectedGroupUid(this.User, out group1) ? group1.Uid : Guid.Empty));
      this.SaveGroup = (ICommand) new RelayCommand((Action<object>) (obj => this.Save()));
      this.CloseCard = (ICommand) new RelayCommand((Action<object>) (obj =>
      {
        if (!this._imageSave.IsNullOrEmpty())
          File.Delete(this._imageSave);
        this.CloseFrm();
      }));
      this.OnPropertyChanged(nameof (IsEnabledComment));
      this.OnPropertyChanged(nameof (IsEnabledFreePrice));
      this.ImageLoad();
    }

    private void Save()
    {
      using (Gbs.Core.Db.DataBase dataBase = Data.GetDataBase())
      {
        GoodGroupsRepository rep = new GoodGroupsRepository(dataBase);
        this.SaveResult = rep.Save(this.Group);
        if (!this.SaveResult)
          return;
        this.ImageDelete.ForEach(new Action<string>(File.Delete));
        Task.Run((Action) (() =>
        {
          PlanfixSetting planfix = new ConfigsRepository<Integrations>().Get().Planfix;
          if (planfix.GoodEntityType == PlanfixSetting.GoodsEntityTypes.Handbook)
            PlanfixHelper.UpdateGoodGroupPfHandbook(this.Group, planfix);
          else
            PlanfixHelper.UpdateGoodGroupPfTask(this.Group, rep.GetByUid(this.Group.ParentGroupUid), planfix);
        }));
        ActionsHistoryHelper.AddActionThread(ActionsHistoryHelper.CreateHistory(this.EntityClone, (IEntity) this.Group, this._isEditCard ? ActionType.Edit : ActionType.Add, GlobalDictionaries.EntityTypes.GoodGroup, this.User), true);
        WindowWithSize.IsCancel = false;
        this.CloseFrm();
      }
    }

    public IEntity EntityClone { get; set; }

    public bool HasNoSavedChanges()
    {
      return Functions.IsObjectEqual<IEntity>(this.EntityClone, (IEntity) this.Group);
    }

    public Gbs.Core.Entities.Users.User User { get; set; }

    public ICommand AddImageCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (obj => this.AddImage()));
    }

    private void AddImage()
    {
      string str = Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, this.Group.Uid.ToString());
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = Translate.ImageGoodViewModel_Фото_товаров____png____bmp____jpg____png___bmp___jpg_;
      openFileDialog1.Multiselect = false;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      openFileDialog2.ShowDialog();
      if (openFileDialog2.FileName.IsNullOrEmpty() || !FileSystemHelper.ExistsOrCreateFolder(str))
        return;
      string extension = Path.GetExtension(openFileDialog2.FileName);
      string[] files = Directory.GetFiles(str);
      if (((IEnumerable<string>) files).Any<string>())
        this.ImageDelete.AddRange((IEnumerable<string>) files);
      string destPath = Path.Combine(str, DateTime.Now.ToString("yyyyMMdd_HHmmss") + extension);
      ImagesHelpers.CompressImage(openFileDialog2.FileName, destPath);
      this._imageSave = destPath;
      this.ImageLoad();
    }

    private void ImageLoad()
    {
      this.Image = (ImageGoodViewModel.ImageSource) null;
      DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(new ConfigsRepository<Gbs.Core.Config.DataBase>().Get().GoodsImagesPath, this.Group.Uid.ToString()));
      if (!directoryInfo.Exists)
        return;
      List<string> list = ((IEnumerable<string>) Directory.GetFiles(directoryInfo.FullName)).Where<string>((Func<string, bool>) (x => this.ImageDelete.All<string>((Func<string, bool>) (p => p != x)))).ToList<string>();
      if (!list.Any<string>())
        return;
      MemoryStream destination = new MemoryStream();
      using (FileStream fileStream = new FileStream(list.First<string>(), FileMode.Open, FileAccess.Read))
      {
        fileStream.CopyTo((Stream) destination);
        destination.Seek(0L, SeekOrigin.Begin);
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = (Stream) destination;
        bitmapImage.EndInit();
        this.Image = new ImageGoodViewModel.ImageSource()
        {
          Image = bitmapImage,
          Path = list.First<string>()
        };
      }
      this.OnPropertyChanged("Image");
    }

    public ICommand DeleteImageCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (obj =>
        {
          if (this.Image == null)
            return;
          this.ImageDelete.Add(this.Image.Path);
          this._imageSave = string.Empty;
          this.ImageLoad();
          this.OnPropertyChanged("Image");
        }));
      }
    }

    public ImageGoodViewModel.ImageSource Image { get; set; }
  }
}
