using ExtractItem.DAO.GoodsDb;
using ExtractItem.POJO.VO;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

using MoreLinq.Extensions;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ExtractItem.ViewModels
{
    public class ExtractItemViewModel : BindableBase
    {
        private List<ItemVO> _Items = new();

        public ExtractItemViewModel()
        {
        }

        #region Dependency Property

        private string _TranslateFilePath;
        public string TranslateFilePath
        {
            get { return _TranslateFilePath; }
            set { SetProperty(ref _TranslateFilePath, value); }
        }

        private string _ServerPath;
        public string ServerPath
        {
            get { return _ServerPath; }
            set { SetProperty(ref _ServerPath, value); }
        }

        private string _ExportPath;
        public string ExportPath
        {
            get { return _ExportPath; }
            set { SetProperty(ref _ExportPath, value); }
        }

        private string _ExportLog = "等待中...";
        public string ExportLog
        {
            get { return _ExportLog; }
            set { SetProperty(ref _ExportLog, value); }
        }

        private bool _IsEnableExport = true;
        public bool IsEnableExport
        {
            get { return _IsEnableExport; }
            set { SetProperty(ref _IsEnableExport, value); }
        }


        private string _ItemsFilePath;
        public string ItemsFilePath
        {
            get { return _ItemsFilePath; }
            set { SetProperty(ref _ItemsFilePath, value); }
        }

        private string _ConnString;
        public string ConnString
        {
            get { return _ConnString; }
            set { SetProperty(ref _ConnString, value); }
        }

        private string _ImportLog = "等待中...";
        public string ImportLog
        {
            get { return _ImportLog; }
            set { SetProperty(ref _ImportLog, value); }
        }

        #endregion


        #region Command

        private DelegateCommand _OpenTranslateCommand;
        public DelegateCommand OpenTranslateCommand => _OpenTranslateCommand ??= new DelegateCommand(ExecuteOpenTranslateCommand).ObservesCanExecute(() => IsEnableExport);

        void ExecuteOpenTranslateCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "xml 文件(*.xml)|*.xml" };
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            TranslateFilePath = ofd.FileName;
        }

        private DelegateCommand _OpenServerPathCommand;
        public DelegateCommand OpenServerPathCommand => _OpenServerPathCommand ??= new DelegateCommand(ExecuteOpenServerPathCommand).ObservesCanExecute(() => IsEnableExport);

        void ExecuteOpenServerPathCommand()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "请选择 服务端Data 文件夹"
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            ServerPath = dialog.FileName;
        }

        private DelegateCommand _OpenExportPathCommand;
        public DelegateCommand OpenExportPathCommand => _OpenExportPathCommand ??= new DelegateCommand(ExecuteOpenExportPathCommand).ObservesCanExecute(() => IsEnableExport);

        void ExecuteOpenExportPathCommand()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "请选择 导出 文件夹"
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            ExportPath = dialog.FileName;
        }

        private DelegateCommand _ExportCommand;
        public DelegateCommand ExportCommand => _ExportCommand ??= new DelegateCommand(ExecuteExportCommand).ObservesCanExecute(() => IsEnableExport);

        async void ExecuteExportCommand()
        {
            IsEnableExport = false;
            /**
             * 1.加载服务端所有Item
             * 2.加载翻译         
             * 3.提取所有ItemId和Alias
             * 4.根据Alias找汉化
             * 5.更新数据库
             */
            ExportLog = "正在加载服务端Item...";
            List<ItemVO> items = await LoadIdAndAlias(ServerPath);

            ExportLog = "正在加载翻译...";
            Dictionary<string, string> translate = await LoadTranslate(TranslateFilePath);

            ExportLog = "正在翻译物品...";
            foreach (var item in items)
            {
                item.Name = translate.GetValueOrDefault(item.Alias, item.Alias);
            }

            ExportLog = "正在导出文件...";
            await ExportItemFile(items, ExportPath);

            ExportLog = "完成!";
            IsEnableExport = true;
        }


        private DelegateCommand _OpenItemsCommand;
        public DelegateCommand OpenItemsCommand =>
            _OpenItemsCommand ?? (_OpenItemsCommand = new DelegateCommand(ExecuteOpenItemsCommand));

        void ExecuteOpenItemsCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "xml 文件(*.xml)|*.xml" };
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            ItemsFilePath = ofd.FileName;
        }

        private DelegateCommand _ImportCommand;
        public DelegateCommand ImportCommand =>
            _ImportCommand ?? (_ImportCommand = new DelegateCommand(ExecuteImportCommand));

        async void ExecuteImportCommand()
        {
            try
            {
                ImportLog = "正在读取Items文件...";
                TableVO table = await LoadItemsFile(ItemsFilePath);

                ImportLog = "正在导入数据库...";
                GoodsDbHelper goodsDbHelper = new GoodsDbHelper(ConnString);
                await goodsDbHelper.ImportItems(table.Items);
                ImportLog = "完成!";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}");
            }
        }

        private async Task<TableVO> LoadItemsFile(string itemsFilePath)
        {
            return await Task.Run(() =>
            {
                FileStream fileStream = File.Open(itemsFilePath, FileMode.Open);
                using (StreamReader streamReader = new(fileStream, new UTF8Encoding(false)))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableVO));
                    TableVO table = (TableVO)xmlSerializer.Deserialize(streamReader);
                    return table;
                }
            });
        }

        #endregion


        #region Method

        private async Task<List<ItemVO>> LoadIdAndAlias(string path)
        {
            return await Task.Run(() =>
            {
                List<ItemVO> items = new();
                foreach (var xmlFile in Directory.EnumerateFiles(path, "*.xml", SearchOption.TopDirectoryOnly))
                {
                    XDocument xml = XDocument.Load(xmlFile);
                    string type = xml.Root.Attribute("type").Value;
                    if (!string.Equals(type, "item"))
                    {
                        continue;
                    }

                    foreach (var element in xml.Root.Elements())
                    {
                        if (element.NodeType == XmlNodeType.Comment)
                        {
                            continue;
                        }
                        string alias = element.Attribute("alias").Value;
                        int id = int.Parse(element.Attribute("id").Value);

                        items.Add(new ItemVO
                        {
                            Id = id,
                            Alias = alias,
                        });
                    }
                }
                return items.OrderBy(item => item.Id).DistinctBy(item => item.Id).ToList();
            });
        }

        private async Task<Dictionary<string, string>> LoadTranslate(string path)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, string> translate = new();

                XDocument xml = XDocument.Load(path);
                foreach (var element in xml.Root.Elements())
                {
                    if (element.NodeType == XmlNodeType.Comment)
                    {
                        continue;
                    }

                    string alias = element.Attribute("alias").Value;
                    if (!alias.StartsWith("Item.Name2."))
                    {
                        continue;
                    }

                    alias = alias[11..];

                    string name = element.Elements().FirstOrDefault().Value;
                    translate[alias] = name;
                }
                return translate;
            });
        }


        private async Task ExportItemFile(List<ItemVO> items, string path)
        {
            await Task.Run(() =>
             {
                 using FileStream fileStream = File.Create(Path.Combine(path, "Items.xml"));
                 StreamWriter streamWriter = new StreamWriter(fileStream, new UTF8Encoding(false));
                 XmlSerializer xmlSerializer = new XmlSerializer(typeof(TableVO));

                 XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                 ns.Add("", "");
                 xmlSerializer.Serialize(streamWriter, new TableVO { Items = items }, ns);
             });
        }

        #endregion

    }
}
