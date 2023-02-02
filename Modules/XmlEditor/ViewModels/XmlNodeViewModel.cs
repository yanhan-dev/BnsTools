using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlEditor.ViewModels
{
    public class XmlNodeViewModel: BindableBase
    {
        public bool UnUse { get; set; }

        public XNode Node { get; set; }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        private string _Desc;
        public string Desc
        {
            get { return _Desc; }
            set { SetProperty(ref _Desc, value); }
        }

        public string DescAttr { get; set; }

        private ObservableCollection<AttributeViewModel> _XmlAttributes;
        public ObservableCollection<AttributeViewModel> XmlAttributes
        {
            get { return _XmlAttributes; }
            set { SetProperty(ref _XmlAttributes, value); }
        }
    }
}
