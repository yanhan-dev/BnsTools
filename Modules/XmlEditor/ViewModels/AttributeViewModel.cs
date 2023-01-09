using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditor.ViewModels
{
    public class AttributeViewModel : BindableBase
    {
        private string _Attribute;
        public string Attribute
        {
            get { return _Attribute; }
            set { SetProperty(ref _Attribute, value); }
        }

        private string _AttributeDesc;
        public string AttributeDesc
        {
            get { return _AttributeDesc; }
            set { SetProperty(ref _AttributeDesc, value); }
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
