using AutoMapper;

using Common;
using Common.Extensions;
using Common.Model;

using HandyControl.Data;
using HandyControl.Tools;

using Masuit.Tools;
using Masuit.Tools.Strings;

using Microsoft.CodeAnalysis.CSharp;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using XmlEditor.Message;

using MessageBox = HandyControl.Controls.MessageBox;

namespace XmlEditor.ViewModels
{
    public class EditingFileViewModel : BindableBase
    {
        static EditingFileViewModel()
        {
            RoutedEvent closingEvent = EventManager.GetRoutedEventsForOwner(typeof(HandyControl.Controls.TabItem)).First(ss => ss.Name == "Closing");
            EventManager.RegisterClassHandler(typeof(HandyControl.Controls.TabItem), closingEvent, new RoutedEventHandler(TabItemClosingHandler));
        }

        public EditingFileViewModel()
        {
        }

        private static void TabItemClosingHandler(object sender, RoutedEventArgs e)
        {
            if (e is not CancelRoutedEventArgs ce || ce.OriginalSource is not EditingFileViewModel editingFileVM)
            {
                return;
            }

            if (!editingFileVM.IsEditing)
            {
                return;
            }

            MessageBoxResult r = MessageBox.Show("Save File ?", editingFileVM.Name, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (r)
            {
                case MessageBoxResult.Cancel:
                    ce.Cancel = true;
                    break;
                case MessageBoxResult.Yes:
                    editingFileVM.Save();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        #region Property

        private XElement Root { get; set; }

        private string FileType { get; set; }

        private int EditingNodeIndex { get; set; }

        #endregion

        #region Dependency Property

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (value.Length > 25)
                {
                    value = value.Truncate(25);
                }
                SetProperty(ref _Title, value);
                return;
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); Title = value; }
        }

        private string _Uri;
        public string Uri
        {
            get { return _Uri; }
            set
            {
                SetProperty(ref _Uri, value);
                Load(_Uri);
            }
        }

        private bool _IsEditing;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set { SetProperty(ref _IsEditing, value); }
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

        private ObservableCollection<AttributeViewModel> _SelectedAttrs;
        public ObservableCollection<AttributeViewModel> SelectedAttrs
        {
            get { return _SelectedAttrs ??= new ObservableCollection<AttributeViewModel>(); }
            set { SetProperty(ref _SelectedAttrs, value); }
        }

        private ObservableCollection<XmlNodeViewModel> _SelectedNodes;
        public ObservableCollection<XmlNodeViewModel> SelectedNodes
        {
            get { return _SelectedNodes ??= new ObservableCollection<XmlNodeViewModel>(); }
            set { SetProperty(ref _SelectedNodes, value); }
        }

        #endregion

        #region Command
        private DelegateCommand<SelectionChangedEventArgs> _NodesSelectionChangedCommand;
        public DelegateCommand<SelectionChangedEventArgs> NodesSelectionChangedCommand => _NodesSelectionChangedCommand ??= new DelegateCommand<SelectionChangedEventArgs>(ExecuteNodesSelectionChangedCommand);

        void ExecuteNodesSelectionChangedCommand(SelectionChangedEventArgs parameter)
        {
            foreach (XmlNodeViewModel item in parameter.RemovedItems)
            {
                SelectedNodes.Remove(item);
            }

            foreach (XmlNodeViewModel item in parameter.AddedItems)
            {
                SelectedNodes.Add(item);
            }
        }

        private DelegateCommand<SelectionChangedEventArgs> _AttrSelectionChangedCommand;
        public DelegateCommand<SelectionChangedEventArgs> AttrSelectionChangedCommand => _AttrSelectionChangedCommand ??= new DelegateCommand<SelectionChangedEventArgs>(ExecuteAttrSelectionChangedCommand);

        void ExecuteAttrSelectionChangedCommand(SelectionChangedEventArgs parameter)
        {
            foreach (AttributeViewModel item in parameter.RemovedItems)
            {
                SelectedAttrs.Remove(item);
            }

            foreach (AttributeViewModel item in parameter.AddedItems)
            {
                SelectedAttrs.Add(item);
            }
        }

        private DelegateCommand _DeleteSelectedAttrCommand;
        public DelegateCommand DeleteSelectedAttrCommand => _DeleteSelectedAttrCommand ??= new DelegateCommand(ExecuteDeleteSelectedAttrCommand);

        void ExecuteDeleteSelectedAttrCommand()
        {
            EditingXmlAttributes.RemoveWhere(eAttr => SelectedAttrs.FirstOrDefault(sAttr => sAttr.Attr == eAttr.Attr) != null);
            IsEditing = true;
        }

        private DelegateCommand _DeleteSelectedNodesCommand;
        public DelegateCommand DeleteSelectedNodesCommand => _DeleteSelectedNodesCommand ??= new DelegateCommand(ExecuteDeleteSelectedNodesCommand);

        void ExecuteDeleteSelectedNodesCommand()
        {
            XmlNodes.RemoveWhere(node => SelectedNodes.FirstOrDefault(sNode => sNode.Title == node.Title) != null);
            IsEditing = true;
        }

        private DelegateCommand<AttributeViewModel> _CopyAddAttrCommand;
        public DelegateCommand<AttributeViewModel> CopyAddAttrCommand => _CopyAddAttrCommand ??= new DelegateCommand<AttributeViewModel>(ExecuteCopyAddAttrCommand);

        void ExecuteCopyAddAttrCommand(AttributeViewModel parameter)
        {
            var attrs = parameter.Attr.Split('-');
            bool isNum = int.TryParse(attrs.LastOrDefault(), out int num);
            if (isNum)
            {
                num += 1;
                attrs[^1] = num.ToString();
            }
            else
            {
                attrs[^1] += "-1";
            }
            string newAttr = string.Join("-", attrs);

            EditingXmlAttributes.InsertAfter(ss => ss.Attr == parameter.Attr, new AttributeViewModel { Attr = newAttr, Value = parameter.Value });
            IsEditing = true;
        }

        private DelegateCommand<XmlNodeViewModel> _CopyAddNodeCommand;
        public DelegateCommand<XmlNodeViewModel> CopyAddNodeCommand => _CopyAddNodeCommand ??= new DelegateCommand<XmlNodeViewModel>(ExecuteCopyAddNodeCommand);

        void ExecuteCopyAddNodeCommand(XmlNodeViewModel parameter)
        {
            XmlNodes.Add(new XmlNodeViewModel
            {
                Title = parameter.Title + "_Copy",
                Desc = parameter.Desc,
                Node = parameter.Node,
                UnUse = parameter.UnUse,
                XmlAttributes = new ObservableCollection<AttributeViewModel>(parameter.XmlAttributes.Select(a => new AttributeViewModel
                {
                    Attr = a.Attr,
                    AttrDesc = a.AttrDesc,
                    Value = a.Value,
                    ValueDesc = a.ValueDesc
                }))
            });
            IsEditing = true;
        }

        private DelegateCommand<XmlNodeViewModel> _NodeLeftDoubleClickCommand;
        public DelegateCommand<XmlNodeViewModel> NodeLeftDoubleClickCommand => _NodeLeftDoubleClickCommand ??= new DelegateCommand<XmlNodeViewModel>(ExecuteNodeLeftDoubleClickCommand);

        void ExecuteNodeLeftDoubleClickCommand(XmlNodeViewModel parameter)
        {
            if (parameter.UnUse)
            {
                return;
            }

            EditingNodeIndex = NodeSelectedIndex;

            EditingXmlAttributes = parameter.XmlAttributes;
        }

        private DelegateCommand<string> _SearchUpCommand;
        public DelegateCommand<string> SearchUpCommand => _SearchUpCommand ??= new DelegateCommand<string>(ExecuteSearchUpCommand);

        void ExecuteSearchUpCommand(string parameter)
        {
            for (int i = NodeSelectedIndex - 1; i > 0; i--)
            {
                if (XmlNodes[i].UnUse)
                {
                    continue;
                }

                if (!XmlNodes[i].Title.Contains(parameter) && null != XmlNodes[i].Desc && !XmlNodes[i].Desc.Contains(parameter))
                {
                    continue;
                }
                NodeSelectedIndex = i;
                return;
            }

            for (int i = XmlNodes.Count - 1; i > NodeSelectedIndex; i--)
            {
                if (XmlNodes[i].UnUse)
                {
                    continue;
                }

                if (!XmlNodes[i].Title.Contains(parameter) && !XmlNodes[i].Desc.Contains(parameter))
                {
                    continue;
                }
                NodeSelectedIndex = i;
                return;
            }
        }

        private DelegateCommand<string> _SearchDownCommand;
        public DelegateCommand<string> SearchDownCommand => _SearchDownCommand ??= new DelegateCommand<string>(ExecuteSearchDownCommand);

        void ExecuteSearchDownCommand(string parameter)
        {
            for (int i = NodeSelectedIndex + 1; i < XmlNodes.Count; i++)
            {
                if (XmlNodes[i].UnUse)
                {
                    continue;
                }

                if ((string.IsNullOrEmpty(XmlNodes[i].Title) || !XmlNodes[i].Title.Contains(parameter)) && (string.IsNullOrEmpty(XmlNodes[i].Desc) || !XmlNodes[i].Desc.Contains(parameter)))
                {
                    continue;
                }
                NodeSelectedIndex = i;
                return;
            }

            for (int i = 0; i < NodeSelectedIndex; i++)
            {
                if (XmlNodes[i].UnUse)
                {
                    continue;
                }

                if ((string.IsNullOrEmpty(XmlNodes[i].Title) || !XmlNodes[i].Title.Contains(parameter)) && (string.IsNullOrEmpty(XmlNodes[i].Desc) || !XmlNodes[i].Desc.Contains(parameter)))
                {
                    continue;
                }
                NodeSelectedIndex = i;
                return;
            }
        }

        private DelegateCommand<DataGridCellEditEndingEventArgs> _CellEditEndingCommand;
        public DelegateCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand =>
            _CellEditEndingCommand ??= new DelegateCommand<DataGridCellEditEndingEventArgs>(ExecuteCellEditEndingCommand);

        void ExecuteCellEditEndingCommand(DataGridCellEditEndingEventArgs parameter)
        {
            if (parameter.EditAction != DataGridEditAction.Commit)
            {
                return;
            }

            // 排除desc
            if (parameter.Column.DisplayIndex == 1 || parameter.Column.DisplayIndex == 3)
            {
                return;
            }

            AttributeViewModel originRow = (AttributeViewModel)parameter.Row.Item;
            var header = parameter.Column.Header.ToString();
            var input = ((TextBox)parameter.EditingElement).Text;

            if (header == "Value" && !string.Equals(originRow.Value, input, StringComparison.Ordinal))
            {
                // update by Attr
                var attrM = Desc.FindAttrAndValueDesc(originRow.Attr, input, FileType);
                originRow.Attr = attrM.Attr;
                originRow.AttrDesc = attrM.AttrDesc;
                originRow.Value = attrM.Value;
                originRow.ValueDesc = attrM.ValueDesc;

                // update title
                if (attrM.Attr == Desc.FindTitleAttr(FileType))
                {
                    XmlNodes[EditingNodeIndex].Title = attrM.Value;
                }

                //update desc
                if (attrM.Attr == XmlNodes[EditingNodeIndex].DescAttr)
                {
                    XmlNodes[EditingNodeIndex].Desc = attrM.ValueDesc;
                }

                IsEditing = true;
                return;
            }
            if (string.Equals(header, "Attr", StringComparison.Ordinal) && !string.Equals(originRow.Attr, input, StringComparison.Ordinal))
            {
                // update by Value
                var attrM = Desc.FindAttrAndValueDesc(input, originRow.Value, FileType);
                originRow.Attr = attrM.Attr;
                originRow.AttrDesc = attrM.AttrDesc;
                originRow.Value = attrM.Value;
                originRow.ValueDesc = attrM.ValueDesc;

                IsEditing = true;
                return;
            }
        }

        #endregion

        #region Method

        private void Load(string uri)
        {
            XDocument xDocument = XDocument.Load(uri);
            FileType = xDocument.Root.Attribute("type").Value;
            string titleAttr = Desc.FileSchemeDescs.GetValueOrDefault(FileType, null)?.TitleAttr;
            List<string> descAttrs = Desc.FileSchemeDescs.GetValueOrDefault(FileType, null)?.DescAttr;
            Root = new XElement(xDocument.Root.Name);
            foreach (var attr in xDocument.Root.Attributes())
            {
                Root.Add(attr);
            }

            XmlNodes = new ObservableCollection<XmlNodeViewModel>(xDocument.Root.Nodes()
                .Select(node =>
                {
                    var xmlNode = new XmlNodeViewModel();

                    // 不是节点需要保留写回源文件
                    if (node.NodeType != XmlNodeType.Element)
                    {
                        xmlNode.UnUse = true;
                        xmlNode.Node = node;
                        xmlNode.Title = node.ToString();
                        xmlNode.Desc = node.NodeType.ToString();
                        return xmlNode;
                    }

                    XElement element = node as XElement;

                    string title = element.Attribute(titleAttr)?.Value ??
                        element.Attribute("alias")?.Value ??
                        element.Attribute("dayofweek")?.Value ??
                        element.Attribute("store2")?.Value ??
                        element.Attribute("job")?.Value;

                    List<AttributeViewModel> attrList = new();
                    foreach (var attr in element.Attributes())
                    {
                        var attrM = Desc.FindAttrAndValueDesc(attr.Name.LocalName, attr.Value, FileType);

                        attrList.Add(new AttributeViewModel()
                        {
                            Attr = attrM.Attr,
                            AttrDesc = attrM.AttrDesc,
                            Value = attrM.Value,
                            ValueDesc = attrM.ValueDesc
                        });
                    }

                    xmlNode.Title = title;
                    xmlNode.XmlAttributes = new(attrList);

                    if (descAttrs.IsNullOrEmpty())
                    {
                        return xmlNode;
                    }

                    var descContains = descAttrs.Where(descAttr => xmlNode.XmlAttributes.FirstOrDefault(attr => attr.Attr == descAttr) != null);
                    var descAttrVM = xmlNode.XmlAttributes.FirstOrDefault(attr => descContains.Contains(attr.Attr) && !string.IsNullOrEmpty(attr.ValueDesc), new AttributeViewModel { ValueDesc = string.Empty });
                    xmlNode.DescAttr = descAttrVM.Attr;
                    xmlNode.Desc = descAttrVM.ValueDesc;

                    return xmlNode;
                }));
        }

        public void Save()
        {
            XDocument xDocument = new();
            xDocument.Add(new XElement(Root));

            foreach (var node in XmlNodes)
            {
                if (node.UnUse)
                {
                    xDocument.Root.Add(node.Node);
                    continue;
                }

                var record = new XElement("record");

                var attrs = node.XmlAttributes.Select(attr => new XAttribute(XName.Get(attr.Attr), attr.Value));
                foreach (var attr in attrs)
                {
                    try
                    {
                        record.Add(attr);

                    }
                    catch (InvalidOperationException e)
                    {
                        MessageBox.Error(attr.Name.LocalName, e.Message);
                        return;
                    }
                }

                xDocument.Root.Add(record);
            }

            XmlWriterSettings settings = new()
            {
                Encoding = new UTF8Encoding(false),
                Indent = true,
                IndentChars = "  ",  // Indent 2 Spaces
            };

            using (XmlWriter writer = XmlWriter.Create(Uri, settings))
            {
                xDocument.Save(writer);
            }

            IsEditing = false;
        }

        #endregion
    }
}
