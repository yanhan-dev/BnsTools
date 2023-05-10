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
                using FileStream fs = File.Create(configFileName);
                var bytes = Encoding.UTF8.GetBytes(Resources.ConfXml);
                fs.Write(bytes, 0, bytes.Length);
            }

            ServerPath = ConfigurationManager.AppSettings[nameof(ServerPath)];
            ClientPath = ConfigurationManager.AppSettings[nameof(ClientPath)];
            DescPath = ConfigurationManager.AppSettings[nameof(DescPath)];
            TranslatePath = ConfigurationManager.AppSettings[nameof(TranslatePath)];

            Common.Config.ServerPath = ServerPath;
            Common.Config.ClientPath = ClientPath;
            Common.Config.DescPath = DescPath;
            Common.Config.TranslatePath = TranslatePath;
        }
        #region Dependency Property

        private bool _IsEnableExport = true;
        public bool IsEnableExport
        {
            get { return _IsEnableExport; }
            set { SetProperty(ref _IsEnableExport, value); }
        }

        private string _TranslateFilePath;
        public string TranslatePath
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

        private string _ClientPath;
        public string ClientPath
        {
            get { return _ClientPath; }
            set { SetProperty(ref _ClientPath, value); }
        }

        private string _ConfigPath;
        public string DescPath
        {
            get { return _ConfigPath; }
            set { SetProperty(ref _ConfigPath, value); }
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

            TranslatePath = ofd.FileName;
        }

        private DelegateCommand _OpenServerPathCommand;
        public DelegateCommand OpenServerPathCommand => _OpenServerPathCommand ??= new DelegateCommand(ExecuteOpenServerPathCommand).ObservesCanExecute(() => IsEnableExport);

        void ExecuteOpenServerPathCommand()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "请选择服务端XML文件夹"
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            ServerPath = dialog.FileName;
        }

        private DelegateCommand _OpenClientPathCommand;
        public DelegateCommand OpenClientPathCommand => _OpenClientPathCommand ??= new DelegateCommand(ExecuteOpenClientPathCommand).ObservesCanExecute(() => IsEnableExport);

        void ExecuteOpenClientPathCommand()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "请选择客户端XML文件夹"
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            ClientPath = dialog.FileName;
        }



        private DelegateCommand _OpenDescPathCommand;
        public DelegateCommand OpenDescPathCommand => _OpenDescPathCommand ??= new DelegateCommand(ExecuteOpenDescPathCommand);

        void ExecuteOpenDescPathCommand()
        {
            CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true,
                Title = "请选择YAML解析文件夹"
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return;
            }

            DescPath = dialog.FileName;
        }

        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ??= new DelegateCommand(ExecuteSaveCommand);

        void ExecuteSaveCommand()
        {
            Common.Config.ServerPath = ServerPath;
            Common.Config.ClientPath = ClientPath;
            Common.Config.DescPath = DescPath;
            Common.Config.TranslatePath = TranslatePath;

            AddOrUpdateConfig(nameof(ServerPath), ServerPath);
            AddOrUpdateConfig(nameof(ClientPath), ClientPath);
            AddOrUpdateConfig(nameof(DescPath), DescPath);
            AddOrUpdateConfig(nameof(TranslatePath), TranslatePath);

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
            TranslatePath = Common.Config.TranslatePath;
        }
    }
}
