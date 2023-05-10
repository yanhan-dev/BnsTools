using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEditor.ViewModels
{
    public class ElementViewModel : BindableBase
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _NameDesc;
        public string NameDesc
        {
            get { return _NameDesc; }
            set { SetProperty(ref _NameDesc, value); }
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }

        private string _ValueDesc;
        public string ValueDesc
        {
            get { return _ValueDesc; }
            set { SetProperty(ref _ValueDesc, value); }
        }
    }
}
