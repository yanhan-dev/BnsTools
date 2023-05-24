using Common;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Windows;
using HandyControl.Tools.Extension;
using Newtonsoft.Json;
using System.Collections.Immutable;
using Common.Model;
using Prism.Events;
using HandyControl.Controls;
using HandyControl.Data;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using AngleSharp.Css;

namespace ServerEditor.ViewModels
{
    public class ServerEditorViewModel : BindableBase
    {
        public ServerEditorViewModel()
        {
            Files = new ObservableCollection<FilesViewModel>(GetFiles(Config.ServerPath));
        }

        #region Dependency Property

        private ObservableCollection<FilesViewModel> _Files;
        public ObservableCollection<FilesViewModel> Files
        {
            get { return _Files; }
            set { SetProperty(ref _Files, value); }
        }

        private int _FilesSelectedIndex;
        public int FilesSelectedIndex
        {
            get { return _FilesSelectedIndex; }
            set { SetProperty(ref _FilesSelectedIndex, value); }
        }

        private ObservableCollection<EditingFileViewModel> _EditingFiles;
        public ObservableCollection<EditingFileViewModel> EditingFiles
        {
            get { return _EditingFiles ??= new ObservableCollection<EditingFileViewModel>(); }
            set { SetProperty(ref _EditingFiles, value); }
        }


        private int _EditingFilesSelectedIndex;
        public int EditingFilesSelectedIndex
        {
            get { return _EditingFilesSelectedIndex; }
            set { SetProperty(ref _EditingFilesSelectedIndex, value); }
        }

        #endregion

        #region Command
        private DelegateCommand<KeyEventArgs> _FileNavigationCommand;
        public DelegateCommand<KeyEventArgs> FileNavigationCommand => _FileNavigationCommand ??= new DelegateCommand<KeyEventArgs>(OnFileNavigation);

        private void OnFileNavigation(KeyEventArgs e)
        {
            // 获取按下的字母键
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                char keyChar = (char)('A' + (e.Key - Key.A)); // 获取按下的字母字符
                string firstChar = keyChar.ToString().ToUpper(); // 转换为大写

                // 查找第一个以指定字母开头的文件名
                int? index = GetFirstFileNameStartingWith(firstChar);
                if (null != index)
                {
                    FilesSelectedIndex = index.Value;
                }

                e.Handled = true; // 标记事件已处理，避免其他事件处理程序响应按键
            }
        }

        private int? GetFirstFileNameStartingWith(string firstChar)
        {
            for (int i = FilesSelectedIndex + 1; i < Files.Count; i++)
            {
                if (Files[i].Name.StartsWith(firstChar, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            for (int i = 0; i < FilesSelectedIndex; i++)
            {
                if (Files[i].Name.StartsWith(firstChar, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return null;
        }

        private DelegateCommand<FilesViewModel> _OpenFileCommand;
        public DelegateCommand<FilesViewModel> OpenFileCommand => _OpenFileCommand ??= new DelegateCommand<FilesViewModel>(ExecuteOpenFileCommand);

        void ExecuteOpenFileCommand(FilesViewModel parameter)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = parameter.Uri
                }
            };
            process.Start();
        }

        private DelegateCommand<FilesViewModel> _FileDoubleClickCommand;
        public DelegateCommand<FilesViewModel> FileDoubleClickCommand => _FileDoubleClickCommand ??= new DelegateCommand<FilesViewModel>(ExecuteCommand);

        void ExecuteCommand(FilesViewModel file)
        {
            var find = EditingFiles.FirstOrDefault(ss => ss.Uri == file.Uri);
            if (find != null)
            {
                EditingFilesSelectedIndex = EditingFiles.IndexOf(find);
                return;
            }

            EditingFileViewModel editingFileViewModel = null;

            try
            {
                editingFileViewModel = new EditingFileViewModel()
                {
                    Name = file.Name,
                    Uri = file.Uri,
                };
                editingFileViewModel.Load();
            }
            catch (XmlException e)
            {
                HandyControl.Controls.MessageBox.Error(e.Message, file.Name);
                return;
            }


            EditingFiles.Insert(0, editingFileViewModel);

            EditingFilesSelectedIndex = 0;
        }

        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand => _SaveCommand ??= new DelegateCommand(ExecuteSaveCommand);

        void ExecuteSaveCommand()
        {
            if (!EditingFiles[EditingFilesSelectedIndex].IsEditing)
            {
                return;
            }

            EditingFiles[EditingFilesSelectedIndex].Save();
        }

        private DelegateCommand<EditingFileViewModel> _CloseTabCommand;
        public DelegateCommand<EditingFileViewModel> CloseTabCommand => _CloseTabCommand ??= new DelegateCommand<EditingFileViewModel>(ExecuteCloseTabCommand);

        void ExecuteCloseTabCommand(EditingFileViewModel parameter)
        {
            EditingFiles.Remove(parameter);
        }

        private DelegateCommand<object> _TabClosingCommand;
        public DelegateCommand<object> TabClosingCommand => _TabClosingCommand ??= new DelegateCommand<object>(ExecuteTabClosingCommand);

        void ExecuteTabClosingCommand(object parameter)
        {

        }

        private DelegateCommand<object> _ClosingTabCommand;
        public DelegateCommand<object> ClosingTabCommand => _ClosingTabCommand ??= new DelegateCommand<object>(ExecuteClosingTabCommand);

        void ExecuteClosingTabCommand(object parameter)
        {

        }

        #endregion

        #region Method

        private IEnumerable<FilesViewModel> GetFiles(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            return files.Select(file =>
            {
                return new FilesViewModel
                {
                    Uri = file,
                    Name = Path.GetFileName(file),
                    Desc = "空"
                };
            });
        }

        #endregion
    }
}
