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

namespace XmlEditor.ViewModels
{
    public class XmlEditorViewModel : BindableBase
    {
        public XmlEditorViewModel()
        {
            Files = new ObservableCollection<FilesViewModel>(GetFiles(Config.ServerPath));
        }

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

        #region Command

        private DelegateCommand<FilesViewModel> _FileDoubleClickCommand;
        public DelegateCommand<FilesViewModel> FileDoubleClickCommand =>
            _FileDoubleClickCommand ?? (_FileDoubleClickCommand = new DelegateCommand<FilesViewModel>(ExecuteCommand));

        void ExecuteCommand(FilesViewModel file)
        {
            if (EditingFiles.FirstOrDefault(ss => ss.Uri == file.Uri) != null)
            {
                return;
            }

            EditingFiles.Add(new EditingFileViewModel
            {
                Name = file.Name,
                Uri = file.Uri,
            });

            EditingFilesSelectedIndex = EditingFiles.Count - 1;
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
