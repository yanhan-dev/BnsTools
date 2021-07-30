using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.POJO.VO
{
    public class SchemaVO
    {
        public string TableName { get; set; }

        /// <summary>
        /// FieldName, Type
        /// </summary>
        public SortedDictionary<string, string> SchemaDictionary { get; set; } = new();
    }
}
