using Config.Properties;

using HandyControl.Controls;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Config.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        public ConfigViewModel()
        {
            //检查配置文件
            string configFileName = Process.GetCurrentProcess().MainModule?.FileName + ".Config";
            if (!File.Exists(configFileName))
            {
                using (FileStream fs = File.Create(configFileName))
                {
                    var bytes = Encoding.UTF8.GetBytes(Resources.ConfXml);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            ServerPath = ConfigurationManager.AppSettings["ServerPath"];
            TranslateFilePath = ConfigurationManager.AppSettings["TranslateFilePath"];

            Common.Config.ServerPath = ServerPath;
            Common.Config.TranslateFilePath = TranslateFilePath;
        }
        #region Dependency Property

        private bool _IsEnableExport = true;
        public bool IsEnableExport
        {
            get { return _IsEnableExport; }
            set { SetProperty(ref _IsEnableExport, value); }
        }

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


        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand =>
            _SaveCommand ?? (_SaveCommand = new DelegateCommand(ExecuteSaveCommand));

        void ExecuteSaveCommand()
        {
            Common.Config.ServerPath = ServerPath;
            Common.Config.TranslateFilePath = TranslateFilePath;

            AddOrUpdateConfig("ServerPath", ServerPath);
            AddOrUpdateConfig("TranslateFilePath", TranslateFilePath);

            MessageBox.Success("保存成功");
        }
        #endregion

        private void AddOrUpdateConfig(string key, string value)
        {
            Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        private DelegateCommand _CancelCommand;
        public DelegateCommand CancelCommand =>
            _CancelCommand ?? (_CancelCommand = new DelegateCommand(ExecuteCancelCommand));

        void ExecuteCancelCommand()
        {
            ServerPath = Common.Config.ServerPath;
            TranslateFilePath = Common.Config.TranslateFilePath;
        }
    }
}
