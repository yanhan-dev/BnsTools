using Newtonsoft.Json.Linq;

using Prism.Events;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientEditor.ViewModels
{
    public class AttributeViewModel : BindableBase
    {

        public AttributeViewModel()
        {
        }

        private string _Attr;
        public string Attr
        {
            get { return _Attr; }
            set { SetProperty(ref _Attr, value); }
        }

        private string _AttrDesc;
        public string AttrDesc
        {
            get { return _AttrDesc; }
            set { SetProperty(ref _AttrDesc, value); }
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
