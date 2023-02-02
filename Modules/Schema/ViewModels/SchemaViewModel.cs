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
using System.Text;

namespace Schema.ViewModels
{
    public class SchemaViewModel : BindableBase
    {
        public SchemaViewModel()
        {

        }

        #region Field

        private string _ExportSchemaPath;
        public string ExportSchemaPath
        {
            get { return _ExportSchemaPath; }
            set { SetProperty(ref _ExportSchemaPath, value); }
        }

        private string _ExportLog = "Waiting...";
        public string ExportLog
        {
            get { return _ExportLog; }
            set { SetProperty(ref _ExportLog, value); }
        }

        #endregion

        #region Command

        private DelegateCommand _SelectExportSchemaPathCommand;
        public DelegateCommand SelectExportSchemaPathCommand => _SelectExportSchemaPathCommand ??= new DelegateCommand(ExecuteSelectExportSchemaPathCommand);

        void ExecuteSelectExportSchemaPathCommand()
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

            ExportSchemaPath = dialog.FileName;
        }

        private DelegateCommand _ExportSchemaCommand;
        public DelegateCommand ExportSchemaCommand => _ExportSchemaCommand ??= new DelegateCommand(async () => await ExecuteExportSchemaCommand());

        async Task ExecuteExportSchemaCommand()
        {
            /**
             * 1.遍历读取所有xml
             * 2.每种不同的xml新建结构
             * 3.添加所有字段
             */

            List<SchemaVO> schemaList = new();

            foreach (var xmlFile in Directory.EnumerateFiles(Config.ServerPath, "*.xml", SearchOption.AllDirectories))
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
            using FileStream fs = new(Path.Combine(ExportSchemaPath, "Schema.json"), FileMode.CreateNew);
            fs.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(schemaList)));
        }

        #endregion
    }
}
