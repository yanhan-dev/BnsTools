using Common;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BatchBoostJob.ViewModels
{
    public class BatchBoostJobViewModel : BindableBase
    {
        public BatchBoostJobViewModel()
        {
        }

        private string _FileName = "effectdata_forcemaster.xml";
        public string FileName
        {
            get { return _FileName; }
            set { SetProperty(ref _FileName, value); }
        }

        private string _Match = "G3";
        public string Match
        {
            get { return _Match; }
            set { SetProperty(ref _Match, value); }
        }

        private string _Attributes = "power-percent-max,power-percent-min";
        public string Attributes
        {
            get { return _Attributes; }
            set { SetProperty(ref _Attributes, value); }
        }

        private string _Symbol;
        public string Symbol
        {
            get { return _Symbol; }
            set { SetProperty(ref _Symbol, value); }
        }

        private List<string> _Symbols = new() { "*", "/" };
        public List<string> Symbols
        {
            get { return _Symbols; }
            set { SetProperty(ref _Symbols, value); }
        }

        private string _Param;
        public string Param
        {
            get { return _Param; }
            set { SetProperty(ref _Param, value); }
        }

        private DelegateCommand _DoCommand;
        public DelegateCommand DoCommand => _DoCommand ??= new DelegateCommand(ExecuteDoCommand);

        void ExecuteDoCommand()
        {
            /**
            1.读取指定xml文件
            2.匹配alias
            3.修改attributes
            4.保存npc
            */

            string[] files = Directory.GetFiles(Config.ServerPath, "*.xml", SearchOption.TopDirectoryOnly);
            var file = files.FirstOrDefault(f => f.EndsWith(FileName));
            if (null == file)
            {
                MessageBox.Show($"not found file {FileName}");
                return;
            }

            XDocument xd = XDocument.Load(file);
            List<XElement> records = xd.Root.Elements().Where(e => e.Attribute("alias").Value.Contains(Match)).ToList();
            foreach (XElement record in records)
            {
                SetNpcAttributes(record, Attributes.Split(','), Symbol, Convert.ToDouble(Param));
            }

            xd.Save(file);

            MessageBox.Show("Success!");
        }

        private void SetNpcAttributes(XElement xe, string[] attributes, string symbol, double param)
        {
            foreach (var attr in attributes)
            {
                if (xe.Attribute(attr) == null)
                {
                    continue;
                }

                switch (symbol)
                {
                    case "*":
                        xe.Attribute(attr).Value = Convert.ToInt64(Convert.ToInt64(xe.Attribute(attr).Value) * param).ToString();
                        break;

                    case "/":
                        xe.Attribute(attr).Value = Convert.ToInt64(Convert.ToInt64(xe.Attribute(attr).Value) / param).ToString();

                        break;

                    default: throw new NotImplementedException(symbol);
                }
            }
        }
    }
}
