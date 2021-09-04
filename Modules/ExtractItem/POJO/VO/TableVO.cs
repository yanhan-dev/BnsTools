using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExtractItem.POJO.VO
{
    [XmlRoot("table")]
    public class TableVO
    {
        [XmlElement("item")]
        public List<ItemVO> Items { get; set; }
    }
}
