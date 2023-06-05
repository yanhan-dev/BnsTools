using Common;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BatchSetNpc.ViewModels
{
    public class BatchSetNpcViewModel : BindableBase
    {
        public BatchSetNpcViewModel()
        {
        }

        private string _ZoneId = "4451,4452,4453,4454";
        public string ZoneId
        {
            get { return _ZoneId; }
            set { SetProperty(ref _ZoneId, value); }
        }

        private string _ModAttribute = "max-hp,hp-regen,hp-regen-combat";
        public string ModAttribute
        {
            get { return _ModAttribute; }
            set { SetProperty(ref _ModAttribute, value); }
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

        private double _Param;
        public double Param
        {
            get { return _Param; }
            set { SetProperty(ref _Param, value); }
        }

        public Dictionary<string, XDocument> NpcFiles { get; set; } = new();
        public Dictionary<string, XDocument> SpawnFiles { get; set; } = new();

        private DelegateCommand _DoCommand;
        public DelegateCommand DoCommand => _DoCommand ??= new DelegateCommand(ExecuteDoCommand);

        void ExecuteDoCommand()
        {
            /**
            1.获取所有xml文件
            2.找到所有type为ZoneNpcSpawn的文件,根据zone找出npc
            3.找到所有type为npc的文件
            4.修改npc属性
            5.保存npc
            */

            string[] zones = ZoneId.Split(',');
            string[] attributes = ModAttribute.Split(",");

            string[] files = Directory.GetFiles(Config.ServerPath, "*.xml", SearchOption.TopDirectoryOnly);

            NpcFiles.Clear();
            SpawnFiles.Clear();

            Parallel.ForEach(files, file =>
            {
                XDocument xd = XDocument.Load(file);
                var type = xd.Root.Attribute("type")?.Value;
                if (type == "npc")
                {
                    NpcFiles.Add(file, xd);
                    return;
                }
                if (type == "ZoneNpcSpawn")
                {
                    SpawnFiles.Add(file, xd);
                    return;
                }
            });

            List<string> npcs = SpawnFiles.Values.SelectMany(xd => xd.Root.Elements()
            .Where(e => zones.Contains(e.Attribute("zone").Value))
            .Select(e => e.Attribute("npc").Value))
            .Distinct()
            .ToList();

            List<string> foundNpc = new();
            foreach (var xd in NpcFiles)
            {
                if (npcs.Count == 0) return;

                List<XElement> records = xd.Value.Root.Elements().Where(e => npcs.Except(foundNpc).Contains(e.Attribute("alias").Value)).ToList();

                if (records.Count == 0) continue;

                records.ForEach(r => SetNpcAttributes(r, attributes, Symbol, Param));

                foundNpc.AddRange(records.Select(x => x.Attribute("alias").Value));

                xd.Value.Save(xd.Key);
            }

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
