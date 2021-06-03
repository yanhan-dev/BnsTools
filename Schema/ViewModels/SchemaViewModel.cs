using Prism.Commands;
using Prism.Mvvm;
using Microsoft.Win32;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
        public DelegateCommand SelectServerDirCommand =>
            _selectServerDirCommand ?? (_selectServerDirCommand = new DelegateCommand(ExecuteSelectServerDirCommand));

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
        public DelegateCommand SelectOutSchemaPathCommand =>
            _selectOutSchemaPathCommand ?? (_selectOutSchemaPathCommand = new DelegateCommand(ExecuteSelectOutSchemaPathCommand));

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

        #endregion
    }
}
