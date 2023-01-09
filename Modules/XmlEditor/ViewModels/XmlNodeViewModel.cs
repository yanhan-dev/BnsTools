using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditor.ViewModels
{
    public class XmlNodeViewModel: BindableBase
    {
        private string _Alias;
        public string Alias
        {
            get { return _Alias; }
            set { SetProperty(ref _Alias, value); }
        }

        private string _Desc;
        public string Desc
        {
            get { return _Desc; }
            set { SetProperty(ref _Desc, value); }
        }

        private ObservableCollection<AttributeViewModel> _XmlAttributes;
        public ObservableCollection<AttributeViewModel> XmlAttributes
        {
            get { return _XmlAttributes; }
            set { SetProperty(ref _XmlAttributes, value); }
        }
    }
}
