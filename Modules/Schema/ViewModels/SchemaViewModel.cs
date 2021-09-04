using Prism.Commands;
using Prism.Mvvm;
using Microsoft.Win32;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using Schema.POJO.VO;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Serialization;
using Common;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Schema.ViewModels
{
    public class SchemaViewModel : BindableBase
    {
        public SchemaViewModel()
        {

        }

        #region Field

        private string _serverDirPath;
        public string ServerDirPath
        {
            get { return _serverDirPath; }
            set { SetProperty(ref _serverDirPath, value); }
        }

        private string _outSchemaPath;
        public string OutSchemaPath
        {
            get { return _outSchemaPath; }
            set { SetProperty(ref _outSchemaPath, value); }
        }

        private string _ExportLog = "等待中...";
        public string ExportLog
        {
            get { return _ExportLog; }
            set { SetProperty(ref _ExportLog, value); }
        }

        #endregion

        #region Command

        private DelegateCommand _selectServerDirCommand;
        public DelegateCommand SelectServerDirCommand => _selectServerDirCommand ??= new DelegateCommand(ExecuteSelectServerDirCommand);

        void ExecuteSelectServerDirCommand()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择服务端目录";
            //dialog.SelectedPath = "D:/";
            //dialog.RootFolder = Environment.SpecialFolder.Programs;
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ServerDirPath = dialog.SelectedPath;
            if (string.IsNullOrWhiteSpace(OutSchemaPath))
            {
                OutSchemaPath = Path.Combine(ServerDirPath, "Schema");
            }
        }

        private DelegateCommand _selectOutSchemaPathCommand;
        public DelegateCommand SelectOutSchemaPathCommand => _selectOutSchemaPathCommand ??= new DelegateCommand(ExecuteSelectOutSchemaPathCommand);

        void ExecuteSelectOutSchemaPathCommand()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "选择输出目录"
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            OutSchemaPath = dialog.FileName;
        }

        private DelegateCommand _exportSchemaCommand;
        public DelegateCommand ExportSchemaCommand => _exportSchemaCommand ??= new DelegateCommand(async () => await ExecuteExportSchemaCommand());

        async Task ExecuteExportSchemaCommand()
        {
            /**
             * 1.遍历读取所有xml
             * 2.每种不同的xml新建结构
             * 3.添加所有字段
             */

            List<SchemaVO> schemaList = new();

            foreach (var xmlFile in Directory.EnumerateFiles(ServerDirPath, "*.xml", SearchOption.AllDirectories))
            {
                var xml = XDocument.Load(xmlFile);
                string type = xml.Root.Attribute("type").Value;
                SchemaVO schemaVO = schemaList.FirstOrDefault(t => type.Equals(t.TableName));
                if (schemaVO == null)
                {
                    schemaVO = new SchemaVO
                    {
                        TableName = type,
                    };
                    schemaList.Add(schemaVO);
                }
                foreach (var element in xml.Root.Elements())
                {
                    if (element.NodeType == XmlNodeType.Comment)
                    {
                        continue;
                    }
                    foreach (var attribute in element.Attributes())
                    {
                        schemaVO.SchemaDictionary.TryAdd(attribute.Name.LocalName, attribute.Value);
                    }
                }
            }

            await File.WriteAllTextAsync(Path.Combine(OutSchemaPath, "Schema.json"),
                JsonConvert.SerializeObject(schemaList));

        }

        #endregion
    }
}
