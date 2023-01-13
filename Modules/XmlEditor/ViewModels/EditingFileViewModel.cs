using Common;

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
using System.Xml.Serialization;

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

        private int _NodeSelectedIndex;
        public int NodeSelectedIndex
        {
            get { return _NodeSelectedIndex; }
            set { SetProperty(ref _NodeSelectedIndex, value); }
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
            _NodeLeftDoubleClickCommand ??= new DelegateCommand<XmlNodeViewModel>(ExecuteNodeLeftDoubleClickCommand);

        void ExecuteNodeLeftDoubleClickCommand(XmlNodeViewModel parameter)
        {
            EditingXmlAttributes = parameter.XmlAttributes;
        }

        private void LoadXmlFile(string uri)
        {
            XDocument xDocument = XDocument.Load(uri);
            string type = xDocument.Root.Attribute("type").Value;
            string titleAttr = Desc.FileSchemeDescs.GetValueOrDefault(type, null)?.TitleAttr;

            XmlNodes = new ObservableCollection<XmlNodeViewModel>(xDocument.Root.Elements()
                .Where(element => element.NodeType != XmlNodeType.Comment)
                .Select(element =>
                {
                    string title = element.Attribute(titleAttr)?.Value ??
                        element.Attribute("alias")?.Value ??
                        element.Attribute("dayofweek")?.Value ??
                        element.Attribute("store2")?.Value ??
                        element.Attribute("job")?.Value;

                    List<AttributeViewModel> attrList = new();
                    foreach (var attr in element.Attributes())
                    {
                        var attrM = Desc.FindAttrAndValueDesc(attr.Name.LocalName, attr.Value, type);

                        attrList.Add(new AttributeViewModel
                        {
                            Attr = attrM.Attr,
                            AttrDesc = attrM.AttrDesc,
                            Value = attrM.Value,
                            ValueDesc = attrM.ValueDesc
                        });
                    }

                    var xmlNode = new XmlNodeViewModel
                    {
                        Title = title,
                        XmlAttributes = new(attrList)
                    };

                    xmlNode.Desc = xmlNode.XmlAttributes.FirstOrDefault(ss => ss.Value == title)?.ValueDesc;

                    return xmlNode;
                }));
        }


        private DelegateCommand<string> _SearchCommand;
        public DelegateCommand<string> SearchCommand =>
            _SearchCommand ?? (_SearchCommand = new DelegateCommand<string>(ExecuteSearchCommand));

        void ExecuteSearchCommand(string parameter)
        {
            for (int i = NodeSelectedIndex + 1; i < XmlNodes.Count; i++)
            {
                if (!XmlNodes[i].Title.Contains(parameter) && null != XmlNodes[i].Desc && !XmlNodes[i].Desc.Contains(parameter))
                {
                    continue;
                }
                NodeSelectedIndex = i;
                return;
            }

            for (int i = 0; i < NodeSelectedIndex; i++)
            {
                if (!XmlNodes[i].Title.Contains(parameter) && !XmlNodes[i].Desc.Contains(parameter))
                {
                    continue;
                }
                NodeSelectedIndex = i;
                return;
            }
        }
    }
}
