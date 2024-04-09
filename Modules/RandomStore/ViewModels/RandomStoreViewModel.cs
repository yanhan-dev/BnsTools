using Common;

using HandyControl.Controls;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

using Prism.Commands;
using Prism.Mvvm;

using RandomStore.POJO.DO;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RandomStore.ViewModels
{
    public class RandomStoreViewModel : BindableBase
    {
        public RandomStoreViewModel()
        {

        }

        RandomStoreItemGroup _RandomStoreItemGroup;

        #region Dependency Property

        private ObservableCollection<RandomStoreItemGroupRecord> _RandomStoreItemGroupRecords;
        public ObservableCollection<RandomStoreItemGroupRecord> RandomStoreItemGroupRecords
        {
            get { return _RandomStoreItemGroupRecords; }
            set { SetProperty(ref _RandomStoreItemGroupRecords, value); }
        }

        private RandomStoreItemGroupRecord _SelectedRecord;
        public RandomStoreItemGroupRecord SelectedRecord
        {
            get { return _SelectedRecord; }
            set
            {
                SetProperty(ref _SelectedRecord, value);
                if (value == null)
                {
                    return;
                }

                SelectedRandomStoreItems = new ObservableCollection<RandomStoreItem>
                {
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 1, Name = _SelectedRecord.Name1, Alias = _SelectedRecord.Item1, Count = _SelectedRecord.ItemCount1, Money = _SelectedRecord.ItemPriceMoney1 },
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 2, Name = _SelectedRecord.Name2, Alias = _SelectedRecord.Item2, Count = _SelectedRecord.ItemCount2, Money = _SelectedRecord.ItemPriceMoney2 },
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 3, Name = _SelectedRecord.Name3, Alias = _SelectedRecord.Item3, Count = _SelectedRecord.ItemCount3, Money = _SelectedRecord.ItemPriceMoney3 },
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 4, Name = _SelectedRecord.Name4, Alias = _SelectedRecord.Item4, Count = _SelectedRecord.ItemCount4, Money = _SelectedRecord.ItemPriceMoney4 },
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 5, Name = _SelectedRecord.Name5, Alias = _SelectedRecord.Item5, Count = _SelectedRecord.ItemCount5, Money = _SelectedRecord.ItemPriceMoney5 },
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 6, Name = _SelectedRecord.Name6, Alias = _SelectedRecord.Item6, Count = _SelectedRecord.ItemCount6, Money = _SelectedRecord.ItemPriceMoney6 },
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 7, Name = _SelectedRecord.Name7, Alias = _SelectedRecord.Item7, Count = _SelectedRecord.ItemCount7, Money = _SelectedRecord.ItemPriceMoney7 },
                    new RandomStoreItem { ParentId = _SelectedRecord.Id, Id = 8, Name = _SelectedRecord.Name8, Alias = _SelectedRecord.Item8, Count = _SelectedRecord.ItemCount8, Money = _SelectedRecord.ItemPriceMoney8 }
                };

                foreach (var item in SelectedRandomStoreItems)
                {
                    item.ItemChanged += Item_ItemChanged;
                }
            }
        }

        private ObservableCollection<RandomStoreItem> _SelectedRandomStoreItems;
        public ObservableCollection<RandomStoreItem> SelectedRandomStoreItems
        {
            get { return _SelectedRandomStoreItems ??= new ObservableCollection<RandomStoreItem>(); }
            set { SetProperty(ref _SelectedRandomStoreItems, value); }
        }

        private DelegateCommand _ReadGroupCommand;
        public DelegateCommand ReadGroupCommand =>
            _ReadGroupCommand ?? (_ReadGroupCommand = new DelegateCommand(ExecuteReadGroupCommand));

        void ExecuteReadGroupCommand()
        {
            using FileStream fileStream = File.Open(Path.Combine(Config.ServerPath, "randomstoreitemgroupdata.xml"), FileMode.Open);
            StreamReader streamReader = new(fileStream, new UTF8Encoding(false));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RandomStoreItemGroup));
            _RandomStoreItemGroup = (RandomStoreItemGroup)xmlSerializer.Deserialize(streamReader);
            foreach (var record in _RandomStoreItemGroup.RandomStoreItemGroupRecords)
            {
                record.Name1 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item1, "未知");
                record.Name2 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item2, "未知");
                record.Name3 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item3, "未知");
                record.Name4 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item4, "未知");
                record.Name5 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item5, "未知");
                record.Name6 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item6, "未知");
                record.Name7 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item7, "未知");
                record.Name8 = Translation.Translate.GetValueOrDefault("Item.Name2." + record.Item8, "未知");
            }

            RandomStoreItemGroupRecords = new ObservableCollection<RandomStoreItemGroupRecord>(_RandomStoreItemGroup.RandomStoreItemGroupRecords);

            SelectedRecord = RandomStoreItemGroupRecords.FirstOrDefault();
        }

        private DelegateCommand _SaveGroupCommand;
        public DelegateCommand SaveGroupCommand =>
            _SaveGroupCommand ?? (_SaveGroupCommand = new DelegateCommand(ExecuteSaveGroupCommand));

        void ExecuteSaveGroupCommand()
        {
            _RandomStoreItemGroup.RandomStoreItemGroupRecords = RandomStoreItemGroupRecords.ToList();

            using FileStream fileStream = File.Create(Path.Combine(Config.ServerPath, "randomstoreitemgroupdata.xml"));
            StreamWriter streamWriter = new(fileStream, new UTF8Encoding(false));
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RandomStoreItemGroup));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xmlSerializer.Serialize(streamWriter, _RandomStoreItemGroup, ns);
            MessageBox.Success("保存成功");
        }

        #endregion


        #region Method        

        private void Item_ItemChanged(object sender, RandomStoreItem parameter)
        {
            var parentRecord = RandomStoreItemGroupRecords.FirstOrDefault(ss => ss.Id == parameter.ParentId);
            switch (parameter.Id)
            {
                case 1:
                    if (parentRecord.Item1 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name1 = parameter.Name;
                    parentRecord.Item1 = parameter.Alias;
                    parentRecord.ItemCount1 = parameter.Count;
                    parentRecord.ItemPriceMoney1 = parameter.Money;
                    break;
                case 2:
                    if (parentRecord.Item2 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name2 = parameter.Name;
                    parentRecord.Item2 = parameter.Alias;
                    parentRecord.ItemCount2 = parameter.Count;
                    parentRecord.ItemPriceMoney2 = parameter.Money;
                    break;
                case 3:
                    if (parentRecord.Item3 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name3 = parameter.Name;
                    parentRecord.Item3 = parameter.Alias;
                    parentRecord.ItemCount3 = parameter.Count;
                    parentRecord.ItemPriceMoney3 = parameter.Money;
                    break;
                case 4:
                    if (parentRecord.Item4 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name4 = parameter.Name;
                    parentRecord.Item4 = parameter.Alias;
                    parentRecord.ItemCount4 = parameter.Count;
                    parentRecord.ItemPriceMoney4 = parameter.Money;
                    break;
                case 5:
                    if (parentRecord.Item5 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name5 = parameter.Name;
                    parentRecord.Item5 = parameter.Alias;
                    parentRecord.ItemCount5 = parameter.Count;
                    parentRecord.ItemPriceMoney5 = parameter.Money;
                    break;
                case 6:
                    if (parentRecord.Item6 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name6 = parameter.Name;
                    parentRecord.Item6 = parameter.Alias;
                    parentRecord.ItemCount6 = parameter.Count;
                    parentRecord.ItemPriceMoney6 = parameter.Money;
                    break;
                case 7:
                    if (parentRecord.Item7 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name7 = parameter.Name;
                    parentRecord.Item7 = parameter.Alias;
                    parentRecord.ItemCount7 = parameter.Count;
                    parentRecord.ItemPriceMoney7 = parameter.Money;
                    break;
                case 8:
                    if (parentRecord.Item8 != parameter.Alias)
                    {
                        parameter.Name = Translation.Translate.GetValueOrDefault(parameter.Alias, "未知");
                    }
                    parentRecord.Name8 = parameter.Name;
                    parentRecord.Item8 = parameter.Alias;
                    parentRecord.ItemCount8 = parameter.Count;
                    parentRecord.ItemPriceMoney8 = parameter.Money;
                    break;
            }
        }

        #endregion

    }
}
