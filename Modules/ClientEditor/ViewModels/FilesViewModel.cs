using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientEditor.ViewModels
{
    public class FilesViewModel : BindableBase
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _Uri;
        public string Uri
        {
            get { return _Uri; }
            set { SetProperty(ref _Uri, value); }
        }

        private string _Desc;
        public string Desc
        {
            get { return _Desc; }
            set { SetProperty(ref _Desc, value); }
        }
    }
}
