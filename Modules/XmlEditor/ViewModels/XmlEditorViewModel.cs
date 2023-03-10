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

namespace XmlEditor.ViewModels
{
    public class XmlEditorViewModel : BindableBase
    {
        public XmlEditorViewModel()
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
            }
            catch (XmlException e)
            {
                HandyControl.Controls.MessageBox.Error(e.Message, file.Name);
                return;
            }


            EditingFiles.Add(editingFileViewModel);

            EditingFilesSelectedIndex = EditingFiles.Count - 1;
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
