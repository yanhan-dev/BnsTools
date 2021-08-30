using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using Microsoft.Win32;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Windows;
using Translate.POJO.VO;

namespace Translate.ViewModels
{
    public class TranslateViewModel : BindableBase
    {
        public TranslateViewModel()
        {
        }

        private string _XmlPath;
        public string XmlPath
        {
            get { return _XmlPath; }
            set { SetProperty(ref _XmlPath, value); }
        }

        private DelegateCommand _OpenCommand;
        public DelegateCommand OpenCommand =>
            _OpenCommand ?? (_OpenCommand = new DelegateCommand(ExecuteOpenCommand));

        void ExecuteOpenCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "xml 文件(*.xml)|*.xml" };
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            XmlPath = ofd.FileName;
        }

        private DelegateCommand _TranslateCommand;
        public DelegateCommand TranslateCommand =>
            _TranslateCommand ?? (_TranslateCommand = new DelegateCommand(ExecuteTranslateCommand));

        void ExecuteTranslateCommand()
        {
            XElement texts = new XElement("table");


            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(XmlPath);
            XmlNodeList xNodeList = xDoc.SelectNodes("table/child::node()");

            foreach (XmlNode xNode in xNodeList)
            {
                string alias = xNode.Attributes[NodeNames.Alias].Value;
                string autoId = xNode.Attributes[NodeNames.AutoId].Value;
                string priority = xNode.Attributes[NodeNames.Priority].Value;
                string original = xNode.FirstChild.InnerText;
                string replacement = ChineseConverter.Convert(original, ChineseConversionDirection.TraditionalToSimplified);

                XElement temp_xml = new XElement(NodeNames.Root,
                   new XAttribute(NodeNames.AutoId, autoId),
                   new XAttribute(NodeNames.Alias, alias),
                   new XAttribute(NodeNames.Priority, priority));

                temp_xml.Add(new XElement(NodeNames.Original, new XCData(xNode.FirstChild.InnerText)),
                             new XElement(NodeNames.Replacement, new XCData(replacement)));

                texts.Add(temp_xml);
            }

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8,
                Indent = true
            };

            using (XmlWriter xw = XmlWriter.Create(XmlPath, settings))
            {
                texts.Save(xw);
            }

            MessageBox.Show("完成");
        }
    }
}
