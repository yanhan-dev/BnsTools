using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace XmlEditor.ViewModels
{
    public class EditingFileViewModel : BindableBase
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
            set
            {
                SetProperty(ref _Uri, value);
                LoadXmlFile(_Uri);
            }
        }

        private ObservableCollection<XmlNodeViewModel> _XmlNodes;
        public ObservableCollection<XmlNodeViewModel> XmlNodes
        {
            get { return _XmlNodes; }
            set { SetProperty(ref _XmlNodes, value); }
        }

        private ObservableCollection<AttributeViewModel> _EditingXmlAttributes;
        public ObservableCollection<AttributeViewModel> EditingXmlAttributes
        {
            get { return _EditingXmlAttributes; }
            set { SetProperty(ref _EditingXmlAttributes, value); }
        }

        private DelegateCommand<XmlNodeViewModel> _NodeLeftDoubleClickCommand;
        public DelegateCommand<XmlNodeViewModel> NodeLeftDoubleClickCommand =>
            _NodeLeftDoubleClickCommand ?? (_NodeLeftDoubleClickCommand = new DelegateCommand<XmlNodeViewModel>(ExecuteNodeLeftDoubleClickCommand));

        void ExecuteNodeLeftDoubleClickCommand(XmlNodeViewModel parameter)
        {
            EditingXmlAttributes = parameter.XmlAttributes;
        }

        private void LoadXmlFile(string uri)
        {
            XDocument xDocument = XDocument.Load(uri);
            XmlNodes = new ObservableCollection<XmlNodeViewModel>(xDocument.Root.Elements()
                .Where(element => element.NodeType != XmlNodeType.Comment)
                .Select(element => new XmlNodeViewModel
                {
                    Alias = element.Attribute("alias")?.Value,
                    Desc = $"{element.Attribute("alias")?.Value} desc",
                    XmlAttributes = new ObservableCollection<AttributeViewModel>(element.Attributes().Select(attr => new AttributeViewModel
                    {
                        Attribute = attr.Name.LocalName,
                        AttributeDesc = $"{attr.Name.LocalName} desc",
                        Value = attr.Value,
                        ValueDesc = $"{attr.Value} desc"
                    }))
                }));
        }
    }
}
