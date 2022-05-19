using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RandomStore.POJO.DO
{
    [XmlRoot("table")]
    public class RandomStoreItemGroup
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("record")]
        public List<RandomStoreItemGroupRecord> RandomStoreItemGroupRecords { get; set; }
    }

    [XmlType(TypeName = "record")]
    public class RandomStoreItemGroupRecord
    {
        [XmlAttribute("alias")]
        public string Alias { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlIgnore]
        public string Name1 { get; set; }

        [XmlIgnore]
        public string Name2 { get; set; }

        [XmlIgnore]
        public string Name3 { get; set; }

        [XmlIgnore]
        public string Name4 { get; set; }

        [XmlIgnore]
        public string Name5 { get; set; }

        [XmlIgnore]
        public string Name6 { get; set; }

        [XmlIgnore]
        public string Name7 { get; set; }
        
        [XmlIgnore]
        public string Name8 { get; set; }

        [XmlAttribute("item-1")]
        public string Item1 { get; set; }

        [XmlAttribute("item-2")]
        public string Item2 { get; set; }

        [XmlAttribute("item-3")]
        public string Item3 { get; set; }

        [XmlAttribute("item-4")]
        public string Item4 { get; set; }

        [XmlAttribute("item-5")]
        public string Item5 { get; set; }

        [XmlAttribute("item-6")]
        public string Item6 { get; set; }

        [XmlAttribute("item-7")]
        public string Item7 { get; set; }

        [XmlAttribute("item-8")]
        public string Item8 { get; set; }

        [XmlAttribute("item-count-1")]
        public int ItemCount1 { get; set; }

        [XmlAttribute("item-count-2")]
        public int ItemCount2 { get; set; }

        [XmlAttribute("item-count-3")]
        public int ItemCount3 { get; set; }

        [XmlAttribute("item-count-4")]
        public int ItemCount4 { get; set; }

        [XmlAttribute("item-count-5")]
        public int ItemCount5 { get; set; }

        [XmlAttribute("item-count-6")]
        public int ItemCount6 { get; set; }

        [XmlAttribute("item-count-7")]
        public int ItemCount7 { get; set; }

        [XmlAttribute("item-count-8")]
        public int ItemCount8 { get; set; }

        [XmlAttribute("item-price-money-1")]
        public int ItemPriceMoney1 { get; set; }

        [XmlAttribute("item-price-money-2")]
        public int ItemPriceMoney2 { get; set; }

        [XmlAttribute("item-price-money-3")]
        public int ItemPriceMoney3 { get; set; }

        [XmlAttribute("item-price-money-4")]
        public int ItemPriceMoney4 { get; set; }

        [XmlAttribute("item-price-money-5")]
        public int ItemPriceMoney5 { get; set; }

        [XmlAttribute("item-price-money-6")]
        public int ItemPriceMoney6 { get; set; }

        [XmlAttribute("item-price-money-7")]
        public int ItemPriceMoney7 { get; set; }

        [XmlAttribute("item-price-money-8")]
        public int ItemPriceMoney8 { get; set; }

        [XmlAttribute("ui-grade")]
        public int UIGrade { get; set; }
    }
}
