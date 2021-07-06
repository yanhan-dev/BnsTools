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
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ServerDirPath = dialog.SelectedPath;
            }
        }

        private DelegateCommand _selectOutSchemaPathCommand;
        public DelegateCommand SelectOutSchemaPathCommand => _selectOutSchemaPathCommand ??= new DelegateCommand(ExecuteSelectOutSchemaPathCommand);

        void ExecuteSelectOutSchemaPathCommand()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择输出目录";
            //dialog.SelectedPath = "D:/";
            //dialog.RootFolder = Environment.SpecialFolder.Programs;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                OutSchemaPath = dialog.SelectedPath;
            }
        }

        private DelegateCommand _exportSchemaCommand;
        public DelegateCommand ExportSchemaCommand => _exportSchemaCommand ??= new DelegateCommand(ExecuteExportSchemaCommand);

        void ExecuteExportSchemaCommand()
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
                foreach (var node in xml.Root.Nodes())
                {

                } 
                schemaVO.SchemaDictionary.TryAdd("", "");
            }

        }

        #endregion
    }
}
