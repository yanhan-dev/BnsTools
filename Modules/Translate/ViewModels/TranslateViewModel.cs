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
using Translate.Lib;
using System.IO;

namespace Translate.ViewModels
{
    public class TranslateViewModel : BindableBase
    {
        public TranslateViewModel()
        {
        }

        private string _BinFilePath;
        public string BinFilePath
        {
            get { return _BinFilePath; }
            set { SetProperty(ref _BinFilePath, value); }
        }

        private string _TranslateFilePath;
        public string TranslateFilePath
        {
            get { return _TranslateFilePath; }
            set { SetProperty(ref _TranslateFilePath, value); }
        }

        private string _LanguageFilePath;
        public string LanguageFilePath
        {
            get { return _LanguageFilePath; }
            set { SetProperty(ref _LanguageFilePath, value); }
        }

        private string _BinNewFilePath;
        public string BinNewFilePath
        {
            get { return _BinNewFilePath; }
            set { SetProperty(ref _BinNewFilePath, value); }
        }

        private DelegateCommand _OpenLanguageFileCommand;
        public DelegateCommand OpenLanguageFileCommand =>
            _OpenLanguageFileCommand ?? (_OpenLanguageFileCommand = new DelegateCommand(ExecuteOpenLanguageFileCommand));

        void ExecuteOpenLanguageFileCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "xml 文件(*.xml)|*.xml" };
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            LanguageFilePath = ofd.FileName;
        }

        private DelegateCommand _ExportBinCommand;
        public DelegateCommand ExportBinCommand =>
            _ExportBinCommand ?? (_ExportBinCommand = new DelegateCommand(ExecuteExportBinCommand));

        void ExecuteExportBinCommand()
        {
            new BDat().ExportTranslate(BinFilePath, BinFilePath + ".xml", BXML_TYPE.BXML_PLAIN, Path.GetFileNameWithoutExtension(BinFilePath).Contains("64"));
        }

        private DelegateCommand _MergeTranslateCommand;
        public DelegateCommand MergeTranslateCommand =>
            _MergeTranslateCommand ?? (_MergeTranslateCommand = new DelegateCommand(ExecuteMergeTranslateCommand));

        void ExecuteMergeTranslateCommand()
        {

        }

        private DelegateCommand _TraditionalToSimplifiedCommand;
        public DelegateCommand TraditionalToSimplifiedCommand =>
            _TraditionalToSimplifiedCommand ?? (_TraditionalToSimplifiedCommand = new DelegateCommand(ExecuteTraditionalToSimplifiedCommand));

        void ExecuteTraditionalToSimplifiedCommand()
        {
            XElement texts = new XElement("table");


            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(LanguageFilePath);
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

            using (XmlWriter xw = XmlWriter.Create(LanguageFilePath, settings))
            {
                texts.Save(xw);
            }

            MessageBox.Show("完成");
        }

        private DelegateCommand _OpenBinCommand;
        public DelegateCommand OpenBinCommand =>
            _OpenBinCommand ?? (_OpenBinCommand = new DelegateCommand(ExecuteOpenBinCommand));

        void ExecuteOpenBinCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "localfile(64).bin (*.bin)|*.bin" };
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            BinFilePath = ofd.FileName;
        }

        private DelegateCommand _OpenTranslateFileCommand;
        public DelegateCommand OpenTranslateFileCommand =>
            _OpenTranslateFileCommand ?? (_OpenTranslateFileCommand = new DelegateCommand(ExecuteOpenTranslateFileCommand));

        void ExecuteOpenTranslateFileCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog() { Filter = "翻译文件 (*.xml)|*.xml" };
            if (ofd.ShowDialog() != true)
            {
                return;
            }

            TranslateFilePath = ofd.FileName;
        }

        private DelegateCommand _OpenBinNewCommand;
        public DelegateCommand OpenBinNewCommand =>
            _OpenBinNewCommand ?? (_OpenBinNewCommand = new DelegateCommand(ExecuteOpenBinNewCommand));

        void ExecuteOpenBinNewCommand()
        {

        }

        private DelegateCommand _BuildBinCommand;
        public DelegateCommand BuildBinCommand =>
            _BuildBinCommand ?? (_BuildBinCommand = new DelegateCommand(ExecuteBuildBinCommand));

        void ExecuteBuildBinCommand()
        {

        }
    }
}
